using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005A RID: 90
	[NullableContext(1)]
	[Nullable(0)]
	internal class FSharpUtils
	{
		// Token: 0x0600051A RID: 1306 RVA: 0x00015CC0 File Offset: 0x00013EC0
		private FSharpUtils(Assembly fsharpCoreAssembly)
		{
			this.FSharpCoreAssembly = fsharpCoreAssembly;
			Type type = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpType");
			MethodInfo methodWithNonPublicFallback = FSharpUtils.GetMethodWithNonPublicFallback(type, "IsUnion", BindingFlags.Static | BindingFlags.Public);
			this.IsUnion = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback);
			MethodInfo methodWithNonPublicFallback2 = FSharpUtils.GetMethodWithNonPublicFallback(type, "GetUnionCases", BindingFlags.Static | BindingFlags.Public);
			this.GetUnionCases = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback2);
			Type type2 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpValue");
			this.PreComputeUnionTagReader = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionTagReader");
			this.PreComputeUnionReader = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionReader");
			this.PreComputeUnionConstructor = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionConstructor");
			Type type3 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.UnionCaseInfo");
			this.GetUnionCaseInfoName = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Name"));
			this.GetUnionCaseInfoTag = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Tag"));
			this.GetUnionCaseInfoDeclaringType = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("DeclaringType"));
			this.GetUnionCaseInfoFields = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(type3.GetMethod("GetFields"));
			Type type4 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.ListModule");
			this._ofSeq = type4.GetMethod("OfSeq");
			this._mapType = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.FSharpMap`2");
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00015E09 File Offset: 0x00014009
		public static FSharpUtils Instance
		{
			get
			{
				return FSharpUtils._instance;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00015E10 File Offset: 0x00014010
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x00015E18 File Offset: 0x00014018
		public Assembly FSharpCoreAssembly { get; private set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00015E21 File Offset: 0x00014021
		// (set) Token: 0x0600051F RID: 1311 RVA: 0x00015E29 File Offset: 0x00014029
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> IsUnion
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00015E32 File Offset: 0x00014032
		// (set) Token: 0x06000521 RID: 1313 RVA: 0x00015E3A File Offset: 0x0001403A
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> GetUnionCases
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x00015E43 File Offset: 0x00014043
		// (set) Token: 0x06000523 RID: 1315 RVA: 0x00015E4B File Offset: 0x0001404B
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> PreComputeUnionTagReader
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00015E54 File Offset: 0x00014054
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x00015E5C File Offset: 0x0001405C
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> PreComputeUnionReader
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00015E65 File Offset: 0x00014065
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x00015E6D File Offset: 0x0001406D
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> PreComputeUnionConstructor
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00015E76 File Offset: 0x00014076
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x00015E7E File Offset: 0x0001407E
		public Func<object, object> GetUnionCaseInfoDeclaringType { get; private set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00015E87 File Offset: 0x00014087
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x00015E8F File Offset: 0x0001408F
		public Func<object, object> GetUnionCaseInfoName { get; private set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00015E98 File Offset: 0x00014098
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x00015EA0 File Offset: 0x000140A0
		public Func<object, object> GetUnionCaseInfoTag { get; private set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00015EA9 File Offset: 0x000140A9
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x00015EB1 File Offset: 0x000140B1
		[Nullable(new byte[] { 1, 1, 2 })]
		public MethodCall<object, object> GetUnionCaseInfoFields
		{
			[return: Nullable(new byte[] { 1, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 1, 1, 2 })]
			private set;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00015EBC File Offset: 0x000140BC
		public static void EnsureInitialized(Assembly fsharpCoreAssembly)
		{
			if (FSharpUtils._instance == null)
			{
				object @lock = FSharpUtils.Lock;
				lock (@lock)
				{
					if (FSharpUtils._instance == null)
					{
						FSharpUtils._instance = new FSharpUtils(fsharpCoreAssembly);
					}
				}
			}
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00015F10 File Offset: 0x00014110
		private static MethodInfo GetMethodWithNonPublicFallback(Type type, string methodName, BindingFlags bindingFlags)
		{
			MethodInfo methodInfo = type.GetMethod(methodName, bindingFlags);
			if (methodInfo == null && (bindingFlags & BindingFlags.NonPublic) != BindingFlags.NonPublic)
			{
				methodInfo = type.GetMethod(methodName, bindingFlags | BindingFlags.NonPublic);
			}
			return methodInfo;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00015F44 File Offset: 0x00014144
		[return: Nullable(new byte[] { 1, 2, 1 })]
		private static MethodCall<object, object> CreateFSharpFuncCall(Type type, string methodName)
		{
			MethodInfo methodWithNonPublicFallback = FSharpUtils.GetMethodWithNonPublicFallback(type, methodName, BindingFlags.Static | BindingFlags.Public);
			MethodInfo method = methodWithNonPublicFallback.ReturnType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
			MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback);
			MethodCall<object, object> invoke = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return ([Nullable(2)] object target, [Nullable(new byte[] { 1, 2 })] object[] args) => new FSharpFunction(call(target, args), invoke);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00015FA0 File Offset: 0x000141A0
		public ObjectConstructor<object> CreateSeq(Type t)
		{
			MethodInfo methodInfo = this._ofSeq.MakeGenericMethod(new Type[] { t });
			return JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(methodInfo);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00015FCE File Offset: 0x000141CE
		public ObjectConstructor<object> CreateMap(Type keyType, Type valueType)
		{
			return (ObjectConstructor<object>)typeof(FSharpUtils).GetMethod("BuildMapCreator").MakeGenericMethod(new Type[] { keyType, valueType }).Invoke(this, null);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00016004 File Offset: 0x00014204
		[NullableContext(2)]
		[return: Nullable(1)]
		public ObjectConstructor<object> BuildMapCreator<TKey, TValue>()
		{
			ConstructorInfo constructor = this._mapType.MakeGenericType(new Type[]
			{
				typeof(TKey),
				typeof(TValue)
			}).GetConstructor(new Type[] { typeof(IEnumerable<Tuple<TKey, TValue>>) });
			ObjectConstructor<object> ctorDelegate = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			return delegate([Nullable(new byte[] { 1, 2 })] object[] args)
			{
				IEnumerable<Tuple<TKey, TValue>> enumerable = ((IEnumerable<KeyValuePair<TKey, TValue>>)args[0]).Select<KeyValuePair<TKey, TValue>, Tuple<TKey, TValue>>((KeyValuePair<TKey, TValue> kv) => new Tuple<TKey, TValue>(kv.Key, kv.Value));
				return ctorDelegate(new object[] { enumerable });
			};
		}

		// Token: 0x040001E7 RID: 487
		private static readonly object Lock = new object();

		// Token: 0x040001E8 RID: 488
		[Nullable(2)]
		private static FSharpUtils _instance;

		// Token: 0x040001E9 RID: 489
		private MethodInfo _ofSeq;

		// Token: 0x040001EA RID: 490
		private Type _mapType;

		// Token: 0x040001F5 RID: 501
		public const string FSharpSetTypeName = "FSharpSet`1";

		// Token: 0x040001F6 RID: 502
		public const string FSharpListTypeName = "FSharpList`1";

		// Token: 0x040001F7 RID: 503
		public const string FSharpMapTypeName = "FSharpMap`2";
	}
}
