using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000086 RID: 134
	public class SearchBodyMissionHandler : MissionLogic
	{
		// Token: 0x0600053A RID: 1338 RVA: 0x000230CC File Offset: 0x000212CC
		public override void OnAgentInteraction(Agent userAgent, Agent agent, sbyte agentBoneIndex)
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				if (Game.Current.GameStateManager.ActiveState is MissionState)
				{
					if (base.Mission.Mode != MissionMode.Conversation && base.Mission.Mode != MissionMode.Battle && this.IsSearchable(agent))
					{
						this.AddItemsToPlayer(agent);
						return;
					}
				}
				else
				{
					Debug.FailedAssert("Agent interaction must occur in MissionState.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\SearchBodyMissionHandler.cs", "OnAgentInteraction", 26);
				}
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0002313F File Offset: 0x0002133F
		public override bool IsThereAgentAction(Agent userAgent, Agent otherAgent)
		{
			return Mission.Current.Mode != MissionMode.Battle && base.Mission.Mode != MissionMode.Duel && base.Mission.Mode != MissionMode.Conversation && this.IsSearchable(otherAgent);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00023176 File Offset: 0x00021376
		private bool IsSearchable(Agent agent)
		{
			return !agent.IsActive() && agent.IsHuman && agent.Character.IsHero;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00023198 File Offset: 0x00021398
		private void AddItemsToPlayer(Agent interactedAgent)
		{
			CharacterObject characterObject = (CharacterObject)interactedAgent.Character;
			if (MBRandom.RandomInt(2) == 0)
			{
				characterObject.HeroObject.SpecialItems.Add(MBObjectManager.Instance.GetObject<ItemObject>("leafblade_throwing_knife"));
			}
			else
			{
				characterObject.HeroObject.SpecialItems.Add(MBObjectManager.Instance.GetObject<ItemObject>("falchion_sword_t2"));
				characterObject.HeroObject.SpecialItems.Add(MBObjectManager.Instance.GetObject<ItemObject>("cleaver_sword_t3"));
			}
			foreach (ItemObject itemObject in characterObject.HeroObject.SpecialItems)
			{
				PartyBase.MainParty.ItemRoster.AddToCounts(itemObject, 1);
				MBTextManager.SetTextVariable("ITEM_NAME", itemObject.Name, false);
				InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_item_taken", null).ToString()));
			}
			characterObject.HeroObject.SpecialItems.Clear();
		}
	}
}
