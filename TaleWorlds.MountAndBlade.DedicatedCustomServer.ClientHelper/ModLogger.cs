using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.DedicatedCustomServer.ClientHelper
{
	// Token: 0x02000006 RID: 6
	internal static class ModLogger
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00003005 File Offset: 0x00001205
		public static void Log(string message, int logLevel = 0, Debug.DebugColor color = Debug.DebugColor.Green)
		{
			Debug.Print("DCS Client Helper :: " + message, logLevel, color, 17592186044416UL);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003022 File Offset: 0x00001222
		public static void Warn(string message)
		{
			ModLogger.Log(message, 0, Debug.DebugColor.Yellow);
		}
	}
}
