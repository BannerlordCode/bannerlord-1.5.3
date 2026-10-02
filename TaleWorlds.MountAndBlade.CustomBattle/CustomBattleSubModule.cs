using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;
using TaleWorlds.MountAndBlade.GauntletUI.SceneNotification;
using TaleWorlds.MountAndBlade.View.CustomBattle;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x02000005 RID: 5
	public class CustomBattleSubModule : MBSubModuleBase
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00004808 File Offset: 0x00002A08
		protected override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();
			CustomBattleFactory.RegisterProvider<CustomBattleProvider>();
			TauntUsageManager.Initialize();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000481B File Offset: 0x00002A1B
		protected override void OnApplicationTick(float dt)
		{
			base.OnApplicationTick(dt);
			if (!this._initialized && GauntletSceneNotification.Current != null)
			{
				if (!Utilities.CommandLineArgumentExists("VisualTests"))
				{
					GauntletSceneNotification.Current.RegisterContextProvider(new CustomBattleSceneNotificationContextProvider());
				}
				this._initialized = true;
			}
		}

		// Token: 0x0400002C RID: 44
		private bool _initialized;
	}
}
