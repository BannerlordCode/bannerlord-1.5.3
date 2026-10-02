using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000204 RID: 516
	public class DefaultItemPickupModel : ItemPickupModel
	{
		// Token: 0x06001E27 RID: 7719 RVA: 0x00066EDC File Offset: 0x000650DC
		public override float GetItemScoreForAgent(SpawnedItemEntity item, Agent agent)
		{
			if (!item.WeaponCopy.Item.ItemFlags.HasAnyFlag(ItemFlags.CannotBePickedUp))
			{
				WeaponClass weaponClass = item.WeaponCopy.Item.PrimaryWeapon.WeaponClass;
				if (MissionGameModels.Current.BattleBannerBearersModel.IsFormationBanner(agent.Formation, item))
				{
					return 120f;
				}
				if (agent.HadSameTypeOfConsumableOrShieldOnSpawn(weaponClass))
				{
					switch (weaponClass)
					{
					case WeaponClass.Arrow:
					case WeaponClass.Bolt:
					case WeaponClass.SlingStone:
						return 80f;
					case WeaponClass.Stone:
					case WeaponClass.BallistaStone:
						return 20f;
					case WeaponClass.Boulder:
					case WeaponClass.BallistaBoulder:
						return -1f;
					case WeaponClass.ThrowingAxe:
						return 60f;
					case WeaponClass.ThrowingKnife:
						return 50f;
					case WeaponClass.Javelin:
						return 70f;
					case WeaponClass.SmallShield:
					case WeaponClass.LargeShield:
						return 100f;
					}
					throw new MBException("This pickable item not scored: " + weaponClass.ToString());
				}
			}
			return 0f;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x00066FF0 File Offset: 0x000651F0
		public override bool IsItemAvailableForAgent(SpawnedItemEntity item, Agent agent, EquipmentIndex slotToPickUp)
		{
			if (!agent.CanReachAndUseObject(item, 0f) || !agent.ObjectHasVacantPosition(item) || item.HasAIMovingTo)
			{
				return false;
			}
			WeaponClass weaponClass = item.WeaponCopy.Item.PrimaryWeapon.WeaponClass;
			if (weaponClass - WeaponClass.Arrow > 2)
			{
				switch (weaponClass)
				{
				case WeaponClass.ThrowingAxe:
				case WeaponClass.ThrowingKnife:
				case WeaponClass.Javelin:
					break;
				case WeaponClass.Pistol:
				case WeaponClass.Musket:
				case WeaponClass.BallistaBoulder:
				case WeaponClass.BallistaStone:
					return false;
				case WeaponClass.SmallShield:
				case WeaponClass.LargeShield:
					return agent.Equipment[slotToPickUp].IsEmpty && agent.HasLostShield();
				case WeaponClass.Banner:
					return agent.Equipment[slotToPickUp].IsEmpty;
				default:
					return false;
				}
			}
			if (item.WeaponCopy.Amount > 0 && !agent.Equipment[slotToPickUp].IsEmpty && agent.Equipment[slotToPickUp].Item.PrimaryWeapon.WeaponClass == weaponClass && (int)agent.Equipment[slotToPickUp].Amount <= agent.Equipment[slotToPickUp].ModifiedMaxAmount >> 1)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x00067128 File Offset: 0x00065328
		public override bool IsAgentEquipmentSuitableForPickUpAvailability(Agent agent)
		{
			if (agent.HasLostShield())
			{
				return true;
			}
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
			{
				MissionWeapon missionWeapon = agent.Equipment[equipmentIndex];
				if (!missionWeapon.IsEmpty && missionWeapon.IsAnyConsumable() && (int)missionWeapon.Amount <= missionWeapon.ModifiedMaxAmount >> 1)
				{
					return true;
				}
			}
			BattleBannerBearersModel battleBannerBearersModel = MissionGameModels.Current.BattleBannerBearersModel;
			return battleBannerBearersModel != null && battleBannerBearersModel.IsBannerSearchingAgent(agent);
		}
	}
}
