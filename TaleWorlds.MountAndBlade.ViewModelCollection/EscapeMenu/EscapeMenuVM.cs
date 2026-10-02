using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu
{
	// Token: 0x02000080 RID: 128
	public class EscapeMenuVM : ViewModel
	{
		// Token: 0x06000AAC RID: 2732 RVA: 0x000262B0 File Offset: 0x000244B0
		public EscapeMenuVM(IEnumerable<EscapeMenuItemVM> items, TextObject title = null)
		{
			this._titleObj = title;
			this.MenuItems = new MBBindingList<EscapeMenuItemVM>();
			if (items != null)
			{
				foreach (EscapeMenuItemVM escapeMenuItemVM in items)
				{
					this.MenuItems.Add(escapeMenuItemVM);
				}
			}
			this.Tips = new GameTipsVM(true, true);
			this.RefreshValues();
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0002632C File Offset: 0x0002452C
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject titleObj = this._titleObj;
			this.Title = ((titleObj != null) ? titleObj.ToString() : null) ?? "";
			this.StartingOptionsTitle = new TextObject("{=rVCb112K}Starting Options", null).ToString();
			this.MenuItems.ApplyActionOnAllItems(delegate(EscapeMenuItemVM x)
			{
				x.RefreshValues();
			});
			this.Tips.RefreshValues();
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x000263AC File Offset: 0x000245AC
		public void InitializeCampaignStartingOptionsInfo(string startScenario, uint startSeed)
		{
			TextObject textObject = new TextObject("{=5FVxxNxG}Scenario: {SCENARIO}", null);
			textObject.SetTextVariable("SCENARIO", startScenario);
			TextObject textObject2 = new TextObject("{=hDaYab60}Seed: {SEED}", null);
			textObject2.SetTextVariable("SEED", startSeed.ToString());
			this._startSeed = startSeed;
			this.StartScenarioText = textObject.ToString();
			this.StartSeedText = textObject2.ToString();
			this.ShowCampaignInfo = !string.IsNullOrEmpty(startScenario);
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0002641F File Offset: 0x0002461F
		public virtual void Tick(float dt)
		{
			if (this.CopiedTextTimer > 0f)
			{
				this.CopiedTextTimer -= dt;
				return;
			}
			if (!string.IsNullOrEmpty(this.CopiedText))
			{
				this.CopiedText = string.Empty;
			}
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00026458 File Offset: 0x00024658
		public void RefreshItems(IEnumerable<EscapeMenuItemVM> items)
		{
			this.MenuItems.Clear();
			foreach (EscapeMenuItemVM escapeMenuItemVM in items)
			{
				this.MenuItems.Add(escapeMenuItemVM);
			}
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x000264B0 File Offset: 0x000246B0
		public void ExecuteCopyGameSeed()
		{
			Input.SetClipboardText(this.StartSeedText);
			this.CopiedText = new TextObject("{=e8AmrZQ9}Copied", null).ToString();
			this.CopiedTextTimer = 2f;
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x000264DE File Offset: 0x000246DE
		// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x000264E6 File Offset: 0x000246E6
		[DataSourceProperty]
		public bool ShowCampaignInfo
		{
			get
			{
				return this._showCampaignInfo;
			}
			set
			{
				if (value != this._showCampaignInfo)
				{
					this._showCampaignInfo = value;
					base.OnPropertyChangedWithValue(value, "ShowCampaignInfo");
				}
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x00026504 File Offset: 0x00024704
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x0002650C File Offset: 0x0002470C
		[DataSourceProperty]
		public float CopiedTextTimer
		{
			get
			{
				return this._copiedTextTimer;
			}
			set
			{
				if (value != this._copiedTextTimer)
				{
					this._copiedTextTimer = value;
					base.OnPropertyChangedWithValue(value, "CopiedTextTimer");
				}
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0002652A File Offset: 0x0002472A
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x00026532 File Offset: 0x00024732
		[DataSourceProperty]
		public string CopiedText
		{
			get
			{
				return this._copiedText;
			}
			set
			{
				if (value != this._copiedText)
				{
					this._copiedText = value;
					base.OnPropertyChangedWithValue<string>(value, "CopiedText");
				}
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00026555 File Offset: 0x00024755
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x0002655D File Offset: 0x0002475D
		[DataSourceProperty]
		public string StartScenarioText
		{
			get
			{
				return this._startScenarioText;
			}
			set
			{
				if (value != this._startScenarioText)
				{
					this._startScenarioText = value;
					base.OnPropertyChangedWithValue<string>(value, "StartScenarioText");
				}
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x00026580 File Offset: 0x00024780
		// (set) Token: 0x06000ABB RID: 2747 RVA: 0x00026588 File Offset: 0x00024788
		[DataSourceProperty]
		public string StartSeedText
		{
			get
			{
				return this._startSeedText;
			}
			set
			{
				if (value != this._startSeedText)
				{
					this._startSeedText = value;
					base.OnPropertyChangedWithValue<string>(value, "StartSeedText");
				}
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x000265AB File Offset: 0x000247AB
		// (set) Token: 0x06000ABD RID: 2749 RVA: 0x000265B3 File Offset: 0x000247B3
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x000265D6 File Offset: 0x000247D6
		// (set) Token: 0x06000ABF RID: 2751 RVA: 0x000265DE File Offset: 0x000247DE
		[DataSourceProperty]
		public string StartingOptionsTitle
		{
			get
			{
				return this._startingOptionsTitle;
			}
			set
			{
				if (value != this._startingOptionsTitle)
				{
					this._startingOptionsTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "StartingOptionsTitle");
				}
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x00026601 File Offset: 0x00024801
		// (set) Token: 0x06000AC1 RID: 2753 RVA: 0x00026609 File Offset: 0x00024809
		[DataSourceProperty]
		public MBBindingList<EscapeMenuItemVM> MenuItems
		{
			get
			{
				return this._menuItems;
			}
			set
			{
				if (value != this._menuItems)
				{
					this._menuItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<EscapeMenuItemVM>>(value, "MenuItems");
				}
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x00026627 File Offset: 0x00024827
		// (set) Token: 0x06000AC3 RID: 2755 RVA: 0x0002662F File Offset: 0x0002482F
		[DataSourceProperty]
		public GameTipsVM Tips
		{
			get
			{
				return this._tips;
			}
			set
			{
				if (value != this._tips)
				{
					this._tips = value;
					base.OnPropertyChangedWithValue<GameTipsVM>(value, "Tips");
				}
			}
		}

		// Token: 0x040004EC RID: 1260
		private const float CopiedTextDisappearTime = 2f;

		// Token: 0x040004ED RID: 1261
		private readonly TextObject _titleObj;

		// Token: 0x040004EE RID: 1262
		private uint _startSeed;

		// Token: 0x040004EF RID: 1263
		private string _title;

		// Token: 0x040004F0 RID: 1264
		private string _startingOptionsTitle;

		// Token: 0x040004F1 RID: 1265
		private MBBindingList<EscapeMenuItemVM> _menuItems;

		// Token: 0x040004F2 RID: 1266
		private GameTipsVM _tips;

		// Token: 0x040004F3 RID: 1267
		private bool _showCampaignInfo;

		// Token: 0x040004F4 RID: 1268
		private float _copiedTextTimer;

		// Token: 0x040004F5 RID: 1269
		private string _copiedText;

		// Token: 0x040004F6 RID: 1270
		private string _startScenarioText;

		// Token: 0x040004F7 RID: 1271
		private string _startSeedText;
	}
}
