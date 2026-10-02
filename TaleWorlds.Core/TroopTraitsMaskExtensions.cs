using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000D9 RID: 217
	public static class TroopTraitsMaskExtensions
	{
		// Token: 0x06000B46 RID: 2886 RVA: 0x00024BE2 File Offset: 0x00022DE2
		public static bool HasMelee(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Melee) > TroopTraitsMask.None;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00024BEA File Offset: 0x00022DEA
		public static bool HasRanged(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Ranged) > TroopTraitsMask.None;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00024BF2 File Offset: 0x00022DF2
		public static bool HasMount(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Mount) > TroopTraitsMask.None;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00024BFA File Offset: 0x00022DFA
		public static bool HasArmor(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Armor) > TroopTraitsMask.None;
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00024C02 File Offset: 0x00022E02
		public static bool HasThrown(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Thrown) > TroopTraitsMask.None;
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00024C0B File Offset: 0x00022E0B
		public static bool HasSpear(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Spear) > TroopTraitsMask.None;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00024C14 File Offset: 0x00022E14
		public static bool HasShield(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Shield) > TroopTraitsMask.None;
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00024C1D File Offset: 0x00022E1D
		public static bool HasLowTier(this TroopTraitsMask troopFilterMask)
		{
			return (troopFilterMask & TroopTraitsMask.LowTier) > TroopTraitsMask.None;
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00024C29 File Offset: 0x00022E29
		public static bool HasHighTier(this TroopTraitsMask troopFilterMask)
		{
			return (troopFilterMask & TroopTraitsMask.HighTier) > TroopTraitsMask.None;
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00024C38 File Offset: 0x00022E38
		public static string GetTroopTraitsText(this TroopTraitsMask troopTraitsMask)
		{
			string text = "";
			if (troopTraitsMask.HasMelee())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Melee, ref text);
			}
			else if (troopTraitsMask.HasRanged())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Ranged, ref text);
			}
			if (troopTraitsMask.HasMount())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Mount, ref text);
			}
			if (troopTraitsMask.HasArmor())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Armor, ref text);
			}
			if (troopTraitsMask.HasThrown())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Thrown, ref text);
			}
			if (troopTraitsMask.HasSpear())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Spear, ref text);
			}
			if (troopTraitsMask.HasShield())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Shield, ref text);
			}
			return text;
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00024CC4 File Offset: 0x00022EC4
		public static string GetTraitsFilterText(this TroopTraitsMask troopTraitsFilter)
		{
			string text = "";
			if (troopTraitsFilter.HasArmor())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Armor, ref text);
			}
			if (troopTraitsFilter.HasThrown())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Thrown, ref text);
			}
			if (troopTraitsFilter.HasSpear())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Spear, ref text);
			}
			if (troopTraitsFilter.HasShield())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Shield, ref text);
			}
			if (troopTraitsFilter.HasLowTier())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.LowTier, ref text);
			}
			else if (troopTraitsFilter.HasHighTier())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.HighTier, ref text);
			}
			return text;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00024D48 File Offset: 0x00022F48
		public static string GetClassFilterText(this TroopTraitsMask troopTraitsFilter)
		{
			string text = "";
			if (troopTraitsFilter.HasMelee())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Melee, ref text);
			}
			if (troopTraitsFilter.HasRanged())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Ranged, ref text);
			}
			if (troopTraitsFilter.HasMount())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Mount, ref text);
			}
			return text;
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00024D8C File Offset: 0x00022F8C
		private static void AddFlagToText(TroopTraitsMask flag, ref string text)
		{
			if (text.Length > 0)
			{
				text += "|";
			}
			string text2;
			if (flag <= TroopTraitsMask.Thrown)
			{
				switch (flag)
				{
				case TroopTraitsMask.Melee:
					text2 = "Melee";
					goto IL_00B7;
				case TroopTraitsMask.Ranged:
					text2 = "Ranged";
					goto IL_00B7;
				case TroopTraitsMask.Melee | TroopTraitsMask.Ranged:
					break;
				case TroopTraitsMask.Mount:
					text2 = "Mount";
					goto IL_00B7;
				default:
					if (flag == TroopTraitsMask.Armor)
					{
						text2 = "Armor";
						goto IL_00B7;
					}
					if (flag == TroopTraitsMask.Thrown)
					{
						text2 = "Thrown";
						goto IL_00B7;
					}
					break;
				}
			}
			else if (flag <= TroopTraitsMask.Shield)
			{
				if (flag == TroopTraitsMask.Spear)
				{
					text2 = "Spear";
					goto IL_00B7;
				}
				if (flag == TroopTraitsMask.Shield)
				{
					text2 = "Shield";
					goto IL_00B7;
				}
			}
			else
			{
				if (flag == TroopTraitsMask.LowTier)
				{
					text2 = "Low Tier";
					goto IL_00B7;
				}
				if (flag == TroopTraitsMask.HighTier)
				{
					text2 = "High Tier";
					goto IL_00B7;
				}
			}
			text2 = "";
			IL_00B7:
			text += text2;
		}
	}
}
