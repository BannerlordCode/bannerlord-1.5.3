using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000072 RID: 114
	public class MissionFaceCacheView : MissionView
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x00020188 File Offset: 0x0001E388
		public MissionFaceCacheView()
		{
			this._totalFaceBudget = (NativeConfig.CharacterDetail + 1) * 100;
			this._randomGenerator = new MBFastRandom((uint)(Time.ApplicationTime * 73f));
			this._currentSimilarityThreshold = 25f;
			this._comprasionThresholdsWrtEmptyBudget = new KeyValuePair<float, float>[5];
			this._comprasionThresholdsWrtEmptyBudget[0] = new KeyValuePair<float, float>(0.2f, 100f);
			this._comprasionThresholdsWrtEmptyBudget[1] = new KeyValuePair<float, float>(0.4f, 200f);
			this._comprasionThresholdsWrtEmptyBudget[2] = new KeyValuePair<float, float>(0.6f, 450f);
			this._comprasionThresholdsWrtEmptyBudget[3] = new KeyValuePair<float, float>(0.8f, 750f);
			this._comprasionThresholdsWrtEmptyBudget[4] = new KeyValuePair<float, float>(1f, 5000f);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00020276 File Offset: 0x0001E476
		public override void OnPreMissionTick(float dt)
		{
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00020278 File Offset: 0x0001E478
		public override void OnBehaviorInitialize()
		{
			Mission.Current.OnComputeTroopBodyProperties += this.GetRandomBodyPropertyForTroop;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00020290 File Offset: 0x0001E490
		public override void OnMissionScreenFinalize()
		{
			Mission.Current.OnComputeTroopBodyProperties -= this.GetRandomBodyPropertyForTroop;
			FaceGen.FlushFaceCache();
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x000202B0 File Offset: 0x0001E4B0
		private float ComputeSimilarityOfFace(FaceGenerationParams f0, FaceGenerationParams f1, ArmorComponent.HairCoverTypes hairCover1, ArmorComponent.HairCoverTypes hairCover2, ArmorComponent.BeardCoverTypes beardCover1, ArmorComponent.BeardCoverTypes beardCover2)
		{
			float num = 0f;
			if (hairCover1 != hairCover2)
			{
				num += 1000000f;
			}
			if (beardCover1 != beardCover2)
			{
				num += 1000000f;
			}
			if (f0.CurrentBeard != f1.CurrentBeard)
			{
				num += 10f;
			}
			if (f0.CurrentHair != f1.CurrentHair)
			{
				num += 10f;
			}
			if (f0.CurrentEyebrow != f1.CurrentEyebrow)
			{
				num += 10f;
			}
			if (f0.CurrentRace != f1.CurrentRace)
			{
				num += 10f;
			}
			if (f0.CurrentGender != f1.CurrentGender)
			{
				num += 1000f;
			}
			if (f0.CurrentFaceTexture != f1.CurrentFaceTexture)
			{
				num += 10f;
			}
			if (f0.CurrentMouthTexture != f1.CurrentMouthTexture)
			{
				num += 5f;
			}
			if (f0.CurrentFaceTattoo != f1.CurrentFaceTattoo)
			{
				num += 250f;
			}
			float num2 = 32.5f;
			for (int i = 0; i < f0.KeyWeights.Length; i++)
			{
				num += MathF.Abs(f0.KeyWeights[i] - f1.KeyWeights[i]) * num2;
			}
			return num;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000203C4 File Offset: 0x0001E5C4
		private int CheckForSimilarFacesFromCache(FaceGenerationParams newFaceGen, ArmorComponent.HairCoverTypes hairCoverType, ArmorComponent.BeardCoverTypes beardCoverType)
		{
			int num = -1;
			float num2 = 1E+09f;
			for (int i = 0; i < this._uniqueCacheIndex; i++)
			{
				float num3 = this.ComputeSimilarityOfFace(newFaceGen, this._alreadyAssignedFaces[i].FaceParamsForSimilarity, hairCoverType, this._alreadyAssignedFaces[i].HairCover, beardCoverType, this._alreadyAssignedFaces[i].BeardCover);
				if (num3 < this._currentSimilarityThreshold && this._randomGenerator.NextFloat() > this._currentRandomSwitchChance)
				{
					return i;
				}
				if (num2 < num3 && (this._randomGenerator.NextFloat() > this._currentRandomSwitchChance || num == -1))
				{
					num = i;
					num2 = num3;
				}
			}
			if (this._uniqueCacheIndex == this._totalFaceBudget)
			{
				return num;
			}
			return -1;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00020478 File Offset: 0x0001E678
		private void UpdateFaceSimilarityThreshold()
		{
			float num = (float)this._uniqueCacheIndex / (float)this._totalFaceBudget;
			float num2 = 0.4f;
			float num3 = 0.7f;
			float num4 = (num - num2) / (num3 - num2);
			num4 = MathF.Clamp(num4, 0f, 1f);
			this._currentRandomSwitchChance = 0.37f * num4;
			if (num < this._comprasionThresholdsWrtEmptyBudget[0].Key)
			{
				this._currentSimilarityThreshold = this._comprasionThresholdsWrtEmptyBudget[0].Value;
				this._currentRandomSwitchChance = 0f;
				return;
			}
			for (int i = 1; i < this._comprasionThresholdsWrtEmptyBudget.Count<KeyValuePair<float, float>>(); i++)
			{
				if (this._comprasionThresholdsWrtEmptyBudget[i].Key > num)
				{
					float value = this._comprasionThresholdsWrtEmptyBudget[i - 1].Value;
					float value2 = this._comprasionThresholdsWrtEmptyBudget[i].Value;
					float key = this._comprasionThresholdsWrtEmptyBudget[i - 1].Key;
					float key2 = this._comprasionThresholdsWrtEmptyBudget[i].Key;
					float num5 = (num - key) / (key2 - key);
					num5 = MathF.Clamp(num5, 0f, 1f);
					this._currentSimilarityThreshold = value + (value2 - value) * num5;
					return;
				}
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000205BC File Offset: 0x0001E7BC
		private BodyProperties GetRandomBodyPropertyForTroop(AgentBuildData agentBuildData, BasicCharacterObject characterObject, Equipment equipment, int seed)
		{
			if (characterObject.IsHero)
			{
				return characterObject.GetBodyProperties(equipment, seed);
			}
			ArmorComponent.HairCoverTypes hairCoverType = equipment.HairCoverType;
			ArmorComponent.BeardCoverTypes beardCoverType = equipment.BeardCoverType;
			bool earsAreHidden = equipment.EarsAreHidden;
			bool mouthIsHidden = equipment.MouthIsHidden;
			BodyProperties bodyProperties = characterObject.GetBodyProperties(equipment, seed);
			FaceGenerationParams faceGenerationParams = default(FaceGenerationParams);
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, bodyProperties, earsAreHidden, mouthIsHidden);
			int num = this.CheckForSimilarFacesFromCache(faceGenerationParams, hairCoverType, beardCoverType);
			if (num != -1)
			{
				BodyProperties bodyProperties2 = new BodyProperties(bodyProperties.DynamicProperties, this._alreadyAssignedFaces[num].BodyProperties.StaticProperties);
				agentBuildData.FaceCacheId = this._alreadyAssignedFaces[num].CacheID;
				return bodyProperties2;
			}
			agentBuildData.FaceCacheId = this._uniqueCacheIndex;
			MissionFaceCacheView.CacheRecord cacheRecord = default(MissionFaceCacheView.CacheRecord);
			cacheRecord.BodyProperties = bodyProperties;
			cacheRecord.CacheID = this._uniqueCacheIndex;
			cacheRecord.FaceParamsForSimilarity = faceGenerationParams;
			cacheRecord.HairCover = hairCoverType;
			cacheRecord.BeardCover = beardCoverType;
			this._alreadyAssignedFaces.Add(cacheRecord);
			this._uniqueCacheIndex++;
			this.UpdateFaceSimilarityThreshold();
			return bodyProperties;
		}

		// Token: 0x0400027C RID: 636
		private int _totalFaceBudget = 250;

		// Token: 0x0400027D RID: 637
		private int _uniqueCacheIndex;

		// Token: 0x0400027E RID: 638
		private float _currentSimilarityThreshold;

		// Token: 0x0400027F RID: 639
		private float _currentRandomSwitchChance;

		// Token: 0x04000280 RID: 640
		private KeyValuePair<float, float>[] _comprasionThresholdsWrtEmptyBudget;

		// Token: 0x04000281 RID: 641
		private List<MissionFaceCacheView.CacheRecord> _alreadyAssignedFaces = new List<MissionFaceCacheView.CacheRecord>();

		// Token: 0x04000282 RID: 642
		private MBFastRandom _randomGenerator;

		// Token: 0x020000DA RID: 218
		private struct CacheRecord
		{
			// Token: 0x040003E1 RID: 993
			public BodyProperties BodyProperties;

			// Token: 0x040003E2 RID: 994
			public int CacheID;

			// Token: 0x040003E3 RID: 995
			public FaceGenerationParams FaceParamsForSimilarity;

			// Token: 0x040003E4 RID: 996
			public ArmorComponent.HairCoverTypes HairCover;

			// Token: 0x040003E5 RID: 997
			public ArmorComponent.BeardCoverTypes BeardCover;
		}
	}
}
