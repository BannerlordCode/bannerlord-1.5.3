using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.DamageFeed
{
	// Token: 0x02000065 RID: 101
	public class MissionAgentDamageFeedItemVM : ViewModel
	{
		// Token: 0x060007F7 RID: 2039 RVA: 0x0001BB90 File Offset: 0x00019D90
		public MissionAgentDamageFeedItemVM(string feedText, Action<MissionAgentDamageFeedItemVM> onRemove)
		{
			this._onRemove = onRemove;
			this.FeedText = feedText;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0001BBA6 File Offset: 0x00019DA6
		public void ExecuteRemove()
		{
			this._onRemove(this);
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0001BBB4 File Offset: 0x00019DB4
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x0001BBBC File Offset: 0x00019DBC
		[DataSourceProperty]
		public string FeedText
		{
			get
			{
				return this._feedText;
			}
			set
			{
				if (value != this._feedText)
				{
					this._feedText = value;
					base.OnPropertyChangedWithValue<string>(value, "FeedText");
				}
			}
		}

		// Token: 0x040003A0 RID: 928
		private readonly Action<MissionAgentDamageFeedItemVM> _onRemove;

		// Token: 0x040003A1 RID: 929
		private string _feedText;
	}
}
