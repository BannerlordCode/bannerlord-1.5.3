using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance
{
	// Token: 0x02000138 RID: 312
	public class ClanFinanceCommonAreaItemVM : ClanFinanceIncomeItemBaseVM
	{
		// Token: 0x06001DD0 RID: 7632 RVA: 0x0006BB54 File Offset: 0x00069D54
		public ClanFinanceCommonAreaItemVM(Alley alley, Action<ClanFinanceIncomeItemBaseVM> onSelection, Action onRefresh)
			: base(onSelection, onRefresh)
		{
			base.IncomeTypeAsEnum = IncomeTypes.CommonArea;
			this._alley = alley;
			GameTexts.SetVariable("SETTLEMENT_NAME", alley.Settlement.Name);
			GameTexts.SetVariable("COMMON_AREA_NAME", alley.Name);
			base.Name = GameTexts.FindText("str_clan_finance_common_area", null).ToString();
			base.Income = Campaign.Current.Models.AlleyModel.GetDailyIncomeOfAlley(alley);
			base.Visual = ((alley.Owner.CharacterObject != null) ? new CharacterImageIdentifierVM(CharacterCode.CreateFrom(alley.Owner.CharacterObject)) : new CharacterImageIdentifierVM(null));
			base.IncomeValueText = base.DetermineIncomeText(base.Income);
			this.PopulateActionList();
			this.PopulateStatsList();
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x0006BC1B File Offset: 0x00069E1B
		protected override void PopulateActionList()
		{
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x0006BC1D File Offset: 0x00069E1D
		protected override void PopulateStatsList()
		{
			base.ItemProperties.Add(new SelectableItemPropertyVM("TEST", "TEST", false, null));
		}

		// Token: 0x04000DA2 RID: 3490
		private Alley _alley;
	}
}
