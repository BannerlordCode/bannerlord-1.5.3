using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000079 RID: 121
	public class MissionFightHandler : MissionLogic
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0001F0EF File Offset: 0x0001D2EF
		private static MissionFightHandler _current
		{
			get
			{
				return Mission.Current.GetMissionBehavior<MissionFightHandler>();
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x0001F0FB File Offset: 0x0001D2FB
		// (set) Token: 0x060004D9 RID: 1241 RVA: 0x0001F103 File Offset: 0x0001D303
		public float MinMissionEndTime { get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x0001F10C File Offset: 0x0001D30C
		public ReadOnlyCollection<Agent> PlayerSideAgents
		{
			get
			{
				return this._playerSideAgents.AsReadOnly();
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x0001F119 File Offset: 0x0001D319
		public ReadOnlyCollection<Agent> OpponentSideAgents
		{
			get
			{
				return this._opponentSideAgents.AsReadOnly();
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x0001F126 File Offset: 0x0001D326
		public bool IsPlayerSideWon
		{
			get
			{
				return this._isPlayerSideWon;
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001F12E File Offset: 0x0001D32E
		public override void OnBehaviorInitialize()
		{
			base.Mission.IsAgentInteractionAllowed_AdditionalCondition += this.IsAgentInteractionAllowed_AdditionalCondition;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001F147 File Offset: 0x0001D347
		public override void EarlyStart()
		{
			this._playerSideAgents = new List<Agent>();
			this._opponentSideAgents = new List<Agent>();
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001F15F File Offset: 0x0001D35F
		public override void AfterStart()
		{
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001F161 File Offset: 0x0001D361
		public override void OnMissionTick(float dt)
		{
			if (base.Mission.CurrentTime > this.MinMissionEndTime && this._finishTimer != null && this._finishTimer.ElapsedTime > 5f)
			{
				this._finishTimer = null;
				this.EndFight(false);
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001F1A0 File Offset: 0x0001D3A0
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (this._state != MissionFightHandler.State.Fighting)
			{
				return;
			}
			if (affectedAgent == Agent.Main)
			{
				Mission.Current.NextCheckTimeEndMission += 8f;
			}
			if (affectorAgent != null && this._playerSideAgents.Contains(affectedAgent))
			{
				this._playerSideAgents.Remove(affectedAgent);
				if (this._playerSideAgents.Count == 0)
				{
					this._isPlayerSideWon = false;
					this._finishTimer = new BasicMissionTimer();
					return;
				}
			}
			else if (affectorAgent != null && this._opponentSideAgents.Contains(affectedAgent))
			{
				this._opponentSideAgents.Remove(affectedAgent);
				if (this._opponentSideAgents.Count == 0)
				{
					this._isPlayerSideWon = true;
					this._finishTimer = new BasicMissionTimer();
				}
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001F250 File Offset: 0x0001D450
		public void StartCustomFight(List<Agent> playerSideAgents, List<Agent> opponentSideAgents, bool dropWeapons, bool isItemUseDisabled, MissionFightHandler.OnFightEndDelegate onFightEndDelegate, float minimumEndTime = 1E-45f)
		{
			this.StartFightInternal(playerSideAgents, opponentSideAgents, dropWeapons, isItemUseDisabled, onFightEndDelegate, minimumEndTime);
			this.SetTeamsForFightAndDuel();
			this._oldMissionMode = Mission.Current.Mode;
			Mission.Current.SetMissionMode(MissionMode.Battle, false);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001F284 File Offset: 0x0001D484
		public void StartFistFight(Agent opponent, MissionFightHandler.OnFightEndDelegate onFightEndDelegate, float minimumEndTime = 1E-45f)
		{
			this.StartFightInternal(new List<Agent> { Agent.Main }, new List<Agent> { opponent }, false, false, delegate(bool playerWon)
			{
				this.AttachCachedEquipment(Agent.Main, opponent);
				MissionFightHandler.OnFightEndDelegate onFightEndDelegate2 = onFightEndDelegate;
				if (onFightEndDelegate2 == null)
				{
					return;
				}
				onFightEndDelegate2(playerWon);
			}, minimumEndTime);
			this.SetTeamsForFightAndDuel();
			this._playerEquipment = new MissionEquipment();
			this._opponentEquipment = new MissionEquipment();
			this.RemoveWeaponsFromAgents(Agent.Main, opponent);
			this._oldMissionMode = Mission.Current.Mode;
			Mission.Current.SetMissionMode(MissionMode.Battle, false);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001F32C File Offset: 0x0001D52C
		private void RemoveWeaponsFromAgents(Agent main, Agent opponent)
		{
			this._playerEquipment.FillFrom(main.Equipment);
			this._opponentEquipment.FillFrom(opponent.Equipment);
			main.TryToSheathWeaponInHand(Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.Instant);
			main.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.Instant);
			opponent.TryToSheathWeaponInHand(Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.Instant);
			opponent.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.Instant);
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				main.RemoveEquippedWeapon(equipmentIndex);
				opponent.RemoveEquippedWeapon(equipmentIndex);
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001F398 File Offset: 0x0001D598
		private void AttachCachedEquipment(Agent main, Agent opponent)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				MissionWeapon missionWeapon = this._playerEquipment[equipmentIndex];
				main.EquipWeaponWithNewEntity(equipmentIndex, ref missionWeapon);
				MissionWeapon missionWeapon2 = this._opponentEquipment[equipmentIndex];
				opponent.EquipWeaponWithNewEntity(equipmentIndex, ref missionWeapon2);
			}
			this._playerEquipment = null;
			this._opponentEquipment = null;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001F3EC File Offset: 0x0001D5EC
		private void StartFightInternal(List<Agent> playerSideAgents, List<Agent> opponentSideAgents, bool dropWeapons, bool isItemUseDisabled, MissionFightHandler.OnFightEndDelegate onFightEndDelegate, float minimumEndTime = 1E-45f)
		{
			this._state = MissionFightHandler.State.Fighting;
			this._opponentSideAgents = opponentSideAgents;
			this._playerSideAgents = playerSideAgents;
			this._playerSideAgentsOldTeamData = new Dictionary<Agent, Team>();
			this._opponentSideAgentsOldTeamData = new Dictionary<Agent, Team>();
			MissionFightHandler._onFightEnd = onFightEndDelegate;
			this._isPlayerSideWon = false;
			Mission.Current.MainAgent.IsItemUseDisabled = isItemUseDisabled;
			foreach (Agent agent in this._opponentSideAgents)
			{
				if (dropWeapons)
				{
					this.DropAllWeapons(agent);
				}
				this._opponentSideAgentsOldTeamData.Add(agent, agent.Team);
				this.ForceAgentForFight(agent);
			}
			foreach (Agent agent2 in this._playerSideAgents)
			{
				if (dropWeapons)
				{
					this.DropAllWeapons(agent2);
				}
				this._playerSideAgentsOldTeamData.Add(agent2, agent2.Team);
				this.ForceAgentForFight(agent2);
			}
			if (minimumEndTime > 0f && !minimumEndTime.ApproximatelyEqualsTo(1E-45f, 1E-05f))
			{
				this.MinMissionEndTime = base.Mission.CurrentTime + minimumEndTime;
				return;
			}
			this.MinMissionEndTime = 0f;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001F540 File Offset: 0x0001D740
		public override InquiryData OnEndMissionRequest(out bool canPlayerLeave)
		{
			canPlayerLeave = true;
			if (this._state == MissionFightHandler.State.Fighting && (this._opponentSideAgents.Count > 0 || this._playerSideAgents.Count > 0))
			{
				MBInformationManager.AddQuickInformation(new TextObject("{=Fpk3BUBs}Your fight has not ended yet!", null), 0, null, null, "");
				canPlayerLeave = false;
			}
			return null;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001F591 File Offset: 0x0001D791
		private void ForceAgentForFight(Agent agent)
		{
			if (agent.GetComponent<CampaignAgentComponent>().AgentNavigator != null)
			{
				AlarmedBehaviorGroup behaviorGroup = agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>();
				behaviorGroup.DisableCalmDown = true;
				behaviorGroup.AddBehavior<FightBehavior>();
				behaviorGroup.SetScriptedBehavior<FightBehavior>();
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001F5C3 File Offset: 0x0001D7C3
		protected override void OnEndMission()
		{
			base.Mission.IsAgentInteractionAllowed_AdditionalCondition -= this.IsAgentInteractionAllowed_AdditionalCondition;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001F5DC File Offset: 0x0001D7DC
		private void SetTeamsForFightAndDuel()
		{
			Mission.Current.PlayerEnemyTeam.SetIsEnemyOf(Mission.Current.PlayerTeam, true);
			foreach (Agent agent in this._playerSideAgents)
			{
				if (agent.IsHuman)
				{
					if (agent.IsAIControlled)
					{
						agent.SetWatchState(Agent.WatchState.Alarmed);
					}
					agent.SetTeam(Mission.Current.PlayerTeam, true);
				}
			}
			foreach (Agent agent2 in this._opponentSideAgents)
			{
				if (agent2.IsHuman)
				{
					if (agent2.IsAIControlled)
					{
						agent2.SetWatchState(Agent.WatchState.Alarmed);
					}
					agent2.SetTeam(Mission.Current.PlayerEnemyTeam, true);
				}
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001F6D0 File Offset: 0x0001D8D0
		private void ResetTeamsForFightAndDuel()
		{
			foreach (Agent agent in this._playerSideAgents)
			{
				if (agent.IsAIControlled)
				{
					agent.ResetEnemyCaches();
					agent.InvalidateTargetAgent();
					agent.InvalidateAIWeaponSelections();
					agent.SetWatchState(Agent.WatchState.Patrolling);
				}
				agent.SetTeam(new Team(this._playerSideAgentsOldTeamData[agent].MBTeam, BattleSideEnum.None, base.Mission, uint.MaxValue, uint.MaxValue, null), true);
			}
			foreach (Agent agent2 in this._opponentSideAgents)
			{
				if (agent2.IsAIControlled)
				{
					agent2.ResetEnemyCaches();
					agent2.InvalidateTargetAgent();
					agent2.InvalidateAIWeaponSelections();
					agent2.SetWatchState(Agent.WatchState.Patrolling);
				}
				agent2.SetTeam(new Team(this._opponentSideAgentsOldTeamData[agent2].MBTeam, BattleSideEnum.None, base.Mission, uint.MaxValue, uint.MaxValue, null), true);
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001F7E8 File Offset: 0x0001D9E8
		private bool IsAgentInteractionAllowed_AdditionalCondition()
		{
			return this._state != MissionFightHandler.State.Fighting;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001F7F8 File Offset: 0x0001D9F8
		public static Agent GetAgentToSpectate()
		{
			MissionFightHandler current = MissionFightHandler._current;
			if (current._playerSideAgents.Count > 0)
			{
				return current._playerSideAgents[0];
			}
			if (current._opponentSideAgents.Count > 0)
			{
				return current._opponentSideAgents[0];
			}
			return null;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001F844 File Offset: 0x0001DA44
		private void DropAllWeapons(Agent agent)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (!agent.Equipment[equipmentIndex].IsEmpty)
				{
					agent.DropItem(equipmentIndex, WeaponClass.Undefined);
				}
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001F87C File Offset: 0x0001DA7C
		private void ResetScriptedBehaviors()
		{
			foreach (Agent agent in this._playerSideAgents)
			{
				if (agent.IsActive() && agent.GetComponent<CampaignAgentComponent>().AgentNavigator != null)
				{
					agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>().DisableScriptedBehavior();
				}
			}
			foreach (Agent agent2 in this._opponentSideAgents)
			{
				if (agent2.IsActive() && agent2.GetComponent<CampaignAgentComponent>().AgentNavigator != null)
				{
					agent2.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>().DisableScriptedBehavior();
				}
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001F958 File Offset: 0x0001DB58
		public void BeginEndFight()
		{
			this._finishTimer = new BasicMissionTimer();
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001F968 File Offset: 0x0001DB68
		public void EndFight(bool overrideDuelWonByPlayer = false)
		{
			this.ResetScriptedBehaviors();
			this.ResetTeamsForFightAndDuel();
			this._state = MissionFightHandler.State.FightEnded;
			foreach (Agent agent in this._playerSideAgents)
			{
				if (agent.IsActive())
				{
					agent.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.WithAnimationUninterruptible);
					agent.TryToSheathWeaponInHand(Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.WithAnimationUninterruptible);
				}
			}
			foreach (Agent agent2 in this._opponentSideAgents)
			{
				if (agent2.IsActive())
				{
					agent2.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.WithAnimationUninterruptible);
					agent2.TryToSheathWeaponInHand(Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.WithAnimationUninterruptible);
				}
			}
			this._playerSideAgents.Clear();
			this._opponentSideAgents.Clear();
			if (Mission.Current.MainAgent != null)
			{
				Mission.Current.MainAgent.IsItemUseDisabled = false;
			}
			if (this._oldMissionMode == MissionMode.Conversation && !Campaign.Current.ConversationManager.IsConversationFlowActive)
			{
				this._oldMissionMode = MissionMode.StartUp;
			}
			Mission.Current.SetMissionMode(this._oldMissionMode, false);
			if (MissionFightHandler._onFightEnd != null)
			{
				MissionFightHandler._onFightEnd(this._isPlayerSideWon || overrideDuelWonByPlayer);
				this._isPlayerSideWon = false;
				MissionFightHandler._onFightEnd = null;
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001FABC File Offset: 0x0001DCBC
		public bool IsThereActiveFight()
		{
			return this._state == MissionFightHandler.State.Fighting;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001FAC8 File Offset: 0x0001DCC8
		public void AddAgentToSide(Agent agent, bool isPlayerSide)
		{
			if (!this.IsThereActiveFight() || this._playerSideAgents.Contains(agent) || this._opponentSideAgents.Contains(agent))
			{
				return;
			}
			if (agent.IsAIControlled)
			{
				agent.SetWatchState(Agent.WatchState.Alarmed);
			}
			if (isPlayerSide)
			{
				agent.SetTeam(Mission.Current.PlayerTeam, true);
				this._playerSideAgents.Add(agent);
				this._playerSideAgentsOldTeamData.Add(agent, agent.Team);
			}
			else
			{
				agent.SetTeam(Mission.Current.PlayerEnemyTeam, true);
				this._opponentSideAgents.Add(agent);
				this._opponentSideAgentsOldTeamData.Add(agent, agent.Team);
			}
			if (this._playerSideAgents.Count == 0 || this._opponentSideAgents.Count == 0)
			{
				this._finishTimer = new BasicMissionTimer();
			}
			else
			{
				this._finishTimer = null;
			}
			this.ForceAgentForFight(agent);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001FBA4 File Offset: 0x0001DDA4
		public IEnumerable<Agent> GetDangerSources(Agent ownerAgent)
		{
			if (!(ownerAgent.Character is CharacterObject))
			{
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\MissionFightHandler.cs", "GetDangerSources", 469);
				return new List<Agent>();
			}
			if (this.IsThereActiveFight() && !MissionFightHandler.IsAgentAggressive(ownerAgent) && Agent.Main != null)
			{
				return new List<Agent> { Agent.Main };
			}
			return new List<Agent>();
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001FC0C File Offset: 0x0001DE0C
		public static bool IsAgentAggressive(Agent agent)
		{
			CharacterObject characterObject = agent.Character as CharacterObject;
			return agent.HasWeapon() || (characterObject != null && (characterObject.Occupation == Occupation.Mercenary || MissionFightHandler.IsAgentVillian(characterObject) || MissionFightHandler.IsAgentJusticeWarrior(characterObject)));
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001FC4D File Offset: 0x0001DE4D
		public static bool IsAgentJusticeWarrior(CharacterObject character)
		{
			return character.Occupation == Occupation.Soldier || character.Occupation == Occupation.Guard || character.Occupation == Occupation.PrisonGuard;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001FC6E File Offset: 0x0001DE6E
		public static bool IsAgentVillian(CharacterObject character)
		{
			return character.Occupation == Occupation.Gangster || character.Occupation == Occupation.GangLeader || character.Occupation == Occupation.Bandit;
		}

		// Token: 0x04000286 RID: 646
		private static MissionFightHandler.OnFightEndDelegate _onFightEnd;

		// Token: 0x04000288 RID: 648
		private List<Agent> _playerSideAgents;

		// Token: 0x04000289 RID: 649
		private List<Agent> _opponentSideAgents;

		// Token: 0x0400028A RID: 650
		private Dictionary<Agent, Team> _playerSideAgentsOldTeamData;

		// Token: 0x0400028B RID: 651
		private Dictionary<Agent, Team> _opponentSideAgentsOldTeamData;

		// Token: 0x0400028C RID: 652
		private MissionFightHandler.State _state;

		// Token: 0x0400028D RID: 653
		private BasicMissionTimer _finishTimer;

		// Token: 0x0400028E RID: 654
		private bool _isPlayerSideWon;

		// Token: 0x0400028F RID: 655
		private MissionMode _oldMissionMode;

		// Token: 0x04000290 RID: 656
		private MissionEquipment _playerEquipment;

		// Token: 0x04000291 RID: 657
		private MissionEquipment _opponentEquipment;

		// Token: 0x0200017D RID: 381
		private enum State
		{
			// Token: 0x0400072F RID: 1839
			NoFight,
			// Token: 0x04000730 RID: 1840
			Fighting,
			// Token: 0x04000731 RID: 1841
			FightEnded
		}

		// Token: 0x0200017E RID: 382
		// (Invoke) Token: 0x06000ECC RID: 3788
		public delegate void OnFightEndDelegate(bool isPlayerSideWon);
	}
}
