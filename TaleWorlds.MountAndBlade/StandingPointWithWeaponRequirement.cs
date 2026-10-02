using System;
using System.Linq;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000366 RID: 870
	public class StandingPointWithWeaponRequirement : StandingPoint
	{
		// Token: 0x0600320E RID: 12814 RVA: 0x000CC122 File Offset: 0x000CA322
		public StandingPointWithWeaponRequirement()
		{
			this.AutoSheathWeapons = false;
			this._requiredWeaponClasses = new WeaponClass[0];
			this._hasAlternative = base.HasAlternative();
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x000CC149 File Offset: 0x000CA349
		protected internal override void OnInit()
		{
			base.OnInit();
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x000CC151 File Offset: 0x000CA351
		public void InitRequiredWeaponClasses(WeaponClass[] requiredWeaponClasses)
		{
			this._requiredWeaponClasses = requiredWeaponClasses;
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x000CC15A File Offset: 0x000CA35A
		public void InitRequiredWeapon(ItemObject weapon)
		{
			this._requiredWeapon = weapon;
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x000CC163 File Offset: 0x000CA363
		public void InitGivenWeapon(ItemObject weapon)
		{
			this._givenWeapon = weapon;
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x000CC16C File Offset: 0x000CA36C
		public override bool IsDisabledForAgent(Agent agent)
		{
			EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
			if (this._requiredWeapon != null)
			{
				if (primaryWieldedItemIndex != EquipmentIndex.None && agent.Equipment[primaryWieldedItemIndex].Item == this._requiredWeapon)
				{
					return base.IsDisabledForAgent(agent);
				}
			}
			else if (this._givenWeapon != null)
			{
				if (primaryWieldedItemIndex == EquipmentIndex.None || agent.Equipment[primaryWieldedItemIndex].Item != this._givenWeapon)
				{
					return base.IsDisabledForAgent(agent);
				}
			}
			else if (!this._requiredWeaponClasses.IsEmpty<WeaponClass>())
			{
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
				{
					if (!agent.Equipment[equipmentIndex].IsEmpty && this._requiredWeaponClasses.Contains(agent.Equipment[equipmentIndex].CurrentUsageItem.WeaponClass) && (!agent.Equipment[equipmentIndex].CurrentUsageItem.IsConsumable || agent.Equipment[equipmentIndex].Amount < agent.Equipment[equipmentIndex].ModifiedMaxAmount || equipmentIndex == EquipmentIndex.ExtraWeaponSlot))
					{
						return base.IsDisabledForAgent(agent);
					}
				}
			}
			return true;
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x000CC299 File Offset: 0x000CA499
		public void SetHasAlternative(bool hasAlternative)
		{
			this._hasAlternative = hasAlternative;
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x000CC2A2 File Offset: 0x000CA4A2
		public override bool HasAlternative()
		{
			return this._hasAlternative;
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x000CC2AA File Offset: 0x000CA4AA
		public void SetUsingBattleSide(BattleSideEnum side)
		{
			this.StandingPointSide = side;
		}

		// Token: 0x0400151B RID: 5403
		private ItemObject _requiredWeapon;

		// Token: 0x0400151C RID: 5404
		private ItemObject _givenWeapon;

		// Token: 0x0400151D RID: 5405
		private WeaponClass[] _requiredWeaponClasses;

		// Token: 0x0400151E RID: 5406
		private bool _hasAlternative;
	}
}
