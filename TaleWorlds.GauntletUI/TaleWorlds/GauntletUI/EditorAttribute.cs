using System;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200001F RID: 31
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class EditorAttribute : Attribute
	{
		// Token: 0x0600025B RID: 603 RVA: 0x0000C259 File Offset: 0x0000A459
		public EditorAttribute(bool includeInnerProperties = false)
		{
			this.IncludeInnerProperties = includeInnerProperties;
		}

		// Token: 0x04000137 RID: 311
		public readonly bool IncludeInnerProperties;
	}
}
