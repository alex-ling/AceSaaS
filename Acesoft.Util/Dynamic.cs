using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;

namespace Acesoft
{
    public delegate object DynamicInstanceCreator();
    public delegate U DynamicInstanceCreator<U>() where U : class;
    public delegate object DynamicPropertyGetter(object target);
    public delegate V DynamicPropertyGetter<U, V>(U target) where U : class;
    public delegate void DynamicPropertySetter(object target, object parameter);
    public delegate void DynamicPropertySetter<U, V>(U target, V parameter) where U : class;
    public delegate object DynamicMethodInvoker(object target, params object[] paramters);
    public delegate V DynamicMethodInvoker<U, V>(U target, params object[] paramters) where U : class;

    /// <summary>
    /// 利用Emit方式构造动态调用，代替反射功能
    /// </summary>
    public static class Dynamic
    {
        static readonly ConcurrentDictionary<Type, PropertyInfo[]> ReflectionCache =
            new ConcurrentDictionary<Type, PropertyInfo[]>();

        public static PropertyInfo[] GetProperties(Type type)
        {
            return ReflectionCache.GetOrAdd(type, _ => type.GetProperties());
        }

        #region internalcreator

        /// <summary>
        /// 返回动态对象的实例委托，可以调用私有对象
        /// </summary>
        /// <param name="moudle">程序集模块</param>
        /// <param name="typeFullName">完成类型名</param>
        /// <param name="isPublic">是否公有</param>
        /// <param name="paramters">参数</param>
        /// <returns>对象的实例</returns>
        public static object InternalCreator(Module moudle, string typeFullName, bool isPublic, params object[] paramters)
        {
            string methodStr = "System.Object CreateInstance(System.String, Boolean, System.Reflection.BindingFlags, "
                + "System.Reflection.Binder, System.Object[], System.Globalization.CultureInfo, System.Object[])";
            foreach (MethodInfo methodInfo in typeof(Assembly).GetMethods())
            {
                if (methodInfo.ToString() == methodStr)
                {
                    DynamicMethodInvoker invoker = GetMethodInvoker(methodInfo);
                    return invoker(moudle.Assembly, typeFullName, false, BindingFlags.Instance |
                        (isPublic ? BindingFlags.Public : BindingFlags.NonPublic), null, null, null, null);
                }
            }

            return null;
        }

        #endregion

        #region instancecreator

        /// <summary>
        /// 返回动态对象的实例委托
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>委托<seealso cref="DynamicInstanceCreator"/></returns>
        public static DynamicInstanceCreator GetInstanceCreator(Type type)
        {
            DynamicMethod dm = new DynamicMethod(string.Empty, type, new Type[0], type.Module);
            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Ret);

            return (DynamicInstanceCreator)dm.CreateDelegate(typeof(DynamicInstanceCreator));
        }

        /// <summary>
        /// 返回动态对象的实例委托
        /// </summary>
        /// <typeparam name="U">范型对象U</typeparam>
        /// <returns>委托<seealso cref="DynamicInstanceCreator&lt;U&gt;"/></returns>
        public static DynamicInstanceCreator<U> GetInstanceCreator<U>() where U : class
        {
            Type type = typeof(U);
            DynamicMethod dm = new DynamicMethod(string.Empty, type, new Type[0], type.Module);
            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Ret);

            return (DynamicInstanceCreator<U>)dm.CreateDelegate(typeof(DynamicInstanceCreator<U>));
        }

        #endregion

        #region propertygetter

        /// <summary>
        /// 返回动态属性GET委托
        /// </summary>
        /// <param name="propInfo">属性Info</param>
        /// <returns>委托<seealso cref="DynamicPropertyGetter"/></returns>
        public static DynamicPropertyGetter GetPropertyGetter(PropertyInfo propInfo)
        {
            DynamicMethod dm = new DynamicMethod(string.Empty, typeof(object),
                new Type[] { typeof(object) }, propInfo.DeclaringType.Module);
            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.EmitCall(OpCodes.Callvirt, propInfo.GetGetMethod(), null);

            EmitBoxIfNeeded(il, propInfo.PropertyType);

            il.Emit(OpCodes.Ret);
            return (DynamicPropertyGetter)dm.CreateDelegate(typeof(DynamicPropertyGetter));
        }

        /// <summary>
        /// 返回动态属性GET委托
        /// </summary>
        /// <typeparam name="U">范型类U</typeparam>
        /// <typeparam name="V">范型属性V</typeparam>
        /// <param name="propInfo">属性Info</param>
        /// <returns>委托<seealso cref="DynamicPropertyGetter&lt;U, V&gt;"/></returns>
        public static DynamicPropertyGetter<U, V> GetPropertyGetter<U, V>(PropertyInfo propInfo) where U : class
        {
            DynamicMethod dm = new DynamicMethod(string.Empty, typeof(V),
                new Type[] { typeof(U) }, propInfo.DeclaringType.Module);
            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.EmitCall(OpCodes.Callvirt, propInfo.GetGetMethod(), null);
            il.Emit(OpCodes.Ret);

            return (DynamicPropertyGetter<U, V>)dm.CreateDelegate(typeof(DynamicPropertyGetter<U, V>));
        }

        /// <summary>
        /// 动态调用属性
        /// </summary>
        /// <param name="obj">Obj对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性值</returns>
        public static object GetPropertyValue(object obj, string propertyName)
        {
            PropertyInfo pro = obj.GetType().GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            return GetPropertyGetter(pro).Invoke(obj);
        }

        #endregion

        #region propertysetter

        /// <summary>
        /// 返回动态属性SET委托
        /// </summary>
        /// <param name="propInfo">属性Info</param>
        /// <returns>委托<seealso cref="DynamicPropertySetter"/></returns>
        public static DynamicPropertySetter GetPropertySetter(PropertyInfo propInfo)
        {
            DynamicMethod dm = new DynamicMethod(string.Empty, null,
                new Type[] { typeof(object), typeof(object) }, propInfo.DeclaringType.Module);
            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);

            EmitCastToReference(il, propInfo.PropertyType);

            il.EmitCall(OpCodes.Callvirt, propInfo.GetSetMethod(), null);
            il.Emit(OpCodes.Ret);

            return (DynamicPropertySetter)dm.CreateDelegate(typeof(DynamicPropertySetter));
        }

        /// <summary>
        /// 返回动态属性SET委托
        /// </summary>
        /// <typeparam name="U">范型类U</typeparam>
        /// <typeparam name="V">范型属性V</typeparam>
        /// <param name="propInfo">属性Info</param>
        /// <returns>委托<seealso cref="DynamicPropertySetter&lt;U, V&gt;"/></returns>
        public static DynamicPropertySetter<U, V> GetPropertySetter<U, V>(PropertyInfo propInfo) where U : class
        {
            DynamicMethod dm = new DynamicMethod(string.Empty, null,
                new Type[] { typeof(U), typeof(V) }, propInfo.DeclaringType.Module);
            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, propInfo.GetSetMethod(), null);
            il.Emit(OpCodes.Ret);

            return (DynamicPropertySetter<U, V>)dm.CreateDelegate(typeof(DynamicPropertySetter<U, V>));
        }

        /// <summary>
        /// 动态设置属性
        /// </summary>
        /// <param name="obj">Obj对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">属性值</param>
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            PropertyInfo pro = obj.GetType().GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (pro == null)
            {
                throw new Exception(string.Format("The object type: {0} doesn't having {1} property.", obj.GetType().FullName, propertyName));
            }
            GetPropertySetter(pro).Invoke(obj, value.ToString().ToObject(pro.PropertyType));
        }

        #endregion

        #region methodinvoker

        /// <summary>
        /// 返回动态调用方法的委托
        /// </summary>
        /// <param name="methodInfo">方法Info</param>
        /// <param name="genericTypes">方法中范型类型集合</param>
        /// <returns>委托<seealso cref="DynamicMethodInvoker"/></returns>
        public static DynamicMethodInvoker GetMethodInvoker(MethodInfo methodInfo, params Type[] genericTypes)
        {
            DynamicMethod dm = new DynamicMethod(string.Empty, typeof(object),
            new Type[] { typeof(object), typeof(object[]) }, methodInfo.DeclaringType.Module);

            if (methodInfo.IsGenericMethod)
            {
                if (genericTypes == null || genericTypes.Length != methodInfo.GetGenericArguments().Length)
                {
                    throw new TargetParameterCountException(string.Format("调用范型方法{0}参数不匹配！", methodInfo.Name));
                }

                methodInfo = methodInfo.MakeGenericMethod(genericTypes);
            }

            IL(dm, methodInfo, false);

            return (DynamicMethodInvoker)dm.CreateDelegate(typeof(DynamicMethodInvoker));
        }

        /// <summary>
        /// 返回动态调用方法的委托
        /// </summary>
        /// <typeparam name="U">范型类U</typeparam>
        /// <typeparam name="V">返回值范型V</typeparam>
        /// <param name="methodInfo">方法Info</param>
        /// <param name="genericTypes">方法中范型类型集合</param>
        /// <returns>委托<seealso cref="DynamicMethodInvoker&lt;U, V&gt;"/></returns>
        public static DynamicMethodInvoker<U, V> GetMethodInvoker<U, V>(MethodInfo methodInfo, params Type[] genericTypes) where U : class
        {
            DynamicMethod dm = new DynamicMethod(string.Empty, typeof(V),
                new Type[] { typeof(U), typeof(object[]) }, methodInfo.DeclaringType.Module);

            if (methodInfo.IsGenericMethod)
            {
                if (genericTypes == null || genericTypes.Length != methodInfo.GetGenericArguments().Length)
                {
                    throw new TargetParameterCountException(string.Format("调用范型方法{0}参数不匹配！", methodInfo.Name));
                }

                methodInfo = methodInfo.MakeGenericMethod(genericTypes);
            }

            IL(dm, methodInfo, true);

            return (DynamicMethodInvoker<U, V>)dm.CreateDelegate(typeof(DynamicMethodInvoker<U, V>));
        }

        #endregion

        #region privates

        private static void IL(DynamicMethod dm, MethodInfo methodInfo, bool isGeneric)
        {
            ILGenerator il = dm.GetILGenerator();
            ParameterInfo[] ps = methodInfo.GetParameters();
            Type[] paramTypes = new Type[ps.Length];

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                }
                else
                {
                    paramTypes[i] = ps[i].ParameterType;
                }
            }

            LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = il.DeclareLocal(paramTypes[i], true);
            }

            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                EmitFastInt(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(il, paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }

            if (!methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    il.Emit(OpCodes.Ldloca_S, locals[i]);
                }
                else
                {
                    il.Emit(OpCodes.Ldloc, locals[i]);
                }
            }

            if (methodInfo.IsStatic)
            {
                il.EmitCall(OpCodes.Call, methodInfo, null);
            }
            else
            {
                il.EmitCall(OpCodes.Callvirt, methodInfo, null);
            }

            if (methodInfo.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (!isGeneric)
            {
                EmitBoxIfNeeded(il, methodInfo.ReturnType);
            }

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    EmitFastInt(il, i);
                    il.Emit(OpCodes.Ldloc, locals[i]);
                    if (locals[i].LocalType.IsValueType)
                    {
                        il.Emit(OpCodes.Box, locals[i].LocalType);
                    }
                    il.Emit(OpCodes.Stelem_Ref);
                }
            }

            il.Emit(OpCodes.Ret);
        }

        private static void EmitCastToReference(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

        private static void EmitBoxIfNeeded(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        private static void EmitFastInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }

        #endregion
    }
}