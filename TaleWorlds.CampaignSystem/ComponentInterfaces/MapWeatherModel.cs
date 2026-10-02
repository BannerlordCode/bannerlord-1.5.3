using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C2 RID: 450
	public abstract class MapWeatherModel : MBGameModel<MapWeatherModel>
	{
		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06001E53 RID: 7763
		public abstract CampaignTime WeatherUpdateFrequency { get; }

		// Token: 0x06001E54 RID: 7764
		public abstract AtmosphereState GetInterpolatedAtmosphereState(CampaignTime timeOfYear, Vec3 pos);

		// Token: 0x06001E55 RID: 7765
		public abstract AtmosphereInfo GetAtmosphereModel(CampaignVec2 position);

		// Token: 0x06001E56 RID: 7766
		public abstract void GetSeasonTimeFactorOfCampaignTime(CampaignTime ct, out float timeFactorForSnow, out float timeFactorForRain, bool snapCampaignTimeToWeatherPeriod = true);

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06001E57 RID: 7767
		public abstract CampaignTime WeatherUpdatePeriod { get; }

		// Token: 0x06001E58 RID: 7768
		public abstract MapWeatherModel.WeatherEvent UpdateWeatherForPosition(CampaignVec2 position, CampaignTime ct);

		// Token: 0x06001E59 RID: 7769
		public abstract void InitializeCaches();

		// Token: 0x06001E5A RID: 7770
		public abstract MapWeatherModel.WeatherEvent GetWeatherEventInPosition(Vec2 pos);

		// Token: 0x06001E5B RID: 7771
		public abstract void GetSnowAndRainDataForPosition(Vec2 position, CampaignTime ct, out float snowValue, out float rainValue);

		// Token: 0x06001E5C RID: 7772
		public abstract MapWeatherModel.WeatherEventEffectOnTerrain GetWeatherEffectOnTerrainForPosition(Vec2 pos);

		// Token: 0x06001E5D RID: 7773
		public abstract Vec2 GetWindForPosition(CampaignVec2 position);

		// Token: 0x0200062E RID: 1582
		public enum WeatherEvent
		{
			// Token: 0x04001A30 RID: 6704
			Clear,
			// Token: 0x04001A31 RID: 6705
			LightRain,
			// Token: 0x04001A32 RID: 6706
			HeavyRain,
			// Token: 0x04001A33 RID: 6707
			Snowy,
			// Token: 0x04001A34 RID: 6708
			Blizzard,
			// Token: 0x04001A35 RID: 6709
			Storm
		}

		// Token: 0x0200062F RID: 1583
		public enum WeatherEventEffectOnTerrain
		{
			// Token: 0x04001A37 RID: 6711
			Default,
			// Token: 0x04001A38 RID: 6712
			Wet
		}
	}
}
