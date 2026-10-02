using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x02000031 RID: 49
	public class MPLobbySigilItemVM : ViewModel
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000D093 File Offset: 0x0000B293
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x0000D09B File Offset: 0x0000B29B
		public int IconID { get; private set; }

		// Token: 0x060003A5 RID: 933 RVA: 0x0000D0A4 File Offset: 0x0000B2A4
		public MPLobbySigilItemVM()
		{
			this.RefreshWith(0);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000D0B3 File Offset: 0x0000B2B3
		public MPLobbySigilItemVM(int iconID, Action<MPLobbySigilItemVM> onSelection)
		{
			this.RefreshWith(iconID);
			this._onSelection = onSelection;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000D0C9 File Offset: 0x0000B2C9
		public void RefreshWith(int iconID)
		{
			this.IconPath = iconID.ToString();
			this.IconID = iconID;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000D0DF File Offset: 0x0000B2DF
		public void RefreshWith(Banner banner)
		{
			this.RefreshWith(banner.GetIconMeshId());
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000D0ED File Offset: 0x0000B2ED
		public void RefreshWith(string bannerCode)
		{
			this.RefreshWith(new Banner(bannerCode));
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000D0FB File Offset: 0x0000B2FB
		private void ExecuteSelectIcon()
		{
			Action<MPLobbySigilItemVM> onSelection = this._onSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this);
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003AB RID: 939 RVA: 0x0000D10E File Offset: 0x0000B30E
		// (set) Token: 0x060003AC RID: 940 RVA: 0x0000D116 File Offset: 0x0000B316
		[DataSourceProperty]
		public string IconPath
		{
			get
			{
				return this._iconPath;
			}
			set
			{
				if (value != this._iconPath)
				{
					this._iconPath = value;
					base.OnPropertyChanged("IconPath");
				}
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003AD RID: 941 RVA: 0x0000D138 File Offset: 0x0000B338
		// (set) Token: 0x060003AE RID: 942 RVA: 0x0000D140 File Offset: 0x0000B340
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
					base.OnPropertyChanged("IsSelected");
				}
			}
		}

		// Token: 0x040001D6 RID: 470
		private readonly Action<MPLobbySigilItemVM> _onSelection;

		// Token: 0x040001D7 RID: 471
		private string _iconPath;

		// Token: 0x040001D8 RID: 472
		private bool _isSelected;
	}
}
