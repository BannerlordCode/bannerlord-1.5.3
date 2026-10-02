using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000132 RID: 306
	public class DefaultMarriageModel : MarriageModel
	{
		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06001960 RID: 6496 RVA: 0x0007C294 File Offset: 0x0007A494
		public override int MinimumMarriageAgeMale
		{
			get
			{
				return 18;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06001961 RID: 6497 RVA: 0x0007C298 File Offset: 0x0007A498
		public override int MinimumMarriageAgeFemale
		{
			get
			{
				return 18;
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0007C29C File Offset: 0x0007A49C
		public override bool IsCoupleSuitableForMarriage(Hero firstHero, Hero secondHero)
		{
			if (this.IsClanSuitableForMarriage(firstHero.Clan) && this.IsClanSuitableForMarriage(secondHero.Clan))
			{
				Clan clan = firstHero.Clan;
				if (((clan != null) ? clan.Leader : null) == firstHero)
				{
					Clan clan2 = secondHero.Clan;
					if (((clan2 != null) ? clan2.Leader : null) == secondHero)
					{
						return false;
					}
				}
				if (firstHero.IsFemale != secondHero.IsFemale && !this.AreHeroesRelated(firstHero, secondHero, 3))
				{
					Hero courtedHeroInOtherClan = Romance.GetCourtedHeroInOtherClan(firstHero, secondHero);
					if (courtedHeroInOtherClan != null && courtedHeroInOtherClan != secondHero)
					{
						return false;
					}
					Hero courtedHeroInOtherClan2 = Romance.GetCourtedHeroInOtherClan(secondHero, firstHero);
					return (courtedHeroInOtherClan2 == null || courtedHeroInOtherClan2 == firstHero) && firstHero.CanMarry() && secondHero.CanMarry();
				}
			}
			return false;
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0007C33C File Offset: 0x0007A53C
		public override bool IsClanSuitableForMarriage(Clan clan)
		{
			return clan != null && !clan.IsBanditFaction && !clan.IsRebelClan && !clan.IsEliminated;
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0007C35C File Offset: 0x0007A55C
		public override float NpcCoupleMarriageChance(Hero firstHero, Hero secondHero)
		{
			if (this.IsCoupleSuitableForMarriage(firstHero, secondHero))
			{
				float num = 0.002f;
				num *= 1f + (firstHero.Age - (float)Campaign.Current.Models.AgeModel.HeroComesOfAge) / 50f;
				num *= 1f + (secondHero.Age - (float)Campaign.Current.Models.AgeModel.HeroComesOfAge) / 50f;
				num *= 1f - MathF.Abs(secondHero.Age - firstHero.Age) / 50f;
				if (firstHero.Clan.Kingdom != secondHero.Clan.Kingdom)
				{
					num *= 0.5f;
				}
				float num2 = 0.5f + (float)firstHero.Clan.GetRelationWithClan(secondHero.Clan) / 200f;
				return num * num2;
			}
			return 0f;
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0007C43B File Offset: 0x0007A63B
		public override bool ShouldNpcMarriageBetweenClansBeAllowed(Clan consideringClan, Clan targetClan)
		{
			return targetClan != consideringClan && !consideringClan.IsAtWarWith(targetClan) && consideringClan.GetRelationWithClan(targetClan) >= -50;
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0007C45C File Offset: 0x0007A65C
		public override List<Hero> GetAdultChildrenSuitableForMarriage(Hero hero)
		{
			List<Hero> list = new List<Hero>();
			foreach (Hero hero2 in hero.Children)
			{
				if (hero2.CanMarry())
				{
					list.Add(hero2);
				}
			}
			return list;
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0007C4C0 File Offset: 0x0007A6C0
		private bool AreHeroesRelatedAux1(Hero firstHero, Hero secondHero, int ancestorDepth)
		{
			return firstHero == secondHero || (ancestorDepth > 0 && ((secondHero.Mother != null && this.AreHeroesRelatedAux1(firstHero, secondHero.Mother, ancestorDepth - 1)) || (secondHero.Father != null && this.AreHeroesRelatedAux1(firstHero, secondHero.Father, ancestorDepth - 1))));
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x0007C510 File Offset: 0x0007A710
		private bool AreHeroesRelatedAux2(Hero firstHero, Hero secondHero, int ancestorDepth, int secondAncestorDepth)
		{
			return this.AreHeroesRelatedAux1(firstHero, secondHero, secondAncestorDepth) || (ancestorDepth > 0 && ((firstHero.Mother != null && this.AreHeroesRelatedAux2(firstHero.Mother, secondHero, ancestorDepth - 1, secondAncestorDepth)) || (firstHero.Father != null && this.AreHeroesRelatedAux2(firstHero.Father, secondHero, ancestorDepth - 1, secondAncestorDepth))));
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x0007C56B File Offset: 0x0007A76B
		private bool AreHeroesRelated(Hero firstHero, Hero secondHero, int ancestorDepth)
		{
			return this.AreHeroesRelatedAux2(firstHero, secondHero, ancestorDepth, ancestorDepth);
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0007C578 File Offset: 0x0007A778
		public override int GetEffectiveRelationIncrease(Hero firstHero, Hero secondHero)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(20f, false, null);
			SkillHelper.AddSkillBonusForCharacter(DefaultSkillEffects.CharmRelationBonus, firstHero.IsFemale ? secondHero.CharacterObject : firstHero.CharacterObject, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0007C5C4 File Offset: 0x0007A7C4
		public override bool IsSuitableForMarriage(Hero maidenOrSuitor)
		{
			if (maidenOrSuitor.IsActive && maidenOrSuitor.Spouse == null && maidenOrSuitor.IsLord && !maidenOrSuitor.IsMinorFactionHero && !maidenOrSuitor.IsNotable && !maidenOrSuitor.IsTemplate)
			{
				MobileParty partyBelongedTo = maidenOrSuitor.PartyBelongedTo;
				if (((partyBelongedTo != null) ? partyBelongedTo.MapEvent : null) == null)
				{
					MobileParty partyBelongedTo2 = maidenOrSuitor.PartyBelongedTo;
					if (((partyBelongedTo2 != null) ? partyBelongedTo2.Army : null) == null)
					{
						IMarriageOfferCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IMarriageOfferCampaignBehavior>();
						if (campaignBehavior != null && campaignBehavior.IsHeroEngaged(maidenOrSuitor))
						{
							return false;
						}
						if (maidenOrSuitor.IsFemale)
						{
							return maidenOrSuitor.CharacterObject.Age >= (float)this.MinimumMarriageAgeFemale;
						}
						return maidenOrSuitor.CharacterObject.Age >= (float)this.MinimumMarriageAgeMale;
					}
				}
			}
			return false;
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x0007C68C File Offset: 0x0007A88C
		public override Clan GetClanAfterMarriage(Hero firstHero, Hero secondHero)
		{
			if (firstHero.IsHumanPlayerCharacter)
			{
				return firstHero.Clan;
			}
			if (secondHero.IsHumanPlayerCharacter)
			{
				return secondHero.Clan;
			}
			if (firstHero.Clan.Leader == firstHero)
			{
				return firstHero.Clan;
			}
			if (secondHero.Clan.Leader == secondHero)
			{
				return secondHero.Clan;
			}
			if (!firstHero.IsFemale)
			{
				return firstHero.Clan;
			}
			return secondHero.Clan;
		}

		// Token: 0x04000844 RID: 2116
		private const float BaseMarriageChanceForNpcs = 0.002f;
	}
}
