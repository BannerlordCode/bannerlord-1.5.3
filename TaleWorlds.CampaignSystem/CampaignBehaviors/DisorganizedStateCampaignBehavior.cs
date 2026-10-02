using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000407 RID: 1031
	internal class DisorganizedStateCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060040BD RID: 16573 RVA: 0x0011CCF8 File Offset: 0x0011AEF8
		public override void RegisterEvents()
		{
			CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnd));
			CampaignEvents.PartyRemovedFromArmyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyRemovedFromArmy));
			CampaignEvents.GameMenuOptionSelectedEvent.AddNonSerializedListener(this, new Action<GameMenu, GameMenuOption>(this.OnGameMenuOptionSelected));
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x0011CD64 File Offset: 0x0011AF64
		private void OnGameMenuOptionSelected(GameMenu gameMenu, GameMenuOption gameMenuOption)
		{
			if (this._checkForEvent && (gameMenuOption.IdString == "str_order_attack" || gameMenuOption.IdString == "attack"))
			{
				foreach (MapEventParty mapEventParty in MobileParty.MainParty.MapEvent.DefenderSide.Parties)
				{
					if (Campaign.Current.Models.PartyImpairmentModel.CanGetDisorganized(mapEventParty.Party))
					{
						mapEventParty.Party.MobileParty.SetDisorganized(true);
					}
				}
			}
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x0011CE1C File Offset: 0x0011B01C
		private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			if (mapEvent.IsSallyOut)
			{
				if (!mapEvent.AttackerSide.IsMainPartyAmongParties())
				{
					using (List<MapEventParty>.Enumerator enumerator = mapEvent.DefenderSide.Parties.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MapEventParty mapEventParty = enumerator.Current;
							if (Campaign.Current.Models.PartyImpairmentModel.CanGetDisorganized(mapEventParty.Party))
							{
								mapEventParty.Party.MobileParty.SetDisorganized(true);
							}
						}
						return;
					}
				}
				this._checkForEvent = true;
			}
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x0011CEB8 File Offset: 0x0011B0B8
		private void OnPartyRemovedFromArmy(MobileParty mobileParty)
		{
			if (Campaign.Current.Models.PartyImpairmentModel.CanGetDisorganized(mobileParty.Party))
			{
				mobileParty.SetDisorganized(true);
			}
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x0011CEE0 File Offset: 0x0011B0E0
		private void OnMapEventEnd(MapEvent mapEvent)
		{
			bool flag;
			if (mapEvent.AttackerSide.Parties.Sum<MapEventParty>((MapEventParty x) => x.HealthyManCountAtStart) == mapEvent.AttackerSide.Parties.Sum<MapEventParty>((MapEventParty x) => x.Party.NumberOfHealthyMembers))
			{
				flag = mapEvent.DefenderSide.Parties.Sum<MapEventParty>((MapEventParty x) => x.HealthyManCountAtStart) != mapEvent.DefenderSide.Parties.Sum<MapEventParty>((MapEventParty x) => x.Party.NumberOfHealthyMembers);
			}
			else
			{
				flag = true;
			}
			if (flag && !mapEvent.IsHideoutBattle)
			{
				foreach (PartyBase partyBase in mapEvent.InvolvedParties)
				{
					if (partyBase.IsActive)
					{
						MobileParty mobileParty = partyBase.MobileParty;
						if ((mobileParty == null || !mobileParty.IsMainParty || !mapEvent.DiplomaticallyFinished || !mapEvent.AttackerSide.MapFaction.IsAtWarWith(mapEvent.DefenderSide.MapFaction)) && (!mapEvent.IsSallyOut || partyBase.MapEventSide.MissionSide == BattleSideEnum.Defender) && Campaign.Current.Models.PartyImpairmentModel.CanGetDisorganized(partyBase) && (mapEvent.RetreatingSide == BattleSideEnum.None || mapEvent.RetreatingSide != partyBase.Side))
						{
							partyBase.MobileParty.SetDisorganized(true);
						}
					}
				}
			}
			this._checkForEvent = false;
		}

		// Token: 0x060040C2 RID: 16578 RVA: 0x0011D09C File Offset: 0x0011B29C
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<bool>("_checkForEvent", ref this._checkForEvent);
		}

		// Token: 0x040013A2 RID: 5026
		private bool _checkForEvent;
	}
}
