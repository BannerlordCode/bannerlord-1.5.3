using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000097 RID: 151
	[NullableContext(1)]
	[Nullable(0)]
	internal static class JsonTypeReflector
	{
		// Token: 0x060007F0 RID: 2032 RVA: 0x00022DE1 File Offset: 0x00020FE1
		[return: Nullable(2)]
		public static T GetCachedAttribute<[Nullable(0)] T>(object attributeProvider) where T : Attribute
		{
			return CachedAttributeGetter<T>.GetAttribute(attributeProvider);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00022DEC File Offset: 0x00020FEC
		public static bool CanTypeDescriptorConvertString(Type type, out TypeConverter typeConverter)
		{
			typeConverter = TypeDescriptor.GetConverter(type);
			if (typeConverter != null)
			{
				Type type2 = typeConverter.GetType();
				if (!string.Equals(type2.FullName, "System.ComponentModel.ComponentConverter", StringComparison.Ordinal) && !string.Equals(type2.FullName, "System.ComponentModel.ReferenceConverter", StringComparison.Ordinal) && !string.Equals(type2.FullName, "System.Windows.Forms.Design.DataSourceConverter", StringComparison.Ordinal) && type2 != typeof(TypeConverter))
				{
					return typeConverter.CanConvertTo(typeof(string));
				}
			}
			return false;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00022E6C File Offset: 0x0002106C
		[return: Nullable(2)]
		public static DataContractAttribute GetDataContractAttribute(Type type)
		{
			Type type2 = type;
			while (type2 != null)
			{
				DataContractAttribute attribute = CachedAttributeGetter<DataContractAttribute>.GetAttribute(type2);
				if (attribute != null)
				{
					return attribute;
				}
				type2 = type2.BaseType();
			}
			return null;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00022E9C File Offset: 0x0002109C
		[return: Nullable(2)]
		public static DataMemberAttribute GetDataMemberAttribute(MemberInfo memberInfo)
		{
			if (memberInfo.MemberType() == MemberTypes.Field)
			{
				return CachedAttributeGetter<DataMemberAttribute>.GetAttribute(memberInfo);
			}
			PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
			DataMemberAttribute dataMemberAttribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(propertyInfo);
			if (dataMemberAttribute == null && propertyInfo.IsVirtual())
			{
				Type type = propertyInfo.DeclaringType;
				while (dataMemberAttribute == null && type != null)
				{
					PropertyInfo propertyInfo2 = (PropertyInfo)ReflectionUtils.GetMemberInfoFromType(type, propertyInfo);
					if (propertyInfo2 != null && propertyInfo2.IsVirtual())
					{
						dataMemberAttribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(propertyInfo2);
					}
					type = type.BaseType();
				}
			}
			return dataMemberAttribute;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00022F14 File Offset: 0x00021114
		public static MemberSerialization GetObjectMemberSerialization(Type objectType, bool ignoreSerializableAttribute)
		{
			JsonObjectAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonObjectAttribute>(objectType);
			if (cachedAttribute != null)
			{
				return cachedAttribute.MemberSerialization;
			}
			if (JsonTypeReflector.GetDataContractAttribute(objectType) != null)
			{
				return MemberSerialization.OptIn;
			}
			if (!ignoreSerializableAttribute && JsonTypeReflector.IsSerializable(objectType))
			{
				return MemberSerialization.Fields;
			}
			return MemberSerialization.OptOut;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00022F4C File Offset: 0x0002114C
		[return: Nullable(2)]
		public static JsonConverter GetJsonConverter(object attributeProvider)
		{
			JsonConverterAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonConverterAttribute>(attributeProvider);
			if (cachedAttribute != null)
			{
				Func<object[], object> func = JsonTypeReflector.CreatorCache.Get(cachedAttribute.ConverterType);
				if (func != null)
				{
					return (JsonConverter)func(cachedAttribute.ConverterParameters);
				}
			}
			return null;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00022F8A File Offset: 0x0002118A
		public static JsonConverter CreateJsonConverterInstance(Type converterType, [Nullable(new byte[] { 2, 1 })] object[] args)
		{
			return (JsonConverter)JsonTypeReflector.CreatorCache.Get(converterType)(args);
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00022FA2 File Offset: 0x000211A2
		public static NamingStrategy CreateNamingStrategyInstance(Type namingStrategyType, [Nullable(new byte[] { 2, 1 })] object[] args)
		{
			return (NamingStrategy)JsonTypeReflector.CreatorCache.Get(namingStrategyType)(args);
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00022FBA File Offset: 0x000211BA
		[return: Nullable(2)]
		public static NamingStrategy GetContainerNamingStrategy(JsonContainerAttribute containerAttribute)
		{
			if (containerAttribute.NamingStrategyInstance == null)
			{
				if (containerAttribute.NamingStrategyType == null)
				{
					return null;
				}
				containerAttribute.NamingStrategyInstance = JsonTypeReflector.CreateNamingStrategyInstance(containerAttribute.NamingStrategyType, containerAttribute.NamingStrategyParameters);
			}
			return containerAttribute.NamingStrategyInstance;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00022FF4 File Offset: 0x000211F4
		[return: Nullable(new byte[] { 1, 2, 1, 1 })]
		private static Func<object[], object> GetCreator(Type type)
		{
			Func<object> defaultConstructor = (ReflectionUtils.HasDefaultConstructor(type, false) ? JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type) : null);
			return delegate([Nullable(new byte[] { 2, 1 })] object[] parameters)
			{
				object obj;
				try
				{
					if (parameters != null)
					{
						Type[] array = parameters.Select<object, Type>(delegate(object param)
						{
							if (param == null)
							{
								throw new InvalidOperationException("Cannot pass a null parameter to the constructor.");
							}
							return param.GetType();
						}).ToArray<Type>();
						ConstructorInfo constructor = type.GetConstructor(array);
						if (!(constructor != null))
						{
							throw new JsonException("No matching parameterized constructor found for '{0}'.".FormatWith(CultureInfo.InvariantCulture, type));
						}
						obj = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor)(parameters);
					}
					else
					{
						if (defaultConstructor == null)
						{
							throw new JsonException("No parameterless constructor defined for '{0}'.".FormatWith(CultureInfo.InvariantCulture, type));
						}
						obj = defaultConstructor();
					}
				}
				catch (Exception ex)
				{
					throw new JsonException("Error creating '{0}'.".FormatWith(CultureInfo.InvariantCulture, type), ex);
				}
				return obj;
			};
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00023041 File Offset: 0x00021241
		[return: Nullable(2)]
		private static Type GetAssociatedMetadataType(Type type)
		{
			return JsonTypeReflector.AssociatedMetadataTypesCache.Get(type);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00023050 File Offset: 0x00021250
		[return: Nullable(2)]
		private static Type GetAssociateMetadataTypeFromAttribute(Type type)
		{
			foreach (Attribute attribute in ReflectionUtils.GetAttributes(type, null, true))
			{
				Type type2 = attribute.GetType();
				if (string.Equals(type2.FullName, "System.ComponentModel.DataAnnotations.MetadataTypeAttribute", StringComparison.Ordinal))
				{
					if (JsonTypeReflector._metadataTypeAttributeReflectionObject == null)
					{
						JsonTypeReflector._metadataTypeAttributeReflectionObject = ReflectionObject.Create(type2, new string[] { "MetadataClassType" });
					}
					return (Type)JsonTypeReflector._metadataTypeAttributeReflectionObject.GetValue(attribute, "MetadataClassType");
				}
			}
			return null;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x000230CC File Offset: 0x000212CC
		[return: Nullable(2)]
		private static T GetAttribute<[Nullable(0)] T>(Type type) where T : Attribute
		{
			Type associatedMetadataType = JsonTypeReflector.GetAssociatedMetadataType(type);
			T t;
			if (associatedMetadataType != null)
			{
				t = ReflectionUtils.GetAttribute<T>(associatedMetadataType, true);
				if (t != null)
				{
					return t;
				}
			}
			t = ReflectionUtils.GetAttribute<T>(type, true);
			if (t != null)
			{
				return t;
			}
			Type[] interfaces = type.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				t = ReflectionUtils.GetAttribute<T>(interfaces[i], true);
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00023140 File Offset: 0x00021340
		[return: Nullable(2)]
		private static T GetAttribute<[Nullable(0)] T>(MemberInfo memberInfo) where T : Attribute
		{
			Type associatedMetadataType = JsonTypeReflector.GetAssociatedMetadataType(memberInfo.DeclaringType);
			T t;
			if (associatedMetadataType != null)
			{
				MemberInfo memberInfoFromType = ReflectionUtils.GetMemberInfoFromType(associatedMetadataType, memberInfo);
				if (memberInfoFromType != null)
				{
					t = ReflectionUtils.GetAttribute<T>(memberInfoFromType, true);
					if (t != null)
					{
						return t;
					}
				}
			}
			t = ReflectionUtils.GetAttribute<T>(memberInfo, true);
			if (t != null)
			{
				return t;
			}
			if (memberInfo.DeclaringType != null)
			{
				Type[] interfaces = memberInfo.DeclaringType.GetInterfaces();
				for (int i = 0; i < interfaces.Length; i++)
				{
					MemberInfo memberInfoFromType2 = ReflectionUtils.GetMemberInfoFromType(interfaces[i], memberInfo);
					if (memberInfoFromType2 != null)
					{
						t = ReflectionUtils.GetAttribute<T>(memberInfoFromType2, true);
						if (t != null)
						{
							return t;
						}
					}
				}
			}
			return default(T);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x000231F6 File Offset: 0x000213F6
		public static bool IsNonSerializable(object provider)
		{
			return ReflectionUtils.GetAttribute<NonSerializedAttribute>(provider, false) != null;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00023202 File Offset: 0x00021402
		public static bool IsSerializable(object provider)
		{
			return ReflectionUtils.GetAttribute<SerializableAttribute>(provider, false) != null;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00023210 File Offset: 0x00021410
		[return: Nullable(2)]
		public static T GetAttribute<[Nullable(0)] T>(object provider) where T : Attribute
		{
			Type type = provider as Type;
			if (type != null)
			{
				return JsonTypeReflector.GetAttribute<T>(type);
			}
			MemberInfo memberInfo = provider as MemberInfo;
			if (memberInfo != null)
			{
				return JsonTypeReflector.GetAttribute<T>(memberInfo);
			}
			return ReflectionUtils.GetAttribute<T>(provider, true);
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x00023246 File Offset: 0x00021446
		public static bool DynamicCodeGeneration
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonTypeReflector._dynamicCodeGeneration == null)
				{
					JsonTypeReflector._dynamicCodeGeneration = new bool?(false);
				}
				return JsonTypeReflector._dynamicCodeGeneration.GetValueOrDefault();
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x0002326C File Offset: 0x0002146C
		public static bool FullyTrusted
		{
			get
			{
				if (JsonTypeReflector._fullyTrusted == null)
				{
					AppDomain currentDomain = AppDomain.CurrentDomain;
					JsonTypeReflector._fullyTrusted = new bool?(currentDomain.IsHomogenous && currentDomain.IsFullyTrusted);
				}
				return JsonTypeReflector._fullyTrusted.GetValueOrDefault();
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x000232B0 File Offset: 0x000214B0
		public static ReflectionDelegateFactory ReflectionDelegateFactory
		{
			get
			{
				return ExpressionReflectionDelegateFactory.Instance;
			}
		}

		// Token: 0x040002CC RID: 716
		private static bool? _dynamicCodeGeneration;

		// Token: 0x040002CD RID: 717
		private static bool? _fullyTrusted;

		// Token: 0x040002CE RID: 718
		public const string IdPropertyName = "$id";

		// Token: 0x040002CF RID: 719
		public const string RefPropertyName = "$ref";

		// Token: 0x040002D0 RID: 720
		public const string TypePropertyName = "$type";

		// Token: 0x040002D1 RID: 721
		public const string ValuePropertyName = "$value";

		// Token: 0x040002D2 RID: 722
		public const string ArrayValuesPropertyName = "$values";

		// Token: 0x040002D3 RID: 723
		public const string ShouldSerializePrefix = "ShouldSerialize";

		// Token: 0x040002D4 RID: 724
		public const string SpecifiedPostfix = "Specified";

		// Token: 0x040002D5 RID: 725
		public const string ConcurrentDictionaryTypeName = "System.Collections.Concurrent.ConcurrentDictionary`2";

		// Token: 0x040002D6 RID: 726
		[Nullable(new byte[] { 1, 1, 1, 2, 1, 1 })]
		private static readonly ThreadSafeStore<Type, Func<object[], object>> CreatorCache = new ThreadSafeStore<Type, Func<object[], object>>(new Func<Type, Func<object[], object>>(JsonTypeReflector.GetCreator));

		// Token: 0x040002D7 RID: 727
		[Nullable(new byte[] { 1, 1, 2 })]
		private static readonly ThreadSafeStore<Type, Type> AssociatedMetadataTypesCache = new ThreadSafeStore<Type, Type>(new Func<Type, Type>(JsonTypeReflector.GetAssociateMetadataTypeFromAttribute));

		// Token: 0x040002D8 RID: 728
		[Nullable(2)]
		private static ReflectionObject _metadataTypeAttributeReflectionObject;
	}
}
