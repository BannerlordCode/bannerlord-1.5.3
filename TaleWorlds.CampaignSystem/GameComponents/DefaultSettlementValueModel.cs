using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200015A RID: 346
	public class DefaultSettlementValueModel : SettlementValueModel
	{
		// Token: 0x06001AF8 RID: 6904 RVA: 0x00088C94 File Offset: 0x00086E94
		private static float GetSettlementScoreForBeingHomeSettlementOfClan(Settlement settlement, Clan clan, float maxDistanceOfSettlementsToHomeSettlement)
		{
			float num = 0f;
			if (clan.IsRebelClan || clan.IsBanditFaction || clan.MapFaction.Settlements.Count == 0)
			{
				if (settlement == clan.InitialHomeSettlement)
				{
					num = float.MaxValue;
				}
				else
				{
					num = float.MinValue;
				}
			}
			else
			{
				if (settlement.SettlementComponent is Hideout || settlement.SettlementComponent is RetirementSettlementComponent)
				{
					return float.MinValue;
				}
				if (settlement.MapFaction.IsAtWarWith(clan.MapFaction))
				{
					num -= 10240f;
				}
				if (settlement.OwnerClan == clan)
				{
					num += 5120f;
				}
				if (settlement.MapFaction == clan.MapFaction)
				{
					num += 2560f;
				}
				if (settlement.IsVillage)
				{
					num += 320f;
				}
				else if (settlement.IsCastle)
				{
					num += 640f;
				}
				else if (settlement.IsTown)
				{
					num += 1280f;
				}
				if (settlement == clan.HomeSettlement)
				{
					num += 4.5f;
				}
				if (settlement == clan.InitialHomeSettlement)
				{
					num += 3.5f;
				}
				if (settlement.Culture == clan.Culture)
				{
					num += 17f;
				}
				Clan ownerClan = settlement.OwnerClan;
				if (((ownerClan != null) ? ownerClan.Culture : null) == clan.Culture)
				{
					num += 12f;
				}
				Settlement factionMidSettlement = clan.MapFaction.FactionMidSettlement;
				if (clan.MapFaction.Settlements.Count > 1 && settlement != factionMidSettlement)
				{
					float num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, factionMidSettlement, false, false, MobileParty.NavigationType.All);
					if (settlement.HasPort)
					{
						float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, factionMidSettlement, true, false, MobileParty.NavigationType.All);
						if (distance < num2)
						{
							num2 = distance;
						}
					}
					if (factionMidSettlement.HasPort)
					{
						float num3 = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, factionMidSettlement, false, true, MobileParty.NavigationType.All);
						if (num3 < num2)
						{
							num2 = num3;
						}
						if (settlement.HasPort)
						{
							num3 = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, factionMidSettlement, true, true, MobileParty.NavigationType.All);
							if (num3 < num2)
							{
								num2 = num3;
							}
						}
					}
					float num4 = 20f - MBMath.Map(num2, 0f, maxDistanceOfSettlementsToHomeSettlement, 0f, 20f);
					num += num4;
				}
				else
				{
					num += 20f;
				}
				int num5 = DefaultSettlementValueModel.CalculateTotalProsperity(settlement);
				float num6 = MBMath.Map(MathF.Sqrt(2500f + (float)num5) / 100f, 0.5f, 1f, 0f, 5f);
				num += num6;
				float num7 = MBMath.Map(SettlementHelper.GetNeighborScoreForConsideringClan(settlement, clan), -2f, 1f, -10f, 10f);
				num += num7;
				int num8 = 0;
				for (;;)
				{
					int num9 = num8;
					Kingdom kingdom = clan.Kingdom;
					int? num10 = ((kingdom != null) ? new int?(kingdom.Clans.Count) : null);
					if (!((num9 < num10.GetValueOrDefault()) & (num10 != null)))
					{
						break;
					}
					Clan clan2 = clan.Kingdom.Clans[num8];
					if (clan2 != clan && settlement == clan2.HomeSettlement)
					{
						num -= 10f;
					}
					num8++;
				}
			}
			return num;
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x00088FA4 File Offset: 0x000871A4
		public override Settlement FindMostSuitableHomeSettlement(Clan clan)
		{
			Settlement settlement = null;
			if (Settlement.All != null && Settlement.All.Count != 0 && !clan.IsRebelClan && !clan.IsBanditFaction && clan.MapFaction.Settlements.Count != 0)
			{
				float minValue = float.MinValue;
				float num = DefaultSettlementValueModel.FindFarthestDistanceBetweenSettlementsInClan(clan);
				DefaultSettlementValueModel.TryToFindHomeSettlementForClan(clan, clan.Fiefs.SelectQ<Town, Settlement>((Town x) => x.Settlement), num, out settlement, ref minValue);
				if (minValue < 5120f && clan.Kingdom != null)
				{
					DefaultSettlementValueModel.TryToFindHomeSettlementForClan(clan, clan.Kingdom.Fiefs.SelectQ<Town, Settlement>((Town x) => x.Settlement), num, out settlement, ref minValue);
				}
				if (minValue < 2560f)
				{
					DefaultSettlementValueModel.TryToFindHomeSettlementForClan(clan, Settlement.All, num, out settlement, ref minValue);
				}
				return settlement;
			}
			if (clan == Clan.PlayerClan && clan.InitialHomeSettlement == null)
			{
				return Settlement.All[0];
			}
			return clan.InitialHomeSettlement;
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x000890B0 File Offset: 0x000872B0
		private static void TryToFindHomeSettlementForClan(Clan clanToConsider, IEnumerable<Settlement> settlementsToConsider, float maxDistance, out Settlement homeSettlement, ref float maxScore)
		{
			homeSettlement = null;
			foreach (Settlement settlement in settlementsToConsider)
			{
				if (settlement.IsFortification || settlement.IsVillage || settlement.IsHideout)
				{
					float settlementScoreForBeingHomeSettlementOfClan = DefaultSettlementValueModel.GetSettlementScoreForBeingHomeSettlementOfClan(settlement, clanToConsider, maxDistance);
					if (settlementScoreForBeingHomeSettlementOfClan > maxScore)
					{
						homeSettlement = settlement;
						maxScore = settlementScoreForBeingHomeSettlementOfClan;
					}
				}
			}
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x00089124 File Offset: 0x00087324
		private static float FindFarthestDistanceBetweenSettlementsInClan(Clan clan)
		{
			float num = float.MinValue;
			foreach (Settlement settlement in clan.MapFaction.Settlements)
			{
				if (settlement != clan.MapFaction.FactionMidSettlement)
				{
					float num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(clan.MapFaction.FactionMidSettlement, settlement, false, false, MobileParty.NavigationType.All);
					if (num2 > num)
					{
						num = num2;
					}
					if (settlement.HasPort)
					{
						num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(clan.MapFaction.FactionMidSettlement, settlement, false, true, MobileParty.NavigationType.All);
						if (num2 > num)
						{
							num = num2;
						}
					}
					if (clan.MapFaction.FactionMidSettlement.HasPort)
					{
						num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(clan.MapFaction.FactionMidSettlement, settlement, true, false, MobileParty.NavigationType.All);
						if (num2 > num)
						{
							num = num2;
						}
						if (settlement.HasPort)
						{
							num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(clan.MapFaction.FactionMidSettlement, settlement, true, true, MobileParty.NavigationType.All);
							if (num2 > num)
							{
								num = num2;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x00089260 File Offset: 0x00087460
		private static int CalculateTotalProsperity(Settlement settlement)
		{
			int num = 0;
			if (settlement.IsFortification)
			{
				num = (int)settlement.Town.Prosperity;
				using (List<Village>.Enumerator enumerator = settlement.BoundVillages.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Village village = enumerator.Current;
						num += (int)village.Hearth;
					}
					return num;
				}
			}
			if (settlement.IsVillage)
			{
				num = (int)settlement.Village.Hearth;
			}
			return num;
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x000892E4 File Offset: 0x000874E4
		public override float CalculateSettlementBaseValue(Settlement settlement)
		{
			float num = (settlement.IsCastle ? 1.25f : 1f);
			float value = settlement.GetValue(null, true);
			float baseGeographicalAdvantage = DefaultSettlementValueModel.GetBaseGeographicalAdvantage(settlement.IsVillage ? settlement.Village.Bound : settlement);
			return num * value * baseGeographicalAdvantage * 0.33f;
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x00089334 File Offset: 0x00087534
		public override float CalculateSettlementValueForFaction(Settlement settlement, IFaction faction)
		{
			float num = (settlement.IsCastle ? 1.25f : 1f);
			float num2 = ((settlement.MapFaction == faction.MapFaction) ? 1.1f : 1f);
			float num3 = ((settlement.Culture == ((faction != null) ? faction.Culture : null)) ? 1.1f : 1f);
			float value = settlement.GetValue(null, true);
			float num4 = DefaultSettlementValueModel.GeographicalAdvantageForFaction(settlement.IsVillage ? settlement.Village.Bound : settlement, faction);
			float num5 = 1f;
			if (settlement.HasPort && settlement.IsFortification)
			{
				num5 = 1.2f;
				if (!faction.Settlements.Any<Settlement>((Settlement x) => x.HasPort))
				{
					num5 *= 1.4f;
				}
			}
			return value * num * num2 * num3 * num4 * num5 * 0.33f;
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x0008941C File Offset: 0x0008761C
		public override float CalculateSettlementValueForEnemyHero(Settlement settlement, Hero hero)
		{
			float num = (settlement.IsCastle ? 1.25f : 1f);
			float num2 = ((settlement.OwnerClan == hero.Clan) ? 1.1f : 1f);
			float num3 = ((settlement.Culture == hero.Culture) ? 1.1f : 1f);
			float value = settlement.GetValue(null, true);
			float num4 = DefaultSettlementValueModel.GeographicalAdvantageForFaction(settlement.IsVillage ? settlement.Village.Bound : settlement, hero.MapFaction);
			float num5 = 1f;
			if (settlement.HasPort && settlement.IsFortification)
			{
				num5 = 1.2f;
				if (!hero.Clan.Settlements.Any<Settlement>((Settlement x) => x.HasPort))
				{
					num5 *= 1.4f;
				}
			}
			return value * num * num3 * num2 * num4 * num5 * 0.33f;
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x00089508 File Offset: 0x00087708
		private static float GetBaseGeographicalAdvantage(Settlement settlement)
		{
			float num = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement.MapFaction.FactionMidSettlement, settlement, false, false, MobileParty.NavigationType.All) / Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All);
			return 1f / (1f + num);
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x00089554 File Offset: 0x00087754
		private static float GeographicalAdvantageForFaction(Settlement settlement, IFaction faction)
		{
			Settlement factionMidSettlement = faction.FactionMidSettlement;
			float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, factionMidSettlement, false, false, MobileParty.NavigationType.All);
			if (faction.FactionMidSettlement.MapFaction != faction)
			{
				return MathF.Clamp(Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All) / (distance + 0.1f), 0f, 4f);
			}
			float distanceToClosestNonAllyFortification = faction.DistanceToClosestNonAllyFortification;
			if (settlement.MapFaction == faction && distance < distanceToClosestNonAllyFortification)
			{
				return MathF.Clamp(Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All) / (distanceToClosestNonAllyFortification - distance), 1f, 4f);
			}
			float num = (distance - distanceToClosestNonAllyFortification) / Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All);
			return 1f / (1f + num);
		}

		// Token: 0x040008F1 RID: 2289
		private const float BenefitRatioForFaction = 0.33f;

		// Token: 0x040008F2 RID: 2290
		private const float CastleMultiplier = 1.25f;

		// Token: 0x040008F3 RID: 2291
		private const float SameMapFactionMultiplier = 1.1f;

		// Token: 0x040008F4 RID: 2292
		private const float SameCultureMultiplier = 1.1f;

		// Token: 0x040008F5 RID: 2293
		private const float BeingOwnerMultiplier = 1.1f;

		// Token: 0x040008F6 RID: 2294
		private const float HavingNoCoastalSettlementMultiplier = 1.4f;

		// Token: 0x040008F7 RID: 2295
		private const float HavingPortMultiplier = 1.2f;

		// Token: 0x040008F8 RID: 2296
		private const int SettlementAtWarWithClan = 10240;

		// Token: 0x040008F9 RID: 2297
		private const int HomeSettlementToOtherClanScore = 10;

		// Token: 0x040008FA RID: 2298
		private const int AlreadyOwnerClanScoreForHomeSettlement = 5120;

		// Token: 0x040008FB RID: 2299
		private const int SameFactionWithClanScoreForHomeSettlement = 2560;

		// Token: 0x040008FC RID: 2300
		private const int SettlementTypeScoreForHomeSettlementTown = 1280;

		// Token: 0x040008FD RID: 2301
		private const int SettlementTypeScoreForHomeSettlementCastle = 640;

		// Token: 0x040008FE RID: 2302
		private const int SettlementTypeScoreForHomeSettlementVillage = 320;

		// Token: 0x040008FF RID: 2303
		private const int MidSettlementDistanceScoreForHomeSettlement = 20;

		// Token: 0x04000900 RID: 2304
		private const int SameCultureWithClanCultureScoreForHomeSettlement = 17;

		// Token: 0x04000901 RID: 2305
		private const float AlreadyHomeSettlementScoreForHomeSettlement = 4.5f;

		// Token: 0x04000902 RID: 2306
		private const float InitialHomeSettlementScoreForHomeSettlement = 3.5f;

		// Token: 0x04000903 RID: 2307
		private const int SettlementOwnerClanCultureSameForHomeSettlement = 12;

		// Token: 0x04000904 RID: 2308
		private const int NeighborScoreForHomeSettlement = 10;

		// Token: 0x04000905 RID: 2309
		private const int ProsperityScoreForHomeSettlement = 5;
	}
}
