using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x0200047A RID: 1146
	public class CommentOnChangeSettlementOwnerBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004AC1 RID: 19137 RVA: 0x00179962 File Offset: 0x00177B62
		public override void RegisterEvents()
		{
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
		}

		// Token: 0x06004AC2 RID: 19138 RVA: 0x0017997B File Offset: 0x00177B7B
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004AC3 RID: 19139 RVA: 0x00179980 File Offset: 0x00177B80
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero previousOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			ChangeSettlementOwnerLogEntry changeSettlementOwnerLogEntry = new ChangeSettlementOwnerLogEntry(settlement, newOwner, previousOwner, false);
			LogEntry.AddLogEntry(changeSettlementOwnerLogEntry);
			if (newOwner != null && newOwner.IsHumanPlayerCharacter)
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new SettlementOwnerChangedMapNotification(settlement, newOwner, previousOwner, changeSettlementOwnerLogEntry.GetEncyclopediaText()));
			}
		}
	}
}
