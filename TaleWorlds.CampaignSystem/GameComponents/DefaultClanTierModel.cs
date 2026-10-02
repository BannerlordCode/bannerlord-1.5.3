using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200010D RID: 269
	public class DefaultClanTierModel : ClanTierModel
	{
		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x060017BA RID: 6074 RVA: 0x0007004D File Offset: 0x0006E24D
		public override int MinClanTier
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x060017BB RID: 6075 RVA: 0x00070050 File Offset: 0x0006E250
		public override int MaxClanTier
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x060017BC RID: 6076 RVA: 0x00070053 File Offset: 0x0006E253
		public override int MercenaryEligibleTier
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x00070056 File Offset: 0x0006E256
		public override int VassalEligibleTier
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060017BE RID: 6078 RVA: 0x00070059 File Offset: 0x0006E259
		public override int BannerEligibleTier
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060017BF RID: 6079 RVA: 0x0007005C File Offset: 0x0006E25C
		public override int RebelClanStartingTier
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x060017C0 RID: 6080 RVA: 0x0007005F File Offset: 0x0006E25F
		public override int CompanionToLordClanStartingTier
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x00070062 File Offset: 0x0006E262
		private int KingdomEligibleTier
		{
			get
			{
				return Campaign.Current.Models.KingdomCreationModel.MinimumClanTierToCreateKingdom;
			}
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x00070078 File Offset: 0x0006E278
		public override int CalculateInitialRenown(Clan clan)
		{
			int num = DefaultClanTierModel.TierLowerRenownLimits[clan.Tier];
			int num2 = ((clan.Tier >= this.MaxClanTier) ? (DefaultClanTierModel.TierLowerRenownLimits[this.MaxClanTier] + 1500) : DefaultClanTierModel.TierLowerRenownLimits[clan.Tier + 1]);
			int num3 = (int)((float)num2 - (float)(num2 - num) * 0.4f);
			return MBRandom.RandomInt(num, num3);
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x000700D9 File Offset: 0x0006E2D9
		public override int CalculateInitialInfluence(Clan clan)
		{
			return (int)(150f + (float)MBRandom.RandomInt((int)((float)this.CalculateInitialRenown(clan) / 15f)) + (float)MBRandom.RandomInt(MBRandom.RandomInt(MBRandom.RandomInt(400))));
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x00070110 File Offset: 0x0006E310
		public override int CalculateTier(Clan clan)
		{
			int num = this.MinClanTier;
			for (int i = this.MinClanTier + 1; i <= this.MaxClanTier; i++)
			{
				if (clan.Renown >= (float)DefaultClanTierModel.TierLowerRenownLimits[i])
				{
					num = i;
				}
			}
			return num;
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x00070150 File Offset: 0x0006E350
		public override ValueTuple<ExplainedNumber, bool> HasUpcomingTier(Clan clan, out TextObject extraExplanation, bool includeDescriptions = false)
		{
			bool flag = clan.Tier < this.MaxClanTier;
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			extraExplanation = null;
			if (flag)
			{
				int num = this.GetPartyLimitForTier(clan, clan.Tier + 1) - this.GetPartyLimitForTier(clan, clan.Tier);
				if (num != 0)
				{
					explainedNumber.Add((float)num, this._partyLimitBonusText, null);
				}
				int num2 = this.GetCompanionLimitFromTier(clan.Tier + 1) - this.GetCompanionLimitFromTier(clan.Tier);
				if (num2 != 0)
				{
					explainedNumber.Add((float)num2, this._companionLimitBonusText, null);
				}
				int nextClanTierPartySizeEffectChangeForHero = Campaign.Current.Models.PartySizeLimitModel.GetNextClanTierPartySizeEffectChangeForHero(clan.Leader);
				if (nextClanTierPartySizeEffectChangeForHero > 0)
				{
					explainedNumber.Add((float)nextClanTierPartySizeEffectChangeForHero, this._additionalCurrentPartySizeBonus, null);
				}
				int num3 = Campaign.Current.Models.WorkshopModel.GetMaxWorkshopCountForClanTier(clan.Tier + 1) - Campaign.Current.Models.WorkshopModel.GetMaxWorkshopCountForClanTier(clan.Tier);
				if (num3 > 0)
				{
					explainedNumber.Add((float)num3, this._additionalWorkshopCountBonus, null);
				}
				if (clan.Tier + 1 == this.MercenaryEligibleTier)
				{
					extraExplanation = this._mercenaryEligibleText;
				}
				else if (clan.Tier + 1 == this.VassalEligibleTier)
				{
					extraExplanation = this._vassalEligibleText;
				}
				else if (clan.Tier + 1 == this.KingdomEligibleTier)
				{
					extraExplanation = this._kingdomEligibleText;
				}
			}
			return new ValueTuple<ExplainedNumber, bool>(explainedNumber, flag);
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x000702B7 File Offset: 0x0006E4B7
		public override int GetRequiredRenownForTier(int tier)
		{
			return DefaultClanTierModel.TierLowerRenownLimits[tier];
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x000702C0 File Offset: 0x0006E4C0
		public override int GetPartyLimitForTier(Clan clan, int clanTierToCheck)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			if (!clan.IsMinorFaction)
			{
				if (clanTierToCheck < 3)
				{
					explainedNumber.Add(1f, null, null);
				}
				else if (clanTierToCheck < 5)
				{
					explainedNumber.Add(2f, null, null);
				}
				else
				{
					explainedNumber.Add(3f, null, null);
				}
			}
			else
			{
				explainedNumber.Add(MathF.Clamp((float)clanTierToCheck, 1f, 4f), null, null);
			}
			this.AddPartyLimitPerkEffects(clan, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0007034A File Offset: 0x0006E54A
		private void AddPartyLimitPerkEffects(Clan clan, ref ExplainedNumber result)
		{
			if (clan.Leader != null)
			{
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Leadership.TalentMagnet, BattleEnvironment.Any, clan.Leader.CharacterObject, false, ref result);
			}
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x00070370 File Offset: 0x0006E570
		public override int GetCompanionLimit(Clan clan)
		{
			int num = this.GetCompanionLimitFromTier(clan.Tier);
			if (clan.Leader.GetPerkValue(DefaultPerks.Leadership.WePledgeOurSwords))
			{
				num += (int)DefaultPerks.Leadership.WePledgeOurSwords.PrimaryBonus;
			}
			if (clan.Leader.GetPerkValue(DefaultPerks.Charm.Camaraderie))
			{
				num += (int)DefaultPerks.Charm.Camaraderie.SecondaryBonus;
			}
			return num;
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x000703CB File Offset: 0x0006E5CB
		private int GetCompanionLimitFromTier(int clanTier)
		{
			return clanTier + 3;
		}

		// Token: 0x040007F8 RID: 2040
		private static readonly int[] TierLowerRenownLimits = new int[] { 0, 50, 150, 350, 900, 2350, 6150 };

		// Token: 0x040007F9 RID: 2041
		private readonly TextObject _partyLimitBonusText = GameTexts.FindText("str_clan_tier_party_limit_bonus", null);

		// Token: 0x040007FA RID: 2042
		private readonly TextObject _companionLimitBonusText = GameTexts.FindText("str_clan_tier_companion_limit_bonus", null);

		// Token: 0x040007FB RID: 2043
		private readonly TextObject _mercenaryEligibleText = GameTexts.FindText("str_clan_tier_mercenary_eligible", null);

		// Token: 0x040007FC RID: 2044
		private readonly TextObject _vassalEligibleText = GameTexts.FindText("str_clan_tier_vassal_eligible", null);

		// Token: 0x040007FD RID: 2045
		private readonly TextObject _additionalCurrentPartySizeBonus = GameTexts.FindText("str_clan_tier_party_size_bonus", null);

		// Token: 0x040007FE RID: 2046
		private readonly TextObject _additionalWorkshopCountBonus = GameTexts.FindText("str_clan_tier_workshop_count_bonus", null);

		// Token: 0x040007FF RID: 2047
		private readonly TextObject _kingdomEligibleText = GameTexts.FindText("str_clan_tier_kingdom_eligible", null);
	}
}
