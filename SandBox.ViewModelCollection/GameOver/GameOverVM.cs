using System;
using SandBox.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.GameOver
{
	// Token: 0x0200005D RID: 93
	public class GameOverVM : ViewModel
	{
		// Token: 0x060005B0 RID: 1456 RVA: 0x00015680 File Offset: 0x00013880
		public GameOverVM(GameOverState.GameOverReason reason, Action onClose)
		{
			this._onClose = onClose;
			this._reason = reason;
			this._statsProvider = new GameOverStatsProvider();
			this.Categories = new MBBindingList<GameOverStatCategoryVM>();
			this.IsPositiveGameOver = this._reason == GameOverState.GameOverReason.Victory;
			this.ClanBanner = new BannerImageIdentifierVM(Hero.MainHero.ClanBanner, true);
			this.ReasonAsString = Enum.GetName(typeof(GameOverState.GameOverReason), this._reason);
			this.RefreshValues();
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00015704 File Offset: 0x00013904
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CloseText = (this.IsPositiveGameOver ? new TextObject("{=AdgAJbAP}Return To The Map", null).ToString() : GameTexts.FindText("str_main_menu", null).ToString());
			this.TitleText = GameTexts.FindText("str_game_over_title", this.ReasonAsString).ToString();
			this.StatisticsTitle = GameTexts.FindText("str_statistics", null).ToString();
			this.Categories.Clear();
			foreach (StatCategory statCategory in this._statsProvider.GetGameOverStats())
			{
				this.Categories.Add(new GameOverStatCategoryVM(statCategory, new Action<GameOverStatCategoryVM>(this.OnCategorySelection)));
			}
			this.OnCategorySelection(this.Categories[0]);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x000157F0 File Offset: 0x000139F0
		private void OnCategorySelection(GameOverStatCategoryVM newCategory)
		{
			if (this._currentCategory != null)
			{
				this._currentCategory.IsSelected = false;
			}
			this._currentCategory = newCategory;
			if (this._currentCategory != null)
			{
				this._currentCategory.IsSelected = true;
			}
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00015821 File Offset: 0x00013A21
		public void ExecuteClose()
		{
			Action onClose = this._onClose;
			if (onClose == null)
			{
				return;
			}
			onClose.DynamicInvokeWithLog(Array.Empty<object>());
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00015839 File Offset: 0x00013A39
		public void SetCloseInputKey(HotKey hotKey)
		{
			this.CloseInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00015848 File Offset: 0x00013A48
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM closeInputKey = this.CloseInputKey;
			if (closeInputKey == null)
			{
				return;
			}
			closeInputKey.OnFinalize();
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00015860 File Offset: 0x00013A60
		// (set) Token: 0x060005B7 RID: 1463 RVA: 0x00015868 File Offset: 0x00013A68
		[DataSourceProperty]
		public string CloseText
		{
			get
			{
				return this._closeText;
			}
			set
			{
				if (value != this._closeText)
				{
					this._closeText = value;
					base.OnPropertyChangedWithValue<string>(value, "CloseText");
				}
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x0001588B File Offset: 0x00013A8B
		// (set) Token: 0x060005B9 RID: 1465 RVA: 0x00015893 File Offset: 0x00013A93
		[DataSourceProperty]
		public string StatisticsTitle
		{
			get
			{
				return this._statisticsTitle;
			}
			set
			{
				if (value != this._statisticsTitle)
				{
					this._statisticsTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "StatisticsTitle");
				}
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x000158B6 File Offset: 0x00013AB6
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x000158BE File Offset: 0x00013ABE
		[DataSourceProperty]
		public string ReasonAsString
		{
			get
			{
				return this._reasonAsString;
			}
			set
			{
				if (value != this._reasonAsString)
				{
					this._reasonAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "ReasonAsString");
				}
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x000158E1 File Offset: 0x00013AE1
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x000158E9 File Offset: 0x00013AE9
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

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0001590C File Offset: 0x00013B0C
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x00015914 File Offset: 0x00013B14
		[DataSourceProperty]
		public BannerImageIdentifierVM ClanBanner
		{
			get
			{
				return this._clanBanner;
			}
			set
			{
				if (value != this._clanBanner)
				{
					this._clanBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ClanBanner");
				}
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x00015932 File Offset: 0x00013B32
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x0001593A File Offset: 0x00013B3A
		[DataSourceProperty]
		public bool IsPositiveGameOver
		{
			get
			{
				return this._isPositiveGameOver;
			}
			set
			{
				if (value != this._isPositiveGameOver)
				{
					this._isPositiveGameOver = value;
					base.OnPropertyChangedWithValue(value, "IsPositiveGameOver");
				}
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x00015958 File Offset: 0x00013B58
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x00015960 File Offset: 0x00013B60
		[DataSourceProperty]
		public InputKeyItemVM CloseInputKey
		{
			get
			{
				return this._closeInputKey;
			}
			set
			{
				if (value != this._closeInputKey)
				{
					this._closeInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CloseInputKey");
				}
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0001597E File Offset: 0x00013B7E
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x00015986 File Offset: 0x00013B86
		[DataSourceProperty]
		public MBBindingList<GameOverStatCategoryVM> Categories
		{
			get
			{
				return this._categories;
			}
			set
			{
				if (value != this._categories)
				{
					this._categories = value;
					base.OnPropertyChangedWithValue<MBBindingList<GameOverStatCategoryVM>>(value, "Categories");
				}
			}
		}

		// Token: 0x040002DB RID: 731
		private readonly Action _onClose;

		// Token: 0x040002DC RID: 732
		private readonly GameOverStatsProvider _statsProvider;

		// Token: 0x040002DD RID: 733
		private readonly GameOverState.GameOverReason _reason;

		// Token: 0x040002DE RID: 734
		private GameOverStatCategoryVM _currentCategory;

		// Token: 0x040002DF RID: 735
		private string _closeText;

		// Token: 0x040002E0 RID: 736
		private string _titleText;

		// Token: 0x040002E1 RID: 737
		private string _reasonAsString;

		// Token: 0x040002E2 RID: 738
		private string _statisticsTitle;

		// Token: 0x040002E3 RID: 739
		private bool _isPositiveGameOver;

		// Token: 0x040002E4 RID: 740
		private InputKeyItemVM _closeInputKey;

		// Token: 0x040002E5 RID: 741
		private BannerImageIdentifierVM _clanBanner;

		// Token: 0x040002E6 RID: 742
		private MBBindingList<GameOverStatCategoryVM> _categories;
	}
}
