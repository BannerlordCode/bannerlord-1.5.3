using System;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MarriageOfferPopup
{
	// Token: 0x0200003A RID: 58
	public class MarriageOfferPopupVM : ViewModel
	{
		// Token: 0x060005AD RID: 1453 RVA: 0x0001E9E4 File Offset: 0x0001CBE4
		public MarriageOfferPopupVM(Hero suitor, Hero maiden, Action onClose)
		{
			this._marriageBehavior = Campaign.Current.GetCampaignBehavior<IMarriageOfferCampaignBehavior>();
			this._onClose = onClose;
			if (suitor.Clan == Clan.PlayerClan)
			{
				this.OffereeClanMember = new MarriageOfferPopupHeroVM(suitor);
				this.OffererClanMember = new MarriageOfferPopupHeroVM(maiden);
			}
			else
			{
				this.OffereeClanMember = new MarriageOfferPopupHeroVM(maiden);
				this.OffererClanMember = new MarriageOfferPopupHeroVM(suitor);
			}
			this.ConsequencesList = new MBBindingList<BindingListStringItem>();
			this.RefreshValues();
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001EA5E File Offset: 0x0001CC5E
		public void Update()
		{
			MarriageOfferPopupHeroVM offereeClanMember = this.OffereeClanMember;
			if (offereeClanMember != null)
			{
				offereeClanMember.Update();
			}
			MarriageOfferPopupHeroVM offererClanMember = this.OffererClanMember;
			if (offererClanMember == null)
			{
				return;
			}
			offererClanMember.Update();
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001EA81 File Offset: 0x0001CC81
		public void ExecuteAcceptOffer()
		{
			IMarriageOfferCampaignBehavior marriageBehavior = this._marriageBehavior;
			if (marriageBehavior != null)
			{
				marriageBehavior.OnMarriageOfferAcceptedOnPopUp();
			}
			Action onClose = this._onClose;
			if (onClose == null)
			{
				return;
			}
			onClose();
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001EAA4 File Offset: 0x0001CCA4
		public void ExecuteDeclineOffer()
		{
			IMarriageOfferCampaignBehavior marriageBehavior = this._marriageBehavior;
			if (marriageBehavior != null)
			{
				marriageBehavior.OnMarriageOfferDeclinedOnPopUp();
			}
			Action onClose = this._onClose;
			if (onClose == null)
			{
				return;
			}
			onClose();
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001EAC8 File Offset: 0x0001CCC8
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject textObject = GameTexts.FindText("str_marriage_offer_from_clan", null);
			textObject.SetTextVariable("CLAN_NAME", this.OffererClanMember.Hero.Clan.Name);
			this.TitleText = textObject.ToString();
			this.ClanText = GameTexts.FindText("str_clan", null).ToString();
			this.AgeText = new TextObject("{=jaaQijQs}Age", null).ToString();
			this.OccupationText = new TextObject("{=GZxFIeiJ}Occupation", null).ToString();
			this.RelationText = new TextObject("{=BlidMNGT}Relation", null).ToString();
			this.ConsequencesText = new TextObject("{=Lm6Mkhru}Consequences", null).ToString();
			this.ButtonOkLabel = new TextObject("{=Y94H6XnK}Accept", null).ToString();
			this.ButtonCancelLabel = new TextObject("{=cOgmdp9e}Decline", null).ToString();
			this.ConsequencesList.Clear();
			IMarriageOfferCampaignBehavior marriageBehavior = this._marriageBehavior;
			foreach (TextObject textObject2 in (((marriageBehavior != null) ? marriageBehavior.GetMarriageAcceptedConsequences() : null) ?? new MBBindingList<TextObject>()))
			{
				this.ConsequencesList.Add(new BindingListStringItem("- " + textObject2.ToString()));
			}
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001EC28 File Offset: 0x0001CE28
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			MarriageOfferPopupHeroVM offereeClanMember = this.OffereeClanMember;
			if (offereeClanMember != null)
			{
				offereeClanMember.OnFinalize();
			}
			MarriageOfferPopupHeroVM offererClanMember = this.OffererClanMember;
			if (offererClanMember == null)
			{
				return;
			}
			offererClanMember.OnFinalize();
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001EC7E File Offset: 0x0001CE7E
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x0001EC90 File Offset: 0x0001CE90
		// (set) Token: 0x060005B5 RID: 1461 RVA: 0x0001EC98 File Offset: 0x0001CE98
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

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0001ECBB File Offset: 0x0001CEBB
		// (set) Token: 0x060005B7 RID: 1463 RVA: 0x0001ECC3 File Offset: 0x0001CEC3
		[DataSourceProperty]
		public string ClanText
		{
			get
			{
				return this._clanText;
			}
			set
			{
				if (value != this._clanText)
				{
					this._clanText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanText");
				}
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x0001ECE6 File Offset: 0x0001CEE6
		// (set) Token: 0x060005B9 RID: 1465 RVA: 0x0001ECEE File Offset: 0x0001CEEE
		[DataSourceProperty]
		public string AgeText
		{
			get
			{
				return this._ageText;
			}
			set
			{
				if (value != this._ageText)
				{
					this._ageText = value;
					base.OnPropertyChangedWithValue<string>(value, "AgeText");
				}
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x0001ED11 File Offset: 0x0001CF11
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x0001ED19 File Offset: 0x0001CF19
		[DataSourceProperty]
		public string OccupationText
		{
			get
			{
				return this._occupationText;
			}
			set
			{
				if (value != this._occupationText)
				{
					this._occupationText = value;
					base.OnPropertyChangedWithValue<string>(value, "OccupationText");
				}
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x0001ED3C File Offset: 0x0001CF3C
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x0001ED44 File Offset: 0x0001CF44
		[DataSourceProperty]
		public string RelationText
		{
			get
			{
				return this._relationText;
			}
			set
			{
				if (value != this._relationText)
				{
					this._relationText = value;
					base.OnPropertyChangedWithValue<string>(value, "RelationText");
				}
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0001ED67 File Offset: 0x0001CF67
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x0001ED6F File Offset: 0x0001CF6F
		[DataSourceProperty]
		public string ConsequencesText
		{
			get
			{
				return this._consequencesText;
			}
			set
			{
				if (value != this._consequencesText)
				{
					this._consequencesText = value;
					base.OnPropertyChangedWithValue<string>(value, "ConsequencesText");
				}
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0001ED92 File Offset: 0x0001CF92
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x0001ED9A File Offset: 0x0001CF9A
		[DataSourceProperty]
		public MBBindingList<BindingListStringItem> ConsequencesList
		{
			get
			{
				return this._consequencesList;
			}
			set
			{
				if (value != this._consequencesList)
				{
					this._consequencesList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BindingListStringItem>>(value, "ConsequencesList");
				}
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0001EDB8 File Offset: 0x0001CFB8
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x0001EDC0 File Offset: 0x0001CFC0
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

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0001EDE3 File Offset: 0x0001CFE3
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x0001EDEB File Offset: 0x0001CFEB
		[DataSourceProperty]
		public string ButtonCancelLabel
		{
			get
			{
				return this._buttonCancelLabel;
			}
			set
			{
				if (value != this._buttonCancelLabel)
				{
					this._buttonCancelLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "ButtonCancelLabel");
				}
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x0001EE0E File Offset: 0x0001D00E
		// (set) Token: 0x060005C7 RID: 1479 RVA: 0x0001EE16 File Offset: 0x0001D016
		[DataSourceProperty]
		public bool IsEncyclopediaOpen
		{
			get
			{
				return this._isEncyclopediaOpen;
			}
			set
			{
				if (value != this._isEncyclopediaOpen)
				{
					this._isEncyclopediaOpen = value;
					base.OnPropertyChangedWithValue(value, "IsEncyclopediaOpen");
				}
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0001EE34 File Offset: 0x0001D034
		// (set) Token: 0x060005C9 RID: 1481 RVA: 0x0001EE3C File Offset: 0x0001D03C
		[DataSourceProperty]
		public MarriageOfferPopupHeroVM OffereeClanMember
		{
			get
			{
				return this._offereeClanMember;
			}
			set
			{
				if (value != this._offereeClanMember)
				{
					this._offereeClanMember = value;
					base.OnPropertyChangedWithValue<MarriageOfferPopupHeroVM>(value, "OffereeClanMember");
				}
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x0001EE5A File Offset: 0x0001D05A
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x0001EE62 File Offset: 0x0001D062
		[DataSourceProperty]
		public MarriageOfferPopupHeroVM OffererClanMember
		{
			get
			{
				return this._offererClanMember;
			}
			set
			{
				if (value != this._offererClanMember)
				{
					this._offererClanMember = value;
					base.OnPropertyChangedWithValue<MarriageOfferPopupHeroVM>(value, "OffererClanMember");
				}
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001EE80 File Offset: 0x0001D080
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001EE8F File Offset: 0x0001D08F
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0001EE9E File Offset: 0x0001D09E
		// (set) Token: 0x060005CF RID: 1487 RVA: 0x0001EEA6 File Offset: 0x0001D0A6
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0001EEC4 File Offset: 0x0001D0C4
		// (set) Token: 0x060005D1 RID: 1489 RVA: 0x0001EECC File Offset: 0x0001D0CC
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

		// Token: 0x0400026E RID: 622
		private readonly IMarriageOfferCampaignBehavior _marriageBehavior;

		// Token: 0x0400026F RID: 623
		private Action _onClose;

		// Token: 0x04000270 RID: 624
		private string _titleText;

		// Token: 0x04000271 RID: 625
		private string _clanText;

		// Token: 0x04000272 RID: 626
		private string _ageText;

		// Token: 0x04000273 RID: 627
		private string _occupationText;

		// Token: 0x04000274 RID: 628
		private string _relationText;

		// Token: 0x04000275 RID: 629
		private string _consequencesText;

		// Token: 0x04000276 RID: 630
		private MBBindingList<BindingListStringItem> _consequencesList;

		// Token: 0x04000277 RID: 631
		private string _buttonOkLabel;

		// Token: 0x04000278 RID: 632
		private string _buttonCancelLabel;

		// Token: 0x04000279 RID: 633
		private bool _isEncyclopediaOpen;

		// Token: 0x0400027A RID: 634
		private MarriageOfferPopupHeroVM _offereeClanMember;

		// Token: 0x0400027B RID: 635
		private MarriageOfferPopupHeroVM _offererClanMember;

		// Token: 0x0400027C RID: 636
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400027D RID: 637
		private InputKeyItemVM _doneInputKey;
	}
}
