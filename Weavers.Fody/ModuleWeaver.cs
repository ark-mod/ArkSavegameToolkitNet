using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Cecil.Cil;
using Fody;

namespace Weavers.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
        {
            foreach (var type in ModuleDefinition.GetTypes())
            {
                if (type.IsInterface || type.IsEnum) continue;

                // skip processing methods in the types we want to remove
                if (type.FullName == "ArkSavegameToolkitNet.DataReaders.ArchiveReader") continue;
                
                ProcessType(type);
            }
        }

        void ProcessType(TypeDefinition typeDefinition)
        {
            foreach (var method in typeDefinition.Methods)
            {
                if (!method.HasBody) continue;

                ProcessMethod(method);
            }
        }

        void ProcessMethod(MethodDefinition method)
        {
            method.Body.SimplifyMacros();

            var instructions = method.Body.Instructions;
            for (var index = 0; index < instructions.Count; index++)
            {
                var instr = instructions[index];
                if (instr.OpCode != OpCodes.Callvirt && instr.OpCode != OpCodes.Call) continue;
                if (!(instr.Operand is MethodReference methodReference)) continue;

                if (methodReference.DeclaringType.FullName != "ArkSavegameToolkitNet.DataReaders.ArchiveReader") continue;
                if (methodReference.Parameters.LastOrDefault(x => x.Name.Equals("varName")) == null) continue;

                var mop = instr.Operand as MethodReference;

                if (instr.Previous?.OpCode == OpCodes.Ldstr)
                {
                    LogInfo($@"{mop.FullName} called from {method.FullName}: Removing argument varName=""{instr.Previous.Operand}""");

                    instructions.RemoveAt(index-1);
                    instructions.Insert(index - 1, Instruction.Create(OpCodes.Ldnull));
                }
            }

            method.Body.OptimizeMacros();
        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield return "netstandard";
            yield return "mscorlib";
            yield return "System";
            yield return "System.Runtime";
            yield return "System.Core";
        }

        public override bool ShouldCleanReference => true;
    }
}