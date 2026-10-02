using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000439 RID: 1081
	public class MapWeatherCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x060045AD RID: 17837 RVA: 0x00151CCE File Offset: 0x0014FECE
		public WeatherNode[] AllWeatherNodes
		{
			get
			{
				return this._weatherNodes;
			}
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x060045AE RID: 17838 RVA: 0x00151CD6 File Offset: 0x0014FED6
		private int DimensionSquared
		{
			get
			{
				return Campaign.Current.DefaultWeatherNodeDimension * Campaign.Current.DefaultWeatherNodeDimension;
			}
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x00151CED File Offset: 0x0014FEED
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunchedEvent));
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x00151D08 File Offset: 0x0014FF08
		private void OnSessionLaunchedEvent(CampaignGameStarter obj)
		{
			this.InitializeTheBehavior();
			for (int i = 0; i < this.DimensionSquared; i++)
			{
				this.UpdateWeatherNodeWithIndex(i);
			}
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x00151D33 File Offset: 0x0014FF33
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<int>("_lastUpdatedNodeIndex", ref this._lastUpdatedNodeIndex);
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x00151D48 File Offset: 0x0014FF48
		private void CreateAndShuffleDataIndicesDeterministic()
		{
			this._weatherNodeDataShuffledIndices = new int[this.DimensionSquared];
			for (int i = 0; i < this.DimensionSquared; i++)
			{
				this._weatherNodeDataShuffledIndices[i] = i;
			}
			MBFastRandom mbfastRandom = new MBFastRandom((uint)Campaign.Current.UniqueGameId.GetDeterministicHashCode());
			for (int j = 0; j < 20; j++)
			{
				for (int k = 0; k < this.DimensionSquared; k++)
				{
					int num = mbfastRandom.Next(this.DimensionSquared);
					int num2 = this._weatherNodeDataShuffledIndices[k];
					this._weatherNodeDataShuffledIndices[k] = this._weatherNodeDataShuffledIndices[num];
					this._weatherNodeDataShuffledIndices[num] = num2;
				}
			}
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x00151DE8 File Offset: 0x0014FFE8
		private void InitializeTheBehavior()
		{
			this.CreateAndShuffleDataIndicesDeterministic();
			this._weatherNodes = new WeatherNode[this.DimensionSquared];
			Vec2 terrainSize = Campaign.Current.MapSceneWrapper.GetTerrainSize();
			int defaultWeatherNodeDimension = Campaign.Current.DefaultWeatherNodeDimension;
			int num = defaultWeatherNodeDimension;
			int num2 = defaultWeatherNodeDimension;
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num; j++)
				{
					bool flag = false;
					float num3 = (float)i / (float)defaultWeatherNodeDimension * terrainSize.X;
					float num4 = (float)j / (float)defaultWeatherNodeDimension * terrainSize.Y;
					Vec2 vec = new Vec2(num3, num4);
					int num5 = 0;
					while (num5 <= 4 && !flag)
					{
						for (int k = 0; k <= 4; k++)
						{
							CampaignVec2 campaignVec = new CampaignVec2(vec + new Vec2((float)(num5 * defaultWeatherNodeDimension) / 4f, (float)(k * defaultWeatherNodeDimension) / 4f), true);
							if (campaignVec.IsValid())
							{
								this._weatherNodes[i * defaultWeatherNodeDimension + j] = new WeatherNode(new CampaignVec2(vec, true));
								flag = true;
								break;
							}
						}
						num5++;
					}
				}
			}
			this.AddEventHandler();
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x00151F04 File Offset: 0x00150104
		private void AddEventHandler()
		{
			long num = Campaign.Current.Models.MapWeatherModel.WeatherUpdateFrequency.NumTicks - CampaignTime.Now.NumTicks % Campaign.Current.Models.MapWeatherModel.WeatherUpdateFrequency.NumTicks;
			this._weatherTickEvent = CampaignPeriodicEventManager.CreatePeriodicEvent(Campaign.Current.Models.MapWeatherModel.WeatherUpdateFrequency, new CampaignTime(num));
			this._weatherTickEvent.AddHandler(new MBCampaignEvent.CampaignEventDelegate(this.WeatherUpdateTick));
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x00151F95 File Offset: 0x00150195
		private void WeatherUpdateTick(MBCampaignEvent campaignEvent, params object[] delegateParams)
		{
			this.UpdateWeatherNodeWithIndex(this._weatherNodeDataShuffledIndices[this._lastUpdatedNodeIndex]);
			this._lastUpdatedNodeIndex++;
			if (this._lastUpdatedNodeIndex == this._weatherNodes.Length)
			{
				this._lastUpdatedNodeIndex = 0;
			}
		}

		// Token: 0x060045B6 RID: 17846 RVA: 0x00151FD0 File Offset: 0x001501D0
		private void UpdateWeatherNodeWithIndex(int index)
		{
			WeatherNode weatherNode = this._weatherNodes[index];
			if (weatherNode != null)
			{
				MapWeatherModel.WeatherEvent currentWeatherEvent = weatherNode.CurrentWeatherEvent;
				MapWeatherModel.WeatherEvent weatherEvent = Campaign.Current.Models.MapWeatherModel.UpdateWeatherForPosition(weatherNode.Position, CampaignTime.Now);
				MapWeatherModel.WeatherEventEffectOnTerrain weatherEffectOnTerrainForPosition = Campaign.Current.Models.MapWeatherModel.GetWeatherEffectOnTerrainForPosition(weatherNode.Position.ToVec2());
				if (currentWeatherEvent != weatherEvent || weatherEffectOnTerrainForPosition == MapWeatherModel.WeatherEventEffectOnTerrain.Wet)
				{
					weatherNode.SetVisualDirty();
					return;
				}
				if (currentWeatherEvent == MapWeatherModel.WeatherEvent.Clear && MBRandom.NondeterministicRandomFloat < 0.1f)
				{
					weatherNode.SetVisualDirty();
				}
			}
		}

		// Token: 0x04001412 RID: 5138
		private WeatherNode[] _weatherNodes;

		// Token: 0x04001413 RID: 5139
		private MBCampaignEvent _weatherTickEvent;

		// Token: 0x04001414 RID: 5140
		private int[] _weatherNodeDataShuffledIndices;

		// Token: 0x04001415 RID: 5141
		private int _lastUpdatedNodeIndex;
	}
}
