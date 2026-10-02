using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000E2 RID: 226
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class LocalizationRequiredAttribute : Attribute
	{
		// Token: 0x06000930 RID: 2352 RVA: 0x0000F9E6 File Offset: 0x0000DBE6
		public LocalizationRequiredAttribute(bool required)
		{
			this.Required = required;
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x0000F9F5 File Offset: 0x0000DBF5
		// (set) Token: 0x06000932 RID: 2354 RVA: 0x0000F9FD File Offset: 0x0000DBFD
		[UsedImplicitly]
		public bool Required { get; set; }

		// Token: 0x06000933 RID: 2355 RVA: 0x0000FA08 File Offset: 0x0000DC08
		public override bool Equals(object obj)
		{
			LocalizationRequiredAttribute localizationRequiredAttribute = obj as LocalizationRequiredAttribute;
			return localizationRequiredAttribute != null && localizationRequiredAttribute.Required == this.Required;
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0000FA2F File Offset: 0x0000DC2F
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
