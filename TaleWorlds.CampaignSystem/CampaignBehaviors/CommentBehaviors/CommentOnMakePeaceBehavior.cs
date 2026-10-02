using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000485 RID: 1157
	public class CommentOnMakePeaceBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004AEE RID: 19182 RVA: 0x00179E1D File Offset: 0x0017801D
		public override void RegisterEvents()
		{
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnMakePeace));
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x00179E36 File Offset: 0x00178036
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004AF0 RID: 19184 RVA: 0x00179E38 File Offset: 0x00178038
		private void OnMakePeace(IFaction faction1, IFaction faction2, MakePeaceAction.MakePeaceDetail detail)
		{
			MakePeaceLogEntry makePeaceLogEntry = new MakePeaceLogEntry(faction1, faction2);
			LogEntry.AddLogEntry(makePeaceLogEntry);
			if (faction2 == Hero.MainHero.MapFaction || (faction1 == Hero.MainHero.MapFaction && detail != MakePeaceAction.MakePeaceDetail.ByKingdomDecision))
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new PeaceMapNotification(faction1, faction2, makePeaceLogEntry.GetEncyclopediaText()));
			}
		}
	}
}
