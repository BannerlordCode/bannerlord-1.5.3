using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.AI.AgentComponents;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000066 RID: 102
	public class CombatMissionWithDialogueController : MissionLogic, IMissionAgentSpawnLogic, IMissionBehavior
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00017757 File Offset: 0x00015957
		public BattleSideEnum PlayerSide
		{
			get
			{
				return BattleSideEnum.None;
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001775A File Offset: 0x0001595A
		public CombatMissionWithDialogueController(IMissionTroopSupplier[] suppliers, BasicCharacterObject characterToTalkTo)
		{
			this._troopSuppliers = suppliers;
			this._characterToTalkTo = characterToTalkTo;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00017770 File Offset: 0x00015970
		public override void OnCreated()
		{
			base.OnCreated();
			base.Mission.DoesMissionRequireCivilianEquipment = true;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00017784 File Offset: 0x00015984
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._battleAgentLogic = Mission.Current.GetMissionBehavior<BattleAgentLogic>();
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001779C File Offset: 0x0001599C
		public override void AfterStart()
		{
			base.AfterStart();
			base.Mission.DeploymentPlan.MakeDefaultDeploymentPlans();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x000177B4 File Offset: 0x000159B4
		public override void OnMissionTick(float dt)
		{
			if (!this._isMissionInitialized)
			{
				this.SpawnAgents();
				this._isMissionInitialized = true;
				base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
				return;
			}
			if (!this._troopsInitialized)
			{
				this._troopsInitialized = true;
				foreach (Agent agent in base.Mission.Agents)
				{
					this._battleAgentLogic.OnAgentBuild(agent, null);
				}
			}
			if (!this._conversationInitialized && Agent.Main != null && Agent.Main.IsActive())
			{
				foreach (Agent agent2 in base.Mission.Agents)
				{
					ScriptedMovementComponent component = agent2.GetComponent<ScriptedMovementComponent>();
					if (component != null && component.ShouldConversationStartWithAgent())
					{
						this.StartConversation(agent2, true);
						this._conversationInitialized = true;
					}
				}
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x000178C0 File Offset: 0x00015AC0
		public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
			if (!this._conversationInitialized && affectedAgent.Team != Mission.Current.PlayerTeam && affectorAgent != null && affectorAgent == Agent.Main)
			{
				this._conversationInitialized = true;
				this.StartFight(false);
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x000178F8 File Offset: 0x00015AF8
		public void StartFight(bool hasPlayerChangedSide)
		{
			base.Mission.SetMissionMode(MissionMode.Battle, false);
			if (hasPlayerChangedSide)
			{
				Agent.Main.SetTeam((Agent.Main.Team == base.Mission.AttackerTeam) ? base.Mission.DefenderTeam : base.Mission.AttackerTeam, true);
				Mission.Current.PlayerTeam = Agent.Main.Team;
			}
			foreach (Agent agent in base.Mission.Agents)
			{
				if (Agent.Main != agent)
				{
					if (hasPlayerChangedSide && agent.Team != Mission.Current.PlayerTeam && agent.Origin.BattleCombatant as PartyBase == PartyBase.MainParty)
					{
						agent.SetTeam(Mission.Current.PlayerTeam, true);
					}
					AgentFlag agentFlags = agent.GetAgentFlags();
					agent.SetAgentFlags(agentFlags | AgentFlag.CanGetAlarmed);
					agent.GetComponent<CampaignAgentComponent>().CreateAgentNavigator();
					agent.GetComponent<CampaignAgentComponent>().AgentNavigator.AddBehaviorGroup<AlarmedBehaviorGroup>();
					agent.SetAlarmState(Agent.AIStateFlag.Alarmed);
				}
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00017A2C File Offset: 0x00015C2C
		public void StartConversation(Agent agent, bool setActionsInstantly)
		{
			Campaign.Current.ConversationManager.SetupAndStartMissionConversation(agent, base.Mission.MainAgent, setActionsInstantly);
			foreach (IAgent agent2 in Campaign.Current.ConversationManager.ConversationAgents)
			{
				Agent agent3 = (Agent)agent2;
				agent3.ForceAiBehaviorSelection();
				agent3.AgentVisuals.SetClothComponentKeepStateOfAllMeshes(true);
			}
			base.Mission.MainAgentServer.AgentVisuals.SetClothComponentKeepStateOfAllMeshes(true);
			base.Mission.SetMissionMode(MissionMode.Conversation, setActionsInstantly);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00017AD0 File Offset: 0x00015CD0
		private void SpawnAgents()
		{
			Agent agent = null;
			IMissionTroopSupplier[] troopSuppliers = this._troopSuppliers;
			for (int i = 0; i < troopSuppliers.Length; i++)
			{
				foreach (IAgentOriginBase agentOriginBase in troopSuppliers[i].SupplyTroops(25).ToList<IAgentOriginBase>())
				{
					Agent agent2 = Mission.Current.SpawnTroop(agentOriginBase, agentOriginBase.BattleCombatant.Side == BattleSideEnum.Attacker, false, false, false, 0, 0, false, true, null, null, null, null, FormationClass.NumberOfAllFormations, false);
					this._numSpawnedTroops++;
					if (!agent2.IsMainAgent)
					{
						agent2.AddComponent(new ScriptedMovementComponent(agent2, agent2.Character == this._characterToTalkTo, (float)(agentOriginBase.IsUnderPlayersCommand ? 5 : 2)));
						if (agent2.Character == this._characterToTalkTo)
						{
							agent = agent2;
						}
					}
				}
			}
			foreach (Agent agent3 in base.Mission.Agents)
			{
				ScriptedMovementComponent component = agent3.GetComponent<ScriptedMovementComponent>();
				if (component != null)
				{
					if (agent3.Team.Side == Mission.Current.PlayerTeam.Side)
					{
						component.SetTargetAgent(agent);
					}
					else
					{
						component.SetTargetAgent(Agent.Main);
					}
				}
				agent3.SetFiringOrder(FiringOrder.RangedWeaponUsageOrderEnum.HoldYourFire);
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00017C64 File Offset: 0x00015E64
		public void StartSpawner(BattleSideEnum side)
		{
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00017C66 File Offset: 0x00015E66
		public void StopSpawner(BattleSideEnum side)
		{
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00017C68 File Offset: 0x00015E68
		public bool IsSideSpawnEnabled(BattleSideEnum side)
		{
			return false;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00017C6B File Offset: 0x00015E6B
		public float GetReinforcementInterval(BattleSideEnum battleSide = BattleSideEnum.None)
		{
			return 0f;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00017C74 File Offset: 0x00015E74
		public bool IsSideDepleted(BattleSideEnum side)
		{
			int num = this._troopSuppliers[(int)side].GetAllTroops().Count<IAgentOriginBase>() - this._troopSuppliers[(int)side].NumTroopsNotSupplied - this._troopSuppliers[(int)side].NumRemovedTroops;
			if (Mission.Current.PlayerTeam == base.Mission.DefenderTeam)
			{
				if (side == BattleSideEnum.Attacker)
				{
					num -= MobileParty.MainParty.Party.NumberOfHealthyMembers;
				}
				else if (Agent.Main != null && Agent.Main.IsActive())
				{
					num += MobileParty.MainParty.Party.NumberOfHealthyMembers;
				}
			}
			return num == 0;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00017D08 File Offset: 0x00015F08
		public IEnumerable<IAgentOriginBase> GetAllTroopsForSide(BattleSideEnum side)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00017D0F File Offset: 0x00015F0F
		public int GetNumberOfPlayerControllableTroops()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00017D16 File Offset: 0x00015F16
		public bool GetSpawnHorses(BattleSideEnum side)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000214 RID: 532
		private BattleAgentLogic _battleAgentLogic;

		// Token: 0x04000215 RID: 533
		private readonly BasicCharacterObject _characterToTalkTo;

		// Token: 0x04000216 RID: 534
		private bool _isMissionInitialized;

		// Token: 0x04000217 RID: 535
		private bool _troopsInitialized;

		// Token: 0x04000218 RID: 536
		private bool _conversationInitialized;

		// Token: 0x04000219 RID: 537
		private int _numSpawnedTroops;

		// Token: 0x0400021A RID: 538
		private readonly IMissionTroopSupplier[] _troopSuppliers;
	}
}
