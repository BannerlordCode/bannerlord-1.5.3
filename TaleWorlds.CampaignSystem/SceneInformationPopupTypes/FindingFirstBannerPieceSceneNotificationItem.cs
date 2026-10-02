using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C1 RID: 193
	public class FindingFirstBannerPieceSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x0005FB98 File Offset: 0x0005DD98
		public Hero PlayerHero { get; }

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x0005FBA0 File Offset: 0x0005DDA0
		public override string SceneID
		{
			get
			{
				return "scn_first_banner_piece_notification";
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x0005FBA8 File Offset: 0x0005DDA8
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_first_banner_piece_found", null);
			}
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0005FBED File Offset: 0x0005DDED
		public override void OnCloseAction()
		{
			base.OnCloseAction();
			Action onCloseAction = this._onCloseAction;
			if (onCloseAction == null)
			{
				return;
			}
			onCloseAction();
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0005FC05 File Offset: 0x0005DE05
		public FindingFirstBannerPieceSceneNotificationItem(Hero playerHero, Action onCloseAction = null)
		{
			this.PlayerHero = playerHero;
			this._creationCampaignTime = CampaignTime.Now;
			this._onCloseAction = onCloseAction;
		}

		// Token: 0x040006A8 RID: 1704
		private readonly Action _onCloseAction;

		// Token: 0x040006A9 RID: 1705
		private readonly CampaignTime _creationCampaignTime;
	}
}
