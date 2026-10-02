using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000067 RID: 103
	public static class FormationClassExtensions
	{
		// Token: 0x06000742 RID: 1858 RVA: 0x000190DA File Offset: 0x000172DA
		public static string GetName(this FormationClass formationClass)
		{
			if (formationClass == FormationClass.NumberOfDefaultFormations)
			{
				return "Skirmisher";
			}
			if (formationClass == FormationClass.NumberOfRegularFormations)
			{
				return "General";
			}
			if (formationClass != FormationClass.NumberOfAllFormations)
			{
				return formationClass.ToString();
			}
			return "Unset";
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001910C File Offset: 0x0001730C
		public static TextObject GetLocalizedName(this FormationClass formationClass)
		{
			string text = "str_troop_group_name";
			int num = (int)formationClass;
			return GameTexts.FindText(text, num.ToString());
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001912C File Offset: 0x0001732C
		public static TroopUsageFlags GetTroopUsageFlags(this FormationClass troopClass)
		{
			switch (troopClass)
			{
			case FormationClass.Ranged:
				return TroopUsageFlags.OnFoot | TroopUsageFlags.Ranged | TroopUsageFlags.BowUser | TroopUsageFlags.ThrownUser | TroopUsageFlags.CrossbowUser;
			case FormationClass.Cavalry:
				return TroopUsageFlags.Mounted | TroopUsageFlags.Melee | TroopUsageFlags.OneHandedUser | TroopUsageFlags.ShieldUser | TroopUsageFlags.TwoHandedUser | TroopUsageFlags.PolearmUser;
			case FormationClass.HorseArcher:
				return TroopUsageFlags.Mounted | TroopUsageFlags.Ranged | TroopUsageFlags.BowUser | TroopUsageFlags.ThrownUser | TroopUsageFlags.CrossbowUser;
			}
			return TroopUsageFlags.OnFoot | TroopUsageFlags.Melee | TroopUsageFlags.OneHandedUser | TroopUsageFlags.ShieldUser | TroopUsageFlags.TwoHandedUser | TroopUsageFlags.PolearmUser;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00019160 File Offset: 0x00017360
		public static TroopType GetTroopTypeForRegularFormation(this FormationClass formationClass)
		{
			TroopType troopType = TroopType.Invalid;
			switch (formationClass)
			{
			case FormationClass.Infantry:
			case FormationClass.HeavyInfantry:
				troopType = TroopType.Infantry;
				break;
			case FormationClass.Ranged:
			case FormationClass.NumberOfDefaultFormations:
				troopType = TroopType.Ranged;
				break;
			case FormationClass.Cavalry:
			case FormationClass.HorseArcher:
			case FormationClass.LightCavalry:
			case FormationClass.HeavyCavalry:
				troopType = TroopType.Cavalry;
				break;
			default:
				Debug.FailedAssert(string.Format("Undefined formation class {0} for TroopType!", formationClass), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\FormationClass.cs", "GetTroopTypeForRegularFormation", 323);
				break;
			}
			return troopType;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x000191C8 File Offset: 0x000173C8
		public static bool IsDefaultFormationClass(this FormationClass formationClass)
		{
			return formationClass >= FormationClass.Infantry && formationClass < FormationClass.NumberOfDefaultFormations;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x000191E4 File Offset: 0x000173E4
		public static bool IsRegularFormationClass(this FormationClass formationClass)
		{
			return formationClass >= FormationClass.Infantry && formationClass < FormationClass.NumberOfRegularFormations;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x000191FD File Offset: 0x000173FD
		public static FormationClass FallbackClass(this FormationClass formationClass)
		{
			if (formationClass == FormationClass.Ranged || formationClass == FormationClass.NumberOfDefaultFormations)
			{
				return FormationClass.Ranged;
			}
			if (formationClass == FormationClass.Cavalry || formationClass == FormationClass.HeavyCavalry)
			{
				return FormationClass.Cavalry;
			}
			if (formationClass == FormationClass.HorseArcher || formationClass == FormationClass.LightCavalry)
			{
				return FormationClass.HorseArcher;
			}
			return FormationClass.Infantry;
		}

		// Token: 0x040003E2 RID: 994
		public const TroopUsageFlags DefaultInfantryTroopUsageFlags = TroopUsageFlags.OnFoot | TroopUsageFlags.Melee | TroopUsageFlags.OneHandedUser | TroopUsageFlags.ShieldUser | TroopUsageFlags.TwoHandedUser | TroopUsageFlags.PolearmUser;

		// Token: 0x040003E3 RID: 995
		public const TroopUsageFlags DefaultRangedTroopUsageFlags = TroopUsageFlags.OnFoot | TroopUsageFlags.Ranged | TroopUsageFlags.BowUser | TroopUsageFlags.ThrownUser | TroopUsageFlags.CrossbowUser;

		// Token: 0x040003E4 RID: 996
		public const TroopUsageFlags DefaultCavalryTroopUsageFlags = TroopUsageFlags.Mounted | TroopUsageFlags.Melee | TroopUsageFlags.OneHandedUser | TroopUsageFlags.ShieldUser | TroopUsageFlags.TwoHandedUser | TroopUsageFlags.PolearmUser;

		// Token: 0x040003E5 RID: 997
		public const TroopUsageFlags DefaultHorseArcherTroopUsageFlags = TroopUsageFlags.Mounted | TroopUsageFlags.Ranged | TroopUsageFlags.BowUser | TroopUsageFlags.ThrownUser | TroopUsageFlags.CrossbowUser;

		// Token: 0x040003E6 RID: 998
		public static FormationClass[] FormationClassValues = (FormationClass[])Enum.GetValues(typeof(FormationClass));
	}
}
