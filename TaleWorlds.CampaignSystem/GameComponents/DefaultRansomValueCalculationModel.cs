using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200014D RID: 333
	public class DefaultRansomValueCalculationModel : RansomValueCalculationModel
	{
		// Token: 0x06001A53 RID: 6739 RVA: 0x00084C78 File Offset: 0x00082E78
		public override int PrisonerRansomValue(CharacterObject prisoner, Hero sellerHero = null)
		{
			int roundedResultNumber = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(prisoner, null, false).RoundedResultNumber;
			float num = 0f;
			float num2 = 0f;
			float num3 = 1f;
			Hero heroObject = prisoner.HeroObject;
			if (((heroObject != null) ? heroObject.Clan : null) != null)
			{
				num = (float)(prisoner.HeroObject.Clan.Tier + 2) * 200f * (prisoner.HeroObject.IsClanLeader ? (prisoner.HeroObject.IsKingdomLeader ? 6f : 2.5f) : 1f);
				num2 = MathF.Sqrt((float)MathF.Max(0, prisoner.HeroObject.Gold)) * 6f;
				if (prisoner.HeroObject.Clan.Kingdom != null)
				{
					int count = prisoner.HeroObject.Clan.Kingdom.Fiefs.Count;
					num3 = (prisoner.HeroObject.MapFaction.IsKingdomFaction ? ((count < 8) ? (((float)count + 1f) / 9f) : (1f + MathF.Sqrt((float)(count - 8)) * 0.1f)) : 1f);
				}
				else
				{
					num3 = 0.5f;
				}
			}
			float num4 = ((prisoner.HeroObject != null) ? (num + num2) : 0f);
			ExplainedNumber explainedNumber = new ExplainedNumber(((float)roundedResultNumber + num4) * ((!prisoner.IsHero) ? 0.25f : 1f) * num3, false, null);
			if (sellerHero != null)
			{
				if (!prisoner.IsHero)
				{
					if (sellerHero.GetPerkValue(DefaultPerks.Roguery.Manhunter) && sellerHero.PartyBelongedTo != null && !sellerHero.PartyBelongedTo.IsCurrentlyAtSea)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.Manhunter, BattleEnvironment.Any, sellerHero.CharacterObject, true, ref explainedNumber);
					}
					TraitEffectHelper.ApplyTraitEffect(sellerHero, DefaultPersonalityTraitEffects.MercyTroopRansomEffect, ref explainedNumber);
				}
				else
				{
					if (sellerHero.IsPartyLeader && sellerHero.GetPerkValue(DefaultPerks.Roguery.RansomBroker))
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Roguery.RansomBroker, sellerHero.PartyBelongedTo, true, ref explainedNumber);
					}
					Clan clan = sellerHero.Clan;
					Hero hero = ((clan != null) ? clan.Leader : null);
					if (hero != null)
					{
						TraitEffectHelper.ApplyTraitEffect(hero, DefaultPersonalityTraitEffects.MercyLordRansomEffect, ref explainedNumber);
					}
				}
			}
			explainedNumber.LimitMin(1f);
			return explainedNumber.RoundedResultNumber;
		}
	}
}
