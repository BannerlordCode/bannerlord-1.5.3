using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000CF RID: 207
	public class NewBornFemaleHeroSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x060014CF RID: 5327 RVA: 0x00061B5A File Offset: 0x0005FD5A
		public Hero MaleHero { get; }

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x060014D0 RID: 5328 RVA: 0x00061B62 File Offset: 0x0005FD62
		public Hero FemaleHero { get; }

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x060014D1 RID: 5329 RVA: 0x00061B6A File Offset: 0x0005FD6A
		public override string SceneID
		{
			get
			{
				return "scn_born_baby_female_hero";
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x060014D2 RID: 5330 RVA: 0x00061B74 File Offset: 0x0005FD74
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("MOTHER_NAME", this.FemaleHero.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_baby_born_only_mother", null);
			}
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x00061BD0 File Offset: 0x0005FDD0
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			CharacterObject characterObject = CharacterObject.All.First<CharacterObject>((CharacterObject h) => h.StringId == "cutscene_midwife");
			Equipment equipment = this.MaleHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			Equipment equipment2 = this.FemaleHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, true, true);
			Equipment equipment3 = characterObject.FirstCivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment3, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.MaleHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.FemaleHero, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(characterObject, equipment3, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			return list.ToArray();
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x00061CB8 File Offset: 0x0005FEB8
		public NewBornFemaleHeroSceneNotificationItem(Hero maleHero, Hero femaleHero, CampaignTime creationTime)
		{
			this.MaleHero = maleHero;
			this.FemaleHero = femaleHero;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x040006E7 RID: 1767
		private readonly CampaignTime _creationCampaignTime;
	}
}
