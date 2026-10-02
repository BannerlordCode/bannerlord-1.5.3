using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FactionBanVote
{
	// Token: 0x020000A3 RID: 163
	public class MultiplayerFactionBanVoteVM : ViewModel
	{
		// Token: 0x06000FD6 RID: 4054 RVA: 0x00031747 File Offset: 0x0002F947
		public MultiplayerFactionBanVoteVM(BasicCultureObject culture, Action<MultiplayerFactionBanVoteVM> onSelect)
		{
			this.Culture = culture;
			this._onSelect = onSelect;
			this._isEnabled = true;
			this._name = culture.Name.ToString();
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x00031775 File Offset: 0x0002F975
		// (set) Token: 0x06000FD8 RID: 4056 RVA: 0x0003177D File Offset: 0x0002F97D
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
					if (value)
					{
						this._onSelect(this);
					}
				}
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x000317AA File Offset: 0x0002F9AA
		// (set) Token: 0x06000FDA RID: 4058 RVA: 0x000317B2 File Offset: 0x0002F9B2
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

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x000317D0 File Offset: 0x0002F9D0
		// (set) Token: 0x06000FDC RID: 4060 RVA: 0x000317D8 File Offset: 0x0002F9D8
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

		// Token: 0x0400075F RID: 1887
		private readonly Action<MultiplayerFactionBanVoteVM> _onSelect;

		// Token: 0x04000760 RID: 1888
		public readonly BasicCultureObject Culture;

		// Token: 0x04000761 RID: 1889
		private string _name;

		// Token: 0x04000762 RID: 1890
		private bool _isEnabled;

		// Token: 0x04000763 RID: 1891
		private bool _isSelected;
	}
}
