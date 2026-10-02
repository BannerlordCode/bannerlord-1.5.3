using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Missions.Handlers
{
	// Token: 0x020003F6 RID: 1014
	public class BattleDeploymentHandler : DeploymentHandler
	{
		// Token: 0x060037F2 RID: 14322 RVA: 0x000E7F66 File Offset: 0x000E6166
		public BattleDeploymentHandler(bool isPlayerAttacker)
			: base(isPlayerAttacker)
		{
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x000E7F6F File Offset: 0x000E616F
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			if (base.PlayerTeam != null)
			{
				base.PlayerTeam.OnOrderIssued -= this.OrderController_OnOrderIssued;
			}
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x000E7F96 File Offset: 0x000E6196
		public override void AfterStart()
		{
			base.AfterStart();
			base.PlayerTeam.OnOrderIssued += this.OrderController_OnOrderIssued;
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x000E7FB8 File Offset: 0x000E61B8
		public override void AutoDeployTeamUsingDeploymentPlan(Team team)
		{
			List<Formation> list = team.FormationsIncludingEmpty.ToList<Formation>();
			if (list.Count > 0)
			{
				bool isTeleportingAgents = base.Mission.IsTeleportingAgents;
				base.Mission.IsTeleportingAgents = true;
				OrderController orderController = (team.IsPlayerTeam ? team.PlayerOrderController : team.MasterOrderController);
				orderController.SelectAllFormations(false);
				this.SetDefaultFormationOrders(orderController);
				orderController.ClearSelectedFormations();
				IMissionDeploymentPlan deploymentPlan = base.Mission.DeploymentPlan;
				if (deploymentPlan.IsPlanMade(team))
				{
					using (List<Formation>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Formation formation = enumerator.Current;
							IFormationDeploymentPlan formationPlan = deploymentPlan.GetFormationPlan(team, formation.FormationIndex, false);
							WorldPosition worldPosition;
							Vec2 vec;
							base.Mission.GetFormationSpawnFrame(formation.Team, formation.FormationIndex, false, out worldPosition, out vec, true);
							if (formationPlan.HasDimensions)
							{
								formation.SetFormOrder(FormOrder.FormOrderCustom(formationPlan.PlannedWidth), true);
							}
							formation.SetMovementOrder(MovementOrder.MovementOrderMove(worldPosition));
							formation.SetFacingOrder(FacingOrder.FacingOrderLookAtDirection(vec));
							formation.SetPositioning(new WorldPosition?(worldPosition), new Vec2?(vec), new int?(formation.ArrangementOrder.GetUnitSpacing()));
							formation.ApplyActionOnEachUnit(delegate(Agent agent)
							{
								agent.ForceUpdateCachedAndFormationValues(true, false);
							}, null);
							formation.SetHasPendingUnitPositions(false);
							formation.SetMovementOrder(MovementOrder.MovementOrderStop);
						}
						goto IL_0189;
					}
				}
				Debug.FailedAssert("Failed to deploy team. Initial deployment plan is not made yet.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\MissionLogics\\BattleDeploymentHandler.cs", "AutoDeployTeamUsingDeploymentPlan", 84);
				IL_0189:
				foreach (Formation formation2 in list)
				{
					formation2.ApplyActionOnEachUnit(delegate(Agent agent)
					{
						agent.ForceUpdateCachedAndFormationValues(true, false);
					}, null);
					formation2.SetHasPendingUnitPositions(false);
				}
				base.Mission.IsTeleportingAgents = isTeleportingAgents;
			}
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x000E81CC File Offset: 0x000E63CC
		public override void ForceUpdateAllUnits()
		{
			DeploymentHandler.OrderController_OnOrderIssued_Aux(OrderType.Move, base.PlayerTeam.FormationsIncludingSpecialAndEmpty, null, Array.Empty<object>());
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x000E81E8 File Offset: 0x000E63E8
		public void SetDefaultFormationOrders(OrderController orderController)
		{
			orderController.SetOrder(OrderType.AIControlOff);
			orderController.SetFormationUpdateEnabledAfterSetOrder(false);
			orderController.SetOrder(OrderType.Mount);
			orderController.SetOrder(OrderType.FireAtWill);
			orderController.SetOrder(OrderType.ArrangementLine);
			orderController.SetOrder(OrderType.StandYourGround);
			orderController.SetOrder((base.Mission.IsSiegeBattle || base.Mission.IsSallyOutBattle) ? OrderType.AIControlOn : OrderType.AIControlOff);
			orderController.SetFormationUpdateEnabledAfterSetOrder(true);
		}

		// Token: 0x060037F8 RID: 14328 RVA: 0x000E8250 File Offset: 0x000E6450
		private void OrderController_OnOrderIssued(OrderType orderType, MBReadOnlyList<Formation> appliedFormations, OrderController orderController, params object[] delegateParams)
		{
			DeploymentHandler.OrderController_OnOrderIssued_Aux(orderType, appliedFormations, orderController, delegateParams);
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x000E825C File Offset: 0x000E645C
		public override void HandleGeneralsDeploymentFrames()
		{
			bool isTeleportingAgents = base.Mission.IsTeleportingAgents;
			base.Mission.IsTeleportingAgents = true;
			foreach (Team team in base.Mission.Teams)
			{
				if (team.GeneralAgent != null)
				{
					WorldPosition worldPosition;
					Vec2 vec;
					base.Mission.GetFormationSpawnFrame(team, FormationClass.NumberOfRegularFormations, false, out worldPosition, out vec, true);
					if (worldPosition.GetNavMesh() != UIntPtr.Zero && worldPosition.IsValid)
					{
						team.GeneralAgent.TrySetFormationFrame(in worldPosition, in vec);
					}
				}
			}
			base.Mission.IsTeleportingAgents = isTeleportingAgents;
		}
	}
}
