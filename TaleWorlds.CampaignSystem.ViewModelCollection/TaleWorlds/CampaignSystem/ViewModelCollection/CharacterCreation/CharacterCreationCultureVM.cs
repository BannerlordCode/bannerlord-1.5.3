using System;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000154 RID: 340
	public class CharacterCreationCultureVM : ViewModel
	{
		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x060020EB RID: 8427 RVA: 0x00076210 File Offset: 0x00074410
		public CultureObject Culture { get; }

		// Token: 0x060020EC RID: 8428 RVA: 0x00076218 File Offset: 0x00074418
		public CharacterCreationCultureVM(CultureObject culture, Action<CharacterCreationCultureVM> onSelection)
		{
			this._onSelection = onSelection;
			this.Culture = culture;
			CharacterCreationState characterCreationState = GameStateManager.Current.ActiveState as CharacterCreationState;
			CharacterCreationContent characterCreationContent = ((characterCreationState != null) ? characterCreationState.CharacterCreationManager.CharacterCreationContent : null);
			MBTextManager.SetTextVariable("FOCUS_VALUE", characterCreationContent.GetFocusToAddByCulture(culture));
			MBTextManager.SetTextVariable("EXP_VALUE", characterCreationContent.GetSkillLevelToAddByCulture(culture));
			this.DescriptionText = GameTexts.FindText("str_culture_description", this.Culture.StringId).ToString();
			this.ShortenedNameText = GameTexts.FindText("str_culture_rich_name", this.Culture.StringId).ToString();
			this.NameText = GameTexts.FindText("str_culture_rich_name", this.Culture.StringId).ToString();
			this.CultureID = ((culture != null) ? culture.StringId : null) ?? "";
			this.CultureColor1 = Color.FromUint((culture != null) ? culture.Color : Color.White.ToUnsignedInteger());
			this.Feats = new MBBindingList<CharacterCreationCultureFeatVM>();
			foreach (FeatObject featObject in this.Culture.GetCulturalFeats((FeatObject x) => x.IsPositive))
			{
				this.Feats.Add(new CharacterCreationCultureFeatVM(true, featObject.Description.ToString()));
			}
			foreach (FeatObject featObject2 in this.Culture.GetCulturalFeats((FeatObject x) => !x.IsPositive))
			{
				this.Feats.Add(new CharacterCreationCultureFeatVM(false, featObject2.Description.ToString()));
			}
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x00076420 File Offset: 0x00074620
		public void ExecuteSelectCulture()
		{
			this._onSelection(this);
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x060020EE RID: 8430 RVA: 0x0007642E File Offset: 0x0007462E
		// (set) Token: 0x060020EF RID: 8431 RVA: 0x00076436 File Offset: 0x00074636
		[DataSourceProperty]
		public string CultureID
		{
			get
			{
				return this._cultureID;
			}
			set
			{
				if (value != this._cultureID)
				{
					this._cultureID = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureID");
				}
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x060020F0 RID: 8432 RVA: 0x00076459 File Offset: 0x00074659
		// (set) Token: 0x060020F1 RID: 8433 RVA: 0x00076461 File Offset: 0x00074661
		[DataSourceProperty]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (value != this._cultureColor1)
				{
					this._cultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor1");
				}
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x060020F2 RID: 8434 RVA: 0x00076484 File Offset: 0x00074684
		// (set) Token: 0x060020F3 RID: 8435 RVA: 0x0007648C File Offset: 0x0007468C
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x060020F4 RID: 8436 RVA: 0x000764AF File Offset: 0x000746AF
		// (set) Token: 0x060020F5 RID: 8437 RVA: 0x000764B7 File Offset: 0x000746B7
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x060020F6 RID: 8438 RVA: 0x000764DA File Offset: 0x000746DA
		// (set) Token: 0x060020F7 RID: 8439 RVA: 0x000764E2 File Offset: 0x000746E2
		[DataSourceProperty]
		public string ShortenedNameText
		{
			get
			{
				return this._shortenedNameText;
			}
			set
			{
				if (value != this._shortenedNameText)
				{
					this._shortenedNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "ShortenedNameText");
				}
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x060020F8 RID: 8440 RVA: 0x00076505 File Offset: 0x00074705
		// (set) Token: 0x060020F9 RID: 8441 RVA: 0x0007650D File Offset: 0x0007470D
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

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x060020FA RID: 8442 RVA: 0x0007652B File Offset: 0x0007472B
		// (set) Token: 0x060020FB RID: 8443 RVA: 0x00076533 File Offset: 0x00074733
		[DataSourceProperty]
		public MBBindingList<CharacterCreationCultureFeatVM> Feats
		{
			get
			{
				return this._feats;
			}
			set
			{
				if (value != this._feats)
				{
					this._feats = value;
					base.OnPropertyChangedWithValue<MBBindingList<CharacterCreationCultureFeatVM>>(value, "Feats");
				}
			}
		}

		// Token: 0x04000F0F RID: 3855
		private readonly Action<CharacterCreationCultureVM> _onSelection;

		// Token: 0x04000F10 RID: 3856
		private string _descriptionText = "";

		// Token: 0x04000F11 RID: 3857
		private string _nameText;

		// Token: 0x04000F12 RID: 3858
		private string _shortenedNameText;

		// Token: 0x04000F13 RID: 3859
		private bool _isSelected;

		// Token: 0x04000F14 RID: 3860
		private string _cultureID;

		// Token: 0x04000F15 RID: 3861
		private Color _cultureColor1;

		// Token: 0x04000F16 RID: 3862
		private MBBindingList<CharacterCreationCultureFeatVM> _feats;
	}
}
