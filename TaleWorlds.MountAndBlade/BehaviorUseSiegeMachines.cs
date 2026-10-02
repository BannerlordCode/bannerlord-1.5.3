using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200013C RID: 316
	public class BehaviorUseSiegeMachines : BehaviorComponent
	{
		// Token: 0x06000F3B RID: 3899 RVA: 0x00028228 File Offset: 0x00026428
		public BehaviorUseSiegeMachines(Formation formation)
			: base(formation)
		{
			this._behaviorSide = formation.AI.Side;
			this._primarySiegeWeapons = new List<SiegeWeapon>();
			foreach (MissionObject missionObject in Mission.Current.ActiveMissionObjects)
			{
				IPrimarySiegeWeapon primarySiegeWeapon;
				if ((primarySiegeWeapon = missionObject as IPrimarySiegeWeapon) != null && primarySiegeWeapon.WeaponSide == this._behaviorSide)
				{
					this._primarySiegeWeapons.Add(missionObject as SiegeWeapon);
				}
			}
			this._teamAISiegeComponent = (TeamAISiegeComponent)formation.Team.TeamAI;
			base.BehaviorCoherence = 0f;
			this._stopOrder = MovementOrder.MovementOrderStop;
			this.RecreateFollowEntityOrder();
			if (this._followEntityOrder.OrderEnum != MovementOrder.MovementOrderEnum.Invalid)
			{
				this._behaviorState = BehaviorUseSiegeMachines.BehaviorState.Follow;
				base.CurrentOrder = this._followEntityOrder;
				return;
			}
			this._behaviorState = BehaviorUseSiegeMachines.BehaviorState.Stop;
			base.CurrentOrder = this._stopOrder;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0002832C File Offset: 0x0002652C
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			TextObject textObject = GameTexts.FindText("str_formation_ai_side_strings", base.Formation.AI.Side.ToString());
			behaviorString.SetTextVariable("SIDE_STRING", textObject);
			behaviorString.SetTextVariable("IS_GENERAL_SIDE", "0");
			return behaviorString;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00028388 File Offset: 0x00026588
		private void RecreateFollowEntityOrder()
		{
			this._followEntityOrder = MovementOrder.MovementOrderStop;
			SiegeWeapon siegeWeapon = this._primarySiegeWeapons.FirstOrDefault<SiegeWeapon>(delegate(SiegeWeapon psw)
			{
				IPrimarySiegeWeapon primarySiegeWeapon;
				return !psw.IsDeactivated && (primarySiegeWeapon = psw as IPrimarySiegeWeapon) != null && !primarySiegeWeapon.HasCompletedAction();
			});
			this._followedEntity = ((siegeWeapon != null) ? siegeWeapon.WaitEntity : null);
			if (this._followedEntity != null)
			{
				this._followEntityOrder = MovementOrder.MovementOrderFollowEntity(this._followedEntity);
			}
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x000283FC File Offset: 0x000265FC
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this._primarySiegeWeapons.Clear();
			foreach (MissionObject missionObject in Mission.Current.ActiveMissionObjects)
			{
				IPrimarySiegeWeapon primarySiegeWeapon;
				if ((primarySiegeWeapon = missionObject as IPrimarySiegeWeapon) != null && primarySiegeWeapon.WeaponSide == this._behaviorSide && !((SiegeWeapon)missionObject).IsDeactivated)
				{
					this._primarySiegeWeapons.Add(missionObject as SiegeWeapon);
				}
			}
			this.RecreateFollowEntityOrder();
			this._behaviorState = BehaviorUseSiegeMachines.BehaviorState.Unset;
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x000284A0 File Offset: 0x000266A0
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			bool flag = false;
			for (int i = this._primarySiegeWeapons.Count - 1; i >= 0; i--)
			{
				SiegeWeapon siegeWeapon = this._primarySiegeWeapons[i];
				if (siegeWeapon.IsDestroyed || siegeWeapon.IsDeactivated)
				{
					this._primarySiegeWeapons.RemoveAt(i);
					flag = true;
				}
			}
			if (flag)
			{
				this.RecreateFollowEntityOrder();
			}
			int num = 0;
			SiegeTower siegeTower = null;
			foreach (SiegeWeapon siegeWeapon2 in this._primarySiegeWeapons)
			{
				if (!((IPrimarySiegeWeapon)siegeWeapon2).HasCompletedAction())
				{
					num++;
					SiegeTower siegeTower2;
					if ((siegeTower2 = siegeWeapon2 as SiegeTower) != null)
					{
						siegeTower = siegeTower2;
					}
				}
			}
			if (num == 0)
			{
				base.CurrentOrder = this._stopOrder;
				return;
			}
			if (this._behaviorState == BehaviorUseSiegeMachines.BehaviorState.Follow)
			{
				if (this._followEntityOrder.OrderEnum == MovementOrder.MovementOrderEnum.Stop)
				{
					this.RecreateFollowEntityOrder();
				}
				base.CurrentOrder = this._followEntityOrder;
			}
			BehaviorUseSiegeMachines.BehaviorState behaviorState = ((siegeTower != null && siegeTower.HasArrivedAtTarget) ? BehaviorUseSiegeMachines.BehaviorState.ClimbSiegeTower : ((this._followEntityOrder.OrderEnum != MovementOrder.MovementOrderEnum.Invalid) ? BehaviorUseSiegeMachines.BehaviorState.Follow : BehaviorUseSiegeMachines.BehaviorState.Stop));
			if (behaviorState != this._behaviorState)
			{
				if (behaviorState == BehaviorUseSiegeMachines.BehaviorState.Follow)
				{
					base.CurrentOrder = this._followEntityOrder;
				}
				else if (behaviorState == BehaviorUseSiegeMachines.BehaviorState.ClimbSiegeTower)
				{
					this.RecreateFollowEntityOrder();
					base.CurrentOrder = this._followEntityOrder;
				}
				else
				{
					base.CurrentOrder = this._stopOrder;
				}
				this._behaviorState = behaviorState;
				bool flag2 = this._behaviorState == BehaviorUseSiegeMachines.BehaviorState.ClimbSiegeTower;
				if (!flag2)
				{
					using (List<SiegeWeapon>.Enumerator enumerator = this._primarySiegeWeapons.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SiegeLadder siegeLadder;
							if ((siegeLadder = enumerator.Current as SiegeLadder) != null && !siegeLadder.IsDisabled)
							{
								flag2 = true;
								break;
							}
						}
					}
				}
				if (flag2)
				{
					base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
				}
				else if (base.Formation.QuerySystem.IsRangedFormation)
				{
					base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
				}
				else
				{
					base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderShieldWall);
				}
			}
			if (this._followedEntity != null && (this._behaviorState == BehaviorUseSiegeMachines.BehaviorState.Follow || this._behaviorState == BehaviorUseSiegeMachines.BehaviorState.ClimbSiegeTower))
			{
				base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtDirection(this._followedEntity.GetGlobalFrame().rotation.f.AsVec2.Normalized()));
			}
			else
			{
				base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			}
			if (base.Formation.AI.ActiveBehavior == this)
			{
				foreach (SiegeWeapon siegeWeapon3 in this._primarySiegeWeapons)
				{
					if (!((IPrimarySiegeWeapon)siegeWeapon3).HasCompletedAction())
					{
						if (!siegeWeapon3.IsUsedByFormation(base.Formation))
						{
							base.Formation.StartUsingMachine(siegeWeapon3, false);
						}
						for (int j = siegeWeapon3.UserFormations.Count - 1; j >= 0; j--)
						{
							Formation formation = siegeWeapon3.UserFormations[j];
							if (formation != base.Formation && formation.IsAIControlled && (formation.AI.Side != this._behaviorSide || !(formation.AI.ActiveBehavior is BehaviorUseSiegeMachines)) && formation.Team == base.Formation.Team)
							{
								formation.StopUsingMachine(siegeWeapon3, false);
							}
						}
					}
				}
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x00028848 File Offset: 0x00026A48
		protected override void OnBehaviorActivatedAux()
		{
			base.Formation.SetArrangementOrder(base.Formation.QuerySystem.IsRangedFormation ? ArrangementOrder.ArrangementOrderScatter : ArrangementOrder.ArrangementOrderShieldWall);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x000288AF File Offset: 0x00026AAF
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x000288B8 File Offset: 0x00026AB8
		protected override float GetAiWeight()
		{
			float num = 0f;
			if (this._teamAISiegeComponent != null && this._primarySiegeWeapons.Count > 0)
			{
				if (this._primarySiegeWeapons.All<SiegeWeapon>((SiegeWeapon psw) => !(psw as IPrimarySiegeWeapon).HasCompletedAction()))
				{
					num = ((!this._teamAISiegeComponent.IsCastleBreached()) ? 0.75f : 0.25f);
				}
			}
			return num;
		}

		// Token: 0x040003B4 RID: 948
		private List<SiegeWeapon> _primarySiegeWeapons;

		// Token: 0x040003B5 RID: 949
		private TeamAISiegeComponent _teamAISiegeComponent;

		// Token: 0x040003B6 RID: 950
		private MovementOrder _followEntityOrder;

		// Token: 0x040003B7 RID: 951
		private GameEntity _followedEntity;

		// Token: 0x040003B8 RID: 952
		private MovementOrder _stopOrder;

		// Token: 0x040003B9 RID: 953
		private BehaviorUseSiegeMachines.BehaviorState _behaviorState;

		// Token: 0x02000458 RID: 1112
		private enum BehaviorState
		{
			// Token: 0x04001A1F RID: 6687
			Unset,
			// Token: 0x04001A20 RID: 6688
			Follow,
			// Token: 0x04001A21 RID: 6689
			ClimbSiegeTower,
			// Token: 0x04001A22 RID: 6690
			Stop
		}
	}
}
