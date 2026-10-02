using System;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.GauntletUI.SceneNotification;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI
{
	// Token: 0x02000005 RID: 5
	public class MultiplayerGauntletUISubModule : MBSubModuleBase
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002243 File Offset: 0x00000443
		protected override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			if (!this._initialized)
			{
				if (!Utilities.CommandLineArgumentExists("VisualTests"))
				{
					GauntletSceneNotification.Current.RegisterContextProvider(new MultiplayerSceneNotificationContextProvider());
				}
				this._initialized = true;
			}
		}

		// Token: 0x04000005 RID: 5
		private bool _initialized;
	}
}
