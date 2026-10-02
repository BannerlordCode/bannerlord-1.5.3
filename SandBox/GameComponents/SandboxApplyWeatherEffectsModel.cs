using System;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace SandBox.GameComponents
{
	// Token: 0x020000C5 RID: 197
	public class SandboxApplyWeatherEffectsModel : ApplyWeatherEffectsModel
	{
		// Token: 0x0600082B RID: 2091 RVA: 0x00039C20 File Offset: 0x00037E20
		public override void ApplyWeatherEffects()
		{
			Scene scene = Mission.Current.Scene;
			if (scene != null)
			{
				bool flag = scene.GetRainDensity() > 0f;
				bool flag2 = scene.GetSnowDensity() > 0f;
				bool flag3 = flag || flag2;
				bool flag4 = scene.GetFog() > 0f;
				Mission.Current.SetBowMissileSpeedModifier(flag3 ? 0.9f : 1f);
				Mission.Current.SetCrossbowMissileSpeedModifier(flag3 ? 0.9f : 1f);
				Mission.Current.SetMissileRangeModifier(flag4 ? 0.8f : 1f);
			}
		}
	}
}
