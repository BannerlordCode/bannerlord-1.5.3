using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BF RID: 191
	public class AntiEmpireConspiracyBeginsSceneNotificationItem : EmpireConspiracySupportsSceneNotificationItemBase
	{
		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x0005FA8C File Offset: 0x0005DC8C
		public override TextObject TitleText
		{
			get
			{
				List<TextObject> list = new List<TextObject>();
				foreach (Kingdom kingdom in this._antiEmpireFactions)
				{
					list.Add(kingdom.InformalName);
				}
				TextObject textObject = GameTexts.FindText("str_empire_conspiracy_supports_antiempire", null);
				textObject.SetTextVariable("FACTION_NAMES", GameTexts.GameTextHelper.MergeTextObjectsWithComma(list, true));
				textObject.SetTextVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(CampaignTime.Now));
				textObject.SetTextVariable("YEAR", CampaignTime.Now.GetYear);
				return textObject;
			}
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0005FB38 File Offset: 0x0005DD38
		public AntiEmpireConspiracyBeginsSceneNotificationItem(Hero kingHero, List<Kingdom> antiEmpireFactions)
			: base(kingHero)
		{
			this._antiEmpireFactions = antiEmpireFactions;
		}

		// Token: 0x040006A6 RID: 1702
		private readonly List<Kingdom> _antiEmpireFactions;
	}
}
