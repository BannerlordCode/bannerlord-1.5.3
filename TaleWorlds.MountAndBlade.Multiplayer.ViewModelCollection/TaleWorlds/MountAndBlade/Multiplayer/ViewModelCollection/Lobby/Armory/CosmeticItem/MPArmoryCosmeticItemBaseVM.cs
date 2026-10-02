using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem
{
	// Token: 0x0200007F RID: 127
	public abstract class MPArmoryCosmeticItemBaseVM : ViewModel
	{
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000CAE RID: 3246 RVA: 0x00027730 File Offset: 0x00025930
		// (remove) Token: 0x06000CAF RID: 3247 RVA: 0x00027764 File Offset: 0x00025964
		public static event Action<MPArmoryCosmeticItemBaseVM> OnEquipped;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000CB0 RID: 3248 RVA: 0x00027798 File Offset: 0x00025998
		// (remove) Token: 0x06000CB1 RID: 3249 RVA: 0x000277CC File Offset: 0x000259CC
		public static event Action<MPArmoryCosmeticItemBaseVM> OnPurchaseRequested;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000CB2 RID: 3250 RVA: 0x00027800 File Offset: 0x00025A00
		// (remove) Token: 0x06000CB3 RID: 3251 RVA: 0x00027834 File Offset: 0x00025A34
		public static event Action<MPArmoryCosmeticItemBaseVM> OnPreviewed;

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x00027867 File Offset: 0x00025A67
		// (set) Token: 0x06000CB5 RID: 3253 RVA: 0x0002786F File Offset: 0x00025A6F
		public string UnequipText { get; private set; }

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00027878 File Offset: 0x00025A78
		public CosmeticsManager.CosmeticType CosmeticType { get; }

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00027880 File Offset: 0x00025A80
		public MPArmoryCosmeticItemBaseVM(CosmeticElement cosmetic, string cosmeticID, CosmeticsManager.CosmeticType cosmeticType)
		{
			this.Cosmetic = cosmetic;
			this.CosmeticID = cosmeticID;
			this.Cost = cosmetic.Cost;
			this.Rarity = (int)cosmetic.Rarity;
			this.CosmeticType = cosmeticType;
			this.IsUnequippable = true;
			this.RefreshValues();
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x000278CD File Offset: 0x00025ACD
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.OwnedText = new TextObject("{=B5bcj3pC}Owned", null).ToString();
			this.UnequipText = new TextObject("{=QndVFTbx}Unequip", null).ToString();
			this.UpdatePreviewAndActionTexts();
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x00027907 File Offset: 0x00025B07
		public override void OnFinalize()
		{
			InputKeyItemVM actionKey = this.ActionKey;
			if (actionKey != null)
			{
				actionKey.OnFinalize();
			}
			InputKeyItemVM previewKey = this.PreviewKey;
			if (previewKey == null)
			{
				return;
			}
			previewKey.OnFinalize();
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0002792A File Offset: 0x00025B2A
		public void ExecuteAction()
		{
			if (!this.IsUnlocked)
			{
				MPArmoryCosmeticItemBaseVM.OnPurchaseRequested(this);
				return;
			}
			Action<MPArmoryCosmeticItemBaseVM> onEquipped = MPArmoryCosmeticItemBaseVM.OnEquipped;
			if (onEquipped == null)
			{
				return;
			}
			onEquipped(this);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x00027950 File Offset: 0x00025B50
		public void ExecutePreview()
		{
			MPArmoryCosmeticItemBaseVM.OnPreviewed(this);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0002795D File Offset: 0x00025B5D
		public void ExecuteEnableActions()
		{
			this.AreActionsEnabled = true;
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00027966 File Offset: 0x00025B66
		public void ExecuteDisableActions()
		{
			this.AreActionsEnabled = false;
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00027970 File Offset: 0x00025B70
		protected void UpdatePreviewAndActionTexts()
		{
			if (this.IsUnlocked)
			{
				if (this.IsUsed)
				{
					this.ActionText = (this.IsUnequippable ? this.UnequipText : string.Empty);
				}
				else
				{
					this.ActionText = new TextObject("{=DKqLY1aJ}Equip", null).ToString();
				}
			}
			else
			{
				this.ActionText = new TextObject("{=i2mNBaxE}Obtain", null).ToString();
			}
			this.PreviewText = new TextObject("{=un7poy9x}Preview", null).ToString();
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x000279F0 File Offset: 0x00025BF0
		public void RefreshKeyBindings(HotKey actionKey, HotKey previewKey)
		{
			if (this.IsUnlocked && this.IsUsed && !this.IsUnequippable)
			{
				this.ActionKey = InputKeyItemVM.CreateFromHotKey(null, false);
			}
			else
			{
				string groupId = actionKey.GroupId;
				InputKeyItemVM actionKey2 = this.ActionKey;
				string text;
				if (actionKey2 == null)
				{
					text = null;
				}
				else
				{
					HotKey hotKey = actionKey2.HotKey;
					text = ((hotKey != null) ? hotKey.GroupId : null);
				}
				if (!(groupId != text))
				{
					string id = actionKey.Id;
					InputKeyItemVM actionKey3 = this.ActionKey;
					string text2;
					if (actionKey3 == null)
					{
						text2 = null;
					}
					else
					{
						HotKey hotKey2 = actionKey3.HotKey;
						text2 = ((hotKey2 != null) ? hotKey2.Id : null);
					}
					if (!(id != text2))
					{
						goto IL_008A;
					}
				}
				this.ActionKey = InputKeyItemVM.CreateFromHotKey(actionKey, false);
			}
			IL_008A:
			string groupId2 = previewKey.GroupId;
			InputKeyItemVM previewKey2 = this.PreviewKey;
			string text3;
			if (previewKey2 == null)
			{
				text3 = null;
			}
			else
			{
				HotKey hotKey3 = previewKey2.HotKey;
				text3 = ((hotKey3 != null) ? hotKey3.GroupId : null);
			}
			if (!(groupId2 != text3))
			{
				string id2 = previewKey.Id;
				InputKeyItemVM previewKey3 = this.PreviewKey;
				string text4;
				if (previewKey3 == null)
				{
					text4 = null;
				}
				else
				{
					HotKey hotKey4 = previewKey3.HotKey;
					text4 = ((hotKey4 != null) ? hotKey4.Id : null);
				}
				if (!(id2 != text4))
				{
					goto IL_00ED;
				}
			}
			this.PreviewKey = InputKeyItemVM.CreateFromHotKey(previewKey, false);
			IL_00ED:
			this.UpdatePreviewAndActionTexts();
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x00027AF0 File Offset: 0x00025CF0
		// (set) Token: 0x06000CC1 RID: 3265 RVA: 0x00027AF8 File Offset: 0x00025CF8
		[DataSourceProperty]
		public bool IsUnlocked
		{
			get
			{
				return this._isUnlocked;
			}
			set
			{
				if (value != this._isUnlocked)
				{
					this._isUnlocked = value;
					base.OnPropertyChangedWithValue(value, "IsUnlocked");
					this.UpdatePreviewAndActionTexts();
				}
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x00027B1C File Offset: 0x00025D1C
		// (set) Token: 0x06000CC3 RID: 3267 RVA: 0x00027B24 File Offset: 0x00025D24
		[DataSourceProperty]
		public bool IsUsed
		{
			get
			{
				return this._isUsed;
			}
			set
			{
				if (value != this._isUsed)
				{
					this._isUsed = value;
					base.OnPropertyChangedWithValue(value, "IsUsed");
					this.UpdatePreviewAndActionTexts();
				}
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x00027B48 File Offset: 0x00025D48
		// (set) Token: 0x06000CC5 RID: 3269 RVA: 0x00027B50 File Offset: 0x00025D50
		[DataSourceProperty]
		public bool AreActionsEnabled
		{
			get
			{
				return this._areActionsEnabled;
			}
			set
			{
				if (value != this._areActionsEnabled)
				{
					this._areActionsEnabled = value;
					base.OnPropertyChangedWithValue(value, "AreActionsEnabled");
				}
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x00027B6E File Offset: 0x00025D6E
		// (set) Token: 0x06000CC7 RID: 3271 RVA: 0x00027B76 File Offset: 0x00025D76
		[DataSourceProperty]
		public bool IsSelectable
		{
			get
			{
				return this._isSelectable;
			}
			set
			{
				if (value != this._isSelectable)
				{
					this._isSelectable = value;
					base.OnPropertyChangedWithValue(value, "IsSelectable");
				}
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x00027B94 File Offset: 0x00025D94
		// (set) Token: 0x06000CC9 RID: 3273 RVA: 0x00027B9C File Offset: 0x00025D9C
		[DataSourceProperty]
		public bool IsUnequippable
		{
			get
			{
				return this._isUnequippable;
			}
			set
			{
				if (value != this._isUnequippable)
				{
					this._isUnequippable = value;
					base.OnPropertyChangedWithValue(value, "IsUnequippable");
				}
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x00027BBA File Offset: 0x00025DBA
		// (set) Token: 0x06000CCB RID: 3275 RVA: 0x00027BC2 File Offset: 0x00025DC2
		[DataSourceProperty]
		public int Cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				if (value != this._cost)
				{
					this._cost = value;
					base.OnPropertyChangedWithValue(value, "Cost");
				}
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x00027BE0 File Offset: 0x00025DE0
		// (set) Token: 0x06000CCD RID: 3277 RVA: 0x00027BE8 File Offset: 0x00025DE8
		[DataSourceProperty]
		public int Rarity
		{
			get
			{
				return this._rarity;
			}
			set
			{
				if (value != this._rarity)
				{
					this._rarity = value;
					base.OnPropertyChangedWithValue(value, "Rarity");
				}
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x00027C06 File Offset: 0x00025E06
		// (set) Token: 0x06000CCF RID: 3279 RVA: 0x00027C0E File Offset: 0x00025E0E
		[DataSourceProperty]
		public int ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (value != this._itemType)
				{
					this._itemType = value;
					base.OnPropertyChangedWithValue(value, "ItemType");
				}
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x00027C2C File Offset: 0x00025E2C
		// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x00027C34 File Offset: 0x00025E34
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x00027C57 File Offset: 0x00025E57
		// (set) Token: 0x06000CD3 RID: 3283 RVA: 0x00027C5F File Offset: 0x00025E5F
		[DataSourceProperty]
		public string OwnedText
		{
			get
			{
				return this._ownedText;
			}
			set
			{
				if (value != this._ownedText)
				{
					this._ownedText = value;
					base.OnPropertyChangedWithValue<string>(value, "OwnedText");
				}
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x00027C82 File Offset: 0x00025E82
		// (set) Token: 0x06000CD5 RID: 3285 RVA: 0x00027C8A File Offset: 0x00025E8A
		[DataSourceProperty]
		public string ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (value != this._actionText)
				{
					this._actionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionText");
				}
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x00027CAD File Offset: 0x00025EAD
		// (set) Token: 0x06000CD7 RID: 3287 RVA: 0x00027CB5 File Offset: 0x00025EB5
		[DataSourceProperty]
		public string PreviewText
		{
			get
			{
				return this._previewText;
			}
			set
			{
				if (value != this._previewText)
				{
					this._previewText = value;
					base.OnPropertyChangedWithValue<string>(value, "PreviewText");
				}
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00027CD8 File Offset: 0x00025ED8
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x00027CE0 File Offset: 0x00025EE0
		[DataSourceProperty]
		public ItemImageIdentifierVM Icon
		{
			get
			{
				return this._icon;
			}
			set
			{
				if (value != this._icon)
				{
					this._icon = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "Icon");
				}
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00027CFE File Offset: 0x00025EFE
		// (set) Token: 0x06000CDB RID: 3291 RVA: 0x00027D06 File Offset: 0x00025F06
		[DataSourceProperty]
		public InputKeyItemVM ActionKey
		{
			get
			{
				return this._actionKey;
			}
			set
			{
				if (value != this._actionKey)
				{
					this._actionKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ActionKey");
				}
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x00027D24 File Offset: 0x00025F24
		// (set) Token: 0x06000CDD RID: 3293 RVA: 0x00027D2C File Offset: 0x00025F2C
		[DataSourceProperty]
		public InputKeyItemVM PreviewKey
		{
			get
			{
				return this._previewKey;
			}
			set
			{
				if (value != this._previewKey)
				{
					this._previewKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PreviewKey");
				}
			}
		}

		// Token: 0x040005BF RID: 1471
		public readonly CosmeticElement Cosmetic;

		// Token: 0x040005C0 RID: 1472
		public readonly string CosmeticID;

		// Token: 0x040005C6 RID: 1478
		private bool _isUnlocked;

		// Token: 0x040005C7 RID: 1479
		private bool _isUsed;

		// Token: 0x040005C8 RID: 1480
		private bool _areActionsEnabled;

		// Token: 0x040005C9 RID: 1481
		private bool _isSelectable;

		// Token: 0x040005CA RID: 1482
		private bool _isUnequippable;

		// Token: 0x040005CB RID: 1483
		private int _cost;

		// Token: 0x040005CC RID: 1484
		private int _rarity;

		// Token: 0x040005CD RID: 1485
		private int _itemType;

		// Token: 0x040005CE RID: 1486
		private string _name;

		// Token: 0x040005CF RID: 1487
		private string _ownedText;

		// Token: 0x040005D0 RID: 1488
		private string _actionText;

		// Token: 0x040005D1 RID: 1489
		private string _previewText;

		// Token: 0x040005D2 RID: 1490
		private ItemImageIdentifierVM _icon;

		// Token: 0x040005D3 RID: 1491
		private InputKeyItemVM _actionKey;

		// Token: 0x040005D4 RID: 1492
		private InputKeyItemVM _previewKey;
	}
}
