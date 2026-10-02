using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200011A RID: 282
	public class BehaviorDestroySiegeWeapons : BehaviorComponent
	{
		// Token: 0x06000E40 RID: 3648 RVA: 0x0001EE50 File Offset: 0x0001D050
		private void DetermineTargetWeapons()
		{
			this._targetWeapons = this._allWeapons.Where<SiegeWeapon>((SiegeWeapon w) => w is IPrimarySiegeWeapon && (w as IPrimarySiegeWeapon).WeaponSide == this._behaviorSide && w.IsDestructible && !w.IsDestroyed && !w.IsDisabled).ToList<SiegeWeapon>();
			if (this._targetWeapons.IsEmpty<SiegeWeapon>())
			{
				this._targetWeapons = this._allWeapons.Where<SiegeWeapon>((SiegeWeapon w) => !(w is IPrimarySiegeWeapon) && w.IsDestructible && !w.IsDestroyed && !w.IsDisabled).ToList<SiegeWeapon>();
				this._isTargetPrimaryWeapon = false;
				return;
			}
			this._isTargetPrimaryWeapon = true;
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0001EED0 File Offset: 0x0001D0D0
		public BehaviorDestroySiegeWeapons(Formation formation)
			: base(formation)
		{
			base.BehaviorCoherence = 0.2f;
			this._behaviorSide = formation.AI.Side;
			this._allWeapons = (from sw in Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeWeapon>()
				where sw.Side != formation.Team.Side
				select sw).ToList<SiegeWeapon>();
			this.DetermineTargetWeapons();
			base.CurrentOrder = MovementOrder.MovementOrderCharge;
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0001EF54 File Offset: 0x0001D154
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			TextObject textObject = GameTexts.FindText("str_formation_ai_side_strings", base.Formation.AI.Side.ToString());
			behaviorString.SetTextVariable("SIDE_STRING", textObject);
			behaviorString.SetTextVariable("IS_GENERAL_SIDE", "0");
			return behaviorString;
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0001EFAE File Offset: 0x0001D1AE
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this.DetermineTargetWeapons();
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0001EFBC File Offset: 0x0001D1BC
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			this._targetWeapons.RemoveAll((SiegeWeapon tw) => tw.IsDestroyed);
			if (this._targetWeapons.Count == 0)
			{
				this.DetermineTargetWeapons();
			}
			if (base.Formation.AI.ActiveBehavior == this)
			{
				if (this._targetWeapons.Count == 0)
				{
					MovementOrder currentOrder = base.CurrentOrder;
					if ((in currentOrder) != MovementOrder.MovementOrderCharge)
					{
						base.CurrentOrder = MovementOrder.MovementOrderCharge;
					}
					this._isTargetPrimaryWeapon = false;
				}
				else
				{
					SiegeWeapon siegeWeapon = this._targetWeapons.MinBy<SiegeWeapon, float>((SiegeWeapon tw) => base.Formation.CachedAveragePosition.DistanceSquared(tw.GameEntity.GlobalPosition.AsVec2));
					if (base.CurrentOrder.OrderEnum != MovementOrder.MovementOrderEnum.AttackEntity || this.LastTargetWeapon != siegeWeapon)
					{
						this.LastTargetWeapon = siegeWeapon;
						base.CurrentOrder = MovementOrder.MovementOrderAttackEntity(GameEntity.CreateFromWeakEntity(this.LastTargetWeapon.GameEntity), true);
					}
				}
				base.Formation.SetMovementOrder(base.CurrentOrder);
			}
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0001F0BC File Offset: 0x0001D2BC
		protected override void OnBehaviorActivatedAux()
		{
			this.DetermineTargetWeapons();
			base.Formation.SetArrangementOrder((base.Formation.QuerySystem.IsCavalryFormation || base.Formation.QuerySystem.IsRangedCavalryFormation) ? ArrangementOrder.ArrangementOrderSkein : ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0001F13B File Offset: 0x0001D33B
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0001F142 File Offset: 0x0001D342
		protected override float GetAiWeight()
		{
			if (this._targetWeapons.IsEmpty<SiegeWeapon>())
			{
				return 0f;
			}
			if (!this._isTargetPrimaryWeapon)
			{
				return 0.7f;
			}
			return 1f;
		}

		// Token: 0x04000362 RID: 866
		private readonly List<SiegeWeapon> _allWeapons;

		// Token: 0x04000363 RID: 867
		private List<SiegeWeapon> _targetWeapons;

		// Token: 0x04000364 RID: 868
		public SiegeWeapon LastTargetWeapon;

		// Token: 0x04000365 RID: 869
		private bool _isTargetPrimaryWeapon;
	}
}
