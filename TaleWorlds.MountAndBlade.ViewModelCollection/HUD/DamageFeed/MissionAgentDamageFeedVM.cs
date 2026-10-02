using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.DamageFeed
{
	// Token: 0x02000066 RID: 102
	public class MissionAgentDamageFeedVM : ViewModel
	{
		// Token: 0x060007FB RID: 2043 RVA: 0x0001BBDF File Offset: 0x00019DDF
		public MissionAgentDamageFeedVM()
		{
			this._takenDamageText = new TextObject("{=meFS5F4V}-{DAMAGE}", null);
			this.FeedList = new MBBindingList<MissionAgentDamageFeedItemVM>();
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001BC04 File Offset: 0x00019E04
		public void OnMainAgentHit(float damage)
		{
			if (damage > 0f)
			{
				this._takenDamageText.SetTextVariable("DAMAGE", damage, 2);
				MissionAgentDamageFeedItemVM missionAgentDamageFeedItemVM = new MissionAgentDamageFeedItemVM(this._takenDamageText.ToString(), new Action<MissionAgentDamageFeedItemVM>(this.RemoveItem));
				this.FeedList.Add(missionAgentDamageFeedItemVM);
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0001BC55 File Offset: 0x00019E55
		private void RemoveItem(MissionAgentDamageFeedItemVM item)
		{
			this.FeedList.Remove(item);
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x0001BC64 File Offset: 0x00019E64
		// (set) Token: 0x060007FF RID: 2047 RVA: 0x0001BC6C File Offset: 0x00019E6C
		[DataSourceProperty]
		public MBBindingList<MissionAgentDamageFeedItemVM> FeedList
		{
			get
			{
				return this._feedList;
			}
			set
			{
				if (value != this._feedList)
				{
					this._feedList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionAgentDamageFeedItemVM>>(value, "FeedList");
				}
			}
		}

		// Token: 0x040003A2 RID: 930
		private readonly TextObject _takenDamageText;

		// Token: 0x040003A3 RID: 931
		private MBBindingList<MissionAgentDamageFeedItemVM> _feedList;
	}
}
