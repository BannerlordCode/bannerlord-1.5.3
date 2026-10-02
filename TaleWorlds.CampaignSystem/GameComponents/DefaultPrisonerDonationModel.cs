using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200014A RID: 330
	public class DefaultPrisonerDonationModel : PrisonerDonationModel
	{
		// Token: 0x06001A41 RID: 6721 RVA: 0x00084568 File Offset: 0x00082768
		public override float CalculateRelationGainAfterHeroPrisonerDonate(PartyBase donatingParty, Hero donatedHero, Settlement donatedSettlement)
		{
			float num = 0f;
			int num2 = Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(donatedHero.CharacterObject, donatingParty.LeaderHero);
			int relation = donatedHero.GetRelation(donatedSettlement.OwnerClan.Leader);
			if (relation <= 0)
			{
				float num3 = 1f - (float)relation / 200f;
				if (donatedHero.IsKingdomLeader)
				{
					num = MathF.Min(40f, MathF.Pow((float)num2, 0.5f) * 0.5f) * num3;
				}
				else if (donatedHero.Clan.Leader == donatedHero)
				{
					num = MathF.Min(30f, MathF.Pow((float)num2, 0.5f) * 0.25f) * num3;
				}
				else
				{
					num = MathF.Min(20f, MathF.Pow((float)num2, 0.5f) * 0.1f) * num3;
				}
			}
			return num;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x0008463C File Offset: 0x0008283C
		public override float CalculateInfluenceGainAfterPrisonerDonation(PartyBase donatingParty, CharacterObject donatedPrisoner, Settlement donatedSettlement)
		{
			return MathF.Pow((float)Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(donatedPrisoner, donatingParty.LeaderHero), 0.4f) * 0.2f;
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x0008466C File Offset: 0x0008286C
		public override float CalculateInfluenceGainAfterTroopDonation(PartyBase donatingParty, CharacterObject donatedCharacter, Settlement donatedSettlement)
		{
			Hero leaderHero = donatingParty.LeaderHero;
			ExplainedNumber explainedNumber = new ExplainedNumber(donatedCharacter.GetPower() / 3f, false, null);
			if (leaderHero != null && leaderHero.GetPerkValue(DefaultPerks.Steward.Relocation))
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.Relocation, donatingParty.MobileParty, true, ref explainedNumber);
			}
			return explainedNumber.ResultNumber;
		}
	}
}
