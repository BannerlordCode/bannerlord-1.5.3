using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200002C RID: 44
	[ApplicationInterfaceBase]
	internal interface ILight
	{
		// Token: 0x060004F2 RID: 1266
		[EngineMethod("create_point_light", false, null, false)]
		Light CreatePointLight(float lightRadius);

		// Token: 0x060004F3 RID: 1267
		[EngineMethod("set_radius", false, null, false)]
		void SetRadius(UIntPtr lightpointer, float radius);

		// Token: 0x060004F4 RID: 1268
		[EngineMethod("set_light_flicker", false, null, false)]
		void SetLightFlicker(UIntPtr lightpointer, float magnitude, float interval);

		// Token: 0x060004F5 RID: 1269
		[EngineMethod("enable_shadow", false, null, false)]
		void EnableShadow(UIntPtr lightpointer, bool shadowEnabled);

		// Token: 0x060004F6 RID: 1270
		[EngineMethod("is_shadow_enabled", false, null, false)]
		bool IsShadowEnabled(UIntPtr lightpointer);

		// Token: 0x060004F7 RID: 1271
		[EngineMethod("set_volumetric_properties", false, null, false)]
		void SetVolumetricProperties(UIntPtr lightpointer, bool volumelightenabled, float volumeparameter);

		// Token: 0x060004F8 RID: 1272
		[EngineMethod("set_visibility", false, null, false)]
		void SetVisibility(UIntPtr lightpointer, bool value);

		// Token: 0x060004F9 RID: 1273
		[EngineMethod("get_radius", false, null, false)]
		float GetRadius(UIntPtr lightpointer);

		// Token: 0x060004FA RID: 1274
		[EngineMethod("set_shadows", false, null, false)]
		void SetShadows(UIntPtr lightPointer, int shadowType);

		// Token: 0x060004FB RID: 1275
		[EngineMethod("set_light_color", false, null, false)]
		void SetLightColor(UIntPtr lightpointer, Vec3 color);

		// Token: 0x060004FC RID: 1276
		[EngineMethod("get_light_color", false, null, false)]
		Vec3 GetLightColor(UIntPtr lightpointer);

		// Token: 0x060004FD RID: 1277
		[EngineMethod("set_intensity", false, null, false)]
		void SetIntensity(UIntPtr lightPointer, float value);

		// Token: 0x060004FE RID: 1278
		[EngineMethod("get_intensity", false, null, false)]
		float GetIntensity(UIntPtr lightPointer);

		// Token: 0x060004FF RID: 1279
		[EngineMethod("release", false, null, false)]
		void Release(UIntPtr lightpointer);

		// Token: 0x06000500 RID: 1280
		[EngineMethod("set_frame", false, null, false)]
		void SetFrame(UIntPtr lightPointer, ref MatrixFrame frame);

		// Token: 0x06000501 RID: 1281
		[EngineMethod("get_frame", false, null, false)]
		void GetFrame(UIntPtr lightPointer, out MatrixFrame result);
	}
}
