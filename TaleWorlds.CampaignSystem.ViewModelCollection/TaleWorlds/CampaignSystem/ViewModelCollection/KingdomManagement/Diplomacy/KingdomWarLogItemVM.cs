using System;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x02000079 RID: 121
	public class KingdomWarLogItemVM : ViewModel
	{
		// Token: 0x06000991 RID: 2449 RVA: 0x0002A940 File Offset: 0x00028B40
		public KingdomWarLogItemVM(IEncyclopediaLog log, IFaction effectorFaction)
		{
			this._log = log;
			this.Banner = new BannerImageIdentifierVM(effectorFaction.Banner, true);
			this.RefreshValues();
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0002A968 File Offset: 0x00028B68
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.WarLogTimeText = this._log.GameTime.ToString();
			this.WarLogText = this._log.GetEncyclopediaText().ToString();
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0002A9B0 File Offset: 0x00028BB0
		private void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x0002A9C2 File Offset: 0x00028BC2
		// (set) Token: 0x06000995 RID: 2453 RVA: 0x0002A9CA File Offset: 0x00028BCA
		[DataSourceProperty]
		public string WarLogTimeText
		{
			get
			{
				return this._warLogTimeText;
			}
			set
			{
				if (value != this._warLogTimeText)
				{
					this._warLogTimeText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarLogTimeText");
				}
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x0002A9ED File Offset: 0x00028BED
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x0002A9F5 File Offset: 0x00028BF5
		[DataSourceProperty]
		public string WarLogText
		{
			get
			{
				return this._warLogText;
			}
			set
			{
				if (value != this._warLogText)
				{
					this._warLogText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarLogText");
				}
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x0002AA18 File Offset: 0x00028C18
		// (set) Token: 0x06000999 RID: 2457 RVA: 0x0002AA20 File Offset: 0x00028C20
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner
		{
			get
			{
				return this._banner;
			}
			set
			{
				if (value != this._banner)
				{
					this._banner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner");
				}
			}
		}

		// Token: 0x04000431 RID: 1073
		private readonly IEncyclopediaLog _log;

		// Token: 0x04000432 RID: 1074
		private string _warLogText;

		// Token: 0x04000433 RID: 1075
		private string _warLogTimeText;

		// Token: 0x04000434 RID: 1076
		private BannerImageIdentifierVM _banner;
	}
}
