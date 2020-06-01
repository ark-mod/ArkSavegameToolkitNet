using System;

namespace ArkSavegameToolkitNet.Exceptions
{
    public class ArkToolkitException : Exception
    {
        public ArkToolkitException() { }

        public ArkToolkitException(string message) : base(message)
        {
        }
    }
}
