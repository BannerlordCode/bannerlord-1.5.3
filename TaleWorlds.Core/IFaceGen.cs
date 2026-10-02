using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000084 RID: 132
	public interface IFaceGen
	{
		// Token: 0x0600088C RID: 2188
		BodyProperties GetRandomBodyProperties(int race, bool isFemale, BodyProperties bodyPropertiesMin, BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tatooTags, float variationAmount);

		// Token: 0x0600088D RID: 2189
		void GenerateParentBody(BodyProperties childBodyProperties, int race, ref BodyProperties motherBodyProperties, ref BodyProperties fatherBodyProperties);

		// Token: 0x0600088E RID: 2190
		void SetBody(ref BodyProperties bodyProperties, int build, int weight);

		// Token: 0x0600088F RID: 2191
		void SetHair(ref BodyProperties bodyProperties, int hair, int beard, int tattoo);

		// Token: 0x06000890 RID: 2192
		void SetPigmentation(ref BodyProperties bodyProperties, int skinColor, int hairColor, int eyeColor);

		// Token: 0x06000891 RID: 2193
		BodyProperties GetBodyPropertiesWithAge(ref BodyProperties bodyProperties, float age);

		// Token: 0x06000892 RID: 2194
		BodyMeshMaturityType GetMaturityTypeWithAge(float age);

		// Token: 0x06000893 RID: 2195
		int GetRaceCount();

		// Token: 0x06000894 RID: 2196
		int GetRaceOrDefault(string raceId);

		// Token: 0x06000895 RID: 2197
		string GetBaseMonsterNameFromRace(int race);

		// Token: 0x06000896 RID: 2198
		string[] GetRaceNames();

		// Token: 0x06000897 RID: 2199
		Monster GetMonster(string monsterID);

		// Token: 0x06000898 RID: 2200
		Monster GetMonsterWithSuffix(int race, string suffix);

		// Token: 0x06000899 RID: 2201
		Monster GetBaseMonsterFromRace(int race);

		// Token: 0x0600089A RID: 2202
		int[] GetHairIndicesByTag(int race, int curGender, float age, string tag);

		// Token: 0x0600089B RID: 2203
		int[] GetFacialIndicesByTag(int race, int curGender, float age, string tag);

		// Token: 0x0600089C RID: 2204
		int[] GetTattooIndicesByTag(int race, int curGender, float age, string tag);

		// Token: 0x0600089D RID: 2205
		float GetTattooZeroProbability(int race, int curGender, float age);
	}
}
