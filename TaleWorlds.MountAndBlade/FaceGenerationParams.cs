using System;
using System.Runtime.InteropServices;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000224 RID: 548
	[EngineStruct("Face_generation_params", false, null)]
	public struct FaceGenerationParams
	{
		// Token: 0x06001FE9 RID: 8169 RVA: 0x0006E664 File Offset: 0x0006C864
		public static FaceGenerationParams Create()
		{
			FaceGenerationParams faceGenerationParams;
			faceGenerationParams.Seed = 0;
			faceGenerationParams.CurrentBeard = 0;
			faceGenerationParams.CurrentHair = 0;
			faceGenerationParams.CurrentEyebrow = 0;
			faceGenerationParams.IsHairFlipped = false;
			faceGenerationParams.CurrentRace = 0;
			faceGenerationParams.CurrentGender = 0;
			faceGenerationParams.CurrentFaceTexture = 0;
			faceGenerationParams.CurrentMouthTexture = 0;
			faceGenerationParams.CurrentFaceTattoo = 0;
			faceGenerationParams.CurrentVoice = 0;
			faceGenerationParams.HairFilter = 0;
			faceGenerationParams.BeardFilter = 0;
			faceGenerationParams.TattooFilter = 0;
			faceGenerationParams.FaceTextureFilter = 0;
			faceGenerationParams.TattooZeroProbability = 0f;
			faceGenerationParams.KeyWeights = new float[320];
			faceGenerationParams.CurrentAge = 0f;
			faceGenerationParams.CurrentWeight = 0f;
			faceGenerationParams.CurrentBuild = 0f;
			faceGenerationParams.CurrentSkinColorOffset = 0f;
			faceGenerationParams.CurrentHairColorOffset = 0f;
			faceGenerationParams.CurrentEyeColorOffset = 0f;
			faceGenerationParams.FaceDirtAmount = 0f;
			faceGenerationParams.CurrentFaceTattooColorOffset1 = 0f;
			faceGenerationParams.HeightMultiplier = 0f;
			faceGenerationParams.VoicePitch = 0f;
			faceGenerationParams.UseCache = false;
			faceGenerationParams.UseGpuMorph = false;
			faceGenerationParams.Padding2 = false;
			faceGenerationParams.FaceCacheId = 0;
			return faceGenerationParams;
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x0006E7A0 File Offset: 0x0006C9A0
		public void SetRaceGenderAndAdjustParams(int race, int gender, int curAge)
		{
			this.CurrentGender = gender;
			this.CurrentRace = race;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			float num8 = 0f;
			MBBodyProperties.GetParamsMax(race, gender, curAge, ref num, ref num2, ref num3, ref num4, ref num7, ref num6, ref num5, ref num8);
			this.CurrentHair = MBMath.ClampInt(this.CurrentHair, 0, num - 1);
			this.CurrentBeard = MBMath.ClampInt(this.CurrentBeard, 0, num2 - 1);
			this.CurrentFaceTexture = MBMath.ClampInt(this.CurrentFaceTexture, 0, num3 - 1);
			this.CurrentMouthTexture = MBMath.ClampInt(this.CurrentMouthTexture, 0, num4 - 1);
			this.CurrentFaceTattoo = MBMath.ClampInt(this.CurrentFaceTattoo, 0, num7 - 1);
			this.CurrentVoice = MBMath.ClampInt(this.CurrentVoice, 0, num6 - 1);
			this.VoicePitch = MBMath.ClampFloat(this.VoicePitch, 0f, 1f);
			this.CurrentEyebrow = MBMath.ClampInt(this.CurrentEyebrow, 0, num5 - 1);
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x0006E89C File Offset: 0x0006CA9C
		public void SetRandomParamsExceptKeys(int race, int gender, int minAge, out float scale)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			scale = 0f;
			MBBodyProperties.GetParamsMax(race, gender, minAge, ref num, ref num2, ref num3, ref num4, ref num7, ref num6, ref num5, ref scale);
			this.CurrentHair = MBRandom.RandomInt(num);
			this.CurrentBeard = MBRandom.RandomInt(num2);
			this.CurrentFaceTexture = MBRandom.RandomInt(num3);
			this.CurrentMouthTexture = MBRandom.RandomInt(num4);
			this.CurrentFaceTattoo = MBRandom.RandomInt(num7);
			this.CurrentVoice = MBRandom.RandomInt(num6);
			this.VoicePitch = MBRandom.RandomFloat;
			this.CurrentEyebrow = MBRandom.RandomInt(num5);
			this.CurrentSkinColorOffset = MBRandom.RandomFloat;
			this.CurrentHairColorOffset = MBRandom.RandomFloat;
			this.CurrentEyeColorOffset = MBRandom.RandomFloat;
			this.CurrentFaceTattooColorOffset1 = MBRandom.RandomFloat;
			this.HeightMultiplier = MBRandom.RandomFloat;
		}

		// Token: 0x04000AE3 RID: 2787
		public int Seed;

		// Token: 0x04000AE4 RID: 2788
		public int CurrentBeard;

		// Token: 0x04000AE5 RID: 2789
		public int CurrentHair;

		// Token: 0x04000AE6 RID: 2790
		public int CurrentEyebrow;

		// Token: 0x04000AE7 RID: 2791
		public int CurrentRace;

		// Token: 0x04000AE8 RID: 2792
		public int CurrentGender;

		// Token: 0x04000AE9 RID: 2793
		public int CurrentFaceTexture;

		// Token: 0x04000AEA RID: 2794
		public int CurrentMouthTexture;

		// Token: 0x04000AEB RID: 2795
		public int CurrentFaceTattoo;

		// Token: 0x04000AEC RID: 2796
		public int CurrentVoice;

		// Token: 0x04000AED RID: 2797
		public int HairFilter;

		// Token: 0x04000AEE RID: 2798
		public int BeardFilter;

		// Token: 0x04000AEF RID: 2799
		public int TattooFilter;

		// Token: 0x04000AF0 RID: 2800
		public int FaceTextureFilter;

		// Token: 0x04000AF1 RID: 2801
		public float TattooZeroProbability;

		// Token: 0x04000AF2 RID: 2802
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 320)]
		public float[] KeyWeights;

		// Token: 0x04000AF3 RID: 2803
		public float CurrentAge;

		// Token: 0x04000AF4 RID: 2804
		public float CurrentWeight;

		// Token: 0x04000AF5 RID: 2805
		public float CurrentBuild;

		// Token: 0x04000AF6 RID: 2806
		public float CurrentSkinColorOffset;

		// Token: 0x04000AF7 RID: 2807
		public float CurrentHairColorOffset;

		// Token: 0x04000AF8 RID: 2808
		public float CurrentEyeColorOffset;

		// Token: 0x04000AF9 RID: 2809
		public float FaceDirtAmount;

		// Token: 0x04000AFA RID: 2810
		public float CurrentFaceTattooColorOffset1;

		// Token: 0x04000AFB RID: 2811
		public float HeightMultiplier;

		// Token: 0x04000AFC RID: 2812
		public float VoicePitch;

		// Token: 0x04000AFD RID: 2813
		[MarshalAs(UnmanagedType.U1)]
		public bool IsHairFlipped;

		// Token: 0x04000AFE RID: 2814
		[MarshalAs(UnmanagedType.U1)]
		public bool UseCache;

		// Token: 0x04000AFF RID: 2815
		[MarshalAs(UnmanagedType.U1)]
		public bool UseGpuMorph;

		// Token: 0x04000B00 RID: 2816
		[MarshalAs(UnmanagedType.U1)]
		public bool Padding2;

		// Token: 0x04000B01 RID: 2817
		public int FaceCacheId;
	}
}
