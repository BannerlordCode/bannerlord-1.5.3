using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Intermission
{
	// Token: 0x0200008F RID: 143
	public class MPIntermissionCultureItemVM : MPCultureItemVM
	{
		// Token: 0x06000DCD RID: 3533 RVA: 0x0002A72B File Offset: 0x0002892B
		public MPIntermissionCultureItemVM(string cultureCode, Action<MPIntermissionCultureItemVM> onPlayerVoted)
			: base(cultureCode, null)
		{
			this._onPlayerVoted = onPlayerVoted;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0002A73C File Offset: 0x0002893C
		public void ExecuteVote()
		{
			this._onPlayerVoted(this);
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06000DCF RID: 3535 RVA: 0x0002A74A File Offset: 0x0002894A
		// (set) Token: 0x06000DD0 RID: 3536 RVA: 0x0002A752 File Offset: 0x00028952
		[DataSourceProperty]
		public int Votes
		{
			get
			{
				return this._votes;
			}
			set
			{
				if (value != this._votes)
				{
					this._votes = value;
					base.OnPropertyChangedWithValue(value, "Votes");
				}
			}
		}

		// Token: 0x04000645 RID: 1605
		private readonly Action<MPIntermissionCultureItemVM> _onPlayerVoted;

		// Token: 0x04000646 RID: 1606
		private int _votes;
	}
}
