using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C8 RID: 200
	public class KingdomCreatedSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x00060AE5 File Offset: 0x0005ECE5
		public Kingdom NewKingdom { get; }

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x00060AED File Offset: 0x0005ECED
		public override string SceneID
		{
			get
			{
				return "scn_kingdom_made";
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x0600149F RID: 5279 RVA: 0x00060AF4 File Offset: 0x0005ECF4
		public override bool PauseActiveState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x060014A0 RID: 5280 RVA: 0x00060AF8 File Offset: 0x0005ECF8
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("KINGDOM_NAME", this.NewKingdom.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("LEADER_NAME", this.NewKingdom.Leader.Name);
				return GameTexts.FindText("str_kingdom_created", null);
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x00060B6C File Offset: 0x0005ED6C
		public override TextObject AffirmativeText
		{
			get
			{
				return GameTexts.FindText("str_ok", null);
			}
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x00060B79 File Offset: 0x0005ED79
		public override Banner[] GetBanners()
		{
			return new Banner[]
			{
				this.NewKingdom.Banner,
				this.NewKingdom.Banner
			};
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x00060BA0 File Offset: 0x0005EDA0
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Hero leader = this.NewKingdom.Leader;
			Equipment equipment = leader.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(leader, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			foreach (Hero hero in CampaignSceneNotificationHelper.GetMilitaryAudienceForKingdom(this.NewKingdom, false).Take<Hero>(5))
			{
				Equipment equipment2 = hero.CivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, true, false);
				list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x00060C74 File Offset: 0x0005EE74
		public KingdomCreatedSceneNotificationItem(Kingdom newKingdom)
		{
			this.NewKingdom = newKingdom;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x040006CB RID: 1739
		private const int NumberOfKingdomMemberAudience = 5;

		// Token: 0x040006CD RID: 1741
		private readonly CampaignTime _creationCampaignTime;
	}
}
