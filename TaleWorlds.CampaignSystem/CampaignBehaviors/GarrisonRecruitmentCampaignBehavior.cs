using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000411 RID: 1041
	public class GarrisonRecruitmentCampaignBehavior : CampaignBehaviorBase, IGarrisonRecruitmentBehavior
	{
		// Token: 0x06004248 RID: 16968 RVA: 0x0012D511 File Offset: 0x0012B711
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004249 RID: 16969 RVA: 0x0012D513 File Offset: 0x0012B713
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.OnDailySettlementTick));
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x0012D52C File Offset: 0x0012B72C
		private static CharacterObject GetBasicTroopForTown(Town town)
		{
			return town.MapFaction.BasicTroop;
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x0012D53C File Offset: 0x0012B73C
		private void OnDailySettlementTick(Settlement settlement)
		{
			if (settlement.IsFortification)
			{
				Town town = settlement.Town;
				if (settlement.Party.MapEvent == null && settlement.Party.SiegeEvent == null)
				{
					this.TickGarrisonChangeForTown(town);
					if (this.CanSettlementAutoRecruit(settlement))
					{
						this.TickAutoRecruitmentGarrisonChange(town);
					}
				}
				if (town.GarrisonParty != null)
				{
					this.HandleGarrisonXpChange(town);
				}
			}
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x0012D598 File Offset: 0x0012B798
		private void TickAutoRecruitmentGarrisonChange(Town town)
		{
			float resultNumber = this.GetAutoRecruitmentGarrisonChangeExplainedNumber(town, false).ResultNumber;
			if (resultNumber > 0f)
			{
				if (town.GarrisonParty == null)
				{
					town.Owner.Settlement.AddGarrisonParty();
				}
				int num = 0;
				while ((float)num < resultNumber)
				{
					GarrisonRecruitmentCampaignBehavior.VolunteerTroop volunteerTroop = this._volunteerListCache.ElementAt<GarrisonRecruitmentCampaignBehavior.VolunteerTroop>(num);
					Hero ownerNotable = volunteerTroop.OwnerNotable;
					int notableVolunteerArrayIndex = volunteerTroop.NotableVolunteerArrayIndex;
					town.GarrisonParty.MemberRoster.AddToCounts(ownerNotable.VolunteerTypes[notableVolunteerArrayIndex], 1, false, 0, 0, true, -1);
					town.Settlement.OwnerClan.AutoRecruitmentExpenses += Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(ownerNotable.VolunteerTypes[notableVolunteerArrayIndex], town.Settlement.OwnerClan.Leader, false).RoundedResultNumber;
					ownerNotable.VolunteerTypes[notableVolunteerArrayIndex] = null;
					num++;
				}
			}
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x0012D680 File Offset: 0x0012B880
		private void TickGarrisonChangeForTown(Town town)
		{
			int num = (int)this.GetBaseGarrisonChangeExplainedNumber(town, false).ResultNumber;
			if (num > 0)
			{
				if (town.GarrisonParty == null)
				{
					town.Owner.Settlement.AddGarrisonParty();
				}
				town.GarrisonParty.MemberRoster.AddToCounts(GarrisonRecruitmentCampaignBehavior.GetBasicTroopForTown(town), num, false, 0, 0, true, -1);
			}
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x0012D6D8 File Offset: 0x0012B8D8
		private void HandleGarrisonXpChange(Town town)
		{
			int num = Campaign.Current.Models.DailyTroopXpBonusModel.CalculateDailyTroopXpBonus(town);
			if (num > 0)
			{
				float num2 = Campaign.Current.Models.DailyTroopXpBonusModel.CalculateGarrisonXpBonusMultiplier(town);
				foreach (TroopRosterElement troopRosterElement in town.GarrisonParty.MemberRoster.GetTroopRoster())
				{
					town.GarrisonParty.MemberRoster.AddXpToTroop(troopRosterElement.Character, MathF.Round((float)num * num2 * (float)troopRosterElement.Number));
				}
			}
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x0012D788 File Offset: 0x0012B988
		private void RepopulateVolunteerListCache(Town town)
		{
			this._volunteerListCache.Clear();
			List<Hero> list = new List<Hero>();
			foreach (Hero hero in town.Settlement.Notables)
			{
				if (hero.IsAlive)
				{
					list.Add(hero);
				}
			}
			foreach (Village village in town.Settlement.BoundVillages)
			{
				if (village.VillageState == Village.VillageStates.Normal)
				{
					foreach (Hero hero2 in village.Settlement.Notables)
					{
						if (hero2.IsAlive)
						{
							list.Add(hero2);
						}
					}
				}
			}
			foreach (Hero hero3 in list)
			{
				int num = Campaign.Current.Models.VolunteerModel.MaximumIndexGarrisonCanRecruitFromHero(town.Settlement, hero3);
				for (int i = 0; i < num; i++)
				{
					if (hero3.VolunteerTypes[i] != null)
					{
						GarrisonRecruitmentCampaignBehavior.VolunteerTroop volunteerTroop = new GarrisonRecruitmentCampaignBehavior.VolunteerTroop(hero3, i);
						this._volunteerListCache.Add(volunteerTroop);
					}
				}
			}
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x0012D924 File Offset: 0x0012BB24
		private ExplainedNumber GetAutoRecruitmentGarrisonChangeExplainedNumber(Town town, bool includeDescriptions)
		{
			ExplainedNumber maximumDailyAutoRecruitmentCount = Campaign.Current.Models.SettlementGarrisonModel.GetMaximumDailyAutoRecruitmentCount(town, includeDescriptions);
			this.RepopulateVolunteerListCache(town);
			if ((float)this._volunteerListCache.Count < maximumDailyAutoRecruitmentCount.LimitMaxValue)
			{
				maximumDailyAutoRecruitmentCount.LimitMax((float)this._volunteerListCache.Count, new TextObject("{=H1hi1kfF}Max Available Units", null));
			}
			MobileParty garrisonParty = town.GarrisonParty;
			int num = ((garrisonParty != null) ? garrisonParty.GetAvailableWageBudget() : town.Settlement.GarrisonWagePaymentLimit);
			int num2 = 0;
			int num3 = 0;
			foreach (GarrisonRecruitmentCampaignBehavior.VolunteerTroop volunteerTroop in this._volunteerListCache)
			{
				num2 += volunteerTroop.Wage;
				if (num2 >= num)
				{
					break;
				}
				num3++;
			}
			if ((float)num3 < maximumDailyAutoRecruitmentCount.LimitMaxValue)
			{
				maximumDailyAutoRecruitmentCount.LimitMax((float)num3, new TextObject("{=7GJOWuUO}Wage Limit", null));
			}
			int num4 = ((town.GarrisonParty == null) ? ((int)Campaign.Current.Models.PartySizeLimitModel.CalculateGarrisonPartySizeLimit(town.Settlement, false).ResultNumber) : (town.GarrisonParty.Party.PartySizeLimit - town.GarrisonParty.Party.NumberOfAllMembers));
			if ((float)num4 < maximumDailyAutoRecruitmentCount.LimitMaxValue)
			{
				maximumDailyAutoRecruitmentCount.LimitMax((float)num4, new TextObject("{=mp68RYnD}Party Size Limit", null));
			}
			return maximumDailyAutoRecruitmentCount;
		}

		// Token: 0x06004251 RID: 16977 RVA: 0x0012DA8C File Offset: 0x0012BC8C
		private ExplainedNumber GetBaseGarrisonChangeExplainedNumber(Town town, bool includeDescriptions)
		{
			ExplainedNumber explainedNumber = Campaign.Current.Models.SettlementGarrisonModel.CalculateBaseGarrisonChange(town.Settlement, includeDescriptions);
			int num = ((town.GarrisonParty == null) ? ((int)Campaign.Current.Models.PartySizeLimitModel.CalculateGarrisonPartySizeLimit(town.Settlement, false).ResultNumber) : (town.GarrisonParty.Party.PartySizeLimit - town.GarrisonParty.Party.NumberOfAllMembers));
			if (explainedNumber.LimitMaxValue > (float)num)
			{
				explainedNumber.LimitMax((float)num, new TextObject("{=mp68RYnD}Party Size Limit", null));
			}
			int characterWage = Campaign.Current.Models.PartyWageModel.GetCharacterWage(GarrisonRecruitmentCampaignBehavior.GetBasicTroopForTown(town));
			MobileParty garrisonParty = town.GarrisonParty;
			int num2 = ((garrisonParty != null) ? garrisonParty.GetAvailableWageBudget() : town.Settlement.GarrisonWagePaymentLimit) / characterWage;
			if (explainedNumber.LimitMaxValue > (float)num2)
			{
				explainedNumber.LimitMax((float)num2, new TextObject("{=7GJOWuUO}Wage Limit", null));
			}
			return explainedNumber;
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x0012DB80 File Offset: 0x0012BD80
		public ExplainedNumber GetGarrisonChangeExplainedNumber(Town town)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, true, null);
			ExplainedNumber baseGarrisonChangeExplainedNumber = this.GetBaseGarrisonChangeExplainedNumber(town, true);
			explainedNumber.AddFromExplainedNumber(baseGarrisonChangeExplainedNumber, new TextObject("{=basevalue}Base", null));
			if (this.CanSettlementAutoRecruit(town.Settlement))
			{
				ExplainedNumber autoRecruitmentGarrisonChangeExplainedNumber = this.GetAutoRecruitmentGarrisonChangeExplainedNumber(town, true);
				explainedNumber.AddFromExplainedNumber(autoRecruitmentGarrisonChangeExplainedNumber, new TextObject("{=Uzsnek6O}Auto Recruitment", null));
			}
			return explainedNumber;
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x0012DBE2 File Offset: 0x0012BDE2
		private bool CanSettlementAutoRecruit(Settlement settlement)
		{
			return settlement.Town.GarrisonAutoRecruitmentIsEnabled && settlement.Town.FoodChange > 0f;
		}

		// Token: 0x040013D2 RID: 5074
		private readonly SortedSet<GarrisonRecruitmentCampaignBehavior.VolunteerTroop> _volunteerListCache = new SortedSet<GarrisonRecruitmentCampaignBehavior.VolunteerTroop>();

		// Token: 0x0200084B RID: 2123
		private struct VolunteerTroop : IComparable
		{
			// Token: 0x060067E3 RID: 26595 RVA: 0x001D3645 File Offset: 0x001D1845
			public VolunteerTroop(Hero ownerNotable, int notableVolunteerArrayIndex)
			{
				this.OwnerNotable = ownerNotable;
				this.NotableVolunteerArrayIndex = notableVolunteerArrayIndex;
				this.Wage = Campaign.Current.Models.PartyWageModel.GetCharacterWage(ownerNotable.VolunteerTypes[notableVolunteerArrayIndex]);
			}

			// Token: 0x060067E4 RID: 26596 RVA: 0x001D3678 File Offset: 0x001D1878
			public int CompareTo(object obj)
			{
				GarrisonRecruitmentCampaignBehavior.VolunteerTroop volunteerTroop = (GarrisonRecruitmentCampaignBehavior.VolunteerTroop)obj;
				int num = this.Wage.CompareTo(volunteerTroop.Wage);
				if (num == 0)
				{
					num = volunteerTroop.NotableVolunteerArrayIndex.CompareTo(this.NotableVolunteerArrayIndex);
				}
				if (num == 0)
				{
					num = volunteerTroop.OwnerNotable.Id.CompareTo(this.OwnerNotable.Id);
				}
				return num;
			}

			// Token: 0x040021DD RID: 8669
			public Hero OwnerNotable;

			// Token: 0x040021DE RID: 8670
			public int NotableVolunteerArrayIndex;

			// Token: 0x040021DF RID: 8671
			public int Wage;
		}
	}
}
