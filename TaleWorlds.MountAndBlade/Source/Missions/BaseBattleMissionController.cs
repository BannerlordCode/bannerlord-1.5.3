using System;
using System.Diagnostics;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Source.Missions
{
	// Token: 0x020003D8 RID: 984
	public abstract class BaseBattleMissionController : MissionLogic
	{
		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06003715 RID: 14101 RVA: 0x000E45C3 File Offset: 0x000E27C3
		// (set) Token: 0x06003716 RID: 14102 RVA: 0x000E45CB File Offset: 0x000E27CB
		private protected bool IsPlayerAttacker { protected get; private set; }

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x06003717 RID: 14103 RVA: 0x000E45D4 File Offset: 0x000E27D4
		// (set) Token: 0x06003718 RID: 14104 RVA: 0x000E45DC File Offset: 0x000E27DC
		private protected int DeployedAttackerTroopCount { protected get; private set; }

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06003719 RID: 14105 RVA: 0x000E45E5 File Offset: 0x000E27E5
		// (set) Token: 0x0600371A RID: 14106 RVA: 0x000E45ED File Offset: 0x000E27ED
		private protected int DeployedDefenderTroopCount { protected get; private set; }

		// Token: 0x0600371B RID: 14107 RVA: 0x000E45F6 File Offset: 0x000E27F6
		protected BaseBattleMissionController(bool isPlayerAttacker)
		{
			this.IsPlayerAttacker = isPlayerAttacker;
			this.game = Game.Current;
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x000E4610 File Offset: 0x000E2810
		public override void EarlyStart()
		{
			this.EarlyStart();
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x000E4618 File Offset: 0x000E2818
		public override void AfterStart()
		{
			base.AfterStart();
			this.CreateTeams();
			base.Mission.SetMissionMode(MissionMode.Battle, true);
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x000E4633 File Offset: 0x000E2833
		protected virtual void SetupTeam(Team team)
		{
			if (team.Side == BattleSideEnum.Attacker)
			{
				this.CreateAttackerTroops();
			}
			else
			{
				this.CreateDefenderTroops();
			}
			if (team == base.Mission.PlayerTeam)
			{
				this.CreatePlayer();
			}
		}

		// Token: 0x0600371F RID: 14111
		protected abstract void CreateDefenderTroops();

		// Token: 0x06003720 RID: 14112
		protected abstract void CreateAttackerTroops();

		// Token: 0x06003721 RID: 14113 RVA: 0x000E4660 File Offset: 0x000E2860
		public virtual TeamAIComponent GetTeamAI(Team team, float thinkTimerTime = 5f, float applyTimerTime = 1f)
		{
			return new TeamAIGeneral(base.Mission, team, thinkTimerTime, applyTimerTime);
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x000E4670 File Offset: 0x000E2870
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x000E4679 File Offset: 0x000E2879
		[Conditional("DEBUG")]
		private void DebugTick()
		{
			if (Input.DebugInput.IsHotKeyPressed("SwapToEnemy"))
			{
				this.BecomeEnemy();
			}
			if (Input.DebugInput.IsHotKeyDown("BaseBattleMissionControllerHotkeyBecomePlayer"))
			{
				this.BecomePlayer();
			}
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x000E46A9 File Offset: 0x000E28A9
		protected bool IsPlayerDead()
		{
			return base.Mission.MainAgent == null || !base.Mission.MainAgent.IsActive();
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x000E46D0 File Offset: 0x000E28D0
		public override bool MissionEnded(ref MissionResult missionResult)
		{
			if (!base.Mission.IsDeploymentFinished)
			{
				return false;
			}
			if (this.IsPlayerDead())
			{
				missionResult = MissionResult.CreateDefeated(base.Mission);
				return true;
			}
			if (base.Mission.GetMemberCountOfSide(BattleSideEnum.Attacker) == 0)
			{
				missionResult = ((base.Mission.PlayerTeam.Side == BattleSideEnum.Attacker) ? MissionResult.CreateDefeated(base.Mission) : MissionResult.CreateSuccessful(base.Mission, false));
				return true;
			}
			if (base.Mission.GetMemberCountOfSide(BattleSideEnum.Defender) == 0)
			{
				missionResult = ((base.Mission.PlayerTeam.Side == BattleSideEnum.Attacker) ? MissionResult.CreateSuccessful(base.Mission, false) : MissionResult.CreateDefeated(base.Mission));
				return true;
			}
			return false;
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x000E4780 File Offset: 0x000E2980
		public override InquiryData OnEndMissionRequest(out bool canPlayerLeave)
		{
			canPlayerLeave = true;
			if (!this.IsPlayerDead() && base.Mission.IsPlayerCloseToAnEnemy(5f))
			{
				canPlayerLeave = false;
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_can_not_retreat", null), 0, null, null, "");
			}
			else
			{
				MissionResult missionResult = null;
				if (!this.IsPlayerDead() && !this.MissionEnded(ref missionResult))
				{
					return new InquiryData("", GameTexts.FindText("str_retreat_question", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(base.Mission.OnEndMissionResult), null, "", 0f, null, null, null);
				}
			}
			return null;
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x000E4838 File Offset: 0x000E2A38
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x000E483C File Offset: 0x000E2A3C
		private void CreateTeams()
		{
			if (!base.Mission.Teams.IsEmpty<Team>())
			{
				throw new MBIllegalValueException("Number of teams is not 0.");
			}
			base.Mission.Teams.Add(BattleSideEnum.Defender, 4278190335U, 4278190335U, null, true, false, true);
			base.Mission.Teams.Add(BattleSideEnum.Attacker, 4278255360U, 4278255360U, null, true, false, true);
			if (this.IsPlayerAttacker)
			{
				base.Mission.PlayerTeam = base.Mission.AttackerTeam;
			}
			else
			{
				base.Mission.PlayerTeam = base.Mission.DefenderTeam;
			}
			TeamAIComponent teamAI = this.GetTeamAI(base.Mission.DefenderTeam, 5f, 1f);
			base.Mission.DefenderTeam.AddTeamAI(teamAI, false);
			TeamAIComponent teamAI2 = this.GetTeamAI(base.Mission.AttackerTeam, 5f, 1f);
			base.Mission.AttackerTeam.AddTeamAI(teamAI2, false);
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x000E4938 File Offset: 0x000E2B38
		protected void IncrementDeploymedTroops(BattleSideEnum side)
		{
			int num;
			if (side == BattleSideEnum.Attacker)
			{
				num = this.DeployedAttackerTroopCount;
				this.DeployedAttackerTroopCount = num + 1;
				return;
			}
			num = this.DeployedDefenderTroopCount;
			this.DeployedDefenderTroopCount = num + 1;
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x000E496C File Offset: 0x000E2B6C
		protected virtual void CreatePlayer()
		{
			this.game.PlayerTroop = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("main_hero");
			FormationClass formationClass = base.Mission.GetFormationSpawnClass(base.Mission.PlayerTeam, FormationClass.NumberOfRegularFormations, false);
			if (formationClass != FormationClass.NumberOfRegularFormations)
			{
				formationClass = this.game.PlayerTroop.DefaultFormationClass;
			}
			WorldPosition worldPosition;
			Vec2 vec;
			base.Mission.GetFormationSpawnFrame(base.Mission.PlayerTeam, formationClass, false, out worldPosition, out vec, true);
			Mission mission = base.Mission;
			AgentBuildData agentBuildData = new AgentBuildData(this.game.PlayerTroop).Team(base.Mission.PlayerTeam);
			Vec3 groundVec = worldPosition.GetGroundVec3();
			Agent agent = mission.SpawnAgent(agentBuildData.InitialPosition(in groundVec).InitialDirection(in vec).Controller(AgentControllerType.Player), false, null, null);
			agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
			base.Mission.MainAgent = agent;
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x000E4A43 File Offset: 0x000E2C43
		protected void BecomeEnemy()
		{
			base.Mission.MainAgent.Controller = AgentControllerType.AI;
			base.Mission.PlayerEnemyTeam.Leader.Controller = AgentControllerType.Player;
			this.SwapTeams();
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x000E4A72 File Offset: 0x000E2C72
		protected void BecomePlayer()
		{
			base.Mission.MainAgent.Controller = AgentControllerType.Player;
			base.Mission.PlayerEnemyTeam.Leader.Controller = AgentControllerType.AI;
			this.SwapTeams();
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x000E4AA1 File Offset: 0x000E2CA1
		protected void SwapTeams()
		{
			base.Mission.PlayerTeam = base.Mission.PlayerEnemyTeam;
			this.IsPlayerAttacker = !this.IsPlayerAttacker;
		}

		// Token: 0x040017C5 RID: 6085
		protected readonly Game game;
	}
}
