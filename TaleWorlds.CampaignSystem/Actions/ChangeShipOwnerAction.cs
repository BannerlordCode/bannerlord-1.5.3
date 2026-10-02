using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004CB RID: 1227
	public static class ChangeShipOwnerAction
	{
		// Token: 0x06004D46 RID: 19782 RVA: 0x00186F2C File Offset: 0x0018512C
		private static void ApplyInternal(PartyBase newOwner, Ship ship, Settlement stashSettlement, ChangeShipOwnerAction.ShipOwnerChangeDetail changeDetail)
		{
			PartyBase owner = ship.Owner;
			if (changeDetail == ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByTrade)
			{
				float shipTradeValue = Campaign.Current.Models.ShipCostModel.GetShipTradeValue(ship, owner, newOwner);
				if (owner.IsSettlement)
				{
					if (newOwner.MobileParty.IsCaravan || newOwner.MobileParty.IsVillager)
					{
						GiveGoldAction.ApplyForPartyToCharacter(newOwner, null, (int)shipTradeValue, false);
					}
					else
					{
						Clan actualClan = newOwner.MobileParty.ActualClan;
						if (((actualClan != null) ? actualClan.Leader : null) != null)
						{
							GiveGoldAction.ApplyBetweenCharacters(newOwner.MobileParty.ActualClan.Leader, null, (int)shipTradeValue, false);
						}
						else if (newOwner.MobileParty.LeaderHero != null)
						{
							GiveGoldAction.ApplyBetweenCharacters(newOwner.MobileParty.LeaderHero, null, (int)shipTradeValue, false);
						}
						else
						{
							Debug.FailedAssert("Unhandled case", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Actions\\ChangeShipOwnerAction.cs", "ApplyInternal", 50);
							GiveGoldAction.ApplyForPartyToCharacter(newOwner, null, (int)shipTradeValue, false);
						}
					}
					if (newOwner.Ships.Any<Ship>() && !newOwner.MobileParty.Anchor.IsValid)
					{
						newOwner.MobileParty.Anchor.Settlement = ship.Owner.Settlement;
					}
				}
				else if (owner.MobileParty.IsCaravan || owner.MobileParty.IsVillager)
				{
					GiveGoldAction.ApplyForCharacterToParty(null, owner, (int)shipTradeValue, false);
				}
				else
				{
					Clan actualClan2 = owner.MobileParty.ActualClan;
					if (((actualClan2 != null) ? actualClan2.Leader : null) != null)
					{
						GiveGoldAction.ApplyBetweenCharacters(null, owner.MobileParty.ActualClan.Leader, (int)shipTradeValue, false);
					}
					else if (owner.LeaderHero != null)
					{
						GiveGoldAction.ApplyBetweenCharacters(null, owner.LeaderHero, (int)shipTradeValue, false);
					}
					else
					{
						Debug.FailedAssert("Unhandled case", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Actions\\ChangeShipOwnerAction.cs", "ApplyInternal", 75);
						GiveGoldAction.ApplyForCharacterToParty(null, owner, (int)shipTradeValue, false);
					}
				}
			}
			else if (changeDetail == ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByStashing && stashSettlement != null)
			{
				if (!stashSettlement.ShipStash.Contains(ship))
				{
					stashSettlement.ShipStash.Add(ship);
				}
			}
			else if (changeDetail == ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByUnstashing && stashSettlement != null)
			{
				stashSettlement.ShipStash.Remove(ship);
			}
			ship.Owner = newOwner;
			if (owner != null)
			{
				MobileParty mobileParty = owner.MobileParty;
				if (mobileParty != null)
				{
					mobileParty.SetNavalVisualAsDirty();
				}
			}
			if (newOwner != null)
			{
				MobileParty mobileParty2 = newOwner.MobileParty;
				if (mobileParty2 != null)
				{
					mobileParty2.SetNavalVisualAsDirty();
				}
			}
			bool flag = false;
			CampaignEventDispatcher.Instance.CanHaveUnlockedUpgradePiece(ship, changeDetail, ref flag);
			if (ship.CanHaveUnlockedPieces != flag)
			{
				ship.CanHaveUnlockedPieces = flag;
			}
			CampaignEventDispatcher.Instance.OnShipOwnerChanged(ship, owner, changeDetail);
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x00187179 File Offset: 0x00185379
		public static void ApplyByTransferring(PartyBase newOwner, Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(newOwner, ship, null, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByTransferring);
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x00187184 File Offset: 0x00185384
		public static void ApplyByTrade(PartyBase newOwner, Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(newOwner, ship, null, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByTrade);
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x0018718F File Offset: 0x0018538F
		public static void ApplyByLooting(PartyBase newOwner, Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(newOwner, ship, null, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByLooting);
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x0018719A File Offset: 0x0018539A
		public static void ApplyByProduction(PartyBase newOwner, Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(newOwner, ship, null, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByProduction);
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x001871A5 File Offset: 0x001853A5
		public static void ApplyByMobilePartyCreation(PartyBase newOwner, Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(newOwner, ship, null, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByMobilePartyCreation);
		}

		// Token: 0x06004D4C RID: 19788 RVA: 0x001871B0 File Offset: 0x001853B0
		public static void ApplyByStashingShipIntoSettlement(Settlement stashSettlement, Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(null, ship, stashSettlement, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByStashing);
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x001871BB File Offset: 0x001853BB
		public static void ApplyByUnstashingShipFromSettlement(PartyBase newOwner, Settlement stashSettlement, Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(newOwner, ship, stashSettlement, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByUnstashing);
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x001871C6 File Offset: 0x001853C6
		public static void ApplyByTemporarilyRemovingShipsFromPlayer(Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(null, ship, null, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByTemporarilyRemovingShipsFromPlayer);
		}

		// Token: 0x06004D4F RID: 19791 RVA: 0x001871D1 File Offset: 0x001853D1
		public static void ApplyByGivingBackShipsToPlayer(Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(PartyBase.MainParty, ship, null, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByGivingBackShipsToPlayer);
		}

		// Token: 0x020008DE RID: 2270
		public enum ShipOwnerChangeDetail
		{
			// Token: 0x04002663 RID: 9827
			ApplyByTrade,
			// Token: 0x04002664 RID: 9828
			ApplyByTransferring,
			// Token: 0x04002665 RID: 9829
			ApplyByLooting,
			// Token: 0x04002666 RID: 9830
			ApplyByMobilePartyCreation,
			// Token: 0x04002667 RID: 9831
			ApplyByProduction,
			// Token: 0x04002668 RID: 9832
			ApplyByStashing,
			// Token: 0x04002669 RID: 9833
			ApplyByUnstashing,
			// Token: 0x0400266A RID: 9834
			ApplyByTemporarilyRemovingShipsFromPlayer,
			// Token: 0x0400266B RID: 9835
			ApplyByGivingBackShipsToPlayer
		}
	}
}
