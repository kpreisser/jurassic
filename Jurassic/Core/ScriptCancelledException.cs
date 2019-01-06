using System;

namespace Jurassic
{
    /// <summary>
    /// Indicates that the current script execution has been cancelled. 
    /// </summary>
    public class ScriptCancelledException : Exception
    {
        public ScriptCancelledException(string message) 
            : base(message)
        {
        }
    }
}
