using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection
{
	// Token: 0x0200000A RID: 10
	public class TournamentRewardVM : ViewModel
	{
		// Token: 0x0600006D RID: 109 RVA: 0x00005287 File Offset: 0x00003487
		public TournamentRewardVM(string text)
		{
			this.Text = text;
			this.GotImageIdentifier = false;
			this.ImageIdentifier = new ItemImageIdentifierVM(null, "");
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000052AE File Offset: 0x000034AE
		public TournamentRewardVM(string text, ItemImageIdentifierVM imageIdentifierVM)
		{
			this.Text = text;
			this.GotImageIdentifier = true;
			this.ImageIdentifier = imageIdentifierVM;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000052CB File Offset: 0x000034CB
		// (set) Token: 0x06000070 RID: 112 RVA: 0x000052D3 File Offset: 0x000034D3
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

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000052F6 File Offset: 0x000034F6
		// (set) Token: 0x06000072 RID: 114 RVA: 0x000052FE File Offset: 0x000034FE
		[DataSourceProperty]
		public bool GotImageIdentifier
		{
			get
			{
				return this._gotImageIdentifier;
			}
			set
			{
				if (value != this._gotImageIdentifier)
				{
					this._gotImageIdentifier = value;
					base.OnPropertyChangedWithValue(value, "GotImageIdentifier");
				}
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000073 RID: 115 RVA: 0x0000531C File Offset: 0x0000351C
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00005324 File Offset: 0x00003524
		[DataSourceProperty]
		public ItemImageIdentifierVM ImageIdentifier
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
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x04000031 RID: 49
		private string _text;

		// Token: 0x04000032 RID: 50
		private ItemImageIdentifierVM _imageIdentifier;

		// Token: 0x04000033 RID: 51
		private bool _gotImageIdentifier;
	}
}
