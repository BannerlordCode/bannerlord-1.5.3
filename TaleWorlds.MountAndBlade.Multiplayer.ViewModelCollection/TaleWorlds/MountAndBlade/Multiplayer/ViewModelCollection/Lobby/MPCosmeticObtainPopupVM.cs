using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x02000027 RID: 39
	public class MPCosmeticObtainPopupVM : ViewModel
	{
		// Token: 0x060002A4 RID: 676 RVA: 0x0000AC40 File Offset: 0x00008E40
		public MPCosmeticObtainPopupVM(Action<string, int> onItemObtained, Func<string> getContinueKeyText)
		{
			this._onItemObtained = onItemObtained;
			this._getExitText = getContinueKeyText;
			this._characterEquipments = new List<EquipmentElement>();
			this.ItemVisual = new ItemCollectionElementViewModel();
			this.Cultures = new MBBindingList<MPCultureItemVM>
			{
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("vlandia").StringId, new Action<MPCultureItemVM>(this.OnCultureSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("sturgia").StringId, new Action<MPCultureItemVM>(this.OnCultureSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire").StringId, new Action<MPCultureItemVM>(this.OnCultureSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("battania").StringId, new Action<MPCultureItemVM>(this.OnCultureSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("khuzait").StringId, new Action<MPCultureItemVM>(this.OnCultureSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("aserai").StringId, new Action<MPCultureItemVM>(this.OnCultureSelection))
			};
			Dictionary<BasicCultureObject, string> dictionary = new Dictionary<BasicCultureObject, string>();
			BasicCultureObject culture = this.Cultures[0].Culture;
			dictionary[culture] = "mp_tall_heater_shield_light";
			BasicCultureObject culture2 = this.Cultures[1].Culture;
			dictionary[culture2] = "mp_worn_kite_shield";
			BasicCultureObject culture3 = this.Cultures[2].Culture;
			dictionary[culture3] = "mp_leather_bound_kite_shield";
			BasicCultureObject culture4 = this.Cultures[3].Culture;
			dictionary[culture4] = "mp_highland_riders_shield";
			BasicCultureObject culture5 = this.Cultures[4].Culture;
			dictionary[culture5] = "mp_eastern_wicker_shield";
			BasicCultureObject culture6 = this.Cultures[5].Culture;
			dictionary[culture6] = "mp_desert_oval_shield";
			this._cultureShieldItemIDs = dictionary;
			this.CharacterVisual = new CharacterViewModel();
			MPArmoryCosmeticsVM.OnEquipmentRefreshed += this.OnCharacterEquipmentRefreshed;
			this.RefreshValues();
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000AEA4 File Offset: 0x000090A4
		public override void RefreshValues()
		{
			this.ContinueText = GameTexts.FindText("str_continue", null).ToString();
			this.NotEnoughLootText = new TextObject("{=FzFqhHKU}Not enough loot", null).ToString();
			this.PreviewAsText = new TextObject("{=V0bpuzV3}Preview as", null).ToString();
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000AEF3 File Offset: 0x000090F3
		public override void OnFinalize()
		{
			base.OnFinalize();
			MPArmoryCosmeticsVM.OnEquipmentRefreshed -= this.OnCharacterEquipmentRefreshed;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000AF0C File Offset: 0x0000910C
		private void OnCharacterEquipmentRefreshed(List<EquipmentElement> equipments)
		{
			this._characterEquipments.Clear();
			this._characterEquipments.AddRange(equipments);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000AF28 File Offset: 0x00009128
		public void OpenWith(MPArmoryCosmeticClothingItemVM item)
		{
			this.OnOpened();
			this.Item = item;
			this.ItemVisual.FillFrom(item.EquipmentElement, null);
			this.ItemVisual.BannerCode = "";
			this.ItemVisual.InitialPanRotation = 0f;
			this.ObtainDescriptionText = new TextObject("{=7uILxbP5}You will obtain this item", null).ToString();
			this._activeCosmeticID = item.CosmeticID;
			this.IsOpenedWithClothingItem = true;
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_continue", null));
			GameTexts.SetVariable("STR2", item.Cost);
			this.ContinueText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			MPArmoryCosmeticClothingItemVM item2 = this.Item;
			int? num = ((item2 != null) ? new int?(item2.Cost) : null);
			int gold = NetworkMain.GameClient.PlayerData.Gold;
			this.CanObtain = (num.GetValueOrDefault() <= gold) & (num != null);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000B024 File Offset: 0x00009224
		public void OpenWith(MPArmoryCosmeticTauntItemVM item, CharacterViewModel sourceCharacter)
		{
			this.OnOpened();
			TauntCosmeticElement tauntCosmeticElement = item.TauntCosmeticElement;
			this.TauntItem = item;
			Equipment equipment = LobbyTauntHelper.PrepareForTaunt(Equipment.CreateFromEquipmentCode(sourceCharacter.EquipmentCode), tauntCosmeticElement, false);
			EquipmentIndex equipmentIndex;
			EquipmentIndex equipmentIndex2;
			bool flag;
			equipment.GetInitialWeaponIndicesToEquip(out equipmentIndex, out equipmentIndex2, out flag, Equipment.InitialWeaponEquipPreference.Any);
			this.CharacterVisual.RightHandWieldedEquipmentIndex = (int)equipmentIndex;
			if (!flag)
			{
				this.CharacterVisual.LeftHandWieldedEquipmentIndex = (int)equipmentIndex2;
			}
			this.CharacterVisual.FillFrom(sourceCharacter, -1);
			this.CharacterVisual.SetEquipment(equipment);
			string defaultAction = TauntUsageManager.Instance.GetDefaultAction(TauntUsageManager.Instance.GetIndexOfAction(tauntCosmeticElement.Id));
			this.CharacterVisual.ExecuteStartCustomAnimation(TauntUsageManager.Instance.GetDefaultAction(TauntUsageManager.Instance.GetIndexOfAction(tauntCosmeticElement.Id)), true, 0.35f);
			this.AnimationVariationText = this.GetAnimationVariationText(defaultAction);
			this.ItemVisual.BannerCode = "";
			this.ItemVisual.InitialPanRotation = 0f;
			this._activeCosmeticID = tauntCosmeticElement.Id;
			this.IsOpenedWithTauntItem = true;
			this.ObtainDescriptionText = new TextObject("{=6mrCNU5U}You will obtain this taunt", null).ToString();
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_continue", null));
			GameTexts.SetVariable("STR2", item.Cost);
			this.ContinueText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			MPArmoryCosmeticTauntItemVM tauntItem = this.TauntItem;
			int? num = ((tauntItem != null) ? new int?(tauntItem.Cost) : null);
			int gold = NetworkMain.GameClient.PlayerData.Gold;
			this.CanObtain = (num.GetValueOrDefault() <= gold) & (num != null);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000B1C4 File Offset: 0x000093C4
		private string GetAnimationVariationText(string animationName)
		{
			if (animationName.EndsWith("leftstance"))
			{
				return new TextObject("{=8DSymjRe}Left Stance", null).ToString();
			}
			if (animationName.EndsWith("bow"))
			{
				return new TextObject("{=5rj7xQE4}Bow", null).ToString();
			}
			return new TextObject("{=fMSYE6Ii}Default", null).ToString();
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000B220 File Offset: 0x00009420
		public void ExecuteSelectNextAnimation(int increment)
		{
			MPArmoryCosmeticTauntItemVM tauntItem = this.TauntItem;
			if (((tauntItem != null) ? tauntItem.TauntCosmeticElement : null) == null)
			{
				Debug.FailedAssert("Invalid taunt cosmetic item", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\MPCosmeticObtainPopupVM.cs", "ExecuteSelectNextAnimation", 177);
				return;
			}
			TauntUsageManager.TauntUsageSet usageSet = TauntUsageManager.Instance.GetUsageSet(this._tauntItem.TauntCosmeticElement.Id);
			if (usageSet == null)
			{
				Debug.FailedAssert("No usage set for taunt: " + this.TauntItem.TauntCosmeticElement.Id, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\MPCosmeticObtainPopupVM.cs", "ExecuteSelectNextAnimation", 185);
				return;
			}
			MBReadOnlyList<TauntUsageManager.TauntUsage> usages = usageSet.GetUsages();
			if (usages == null || usages.Count == 0)
			{
				Debug.FailedAssert("No usages assigned for taunt usage set: " + this.TauntItem.TauntCosmeticElement.Id, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\MPCosmeticObtainPopupVM.cs", "ExecuteSelectNextAnimation", 193);
				return;
			}
			this._currentTauntUsageIndex += increment;
			if (this._currentTauntUsageIndex >= usages.Count)
			{
				this._currentTauntUsageIndex = 0;
			}
			else if (this._currentTauntUsageIndex < 0)
			{
				this._currentTauntUsageIndex = usages.Count - 1;
			}
			string action = usages[this._currentTauntUsageIndex].GetAction();
			this.CharacterVisual.ExecuteStartCustomAnimation(usages[this._currentTauntUsageIndex].GetAction(), true, 0f);
			this.AnimationVariationText = this.GetAnimationVariationText(action);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000B368 File Offset: 0x00009568
		public void OpenWith(MPLobbyCosmeticSigilItemVM sigilItem)
		{
			this.OnOpened();
			this.SigilItem = sigilItem;
			this._activeCosmeticID = sigilItem.CosmeticID;
			this.IsOpenedWithSigilItem = true;
			this.ItemVisual.InitialPanRotation = -3.3f;
			this.ObtainDescriptionText = new TextObject("{=7uILxbP5}You will obtain this item", null).ToString();
			MPCultureItemVM mpcultureItemVM = this.Cultures.FirstOrDefault<MPCultureItemVM>((MPCultureItemVM c) => c.IsSelected);
			if (mpcultureItemVM != null)
			{
				mpcultureItemVM.IsSelected = false;
			}
			this.Cultures[0].IsSelected = true;
			this.OnCultureSelection(this.Cultures[0]);
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_continue", null));
			GameTexts.SetVariable("STR2", sigilItem.Cost);
			this.ContinueText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			this.CanObtain = this.SigilItem.Cost <= NetworkMain.GameClient.PlayerData.Gold;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000B474 File Offset: 0x00009674
		private void OnOpened()
		{
			this.Item = null;
			this.SigilItem = null;
			this.IsOpenedWithSigilItem = false;
			this.IsOpenedWithClothingItem = false;
			this.IsOpenedWithTauntItem = false;
			this.IsObtainSuccessful = false;
			this.ObtainState = 0;
			this.IsEnabled = true;
			this._currentLootTextObject.SetTextVariable("LOOT", NetworkMain.GameClient.PlayerData.Gold);
			this.CurrentLootText = this._currentLootTextObject.ToString();
			Func<string> getExitText = this._getExitText;
			this.ClickToCloseText = ((getExitText != null) ? getExitText() : null);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000B504 File Offset: 0x00009704
		internal async void ExecuteAction()
		{
			if (this.ObtainState == 2 || this.ObtainState == 3)
			{
				this.ExecuteClosePopup();
			}
			else if (this.ObtainState == 0)
			{
				this.ObtainState = 1;
				ValueTuple<bool, int> valueTuple = await NetworkMain.GameClient.BuyCosmetic(this._activeCosmeticID);
				bool item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				this.ContinueText = GameTexts.FindText("str_continue", null).ToString();
				if (item)
				{
					if (this.Item != null)
					{
						this.Item.IsUnlocked = true;
					}
					else if (this.SigilItem != null)
					{
						this.SigilItem.IsUnlocked = true;
					}
					NetworkMain.GameClient.PlayerData.Gold = item2;
					this.ObtainResultText = new TextObject("{=V0k0urbO}Item obtained", null).ToString();
					string text = (this.IsOpenedWithSigilItem ? this.SigilItem.CosmeticID : (this.IsOpenedWithClothingItem ? this.Item.CosmeticID : string.Empty));
					this._onItemObtained(text, item2);
					this.IsObtainSuccessful = true;
					this.ObtainState = 2;
					SoundEvent.PlaySound2D("event:/ui/multiplayer/shop_purchase_complete");
				}
				else
				{
					this.ObtainResultText = new TextObject("{=XtVZe9cC}Item can not be obtained", null).ToString();
					this.IsObtainSuccessful = false;
					this.ObtainState = 3;
				}
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000B53D File Offset: 0x0000973D
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000B548 File Offset: 0x00009748
		private void OnCultureSelection(MPCultureItemVM cultureItem)
		{
			ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(this._cultureShieldItemIDs[cultureItem.Culture]);
			Banner banner = Banner.CreateOneColoredBannerWithOneIcon(cultureItem.Culture.BackgroundColor1, cultureItem.Culture.ForegroundColor1, this.SigilItem.IconID);
			this.ItemVisual.FillFrom(new EquipmentElement(@object, null, null, false), banner);
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000B5AD File Offset: 0x000097AD
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0000B5B5 File Offset: 0x000097B5
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

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000B5D3 File Offset: 0x000097D3
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0000B5DB File Offset: 0x000097DB
		[DataSourceProperty]
		public bool CanObtain
		{
			get
			{
				return this._canObtain;
			}
			set
			{
				if (value != this._canObtain)
				{
					this._canObtain = value;
					base.OnPropertyChangedWithValue(value, "CanObtain");
				}
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000B5F9 File Offset: 0x000097F9
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x0000B601 File Offset: 0x00009801
		[DataSourceProperty]
		public bool IsOpenedWithClothingItem
		{
			get
			{
				return this._isOpenedWithClothingItem;
			}
			set
			{
				if (value != this._isOpenedWithClothingItem)
				{
					this._isOpenedWithClothingItem = value;
					base.OnPropertyChangedWithValue(value, "IsOpenedWithClothingItem");
				}
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000B61F File Offset: 0x0000981F
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0000B627 File Offset: 0x00009827
		[DataSourceProperty]
		public bool IsOpenedWithSigilItem
		{
			get
			{
				return this._isOpenedWithSigilItem;
			}
			set
			{
				if (value != this._isOpenedWithSigilItem)
				{
					this._isOpenedWithSigilItem = value;
					base.OnPropertyChangedWithValue(value, "IsOpenedWithSigilItem");
				}
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000B645 File Offset: 0x00009845
		// (set) Token: 0x060002BA RID: 698 RVA: 0x0000B64D File Offset: 0x0000984D
		[DataSourceProperty]
		public bool IsOpenedWithTauntItem
		{
			get
			{
				return this._isOpenedWithTauntItem;
			}
			set
			{
				if (value != this._isOpenedWithTauntItem)
				{
					this._isOpenedWithTauntItem = value;
					base.OnPropertyChangedWithValue(value, "IsOpenedWithTauntItem");
				}
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000B66B File Offset: 0x0000986B
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0000B673 File Offset: 0x00009873
		[DataSourceProperty]
		public bool IsObtainSuccessful
		{
			get
			{
				return this._isObtainSuccessful;
			}
			set
			{
				if (value != this._isObtainSuccessful)
				{
					this._isObtainSuccessful = value;
					base.OnPropertyChangedWithValue(value, "IsObtainSuccessful");
				}
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000B691 File Offset: 0x00009891
		// (set) Token: 0x060002BE RID: 702 RVA: 0x0000B699 File Offset: 0x00009899
		[DataSourceProperty]
		public int ObtainState
		{
			get
			{
				return this._obtainState;
			}
			set
			{
				if (value != this._obtainState)
				{
					this._obtainState = value;
					base.OnPropertyChangedWithValue(value, "ObtainState");
				}
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000B6B7 File Offset: 0x000098B7
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x0000B6BF File Offset: 0x000098BF
		[DataSourceProperty]
		public string ObtainDescriptionText
		{
			get
			{
				return this._obtainDescriptionText;
			}
			set
			{
				if (value != this._obtainDescriptionText)
				{
					this._obtainDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ObtainDescriptionText");
				}
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000B6E2 File Offset: 0x000098E2
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x0000B6EA File Offset: 0x000098EA
		[DataSourceProperty]
		public string AnimationVariationText
		{
			get
			{
				return this._animationVariationText;
			}
			set
			{
				if (value != this._animationVariationText)
				{
					this._animationVariationText = value;
					base.OnPropertyChangedWithValue<string>(value, "AnimationVariationText");
				}
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000B70D File Offset: 0x0000990D
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x0000B715 File Offset: 0x00009915
		[DataSourceProperty]
		public string ContinueText
		{
			get
			{
				return this._continueText;
			}
			set
			{
				if (value != this._continueText)
				{
					this._continueText = value;
					base.OnPropertyChangedWithValue<string>(value, "ContinueText");
				}
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000B738 File Offset: 0x00009938
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x0000B740 File Offset: 0x00009940
		[DataSourceProperty]
		public string NotEnoughLootText
		{
			get
			{
				return this._notEnoughLootText;
			}
			set
			{
				if (value != this._notEnoughLootText)
				{
					this._notEnoughLootText = value;
					base.OnPropertyChangedWithValue<string>(value, "NotEnoughLootText");
				}
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000B763 File Offset: 0x00009963
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x0000B76B File Offset: 0x0000996B
		[DataSourceProperty]
		public string ObtainResultText
		{
			get
			{
				return this._obtainResultText;
			}
			set
			{
				if (value != this._obtainResultText)
				{
					this._obtainResultText = value;
					base.OnPropertyChangedWithValue<string>(value, "ObtainResultText");
				}
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000B78E File Offset: 0x0000998E
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000B796 File Offset: 0x00009996
		[DataSourceProperty]
		public string PreviewAsText
		{
			get
			{
				return this._previewAsText;
			}
			set
			{
				if (value != this._previewAsText)
				{
					this._previewAsText = value;
					base.OnPropertyChangedWithValue<string>(value, "PreviewAsText");
				}
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000B7B9 File Offset: 0x000099B9
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0000B7C1 File Offset: 0x000099C1
		[DataSourceProperty]
		public string CurrentLootText
		{
			get
			{
				return this._currentLootText;
			}
			set
			{
				if (value != this._currentLootText)
				{
					this._currentLootText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentLootText");
				}
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000B7E4 File Offset: 0x000099E4
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0000B7EC File Offset: 0x000099EC
		[DataSourceProperty]
		public string ClickToCloseText
		{
			get
			{
				return this._clickToCloseText;
			}
			set
			{
				if (value != this._clickToCloseText)
				{
					this._clickToCloseText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClickToCloseText");
				}
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000B80F File Offset: 0x00009A0F
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x0000B817 File Offset: 0x00009A17
		[DataSourceProperty]
		public CharacterViewModel CharacterVisual
		{
			get
			{
				return this._characterVisual;
			}
			set
			{
				if (value != this._characterVisual)
				{
					this._characterVisual = value;
					base.OnPropertyChangedWithValue<CharacterViewModel>(value, "CharacterVisual");
				}
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000B835 File Offset: 0x00009A35
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x0000B83D File Offset: 0x00009A3D
		[DataSourceProperty]
		public MPLobbyCosmeticSigilItemVM SigilItem
		{
			get
			{
				return this._sigilItem;
			}
			set
			{
				if (value != this._sigilItem)
				{
					this._sigilItem = value;
					base.OnPropertyChangedWithValue<MPLobbyCosmeticSigilItemVM>(value, "SigilItem");
				}
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000B85B File Offset: 0x00009A5B
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x0000B863 File Offset: 0x00009A63
		[DataSourceProperty]
		public MPArmoryCosmeticClothingItemVM Item
		{
			get
			{
				return this._item;
			}
			set
			{
				if (value != this._item)
				{
					this._item = value;
					base.OnPropertyChangedWithValue<MPArmoryCosmeticClothingItemVM>(value, "Item");
				}
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000B881 File Offset: 0x00009A81
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x0000B889 File Offset: 0x00009A89
		[DataSourceProperty]
		public MPArmoryCosmeticTauntItemVM TauntItem
		{
			get
			{
				return this._tauntItem;
			}
			set
			{
				if (value != this._tauntItem)
				{
					this._tauntItem = value;
					base.OnPropertyChangedWithValue<MPArmoryCosmeticTauntItemVM>(value, "TauntItem");
				}
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000B8A7 File Offset: 0x00009AA7
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x0000B8AF File Offset: 0x00009AAF
		[DataSourceProperty]
		public ItemCollectionElementViewModel ItemVisual
		{
			get
			{
				return this._itemVisual;
			}
			set
			{
				if (value != this._itemVisual)
				{
					this._itemVisual = value;
					base.OnPropertyChangedWithValue<ItemCollectionElementViewModel>(value, "ItemVisual");
				}
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000B8CD File Offset: 0x00009ACD
		// (set) Token: 0x060002DA RID: 730 RVA: 0x0000B8D5 File Offset: 0x00009AD5
		[DataSourceProperty]
		public MBBindingList<MPCultureItemVM> Cultures
		{
			get
			{
				return this._cultures;
			}
			set
			{
				if (value != this._cultures)
				{
					this._cultures = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPCultureItemVM>>(value, "Cultures");
				}
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000B8F3 File Offset: 0x00009AF3
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000B902 File Offset: 0x00009B02
		// (set) Token: 0x060002DD RID: 733 RVA: 0x0000B90A File Offset: 0x00009B0A
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
					base.OnPropertyChanged("DoneInputKey");
				}
			}
		}

		// Token: 0x0400015F RID: 351
		private readonly Action<string, int> _onItemObtained;

		// Token: 0x04000160 RID: 352
		private readonly Func<string> _getExitText;

		// Token: 0x04000161 RID: 353
		private int _currentTauntUsageIndex;

		// Token: 0x04000162 RID: 354
		private string _activeCosmeticID = string.Empty;

		// Token: 0x04000163 RID: 355
		private readonly Dictionary<BasicCultureObject, string> _cultureShieldItemIDs;

		// Token: 0x04000164 RID: 356
		private TextObject _currentLootTextObject = new TextObject("{=7vbGaapv}Your Loot: {LOOT}", null);

		// Token: 0x04000165 RID: 357
		private const string _purchaseCompleteSound = "event:/ui/multiplayer/shop_purchase_complete";

		// Token: 0x04000166 RID: 358
		private List<EquipmentElement> _characterEquipments;

		// Token: 0x04000167 RID: 359
		private bool _isEnabled;

		// Token: 0x04000168 RID: 360
		private bool _canObtain;

		// Token: 0x04000169 RID: 361
		private bool _isOpenedWithClothingItem;

		// Token: 0x0400016A RID: 362
		private bool _isOpenedWithSigilItem;

		// Token: 0x0400016B RID: 363
		private bool _isOpenedWithTauntItem;

		// Token: 0x0400016C RID: 364
		private bool _isObtainSuccessful;

		// Token: 0x0400016D RID: 365
		private int _obtainState;

		// Token: 0x0400016E RID: 366
		private string _animationVariationText;

		// Token: 0x0400016F RID: 367
		private string _obtainDescriptionText;

		// Token: 0x04000170 RID: 368
		private string _continueText;

		// Token: 0x04000171 RID: 369
		private string _notEnoughLootText;

		// Token: 0x04000172 RID: 370
		private string _obtainResultText;

		// Token: 0x04000173 RID: 371
		private string _previewAsText;

		// Token: 0x04000174 RID: 372
		private string _currentLootText;

		// Token: 0x04000175 RID: 373
		private string _clickToCloseText;

		// Token: 0x04000176 RID: 374
		private CharacterViewModel _characterVisual;

		// Token: 0x04000177 RID: 375
		private MPLobbyCosmeticSigilItemVM _sigilItem;

		// Token: 0x04000178 RID: 376
		private MPArmoryCosmeticClothingItemVM _item;

		// Token: 0x04000179 RID: 377
		private MPArmoryCosmeticTauntItemVM _tauntItem;

		// Token: 0x0400017A RID: 378
		private ItemCollectionElementViewModel _itemVisual;

		// Token: 0x0400017B RID: 379
		private MBBindingList<MPCultureItemVM> _cultures;

		// Token: 0x0400017C RID: 380
		private InputKeyItemVM _doneInputKey;

		// Token: 0x020000D5 RID: 213
		public enum CosmeticObtainState
		{
			// Token: 0x0400086A RID: 2154
			Initialized,
			// Token: 0x0400086B RID: 2155
			Ongoing,
			// Token: 0x0400086C RID: 2156
			FinishedSuccessfully,
			// Token: 0x0400086D RID: 2157
			FinishedUnsuccessfully
		}
	}
}
