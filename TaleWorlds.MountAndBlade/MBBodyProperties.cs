using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C6 RID: 454
	public static class MBBodyProperties
	{
		// Token: 0x06001978 RID: 6520 RVA: 0x00053BE3 File Offset: 0x00051DE3
		public static int GetNumEditableDeformKeys(int race, bool initialGender, int age)
		{
			return MBAPI.IMBFaceGen.GetNumEditableDeformKeys(race, initialGender, (float)age);
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00053BF3 File Offset: 0x00051DF3
		public static void GetParamsFromKey(ref FaceGenerationParams faceGenerationParams, BodyProperties bodyProperties, bool earsAreHidden, bool mouthHidden)
		{
			MBAPI.IMBFaceGen.GetParamsFromKey(ref faceGenerationParams, ref bodyProperties, earsAreHidden, mouthHidden);
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x00053C04 File Offset: 0x00051E04
		public static void GetParamsMax(int race, int curGender, int curAge, ref int hairNum, ref int beardNum, ref int faceTextureNum, ref int mouthTextureNum, ref int faceTattooNum, ref int soundNum, ref int eyebrowNum, ref float scale)
		{
			MBAPI.IMBFaceGen.GetParamsMax(race, curGender, (float)curAge, ref hairNum, ref beardNum, ref faceTextureNum, ref mouthTextureNum, ref faceTattooNum, ref soundNum, ref eyebrowNum, ref scale);
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x00053C2E File Offset: 0x00051E2E
		public static void GetZeroProbabilities(int race, int curGender, float curAge, ref float tattooZeroProbability)
		{
			MBAPI.IMBFaceGen.GetZeroProbabilities(race, curGender, curAge, ref tattooZeroProbability);
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x00053C3E File Offset: 0x00051E3E
		public static void ProduceNumericKeyWithParams(FaceGenerationParams faceGenerationParams, bool earsAreHidden, bool mouthIsHidden, ref BodyProperties bodyProperties)
		{
			MBAPI.IMBFaceGen.ProduceNumericKeyWithParams(ref faceGenerationParams, earsAreHidden, mouthIsHidden, ref bodyProperties);
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x00053C4F File Offset: 0x00051E4F
		public static void TransformFaceKeysToDefaultFace(ref FaceGenerationParams faceGenerationParams)
		{
			MBAPI.IMBFaceGen.TransformFaceKeysToDefaultFace(ref faceGenerationParams);
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x00053C5C File Offset: 0x00051E5C
		public static void ProduceNumericKeyWithDefaultValues(ref BodyProperties initialBodyProperties, bool earsAreHidden, bool mouthIsHidden, int race, int gender, int age)
		{
			MBAPI.IMBFaceGen.ProduceNumericKeyWithDefaultValues(ref initialBodyProperties, earsAreHidden, mouthIsHidden, race, gender, (float)age);
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x00053C74 File Offset: 0x00051E74
		public static BodyProperties GetRandomBodyProperties(int race, bool isFemale, BodyProperties bodyPropertiesMin, BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tatooTags, float variationAmount)
		{
			BodyProperties bodyProperties = default(BodyProperties);
			MBAPI.IMBFaceGen.GetRandomBodyProperties(race, isFemale ? 1 : 0, ref bodyPropertiesMin, ref bodyPropertiesMax, hairCoverType, seed, hairTags, beardTags, tatooTags, variationAmount, ref bodyProperties);
			return bodyProperties;
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x00053CB0 File Offset: 0x00051EB0
		public static DeformKeyData GetDeformKeyData(int keyNo, int race, int gender, int age)
		{
			DeformKeyData deformKeyData = default(DeformKeyData);
			MBAPI.IMBFaceGen.GetDeformKeyData(keyNo, ref deformKeyData, race, gender, (float)age);
			return deformKeyData;
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x00053CD7 File Offset: 0x00051ED7
		public static int GetFaceGenInstancesLength(int race, int gender, int age)
		{
			return MBAPI.IMBFaceGen.GetFaceGenInstancesLength(race, gender, (float)age);
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x00053CE7 File Offset: 0x00051EE7
		public static bool EnforceConstraints(ref FaceGenerationParams faceGenerationParams)
		{
			return MBAPI.IMBFaceGen.EnforceConstraints(ref faceGenerationParams);
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x00053CF4 File Offset: 0x00051EF4
		public static float GetScaleFromKey(int race, int gender, BodyProperties bodyProperties)
		{
			return MBAPI.IMBFaceGen.GetScaleFromKey(race, gender, ref bodyProperties);
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x00053D04 File Offset: 0x00051F04
		public static int GetHairColorCount(int race, int curGender, int age)
		{
			return MBAPI.IMBFaceGen.GetHairColorCount(race, curGender, (float)age);
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x00053D14 File Offset: 0x00051F14
		public static List<uint> GetHairColorGradientPoints(int race, int curGender, int age)
		{
			int hairColorCount = MBBodyProperties.GetHairColorCount(race, curGender, age);
			List<uint> list = new List<uint>();
			Vec3[] array = new Vec3[hairColorCount];
			MBAPI.IMBFaceGen.GetHairColorGradientPoints(race, curGender, (float)age, array);
			foreach (Vec3 vec in array)
			{
				list.Add(MBMath.ColorFromRGBA(vec.x, vec.y, vec.z, 1f));
			}
			return list;
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x00053D83 File Offset: 0x00051F83
		public static int GetTatooColorCount(int race, int curGender, int age)
		{
			return MBAPI.IMBFaceGen.GetTatooColorCount(race, curGender, (float)age);
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x00053D94 File Offset: 0x00051F94
		public static List<uint> GetTatooColorGradientPoints(int race, int curGender, int age)
		{
			int tatooColorCount = MBBodyProperties.GetTatooColorCount(race, curGender, age);
			List<uint> list = new List<uint>();
			Vec3[] array = new Vec3[tatooColorCount];
			MBAPI.IMBFaceGen.GetTatooColorGradientPoints(race, curGender, (float)age, array);
			foreach (Vec3 vec in array)
			{
				list.Add(MBMath.ColorFromRGBA(vec.x, vec.y, vec.z, 1f));
			}
			return list;
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x00053E03 File Offset: 0x00052003
		public static int GetSkinColorCount(int race, int curGender, int age)
		{
			return MBAPI.IMBFaceGen.GetSkinColorCount(race, curGender, (float)age);
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x00053E13 File Offset: 0x00052013
		public static BodyMeshMaturityType GetMaturityType(float age)
		{
			return (BodyMeshMaturityType)MBAPI.IMBFaceGen.GetMaturityType(age);
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x00053E20 File Offset: 0x00052020
		public static void FlushFaceCache()
		{
			MBAPI.IMBFaceGen.FlushFaceCache();
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x00053E2C File Offset: 0x0005202C
		public static string[] GetRaceIds()
		{
			return MBAPI.IMBFaceGen.GetRaceIds().Split(new char[] { ';' });
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x00053E48 File Offset: 0x00052048
		public static int[] GetHairIndicesByTag(int race, int curGender, float age, string tag)
		{
			return MBAPI.IMBFaceGen.GetHairIndicesByTag(race, curGender, age, tag).Split(new char[] { ',' }).Where<string>(delegate(string x)
			{
				int num;
				return int.TryParse(x, out num);
			})
				.Select<string, int>(new Func<string, int>(int.Parse))
				.ToArray<int>();
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x00053EB0 File Offset: 0x000520B0
		public static int[] GetFacialIndicesByTag(int race, int curGender, float age, string tag)
		{
			return MBAPI.IMBFaceGen.GetFacialIndicesByTag(race, curGender, age, tag).Split(new char[] { ',' }).Where<string>(delegate(string x)
			{
				int num;
				return int.TryParse(x, out num);
			})
				.Select<string, int>(new Func<string, int>(int.Parse))
				.ToArray<int>();
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x00053F18 File Offset: 0x00052118
		public static int[] GetTattooIndicesByTag(int race, int curGender, float age, string tag)
		{
			return MBAPI.IMBFaceGen.GetTattooIndicesByTag(race, curGender, age, tag).Split(new char[] { ',' }).Where<string>(delegate(string x)
			{
				int num;
				return int.TryParse(x, out num);
			})
				.Select<string, int>(new Func<string, int>(int.Parse))
				.ToArray<int>();
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x00053F80 File Offset: 0x00052180
		public static List<uint> GetSkinColorGradientPoints(int race, int curGender, int age)
		{
			int skinColorCount = MBBodyProperties.GetSkinColorCount(race, curGender, age);
			List<uint> list = new List<uint>();
			Vec3[] array = new Vec3[skinColorCount];
			MBAPI.IMBFaceGen.GetSkinColorGradientPoints(race, curGender, (float)age, array);
			foreach (Vec3 vec in array)
			{
				list.Add(MBMath.ColorFromRGBA(vec.x, vec.y, vec.z, 1f));
			}
			return list;
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x00053FF0 File Offset: 0x000521F0
		public static List<bool> GetVoiceTypeUsableForPlayerData(int race, int curGender, float age, int voiceTypeCount)
		{
			bool[] array = new bool[voiceTypeCount];
			MBAPI.IMBFaceGen.GetVoiceTypeUsableForPlayerData(race, curGender, age, array);
			return new List<bool>(array);
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x00054018 File Offset: 0x00052218
		public static void SetHair(ref BodyProperties bodyProperties, int hair, int beard, int tattoo)
		{
			FaceGenerationParams faceGenerationParams = FaceGenerationParams.Create();
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, bodyProperties, false, false);
			if (hair > -1)
			{
				faceGenerationParams.CurrentHair = hair;
			}
			if (beard > -1)
			{
				faceGenerationParams.CurrentBeard = beard;
			}
			if (tattoo > -1)
			{
				faceGenerationParams.CurrentFaceTattoo = tattoo;
			}
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams, false, false, ref bodyProperties);
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x00054068 File Offset: 0x00052268
		public static void SetBody(ref BodyProperties bodyProperties, int build, int weight)
		{
			FaceGenerationParams faceGenerationParams = FaceGenerationParams.Create();
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, bodyProperties, false, false);
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams, false, false, ref bodyProperties);
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x0005409C File Offset: 0x0005229C
		public static void SetPigmentation(ref BodyProperties bodyProperties, int skinColor, int hairColor, int eyeColor)
		{
			FaceGenerationParams faceGenerationParams = FaceGenerationParams.Create();
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, bodyProperties, false, false);
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams, false, false, ref bodyProperties);
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x000540D4 File Offset: 0x000522D4
		public static void GenerateParentKey(BodyProperties childBodyProperties, int race, ref BodyProperties motherBodyProperties, ref BodyProperties fatherBodyProperties)
		{
			FaceGenerationParams faceGenerationParams = FaceGenerationParams.Create();
			FaceGenerationParams faceGenerationParams2 = FaceGenerationParams.Create();
			FaceGenerationParams faceGenerationParams3 = FaceGenerationParams.Create();
			MBBodyProperties.GenerationType[] array = new MBBodyProperties.GenerationType[4];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (MBBodyProperties.GenerationType)MBRandom.RandomInt(2);
			}
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, childBodyProperties, false, false);
			int faceGenInstancesLength = MBBodyProperties.GetFaceGenInstancesLength(race, faceGenerationParams.CurrentGender, (int)faceGenerationParams.CurrentAge);
			for (int j = 0; j < faceGenInstancesLength; j++)
			{
				DeformKeyData deformKeyData = MBBodyProperties.GetDeformKeyData(j, race, faceGenerationParams.CurrentGender, (int)faceGenerationParams.CurrentAge);
				if (deformKeyData.GroupId >= 0 && deformKeyData.GroupId != 0 && deformKeyData.GroupId != 5 && deformKeyData.GroupId != 6)
				{
					float num = MBRandom.RandomFloat * MathF.Min(faceGenerationParams.KeyWeights[j], 1f - faceGenerationParams.KeyWeights[j]);
					if (array[deformKeyData.GroupId - 1] == MBBodyProperties.GenerationType.FromMother)
					{
						faceGenerationParams3.KeyWeights[j] = faceGenerationParams.KeyWeights[j];
						faceGenerationParams2.KeyWeights[j] = faceGenerationParams.KeyWeights[j] + num;
					}
					else if (array[deformKeyData.GroupId - 1] == MBBodyProperties.GenerationType.FromFather)
					{
						faceGenerationParams2.KeyWeights[j] = faceGenerationParams.KeyWeights[j];
						faceGenerationParams3.KeyWeights[j] = faceGenerationParams.KeyWeights[j] + num;
					}
					else
					{
						faceGenerationParams3.KeyWeights[j] = faceGenerationParams.KeyWeights[j] + num;
						faceGenerationParams2.KeyWeights[j] = faceGenerationParams.KeyWeights[j] - num;
					}
				}
			}
			faceGenerationParams2.CurrentAge = faceGenerationParams.CurrentAge + (float)MBRandom.RandomInt(18, 25);
			float num2;
			faceGenerationParams2.SetRandomParamsExceptKeys(race, 0, (int)faceGenerationParams2.CurrentAge, out num2);
			faceGenerationParams2.CurrentFaceTattoo = 0;
			faceGenerationParams3.CurrentAge = faceGenerationParams.CurrentAge + (float)MBRandom.RandomInt(18, 22);
			float num3;
			faceGenerationParams3.SetRandomParamsExceptKeys(race, 1, (int)faceGenerationParams3.CurrentAge, out num3);
			faceGenerationParams3.CurrentFaceTattoo = 0;
			faceGenerationParams3.HeightMultiplier = faceGenerationParams2.HeightMultiplier * MBRandom.RandomFloatRanged(0.7f, 0.9f);
			if (faceGenerationParams3.CurrentHair == 0)
			{
				faceGenerationParams3.CurrentHair = 1;
			}
			float num4 = MBRandom.RandomFloat * MathF.Min(faceGenerationParams.CurrentSkinColorOffset, 1f - faceGenerationParams.CurrentSkinColorOffset);
			float num5 = MBRandom.RandomFloat * MathF.Min(faceGenerationParams.CurrentHairColorOffset, 1f - faceGenerationParams.CurrentHairColorOffset);
			int num6 = MBRandom.RandomInt(2);
			if (num6 == 1)
			{
				faceGenerationParams2.CurrentSkinColorOffset = faceGenerationParams.CurrentSkinColorOffset + num4;
				faceGenerationParams3.CurrentSkinColorOffset = faceGenerationParams.CurrentSkinColorOffset - num4;
			}
			else
			{
				faceGenerationParams2.CurrentSkinColorOffset = faceGenerationParams.CurrentSkinColorOffset - num4;
				faceGenerationParams3.CurrentSkinColorOffset = faceGenerationParams.CurrentSkinColorOffset + num4;
			}
			if (num6 == 1)
			{
				faceGenerationParams2.CurrentHairColorOffset = faceGenerationParams.CurrentHairColorOffset + num5;
				faceGenerationParams3.CurrentHairColorOffset = faceGenerationParams.CurrentHairColorOffset - num5;
			}
			else
			{
				faceGenerationParams2.CurrentHairColorOffset = faceGenerationParams.CurrentHairColorOffset - num5;
				faceGenerationParams3.CurrentHairColorOffset = faceGenerationParams.CurrentHairColorOffset + num5;
			}
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams3, false, false, ref motherBodyProperties);
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams2, false, false, ref fatherBodyProperties);
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x000543C4 File Offset: 0x000525C4
		public static BodyProperties GetBodyPropertiesWithAge(ref BodyProperties bodyProperties, float age)
		{
			FaceGenerationParams faceGenerationParams = default(FaceGenerationParams);
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, bodyProperties, false, false);
			faceGenerationParams.CurrentAge = age;
			BodyProperties bodyProperties2 = default(BodyProperties);
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams, false, false, ref bodyProperties2);
			return bodyProperties2;
		}

		// Token: 0x020004F3 RID: 1267
		public enum GenerationType
		{
			// Token: 0x04001CC9 RID: 7369
			FromMother,
			// Token: 0x04001CCA RID: 7370
			FromFather,
			// Token: 0x04001CCB RID: 7371
			Count
		}
	}
}
