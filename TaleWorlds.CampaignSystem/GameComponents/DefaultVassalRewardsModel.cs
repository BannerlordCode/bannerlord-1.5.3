using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200016B RID: 363
	public class DefaultVassalRewardsModel : VassalRewardsModel
	{
		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06001B99 RID: 7065 RVA: 0x0008EEA1 File Offset: 0x0008D0A1
		public override int RelationRewardWithLeader
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06001B9A RID: 7066 RVA: 0x0008EEA5 File Offset: 0x0008D0A5
		public override float InfluenceReward
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x0008EEAC File Offset: 0x0008D0AC
		public override ItemRoster GetEquipmentRewardsForJoiningKingdom(Kingdom kingdom)
		{
			ItemRoster itemRoster = new ItemRoster();
			foreach (ItemObject itemObject in kingdom.Culture.VassalRewardItems)
			{
				itemRoster.AddToCounts(itemObject, 1);
			}
			ItemObject randomBannerAtLevel = this.GetRandomBannerAtLevel(2, kingdom.Culture);
			if (randomBannerAtLevel != null)
			{
				itemRoster.AddToCounts(randomBannerAtLevel, 1);
			}
			return itemRoster;
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x0008EF28 File Offset: 0x0008D128
		private ItemObject GetRandomBannerAtLevel(int bannerLevel, CultureObject culture = null)
		{
			MBList<ItemObject> mblist = Campaign.Current.Models.BannerItemModel.GetPossibleRewardBannerItems().ToMBList<ItemObject>();
			if (culture == null)
			{
				return mblist.GetRandomElementWithPredicate<ItemObject>((ItemObject i) => (i.ItemComponent as BannerComponent).BannerLevel == bannerLevel);
			}
			return mblist.GetRandomElementWithPredicate<ItemObject>((ItemObject i) => (i.ItemComponent as BannerComponent).BannerLevel == bannerLevel && i.Culture == culture);
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x0008EF90 File Offset: 0x0008D190
		public override TroopRoster GetTroopRewardsForJoiningKingdom(Kingdom kingdom)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (PartyTemplateStack partyTemplateStack in kingdom.Culture.VassalRewardTroopsPartyTemplate.Stacks)
			{
				troopRoster.AddToCounts(partyTemplateStack.Character, partyTemplateStack.MaxValue, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x04000939 RID: 2361
		private const int VassalRewardBannerLevel = 2;
	}
}
