using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200040B RID: 1035
	public class EmptyClanPartiesCampaignBehavior : CampaignBehaviorBase, IEmptyClanPartiesCampaignBehavior
	{
		// Token: 0x060040FB RID: 16635 RVA: 0x001236A0 File Offset: 0x001218A0
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<EmptyClanPartiesCampaignBehavior.EmptyClanParty>>("_emptyClanParties", ref this._emptyClanParties);
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x001236B4 File Offset: 0x001218B4
		public override void RegisterEvents()
		{
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.BeforeHeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnBeforeHeroKilled));
			CampaignEvents.MobilePartyCreated.AddNonSerializedListener(this, new Action<MobileParty>(this.OnMobilePartyCreated));
			CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, new Action<Hero, PartyBase, IFaction, EndCaptivityDetail, bool>(this.OnHeroPrisonerReleased));
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x00123734 File Offset: 0x00121934
		public void TransferCachedLordPartyToNewPartyForPlayerClan(Hero cachedPartyLeader, PartyBase newParty)
		{
			EmptyClanPartiesCampaignBehavior.EmptyClanParty emptyClanParty;
			if (this.TryGetEmptyClanPartyForHero(cachedPartyLeader, out emptyClanParty))
			{
				this.RemoveEmptyClanParty(emptyClanParty);
				foreach (Ship ship in emptyClanParty.Ships)
				{
					ChangeShipOwnerAction.ApplyByTransferring(newParty, ship);
				}
			}
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x0012379C File Offset: 0x0012199C
		public void DisbandCachedLordPartyForPlayerClan(Hero hero)
		{
			EmptyClanPartiesCampaignBehavior.EmptyClanParty emptyClanParty;
			if (this.TryGetEmptyClanPartyForHero(hero, out emptyClanParty))
			{
				this.RemoveEmptyClanParty(emptyClanParty);
				this.DistributePartyShipsAndRecoverGold(emptyClanParty);
			}
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x001237C4 File Offset: 0x001219C4
		public MBReadOnlyList<Ship> GetShipsForCachedLordPartyForPlayerClan(Hero hero)
		{
			MBReadOnlyList<Ship> mbreadOnlyList = null;
			foreach (EmptyClanPartiesCampaignBehavior.EmptyClanParty emptyClanParty in this._emptyClanParties)
			{
				if (emptyClanParty.Leader == hero)
				{
					mbreadOnlyList = emptyClanParty.Ships.ToMBList<Ship>();
				}
			}
			return mbreadOnlyList;
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x00123828 File Offset: 0x00121A28
		public int GetShipCountForCachedLordPartyForPlayerClan(Hero hero)
		{
			int num = 0;
			foreach (EmptyClanPartiesCampaignBehavior.EmptyClanParty emptyClanParty in this._emptyClanParties)
			{
				if (emptyClanParty.Leader == hero)
				{
					num = emptyClanParty.Ships.Count;
				}
			}
			return num;
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x0012388C File Offset: 0x00121A8C
		public MBReadOnlyList<Hero> GetEmptyClanPartyLeaders()
		{
			MBList<Hero> mblist = new MBList<Hero>();
			foreach (EmptyClanPartiesCampaignBehavior.EmptyClanParty emptyClanParty in this._emptyClanParties)
			{
				mblist.Add(emptyClanParty.Leader);
			}
			return mblist;
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x001238EC File Offset: 0x00121AEC
		private void OnBeforeHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			EmptyClanPartiesCampaignBehavior.EmptyClanParty emptyClanParty;
			if (victim.Clan == Clan.PlayerClan && !victim.IsHumanPlayerCharacter && this.TryGetEmptyClanPartyForHero(victim, out emptyClanParty))
			{
				this.RemoveEmptyClanParty(emptyClanParty);
				this.DistributePartyShipsAndRecoverGold(emptyClanParty);
			}
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x00123927 File Offset: 0x00121B27
		private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			if (hero != null && party == null && settlement != null)
			{
				this.TryCreateNewClanMobilePartyFromCachedEmptyParties(hero, settlement);
			}
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x0012393C File Offset: 0x00121B3C
		private void OnMobilePartyDestroyed(MobileParty party, PartyBase _)
		{
			Hero owner = party.Owner;
			if (owner != null && owner.Clan == Clan.PlayerClan && !owner.IsHumanPlayerCharacter && !owner.IsDead && !party.IsCaravan && !party.IsDisbanding)
			{
				EmptyClanPartiesCampaignBehavior.EmptyClanParty emptyClanParty = new EmptyClanPartiesCampaignBehavior.EmptyClanParty(owner);
				this.AddEmptyClanParty(emptyClanParty);
				if (!party.IsCurrentlyAtSea && party.Ships.Count > 0)
				{
					int num = party.Ships.Count - 1;
					while (0 <= num)
					{
						Ship ship = party.Ships[num];
						ship.Owner = null;
						emptyClanParty.Ships.Add(ship);
						num--;
					}
				}
			}
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x001239E0 File Offset: 0x00121BE0
		private void OnMobilePartyCreated(MobileParty party)
		{
			if (party.ActualClan == Clan.PlayerClan && party.LeaderHero != null && !party.IsCaravan && !party.IsMainParty)
			{
				Hero leaderHero = party.LeaderHero;
				EmptyClanPartiesCampaignBehavior.EmptyClanParty emptyClanParty;
				if (this.TryGetEmptyClanPartyForHero(leaderHero, out emptyClanParty))
				{
					this.RemoveEmptyClanParty(emptyClanParty);
					foreach (Ship ship in emptyClanParty.Ships)
					{
						ChangeShipOwnerAction.ApplyByTransferring(party.Party, ship);
					}
				}
			}
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x00123A78 File Offset: 0x00121C78
		private void OnHeroPrisonerReleased(Hero hero, PartyBase party, IFaction faction, EndCaptivityDetail detail, bool showNotification)
		{
			if (detail == EndCaptivityDetail.Ransom && hero != null && hero.CurrentSettlement != null && !hero.CurrentSettlement.MapFaction.IsAtWarWith(hero.MapFaction))
			{
				this.TryCreateNewClanMobilePartyFromCachedEmptyParties(hero, hero.CurrentSettlement);
			}
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x00123AB0 File Offset: 0x00121CB0
		private void TryCreateNewClanMobilePartyFromCachedEmptyParties(Hero hero, Settlement settlement)
		{
			EmptyClanPartiesCampaignBehavior.EmptyClanParty emptyClanParty;
			if (hero != null && hero.IsActive && hero.Clan == Clan.PlayerClan && !hero.IsHumanPlayerCharacter && hero.PartyBelongedTo == null && this.TryGetEmptyClanPartyForHero(hero, out emptyClanParty))
			{
				this.RemoveEmptyClanParty(emptyClanParty);
				MobileParty mobileParty = MobilePartyHelper.SpawnLordParty(hero, settlement);
				foreach (Ship ship in emptyClanParty.Ships)
				{
					ChangeShipOwnerAction.ApplyByTransferring(mobileParty.Party, ship);
				}
			}
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x00123B4C File Offset: 0x00121D4C
		private bool TryGetEmptyClanPartyForHero(Hero hero, out EmptyClanPartiesCampaignBehavior.EmptyClanParty foundEmptyClanParty)
		{
			bool flag = false;
			foundEmptyClanParty = default(EmptyClanPartiesCampaignBehavior.EmptyClanParty);
			foreach (EmptyClanPartiesCampaignBehavior.EmptyClanParty emptyClanParty in this._emptyClanParties)
			{
				if (emptyClanParty.Leader == hero)
				{
					foundEmptyClanParty = emptyClanParty;
					flag = true;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x00123BB8 File Offset: 0x00121DB8
		private void AddEmptyClanParty(EmptyClanPartiesCampaignBehavior.EmptyClanParty emptyClanParty)
		{
			this._emptyClanParties.Add(emptyClanParty);
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x00123BC6 File Offset: 0x00121DC6
		private void RemoveEmptyClanParty(EmptyClanPartiesCampaignBehavior.EmptyClanParty emptyClanParty)
		{
			this._emptyClanParties.Remove(emptyClanParty);
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x00123BD8 File Offset: 0x00121DD8
		private void DistributePartyShipsAndRecoverGold(EmptyClanPartiesCampaignBehavior.EmptyClanParty emptyClanParty)
		{
			for (int i = emptyClanParty.Ships.Count - 1; i >= 0; i--)
			{
				Ship shipToSend = emptyClanParty.Ships[i];
				if (Clan.PlayerClan.WarPartyComponents.AnyQ<WarPartyComponent>((WarPartyComponent x) => Campaign.Current.Models.ShipDistributionModel.CanSendShipToParty(shipToSend, x.MobileParty)))
				{
					bool flag;
					MobileParty clanPartyToGetAvailableShip = ShipHelper.GetClanPartyToGetAvailableShip(shipToSend, Clan.PlayerClan, out flag);
					if (clanPartyToGetAvailableShip != null && flag)
					{
						ChangeShipOwnerAction.ApplyByTransferring(clanPartyToGetAvailableShip.Party, shipToSend);
						emptyClanParty.Ships.RemoveAt(i);
					}
				}
			}
			if (emptyClanParty.Ships.Count > 0)
			{
				int amountToRecoverFromRemainingShipsAfterDistribution = ShipHelper.GetAmountToRecoverFromRemainingShipsAfterDistribution(emptyClanParty.Ships.ToMBList<Ship>(), MobileParty.MainParty);
				MBTextManager.SetTextVariable("GOLD_AMOUNT", amountToRecoverFromRemainingShipsAfterDistribution);
				MBTextManager.SetTextVariable("LEADER_NAME", emptyClanParty.Leader.Name, false);
				MBTextManager.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">", false);
				MBInformationManager.AddQuickInformation(new TextObject("{=YaSnA9j0}{LEADER_NAME}'s party has disbanded. You recovered {GOLD_AMOUNT}{GOLD_ICON} from its ships.", null), 0, null, null, "");
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, amountToRecoverFromRemainingShipsAfterDistribution, false);
			}
		}

		// Token: 0x040013BE RID: 5054
		private List<EmptyClanPartiesCampaignBehavior.EmptyClanParty> _emptyClanParties = new List<EmptyClanPartiesCampaignBehavior.EmptyClanParty>();

		// Token: 0x02000845 RID: 2117
		public class EmptyClanPartiesCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060067B2 RID: 26546 RVA: 0x001D3257 File Offset: 0x001D1457
			public EmptyClanPartiesCampaignBehaviorTypeDefiner()
				: base(312280)
			{
			}

			// Token: 0x060067B3 RID: 26547 RVA: 0x001D3264 File Offset: 0x001D1464
			protected override void DefineStructTypes()
			{
				base.AddStructDefinition(typeof(EmptyClanPartiesCampaignBehavior.EmptyClanParty), 1, null);
			}

			// Token: 0x060067B4 RID: 26548 RVA: 0x001D3278 File Offset: 0x001D1478
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(List<EmptyClanPartiesCampaignBehavior.EmptyClanParty>));
			}
		}

		// Token: 0x02000846 RID: 2118
		internal struct EmptyClanParty
		{
			// Token: 0x060067B5 RID: 26549 RVA: 0x001D328A File Offset: 0x001D148A
			public EmptyClanParty(Hero leader)
			{
				this.Leader = leader;
				this.Ships = new List<Ship>();
			}

			// Token: 0x060067B6 RID: 26550 RVA: 0x001D32A0 File Offset: 0x001D14A0
			public static void AutoGeneratedStaticCollectObjectsEmptyClanParty(object o, List<object> collectedObjects)
			{
				((EmptyClanPartiesCampaignBehavior.EmptyClanParty)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060067B7 RID: 26551 RVA: 0x001D32BC File Offset: 0x001D14BC
			private void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Leader);
				collectedObjects.Add(this.Ships);
			}

			// Token: 0x060067B8 RID: 26552 RVA: 0x001D32D6 File Offset: 0x001D14D6
			internal static object AutoGeneratedGetMemberValueLeader(object o)
			{
				return ((EmptyClanPartiesCampaignBehavior.EmptyClanParty)o).Leader;
			}

			// Token: 0x060067B9 RID: 26553 RVA: 0x001D32E3 File Offset: 0x001D14E3
			internal static object AutoGeneratedGetMemberValueShips(object o)
			{
				return ((EmptyClanPartiesCampaignBehavior.EmptyClanParty)o).Ships;
			}

			// Token: 0x040021B6 RID: 8630
			[SaveableField(0)]
			public readonly Hero Leader;

			// Token: 0x040021B7 RID: 8631
			[SaveableField(1)]
			public List<Ship> Ships;
		}
	}
}
