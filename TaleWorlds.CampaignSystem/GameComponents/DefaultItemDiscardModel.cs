using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200012A RID: 298
	public class DefaultItemDiscardModel : ItemDiscardModel
	{
		// Token: 0x060018E6 RID: 6374 RVA: 0x000792A4 File Offset: 0x000774A4
		public override bool PlayerCanDonateItem(ItemObject item)
		{
			bool flag = false;
			if (item.HasWeaponComponent)
			{
				Hero hero = null;
				flag = MobileParty.MainParty.HasPerk(DefaultPerks.Steward.GivingHands, out hero, false);
			}
			else if (item.HasArmorComponent)
			{
				Hero hero2 = null;
				flag = MobileParty.MainParty.HasPerk(DefaultPerks.Steward.PaidInPromise, out hero2, true);
			}
			return flag;
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x000792F0 File Offset: 0x000774F0
		public override int GetXpBonusForDiscardingItem(ItemObject item, int amount = 1)
		{
			int num = 0;
			if (this.PlayerCanDonateItem(item))
			{
				switch (item.Tier)
				{
				case ItemObject.ItemTiers.Tier1:
					num = 75;
					break;
				case ItemObject.ItemTiers.Tier2:
					num = 150;
					break;
				case ItemObject.ItemTiers.Tier3:
					num = 250;
					break;
				case ItemObject.ItemTiers.Tier4:
				case ItemObject.ItemTiers.Tier5:
				case ItemObject.ItemTiers.Tier6:
					num = 300;
					break;
				default:
					num = 35;
					break;
				}
			}
			return num * amount;
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x00079354 File Offset: 0x00077554
		public override int GetXpBonusForDiscardingItems(ItemRoster itemRoster)
		{
			float num = 0f;
			for (int i = 0; i < itemRoster.Count; i++)
			{
				ItemObject itemAtIndex = itemRoster.GetItemAtIndex(i);
				num += (float)this.GetXpBonusForDiscardingItem(itemAtIndex, itemRoster.GetElementNumber(i));
			}
			return MathF.Floor(num);
		}
	}
}
