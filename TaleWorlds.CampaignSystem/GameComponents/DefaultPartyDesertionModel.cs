using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000138 RID: 312
	public class DefaultPartyDesertionModel : PartyDesertionModel
	{
		// Token: 0x060019A3 RID: 6563 RVA: 0x0007F62B File Offset: 0x0007D82B
		public override int GetMoraleThresholdForTroopDesertion()
		{
			return 10;
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0007F62F File Offset: 0x0007D82F
		public override float GetDesertionChanceForTroop(MobileParty mobileParty, in TroopRosterElement troopRosterElement)
		{
			return this.CalculateDesertionChanceFromTroopLevel(mobileParty.Morale, troopRosterElement.Character.Level);
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0007F648 File Offset: 0x0007D848
		private float CalculateDesertionChanceFromTroopLevel(float partyMorale, int level)
		{
			int moraleThresholdForTroopDesertion = Campaign.Current.Models.PartyDesertionModel.GetMoraleThresholdForTroopDesertion();
			float num = ((partyMorale > (float)moraleThresholdForTroopDesertion) ? ((float)moraleThresholdForTroopDesertion) : partyMorale);
			return 1f - MathF.Pow((float)level * 0.01f, 0.1f * (((float)moraleThresholdForTroopDesertion - num) / (float)moraleThresholdForTroopDesertion));
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0007F698 File Offset: 0x0007D898
		public override TroopRoster GetTroopsToDesert(MobileParty mobileParty)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			this.GetTroopsToDesertDueToMorale(mobileParty, troopRoster);
			this.GetTroopsToDesertDueToWageAndPartySize(mobileParty, troopRoster);
			return troopRoster;
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x0007F6BC File Offset: 0x0007D8BC
		private void GetTroopsToDesertDueToMorale(MobileParty mobileParty, TroopRoster troopsToDesert)
		{
			int num = (int)((float)mobileParty.Party.NumberOfRegularMembers * this.CalculateDesertionChanceFromTroopLevel(mobileParty.Morale, 20));
			if (num <= 0)
			{
				return;
			}
			this.SelectTroopsForDesertion(mobileParty, troopsToDesert, num, true);
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0007F6F8 File Offset: 0x0007D8F8
		private void GetTroopsToDesertDueToWageAndPartySize(MobileParty mobileParty, TroopRoster troopsToDesert)
		{
			int num = 0;
			int num2 = 0;
			int num3 = mobileParty.Party.NumberOfAllMembers - troopsToDesert.TotalManCount - mobileParty.Party.PartySizeLimit;
			float resultNumber = Campaign.Current.Models.PartyWageModel.GetTotalWage(mobileParty, troopsToDesert, false).ResultNumber;
			float num4 = (float)mobileParty.TotalWage - resultNumber;
			if (mobileParty.HasLimitedWage() && (float)mobileParty.PaymentLimit < num4)
			{
				int num5 = mobileParty.TotalWage - mobileParty.PaymentLimit;
				num = MathF.Min(20, MathF.Max(1, (int)((float)num5 / Campaign.Current.AverageWage * 0.25f)));
			}
			if (num3 > 0)
			{
				num2 = MathF.Max(1, (int)((float)num3 * 0.25f));
			}
			int num6 = MathF.Max(num, num2);
			if (mobileParty.IsGarrison && mobileParty.HasUnpaidWages > 0f)
			{
				num6 += MathF.Min(mobileParty.Party.NumberOfHealthyMembers, 5);
			}
			num6 = MathF.Min(num6, mobileParty.MemberRoster.TotalRegulars);
			if (num6 <= 0)
			{
				return;
			}
			this.SelectTroopsForDesertion(mobileParty, troopsToDesert, num6, false);
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0007F80C File Offset: 0x0007DA0C
		private void SelectTroopsForDesertion(MobileParty mobileParty, TroopRoster troopsToDesert, int maxDesertionCount, bool useProbability)
		{
			int num = 0;
			int num2 = mobileParty.MemberRoster.Count - 1;
			while (num2 >= 0 && num < maxDesertionCount)
			{
				TroopRosterElement elementCopyAtIndex = mobileParty.MemberRoster.GetElementCopyAtIndex(num2);
				if (elementCopyAtIndex.Character.HeroObject == null)
				{
					int num3 = 0;
					int num4 = 0;
					float num5 = (useProbability ? this.GetDesertionChanceForTroop(mobileParty, in elementCopyAtIndex) : 1f);
					int troopCount = troopsToDesert.GetTroopCount(elementCopyAtIndex.Character);
					int num6 = 0;
					while (num6 < elementCopyAtIndex.WoundedNumber - troopCount && num + num4 < maxDesertionCount)
					{
						if (!useProbability || num5 > mobileParty.RandomFloatWithSeed((uint)(CampaignTime.Now.ToHours + (double)(num2 * 100 + num6))))
						{
							num4++;
						}
						num6++;
					}
					int num7 = 0;
					while (num7 < elementCopyAtIndex.Number - elementCopyAtIndex.WoundedNumber - troopCount && num + num4 + num3 < maxDesertionCount)
					{
						if (!useProbability || num5 > mobileParty.RandomFloatWithSeed((uint)(CampaignTime.Now.ToHours + (double)(num2 * 100 + num7))))
						{
							num3++;
						}
						num7++;
					}
					if (num3 != 0 || num4 != 0)
					{
						int num8 = num3 + num4;
						troopsToDesert.AddToCounts(elementCopyAtIndex.Character, num8, false, num4, 0, true, -1);
						num += num8;
					}
				}
				num2--;
			}
		}

		// Token: 0x0400084F RID: 2127
		private const int MaxAcceptableDesertionCountForNormal = 20;

		// Token: 0x04000850 RID: 2128
		private const int MoraleThresholdForParty = 10;

		// Token: 0x04000851 RID: 2129
		private const int AverageTroopLevel = 20;
	}
}
