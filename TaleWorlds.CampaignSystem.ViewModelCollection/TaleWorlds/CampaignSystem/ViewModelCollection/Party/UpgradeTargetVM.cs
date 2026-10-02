using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x02000030 RID: 48
	public class UpgradeTargetVM : ViewModel
	{
		// Token: 0x060004BF RID: 1215 RVA: 0x0001C094 File Offset: 0x0001A294
		public UpgradeTargetVM(int upgradeIndex, CharacterObject character, CharacterCode upgradeCharacterCode, Action<int, int> onUpgraded, Action<UpgradeTargetVM> onFocused)
		{
			this._upgradeIndex = upgradeIndex;
			this._originalCharacter = character;
			this._upgradeTarget = this._originalCharacter.UpgradeTargets[upgradeIndex];
			this._onUpgraded = onUpgraded;
			this._onFocused = onFocused;
			PerkObject perkObject;
			Campaign.Current.Models.PartyTroopUpgradeModel.DoesPartyHaveRequiredPerksForUpgrade(PartyBase.MainParty, this._originalCharacter, this._upgradeTarget, out perkObject);
			this.Requirements = new UpgradeRequirementsVM();
			this.Requirements.SetItemRequirement(this._upgradeTarget.UpgradeRequiresItemFromCategory);
			this.Requirements.SetPerkRequirement(perkObject);
			this.TroopImage = new CharacterImageIdentifierVM(upgradeCharacterCode);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001C139 File Offset: 0x0001A339
		public override void RefreshValues()
		{
			base.RefreshValues();
			UpgradeRequirementsVM requirements = this.Requirements;
			if (requirements == null)
			{
				return;
			}
			requirements.RefreshValues();
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001C154 File Offset: 0x0001A354
		public void Refresh(int upgradableAmount, bool isAvailable, bool isInsufficient, bool itemRequirementsMet, bool perkRequirementsMet, string hintString, bool isMarinerTroop)
		{
			this.AvailableUpgrades = upgradableAmount;
			this.IsAvailable = isAvailable;
			this.IsInsufficient = isInsufficient;
			this.IsMarinerTroop = isMarinerTroop;
			UpgradeRequirementsVM requirements = this.Requirements;
			if (requirements != null)
			{
				requirements.SetRequirementsMet(itemRequirementsMet, perkRequirementsMet);
			}
			this._hintString = hintString;
			this.Hint = new BasicTooltipViewModel(() => this.GetHint());
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0001C1B4 File Offset: 0x0001A3B4
		private string GetHint()
		{
			string stackModifierString = CampaignUIHelper.GetStackModifierString(GameTexts.FindText("str_entire_stack_shortcut_upgrade_units", null), GameTexts.FindText("str_five_stack_shortcut_upgrade_units", null), this.AvailableUpgrades >= 5);
			if (string.IsNullOrEmpty(stackModifierString) || this.AvailableUpgrades < 1)
			{
				return this._hintString;
			}
			return GameTexts.FindText("str_string_newline_string", null).SetTextVariable("STR1", this._hintString).SetTextVariable("STR2", stackModifierString)
				.ToString();
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001C22C File Offset: 0x0001A42C
		public void ExecuteUpgradeEncyclopediaLink()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this._upgradeTarget.EncyclopediaLink);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001C248 File Offset: 0x0001A448
		public void ExecuteUpgrade()
		{
			if (this.IsAvailable && !this.IsInsufficient)
			{
				Action<int, int> onUpgraded = this._onUpgraded;
				if (onUpgraded == null)
				{
					return;
				}
				onUpgraded(this._upgradeIndex, this.AvailableUpgrades);
			}
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001C276 File Offset: 0x0001A476
		public void ExecuteSetFocused()
		{
			if (this._upgradeTarget != null)
			{
				Action<UpgradeTargetVM> onFocused = this._onFocused;
				if (onFocused == null)
				{
					return;
				}
				onFocused(this);
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001C291 File Offset: 0x0001A491
		public void ExecuteSetUnfocused()
		{
			Action<UpgradeTargetVM> onFocused = this._onFocused;
			if (onFocused == null)
			{
				return;
			}
			onFocused(null);
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x0001C2A4 File Offset: 0x0001A4A4
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x0001C2AC File Offset: 0x0001A4AC
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

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x0001C2CA File Offset: 0x0001A4CA
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x0001C2D2 File Offset: 0x0001A4D2
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

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x0001C2F0 File Offset: 0x0001A4F0
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x0001C2F8 File Offset: 0x0001A4F8
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

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x0001C316 File Offset: 0x0001A516
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x0001C31E File Offset: 0x0001A51E
		[DataSourceProperty]
		public UpgradeRequirementsVM Requirements
		{
			get
			{
				return this._requirements;
			}
			set
			{
				if (value != this._requirements)
				{
					this._requirements = value;
					base.OnPropertyChangedWithValue<UpgradeRequirementsVM>(value, "Requirements");
				}
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x0001C33C File Offset: 0x0001A53C
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x0001C344 File Offset: 0x0001A544
		[DataSourceProperty]
		public CharacterImageIdentifierVM TroopImage
		{
			get
			{
				return this._troopImage;
			}
			set
			{
				if (value != this._troopImage)
				{
					this._troopImage = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "TroopImage");
				}
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x0001C362 File Offset: 0x0001A562
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x0001C36A File Offset: 0x0001A56A
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x0001C388 File Offset: 0x0001A588
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x0001C390 File Offset: 0x0001A590
		[DataSourceProperty]
		public int AvailableUpgrades
		{
			get
			{
				return this._availableUpgrades;
			}
			set
			{
				if (value != this._availableUpgrades)
				{
					this._availableUpgrades = value;
					base.OnPropertyChangedWithValue(value, "AvailableUpgrades");
				}
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0001C3AE File Offset: 0x0001A5AE
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x0001C3B6 File Offset: 0x0001A5B6
		[DataSourceProperty]
		public bool IsAvailable
		{
			get
			{
				return this._isAvailable;
			}
			set
			{
				if (value != this._isAvailable)
				{
					this._isAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsAvailable");
				}
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0001C3D4 File Offset: 0x0001A5D4
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x0001C3DC File Offset: 0x0001A5DC
		[DataSourceProperty]
		public bool IsInsufficient
		{
			get
			{
				return this._isInsufficient;
			}
			set
			{
				if (value != this._isInsufficient)
				{
					this._isInsufficient = value;
					base.OnPropertyChangedWithValue(value, "IsInsufficient");
				}
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0001C3FA File Offset: 0x0001A5FA
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x0001C402 File Offset: 0x0001A602
		[DataSourceProperty]
		public bool IsHighlighted
		{
			get
			{
				return this._isHighlighted;
			}
			set
			{
				if (value != this._isHighlighted)
				{
					this._isHighlighted = value;
					base.OnPropertyChangedWithValue(value, "IsHighlighted");
				}
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x0001C420 File Offset: 0x0001A620
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x0001C428 File Offset: 0x0001A628
		[DataSourceProperty]
		public bool IsMarinerTroop
		{
			get
			{
				return this._isMarinerTroop;
			}
			set
			{
				if (value != this._isMarinerTroop)
				{
					this._isMarinerTroop = value;
					base.OnPropertyChangedWithValue(value, "IsMarinerTroop");
				}
			}
		}

		// Token: 0x0400020C RID: 524
		private CharacterObject _originalCharacter;

		// Token: 0x0400020D RID: 525
		private CharacterObject _upgradeTarget;

		// Token: 0x0400020E RID: 526
		private Action<int, int> _onUpgraded;

		// Token: 0x0400020F RID: 527
		private Action<UpgradeTargetVM> _onFocused;

		// Token: 0x04000210 RID: 528
		private int _upgradeIndex;

		// Token: 0x04000211 RID: 529
		private string _hintString;

		// Token: 0x04000212 RID: 530
		private InputKeyItemVM _primaryActionInputKey;

		// Token: 0x04000213 RID: 531
		private InputKeyItemVM _secondaryActionInputKey;

		// Token: 0x04000214 RID: 532
		private InputKeyItemVM _tertiaryActionInputKey;

		// Token: 0x04000215 RID: 533
		private UpgradeRequirementsVM _requirements;

		// Token: 0x04000216 RID: 534
		private CharacterImageIdentifierVM _troopImage;

		// Token: 0x04000217 RID: 535
		private BasicTooltipViewModel _hint;

		// Token: 0x04000218 RID: 536
		private int _availableUpgrades;

		// Token: 0x04000219 RID: 537
		private bool _isAvailable;

		// Token: 0x0400021A RID: 538
		private bool _isInsufficient;

		// Token: 0x0400021B RID: 539
		private bool _isHighlighted;

		// Token: 0x0400021C RID: 540
		private bool _isMarinerTroop;
	}
}
