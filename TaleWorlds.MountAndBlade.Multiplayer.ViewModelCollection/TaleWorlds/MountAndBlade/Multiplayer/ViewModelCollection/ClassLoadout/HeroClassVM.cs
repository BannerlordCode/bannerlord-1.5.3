using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000A8 RID: 168
	public class HeroClassVM : ViewModel
	{
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x0600101B RID: 4123 RVA: 0x00032423 File Offset: 0x00030623
		// (set) Token: 0x0600101C RID: 4124 RVA: 0x0003242B File Offset: 0x0003062B
		public List<IReadOnlyPerkObject> SelectedPerks { get; private set; }

		// Token: 0x0600101D RID: 4125 RVA: 0x00032434 File Offset: 0x00030634
		public HeroClassVM(Action<HeroClassVM> onSelect, Action<HeroPerkVM, MPPerkVM> onPerkSelect, MultiplayerClassDivisions.MPHeroClass heroClass, MultiplayerBattleColors.MultiplayerCultureColorInfo colorInfo)
		{
			this.HeroClass = heroClass;
			this._onSelect = onSelect;
			this._onPerkSelect = onPerkSelect;
			this.CultureId = heroClass.Culture.StringId;
			this.IconType = heroClass.IconType.ToString();
			this.TroopTypeId = heroClass.ClassGroup.StringId;
			this.CultureColor = colorInfo.Color1;
			this._gameMode = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this.Gold = (this._gameMode.IsGameModeUsingCasualGold ? this.HeroClass.TroopCasualCost : ((this._gameMode.GameType == MultiplayerGameType.Battle) ? this.HeroClass.TroopBattleCost : this.HeroClass.TroopCost));
			this.InitPerksList();
			int intValue = MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			this.IsNumOfTroopsEnabled = !this._gameMode.IsInWarmup && intValue > 0;
			if (this.IsNumOfTroopsEnabled)
			{
				this.NumOfTroops = MPPerkObject.GetTroopCount(heroClass, intValue, MPPerkObject.GetOnSpawnPerkHandler(this._perks.Select<HeroPerkVM, IReadOnlyPerkObject>((HeroPerkVM p) => p.SelectedPerk)));
			}
			this.UpdateEnabled();
			this.RefreshValues();
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00032574 File Offset: 0x00030774
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.HeroClass.HeroName.ToString();
			this.Perks.ApplyActionOnAllItems(delegate(HeroPerkVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x000325C8 File Offset: 0x000307C8
		private void InitPerksList()
		{
			List<List<IReadOnlyPerkObject>> allPerksForHeroClass = MultiplayerClassDivisions.GetAllPerksForHeroClass(this.HeroClass, null);
			if (this.SelectedPerks == null)
			{
				this.SelectedPerks = new List<IReadOnlyPerkObject>();
			}
			else
			{
				this.SelectedPerks.Clear();
			}
			for (int i = 0; i < allPerksForHeroClass.Count; i++)
			{
				if (allPerksForHeroClass[i].Count > 0)
				{
					this.SelectedPerks.Add(allPerksForHeroClass[i][0]);
				}
				else
				{
					this.SelectedPerks.Add(null);
				}
			}
			if (GameNetwork.IsMyPeerReady)
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				int num = MultiplayerClassDivisions.GetMPHeroClasses(this.HeroClass.Culture).ToList<MultiplayerClassDivisions.MPHeroClass>().IndexOf(this.HeroClass);
				component.NextSelectedTroopIndex = num;
				for (int j = 0; j < allPerksForHeroClass.Count; j++)
				{
					if (allPerksForHeroClass[j].Count > 0)
					{
						int num2 = component.GetSelectedPerkIndexWithPerkListIndex(num, j);
						if (num2 >= allPerksForHeroClass[j].Count)
						{
							num2 = 0;
						}
						IReadOnlyPerkObject readOnlyPerkObject = allPerksForHeroClass[j][num2];
						this.SelectedPerks[j] = readOnlyPerkObject;
					}
				}
			}
			MBBindingList<HeroPerkVM> mbbindingList = new MBBindingList<HeroPerkVM>();
			for (int k = 0; k < allPerksForHeroClass.Count; k++)
			{
				if (allPerksForHeroClass[k].Count > 0)
				{
					mbbindingList.Add(new HeroPerkVM(this._onPerkSelect, this.SelectedPerks[k], allPerksForHeroClass[k], k));
				}
			}
			this.Perks = mbbindingList;
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00032748 File Offset: 0x00030948
		public void UpdateEnabled()
		{
			this.IsEnabled = this._gameMode.IsClassAvailable(this.HeroClass) && (this._gameMode.IsInWarmup || !this._gameMode.IsGameModeUsingGold || this._gameMode.GetGoldAmount() >= this.Gold);
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x000327A4 File Offset: 0x000309A4
		[UsedImplicitly]
		public void OnSelect()
		{
			this._onSelect(this);
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001022 RID: 4130 RVA: 0x000327B2 File Offset: 0x000309B2
		// (set) Token: 0x06001023 RID: 4131 RVA: 0x000327BA File Offset: 0x000309BA
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x000327D8 File Offset: 0x000309D8
		// (set) Token: 0x06001025 RID: 4133 RVA: 0x000327E0 File Offset: 0x000309E0
		[DataSourceProperty]
		public MBBindingList<HeroPerkVM> Perks
		{
			get
			{
				return this._perks;
			}
			set
			{
				if (value != this._perks)
				{
					this._perks = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroPerkVM>>(value, "Perks");
				}
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x000327FE File Offset: 0x000309FE
		// (set) Token: 0x06001027 RID: 4135 RVA: 0x00032806 File Offset: 0x00030A06
		[DataSourceProperty]
		public string CultureId
		{
			get
			{
				return this._cultureId;
			}
			set
			{
				if (value != this._cultureId)
				{
					this._cultureId = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureId");
				}
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001028 RID: 4136 RVA: 0x00032829 File Offset: 0x00030A29
		// (set) Token: 0x06001029 RID: 4137 RVA: 0x00032831 File Offset: 0x00030A31
		[DataSourceProperty]
		public string TroopTypeId
		{
			get
			{
				return this._troopTypeId;
			}
			set
			{
				if (value != this._troopTypeId)
				{
					this._troopTypeId = value;
					base.OnPropertyChangedWithValue<string>(value, "TroopTypeId");
				}
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x0600102A RID: 4138 RVA: 0x00032854 File Offset: 0x00030A54
		// (set) Token: 0x0600102B RID: 4139 RVA: 0x0003285C File Offset: 0x00030A5C
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x0600102C RID: 4140 RVA: 0x0003287A File Offset: 0x00030A7A
		// (set) Token: 0x0600102D RID: 4141 RVA: 0x00032882 File Offset: 0x00030A82
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x0600102E RID: 4142 RVA: 0x000328A5 File Offset: 0x00030AA5
		// (set) Token: 0x0600102F RID: 4143 RVA: 0x000328AD File Offset: 0x00030AAD
		[DataSourceProperty]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChangedWithValue<string>(value, "IconType");
				}
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001030 RID: 4144 RVA: 0x000328D0 File Offset: 0x00030AD0
		// (set) Token: 0x06001031 RID: 4145 RVA: 0x000328D8 File Offset: 0x00030AD8
		[DataSourceProperty]
		public int Gold
		{
			get
			{
				return this._gold;
			}
			set
			{
				if (value != this._gold)
				{
					this._gold = value;
					base.OnPropertyChangedWithValue(value, "Gold");
				}
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001032 RID: 4146 RVA: 0x000328F6 File Offset: 0x00030AF6
		// (set) Token: 0x06001033 RID: 4147 RVA: 0x000328FE File Offset: 0x00030AFE
		[DataSourceProperty]
		public int NumOfTroops
		{
			get
			{
				return this._numOfTroops;
			}
			set
			{
				if (value != this._numOfTroops)
				{
					this._numOfTroops = value;
					base.OnPropertyChangedWithValue(value, "NumOfTroops");
				}
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0003291C File Offset: 0x00030B1C
		// (set) Token: 0x06001035 RID: 4149 RVA: 0x00032924 File Offset: 0x00030B24
		[DataSourceProperty]
		public bool IsGoldEnabled
		{
			get
			{
				return this._isGoldEnabled;
			}
			set
			{
				if (value != this._isGoldEnabled)
				{
					this._isGoldEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsGoldEnabled");
				}
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x00032942 File Offset: 0x00030B42
		// (set) Token: 0x06001037 RID: 4151 RVA: 0x0003294A File Offset: 0x00030B4A
		[DataSourceProperty]
		public bool IsNumOfTroopsEnabled
		{
			get
			{
				return this._isNumOfTroopsEnabled;
			}
			set
			{
				if (value != this._isNumOfTroopsEnabled)
				{
					this._isNumOfTroopsEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsNumOfTroopsEnabled");
				}
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x00032968 File Offset: 0x00030B68
		// (set) Token: 0x06001039 RID: 4153 RVA: 0x00032970 File Offset: 0x00030B70
		[DataSourceProperty]
		public Color CultureColor
		{
			get
			{
				return this._cultureColor;
			}
			set
			{
				if (value != this._cultureColor)
				{
					this._cultureColor = value;
					base.OnPropertyChangedWithValue(value, "CultureColor");
				}
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x00032993 File Offset: 0x00030B93
		[DataSourceProperty]
		public HeroPerkVM FirstPerk
		{
			get
			{
				return this.Perks.ElementAtOrDefault<HeroPerkVM>(0);
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x000329A1 File Offset: 0x00030BA1
		[DataSourceProperty]
		public HeroPerkVM SecondPerk
		{
			get
			{
				return this.Perks.ElementAtOrDefault<HeroPerkVM>(1);
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x0600103C RID: 4156 RVA: 0x000329AF File Offset: 0x00030BAF
		[DataSourceProperty]
		public HeroPerkVM ThirdPerk
		{
			get
			{
				return this.Perks.ElementAtOrDefault<HeroPerkVM>(2);
			}
		}

		// Token: 0x0400078D RID: 1933
		private readonly MissionMultiplayerGameModeBaseClient _gameMode;

		// Token: 0x0400078E RID: 1934
		public readonly MultiplayerClassDivisions.MPHeroClass HeroClass;

		// Token: 0x0400078F RID: 1935
		private readonly Action<HeroClassVM> _onSelect;

		// Token: 0x04000790 RID: 1936
		private Action<HeroPerkVM, MPPerkVM> _onPerkSelect;

		// Token: 0x04000792 RID: 1938
		private bool _isSelected;

		// Token: 0x04000793 RID: 1939
		private string _name;

		// Token: 0x04000794 RID: 1940
		private string _iconType;

		// Token: 0x04000795 RID: 1941
		private int _gold;

		// Token: 0x04000796 RID: 1942
		private int _numOfTroops;

		// Token: 0x04000797 RID: 1943
		private bool _isEnabled;

		// Token: 0x04000798 RID: 1944
		private bool _isGoldEnabled;

		// Token: 0x04000799 RID: 1945
		private bool _isNumOfTroopsEnabled;

		// Token: 0x0400079A RID: 1946
		private string _cultureId;

		// Token: 0x0400079B RID: 1947
		private string _troopTypeId;

		// Token: 0x0400079C RID: 1948
		private Color _cultureColor;

		// Token: 0x0400079D RID: 1949
		private MBBindingList<HeroPerkVM> _perks;
	}
}
