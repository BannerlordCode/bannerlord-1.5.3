using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000029 RID: 41
	public static class WeaponComponentViewExtensions
	{
		// Token: 0x06000132 RID: 306 RVA: 0x00008994 File Offset: 0x00006B94
		public static MetaMesh GetFlyingMeshCopy(this WeaponComponentData weaponComponentData, ItemObject item)
		{
			if (item.WeaponDesign != null)
			{
				if (!weaponComponentData.IsRangedWeapon || !weaponComponentData.IsConsumable)
				{
					return null;
				}
				MetaMesh weaponMesh = CraftedDataViewManager.GetCraftedDataView(item.WeaponDesign).WeaponMesh;
				if (!(weaponMesh != null))
				{
					return null;
				}
				return weaponMesh.CreateCopy();
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

		// Token: 0x06000133 RID: 307 RVA: 0x00008A00 File Offset: 0x00006C00
		public static MetaMesh GetFlyingMeshIfExists(this WeaponComponentData weaponComponentData, ItemObject item)
		{
			if (item.WeaponDesign != null && weaponComponentData.IsRangedWeapon && weaponComponentData.IsConsumable)
			{
				return CraftedDataViewManager.GetCraftedDataView(item.WeaponDesign).WeaponMesh;
			}
			return null;
		}
	}
}
