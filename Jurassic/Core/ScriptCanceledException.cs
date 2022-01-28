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
        /// <param name="engine"></param>
        internal ScriptCanceledException(string message, ScriptEngine engine)
            : base(message)
        {
            this.Stack = engine.FormatStackTrace(null, null, null, null, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public string Stack
        {
            get;
        }
    }
}
