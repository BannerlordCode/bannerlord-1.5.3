using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BB RID: 187
	public class DeathOldAgeSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x0005F2E6 File Offset: 0x0005D4E6
		public Hero DeadHero { get; }

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x0005F2EE File Offset: 0x0005D4EE
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_death_old_age";
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x0005F2F8 File Offset: 0x0005D4F8
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("NAME", this.DeadHero.Name);
				return GameTexts.FindText("str_died_of_old_age", null);
			}
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0005F352 File Offset: 0x0005D552
		public override Banner[] GetBanners()
		{
			return new Banner[] { this.DeadHero.ClanBanner };
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0005F368 File Offset: 0x0005D568
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.DeadHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.DeadHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			foreach (Hero hero in CampaignSceneNotificationHelper.GetMilitaryAudienceForHero(this.DeadHero, true, false).Take<Hero>(5))
			{
				Equipment equipment2 = hero.CivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, false, false);
				list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0005F438 File Offset: 0x0005D638
		public DeathOldAgeSceneNotificationItem(Hero deadHero)
		{
			this.DeadHero = deadHero;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x04000699 RID: 1689
		private const int NumberOfAudienceHeroes = 5;

		// Token: 0x0400069B RID: 1691
		private readonly CampaignTime _creationCampaignTime;
	}
}
