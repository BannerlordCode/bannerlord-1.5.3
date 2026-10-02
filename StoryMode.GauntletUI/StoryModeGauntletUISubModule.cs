using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.View.Map;
using StoryMode.ViewModelCollection.Map;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;

namespace StoryMode.GauntletUI
{
	// Token: 0x02000003 RID: 3
	public class StoryModeGauntletUISubModule : MBSubModuleBase
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002214 File Offset: 0x00000414
		public override void OnGameInitializationFinished(Game game)
		{
			base.OnGameInitializationFinished(game);
			if (game.GameType.RequiresTutorial)
			{
				GauntletTutorialSystem.OnInitialize();
				ScreenManager.OnPushScreen += this.OnScreenManagerPushScreen;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002240 File Offset: 0x00000440
		private void OnScreenManagerPushScreen(ScreenBase pushedScreen)
		{
			MapScreen mapScreen;
			if (!this._registered && (mapScreen = pushedScreen as MapScreen) != null)
			{
				mapScreen.MapNotificationView.RegisterMapNotificationType(typeof(ConspiracyQuestMapNotification), typeof(ConspiracyQuestMapNotificationItemVM));
				this._registered = true;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002285 File Offset: 0x00000485
		public override void OnGameEnd(Game game)
		{
			base.OnGameEnd(game);
			if (game.GameType.RequiresTutorial)
			{
				GauntletTutorialSystem.OnUnload();
				ScreenManager.OnPushScreen -= this.OnScreenManagerPushScreen;
			}
			this._registered = false;
		}

		// Token: 0x04000004 RID: 4
		private bool _registered;
	}
}
