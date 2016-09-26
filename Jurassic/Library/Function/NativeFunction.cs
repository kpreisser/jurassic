namespace Jurassic.Library
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NativeFunction : FunctionInstance
    {
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
        public NativeFunction(ObjectInstance prototype, ObjectInstance instancePrototype,
            string name, int argumentsLength)
            : base(prototype)
        {
            // Set function properties.
            this.DefineProperty("name", new PropertyDescriptor(
                name, PropertyAttributes.Configurable), true);
            this.DefineProperty("length", new PropertyDescriptor(
                argumentsLength, PropertyAttributes.Configurable), true);
            this.DefineProperty("prototype", new PropertyDescriptor(
                instancePrototype, PropertyAttributes.Writable), true);

            instancePrototype.DefineProperty("constructor", new PropertyDescriptor(
                this, PropertyAttributes.Configurable | PropertyAttributes.Writable), true);

            PopulateFunctions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisObject"></param>
        /// <param name="argumentValues"></param>
        /// <returns></returns>
        public override object CallLateBound(object thisObject, params object[] argumentValues)
        {
            if (this.Engine.CompatibilityMode == CompatibilityMode.ECMAScript3)
            {
                // Convert null or undefined to the global object.
                if (TypeUtilities.IsUndefined(thisObject) == true || thisObject == Null.Value)
                    thisObject = this.Engine.Global;
                else
                    thisObject = TypeConverter.ToObject(this.Engine, thisObject);
            }
            try
            {
                return this.CallLateBoundCore(thisObject, argumentValues);
            }
            catch (JavaScriptException ex)
            {
                if (ex.FunctionName == null && ex.SourcePath == null && ex.LineNumber == 0)
                {
                    ex.FunctionName = this.DisplayName;
                    ex.SourcePath = "native";
                    ex.PopulateStackTrace(true);
                }
                throw;
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
