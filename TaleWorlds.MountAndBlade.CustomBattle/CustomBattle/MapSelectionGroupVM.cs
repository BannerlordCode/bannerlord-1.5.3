using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle
{
	// Token: 0x0200001C RID: 28
	public class MapSelectionGroupVM : ViewModel
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00009555 File Offset: 0x00007755
		// (set) Token: 0x0600014D RID: 333 RVA: 0x0000955D File Offset: 0x0000775D
		public int SelectedWallBreachedCount { get; private set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00009566 File Offset: 0x00007766
		// (set) Token: 0x0600014F RID: 335 RVA: 0x0000956E File Offset: 0x0000776E
		public int SelectedSceneLevel { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00009577 File Offset: 0x00007777
		// (set) Token: 0x06000151 RID: 337 RVA: 0x0000957F File Offset: 0x0000777F
		public int SelectedTimeOfDay { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00009588 File Offset: 0x00007788
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00009590 File Offset: 0x00007790
		public string SelectedSeasonId { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00009599 File Offset: 0x00007799
		// (set) Token: 0x06000155 RID: 341 RVA: 0x000095A1 File Offset: 0x000077A1
		public MapItemVM SelectedMap { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000156 RID: 342 RVA: 0x000095AA File Offset: 0x000077AA
		// (set) Token: 0x06000157 RID: 343 RVA: 0x000095B2 File Offset: 0x000077B2
		private List<MapItemVM> _battleMaps { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000158 RID: 344 RVA: 0x000095BB File Offset: 0x000077BB
		// (set) Token: 0x06000159 RID: 345 RVA: 0x000095C3 File Offset: 0x000077C3
		private List<MapItemVM> _villageMaps { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600015A RID: 346 RVA: 0x000095CC File Offset: 0x000077CC
		// (set) Token: 0x0600015B RID: 347 RVA: 0x000095D4 File Offset: 0x000077D4
		private List<MapItemVM> _siegeMaps { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000095DD File Offset: 0x000077DD
		// (set) Token: 0x0600015D RID: 349 RVA: 0x000095E5 File Offset: 0x000077E5
		private List<MapItemVM> _availableMaps { get; set; }

		// Token: 0x0600015E RID: 350 RVA: 0x000095F0 File Offset: 0x000077F0
		public MapSelectionGroupVM()
		{
			this._battleMaps = new List<MapItemVM>();
			this._villageMaps = new List<MapItemVM>();
			this._siegeMaps = new List<MapItemVM>();
			this._availableMaps = this._battleMaps;
			this.MapSelection = new SelectorVM<MapItemVM>(0, new Action<SelectorVM<MapItemVM>>(this.OnMapSelection));
			this.WallHitpointSelection = new SelectorVM<WallHitpointItemVM>(0, new Action<SelectorVM<WallHitpointItemVM>>(this.OnWallHitpointSelection));
			this.SceneLevelSelection = new SelectorVM<SceneLevelItemVM>(0, new Action<SelectorVM<SceneLevelItemVM>>(this.OnSceneLevelSelection));
			this.SeasonSelection = new SelectorVM<SeasonItemVM>(0, new Action<SelectorVM<SeasonItemVM>>(this.OnSeasonSelection));
			this.TimeOfDaySelection = new SelectorVM<TimeOfDayItemVM>(0, new Action<SelectorVM<TimeOfDayItemVM>>(this.OnTimeOfDaySelection));
			this.RefreshValues();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000096B0 File Offset: 0x000078B0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PrepareMapLists();
			this.TitleText = new TextObject("{=customgametitle}Map", null).ToString();
			this.MapText = new TextObject("{=customgamemapname}Map", null).ToString();
			this.SeasonText = new TextObject("{=xTzDM5XE}Season", null).ToString();
			this.TimeOfDayText = new TextObject("{=DszSWnc3}Time of Day", null).ToString();
			this.SceneLevelText = new TextObject("{=0s52GQJt}Scene Level", null).ToString();
			this.WallHitpointsText = new TextObject("{=4IuXGSdc}Wall Hitpoints", null).ToString();
			this.AttackerSiegeMachinesText = new TextObject("{=AmfIfeIc}Choose Attacker Siege Machines", null).ToString();
			this.DefenderSiegeMachinesText = new TextObject("{=UoiSWe87}Choose Defender Siege Machines", null).ToString();
			this.SalloutText = new TextObject("{=EcKMGoFv}Sallyout", null).ToString();
			this.MapSelection.ItemList.Clear();
			this.WallHitpointSelection.ItemList.Clear();
			this.SceneLevelSelection.ItemList.Clear();
			this.SeasonSelection.ItemList.Clear();
			this.TimeOfDaySelection.ItemList.Clear();
			foreach (MapItemVM mapItemVM in this._availableMaps)
			{
				this.MapSelection.AddItem(new MapItemVM(mapItemVM.MapName, mapItemVM.MapId, mapItemVM.ForcedSceneLevel));
			}
			foreach (Tuple<string, int> tuple in CustomBattleData.WallHitpoints)
			{
				this.WallHitpointSelection.AddItem(new WallHitpointItemVM(tuple.Item1, tuple.Item2));
			}
			foreach (int num in CustomBattleData.SceneLevels)
			{
				this.SceneLevelSelection.AddItem(new SceneLevelItemVM(num));
			}
			foreach (Tuple<string, string> tuple2 in CustomBattleData.Seasons)
			{
				this.SeasonSelection.AddItem(new SeasonItemVM(tuple2.Item1, tuple2.Item2));
			}
			foreach (Tuple<string, CustomBattleTimeOfDay> tuple3 in CustomBattleData.TimesOfDay)
			{
				this.TimeOfDaySelection.AddItem(new TimeOfDayItemVM(tuple3.Item1, (int)tuple3.Item2));
			}
			this.MapSelection.SelectedIndex = 0;
			this.WallHitpointSelection.SelectedIndex = 0;
			this.SceneLevelSelection.SelectedIndex = 0;
			this.SeasonSelection.SelectedIndex = 0;
			this.TimeOfDaySelection.SelectedIndex = 0;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000099C8 File Offset: 0x00007BC8
		public void ExecuteSallyOutChange()
		{
			this.IsSallyOutSelected = !this.IsSallyOutSelected;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000099DC File Offset: 0x00007BDC
		private void PrepareMapLists()
		{
			this._battleMaps.Clear();
			this._villageMaps.Clear();
			this._siegeMaps.Clear();
			bool isOnlyCoreContentEnabled = Module.CurrentModule.IsOnlyCoreContentEnabled;
			if (CustomGame.Current != null)
			{
				IEnumerable<CustomBattleSceneData> enumerable;
				if (isOnlyCoreContentEnabled)
				{
					enumerable = CustomGame.Current.CustomBattleScenes.Where<CustomBattleSceneData>((CustomBattleSceneData s) => s.SceneID == "battle_terrain_029");
				}
				else
				{
					IEnumerable<CustomBattleSceneData> enumerable2 = CustomGame.Current.CustomBattleScenes.ToList<CustomBattleSceneData>();
					enumerable = enumerable2;
				}
				foreach (CustomBattleSceneData customBattleSceneData in enumerable)
				{
					MapItemVM mapItemVM = new MapItemVM(customBattleSceneData.Name.ToString(), customBattleSceneData.SceneID, customBattleSceneData.ForcedSceneLevel);
					if (customBattleSceneData.IsVillageMap)
					{
						this._villageMaps.Add(mapItemVM);
					}
					else if (customBattleSceneData.IsSiegeMap)
					{
						this._siegeMaps.Add(mapItemVM);
					}
					else if (!customBattleSceneData.IsLordsHallMap)
					{
						this._battleMaps.Add(mapItemVM);
					}
				}
			}
			Comparer<MapItemVM> comparer = Comparer<MapItemVM>.Create((MapItemVM x, MapItemVM y) => x.MapName.CompareTo(y.MapName));
			this._battleMaps.Sort(comparer);
			this._villageMaps.Sort(comparer);
			this._siegeMaps.Sort(comparer);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00009B48 File Offset: 0x00007D48
		private void OnMapSelection(SelectorVM<MapItemVM> selector)
		{
			this.SelectedMap = selector.SelectedItem;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00009B56 File Offset: 0x00007D56
		private void OnWallHitpointSelection(SelectorVM<WallHitpointItemVM> selector)
		{
			this.SelectedWallBreachedCount = selector.SelectedItem.BreachedWallCount;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00009B69 File Offset: 0x00007D69
		private void OnSceneLevelSelection(SelectorVM<SceneLevelItemVM> selector)
		{
			this.SelectedSceneLevel = selector.SelectedItem.Level;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00009B7C File Offset: 0x00007D7C
		private void OnSeasonSelection(SelectorVM<SeasonItemVM> selector)
		{
			this.SelectedSeasonId = selector.SelectedItem.SeasonId;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00009B8F File Offset: 0x00007D8F
		private void OnTimeOfDaySelection(SelectorVM<TimeOfDayItemVM> selector)
		{
			this.SelectedTimeOfDay = selector.SelectedItem.TimeOfDay;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00009BA4 File Offset: 0x00007DA4
		public void OnGameTypeChange(string gameTypeStringId)
		{
			this.MapSelection.ItemList.Clear();
			if (gameTypeStringId == "Battle")
			{
				this.IsCurrentMapSiege = false;
				this._availableMaps = this._battleMaps;
			}
			else if (gameTypeStringId == "Village")
			{
				this.IsCurrentMapSiege = false;
				this._availableMaps = this._villageMaps;
			}
			else if (gameTypeStringId == "Siege")
			{
				this.IsCurrentMapSiege = true;
				this._availableMaps = this._siegeMaps;
			}
			foreach (MapItemVM mapItemVM in this._availableMaps)
			{
				this.MapSelection.AddItem(mapItemVM);
			}
			this.MapSelection.SelectedIndex = 0;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00009C7C File Offset: 0x00007E7C
		public void RandomizeAll()
		{
			this.MapSelection.ExecuteRandomize();
			this.SceneLevelSelection.ExecuteRandomize();
			this.SeasonSelection.ExecuteRandomize();
			this.WallHitpointSelection.ExecuteRandomize();
			this.TimeOfDaySelection.ExecuteRandomize();
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00009CB5 File Offset: 0x00007EB5
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00009CBD File Offset: 0x00007EBD
		[DataSourceProperty]
		public SelectorVM<MapItemVM> MapSelection
		{
			get
			{
				return this._mapSelection;
			}
			set
			{
				if (value != this._mapSelection)
				{
					this._mapSelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<MapItemVM>>(value, "MapSelection");
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00009CDB File Offset: 0x00007EDB
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00009CE3 File Offset: 0x00007EE3
		[DataSourceProperty]
		public SelectorVM<SceneLevelItemVM> SceneLevelSelection
		{
			get
			{
				return this._sceneLevelSelection;
			}
			set
			{
				if (value != this._sceneLevelSelection)
				{
					this._sceneLevelSelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<SceneLevelItemVM>>(value, "SceneLevelSelection");
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00009D01 File Offset: 0x00007F01
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00009D09 File Offset: 0x00007F09
		[DataSourceProperty]
		public SelectorVM<WallHitpointItemVM> WallHitpointSelection
		{
			get
			{
				return this._wallHitpointSelection;
			}
			set
			{
				if (value != this._wallHitpointSelection)
				{
					this._wallHitpointSelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<WallHitpointItemVM>>(value, "WallHitpointSelection");
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00009D27 File Offset: 0x00007F27
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00009D2F File Offset: 0x00007F2F
		[DataSourceProperty]
		public SelectorVM<SeasonItemVM> SeasonSelection
		{
			get
			{
				return this._seasonSelection;
			}
			set
			{
				if (value != this._seasonSelection)
				{
					this._seasonSelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<SeasonItemVM>>(value, "SeasonSelection");
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00009D4D File Offset: 0x00007F4D
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00009D55 File Offset: 0x00007F55
		[DataSourceProperty]
		public SelectorVM<TimeOfDayItemVM> TimeOfDaySelection
		{
			get
			{
				return this._timeOfDaySelection;
			}
			set
			{
				if (value != this._timeOfDaySelection)
				{
					this._timeOfDaySelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<TimeOfDayItemVM>>(value, "TimeOfDaySelection");
				}
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00009D73 File Offset: 0x00007F73
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00009D7B File Offset: 0x00007F7B
		[DataSourceProperty]
		public bool IsCurrentMapSiege
		{
			get
			{
				return this._isCurrentMapSiege;
			}
			set
			{
				if (value != this._isCurrentMapSiege)
				{
					this._isCurrentMapSiege = value;
					base.OnPropertyChangedWithValue(value, "IsCurrentMapSiege");
				}
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00009D99 File Offset: 0x00007F99
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00009DA1 File Offset: 0x00007FA1
		[DataSourceProperty]
		public bool IsSallyOutSelected
		{
			get
			{
				return this._isSallyOutSelected;
			}
			set
			{
				if (value != this._isSallyOutSelected)
				{
					this._isSallyOutSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSallyOutSelected");
				}
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00009DBF File Offset: 0x00007FBF
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00009DC7 File Offset: 0x00007FC7
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00009DEA File Offset: 0x00007FEA
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00009DF2 File Offset: 0x00007FF2
		[DataSourceProperty]
		public string MapText
		{
			get
			{
				return this._mapText;
			}
			set
			{
				if (value != this._mapText)
				{
					this._mapText = value;
					base.OnPropertyChangedWithValue<string>(value, "MapText");
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00009E15 File Offset: 0x00008015
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00009E1D File Offset: 0x0000801D
		[DataSourceProperty]
		public string SeasonText
		{
			get
			{
				return this._seasonText;
			}
			set
			{
				if (value != this._seasonText)
				{
					this._seasonText = value;
					base.OnPropertyChangedWithValue<string>(value, "SeasonText");
				}
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00009E40 File Offset: 0x00008040
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00009E48 File Offset: 0x00008048
		[DataSourceProperty]
		public string TimeOfDayText
		{
			get
			{
				return this._timeOfDayText;
			}
			set
			{
				if (value != this._timeOfDayText)
				{
					this._timeOfDayText = value;
					base.OnPropertyChangedWithValue<string>(value, "TimeOfDayText");
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00009E6B File Offset: 0x0000806B
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00009E73 File Offset: 0x00008073
		[DataSourceProperty]
		public string SceneLevelText
		{
			get
			{
				return this._sceneLevelText;
			}
			set
			{
				if (value != this._sceneLevelText)
				{
					this._sceneLevelText = value;
					base.OnPropertyChangedWithValue<string>(value, "SceneLevelText");
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00009E96 File Offset: 0x00008096
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00009E9E File Offset: 0x0000809E
		[DataSourceProperty]
		public string WallHitpointsText
		{
			get
			{
				return this._wallHitpointsText;
			}
			set
			{
				if (value != this._wallHitpointsText)
				{
					this._wallHitpointsText = value;
					base.OnPropertyChangedWithValue<string>(value, "WallHitpointsText");
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00009EC1 File Offset: 0x000080C1
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00009EC9 File Offset: 0x000080C9
		[DataSourceProperty]
		public string AttackerSiegeMachinesText
		{
			get
			{
				return this._attackerSiegeMachinesText;
			}
			set
			{
				if (value != this._attackerSiegeMachinesText)
				{
					this._attackerSiegeMachinesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AttackerSiegeMachinesText");
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00009EEC File Offset: 0x000080EC
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00009EF4 File Offset: 0x000080F4
		[DataSourceProperty]
		public string DefenderSiegeMachinesText
		{
			get
			{
				return this._defenderSiegeMachinesText;
			}
			set
			{
				if (value != this._defenderSiegeMachinesText)
				{
					this._defenderSiegeMachinesText = value;
					base.OnPropertyChangedWithValue<string>(value, "DefenderSiegeMachinesText");
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00009F17 File Offset: 0x00008117
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00009F1F File Offset: 0x0000811F
		[DataSourceProperty]
		public string SalloutText
		{
			get
			{
				return this._salloutText;
			}
			set
			{
				if (value != this._salloutText)
				{
					this._salloutText = value;
					base.OnPropertyChangedWithValue<string>(value, "SalloutText");
				}
			}
		}

		// Token: 0x040000E7 RID: 231
		private bool _isCurrentMapSiege;

		// Token: 0x040000E8 RID: 232
		private bool _isSallyOutSelected;

		// Token: 0x040000E9 RID: 233
		private SelectorVM<MapItemVM> _mapSelection;

		// Token: 0x040000EA RID: 234
		private SelectorVM<SceneLevelItemVM> _sceneLevelSelection;

		// Token: 0x040000EB RID: 235
		private SelectorVM<WallHitpointItemVM> _wallHitpointSelection;

		// Token: 0x040000EC RID: 236
		private SelectorVM<SeasonItemVM> _seasonSelection;

		// Token: 0x040000ED RID: 237
		private SelectorVM<TimeOfDayItemVM> _timeOfDaySelection;

		// Token: 0x040000EE RID: 238
		private string _titleText;

		// Token: 0x040000EF RID: 239
		private string _mapText;

		// Token: 0x040000F0 RID: 240
		private string _seasonText;

		// Token: 0x040000F1 RID: 241
		private string _timeOfDayText;

		// Token: 0x040000F2 RID: 242
		private string _sceneLevelText;

		// Token: 0x040000F3 RID: 243
		private string _wallHitpointsText;

		// Token: 0x040000F4 RID: 244
		private string _attackerSiegeMachinesText;

		// Token: 0x040000F5 RID: 245
		private string _defenderSiegeMachinesText;

		// Token: 0x040000F6 RID: 246
		private string _salloutText;
	}
}
