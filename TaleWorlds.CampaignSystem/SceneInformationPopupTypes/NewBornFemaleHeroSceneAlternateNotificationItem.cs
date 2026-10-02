using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000CE RID: 206
	public class NewBornFemaleHeroSceneAlternateNotificationItem : SceneNotificationData
	{
		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x060014C9 RID: 5321 RVA: 0x00061A05 File Offset: 0x0005FC05
		public Hero MaleHero { get; }

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x00061A0D File Offset: 0x0005FC0D
		public Hero FemaleHero { get; }

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x00061A15 File Offset: 0x0005FC15
		public override string SceneID
		{
			get
			{
				return "scn_born_baby_female_hero2";
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x060014CC RID: 5324 RVA: 0x00061A1C File Offset: 0x0005FC1C
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("MOTHER_NAME", this.FemaleHero.Name);
				return GameTexts.FindText("str_baby_born_only_mother", null);
			}
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00061A78 File Offset: 0x0005FC78
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.FemaleHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, true);
			CharacterObject characterObject = CharacterObject.All.First<CharacterObject>((CharacterObject h) => h.StringId == "cutscene_midwife");
			Equipment equipment2 = characterObject.FirstCivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, false, false);
			list.Add(new SceneNotificationData.SceneNotificationCharacter(null, null, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.FemaleHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(characterObject, equipment2, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			return list.ToArray();
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00061B3D File Offset: 0x0005FD3D
		public NewBornFemaleHeroSceneAlternateNotificationItem(Hero maleHero, Hero femaleHero, CampaignTime creationTime)
		{
			this.MaleHero = maleHero;
			this.FemaleHero = femaleHero;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x040006E4 RID: 1764
		private readonly CampaignTime _creationCampaignTime;
	}
}
