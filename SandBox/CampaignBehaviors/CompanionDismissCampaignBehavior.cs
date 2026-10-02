using System;
using SandBox.Conversation;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements.Locations;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000D8 RID: 216
	internal class CompanionDismissCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060009C9 RID: 2505 RVA: 0x00048E23 File Offset: 0x00047023
		public override void RegisterEvents()
		{
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.OnCompanionRemoved));
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x00048E3C File Offset: 0x0004703C
		private void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			if (LocationComplex.Current != null)
			{
				LocationComplex.Current.RemoveCharacterIfExists(companion);
			}
			if (PlayerEncounter.LocationEncounter != null)
			{
				PlayerEncounter.LocationEncounter.RemoveAccompanyingCharacter(companion);
			}
			if (detail == RemoveCompanionAction.RemoveCompanionDetail.Fire && Hero.MainHero.CurrentSettlement != null)
			{
				AgentNavigator agentNavigator = ConversationMission.OneToOneConversationAgent.GetComponent<CampaignAgentComponent>().AgentNavigator;
				if (((agentNavigator != null) ? agentNavigator.GetActiveBehavior() : null) is FollowAgentBehavior)
				{
					agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().RemoveBehavior<FollowAgentBehavior>();
				}
			}
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00048EAA File Offset: 0x000470AA
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
