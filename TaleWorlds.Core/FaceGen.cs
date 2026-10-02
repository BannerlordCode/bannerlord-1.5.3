using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200005C RID: 92
	public static class FaceGen
	{
		// Token: 0x0600071F RID: 1823 RVA: 0x00018D0C File Offset: 0x00016F0C
		public static void SetInstance(IFaceGen faceGen)
		{
			FaceGen._instance = faceGen;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00018D14 File Offset: 0x00016F14
		public static BodyProperties GetRandomBodyProperties(int race, bool isFemale, BodyProperties bodyPropertiesMin, BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tatooTags, float variationAmount)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetRandomBodyProperties(race, isFemale, bodyPropertiesMin, bodyPropertiesMax, hairCoverType, seed, hairTags, beardTags, tatooTags, variationAmount);
			}
			return bodyPropertiesMin;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00018D44 File Offset: 0x00016F44
		public static int GetRaceCount()
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return 0;
			}
			return instance.GetRaceCount();
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00018D56 File Offset: 0x00016F56
		public static int GetRaceOrDefault(string raceId)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return 0;
			}
			return instance.GetRaceOrDefault(raceId);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00018D69 File Offset: 0x00016F69
		public static string GetBaseMonsterNameFromRace(int race)
		{
			IFaceGen instance = FaceGen._instance;
			return ((instance != null) ? instance.GetBaseMonsterNameFromRace(race) : null) ?? null;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00018D82 File Offset: 0x00016F82
		public static string[] GetRaceNames()
		{
			IFaceGen instance = FaceGen._instance;
			return ((instance != null) ? instance.GetRaceNames() : null) ?? null;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00018D9A File Offset: 0x00016F9A
		public static Monster GetMonster(string monsterID)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return null;
			}
			return instance.GetMonster(monsterID);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00018DAD File Offset: 0x00016FAD
		public static Monster GetMonsterWithSuffix(int race, string suffix)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return null;
			}
			return instance.GetMonsterWithSuffix(race, suffix);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00018DC1 File Offset: 0x00016FC1
		public static Monster GetBaseMonsterFromRace(int race)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return null;
			}
			return instance.GetBaseMonsterFromRace(race);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00018DD4 File Offset: 0x00016FD4
		public static void GenerateParentKey(BodyProperties childBodyProperties, int race, ref BodyProperties motherBodyProperties, ref BodyProperties fatherBodyProperties)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return;
			}
			instance.GenerateParentBody(childBodyProperties, race, ref motherBodyProperties, ref fatherBodyProperties);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00018DE9 File Offset: 0x00016FE9
		public static void SetHair(ref BodyProperties bodyProperties, int hair, int beard, int tattoo)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return;
			}
			instance.SetHair(ref bodyProperties, hair, beard, tattoo);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00018DFE File Offset: 0x00016FFE
		public static void SetBody(ref BodyProperties bodyProperties, int build, int weight)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return;
			}
			instance.SetBody(ref bodyProperties, build, weight);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00018E12 File Offset: 0x00017012
		public static void SetPigmentation(ref BodyProperties bodyProperties, int skinColor, int hairColor, int eyeColor)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return;
			}
			instance.SetPigmentation(ref bodyProperties, skinColor, hairColor, eyeColor);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00018E27 File Offset: 0x00017027
		public static BodyProperties GetBodyPropertiesWithAge(ref BodyProperties originalBodyProperties, float age)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetBodyPropertiesWithAge(ref originalBodyProperties, age);
			}
			return originalBodyProperties;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00018E43 File Offset: 0x00017043
		public static BodyMeshMaturityType GetMaturityTypeWithAge(float age)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetMaturityTypeWithAge(age);
			}
			return BodyMeshMaturityType.Child;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00018E59 File Offset: 0x00017059
		public static int[] GetHairIndicesByTag(int race, int curGender, float age, string tag)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetHairIndicesByTag(race, curGender, age, tag);
			}
			return Array.Empty<int>();
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00018E76 File Offset: 0x00017076
		public static int[] GetFacialIndicesByTag(int race, int curGender, float age, string tag)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetFacialIndicesByTag(race, curGender, age, tag);
			}
			return Array.Empty<int>();
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00018E93 File Offset: 0x00017093
		public static int[] GetTattooIndicesByTag(int race, int curGender, float age, string tag)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetTattooIndicesByTag(race, curGender, age, tag);
			}
			return Array.Empty<int>();
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00018EB0 File Offset: 0x000170B0
		public static float GetTattooZeroProbability(int race, int curGender, float age)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetTattooZeroProbability(race, curGender, age);
			}
			return 0f;
		}

		// Token: 0x04000398 RID: 920
		public const string MonsterSuffixSettlement = "_settlement";

		// Token: 0x04000399 RID: 921
		public const string MonsterSuffixSettlementSlow = "_settlement_slow";

		// Token: 0x0400039A RID: 922
		public const string MonsterSuffixSettlementFast = "_settlement_fast";

		// Token: 0x0400039B RID: 923
		public const string MonsterSuffixChild = "_child";

		// Token: 0x0400039C RID: 924
		public static bool ShowDebugValues;

		// Token: 0x0400039D RID: 925
		public static bool UpdateDeformKeys;

		// Token: 0x0400039E RID: 926
		private static IFaceGen _instance;
	}
}
