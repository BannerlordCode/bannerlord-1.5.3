using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C3 RID: 195
	public class FindingThirdBannerPieceSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x0005FCAD File Offset: 0x0005DEAD
		public override string SceneID
		{
			get
			{
				return "scn_third_banner_piece_notification";
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001465 RID: 5221 RVA: 0x0005FCB4 File Offset: 0x0005DEB4
		public override bool IsAffirmativeOptionShown { get; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001466 RID: 5222 RVA: 0x0005FCBC File Offset: 0x0005DEBC
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_third_banner_piece_found", null);
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001467 RID: 5223 RVA: 0x0005FD01 File Offset: 0x0005DF01
		public override TextObject AffirmativeTitleText
		{
			get
			{
				return GameTexts.FindText("str_third_banner_piece_found_assembled", null);
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001468 RID: 5224 RVA: 0x0005FD0E File Offset: 0x0005DF0E
		public override TextObject AffirmativeText
		{
			get
			{
				return new TextObject("{=6mgapvxb}Assemble", null);
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001469 RID: 5225 RVA: 0x0005FD1B File Offset: 0x0005DF1B
		public override TextObject AffirmativeDescriptionText
		{
			get
			{
				return new TextObject("{=IRLB42FY}Assemble the dragon banner!", null);
			}
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0005FD28 File Offset: 0x0005DF28
		public override Banner[] GetBanners()
		{
			return new Banner[] { Hero.MainHero.ClanBanner };
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0005FD3D File Offset: 0x0005DF3D
		public FindingThirdBannerPieceSceneNotificationItem()
		{
			this.IsAffirmativeOptionShown = true;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x040006AD RID: 1709
		private readonly CampaignTime _creationCampaignTime;
	}
}
