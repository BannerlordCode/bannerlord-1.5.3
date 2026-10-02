using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000056 RID: 86
	[EngineClass("rglLight")]
	public sealed class Light : GameEntityComponent
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x00006E43 File Offset: 0x00005043
		public bool IsValid
		{
			get
			{
				return base.Pointer != UIntPtr.Zero;
			}
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00006E55 File Offset: 0x00005055
		internal Light(UIntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x00006E5E File Offset: 0x0000505E
		public static Light CreatePointLight(float lightRadius)
		{
			return EngineApplicationInterface.ILight.CreatePointLight(lightRadius);
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x00006E6C File Offset: 0x0000506C
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x00006E8C File Offset: 0x0000508C
		public MatrixFrame Frame
		{
			get
			{
				MatrixFrame matrixFrame;
				EngineApplicationInterface.ILight.GetFrame(base.Pointer, out matrixFrame);
				return matrixFrame;
			}
			set
			{
				EngineApplicationInterface.ILight.SetFrame(base.Pointer, ref value);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x00006EA0 File Offset: 0x000050A0
		// (set) Token: 0x060008BD RID: 2237 RVA: 0x00006EB2 File Offset: 0x000050B2
		public Vec3 LightColor
		{
			get
			{
				return EngineApplicationInterface.ILight.GetLightColor(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.ILight.SetLightColor(base.Pointer, value);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x00006EC5 File Offset: 0x000050C5
		// (set) Token: 0x060008BF RID: 2239 RVA: 0x00006ED7 File Offset: 0x000050D7
		public float Intensity
		{
			get
			{
				return EngineApplicationInterface.ILight.GetIntensity(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.ILight.SetIntensity(base.Pointer, value);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00006EEA File Offset: 0x000050EA
		// (set) Token: 0x060008C1 RID: 2241 RVA: 0x00006EFC File Offset: 0x000050FC
		public float Radius
		{
			get
			{
				return EngineApplicationInterface.ILight.GetRadius(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.ILight.SetRadius(base.Pointer, value);
			}
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00006F0F File Offset: 0x0000510F
		public void SetShadowType(Light.ShadowType type)
		{
			EngineApplicationInterface.ILight.SetShadows(base.Pointer, (int)type);
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x00006F22 File Offset: 0x00005122
		// (set) Token: 0x060008C4 RID: 2244 RVA: 0x00006F34 File Offset: 0x00005134
		public bool ShadowEnabled
		{
			get
			{
				return EngineApplicationInterface.ILight.IsShadowEnabled(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.ILight.EnableShadow(base.Pointer, value);
			}
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00006F47 File Offset: 0x00005147
		public void SetLightFlicker(float magnitude, float interval)
		{
			EngineApplicationInterface.ILight.SetLightFlicker(base.Pointer, magnitude, interval);
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00006F5B File Offset: 0x0000515B
		public void SetVolumetricProperties(bool volumetricLightEnabled, float volumeParameters)
		{
			EngineApplicationInterface.ILight.SetVolumetricProperties(base.Pointer, volumetricLightEnabled, volumeParameters);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00006F6F File Offset: 0x0000516F
		public void Dispose()
		{
			if (this.IsValid)
			{
				this.Release();
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00006F85 File Offset: 0x00005185
		public void SetVisibility(bool value)
		{
			EngineApplicationInterface.ILight.SetVisibility(base.Pointer, value);
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00006F98 File Offset: 0x00005198
		private void Release()
		{
			EngineApplicationInterface.ILight.Release(base.Pointer);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00006FAC File Offset: 0x000051AC
		~Light()
		{
			this.Dispose();
		}

		// Token: 0x020000C4 RID: 196
		public enum ShadowType
		{
			// Token: 0x040003EA RID: 1002
			NoShadow,
			// Token: 0x040003EB RID: 1003
			StaticShadow,
			// Token: 0x040003EC RID: 1004
			DynamicShadow,
			// Token: 0x040003ED RID: 1005
			Count
		}
	}
}
