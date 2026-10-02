using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000222 RID: 546
	public class FaceGen : IFaceGen
	{
		// Token: 0x06001FD1 RID: 8145 RVA: 0x0006E32C File Offset: 0x0006C52C
		private FaceGen()
		{
			this._raceNamesDictionary = new Dictionary<string, int>();
			this._raceNamesArray = MBAPI.IMBFaceGen.GetRaceIds().Split(new char[] { ';' });
			for (int i = 0; i < this._raceNamesArray.Length; i++)
			{
				this._raceNamesDictionary[this._raceNamesArray[i]] = i;
			}
			this._monstersDictionary = new Dictionary<string, Monster>();
			this._monstersArray = new Monster[this._raceNamesArray.Length];
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x0006E3AF File Offset: 0x0006C5AF
		public static void CreateInstance()
		{
			FaceGen.SetInstance(new FaceGen());
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0006E3BC File Offset: 0x0006C5BC
		public Monster GetMonster(string monsterID)
		{
			Monster @object;
			if (!this._monstersDictionary.TryGetValue(monsterID, out @object))
			{
				@object = Game.Current.ObjectManager.GetObject<Monster>(monsterID);
				this._monstersDictionary[monsterID] = @object;
			}
			return @object;
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0006E3F8 File Offset: 0x0006C5F8
		public Monster GetMonsterWithSuffix(int race, string suffix)
		{
			return this.GetMonster(this._raceNamesArray[race] + suffix);
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x0006E410 File Offset: 0x0006C610
		public Monster GetBaseMonsterFromRace(int race)
		{
			if (race >= 0 && race < this._monstersArray.Length)
			{
				Monster monster = this._monstersArray[race];
				if (monster == null)
				{
					monster = Game.Current.ObjectManager.GetObject<Monster>(this._raceNamesArray[race]);
					this._monstersArray[race] = monster;
				}
				return monster;
			}
			Debug.FailedAssert("Monster race index is out of bounds: " + race, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\FaceGen.cs", "GetBaseMonsterFromRace", 65);
			return null;
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x0006E480 File Offset: 0x0006C680
		public BodyProperties GetRandomBodyProperties(int race, bool isFemale, BodyProperties bodyPropertiesMin, BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tattooTags, float variationAmount)
		{
			return MBBodyProperties.GetRandomBodyProperties(race, isFemale, bodyPropertiesMin, bodyPropertiesMax, hairCoverType, seed, hairTags, beardTags, tattooTags, variationAmount);
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x0006E4A3 File Offset: 0x0006C6A3
		void IFaceGen.GenerateParentBody(BodyProperties childBodyProperties, int race, ref BodyProperties motherBodyProperties, ref BodyProperties fatherBodyProperties)
		{
			MBBodyProperties.GenerateParentKey(childBodyProperties, race, ref motherBodyProperties, ref fatherBodyProperties);
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x0006E4AF File Offset: 0x0006C6AF
		void IFaceGen.SetHair(ref BodyProperties bodyProperties, int hair, int beard, int tattoo)
		{
			MBBodyProperties.SetHair(ref bodyProperties, hair, beard, tattoo);
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x0006E4BB File Offset: 0x0006C6BB
		void IFaceGen.SetBody(ref BodyProperties bodyProperties, int build, int weight)
		{
			MBBodyProperties.SetBody(ref bodyProperties, build, weight);
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x0006E4C5 File Offset: 0x0006C6C5
		void IFaceGen.SetPigmentation(ref BodyProperties bodyProperties, int skinColor, int hairColor, int eyeColor)
		{
			MBBodyProperties.SetPigmentation(ref bodyProperties, skinColor, hairColor, eyeColor);
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x0006E4D1 File Offset: 0x0006C6D1
		public BodyProperties GetBodyPropertiesWithAge(ref BodyProperties bodyProperties, float age)
		{
			return MBBodyProperties.GetBodyPropertiesWithAge(ref bodyProperties, age);
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x0006E4DA File Offset: 0x0006C6DA
		public void GetParamsFromBody(ref FaceGenerationParams faceGenerationParams, BodyProperties bodyProperties, bool earsAreHidden, bool mouthIsHidden)
		{
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, bodyProperties, earsAreHidden, mouthIsHidden);
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x0006E4E6 File Offset: 0x0006C6E6
		public BodyMeshMaturityType GetMaturityTypeWithAge(float age)
		{
			return MBBodyProperties.GetMaturityType(age);
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x0006E4EE File Offset: 0x0006C6EE
		public static void FlushFaceCache()
		{
			MBBodyProperties.FlushFaceCache();
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x0006E4F5 File Offset: 0x0006C6F5
		public int GetRaceCount()
		{
			return this._raceNamesArray.Length;
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x0006E4FF File Offset: 0x0006C6FF
		public int GetRaceOrDefault(string raceId)
		{
			return this._raceNamesDictionary[raceId];
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x0006E50D File Offset: 0x0006C70D
		public string GetBaseMonsterNameFromRace(int race)
		{
			return this._raceNamesArray[race];
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x0006E517 File Offset: 0x0006C717
		public string[] GetRaceNames()
		{
			return (string[])this._raceNamesArray.Clone();
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x0006E529 File Offset: 0x0006C729
		public int[] GetHairIndicesByTag(int race, int curGender, float age, string tag)
		{
			return MBBodyProperties.GetHairIndicesByTag(race, curGender, age, tag);
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x0006E535 File Offset: 0x0006C735
		public int[] GetFacialIndicesByTag(int race, int curGender, float age, string tag)
		{
			return MBBodyProperties.GetFacialIndicesByTag(race, curGender, age, tag);
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x0006E541 File Offset: 0x0006C741
		public int[] GetTattooIndicesByTag(int race, int curGender, float age, string tag)
		{
			return MBBodyProperties.GetTattooIndicesByTag(race, curGender, age, tag);
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x0006E550 File Offset: 0x0006C750
		public float GetTattooZeroProbability(int race, int curGender, float age)
		{
			float num = 0f;
			MBBodyProperties.GetZeroProbabilities(race, curGender, age, ref num);
			return num;
		}

		// Token: 0x04000AD2 RID: 2770
		private readonly Dictionary<string, int> _raceNamesDictionary;

		// Token: 0x04000AD3 RID: 2771
		private readonly string[] _raceNamesArray;

		// Token: 0x04000AD4 RID: 2772
		private readonly Dictionary<string, Monster> _monstersDictionary;

		// Token: 0x04000AD5 RID: 2773
		private readonly Monster[] _monstersArray;
	}
}
