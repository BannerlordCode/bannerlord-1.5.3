using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C4 RID: 196
	public class HeirComingOfAgeFemaleSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x0005FD57 File Offset: 0x0005DF57
		public Hero MentorHero { get; }

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x0600146D RID: 5229 RVA: 0x0005FD5F File Offset: 0x0005DF5F
		public Hero HeroCameOfAge { get; }

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x0600146E RID: 5230 RVA: 0x0005FD67 File Offset: 0x0005DF67
		public override string SceneID
		{
			get
			{
				return "scn_hero_come_of_age_female";
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x0600146F RID: 5231 RVA: 0x0005FD70 File Offset: 0x0005DF70
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

		// Token: 0x06001470 RID: 5232 RVA: 0x0005FDCC File Offset: 0x0005DFCC
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.MentorHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.MentorHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			string childStageEquipmentIDFromCulture = CampaignSceneNotificationHelper.GetChildStageEquipmentIDFromCulture(this.HeroCameOfAge.Culture);
			Equipment equipment2 = MBObjectManager.Instance.GetObject<MBEquipmentRoster>(childStageEquipmentIDFromCulture).DefaultEquipment.Clone(false);
			BodyProperties bodyProperties = new BodyProperties(new DynamicBodyProperties(6f, this.HeroCameOfAge.Weight, this.HeroCameOfAge.Build), this.HeroCameOfAge.StaticBodyProperties);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, equipment2, false, bodyProperties, uint.MaxValue, uint.MaxValue, false));
			BodyProperties bodyProperties2 = new BodyProperties(new DynamicBodyProperties(14f, this.HeroCameOfAge.Weight, this.HeroCameOfAge.Build), this.HeroCameOfAge.StaticBodyProperties);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, equipment2, false, bodyProperties2, uint.MaxValue, uint.MaxValue, false));
			Equipment equipment3 = this.HeroCameOfAge.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment3, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, equipment3, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			return list.ToArray();
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0005FF16 File Offset: 0x0005E116
		public HeirComingOfAgeFemaleSceneNotificationItem(Hero mentorHero, Hero heroCameOfAge, CampaignTime creationTime)
		{
			this.MentorHero = mentorHero;
			this.HeroCameOfAge = heroCameOfAge;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x040006B0 RID: 1712
		private readonly CampaignTime _creationCampaignTime;
	}
}
