using System;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000C2 RID: 194
	public class MenuOverlay : Attribute
	{
		// Token: 0x060012C3 RID: 4803 RVA: 0x0004C26D File Offset: 0x0004A46D
		public MenuOverlay(string typeId)
		{
			this.TypeId = typeId;
		}

		// Token: 0x04000882 RID: 2178
		public new string TypeId;
	}
}
