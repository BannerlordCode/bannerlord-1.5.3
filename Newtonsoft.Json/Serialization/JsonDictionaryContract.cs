using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000089 RID: 137
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonDictionaryContract : JsonContainerContract
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0001C27A File Offset: 0x0001A47A
		// (set) Token: 0x060006BC RID: 1724 RVA: 0x0001C282 File Offset: 0x0001A482
		[Nullable(new byte[] { 2, 1, 1 })]
		public Func<string, string> DictionaryKeyResolver
		{
			[return: Nullable(new byte[] { 2, 1, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 1 })]
			set;
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x0001C28B File Offset: 0x0001A48B
		public Type DictionaryKeyType { get; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x0001C293 File Offset: 0x0001A493
		public Type DictionaryValueType { get; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x0001C29B File Offset: 0x0001A49B
		// (set) Token: 0x060006C0 RID: 1728 RVA: 0x0001C2A3 File Offset: 0x0001A4A3
		internal JsonContract KeyContract { get; set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x0001C2AC File Offset: 0x0001A4AC
		internal bool ShouldCreateWrapper { get; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x0001C2B4 File Offset: 0x0001A4B4
		[Nullable(new byte[] { 2, 1 })]
		internal ObjectConstructor<object> ParameterizedCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				if (this._parameterizedCreator == null && this._parameterizedConstructor != null)
				{
					this._parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(this._parameterizedConstructor);
				}
				return this._parameterizedCreator;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0001C2E8 File Offset: 0x0001A4E8
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x0001C2F0 File Offset: 0x0001A4F0
		[Nullable(new byte[] { 2, 1 })]
		public ObjectConstructor<object> OverrideCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._overrideCreator;
			}
			[param: Nullable(new byte[] { 2, 1 })]
			set
			{
				this._overrideCreator = value;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0001C2F9 File Offset: 0x0001A4F9
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x0001C301 File Offset: 0x0001A501
		public bool HasParameterizedCreator { get; set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0001C30A File Offset: 0x0001A50A
		internal bool HasParameterizedCreatorInternal
		{
			get
			{
				return this.HasParameterizedCreator || this._parameterizedCreator != null || this._parameterizedConstructor != null;
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001C32C File Offset: 0x0001A52C
		[NullableContext(1)]
		public JsonDictionaryContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Dictionary;
			Type type;
			Type type2;
			if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof(IDictionary<, >), out this._genericCollectionDefinitionType))
			{
				type = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				type2 = this._genericCollectionDefinitionType.GetGenericArguments()[1];
				if (ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(IDictionary<, >)))
				{
					base.CreatedType = typeof(Dictionary<, >).MakeGenericType(new Type[] { type, type2 });
				}
				else if (this.NonNullableUnderlyingType.IsGenericType() && this.NonNullableUnderlyingType.GetGenericTypeDefinition().FullName == "System.Collections.Concurrent.ConcurrentDictionary`2")
				{
					this.ShouldCreateWrapper = true;
				}
				this.IsReadOnlyOrFixedSize = ReflectionUtils.InheritsGenericDefinition(this.NonNullableUnderlyingType, typeof(ReadOnlyDictionary<, >));
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof(IReadOnlyDictionary<, >), out this._genericCollectionDefinitionType))
			{
				type = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				type2 = this._genericCollectionDefinitionType.GetGenericArguments()[1];
				if (ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(IReadOnlyDictionary<, >)))
				{
					base.CreatedType = typeof(ReadOnlyDictionary<, >).MakeGenericType(new Type[] { type, type2 });
				}
				this.IsReadOnlyOrFixedSize = true;
			}
			else
			{
				ReflectionUtils.GetDictionaryKeyValueTypes(this.NonNullableUnderlyingType, out type, out type2);
				if (this.NonNullableUnderlyingType == typeof(IDictionary))
				{
					base.CreatedType = typeof(Dictionary<object, object>);
				}
			}
			if (type != null && type2 != null)
			{
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(base.CreatedType, typeof(KeyValuePair<, >).MakeGenericType(new Type[] { type, type2 }), typeof(IDictionary<, >).MakeGenericType(new Type[] { type, type2 }));
				if (!this.HasParameterizedCreatorInternal && this.NonNullableUnderlyingType.Name == "FSharpMap`2")
				{
					FSharpUtils.EnsureInitialized(this.NonNullableUnderlyingType.Assembly());
					this._parameterizedCreator = FSharpUtils.Instance.CreateMap(type, type2);
				}
			}
			if (!typeof(IDictionary).IsAssignableFrom(base.CreatedType))
			{
				this.ShouldCreateWrapper = true;
			}
			this.DictionaryKeyType = type;
			this.DictionaryValueType = type2;
			Type type3;
			ObjectConstructor<object> objectConstructor;
			if (this.DictionaryKeyType != null && this.DictionaryValueType != null && ImmutableCollectionsUtils.TryBuildImmutableForDictionaryContract(this.NonNullableUnderlyingType, this.DictionaryKeyType, this.DictionaryValueType, out type3, out objectConstructor))
			{
				base.CreatedType = type3;
				this._parameterizedCreator = objectConstructor;
				this.IsReadOnlyOrFixedSize = true;
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001C5E0 File Offset: 0x0001A7E0
		[NullableContext(1)]
		internal IWrappedDictionary CreateWrapper(object dictionary)
		{
			if (this._genericWrapperCreator == null)
			{
				this._genericWrapperType = typeof(DictionaryWrapper<, >).MakeGenericType(new Type[] { this.DictionaryKeyType, this.DictionaryValueType });
				ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[] { this._genericCollectionDefinitionType });
				this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			}
			return (IWrappedDictionary)this._genericWrapperCreator(new object[] { dictionary });
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001C668 File Offset: 0x0001A868
		[NullableContext(1)]
		internal IDictionary CreateTemporaryDictionary()
		{
			if (this._genericTemporaryDictionaryCreator == null)
			{
				Type type = typeof(Dictionary<, >).MakeGenericType(new Type[]
				{
					this.DictionaryKeyType ?? typeof(object),
					this.DictionaryValueType ?? typeof(object)
				});
				this._genericTemporaryDictionaryCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type);
			}
			return (IDictionary)this._genericTemporaryDictionaryCreator();
		}

		// Token: 0x0400027E RID: 638
		private readonly Type _genericCollectionDefinitionType;

		// Token: 0x0400027F RID: 639
		private Type _genericWrapperType;

		// Token: 0x04000280 RID: 640
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _genericWrapperCreator;

		// Token: 0x04000281 RID: 641
		[Nullable(new byte[] { 2, 1 })]
		private Func<object> _genericTemporaryDictionaryCreator;

		// Token: 0x04000283 RID: 643
		private readonly ConstructorInfo _parameterizedConstructor;

		// Token: 0x04000284 RID: 644
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _overrideCreator;

		// Token: 0x04000285 RID: 645
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _parameterizedCreator;
	}
}
