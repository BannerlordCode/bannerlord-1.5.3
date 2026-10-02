using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200015D RID: 349
	[Serializable]
	public class ServerNotification
	{
		// Token: 0x17000325 RID: 805
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x0000F373 File Offset: 0x0000D573
		public ServerNotificationType Type { get; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0000F37B File Offset: 0x0000D57B
		public string Message { get; }

		// Token: 0x060009CD RID: 2509 RVA: 0x0000F383 File Offset: 0x0000D583
		public ServerNotification(ServerNotificationType type, string message)
		{
			this.Type = type;
			this.Message = message;
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0000F39C File Offset: 0x0000D59C
		public TextObject GetTextObjectOfMessage()
		{
			TextObject textObject;
			if (!GameTexts.TryGetText(this.Message, out textObject, null))
			{
				textObject = new TextObject("{=!}" + this.Message, null);
			}
			return textObject;
		}
	}
}
