using System;
using Galaxy.Api;
using TaleWorlds.Library;

namespace TaleWorlds.PlatformService.GOG
{
	// Token: 0x0200000C RID: 12
	public class GogServicesConnectionStateListener : GlobalGogServicesConnectionStateListener
	{
		// Token: 0x0600007F RID: 127 RVA: 0x00003250 File Offset: 0x00001450
		public override void OnConnectionStateChange(GogServicesConnectionState connected)
		{
			Debug.Print("Connection state to GOG services changed to " + connected, 0, Debug.DebugColor.White, 17592186044416UL);
		}
	}
}
