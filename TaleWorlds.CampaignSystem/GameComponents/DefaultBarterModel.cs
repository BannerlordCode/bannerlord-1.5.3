using System;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FA RID: 250
	public class DefaultBarterModel : BarterModel
	{
		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060016F0 RID: 5872 RVA: 0x0006A7C4 File Offset: 0x000689C4
		public override int BarterCooldownWithHeroInDays
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060016F1 RID: 5873 RVA: 0x0006A7C7 File Offset: 0x000689C7
		private int MaximumOverpayRelationBonus
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060016F2 RID: 5874 RVA: 0x0006A7CA File Offset: 0x000689CA
		public override float MaximumPercentageOfNpcGoldToSpendAtBarter
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x0006A7D4 File Offset: 0x000689D4
		public override int CalculateOverpayRelationIncreaseCosts(Hero hero, float overpayAmount)
		{
			int num = (int)hero.GetRelationWithPlayer();
			float num2 = MathF.Clamp((float)(num + this.MaximumOverpayRelationBonus), -100f, 100f);
			float num3 = 0f;
			int num4 = num;
			while ((float)num4 < num2)
			{
				int num5 = 1000 + 100 * (num4 * num4);
				if (overpayAmount >= (float)num5)
				{
					overpayAmount -= (float)num5;
					num3 += 1f;
					num4++;
				}
				else
				{
					if (MBRandom.RandomFloat <= overpayAmount / (float)num5)
					{
						num3 += 1f;
						break;
					}
					break;
				}
			}
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Charm.Tribute))
			{
				num3 *= 1f + DefaultPerks.Charm.Tribute.PrimaryBonus;
			}
			return MathF.Ceiling(num3);
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x0006A874 File Offset: 0x00068A74
		public override ExplainedNumber GetBarterPenalty(IFaction faction, ItemBarterable itemBarterable, Hero otherHero, PartyBase otherParty)
		{
			ExplainedNumber explainedNumber;
			if (faction == ((otherHero != null) ? otherHero.Clan : null) || faction == ((otherHero != null) ? otherHero.MapFaction : null) || faction == ((otherParty != null) ? otherParty.MapFaction : null))
			{
				explainedNumber = new ExplainedNumber(0.4f, false, null);
				if (otherHero != null && itemBarterable.OriginalOwner != null && otherHero != itemBarterable.OriginalOwner && otherHero.MapFaction != null && otherHero.IsPartyLeader)
				{
					CultureObject culture = otherHero.Culture;
					Hero originalOwner = itemBarterable.OriginalOwner;
					if (culture == ((originalOwner != null) ? originalOwner.Culture : null))
					{
						if (itemBarterable.OriginalOwner.GetPerkValue(DefaultPerks.Charm.EffortForThePeople))
						{
							explainedNumber.AddFactor(-DefaultPerks.Charm.EffortForThePeople.SecondaryBonus, null);
						}
					}
					else if (itemBarterable.OriginalOwner.GetPerkValue(DefaultPerks.Charm.SlickNegotiator))
					{
						explainedNumber.AddFactor(-DefaultPerks.Charm.SlickNegotiator.SecondaryBonus, null);
					}
					if (itemBarterable.OriginalOwner.GetPerkValue(DefaultPerks.Trade.SelfMadeMan))
					{
						explainedNumber.AddFactor(-DefaultPerks.Trade.SelfMadeMan.PrimaryBonus, null);
					}
				}
			}
			else
			{
				Hero originalOwner2 = itemBarterable.OriginalOwner;
				if (faction != ((originalOwner2 != null) ? originalOwner2.Clan : null))
				{
					Hero originalOwner3 = itemBarterable.OriginalOwner;
					if (faction != ((originalOwner3 != null) ? originalOwner3.MapFaction : null))
					{
						PartyBase originalParty = itemBarterable.OriginalParty;
						if (faction != ((originalParty != null) ? originalParty.MapFaction : null))
						{
							explainedNumber = new ExplainedNumber(0f, false, null);
							return explainedNumber;
						}
					}
				}
				if (itemBarterable.ItemRosterElement.EquipmentElement.Item.IsAnimal || itemBarterable.ItemRosterElement.EquipmentElement.Item.IsMountable)
				{
					explainedNumber = new ExplainedNumber(-8.4f, false, null);
				}
				else if (itemBarterable.ItemRosterElement.EquipmentElement.Item.IsFood)
				{
					explainedNumber = new ExplainedNumber(-12.6f, false, null);
				}
				else
				{
					explainedNumber = new ExplainedNumber(-2.1f, false, null);
				}
				if (otherHero != null && otherHero != itemBarterable.OriginalOwner && otherHero.MapFaction != null && otherHero.IsPartyLeader)
				{
					CultureObject culture2 = otherHero.Culture;
					Hero originalOwner4 = itemBarterable.OriginalOwner;
					if (culture2 == ((originalOwner4 != null) ? originalOwner4.Culture : null))
					{
						if (otherHero.GetPerkValue(DefaultPerks.Charm.EffortForThePeople))
						{
							explainedNumber.AddFactor(DefaultPerks.Charm.EffortForThePeople.SecondaryBonus, null);
						}
					}
					else if (otherHero.GetPerkValue(DefaultPerks.Charm.SlickNegotiator))
					{
						explainedNumber.AddFactor(DefaultPerks.Charm.SlickNegotiator.SecondaryBonus, null);
					}
					if (otherHero.GetPerkValue(DefaultPerks.Trade.SelfMadeMan))
					{
						explainedNumber.AddFactor(DefaultPerks.Trade.SelfMadeMan.PrimaryBonus, null);
					}
				}
			}
			return explainedNumber;
		}
	}
}
