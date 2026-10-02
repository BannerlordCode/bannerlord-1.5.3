using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.HeirSelectionPopup
{
	// Token: 0x02000067 RID: 103
	public class HeirSelectionPopupVM : ViewModel
	{
		// Token: 0x06000759 RID: 1881 RVA: 0x000237B0 File Offset: 0x000219B0
		public HeirSelectionPopupVM(Dictionary<Hero, int> heirApparents)
		{
			this.HeirApparents = new MBBindingList<HeirSelectionPopupHeroVM>();
			foreach (KeyValuePair<Hero, int> keyValuePair in heirApparents.OrderByDescending<KeyValuePair<Hero, int>, int>((KeyValuePair<Hero, int> x) => x.Value))
			{
				this.HeirApparents.Add(new HeirSelectionPopupHeroVM(keyValuePair.Key));
			}
			this.CurrentSelectedHero = this.HeirApparents[0];
			this.CurrentSelectedHero.IsSelected = true;
			this.ClanBanner = new BannerImageIdentifierVM(Clan.PlayerClan.Banner, true);
			this.RefreshValues();
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00023878 File Offset: 0x00021A78
		public void Update()
		{
			for (int i = 0; i < this.HeirApparents.Count; i++)
			{
				if (this.HeirApparents[i].IsSelected && this.HeirApparents[i] != this.CurrentSelectedHero)
				{
					this.CurrentSelectedHero.IsSelected = false;
					this.CurrentSelectedHero = this.HeirApparents[i];
				}
			}
			this.AreHotkeysVisible = !InformationManager.IsAnyInquiryActive();
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x000238F0 File Offset: 0x00021AF0
		public void ExecuteSelectHeir()
		{
			TextObject textObject = GameTexts.FindText("str_STR1_space_STR2", null);
			TextObject textObject2 = new TextObject("{=GEvP9i5f}You will play on as {HEIR.NAME}.", null);
			textObject2.SetCharacterProperties("HEIR", this.CurrentSelectedHero.Hero.CharacterObject, false);
			textObject.SetTextVariable("STR1", textObject2);
			textObject.SetTextVariable("STR2", new TextObject("{=awjomtnJ}Are you sure?", null));
			InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_decision", null).ToString(), textObject.ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
			{
				this.ExecuteFinalizeHeirSelection(this.CurrentSelectedHero.Hero);
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x000239B3 File Offset: 0x00021BB3
		private void ExecuteFinalizeHeirSelection(Hero selectedHeir)
		{
			CampaignEventDispatcher.Instance.OnHeirSelectionOver(selectedHeir);
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x000239C0 File Offset: 0x00021BC0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=2maftPJP}Assign As Clan & Faction Leader", null).ToString();
			this.ButtonOkLabel = new TextObject("{=KXQ7Mvec}Select As Main Character", null).ToString();
			this.NameLabel = GameTexts.FindText("str_LEFT_colon_wSpace", null).SetTextVariable("LEFT", GameTexts.FindText("str_name", null)).ToString();
			this.AgeLabel = GameTexts.FindText("str_LEFT_colon_wSpace", null).SetTextVariable("LEFT", GameTexts.FindText("str_age", null)).ToString();
			this.CultureLabel = GameTexts.FindText("str_LEFT_colon_wSpace", null).SetTextVariable("LEFT", GameTexts.FindText("str_culture", null)).ToString();
			this.OccupationLabel = GameTexts.FindText("str_LEFT_colon_wSpace", null).SetTextVariable("LEFT", GameTexts.FindText("str_occupation", null)).ToString();
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00023AAC File Offset: 0x00021CAC
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			for (int i = 0; i < this.HeirApparents.Count; i++)
			{
				this.HeirApparents[i].OnFinalize();
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x00023AF7 File Offset: 0x00021CF7
		// (set) Token: 0x06000760 RID: 1888 RVA: 0x00023AFF File Offset: 0x00021CFF
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

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x00023B22 File Offset: 0x00021D22
		// (set) Token: 0x06000762 RID: 1890 RVA: 0x00023B2A File Offset: 0x00021D2A
		[DataSourceProperty]
		public string ButtonOkLabel
		{
			get
			{
				return this._buttonOkLabel;
			}
			set
			{
				if (value != this._buttonOkLabel)
				{
					this._buttonOkLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "ButtonOkLabel");
				}
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x00023B4D File Offset: 0x00021D4D
		// (set) Token: 0x06000764 RID: 1892 RVA: 0x00023B55 File Offset: 0x00021D55
		[DataSourceProperty]
		public string NameLabel
		{
			get
			{
				return this._nameLabel;
			}
			set
			{
				if (value != this._nameLabel)
				{
					this._nameLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "NameLabel");
				}
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x00023B78 File Offset: 0x00021D78
		// (set) Token: 0x06000766 RID: 1894 RVA: 0x00023B80 File Offset: 0x00021D80
		[DataSourceProperty]
		public string AgeLabel
		{
			get
			{
				return this._ageLabel;
			}
			set
			{
				if (value != this._ageLabel)
				{
					this._ageLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "AgeLabel");
				}
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x00023BA3 File Offset: 0x00021DA3
		// (set) Token: 0x06000768 RID: 1896 RVA: 0x00023BAB File Offset: 0x00021DAB
		[DataSourceProperty]
		public string CultureLabel
		{
			get
			{
				return this._cultureLabel;
			}
			set
			{
				if (value != this._cultureLabel)
				{
					this._cultureLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureLabel");
				}
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x00023BCE File Offset: 0x00021DCE
		// (set) Token: 0x0600076A RID: 1898 RVA: 0x00023BD6 File Offset: 0x00021DD6
		[DataSourceProperty]
		public string OccupationLabel
		{
			get
			{
				return this._occupationLabel;
			}
			set
			{
				if (value != this._occupationLabel)
				{
					this._occupationLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "OccupationLabel");
				}
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x00023BF9 File Offset: 0x00021DF9
		// (set) Token: 0x0600076C RID: 1900 RVA: 0x00023C01 File Offset: 0x00021E01
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

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x00023C1F File Offset: 0x00021E1F
		// (set) Token: 0x0600076E RID: 1902 RVA: 0x00023C27 File Offset: 0x00021E27
		[DataSourceProperty]
		public MBBindingList<HeirSelectionPopupHeroVM> HeirApparents
		{
			get
			{
				return this._heirApparents;
			}
			set
			{
				if (value != this._heirApparents)
				{
					this._heirApparents = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeirSelectionPopupHeroVM>>(value, "HeirApparents");
				}
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x00023C45 File Offset: 0x00021E45
		// (set) Token: 0x06000770 RID: 1904 RVA: 0x00023C4D File Offset: 0x00021E4D
		[DataSourceProperty]
		public HeirSelectionPopupHeroVM CurrentSelectedHero
		{
			get
			{
				return this._currentSelectedHero;
			}
			set
			{
				if (value != this._currentSelectedHero)
				{
					this._currentSelectedHero = value;
					base.OnPropertyChangedWithValue<HeirSelectionPopupHeroVM>(value, "CurrentSelectedHero");
				}
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x00023C6B File Offset: 0x00021E6B
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x00023C73 File Offset: 0x00021E73
		[DataSourceProperty]
		public bool AreHotkeysVisible
		{
			get
			{
				return this._areHotkeysVisible;
			}
			set
			{
				if (value != this._areHotkeysVisible)
				{
					this._areHotkeysVisible = value;
					base.OnPropertyChangedWithValue(value, "AreHotkeysVisible");
				}
			}
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00023C91 File Offset: 0x00021E91
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x00023CA0 File Offset: 0x00021EA0
		// (set) Token: 0x06000775 RID: 1909 RVA: 0x00023CA8 File Offset: 0x00021EA8
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x04000329 RID: 809
		private string _titleText;

		// Token: 0x0400032A RID: 810
		private string _buttonOkLabel;

		// Token: 0x0400032B RID: 811
		private string _nameLabel;

		// Token: 0x0400032C RID: 812
		private string _ageLabel;

		// Token: 0x0400032D RID: 813
		private string _cultureLabel;

		// Token: 0x0400032E RID: 814
		private string _occupationLabel;

		// Token: 0x0400032F RID: 815
		private BannerImageIdentifierVM _clanBanner;

		// Token: 0x04000330 RID: 816
		private MBBindingList<HeirSelectionPopupHeroVM> _heirApparents;

		// Token: 0x04000331 RID: 817
		private HeirSelectionPopupHeroVM _currentSelectedHero;

		// Token: 0x04000332 RID: 818
		private bool _areHotkeysVisible;

		// Token: 0x04000333 RID: 819
		private InputKeyItemVM _doneInputKey;
	}
}
