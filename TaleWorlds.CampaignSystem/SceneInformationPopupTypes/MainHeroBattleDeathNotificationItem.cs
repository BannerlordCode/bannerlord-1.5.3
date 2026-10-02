using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000CA RID: 202
	public class MainHeroBattleDeathNotificationItem : SceneNotificationData
	{
		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x060014AB RID: 5291 RVA: 0x00060D95 File Offset: 0x0005EF95
		public Hero DeadHero { get; }

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x060014AC RID: 5292 RVA: 0x00060D9D File Offset: 0x0005EF9D
		public CultureObject KillerCulture { get; }

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x060014AD RID: 5293 RVA: 0x00060DA5 File Offset: 0x0005EFA5
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_main_hero_battle_death";
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x060014AE RID: 5294 RVA: 0x00060DAC File Offset: 0x0005EFAC
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("NAME", this.DeadHero.Name);
				return GameTexts.FindText("str_main_hero_battle_death", null);
			}
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x00060E08 File Offset: 0x0005F008
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.DeadHero.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.DeadHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			for (int i = 0; i < 23; i++)
			{
				CharacterObject randomTroopForCulture = CampaignSceneNotificationHelper.GetRandomTroopForCulture((this.KillerCulture != null && (float)i > 11.5f) ? this.KillerCulture : this.DeadHero.MapFaction.Culture);
				Equipment equipment2 = randomTroopForCulture.FirstBattleEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, false, false);
				BodyProperties bodyProperties = randomTroopForCulture.GetBodyProperties(equipment2, MBRandom.RandomInt(100));
				list.Add(new SceneNotificationData.SceneNotificationCharacter(randomTroopForCulture, equipment2, bodyProperties, false, uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00060ED7 File Offset: 0x0005F0D7
		public MainHeroBattleDeathNotificationItem(Hero deadHero, CultureObject killerCulture = null)
		{
			this.DeadHero = deadHero;
			this.KillerCulture = killerCulture;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x040006D1 RID: 1745
		private const int NumberOfCorpses = 23;

		// Token: 0x040006D4 RID: 1748
		private readonly CampaignTime _creationCampaignTime;
	}
}
