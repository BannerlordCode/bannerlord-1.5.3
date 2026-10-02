using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000107 RID: 263
	public class DefaultCaravanModel : CaravanModel
	{
		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001759 RID: 5977 RVA: 0x0006C7B7 File Offset: 0x0006A9B7
		public override int MaxNumberOfItemsToBuyFromSingleCategory
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x0006C7C0 File Offset: 0x0006A9C0
		public override float GetEliteCaravanSpawnChance(Hero hero)
		{
			float num = 0f;
			if (hero.Power >= 112f)
			{
				num = hero.Power * 0.0045f - 0.5f;
			}
			return num;
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x0006C7F4 File Offset: 0x0006A9F4
		public override int GetPowerChangeAfterCaravanCreation(Hero hero, MobileParty caravanParty)
		{
			if (hero.Power >= 50f)
			{
				return -30;
			}
			return 0;
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x0006C808 File Offset: 0x0006AA08
		public override bool CanHeroCreateCaravan(Hero hero)
		{
			if (hero.IsMerchant && hero.PartyBelongedTo == null)
			{
				if (hero.OwnedCaravans.Count<CaravanPartyComponent>((CaravanPartyComponent x) => !x.MobileParty.Ai.IsDisabled) == 0 && hero.IsActive && !hero.IsTemplate)
				{
					return hero.CanLeadParty();
				}
			}
			return false;
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x0006C86C File Offset: 0x0006AA6C
		public override int GetCaravanFormingCost(bool largerCaravan, bool navalCaravan)
		{
			int num = (largerCaravan ? 22500 : 15000);
			if (CharacterObject.PlayerCharacter.Culture.HasFeat(DefaultCulturalFeats.AseraiTraderFeat))
			{
				return MathF.Round((float)num * DefaultCulturalFeats.AseraiTraderFeat.EffectBonus);
			}
			return num;
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x0006C8B4 File Offset: 0x0006AAB4
		public override int GetInitialTradeGold(Hero owner, bool navalCaravan, bool largeCaravan)
		{
			int num = 10000;
			int num2 = ((owner == Hero.MainHero) ? 5000 : 0);
			if (largeCaravan)
			{
				num = 17500;
			}
			return num + num2;
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x0006C8E4 File Offset: 0x0006AAE4
		public override int GetMaxGoldToSpendOnOneItemCategory(MobileParty caravan, ItemCategory itemCategory)
		{
			return 1500;
		}
	}
}
