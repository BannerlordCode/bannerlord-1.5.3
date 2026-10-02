using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x0200047F RID: 1151
	public class CommentOnDeclareWarBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004AD6 RID: 19158 RVA: 0x00179C3D File Offset: 0x00177E3D
		public override void RegisterEvents()
		{
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x00179C56 File Offset: 0x00177E56
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x00179C58 File Offset: 0x00177E58
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
		{
			DeclareWarLogEntry declareWarLogEntry = new DeclareWarLogEntry(faction1, faction2);
			LogEntry.AddLogEntry(declareWarLogEntry);
			if (detail != DeclareWarAction.DeclareWarDetail.CausedByQuest && (faction2 == Hero.MainHero.MapFaction || (faction1 == Hero.MainHero.MapFaction && detail != DeclareWarAction.DeclareWarDetail.CausedByKingdomDecision)))
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new WarMapNotification(faction1, faction2, declareWarLogEntry.GetEncyclopediaText()));
			}
		}
	}
}
