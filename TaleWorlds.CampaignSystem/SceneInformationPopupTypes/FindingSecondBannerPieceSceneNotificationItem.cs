using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C2 RID: 194
	public class FindingSecondBannerPieceSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x0005FC26 File Offset: 0x0005DE26
		public Hero PlayerHero { get; }

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001460 RID: 5216 RVA: 0x0005FC2E File Offset: 0x0005DE2E
		public override string SceneID
		{
			get
			{
				return "scn_second_banner_piece_notification";
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x0005FC38 File Offset: 0x0005DE38
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_second_banner_piece_found", null);
			}
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0005FC7D File Offset: 0x0005DE7D
		public override Banner[] GetBanners()
		{
			return new Banner[] { this.PlayerHero.ClanBanner };
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0005FC93 File Offset: 0x0005DE93
		public FindingSecondBannerPieceSceneNotificationItem(Hero playerHero)
		{
			this.PlayerHero = playerHero;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x040006AB RID: 1707
		private readonly CampaignTime _creationCampaignTime;
	}
}
