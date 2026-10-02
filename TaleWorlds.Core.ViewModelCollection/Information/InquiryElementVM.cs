using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core.ViewModelCollection.Information
{
	// Token: 0x02000017 RID: 23
	public class InquiryElementVM : ViewModel
	{
		// Token: 0x06000120 RID: 288 RVA: 0x00004594 File Offset: 0x00002794
		public InquiryElementVM(InquiryElement elementData, TextObject hint, Action<InquiryElementVM, bool> onSelectedStateChanged = null)
		{
			this.Text = elementData.Title;
			this.ImageIdentifier = new GenericImageIdentifierVM(elementData.ImageIdentifier);
			this.InquiryElement = elementData;
			this.IsEnabled = elementData.IsEnabled;
			this.HasVisuals = elementData.ImageIdentifier != null;
			this.Hint = new HintViewModel(hint, null);
			this._onSelectedStateChanged = onSelectedStateChanged;
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000121 RID: 289 RVA: 0x000045FA File Offset: 0x000027FA
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00004602 File Offset: 0x00002802
		[DataSourceProperty]
		public bool IsFilteredOut
		{
			get
			{
				return this._isFilteredOut;
			}
			set
			{
				if (this._isFilteredOut != value)
				{
					this._isFilteredOut = value;
					base.OnPropertyChangedWithValue(value, "IsFilteredOut");
				}
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00004620 File Offset: 0x00002820
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00004628 File Offset: 0x00002828
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (this._isSelected != value)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
					Action<InquiryElementVM, bool> onSelectedStateChanged = this._onSelectedStateChanged;
					if (onSelectedStateChanged == null)
					{
						return;
					}
					onSelectedStateChanged(this, value);
				}
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00004658 File Offset: 0x00002858
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00004660 File Offset: 0x00002860
		[DataSourceProperty]
		public bool HasVisuals
		{
			get
			{
				return this._hasVisuals;
			}
			set
			{
				if (this._hasVisuals != value)
				{
					this._hasVisuals = value;
					base.OnPropertyChangedWithValue(value, "HasVisuals");
				}
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000127 RID: 295 RVA: 0x0000467E File Offset: 0x0000287E
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00004686 File Offset: 0x00002886
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (this._isEnabled != value)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000129 RID: 297 RVA: 0x000046A4 File Offset: 0x000028A4
		// (set) Token: 0x0600012A RID: 298 RVA: 0x000046AC File Offset: 0x000028AC
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (this._text != value)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600012B RID: 299 RVA: 0x000046CF File Offset: 0x000028CF
		// (set) Token: 0x0600012C RID: 300 RVA: 0x000046D7 File Offset: 0x000028D7
		[DataSourceProperty]
		public ImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (this._imageIdentifier != value)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<ImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600012D RID: 301 RVA: 0x000046F5 File Offset: 0x000028F5
		// (set) Token: 0x0600012E RID: 302 RVA: 0x000046FD File Offset: 0x000028FD
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (this._hint != value)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x0400007A RID: 122
		public readonly InquiryElement InquiryElement;

		// Token: 0x0400007B RID: 123
		private readonly Action<InquiryElementVM, bool> _onSelectedStateChanged;

		// Token: 0x0400007C RID: 124
		private bool _isFilteredOut;

		// Token: 0x0400007D RID: 125
		private bool _isSelected;

		// Token: 0x0400007E RID: 126
		private bool _isEnabled;

		// Token: 0x0400007F RID: 127
		private string _text;

		// Token: 0x04000080 RID: 128
		private bool _hasVisuals;

		// Token: 0x04000081 RID: 129
		private ImageIdentifierVM _imageIdentifier;

		// Token: 0x04000082 RID: 130
		private HintViewModel _hint;
	}
}
