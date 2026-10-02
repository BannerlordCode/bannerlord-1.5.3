using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C0 RID: 192
	public class ProEmpireConspiracyBeginsSceneNotificationItem : EmpireConspiracySupportsSceneNotificationItemBase
	{
		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x0005FB48 File Offset: 0x0005DD48
		public override TextObject TitleText
		{
			get
			{
				TextObject textObject = GameTexts.FindText("str_empire_conspiracy_supports_proempire", null);
				textObject.SetTextVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(CampaignTime.Now));
				textObject.SetTextVariable("YEAR", CampaignTime.Now.GetYear);
				return textObject;
			}
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0005FB8F File Offset: 0x0005DD8F
		public ProEmpireConspiracyBeginsSceneNotificationItem(Hero kingHero)
			: base(kingHero)
		{
		}
	}
}
