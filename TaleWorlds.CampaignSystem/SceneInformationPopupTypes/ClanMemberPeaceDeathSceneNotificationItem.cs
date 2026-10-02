using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B9 RID: 185
	public class ClanMemberPeaceDeathSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x0005EFC3 File Offset: 0x0005D1C3
		public Hero DeadHero { get; }

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x0005EFCB File Offset: 0x0005D1CB
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_family_member_death";
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x0005EFD2 File Offset: 0x0005D1D2
		// (set) Token: 0x0600142F RID: 5167 RVA: 0x0005EFDA File Offset: 0x0005D1DA
		public KillCharacterAction.KillCharacterActionDetail KillDetail { get; private set; }

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x0005EFE4 File Offset: 0x0005D1E4
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("NAME", this.DeadHero.Name);
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

		// Token: 0x06001431 RID: 5169 RVA: 0x0005F07D File Offset: 0x0005D27D
		public override Banner[] GetBanners()
		{
			return new Banner[] { this.DeadHero.ClanBanner };
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0005F094 File Offset: 0x0005D294
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			Equipment equipment = this.DeadHero.CivilianEquipment.Clone(false);
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.DeadHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			foreach (Hero hero in CampaignSceneNotificationHelper.GetMilitaryAudienceForHero(this.DeadHero, true, false).Take<Hero>(5))
			{
				Equipment equipment2 = hero.CivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, false, false);
				list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0005F164 File Offset: 0x0005D364
		public ClanMemberPeaceDeathSceneNotificationItem(Hero deadHero, CampaignTime creationTime, KillCharacterAction.KillCharacterActionDetail killDetail)
		{
			this.DeadHero = deadHero;
			this._creationCampaignTime = creationTime;
			this.KillDetail = killDetail;
		}

		// Token: 0x04000692 RID: 1682
		private const int NumberOfAudienceHeroes = 5;

		// Token: 0x04000695 RID: 1685
		private readonly CampaignTime _creationCampaignTime;
	}
}
