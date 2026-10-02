using System;
using System.Linq;
using Helpers;
using StoryMode.Quests.SecondPhase.ConspiracyQuests;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace StoryMode.GameComponents
{
	// Token: 0x02000041 RID: 65
	public class StoryModeEncounterGameMenuModel : EncounterGameMenuModel
	{
		// Token: 0x06000445 RID: 1093 RVA: 0x00019284 File Offset: 0x00017484
		public override string GetEncounterMenu(PartyBase attackerParty, PartyBase defenderParty, out bool startBattle, out bool joinBattle)
		{
			Settlement settlement = MapEventHelper.GetEncounteredPartyBase(attackerParty, defenderParty).Settlement;
			string text;
			if (settlement != null && settlement.SettlementComponent is TrainingField)
			{
				text = "training_field_menu";
				startBattle = false;
				joinBattle = false;
			}
			else if (StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted)
			{
				text = "storymode_game_menu_blocker";
				startBattle = false;
				joinBattle = false;
			}
			else if (StoryModeManager.Current.MainStoryLine.SecondPhase != null && (StoryModeManager.Current.MainStoryLine.SecondPhase.ConspiracyClan == attackerParty.MapFaction || StoryModeManager.Current.MainStoryLine.SecondPhase.ConspiracyClan == defenderParty.MapFaction))
			{
				QuestBase questBase = Campaign.Current.QuestManager.Quests.FirstOrDefault<QuestBase>((QuestBase q) => !q.IsFinalized && q.GetType() == typeof(DisruptSupplyLinesConspiracyQuest));
				if (questBase != null && ((DisruptSupplyLinesConspiracyQuest)questBase).ConspiracyCaravan == defenderParty.MobileParty)
				{
					text = base.BaseModel.GetEncounterMenu(attackerParty, defenderParty, out startBattle, out joinBattle);
				}
				else
				{
					text = "encounter";
					startBattle = true;
					joinBattle = true;
				}
			}
			else
			{
				text = base.BaseModel.GetEncounterMenu(attackerParty, defenderParty, out startBattle, out joinBattle);
			}
			return text;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000193AD File Offset: 0x000175AD
		public override string GetGenericStateMenu()
		{
			return base.BaseModel.GetGenericStateMenu();
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000193BA File Offset: 0x000175BA
		public override string GetNewPartyJoinMenu(MobileParty newParty)
		{
			return base.BaseModel.GetNewPartyJoinMenu(newParty);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x000193C8 File Offset: 0x000175C8
		public override string GetRaidCompleteMenu()
		{
			return base.BaseModel.GetRaidCompleteMenu();
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000193D5 File Offset: 0x000175D5
		public override bool IsPlunderMenu(string menuId)
		{
			return base.BaseModel.IsPlunderMenu(menuId);
		}
	}
}
