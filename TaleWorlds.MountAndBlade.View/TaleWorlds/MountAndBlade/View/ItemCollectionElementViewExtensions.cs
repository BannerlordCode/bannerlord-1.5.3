using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000018 RID: 24
	public static class ItemCollectionElementViewExtensions
	{
		// Token: 0x0600009A RID: 154 RVA: 0x00004DF8 File Offset: 0x00002FF8
		public static string GetMaterialCacheID(object o)
		{
			ItemRosterElement itemRosterElement = (ItemRosterElement)o;
			if (!itemRosterElement.EquipmentElement.Item.IsCraftedWeapon)
			{
				return "InventorySlot_" + itemRosterElement.EquipmentElement.Item.MultiMeshName;
			}
			return "";
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004E48 File Offset: 0x00003048
		public static MetaMesh GetMultiMesh(this ItemObject item, bool isFemale, bool useSlimVersion, bool needBatchedVersion)
		{
			MetaMesh metaMesh = null;
			if (item != null)
			{
				bool flag = false;
				if (item.HasArmorComponent)
				{
					flag = item.ArmorComponent.MultiMeshHasGenderVariations;
				}
				metaMesh = item.GetMultiMeshCopyWithGenderData(flag && isFemale, useSlimVersion, needBatchedVersion);
				if (metaMesh == null || metaMesh.MeshCount == 0)
				{
					metaMesh = item.GetMultiMeshCopy();
				}
			}
			return metaMesh;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004E95 File Offset: 0x00003095
		public static MetaMesh GetMultiMesh(this EquipmentElement equipmentElement, bool isFemale, bool useSlimVersion, bool needBatchedVersion)
		{
			if (equipmentElement.CosmeticItem == null)
			{
				return equipmentElement.Item.GetMultiMesh(isFemale, useSlimVersion, needBatchedVersion);
			}
			return equipmentElement.CosmeticItem.GetMultiMesh(isFemale, useSlimVersion, needBatchedVersion);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004EBD File Offset: 0x000030BD
		public static MetaMesh GetMultiMesh(this MissionWeapon weapon, bool isFemale, bool useSlimVersion, bool needBatchedVersion)
		{
			return weapon.Item.GetMultiMesh(isFemale, useSlimVersion, needBatchedVersion);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004ED0 File Offset: 0x000030D0
		public static MetaMesh GetItemMeshForInventory(this ItemRosterElement rosterElement, bool isFemale = false)
		{
			if (rosterElement.EquipmentElement.Item.ItemType != ItemObject.ItemTypeEnum.Arrows && rosterElement.EquipmentElement.Item.ItemType != ItemObject.ItemTypeEnum.Bolts && rosterElement.EquipmentElement.Item.ItemType != ItemObject.ItemTypeEnum.SlingStones)
			{
				return rosterElement.EquipmentElement.GetMultiMesh(isFemale, false, false);
			}
			return rosterElement.EquipmentElement.Item.GetHolsterMeshCopy();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004F48 File Offset: 0x00003148
		public static MetaMesh GetHolsterMeshCopy(this ItemObject item)
		{
			if (item.WeaponDesign != null)
			{
				MetaMesh holsterMesh = CraftedDataViewManager.GetCraftedDataView(item.WeaponDesign).HolsterMesh;
				if (!(holsterMesh != null))
				{
					return null;
				}
				return holsterMesh.CreateCopy();
			}
			else
			{
				if (!string.IsNullOrEmpty(item.HolsterMeshName))
				{
					return MetaMesh.GetCopy(item.HolsterMeshName, true, false);
				}
				return null;
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004FA2 File Offset: 0x000031A2
		public static MetaMesh GetHolsterMeshIfExists(this ItemObject item)
		{
			if (!(item.WeaponDesign != null))
			{
				return null;
			}
			return CraftedDataViewManager.GetCraftedDataView(item.WeaponDesign).HolsterMesh;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004FC4 File Offset: 0x000031C4
		public static MetaMesh GetHolsterWithWeaponMeshCopy(this ItemObject item, bool needBatchedVersion)
		{
			if (item.WeaponDesign != null)
			{
				CraftedDataView craftedDataView = CraftedDataViewManager.GetCraftedDataView(item.WeaponDesign);
				MetaMesh metaMesh = (needBatchedVersion ? craftedDataView.HolsterMeshWithWeapon : craftedDataView.NonBatchedHolsterMeshWithWeapon);
				if (!(metaMesh != null))
				{
					return null;
				}
				return metaMesh.CreateCopy();
			}
			else
			{
				if (!string.IsNullOrEmpty(item.HolsterWithWeaponMeshName))
				{
					return MetaMesh.GetCopy(item.HolsterWithWeaponMeshName, true, false);
				}
				return null;
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000502B File Offset: 0x0000322B
		public static MetaMesh GetHolsterWithWeaponMeshIfExists(this ItemObject item)
		{
			if (!(item.WeaponDesign != null))
			{
				return null;
			}
			return CraftedDataViewManager.GetCraftedDataView(item.WeaponDesign).HolsterMeshWithWeapon;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005050 File Offset: 0x00003250
		public static MetaMesh GetFlyingMeshCopy(this ItemObject item, bool needBatchedVersion)
		{
			WeaponComponent weaponComponent = item.WeaponComponent;
			WeaponComponentData weaponComponentData = ((weaponComponent != null) ? weaponComponent.PrimaryWeapon : null);
			if (item.WeaponDesign != null && weaponComponentData != null)
			{
				if (!weaponComponentData.IsRangedWeapon || !weaponComponentData.IsConsumable)
				{
					return null;
				}
				CraftedDataView craftedDataView = CraftedDataViewManager.GetCraftedDataView(item.WeaponDesign);
				MetaMesh metaMesh = (needBatchedVersion ? craftedDataView.WeaponMesh : craftedDataView.NonBatchedWeaponMesh);
				if (!(metaMesh != null))
				{
					return null;
				}
				return metaMesh.CreateCopy();
			}
			else
			{
				if (!string.IsNullOrEmpty(item.FlyingMeshName))
				{
					return MetaMesh.GetCopy(item.FlyingMeshName, true, false);
				}
				return null;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000050DF File Offset: 0x000032DF
		public static MetaMesh GetFlyingMeshIfExists(this ItemObject item)
		{
			if (item == null)
			{
				return null;
			}
			WeaponComponent weaponComponent = item.WeaponComponent;
			if (weaponComponent == null)
			{
				return null;
			}
			return weaponComponent.PrimaryWeapon.GetFlyingMeshIfExists(item);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005100 File Offset: 0x00003300
		internal static Material GetTableauMaterial(this ItemObject item, Banner banner)
		{
			Material tableauMaterial = null;
			if (item != null && item.IsUsingTableau)
			{
				Material material = null;
				MetaMesh metaMesh = item.GetMultiMeshCopy();
				int meshCount = metaMesh.MeshCount;
				for (int i = 0; i < meshCount; i++)
				{
					Mesh meshAtIndex = metaMesh.GetMeshAtIndex(i);
					if (!meshAtIndex.HasTag("dont_use_tableau"))
					{
						material = meshAtIndex.GetMaterial();
						meshAtIndex.ManualInvalidate();
						break;
					}
					meshAtIndex.ManualInvalidate();
				}
				metaMesh.ManualInvalidate();
				if (meshCount == 0 || material == null)
				{
					metaMesh = item.GetMultiMeshCopy();
					Mesh meshAtIndex2 = metaMesh.GetMeshAtIndex(0);
					material = meshAtIndex2.GetMaterial();
					meshAtIndex2.ManualInvalidate();
					metaMesh.ManualInvalidate();
				}
				if (banner != null)
				{
					BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual("ItemCollectionElementViewExtensions");
					if (material == null)
					{
						material = Material.GetDefaultTableauSampleMaterial(true);
					}
					uint flagMask = (uint)material.GetShader().GetMaterialShaderFlagMask("use_tableau_blending", true);
					Dictionary<Tuple<Material, Banner>, Material> dictionary = null;
					if (ViewSubModule.BannerTexturedMaterialCache != null)
					{
						dictionary = ViewSubModule.BannerTexturedMaterialCache;
					}
					if (dictionary != null)
					{
						if (dictionary.ContainsKey(new Tuple<Material, Banner>(material, banner)))
						{
							tableauMaterial = dictionary[new Tuple<Material, Banner>(material, banner)];
						}
						else
						{
							tableauMaterial = material.CreateCopy();
							Action<Texture> action = delegate(Texture tex)
							{
								ulong shaderFlags = tableauMaterial.GetShaderFlags();
								tableauMaterial.SetShaderFlags(shaderFlags | (ulong)flagMask);
								tableauMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, tex);
							};
							banner.GetTableauTextureSmall(in bannerDebugInfo, action);
							dictionary.Add(new Tuple<Material, Banner>(material, banner), tableauMaterial);
						}
					}
					else
					{
						tableauMaterial = material.CreateCopy();
						Action<Texture> action2 = delegate(Texture tex)
						{
							ulong shaderFlags2 = tableauMaterial.GetShaderFlags();
							tableauMaterial.SetShaderFlags(shaderFlags2 | (ulong)flagMask);
							tableauMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, tex);
						};
						banner.GetTableauTextureSmall(in bannerDebugInfo, action2);
					}
				}
			}
			return tableauMaterial;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000052B2 File Offset: 0x000034B2
		public static MatrixFrame GetCameraFrameForInventory(this ItemRosterElement itemRosterElement)
		{
			return MatrixFrame.Identity;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000052BC File Offset: 0x000034BC
		public static MatrixFrame GetItemFrameForInventory(this ItemRosterElement itemRosterElement)
		{
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			Mat3 identity = Mat3.Identity;
			float num = 0.95f;
			Vec3 vec = new Vec3(0f, 0f, 0f, -1f);
			MetaMesh itemMeshForInventory = itemRosterElement.GetItemMeshForInventory(false);
			if (itemMeshForInventory != null)
			{
				switch (itemRosterElement.EquipmentElement.Item.ItemType)
				{
				case ItemObject.ItemTypeEnum.Horse:
				case ItemObject.ItemTypeEnum.Animal:
				case ItemObject.ItemTypeEnum.HorseHarness:
					identity.RotateAboutSide(-1.5707964f);
					identity.RotateAboutUp(1.5707964f);
					break;
				case ItemObject.ItemTypeEnum.OneHandedWeapon:
				case ItemObject.ItemTypeEnum.TwoHandedWeapon:
				case ItemObject.ItemTypeEnum.Polearm:
					identity.RotateAboutSide(-1.5707964f);
					identity.RotateAboutForward(-0.7853982f);
					break;
				case ItemObject.ItemTypeEnum.Arrows:
					identity.RotateAboutSide(-1.5707964f);
					identity.RotateAboutForward(-0.7853982f);
					break;
				case ItemObject.ItemTypeEnum.Bolts:
				case ItemObject.ItemTypeEnum.SlingStones:
				case ItemObject.ItemTypeEnum.Thrown:
					identity.RotateAboutSide(-1.5707964f);
					identity.RotateAboutForward(-0.7853982f);
					break;
				case ItemObject.ItemTypeEnum.Shield:
					identity.RotateAboutUp(3.1415927f);
					break;
				case ItemObject.ItemTypeEnum.Bow:
				case ItemObject.ItemTypeEnum.Sling:
					identity.RotateAboutSide(-1.5707964f);
					identity.RotateAboutForward(-0.7853982f);
					break;
				case ItemObject.ItemTypeEnum.Crossbow:
					identity.RotateAboutForward(2.3561945f);
					identity.RotateAboutSide(-2.3561945f);
					identity.RotateAboutUp(-1.5707964f);
					break;
				case ItemObject.ItemTypeEnum.Goods:
					identity.RotateAboutSide(-1.0995574f);
					identity.RotateAboutUp(0.7853982f);
					break;
				case ItemObject.ItemTypeEnum.HeadArmor:
				case ItemObject.ItemTypeEnum.BodyArmor:
				case ItemObject.ItemTypeEnum.Cape:
					identity.RotateAboutSide(-1.5707964f);
					identity.RotateAboutUp(3.1415927f);
					identity.RotateAboutSide(-0.18849556f);
					break;
				case ItemObject.ItemTypeEnum.LegArmor:
					identity.RotateAboutSide(-1.5707964f);
					identity.RotateAboutUp(3.1415927f);
					identity.RotateAboutSide(-0.31415927f);
					identity.RotateAboutUp(0.47123894f);
					break;
				case ItemObject.ItemTypeEnum.HandArmor:
					identity.RotateAboutSide(1.7278761f);
					num = 2.1f;
					vec = new Vec3(0f, -0.4f, 0f, -1f);
					break;
				case ItemObject.ItemTypeEnum.Book:
					identity.RotateAboutSide(-0.62831855f);
					identity.RotateAboutUp(-0.47123894f);
					break;
				}
				if (itemRosterElement.EquipmentElement.Item.IsCraftedWeapon)
				{
					num *= 0.55f;
				}
				matrixFrame = itemRosterElement.EquipmentElement.Item.GetScaledFrame(identity, itemMeshForInventory, num, vec);
				if (itemRosterElement.EquipmentElement.Item.IsCraftedWeapon)
				{
					matrixFrame.Elevate(-0.01f * ((float)itemRosterElement.EquipmentElement.Item.WeaponComponent.PrimaryWeapon.WeaponLength / 2f));
				}
			}
			return matrixFrame;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000055A0 File Offset: 0x000037A0
		public static MatrixFrame GetItemFrameForItemTooltip(this ItemRosterElement itemRosterElement)
		{
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			Mat3 identity = Mat3.Identity;
			float num = 0.85f;
			Vec3 vec = new Vec3(0f, 0f, 0f, -1f);
			MetaMesh itemMeshForInventory = itemRosterElement.GetItemMeshForInventory(false);
			if (itemMeshForInventory != null)
			{
				ItemObject.ItemTypeEnum itemType = itemRosterElement.EquipmentElement.Item.ItemType;
				if (itemType == ItemObject.ItemTypeEnum.BodyArmor || itemType == ItemObject.ItemTypeEnum.Cape || itemType == ItemObject.ItemTypeEnum.HeadArmor)
				{
					identity.RotateAboutSide(-1.5707964f);
					identity.RotateAboutUp(3.1415927f);
				}
				else if (itemType == ItemObject.ItemTypeEnum.Arrows)
				{
					identity.RotateAboutSide(-1.5707964f);
				}
				else if (itemType == ItemObject.ItemTypeEnum.Horse || itemType == ItemObject.ItemTypeEnum.HorseHarness || itemType == ItemObject.ItemTypeEnum.Animal)
				{
					identity.RotateAboutSide(-1.5707964f);
					identity.RotateAboutUp(1.5707964f);
					num = 0.65f;
				}
				else if (itemType == ItemObject.ItemTypeEnum.OneHandedWeapon || itemType == ItemObject.ItemTypeEnum.TwoHandedWeapon || itemType == ItemObject.ItemTypeEnum.Bow || itemType == ItemObject.ItemTypeEnum.Thrown || itemType == ItemObject.ItemTypeEnum.SlingStones || itemType == ItemObject.ItemTypeEnum.Bolts || itemType == ItemObject.ItemTypeEnum.Polearm || itemType == ItemObject.ItemTypeEnum.Crossbow || itemType == ItemObject.ItemTypeEnum.Sling)
				{
					identity.RotateAboutSide(-1.5707964f);
					identity.RotateAboutForward(-0.7853982f);
				}
				else if (itemType == ItemObject.ItemTypeEnum.LegArmor)
				{
					identity.RotateAboutSide(-1.5707964f);
					identity.RotateAboutUp(3.1415927f);
				}
				else if (itemType == ItemObject.ItemTypeEnum.HandArmor)
				{
					identity.RotateAboutSide(1.5707964f);
					identity.RotateAboutSide(-0.25132743f);
					num = 2.1f;
					vec = new Vec3(0f, -0.4f, 0f, -1f);
				}
				else if (itemType == ItemObject.ItemTypeEnum.Shield)
				{
					identity.RotateAboutUp(2.261947f);
				}
				else if (itemType != ItemObject.ItemTypeEnum.Musket)
				{
					if (itemType == ItemObject.ItemTypeEnum.Goods)
					{
						identity.RotateAboutSide(-1.0995574f);
						identity.RotateAboutUp(0.7853982f);
					}
					else if (itemType == ItemObject.ItemTypeEnum.Book)
					{
						identity.RotateAboutSide(-0.62831855f);
						identity.RotateAboutUp(-0.47123894f);
					}
				}
				if (itemRosterElement.EquipmentElement.Item.IsCraftedWeapon)
				{
					num *= 0.55f;
				}
				matrixFrame = itemRosterElement.EquipmentElement.Item.GetScaledFrame(identity, itemMeshForInventory, num, vec);
				matrixFrame.origin.z = matrixFrame.origin.z - 5f;
			}
			if (itemRosterElement.EquipmentElement.Item.IsCraftedWeapon)
			{
				matrixFrame.Elevate(-0.01f * ((float)itemRosterElement.EquipmentElement.Item.WeaponComponent.PrimaryWeapon.WeaponLength / 2f));
			}
			return matrixFrame;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005824 File Offset: 0x00003A24
		public static void OnGetWeaponData(ref WeaponData weaponData, MissionWeapon weapon, bool isFemale, Banner banner, bool needBatchedVersion)
		{
			MetaMesh multiMesh = weapon.GetMultiMesh(isFemale, false, needBatchedVersion);
			weaponData.WeaponMesh = multiMesh;
			MetaMesh holsterMeshCopy = weapon.Item.GetHolsterMeshCopy();
			weaponData.HolsterMesh = holsterMeshCopy;
			MetaMesh holsterWithWeaponMeshCopy = weapon.Item.GetHolsterWithWeaponMeshCopy(needBatchedVersion);
			weaponData.Prefab = weapon.Item.PrefabName;
			weaponData.HolsterMeshWithWeapon = holsterWithWeaponMeshCopy;
			MetaMesh flyingMeshCopy = weapon.Item.GetFlyingMeshCopy(needBatchedVersion);
			weaponData.FlyingMesh = flyingMeshCopy;
			Material tableauMaterial = weapon.Item.GetTableauMaterial(banner);
			weaponData.TableauMaterial = tableauMaterial;
		}
	}
}
