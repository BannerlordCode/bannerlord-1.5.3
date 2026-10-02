using System;
using System.Runtime.InteropServices;
using TaleWorlds.Core;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000223 RID: 547
	[EngineStruct("Skin_generation_params", false, null)]
	public struct SkinGenerationParams
	{
		// Token: 0x06001FE7 RID: 8167 RVA: 0x0006E570 File Offset: 0x0006C770
		public static SkinGenerationParams Create()
		{
			SkinGenerationParams skinGenerationParams;
			skinGenerationParams._skinMeshesVisibilityMask = 481;
			skinGenerationParams._underwearType = Equipment.UnderwearTypes.FullUnderwear;
			skinGenerationParams._bodyMeshType = 0;
			skinGenerationParams._hairCoverType = 0;
			skinGenerationParams._beardCoverType = 0;
			skinGenerationParams._prepareImmediately = false;
			skinGenerationParams._bodyDeformType = -1;
			skinGenerationParams._faceDirtAmount = 0f;
			skinGenerationParams._gender = 0;
			skinGenerationParams._race = 0;
			skinGenerationParams._useTranslucency = false;
			skinGenerationParams._useTesselation = false;
			skinGenerationParams._faceCacheId = 0;
			return skinGenerationParams;
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x0006E5F0 File Offset: 0x0006C7F0
		public SkinGenerationParams(int skinMeshesVisibilityMask, Equipment.UnderwearTypes underwearType, int bodyMeshType, int hairCoverType, int beardCoverType, int bodyDeformType, bool prepareImmediately, float faceDirtAmount, int gender, int race, bool useTranslucency, bool useTesselation, int faceCacheID)
		{
			this._skinMeshesVisibilityMask = skinMeshesVisibilityMask;
			this._underwearType = underwearType;
			this._bodyMeshType = bodyMeshType;
			this._hairCoverType = hairCoverType;
			this._beardCoverType = beardCoverType;
			this._bodyDeformType = bodyDeformType;
			this._prepareImmediately = prepareImmediately;
			this._faceDirtAmount = faceDirtAmount;
			this._gender = gender;
			this._race = race;
			this._useTranslucency = useTranslucency;
			this._useTesselation = useTesselation;
			this._faceCacheId = faceCacheID;
		}

		// Token: 0x04000AD6 RID: 2774
		public int _skinMeshesVisibilityMask;

		// Token: 0x04000AD7 RID: 2775
		public Equipment.UnderwearTypes _underwearType;

		// Token: 0x04000AD8 RID: 2776
		public int _bodyMeshType;

		// Token: 0x04000AD9 RID: 2777
		public int _hairCoverType;

		// Token: 0x04000ADA RID: 2778
		public int _beardCoverType;

		// Token: 0x04000ADB RID: 2779
		public int _bodyDeformType;

		// Token: 0x04000ADC RID: 2780
		[MarshalAs(UnmanagedType.U1)]
		public bool _prepareImmediately;

		// Token: 0x04000ADD RID: 2781
		[MarshalAs(UnmanagedType.U1)]
		public bool _useTranslucency;

		// Token: 0x04000ADE RID: 2782
		[MarshalAs(UnmanagedType.U1)]
		public bool _useTesselation;

		// Token: 0x04000ADF RID: 2783
		public float _faceDirtAmount;

		// Token: 0x04000AE0 RID: 2784
		public int _gender;

		// Token: 0x04000AE1 RID: 2785
		public int _race;

		// Token: 0x04000AE2 RID: 2786
		public int _faceCacheId;
	}
}
