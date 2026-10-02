using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001BF RID: 447
	[ScriptingInterfaceBase]
	internal interface IMBFaceGen
	{
		// Token: 0x0600193E RID: 6462
		[EngineMethod("get_num_editable_deform_keys", false, null, false)]
		int GetNumEditableDeformKeys(int race, bool initialGender, float age);

		// Token: 0x0600193F RID: 6463
		[EngineMethod("get_params_from_key", false, null, false)]
		void GetParamsFromKey(ref FaceGenerationParams faceGenerationParams, ref BodyProperties bodyProperties, bool earsAreHidden, bool mouthHidden);

		// Token: 0x06001940 RID: 6464
		[EngineMethod("get_params_max", false, null, false)]
		void GetParamsMax(int race, int curGender, float curAge, ref int hairNum, ref int beardNum, ref int faceTextureNum, ref int mouthTextureNum, ref int faceTattooNum, ref int soundNum, ref int eyebrowNum, ref float scale);

		// Token: 0x06001941 RID: 6465
		[EngineMethod("get_zero_probabilities", false, null, false)]
		void GetZeroProbabilities(int race, int curGender, float curAge, ref float tattooZeroProbability);

		// Token: 0x06001942 RID: 6466
		[EngineMethod("produce_numeric_key_with_params", false, null, false)]
		void ProduceNumericKeyWithParams(ref FaceGenerationParams faceGenerationParams, bool earsAreHidden, bool mouthIsHidden, ref BodyProperties bodyProperties);

		// Token: 0x06001943 RID: 6467
		[EngineMethod("produce_numeric_key_with_default_values", false, null, false)]
		void ProduceNumericKeyWithDefaultValues(ref BodyProperties initialBodyProperties, bool earsAreHidden, bool mouthIsHidden, int race, int gender, float age);

		// Token: 0x06001944 RID: 6468
		[EngineMethod("transform_face_keys_to_default_face", false, null, false)]
		void TransformFaceKeysToDefaultFace(ref FaceGenerationParams faceGenerationParams);

		// Token: 0x06001945 RID: 6469
		[EngineMethod("get_random_body_properties", false, null, false)]
		void GetRandomBodyProperties(int race, int gender, ref BodyProperties bodyPropertiesMin, ref BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tatooTags, float variationAmount, ref BodyProperties outBodyProperties);

		// Token: 0x06001946 RID: 6470
		[EngineMethod("enforce_constraints", false, null, false)]
		bool EnforceConstraints(ref FaceGenerationParams faceGenerationParams);

		// Token: 0x06001947 RID: 6471
		[EngineMethod("get_deform_key_data", false, null, false)]
		void GetDeformKeyData(int keyNo, ref DeformKeyData deformKeyData, int race, int gender, float age);

		// Token: 0x06001948 RID: 6472
		[EngineMethod("get_face_gen_instances_length", false, null, false)]
		int GetFaceGenInstancesLength(int race, int gender, float age);

		// Token: 0x06001949 RID: 6473
		[EngineMethod("get_scale", false, null, false)]
		float GetScaleFromKey(int race, int gender, ref BodyProperties initialBodyProperties);

		// Token: 0x0600194A RID: 6474
		[EngineMethod("get_voice_records_count", false, null, false)]
		int GetVoiceRecordsCount(int race, int curGender, float age);

		// Token: 0x0600194B RID: 6475
		[EngineMethod("get_hair_color_count", false, null, false)]
		int GetHairColorCount(int race, int curGender, float age);

		// Token: 0x0600194C RID: 6476
		[EngineMethod("get_hair_color_gradient_points", false, null, false)]
		void GetHairColorGradientPoints(int race, int curGender, float age, Vec3[] colors);

		// Token: 0x0600194D RID: 6477
		[EngineMethod("get_tatoo_color_count", false, null, false)]
		int GetTatooColorCount(int race, int curGender, float age);

		// Token: 0x0600194E RID: 6478
		[EngineMethod("get_tatoo_color_gradient_points", false, null, false)]
		void GetTatooColorGradientPoints(int race, int curGender, float age, Vec3[] colors);

		// Token: 0x0600194F RID: 6479
		[EngineMethod("get_skin_color_count", false, null, false)]
		int GetSkinColorCount(int race, int curGender, float age);

		// Token: 0x06001950 RID: 6480
		[EngineMethod("get_maturity_type", false, null, false)]
		int GetMaturityType(float age);

		// Token: 0x06001951 RID: 6481
		[EngineMethod("flush_face_cache", false, null, false)]
		void FlushFaceCache();

		// Token: 0x06001952 RID: 6482
		[EngineMethod("get_voice_type_usable_for_player_data", false, null, false)]
		void GetVoiceTypeUsableForPlayerData(int race, int curGender, float age, bool[] aiArray);

		// Token: 0x06001953 RID: 6483
		[EngineMethod("get_skin_color_gradient_points", false, null, false)]
		void GetSkinColorGradientPoints(int race, int curGender, float age, Vec3[] colors);

		// Token: 0x06001954 RID: 6484
		[EngineMethod("get_race_ids", false, null, false)]
		string GetRaceIds();

		// Token: 0x06001955 RID: 6485
		[EngineMethod("get_hair_indices_by_tag", false, null, false)]
		string GetHairIndicesByTag(int race, int curGender, float age, string tag);

		// Token: 0x06001956 RID: 6486
		[EngineMethod("get_facial_indices_by_tag", false, null, false)]
		string GetFacialIndicesByTag(int race, int curGender, float age, string tag);

		// Token: 0x06001957 RID: 6487
		[EngineMethod("get_tattoo_indices_by_tag", false, null, false)]
		string GetTattooIndicesByTag(int race, int curGender, float age, string tag);
	}
}
