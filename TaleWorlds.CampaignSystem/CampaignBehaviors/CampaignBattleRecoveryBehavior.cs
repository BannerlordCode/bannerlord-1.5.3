using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003F3 RID: 1011
	public class CampaignBattleRecoveryBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003D8A RID: 15754 RVA: 0x001017A7 File Offset: 0x000FF9A7
		public override void RegisterEvents()
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickParty));
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x001017D8 File Offset: 0x000FF9D8
		private void DailyTickParty(MobileParty party)
		{
			Hero hero = null;
			if (!party.IsCurrentlyAtSea && MBRandom.RandomFloat < DefaultPerks.Medicine.Veterinarian.PrimaryBonus && party.HasPerk(DefaultPerks.Medicine.Veterinarian, out hero, false))
			{
				ItemModifier @object = MBObjectManager.Instance.GetObject<ItemModifier>("lame_horse");
				int num = MBRandom.RandomInt(party.ItemRoster.Count);
				for (int i = num; i < party.ItemRoster.Count + num; i++)
				{
					int num2 = i % party.ItemRoster.Count;
					ItemObject itemAtIndex = party.ItemRoster.GetItemAtIndex(num2);
					ItemRosterElement elementCopyAtIndex = party.ItemRoster.GetElementCopyAtIndex(num2);
					if (elementCopyAtIndex.EquipmentElement.ItemModifier == @object)
					{
						party.ItemRoster.AddToCounts(elementCopyAtIndex.EquipmentElement, -1);
						party.ItemRoster.Add(new ItemRosterElement(itemAtIndex, 1, null));
						return;
					}
				}
			}
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x001018BB File Offset: 0x000FFABB
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x001018BD File Offset: 0x000FFABD
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			this.CheckRecoveryForMapEventSide(mapEvent.AttackerSide);
			this.CheckRecoveryForMapEventSide(mapEvent.DefenderSide);
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x001018D8 File Offset: 0x000FFAD8
		private void CheckRecoveryForMapEventSide(MapEventSide mapEventSide)
		{
			if (mapEventSide.MapEvent.EventType == MapEvent.BattleTypes.FieldBattle || mapEventSide.MapEvent.EventType == MapEvent.BattleTypes.Siege || mapEventSide.MapEvent.EventType == MapEvent.BattleTypes.SiegeOutside)
			{
				foreach (MapEventParty mapEventParty in mapEventSide.Parties)
				{
					PartyBase party = mapEventParty.Party;
					if (party.IsMobile)
					{
						MobileParty mobileParty = party.MobileParty;
						foreach (TroopRosterElement troopRosterElement in mapEventParty.WoundedInBattle.GetTroopRoster())
						{
							int num = mapEventParty.WoundedInBattle.FindIndexOfTroop(troopRosterElement.Character);
							int elementNumber = mapEventParty.WoundedInBattle.GetElementNumber(num);
							Hero hero = null;
							if (mobileParty.HasPerk(DefaultPerks.Medicine.BattleHardened, out hero, false))
							{
								float num2 = DefaultPerks.Medicine.BattleHardened.PrimaryBonus;
								if (mobileParty.IsCurrentlyAtSea)
								{
									num2 *= 0.5f;
								}
								int num3 = MathF.Round(num2);
								this.GiveTroopXp(troopRosterElement, elementNumber, party, num3);
							}
						}
						foreach (TroopRosterElement troopRosterElement2 in mapEventParty.DiedInBattle.GetTroopRoster())
						{
							int num4 = mapEventParty.DiedInBattle.FindIndexOfTroop(troopRosterElement2.Character);
							int elementNumber2 = mapEventParty.DiedInBattle.GetElementNumber(num4);
							Hero hero2 = null;
							if (mobileParty.HasPerk(DefaultPerks.Medicine.Veterinarian, out hero2, false) && troopRosterElement2.Character.IsMounted)
							{
								this.RecoverMountWithChance(troopRosterElement2, elementNumber2, party);
							}
						}
					}
				}
			}
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x00101AD8 File Offset: 0x000FFCD8
		private void RecoverMountWithChance(TroopRosterElement troopRosterElement, int count, PartyBase party)
		{
			EquipmentElement equipmentElement = troopRosterElement.Character.Equipment[10];
			if (equipmentElement.Item != null)
			{
				for (int i = 0; i < count; i++)
				{
					if (MBRandom.RandomFloat < DefaultPerks.Medicine.Veterinarian.SecondaryBonus)
					{
						party.ItemRoster.AddToCounts(equipmentElement.Item, 1);
					}
				}
			}
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x00101B32 File Offset: 0x000FFD32
		private void GiveTroopXp(TroopRosterElement troopRosterElement, int count, PartyBase partyBase, int xp)
		{
			partyBase.MemberRoster.AddXpToTroop(troopRosterElement.Character, xp * count);
		}
	}
}
