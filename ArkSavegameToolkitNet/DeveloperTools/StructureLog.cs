using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

// NOTES
// Don't use this code unless you know what you are doing.

// TODO
// - Dump into multiple templates (all sections, individual sections, individual objects etc.)
// - FSeeks must be in order (at least in the root scope)
// - Dump the last x reads before an error occured
// - Dump a template with all offsets that were not read.

namespace ArkSavegameToolkitNet.DeveloperTools
{
    class StackEntry
    {
        public StackEntry(string typeName) => TypeName = typeName;

        public string TypeName { get; set; }
        public string VarName { get; set; }
        public string Comment { get; set; }
        public int Num { get; set; }

        public override string ToString() => VarName != null ? $"{TypeName}_{VarName}" : TypeName;
    }

    class StackLog
    {
        public StackLog(int id, int level, StackEntry stack, int fromId)
        {
            Id = id;
            Level = level;
            Stack = stack;
            FromId = fromId;
        }

        public StackLog Clone()
        {
            return new StackLog(Id, Level, Stack, FromId) { ToId = ToId };
        }


        public int Id { get; set; }
        public int Level { get; set; }
        public StackEntry Stack { get; set; }
        public int FromId { get; set; }
        public int? ToId { get; set; }
    }

    interface IHasOffset
    {
        long Start { get; }
    }

    class StackLogTreeNode : StackLog, IHasOffset
    {
        public static StackLogTreeNode FromStackLog(StackLog sl)
        {
            return new StackLogTreeNode(sl.Id, sl.Level, sl.Stack, sl.FromId) { ToId = sl.ToId };
        }

        public StackLogTreeNode(int id, int level, StackEntry stack, int fromId) : base(id, level, stack, fromId)
        {
            Children = new List<StackLogTreeNode>();
            Offsets = new List<OffsetLog>();
        }

        [JsonIgnore]
        public StackLogTreeNode Parent { get; set; }
        [JsonProperty(Order = 100)]
        public List<StackLogTreeNode> Children { get; set; }
        [JsonProperty(Order = 99)]
        public List<OffsetLog> Offsets { get; set; }

        [JsonIgnore]
        public IEnumerable<IHasOffset> Ordered => Children.Cast<IHasOffset>().Concat(Offsets).OrderBy(x => x.Start);

        public long Start => Offsets.Count > 0 && Children.Count > 0 ? Math.Min(Offsets.Min(x => x.Start), Children.Min(x => x.Start)) : Offsets.Count > 0 ? Offsets.Min(x => x.Start) : Children.Count > 0 ? Children.Min(x => x.Start) : long.MaxValue;
    }

    class OffsetLog : IHasOffset
    {
        public OffsetLog(long start, int len, Type varType, string name, int id)
        {
            Start = start;
            Len = len;
            VarType = varType;
            Name = name;
            Id = id;
        }

        public long Start { get; set; }
        public int Len { get; set; }
        public Type VarType { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }

    enum StructureLogMode { Immediate }

    class StructureLog : IDisposable
    {

        private StructureLogMode _mode;
        private string _binaryTemplateOutputDirPath;
        private int _maxHistory;

        private StreamWriter _sw;
        private int _numi;
        private long _prevOffset;

        private int _stackIdLast = 0;
        private int _offsetIdLast = 0;

        private string _stackPath;
        private Stack<StackEntry> _stack = new Stack<StackEntry>();
        private List<StackLog> _stackLog = new List<StackLog>();
        private List<(long start, int len, Type type, string name, int id)> _offsets = new List<(long start, int len, Type type, string name, int id)>();
        public StructureLog(StructureLogMode mode, string binaryTemplateOutputDirPath, int maxHistory = 100)
        {
            _mode = mode;
            _binaryTemplateOutputDirPath = binaryTemplateOutputDirPath;
            _maxHistory = maxHistory;
        }

        [System.Diagnostics.Conditional("STRUCTURELOG")]
        public void Add<TValueType>(long start, int length, string name)
        {
            _offsetIdLast++;
            if (_offsets.Count > _maxHistory)
            {
                _offsets.RemoveRange(0, _offsets.Count - _maxHistory + 1);
                var of = _offsets.FirstOrDefault();
                if (of != default)
                {
                    var i = 0;
                    while (_stackLog.Count >= i + 1)
                    {
                        var sl = _stackLog[i];
                        if (!sl.ToId.HasValue)
                        {
                            i++;
                            continue;
                        }
                        if (sl.ToId.Value < of.id) _stackLog.Remove(sl);
                        else break;
                    }
                }
            }
            _offsets.Add((start, length, typeof(TValueType), name, _offsetIdLast));
        }

        [System.Diagnostics.Conditional("STRUCTURELOG")]
        public void PushStack(string typeName, string varName = null)
        {
            _stack.Push(new StackEntry(typeName) { VarName = varName, Num = _numi });
            _stackPath = _stack.Count > 0 ? string.Join("->", _stack) : null;

            var level = _stackLog.LastOrDefault(x => !x.ToId.HasValue)?.Level + 1 ?? 0;
            _stackLog.Add(new StackLog(_stackIdLast++, level, _stack.Peek(), _offsetIdLast + 1));
        }

        [System.Diagnostics.Conditional("STRUCTURELOG")]
        public void PopStack()
        {
            var prev = _stack.Pop();
            _stackPath = _stack.Count > 0 ? string.Join("->", _stack) : null;

            var last = _stackLog.LastOrDefault(x => !x.ToId.HasValue);
            if (last != null) last.ToId = _offsetIdLast;
        }

        [System.Diagnostics.Conditional("STRUCTURELOG")]
        public void UpdateStack(string comment)
        {
            var current = _stack.Peek();
            if (current == null) return;

            current.Comment = comment;
        }

        [System.Diagnostics.Conditional("STRUCTURELOG")]
        public void Save()
        {
            if (!Directory.Exists(_binaryTemplateOutputDirPath)) Directory.CreateDirectory(_binaryTemplateOutputDirPath);

            string path;
            var num = 0;
            while (true)
            {
                num++;
                path = Path.Combine(_binaryTemplateOutputDirPath, $"toolkit_{num:0000}.bt");
                if (!File.Exists(path)) break;
                else if (num > 1000) return;
            }

            using var sw = new StreamWriter(path);

            var flat = new List<StackLogTreeNode>();
            StackLogTreeNode root = null;
            StackLogTreeNode currentNode = null;
            StackLogTreeNode toIdNullMaxLevelNode = null;
            foreach (var sl in _stackLog)
            {
                var node = StackLogTreeNode.FromStackLog(sl);
                flat.Add(node);
                if (root == null) root = currentNode = node;
                else
                {
                    if (node.Level < currentNode.Level)
                    {
                        for (var i = currentNode.Level; i >= node.Level; i--) currentNode = currentNode.Parent;
                    }
                    else if (node.Level == currentNode.Level)
                    {
                        currentNode = currentNode.Parent;
                    }

                    node.Parent = currentNode;
                    currentNode.Children.Add(node);
                    currentNode = node;
                }
            }

            // we are looking for the current stack entry here
            // get the last struct without set toId, if the end has multiple we want the last one with a previous set
            toIdNullMaxLevelNode = flat.LastOrDefault();

            var prev = flat.TakeWhile(x => x != toIdNullMaxLevelNode).ToList();
            prev.Add(toIdNullMaxLevelNode);
            prev.Reverse();

            var tmp = _offsets.Select(x => new OffsetLog(x.start, x.len, x.type, x.name, x.id)).ToList();

            foreach (var c in prev)
            {
                OffsetLog[] ofs = null;
                if (c == toIdNullMaxLevelNode) ofs = tmp.Where(x => x.Id >= c.FromId).ToArray();
                else if (!c.ToId.HasValue) continue;
                else ofs = tmp.Where(x => x.Id >= c.FromId && x.Id <= c.ToId.Value).ToArray();

                foreach (var o in ofs) tmp.Remove(o);
                c.Offsets.AddRange(ofs);
            }

            string RecursiveGetNextName(Dictionary<string, (string name, int? num)> propNames, string name, int? num = null)
            {
                var nextName = name + (num != null ? $"_{num}" : "");

                if (!propNames.TryGetValue(nextName, out var prevVarName)) prevVarName = (name, num);
                else return RecursiveGetNextName(propNames, name, (num ?? 0) + 1);
                
                propNames.Add(nextName, prevVarName);

                return nextName;
            }

            void RecursiveWriteTreeNode(StackLogTreeNode node, string structName)
            {
                WriteStructStart(sw, node);
                var propNames = new Dictionary<string, (string name, int? num)>();
                foreach (var o in node.Ordered)
                {
                    if (o is OffsetLog)
                    {
                        var log = o as OffsetLog;
                        var name = RecursiveGetNextName(propNames, log.Name);
                        WriteVariable(sw, log, name, node.Level + 1);
                    }
                    else if (o is StackLogTreeNode)
                    {
                        var sn = o as StackLogTreeNode;
                        var name = sn.Stack.VarName != null ? RecursiveGetNextName(propNames, sn.Stack.VarName) : sn.Stack.TypeName;
                        RecursiveWriteTreeNode(sn, name);
                    }
                }
                WriteStructEnd(sw, node, structName);
            }

            RecursiveWriteTreeNode(root, "ARK");

            sw.Flush();

            var tmp2 = _offsets.Select(x => new OffsetLog(x.start, x.len, x.type, x.name, x.id)).ToList();
            var tmp3 = prev.Select(x => StackLogTreeNode.FromStackLog(x)).ToList();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(root, Newtonsoft.Json.Formatting.Indented);
            var json2 = Newtonsoft.Json.JsonConvert.SerializeObject(_stackLog, Newtonsoft.Json.Formatting.Indented);
            var json3 = Newtonsoft.Json.JsonConvert.SerializeObject(tmp2, Newtonsoft.Json.Formatting.Indented);
            var json4 = Newtonsoft.Json.JsonConvert.SerializeObject(tmp3, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(Path.Combine(_binaryTemplateOutputDirPath, $"toolkit_tree.json"), json);
            File.WriteAllText(Path.Combine(_binaryTemplateOutputDirPath, $"toolkit_stackLog.json"), json2);
            File.WriteAllText(Path.Combine(_binaryTemplateOutputDirPath, $"toolkit_offsets.json"), json3);
            File.WriteAllText(Path.Combine(_binaryTemplateOutputDirPath, $"toolkit_prev_order.json"), json4);
        }

        private void WriteStructStart(StreamWriter sw, StackLogTreeNode node)
        {
            WriteOffset(sw, node.Start, node.Level);
            sw.WriteLine($@"{new string('\t', node.Level)}struct {{");
        }

        private void WriteStructEnd(StreamWriter sw, StackLogTreeNode node, string name)
        {
            var comments = new List<string>();
            if (node.Stack.VarName != null) comments.Add(node.Stack.TypeName);
            if (node.Stack.Comment != null) comments.Add(node.Stack.Comment);
            var commentStr = comments.Count > 0 ? @$",comment=""{string.Join(", ", comments)}""" : "";
            sw.WriteLine($@"{new string('\t', node.Level)}}} {AsVariableName(name)} <bgcolor={RandomColor()}{commentStr}>;"); //sl.Stack.TypeName
        }

        private void WriteOffset(StreamWriter sw, long offset, int level, int? size = null)
        {
            if (_prevOffset != offset) sw.WriteLine($@"{new string('\t', level)}FSeek({offset});");
            _prevOffset = offset + (size ?? 0);
        }

        private void WriteVariable(StreamWriter sw, OffsetLog o, string name, int level)
        {
            _numi++;
            WriteOffset(sw, o.Start, level, o.Len);

            var typeStr = "byte";
            var typeSize = o.Len;
            if (o.VarType == typeof(int))
            {
                typeStr = "int";
                typeSize = typeSize / 4;
            }
            var arrStr = "";
            if (typeSize > 1) arrStr = $"[{typeSize}]";

            var color = _numi % 2 == 0 ? "0xffbf77" : "0xffa43d";
            sw.WriteLine($@"{new string('\t', level)}{typeStr} {AsVariableName(name)}{arrStr} <bgcolor={color}>;");
        }

        private Regex _rAsVariableName = new Regex(@"[^a-z0-9]+", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private string AsVariableName(string str)
        {
            return _rAsVariableName.Replace(str, "_");
        }

        private Random _rnd = new Random();
        private string RandomColor()
        {
            return $"0x{_rnd.Next(0, 255):x2}{_rnd.Next(0, 255):x2}{_rnd.Next(0, 255):x2}";
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _sw?.Dispose();
                    _sw = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~StructureLog()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
