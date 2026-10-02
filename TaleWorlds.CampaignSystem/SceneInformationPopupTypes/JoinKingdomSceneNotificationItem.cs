using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C7 RID: 199
	public class JoinKingdomSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x00060940 File Offset: 0x0005EB40
		public Clan NewMemberClan { get; }

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001496 RID: 5270 RVA: 0x00060948 File Offset: 0x0005EB48
		public Kingdom KingdomToUse { get; }

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x00060950 File Offset: 0x0005EB50
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_factionjoin";
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x00060957 File Offset: 0x0005EB57
		public override SceneNotificationData.RelevantContextType RelevantContext
		{
			get
			{
				return SceneNotificationData.RelevantContextType.Any;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x0006095C File Offset: 0x0005EB5C
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("CLAN_NAME", this.NewMemberClan.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("KINGDOM_FORMALNAME", this.KingdomToUse.FormalName);
				return GameTexts.FindText("str_new_faction_member", null);
			}
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x000609CB File Offset: 0x0005EBCB
		public override Banner[] GetBanners()
		{
			return new Banner[]
			{
				this.KingdomToUse.Banner,
				this.KingdomToUse.Banner
			};
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x000609F0 File Offset: 0x0005EBF0
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Hero leader = this.NewMemberClan.Leader;
			Equipment equipment = leader.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(leader, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			foreach (Hero hero in CampaignSceneNotificationHelper.GetMilitaryAudienceForKingdom(this.KingdomToUse, true).Take<Hero>(5))
			{
				Equipment equipment2 = hero.CivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, true, false);
				list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x00060AC4 File Offset: 0x0005ECC4
		public JoinKingdomSceneNotificationItem(Clan newMember, Kingdom kingdom)
		{
			this.NewMemberClan = newMember;
			this.KingdomToUse = kingdom;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x040006C7 RID: 1735
		private const int NumberOfKingdomMembers = 5;

		// Token: 0x040006CA RID: 1738
		private readonly CampaignTime _creationCampaignTime;
	}
}
