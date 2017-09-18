using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Exceptions
{
    public class UnreadablePropertyException : Exception
    {
        public UnreadablePropertyException() : base()
        {
        }

        public UnreadablePropertyException(string message, Exception ex) : base(message, ex)
        {
        }

        public UnreadablePropertyException(string message) : base(message)
        {
        }

        public UnreadablePropertyException(Exception ex) : base(null, ex)
        {
        }

    }
}
