using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000036 RID: 54
	public abstract class CampaignBehaviorBase : ICampaignBehavior
	{
		// Token: 0x0600037E RID: 894 RVA: 0x000185E2 File Offset: 0x000167E2
		public CampaignBehaviorBase(string stringId)
		{
			this.StringId = stringId;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000185F1 File Offset: 0x000167F1
		public CampaignBehaviorBase()
		{
			this.StringId = base.GetType().Name;
		}

		// Token: 0x06000380 RID: 896
		public abstract void RegisterEvents();

		// Token: 0x06000381 RID: 897 RVA: 0x0001860A File Offset: 0x0001680A
		public static T GetCampaignBehavior<T>()
		{
			return Campaign.Current.GetCampaignBehavior<T>();
		}

		// Token: 0x06000382 RID: 898
		public abstract void SyncData(IDataStore dataStore);

		// Token: 0x040000D3 RID: 211
		public readonly string StringId;
	}
}
