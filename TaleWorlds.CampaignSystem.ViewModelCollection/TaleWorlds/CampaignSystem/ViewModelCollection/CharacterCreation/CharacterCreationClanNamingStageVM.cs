using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000151 RID: 337
	public class CharacterCreationClanNamingStageVM : CharacterCreationStageBaseVM
	{
		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x060020AF RID: 8367 RVA: 0x00075804 File Offset: 0x00073A04
		// (set) Token: 0x060020B0 RID: 8368 RVA: 0x0007580C File Offset: 0x00073A0C
		public BasicCharacterObject Character { get; private set; }

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x060020B1 RID: 8369 RVA: 0x00075815 File Offset: 0x00073A15
		// (set) Token: 0x060020B2 RID: 8370 RVA: 0x0007581D File Offset: 0x00073A1D
		public int ShieldSlotIndex { get; private set; } = 3;

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x060020B3 RID: 8371 RVA: 0x00075826 File Offset: 0x00073A26
		// (set) Token: 0x060020B4 RID: 8372 RVA: 0x0007582E File Offset: 0x00073A2E
		public ItemRosterElement ShieldRosterElement { get; private set; }

		// Token: 0x060020B5 RID: 8373 RVA: 0x00075838 File Offset: 0x00073A38
		public CharacterCreationClanNamingStageVM(BasicCharacterObject character, CharacterCreationManager characterCreationManager, Action affirmativeAction, TextObject affirmativeActionText, Action negativeAction, TextObject negativeActionText)
			: base(characterCreationManager, affirmativeAction, affirmativeActionText, negativeAction, negativeActionText)
		{
			this.Character = character;
			this.ClanName = Hero.MainHero.Clan.Name.ToString();
			ItemObject itemObject = this.FindShield();
			this.ShieldRosterElement = new ItemRosterElement(itemObject, 1, null);
			this.ClanBanner = new BannerImageIdentifierVM(Hero.MainHero.Clan.Banner, true);
			this.CameraControlKeys = new MBBindingList<InputKeyItemVM>();
			this.RefreshValues();
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x000758BC File Offset: 0x00073ABC
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.Title = new TextObject("{=wNUcqcJP}Clan Name", null).ToString();
			base.Description = new TextObject("{=RSn1j3tA}Choose your family name: ", null).ToString();
			this.BottomHintText = new TextObject("{=dbBAJ8yi}You can change your banner and clan name later on clan screen", null).ToString();
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00075914 File Offset: 0x00073B14
		public override bool CanAdvanceToNextStage()
		{
			Tuple<bool, string> tuple = FactionHelper.IsClanNameApplicable(this.ClanName);
			this.ClanNameNotApplicableReason = tuple.Item2;
			return tuple.Item1;
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x0007593F File Offset: 0x00073B3F
		public override void OnNextStage()
		{
			this._affirmativeAction();
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x0007594C File Offset: 0x00073B4C
		public override void OnPreviousStage()
		{
			this._negativeAction();
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x0007595C File Offset: 0x00073B5C
		private ItemObject FindShield()
		{
			for (int i = 0; i < 4; i++)
			{
				EquipmentElement equipmentFromSlot = this.Character.Equipment.GetEquipmentFromSlot((EquipmentIndex)i);
				ItemObject item = equipmentFromSlot.Item;
				if (((item != null) ? item.PrimaryWeapon : null) != null && equipmentFromSlot.Item.PrimaryWeapon.IsShield && equipmentFromSlot.Item.IsUsingTableau)
				{
					return equipmentFromSlot.Item;
				}
			}
			foreach (ItemObject itemObject in Game.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (itemObject.PrimaryWeapon != null && itemObject.PrimaryWeapon.IsShield && itemObject.IsUsingTableau)
				{
					return itemObject;
				}
			}
			return null;
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x00075A34 File Offset: 0x00073C34
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			foreach (InputKeyItemVM inputKeyItemVM in this.CameraControlKeys)
			{
				inputKeyItemVM.OnFinalize();
			}
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x00075AA8 File Offset: 0x00073CA8
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x00075AB7 File Offset: 0x00073CB7
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x00075AC8 File Offset: 0x00073CC8
		public void AddCameraControlInputKey(HotKey hotKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x00075AEC File Offset: 0x00073CEC
		public void AddCameraControlInputKey(GameKey gameKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromGameKey(gameKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x00075B10 File Offset: 0x00073D10
		public void AddCameraControlInputKey(GameAxisKey gameAxisKey, TextObject keyName)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromForcedID(gameAxisKey.AxisKey.ToString(), keyName, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x060020C1 RID: 8385 RVA: 0x00075B3C File Offset: 0x00073D3C
		// (set) Token: 0x060020C2 RID: 8386 RVA: 0x00075B44 File Offset: 0x00073D44
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

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x060020C3 RID: 8387 RVA: 0x00075B62 File Offset: 0x00073D62
		// (set) Token: 0x060020C4 RID: 8388 RVA: 0x00075B6A File Offset: 0x00073D6A
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

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x060020C5 RID: 8389 RVA: 0x00075B88 File Offset: 0x00073D88
		// (set) Token: 0x060020C6 RID: 8390 RVA: 0x00075B90 File Offset: 0x00073D90
		[DataSourceProperty]
		public MBBindingList<InputKeyItemVM> CameraControlKeys
		{
			get
			{
				return this._cameraControlKeys;
			}
			set
			{
				if (value != this._cameraControlKeys)
				{
					this._cameraControlKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<InputKeyItemVM>>(value, "CameraControlKeys");
				}
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x060020C7 RID: 8391 RVA: 0x00075BAE File Offset: 0x00073DAE
		// (set) Token: 0x060020C8 RID: 8392 RVA: 0x00075BB6 File Offset: 0x00073DB6
		[DataSourceProperty]
		public string ClanName
		{
			get
			{
				return this._clanName;
			}
			set
			{
				if (value != this._clanName)
				{
					this._clanName = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanName");
					base.CanAdvance = this.CanAdvanceToNextStage();
				}
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x00075BE5 File Offset: 0x00073DE5
		// (set) Token: 0x060020CA RID: 8394 RVA: 0x00075BED File Offset: 0x00073DED
		[DataSourceProperty]
		public string ClanNameNotApplicableReason
		{
			get
			{
				return this._clanNameNotApplicableReason;
			}
			set
			{
				if (value != this._clanNameNotApplicableReason)
				{
					this._clanNameNotApplicableReason = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanNameNotApplicableReason");
				}
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x060020CB RID: 8395 RVA: 0x00075C10 File Offset: 0x00073E10
		// (set) Token: 0x060020CC RID: 8396 RVA: 0x00075C18 File Offset: 0x00073E18
		[DataSourceProperty]
		public string BottomHintText
		{
			get
			{
				return this._bottomHintText;
			}
			set
			{
				if (value != this._bottomHintText)
				{
					this._bottomHintText = value;
					base.OnPropertyChangedWithValue<string>(value, "BottomHintText");
				}
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x060020CD RID: 8397 RVA: 0x00075C3B File Offset: 0x00073E3B
		// (set) Token: 0x060020CE RID: 8398 RVA: 0x00075C43 File Offset: 0x00073E43
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

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x060020CF RID: 8399 RVA: 0x00075C61 File Offset: 0x00073E61
		// (set) Token: 0x060020D0 RID: 8400 RVA: 0x00075C69 File Offset: 0x00073E69
		[DataSourceProperty]
		public bool CharacterGamepadControlsEnabled
		{
			get
			{
				return this._characterGamepadControlsEnabled;
			}
			set
			{
				if (value != this._characterGamepadControlsEnabled)
				{
					this._characterGamepadControlsEnabled = value;
					base.OnPropertyChangedWithValue(value, "CharacterGamepadControlsEnabled");
				}
			}
		}

		// Token: 0x04000EFE RID: 3838
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000EFF RID: 3839
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000F00 RID: 3840
		private MBBindingList<InputKeyItemVM> _cameraControlKeys;

		// Token: 0x04000F01 RID: 3841
		private string _clanName;

		// Token: 0x04000F02 RID: 3842
		private string _clanNameNotApplicableReason;

		// Token: 0x04000F03 RID: 3843
		private string _bottomHintText;

		// Token: 0x04000F04 RID: 3844
		private BannerImageIdentifierVM _clanBanner;

		// Token: 0x04000F05 RID: 3845
		private bool _characterGamepadControlsEnabled;
	}
}
