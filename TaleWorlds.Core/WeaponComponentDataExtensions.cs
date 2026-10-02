using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000E1 RID: 225
	public static class WeaponComponentDataExtensions
	{
		// Token: 0x06000B7E RID: 2942 RVA: 0x0002540F File Offset: 0x0002360F
		public static int GetModifiedThrustDamage(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.ThrustDamage > 0)
			{
				return itemModifier.ModifyDamage(componentData.ThrustDamage);
			}
			return componentData.ThrustDamage;
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00025430 File Offset: 0x00023630
		public static int GetModifiedSwingDamage(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.SwingDamage > 0)
			{
				return itemModifier.ModifyDamage(componentData.SwingDamage);
			}
			return componentData.SwingDamage;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x00025451 File Offset: 0x00023651
		public static int GetModifiedMissileDamage(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.MissileDamage > 0)
			{
				return itemModifier.ModifyDamage(componentData.MissileDamage);
			}
			return componentData.MissileDamage;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00025472 File Offset: 0x00023672
		public static int GetModifiedThrustSpeed(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.ThrustSpeed > 0)
			{
				return itemModifier.ModifySpeed(componentData.ThrustSpeed);
			}
			return componentData.ThrustSpeed;
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00025493 File Offset: 0x00023693
		public static int GetModifiedSwingSpeed(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.SwingSpeed > 0)
			{
				return itemModifier.ModifySpeed(componentData.SwingSpeed);
			}
			return componentData.SwingSpeed;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x000254B4 File Offset: 0x000236B4
		public static int GetModifiedMissileSpeed(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.MissileSpeed > 0)
			{
				return itemModifier.ModifyMissileSpeed(componentData.MissileSpeed);
			}
			return componentData.MissileSpeed;
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x000254D5 File Offset: 0x000236D5
		public static int GetModifiedHandling(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.Handling > 0)
			{
				return itemModifier.ModifySpeed(componentData.Handling);
			}
			return componentData.Handling;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x000254F6 File Offset: 0x000236F6
		public static short GetModifiedStackCount(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.MaxDataValue > 0)
			{
				return itemModifier.ModifyStackCount(componentData.MaxDataValue);
			}
			return componentData.MaxDataValue;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00025517 File Offset: 0x00023717
		public static short GetModifiedMaximumHitPoints(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.MaxDataValue > 0)
			{
				return itemModifier.ModifyHitPoints(componentData.MaxDataValue);
			}
			return componentData.MaxDataValue;
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00025538 File Offset: 0x00023738
		public static int GetModifiedArmor(this WeaponComponentData componentData, ItemModifier itemModifier)
		{
			if (itemModifier != null && componentData.BodyArmor > 0)
			{
				return itemModifier.ModifyArmor(componentData.BodyArmor);
			}
			return componentData.BodyArmor;
		}
	}
}
