using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.MapSiege
{
	// Token: 0x02000056 RID: 86
	public class MapSiegeVM : ViewModel
	{
		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x00014A35 File Offset: 0x00012C35
		private bool IsPlayerLeaderOfSiegeEvent
		{
			get
			{
				SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
				return playerSiegeEvent != null && playerSiegeEvent.IsPlayerSiegeEvent && Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(PlayerSiege.PlayerSiegeEvent, PlayerSiege.PlayerSide) == Hero.MainHero;
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00014A74 File Offset: 0x00012C74
		public MapSiegeVM(Camera mapCamera, MatrixFrame[] batteringRamFrames, MatrixFrame[] rangedSiegeEngineFrames, MatrixFrame[] towerSiegeEngineFrames, MatrixFrame[] defenderSiegeEngineFrames, MatrixFrame[] breachableWallFrames)
		{
			this._mapCamera = mapCamera;
			this.PointsOfInterest = new MBBindingList<MapSiegePOIVM>();
			this._poiDistanceComparer = new MapSiegeVM.SiegePOIDistanceComparer();
			for (int i = 0; i < batteringRamFrames.Length; i++)
			{
				this.PointsOfInterest.Add(new MapSiegePOIVM(MapSiegePOIVM.POIType.AttackerRamSiegeMachine, batteringRamFrames[i], this._mapCamera, i, new Action<MapSiegePOIVM>(this.OnPOISelection)));
			}
			for (int j = 0; j < rangedSiegeEngineFrames.Length; j++)
			{
				this.PointsOfInterest.Add(new MapSiegePOIVM(MapSiegePOIVM.POIType.AttackerRangedSiegeMachine, rangedSiegeEngineFrames[j], this._mapCamera, j, new Action<MapSiegePOIVM>(this.OnPOISelection)));
			}
			for (int k = 0; k < towerSiegeEngineFrames.Length; k++)
			{
				this.PointsOfInterest.Add(new MapSiegePOIVM(MapSiegePOIVM.POIType.AttackerTowerSiegeMachine, towerSiegeEngineFrames[k], this._mapCamera, batteringRamFrames.Length + k, new Action<MapSiegePOIVM>(this.OnPOISelection)));
			}
			for (int l = 0; l < defenderSiegeEngineFrames.Length; l++)
			{
				this.PointsOfInterest.Add(new MapSiegePOIVM(MapSiegePOIVM.POIType.DefenderSiegeMachine, defenderSiegeEngineFrames[l], this._mapCamera, l, new Action<MapSiegePOIVM>(this.OnPOISelection)));
			}
			for (int m = 0; m < breachableWallFrames.Length; m++)
			{
				this.PointsOfInterest.Add(new MapSiegePOIVM(MapSiegePOIVM.POIType.WallSection, breachableWallFrames[m], this._mapCamera, m, new Action<MapSiegePOIVM>(this.OnPOISelection)));
			}
			this.ProductionController = new MapSiegeProductionVM();
			this.RefreshValues();
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00014BE4 File Offset: 0x00012DE4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PreparationTitleText = GameTexts.FindText("str_building_siege_camp", null).ToString();
			SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
			this.IsPreparationsCompleted = (playerSiegeEvent != null && playerSiegeEvent.BesiegerCamp.IsPreparationComplete) || PlayerSiege.PlayerSide == BattleSideEnum.Defender;
			this.ProductionController.RefreshValues();
			this.PointsOfInterest.ApplyActionOnAllItems(delegate(MapSiegePOIVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00014C6B File Offset: 0x00012E6B
		private void OnPOISelection(MapSiegePOIVM poi)
		{
			if (this.ProductionController.LatestSelectedPOI != null)
			{
				this.ProductionController.LatestSelectedPOI.IsSelected = false;
			}
			if (this.IsPlayerLeaderOfSiegeEvent)
			{
				this.ProductionController.OnMachineSelection(poi);
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00014CA0 File Offset: 0x00012EA0
		public void OnSelectionFromScene(MatrixFrame frameOfEngine)
		{
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				Settlement besiegedSettlement = PlayerSiege.BesiegedSettlement;
				if ((besiegedSettlement == null || besiegedSettlement.CurrentSiegeState != Settlement.SiegeState.InTheLordsHall) && this.IsPlayerLeaderOfSiegeEvent)
				{
					IEnumerable<MapSiegePOIVM> enumerable = this.PointsOfInterest.Where<MapSiegePOIVM>((MapSiegePOIVM poi) => frameOfEngine.NearlyEquals(poi.MapSceneLocationFrame, 1E-05f));
					if (enumerable == null)
					{
						return;
					}
					MapSiegePOIVM mapSiegePOIVM = enumerable.FirstOrDefault<MapSiegePOIVM>();
					if (mapSiegePOIVM == null)
					{
						return;
					}
					mapSiegePOIVM.ExecuteSelection();
				}
			}
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00014D10 File Offset: 0x00012F10
		public void Update(float mapCameraDistanceValue)
		{
			SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
			this.IsPreparationsCompleted = (playerSiegeEvent != null && playerSiegeEvent.BesiegerCamp.IsPreparationComplete) || PlayerSiege.PlayerSide == BattleSideEnum.Defender;
			SiegeEvent playerSiegeEvent2 = PlayerSiege.PlayerSiegeEvent;
			float? num;
			if (playerSiegeEvent2 == null)
			{
				num = null;
			}
			else
			{
				SiegeEvent.SiegeEnginesContainer siegeEngines = playerSiegeEvent2.BesiegerCamp.SiegeEngines;
				if (siegeEngines == null)
				{
					num = null;
				}
				else
				{
					SiegeEvent.SiegeEngineConstructionProgress siegePreparations = siegeEngines.SiegePreparations;
					num = ((siegePreparations != null) ? new float?(siegePreparations.Progress) : null);
				}
			}
			this.PreparationProgress = num ?? 0f;
			TWParallel.For(0, this.PointsOfInterest.Count, delegate(int startInclusive, int endExclusive)
			{
				for (int i = startInclusive; i < endExclusive; i++)
				{
					this.PointsOfInterest[i].RefreshDistanceValue(mapCameraDistanceValue);
					this.PointsOfInterest[i].RefreshPosition();
					this.PointsOfInterest[i].UpdateProperties();
				}
			}, 16);
			foreach (MapSiegePOIVM mapSiegePOIVM in this.PointsOfInterest)
			{
				mapSiegePOIVM.RefreshBinding();
			}
			this.ProductionController.Update();
			this.PointsOfInterest.Sort(this._poiDistanceComparer);
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00014E3C File Offset: 0x0001303C
		// (set) Token: 0x06000572 RID: 1394 RVA: 0x00014E44 File Offset: 0x00013044
		[DataSourceProperty]
		public float PreparationProgress
		{
			get
			{
				return this._preparationProgress;
			}
			set
			{
				if (value != this._preparationProgress)
				{
					this._preparationProgress = value;
					base.OnPropertyChangedWithValue(value, "PreparationProgress");
				}
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00014E62 File Offset: 0x00013062
		// (set) Token: 0x06000574 RID: 1396 RVA: 0x00014E6A File Offset: 0x0001306A
		[DataSourceProperty]
		public bool IsPreparationsCompleted
		{
			get
			{
				return this._isPreparationsCompleted;
			}
			set
			{
				if (value != this._isPreparationsCompleted)
				{
					this._isPreparationsCompleted = value;
					base.OnPropertyChangedWithValue(value, "IsPreparationsCompleted");
				}
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00014E88 File Offset: 0x00013088
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x00014E90 File Offset: 0x00013090
		[DataSourceProperty]
		public string PreparationTitleText
		{
			get
			{
				return this._preparationTitleText;
			}
			set
			{
				if (value != this._preparationTitleText)
				{
					this._preparationTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "PreparationTitleText");
				}
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00014EB3 File Offset: 0x000130B3
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x00014EBB File Offset: 0x000130BB
		[DataSourceProperty]
		public MapSiegeProductionVM ProductionController
		{
			get
			{
				return this._productionController;
			}
			set
			{
				if (value != this._productionController)
				{
					this._productionController = value;
					base.OnPropertyChangedWithValue<MapSiegeProductionVM>(value, "ProductionController");
				}
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00014ED9 File Offset: 0x000130D9
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x00014EE1 File Offset: 0x000130E1
		[DataSourceProperty]
		public MBBindingList<MapSiegePOIVM> PointsOfInterest
		{
			get
			{
				return this._pointsOfInterest;
			}
			set
			{
				if (value != this._pointsOfInterest)
				{
					this._pointsOfInterest = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapSiegePOIVM>>(value, "PointsOfInterest");
				}
			}
		}

		// Token: 0x040002BB RID: 699
		private readonly Camera _mapCamera;

		// Token: 0x040002BC RID: 700
		private readonly MapSiegeVM.SiegePOIDistanceComparer _poiDistanceComparer;

		// Token: 0x040002BD RID: 701
		private MBBindingList<MapSiegePOIVM> _pointsOfInterest;

		// Token: 0x040002BE RID: 702
		private MapSiegeProductionVM _productionController;

		// Token: 0x040002BF RID: 703
		private float _preparationProgress;

		// Token: 0x040002C0 RID: 704
		private string _preparationTitleText;

		// Token: 0x040002C1 RID: 705
		private bool _isPreparationsCompleted;

		// Token: 0x020000B2 RID: 178
		public class SiegePOIDistanceComparer : IComparer<MapSiegePOIVM>
		{
			// Token: 0x06000749 RID: 1865 RVA: 0x000189C4 File Offset: 0x00016BC4
			public int Compare(MapSiegePOIVM x, MapSiegePOIVM y)
			{
				return y.LatestW.CompareTo(x.LatestW);
			}
		}
	}
}
