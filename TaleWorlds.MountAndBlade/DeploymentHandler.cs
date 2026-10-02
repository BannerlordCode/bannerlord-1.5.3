using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000288 RID: 648
	public abstract class DeploymentHandler : MissionLogic
	{
		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06002444 RID: 9284 RVA: 0x0008263C File Offset: 0x0008083C
		// (remove) Token: 0x06002445 RID: 9285 RVA: 0x00082674 File Offset: 0x00080874
		public event Action OnPlayerSideDeploymentReady;

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06002446 RID: 9286 RVA: 0x000826AC File Offset: 0x000808AC
		// (remove) Token: 0x06002447 RID: 9287 RVA: 0x000826E4 File Offset: 0x000808E4
		public event Action OnEnemySideDeploymentReady;

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002448 RID: 9288 RVA: 0x00082719 File Offset: 0x00080919
		public Team PlayerTeam
		{
			get
			{
				return base.Mission.PlayerTeam;
			}
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x00082726 File Offset: 0x00080926
		public DeploymentHandler(bool isPlayerAttacker)
		{
			this.IsPlayerAttacker = isPlayerAttacker;
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x00082735 File Offset: 0x00080935
		public override void OnBehaviorInitialize()
		{
			this._deploymentMissionController = base.Mission.GetMissionBehavior<DeploymentMissionController>();
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x00082748 File Offset: 0x00080948
		public override void EarlyStart()
		{
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x0008274A File Offset: 0x0008094A
		public override void AfterStart()
		{
			this.PreviousMissionMode = base.Mission.Mode;
			base.Mission.SetMissionMode(MissionMode.Deployment, true);
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x0008276A File Offset: 0x0008096A
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			base.Mission.SetMissionMode(this.PreviousMissionMode, false);
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x00082784 File Offset: 0x00080984
		public override void OnBattleSideSpawned(BattleSideEnum side)
		{
			if (side == base.Mission.PlayerTeam.Side)
			{
				Action onPlayerSideDeploymentReady = this.OnPlayerSideDeploymentReady;
				if (onPlayerSideDeploymentReady == null)
				{
					return;
				}
				onPlayerSideDeploymentReady();
				return;
			}
			else
			{
				Action onEnemySideDeploymentReady = this.OnEnemySideDeploymentReady;
				if (onEnemySideDeploymentReady == null)
				{
					return;
				}
				onEnemySideDeploymentReady();
				return;
			}
		}

		// Token: 0x0600244F RID: 9295
		public abstract void AutoDeployTeamUsingDeploymentPlan(Team playerTeam);

		// Token: 0x06002450 RID: 9296
		public abstract void ForceUpdateAllUnits();

		// Token: 0x06002451 RID: 9297 RVA: 0x000827BA File Offset: 0x000809BA
		public virtual void FinishDeployment()
		{
			this._deploymentMissionController.FinishDeployment();
			(base.Mission ?? Mission.Current).IsTeleportingAgents = false;
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x000827DC File Offset: 0x000809DC
		public void InitializeDeploymentPoints()
		{
			if (!this._areDeploymentPointsInitialized)
			{
				foreach (DeploymentPoint deploymentPoint in base.Mission.ActiveMissionObjects.FindAllWithType<DeploymentPoint>())
				{
					deploymentPoint.Hide();
				}
				this._areDeploymentPointsInitialized = true;
			}
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x00082840 File Offset: 0x00080A40
		public static void OrderController_OnOrderIssued_Aux(OrderType orderType, MBReadOnlyList<Formation> appliedFormations, OrderController orderController = null, params object[] delegateParams)
		{
			DeploymentHandler.<>c__DisplayClass22_0 CS$<>8__locals1;
			CS$<>8__locals1.appliedFormations = appliedFormations;
			CS$<>8__locals1.orderController = orderController;
			bool flag = false;
			using (List<Formation>.Enumerator enumerator = CS$<>8__locals1.appliedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CountOfUnits > 0)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			switch (orderType)
			{
			case OrderType.None:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\MissionLogics\\DeploymentHandler.cs", "OrderController_OnOrderIssued_Aux", 159);
				return;
			case OrderType.Move:
			case OrderType.MoveToLineSegment:
			case OrderType.MoveToLineSegmentWithHorizontalLayout:
			case OrderType.FollowMe:
			case OrderType.FollowEntity:
			case OrderType.Advance:
			case OrderType.FallBack:
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForcePositioning|22_1(ref CS$<>8__locals1);
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForceUpdateFormationParams|22_0(ref CS$<>8__locals1);
				return;
			case OrderType.Charge:
			case OrderType.ChargeWithTarget:
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForcePositioning|22_1(ref CS$<>8__locals1);
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForceUpdateFormationParams|22_0(ref CS$<>8__locals1);
				return;
			case OrderType.StandYourGround:
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForcePositioning|22_1(ref CS$<>8__locals1);
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForceUpdateFormationParams|22_0(ref CS$<>8__locals1);
				return;
			case OrderType.Retreat:
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForcePositioning|22_1(ref CS$<>8__locals1);
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForceUpdateFormationParams|22_0(ref CS$<>8__locals1);
				return;
			case OrderType.LookAtEnemy:
			case OrderType.LookAtDirection:
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForcePositioning|22_1(ref CS$<>8__locals1);
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForceUpdateFormationParams|22_0(ref CS$<>8__locals1);
				return;
			case OrderType.ArrangementLine:
			case OrderType.ArrangementCloseOrder:
			case OrderType.ArrangementLoose:
			case OrderType.ArrangementCircular:
			case OrderType.ArrangementSchiltron:
			case OrderType.ArrangementVee:
			case OrderType.ArrangementColumn:
			case OrderType.ArrangementScatter:
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForceUpdateFormationParams|22_0(ref CS$<>8__locals1);
				return;
			case OrderType.FormCustom:
			case OrderType.FormDeep:
			case OrderType.FormWide:
			case OrderType.FormWider:
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForceUpdateFormationParams|22_0(ref CS$<>8__locals1);
				return;
			case OrderType.CohesionHigh:
			case OrderType.CohesionMedium:
			case OrderType.CohesionLow:
			case OrderType.HoldFire:
			case OrderType.FireAtWill:
				return;
			case OrderType.Mount:
			case OrderType.Dismount:
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForceUpdateFormationParams|22_0(ref CS$<>8__locals1);
				return;
			case OrderType.AIControlOn:
			case OrderType.AIControlOff:
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForcePositioning|22_1(ref CS$<>8__locals1);
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForceUpdateFormationParams|22_0(ref CS$<>8__locals1);
				return;
			case OrderType.Transfer:
			case OrderType.Use:
			case OrderType.AttackEntity:
				DeploymentHandler.<OrderController_OnOrderIssued_Aux>g__ForceUpdateFormationParams|22_0(ref CS$<>8__locals1);
				return;
			case OrderType.PointDefence:
				Debug.FailedAssert("will be removed", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\MissionLogics\\DeploymentHandler.cs", "OrderController_OnOrderIssued_Aux", 231);
				return;
			}
			Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\MissionLogics\\DeploymentHandler.cs", "OrderController_OnOrderIssued_Aux", 234);
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x00082A28 File Offset: 0x00080C28
		public virtual void HandleGeneralsDeploymentFrames()
		{
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x00082A2C File Offset: 0x00080C2C
		[CompilerGenerated]
		internal unsafe static void <OrderController_OnOrderIssued_Aux>g__ForceUpdateFormationParams|22_0(ref DeploymentHandler.<>c__DisplayClass22_0 A_0)
		{
			foreach (Formation formation in A_0.appliedFormations)
			{
				if (formation.CountOfUnits > 0 && (A_0.orderController == null || A_0.orderController.FormationUpdateEnabledAfterSetOrder))
				{
					MovementOrder movementOrder = *formation.GetReadonlyMovementOrderReference();
					bool flag = false;
					if (formation.IsPlayerTroopInFormation)
					{
						flag = movementOrder.OrderEnum == MovementOrder.MovementOrderEnum.Follow;
					}
					bool flag2 = movementOrder.OrderEnum == MovementOrder.MovementOrderEnum.Stop;
					OrderController.TryCancelStopOrder(formation);
					formation.ApplyActionOnEachUnit(delegate(Agent agent)
					{
						agent.ForceUpdateCachedAndFormationValues(true, false);
					}, flag ? Mission.Current.InitialPlayerAgent : null);
					formation.SetHasPendingUnitPositions(false);
					if (flag2)
					{
						formation.SetMovementOrder(MovementOrder.MovementOrderStop);
					}
				}
			}
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x00082B18 File Offset: 0x00080D18
		[CompilerGenerated]
		internal unsafe static void <OrderController_OnOrderIssued_Aux>g__ForcePositioning|22_1(ref DeploymentHandler.<>c__DisplayClass22_0 A_0)
		{
			foreach (Formation formation in A_0.appliedFormations)
			{
				if (formation.CountOfUnits > 0)
				{
					Vec2 direction = formation.FacingOrder.GetDirection(formation, null);
					Formation formation2 = formation;
					MovementOrder movementOrder = *formation.GetReadonlyMovementOrderReference();
					formation2.SetPositioning(new WorldPosition?(movementOrder.CreateNewOrderWorldPositionMT(formation, WorldPosition.WorldPositionEnforcedCache.None)), new Vec2?(direction), null);
				}
			}
		}

		// Token: 0x04000DF0 RID: 3568
		protected MissionMode PreviousMissionMode;

		// Token: 0x04000DF1 RID: 3569
		protected readonly bool IsPlayerAttacker;

		// Token: 0x04000DF2 RID: 3570
		protected DeploymentMissionController _deploymentMissionController;

		// Token: 0x04000DF3 RID: 3571
		private bool _areDeploymentPointsInitialized;
	}
}
