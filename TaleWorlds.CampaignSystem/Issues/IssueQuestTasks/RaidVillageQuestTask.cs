using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues.IssueQuestTasks
{
	// Token: 0x0200039C RID: 924
	public class RaidVillageQuestTask : QuestTaskBase
	{
		// Token: 0x06003668 RID: 13928 RVA: 0x000DEC26 File Offset: 0x000DCE26
		public RaidVillageQuestTask(Village village, Action onSucceededAction, Action onFailedAction, Action onCanceledAction, DialogFlow dialogFlow = null)
			: base(dialogFlow, onSucceededAction, onFailedAction, onCanceledAction)
		{
			this._targetVillage = village;
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x000DEC3B File Offset: 0x000DCE3B
		public void OnVillageLooted(Village village)
		{
			if (this._targetVillage == village)
			{
				base.Finish((this._targetVillage.Owner.MapEvent.AttackerSide.LeaderParty == MobileParty.MainParty.Party) ? QuestTaskBase.FinishStates.Success : QuestTaskBase.FinishStates.Fail);
			}
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x000DEC76 File Offset: 0x000DCE76
		public void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			if (!FactionManager.IsAtWarAgainstFaction(newKingdom, this._targetVillage.Settlement.MapFaction))
			{
				base.Finish(QuestTaskBase.FinishStates.Cancel);
			}
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x000DEC97 File Offset: 0x000DCE97
		public override void SetReferences()
		{
			CampaignEvents.VillageLooted.AddNonSerializedListener(this, new Action<Village>(this.OnVillageLooted));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
		}

		// Token: 0x04000F4F RID: 3919
		[SaveableField(50)]
		private readonly Village _targetVillage;
	}
}
