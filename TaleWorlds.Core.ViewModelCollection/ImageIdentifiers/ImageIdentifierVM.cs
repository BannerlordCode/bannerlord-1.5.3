using System;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.ImageIdentifiers
{
	// Token: 0x02000021 RID: 33
	public abstract class ImageIdentifierVM : ViewModel
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x000059CD File Offset: 0x00003BCD
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x000059D8 File Offset: 0x00003BD8
		protected ImageIdentifier ImageIdentifier
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
					ImageIdentifier imageIdentifier = this._imageIdentifier;
					this.Id = ((imageIdentifier != null) ? imageIdentifier.Id : null) ?? string.Empty;
					ImageIdentifier imageIdentifier2 = this._imageIdentifier;
					this.AdditionalArgs = ((imageIdentifier2 != null) ? imageIdentifier2.AdditionalArgs : null) ?? string.Empty;
					ImageIdentifier imageIdentifier3 = this._imageIdentifier;
					this.TextureProviderName = ((imageIdentifier3 != null) ? imageIdentifier3.TextureProviderName : null) ?? string.Empty;
				}
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00005A58 File Offset: 0x00003C58
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.ImageIdentifier = null;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00005A67 File Offset: 0x00003C67
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00005A6F File Offset: 0x00003C6F
		[DataSourceProperty]
		public string Id
		{
			get
			{
				return this._id;
			}
			set
			{
				if (this._id != value)
				{
					this._id = value;
					base.OnPropertyChangedWithValue<string>(value, "Id");
				}
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00005A92 File Offset: 0x00003C92
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00005A9A File Offset: 0x00003C9A
		[DataSourceProperty]
		public string AdditionalArgs
		{
			get
			{
				return this._additionalArgs;
			}
			set
			{
				if (value != this._additionalArgs)
				{
					this._additionalArgs = value;
					base.OnPropertyChangedWithValue<string>(value, "AdditionalArgs");
				}
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00005ABD File Offset: 0x00003CBD
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00005AC5 File Offset: 0x00003CC5
		[DataSourceProperty]
		public string TextureProviderName
		{
			get
			{
				return this._textureProviderName;
			}
			set
			{
				if (value != this._textureProviderName)
				{
					this._textureProviderName = value;
					base.OnPropertyChangedWithValue<string>(value, "TextureProviderName");
				}
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00005AE8 File Offset: 0x00003CE8
		[DataSourceProperty]
		public bool IsEmpty
		{
			get
			{
				return !string.IsNullOrEmpty(this.TextureProviderName) && string.IsNullOrEmpty(this.ImageIdentifier.Id);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00005B09 File Offset: 0x00003D09
		[DataSourceProperty]
		public bool IsValid
		{
			get
			{
				return !this.IsEmpty;
			}
		}

		// Token: 0x040000A7 RID: 167
		private ImageIdentifier _imageIdentifier;

		// Token: 0x040000A8 RID: 168
		private string _id;

		// Token: 0x040000A9 RID: 169
		private string _additionalArgs;

		// Token: 0x040000AA RID: 170
		private string _textureProviderName;
	}
}
