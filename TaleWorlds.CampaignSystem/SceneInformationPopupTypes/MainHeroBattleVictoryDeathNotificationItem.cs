using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000CB RID: 203
	public class MainHeroBattleVictoryDeathNotificationItem : SceneNotificationData
	{
		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x00060EF8 File Offset: 0x0005F0F8
		public Hero DeadHero { get; }

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x00060F00 File Offset: 0x0005F100
		public List<CharacterObject> EncounterAllyCharacters { get; }

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060014B3 RID: 5299 RVA: 0x00060F08 File Offset: 0x0005F108
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_main_hero_battle_victory_death";
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060014B4 RID: 5300 RVA: 0x00060F10 File Offset: 0x0005F110
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

		// Token: 0x060014B5 RID: 5301 RVA: 0x00060F6C File Offset: 0x0005F16C
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.DeadHero.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.DeadHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			for (int i = 0; i < 2; i++)
			{
				CharacterObject randomTroopForCulture = CampaignSceneNotificationHelper.GetRandomTroopForCulture(this.DeadHero.MapFaction.Culture);
				Equipment equipment2 = randomTroopForCulture.FirstBattleEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, false, false);
				BodyProperties bodyProperties = randomTroopForCulture.GetBodyProperties(equipment2, MBRandom.RandomInt(100));
				list.Add(new SceneNotificationData.SceneNotificationCharacter(randomTroopForCulture, equipment2, bodyProperties, false, uint.MaxValue, uint.MaxValue, false));
			}
			List<CharacterObject> encounterAllyCharacters = this.EncounterAllyCharacters;
			foreach (CharacterObject characterObject in ((encounterAllyCharacters != null) ? encounterAllyCharacters.Take<CharacterObject>(3) : null))
			{
				if (characterObject.IsHero)
				{
					Equipment equipment3 = characterObject.HeroObject.BattleEquipment.Clone(false);
					CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment3, false, false);
					list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(characterObject.HeroObject, equipment3, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
				}
				else
				{
					Equipment equipment4 = characterObject.FirstBattleEquipment.Clone(false);
					CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment4, false, false);
					list.Add(new SceneNotificationData.SceneNotificationCharacter(characterObject, equipment4, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
				}
			}
			return list.ToArray();
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x000610F0 File Offset: 0x0005F2F0
		public MainHeroBattleVictoryDeathNotificationItem(Hero deadHero, List<CharacterObject> encounterAllyCharacters)
		{
			this.DeadHero = deadHero;
			this.EncounterAllyCharacters = encounterAllyCharacters;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x040006D5 RID: 1749
		private const int NumberOfCorpses = 2;

		// Token: 0x040006D6 RID: 1750
		private const int NumberOfCompanions = 3;

		// Token: 0x040006D9 RID: 1753
		private readonly CampaignTime _creationCampaignTime;
	}
}
