using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000090 RID: 144
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonProperty
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0001CD5C File Offset: 0x0001AF5C
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0001CD64 File Offset: 0x0001AF64
		internal JsonContract PropertyContract { get; set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0001CD6D File Offset: 0x0001AF6D
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x0001CD75 File Offset: 0x0001AF75
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
			set
			{
				this._propertyName = value;
				this._skipPropertyNameEscape = !JavaScriptUtils.ShouldEscapeJavaScriptString(this._propertyName, JavaScriptUtils.HtmlCharEscapeFlags);
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0001CD97 File Offset: 0x0001AF97
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x0001CD9F File Offset: 0x0001AF9F
		public Type DeclaringType { get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0001CDA8 File Offset: 0x0001AFA8
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0001CDB0 File Offset: 0x0001AFB0
		public int? Order { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0001CDB9 File Offset: 0x0001AFB9
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0001CDC1 File Offset: 0x0001AFC1
		public string UnderlyingName { get; set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0001CDCA File Offset: 0x0001AFCA
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0001CDD2 File Offset: 0x0001AFD2
		public IValueProvider ValueProvider { get; set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0001CDDB File Offset: 0x0001AFDB
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x0001CDE3 File Offset: 0x0001AFE3
		public IAttributeProvider AttributeProvider { get; set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0001CDEC File Offset: 0x0001AFEC
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x0001CDF4 File Offset: 0x0001AFF4
		public Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
			set
			{
				if (this._propertyType != value)
				{
					this._propertyType = value;
					this._hasGeneratedDefaultValue = false;
				}
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0001CE12 File Offset: 0x0001B012
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x0001CE1A File Offset: 0x0001B01A
		public JsonConverter Converter { get; set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0001CE23 File Offset: 0x0001B023
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x0001CE2B File Offset: 0x0001B02B
		[Obsolete("MemberConverter is obsolete. Use Converter instead.")]
		public JsonConverter MemberConverter
		{
			get
			{
				return this.Converter;
			}
			set
			{
				this.Converter = value;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x0001CE34 File Offset: 0x0001B034
		// (set) Token: 0x0600071C RID: 1820 RVA: 0x0001CE3C File Offset: 0x0001B03C
		public bool Ignored { get; set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x0001CE45 File Offset: 0x0001B045
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x0001CE4D File Offset: 0x0001B04D
		public bool Readable { get; set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0001CE56 File Offset: 0x0001B056
		// (set) Token: 0x06000720 RID: 1824 RVA: 0x0001CE5E File Offset: 0x0001B05E
		public bool Writable { get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x0001CE67 File Offset: 0x0001B067
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x0001CE6F File Offset: 0x0001B06F
		public bool HasMemberAttribute { get; set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x0001CE78 File Offset: 0x0001B078
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x0001CE8A File Offset: 0x0001B08A
		public object DefaultValue
		{
			get
			{
				if (!this._hasExplicitDefaultValue)
				{
					return null;
				}
				return this._defaultValue;
			}
			set
			{
				this._hasExplicitDefaultValue = true;
				this._defaultValue = value;
			}
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001CE9A File Offset: 0x0001B09A
		internal object GetResolvedDefaultValue()
		{
			if (this._propertyType == null)
			{
				return null;
			}
			if (!this._hasExplicitDefaultValue && !this._hasGeneratedDefaultValue)
			{
				this._defaultValue = ReflectionUtils.GetDefaultValue(this._propertyType);
				this._hasGeneratedDefaultValue = true;
			}
			return this._defaultValue;
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0001CEDA File Offset: 0x0001B0DA
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x0001CEE7 File Offset: 0x0001B0E7
		public Required Required
		{
			get
			{
				return this._required.GetValueOrDefault();
			}
			set
			{
				this._required = new Required?(value);
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0001CEF5 File Offset: 0x0001B0F5
		public bool IsRequiredSpecified
		{
			get
			{
				return this._required != null;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x0001CF02 File Offset: 0x0001B102
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x0001CF0A File Offset: 0x0001B10A
		public bool? IsReference { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x0001CF13 File Offset: 0x0001B113
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x0001CF1B File Offset: 0x0001B11B
		public NullValueHandling? NullValueHandling { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0001CF24 File Offset: 0x0001B124
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x0001CF2C File Offset: 0x0001B12C
		public DefaultValueHandling? DefaultValueHandling { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0001CF35 File Offset: 0x0001B135
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x0001CF3D File Offset: 0x0001B13D
		public ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0001CF46 File Offset: 0x0001B146
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x0001CF4E File Offset: 0x0001B14E
		public ObjectCreationHandling? ObjectCreationHandling { get; set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x0001CF57 File Offset: 0x0001B157
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x0001CF5F File Offset: 0x0001B15F
		public TypeNameHandling? TypeNameHandling { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x0001CF68 File Offset: 0x0001B168
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x0001CF70 File Offset: 0x0001B170
		[Nullable(new byte[] { 2, 1 })]
		public Predicate<object> ShouldSerialize
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0001CF79 File Offset: 0x0001B179
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x0001CF81 File Offset: 0x0001B181
		[Nullable(new byte[] { 2, 1 })]
		public Predicate<object> ShouldDeserialize
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x0001CF8A File Offset: 0x0001B18A
		// (set) Token: 0x0600073A RID: 1850 RVA: 0x0001CF92 File Offset: 0x0001B192
		[Nullable(new byte[] { 2, 1 })]
		public Predicate<object> GetIsSpecified
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x0001CF9B File Offset: 0x0001B19B
		// (set) Token: 0x0600073C RID: 1852 RVA: 0x0001CFA3 File Offset: 0x0001B1A3
		[Nullable(new byte[] { 2, 1, 2 })]
		public Action<object, object> SetIsSpecified
		{
			[return: Nullable(new byte[] { 2, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 2 })]
			set;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001CFAC File Offset: 0x0001B1AC
		[NullableContext(1)]
		public override string ToString()
		{
			return this.PropertyName ?? string.Empty;
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0001CFBD File Offset: 0x0001B1BD
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x0001CFC5 File Offset: 0x0001B1C5
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x0001CFCE File Offset: 0x0001B1CE
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x0001CFD6 File Offset: 0x0001B1D6
		public bool? ItemIsReference { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0001CFDF File Offset: 0x0001B1DF
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x0001CFE7 File Offset: 0x0001B1E7
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0001CFF0 File Offset: 0x0001B1F0
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x0001CFF8 File Offset: 0x0001B1F8
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x06000746 RID: 1862 RVA: 0x0001D004 File Offset: 0x0001B204
		[NullableContext(1)]
		internal void WritePropertyName(JsonWriter writer)
		{
			string propertyName = this.PropertyName;
			if (this._skipPropertyNameEscape)
			{
				writer.WritePropertyName(propertyName, false);
				return;
			}
			writer.WritePropertyName(propertyName);
		}

		// Token: 0x0400029F RID: 671
		internal Required? _required;

		// Token: 0x040002A0 RID: 672
		internal bool _hasExplicitDefaultValue;

		// Token: 0x040002A1 RID: 673
		private object _defaultValue;

		// Token: 0x040002A2 RID: 674
		private bool _hasGeneratedDefaultValue;

		// Token: 0x040002A3 RID: 675
		private string _propertyName;

		// Token: 0x040002A4 RID: 676
		internal bool _skipPropertyNameEscape;

		// Token: 0x040002A5 RID: 677
		private Type _propertyType;
	}
}
