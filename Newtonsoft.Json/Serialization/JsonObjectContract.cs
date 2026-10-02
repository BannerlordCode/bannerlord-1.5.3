using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008E RID: 142
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonObjectContract : JsonContainerContract
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0001C9BE File Offset: 0x0001ABBE
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x0001C9C6 File Offset: 0x0001ABC6
		public MemberSerialization MemberSerialization { get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0001C9CF File Offset: 0x0001ABCF
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x0001C9D7 File Offset: 0x0001ABD7
		public MissingMemberHandling? MissingMemberHandling { get; set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0001C9E0 File Offset: 0x0001ABE0
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x0001C9E8 File Offset: 0x0001ABE8
		public Required? ItemRequired { get; set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0001C9F1 File Offset: 0x0001ABF1
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x0001C9F9 File Offset: 0x0001ABF9
		public NullValueHandling? ItemNullValueHandling { get; set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x0001CA02 File Offset: 0x0001AC02
		[Nullable(1)]
		public JsonPropertyCollection Properties
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0001CA0A File Offset: 0x0001AC0A
		[Nullable(1)]
		public JsonPropertyCollection CreatorParameters
		{
			[NullableContext(1)]
			get
			{
				if (this._creatorParameters == null)
				{
					this._creatorParameters = new JsonPropertyCollection(base.UnderlyingType);
				}
				return this._creatorParameters;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0001CA2B File Offset: 0x0001AC2B
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x0001CA33 File Offset: 0x0001AC33
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

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0001CA3C File Offset: 0x0001AC3C
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x0001CA44 File Offset: 0x0001AC44
		[Nullable(new byte[] { 2, 1 })]
		internal ObjectConstructor<object> ParameterizedCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._parameterizedCreator;
			}
			[param: Nullable(new byte[] { 2, 1 })]
			set
			{
				this._parameterizedCreator = value;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0001CA4D File Offset: 0x0001AC4D
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x0001CA55 File Offset: 0x0001AC55
		public ExtensionDataSetter ExtensionDataSetter { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0001CA5E File Offset: 0x0001AC5E
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x0001CA66 File Offset: 0x0001AC66
		public ExtensionDataGetter ExtensionDataGetter { get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0001CA6F File Offset: 0x0001AC6F
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x0001CA77 File Offset: 0x0001AC77
		public Type ExtensionDataValueType
		{
			get
			{
				return this._extensionDataValueType;
			}
			set
			{
				this._extensionDataValueType = value;
				this.ExtensionDataIsJToken = value != null && typeof(JToken).IsAssignableFrom(value);
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0001CAA2 File Offset: 0x0001ACA2
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x0001CAAA File Offset: 0x0001ACAA
		[Nullable(new byte[] { 2, 1, 1 })]
		public Func<string, string> ExtensionDataNameResolver
		{
			[return: Nullable(new byte[] { 2, 1, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 1 })]
			set;
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0001CAB4 File Offset: 0x0001ACB4
		internal bool HasRequiredOrDefaultValueProperties
		{
			get
			{
				if (this._hasRequiredOrDefaultValueProperties == null)
				{
					this._hasRequiredOrDefaultValueProperties = new bool?(false);
					if (this.ItemRequired.GetValueOrDefault(Required.Default) != Required.Default)
					{
						this._hasRequiredOrDefaultValueProperties = new bool?(true);
					}
					else
					{
						foreach (JsonProperty jsonProperty in this.Properties)
						{
							if (jsonProperty.Required == Required.Default)
							{
								DefaultValueHandling? defaultValueHandling = jsonProperty.DefaultValueHandling & DefaultValueHandling.Populate;
								DefaultValueHandling defaultValueHandling2 = DefaultValueHandling.Populate;
								if (!((defaultValueHandling.GetValueOrDefault() == defaultValueHandling2) & (defaultValueHandling != null)))
								{
									continue;
								}
							}
							this._hasRequiredOrDefaultValueProperties = new bool?(true);
							break;
						}
					}
				}
				return this._hasRequiredOrDefaultValueProperties.GetValueOrDefault();
			}
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001CBA0 File Offset: 0x0001ADA0
		[NullableContext(1)]
		public JsonObjectContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Object;
			this.Properties = new JsonPropertyCollection(base.UnderlyingType);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001CBC1 File Offset: 0x0001ADC1
		[NullableContext(1)]
		[SecuritySafeCritical]
		internal object GetUninitializedObject()
		{
			if (!JsonTypeReflector.FullyTrusted)
			{
				throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith(CultureInfo.InvariantCulture, this.NonNullableUnderlyingType));
			}
			return FormatterServices.GetUninitializedObject(this.NonNullableUnderlyingType);
		}

		// Token: 0x04000297 RID: 663
		internal bool ExtensionDataIsJToken;

		// Token: 0x04000298 RID: 664
		private bool? _hasRequiredOrDefaultValueProperties;

		// Token: 0x04000299 RID: 665
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _overrideCreator;

		// Token: 0x0400029A RID: 666
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _parameterizedCreator;

		// Token: 0x0400029B RID: 667
		private JsonPropertyCollection _creatorParameters;

		// Token: 0x0400029C RID: 668
		private Type _extensionDataValueType;
	}
}
