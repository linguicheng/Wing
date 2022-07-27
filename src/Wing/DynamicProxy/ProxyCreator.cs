using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Wing.DynamicProxy
{
    public class ProxyCreator
    {
        private const string NamespacePrefix = "Wing.Proxy";
        private static readonly ModuleBuilder ModuleBuilder;

        static ProxyCreator()
        {
            ModuleBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(NamespacePrefix), AssemblyBuilderAccess.Run)
                            .DefineDynamicModule($"{NamespacePrefix}Module");
        }

        public static Type CreateClassType(Type type)
        {
            var typeBuilder = ModuleBuilder.DefineType($"{NamespacePrefix}.{type.Name}", type.Attributes, type);
            var methodAttrTuple = GetMethodAttribute(type, typeBuilder);
            var attrFields = methodAttrTuple.Item1;
            if (!attrFields.Any())
            {
                return type;
            }

            var aspectContextField = typeBuilder.DefineField("_" + nameof(AspectContext), typeof(IAspectContext), FieldAttributes.Private | FieldAttributes.InitOnly);
            CreateConstructor(type, aspectContextField, typeBuilder, attrFields);

            foreach (var item in methodAttrTuple.Item2)
            {
                var method = item.Key;
                var attrType = item.Value;
                var returnType = method.ReturnType;
                var parameters = method.GetParameters();
                var methodBuilder = typeBuilder.DefineMethod(
                    method.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                    CallingConventions.Standard,
                    returnType,
                    parameters.Select(x => x.ParameterType).ToArray());
                if (method.IsGenericMethod)
                {
                    methodBuilder.DefineGenericParameters(method.GetGenericArguments().Select(x => x.Name).ToArray());
                }

                var ilGen = methodBuilder.GetILGenerator();
                if (returnType != typeof(void))
                {
                    ilGen.DeclareLocal(returnType);
                }

                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, aspectContextField);
                ilGen.Emit(OpCodes.Castclass, typeof(AspectContext));
                ilGen.Emit(OpCodes.Callvirt, typeof(AspectContext).GetMethod($"get_{nameof(AspectContext.NewContext)}"));
                ilGen.Emit(OpCodes.Stloc_0);

                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Call, typeof(MethodBase).GetMethod(nameof(MethodBase.GetCurrentMethod))); // 执行方法
                ilGen.Emit(OpCodes.Castclass, typeof(MethodInfo));          // 尝试将引用传递的对象转换为指定的类。
                ilGen.Emit(OpCodes.Callvirt, typeof(AspectContext).GetMethod($"set_{nameof(AspectContext.ProxyMethod)}"));
            }

            return type;
        }

        private static void CreateConstructor(Type type, FieldBuilder aspectContextField, TypeBuilder typeBuilder, Dictionary<Type, FieldBuilder> attrFields)
        {
            var constructors = type.GetConstructors();
            foreach (var item in constructors)
            {
                var parameters = item.GetParameters();
                var constructorBuilder = typeBuilder.DefineConstructor(item.Attributes, item.CallingConvention, parameters.Select(x => x.ParameterType).ToArray());
                var ilGen = constructorBuilder.GetILGenerator();
                foreach (var p in parameters)
                {
                    ilGen.DeclareLocal(p.ParameterType);
                }

                ilGen.Emit(OpCodes.Ldarg_0);
                EmitMethodParamters(parameters.Length, ilGen);
                ilGen.Emit(OpCodes.Call, item);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Newobj, typeof(AspectContext).GetConstructor(new Type[0]));
                ilGen.Emit(OpCodes.Stfld, aspectContextField);
                foreach (var attr in attrFields)
                {
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Newobj, attr.Key.GetConstructor(new Type[0]));
                    ilGen.Emit(OpCodes.Stfld, attr.Value);
                }

                ilGen.Emit(OpCodes.Ret);
            }
        }

        private static void EmitMethodParamters(int length, ILGenerator ilGen)
        {
            if (length <= 0)
            {
                return;
            }

            switch (length)
            {
                case 1:
                    ilGen.Emit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Ldarg_2);
                    ilGen.Emit(OpCodes.Ldarg_3);
                    break;
                default:
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Ldarg_2);
                    ilGen.Emit(OpCodes.Ldarg_3);
                    for (int i = 4; i <= length; i++)
                    {
                        ilGen.Emit(OpCodes.Ldarg_S, i);
                    }

                    break;
            }
        }

        private static (Dictionary<Type, FieldBuilder>, Dictionary<MethodInfo, Type>) GetMethodAttribute(Type type, TypeBuilder typeBuilder)
        {
            var attrFields = new Dictionary<Type, FieldBuilder>();
            var attrMethods = new Dictionary<MethodInfo, Type>();
            foreach (var item in type.GetMethods())
            {
                var attr = item.GetCustomAttributes().FirstOrDefault(x => x.GetType().BaseType == typeof(InterceptorAttribute));
                if (attr == null)
                {
                    continue;
                }

                var attrType = attr.GetType();
                attrMethods.Add(item, attrType);
                if (!attrFields.ContainsKey(attrType))
                {
                    attrFields.Add(attrType, typeBuilder.DefineField("_" + attrType.Name, attrType, FieldAttributes.Private | FieldAttributes.InitOnly));
                }
            }

            return (attrFields, attrMethods);
        }
    }
}
