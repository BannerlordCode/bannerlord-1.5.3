using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x02000073 RID: 115
	public class KingdomDiplomacyFactionItemVM : ViewModel
	{
		// Token: 0x060008FC RID: 2300 RVA: 0x0002853E File Offset: 0x0002673E
		public KingdomDiplomacyFactionItemVM(IFaction faction)
		{
			this.Hint = new HintViewModel(faction.Name, null);
			this.Visual = new BannerImageIdentifierVM(faction.Banner, true);
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x0002856A File Offset: 0x0002676A
		// (set) Token: 0x060008FE RID: 2302 RVA: 0x00028572 File Offset: 0x00026772
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x00028590 File Offset: 0x00026790
		// (set) Token: 0x06000900 RID: 2304 RVA: 0x00028598 File Offset: 0x00026798
		[DataSourceProperty]
		public BannerImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x040003EB RID: 1003
		private HintViewModel _hint;

		// Token: 0x040003EC RID: 1004
		private BannerImageIdentifierVM _visual;
	}
}
