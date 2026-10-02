using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BA RID: 186
	public class ClanMemberWarDeathSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x0005F181 File Offset: 0x0005D381
		public Hero DeadHero { get; }

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x0005F189 File Offset: 0x0005D389
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_family_member_death_war";
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x0005F190 File Offset: 0x0005D390
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("NAME", this.DeadHero.Name);
				return GameTexts.FindText("str_family_member_death_war", null);
			}
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0005F1EA File Offset: 0x0005D3EA
		public override Banner[] GetBanners()
		{
			return new Banner[] { this.DeadHero.ClanBanner };
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0005F200 File Offset: 0x0005D400
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.DeadHero.CivilianEquipment.Clone(false);
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

		// Token: 0x06001439 RID: 5177 RVA: 0x0005F2D0 File Offset: 0x0005D4D0
		public ClanMemberWarDeathSceneNotificationItem(Hero deadHero, CampaignTime creationTime)
		{
			this.DeadHero = deadHero;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x04000696 RID: 1686
		private const int NumberOfAudienceHeroes = 5;

		// Token: 0x04000698 RID: 1688
		private readonly CampaignTime _creationCampaignTime;
	}
}
