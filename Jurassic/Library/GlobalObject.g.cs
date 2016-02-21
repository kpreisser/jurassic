/*
 * This file is auto-generated, do not modify directly.
 */

using System.Collections.Generic;
using Jurassic;

namespace Jurassic.Library
{

	public partial class GlobalObject
	{
		internal new List<PropertyNameAndValue> GetDeclarativeProperties()
		{
			return new List<PropertyNameAndValue>(15)
			{
				new PropertyNameAndValue("decodeURI", new ClrStubFunction(this.Engine.Function.InstancePrototype, "decodeURI", 1, __STUB__decodeURI), PropertyAttributes.NonEnumerable),
				new PropertyNameAndValue("decodeURIComponent", new ClrStubFunction(this.Engine.Function.InstancePrototype, "decodeURIComponent", 1, __STUB__decodeURIComponent), PropertyAttributes.NonEnumerable),
				new PropertyNameAndValue("encodeURI", new ClrStubFunction(this.Engine.Function.InstancePrototype, "encodeURI", 1, __STUB__encodeURI), PropertyAttributes.NonEnumerable),
				new PropertyNameAndValue("encodeURIComponent", new ClrStubFunction(this.Engine.Function.InstancePrototype, "encodeURIComponent", 1, __STUB__encodeURIComponent), PropertyAttributes.NonEnumerable),
				new PropertyNameAndValue("escape", new ClrStubFunction(this.Engine.Function.InstancePrototype, "escape", 1, __STUB__escape), PropertyAttributes.NonEnumerable),
				new PropertyNameAndValue("eval", new ClrStubFunction(this.Engine.Function.InstancePrototype, "eval", 1, __STUB__eval), PropertyAttributes.NonEnumerable),
				new PropertyNameAndValue("isFinite", new ClrStubFunction(this.Engine.Function.InstancePrototype, "isFinite", 1, __STUB__isFinite), PropertyAttributes.NonEnumerable),
				new PropertyNameAndValue("isNaN", new ClrStubFunction(this.Engine.Function.InstancePrototype, "isNaN", 1, __STUB__isNaN), PropertyAttributes.NonEnumerable),
				new PropertyNameAndValue("parseFloat", new ClrStubFunction(this.Engine.Function.InstancePrototype, "parseFloat", 1, __STUB__parseFloat), PropertyAttributes.NonEnumerable),
				new PropertyNameAndValue("parseInt", new ClrStubFunction(this.Engine.Function.InstancePrototype, "parseInt", 2, __STUB__parseInt), PropertyAttributes.NonEnumerable),
				new PropertyNameAndValue("unescape", new ClrStubFunction(this.Engine.Function.InstancePrototype, "unescape", 1, __STUB__unescape), PropertyAttributes.NonEnumerable),
			};
		}

		private static object __STUB__decodeURI(ScriptEngine engine, object thisObj, object[] args)
		{
			switch (args.Length)
			{
				case 0:
					return DecodeURI(engine, "undefined");
				default:
					return DecodeURI(engine, TypeConverter.ToString(args[0]));
			}
		}

		private static object __STUB__decodeURIComponent(ScriptEngine engine, object thisObj, object[] args)
		{
			switch (args.Length)
			{
				case 0:
					return DecodeURIComponent(engine, "undefined");
				default:
					return DecodeURIComponent(engine, TypeConverter.ToString(args[0]));
			}
		}

		private static object __STUB__encodeURI(ScriptEngine engine, object thisObj, object[] args)
		{
			switch (args.Length)
			{
				case 0:
					return EncodeURI(engine, "undefined");
				default:
					return EncodeURI(engine, TypeConverter.ToString(args[0]));
			}
		}

		private static object __STUB__encodeURIComponent(ScriptEngine engine, object thisObj, object[] args)
		{
			switch (args.Length)
			{
				case 0:
					return EncodeURIComponent(engine, "undefined");
				default:
					return EncodeURIComponent(engine, TypeConverter.ToString(args[0]));
			}
		}

		private static object __STUB__escape(ScriptEngine engine, object thisObj, object[] args)
		{
			switch (args.Length)
			{
				case 0:
					return Escape("undefined");
				default:
					return Escape(TypeConverter.ToString(args[0]));
			}
		}

		private static object __STUB__eval(ScriptEngine engine, object thisObj, object[] args)
		{
			switch (args.Length)
			{
				case 0:
					return Eval(engine, Undefined.Value);
				default:
					return Eval(engine, args[0]);
			}
		}

		private static object __STUB__isFinite(ScriptEngine engine, object thisObj, object[] args)
		{
			switch (args.Length)
			{
				case 0:
					return IsFinite(double.NaN);
				default:
					return IsFinite(TypeConverter.ToNumber(args[0]));
			}
		}

		private static object __STUB__isNaN(ScriptEngine engine, object thisObj, object[] args)
		{
			switch (args.Length)
			{
				case 0:
					return IsNaN(double.NaN);
				default:
					return IsNaN(TypeConverter.ToNumber(args[0]));
			}
		}

		private static object __STUB__parseFloat(ScriptEngine engine, object thisObj, object[] args)
		{
			switch (args.Length)
			{
				case 0:
					return ParseFloat("undefined");
				default:
					return ParseFloat(TypeConverter.ToString(args[0]));
			}
		}

		private static object __STUB__parseInt(ScriptEngine engine, object thisObj, object[] args)
		{
			switch (args.Length)
			{
				case 0:
					return ParseInt(engine, "undefined", 0);
				case 1:
					return ParseInt(engine, TypeConverter.ToString(args[0]), 0);
				default:
					return ParseInt(engine, TypeConverter.ToString(args[0]), TypeConverter.ToNumber(args[1], 0));
			}
		}

		private static object __STUB__unescape(ScriptEngine engine, object thisObj, object[] args)
		{
			switch (args.Length)
			{
				case 0:
					return Unescape("undefined");
				default:
					return Unescape(TypeConverter.ToString(args[0]));
			}
		}
	}

}