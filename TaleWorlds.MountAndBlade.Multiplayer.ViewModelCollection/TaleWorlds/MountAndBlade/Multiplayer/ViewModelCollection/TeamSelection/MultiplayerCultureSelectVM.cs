using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.TeamSelection
{
	// Token: 0x02000019 RID: 25
	public class MultiplayerCultureSelectVM : ViewModel
	{
		// Token: 0x06000155 RID: 341 RVA: 0x000062FC File Offset: 0x000044FC
		public MultiplayerCultureSelectVM(Action<BasicCultureObject> onCultureSelected, Action onClose)
		{
			this._onCultureSelected = onCultureSelected;
			this._onClose = onClose;
			this._firstCulture = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			this._secondCulture = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			this.FirstCultureCode = this._firstCulture.StringId;
			this.SecondCultureCode = this._secondCulture.StringId;
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(this._firstCulture, this._secondCulture);
			this.FirstCultureColor1 = multiplayerBattleColors.AttackerColors.Color1;
			this.FirstCultureColor2 = multiplayerBattleColors.AttackerColors.Color2;
			this.SecondCultureColor1 = multiplayerBattleColors.DefenderColors.Color1;
			this.SecondCultureColor2 = multiplayerBattleColors.DefenderColors.Color2;
			this.RefreshValues();
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000063CC File Offset: 0x000045CC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.GameModeText = GameTexts.FindText("str_multiplayer_official_game_type_name", MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)).ToString();
			this.CultureSelectionText = new TextObject("{=yQ0p8Glo}Select Culture", null).ToString();
			this.FirstCultureName = this._firstCulture.Name.ToString();
			this.SecondCultureName = this._secondCulture.Name.ToString();
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006440 File Offset: 0x00004640
		public void ExecuteSelectCulture(int cultureIndex)
		{
			if (cultureIndex == 0)
			{
				Action<BasicCultureObject> onCultureSelected = this._onCultureSelected;
				if (onCultureSelected == null)
				{
					return;
				}
				onCultureSelected(this._firstCulture);
				return;
			}
			else
			{
				if (cultureIndex != 1)
				{
					Debug.FailedAssert("Invalid Culture Index!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\TeamSelection\\MultiplayerCultureSelectVM.cs", "ExecuteSelectCulture", 65);
					return;
				}
				Action<BasicCultureObject> onCultureSelected2 = this._onCultureSelected;
				if (onCultureSelected2 == null)
				{
					return;
				}
				onCultureSelected2(this._secondCulture);
				return;
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006498 File Offset: 0x00004698
		public void ExecuteClose()
		{
			Action onClose = this._onClose;
			if (onClose == null)
			{
				return;
			}
			onClose();
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000064AA File Offset: 0x000046AA
		// (set) Token: 0x0600015A RID: 346 RVA: 0x000064B2 File Offset: 0x000046B2
		[DataSourceProperty]
		public string GameModeText
		{
			get
			{
				return this._gameModeText;
			}
			set
			{
				if (value != this._gameModeText)
				{
					this._gameModeText = value;
					base.OnPropertyChangedWithValue<string>(value, "GameModeText");
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600015B RID: 347 RVA: 0x000064D5 File Offset: 0x000046D5
		// (set) Token: 0x0600015C RID: 348 RVA: 0x000064DD File Offset: 0x000046DD
		[DataSourceProperty]
		public string CultureSelectionText
		{
			get
			{
				return this._cultureSelectionText;
			}
			set
			{
				if (value != this._cultureSelectionText)
				{
					this._cultureSelectionText = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureSelectionText");
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00006500 File Offset: 0x00004700
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00006508 File Offset: 0x00004708
		[DataSourceProperty]
		public string FirstCultureName
		{
			get
			{
				return this._firstCultureName;
			}
			set
			{
				if (value != this._firstCultureName)
				{
					this._firstCultureName = value;
					base.OnPropertyChangedWithValue<string>(value, "FirstCultureName");
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000652B File Offset: 0x0000472B
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00006533 File Offset: 0x00004733
		[DataSourceProperty]
		public string SecondCultureName
		{
			get
			{
				return this._secondCultureName;
			}
			set
			{
				if (value != this._secondCultureName)
				{
					this._secondCultureName = value;
					base.OnPropertyChangedWithValue<string>(value, "SecondCultureName");
				}
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00006556 File Offset: 0x00004756
		// (set) Token: 0x06000162 RID: 354 RVA: 0x0000655E File Offset: 0x0000475E
		[DataSourceProperty]
		public string FirstCultureCode
		{
			get
			{
				return this._firstCultureCode;
			}
			set
			{
				if (value != this._firstCultureCode)
				{
					this._firstCultureCode = value;
					base.OnPropertyChangedWithValue<string>(value, "FirstCultureCode");
				}
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00006581 File Offset: 0x00004781
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00006589 File Offset: 0x00004789
		[DataSourceProperty]
		public string SecondCultureCode
		{
			get
			{
				return this._secondCultureCode;
			}
			set
			{
				if (value != this._secondCultureCode)
				{
					this._secondCultureCode = value;
					base.OnPropertyChangedWithValue<string>(value, "SecondCultureCode");
				}
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000165 RID: 357 RVA: 0x000065AC File Offset: 0x000047AC
		// (set) Token: 0x06000166 RID: 358 RVA: 0x000065B4 File Offset: 0x000047B4
		[DataSourceProperty]
		public Color FirstCultureColor1
		{
			get
			{
				return this._firstCultureColor1;
			}
			set
			{
				if (value != this._firstCultureColor1)
				{
					this._firstCultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "FirstCultureColor1");
				}
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000167 RID: 359 RVA: 0x000065D7 File Offset: 0x000047D7
		// (set) Token: 0x06000168 RID: 360 RVA: 0x000065DF File Offset: 0x000047DF
		[DataSourceProperty]
		public Color FirstCultureColor2
		{
			get
			{
				return this._firstCultureColor2;
			}
			set
			{
				if (value != this._firstCultureColor2)
				{
					this._firstCultureColor2 = value;
					base.OnPropertyChangedWithValue(value, "FirstCultureColor2");
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00006602 File Offset: 0x00004802
		// (set) Token: 0x0600016A RID: 362 RVA: 0x0000660A File Offset: 0x0000480A
		[DataSourceProperty]
		public Color SecondCultureColor1
		{
			get
			{
				return this._secondCultureColor1;
			}
			set
			{
				if (value != this._secondCultureColor1)
				{
					this._secondCultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "SecondCultureColor1");
				}
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000662D File Offset: 0x0000482D
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00006635 File Offset: 0x00004835
		[DataSourceProperty]
		public Color SecondCultureColor2
		{
			get
			{
				return this._secondCultureColor2;
			}
			set
			{
				if (value != this._secondCultureColor2)
				{
					this._secondCultureColor2 = value;
					base.OnPropertyChangedWithValue(value, "SecondCultureColor2");
				}
			}
		}

		// Token: 0x040000B3 RID: 179
		private BasicCultureObject _firstCulture;

		// Token: 0x040000B4 RID: 180
		private BasicCultureObject _secondCulture;

		// Token: 0x040000B5 RID: 181
		private Action<BasicCultureObject> _onCultureSelected;

		// Token: 0x040000B6 RID: 182
		private Action _onClose;

		// Token: 0x040000B7 RID: 183
		private string _gameModeText;

		// Token: 0x040000B8 RID: 184
		private string _cultureSelectionText;

		// Token: 0x040000B9 RID: 185
		private string _firstCultureName;

		// Token: 0x040000BA RID: 186
		private string _secondCultureName;

		// Token: 0x040000BB RID: 187
		private Color _firstCultureColor1;

		// Token: 0x040000BC RID: 188
		private Color _firstCultureColor2;

		// Token: 0x040000BD RID: 189
		private Color _secondCultureColor1;

		// Token: 0x040000BE RID: 190
		private Color _secondCultureColor2;

		// Token: 0x040000BF RID: 191
		private string _firstCultureCode;

		// Token: 0x040000C0 RID: 192
		private string _secondCultureCode;
	}
}
