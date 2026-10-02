using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FF RID: 255
	public class DefaultBribeCalculationModel : BribeCalculationModel
	{
		// Token: 0x0600171D RID: 5917 RVA: 0x0006BC6C File Offset: 0x00069E6C
		public override bool IsBribeNotNeededToEnterKeep(Settlement settlement)
		{
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterLordsHall(settlement, out accessDetails);
			return accessDetails.AccessLevel != SettlementAccessModel.AccessLevel.LimitedAccess || (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && accessDetails.LimitedAccessSolution != SettlementAccessModel.LimitedAccessSolution.Bribe);
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x0006BCB4 File Offset: 0x00069EB4
		public override bool IsBribeNotNeededToEnterDungeon(Settlement settlement)
		{
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterDungeon(settlement, out accessDetails);
			return accessDetails.AccessLevel != SettlementAccessModel.AccessLevel.LimitedAccess || (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && accessDetails.LimitedAccessSolution != SettlementAccessModel.LimitedAccessSolution.Bribe);
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x0006BCFA File Offset: 0x00069EFA
		private float GetSkillFactor()
		{
			return (1f - (float)Hero.MainHero.GetSkillValue(DefaultSkills.Roguery) / 300f) * 0.65f + 0.35f;
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x0006BD24 File Offset: 0x00069F24
		private int GetBribeForCriminalRating(IFaction faction)
		{
			return MathF.Round(Campaign.Current.Models.CrimeModel.GetCost(faction, CrimeModel.PaymentMethod.Gold, 0f)) / 5;
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x0006BD48 File Offset: 0x00069F48
		private int GetBaseBribeValue(IFaction faction)
		{
			if (faction.IsAtWarWith(Clan.PlayerClan))
			{
				return 5000;
			}
			if (faction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				return 3000;
			}
			if (FactionManager.IsNeutralWithFaction(faction, Clan.PlayerClan))
			{
				return 100;
			}
			if (Hero.MainHero.Clan == faction)
			{
				return 0;
			}
			if (Hero.MainHero.MapFaction == faction)
			{
				return 0;
			}
			if (faction is Clan)
			{
				IFaction mapFaction = Hero.MainHero.MapFaction;
				Kingdom kingdom = (faction as Clan).Kingdom;
				return 0;
			}
			return 0;
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x0006BDD0 File Offset: 0x00069FD0
		public override int GetBribeToEnterLordsHall(Settlement settlement)
		{
			if (this.IsBribeNotNeededToEnterKeep(settlement))
			{
				return 0;
			}
			return this.GetBribeInternal(settlement);
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x0006BDE4 File Offset: 0x00069FE4
		public override int GetBribeToEnterDungeon(Settlement settlement)
		{
			return this.GetBribeToEnterLordsHall(settlement);
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x0006BDF0 File Offset: 0x00069FF0
		private int GetBribeInternal(Settlement settlement)
		{
			int num = this.GetBaseBribeValue(settlement.MapFaction);
			num += this.GetBribeForCriminalRating(settlement.MapFaction);
			if (Clan.PlayerClan.Renown < 500f)
			{
				num += (500 - (int)Clan.PlayerClan.Renown) * 15 / 10;
				num = MathF.Max(num, 50);
			}
			num = (int)((float)num * this.GetSkillFactor() / 25f) * 25;
			return MathF.Max(num - settlement.BribePaid, 0);
		}
	}
}
