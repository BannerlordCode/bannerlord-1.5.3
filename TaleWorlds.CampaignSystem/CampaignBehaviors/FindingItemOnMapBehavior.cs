using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200040F RID: 1039
	public class FindingItemOnMapBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004239 RID: 16953 RVA: 0x0012C99B File Offset: 0x0012AB9B
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickParty));
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x0012C9B4 File Offset: 0x0012ABB4
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x0012C9B8 File Offset: 0x0012ABB8
		public void DailyTickParty(MobileParty party)
		{
			Hero hero = null;
			if (MBRandom.RandomFloat < DefaultPerks.Scouting.BeastWhisperer.PrimaryBonus && party.HasPerk(DefaultPerks.Scouting.BeastWhisperer, out hero, false))
			{
				TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(party.CurrentNavigationFace);
				if (faceTerrainType == TerrainType.Steppe || faceTerrainType == TerrainType.Plain)
				{
					ItemObject randomElementWithPredicate = Items.All.GetRandomElementWithPredicate<ItemObject>((ItemObject x) => x.IsMountable && !x.NotMerchandise);
					if (randomElementWithPredicate != null)
					{
						party.ItemRoster.AddToCounts(randomElementWithPredicate, 1);
						if (party.IsMainParty)
						{
							TextObject textObject = new TextObject("{=vl9bawa7}{COUNT} {?(COUNT > 1)}{PLURAL(ANIMAL_NAME)} are{?}{ANIMAL_NAME} is{\\?} added to your party.", null);
							textObject.SetTextVariable("COUNT", 1);
							textObject.SetTextVariable("ANIMAL_NAME", randomElementWithPredicate.Name);
							InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
						}
					}
				}
			}
		}
	}
}
