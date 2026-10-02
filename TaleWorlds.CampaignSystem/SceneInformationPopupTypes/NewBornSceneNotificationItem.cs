using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000D0 RID: 208
	public class NewBornSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x060014D5 RID: 5333 RVA: 0x00061CD5 File Offset: 0x0005FED5
		public Hero MaleHero { get; }

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x060014D6 RID: 5334 RVA: 0x00061CDD File Offset: 0x0005FEDD
		public Hero FemaleHero { get; }

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x060014D7 RID: 5335 RVA: 0x00061CE5 File Offset: 0x0005FEE5
		public override string SceneID
		{
			get
			{
				return "scn_born_baby";
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x060014D8 RID: 5336 RVA: 0x00061CEC File Offset: 0x0005FEEC
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("FATHER_NAME", this.MaleHero.Name);
				GameTexts.SetVariable("MOTHER_NAME", this.FemaleHero.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_baby_born", null);
			}
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x00061D5C File Offset: 0x0005FF5C
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

		// Token: 0x060014DA RID: 5338 RVA: 0x00061E44 File Offset: 0x00060044
		public NewBornSceneNotificationItem(Hero maleHero, Hero femaleHero, CampaignTime creationTime)
		{
			this.MaleHero = maleHero;
			this.FemaleHero = femaleHero;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x040006EA RID: 1770
		private readonly CampaignTime _creationCampaignTime;
	}
}
