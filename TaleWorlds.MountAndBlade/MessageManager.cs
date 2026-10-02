using System;
using System.Diagnostics;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E9 RID: 489
	public static class MessageManager
	{
		// Token: 0x06001CD4 RID: 7380 RVA: 0x00062362 File Offset: 0x00060562
		public static void DisplayMessage(string message)
		{
			MBAPI.IMBMessageManager.DisplayMessage(message);
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x0006236F File Offset: 0x0006056F
		public static void DisplayMessage(string message, uint color)
		{
			MBAPI.IMBMessageManager.DisplayMessageWithColor(message, color);
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x00062380 File Offset: 0x00060580
		[Conditional("DEBUG")]
		public static void DisplayDebugMessage(string message)
		{
			if (message.Length > 4 && message.Substring(0, 4).Equals("[DEBUG]"))
			{
				message = message.Substring(4);
			}
			MBAPI.IMBMessageManager.DisplayMessageWithColor("[DEBUG]: " + message, 4294936712U);
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x000623D0 File Offset: 0x000605D0
		public static void DisplayMultilineMessage(string message, uint color)
		{
			if (message.Contains("\n"))
			{
				string[] array = message.Split(new char[] { '\n' });
				for (int i = 0; i < array.Length; i++)
				{
					MBAPI.IMBMessageManager.DisplayMessageWithColor(array[i], color);
				}
				return;
			}
			MBAPI.IMBMessageManager.DisplayMessageWithColor(message, color);
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x00062425 File Offset: 0x00060625
		public static void EraseMessageLines()
		{
			MBAPI.IMBWindowManager.EraseMessageLines();
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x00062431 File Offset: 0x00060631
		public static void SetMessageManager(MessageManagerBase messageManager)
		{
			MBAPI.IMBMessageManager.SetMessageManager(messageManager);
		}
	}
}
