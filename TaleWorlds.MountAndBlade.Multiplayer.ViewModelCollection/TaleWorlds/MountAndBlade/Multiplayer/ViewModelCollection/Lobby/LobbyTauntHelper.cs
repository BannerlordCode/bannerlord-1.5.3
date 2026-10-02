using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x02000026 RID: 38
	internal static class LobbyTauntHelper
	{
		// Token: 0x0600029E RID: 670 RVA: 0x0000A5D4 File Offset: 0x000087D4
		public static Equipment PrepareForTaunt(Equipment originalEquipment, TauntCosmeticElement taunt, bool doNotAddComplimentaryWeapons = false)
		{
			TauntUsageManager.TauntUsageSet usageSet = TauntUsageManager.Instance.GetUsageSet(taunt.Id);
			MBReadOnlyList<TauntUsageManager.TauntUsage> mbreadOnlyList = ((usageSet != null) ? usageSet.GetUsages() : null);
			if (mbreadOnlyList == null || mbreadOnlyList.Count == 0)
			{
				return originalEquipment;
			}
			Equipment equipment = new Equipment(originalEquipment);
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
			WeaponComponentData weaponComponentData3 = null;
			if (!flag && equipmentIndex2 != EquipmentIndex.None)
			{
				ItemObject item2 = equipment[equipmentIndex2].Item;
				weaponComponentData3 = ((item2 != null) ? item2.PrimaryWeapon : null);
			}
			using (List<TauntUsageManager.TauntUsage>.Enumerator enumerator = mbreadOnlyList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsSuitable(false, true, weaponComponentData2, weaponComponentData3))
					{
						return equipment;
					}
				}
			}
			TauntUsageManager.TauntUsage tauntUsage = mbreadOnlyList.FirstOrDefault<TauntUsageManager.TauntUsage>((TauntUsageManager.TauntUsage u) => !u.UsageFlag.HasAnyFlag(TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresBow | TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresShield)) ?? mbreadOnlyList[0];
			for (EquipmentIndex equipmentIndex3 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex3 < EquipmentIndex.NumAllWeaponSlots; equipmentIndex3++)
			{
				equipment[equipmentIndex3] = default(EquipmentElement);
			}
			List<ItemObject> list = MBObjectManager.Instance.GetObjectTypeList<ItemObject>().ToList<ItemObject>();
			list.Sort((ItemObject first, ItemObject second) => first.Value.CompareTo(second.Value));
			EquipmentIndex equipmentIndex4 = EquipmentIndex.WeaponItemBeginSlot;
			if (tauntUsage.UsageFlag.HasAnyFlag(TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresBow))
			{
				ItemObject randomElementWithPredicate = list.GetRandomElementWithPredicate<ItemObject>(delegate(ItemObject i)
				{
					WeaponComponentData primaryWeapon = i.PrimaryWeapon;
					return CosmeticsManagerHelper.IsWeaponClassBow((primaryWeapon != null) ? primaryWeapon.WeaponClass : WeaponClass.Undefined);
				});
				if (!equipment.TryAddElement(ref equipmentIndex4, new EquipmentElement(randomElementWithPredicate, null, null, false)))
				{
					return equipment;
				}
				ItemObject randomElementWithPredicate2 = list.GetRandomElementWithPredicate<ItemObject>(delegate(ItemObject i)
				{
					WeaponComponentData primaryWeapon2 = i.PrimaryWeapon;
					return primaryWeapon2 != null && primaryWeapon2.WeaponClass == WeaponClass.Arrow;
				});
				equipment.TryAddElement(ref equipmentIndex4, new EquipmentElement(randomElementWithPredicate2, null, null, false));
				return equipment;
			}
			else
			{
				if (tauntUsage.UsageFlag.HasAnyFlag(TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresShield))
				{
					ItemObject randomElementWithPredicate3 = list.GetRandomElementWithPredicate<ItemObject>(delegate(ItemObject i)
					{
						WeaponComponentData primaryWeapon3 = i.PrimaryWeapon;
						return CosmeticsManagerHelper.IsWeaponClassShield((primaryWeapon3 != null) ? primaryWeapon3.WeaponClass : WeaponClass.Undefined);
					});
					if (!equipment.TryAddElement(ref equipmentIndex4, new EquipmentElement(randomElementWithPredicate3, null, null, false)))
					{
						return equipment;
					}
					if (!tauntUsage.UsageFlag.HasAnyFlag(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForOneHanded))
					{
						ItemObject randomElementWithPredicate4 = list.GetRandomElementWithPredicate<ItemObject>(delegate(ItemObject i)
						{
							WeaponComponentData primaryWeapon4 = i.PrimaryWeapon;
							return CosmeticsManagerHelper.IsWeaponClassOneHanded((primaryWeapon4 != null) ? primaryWeapon4.WeaponClass : WeaponClass.Undefined);
						});
						equipment.TryAddElement(ref equipmentIndex4, new EquipmentElement(randomElementWithPredicate4, null, null, false));
						return equipment;
					}
				}
				if (!tauntUsage.UsageFlag.HasAnyFlag(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForTwoHanded))
				{
					ItemObject randomElementWithPredicate5 = list.GetRandomElementWithPredicate<ItemObject>(delegate(ItemObject i)
					{
						WeaponComponentData primaryWeapon5 = i.PrimaryWeapon;
						return CosmeticsManagerHelper.IsWeaponClassTwoHanded((primaryWeapon5 != null) ? primaryWeapon5.WeaponClass : WeaponClass.Undefined);
					});
					if (!equipment.TryAddElement(ref equipmentIndex4, new EquipmentElement(randomElementWithPredicate5, null, null, false)))
					{
						return equipment;
					}
					if (tauntUsage.IsSuitable(false, true, randomElementWithPredicate5.PrimaryWeapon, null))
					{
						return equipment;
					}
				}
				if (!tauntUsage.UsageFlag.HasAnyFlag(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForOneHanded))
				{
					ItemObject randomElementWithPredicate6 = list.GetRandomElementWithPredicate<ItemObject>(delegate(ItemObject i)
					{
						WeaponComponentData primaryWeapon6 = i.PrimaryWeapon;
						return CosmeticsManagerHelper.IsWeaponClassOneHanded((primaryWeapon6 != null) ? primaryWeapon6.WeaponClass : WeaponClass.Undefined);
					});
					if (!equipment.TryAddElement(ref equipmentIndex4, new EquipmentElement(randomElementWithPredicate6, null, null, false)))
					{
						return equipment;
					}
					if (tauntUsage.IsSuitable(false, true, randomElementWithPredicate6.PrimaryWeapon, null))
					{
						return equipment;
					}
				}
				if (!tauntUsage.UsageFlag.HasAnyFlag(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForShield))
				{
					ItemObject randomElementWithPredicate7 = list.GetRandomElementWithPredicate<ItemObject>(delegate(ItemObject i)
					{
						WeaponComponentData primaryWeapon7 = i.PrimaryWeapon;
						return CosmeticsManagerHelper.IsWeaponClassShield((primaryWeapon7 != null) ? primaryWeapon7.WeaponClass : WeaponClass.Undefined);
					});
					if (!equipment.TryAddElement(ref equipmentIndex4, new EquipmentElement(randomElementWithPredicate7, null, null, false)))
					{
						return equipment;
					}
				}
				if (!tauntUsage.UsageFlag.HasAnyFlag(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForBow))
				{
					ItemObject randomElementWithPredicate8 = list.GetRandomElementWithPredicate<ItemObject>(delegate(ItemObject i)
					{
						WeaponComponentData primaryWeapon8 = i.PrimaryWeapon;
						return CosmeticsManagerHelper.IsWeaponClassBow((primaryWeapon8 != null) ? primaryWeapon8.WeaponClass : WeaponClass.Undefined);
					});
					if (!equipment.TryAddElement(ref equipmentIndex4, new EquipmentElement(randomElementWithPredicate8, null, null, false)))
					{
						return equipment;
					}
					ItemObject randomElementWithPredicate9 = list.GetRandomElementWithPredicate<ItemObject>(delegate(ItemObject i)
					{
						WeaponComponentData primaryWeapon9 = i.PrimaryWeapon;
						return primaryWeapon9 != null && primaryWeapon9.WeaponClass == WeaponClass.Arrow;
					});
					equipment.TryAddElement(ref equipmentIndex4, new EquipmentElement(randomElementWithPredicate9, null, null, false));
					return equipment;
				}
				else
				{
					if (tauntUsage.UsageFlag.HasAnyFlag(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForCrossbow))
					{
						return equipment;
					}
					ItemObject randomElementWithPredicate10 = list.GetRandomElementWithPredicate<ItemObject>(delegate(ItemObject i)
					{
						WeaponComponentData primaryWeapon10 = i.PrimaryWeapon;
						return CosmeticsManagerHelper.IsWeaponClassCrossbow((primaryWeapon10 != null) ? primaryWeapon10.WeaponClass : WeaponClass.Undefined);
					});
					if (!equipment.TryAddElement(ref equipmentIndex4, new EquipmentElement(randomElementWithPredicate10, null, null, false)))
					{
						return equipment;
					}
					ItemObject randomElementWithPredicate11 = list.GetRandomElementWithPredicate<ItemObject>(delegate(ItemObject i)
					{
						WeaponComponentData primaryWeapon11 = i.PrimaryWeapon;
						return primaryWeapon11 != null && primaryWeapon11.WeaponClass == WeaponClass.Bolt;
					});
					equipment.TryAddElement(ref equipmentIndex4, new EquipmentElement(randomElementWithPredicate11, null, null, false));
					return equipment;
				}
			}
			Equipment equipment2;
			return equipment2;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000AAA8 File Offset: 0x00008CA8
		private static Tuple<EquipmentIndex, EquipmentElement, WeaponComponentData> GetWeaponInfoOfType(this Equipment equipment, WeaponClass type)
		{
			Func<WeaponComponentData, bool> <>9__0;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				ItemObject item = equipment[equipmentIndex].Item;
				WeaponComponentData weaponComponentData;
				if (item == null)
				{
					weaponComponentData = null;
				}
				else
				{
					MBReadOnlyList<WeaponComponentData> weapons = item.Weapons;
					if (weapons == null)
					{
						weaponComponentData = null;
					}
					else
					{
						Func<WeaponComponentData, bool> func;
						if ((func = <>9__0) == null)
						{
							func = (<>9__0 = (WeaponComponentData w) => w.WeaponClass == type);
						}
						weaponComponentData = weapons.FirstOrDefault<WeaponComponentData>(func);
					}
				}
				WeaponComponentData weaponComponentData2 = weaponComponentData;
				if (weaponComponentData2 != null)
				{
					return new Tuple<EquipmentIndex, EquipmentElement, WeaponComponentData>(equipmentIndex, equipment[equipmentIndex], weaponComponentData2);
				}
			}
			return null;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000AB2C File Offset: 0x00008D2C
		private static Tuple<EquipmentIndex, EquipmentElement, WeaponComponentData> GetWeaponInfoOfPredicate(this Equipment equipment, Predicate<WeaponComponentData> predicate)
		{
			if (predicate == null)
			{
				return null;
			}
			Func<WeaponComponentData, bool> <>9__0;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				ItemObject item = equipment[equipmentIndex].Item;
				WeaponComponentData weaponComponentData;
				if (item == null)
				{
					weaponComponentData = null;
				}
				else
				{
					MBReadOnlyList<WeaponComponentData> weapons = item.Weapons;
					if (weapons == null)
					{
						weaponComponentData = null;
					}
					else
					{
						Func<WeaponComponentData, bool> func;
						if ((func = <>9__0) == null)
						{
							func = (<>9__0 = (WeaponComponentData w) => predicate(w));
						}
						weaponComponentData = weapons.FirstOrDefault<WeaponComponentData>(func);
					}
				}
				WeaponComponentData weaponComponentData2 = weaponComponentData;
				if (weaponComponentData2 != null)
				{
					return new Tuple<EquipmentIndex, EquipmentElement, WeaponComponentData>(equipmentIndex, equipment[equipmentIndex], weaponComponentData2);
				}
			}
			return null;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000ABB8 File Offset: 0x00008DB8
		private static Tuple<EquipmentIndex, EquipmentElement, WeaponComponentData> GetTwoHandedWeaponInfo(this Equipment equipment)
		{
			Tuple<EquipmentIndex, EquipmentElement, WeaponComponentData> tuple;
			if ((tuple = equipment.GetWeaponInfoOfType(WeaponClass.TwoHandedAxe)) == null && (tuple = equipment.GetWeaponInfoOfType(WeaponClass.TwoHandedSword)) == null)
			{
				tuple = equipment.GetWeaponInfoOfType(WeaponClass.TwoHandedMace) ?? equipment.GetWeaponInfoOfType(WeaponClass.TwoHandedPolearm);
			}
			return tuple;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000ABE3 File Offset: 0x00008DE3
		private static bool TryAddElement(this Equipment equipment, ref EquipmentIndex eqIndex, EquipmentElement element)
		{
			if (eqIndex < EquipmentIndex.WeaponItemBeginSlot || eqIndex > EquipmentIndex.Weapon1)
			{
				return false;
			}
			if (Equipment.IsItemFitsToSlot(eqIndex, element.Item))
			{
				equipment[eqIndex] = element;
				eqIndex++;
			}
			return true;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000AC14 File Offset: 0x00008E14
		private static void SwapItems(this Equipment equipment, EquipmentIndex first, EquipmentIndex second)
		{
			EquipmentElement equipmentElement = equipment[first];
			equipment[first] = equipment[second];
			equipment[second] = equipmentElement;
		}
	}
}
