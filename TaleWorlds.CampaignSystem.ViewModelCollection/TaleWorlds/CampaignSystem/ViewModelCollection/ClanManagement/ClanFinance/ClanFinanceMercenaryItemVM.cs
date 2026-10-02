using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance
{
	// Token: 0x02000139 RID: 313
	public class ClanFinanceMercenaryItemVM : ClanFinanceIncomeItemBaseVM
	{
		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06001DD3 RID: 7635 RVA: 0x0006BC3B File Offset: 0x00069E3B
		// (set) Token: 0x06001DD4 RID: 7636 RVA: 0x0006BC43 File Offset: 0x00069E43
		public Clan Clan { get; private set; }

		// Token: 0x06001DD5 RID: 7637 RVA: 0x0006BC4C File Offset: 0x00069E4C
		public ClanFinanceMercenaryItemVM(Action<ClanFinanceIncomeItemBaseVM> onSelection, Action onRefresh)
			: base(onSelection, onRefresh)
		{
			base.IncomeTypeAsEnum = IncomeTypes.MercenaryService;
			this.Clan = Clan.PlayerClan;
			if (this.Clan.IsUnderMercenaryService)
			{
				base.Name = GameTexts.FindText("str_mercenary_service", null).ToString();
				base.Income = (int)(this.Clan.Influence * (float)this.Clan.MercenaryAwardMultiplier);
				base.Visual = new BannerImageIdentifierVM(this.Clan.Banner, false);
				base.IncomeValueText = base.DetermineIncomeText(base.Income);
			}
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x0006BCDE File Offset: 0x00069EDE
		protected override void PopulateStatsList()
		{
			base.ItemProperties.Add(new SelectableItemPropertyVM("TEST", "TEST", false, null));
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x0006BCFC File Offset: 0x00069EFC
		protected override void PopulateActionList()
		{
		}
	}
}
