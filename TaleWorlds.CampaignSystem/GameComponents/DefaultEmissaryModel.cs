using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000119 RID: 281
	public class DefaultEmissaryModel : EmissaryModel
	{
		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001861 RID: 6241 RVA: 0x00074E0C File Offset: 0x0007300C
		public override int EmissaryRelationBonusForMainClan
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x00074E10 File Offset: 0x00073010
		public override bool IsEmissary(Hero hero)
		{
			return (hero.CompanionOf == Clan.PlayerClan || hero.Clan == Clan.PlayerClan) && hero.PartyBelongedTo == null && hero.CurrentSettlement != null && hero.CurrentSettlement.IsFortification && !hero.IsPrisoner && hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge;
		}
	}
}
