using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A1 RID: 673
	public class SiegeDeploymentMissionController : DeploymentMissionController
	{
		// Token: 0x06002560 RID: 9568 RVA: 0x0008832F File Offset: 0x0008652F
		public SiegeDeploymentMissionController(bool isPlayerAttacker)
			: base(isPlayerAttacker)
		{
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x00088338 File Offset: 0x00086538
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._siegeDeploymentHandler = base.Mission.GetMissionBehavior<SiegeDeploymentHandler>();
			this.MissionAgentSpawnLogic = base.Mission.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x00088364 File Offset: 0x00086564
		public List<ItemObject> GetSiegeMissiles()
		{
			List<ItemObject> list = new List<ItemObject>();
			foreach (WeakGameEntity weakGameEntity in Mission.Current.GetActiveEntitiesWithScriptComponentOfType<RangedSiegeWeapon>())
			{
				RangedSiegeWeapon firstScriptOfType = weakGameEntity.GetFirstScriptOfType<RangedSiegeWeapon>();
				if (!string.IsNullOrEmpty(firstScriptOfType.MissileItemID))
				{
					ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MissileItemID);
					if (!list.Contains(@object))
					{
						list.Add(@object);
					}
				}
				foreach (ItemObject itemObject in new List<ItemObject>
				{
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MultipleProjectileId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MultipleProjectileFlyingId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MultipleFireProjectileId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MultipleFireProjectileFlyingId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.SingleProjectileId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.SingleProjectileFlyingId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.SingleFireProjectileId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.SingleFireProjectileFlyingId)
				})
				{
					if (!list.Contains(itemObject))
					{
						list.Add(itemObject);
					}
				}
			}
			foreach (WeakGameEntity weakGameEntity2 in Mission.Current.GetActiveEntitiesWithScriptComponentOfType<StonePile>())
			{
				StonePile firstScriptOfType2 = weakGameEntity2.GetFirstScriptOfType<StonePile>();
				if (!string.IsNullOrEmpty(firstScriptOfType2.GivenItemID))
				{
					ItemObject object2 = MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType2.GivenItemID);
					if (!list.Contains(object2))
					{
						list.Add(object2);
					}
				}
			}
			return list;
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x0008858C File Offset: 0x0008678C
		protected override void OnAfterStart()
		{
			this._siegeDeploymentHandler.InitializeDeploymentPoints();
			for (int i = 0; i < 2; i++)
			{
				this.MissionAgentSpawnLogic.SetSpawnTroops((BattleSideEnum)i, false, false);
			}
			this.MissionAgentSpawnLogic.SetReinforcementsSpawnEnabled(false, true);
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000885CC File Offset: 0x000867CC
		protected override void OnSetupTeamsOfSide(BattleSideEnum battleSide)
		{
			foreach (Team team in base.Mission.Teams)
			{
				if (team.Side == battleSide && team.GeneralAgent != null && team.GeneralAgent != base.Mission.InitialPlayerAgent)
				{
					team.GeneralAgent.SetDetachableFromFormation(false);
				}
			}
			Team team2 = ((battleSide == BattleSideEnum.Attacker) ? base.Mission.AttackerTeam : base.Mission.DefenderTeam);
			if (team2 == base.Mission.PlayerTeam)
			{
				this._siegeDeploymentHandler.RemoveUnavailableDeploymentPoints(battleSide);
				this._siegeDeploymentHandler.UnHideDeploymentPoints(battleSide);
				this._siegeDeploymentHandler.DeployAllSiegeWeaponsOfPlayer();
			}
			else
			{
				this._siegeDeploymentHandler.DeployAllSiegeWeaponsOfAi();
			}
			this.MissionAgentSpawnLogic.SetSpawnTroops(battleSide, true, true);
			foreach (WeakGameEntity weakGameEntity in base.Mission.GetActiveEntitiesWithScriptComponentOfType<SiegeWeapon>())
			{
				SiegeWeapon firstScriptOfType = weakGameEntity.GetFirstScriptOfType<SiegeWeapon>();
				if (firstScriptOfType != null && firstScriptOfType.GetSide() == battleSide)
				{
					firstScriptOfType.TickAuxForInit();
				}
			}
			base.SetupAgentAIStatesForSide(battleSide);
			if (team2 == base.Mission.PlayerTeam)
			{
				foreach (Formation formation in team2.FormationsIncludingEmpty)
				{
					formation.SetControlledByAI(true, false);
				}
			}
			this.MissionAgentSpawnLogic.OnSideDeploymentOver(team2.Side);
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x00088778 File Offset: 0x00086978
		protected override void OnSetupTeamsFinished()
		{
			this._siegeDeploymentHandler.HandleGeneralsDeploymentFrames();
			base.Mission.IsTeleportingAgents = true;
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x00088794 File Offset: 0x00086994
		protected override void BeforeDeploymentFinished()
		{
			BattleSideEnum side = base.Mission.PlayerTeam.Side;
			this._siegeDeploymentHandler.RemoveDeploymentPoints(side);
			foreach (SiegeLadder siegeLadder in (from sl in Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeLadder>()
				where !sl.GameEntity.IsVisibleIncludeParents()
				select sl).ToList<SiegeLadder>())
			{
				siegeLadder.SetDisabledSynched();
			}
			foreach (Team team in base.Mission.Teams)
			{
				if (team.GeneralAgent != null && team.GeneralAgent != base.Mission.InitialPlayerAgent)
				{
					team.GeneralAgent.SetDetachableFromFormation(true);
				}
			}
			base.Mission.IsTeleportingAgents = false;
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000888A8 File Offset: 0x00086AA8
		protected override void AfterDeploymentFinished()
		{
			this.MissionAgentSpawnLogic.SetReinforcementsSpawnEnabled(true, true);
			base.Mission.RemoveMissionBehavior(this._siegeDeploymentHandler);
		}

		// Token: 0x04000E82 RID: 3714
		protected DefaultBattleMissionAgentSpawnLogic MissionAgentSpawnLogic;

		// Token: 0x04000E83 RID: 3715
		private SiegeDeploymentHandler _siegeDeploymentHandler;
	}
}
