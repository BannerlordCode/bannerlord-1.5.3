using System;
using System.Threading;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200025D RID: 605
	public class MissionEquipment
	{
		// Token: 0x0600225A RID: 8794 RVA: 0x00078DE8 File Offset: 0x00076FE8
		public MissionEquipment()
		{
			this._weaponSlots = new MissionWeapon[5];
			this._cache = default(MissionEquipment.MissionEquipmentCache);
			this._cache.Initialize();
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x00078E20 File Offset: 0x00077020
		public MissionEquipment(Equipment spawnEquipment, Banner banner)
			: this()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				this._weaponSlots[(int)equipmentIndex] = new MissionWeapon(spawnEquipment[equipmentIndex].Item, spawnEquipment[equipmentIndex].ItemModifier, banner);
			}
		}

		// Token: 0x170006E5 RID: 1765
		public MissionWeapon this[int index]
		{
			get
			{
				return this._weaponSlots[index];
			}
			set
			{
				this._weaponSlots[index] = value;
				this._cache.InvalidateOnWeaponSlotUpdated();
				Action onWeaponSlotUpdated = this.OnWeaponSlotUpdated;
				if (onWeaponSlotUpdated == null)
				{
					return;
				}
				onWeaponSlotUpdated();
			}
		}

		// Token: 0x170006E6 RID: 1766
		public MissionWeapon this[EquipmentIndex index]
		{
			get
			{
				return this._weaponSlots[(int)index];
			}
			set
			{
				this[(int)index] = value;
			}
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x00078EC0 File Offset: 0x000770C0
		public void FillFrom(MissionEquipment sourceEquipment)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				this[equipmentIndex] = new MissionWeapon(sourceEquipment[equipmentIndex].Item, sourceEquipment[equipmentIndex].ItemModifier, null);
			}
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x00078F04 File Offset: 0x00077104
		public void FillFrom(Equipment sourceEquipment, Banner banner)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				this[equipmentIndex] = new MissionWeapon(sourceEquipment[equipmentIndex].Item, sourceEquipment[equipmentIndex].ItemModifier, banner);
			}
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x00078F48 File Offset: 0x00077148
		private float CalculateGetTotalWeightOfWeapons()
		{
			float num = 0f;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				MissionWeapon missionWeapon = this[equipmentIndex];
				if (!missionWeapon.IsEmpty)
				{
					if (missionWeapon.CurrentUsageItem.IsShield)
					{
						if (missionWeapon.HitPoints > 0)
						{
							num += missionWeapon.GetWeight();
						}
					}
					else
					{
						num += missionWeapon.GetWeight();
					}
				}
			}
			return num;
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x00078FA8 File Offset: 0x000771A8
		public float GetTotalWeightOfWeapons()
		{
			this._cacheLock.EnterReadLock();
			try
			{
				if (this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedFloat.TotalWeightOfWeapons))
				{
					return this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedFloat.TotalWeightOfWeapons);
				}
			}
			finally
			{
				this._cacheLock.ExitReadLock();
			}
			this._cacheLock.EnterWriteLock();
			float value;
			try
			{
				if (!this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedFloat.TotalWeightOfWeapons))
				{
					this._cache.UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedFloat.TotalWeightOfWeapons, this.CalculateGetTotalWeightOfWeapons());
				}
				value = this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedFloat.TotalWeightOfWeapons);
			}
			finally
			{
				this._cacheLock.ExitWriteLock();
			}
			return value;
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x00079050 File Offset: 0x00077250
		public static EquipmentIndex SelectWeaponPickUpSlot(Agent agentPickingUp, MissionWeapon weaponBeingPickedUp, bool isStuckMissile)
		{
			EquipmentIndex equipmentIndex = EquipmentIndex.None;
			if (weaponBeingPickedUp.Item.ItemFlags.HasAnyFlag(ItemFlags.DropOnWeaponChange | ItemFlags.DropOnAnyAction))
			{
				equipmentIndex = EquipmentIndex.ExtraWeaponSlot;
			}
			else
			{
				bool flag = weaponBeingPickedUp.Item.ItemFlags.HasAnyFlag(ItemFlags.HeldInOffHand);
				EquipmentIndex equipmentIndex2 = (flag ? agentPickingUp.GetOffhandWieldedItemIndex() : agentPickingUp.GetPrimaryWieldedItemIndex());
				MissionWeapon missionWeapon = ((equipmentIndex2 != EquipmentIndex.None) ? agentPickingUp.Equipment[equipmentIndex2] : MissionWeapon.Invalid);
				if (isStuckMissile)
				{
					bool flag2 = false;
					bool flag3 = false;
					bool isConsumable = weaponBeingPickedUp.Item.PrimaryWeapon.IsConsumable;
					if (isConsumable)
					{
						flag2 = !missionWeapon.IsEmpty && missionWeapon.IsEqualTo(weaponBeingPickedUp) && missionWeapon.HasEnoughSpaceForAmount((int)weaponBeingPickedUp.Amount);
						flag3 = !missionWeapon.IsEmpty && missionWeapon.IsSameType(weaponBeingPickedUp) && missionWeapon.HasEnoughSpaceForAmount((int)weaponBeingPickedUp.Amount);
					}
					EquipmentIndex equipmentIndex3 = EquipmentIndex.None;
					EquipmentIndex equipmentIndex4 = EquipmentIndex.None;
					EquipmentIndex equipmentIndex5 = EquipmentIndex.None;
					EquipmentIndex equipmentIndex6 = EquipmentIndex.WeaponItemBeginSlot;
					while (equipmentIndex6 < EquipmentIndex.ExtraWeaponSlot)
					{
						if (!isConsumable)
						{
							goto IL_019F;
						}
						if (equipmentIndex4 != EquipmentIndex.None && !agentPickingUp.Equipment[equipmentIndex6].IsEmpty && agentPickingUp.Equipment[equipmentIndex6].IsEqualTo(weaponBeingPickedUp) && agentPickingUp.Equipment[equipmentIndex6].HasEnoughSpaceForAmount((int)weaponBeingPickedUp.Amount))
						{
							equipmentIndex4 = equipmentIndex6;
						}
						else
						{
							if (equipmentIndex5 != EquipmentIndex.None || agentPickingUp.Equipment[equipmentIndex6].IsEmpty || !agentPickingUp.Equipment[equipmentIndex6].IsSameType(weaponBeingPickedUp) || !agentPickingUp.Equipment[equipmentIndex6].HasEnoughSpaceForAmount((int)weaponBeingPickedUp.Amount))
							{
								goto IL_019F;
							}
							equipmentIndex5 = equipmentIndex6;
						}
						IL_01C0:
						equipmentIndex6++;
						continue;
						IL_019F:
						if (equipmentIndex3 == EquipmentIndex.None && agentPickingUp.Equipment[equipmentIndex6].IsEmpty)
						{
							equipmentIndex3 = equipmentIndex6;
							goto IL_01C0;
						}
						goto IL_01C0;
					}
					if (flag2)
					{
						equipmentIndex = equipmentIndex2;
					}
					else if (equipmentIndex4 != EquipmentIndex.None)
					{
						equipmentIndex = equipmentIndex5;
					}
					else if (flag3)
					{
						equipmentIndex = equipmentIndex2;
					}
					else if (equipmentIndex5 != EquipmentIndex.None)
					{
						equipmentIndex = equipmentIndex5;
					}
					else if (equipmentIndex3 != EquipmentIndex.None)
					{
						equipmentIndex = equipmentIndex3;
					}
				}
				else
				{
					bool isConsumable2 = weaponBeingPickedUp.Item.PrimaryWeapon.IsConsumable;
					if (isConsumable2 && weaponBeingPickedUp.Amount == 0)
					{
						equipmentIndex = EquipmentIndex.None;
					}
					else
					{
						if (flag && equipmentIndex2 != EquipmentIndex.None)
						{
							for (int i = 0; i < 4; i++)
							{
								if (i != (int)equipmentIndex2 && !agentPickingUp.Equipment[i].IsEmpty && agentPickingUp.Equipment[i].Item.ItemFlags.HasAnyFlag(ItemFlags.HeldInOffHand))
								{
									equipmentIndex = equipmentIndex2;
									break;
								}
							}
						}
						if (equipmentIndex == EquipmentIndex.None && isConsumable2)
						{
							for (EquipmentIndex equipmentIndex7 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex7 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex7++)
							{
								if (!agentPickingUp.Equipment[equipmentIndex7].IsEmpty && agentPickingUp.Equipment[equipmentIndex7].IsSameType(weaponBeingPickedUp) && agentPickingUp.Equipment[equipmentIndex7].Amount < agentPickingUp.Equipment[equipmentIndex7].ModifiedMaxAmount)
								{
									equipmentIndex = equipmentIndex7;
									break;
								}
							}
						}
						if (equipmentIndex == EquipmentIndex.None)
						{
							for (EquipmentIndex equipmentIndex8 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex8 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex8++)
							{
								if (agentPickingUp.Equipment[equipmentIndex8].IsEmpty)
								{
									equipmentIndex = equipmentIndex8;
									break;
								}
							}
						}
						if (equipmentIndex == EquipmentIndex.None)
						{
							for (EquipmentIndex equipmentIndex9 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex9 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex9++)
							{
								if (!agentPickingUp.Equipment[equipmentIndex9].IsEmpty && agentPickingUp.Equipment[equipmentIndex9].IsAnyConsumable() && agentPickingUp.Equipment[equipmentIndex9].Amount == 0)
								{
									equipmentIndex = equipmentIndex9;
									break;
								}
							}
						}
						if (equipmentIndex == EquipmentIndex.None && !missionWeapon.IsEmpty)
						{
							equipmentIndex = equipmentIndex2;
						}
						if (equipmentIndex == EquipmentIndex.None)
						{
							equipmentIndex = EquipmentIndex.WeaponItemBeginSlot;
						}
					}
				}
			}
			return equipmentIndex;
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x00079418 File Offset: 0x00077618
		public bool HasAmmo(EquipmentIndex equipmentIndex, out int rangedUsageIndex, out bool hasLoadedAmmo, out bool noAmmoInThisSlot)
		{
			hasLoadedAmmo = false;
			noAmmoInThisSlot = false;
			MissionWeapon missionWeapon = this._weaponSlots[(int)equipmentIndex];
			rangedUsageIndex = missionWeapon.GetRangedUsageIndex();
			if (rangedUsageIndex >= 0)
			{
				if (missionWeapon.Ammo > 0)
				{
					hasLoadedAmmo = true;
					return true;
				}
				noAmmoInThisSlot = missionWeapon.IsAnyConsumable() && missionWeapon.Amount == 0;
				for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 < EquipmentIndex.NumAllWeaponSlots; equipmentIndex2++)
				{
					MissionWeapon missionWeapon2 = this[(int)equipmentIndex2];
					if (!missionWeapon2.IsEmpty && missionWeapon2.HasAnyUsageWithWeaponClass(missionWeapon.GetWeaponComponentDataForUsage(rangedUsageIndex).AmmoClass) && this[(int)equipmentIndex2].ModifiedMaxAmount > 1 && missionWeapon2.Amount > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x000794C4 File Offset: 0x000776C4
		public int GetAmmoAmount(EquipmentIndex weaponIndex)
		{
			if (this[weaponIndex].IsAnyConsumable() && this[weaponIndex].ModifiedMaxAmount <= 1)
			{
				return (int)this[weaponIndex].ModifiedMaxAmount;
			}
			int num = 0;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (!this[(int)equipmentIndex].IsEmpty && this[(int)equipmentIndex].CurrentUsageItem.WeaponClass == this[weaponIndex].CurrentUsageItem.AmmoClass && this[(int)equipmentIndex].ModifiedMaxAmount > 1)
				{
					num += (int)this[(int)equipmentIndex].Amount;
				}
			}
			return num;
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x00079574 File Offset: 0x00077774
		public int GetMaxAmmo(EquipmentIndex weaponIndex)
		{
			if (this[weaponIndex].IsAnyConsumable() && this[weaponIndex].ModifiedMaxAmount <= 1)
			{
				return (int)this[weaponIndex].ModifiedMaxAmount;
			}
			int num = 0;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (!this[(int)equipmentIndex].IsEmpty && this[(int)equipmentIndex].CurrentUsageItem.WeaponClass == this[weaponIndex].CurrentUsageItem.AmmoClass && this[(int)equipmentIndex].ModifiedMaxAmount > 1)
				{
					num += (int)this[(int)equipmentIndex].ModifiedMaxAmount;
				}
			}
			return num;
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x00079624 File Offset: 0x00077824
		public void GetAmmoCountAndIndexOfType(ItemObject.ItemTypeEnum itemType, out int ammoCount, out EquipmentIndex eIndex, EquipmentIndex equippedIndex = EquipmentIndex.None)
		{
			ItemObject.ItemTypeEnum ammoTypeForItemType = ItemObject.GetAmmoTypeForItemType(itemType);
			ItemObject itemObject;
			if (equippedIndex != EquipmentIndex.None)
			{
				itemObject = this[equippedIndex].Item;
				ammoCount = 0;
			}
			else
			{
				itemObject = null;
				ammoCount = -1;
			}
			eIndex = equippedIndex;
			if (ammoTypeForItemType != ItemObject.ItemTypeEnum.Invalid)
			{
				for (EquipmentIndex equipmentIndex = EquipmentIndex.Weapon3; equipmentIndex >= EquipmentIndex.WeaponItemBeginSlot; equipmentIndex--)
				{
					if (!this[equipmentIndex].IsEmpty && this[equipmentIndex].Item.Type == ammoTypeForItemType)
					{
						int amount = (int)this[equipmentIndex].Amount;
						if (amount > 0)
						{
							if (itemObject == null)
							{
								eIndex = equipmentIndex;
								itemObject = this[equipmentIndex].Item;
								ammoCount = amount;
							}
							else if (itemObject.Id == this[equipmentIndex].Item.Id)
							{
								ammoCount += amount;
							}
						}
					}
				}
			}
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x000796F8 File Offset: 0x000778F8
		public static bool DoesWeaponFitToSlot(EquipmentIndex slotIndex, MissionWeapon weapon)
		{
			bool flag;
			if (weapon.IsEmpty)
			{
				flag = true;
			}
			else if (weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.DropOnWeaponChange | ItemFlags.DropOnAnyAction))
			{
				flag = slotIndex == EquipmentIndex.ExtraWeaponSlot;
			}
			else
			{
				flag = slotIndex >= EquipmentIndex.WeaponItemBeginSlot && slotIndex < EquipmentIndex.ExtraWeaponSlot;
			}
			return flag;
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x00079740 File Offset: 0x00077940
		public void CheckLoadedAmmos()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (!this[equipmentIndex].IsEmpty && this[equipmentIndex].Item.PrimaryWeapon.WeaponClass == WeaponClass.Crossbow)
				{
					int num;
					EquipmentIndex equipmentIndex2;
					this.GetAmmoCountAndIndexOfType(this[equipmentIndex].Item.Type, out num, out equipmentIndex2, EquipmentIndex.None);
					if (equipmentIndex2 != EquipmentIndex.None)
					{
						MissionWeapon missionWeapon = this._weaponSlots[(int)equipmentIndex2].Consume(MathF.Min(this[equipmentIndex].MaxAmmo, this._weaponSlots[(int)equipmentIndex2].Amount));
						this._weaponSlots[(int)equipmentIndex].ReloadAmmo(missionWeapon, this._weaponSlots[(int)equipmentIndex].ReloadPhaseCount);
					}
				}
			}
			this._cache.InvalidateOnWeaponAmmoUpdated();
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x0007981A File Offset: 0x00077A1A
		public void SetUsageIndexOfSlot(EquipmentIndex slotIndex, int usageIndex)
		{
			this._weaponSlots[(int)slotIndex].CurrentUsageIndex = usageIndex;
			this._cache.InvalidateOnWeaponUsageIndexUpdated();
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x00079839 File Offset: 0x00077A39
		public void SetReloadPhaseOfSlot(EquipmentIndex slotIndex, short reloadPhase)
		{
			this._weaponSlots[(int)slotIndex].ReloadPhase = reloadPhase;
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x00079850 File Offset: 0x00077A50
		public void SetAmountOfSlot(EquipmentIndex slotIndex, short dataValue, bool addOverflowToMaxAmount = false)
		{
			if (addOverflowToMaxAmount)
			{
				short num = dataValue - this._weaponSlots[(int)slotIndex].Amount;
				if (num > 0)
				{
					this._weaponSlots[(int)slotIndex].AddExtraModifiedMaxValue(num);
				}
			}
			short amount = this._weaponSlots[(int)slotIndex].Amount;
			this._weaponSlots[(int)slotIndex].Amount = dataValue;
			this._cache.InvalidateOnWeaponAmmoUpdated();
			if ((amount != 0 && dataValue == 0) || (amount == 0 && dataValue != 0))
			{
				this._cache.InvalidateOnWeaponAmmoAvailabilityChanged();
			}
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000798D4 File Offset: 0x00077AD4
		public void SetHitPointsOfSlot(EquipmentIndex slotIndex, short dataValue, bool addOverflowToMaxHitPoints = false)
		{
			if (addOverflowToMaxHitPoints)
			{
				short num = dataValue - this._weaponSlots[(int)slotIndex].HitPoints;
				if (num > 0)
				{
					this._weaponSlots[(int)slotIndex].AddExtraModifiedMaxValue(num);
				}
			}
			this._weaponSlots[(int)slotIndex].HitPoints = dataValue;
			this._cache.InvalidateOnWeaponHitPointsUpdated();
			if (dataValue == 0)
			{
				this._cache.InvalidateOnWeaponDestroyed();
			}
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x0007993C File Offset: 0x00077B3C
		public void SetReloadedAmmoOfSlot(EquipmentIndex slotIndex, EquipmentIndex ammoSlotIndex, short totalAmmo)
		{
			if (ammoSlotIndex == EquipmentIndex.None)
			{
				this._weaponSlots[(int)slotIndex].SetAmmo(MissionWeapon.Invalid);
			}
			else
			{
				MissionWeapon missionWeapon = this._weaponSlots[(int)ammoSlotIndex];
				missionWeapon.Amount = totalAmmo;
				this._weaponSlots[(int)slotIndex].SetAmmo(missionWeapon);
			}
			this._cache.InvalidateOnWeaponAmmoUpdated();
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x00079997 File Offset: 0x00077B97
		public void SetConsumedAmmoOfSlot(EquipmentIndex slotIndex, short count)
		{
			this._weaponSlots[(int)slotIndex].ConsumeAmmo(count);
			this._cache.InvalidateOnWeaponAmmoUpdated();
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000799B6 File Offset: 0x00077BB6
		public void AttachWeaponToWeaponInSlot(EquipmentIndex slotIndex, ref MissionWeapon weapon, ref MatrixFrame attachLocalFrame)
		{
			this._weaponSlots[(int)slotIndex].AttachWeapon(weapon, ref attachLocalFrame);
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x000799D0 File Offset: 0x00077BD0
		public bool HasShield()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				WeaponComponentData currentUsageItem = this._weaponSlots[(int)equipmentIndex].CurrentUsageItem;
				if (currentUsageItem != null && currentUsageItem.IsShield)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x00079A0C File Offset: 0x00077C0C
		public bool HasAnyWeapon()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (this._weaponSlots[(int)equipmentIndex].CurrentUsageItem != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x00079A3C File Offset: 0x00077C3C
		public bool HasAnyWeaponWithFlags(WeaponFlags flags)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				WeaponComponentData currentUsageItem = this._weaponSlots[(int)equipmentIndex].CurrentUsageItem;
				if (currentUsageItem != null && currentUsageItem.WeaponFlags.HasAllFlags(flags))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x00079A7C File Offset: 0x00077C7C
		public bool HasAnyWeaponWithItemUsageSetFlags(ItemObject.ItemUsageSetFlags flags)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				MissionWeapon missionWeapon = this._weaponSlots[(int)equipmentIndex];
				if (missionWeapon.HasAnyUsageWithItemUsageSetFlags(flags))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x00079AB0 File Offset: 0x00077CB0
		public ItemObject GetBanner()
		{
			ItemObject itemObject = null;
			MissionWeapon missionWeapon = this._weaponSlots[4];
			ItemObject item = missionWeapon.Item;
			if (item != null && item.IsBannerItem && item.BannerComponent != null)
			{
				itemObject = item;
			}
			return itemObject;
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x00079AEC File Offset: 0x00077CEC
		public bool HasRangedWeapon(WeaponClass requiredAmmoClass = WeaponClass.Undefined)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				WeaponComponentData currentUsageItem = this._weaponSlots[(int)equipmentIndex].CurrentUsageItem;
				if (currentUsageItem != null && currentUsageItem.IsRangedWeapon && (requiredAmmoClass == WeaponClass.Undefined || currentUsageItem.AmmoClass == requiredAmmoClass))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00079B34 File Offset: 0x00077D34
		public bool ContainsNonConsumableRangedWeaponWithAmmo()
		{
			this._cacheLock.EnterReadLock();
			try
			{
				if (this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsNonConsumableRangedWeaponWithAmmo))
				{
					return this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsNonConsumableRangedWeaponWithAmmo);
				}
			}
			finally
			{
				this._cacheLock.ExitReadLock();
			}
			this._cacheLock.EnterWriteLock();
			bool value;
			try
			{
				if (!this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsNonConsumableRangedWeaponWithAmmo))
				{
					this.GatherInformationAndUpdateCache();
				}
				value = this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsNonConsumableRangedWeaponWithAmmo);
			}
			finally
			{
				this._cacheLock.ExitWriteLock();
			}
			return value;
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x00079BD0 File Offset: 0x00077DD0
		public bool ContainsMeleeWeapon()
		{
			this._cacheLock.EnterReadLock();
			try
			{
				if (this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsMeleeWeapon))
				{
					return this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsMeleeWeapon);
				}
			}
			finally
			{
				this._cacheLock.ExitReadLock();
			}
			this._cacheLock.EnterWriteLock();
			bool value;
			try
			{
				if (!this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsMeleeWeapon))
				{
					this.GatherInformationAndUpdateCache();
				}
				value = this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsMeleeWeapon);
			}
			finally
			{
				this._cacheLock.ExitWriteLock();
			}
			return value;
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x00079C6C File Offset: 0x00077E6C
		public bool ContainsShield()
		{
			this._cacheLock.EnterReadLock();
			try
			{
				if (this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsShield))
				{
					return this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsShield);
				}
			}
			finally
			{
				this._cacheLock.ExitReadLock();
			}
			this._cacheLock.EnterWriteLock();
			bool value;
			try
			{
				if (!this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsShield))
				{
					this.GatherInformationAndUpdateCache();
				}
				value = this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsShield);
			}
			finally
			{
				this._cacheLock.ExitWriteLock();
			}
			return value;
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x00079D08 File Offset: 0x00077F08
		public bool ContainsSpear()
		{
			this._cacheLock.EnterReadLock();
			try
			{
				if (this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsSpear))
				{
					return this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsSpear);
				}
			}
			finally
			{
				this._cacheLock.ExitReadLock();
			}
			this._cacheLock.EnterWriteLock();
			bool value;
			try
			{
				if (!this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsSpear))
				{
					this.GatherInformationAndUpdateCache();
				}
				value = this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsSpear);
			}
			finally
			{
				this._cacheLock.ExitWriteLock();
			}
			return value;
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x00079DA4 File Offset: 0x00077FA4
		public bool ContainsThrownWeapon()
		{
			this._cacheLock.EnterReadLock();
			try
			{
				if (this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsThrownWeapon))
				{
					return this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsThrownWeapon);
				}
			}
			finally
			{
				this._cacheLock.ExitReadLock();
			}
			this._cacheLock.EnterWriteLock();
			bool value;
			try
			{
				if (!this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsThrownWeapon))
				{
					this.GatherInformationAndUpdateCache();
				}
				value = this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsThrownWeapon);
			}
			finally
			{
				this._cacheLock.ExitWriteLock();
			}
			return value;
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x00079E40 File Offset: 0x00078040
		private void GatherInformationAndUpdateCache()
		{
			bool flag;
			bool flag2;
			bool flag3;
			bool flag4;
			bool flag5;
			this.GatherInformation(out flag, out flag2, out flag3, out flag4, out flag5);
			this._cache.UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsMeleeWeapon, flag);
			this._cache.UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsShield, flag2);
			this._cache.UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsSpear, flag3);
			this._cache.UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsNonConsumableRangedWeaponWithAmmo, flag4);
			this._cache.UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsThrownWeapon, flag5);
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x00079EA0 File Offset: 0x000780A0
		private void GatherInformation(out bool containsMeleeWeapon, out bool containsShield, out bool containsSpear, out bool containsNonConsumableRangedWeaponWithAmmo, out bool containsThrownWeapon)
		{
			containsMeleeWeapon = false;
			containsShield = false;
			containsSpear = false;
			containsNonConsumableRangedWeaponWithAmmo = false;
			containsThrownWeapon = false;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				bool flag;
				bool flag2;
				bool flag3;
				bool flag4;
				bool flag5;
				WeaponClass weaponClass;
				this._weaponSlots[(int)equipmentIndex].GatherInformationFromWeapon(out flag, out flag2, out flag3, out flag4, out flag5, out weaponClass);
				containsMeleeWeapon = containsMeleeWeapon || flag;
				containsShield = containsShield || flag2;
				containsSpear = containsSpear || flag3;
				containsThrownWeapon = containsThrownWeapon || flag5;
				if (flag4)
				{
					containsNonConsumableRangedWeaponWithAmmo = containsNonConsumableRangedWeaponWithAmmo || this.GetAmmoAmount(equipmentIndex) > 0;
				}
			}
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x00079F1C File Offset: 0x0007811C
		public void SetGlossMultipliersOfWeaponsRandomly(int seed)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				this._weaponSlots[(int)equipmentIndex].SetRandomGlossMultiplier(seed);
			}
		}

		// Token: 0x04000D66 RID: 3430
		private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();

		// Token: 0x04000D67 RID: 3431
		public Action OnWeaponSlotUpdated;

		// Token: 0x04000D68 RID: 3432
		private readonly MissionWeapon[] _weaponSlots;

		// Token: 0x04000D69 RID: 3433
		private MissionEquipment.MissionEquipmentCache _cache;

		// Token: 0x0200054A RID: 1354
		private struct MissionEquipmentCache
		{
			// Token: 0x06003D4F RID: 15695 RVA: 0x000F537B File Offset: 0x000F357B
			public void Initialize()
			{
				this._cachedBool = default(StackArray.StackArray5Bool);
				this._validity = default(StackArray.StackArray6Bool);
			}

			// Token: 0x06003D50 RID: 15696 RVA: 0x000F5395 File Offset: 0x000F3595
			public bool IsValid(MissionEquipment.MissionEquipmentCache.CachedBool queriedData)
			{
				return this._validity[(int)queriedData];
			}

			// Token: 0x06003D51 RID: 15697 RVA: 0x000F53A4 File Offset: 0x000F35A4
			public void UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedBool data, bool value)
			{
				this._cachedBool[(int)data] = value;
				this._validity[(int)data] = true;
			}

			// Token: 0x06003D52 RID: 15698 RVA: 0x000F53CD File Offset: 0x000F35CD
			public bool GetValue(MissionEquipment.MissionEquipmentCache.CachedBool data)
			{
				return this._cachedBool[(int)data];
			}

			// Token: 0x06003D53 RID: 15699 RVA: 0x000F53DB File Offset: 0x000F35DB
			public bool IsValid(MissionEquipment.MissionEquipmentCache.CachedFloat queriedData)
			{
				return this._validity[(int)(5 + queriedData)];
			}

			// Token: 0x06003D54 RID: 15700 RVA: 0x000F53EC File Offset: 0x000F35EC
			public void UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedFloat data, float value)
			{
				this._cachedFloat = value;
				this._validity[(int)(5 + data)] = true;
			}

			// Token: 0x06003D55 RID: 15701 RVA: 0x000F5411 File Offset: 0x000F3611
			public float GetValue(MissionEquipment.MissionEquipmentCache.CachedFloat data)
			{
				return this._cachedFloat;
			}

			// Token: 0x06003D56 RID: 15702 RVA: 0x000F541C File Offset: 0x000F361C
			public void InvalidateOnWeaponSlotUpdated()
			{
				this._validity[0] = false;
				this._validity[1] = false;
				this._validity[2] = false;
				this._validity[3] = false;
				this._validity[4] = false;
				this._validity[5] = false;
			}

			// Token: 0x06003D57 RID: 15703 RVA: 0x000F5477 File Offset: 0x000F3677
			public void InvalidateOnWeaponUsageIndexUpdated()
			{
			}

			// Token: 0x06003D58 RID: 15704 RVA: 0x000F5479 File Offset: 0x000F3679
			public void InvalidateOnWeaponAmmoUpdated()
			{
				this._validity[5] = false;
			}

			// Token: 0x06003D59 RID: 15705 RVA: 0x000F5488 File Offset: 0x000F3688
			public void InvalidateOnWeaponAmmoAvailabilityChanged()
			{
				this._validity[3] = false;
			}

			// Token: 0x06003D5A RID: 15706 RVA: 0x000F5497 File Offset: 0x000F3697
			public void InvalidateOnWeaponHitPointsUpdated()
			{
				this._validity[5] = false;
			}

			// Token: 0x06003D5B RID: 15707 RVA: 0x000F54A6 File Offset: 0x000F36A6
			public void InvalidateOnWeaponDestroyed()
			{
				this._validity[1] = false;
			}

			// Token: 0x04001E02 RID: 7682
			private const int CachedBoolCount = 5;

			// Token: 0x04001E03 RID: 7683
			private const int CachedFloatCount = 1;

			// Token: 0x04001E04 RID: 7684
			private float _cachedFloat;

			// Token: 0x04001E05 RID: 7685
			private StackArray.StackArray5Bool _cachedBool;

			// Token: 0x04001E06 RID: 7686
			private StackArray.StackArray6Bool _validity;

			// Token: 0x020006C2 RID: 1730
			public enum CachedBool
			{
				// Token: 0x040023A3 RID: 9123
				ContainsMeleeWeapon,
				// Token: 0x040023A4 RID: 9124
				ContainsShield,
				// Token: 0x040023A5 RID: 9125
				ContainsSpear,
				// Token: 0x040023A6 RID: 9126
				ContainsNonConsumableRangedWeaponWithAmmo,
				// Token: 0x040023A7 RID: 9127
				ContainsThrownWeapon,
				// Token: 0x040023A8 RID: 9128
				Count
			}

			// Token: 0x020006C3 RID: 1731
			public enum CachedFloat
			{
				// Token: 0x040023AA RID: 9130
				TotalWeightOfWeapons,
				// Token: 0x040023AB RID: 9131
				Count
			}
		}
	}
}
