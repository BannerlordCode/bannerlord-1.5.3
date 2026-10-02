using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000D2 RID: 210
	public class SupportedFactionDefeatedSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x00062147 File Offset: 0x00060347
		public Kingdom Faction { get; }

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x060014E3 RID: 5347 RVA: 0x0006214F File Offset: 0x0006034F
		public bool PlayerWantsRestore { get; }

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x060014E4 RID: 5348 RVA: 0x00062157 File Offset: 0x00060357
		public override string SceneID
		{
			get
			{
				return "scn_supported_faction_defeated_notification";
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x060014E5 RID: 5349 RVA: 0x00062160 File Offset: 0x00060360
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("FORMAL_NAME", this.Faction.FormalName);
				GameTexts.SetVariable("PLAYER_WANTS_RESTORE", this.PlayerWantsRestore ? 1 : 0);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_supported_faction_defeated", null);
			}
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x000621D0 File Offset: 0x000603D0
		public override Banner[] GetBanners()
		{
			return new Banner[]
			{
				this.Faction.Banner,
				this.Faction.Banner
			};
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x000621F4 File Offset: 0x000603F4
		public SupportedFactionDefeatedSceneNotificationItem(Kingdom faction, bool playerWantsRestore)
		{
			this.Faction = faction;
			this.PlayerWantsRestore = playerWantsRestore;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x040006F1 RID: 1777
		private readonly CampaignTime _creationCampaignTime;
	}
}
