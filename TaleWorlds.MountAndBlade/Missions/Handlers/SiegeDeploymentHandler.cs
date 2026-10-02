using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.AI;

namespace TaleWorlds.MountAndBlade.Missions.Handlers
{
	// Token: 0x020003F7 RID: 1015
	public class SiegeDeploymentHandler : BattleDeploymentHandler
	{
		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x060037FA RID: 14330 RVA: 0x000E8318 File Offset: 0x000E6518
		// (set) Token: 0x060037FB RID: 14331 RVA: 0x000E8320 File Offset: 0x000E6520
		public IEnumerable<DeploymentPoint> PlayerDeploymentPoints { get; private set; }

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x060037FC RID: 14332 RVA: 0x000E8329 File Offset: 0x000E6529
		// (set) Token: 0x060037FD RID: 14333 RVA: 0x000E8331 File Offset: 0x000E6531
		public IEnumerable<DeploymentPoint> AllDeploymentPoints { get; private set; }

		// Token: 0x060037FE RID: 14334 RVA: 0x000E833A File Offset: 0x000E653A
		public SiegeDeploymentHandler(bool isPlayerAttacker)
			: base(isPlayerAttacker)
		{
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x000E8344 File Offset: 0x000E6544
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			MissionSiegeEnginesLogic missionBehavior = base.Mission.GetMissionBehavior<MissionSiegeEnginesLogic>();
			this._defenderSiegeWeaponsController = missionBehavior.GetSiegeWeaponsController(BattleSideEnum.Defender);
			this._attackerSiegeWeaponsController = missionBehavior.GetSiegeWeaponsController(BattleSideEnum.Attacker);
			this._defenderReferencePosition = WorldPosition.Invalid;
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x000E8388 File Offset: 0x000E6588
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			base.Mission.IsFormationUnitPositionAvailable_AdditionalCondition -= this.Mission_IsFormationUnitPositionAvailable_AdditionalCondition;
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x000E83A8 File Offset: 0x000E65A8
		public override void AfterStart()
		{
			base.AfterStart();
			this.AllDeploymentPoints = Mission.Current.ActiveMissionObjects.FindAllWithType<DeploymentPoint>();
			this.PlayerDeploymentPoints = this.AllDeploymentPoints.Where<DeploymentPoint>((DeploymentPoint dp) => dp.Side == base.PlayerTeam.Side);
			foreach (DeploymentPoint deploymentPoint in this.AllDeploymentPoints)
			{
				deploymentPoint.OnDeploymentStateChanged += this.OnDeploymentStateChange;
			}
			base.Mission.IsFormationUnitPositionAvailable_AdditionalCondition += this.Mission_IsFormationUnitPositionAvailable_AdditionalCondition;
			foreach (DeploymentPoint deploymentPoint2 in this.PlayerDeploymentPoints)
			{
				deploymentPoint2.OnDeployOrDisband += this.OnWeaponDeployOrDisband;
			}
			base.Mission.PlayerTeam.OnFormationsChangedInDeployment += this.OnFormationsChanged;
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x000E84B0 File Offset: 0x000E66B0
		private void OnWeaponDeployOrDisband(DeploymentPoint deploymentPoint)
		{
			if (deploymentPoint.IsDeployed)
			{
				SiegeWeapon siegeWeapon = deploymentPoint.DeployedWeapon as SiegeWeapon;
				if (siegeWeapon != null)
				{
					siegeWeapon.TickAuxForInit();
				}
				this.AutoAssignDetachmentsForDeployment(base.PlayerTeam);
			}
			using (List<Formation>.Enumerator enumerator = base.PlayerTeam.FormationsIncludingEmpty.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.ApplyActionOnEachUnit(delegate(Agent agent)
					{
						agent.ForceUpdateCachedAndFormationValues(false, false);
					}, null);
				}
			}
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x000E8550 File Offset: 0x000E6750
		public override void FinishDeployment()
		{
			foreach (DeploymentPoint deploymentPoint in this.AllDeploymentPoints)
			{
				deploymentPoint.OnDeploymentStateChanged -= this.OnDeploymentStateChange;
			}
			foreach (DeploymentPoint deploymentPoint2 in this.PlayerDeploymentPoints)
			{
				deploymentPoint2.OnDeployOrDisband -= this.OnWeaponDeployOrDisband;
			}
			base.Mission.PlayerTeam.OnFormationsChangedInDeployment -= this.OnFormationsChanged;
			base.FinishDeployment();
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x000E8610 File Offset: 0x000E6810
		public void DeployAllSiegeWeaponsOfPlayer()
		{
			BattleSideEnum side = (this.IsPlayerAttacker ? BattleSideEnum.Attacker : BattleSideEnum.Defender);
			new SiegeWeaponAutoDeployer((from dp in base.Mission.ActiveMissionObjects.FindAllWithType<DeploymentPoint>()
				where dp.Side == side
				select dp).ToList<DeploymentPoint>(), this.GetWeaponsControllerOfSide(side)).DeployAll(side);
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x000E8677 File Offset: 0x000E6877
		public int GetMaxDeployableWeaponCountOfPlayer(Type weapon)
		{
			return this.GetWeaponsControllerOfSide(this.IsPlayerAttacker ? BattleSideEnum.Attacker : BattleSideEnum.Defender).GetMaxDeployableWeaponCount(weapon);
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x000E8694 File Offset: 0x000E6894
		public void DeployAllSiegeWeaponsOfAi()
		{
			BattleSideEnum side = (this.IsPlayerAttacker ? BattleSideEnum.Defender : BattleSideEnum.Attacker);
			new SiegeWeaponAutoDeployer((from dp in base.Mission.ActiveMissionObjects.FindAllWithType<DeploymentPoint>()
				where dp.Side == side
				select dp).ToList<DeploymentPoint>(), this.GetWeaponsControllerOfSide(side)).DeployAll(side);
			this.RemoveDeploymentPoints(side);
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x000E8708 File Offset: 0x000E6908
		public void RemoveDeploymentPoints(BattleSideEnum side)
		{
			IEnumerable<DeploymentPoint> enumerable = base.Mission.ActiveMissionObjects.FindAllWithType<DeploymentPoint>();
			Func<DeploymentPoint, bool> <>9__0;
			Func<DeploymentPoint, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (DeploymentPoint dp) => dp.Side == side);
			}
			foreach (DeploymentPoint deploymentPoint in enumerable.Where<DeploymentPoint>(func).ToArray<DeploymentPoint>())
			{
				foreach (SynchedMissionObject synchedMissionObject in deploymentPoint.DeployableWeapons.ToArray<SynchedMissionObject>())
				{
					if (deploymentPoint.DeployedWeapon == null || !synchedMissionObject.GameEntity.IsVisibleIncludeParents())
					{
						SiegeWeapon siegeWeapon = synchedMissionObject as SiegeWeapon;
						if (siegeWeapon != null)
						{
							siegeWeapon.SetDisabledSynched();
						}
					}
				}
				deploymentPoint.SetDisabledSynched();
			}
		}

		// Token: 0x06003808 RID: 14344 RVA: 0x000E87D0 File Offset: 0x000E69D0
		public void RemoveUnavailableDeploymentPoints(BattleSideEnum side)
		{
			IMissionSiegeWeaponsController weapons = ((side == BattleSideEnum.Defender) ? this._defenderSiegeWeaponsController : this._attackerSiegeWeaponsController);
			IEnumerable<DeploymentPoint> enumerable = base.Mission.ActiveMissionObjects.FindAllWithType<DeploymentPoint>();
			Func<DeploymentPoint, bool> <>9__0;
			Func<DeploymentPoint, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (DeploymentPoint dp) => dp.Side == side);
			}
			Func<Type, bool> <>9__1;
			foreach (DeploymentPoint deploymentPoint in enumerable.Where<DeploymentPoint>(func).ToArray<DeploymentPoint>())
			{
				IEnumerable<Type> deployableWeaponTypes = deploymentPoint.DeployableWeaponTypes;
				Func<Type, bool> func2;
				if ((func2 = <>9__1) == null)
				{
					func2 = (<>9__1 = (Type wt) => weapons.GetMaxDeployableWeaponCount(wt) > 0);
				}
				if (!deployableWeaponTypes.Any<Type>(func2))
				{
					foreach (SiegeWeapon siegeWeapon in deploymentPoint.DeployableWeapons.Select<SynchedMissionObject, SiegeWeapon>((SynchedMissionObject sw) => sw as SiegeWeapon))
					{
						siegeWeapon.SetDisabledSynched();
					}
					deploymentPoint.SetDisabledSynched();
				}
			}
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x000E88F8 File Offset: 0x000E6AF8
		public void UnHideDeploymentPoints(BattleSideEnum side)
		{
			IEnumerable<DeploymentPoint> enumerable = base.Mission.ActiveMissionObjects.FindAllWithType<DeploymentPoint>();
			Func<DeploymentPoint, bool> <>9__0;
			Func<DeploymentPoint, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (DeploymentPoint dp) => !dp.IsDisabled && dp.Side == side);
			}
			foreach (DeploymentPoint deploymentPoint in enumerable.Where<DeploymentPoint>(func))
			{
				deploymentPoint.Show();
			}
		}

		// Token: 0x0600380A RID: 14346 RVA: 0x000E8980 File Offset: 0x000E6B80
		public int GetDeployableWeaponCountOfPlayer(Type weapon)
		{
			return this.GetWeaponsControllerOfSide(this.IsPlayerAttacker ? BattleSideEnum.Attacker : BattleSideEnum.Defender).GetMaxDeployableWeaponCount(weapon) - this.PlayerDeploymentPoints.Count<DeploymentPoint>((DeploymentPoint dp) => dp.IsDeployed && MissionSiegeWeaponsController.GetWeaponType(dp.DeployedWeapon) == weapon);
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x000E89D0 File Offset: 0x000E6BD0
		public void AutoDeployTeamUsingTeamAI(Team team, bool autoAssignDetachments = true)
		{
			List<Formation> list = team.FormationsIncludingEmpty.ToList<Formation>();
			bool allowAiTicking = base.Mission.AllowAiTicking;
			bool forceTickOccasionally = base.Mission.ForceTickOccasionally;
			bool isTeleportingAgents = base.Mission.IsTeleportingAgents;
			base.Mission.AllowAiTicking = true;
			base.Mission.ForceTickOccasionally = true;
			base.Mission.IsTeleportingAgents = true;
			OrderController orderController = (team.IsPlayerTeam ? team.PlayerOrderController : team.MasterOrderController);
			orderController.SelectAllFormations(false);
			base.SetDefaultFormationOrders(orderController);
			team.ResetTactic();
			team.Tick(0f);
			foreach (Formation formation in list)
			{
				formation.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.ForceUpdateCachedAndFormationValues(false, false);
				}, null);
				formation.SetHasPendingUnitPositions(false);
			}
			orderController.ClearSelectedFormations();
			if (autoAssignDetachments)
			{
				this.AutoAssignDetachmentsForDeployment(team);
			}
			base.Mission.IsTeleportingAgents = isTeleportingAgents;
			base.Mission.ForceTickOccasionally = forceTickOccasionally;
			base.Mission.AllowAiTicking = allowAiTicking;
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x000E8B00 File Offset: 0x000E6D00
		public void AutoAssignDetachmentsForDeployment(Team team)
		{
			List<Formation> list = team.FormationsIncludingEmpty.ToList<Formation>();
			bool allowAiTicking = base.Mission.AllowAiTicking;
			bool isTeleportingAgents = base.Mission.IsTeleportingAgents;
			base.Mission.AllowAiTicking = true;
			base.Mission.IsTeleportingAgents = true;
			if (!team.DetachmentManager.Detachments.IsEmpty<ValueTuple<IDetachment, DetachmentData>>())
			{
				using (List<Formation>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							Formation formation2 = agent.Formation;
							if (formation2 == null)
							{
								return;
							}
							formation2.Team.DetachmentManager.TickAgent(agent);
						}, null);
					}
				}
				int num = 0;
				int num2 = 0;
				foreach (ValueTuple<IDetachment, DetachmentData> valueTuple in team.DetachmentManager.Detachments)
				{
					num += valueTuple.Item1.GetNumberOfUsableSlots();
				}
				foreach (Formation formation in team.FormationsIncludingEmpty)
				{
					num2 += formation.CountOfDetachableNonPlayerUnits;
				}
				for (int i = 0; i < MathF.Min(num, num2); i++)
				{
					team.DetachmentManager.TickDetachments();
				}
				using (List<Formation>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							if (agent.Detachment != null)
							{
								agent.ForceUpdateCachedAndFormationValues(false, false);
							}
						}, null);
					}
				}
			}
			base.Mission.IsTeleportingAgents = isTeleportingAgents;
			base.Mission.AllowAiTicking = allowAiTicking;
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x000E8CF4 File Offset: 0x000E6EF4
		private void OnFormationsChanged(Team team)
		{
			this.AutoAssignDetachmentsForDeployment(team);
		}

		// Token: 0x0600380E RID: 14350 RVA: 0x000E8D00 File Offset: 0x000E6F00
		protected bool Mission_IsFormationUnitPositionAvailable_AdditionalCondition(WorldPosition position, Team team)
		{
			if (team != null && team.Side == BattleSideEnum.Defender)
			{
				Scene scene = base.Mission.Scene;
				if (!this._defenderReferencePosition.IsValid)
				{
					WeakGameEntity weakGameEntity = scene.FindWeakEntityWithTag("defender_infantry");
					this._defenderReferencePosition = new WorldPosition(scene, UIntPtr.Zero, weakGameEntity.GlobalPosition, false);
				}
				return scene.DoesPathExistBetweenPositions(this._defenderReferencePosition, position);
			}
			return true;
		}

		// Token: 0x0600380F RID: 14351 RVA: 0x000E8D68 File Offset: 0x000E6F68
		private void OnDeploymentStateChange(DeploymentPoint deploymentPoint, SynchedMissionObject targetObject)
		{
			if (!deploymentPoint.IsDeployed && base.PlayerTeam.DetachmentManager.ContainsDetachment(deploymentPoint.DisbandedWeapon as IDetachment))
			{
				base.PlayerTeam.DetachmentManager.DestroyDetachment(deploymentPoint.DisbandedWeapon as IDetachment);
			}
			SiegeWeapon siegeWeapon;
			if ((siegeWeapon = targetObject as SiegeWeapon) != null)
			{
				IMissionSiegeWeaponsController weaponsControllerOfSide = this.GetWeaponsControllerOfSide(deploymentPoint.Side);
				if (deploymentPoint.IsDeployed)
				{
					weaponsControllerOfSide.OnWeaponDeployed(siegeWeapon);
					return;
				}
				weaponsControllerOfSide.OnWeaponUndeployed(siegeWeapon);
			}
		}

		// Token: 0x06003810 RID: 14352 RVA: 0x000E8DE3 File Offset: 0x000E6FE3
		private IMissionSiegeWeaponsController GetWeaponsControllerOfSide(BattleSideEnum side)
		{
			if (side != BattleSideEnum.Defender)
			{
				return this._attackerSiegeWeaponsController;
			}
			return this._defenderSiegeWeaponsController;
		}

		// Token: 0x06003811 RID: 14353 RVA: 0x000E8DF8 File Offset: 0x000E6FF8
		public Vec2 GetEstimatedAverageDefenderPosition()
		{
			WorldPosition worldPosition;
			Vec2 vec;
			base.Mission.GetFormationSpawnFrame(Mission.Current.DefenderTeam, FormationClass.Infantry, false, out worldPosition, out vec, true);
			return worldPosition.AsVec2;
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x000E8E28 File Offset: 0x000E7028
		[Conditional("DEBUG")]
		private void AssertSiegeWeapons(IEnumerable<DeploymentPoint> allDeploymentPoints)
		{
			HashSet<SynchedMissionObject> hashSet = new HashSet<SynchedMissionObject>();
			foreach (SynchedMissionObject synchedMissionObject in allDeploymentPoints.SelectMany<DeploymentPoint, SynchedMissionObject>((DeploymentPoint amo) => amo.DeployableWeapons))
			{
				if (!hashSet.Add(synchedMissionObject))
				{
					break;
				}
			}
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x000E8EA0 File Offset: 0x000E70A0
		public override void HandleGeneralsDeploymentFrames()
		{
			bool isTeleportingAgents = base.Mission.IsTeleportingAgents;
			base.Mission.IsTeleportingAgents = true;
			Agent initialPlayerAgent = base.Mission.InitialPlayerAgent;
			if (initialPlayerAgent != null)
			{
				Team team = initialPlayerAgent.Team;
				if (team != null)
				{
					if (base.Mission.DeploymentPlan.HasPlayerSpawnFrame(team.Side))
					{
						WorldPosition worldPosition;
						Vec2 vec;
						base.Mission.DeploymentPlan.GetPlayerSpawnFrame(team.Side, out worldPosition, out vec);
						if (worldPosition.GetNavMesh() != UIntPtr.Zero && worldPosition.IsValid)
						{
							initialPlayerAgent.TrySetFormationFrame(in worldPosition, in vec);
						}
					}
					else if (team.GeneralAgent == initialPlayerAgent)
					{
						WorldPosition worldPosition2;
						Vec2 vec2;
						base.Mission.GetFormationSpawnFrame(team, FormationClass.NumberOfRegularFormations, false, out worldPosition2, out vec2, true);
						if (worldPosition2.GetNavMesh() != UIntPtr.Zero && worldPosition2.IsValid)
						{
							initialPlayerAgent.TrySetFormationFrame(in worldPosition2, in vec2);
						}
					}
				}
			}
			base.Mission.IsTeleportingAgents = isTeleportingAgents;
		}

		// Token: 0x0400182C RID: 6188
		private IMissionSiegeWeaponsController _defenderSiegeWeaponsController;

		// Token: 0x0400182D RID: 6189
		private IMissionSiegeWeaponsController _attackerSiegeWeaponsController;

		// Token: 0x0400182E RID: 6190
		private WorldPosition _defenderReferencePosition;
	}
}
