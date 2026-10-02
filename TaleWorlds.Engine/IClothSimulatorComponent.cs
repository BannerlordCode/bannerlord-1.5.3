using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000020 RID: 32
	[ApplicationInterfaceBase]
	internal interface IClothSimulatorComponent
	{
		// Token: 0x060001C8 RID: 456
		[EngineMethod("set_maxdistance_multiplier", false, null, false)]
		void SetMaxDistanceMultiplier(UIntPtr cloth_pointer, float multiplier);

		// Token: 0x060001C9 RID: 457
		[EngineMethod("set_forced_wind", false, null, false)]
		void SetForcedWind(UIntPtr cloth_pointer, Vec3 windVector, bool isLocal);

		// Token: 0x060001CA RID: 458
		[EngineMethod("disable_forced_wind", false, null, false)]
		void DisableForcedWind(UIntPtr cloth_pointer);

		// Token: 0x060001CB RID: 459
		[EngineMethod("set_forced_gust_strength", false, null, false)]
		void SetForcedGustStrength(UIntPtr cloth_pointer, float gustStrength);

		// Token: 0x060001CC RID: 460
		[EngineMethod("set_reset_required", false, null, false)]
		void SetResetRequired(UIntPtr cloth_pointer);

		// Token: 0x060001CD RID: 461
		[EngineMethod("disable_morph_animation", false, null, false)]
		void DisableMorphAnimation(UIntPtr cloth_pointer);

		// Token: 0x060001CE RID: 462
		[EngineMethod("set_morph_animation", false, null, false)]
		void SetMorphAnimation(UIntPtr cloth_pointer, float morphKey);

		// Token: 0x060001CF RID: 463
		[EngineMethod("get_number_of_morph_keys", false, null, false)]
		int GetNumberOfMorphKeys(UIntPtr cloth_pointer);

		// Token: 0x060001D0 RID: 464
		[EngineMethod("set_vector_argument", false, null, false)]
		void SetVectorArgument(UIntPtr cloth_pointer, float x, float y, float z, float w);

		// Token: 0x060001D1 RID: 465
		[EngineMethod("get_morph_anim_left_points", false, null, false)]
		void GetMorphAnimLeftPoints(UIntPtr cloth_pointer, Vec3[] leftPoints);

		// Token: 0x060001D2 RID: 466
		[EngineMethod("get_morph_anim_right_points", false, null, false)]
		void GetMorphAnimRightPoints(UIntPtr cloth_pointer, Vec3[] rightPoints);

		// Token: 0x060001D3 RID: 467
		[EngineMethod("get_morph_anim_center_points", false, null, false)]
		void GetMorphAnimCenterPoints(UIntPtr cloth_pointer, Vec3[] leftPoints);

		// Token: 0x060001D4 RID: 468
		[EngineMethod("set_forced_velocity", false, null, true)]
		void SetForcedVelocity(UIntPtr cloth_pointer, in Vec3 velocity);
	}
}
