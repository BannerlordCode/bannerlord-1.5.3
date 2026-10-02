using System;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000DD RID: 221
	public class EncyclopediaViewModel : Attribute
	{
		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x060014C9 RID: 5321 RVA: 0x00052DD6 File Offset: 0x00050FD6
		// (set) Token: 0x060014CA RID: 5322 RVA: 0x00052DDE File Offset: 0x00050FDE
		public Type PageTargetType { get; private set; }

		// Token: 0x060014CB RID: 5323 RVA: 0x00052DE7 File Offset: 0x00050FE7
		public EncyclopediaViewModel(Type pageTargetType)
		{
			this.PageTargetType = pageTargetType;
		}
	}
}
