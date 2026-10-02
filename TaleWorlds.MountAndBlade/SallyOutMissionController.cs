using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200029E RID: 670
	public abstract class SallyOutMissionController : MissionLogic
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x0600253B RID: 9531 RVA: 0x000875B6 File Offset: 0x000857B6
		private float BesiegedDeploymentDuration
		{
			get
			{
				return 55f;
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x0600253C RID: 9532 RVA: 0x000875BD File Offset: 0x000857BD
		private float BesiegerActivationDuration
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x0600253D RID: 9533 RVA: 0x000875C4 File Offset: 0x000857C4
		public MBReadOnlyList<SiegeWeapon> BesiegerSiegeEngines
		{
			get
			{
				return this._besiegerSiegeEngines;
			}
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x000875CC File Offset: 0x000857CC
		public SallyOutMissionController(bool isSallyOutAmbush)
		{
			this._isSallyOutAmbush = isSallyOutAmbush;
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x000875DB File Offset: 0x000857DB
		public override void OnBehaviorInitialize()
		{
			this.MissionAgentSpawnLogic = base.Mission.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
			this._sallyOutNotificationsHandler = new SallyOutMissionNotificationsHandler(this.MissionAgentSpawnLogic, this);
			Mission.Current.GetOverriddenFleePositionForAgent += this.GetSallyOutFleePositionForAgent;
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x00087618 File Offset: 0x00085818
		public override void AfterStart()
		{
			this._sallyOutNotificationsHandler.OnAfterStart();
			int num;
			int num2;
			this.GetInitialTroopCounts(out num, out num2);
			this.SetupInitialSpawn(num, num2);
			this._castleGates = base.Mission.MissionObjects.FindAllWithType<CastleGate>().ToList<CastleGate>();
			this._besiegedDeploymentTimer = new BasicMissionTimer();
			TeamAIComponent teamAI = base.Mission.DefenderTeam.TeamAI;
			teamAI.OnNotifyTacticalDecision = (TeamAIComponent.TacticalDecisionDelegate)Delegate.Combine(teamAI.OnNotifyTacticalDecision, new TeamAIComponent.TacticalDecisionDelegate(this.OnDefenderTeamTacticalDecision));
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x00087699 File Offset: 0x00085899
		public override void OnMissionTick(float dt)
		{
			this._sallyOutNotificationsHandler.OnMissionTick(dt);
			this.UpdateTimers();
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x000876B0 File Offset: 0x000858B0
		public override void OnDeploymentFinished()
		{
			this._besiegerSiegeEngines = SallyOutMissionController.GetBesiegerSiegeEngines();
			SallyOutMissionController.DisableSiegeEngines();
			if (this._isSallyOutAmbush)
			{
				Mission.Current.AddMissionBehavior(new SallyOutEndLogic());
			}
			this._sallyOutNotificationsHandler.OnDeploymentFinished();
			this._besiegerActivationTimer = new BasicMissionTimer();
			this.DeactivateBesiegers();
			this.ActivateDefenders();
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x00087706 File Offset: 0x00085906
		protected override void OnEndMission()
		{
			this._sallyOutNotificationsHandler.OnMissionEnd();
			Mission.Current.GetOverriddenFleePositionForAgent -= this.GetSallyOutFleePositionForAgent;
		}

		// Token: 0x06002544 RID: 9540
		protected abstract void GetInitialTroopCounts(out int besiegedTotalTroopCount, out int besiegerTotalTroopCount);

		// Token: 0x06002545 RID: 9541 RVA: 0x0008772C File Offset: 0x0008592C
		private void UpdateTimers()
		{
			if (this._besiegedDeploymentTimer != null)
			{
				if (this._besiegedDeploymentTimer.ElapsedTime >= this.BesiegedDeploymentDuration)
				{
					foreach (CastleGate castleGate in this._castleGates)
					{
						castleGate.SetAutoOpenState(true);
					}
					this._besiegedDeploymentTimer = null;
					goto IL_012B;
				}
				using (List<CastleGate>.Enumerator enumerator = this._castleGates.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CastleGate castleGate2 = enumerator.Current;
						if (!castleGate2.IsDestroyed && !castleGate2.IsGateOpen)
						{
							castleGate2.OpenDoor();
						}
					}
					goto IL_012B;
				}
			}
			Agent mainAgent = base.Mission.MainAgent;
			if (mainAgent != null && mainAgent.IsActive())
			{
				Vec3 eyeGlobalPosition = mainAgent.GetEyeGlobalPosition();
				foreach (CastleGate castleGate3 in this._castleGates)
				{
					if (!castleGate3.IsDestroyed && !castleGate3.IsGateOpen && eyeGlobalPosition.DistanceSquared(castleGate3.GameEntity.GlobalPosition) <= 25f)
					{
						castleGate3.OpenDoor();
					}
				}
			}
			IL_012B:
			if (this._besiegerActivationTimer != null && this._besiegerActivationTimer.ElapsedTime >= this.BesiegerActivationDuration)
			{
				this.ActivateBesiegers();
				this._besiegerActivationTimer = null;
			}
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x000878B4 File Offset: 0x00085AB4
		private void ActivateDefenders()
		{
			if (base.Mission.DefenderAllyTeam != null)
			{
				foreach (Agent agent in base.Mission.DefenderAllyTeam.ActiveAgents.ToList<Agent>())
				{
					FormationClass formationIndex = agent.Formation.FormationIndex;
					agent.SetTeam(base.Mission.DefenderTeam, true);
					agent.Formation = base.Mission.DefenderTeam.GetFormation(formationIndex);
				}
			}
			foreach (Formation formation in base.Mission.DefenderTeam.FormationsIncludingSpecialAndEmpty)
			{
				formation.SetMovementOrder(MovementOrder.MovementOrderCharge);
			}
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x000879A0 File Offset: 0x00085BA0
		private void AdjustTotalTroopCounts(ref int besiegedTotalTroopCount, ref int besiegerTotalTroopCount)
		{
			float num = 0.25f;
			float num2 = 1f - num;
			int num3 = (int)((float)this.MissionAgentSpawnLogic.BattleSize * num);
			int num4 = (int)((float)this.MissionAgentSpawnLogic.BattleSize * num2);
			besiegedTotalTroopCount = MathF.Min(besiegedTotalTroopCount, num3);
			besiegerTotalTroopCount = MathF.Min(besiegerTotalTroopCount, num4);
			float num5 = num2 / num;
			if ((float)besiegerTotalTroopCount / (float)besiegedTotalTroopCount <= num5)
			{
				int num6 = (int)((float)besiegerTotalTroopCount / num5);
				besiegedTotalTroopCount = MathF.Min(num6, besiegedTotalTroopCount);
				return;
			}
			int num7 = (int)((float)besiegedTotalTroopCount * num5);
			besiegerTotalTroopCount = MathF.Min(num7, besiegerTotalTroopCount);
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x00087A28 File Offset: 0x00085C28
		private void SetupInitialSpawn(int besiegedTotalTroopCount, int besiegerTotalTroopCount)
		{
			this.AdjustTotalTroopCounts(ref besiegedTotalTroopCount, ref besiegerTotalTroopCount);
			int num = besiegedTotalTroopCount + besiegerTotalTroopCount;
			int num2 = MathF.Min(besiegedTotalTroopCount, MathF.Ceiling((float)num * 0.1f));
			int num3 = MathF.Min(besiegerTotalTroopCount, MathF.Ceiling((float)num * 0.1f));
			this.MissionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Defender, true);
			this.MissionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Attacker, false);
			MissionSpawnSettings missionSpawnSettings = SallyOutMissionController.CreateSallyOutSpawnSettings(0.01f, 0.1f);
			this.MissionAgentSpawnLogic.InitWithSinglePhase(besiegedTotalTroopCount, besiegerTotalTroopCount, num2, num3, false, false, in missionSpawnSettings);
			this.MissionAgentSpawnLogic.SetCustomReinforcementSpawnTimer(new SallyOutReinforcementSpawnTimer(1f, 90f, 15f, 5));
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x00087AC8 File Offset: 0x00085CC8
		private WorldPosition? GetSallyOutFleePositionForAgent(Agent agent)
		{
			if (!agent.IsHuman)
			{
				return null;
			}
			Formation formation = agent.Formation;
			if (formation == null || formation.Team.Side == BattleSideEnum.Attacker)
			{
				return null;
			}
			bool flag = !agent.HasMount;
			bool isRangedCached = agent.IsRangedCached;
			FormationClass formationClass;
			if (flag)
			{
				formationClass = (isRangedCached ? FormationClass.Ranged : FormationClass.Infantry);
			}
			else
			{
				formationClass = (isRangedCached ? FormationClass.HorseArcher : FormationClass.Cavalry);
			}
			return new WorldPosition?(Mission.Current.DeploymentPlan.GetFormationPlan(formation.Team, formationClass, false).CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3));
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x00087B50 File Offset: 0x00085D50
		private static MissionSpawnSettings CreateSallyOutSpawnSettings(float besiegedReinforcementPercentage, float besiegerReinforcementPercentage)
		{
			return new MissionSpawnSettings(MissionSpawnSettings.InitialSpawnMethod.FreeAllocation, MissionSpawnSettings.ReinforcementTimingMethod.CustomTimer, MissionSpawnSettings.ReinforcementSpawnMethod.Fixed, 0f, 0f, 0f, 0f, 0, besiegedReinforcementPercentage, besiegerReinforcementPercentage, 1f, 0.75f);
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x00087B88 File Offset: 0x00085D88
		private void OnDefenderTeamTacticalDecision(in TacticalDecision decision)
		{
			TacticalDecision tacticalDecision = decision;
			if (tacticalDecision.DecisionCode == 31)
			{
				this._sallyOutNotificationsHandler.OnBesiegedSideFallsbackToKeep();
			}
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x00087BB4 File Offset: 0x00085DB4
		private void DeactivateBesiegers()
		{
			foreach (Formation formation in base.Mission.AttackerTeam.FormationsIncludingSpecialAndEmpty)
			{
				formation.SetMovementOrder(MovementOrder.MovementOrderStop);
				formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
				formation.SetControlledByAI(false, false);
			}
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x00087C28 File Offset: 0x00085E28
		private void ActivateBesiegers()
		{
			Team attackerTeam = base.Mission.AttackerTeam;
			foreach (Formation formation in base.Mission.AttackerTeam.FormationsIncludingSpecialAndEmpty)
			{
				formation.SetControlledByAI(true, false);
			}
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x00087C90 File Offset: 0x00085E90
		public static MBReadOnlyList<SiegeWeapon> GetBesiegerSiegeEngines()
		{
			MBList<SiegeWeapon> mblist = new MBList<SiegeWeapon>();
			using (List<MissionObject>.Enumerator enumerator = Mission.Current.ActiveMissionObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SiegeWeapon siegeWeapon;
					if ((siegeWeapon = enumerator.Current as SiegeWeapon) != null && siegeWeapon.DestructionComponent != null && siegeWeapon.Side == BattleSideEnum.Attacker)
					{
						mblist.Add(siegeWeapon);
					}
				}
			}
			return mblist;
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x00087D08 File Offset: 0x00085F08
		public static void DisableSiegeEngines()
		{
			for (int i = Mission.Current.ActiveMissionObjects.Count - 1; i >= 0; i--)
			{
				SiegeWeapon siegeWeapon;
				if ((siegeWeapon = Mission.Current.ActiveMissionObjects[i] as SiegeWeapon) != null && siegeWeapon.DestructionComponent != null && !siegeWeapon.IsDeactivated)
				{
					siegeWeapon.Disable();
					siegeWeapon.Deactivate();
				}
			}
		}

		// Token: 0x04000E5F RID: 3679
		private const float BesiegedTotalTroopRatio = 0.25f;

		// Token: 0x04000E60 RID: 3680
		private const float BesiegedInitialTroopRatio = 0.1f;

		// Token: 0x04000E61 RID: 3681
		private const float BesiegedReinforcementRatio = 0.01f;

		// Token: 0x04000E62 RID: 3682
		private const float BesiegerInitialTroopRatio = 0.1f;

		// Token: 0x04000E63 RID: 3683
		private const float BesiegerReinforcementRatio = 0.1f;

		// Token: 0x04000E64 RID: 3684
		private const float BesiegedInitialInterval = 1f;

		// Token: 0x04000E65 RID: 3685
		private const float BesiegerInitialInterval = 90f;

		// Token: 0x04000E66 RID: 3686
		private const float BesiegerIntervalChange = 15f;

		// Token: 0x04000E67 RID: 3687
		private const int BesiegerIntervalChangeCount = 5;

		// Token: 0x04000E68 RID: 3688
		private const float PlayerToGateSquaredDistanceThreshold = 25f;

		// Token: 0x04000E69 RID: 3689
		private SallyOutMissionNotificationsHandler _sallyOutNotificationsHandler;

		// Token: 0x04000E6A RID: 3690
		private List<CastleGate> _castleGates;

		// Token: 0x04000E6B RID: 3691
		private BasicMissionTimer _besiegedDeploymentTimer;

		// Token: 0x04000E6C RID: 3692
		private BasicMissionTimer _besiegerActivationTimer;

		// Token: 0x04000E6D RID: 3693
		private MBReadOnlyList<SiegeWeapon> _besiegerSiegeEngines;

		// Token: 0x04000E6E RID: 3694
		protected DefaultBattleMissionAgentSpawnLogic MissionAgentSpawnLogic;

		// Token: 0x04000E6F RID: 3695
		private bool _isSallyOutAmbush;
	}
}
