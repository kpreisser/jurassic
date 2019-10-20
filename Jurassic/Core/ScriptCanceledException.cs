using System;

namespace Jurassic
{
    /// <summary>
    /// Indicates that the current script execution has been cancelled. 
    /// </summary>
    public class ScriptCanceledException : OperationCanceledException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ScriptCanceledException(string message) 
            : base(message)
        {
        }
    }
}
