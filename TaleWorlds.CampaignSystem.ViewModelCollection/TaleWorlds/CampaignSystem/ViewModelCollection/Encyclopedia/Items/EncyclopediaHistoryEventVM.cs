using System;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000ED RID: 237
	public class EncyclopediaHistoryEventVM : EncyclopediaLinkVM
	{
		// Token: 0x060015D0 RID: 5584 RVA: 0x00056217 File Offset: 0x00054417
		public EncyclopediaHistoryEventVM(IEncyclopediaLog log)
		{
			this._log = log;
			this.RefreshValues();
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x0005622C File Offset: 0x0005442C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.HistoryEventTimeText = this._log.GameTime.ToString();
			this.HistoryEventText = this._log.GetEncyclopediaText().ToString();
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x00056274 File Offset: 0x00054474
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x060015D3 RID: 5587 RVA: 0x00056286 File Offset: 0x00054486
		// (set) Token: 0x060015D4 RID: 5588 RVA: 0x0005628E File Offset: 0x0005448E
		[DataSourceProperty]
		public string HistoryEventTimeText
		{
			get
			{
				return this._historyEventTimeText;
			}
			set
			{
				if (value != this._historyEventTimeText)
				{
					this._historyEventTimeText = value;
					base.OnPropertyChangedWithValue<string>(value, "HistoryEventTimeText");
				}
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x060015D5 RID: 5589 RVA: 0x000562B1 File Offset: 0x000544B1
		// (set) Token: 0x060015D6 RID: 5590 RVA: 0x000562B9 File Offset: 0x000544B9
		[DataSourceProperty]
		public string HistoryEventText
		{
			get
			{
				return this._historyEventText;
			}
			set
			{
				if (value != this._historyEventText)
				{
					this._historyEventText = value;
					base.OnPropertyChangedWithValue<string>(value, "HistoryEventText");
				}
			}
		}

		// Token: 0x040009E0 RID: 2528
		private readonly IEncyclopediaLog _log;

		// Token: 0x040009E1 RID: 2529
		private string _historyEventText;

		// Token: 0x040009E2 RID: 2530
		private string _historyEventTimeText;
	}
}
