using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000131 RID: 305
	public class DefaultMapWeatherModel : MapWeatherModel
	{
		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06001931 RID: 6449 RVA: 0x0007AC55 File Offset: 0x00078E55
		private float SunRiseNorm
		{
			get
			{
				return (float)CampaignTime.SunRise / (float)CampaignTime.HoursInDay;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06001932 RID: 6450 RVA: 0x0007AC64 File Offset: 0x00078E64
		private float SunSetNorm
		{
			get
			{
				return (float)CampaignTime.SunSet / (float)CampaignTime.HoursInDay;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06001933 RID: 6451 RVA: 0x0007AC73 File Offset: 0x00078E73
		private float DayTime
		{
			get
			{
				return (float)(CampaignTime.SunSet - CampaignTime.SunRise);
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06001934 RID: 6452 RVA: 0x0007AC81 File Offset: 0x00078E81
		public override CampaignTime WeatherUpdatePeriod
		{
			get
			{
				return CampaignTime.Hours(4f);
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06001935 RID: 6453 RVA: 0x0007AC90 File Offset: 0x00078E90
		public override CampaignTime WeatherUpdateFrequency
		{
			get
			{
				return new CampaignTime(this.WeatherUpdatePeriod.NumTicks / (long)(Campaign.Current.DefaultWeatherNodeDimension * Campaign.Current.DefaultWeatherNodeDimension));
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06001936 RID: 6454 RVA: 0x0007ACC7 File Offset: 0x00078EC7
		private CampaignTime PreviousRainDataCheckForWetness
		{
			get
			{
				return CampaignTime.Hours(24f);
			}
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x0007ACD4 File Offset: 0x00078ED4
		private uint GetSeed(CampaignTime campaignTime, Vec2 position)
		{
			campaignTime += new CampaignTime((long)Campaign.Current.UniqueGameId.GetHashCode());
			int num;
			int num2;
			this.GetNodePositionForWeather(position, out num, out num2);
			uint num3 = (uint)(campaignTime.ToHours / this.WeatherUpdatePeriod.ToHours);
			if (campaignTime.ToSeconds % this.WeatherUpdatePeriod.ToSeconds < this.WeatherUpdateFrequency.ToSeconds * (double)(num * Campaign.Current.DefaultWeatherNodeDimension + num2))
			{
				num3 -= 1U;
			}
			return num3;
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x0007AD65 File Offset: 0x00078F65
		public override AtmosphereState GetInterpolatedAtmosphereState(CampaignTime timeOfYear, Vec3 pos)
		{
			if (this._atmosphereGrid == null)
			{
				this._atmosphereGrid = new AtmosphereGrid();
				this._atmosphereGrid.Initialize();
			}
			return this._atmosphereGrid.GetInterpolatedStateInfo(pos);
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x0007AD94 File Offset: 0x00078F94
		private Vec2 GetNodePositionForWeather(Vec2 pos, out int xIndex, out int yIndex)
		{
			if (Campaign.Current.MapSceneWrapper != null)
			{
				Vec2 terrainSize = Campaign.Current.MapSceneWrapper.GetTerrainSize();
				float num = terrainSize.X / (float)Campaign.Current.DefaultWeatherNodeDimension;
				float num2 = terrainSize.Y / (float)Campaign.Current.DefaultWeatherNodeDimension;
				xIndex = (int)(pos.x / num);
				yIndex = (int)(pos.y / num2);
				float num3 = (float)xIndex * num;
				float num4 = (float)yIndex * num2;
				return new Vec2(num3, num4);
			}
			xIndex = 0;
			yIndex = 0;
			return Vec2.Zero;
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x0007AE18 File Offset: 0x00079018
		public override AtmosphereInfo GetAtmosphereModel(CampaignVec2 position)
		{
			float hourOfDayNormalized = this.GetHourOfDayNormalized();
			float num;
			float num2;
			this.GetSeasonTimeFactorOfCampaignTime(CampaignTime.Now, out num, out num2, true);
			DefaultMapWeatherModel.SunPosition sunPosition = this.GetSunPosition(hourOfDayNormalized, num);
			float environmentMultiplier = this.GetEnvironmentMultiplier(sunPosition);
			float num3 = this.GetModifiedEnvironmentMultiplier(environmentMultiplier);
			num3 = MathF.Max(MathF.Pow(num3, 1.5f), 0.001f);
			Vec3 sunColor = this.GetSunColor(environmentMultiplier);
			AtmosphereState interpolatedAtmosphereState = this.GetInterpolatedAtmosphereState(CampaignTime.Now, position.AsVec3());
			float temperature = this.GetTemperature(ref interpolatedAtmosphereState, num);
			float humidity = this.GetHumidity(ref interpolatedAtmosphereState, num);
			Campaign.Current.Models.MapWeatherModel.UpdateWeatherForPosition(position, CampaignTime.Now);
			CampaignTime.Seasons seasons;
			bool flag;
			float num4;
			float num5;
			this.GetSeasonRainAndSnowDataForOpeningMission(position.ToVec2(), out seasons, out flag, out num4, out num5);
			string selectedAtmosphereId = this.GetSelectedAtmosphereId(seasons, flag, num5, num4);
			TerrainType terrainTypeAtPosition = Campaign.Current.MapSceneWrapper.GetTerrainTypeAtPosition(in position);
			AtmosphereInfo atmosphereInfo = default(AtmosphereInfo);
			atmosphereInfo.Seed = (uint)CampaignTime.Now.ToSeconds;
			atmosphereInfo.SunInfo.Altitude = sunPosition.Altitude;
			atmosphereInfo.SunInfo.Angle = sunPosition.Angle;
			atmosphereInfo.SunInfo.Color = sunColor;
			atmosphereInfo.SunInfo.Brightness = this.GetSunBrightness(environmentMultiplier, false);
			atmosphereInfo.SunInfo.Size = this.GetSunSize(environmentMultiplier);
			atmosphereInfo.SunInfo.RayStrength = this.GetSunRayStrength(environmentMultiplier);
			atmosphereInfo.SunInfo.MaxBrightness = this.GetSunBrightness(1f, true);
			atmosphereInfo.RainInfo.Density = num4;
			atmosphereInfo.SnowInfo.Density = num5;
			atmosphereInfo.AmbientInfo.EnvironmentMultiplier = MathF.Max(num3 * 0.5f, 0.001f);
			atmosphereInfo.AmbientInfo.AmbientColor = this.GetAmbientFogColor(num3);
			atmosphereInfo.AmbientInfo.MieScatterStrength = this.GetMieScatterStrength(environmentMultiplier);
			atmosphereInfo.AmbientInfo.RayleighConstant = this.GetRayleighConstant(environmentMultiplier);
			atmosphereInfo.SkyInfo.Brightness = this.GetSkyBrightness(hourOfDayNormalized, environmentMultiplier);
			atmosphereInfo.FogInfo.Density = this.GetFogDensity(environmentMultiplier, position.AsVec3());
			atmosphereInfo.FogInfo.Color = this.GetFogColor(num3);
			atmosphereInfo.FogInfo.Falloff = 1.48f;
			atmosphereInfo.TimeInfo.TimeOfDay = this.GetHourOfDay();
			atmosphereInfo.TimeInfo.WinterTimeFactor = this.GetWinterTimeFactor(CampaignTime.Now);
			atmosphereInfo.TimeInfo.DrynessFactor = this.GetDrynessFactor(CampaignTime.Now);
			atmosphereInfo.TimeInfo.NightTimeFactor = this.GetNightTimeFactor();
			atmosphereInfo.TimeInfo.Season = (int)seasons;
			atmosphereInfo.NauticalInfo.WaveStrength = this.GetWaveStrengthForPosition(position);
			atmosphereInfo.NauticalInfo.WindVector = Campaign.Current.Models.MapWeatherModel.GetWindForPosition(position);
			atmosphereInfo.NauticalInfo.CanUseLowAltitudeAtmosphere = 0;
			atmosphereInfo.NauticalInfo.UseSceneWindDirection = 1;
			atmosphereInfo.NauticalInfo.IsRiverBattle = ((terrainTypeAtPosition == TerrainType.River) ? 1 : 0);
			atmosphereInfo.NauticalInfo.UsesNavalSimulatedWater = ((terrainTypeAtPosition == TerrainType.River || terrainTypeAtPosition == TerrainType.Water || terrainTypeAtPosition == TerrainType.OpenSea || terrainTypeAtPosition == TerrainType.CoastalSea) ? 1 : 0);
			atmosphereInfo.AreaInfo.Temperature = temperature;
			atmosphereInfo.AreaInfo.Humidity = humidity;
			atmosphereInfo.PostProInfo.MinExposure = MBMath.Lerp(-3f, -2f, this.GetExposureCoefficientBetweenDayNight(), 1E-05f);
			atmosphereInfo.PostProInfo.MaxExposure = MBMath.Lerp(2f, 0f, num3, 1E-05f);
			atmosphereInfo.PostProInfo.BrightpassThreshold = MBMath.Lerp(0.7f, 0.9f, num3, 1E-05f);
			atmosphereInfo.PostProInfo.MiddleGray = 0.1f;
			atmosphereInfo.InterpolatedAtmosphereName = selectedAtmosphereId;
			return atmosphereInfo;
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x0007B1F7 File Offset: 0x000793F7
		public override void InitializeCaches()
		{
			this._weatherDataCache = new MapWeatherModel.WeatherEvent[Campaign.Current.DefaultWeatherNodeDimension * Campaign.Current.DefaultWeatherNodeDimension];
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x0007B21C File Offset: 0x0007941C
		public override MapWeatherModel.WeatherEvent UpdateWeatherForPosition(CampaignVec2 position, CampaignTime ct)
		{
			float num;
			float num2;
			this.GetSnowAndRainDataForPosition(position.ToVec2(), ct, out num, out num2);
			Vec2 vec;
			if (num > 0.55f)
			{
				float num3 = num;
				vec = position.ToVec2();
				return this.SetIsBlizzardOrSnowFromFunction(num3, ct, in vec);
			}
			float num4 = num2;
			vec = position.ToVec2();
			return this.SetIsRainingOrWetFromFunction(num4, ct, in vec);
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x0007B268 File Offset: 0x00079468
		private MapWeatherModel.WeatherEvent SetIsBlizzardOrSnowFromFunction(float snowValue, CampaignTime campaignTime, in Vec2 position)
		{
			int defaultWeatherNodeDimension = Campaign.Current.DefaultWeatherNodeDimension;
			int num;
			int num2;
			Vec2 nodePositionForWeather = this.GetNodePositionForWeather(position, out num, out num2);
			if (snowValue >= 0.65000004f)
			{
				float num3 = (snowValue - 0.55f) / 0.45f;
				uint seed = this.GetSeed(campaignTime, position);
				bool currentWeatherInAdjustedPosition = this.GetCurrentWeatherInAdjustedPosition(seed, num3, 0.1f, in nodePositionForWeather);
				this._weatherDataCache[num2 * defaultWeatherNodeDimension + num] = (currentWeatherInAdjustedPosition ? MapWeatherModel.WeatherEvent.Blizzard : MapWeatherModel.WeatherEvent.Snowy);
			}
			else
			{
				this._weatherDataCache[num2 * defaultWeatherNodeDimension + num] = ((snowValue > 0.55f) ? MapWeatherModel.WeatherEvent.Snowy : MapWeatherModel.WeatherEvent.Clear);
			}
			return this._weatherDataCache[num2 * defaultWeatherNodeDimension + num];
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x0007B304 File Offset: 0x00079504
		private MapWeatherModel.WeatherEvent SetIsRainingOrWetFromFunction(float rainValue, CampaignTime campaignTime, in Vec2 position)
		{
			int defaultWeatherNodeDimension = Campaign.Current.DefaultWeatherNodeDimension;
			int num;
			int num2;
			Vec2 nodePositionForWeather = this.GetNodePositionForWeather(position, out num, out num2);
			if (rainValue >= 0.6f)
			{
				float num3 = (rainValue - 0.6f) / 0.39999998f;
				uint seed = this.GetSeed(campaignTime, position);
				this._weatherDataCache[num2 * defaultWeatherNodeDimension + num] = MapWeatherModel.WeatherEvent.Clear;
				if (this.GetCurrentWeatherInAdjustedPosition(seed, num3, 0.45f, in nodePositionForWeather))
				{
					this._weatherDataCache[num2 * defaultWeatherNodeDimension + num] = MapWeatherModel.WeatherEvent.HeavyRain;
				}
				else
				{
					CampaignTime campaignTime2 = new CampaignTime(campaignTime.NumTicks - this.WeatherUpdatePeriod.NumTicks);
					uint num4 = this.GetSeed(campaignTime2, position);
					float num5;
					float num6;
					this.GetSnowAndRainDataForPosition(position, campaignTime2, out num5, out num6);
					float num7 = (num6 - 0.6f) / 0.39999998f;
					while (campaignTime.NumTicks - campaignTime2.NumTicks < this.PreviousRainDataCheckForWetness.NumTicks)
					{
						if (this.GetCurrentWeatherInAdjustedPosition(num4, num7, 0.45f, in nodePositionForWeather))
						{
							this._weatherDataCache[num2 * defaultWeatherNodeDimension + num] = MapWeatherModel.WeatherEvent.LightRain;
							break;
						}
						campaignTime2 = new CampaignTime(campaignTime2.NumTicks - this.WeatherUpdatePeriod.NumTicks);
						num4 = this.GetSeed(campaignTime2, position);
						this.GetSnowAndRainDataForPosition(position, campaignTime2, out num5, out num6);
						num7 = (num6 - 0.6f) / 0.39999998f;
					}
				}
			}
			else
			{
				this._weatherDataCache[num2 * defaultWeatherNodeDimension + num] = MapWeatherModel.WeatherEvent.Clear;
			}
			return this._weatherDataCache[num2 * defaultWeatherNodeDimension + num];
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x0007B490 File Offset: 0x00079690
		private bool GetCurrentWeatherInAdjustedPosition(uint seed, float frequency, float chanceModifier, in Vec2 adjustedPosition)
		{
			float num = frequency * chanceModifier;
			float mapDiagonal = Campaign.MapDiagonal;
			Vec2 vec = adjustedPosition;
			float num2 = mapDiagonal * vec.X;
			vec = adjustedPosition;
			return num > MBRandom.RandomFloatWithSeed(seed, (uint)(num2 + vec.Y));
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x0007B4D0 File Offset: 0x000796D0
		private string GetSelectedAtmosphereId(CampaignTime.Seasons selectedSeason, bool isRaining, float snowValue, float rainValue)
		{
			string text = "semicloudy_field_battle";
			if (Settlement.CurrentSettlement != null && (Settlement.CurrentSettlement.IsFortification || Settlement.CurrentSettlement.IsVillage))
			{
				text = "semicloudy_" + Settlement.CurrentSettlement.Culture.StringId;
			}
			if (selectedSeason == CampaignTime.Seasons.Winter)
			{
				if (snowValue >= 0.85f)
				{
					text = "dense_snowy";
				}
				else
				{
					text = "semi_snowy";
				}
			}
			else
			{
				if (rainValue > 0.6f)
				{
					text = "wet";
				}
				if (isRaining)
				{
					if (rainValue >= 0.85f)
					{
						text = "dense_rainy";
					}
					else
					{
						text = "semi_rainy";
					}
				}
			}
			return text;
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x0007B564 File Offset: 0x00079764
		private void GetSeasonRainAndSnowDataForOpeningMission(Vec2 position, out CampaignTime.Seasons selectedSeason, out bool isRaining, out float rainValue, out float snowFallDensity)
		{
			MapWeatherModel.WeatherEvent weatherEventInPosition = Campaign.Current.Models.MapWeatherModel.GetWeatherEventInPosition(position);
			MapWeatherModel.WeatherEventEffectOnTerrain weatherEffectOnTerrainForPosition = Campaign.Current.Models.MapWeatherModel.GetWeatherEffectOnTerrainForPosition(position);
			selectedSeason = CampaignTime.Now.GetSeasonOfYear;
			rainValue = 0f;
			snowFallDensity = 0.85f;
			isRaining = false;
			switch (weatherEventInPosition)
			{
			case MapWeatherModel.WeatherEvent.Clear:
				if (selectedSeason == CampaignTime.Seasons.Winter)
				{
					selectedSeason = ((CampaignTime.Now.GetDayOfSeason > CampaignTime.DaysInSeason / 2) ? CampaignTime.Seasons.Spring : CampaignTime.Seasons.Autumn);
				}
				break;
			case MapWeatherModel.WeatherEvent.LightRain:
				if (selectedSeason == CampaignTime.Seasons.Winter)
				{
					selectedSeason = ((CampaignTime.Now.GetDayOfSeason > CampaignTime.DaysInSeason / 2) ? CampaignTime.Seasons.Spring : CampaignTime.Seasons.Autumn);
				}
				rainValue = 0.7f;
				break;
			case MapWeatherModel.WeatherEvent.HeavyRain:
				if (selectedSeason == CampaignTime.Seasons.Winter)
				{
					selectedSeason = ((CampaignTime.Now.GetDayOfSeason > CampaignTime.DaysInSeason / 2) ? CampaignTime.Seasons.Spring : CampaignTime.Seasons.Autumn);
				}
				isRaining = true;
				rainValue = 0.85f + MBRandom.RandomFloatRanged(0f, 0.14999998f);
				break;
			case MapWeatherModel.WeatherEvent.Snowy:
				selectedSeason = CampaignTime.Seasons.Winter;
				rainValue = 0.55f;
				snowFallDensity = 0.55f + MBRandom.RandomFloatRanged(0f, 0.3f);
				break;
			case MapWeatherModel.WeatherEvent.Blizzard:
				selectedSeason = CampaignTime.Seasons.Winter;
				rainValue = 0.85f;
				snowFallDensity = 0.85f;
				break;
			case MapWeatherModel.WeatherEvent.Storm:
				isRaining = true;
				rainValue = 0.85f + MBRandom.RandomFloatRanged(0f, 0.14999998f);
				snowFallDensity = ((selectedSeason != CampaignTime.Seasons.Winter) ? 0f : snowFallDensity);
				if (selectedSeason == CampaignTime.Seasons.Winter)
				{
					selectedSeason = ((CampaignTime.Now.GetDayOfSeason > CampaignTime.DaysInSeason / 2) ? CampaignTime.Seasons.Spring : CampaignTime.Seasons.Autumn);
				}
				break;
			}
			if (weatherEffectOnTerrainForPosition == MapWeatherModel.WeatherEventEffectOnTerrain.Wet)
			{
				rainValue = MathF.Max(0.6f, rainValue);
			}
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x0007B718 File Offset: 0x00079918
		private DefaultMapWeatherModel.SunPosition GetSunPosition(float hourNorm, float seasonFactor)
		{
			float num2;
			float num3;
			if (hourNorm >= this.SunRiseNorm && hourNorm < this.SunSetNorm)
			{
				this._sunIsMoon = false;
				float num = (hourNorm - this.SunRiseNorm) / (this.SunSetNorm - this.SunRiseNorm);
				num2 = MBMath.Lerp(0f, 180f, num, 1E-05f);
				num3 = 50f * seasonFactor;
			}
			else
			{
				this._sunIsMoon = true;
				if (hourNorm >= this.SunSetNorm)
				{
					hourNorm -= 1f;
				}
				float num4 = (hourNorm - (this.SunSetNorm - 1f)) / (this.SunRiseNorm - (this.SunSetNorm - 1f));
				num4 = ((num4 < 0f) ? 0f : ((num4 > 1f) ? 1f : num4));
				num2 = MBMath.Lerp(180f, 0f, num4, 1E-05f);
				num3 = 50f * seasonFactor;
			}
			return new DefaultMapWeatherModel.SunPosition(num3, num2);
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x0007B7F8 File Offset: 0x000799F8
		private Vec3 GetSunColor(float environmentMultiplier)
		{
			Vec3 vec;
			if (!this._sunIsMoon)
			{
				vec = new Vec3(1f, 1f - (1f - MathF.Pow(environmentMultiplier, 0.3f)) / 2f, 0.9f - (1f - MathF.Pow(environmentMultiplier, 0.3f)) / 2.5f, -1f);
			}
			else
			{
				vec = new Vec3(0.85f - MathF.Pow(environmentMultiplier, 0.4f), 0.8f - MathF.Pow(environmentMultiplier, 0.5f), 0.8f - MathF.Pow(environmentMultiplier, 0.8f), -1f);
				vec = Vec3.Vec3Max(vec, new Vec3(0.05f, 0.05f, 0.1f, -1f));
			}
			return vec;
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x0007B8BC File Offset: 0x00079ABC
		private float GetSunBrightness(float environmentMultiplier, bool forceDay = false)
		{
			float num;
			if (!this._sunIsMoon || forceDay)
			{
				num = MathF.Sin(MathF.Pow((environmentMultiplier - 0.001f) / 0.999f, 1.2f) * 1.5707964f) * 85f;
				num = MathF.Min(MathF.Max(num, 0.2f), 35f);
			}
			else
			{
				num = 0.2f;
			}
			return num;
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x0007B91E File Offset: 0x00079B1E
		private float GetSunSize(float envMultiplier)
		{
			return 0.1f + (1f - envMultiplier) / 8f;
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x0007B934 File Offset: 0x00079B34
		private float GetSunRayStrength(float envMultiplier)
		{
			return MathF.Min(MathF.Max(MathF.Sin(MathF.Pow((envMultiplier - 0.001f) / 0.999f, 0.4f) * 3.1415927f / 2f) - 0.15f, 0.01f), 0.5f);
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x0007B984 File Offset: 0x00079B84
		private float GetEnvironmentMultiplier(DefaultMapWeatherModel.SunPosition sunPos)
		{
			float num;
			if (this._sunIsMoon)
			{
				num = sunPos.Altitude / 180f * 2f;
			}
			else
			{
				num = sunPos.Altitude / 180f * 2f;
			}
			num = ((num > 1f) ? (2f - num) : num);
			num = MathF.Pow(num, 0.5f);
			float num2 = 1f - 0.011111111f * sunPos.Angle;
			float num3 = MBMath.ClampFloat(num * num2, 0f, 1f);
			return MBMath.ClampFloat(MathF.Min(MathF.Sin(num3 * num3) * 2f, 1f), 0f, 1f) * 0.999f + 0.001f;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x0007BA3C File Offset: 0x00079C3C
		private float GetModifiedEnvironmentMultiplier(float envMultiplier)
		{
			float num;
			if (!this._sunIsMoon)
			{
				num = (envMultiplier - 0.001f) / 0.999f;
				num = num * 0.999f + 0.001f;
			}
			else
			{
				num = (envMultiplier - 0.001f) / 0.999f;
				num = num * 0f + 0.001f;
			}
			return num;
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x0007BA8C File Offset: 0x00079C8C
		private float GetSkyBrightness(float hourNorm, float envMultiplier)
		{
			float num = (envMultiplier - 0.001f) / 0.999f;
			float num2;
			if (!this._sunIsMoon)
			{
				num2 = MathF.Sin(MathF.Pow(num, 1.3f) * 1.5707964f) * 80f;
				num2 -= 1f;
				num2 = MathF.Min(MathF.Max(num2, 0.055f), 25f);
			}
			else
			{
				num2 = 0.055f;
			}
			return num2;
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x0007BAFC File Offset: 0x00079CFC
		private float GetFogDensity(float environmentMultiplier, Vec3 pos)
		{
			float num = (this._sunIsMoon ? 0.5f : 0.4f);
			float num2 = 1f - environmentMultiplier;
			float num3 = 1f - MBMath.ClampFloat((pos.z - 30f) / 200f, 0f, 0.9f);
			return MathF.Min((0f + num * num2) * num3, 10f);
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x0007BB64 File Offset: 0x00079D64
		private Vec3 GetFogColor(float environmentMultiplier)
		{
			Vec3 vec;
			if (!this._sunIsMoon)
			{
				vec = new Vec3(1f - (1f - environmentMultiplier) / 7f, 0.75f - environmentMultiplier / 4f, 0.55f - environmentMultiplier / 5f, -1f);
			}
			else
			{
				vec = new Vec3(1f - environmentMultiplier * 10f, 0.75f + environmentMultiplier * 1.5f, 0.65f + environmentMultiplier * 2f, -1f);
				vec = Vec3.Vec3Max(vec, new Vec3(0.55f, 0.59f, 0.6f, -1f));
			}
			return vec;
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x0007BC08 File Offset: 0x00079E08
		private Vec3 GetAmbientFogColor(float moddedEnvMul)
		{
			return Vec3.Vec3Min(new Vec3(0.15f, 0.3f, 0.5f, -1f) + new Vec3(moddedEnvMul / 3f, moddedEnvMul / 2f, moddedEnvMul / 1.5f, -1f), new Vec3(1f, 1f, 1f, -1f));
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x0007BC70 File Offset: 0x00079E70
		private float GetMieScatterStrength(float envMultiplier)
		{
			return (1f + (1f - envMultiplier)) * 10f;
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x0007BC88 File Offset: 0x00079E88
		private float GetRayleighConstant(float envMultiplier)
		{
			float num = (envMultiplier - 0.001f) / 0.999f;
			return MathF.Min(MathF.Max(1f - MathF.Sin(MathF.Pow(num, 0.45f) * 3.1415927f / 2f) + (0.14f + num * 2f), 0.65f), 0.99f);
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0007BCE8 File Offset: 0x00079EE8
		private float GetHourOfDay()
		{
			return (float)(CampaignTime.Now.ToHours % (double)CampaignTime.HoursInDay);
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x0007BD0A File Offset: 0x00079F0A
		private float GetHourOfDayNormalized()
		{
			return this.GetHourOfDay() / (float)CampaignTime.HoursInDay;
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x0007BD1C File Offset: 0x00079F1C
		private float GetNightTimeFactor()
		{
			float num = this.GetHourOfDay() - (float)CampaignTime.SunRise;
			for (num %= (float)CampaignTime.HoursInDay; num < 0f; num += (float)CampaignTime.HoursInDay)
			{
			}
			num = MathF.Max(num - this.DayTime, 0f);
			return MathF.Min(num / 0.1f, 1f);
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x0007BD7C File Offset: 0x00079F7C
		private float GetExposureCoefficientBetweenDayNight()
		{
			float hourOfDay = this.GetHourOfDay();
			float num = 0f;
			if (hourOfDay > (float)CampaignTime.SunRise && hourOfDay < (float)(CampaignTime.SunRise + 2))
			{
				num = 1f - (hourOfDay - (float)CampaignTime.SunRise) / 2f;
			}
			if (hourOfDay < (float)CampaignTime.SunSet && hourOfDay > (float)(CampaignTime.SunSet - 2))
			{
				num = (hourOfDay - (float)(CampaignTime.SunSet - 2)) / 2f;
			}
			if (hourOfDay > (float)CampaignTime.SunSet || hourOfDay < (float)CampaignTime.SunRise)
			{
				num = 1f;
			}
			return num;
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0007BE00 File Offset: 0x0007A000
		public override void GetSnowAndRainDataForPosition(Vec2 position, CampaignTime ct, out float snowValue, out float rainValue)
		{
			int num;
			int num2;
			Vec2 nodePositionForWeather = this.GetNodePositionForWeather(position, out num, out num2);
			float snowAmountAtPosition = Campaign.Current.MapSceneWrapper.GetSnowAmountAtPosition(position);
			float rainAmountAtPosition = Campaign.Current.MapSceneWrapper.GetRainAmountAtPosition(nodePositionForWeather);
			float num3 = snowAmountAtPosition / 255f;
			float num4 = rainAmountAtPosition / 255f;
			float num5;
			float num6;
			Campaign.Current.Models.MapWeatherModel.GetSeasonTimeFactorOfCampaignTime(ct, out num5, out num6, true);
			float num7 = MBMath.Lerp(0.55f, -0.1f, num5, 1E-05f);
			float num8 = MBMath.Lerp(0.7f, 0.3f, num6, 1E-05f);
			float num9 = MBMath.SmoothStep(num7 - 0.65f, num7 + 0.65f, num3);
			float num10 = MBMath.SmoothStep(num8 - 0.45f, num8 + 0.45f, num4);
			snowValue = MBMath.Lerp(0f, num9, num9, 1E-05f);
			rainValue = num10;
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x0007BEE0 File Offset: 0x0007A0E0
		public override MapWeatherModel.WeatherEvent GetWeatherEventInPosition(Vec2 pos)
		{
			int num;
			int num2;
			this.GetNodePositionForWeather(pos, out num, out num2);
			return this._weatherDataCache[num2 * Campaign.Current.DefaultWeatherNodeDimension + num];
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x0007BF10 File Offset: 0x0007A110
		public override MapWeatherModel.WeatherEventEffectOnTerrain GetWeatherEffectOnTerrainForPosition(Vec2 pos)
		{
			switch (this.GetWeatherEventInPosition(pos))
			{
			case MapWeatherModel.WeatherEvent.Clear:
				return MapWeatherModel.WeatherEventEffectOnTerrain.Default;
			case MapWeatherModel.WeatherEvent.LightRain:
				return MapWeatherModel.WeatherEventEffectOnTerrain.Wet;
			case MapWeatherModel.WeatherEvent.HeavyRain:
				return MapWeatherModel.WeatherEventEffectOnTerrain.Wet;
			case MapWeatherModel.WeatherEvent.Snowy:
				return MapWeatherModel.WeatherEventEffectOnTerrain.Wet;
			case MapWeatherModel.WeatherEvent.Blizzard:
				return MapWeatherModel.WeatherEventEffectOnTerrain.Wet;
			default:
				return MapWeatherModel.WeatherEventEffectOnTerrain.Default;
			}
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x0007BF4C File Offset: 0x0007A14C
		private float GetWinterTimeFactor(CampaignTime timeOfYear)
		{
			float num = 0f;
			if (timeOfYear.GetSeasonOfYear == CampaignTime.Seasons.Winter)
			{
				float num2 = MathF.Abs((float)Math.IEEERemainder(CampaignTime.Now.ToSeasons, 1.0));
				num = MBMath.SplitLerp(0f, 0.75f, 0f, 0.5f, num2, 1E-05f);
			}
			return num;
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x0007BFAC File Offset: 0x0007A1AC
		private float GetDrynessFactor(CampaignTime timeOfYear)
		{
			float num = 0f;
			float num2 = MathF.Abs((float)Math.IEEERemainder(CampaignTime.Now.ToSeasons, 1.0));
			switch (timeOfYear.GetSeasonOfYear)
			{
			case CampaignTime.Seasons.Summer:
			{
				float num3 = MBMath.ClampFloat(num2 * 2f, 0f, 1f);
				num = MBMath.Lerp(0f, 1f, num3, 1E-05f);
				break;
			}
			case CampaignTime.Seasons.Autumn:
				num = 1f;
				break;
			case CampaignTime.Seasons.Winter:
				num = MBMath.Lerp(1f, 0f, num2, 1E-05f);
				break;
			}
			return num;
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x0007C050 File Offset: 0x0007A250
		public override void GetSeasonTimeFactorOfCampaignTime(CampaignTime ct, out float timeFactorForSnow, out float timeFactorForRain, bool snapCampaignTimeToWeatherPeriod = true)
		{
			if (snapCampaignTimeToWeatherPeriod)
			{
				ct = CampaignTime.Hours((float)((int)(ct.ToHours / this.WeatherUpdatePeriod.ToHours / 2.0) * (int)this.WeatherUpdatePeriod.ToHours * 2));
			}
			float num = (float)ct.ToSeasons % 4f;
			timeFactorForSnow = this.CalculateTimeFactorForSnow(num);
			timeFactorForRain = this.CalculateTimeFactorForRain(num);
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x0007C0C0 File Offset: 0x0007A2C0
		private float CalculateTimeFactorForSnow(float yearProgress)
		{
			float num = 0f;
			if (yearProgress > 1.5f && (double)yearProgress <= 3.5)
			{
				num = MBMath.Map(yearProgress, 1.5f, 3.5f, 0f, 1f);
			}
			else if (yearProgress <= 1.5f)
			{
				num = MBMath.Map(yearProgress, 0f, 1.5f, 0.75f, 0f);
			}
			else if (yearProgress > 3.5f)
			{
				num = MBMath.Map(yearProgress, 3.5f, 4f, 1f, 0.75f);
			}
			return num;
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x0007C150 File Offset: 0x0007A350
		private float CalculateTimeFactorForRain(float yearProgress)
		{
			float num = 0f;
			if (yearProgress > 1f && (double)yearProgress <= 2.5)
			{
				num = MBMath.Map(yearProgress, 1f, 2.5f, 0f, 1f);
			}
			else if (yearProgress <= 1f)
			{
				num = MBMath.Map(yearProgress, 0f, 1f, 1f, 0f);
			}
			else if (yearProgress > 2.5f)
			{
				num = 1f;
			}
			return num;
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x0007C1CC File Offset: 0x0007A3CC
		private float GetTemperature(ref AtmosphereState gridInfo, float seasonFactor)
		{
			if (gridInfo == null)
			{
				return 0f;
			}
			float temperatureAverage = gridInfo.TemperatureAverage;
			float num = (seasonFactor - 0.5f) * -2f;
			float num2 = gridInfo.TemperatureVariance * num;
			return temperatureAverage + num2;
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0007C204 File Offset: 0x0007A404
		private float GetHumidity(ref AtmosphereState gridInfo, float seasonFactor)
		{
			if (gridInfo == null)
			{
				return 0f;
			}
			float humidityAverage = gridInfo.HumidityAverage;
			float num = (seasonFactor - 0.5f) * 2f;
			float num2 = gridInfo.HumidityVariance * num;
			return MBMath.ClampFloat(humidityAverage + num2, 0f, 100f);
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0007C24B File Offset: 0x0007A44B
		public override Vec2 GetWindForPosition(CampaignVec2 position)
		{
			return Vec2.Side * 0.26f;
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0007C25C File Offset: 0x0007A45C
		private float GetWaveStrengthForPosition(CampaignVec2 position)
		{
			if (position.IsOnLand)
			{
				return 0.26f;
			}
			return Campaign.Current.Models.MapWeatherModel.GetWindForPosition(position).Length;
		}

		// Token: 0x04000831 RID: 2097
		private const float MinSunAngle = 0f;

		// Token: 0x04000832 RID: 2098
		private const float MaxSunAngle = 50f;

		// Token: 0x04000833 RID: 2099
		private const float MinEnvironmentMultiplier = 0.001f;

		// Token: 0x04000834 RID: 2100
		private const float DayEnvironmentMultiplier = 1f;

		// Token: 0x04000835 RID: 2101
		private const float NightEnvironmentMultiplier = 0.001f;

		// Token: 0x04000836 RID: 2102
		private const float SnowStartThreshold = 0.55f;

		// Token: 0x04000837 RID: 2103
		private const float DenseSnowStartThreshold = 0.85f;

		// Token: 0x04000838 RID: 2104
		private const float NoSnowDelta = 0.1f;

		// Token: 0x04000839 RID: 2105
		private const float WetThreshold = 0.6f;

		// Token: 0x0400083A RID: 2106
		private const float WetThresholdForTexture = 0.3f;

		// Token: 0x0400083B RID: 2107
		private const float LightRainStartThreshold = 0.7f;

		// Token: 0x0400083C RID: 2108
		private const float DenseRainStartThreshold = 0.85f;

		// Token: 0x0400083D RID: 2109
		private const float SnowFrequencyModifier = 0.1f;

		// Token: 0x0400083E RID: 2110
		private const float RainFrequencyModifier = 0.45f;

		// Token: 0x0400083F RID: 2111
		private const float MaxSnowCoverage = 0.75f;

		// Token: 0x04000840 RID: 2112
		private const float WaveMultiplierForSettlements = 0.3f;

		// Token: 0x04000841 RID: 2113
		private MapWeatherModel.WeatherEvent[] _weatherDataCache;

		// Token: 0x04000842 RID: 2114
		private AtmosphereGrid _atmosphereGrid;

		// Token: 0x04000843 RID: 2115
		private bool _sunIsMoon;

		// Token: 0x020005C2 RID: 1474
		private struct SunPosition
		{
			// Token: 0x17000F81 RID: 3969
			// (get) Token: 0x06005170 RID: 20848 RVA: 0x0019084B File Offset: 0x0018EA4B
			// (set) Token: 0x06005171 RID: 20849 RVA: 0x00190853 File Offset: 0x0018EA53
			public float Angle { get; private set; }

			// Token: 0x17000F82 RID: 3970
			// (get) Token: 0x06005172 RID: 20850 RVA: 0x0019085C File Offset: 0x0018EA5C
			// (set) Token: 0x06005173 RID: 20851 RVA: 0x00190864 File Offset: 0x0018EA64
			public float Altitude { get; private set; }

			// Token: 0x06005174 RID: 20852 RVA: 0x0019086D File Offset: 0x0018EA6D
			public SunPosition(float angle, float altitude)
			{
				this = default(DefaultMapWeatherModel.SunPosition);
				this.Angle = angle;
				this.Altitude = altitude;
			}
		}
	}
}
