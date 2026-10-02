using System;
using System.Collections.Generic;
using SandBox.View.Map.Visuals;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.View.Map.Managers
{
	// Token: 0x02000078 RID: 120
	public class MapWeatherVisualManager : EntityVisualManagerBase<WeatherNode>
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00027667 File Offset: 0x00025867
		public static MapWeatherVisualManager Current
		{
			get
			{
				return SandBoxViewSubModule.SandBoxViewVisualManager.GetEntityComponent<MapWeatherVisualManager>();
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00027673 File Offset: 0x00025873
		public override int Priority
		{
			get
			{
				return 60;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00027677 File Offset: 0x00025877
		private int DimensionSquared
		{
			get
			{
				return Campaign.Current.DefaultWeatherNodeDimension * Campaign.Current.DefaultWeatherNodeDimension;
			}
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00027690 File Offset: 0x00025890
		public MapWeatherVisualManager()
		{
			this._unusedRainPrefabEntityPool = new List<GameEntity>();
			this._unusedBlizzardPrefabEntityPool = new List<GameEntity>();
			for (int i = 0; i < this.DimensionSquared * 2; i++)
			{
				this._rainData[i] = 0;
				this._rainDataTemporal[i] = 0;
			}
			this._allWeatherNodeVisuals = new MapWeatherVisual[this.DimensionSquared];
			this._mapScene = ((MapScene)Campaign.Current.MapSceneWrapper).Scene;
			WeatherNode[] allWeatherNodes = Campaign.Current.GetCampaignBehavior<MapWeatherCampaignBehavior>().AllWeatherNodes;
			for (int j = 0; j < allWeatherNodes.Length; j++)
			{
				if (allWeatherNodes[j] != null)
				{
					this._allWeatherNodeVisuals[j] = new MapWeatherVisual(allWeatherNodes[j]);
				}
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00027780 File Offset: 0x00025980
		public override void OnVisualTick(MapScreen screen, float realDt, float dt)
		{
			for (int i = 0; i < this._allWeatherNodeVisuals.Length; i++)
			{
				MapWeatherVisual mapWeatherVisual = this._allWeatherNodeVisuals[i];
				if (mapWeatherVisual != null)
				{
					mapWeatherVisual.Tick();
				}
			}
			TWParallel.For(0, this.DimensionSquared, delegate(int startInclusive, int endExclusive)
			{
				for (int j = startInclusive; j < endExclusive; j++)
				{
					int num = j * 2;
					this._rainDataTemporal[num] = (byte)MBMath.Lerp((float)this._rainDataTemporal[num], (float)this._rainData[num], 1f - (float)Math.Exp((double)(-1.8f * (realDt + dt))), 1E-05f);
					this._rainDataTemporal[num + 1] = (byte)MBMath.Lerp((float)this._rainDataTemporal[num + 1], (float)this._rainData[num + 1], 1f - (float)Math.Exp((double)(-1.8f * (realDt + dt))), 1E-05f);
				}
			}, 16);
			this._mapScene.SetLandscapeRainMaskData(this._rainDataTemporal);
			this.WeatherAudioAndVisualTick();
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x000277FF File Offset: 0x000259FF
		public void SetRainData(int dataIndex, byte value)
		{
			this._rainData[dataIndex * 2] = value;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0002780C File Offset: 0x00025A0C
		public void SetCloudData(int dataIndex, byte value)
		{
			this._rainData[dataIndex * 2 + 1] = value;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0002781C File Offset: 0x00025A1C
		private void WeatherAudioAndVisualTick()
		{
			SoundManager.SetGlobalParameter("Rainfall", 0.5f);
			float num = 0f;
			int num2 = 26;
			MatrixFrame lastFinalRenderCameraFrame = this._mapScene.LastFinalRenderCameraFrame;
			Vec2 asVec = lastFinalRenderCameraFrame.origin.AsVec2;
			IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
			CampaignVec2 campaignVec = new CampaignVec2(asVec, true);
			mapSceneWrapper.GetHeightAtPoint(in campaignVec, ref num);
			Vec2 terrainSize = Campaign.Current.MapSceneWrapper.GetTerrainSize();
			float num3 = MBMath.ClampFloat(asVec.x, 0f, terrainSize.x);
			float num4 = MBMath.ClampFloat(asVec.y, 0f, terrainSize.y);
			Vec2 vec = new Vec2(num3, num4);
			MapWeatherModel.WeatherEvent weatherEvent = Campaign.Current.Models.MapWeatherModel.GetWeatherEventInPosition(vec);
			GameEntity gameEntity = this._cameraRainEffect;
			if (weatherEvent == MapWeatherModel.WeatherEvent.Storm)
			{
				weatherEvent = MapWeatherModel.WeatherEvent.HeavyRain;
				gameEntity = this._cameraStormEffect;
				num2 = 20;
			}
			if (weatherEvent == MapWeatherModel.WeatherEvent.HeavyRain || weatherEvent == MapWeatherModel.WeatherEvent.Blizzard)
			{
				if (weatherEvent == MapWeatherModel.WeatherEvent.HeavyRain)
				{
					if (lastFinalRenderCameraFrame.origin.Z < (float)num2 * 2.5f)
					{
						gameEntity.SetVisibilityExcludeParents(true);
						MatrixFrame matrixFrame = lastFinalRenderCameraFrame.Elevate(-5f);
						gameEntity.SetFrame(ref matrixFrame, true);
					}
					else
					{
						gameEntity.SetVisibilityExcludeParents(false);
					}
					this.DestroyBlizzardSound();
					this.StartRainSoundIfNeeded();
					MBMapScene.ApplyRainColorGrade = true;
					return;
				}
				if (weatherEvent == MapWeatherModel.WeatherEvent.Blizzard)
				{
					this.DestroyRainSound();
					this.StartBlizzardSoundIfNeeded();
					gameEntity.SetVisibilityExcludeParents(false);
					MBMapScene.ApplyRainColorGrade = false;
					return;
				}
			}
			else
			{
				this.DestroyBlizzardSound();
				this.DestroyRainSound();
				this._cameraRainEffect.SetVisibilityExcludeParents(false);
				this._cameraStormEffect.SetVisibilityExcludeParents(false);
				MBMapScene.ApplyRainColorGrade = false;
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x000279A4 File Offset: 0x00025BA4
		private void DestroyRainSound()
		{
			if (this._currentRainSound != null)
			{
				this._currentRainSound.Stop();
				this._currentRainSound = null;
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x000279C0 File Offset: 0x00025BC0
		private void DestroyBlizzardSound()
		{
			if (this._currentBlizzardSound != null)
			{
				this._currentBlizzardSound.Stop();
				this._currentBlizzardSound = null;
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x000279DC File Offset: 0x00025BDC
		private void StartRainSoundIfNeeded()
		{
			if (this._currentRainSound == null)
			{
				this._currentRainSound = SoundManager.CreateEvent("event:/map/ambient/bed/rain", this._mapScene);
				this._currentRainSound.Play();
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00027A08 File Offset: 0x00025C08
		private void StartBlizzardSoundIfNeeded()
		{
			if (this._currentBlizzardSound == null)
			{
				this._currentBlizzardSound = SoundManager.CreateEvent("event:/map/ambient/bed/snow", this._mapScene);
				this._currentBlizzardSound.Play();
			}
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00027A34 File Offset: 0x00025C34
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this.InitializeObjectPoolWithDefaultCount();
			this._cameraRainEffect = GameEntity.Instantiate(this._mapScene, "map_camera_rain_prefab", MatrixFrame.Identity, true);
			this._cameraStormEffect = GameEntity.Instantiate(this._mapScene, "map_camera_storm_prefab", MatrixFrame.Identity, true);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00027A88 File Offset: 0x00025C88
		public GameEntity GetRainPrefabFromPool()
		{
			if (this._unusedRainPrefabEntityPool.IsEmpty<GameEntity>())
			{
				this._unusedRainPrefabEntityPool.AddRange(this.CreateNewWeatherPrefabPoolElements("campaign_rain_prefab", 5));
			}
			GameEntity gameEntity = this._unusedRainPrefabEntityPool[0];
			this._unusedRainPrefabEntityPool.Remove(gameEntity);
			return gameEntity;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00027AD4 File Offset: 0x00025CD4
		public GameEntity GetBlizzardPrefabFromPool()
		{
			if (this._unusedBlizzardPrefabEntityPool.IsEmpty<GameEntity>())
			{
				this._unusedBlizzardPrefabEntityPool.AddRange(this.CreateNewWeatherPrefabPoolElements("campaign_snow_prefab", 5));
			}
			GameEntity gameEntity = this._unusedBlizzardPrefabEntityPool[0];
			this._unusedBlizzardPrefabEntityPool.Remove(gameEntity);
			return gameEntity;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00027B20 File Offset: 0x00025D20
		public void ReleaseRainPrefab(GameEntity prefab)
		{
			this._unusedRainPrefabEntityPool.Add(prefab);
			prefab.SetVisibilityExcludeParents(false);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00027B35 File Offset: 0x00025D35
		public void ReleaseBlizzardPrefab(GameEntity prefab)
		{
			this._unusedBlizzardPrefabEntityPool.Add(prefab);
			prefab.SetVisibilityExcludeParents(false);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00027B4A File Offset: 0x00025D4A
		private void InitializeObjectPoolWithDefaultCount()
		{
			this._unusedRainPrefabEntityPool.AddRange(this.CreateNewWeatherPrefabPoolElements("campaign_rain_prefab", 5));
			this._unusedBlizzardPrefabEntityPool.AddRange(this.CreateNewWeatherPrefabPoolElements("campaign_snow_prefab", 5));
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00027B7C File Offset: 0x00025D7C
		private List<GameEntity> CreateNewWeatherPrefabPoolElements(string prefabName, int delta)
		{
			List<GameEntity> list = new List<GameEntity>();
			for (int i = 0; i < delta; i++)
			{
				GameEntity gameEntity = GameEntity.Instantiate(this._mapScene, prefabName, MatrixFrame.Identity, true);
				gameEntity.SetVisibilityExcludeParents(false);
				list.Add(gameEntity);
			}
			return list;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00027BBD File Offset: 0x00025DBD
		public override MapEntityVisual<WeatherNode> GetVisualOfEntity(WeatherNode entity)
		{
			return null;
		}

		// Token: 0x0400024F RID: 591
		public const int DefaultCloudHeight = 26;

		// Token: 0x04000250 RID: 592
		public const int OpenSeaStormCloudHeight = 20;

		// Token: 0x04000251 RID: 593
		private MapWeatherVisual[] _allWeatherNodeVisuals;

		// Token: 0x04000252 RID: 594
		private const string RainPrefabName = "campaign_rain_prefab";

		// Token: 0x04000253 RID: 595
		private const string BlizzardPrefabName = "campaign_snow_prefab";

		// Token: 0x04000254 RID: 596
		private const string RainSoundPath = "event:/map/ambient/bed/rain";

		// Token: 0x04000255 RID: 597
		private const string SnowSoundPath = "event:/map/ambient/bed/snow";

		// Token: 0x04000256 RID: 598
		private const string WeatherEventParameterName = "Rainfall";

		// Token: 0x04000257 RID: 599
		private const string CameraRainPrefabName = "map_camera_rain_prefab";

		// Token: 0x04000258 RID: 600
		private const string CameraStormPrefabName = "map_camera_storm_prefab";

		// Token: 0x04000259 RID: 601
		private const int DefaultRainObjectPoolCount = 5;

		// Token: 0x0400025A RID: 602
		private const int DefaultBlizzardObjectPoolCount = 5;

		// Token: 0x0400025B RID: 603
		private const int WeatherCheckOriginZDelta = 25;

		// Token: 0x0400025C RID: 604
		private readonly List<GameEntity> _unusedRainPrefabEntityPool;

		// Token: 0x0400025D RID: 605
		private readonly List<GameEntity> _unusedBlizzardPrefabEntityPool;

		// Token: 0x0400025E RID: 606
		private readonly Scene _mapScene;

		// Token: 0x0400025F RID: 607
		private readonly byte[] _rainData = new byte[Campaign.Current.DefaultWeatherNodeDimension * Campaign.Current.DefaultWeatherNodeDimension * 2];

		// Token: 0x04000260 RID: 608
		private readonly byte[] _rainDataTemporal = new byte[Campaign.Current.DefaultWeatherNodeDimension * Campaign.Current.DefaultWeatherNodeDimension * 2];

		// Token: 0x04000261 RID: 609
		private SoundEvent _currentRainSound;

		// Token: 0x04000262 RID: 610
		private SoundEvent _currentBlizzardSound;

		// Token: 0x04000263 RID: 611
		private GameEntity _cameraRainEffect;

		// Token: 0x04000264 RID: 612
		private GameEntity _cameraStormEffect;
	}
}
