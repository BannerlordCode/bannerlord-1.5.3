using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200009A RID: 154
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class NamingStrategy
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x000234C0 File Offset: 0x000216C0
		// (set) Token: 0x06000810 RID: 2064 RVA: 0x000234C8 File Offset: 0x000216C8
		public bool ProcessDictionaryKeys { get; set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x000234D1 File Offset: 0x000216D1
		// (set) Token: 0x06000812 RID: 2066 RVA: 0x000234D9 File Offset: 0x000216D9
		public bool ProcessExtensionDataNames { get; set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x000234E2 File Offset: 0x000216E2
		// (set) Token: 0x06000814 RID: 2068 RVA: 0x000234EA File Offset: 0x000216EA
		public bool OverrideSpecifiedNames { get; set; }

		// Token: 0x06000815 RID: 2069 RVA: 0x000234F3 File Offset: 0x000216F3
		public virtual string GetPropertyName(string name, bool hasSpecifiedName)
		{
			if (hasSpecifiedName && !this.OverrideSpecifiedNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00023509 File Offset: 0x00021709
		public virtual string GetExtensionDataName(string name)
		{
			if (!this.ProcessExtensionDataNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0002351C File Offset: 0x0002171C
		public virtual string GetDictionaryKey(string key)
		{
			if (!this.ProcessDictionaryKeys)
			{
				return key;
			}
			return this.ResolvePropertyName(key);
		}

		// Token: 0x06000818 RID: 2072
		protected abstract string ResolvePropertyName(string name);

		// Token: 0x06000819 RID: 2073 RVA: 0x00023530 File Offset: 0x00021730
		public override int GetHashCode()
		{
			return (((((base.GetType().GetHashCode() * 397) ^ this.ProcessDictionaryKeys.GetHashCode()) * 397) ^ this.ProcessExtensionDataNames.GetHashCode()) * 397) ^ this.OverrideSpecifiedNames.GetHashCode();
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00023587 File Offset: 0x00021787
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as NamingStrategy);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00023598 File Offset: 0x00021798
		[NullableContext(2)]
		protected bool Equals(NamingStrategy other)
		{
			return other != null && (base.GetType() == other.GetType() && this.ProcessDictionaryKeys == other.ProcessDictionaryKeys && this.ProcessExtensionDataNames == other.ProcessExtensionDataNames) && this.OverrideSpecifiedNames == other.OverrideSpecifiedNames;
		}
	}
}
