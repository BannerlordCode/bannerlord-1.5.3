using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003EF RID: 1007
	public class BattleCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003D05 RID: 15621 RVA: 0x000FC053 File Offset: 0x000FA253
		public override void RegisterEvents()
		{
			CampaignEvents.OnHeroCombatHitEvent.AddNonSerializedListener(this, new Action<CharacterObject, CharacterObject, PartyBase, WeaponComponentData, bool, int>(BattleCampaignBehavior.OnHeroCombatHit));
			CampaignEvents.OnCollectLootsItemsEvent.AddNonSerializedListener(this, new Action<PartyBase, ItemRoster>(BattleCampaignBehavior.OnCollectLootItems));
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x000FC084 File Offset: 0x000FA284
		private static void OnCollectLootItems(PartyBase winnerParty, ItemRoster gainedLoots)
		{
			Hero hero = null;
			if (winnerParty.IsMobile && winnerParty.MobileParty.HasPerk(DefaultPerks.Engineering.Metallurgy, out hero, false))
			{
				foreach (ItemRosterElement itemRosterElement in gainedLoots.ToMBList<ItemRosterElement>())
				{
					ItemModifier itemModifier = itemRosterElement.EquipmentElement.ItemModifier;
					if (itemModifier != null && itemModifier.PriceMultiplier < 1f)
					{
						for (int i = 0; i < itemRosterElement.Amount; i++)
						{
							if (MBRandom.RandomFloat < DefaultPerks.Engineering.Metallurgy.PrimaryBonus)
							{
								gainedLoots.AddToCounts(itemRosterElement.EquipmentElement, -1);
								ItemRosterElement itemRosterElement2 = new ItemRosterElement(itemRosterElement.EquipmentElement.Item, 1, null);
								gainedLoots.Add(itemRosterElement2);
							}
						}
					}
				}
			}
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x000FC178 File Offset: 0x000FA378
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003D08 RID: 15624 RVA: 0x000FC17C File Offset: 0x000FA37C
		private static void OnHeroCombatHit(CharacterObject attacker, CharacterObject attacked, PartyBase party, WeaponComponentData attackerWeapon, bool isFatal, int xpGained)
		{
			if (isFatal && attackerWeapon != null && party.MemberRoster.TotalRegulars > 0 && BattleCampaignBehavior.IsWeaponSuitableToGetBaptisedInBloodPerkBonus(attackerWeapon) && attacker.HeroObject.GetPerkValue(DefaultPerks.TwoHanded.BaptisedInBlood))
			{
				for (int i = 0; i < party.MemberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(i);
					if (!elementCopyAtIndex.Character.IsHero && elementCopyAtIndex.Character.IsInfantry)
					{
						int num = (int)DefaultPerks.TwoHanded.BaptisedInBlood.PrimaryBonus * elementCopyAtIndex.Number;
						party.MemberRoster.AddXpToTroopAtIndex(i, num);
					}
				}
			}
		}

		// Token: 0x06003D09 RID: 15625 RVA: 0x000FC21C File Offset: 0x000FA41C
		private static bool IsWeaponSuitableToGetBaptisedInBloodPerkBonus(WeaponComponentData attackerWeapon)
		{
			return attackerWeapon.WeaponClass == WeaponClass.TwoHandedSword || attackerWeapon.WeaponClass == WeaponClass.TwoHandedPolearm || attackerWeapon.WeaponClass == WeaponClass.TwoHandedAxe || attackerWeapon.WeaponClass == WeaponClass.TwoHandedMace;
		}
	}
}
