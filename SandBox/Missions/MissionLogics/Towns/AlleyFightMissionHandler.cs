using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics.Towns
{
	// Token: 0x0200008D RID: 141
	public class AlleyFightMissionHandler : MissionLogic, IMissionAgentSpawnLogic, IMissionBehavior
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x000244A3 File Offset: 0x000226A3
		public BattleSideEnum PlayerSide
		{
			get
			{
				return BattleSideEnum.Defender;
			}
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x000244A6 File Offset: 0x000226A6
		public AlleyFightMissionHandler(TroopRoster playerSideTroops, TroopRoster rivalSideTroops)
		{
			this._playerSideTroops = playerSideTroops;
			this._rivalSideTroops = rivalSideTroops;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x000244D2 File Offset: 0x000226D2
		public override void OnBehaviorInitialize()
		{
			base.Mission.GetAgentTroopClass_Override += this.GetAlleyFightMissionTroopClass;
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x000244EC File Offset: 0x000226EC
		public override void EarlyStart()
		{
			base.EarlyStart();
			base.Mission.Teams.Add(BattleSideEnum.Defender, Clan.PlayerClan.Color, Clan.PlayerClan.Color2, Clan.PlayerClan.Banner, true, false, true);
			base.Mission.Teams.Add(BattleSideEnum.Attacker, Clan.BanditFactions.First<Clan>().Color, Clan.BanditFactions.First<Clan>().Color2, Clan.BanditFactions.First<Clan>().Banner, true, false, true);
			base.Mission.PlayerTeam = base.Mission.DefenderTeam;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0002458C File Offset: 0x0002278C
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
			if (this._playerSideAliveAgents.Contains(affectedAgent))
			{
				this._playerSideAliveAgents.Remove(affectedAgent);
				this._playerSideTroops.RemoveTroop(affectedAgent.Character as CharacterObject, 1, default(UniqueTroopDescriptor), 0);
			}
			else if (this._rivalSideAliveAgents.Contains(affectedAgent))
			{
				this._rivalSideAliveAgents.Remove(affectedAgent);
				this._rivalSideTroops.RemoveTroop(affectedAgent.Character as CharacterObject, 1, default(UniqueTroopDescriptor), 0);
			}
			if (affectedAgent == Agent.Main)
			{
				Campaign.Current.GetCampaignBehavior<IAlleyCampaignBehavior>().OnPlayerDiedInMission();
			}
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00024638 File Offset: 0x00022838
		public override void AfterStart()
		{
			DefaultMissionDeploymentPlan defaultMissionDeploymentPlan;
			base.Mission.GetDeploymentPlan<DefaultMissionDeploymentPlan>(out defaultMissionDeploymentPlan);
			defaultMissionDeploymentPlan.AddTroops(base.Mission.DefenderTeam, FormationClass.Infantry, this._playerSideTroops.TotalManCount, 0, false);
			defaultMissionDeploymentPlan.AddTroops(base.Mission.AttackerTeam, FormationClass.Infantry, this._rivalSideTroops.TotalManCount, 0, false);
			base.Mission.DeploymentPlan.MakeDefaultDeploymentPlans();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x000246A4 File Offset: 0x000228A4
		public override InquiryData OnEndMissionRequest(out bool canLeave)
		{
			canLeave = true;
			return new InquiryData("", GameTexts.FindText("str_give_up_fight", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(base.Mission.OnEndMissionResult), null, "", 0f, null, null, null);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0002470F File Offset: 0x0002290F
		public override void OnRetreatMission()
		{
			Campaign.Current.GetCampaignBehavior<IAlleyCampaignBehavior>().OnPlayerRetreatedFromMission();
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00024720 File Offset: 0x00022920
		public override void OnRenderingStarted()
		{
			Mission.Current.SetMissionMode(MissionMode.Battle, true);
			this.SpawnAgentsForBothSides();
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
			base.Mission.PlayerTeam.PlayerOrderController.SelectAllFormations(false);
			base.Mission.PlayerTeam.PlayerOrderController.SetOrder(OrderType.Charge);
			base.Mission.PlayerEnemyTeam.MasterOrderController.SelectAllFormations(false);
			base.Mission.PlayerEnemyTeam.MasterOrderController.SetOrder(OrderType.Charge);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x000247A3 File Offset: 0x000229A3
		public override void OnMissionStateFinalized()
		{
			base.Mission.GetAgentTroopClass_Override -= this.GetAlleyFightMissionTroopClass;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x000247BC File Offset: 0x000229BC
		private void SpawnAgentsForBothSides()
		{
			Mission.Current.PlayerEnemyTeam.SetIsEnemyOf(Mission.Current.PlayerTeam, true);
			foreach (TroopRosterElement troopRosterElement in this._playerSideTroops.GetTroopRoster())
			{
				for (int i = 0; i < troopRosterElement.Number; i++)
				{
					this.SpawnATroop(troopRosterElement.Character, true);
				}
			}
			foreach (TroopRosterElement troopRosterElement2 in this._rivalSideTroops.GetTroopRoster())
			{
				for (int j = 0; j < troopRosterElement2.Number; j++)
				{
					this.SpawnATroop(troopRosterElement2.Character, false);
				}
			}
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x000248AC File Offset: 0x00022AAC
		private void SpawnATroop(CharacterObject character, bool isPlayerSide)
		{
			SimpleAgentOrigin simpleAgentOrigin = new SimpleAgentOrigin(character, -1, null, default(UniqueTroopDescriptor));
			Agent agent = Mission.Current.SpawnTroop(simpleAgentOrigin, isPlayerSide, true, false, false, 0, 0, true, true, null, null, null, null, FormationClass.NumberOfAllFormations, false);
			if (isPlayerSide)
			{
				this._playerSideAliveAgents.Add(agent);
			}
			else
			{
				this._rivalSideAliveAgents.Add(agent);
			}
			AgentFlag agentFlags = agent.GetAgentFlags();
			agent.SetAgentFlags((agentFlags | AgentFlag.CanGetAlarmed) & ~AgentFlag.CanRetreat);
			if (agent.IsAIControlled)
			{
				agent.SetWatchState(Agent.WatchState.Alarmed);
			}
			if (isPlayerSide)
			{
				agent.SetTeam(Mission.Current.PlayerTeam, true);
				return;
			}
			agent.SetTeam(Mission.Current.PlayerEnemyTeam, true);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00024964 File Offset: 0x00022B64
		public void StartSpawner(BattleSideEnum side)
		{
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00024966 File Offset: 0x00022B66
		public void StopSpawner(BattleSideEnum side)
		{
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00024968 File Offset: 0x00022B68
		public bool IsSideSpawnEnabled(BattleSideEnum side)
		{
			return true;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0002496B File Offset: 0x00022B6B
		public bool IsSideDepleted(BattleSideEnum side)
		{
			if (side != BattleSideEnum.Attacker)
			{
				return this._playerSideAliveAgents.Count == 0;
			}
			return this._rivalSideAliveAgents.Count == 0;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0002498E File Offset: 0x00022B8E
		public float GetReinforcementInterval(BattleSideEnum side = BattleSideEnum.None)
		{
			return float.MaxValue;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00024995 File Offset: 0x00022B95
		public IEnumerable<IAgentOriginBase> GetAllTroopsForSide(BattleSideEnum side)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0002499C File Offset: 0x00022B9C
		public int GetNumberOfPlayerControllableTroops()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x000249A3 File Offset: 0x00022BA3
		public bool GetSpawnHorses(BattleSideEnum side)
		{
			return false;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x000249A6 File Offset: 0x00022BA6
		private FormationClass GetAlleyFightMissionTroopClass(BattleSideEnum battleSide, BasicCharacterObject agentCharacter)
		{
			return agentCharacter.GetFormationClass().DismountedClass();
		}

		// Token: 0x040002DC RID: 732
		private TroopRoster _playerSideTroops;

		// Token: 0x040002DD RID: 733
		private TroopRoster _rivalSideTroops;

		// Token: 0x040002DE RID: 734
		private List<Agent> _playerSideAliveAgents = new List<Agent>();

		// Token: 0x040002DF RID: 735
		private List<Agent> _rivalSideAliveAgents = new List<Agent>();
	}
}
