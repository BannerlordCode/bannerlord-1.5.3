using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using SandBox.Conversation.MissionLogics;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000075 RID: 117
	public class MissionAlleyHandler : MissionLogic
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0001E3E1 File Offset: 0x0001C5E1
		public bool CanThugConversationBeTriggered
		{
			get
			{
				return this._disguiseMissionLogic == null || this._disguiseMissionLogic.CanCommonAreaFightBeTriggered();
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0001E3F8 File Offset: 0x0001C5F8
		public override void OnMissionTick(float dt)
		{
			if (!this._agentCachesInitialized)
			{
				this._conversationTriggeredAlleys = new Dictionary<Alley, bool>();
				foreach (Agent agent in base.Mission.Agents)
				{
					if (agent.IsHuman)
					{
						CampaignAgentComponent component = agent.GetComponent<CampaignAgentComponent>();
						bool flag;
						if (component == null)
						{
							flag = null != null;
						}
						else
						{
							AgentNavigator agentNavigator = component.AgentNavigator;
							flag = ((agentNavigator != null) ? agentNavigator.MemberOfAlley : null) != null;
						}
						if (flag && component.AgentNavigator.MemberOfAlley.Owner != Hero.MainHero)
						{
							if (!this._rivalThugAgentsAndAgentNavigators.ContainsKey(agent))
							{
								this._rivalThugAgentsAndAgentNavigators.Add(agent, component.AgentNavigator);
							}
							if (!this._conversationTriggeredAlleys.ContainsKey(component.AgentNavigator.MemberOfAlley))
							{
								this._conversationTriggeredAlleys.Add(component.AgentNavigator.MemberOfAlley, false);
							}
						}
					}
				}
				this._agentCachesInitialized = base.Mission.Agents.Count > 0;
			}
			if (Mission.Current.Mode == MissionMode.Battle)
			{
				this.EndFightIfPlayerIsFarAwayOrNearGuard();
				return;
			}
			if (MBRandom.RandomFloat < dt * 10f && this.CanThugConversationBeTriggered)
			{
				this.CheckAndTriggerConversationWithRivalThug();
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001E548 File Offset: 0x0001C748
		private void CheckAndTriggerConversationWithRivalThug()
		{
			if (!Campaign.Current.ConversationManager.IsConversationFlowActive && Agent.Main != null)
			{
				foreach (KeyValuePair<Agent, AgentNavigator> keyValuePair in this._rivalThugAgentsAndAgentNavigators)
				{
					bool flag;
					if (keyValuePair.Key.IsActive() && this._conversationTriggeredAlleys.TryGetValue(keyValuePair.Value.MemberOfAlley, out flag) && !flag)
					{
						Agent key = keyValuePair.Key;
						if (key.GetDistanceTo(Agent.Main) < 5f && keyValuePair.Value.CanSeeAgent(Agent.Main))
						{
							Mission.Current.GetMissionBehavior<MissionConversationLogic>().StartConversation(key, false, false);
							this._conversationTriggeredAlleys[keyValuePair.Value.MemberOfAlley] = true;
							break;
						}
					}
				}
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001E640 File Offset: 0x0001C840
		public override void AfterStart()
		{
			this._disguiseMissionLogic = Mission.Current.GetMissionBehavior<DisguiseMissionLogic>();
			MissionAlleyHandler._guardAgents = new List<Agent>();
			this._rivalThugAgentsAndAgentNavigators = new Dictionary<Agent, AgentNavigator>();
			MissionAlleyHandler._fightPosition = Vec3.Invalid;
			this._missionFightHandler = Mission.Current.GetMissionBehavior<MissionFightHandler>();
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0001E68C File Offset: 0x0001C88C
		private void EndFightIfPlayerIsFarAwayOrNearGuard()
		{
			if (Agent.Main != null)
			{
				bool flag = false;
				foreach (Agent agent in MissionAlleyHandler._guardAgents)
				{
					if ((Agent.Main.Position - agent.Position).Length <= 10f)
					{
						flag = true;
						break;
					}
				}
				if (MissionAlleyHandler._fightPosition != Vec3.Invalid && (Agent.Main.Position - MissionAlleyHandler._fightPosition).Length >= 20f)
				{
					flag = true;
				}
				if (flag)
				{
					this.EndFight();
				}
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001E74C File Offset: 0x0001C94C
		private ValueTuple<bool, string> CanPlayerOccupyTheCurrentAlley()
		{
			if (!Settlement.CurrentSettlement.Alleys.All<Alley>((Alley x) => x.Owner != Hero.MainHero))
			{
				TextObject textObject = new TextObject("{=ribkM9dl}You already own another alley in the settlement.", null);
				return new ValueTuple<bool, string>(false, textObject.ToString());
			}
			if (!Campaign.Current.Models.AlleyModel.GetClanMembersAndAvailabilityDetailsForLeadingAnAlley(CampaignMission.Current.LastVisitedAlley).Any<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>>((ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail> x) => x.Item2 == DefaultAlleyModel.AlleyMemberAvailabilityDetail.Available || x.Item2 == DefaultAlleyModel.AlleyMemberAvailabilityDetail.AvailableWithDelay))
			{
				TextObject textObject = new TextObject("{=hnhKJYbx}You don't have any suitable clan members to assign this alley. ({ROGUERY_SKILL} skill {NEEDED_SKILL_LEVEL} or higher, {TRAIT_NAME} trait {MAX_TRAIT_AMOUNT} or lower)", null);
				textObject.SetTextVariable("ROGUERY_SKILL", DefaultSkills.Roguery.Name);
				textObject.SetTextVariable("NEEDED_SKILL_LEVEL", 30);
				textObject.SetTextVariable("TRAIT_NAME", DefaultTraits.Mercy.Name);
				textObject.SetTextVariable("MAX_TRAIT_AMOUNT", 0);
				return new ValueTuple<bool, string>(false, textObject.ToString());
			}
			if (MobileParty.MainParty.MemberRoster.TotalRegulars < Campaign.Current.Models.AlleyModel.MinimumTroopCountInPlayerOwnedAlley)
			{
				TextObject textObject = new TextObject("{=zLnqZdIK}You don't have enough troops to assign this alley. (Needed at least {NEEDED_TROOP_NUMBER})", null);
				textObject.SetTextVariable("NEEDED_TROOP_NUMBER", Campaign.Current.Models.AlleyModel.MinimumTroopCountInPlayerOwnedAlley);
				return new ValueTuple<bool, string>(false, textObject.ToString());
			}
			return new ValueTuple<bool, string>(true, null);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001E8B0 File Offset: 0x0001CAB0
		private void EndFight()
		{
			this._missionFightHandler.EndFight(false);
			foreach (Agent agent in MissionAlleyHandler._guardAgents)
			{
				agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>().GetBehavior<FightBehavior>().IsActive = false;
			}
			MissionAlleyHandler._guardAgents.Clear();
			Mission.Current.SetMissionMode(MissionMode.StartUp, false);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001E938 File Offset: 0x0001CB38
		private void OnTakeOverTheAlley()
		{
			AlleyHelper.CreateMultiSelectionInquiryForSelectingClanMemberToAlley(CampaignMission.Current.LastVisitedAlley, new Action<List<InquiryElement>>(this.OnCompanionSelectedForNewAlley), new Action<List<InquiryElement>>(this.OnCompanionSelectionCancel));
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001E961 File Offset: 0x0001CB61
		private void OnCompanionSelectionCancel(List<InquiryElement> obj)
		{
			this.OnLeaveItEmpty();
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001E96C File Offset: 0x0001CB6C
		private void OnCompanionSelectedForNewAlley(List<InquiryElement> companion)
		{
			CharacterObject characterObject = companion.First<InquiryElement>().Identifier as CharacterObject;
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			troopRoster.AddToCounts(characterObject, 1, false, 0, 0, true, -1);
			AlleyHelper.OpenScreenForManagingAlley(true, troopRoster, new PartyPresentationDoneButtonDelegate(this.OnPartyScreenDoneClicked), new TextObject("{=s8dsW6m0}New Alley", null), new PartyPresentationCancelButtonDelegate(this.OnPartyScreenCancel));
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001E9C8 File Offset: 0x0001CBC8
		private void OnPartyScreenCancel()
		{
			this.OnLeaveItEmpty();
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001E9D0 File Offset: 0x0001CBD0
		public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon attackerWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
			if (!affectedAgent.IsHuman)
			{
				return;
			}
			if (affectorAgent != null && affectorAgent == Agent.Main && affectorAgent.IsHuman && affectedAgent.GetComponent<CampaignAgentComponent>().AgentNavigator != null)
			{
				InterruptingBehaviorGroup behaviorGroup = affectedAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<InterruptingBehaviorGroup>();
				TalkBehavior talkBehavior = ((behaviorGroup != null) ? behaviorGroup.GetBehavior<TalkBehavior>() : null);
				if (talkBehavior != null)
				{
					talkBehavior.Disable();
				}
				if (!affectedAgent.IsEnemyOf(affectorAgent) && affectedAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.MemberOfAlley != null)
				{
					this.StartCommonAreaBattle(affectedAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.MemberOfAlley);
				}
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001EA60 File Offset: 0x0001CC60
		private bool OnPartyScreenDoneClicked(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty, PartyBase rightParty)
		{
			TeleportHeroAction.ApplyDelayedTeleportToSettlement(leftMemberRoster.GetTroopRoster().Find((TroopRosterElement x) => x.Character.IsHero).Character.HeroObject, MobileParty.MainParty.CurrentSettlement);
			foreach (TroopRosterElement troopRosterElement in leftMemberRoster.GetTroopRoster())
			{
				if (!troopRosterElement.Character.IsHero)
				{
					MobileParty.MainParty.MemberRoster.RemoveTroop(troopRosterElement.Character, troopRosterElement.Number, default(UniqueTroopDescriptor), 0);
				}
			}
			CampaignEventDispatcher.Instance.OnAlleyOccupiedByPlayer(CampaignMission.Current.LastVisitedAlley, leftMemberRoster);
			return true;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001EB38 File Offset: 0x0001CD38
		public void StartCommonAreaBattle(Alley alley)
		{
			MissionAlleyHandler._guardAgents.Clear();
			this._conversationTriggeredAlleys[alley] = true;
			List<Agent> accompanyingAgents = new List<Agent>();
			foreach (Agent agent2 in Mission.Current.Agents)
			{
				LocationCharacter locationCharacter = LocationComplex.Current.FindCharacter(agent2);
				AccompanyingCharacter accompanyingCharacter = PlayerEncounter.LocationEncounter.GetAccompanyingCharacter(locationCharacter);
				CharacterObject characterObject = (CharacterObject)agent2.Character;
				if (accompanyingCharacter != null && accompanyingCharacter.IsFollowingPlayerAtMissionStart)
				{
					accompanyingAgents.Add(agent2);
				}
				else if (characterObject != null && (characterObject.Occupation == Occupation.Guard || characterObject.Occupation == Occupation.Soldier))
				{
					MissionAlleyHandler._guardAgents.Add(agent2);
				}
			}
			List<Agent> list = Mission.Current.Agents.Where<Agent>((Agent agent) => agent.IsHuman && agent.Character.IsHero && (agent.IsPlayerControlled || accompanyingAgents.Contains(agent))).ToList<Agent>();
			List<Agent> list2 = Mission.Current.Agents.Where<Agent>((Agent agent) => agent.IsHuman && agent.GetComponent<CampaignAgentComponent>().AgentNavigator != null && agent.GetComponent<CampaignAgentComponent>().AgentNavigator.MemberOfAlley == alley).ToList<Agent>();
			MissionAlleyHandler._fightPosition = Agent.Main.Position;
			Mission.Current.GetMissionBehavior<MissionFightHandler>().StartCustomFight(list, list2, false, false, new MissionFightHandler.OnFightEndDelegate(this.OnAlleyFightEnd), float.Epsilon);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0001ECA0 File Offset: 0x0001CEA0
		private void OnLeaveItEmpty()
		{
			CampaignEventDispatcher.Instance.OnAlleyClearedByPlayer(CampaignMission.Current.LastVisitedAlley);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001ECB8 File Offset: 0x0001CEB8
		private void OnAlleyFightEnd(bool isPlayerSideWon)
		{
			if (isPlayerSideWon)
			{
				object obj = new TextObject("{=4QfQBi2k}Alley fight won", null);
				TextObject textObject = new TextObject("{=8SK2BZum}You have cleared an alley which belonged to a gang leader. Now, you can either take it over for your own benefit or leave it empty to help the town. To own an alley, you will need to assign a suitable clan member and some troops to watch over it. This will provide denars to your clan, but also increase your crime rating.", null);
				TextObject textObject2 = new TextObject("{=qxY2ASqp}Take over the alley", null);
				TextObject textObject3 = new TextObject("{=jjEzdO0Y}Leave it empty", null);
				InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, true, textObject2.ToString(), textObject3.ToString(), new Action(this.OnTakeOverTheAlley), new Action(this.OnLeaveItEmpty), "", 0f, null, new Func<ValueTuple<bool, string>>(this.CanPlayerOccupyTheCurrentAlley), null), true, false);
			}
			else if (Agent.Main == null || !Agent.Main.IsActive())
			{
				Mission.Current.NextCheckTimeEndMission = 0f;
				if (!Campaign.Current.IsMainHeroDisguised)
				{
					Campaign.Current.GameMenuManager.SetNextMenu("settlement_player_unconscious");
				}
			}
			MissionAlleyHandler._fightPosition = Vec3.Invalid;
		}

		// Token: 0x0400027C RID: 636
		private const float ConstantForInitiatingConversation = 5f;

		// Token: 0x0400027D RID: 637
		private static Vec3 _fightPosition = Vec3.Invalid;

		// Token: 0x0400027E RID: 638
		private Dictionary<Agent, AgentNavigator> _rivalThugAgentsAndAgentNavigators;

		// Token: 0x0400027F RID: 639
		private const int DistanceForEndingAlleyFight = 20;

		// Token: 0x04000280 RID: 640
		private const int GuardAgentSafeZone = 10;

		// Token: 0x04000281 RID: 641
		private static List<Agent> _guardAgents;

		// Token: 0x04000282 RID: 642
		private Dictionary<Alley, bool> _conversationTriggeredAlleys;

		// Token: 0x04000283 RID: 643
		private bool _agentCachesInitialized;

		// Token: 0x04000284 RID: 644
		private MissionFightHandler _missionFightHandler;

		// Token: 0x04000285 RID: 645
		private DisguiseMissionLogic _disguiseMissionLogic;
	}
}
