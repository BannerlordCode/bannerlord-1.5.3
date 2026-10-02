using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party.PartyTroopManagerPopUp
{
	// Token: 0x02000034 RID: 52
	public abstract class PartyTroopManagerVM : ViewModel
	{
		// Token: 0x06000513 RID: 1299 RVA: 0x0001CAEF File Offset: 0x0001ACEF
		public virtual void ExecuteItemPrimaryAction()
		{
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001CAF1 File Offset: 0x0001ACF1
		public virtual void ExecuteItemSecondaryAction()
		{
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001CAF3 File Offset: 0x0001ACF3
		public virtual void ExecuteItemTertiaryAction()
		{
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001CAF8 File Offset: 0x0001ACF8
		public PartyTroopManagerVM(PartyVM partyVM)
		{
			this._partyVM = partyVM;
			this.Troops = new MBBindingList<PartyTroopManagerItemVM>();
			this.OpenButtonHint = new HintViewModel();
			this.UsedHorsesHint = new BasicTooltipViewModel();
			this.RefreshValues();
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0001CB70 File Offset: 0x0001AD70
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.AvatarText = new TextObject("{=5tbWdY1j}Avatar", null).ToString();
			this.NameText = new TextObject("{=PDdh1sBj}Name", null).ToString();
			this.CountText = new TextObject("{=zFDoDbNj}Count", null).ToString();
			this.DoneLbl = GameTexts.FindText("str_done", null).ToString();
			this.CancelLbl = GameTexts.FindText("str_cancel", null).ToString();
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001CBF4 File Offset: 0x0001ADF4
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.DoneInputKey.OnFinalize();
			this.CancelInputKey.OnFinalize();
			InputKeyItemVM primaryActionInputKey = this.PrimaryActionInputKey;
			if (primaryActionInputKey != null)
			{
				primaryActionInputKey.OnFinalize();
			}
			InputKeyItemVM secondaryActionInputKey = this.SecondaryActionInputKey;
			if (secondaryActionInputKey != null)
			{
				secondaryActionInputKey.OnFinalize();
			}
			InputKeyItemVM tertiaryActionInputKey = this.TertiaryActionInputKey;
			if (tertiaryActionInputKey == null)
			{
				return;
			}
			tertiaryActionInputKey.OnFinalize();
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001CC50 File Offset: 0x0001AE50
		public virtual void OpenPopUp()
		{
			this._partyVM.PartyScreenLogic.SavePartyScreenData();
			this._initialGoldChange = this._partyVM.PartyScreenLogic.CurrentData.PartyGoldChangeAmount;
			this._initialHorseChange = this._partyVM.PartyScreenLogic.CurrentData.PartyHorseChangeAmount;
			this._initialMoraleChange = this._partyVM.PartyScreenLogic.CurrentData.PartyMoraleChangeAmount;
			this._initialUsedUpgradeHorsesHistory.Clear();
			foreach (Tuple<EquipmentElement, int> tuple in this._partyVM.PartyScreenLogic.CurrentData.UsedUpgradeHorsesHistory)
			{
				this._initialUsedUpgradeHorsesHistory.Add(tuple);
			}
			this.UpdateLabels();
			this._hasMadeChanges = false;
			this.IsOpen = true;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001CD38 File Offset: 0x0001AF38
		public virtual void ExecuteDone()
		{
			this.IsOpen = false;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001CD41 File Offset: 0x0001AF41
		protected virtual void ConfirmCancel()
		{
			this._partyVM.PartyScreenLogic.ResetToLastSavedPartyScreenData(false);
			this.IsOpen = false;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001CD5C File Offset: 0x0001AF5C
		public void UpdateOpenButtonHint(bool isDisabled, bool isIrrelevant, bool isUpgradesDisabled)
		{
			TextObject textObject;
			if (isIrrelevant)
			{
				textObject = this._openButtonIrrelevantScreenHint;
			}
			else if (isUpgradesDisabled)
			{
				textObject = this._openButtonUpgradesDisabledHint;
			}
			else if (isDisabled)
			{
				textObject = this._openButtonNoTroopsHint;
			}
			else
			{
				textObject = this._openButtonEnabledHint;
			}
			this.OpenButtonHint.HintText = textObject;
		}

		// Token: 0x0600051D RID: 1309
		public abstract void ExecuteCancel();

		// Token: 0x0600051E RID: 1310 RVA: 0x0001CDA4 File Offset: 0x0001AFA4
		protected void ShowCancelInquiry(Action confirmCancel)
		{
			if (this._hasMadeChanges)
			{
				string text = new TextObject("{=a8NoW1Q2}Are you sure you want to cancel your changes?", null).ToString();
				InformationManager.ShowInquiry(new InquiryData("", text, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					confirmCancel();
				}, null, "", 0f, null, null, null), false, false);
				return;
			}
			confirmCancel();
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001CE34 File Offset: 0x0001B034
		protected void UpdateLabels()
		{
			MBTextManager.SetTextVariable("PAY_OR_GET", 0);
			int num = this._partyVM.PartyScreenLogic.CurrentData.PartyGoldChangeAmount - this._initialGoldChange;
			int num2 = this._partyVM.PartyScreenLogic.CurrentData.PartyHorseChangeAmount - this._initialHorseChange;
			float num3 = this._partyVM.PartyScreenLogic.CurrentData.PartyMoraleChangeAmount - this._initialMoraleChange;
			MBTextManager.SetTextVariable("LABEL_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">", false);
			MBTextManager.SetTextVariable("TRADE_AMOUNT", MathF.Abs(num));
			this.GoldChangeText = ((num == 0) ? "" : GameTexts.FindText("str_party_generic_label", null).ToString());
			MBTextManager.SetTextVariable("LABEL_ICON", "{=!}<img src=\"StdAssets\\ItemIcons\\Mount\" extend=\"14\">", false);
			MBTextManager.SetTextVariable("TRADE_AMOUNT", MathF.Abs(num2));
			this.HorseChangeText = ((num2 == 0) ? "" : GameTexts.FindText("str_party_generic_label", null).ToString());
			MBTextManager.SetTextVariable("LABEL_ICON", "{=!}<img src=\"General\\Icons\\Morale@2x\" extend=\"4\">", false);
			MBTextManager.SetTextVariable("TRADE_AMOUNT", MathF.Abs(num3), 2);
			this.MoraleChangeText = ((num3 == 0f) ? "" : GameTexts.FindText("str_party_generic_label", null).ToString());
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001CF6C File Offset: 0x0001B16C
		protected void SetFocusedCharacter(PartyTroopManagerItemVM troop)
		{
			this.FocusedTroop = troop;
			this.IsFocusedOnACharacter = troop != null;
			if (this.FocusedTroop == null)
			{
				this.IsPrimaryActionAvailable = false;
				this.IsSecondaryActionAvailable = false;
				this.IsTertiaryActionAvailable = false;
				return;
			}
			if (this.IsUpgradePopUp)
			{
				MBBindingList<UpgradeTargetVM> upgrades = this.FocusedTroop.PartyCharacter.Upgrades;
				this.IsPrimaryActionAvailable = upgrades.Count > 0 && upgrades[0].IsAvailable && !upgrades[0].IsInsufficient;
				this.IsSecondaryActionAvailable = upgrades.Count > 1 && upgrades[1].IsAvailable && !upgrades[1].IsInsufficient;
				this.IsTertiaryActionAvailable = upgrades.Count > 2 && upgrades[2].IsAvailable && !upgrades[2].IsInsufficient;
				return;
			}
			this.IsPrimaryActionAvailable = this.FocusedTroop.IsTroopRecruitable;
			this.IsSecondaryActionAvailable = false;
			this.IsTertiaryActionAvailable = false;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001D071 File Offset: 0x0001B271
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001D080 File Offset: 0x0001B280
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001D08F File Offset: 0x0001B28F
		public void SetPrimaryActionInputKey(HotKey hotKey)
		{
			this.PrimaryActionInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001D09E File Offset: 0x0001B29E
		public void SetSecondaryActionInputKey(HotKey hotKey)
		{
			this.SecondaryActionInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001D0AD File Offset: 0x0001B2AD
		public void SetTertiaryActionInputKey(HotKey hotKey)
		{
			this.TertiaryActionInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0001D0BC File Offset: 0x0001B2BC
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x0001D0C4 File Offset: 0x0001B2C4
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

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x0001D0E2 File Offset: 0x0001B2E2
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x0001D0EA File Offset: 0x0001B2EA
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

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x0001D108 File Offset: 0x0001B308
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x0001D110 File Offset: 0x0001B310
		[DataSourceProperty]
		public InputKeyItemVM PrimaryActionInputKey
		{
			get
			{
				return this._primaryActionInputKey;
			}
			set
			{
				if (value != this._primaryActionInputKey)
				{
					this._primaryActionInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PrimaryActionInputKey");
				}
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x0001D12E File Offset: 0x0001B32E
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x0001D136 File Offset: 0x0001B336
		[DataSourceProperty]
		public InputKeyItemVM SecondaryActionInputKey
		{
			get
			{
				return this._secondaryActionInputKey;
			}
			set
			{
				if (value != this._secondaryActionInputKey)
				{
					this._secondaryActionInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "SecondaryActionInputKey");
				}
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x0001D154 File Offset: 0x0001B354
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x0001D15C File Offset: 0x0001B35C
		[DataSourceProperty]
		public InputKeyItemVM TertiaryActionInputKey
		{
			get
			{
				return this._tertiaryActionInputKey;
			}
			set
			{
				if (value != this._tertiaryActionInputKey)
				{
					this._tertiaryActionInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "TertiaryActionInputKey");
				}
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x0001D17A File Offset: 0x0001B37A
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x0001D182 File Offset: 0x0001B382
		[DataSourceProperty]
		public bool IsFocusedOnACharacter
		{
			get
			{
				return this._isFocusedOnACharacter;
			}
			set
			{
				if (value != this._isFocusedOnACharacter)
				{
					this._isFocusedOnACharacter = value;
					base.OnPropertyChangedWithValue(value, "IsFocusedOnACharacter");
				}
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x0001D1A0 File Offset: 0x0001B3A0
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x0001D1A8 File Offset: 0x0001B3A8
		[DataSourceProperty]
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
			set
			{
				if (value != this._isOpen)
				{
					this._isOpen = value;
					base.OnPropertyChangedWithValue(value, "IsOpen");
				}
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x0001D1C6 File Offset: 0x0001B3C6
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x0001D1CE File Offset: 0x0001B3CE
		[DataSourceProperty]
		public bool IsUpgradePopUp
		{
			get
			{
				return this._isUpgradePopUp;
			}
			set
			{
				if (value != this._isUpgradePopUp)
				{
					this._isUpgradePopUp = value;
					base.OnPropertyChangedWithValue(value, "IsUpgradePopUp");
				}
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0001D1EC File Offset: 0x0001B3EC
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x0001D1F4 File Offset: 0x0001B3F4
		[DataSourceProperty]
		public bool IsPrimaryActionAvailable
		{
			get
			{
				return this._isPrimaryActionAvailable;
			}
			set
			{
				if (value != this._isPrimaryActionAvailable)
				{
					this._isPrimaryActionAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsPrimaryActionAvailable");
				}
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001D212 File Offset: 0x0001B412
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x0001D21A File Offset: 0x0001B41A
		[DataSourceProperty]
		public bool IsSecondaryActionAvailable
		{
			get
			{
				return this._isSecondaryActionAvailable;
			}
			set
			{
				if (value != this._isSecondaryActionAvailable)
				{
					this._isSecondaryActionAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsSecondaryActionAvailable");
				}
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0001D238 File Offset: 0x0001B438
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x0001D240 File Offset: 0x0001B440
		[DataSourceProperty]
		public bool IsTertiaryActionAvailable
		{
			get
			{
				return this._isTertiaryActionAvailable;
			}
			set
			{
				if (value != this._isTertiaryActionAvailable)
				{
					this._isTertiaryActionAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsTertiaryActionAvailable");
				}
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001D25E File Offset: 0x0001B45E
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x0001D266 File Offset: 0x0001B466
		[DataSourceProperty]
		public PartyTroopManagerItemVM FocusedTroop
		{
			get
			{
				return this._focusedTroop;
			}
			set
			{
				if (value != this._focusedTroop)
				{
					this._focusedTroop = value;
					base.OnPropertyChangedWithValue<PartyTroopManagerItemVM>(value, "FocusedTroop");
				}
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0001D284 File Offset: 0x0001B484
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x0001D28C File Offset: 0x0001B48C
		[DataSourceProperty]
		public MBBindingList<PartyTroopManagerItemVM> Troops
		{
			get
			{
				return this._troops;
			}
			set
			{
				if (value != this._troops)
				{
					this._troops = value;
					base.OnPropertyChangedWithValue<MBBindingList<PartyTroopManagerItemVM>>(value, "Troops");
				}
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x0001D2AA File Offset: 0x0001B4AA
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x0001D2B2 File Offset: 0x0001B4B2
		[DataSourceProperty]
		public HintViewModel OpenButtonHint
		{
			get
			{
				return this._openButtonHint;
			}
			set
			{
				if (value != this._openButtonHint)
				{
					this._openButtonHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "OpenButtonHint");
				}
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x0001D2D0 File Offset: 0x0001B4D0
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x0001D2D8 File Offset: 0x0001B4D8
		[DataSourceProperty]
		public BasicTooltipViewModel UsedHorsesHint
		{
			get
			{
				return this._usedHorsesHint;
			}
			set
			{
				if (value != this._usedHorsesHint)
				{
					this._usedHorsesHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "UsedHorsesHint");
				}
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x0001D2F6 File Offset: 0x0001B4F6
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x0001D2FE File Offset: 0x0001B4FE
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

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x0001D321 File Offset: 0x0001B521
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x0001D329 File Offset: 0x0001B529
		[DataSourceProperty]
		public string AvatarText
		{
			get
			{
				return this._avatarText;
			}
			set
			{
				if (value != this._avatarText)
				{
					this._avatarText = value;
					base.OnPropertyChangedWithValue<string>(value, "AvatarText");
				}
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x0001D34C File Offset: 0x0001B54C
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x0001D354 File Offset: 0x0001B554
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

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x0001D377 File Offset: 0x0001B577
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x0001D37F File Offset: 0x0001B57F
		[DataSourceProperty]
		public string CountText
		{
			get
			{
				return this._countText;
			}
			set
			{
				if (value != this._countText)
				{
					this._countText = value;
					base.OnPropertyChangedWithValue<string>(value, "CountText");
				}
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x0001D3A2 File Offset: 0x0001B5A2
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x0001D3AA File Offset: 0x0001B5AA
		[DataSourceProperty]
		public string GoldChangeText
		{
			get
			{
				return this._goldChangeText;
			}
			set
			{
				if (value != this._goldChangeText)
				{
					this._goldChangeText = value;
					base.OnPropertyChangedWithValue<string>(value, "GoldChangeText");
				}
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0001D3CD File Offset: 0x0001B5CD
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x0001D3D5 File Offset: 0x0001B5D5
		[DataSourceProperty]
		public string HorseChangeText
		{
			get
			{
				return this._horseChangeText;
			}
			set
			{
				if (value != this._horseChangeText)
				{
					this._horseChangeText = value;
					base.OnPropertyChangedWithValue<string>(value, "HorseChangeText");
				}
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0001D3F8 File Offset: 0x0001B5F8
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x0001D400 File Offset: 0x0001B600
		[DataSourceProperty]
		public string MoraleChangeText
		{
			get
			{
				return this._moraleChangeText;
			}
			set
			{
				if (value != this._moraleChangeText)
				{
					this._moraleChangeText = value;
					base.OnPropertyChangedWithValue<string>(value, "MoraleChangeText");
				}
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0001D423 File Offset: 0x0001B623
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x0001D42B File Offset: 0x0001B62B
		[DataSourceProperty]
		public string DoneLbl
		{
			get
			{
				return this._doneLbl;
			}
			set
			{
				if (value != this._doneLbl)
				{
					this._doneLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneLbl");
				}
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x0001D44E File Offset: 0x0001B64E
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x0001D456 File Offset: 0x0001B656
		[DataSourceProperty]
		public string CancelLbl
		{
			get
			{
				return this._cancelLbl;
			}
			set
			{
				if (value != this._cancelLbl)
				{
					this._cancelLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelLbl");
				}
			}
		}

		// Token: 0x0400022D RID: 557
		protected PartyVM _partyVM;

		// Token: 0x0400022E RID: 558
		protected bool _hasMadeChanges;

		// Token: 0x0400022F RID: 559
		protected TextObject _openButtonEnabledHint = TextObject.GetEmpty();

		// Token: 0x04000230 RID: 560
		protected TextObject _openButtonNoTroopsHint = TextObject.GetEmpty();

		// Token: 0x04000231 RID: 561
		protected TextObject _openButtonIrrelevantScreenHint = TextObject.GetEmpty();

		// Token: 0x04000232 RID: 562
		protected TextObject _openButtonUpgradesDisabledHint = TextObject.GetEmpty();

		// Token: 0x04000233 RID: 563
		private int _initialGoldChange;

		// Token: 0x04000234 RID: 564
		private int _initialHorseChange;

		// Token: 0x04000235 RID: 565
		private float _initialMoraleChange;

		// Token: 0x04000236 RID: 566
		protected List<Tuple<EquipmentElement, int>> _initialUsedUpgradeHorsesHistory = new List<Tuple<EquipmentElement, int>>();

		// Token: 0x04000237 RID: 567
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000238 RID: 568
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000239 RID: 569
		private InputKeyItemVM _primaryActionInputKey;

		// Token: 0x0400023A RID: 570
		private InputKeyItemVM _secondaryActionInputKey;

		// Token: 0x0400023B RID: 571
		private InputKeyItemVM _tertiaryActionInputKey;

		// Token: 0x0400023C RID: 572
		private bool _isFocusedOnACharacter;

		// Token: 0x0400023D RID: 573
		private bool _isOpen;

		// Token: 0x0400023E RID: 574
		private bool _isUpgradePopUp;

		// Token: 0x0400023F RID: 575
		private bool _isPrimaryActionAvailable;

		// Token: 0x04000240 RID: 576
		private bool _isSecondaryActionAvailable;

		// Token: 0x04000241 RID: 577
		private bool _isTertiaryActionAvailable;

		// Token: 0x04000242 RID: 578
		private PartyTroopManagerItemVM _focusedTroop;

		// Token: 0x04000243 RID: 579
		private MBBindingList<PartyTroopManagerItemVM> _troops;

		// Token: 0x04000244 RID: 580
		private HintViewModel _openButtonHint;

		// Token: 0x04000245 RID: 581
		private BasicTooltipViewModel _usedHorsesHint;

		// Token: 0x04000246 RID: 582
		private string _titleText;

		// Token: 0x04000247 RID: 583
		private string _avatarText;

		// Token: 0x04000248 RID: 584
		private string _nameText;

		// Token: 0x04000249 RID: 585
		private string _countText;

		// Token: 0x0400024A RID: 586
		private string _goldChangeText;

		// Token: 0x0400024B RID: 587
		private string _horseChangeText;

		// Token: 0x0400024C RID: 588
		private string _moraleChangeText;

		// Token: 0x0400024D RID: 589
		private string _doneLbl;

		// Token: 0x0400024E RID: 590
		private string _cancelLbl;
	}
}
