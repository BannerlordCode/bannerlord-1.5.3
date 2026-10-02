using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200020E RID: 526
	public static class CosmeticsManagerHelper
	{
		// Token: 0x06001E8F RID: 7823 RVA: 0x00068BFC File Offset: 0x00066DFC
		public static Dictionary<int, List<int>> GetUsedIndicesFromIds(Dictionary<string, List<string>> usedCosmetics)
		{
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			MBReadOnlyList<MultiplayerClassDivisions.MPHeroClass> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<MultiplayerClassDivisions.MPHeroClass>();
			foreach (KeyValuePair<string, List<string>> keyValuePair in usedCosmetics)
			{
				int num = -1;
				for (int i = 0; i < objectTypeList.Count; i++)
				{
					if (objectTypeList[i].StringId == keyValuePair.Key)
					{
						num = i;
						break;
					}
				}
				if (num != -1)
				{
					List<int> list = new List<int>();
					foreach (string text in keyValuePair.Value)
					{
						int num2 = -1;
						for (int j = 0; j < CosmeticsManager.CosmeticElementsList.Count; j++)
						{
							if (CosmeticsManager.CosmeticElementsList[j].Id == text)
							{
								num2 = j;
								break;
							}
						}
						if (num2 >= 0)
						{
							list.Add(num2);
						}
					}
					if (list.Count > 0)
					{
						dictionary.Add(num, list);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x00068D40 File Offset: 0x00066F40
		public static ActionIndexCache GetSuitableTauntAction(Agent agent, int tauntIndex)
		{
			if (agent.Equipment == null)
			{
				return ActionIndexCache.act_none;
			}
			WeaponComponentData currentUsageItem = agent.WieldedWeapon.CurrentUsageItem;
			WeaponComponentData currentUsageItem2 = agent.WieldedOffhandWeapon.CurrentUsageItem;
			return ActionIndexCache.Create(TauntUsageManager.Instance.GetAction(tauntIndex, agent.GetIsLeftStance(), !agent.HasMount, currentUsageItem, currentUsageItem2));
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x00068D9C File Offset: 0x00066F9C
		public static TauntUsageManager.TauntUsage.TauntUsageFlag GetActionNotUsableReason(Agent agent, int tauntIndex)
		{
			WeaponComponentData currentUsageItem = agent.WieldedWeapon.CurrentUsageItem;
			WeaponComponentData currentUsageItem2 = agent.WieldedOffhandWeapon.CurrentUsageItem;
			return TauntUsageManager.Instance.GetIsActionNotSuitableReason(tauntIndex, agent.GetIsLeftStance(), !agent.HasMount, currentUsageItem, currentUsageItem2);
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x00068DE4 File Offset: 0x00066FE4
		public static string GetSuitableTauntActionForEquipment(Equipment equipment, TauntCosmeticElement taunt)
		{
			if (equipment == null)
			{
				return null;
			}
			EquipmentIndex equipmentIndex;
			EquipmentIndex equipmentIndex2;
			bool flag;
			equipment.GetInitialWeaponIndicesToEquip(out equipmentIndex, out equipmentIndex2, out flag, Equipment.InitialWeaponEquipPreference.Any);
			WeaponComponentData weaponComponentData;
			if (equipmentIndex == EquipmentIndex.None)
			{
				weaponComponentData = null;
			}
			else
			{
				ItemObject item = equipment[equipmentIndex].Item;
				weaponComponentData = ((item != null) ? item.PrimaryWeapon : null);
			}
			WeaponComponentData weaponComponentData2 = weaponComponentData;
			WeaponComponentData weaponComponentData3;
			if (equipmentIndex2 == EquipmentIndex.None)
			{
				weaponComponentData3 = null;
			}
			else
			{
				ItemObject item2 = equipment[equipmentIndex2].Item;
				weaponComponentData3 = ((item2 != null) ? item2.PrimaryWeapon : null);
			}
			WeaponComponentData weaponComponentData4 = weaponComponentData3;
			return TauntUsageManager.Instance.GetAction(TauntUsageManager.Instance.GetIndexOfAction(taunt.Id), false, true, weaponComponentData2, weaponComponentData4);
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x00068E6B File Offset: 0x0006706B
		public static bool IsWeaponClassOneHanded(WeaponClass weaponClass)
		{
			return weaponClass == WeaponClass.OneHandedAxe || weaponClass == WeaponClass.OneHandedPolearm || weaponClass == WeaponClass.OneHandedSword;
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x00068E7C File Offset: 0x0006707C
		public static bool IsWeaponClassTwoHanded(WeaponClass weaponClass)
		{
			return weaponClass == WeaponClass.TwoHandedAxe || weaponClass == WeaponClass.TwoHandedMace || weaponClass == WeaponClass.TwoHandedPolearm || weaponClass == WeaponClass.TwoHandedSword;
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x00068E91 File Offset: 0x00067091
		public static bool IsWeaponClassShield(WeaponClass weaponClass)
		{
			return weaponClass == WeaponClass.LargeShield || weaponClass == WeaponClass.SmallShield;
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x00068E9F File Offset: 0x0006709F
		public static bool IsWeaponClassBow(WeaponClass weaponClass)
		{
			return weaponClass == WeaponClass.Bow;
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x00068EA6 File Offset: 0x000670A6
		public static bool IsWeaponClassCrossbow(WeaponClass weaponClass)
		{
			return weaponClass == WeaponClass.Crossbow;
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x00068EB0 File Offset: 0x000670B0
		public static WeaponClass[] GetComplimentaryWeaponClasses(WeaponClass weaponClass)
		{
			switch (weaponClass)
			{
			case WeaponClass.OneHandedSword:
			case WeaponClass.OneHandedAxe:
			case WeaponClass.Mace:
			case WeaponClass.Pick:
			case WeaponClass.OneHandedPolearm:
			case WeaponClass.LowGripPolearm:
			case WeaponClass.Stone:
			case WeaponClass.ThrowingAxe:
			case WeaponClass.ThrowingKnife:
			case WeaponClass.Javelin:
			case WeaponClass.BallistaStone:
				return new WeaponClass[]
				{
					WeaponClass.SmallShield,
					WeaponClass.LargeShield
				};
			case WeaponClass.Arrow:
				return new WeaponClass[] { WeaponClass.Bow };
			case WeaponClass.Bolt:
				return new WeaponClass[] { WeaponClass.Crossbow };
			case WeaponClass.SlingStone:
				return new WeaponClass[] { WeaponClass.Sling };
			case WeaponClass.Bow:
				return new WeaponClass[] { WeaponClass.Arrow };
			case WeaponClass.Crossbow:
				return new WeaponClass[] { WeaponClass.Bolt };
			case WeaponClass.Sling:
				return new WeaponClass[] { WeaponClass.SlingStone };
			case WeaponClass.SmallShield:
			case WeaponClass.LargeShield:
				return new WeaponClass[]
				{
					WeaponClass.OneHandedAxe,
					WeaponClass.OneHandedSword,
					WeaponClass.OneHandedPolearm,
					WeaponClass.Mace
				};
			}
			return new WeaponClass[0];
		}
	}
}
