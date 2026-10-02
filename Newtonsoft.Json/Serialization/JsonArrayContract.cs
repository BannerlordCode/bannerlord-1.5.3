using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000081 RID: 129
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonArrayContract : JsonContainerContract
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x0001B6C8 File Offset: 0x000198C8
		public Type CollectionItemType { get; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0001B6D0 File Offset: 0x000198D0
		public bool IsMultidimensionalArray { get; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x0001B6D8 File Offset: 0x000198D8
		internal bool IsArray { get; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x0001B6E0 File Offset: 0x000198E0
		internal bool ShouldCreateWrapper { get; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x0001B6E8 File Offset: 0x000198E8
		// (set) Token: 0x0600067A RID: 1658 RVA: 0x0001B6F0 File Offset: 0x000198F0
		internal bool CanDeserialize { get; private set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0001B6F9 File Offset: 0x000198F9
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

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x0001B72D File Offset: 0x0001992D
		// (set) Token: 0x0600067D RID: 1661 RVA: 0x0001B735 File Offset: 0x00019935
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
				this.CanDeserialize = true;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0001B745 File Offset: 0x00019945
		// (set) Token: 0x0600067F RID: 1663 RVA: 0x0001B74D File Offset: 0x0001994D
		public bool HasParameterizedCreator { get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0001B756 File Offset: 0x00019956
		internal bool HasParameterizedCreatorInternal
		{
			get
			{
				return this.HasParameterizedCreator || this._parameterizedCreator != null || this._parameterizedConstructor != null;
			}
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001B778 File Offset: 0x00019978
		[NullableContext(1)]
		public JsonArrayContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Array;
			this.IsArray = base.CreatedType.IsArray || (this.NonNullableUnderlyingType.IsGenericType() && this.NonNullableUnderlyingType.GetGenericTypeDefinition().FullName == "System.Linq.EmptyPartition`1");
			bool flag;
			Type type;
			if (this.IsArray)
			{
				this.CollectionItemType = ReflectionUtils.GetCollectionItemType(base.UnderlyingType);
				this.IsReadOnlyOrFixedSize = true;
				this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
				flag = true;
				this.IsMultidimensionalArray = base.CreatedType.IsArray && base.UnderlyingType.GetArrayRank() > 1;
			}
			else if (typeof(IList).IsAssignableFrom(this.NonNullableUnderlyingType))
			{
				if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof(ICollection<>), out this._genericCollectionDefinitionType))
				{
					this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				}
				else
				{
					this.CollectionItemType = ReflectionUtils.GetCollectionItemType(this.NonNullableUnderlyingType);
				}
				if (this.NonNullableUnderlyingType == typeof(IList))
				{
					base.CreatedType = typeof(List<object>);
				}
				if (this.CollectionItemType != null)
				{
					this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.NonNullableUnderlyingType, this.CollectionItemType);
				}
				this.IsReadOnlyOrFixedSize = ReflectionUtils.InheritsGenericDefinition(this.NonNullableUnderlyingType, typeof(ReadOnlyCollection<>));
				flag = true;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof(ICollection<>), out this._genericCollectionDefinitionType))
			{
				this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(ICollection<>)) || ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(IList<>)))
				{
					base.CreatedType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				if (ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(ISet<>)))
				{
					base.CreatedType = typeof(HashSet<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.NonNullableUnderlyingType, this.CollectionItemType);
				flag = true;
				this.ShouldCreateWrapper = true;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof(IReadOnlyCollection<>), out type))
			{
				this.CollectionItemType = type.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(IReadOnlyCollection<>)) || ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(IReadOnlyList<>)))
				{
					base.CreatedType = typeof(ReadOnlyCollection<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(base.CreatedType, this.CollectionItemType);
				this.StoreFSharpListCreatorIfNecessary(this.NonNullableUnderlyingType);
				this.IsReadOnlyOrFixedSize = true;
				flag = this.HasParameterizedCreatorInternal;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof(IEnumerable<>), out type))
			{
				this.CollectionItemType = type.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(base.UnderlyingType, typeof(IEnumerable<>)))
				{
					base.CreatedType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.NonNullableUnderlyingType, this.CollectionItemType);
				this.StoreFSharpListCreatorIfNecessary(this.NonNullableUnderlyingType);
				if (this.NonNullableUnderlyingType.IsGenericType() && this.NonNullableUnderlyingType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					this._genericCollectionDefinitionType = type;
					this.IsReadOnlyOrFixedSize = false;
					this.ShouldCreateWrapper = false;
					flag = true;
				}
				else
				{
					this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
					this.IsReadOnlyOrFixedSize = true;
					this.ShouldCreateWrapper = true;
					flag = this.HasParameterizedCreatorInternal;
				}
			}
			else
			{
				flag = false;
				this.ShouldCreateWrapper = true;
			}
			this.CanDeserialize = flag;
			Type type2;
			ObjectConstructor<object> objectConstructor;
			if (this.CollectionItemType != null && ImmutableCollectionsUtils.TryBuildImmutableForArrayContract(this.NonNullableUnderlyingType, this.CollectionItemType, out type2, out objectConstructor))
			{
				base.CreatedType = type2;
				this._parameterizedCreator = objectConstructor;
				this.IsReadOnlyOrFixedSize = true;
				this.CanDeserialize = true;
			}
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001BC20 File Offset: 0x00019E20
		[NullableContext(1)]
		internal IWrappedCollection CreateWrapper(object list)
		{
			if (this._genericWrapperCreator == null)
			{
				this._genericWrapperType = typeof(CollectionWrapper<>).MakeGenericType(new Type[] { this.CollectionItemType });
				Type type;
				if (ReflectionUtils.InheritsGenericDefinition(this._genericCollectionDefinitionType, typeof(List<>)) || this._genericCollectionDefinitionType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					type = typeof(ICollection<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				else
				{
					type = this._genericCollectionDefinitionType;
				}
				ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[] { type });
				this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			}
			return (IWrappedCollection)this._genericWrapperCreator(new object[] { list });
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001BCF8 File Offset: 0x00019EF8
		[NullableContext(1)]
		internal IList CreateTemporaryCollection()
		{
			if (this._genericTemporaryCollectionCreator == null)
			{
				Type type = ((this.IsMultidimensionalArray || this.CollectionItemType == null) ? typeof(object) : this.CollectionItemType);
				Type type2 = typeof(List<>).MakeGenericType(new Type[] { type });
				this._genericTemporaryCollectionCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type2);
			}
			return (IList)this._genericTemporaryCollectionCreator();
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001BD71 File Offset: 0x00019F71
		[NullableContext(1)]
		private void StoreFSharpListCreatorIfNecessary(Type underlyingType)
		{
			if (!this.HasParameterizedCreatorInternal && underlyingType.Name == "FSharpList`1")
			{
				FSharpUtils.EnsureInitialized(underlyingType.Assembly());
				this._parameterizedCreator = FSharpUtils.Instance.CreateSeq(this.CollectionItemType);
			}
		}

		// Token: 0x0400024A RID: 586
		private readonly Type _genericCollectionDefinitionType;

		// Token: 0x0400024B RID: 587
		private Type _genericWrapperType;

		// Token: 0x0400024C RID: 588
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _genericWrapperCreator;

		// Token: 0x0400024D RID: 589
		[Nullable(new byte[] { 2, 1 })]
		private Func<object> _genericTemporaryCollectionCreator;

		// Token: 0x04000251 RID: 593
		private readonly ConstructorInfo _parameterizedConstructor;

		// Token: 0x04000252 RID: 594
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _parameterizedCreator;

		// Token: 0x04000253 RID: 595
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _overrideCreator;
	}
}
