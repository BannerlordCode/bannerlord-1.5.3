using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B7 RID: 183
	public class BecomeKingSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x0005E537 File Offset: 0x0005C737
		public Hero NewLeaderHero { get; }

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x0005E53F File Offset: 0x0005C73F
		public override string SceneID
		{
			get
			{
				return "scn_become_king_notification";
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x0005E548 File Offset: 0x0005C748
		public override TextObject TitleText
		{
			get
			{
				TextObject textObject;
				if (this.NewLeaderHero.Clan.Kingdom.Culture.StringId.Equals("empire", StringComparison.InvariantCultureIgnoreCase))
				{
					textObject = GameTexts.FindText("str_become_king_empire", null);
				}
				else
				{
					TextObject textObject2 = (this.NewLeaderHero.IsFemale ? GameTexts.FindText("str_liege_title_female", this.NewLeaderHero.Clan.Kingdom.Culture.StringId) : GameTexts.FindText("str_liege_title", this.NewLeaderHero.Clan.Kingdom.Culture.StringId));
					textObject = GameTexts.FindText("str_become_king_nonempire", null);
					textObject.SetTextVariable("TITLE_NAME", textObject2);
				}
				textObject.SetTextVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				textObject.SetTextVariable("YEAR", this._creationCampaignTime.GetYear);
				textObject.SetTextVariable("KING_NAME", this.NewLeaderHero.Name);
				textObject.SetTextVariable("IS_KING_MALE", this.NewLeaderHero.IsFemale ? 0 : 1);
				return textObject;
			}
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0005E661 File Offset: 0x0005C861
		public override Banner[] GetBanners()
		{
			return new Banner[]
			{
				this.NewLeaderHero.Clan.Kingdom.Banner,
				this.NewLeaderHero.Clan.Kingdom.Banner
			};
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0005E69C File Offset: 0x0005C89C
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			Equipment equipment = this.NewLeaderHero.CharacterObject.Equipment.Clone(true);
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			list.Add(new SceneNotificationData.SceneNotificationCharacter(this.NewLeaderHero.CharacterObject, equipment, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			for (int i = 0; i < 14; i++)
			{
				CharacterObject characterObject = (this.IsAudienceFemale(i) ? this.NewLeaderHero.Clan.Kingdom.Culture.Townswoman : this.NewLeaderHero.Clan.Kingdom.Culture.Townsman);
				Equipment equipment2 = characterObject.FirstCivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, true, false);
				uint color = BannerManager.Instance.ReadOnlyColorPalette.GetRandomElementInefficiently<KeyValuePair<int, BannerColor>>().Value.Color;
				uint color2 = BannerManager.Instance.ReadOnlyColorPalette.GetRandomElementInefficiently<KeyValuePair<int, BannerColor>>().Value.Color;
				list.Add(new SceneNotificationData.SceneNotificationCharacter(characterObject, equipment2, characterObject.GetBodyProperties(equipment2, MBRandom.RandomInt(100)), false, color, color2, false));
			}
			for (int j = 0; j < 2; j++)
			{
				list.Add(CampaignSceneNotificationHelper.GetBodyguardOfCulture(this.NewLeaderHero.Clan.Kingdom.MapFaction.Culture));
			}
			foreach (Hero hero in CampaignSceneNotificationHelper.GetMilitaryAudienceForHero(this.NewLeaderHero, false, false).Take<Hero>(4))
			{
				Equipment equipment3 = hero.CivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment3, false, false);
				list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, equipment3, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0005E87C File Offset: 0x0005CA7C
		public BecomeKingSceneNotificationItem(Hero newLeaderHero)
		{
			this.NewLeaderHero = newLeaderHero;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x0005E896 File Offset: 0x0005CA96
		private bool IsAudienceFemale(int indexOfAudience)
		{
			return indexOfAudience == 2 || indexOfAudience == 5 || indexOfAudience - 11 <= 2;
		}

		// Token: 0x0400068D RID: 1677
		private const int NumberOfAudience = 14;

		// Token: 0x0400068E RID: 1678
		private const int NumberOfGuards = 2;

		// Token: 0x0400068F RID: 1679
		private const int NumberOfCompanions = 4;

		// Token: 0x04000691 RID: 1681
		private readonly CampaignTime _creationCampaignTime;
	}
}
