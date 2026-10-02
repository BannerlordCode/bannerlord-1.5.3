using System;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000184 RID: 388
	public abstract class EncyclopediaModelBase : Attribute
	{
		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06001C2C RID: 7212 RVA: 0x00091175 File Offset: 0x0008F375
		// (set) Token: 0x06001C2D RID: 7213 RVA: 0x0009117D File Offset: 0x0008F37D
		public Type[] PageTargetTypes { get; private set; }

		// Token: 0x06001C2E RID: 7214 RVA: 0x00091186 File Offset: 0x0008F386
		public EncyclopediaModelBase(Type[] pageTargetTypes)
		{
			this.PageTargetTypes = pageTargetTypes;
		}
	}
}
