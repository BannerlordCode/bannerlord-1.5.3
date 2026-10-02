using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000CD RID: 205
	public class NavalDeathSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060014C0 RID: 5312 RVA: 0x000618D8 File Offset: 0x0005FAD8
		public Hero DeadHero { get; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x000618E0 File Offset: 0x0005FAE0
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_main_hero_naval_battle_death";
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x000618E7 File Offset: 0x0005FAE7
		// (set) Token: 0x060014C3 RID: 5315 RVA: 0x000618EF File Offset: 0x0005FAEF
		public KillCharacterAction.KillCharacterActionDetail KillDetail { get; private set; }

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x000618F8 File Offset: 0x0005FAF8
		public override SceneNotificationData.NotificationSceneProperties SceneProperties
		{
			get
			{
				return new SceneNotificationData.NotificationSceneProperties
				{
					InitializePhysics = true,
					DisableStaticShadows = true,
					OverriddenWaterStrength = null
				};
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x060014C5 RID: 5317 RVA: 0x0006192C File Offset: 0x0005FB2C
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("NAME", this.DeadHero.Name);
				if (this.KillDetail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle)
				{
					return GameTexts.FindText("str_main_hero_battle_death", null);
				}
				if (this.KillDetail == KillCharacterAction.KillCharacterActionDetail.DiedInLabor)
				{
					return GameTexts.FindText("str_main_hero_battle_death_in_labor", null);
				}
				if (this.KillDetail == KillCharacterAction.KillCharacterActionDetail.Executed)
				{
					return GameTexts.FindText("str_main_hero_battle_executed", null);
				}
				if (this.KillDetail == KillCharacterAction.KillCharacterActionDetail.Murdered)
				{
					return GameTexts.FindText("str_main_hero_battle_murdered", null);
				}
				return GameTexts.FindText("str_family_member_death", null);
			}
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x000619DA File Offset: 0x0005FBDA
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			return Array.Empty<SceneNotificationData.SceneNotificationCharacter>();
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x000619E1 File Offset: 0x0005FBE1
		public override SceneNotificationData.SceneNotificationShip[] GetShips()
		{
			return Array.Empty<SceneNotificationData.SceneNotificationShip>();
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x000619E8 File Offset: 0x0005FBE8
		public NavalDeathSceneNotificationItem(Hero deadHero, CampaignTime creationTime, KillCharacterAction.KillCharacterActionDetail killDetail)
		{
			this.DeadHero = deadHero;
			this._creationCampaignTime = creationTime;
			this.KillDetail = killDetail;
		}

		// Token: 0x040006E1 RID: 1761
		private readonly CampaignTime _creationCampaignTime;
	}
}
