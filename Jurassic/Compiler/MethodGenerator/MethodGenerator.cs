﻿using System;

namespace Jurassic.Compiler
{
    /// <summary>
    /// Represents the unit of compilation.
    /// </summary>
    internal abstract class MethodGenerator
    {
        /// <summary>
        /// Creates a new MethodGenerator instance.
        /// </summary>
        /// <param name="scope"> The initial scope. </param>
        /// <param name="source"> The source of javascript code. </param>
        /// <param name="options"> Options that influence the compiler. </param>
        protected MethodGenerator(Scope scope, ScriptSource source, CompilerOptions options)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            this.InitialScope = scope;
            this.Source = source;
            this.Options = options;
            this.StrictMode = this.Options.ForceStrictMode;
            this.GenerateCancellationChecks = this.Options.GenerateCancellationChecks;
        }

        /// <summary>
        /// Gets a reference to any compiler options.
        /// </summary>
        public CompilerOptions Options
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the source of javascript code.
        /// </summary>
        public ScriptSource Source
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value that indicates whether strict mode is enabled.
        /// </summary>
        public bool StrictMode
        {
            get;
            protected set;
        }

        public bool GenerateCancellationChecks
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the top-level scope associated with the context.
        /// </summary>
        public Scope InitialScope
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the root node of the abstract syntax tree.  This will be <c>null</c> until Parse()
        /// is called.
        /// </summary>
        public Statement AbstractSyntaxTree
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets optimization information.
        /// </summary>
        public MethodOptimizationHints MethodOptimizationHints
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the generated IL.  This will be <c>null</c> until GenerateCode() is
        /// called.
        /// </summary>
        public ILGenerator ILGenerator
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a delegate to the emitted dynamic method, plus any dependencies.  This will be
        /// <c>null</c> until GenerateCode() is called.
        /// </summary>
        public GeneratedMethod GeneratedMethod
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a name for the generated method.
        /// </summary>
        /// <returns> A name for the generated method. </returns>
        protected abstract string GetMethodName();

        /// <summary>
        /// Gets a name for the function, as it appears in the stack trace.
        /// </summary>
        /// <returns> A name for the function, as it appears in the stack trace, or <c>null</c> if
        /// this generator is generating code in the global scope. </returns>
        protected virtual string GetStackName()
        {
            return null;
        }

        /// <summary>
        /// Gets an array of types - one for each parameter accepted by the method generated by
        /// this context.
        /// </summary>
        /// <returns> An array of parameter types. </returns>
        protected virtual Type[] GetParameterTypes()
        {
            return new Type[] {
                typeof(ScriptEngine),   // The script engine.
                typeof(Scope),          // The scope.
                typeof(object),         // The "this" object.
            };
        }

        /// <summary>
        /// Gets an array of names - one for each parameter accepted by the method being generated.
        /// </summary>
        /// <returns> An array of parameter names. </returns>
        protected virtual string[] GetParameterNames()
        {
            return new string[] { "engine", "scope", "this" };
        }

        /// <summary>
        /// Parses the source text into an abstract syntax tree.
        /// </summary>
        public abstract void Parse();

        /// <summary>
        /// Optimizes the abstract syntax tree.
        /// </summary>
        public void Optimize()
        {
        }

#if ENABLE_DEBUGGING

        internal class ReflectionEmitModuleInfo
        {
            public System.Reflection.Emit.AssemblyBuilder AssemblyBuilder;
            public System.Reflection.Emit.ModuleBuilder ModuleBuilder;
            public int TypeCount;
        }

        private static object reflectionEmitInfoLock = new object();

        /// <summary>
        /// Gets or sets information needed by Reflection.Emit.
        /// </summary>
        private static ReflectionEmitModuleInfo ReflectionEmitInfo;

        /// <summary>
        /// Gets the language type GUID for the symbol store.
        /// </summary>
        private static readonly Guid LanguageType =      // JScript
            new Guid("3A12D0B6-C26C-11D0-B442-00A0244A1DD2");

        /// <summary>
        /// Gets the language vendor GUID for the symbol store.
        /// </summary>
        private static readonly Guid LanguageVendor =
            new Guid("CFA05A92-B7CC-4D3D-92E1-4D18CDACDC8D");


        /// <summary>
        /// Gets the document type GUID for the symbol store.
        /// </summary>
        private static readonly Guid DocumentType =
            new Guid("5A869D0B-6611-11D3-BD2A-0000F80849BD");

#endif

        /// <summary>
        /// Generates IL for the script.
        /// </summary>
        public void GenerateCode()
        {
            // Generate the abstract syntax tree if it hasn't already been generated.
            if (this.AbstractSyntaxTree == null)
            {
                Parse();
                Optimize();
            }

            // Initialize global code-gen information.
            var optimizationInfo = new OptimizationInfo();
            optimizationInfo.AbstractSyntaxTree = this.AbstractSyntaxTree;
            optimizationInfo.StrictMode = this.StrictMode;
            optimizationInfo.GenerateCancellationChecks = this.GenerateCancellationChecks;
            optimizationInfo.MethodOptimizationHints = this.MethodOptimizationHints;
            optimizationInfo.FunctionName = this.GetStackName();
            optimizationInfo.Source = this.Source;

            ILGenerator generator;
            if (this.Options.EnableDebugging == false)
            {
                // DynamicMethod requires full trust because of generator.LoadMethodPointer in the
                // FunctionExpression class.

                // Create a new dynamic method.
                System.Reflection.Emit.DynamicMethod dynamicMethod = new System.Reflection.Emit.DynamicMethod(
                    GetMethodName(),                                        // Name of the generated method.
                    typeof(object),                                         // Return type of the generated method.
                    GetParameterTypes(),                                    // Parameter types of the generated method.
                    typeof(MethodGenerator),                                // Owner type.
                    true);                                                  // Skip visibility checks.
#if USE_DYNAMIC_IL_INFO
                generator = new DynamicILGenerator(dynamicMethod);
#else
                generator = new ReflectionEmitILGenerator(dynamicMethod.GetILGenerator(), emitDebugInfo: false);
#endif

                if (this.Options.EnableILAnalysis == true)
                {
                    // Replace the generator with one that logs.
                    generator = new LoggingILGenerator(generator);
                }

                // Initialization code will appear to come from line 1.
                optimizationInfo.MarkSequencePoint(generator, new SourceCodeSpan(1, 1, 1, 1));

                // Generate the null check that returns immediately if scriptEngine is null,
                // so that we can call it afterwards with null values, in order to have the
                // JIT compiler generate the native code for it immediately, instead of when
                // it is called the first time in the script code.
                GenerateBeginOfMethodNullCheck(generator);

                // Generate the IL.
                GenerateCode(generator, optimizationInfo);
                generator.Complete();

                // Create a delegate from the method.
                this.GeneratedMethod = new GeneratedMethod(dynamicMethod.CreateDelegate(GetDelegate()), optimizationInfo.NestedFunctions);

                // Call the method with null arguments to have the JIT generate native code now
                // instead of when calling the function for the first time (which may lead to
                // delays in time-critical code).
                CallGeneratedMethodWithNullArguments();
            }
            else
            {
#if ENABLE_DEBUGGING
                // Debugging or low trust path.
                ReflectionEmitModuleInfo reflectionEmitInfo;
                System.Reflection.Emit.TypeBuilder typeBuilder;
                lock (reflectionEmitInfoLock)
                {
                    reflectionEmitInfo = ReflectionEmitInfo;
                    if (reflectionEmitInfo == null)
                    {
                        reflectionEmitInfo = new ReflectionEmitModuleInfo();

                        // Create a dynamic assembly and module.
                        reflectionEmitInfo.AssemblyBuilder = System.Threading.Thread.GetDomain().DefineDynamicAssembly(
                            new System.Reflection.AssemblyName("Jurassic Dynamic Assembly"), System.Reflection.Emit.AssemblyBuilderAccess.Run);

                        // Mark the assembly as debuggable.  This must be done before the module is created.
                        var debuggableAttributeConstructor = typeof(System.Diagnostics.DebuggableAttribute).GetConstructor(
                            new Type[] { typeof(System.Diagnostics.DebuggableAttribute.DebuggingModes) });
                        reflectionEmitInfo.AssemblyBuilder.SetCustomAttribute(
                            new System.Reflection.Emit.CustomAttributeBuilder(debuggableAttributeConstructor,
                                new object[] {
                                System.Diagnostics.DebuggableAttribute.DebuggingModes.DisableOptimizations |
                                System.Diagnostics.DebuggableAttribute.DebuggingModes.Default }));

                        // Create a dynamic module.
                        reflectionEmitInfo.ModuleBuilder = reflectionEmitInfo.AssemblyBuilder.DefineDynamicModule("Module", this.Options.EnableDebugging);

                        ReflectionEmitInfo = reflectionEmitInfo;
                    }

                    // Create a new type to hold our method.
                    typeBuilder = reflectionEmitInfo.ModuleBuilder.DefineType("JavaScriptClass" + reflectionEmitInfo.TypeCount.ToString(), System.Reflection.TypeAttributes.Public | System.Reflection.TypeAttributes.Class);
                    reflectionEmitInfo.TypeCount++;
                }

                // Create a method.
                var methodBuilder = typeBuilder.DefineMethod(this.GetMethodName(),
                    System.Reflection.MethodAttributes.HideBySig | System.Reflection.MethodAttributes.Static | System.Reflection.MethodAttributes.Public,
                    typeof(object), GetParameterTypes());

                // Generate the IL for the method.
                generator = new ReflectionEmitILGenerator(methodBuilder.GetILGenerator(), emitDebugInfo: true);

                if (this.Options.EnableILAnalysis == true)
                {
                    // Replace the generator with one that logs.
                    generator = new LoggingILGenerator(generator);
                }

                if (this.Source.Path != null && this.Options.EnableDebugging == true)
                {
                    // Initialize the debugging information.
                    optimizationInfo.DebugDocument = reflectionEmitInfo.ModuleBuilder.DefineDocument(this.Source.Path, LanguageType, LanguageVendor, DocumentType);
                    var parameterNames = GetParameterNames();
                    for (var i = 0; i < parameterNames.Length; i ++)
                        methodBuilder.DefineParameter(i + 1, System.Reflection.ParameterAttributes.In, parameterNames[i]);
                }
                optimizationInfo.MarkSequencePoint(generator, new SourceCodeSpan(1, 1, 1, 1));
                GenerateCode(generator, optimizationInfo);
                generator.Complete();

                // Bake it.
                var type = typeBuilder.CreateType();
                var methodInfo = type.GetMethod(this.GetMethodName());
                this.GeneratedMethod = new GeneratedMethod(Delegate.CreateDelegate(GetDelegate(), methodInfo), optimizationInfo.NestedFunctions);
#else
                throw new NotImplementedException();
#endif // ENABLE_DEBUGGING
            }

            if (this.Options.EnableILAnalysis == true)
            {
                // Store the disassembled IL so it can be retrieved for analysis purposes.
                this.GeneratedMethod.DisassembledIL = generator.ToString();
            }
        }

        /// <summary>
        /// Generates IL for the script.
        /// </summary>
        /// <param name="generator"> The generator to output the CIL to. </param>
        /// <param name="optimizationInfo"> Information about any optimizations that should be performed. </param>
        protected abstract void GenerateCode(ILGenerator generator, OptimizationInfo optimizationInfo);

        /// <summary>
        /// Represents a delegate that is used for global code.  For internal use only.
        /// </summary>
        /// <param name="engine"> The associated script engine. </param>
        /// <param name="scope"> The scope (global or eval context) or the parent scope (function
        /// context). </param>
        /// <param name="thisObject"> The value of the <c>this</c> keyword. </param>
        /// <returns> The result of calling the method. </returns>
        protected delegate object GlobalCodeDelegate(ScriptEngine engine, Scope scope, object thisObject);

        /// <summary>
        /// Retrieves a delegate for the generated method.
        /// </summary>
        /// <returns> The delegate type that matches the method parameters. </returns>
        protected virtual Type GetDelegate()
        {
            return typeof(GlobalCodeDelegate);
        }

        protected virtual void CallGeneratedMethodWithNullArguments()
        {
            var del = (GlobalCodeDelegate)this.GeneratedMethod.GeneratedDelegate;
            del(null, null, null);
        }

        /// <summary>
        /// Generates an "if (scriptEngine is null) return null;", intended to be generated at
        /// the start of the method. This allows to call the method in order to have the JIT
        /// compiler generate the native code for this method.
        /// </summary>
        /// <param name="generator"></param>
        private void GenerateBeginOfMethodNullCheck(ILGenerator generator)
        {
            EmitHelpers.LoadScriptEngine(generator);
            var afterCheckLabel = generator.CreateLabel();
            generator.BranchIfNotNull(afterCheckLabel);
            generator.LoadNull();
            generator.Return();
            generator.DefineLabelPosition(afterCheckLabel);
        }
    }

}