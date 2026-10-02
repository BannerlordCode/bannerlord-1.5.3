using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace Helpers
{
	// Token: 0x0200002A RID: 42
	public static class ShipHelper
	{
		// Token: 0x06000188 RID: 392 RVA: 0x00011C40 File Offset: 0x0000FE40
		public static bool TryGetShipBanner(IShipOrigin shipOrigin, out Banner banner, IAgent captain = null)
		{
			banner = Banner.CreateOneColoredEmptyBanner(92);
			CharacterObject characterObject;
			if ((characterObject = ((captain != null) ? captain.Character : null) as CharacterObject) != null && characterObject.IsHero)
			{
				banner = characterObject.HeroObject.ClanBanner;
				return true;
			}
			Ship ship;
			if ((ship = shipOrigin as Ship) != null && ship.Owner != null)
			{
				if (ship.Owner.IsMobile && ship.Owner.MobileParty.Army != null)
				{
					banner = ship.Owner.MobileParty.Army.LeaderParty.MapFaction.Banner;
				}
				else
				{
					banner = ship.Owner.Banner;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00011CE8 File Offset: 0x0000FEE8
		public static bool TryGetSailColors(IShipOrigin shipOrigin, [TupleElementNames(new string[] { "sailColor1", "sailColor2" })] out ValueTuple<uint, uint> sailColors, IAgent captain = null)
		{
			sailColors = new ValueTuple<uint, uint>(4291609515U, 4291609515U);
			CharacterObject characterObject;
			if ((characterObject = ((captain != null) ? captain.Character : null) as CharacterObject) != null && characterObject.IsHero)
			{
				sailColors.Item1 = characterObject.HeroObject.MapFaction.Color;
				sailColors.Item2 = characterObject.HeroObject.MapFaction.Color2;
				return true;
			}
			Ship ship;
			if ((ship = shipOrigin as Ship) != null && ship.Owner != null)
			{
				if (ship.Owner.IsMobile && ship.Owner.MobileParty.Army != null)
				{
					sailColors.Item1 = ship.Owner.MobileParty.Army.LeaderParty.MapFaction.Color;
					sailColors.Item2 = ship.Owner.MobileParty.Army.LeaderParty.MapFaction.Color2;
				}
				else
				{
					sailColors.Item1 = ship.Owner.MapFaction.Color;
					sailColors.Item2 = ship.Owner.MapFaction.Color2;
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00011E08 File Offset: 0x00010008
		public static Banner GetShipBannerForParty(PartyBase party = null)
		{
			if (party == null)
			{
				return Banner.CreateOneColoredEmptyBanner(92);
			}
			if (party.IsMobile && party.MobileParty.Army != null)
			{
				return party.MobileParty.Army.LeaderParty.MapFaction.Banner;
			}
			return party.Banner;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00011E58 File Offset: 0x00010058
		[return: TupleElementNames(new string[] { "sailColor1", "sailColor2" })]
		public static ValueTuple<uint, uint> GetSailColorsForParty(PartyBase party = null)
		{
			ValueTuple<uint, uint> valueTuple = new ValueTuple<uint, uint>(4291609515U, 4291609515U);
			if (party != null)
			{
				if (party.IsMobile && party.MobileParty.Army != null)
				{
					valueTuple.Item1 = party.MobileParty.Army.LeaderParty.MapFaction.Color;
					valueTuple.Item2 = party.MobileParty.Army.LeaderParty.MapFaction.Color2;
				}
				else
				{
					valueTuple.Item1 = party.Owner.MapFaction.Color;
					valueTuple.Item2 = party.Owner.MapFaction.Color2;
				}
			}
			return valueTuple;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00011F04 File Offset: 0x00010104
		public static List<Ship> GetOrderedNavalRaidShipsOfPlayerParty()
		{
			List<Ship> list = new List<Ship>();
			foreach (Ship ship in MobileParty.MainParty.Ships)
			{
				if (ship.ShipHull.CanNavigateShallowWater)
				{
					list.Add(ship);
				}
			}
			return list.OrderByDescending<Ship, int>((Ship x) => x.ShipHull.MainDeckCrewCapacity).Take<Ship>(3).ToList<Ship>();
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00011FA0 File Offset: 0x000101A0
		public static MobileParty GetClanPartyToGetAvailableShip(Ship ship, Clan clan, out bool doesPartyNeedShips)
		{
			MobileParty mobileParty = null;
			float num = float.MinValue;
			MBList<Ship> mblist = new MBList<Ship>();
			doesPartyNeedShips = false;
			foreach (WarPartyComponent warPartyComponent in clan.WarPartyComponents)
			{
				if (warPartyComponent.Party != ship.Owner && Campaign.Current.Models.ShipDistributionModel.CanSendShipToParty(ship, warPartyComponent.MobileParty) && (mobileParty == null || warPartyComponent.Party.Ships.Count <= mobileParty.Ships.Count))
				{
					mblist.Clear();
					mblist.AddRange(warPartyComponent.Party.Ships);
					float scoreForPartyShipComposition = Campaign.Current.Models.ShipDistributionModel.GetScoreForPartyShipComposition(warPartyComponent.MobileParty, mblist);
					mblist.Add(ship);
					float num2 = Campaign.Current.Models.ShipDistributionModel.GetScoreForPartyShipComposition(warPartyComponent.MobileParty, mblist) - scoreForPartyShipComposition;
					if (num2 > num)
					{
						mobileParty = warPartyComponent.MobileParty;
						num = num2;
					}
				}
			}
			if (num > 0f)
			{
				doesPartyNeedShips = true;
			}
			return mobileParty;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000120D4 File Offset: 0x000102D4
		public static int GetAmountToRecoverFromRemainingShipsAfterDistribution(MBReadOnlyList<Ship> shipsToRecover, MobileParty seller)
		{
			int num = (int)shipsToRecover.SumQ<Ship>((Ship x) => Campaign.Current.Models.ShipCostModel.GetShipTradeValue(x, seller.Party, null));
			if (seller.ActualClan == Clan.PlayerClan)
			{
				float shipSellingPenalty = Campaign.Current.Models.ShipCostModel.GetShipSellingPenalty();
				num = (int)((float)num * shipSellingPenalty);
			}
			return num;
		}

		// Token: 0x04000007 RID: 7
		public const int NavalRaidMissionShipLimit = 3;
	}
}
