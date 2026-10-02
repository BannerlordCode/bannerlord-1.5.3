using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000125 RID: 293
	public class ClanCardSelectionPopupItemVM : ViewModel
	{
		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06001A6E RID: 6766 RVA: 0x00064132 File Offset: 0x00062332
		public object Identifier { get; }

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06001A6F RID: 6767 RVA: 0x0006413A File Offset: 0x0006233A
		public TextObject ActionResultText { get; }

		// Token: 0x06001A70 RID: 6768 RVA: 0x00064144 File Offset: 0x00062344
		public ClanCardSelectionPopupItemVM(in ClanCardSelectionItemInfo info, Action<ClanCardSelectionPopupItemVM> onSelected)
		{
			this.Identifier = info.Identifier;
			this._onSelected = onSelected;
			this.ActionResultText = info.ActionResult;
			this._titleText = info.Title;
			this._disabledReasonText = info.DisabledReason;
			this._specialActionText = info.SpecialActionText;
			this.DisabledHint = new HintViewModel();
			this.Properties = new MBBindingList<ClanCardSelectionPopupItemPropertyVM>();
			if (info.Properties != null)
			{
				foreach (ClanCardSelectionItemPropertyInfo clanCardSelectionItemPropertyInfo in info.Properties)
				{
					this.Properties.Add(new ClanCardSelectionPopupItemPropertyVM(in clanCardSelectionItemPropertyInfo));
				}
			}
			this.IsDisabled = info.IsDisabled;
			this.IsSpecialActionItem = info.IsSpecialActionItem;
			if (info.IsInitiallySelected)
			{
				this.ExecuteSelect();
			}
			this.HasSprite = !string.IsNullOrEmpty(info.SpriteName);
			this.HasImage = info.Image != null;
			this.SpriteType = info.SpriteType.ToString();
			this.SpriteName = info.SpriteName ?? string.Empty;
			this.SpriteLabel = info.SpriteLabel ?? string.Empty;
			this.Image = new GenericImageIdentifierVM(info.Image);
			this.RefreshValues();
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x000642A4 File Offset: 0x000624A4
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject titleText = this._titleText;
			this.Title = ((titleText != null) ? titleText.ToString() : null) ?? string.Empty;
			TextObject specialActionText = this._specialActionText;
			this.SpecialAction = ((specialActionText != null) ? specialActionText.ToString() : null) ?? string.Empty;
			this.DisabledHint.HintText = (this.IsDisabled ? this._disabledReasonText : TextObject.GetEmpty());
			this.Properties.ApplyActionOnAllItems(delegate(ClanCardSelectionPopupItemPropertyVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x00064343 File Offset: 0x00062543
		public void ExecuteSelect()
		{
			Action<ClanCardSelectionPopupItemVM> onSelected = this._onSelected;
			if (onSelected == null)
			{
				return;
			}
			onSelected(this);
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06001A73 RID: 6771 RVA: 0x00064356 File Offset: 0x00062556
		// (set) Token: 0x06001A74 RID: 6772 RVA: 0x0006435E File Offset: 0x0006255E
		[DataSourceProperty]
		public ImageIdentifierVM Image
		{
			get
			{
				return this._image;
			}
			set
			{
				if (value != this._image)
				{
					this._image = value;
					base.OnPropertyChangedWithValue<ImageIdentifierVM>(value, "Image");
				}
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06001A75 RID: 6773 RVA: 0x0006437C File Offset: 0x0006257C
		// (set) Token: 0x06001A76 RID: 6774 RVA: 0x00064384 File Offset: 0x00062584
		[DataSourceProperty]
		public MBBindingList<ClanCardSelectionPopupItemPropertyVM> Properties
		{
			get
			{
				return this._properties;
			}
			set
			{
				if (value != this._properties)
				{
					this._properties = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanCardSelectionPopupItemPropertyVM>>(value, "Properties");
				}
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06001A77 RID: 6775 RVA: 0x000643A2 File Offset: 0x000625A2
		// (set) Token: 0x06001A78 RID: 6776 RVA: 0x000643AA File Offset: 0x000625AA
		[DataSourceProperty]
		public HintViewModel DisabledHint
		{
			get
			{
				return this._disabledHint;
			}
			set
			{
				if (value != this._disabledHint)
				{
					this._disabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DisabledHint");
				}
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06001A79 RID: 6777 RVA: 0x000643C8 File Offset: 0x000625C8
		// (set) Token: 0x06001A7A RID: 6778 RVA: 0x000643D0 File Offset: 0x000625D0
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

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06001A7B RID: 6779 RVA: 0x000643F3 File Offset: 0x000625F3
		// (set) Token: 0x06001A7C RID: 6780 RVA: 0x000643FB File Offset: 0x000625FB
		[DataSourceProperty]
		public string SpriteType
		{
			get
			{
				return this._spriteType;
			}
			set
			{
				if (value != this._spriteType)
				{
					this._spriteType = value;
					base.OnPropertyChangedWithValue<string>(value, "SpriteType");
				}
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06001A7D RID: 6781 RVA: 0x0006441E File Offset: 0x0006261E
		// (set) Token: 0x06001A7E RID: 6782 RVA: 0x00064426 File Offset: 0x00062626
		[DataSourceProperty]
		public string SpriteName
		{
			get
			{
				return this._spriteName;
			}
			set
			{
				if (value != this._spriteName)
				{
					this._spriteName = value;
					base.OnPropertyChangedWithValue<string>(value, "SpriteName");
				}
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06001A7F RID: 6783 RVA: 0x00064449 File Offset: 0x00062649
		// (set) Token: 0x06001A80 RID: 6784 RVA: 0x00064451 File Offset: 0x00062651
		[DataSourceProperty]
		public string SpriteLabel
		{
			get
			{
				return this._spriteLabel;
			}
			set
			{
				if (value != this._spriteLabel)
				{
					this._spriteLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "SpriteLabel");
				}
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06001A81 RID: 6785 RVA: 0x00064474 File Offset: 0x00062674
		// (set) Token: 0x06001A82 RID: 6786 RVA: 0x0006447C File Offset: 0x0006267C
		[DataSourceProperty]
		public string SpecialAction
		{
			get
			{
				return this._specialAction;
			}
			set
			{
				if (value != this._specialAction)
				{
					this._specialAction = value;
					base.OnPropertyChangedWithValue<string>(value, "SpecialAction");
				}
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06001A83 RID: 6787 RVA: 0x0006449F File Offset: 0x0006269F
		// (set) Token: 0x06001A84 RID: 6788 RVA: 0x000644A7 File Offset: 0x000626A7
		[DataSourceProperty]
		public bool HasImage
		{
			get
			{
				return this._hasImage;
			}
			set
			{
				if (value != this._hasImage)
				{
					this._hasImage = value;
					base.OnPropertyChangedWithValue(value, "HasImage");
				}
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06001A85 RID: 6789 RVA: 0x000644C5 File Offset: 0x000626C5
		// (set) Token: 0x06001A86 RID: 6790 RVA: 0x000644CD File Offset: 0x000626CD
		[DataSourceProperty]
		public bool HasSprite
		{
			get
			{
				return this._hasSprite;
			}
			set
			{
				if (value != this._hasSprite)
				{
					this._hasSprite = value;
					base.OnPropertyChangedWithValue(value, "HasSprite");
				}
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06001A87 RID: 6791 RVA: 0x000644EB File Offset: 0x000626EB
		// (set) Token: 0x06001A88 RID: 6792 RVA: 0x000644F3 File Offset: 0x000626F3
		[DataSourceProperty]
		public bool IsSpecialActionItem
		{
			get
			{
				return this._isSpecialActionItem;
			}
			set
			{
				if (value != this._isSpecialActionItem)
				{
					this._isSpecialActionItem = value;
					base.OnPropertyChangedWithValue(value, "IsSpecialActionItem");
				}
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06001A89 RID: 6793 RVA: 0x00064511 File Offset: 0x00062711
		// (set) Token: 0x06001A8A RID: 6794 RVA: 0x00064519 File Offset: 0x00062719
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06001A8B RID: 6795 RVA: 0x00064537 File Offset: 0x00062737
		// (set) Token: 0x06001A8C RID: 6796 RVA: 0x0006453F File Offset: 0x0006273F
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

		// Token: 0x04000C2E RID: 3118
		private readonly TextObject _titleText;

		// Token: 0x04000C2F RID: 3119
		private readonly TextObject _disabledReasonText;

		// Token: 0x04000C30 RID: 3120
		private readonly TextObject _specialActionText;

		// Token: 0x04000C31 RID: 3121
		private readonly Action<ClanCardSelectionPopupItemVM> _onSelected;

		// Token: 0x04000C32 RID: 3122
		private ImageIdentifierVM _image;

		// Token: 0x04000C33 RID: 3123
		private MBBindingList<ClanCardSelectionPopupItemPropertyVM> _properties;

		// Token: 0x04000C34 RID: 3124
		private HintViewModel _disabledHint;

		// Token: 0x04000C35 RID: 3125
		private string _title;

		// Token: 0x04000C36 RID: 3126
		private string _spriteType;

		// Token: 0x04000C37 RID: 3127
		private string _spriteName;

		// Token: 0x04000C38 RID: 3128
		private string _spriteLabel;

		// Token: 0x04000C39 RID: 3129
		private string _specialAction;

		// Token: 0x04000C3A RID: 3130
		private bool _hasImage;

		// Token: 0x04000C3B RID: 3131
		private bool _hasSprite;

		// Token: 0x04000C3C RID: 3132
		private bool _isSpecialActionItem;

		// Token: 0x04000C3D RID: 3133
		private bool _isDisabled;

		// Token: 0x04000C3E RID: 3134
		private bool _isSelected;
	}
}
