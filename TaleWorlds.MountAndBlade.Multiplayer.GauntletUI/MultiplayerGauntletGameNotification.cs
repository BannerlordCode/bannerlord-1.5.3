using System;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI
{
	// Token: 0x02000004 RID: 4
	public class MultiplayerGauntletGameNotification : GauntletGameNotification
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002203 File Offset: 0x00000403
		protected override string MovieName
		{
			get
			{
				return "MultiplayerGameNotificationUI";
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000220A File Offset: 0x0000040A
		public new static void Initialize()
		{
			GauntletGameNotification gauntletGameNotification = GauntletGameNotification.Current;
			if (gauntletGameNotification != null)
			{
				gauntletGameNotification.OnFinalize();
			}
			GauntletGameNotification.Current = new MultiplayerGauntletGameNotification();
			ScreenManager.AddGlobalLayer(GauntletGameNotification.Current, false);
			GauntletGameNotification.Current.RegisterEvents();
		}
	}
}
