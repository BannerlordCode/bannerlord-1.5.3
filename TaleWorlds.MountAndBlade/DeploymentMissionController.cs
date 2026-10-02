using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000289 RID: 649
	public abstract class DeploymentMissionController : MissionLogic
	{
		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x00082BB0 File Offset: 0x00080DB0
		// (set) Token: 0x06002458 RID: 9304 RVA: 0x00082BB8 File Offset: 0x00080DB8
		public bool TeamSetupOver { get; private set; }

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06002459 RID: 9305 RVA: 0x00082BC4 File Offset: 0x00080DC4
		// (remove) Token: 0x0600245A RID: 9306 RVA: 0x00082BFC File Offset: 0x00080DFC
		public event Action OnAfterSetupTeams;

		// Token: 0x0600245B RID: 9307 RVA: 0x00082C31 File Offset: 0x00080E31
		public DeploymentMissionController(bool isPlayerAttacker)
		{
			this.IsPlayerAttacker = isPlayerAttacker;
			this.PlayerSide = (this.IsPlayerAttacker ? BattleSideEnum.Attacker : BattleSideEnum.Defender);
			this.EnemySide = (this.IsPlayerAttacker ? BattleSideEnum.Defender : BattleSideEnum.Attacker);
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x00082C64 File Offset: 0x00080E64
		public override void AfterStart()
		{
			base.Mission.AllowAiTicking = false;
			this.OnAfterStart();
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x00082C78 File Offset: 0x00080E78
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			base.Mission.AreOrderGesturesEnabled_AdditionalCondition += this.AreOrderGesturesEnabled_AdditionalCondition;
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x00082C98 File Offset: 0x00080E98
		public void FinishDeployment()
		{
			this.BeforeDeploymentFinished();
			if (this.IsPlayerAttacker)
			{
				this.UnhideAgentsOfSide(BattleSideEnum.Defender);
			}
			base.Mission.OnDeploymentFinished();
			foreach (Team team in base.Mission.Teams)
			{
				foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							if (agent.IsAIControlled)
							{
								agent.SetAlarmState(Agent.AIStateFlag.Alarmed);
								agent.SetIsAIPaused(false);
								if (agent.GetAgentFlags().HasAnyFlag(AgentFlag.CanWieldWeapon))
								{
									agent.ResetEnemyCaches();
								}
								HumanAIComponent humanAIComponent = agent.HumanAIComponent;
								if (humanAIComponent == null)
								{
									return;
								}
								humanAIComponent.SyncBehaviorParamsIfNecessary();
							}
						}, null);
					}
				}
			}
			Agent initialPlayerAgent = base.Mission.InitialPlayerAgent;
			initialPlayerAgent.SetDetachableFromFormation(true);
			initialPlayerAgent.Controller = AgentControllerType.Player;
			base.Mission.AllowAiTicking = true;
			base.Mission.DisableDying = false;
			base.Mission.SetFallAvoidSystemActive(false);
			base.Mission.OnAfterDeploymentFinished();
			this.AfterDeploymentFinished();
			base.Mission.RemoveMissionBehavior(this);
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x00082DCC File Offset: 0x00080FCC
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (!this.TeamSetupOver && base.Mission.Scene != null)
			{
				this.SetupTeams();
				this.TeamSetupOver = true;
			}
			if (this.TeamSetupOver && !this.AfterSetupTeamsCalled)
			{
				Action onAfterSetupTeams = this.OnAfterSetupTeams;
				if (onAfterSetupTeams != null)
				{
					onAfterSetupTeams();
				}
				this.AfterSetupTeamsCalled = true;
			}
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x00082E30 File Offset: 0x00081030
		protected void SetupAgentAIStatesForSide(BattleSideEnum battleSide)
		{
			foreach (Team team in Mission.GetTeamsOfSide(battleSide))
			{
				foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							if (agent.IsAIControlled)
							{
								agent.SetAlarmState(Agent.AIStateFlag.None);
								agent.SetIsAIPaused(true);
							}
						}, null);
					}
				}
			}
		}

		// Token: 0x06002461 RID: 9313
		protected abstract void OnAfterStart();

		// Token: 0x06002462 RID: 9314
		protected abstract void OnSetupTeamsOfSide(BattleSideEnum side);

		// Token: 0x06002463 RID: 9315
		protected abstract void OnSetupTeamsFinished();

		// Token: 0x06002464 RID: 9316
		protected abstract void BeforeDeploymentFinished();

		// Token: 0x06002465 RID: 9317
		protected abstract void AfterDeploymentFinished();

		// Token: 0x06002466 RID: 9318 RVA: 0x00082EE0 File Offset: 0x000810E0
		protected virtual void SetupAIOfEnemySide(BattleSideEnum enemySide)
		{
			Team team = ((enemySide == BattleSideEnum.Attacker) ? base.Mission.AttackerTeam : base.Mission.DefenderTeam);
			this.SetupAIOfEnemyTeam(team);
			Team team2 = ((enemySide == BattleSideEnum.Attacker) ? base.Mission.AttackerAllyTeam : base.Mission.DefenderAllyTeam);
			if (team2 != null)
			{
				this.SetupAIOfEnemyTeam(team2);
			}
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x00082F38 File Offset: 0x00081138
		protected virtual void SetupAIOfEnemyTeam(Team team)
		{
			foreach (Formation formation in team.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.SetControlledByAI(true, false);
				}
			}
			team.QuerySystem.Expire();
			base.Mission.AllowAiTicking = true;
			base.Mission.ForceTickOccasionally = true;
			team.ResetTactic();
			bool isTeleportingAgents = Mission.Current.IsTeleportingAgents;
			base.Mission.IsTeleportingAgents = true;
			team.Tick(0f);
			base.Mission.IsTeleportingAgents = isTeleportingAgents;
			base.Mission.AllowAiTicking = false;
			base.Mission.ForceTickOccasionally = false;
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x00083004 File Offset: 0x00081204
		private void SetupTeams()
		{
			Utilities.SetLoadingScreenPercentage(0.92f);
			base.Mission.DisableDying = true;
			base.Mission.SetFallAvoidSystemActive(true);
			this.OnSetupTeamsOfSide(this.EnemySide);
			this.SetupAIOfEnemySide(this.EnemySide);
			if (this.IsPlayerAttacker)
			{
				this.HideAgentsOfSide(BattleSideEnum.Defender);
			}
			this.OnSetupTeamsOfSide(this.PlayerSide);
			Agent initialPlayerAgent = base.Mission.InitialPlayerAgent;
			initialPlayerAgent.Controller = AgentControllerType.None;
			initialPlayerAgent.SetIsAIPaused(true);
			initialPlayerAgent.SetDetachableFromFormation(false);
			this.OnSetupTeamsFinished();
			base.Mission.AreOrderGesturesEnabled_AdditionalCondition -= this.AreOrderGesturesEnabled_AdditionalCondition;
			Utilities.SetLoadingScreenPercentage(0.96f);
			if (!MissionGameModels.Current.BattleInitializationModel.CanPlayerSideDeployWithOrderOfBattle())
			{
				this.FinishDeployment();
			}
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000830C4 File Offset: 0x000812C4
		private void HideAgentsOfSide(BattleSideEnum side)
		{
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.IsHuman && agent.Team != null && agent.Team.Side == side)
				{
					agent.SetRenderCheckEnabled(false);
					agent.AgentVisuals.SetVisible(false);
					Agent mountAgent = agent.MountAgent;
					if (mountAgent != null)
					{
						mountAgent.SetRenderCheckEnabled(false);
					}
					Agent mountAgent2 = agent.MountAgent;
					if (mountAgent2 != null)
					{
						mountAgent2.AgentVisuals.SetVisible(false);
					}
				}
			}
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x00083170 File Offset: 0x00081370
		private void UnhideAgentsOfSide(BattleSideEnum side)
		{
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.IsHuman && agent.Team != null && agent.Team.Side == side)
				{
					agent.SetRenderCheckEnabled(true);
					agent.AgentVisuals.SetVisible(true);
					Agent mountAgent = agent.MountAgent;
					if (mountAgent != null)
					{
						mountAgent.SetRenderCheckEnabled(true);
					}
					Agent mountAgent2 = agent.MountAgent;
					if (mountAgent2 != null)
					{
						mountAgent2.AgentVisuals.SetVisible(true);
					}
				}
			}
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x0008321C File Offset: 0x0008141C
		private bool AreOrderGesturesEnabled_AdditionalCondition()
		{
			return false;
		}

		// Token: 0x04000DF5 RID: 3573
		protected readonly bool IsPlayerAttacker;

		// Token: 0x04000DF6 RID: 3574
		protected BattleSideEnum PlayerSide;

		// Token: 0x04000DF7 RID: 3575
		protected BattleSideEnum EnemySide;

		// Token: 0x04000DF8 RID: 3576
		protected bool AfterSetupTeamsCalled;
	}
}
