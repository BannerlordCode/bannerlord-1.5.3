using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x0200015B RID: 347
	public class CharacterCreationReviewStageItemVM : ViewModel
	{
		// Token: 0x06002147 RID: 8519 RVA: 0x000776D2 File Offset: 0x000758D2
		public CharacterCreationReviewStageItemVM(BannerImageIdentifierVM imageIdentifier, string title, string text, string description)
			: this(title, text, description)
		{
			this.HasImage = true;
			this.ImageIdentifier = imageIdentifier;
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x000776EC File Offset: 0x000758EC
		public CharacterCreationReviewStageItemVM(string title, string text, string description)
		{
			this.Title = title;
			this.Text = text;
			this.Description = description;
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06002149 RID: 8521 RVA: 0x00077709 File Offset: 0x00075909
		// (set) Token: 0x0600214A RID: 8522 RVA: 0x00077711 File Offset: 0x00075911
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

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x0600214B RID: 8523 RVA: 0x0007772F File Offset: 0x0007592F
		// (set) Token: 0x0600214C RID: 8524 RVA: 0x00077737 File Offset: 0x00075937
		[DataSourceProperty]
		public BannerImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x0600214D RID: 8525 RVA: 0x00077755 File Offset: 0x00075955
		// (set) Token: 0x0600214E RID: 8526 RVA: 0x0007775D File Offset: 0x0007595D
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

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x0600214F RID: 8527 RVA: 0x00077780 File Offset: 0x00075980
		// (set) Token: 0x06002150 RID: 8528 RVA: 0x00077788 File Offset: 0x00075988
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06002151 RID: 8529 RVA: 0x000777AB File Offset: 0x000759AB
		// (set) Token: 0x06002152 RID: 8530 RVA: 0x000777B3 File Offset: 0x000759B3
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x04000F37 RID: 3895
		private bool _hasImage;

		// Token: 0x04000F38 RID: 3896
		private BannerImageIdentifierVM _imageIdentifier;

		// Token: 0x04000F39 RID: 3897
		private string _title;

		// Token: 0x04000F3A RID: 3898
		private string _text;

		// Token: 0x04000F3B RID: 3899
		private string _description;
	}
}
