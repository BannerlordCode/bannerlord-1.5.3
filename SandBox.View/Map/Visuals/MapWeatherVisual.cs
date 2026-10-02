using System;
using SandBox.View.Map.Managers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.View.Map.Visuals
{
	// Token: 0x02000065 RID: 101
	public class MapWeatherVisual : MapEntityVisual<WeatherNode>
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x0001F165 File Offset: 0x0001D365
		public Vec2 Position
		{
			get
			{
				return base.MapEntity.Position.ToVec2();
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x0001F178 File Offset: 0x0001D378
		public Vec2 PrefabSpawnOffset
		{
			get
			{
				Vec2 terrainSize = Campaign.Current.MapSceneWrapper.GetTerrainSize();
				float num = terrainSize.X / (float)Campaign.Current.DefaultWeatherNodeDimension;
				float num2 = terrainSize.Y / (float)Campaign.Current.DefaultWeatherNodeDimension;
				return new Vec2(num * 0.5f, num2 * 0.5f);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0001F1D0 File Offset: 0x0001D3D0
		public int MaskPixelIndex
		{
			get
			{
				if (this._maskPixelIndex == -1)
				{
					Vec2 terrainSize = Campaign.Current.MapSceneWrapper.GetTerrainSize();
					float num = terrainSize.X / (float)Campaign.Current.DefaultWeatherNodeDimension;
					float num2 = terrainSize.Y / (float)Campaign.Current.DefaultWeatherNodeDimension;
					int num3 = (int)(this.Position.X / num);
					int num4 = (int)(this.Position.Y / num2);
					this._maskPixelIndex = num4 * Campaign.Current.DefaultWeatherNodeDimension + num3;
				}
				return this._maskPixelIndex;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x0001F260 File Offset: 0x0001D460
		public override CampaignVec2 InteractionPositionForPlayer
		{
			get
			{
				return new CampaignVec2(this.Position, true);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0001F26E File Offset: 0x0001D46E
		public override MapEntityVisual AttachedTo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0001F274 File Offset: 0x0001D474
		public override string ToString()
		{
			return this.Position.ToString();
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001F295 File Offset: 0x0001D495
		public MapWeatherVisual(WeatherNode weatherNode)
			: base(weatherNode)
		{
			this._previousWeatherEvent = MapWeatherModel.WeatherEvent.Clear;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001F2AC File Offset: 0x0001D4AC
		public void Tick()
		{
			if (base.MapEntity.IsVisuallyDirty)
			{
				bool flag = this._previousWeatherEvent == MapWeatherModel.WeatherEvent.HeavyRain;
				bool flag2 = this._previousWeatherEvent == MapWeatherModel.WeatherEvent.Blizzard;
				MapWeatherModel.WeatherEvent weatherEventInPosition = Campaign.Current.Models.MapWeatherModel.GetWeatherEventInPosition(this.Position);
				bool flag3 = weatherEventInPosition == MapWeatherModel.WeatherEvent.HeavyRain;
				bool flag4 = Campaign.Current.Models.MapWeatherModel.GetWeatherEffectOnTerrainForPosition(this.Position) == MapWeatherModel.WeatherEventEffectOnTerrain.Wet;
				bool flag5 = weatherEventInPosition == MapWeatherModel.WeatherEvent.Blizzard;
				byte b = (flag4 ? 125 : (flag3 ? 200 : 0));
				byte b2 = (byte)Math.Max((int)b, flag5 ? 200 : 0);
				MapWeatherVisualManager.Current.SetRainData(this.MaskPixelIndex, b);
				MapWeatherVisualManager.Current.SetCloudData(this.MaskPixelIndex, b2);
				if (this.Prefab == null)
				{
					if (flag3)
					{
						this.AttachNewRainPrefabToVisual();
					}
					else if (flag5)
					{
						this.AttachNewBlizzardPrefabToVisual();
					}
					else if (MBRandom.RandomFloat < 0.1f)
					{
						MapWeatherVisualManager.Current.SetCloudData(this.MaskPixelIndex, 200);
					}
				}
				else
				{
					if (flag && !flag3 && flag5)
					{
						MapWeatherVisualManager.Current.ReleaseRainPrefab(this.Prefab);
						this.AttachNewBlizzardPrefabToVisual();
					}
					else if (flag2 && !flag5 && flag3)
					{
						MapWeatherVisualManager.Current.ReleaseBlizzardPrefab(this.Prefab);
						this.AttachNewRainPrefabToVisual();
					}
					if (!flag3 && !flag5)
					{
						if (flag)
						{
							MapWeatherVisualManager.Current.ReleaseRainPrefab(this.Prefab);
						}
						else if (flag2)
						{
							MapWeatherVisualManager.Current.ReleaseBlizzardPrefab(this.Prefab);
						}
						this.Prefab = null;
					}
				}
				this._previousWeatherEvent = weatherEventInPosition;
				base.MapEntity.OnVisualUpdated();
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001F458 File Offset: 0x0001D658
		private void AttachNewRainPrefabToVisual()
		{
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin = new Vec3(this.Position + this.PrefabSpawnOffset, 26f, -1f);
			GameEntity rainPrefabFromPool = MapWeatherVisualManager.Current.GetRainPrefabFromPool();
			rainPrefabFromPool.SetVisibilityExcludeParents(true);
			rainPrefabFromPool.SetGlobalFrame(in identity, true);
			this.Prefab = rainPrefabFromPool;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001F4B4 File Offset: 0x0001D6B4
		private void AttachNewBlizzardPrefabToVisual()
		{
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin = new Vec3(this.Position + this.PrefabSpawnOffset, 26f, -1f);
			GameEntity blizzardPrefabFromPool = MapWeatherVisualManager.Current.GetBlizzardPrefabFromPool();
			blizzardPrefabFromPool.SetVisibilityExcludeParents(true);
			blizzardPrefabFromPool.SetGlobalFrame(in identity, true);
			this.Prefab = blizzardPrefabFromPool;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001F510 File Offset: 0x0001D710
		public override bool OnMapClick(bool followModifierUsed)
		{
			return false;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001F513 File Offset: 0x0001D713
		public override void OnHover()
		{
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001F515 File Offset: 0x0001D715
		public override void OnOpenEncyclopedia()
		{
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001F517 File Offset: 0x0001D717
		public override bool IsVisibleOrFadingOut()
		{
			return false;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001F51C File Offset: 0x0001D71C
		public override Vec3 GetVisualPosition()
		{
			return this.InteractionPositionForPlayer.AsVec3();
		}

		// Token: 0x04000207 RID: 519
		public GameEntity Prefab;

		// Token: 0x04000208 RID: 520
		private MapWeatherModel.WeatherEvent _previousWeatherEvent;

		// Token: 0x04000209 RID: 521
		private int _maskPixelIndex = -1;
	}
}
