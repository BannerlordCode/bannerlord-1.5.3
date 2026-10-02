using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000238 RID: 568
	public sealed class MapNotificationHotKeyCategory : GameKeyContext
	{
		// Token: 0x0600215D RID: 8541 RVA: 0x000762F3 File Offset: 0x000744F3
		public MapNotificationHotKeyCategory()
			: base("MapNotificationHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x00076309 File Offset: 0x00074509
		private void RegisterHotKeys()
		{
			base.RegisterHotKey(new HotKey("RemoveNotification", "MapNotificationHotKeyCategory", InputKey.ControllerRUp, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x04000C7C RID: 3196
		public const string CategoryId = "MapNotificationHotKeyCategory";

		// Token: 0x04000C7D RID: 3197
		public const string RemoveNotification = "RemoveNotification";
	}
}
