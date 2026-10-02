using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C5 RID: 197
	public class HeirComingOfAgeSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x0005FF33 File Offset: 0x0005E133
		public Hero MentorHero { get; }

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001473 RID: 5235 RVA: 0x0005FF3B File Offset: 0x0005E13B
		public Hero HeroCameOfAge { get; }

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x0005FF43 File Offset: 0x0005E143
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_heir_coming_of_age";
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001475 RID: 5237 RVA: 0x0005FF4C File Offset: 0x0005E14C
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("HERO_NAME", this.HeroCameOfAge.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_hero_came_of_age", null);
			}
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0005FFA8 File Offset: 0x0005E1A8
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.MentorHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.MentorHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			string childStageEquipmentIDFromCulture = CampaignSceneNotificationHelper.GetChildStageEquipmentIDFromCulture(this.HeroCameOfAge.Culture);
			Equipment equipment2 = MBObjectManager.Instance.GetObject<MBEquipmentRoster>(childStageEquipmentIDFromCulture).DefaultEquipment.Clone(false);
			BodyProperties bodyProperties = new BodyProperties(new DynamicBodyProperties(6f, this.HeroCameOfAge.Weight, this.HeroCameOfAge.Build), this.HeroCameOfAge.StaticBodyProperties);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, equipment2, false, bodyProperties, uint.MaxValue, uint.MaxValue, false));
			BodyProperties bodyProperties2 = new BodyProperties(new DynamicBodyProperties(14f, this.HeroCameOfAge.Weight, this.HeroCameOfAge.Build), this.HeroCameOfAge.StaticBodyProperties);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, equipment2, false, bodyProperties2, uint.MaxValue, uint.MaxValue, false));
			Equipment equipment3 = this.HeroCameOfAge.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment3, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, equipment3, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			return list.ToArray();
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x000600F2 File Offset: 0x0005E2F2
		public HeirComingOfAgeSceneNotificationItem(Hero mentorHero, Hero heroCameOfAge, CampaignTime creationTime)
		{
			this.MentorHero = mentorHero;
			this.HeroCameOfAge = heroCameOfAge;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x040006B3 RID: 1715
		private readonly CampaignTime _creationCampaignTime;
	}
}
