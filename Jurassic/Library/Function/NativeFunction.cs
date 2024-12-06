using System;

namespace Jurassic.Library
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NativeFunction : FunctionInstance
    {
        private readonly bool producesStackFrame;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prototype"></param>
        public NativeFunction(ObjectInstance prototype)
            : this(prototype,
                  ObjectConstructor.Create(prototype.Engine, prototype.Engine.Object.InstancePrototype),
                  null, 0)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prototype"></param>
        /// <param name="instancePrototype"></param>
        /// <param name="name"></param>
        /// <param name="argumentsLength"></param>
        /// <param name="producesStackFrame"></param>
        public NativeFunction(ObjectInstance prototype, ObjectInstance instancePrototype,
            string name, int argumentsLength, bool producesStackFrame = true)
            : base(prototype)
        {
            this.producesStackFrame = producesStackFrame;

            // Set function properties.
            this.DefineProperty("name", new PropertyDescriptor(
                name, PropertyAttributes.Configurable), true);
            this.DefineProperty("length", new PropertyDescriptor(
                argumentsLength, PropertyAttributes.Configurable), true);
            // The prototype property of built-in function is generally sealed
            // (see e.g. ArrayBuffer.prototype), so we do the same.
            this.DefineProperty("prototype", new PropertyDescriptor(
                instancePrototype, PropertyAttributes.Sealed), true);

            instancePrototype.DefineProperty("constructor", new PropertyDescriptor(
                this, PropertyAttributes.Configurable | PropertyAttributes.Writable), true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisObject"></param>
        /// <param name="argumentValues"></param>
        /// <returns></returns>
        public override object CallLateBound(object thisObject, params object[] argumentValues)
        {
            // Check the allowed recursion depth.
            if (this.Engine.RecursionDepthLimit > 0 && UserDefinedFunction.currentRecursionDepth >= this.Engine.RecursionDepthLimit)
                throw new StackOverflowException("The allowed recursion depth of the script engine has been exceeded.");

            if (this.Engine.CompatibilityMode == CompatibilityMode.ECMAScript3)
            {
                // Convert null or undefined to the global object.
                if (TypeUtilities.IsUndefined(thisObject) == true || thisObject == Null.Value)
                    thisObject = this.Engine.Global;
                else
                    thisObject = TypeConverter.ToObject(this.Engine, thisObject);
            }

            // For NativeFunction, we immediately push a stack frame. For Clr[Stub]Function, Jurassic
            // pushes the stack frame e.g. when calling another function by using CallFromNative(), or
            // when an JavaScriptException is thrown.
            // TODO: The CallType specifies the type of the next method call, but we push the stack frame
            // immediately since we don't have a line number for native methods and this makes constructing
            // Error objects easier since they will already contain the correct stack. However, if the
            // native method calls a constructor function, the calltype will be incorrect.
            // To fix this, we would need to somehow allow to modify the current stack frame.
            if (this.producesStackFrame)
                this.Engine.PushStackFrame("native", Name, 0, ScriptEngine.CallType.MethodCall);
            UserDefinedFunction.currentRecursionDepth++;
            try
            {
                return this.CallLateBoundCore(thisObject, argumentValues);
            }
            catch (JavaScriptException ex)
            {
                // Ensure to populate the stack trace now.
                ex.GetErrorObject(Engine);
                throw;
            }
            finally
            {
                UserDefinedFunction.currentRecursionDepth--;
                if (this.producesStackFrame)
                    this.Engine.PopStackFrame();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisObject"></param>
        /// <param name="argumentValues"></param>
        /// <returns></returns>
        protected abstract object CallLateBoundCore(object thisObject, params object[] argumentValues);
    }
}
