using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005F RID: 95
	[NullableContext(1)]
	[Nullable(0)]
	internal class LateBoundReflectionDelegateFactory : ReflectionDelegateFactory
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x00016D5F File Offset: 0x00014F5F
		internal static ReflectionDelegateFactory Instance
		{
			get
			{
				return LateBoundReflectionDelegateFactory._instance;
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00016D68 File Offset: 0x00014F68
		public override ObjectConstructor<object> CreateParameterizedConstructor(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			ConstructorInfo c = method as ConstructorInfo;
			if (c != null)
			{
				return ([Nullable(new byte[] { 1, 2 })] object[] a) => c.Invoke(a);
			}
			return ([Nullable(new byte[] { 1, 2 })] object[] a) => method.Invoke(null, a);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00016DC4 File Offset: 0x00014FC4
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public override MethodCall<T, object> CreateMethodCall<[Nullable(2)] T>(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			ConstructorInfo c = method as ConstructorInfo;
			if (c != null)
			{
				return (T o, [Nullable(new byte[] { 1, 2 })] object[] a) => c.Invoke(a);
			}
			return (T o, [Nullable(new byte[] { 1, 2 })] object[] a) => method.Invoke(o, a);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00016E20 File Offset: 0x00015020
		public override Func<T> CreateDefaultConstructor<[Nullable(2)] T>(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsValueType())
			{
				return () => (T)((object)Activator.CreateInstance(type));
			}
			ConstructorInfo constructorInfo = ReflectionUtils.GetDefaultConstructor(type, true);
			if (constructorInfo == null)
			{
				throw new InvalidOperationException("Unable to find default constructor for " + type.FullName);
			}
			return () => (T)((object)constructorInfo.Invoke(null));
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00016EAB File Offset: 0x000150AB
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public override Func<T, object> CreateGet<[Nullable(2)] T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			return (T o) => propertyInfo.GetValue(o, null);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00016ED4 File Offset: 0x000150D4
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public override Func<T, object> CreateGet<[Nullable(2)] T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			return (T o) => fieldInfo.GetValue(o);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00016EFD File Offset: 0x000150FD
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public override Action<T, object> CreateSet<[Nullable(2)] T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			return delegate(T o, [Nullable(2)] object v)
			{
				fieldInfo.SetValue(o, v);
			};
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00016F26 File Offset: 0x00015126
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public override Action<T, object> CreateSet<[Nullable(2)] T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			return delegate(T o, [Nullable(2)] object v)
			{
				propertyInfo.SetValue(o, v, null);
			};
		}

		// Token: 0x04000214 RID: 532
		private static readonly LateBoundReflectionDelegateFactory _instance = new LateBoundReflectionDelegateFactory();
	}
}
