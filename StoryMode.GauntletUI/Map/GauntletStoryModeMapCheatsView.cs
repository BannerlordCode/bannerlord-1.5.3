using System;
using SandBox.GauntletUI.Map;
using SandBox.View.Map;
using StoryMode.GameComponents.CampaignBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace StoryMode.GauntletUI.Map
{
	// Token: 0x0200004D RID: 77
	[OverrideView(typeof(MapCheatsView))]
	internal class GauntletStoryModeMapCheatsView : GauntletMapCheatsView
	{
		// Token: 0x06000171 RID: 369 RVA: 0x00004B38 File Offset: 0x00002D38
		protected override void CreateLayout()
		{
			base.CreateLayout();
			AchievementsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<AchievementsCampaignBehavior>();
			TextObject textObject;
			if (campaignBehavior == null || !campaignBehavior.CheckAchievementSystemActivity(out textObject))
			{
				this.EnableCheatMenu();
				return;
			}
			this._layerAsGauntletLayer.UIContext.ContextAlpha = 0f;
			InformationManager.ShowInquiry(new InquiryData(new TextObject("{=4Ygn4OGE}Enable Cheats", null).ToString(), new TextObject("{=YkbOfPRU}Enabling cheats will disable the achievements this game. Do you want to proceed?", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.EnableCheatMenu), new Action(this.RemoveCheatMenu), "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00004BF8 File Offset: 0x00002DF8
		private void EnableCheatMenu()
		{
			this._layerAsGauntletLayer.UIContext.ContextAlpha = 1f;
			AchievementsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<AchievementsCampaignBehavior>();
			TextObject textObject;
			if (campaignBehavior != null && campaignBehavior.CheckAchievementSystemActivity(out textObject) && campaignBehavior != null)
			{
				campaignBehavior.DeactivateAchievements(new TextObject("{=sO8Zh3ZH}Achievements are disabled due to cheat usage.", null), true, false);
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00004C48 File Offset: 0x00002E48
		private void RemoveCheatMenu()
		{
			base.MapScreen.CloseGameplayCheats();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00004C55 File Offset: 0x00002E55
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00004C71 File Offset: 0x00002E71
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}
	}
}
