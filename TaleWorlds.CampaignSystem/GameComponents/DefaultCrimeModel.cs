using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000111 RID: 273
	public class DefaultCrimeModel : CrimeModel
	{
		// Token: 0x060017E9 RID: 6121 RVA: 0x000713EA File Offset: 0x0006F5EA
		public override bool DoesPlayerHaveAnyCrimeRating(IFaction faction)
		{
			return faction.MainHeroCrimeRating > 0f;
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x000713F9 File Offset: 0x0006F5F9
		public override bool IsPlayerCrimeRatingSevere(IFaction faction)
		{
			return faction.MainHeroCrimeRating >= 65f;
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x0007140B File Offset: 0x0006F60B
		public override bool IsPlayerCrimeRatingModerate(IFaction faction)
		{
			return faction.MainHeroCrimeRating > 30f && faction.MainHeroCrimeRating <= 65f;
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x0007142C File Offset: 0x0006F62C
		public override bool IsPlayerCrimeRatingMild(IFaction faction)
		{
			return faction.MainHeroCrimeRating > 0f && faction.MainHeroCrimeRating <= 30f;
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x00071450 File Offset: 0x0006F650
		public override float GetCost(IFaction faction, CrimeModel.PaymentMethod paymentMethod, float minimumCrimeRating)
		{
			float num = MathF.Max(0f, faction.MainHeroCrimeRating - minimumCrimeRating);
			if (paymentMethod == CrimeModel.PaymentMethod.Gold)
			{
				return (float)((int)(MathF.Pow(num, 1.2f) * 100f));
			}
			if (paymentMethod != CrimeModel.PaymentMethod.Influence)
			{
				return 0f;
			}
			return MathF.Pow(num, 1.2f);
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x000714A0 File Offset: 0x0006F6A0
		public override ExplainedNumber GetEffectiveCrimeChange(IFaction faction, float deltaCrimeRating)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(deltaCrimeRating, false, null);
			if (deltaCrimeRating > 0f)
			{
				TraitEffectHelper.ApplyTraitEffect(Hero.MainHero, DefaultPersonalityTraitEffects.HonorCrimeIncreaseSlowEffect, ref explainedNumber);
			}
			explainedNumber.Add(faction.MainHeroCrimeRating, null, null);
			explainedNumber.LimitMin(0f);
			explainedNumber.LimitMax(Campaign.Current.Models.CrimeModel.GetMaxCrimeRating(), null);
			return explainedNumber;
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x00071508 File Offset: 0x0006F708
		public override ExplainedNumber GetDailyCrimeRatingChange(IFaction faction, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			int num = faction.Settlements.Count<Settlement>(delegate(Settlement x)
			{
				if (x.IsTown)
				{
					return x.Alleys.Any<Alley>((Alley y) => y.Owner == Hero.MainHero);
				}
				return false;
			});
			explainedNumber.Add((float)num * Campaign.Current.Models.AlleyModel.GetDailyCrimeRatingOfAlley, includeDescriptions ? new TextObject("{=t87T82jq}Owned alleys", null) : null, null);
			if (faction.MainHeroCrimeRating.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return explainedNumber;
			}
			Clan clan;
			if (Hero.MainHero.Clan == faction)
			{
				explainedNumber.Add(-5f, includeDescriptions ? new TextObject("{=eNtRt6F5}Your own Clan", null) : null, null);
			}
			else if (faction.IsKingdomFaction && faction.Leader == Hero.MainHero)
			{
				explainedNumber.Add(-5f, includeDescriptions ? new TextObject("{=xer2bta5}Your own Kingdom", null) : null, null);
			}
			else if (Hero.MainHero.MapFaction == faction)
			{
				explainedNumber.Add(-1.5f, includeDescriptions ? new TextObject("{=QRwaQIbm}Is in Kingdom", null) : null, null);
			}
			else if ((clan = faction as Clan) != null && Hero.MainHero.MapFaction == clan.Kingdom)
			{
				explainedNumber.Add(-1.25f, includeDescriptions ? new TextObject("{=hXGByLG9}Sharing the same Kingdom", null) : null, null);
			}
			else if (Hero.MainHero.Clan.IsAtWarWith(faction))
			{
				explainedNumber.Add(-0.25f, includeDescriptions ? new TextObject("{=BYTrUJyj}In War", null) : null, null);
			}
			else
			{
				explainedNumber.Add(-1f, includeDescriptions ? new TextObject("{=basevalue}Base", null) : null, null);
			}
			TraitEffectHelper.ApplyTraitEffect(Hero.MainHero, DefaultPersonalityTraitEffects.HonorCrimeDecaySlowEffect, ref explainedNumber);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.WhiteLies, BattleEnvironment.Any, Hero.MainHero.CharacterObject, true, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060017F0 RID: 6128 RVA: 0x000716E6 File Offset: 0x0006F8E6
		public override float DeclareWarCrimeRatingThreshold
		{
			get
			{
				return 60f;
			}
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x000716ED File Offset: 0x0006F8ED
		public override float GetMaxCrimeRating()
		{
			return 100f;
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x000716F4 File Offset: 0x0006F8F4
		public override float GetMinAcceptableCrimeRating(IFaction faction)
		{
			if (faction != Hero.MainHero.MapFaction)
			{
				return 30f;
			}
			return 20f;
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x0007170E File Offset: 0x0006F90E
		public override float GetCrimeRatingAfterPunishment()
		{
			return 25f;
		}

		// Token: 0x04000800 RID: 2048
		private const float ModerateCrimeRatingThreshold = 30f;

		// Token: 0x04000801 RID: 2049
		private const float SevereCrimeRatingThreshold = 65f;
	}
}
