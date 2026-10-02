using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C9 RID: 201
	public class KingdomDestroyedSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x00060C8E File Offset: 0x0005EE8E
		public Kingdom DestroyedKingdom { get; }

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x060014A6 RID: 5286 RVA: 0x00060C96 File Offset: 0x0005EE96
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_enemykingdom_destroyed";
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x00060CA0 File Offset: 0x0005EEA0
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("FORMAL_NAME", this.DestroyedKingdom.FormalName);
				return GameTexts.FindText("str_kingdom_destroyed_scene_notification", null);
			}
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x00060CFA File Offset: 0x0005EEFA
		public override Banner[] GetBanners()
		{
			return new Banner[] { this.DestroyedKingdom.Banner };
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x00060D10 File Offset: 0x0005EF10
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			for (int i = 0; i < 2; i++)
			{
				CharacterObject randomTroopForCulture = CampaignSceneNotificationHelper.GetRandomTroopForCulture(this.DestroyedKingdom.Culture);
				Equipment equipment = randomTroopForCulture.FirstBattleEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
				BodyProperties bodyProperties = randomTroopForCulture.GetBodyProperties(equipment, MBRandom.RandomInt(100));
				list.Add(new SceneNotificationData.SceneNotificationCharacter(randomTroopForCulture, equipment, bodyProperties, false, uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x00060D7F File Offset: 0x0005EF7F
		public KingdomDestroyedSceneNotificationItem(Kingdom destroyedKingdom, CampaignTime creationTime)
		{
			this.DestroyedKingdom = destroyedKingdom;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x040006CE RID: 1742
		private const int NumberOfDeadTroops = 2;

		// Token: 0x040006D0 RID: 1744
		private readonly CampaignTime _creationCampaignTime;
	}
}
