using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000468 RID: 1128
	public class TownSecurityCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004909 RID: 18697 RVA: 0x0016D914 File Offset: 0x0016BB14
		public override void RegisterEvents()
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.MapEventEnded));
			CampaignEvents.OnSiegeEventEndedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.SiegeEventEnded));
			CampaignEvents.OnHideoutDeactivatedEvent.AddNonSerializedListener(this, new Action<Settlement>(this.OnHideoutDeactivated));
		}

		// Token: 0x0600490A RID: 18698 RVA: 0x0016D968 File Offset: 0x0016BB68
		private void OnHideoutDeactivated(Settlement hideout)
		{
			SettlementSecurityModel model = Campaign.Current.Models.SettlementSecurityModel;
			foreach (Settlement settlement in Settlement.All.Where<Settlement>((Settlement t) => t.IsTown && t.Position.DistanceSquared(hideout.Position) < model.HideoutClearedSecurityEffectRadius * model.HideoutClearedSecurityEffectRadius).ToList<Settlement>())
			{
				settlement.Town.Security += (float)model.HideoutClearedSecurityGain;
			}
		}

		// Token: 0x0600490B RID: 18699 RVA: 0x0016DA08 File Offset: 0x0016BC08
		private void MapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.IsFieldBattle && mapEvent.HasWinner)
			{
				SettlementSecurityModel model = Campaign.Current.Models.SettlementSecurityModel;
				using (List<Settlement>.Enumerator enumerator = Settlement.All.Where<Settlement>((Settlement t) => t.IsTown && t.Position.DistanceSquared(mapEvent.Position) < model.MapEventSecurityEffectRadius * model.MapEventSecurityEffectRadius).ToList<Settlement>().GetEnumerator())
				{
					Func<PartyBase, bool> <>9__3;
					while (enumerator.MoveNext())
					{
						Settlement town = enumerator.Current;
						if (mapEvent.Winner.Parties.Any<MapEventParty>((MapEventParty party) => party.Party.IsMobile && party.Party.MobileParty.IsBandit) && mapEvent.InvolvedParties.Any<PartyBase>((PartyBase party) => this.ValidCivilianPartyCondition(party, mapEvent, town.MapFaction)))
						{
							float num = mapEvent.StrengthOfSide[(int)mapEvent.DefeatedSide];
							town.Town.Security += model.GetLootedNearbyPartySecurityEffect(town.Town, num);
						}
						else
						{
							IEnumerable<PartyBase> involvedParties = mapEvent.InvolvedParties;
							Func<PartyBase, bool> func;
							if ((func = <>9__3) == null)
							{
								func = (<>9__3 = (PartyBase party) => this.ValidBanditPartyCondition(party, mapEvent));
							}
							if (involvedParties.Any<PartyBase>(func))
							{
								float num2 = mapEvent.StrengthOfSide[(int)mapEvent.DefeatedSide];
								town.Town.Security += model.GetNearbyBanditPartyDefeatedSecurityEffect(town.Town, num2);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x0016DC70 File Offset: 0x0016BE70
		private bool ValidCivilianPartyCondition(PartyBase party, MapEvent mapEvent, IFaction mapFaction)
		{
			return party.IsMobile && ((party.Side != mapEvent.WinningSide && party.MobileParty.IsVillager && DiplomacyHelper.IsSameFactionAndNotEliminated(party.MapFaction, mapFaction)) || (party.MobileParty.IsCaravan && !party.MapFaction.IsAtWarWith(mapFaction)));
		}

		// Token: 0x0600490D RID: 18701 RVA: 0x0016DCD0 File Offset: 0x0016BED0
		private bool ValidBanditPartyCondition(PartyBase party, MapEvent mapEvent)
		{
			if (party.Side != mapEvent.WinningSide)
			{
				MobileParty mobileParty = party.MobileParty;
				return mobileParty != null && mobileParty.IsBandit;
			}
			return false;
		}

		// Token: 0x0600490E RID: 18702 RVA: 0x0016DCF3 File Offset: 0x0016BEF3
		private void SiegeEventEnded(SiegeEvent siegeEvent)
		{
		}

		// Token: 0x0600490F RID: 18703 RVA: 0x0016DCF5 File Offset: 0x0016BEF5
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
