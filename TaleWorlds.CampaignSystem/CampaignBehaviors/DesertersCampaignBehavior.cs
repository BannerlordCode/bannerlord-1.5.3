using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000403 RID: 1027
	public class DesertersCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06004077 RID: 16503 RVA: 0x0011AA42 File Offset: 0x00118C42
		public static int MergePartiesMaxSize
		{
			get
			{
				return 120;
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06004078 RID: 16504 RVA: 0x0011AA46 File Offset: 0x00118C46
		private float DesertersSpawnRadiusAroundVillages
		{
			get
			{
				return 0.2f * Campaign.Current.EstimatedAverageBanditPartySpeed * (float)CampaignTime.HoursInDay;
			}
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06004079 RID: 16505 RVA: 0x0011AA5F File Offset: 0x00118C5F
		private Clan DeserterClan
		{
			get
			{
				if (this._deserterClan == null)
				{
					this._deserterClan = Clan.FindFirst((Clan x) => x.StringId == "deserters");
				}
				return this._deserterClan;
			}
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x0011AA99 File Offset: 0x00118C99
		public override void RegisterEvents()
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.MapEventEnded));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.HourlyTickParty));
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x0011AACC File Offset: 0x00118CCC
		private void HourlyTickParty(MobileParty party)
		{
			if (this.IsDeserterParty(party) && this.CanPartyMerge(party) && party.MemberRoster.TotalRegulars < DesertersCampaignBehavior.MergePartiesMaxSize)
			{
				LocatableSearchData<MobileParty> locatableSearchData = MobileParty.StartFindingLocatablesAroundPosition(party.Position.ToVec2(), this.GetMergeDistance(party));
				for (MobileParty mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData); mobileParty != null; mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData))
				{
					if (this.IsDeserterParty(mobileParty) && mobileParty != party && this.CanPartyMerge(mobileParty) && mobileParty.MemberRoster.TotalRegulars + party.MemberRoster.TotalRegulars <= DesertersCampaignBehavior.MergePartiesMaxSize && MBRandom.RandomFloat < 0.05f)
					{
						this.MergeParties(party, mobileParty);
						return;
					}
				}
			}
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x0011AB7C File Offset: 0x00118D7C
		private bool CanPartyMerge(MobileParty mobileParty)
		{
			return mobileParty.IsActive && mobileParty.MapEvent == null && !mobileParty.IsCurrentlyUsedByAQuest && !mobileParty.IsCurrentlyEngagingParty && !mobileParty.IsFleeing();
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x0011ABAC File Offset: 0x00118DAC
		private void MergeParties(MobileParty party, MobileParty nearbyParty)
		{
			Debug.Print(string.Format("Deserter parties {0} of {1} and {2} of {3} merged.", new object[]
			{
				party.StringId,
				party.MemberRoster.TotalManCount,
				nearbyParty.StringId,
				nearbyParty.MemberRoster.TotalManCount
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			party.MemberRoster.Add(nearbyParty.MemberRoster);
			foreach (TroopRosterElement troopRosterElement in nearbyParty.PrisonRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.HeroObject != null)
				{
					TransferPrisonerAction.Apply(troopRosterElement.Character, nearbyParty.Party, party.Party);
				}
			}
			if (party.PrisonRoster.Count > 0)
			{
				party.PrisonRoster.Add(nearbyParty.PrisonRoster);
			}
			party.PartyTradeGold += nearbyParty.PartyTradeGold;
			party.ItemRoster.Add(nearbyParty.ItemRoster);
			DestroyPartyAction.Apply(null, nearbyParty);
			PartyBaseHelper.SortRoster(party);
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x0011ACDC File Offset: 0x00118EDC
		private void MapEventEnded(MapEvent mapEvent)
		{
			if (!mapEvent.IsNavalMapEvent && (mapEvent.IsFieldBattle || mapEvent.IsSiegeAssault || mapEvent.IsSiegeOutside || mapEvent.IsSallyOut) && mapEvent.HasWinner && this.DeserterClan != null && this.DeserterClan.WarPartyComponents.Count < Campaign.Current.Models.BanditDensityModel.GetMaxSupportedNumberOfLootersForClan(this.DeserterClan))
			{
				MapEventSide mapEventSide = mapEvent.GetMapEventSide(mapEvent.DefeatedSide);
				TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
				foreach (MapEventParty mapEventParty in mapEventSide.Parties)
				{
					if (this.CanPartyGenerateDeserters(mapEventParty))
					{
						troopRoster.Add(mapEventParty.RoutedInBattle);
						troopRoster.Add(mapEventParty.DiedInBattle);
					}
				}
				if (MBRandom.RandomFloat < 0.9f)
				{
					troopRoster.RemoveIf((TroopRosterElement x) => x.Character.IsHero);
					if (troopRoster.TotalManCount >= 15)
					{
						this.TrySpawnDeserters(mapEvent, troopRoster);
					}
				}
			}
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x0011AE14 File Offset: 0x00119014
		private bool CanPartyGenerateDeserters(MapEventParty mapEventParty)
		{
			return mapEventParty.Party.IsMobile && mapEventParty.Party.MobileParty.IsLordParty && mapEventParty.Party.MobileParty.ActualClan != null && !mapEventParty.Party.MobileParty.ActualClan.IsMinorFaction;
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x0011AE6C File Offset: 0x0011906C
		private void TrySpawnDeserters(MapEvent mapEvent, TroopRoster routedTroops)
		{
			int maxDeserterPartyCountForMapEvent = this.GetMaxDeserterPartyCountForMapEvent(mapEvent);
			List<TroopRoster> rostersSuitableForDeserters = this.GetRostersSuitableForDeserters(routedTroops, maxDeserterPartyCountForMapEvent);
			List<Settlement> list = this.SelectRandomSettlementsForDeserters(mapEvent, rostersSuitableForDeserters.Count);
			for (int i = 0; i < rostersSuitableForDeserters.Count; i++)
			{
				this.SpawnDesertersParty(mapEvent, rostersSuitableForDeserters[i], list[i]);
			}
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x0011AEC0 File Offset: 0x001190C0
		private int GetMaxDeserterPartyCountForMapEvent(MapEvent mapEvent)
		{
			bool flag = mapEvent.AttackerSide.Parties.Any<MapEventParty>((MapEventParty x) => this.CanPartyGenerateDeserters(x) && x.Party.MobileParty.Army != null && (x.Party.MobileParty.AttachedTo != null || x.Party.MobileParty.Army.LeaderParty == x.Party.MobileParty));
			bool flag2 = mapEvent.DefenderSide.Parties.Any<MapEventParty>((MapEventParty x) => this.CanPartyGenerateDeserters(x) && x.Party.MobileParty.Army != null && (x.Party.MobileParty.AttachedTo != null || x.Party.MobileParty.Army.LeaderParty == x.Party.MobileParty));
			if (flag && flag2)
			{
				return 5;
			}
			return 3;
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x0011AF10 File Offset: 0x00119110
		private List<TroopRoster> GetRostersSuitableForDeserters(TroopRoster routedTroops, int maxPartyCount)
		{
			int totalManCount = routedTroops.TotalManCount;
			int maxSupportedNumberOfLootersForClan = Campaign.Current.Models.BanditDensityModel.GetMaxSupportedNumberOfLootersForClan(this.DeserterClan);
			int num = Math.Min(maxPartyCount, maxSupportedNumberOfLootersForClan - this.DeserterClan.WarPartyComponents.Count);
			int num2 = totalManCount / 15;
			int num3 = Math.Min(num, num2);
			List<TroopRoster> list = new List<TroopRoster>();
			for (int i = 0; i < num3; i++)
			{
				list.Add(routedTroops.RemoveNumberOfNonHeroTroopsRandomly(Math.Min(routedTroops.TotalManCount / (num3 - i), 40)));
			}
			return list;
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x0011AF9B File Offset: 0x0011919B
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x0011AFA0 File Offset: 0x001191A0
		private void SpawnDesertersParty(MapEvent mapEvent, TroopRoster troops, Settlement settlement)
		{
			CampaignVec2 deserterSpawnPosition = this.GetDeserterSpawnPosition(settlement);
			MobileParty mobileParty = BanditPartyComponent.CreateLooterParty(this.DeserterClan.StringId + "_1", this.DeserterClan, settlement, false, null, deserterSpawnPosition);
			mobileParty.MemberRoster.Add(troops);
			this.InitializeDeserterParty(mobileParty);
			mobileParty.SetMovePatrolAroundPoint(mobileParty.Position, MobileParty.NavigationType.Default);
			PartyBaseHelper.SortRoster(mobileParty);
		}

		// Token: 0x06004085 RID: 16517 RVA: 0x0011B000 File Offset: 0x00119200
		private List<Settlement> SelectRandomSettlementsForDeserters(MapEvent mapEvent, int count)
		{
			CampaignVec2 campaignVec = mapEvent.Position;
			List<Settlement> list = DesertersCampaignBehavior.FindSettlementsAroundPoint(in campaignVec, (Settlement x) => x.IsVillage, MobileParty.NavigationType.Default, this.GetMaxVillageDistance());
			if (list.Count > count)
			{
				list.Shuffle<Settlement>();
				return list.Take<Settlement>(count).ToList<Settlement>();
			}
			if (list.Count == 0)
			{
				List<Settlement> list2 = list;
				campaignVec = mapEvent.Position;
				list2.Add(SettlementHelper.FindNearestSettlementToPoint(in campaignVec, (Settlement x) => x.IsVillage));
			}
			int count2 = list.Count;
			for (int i = 0; i < count - count2; i++)
			{
				list.Add(list[MBRandom.RandomInt(0, count2 - 1)]);
			}
			return list;
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x0011B0C4 File Offset: 0x001192C4
		private static List<Settlement> FindSettlementsAroundPoint(in CampaignVec2 point, Func<Settlement, bool> condition, MobileParty.NavigationType navCapabilities, float maxDistance)
		{
			List<Settlement> list = new List<Settlement>();
			foreach (Settlement settlement in Settlement.All)
			{
				if ((condition == null || condition(settlement)) && settlement.Position.Distance(point) < maxDistance)
				{
					list.Add(settlement);
				}
			}
			return list;
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x0011B140 File Offset: 0x00119340
		private float GetMaxVillageDistance()
		{
			return Campaign.Current.EstimatedAverageBanditPartySpeed * (float)CampaignTime.HoursInDay / 2f;
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x0011B15C File Offset: 0x0011935C
		private CampaignVec2 GetDeserterSpawnPosition(Settlement settlement)
		{
			CampaignVec2 campaignVec = NavigationHelper.FindPointAroundPosition(settlement.GatePosition, MobileParty.NavigationType.Default, this.DesertersSpawnRadiusAroundVillages, 0f, true, false);
			float seeingRange = MobileParty.MainParty.SeeingRange;
			float num = seeingRange * seeingRange;
			if (campaignVec.DistanceSquared(MobileParty.MainParty.Position) < num)
			{
				for (int i = 0; i < 15; i++)
				{
					CampaignVec2 campaignVec2 = NavigationHelper.FindReachablePointAroundPosition(campaignVec, MobileParty.NavigationType.Default, this.DesertersSpawnRadiusAroundVillages, 0f, false);
					if (NavigationHelper.IsPositionValidForNavigationType(campaignVec2, MobileParty.NavigationType.Default))
					{
						float num3;
						float num2 = DistanceHelper.FindClosestDistanceFromMobilePartyToPoint(MobileParty.MainParty, campaignVec2, MobileParty.NavigationType.Default, out num3);
						if (num2 * num2 > num)
						{
							campaignVec = campaignVec2;
							break;
						}
					}
				}
			}
			return campaignVec;
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x0011B1E6 File Offset: 0x001193E6
		private void InitializeDeserterParty(MobileParty banditParty)
		{
			banditParty.Party.SetVisualAsDirty();
			banditParty.ActualClan = this.DeserterClan;
			banditParty.Aggressiveness = 1f - 0.2f * MBRandom.RandomFloat;
			DesertersCampaignBehavior.CreatePartyTrade(banditParty);
			this.GiveFoodToBanditParty(banditParty);
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x0011B224 File Offset: 0x00119424
		private static void CreatePartyTrade(MobileParty banditParty)
		{
			int num = (int)(10f * (float)banditParty.Party.MemberRoster.TotalManCount * (0.5f + 1f * MBRandom.RandomFloat));
			banditParty.InitializePartyTrade(num);
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x0011B264 File Offset: 0x00119464
		private void GiveFoodToBanditParty(MobileParty banditParty)
		{
			foreach (ItemObject itemObject in Items.All)
			{
				if (itemObject.IsFood)
				{
					int num = MBRandom.RoundRandomized((float)banditParty.MemberRoster.TotalManCount * (1f / (float)itemObject.Value) * 8f * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat);
					if (num > 0)
					{
						banditParty.ItemRoster.AddToCounts(itemObject, num);
					}
				}
			}
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x0011B308 File Offset: 0x00119508
		private float GetMergeDistance(MobileParty mobileParty)
		{
			return mobileParty._lastCalculatedSpeed * 2f;
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x0011B316 File Offset: 0x00119516
		private bool IsDeserterParty(MobileParty mobileParty)
		{
			return mobileParty.ActualClan != null && mobileParty.ActualClan == this.DeserterClan;
		}

		// Token: 0x04001399 RID: 5017
		public const int MinimumDeserterPartyCount = 15;

		// Token: 0x0400139A RID: 5018
		public const int MaximumDeserterPartyCount = 40;

		// Token: 0x0400139B RID: 5019
		private const int MaxDeserterPartyCountAfterBattle = 3;

		// Token: 0x0400139C RID: 5020
		private const int MaxDeserterPartyCountAfterArmyBattle = 5;

		// Token: 0x0400139D RID: 5021
		private Clan _deserterClan;
	}
}
