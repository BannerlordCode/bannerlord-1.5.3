using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200022B RID: 555
	public sealed class ChatLogHotKeyCategory : GameKeyContext
	{
		// Token: 0x0600212C RID: 8492 RVA: 0x00074BE6 File Offset: 0x00072DE6
		public ChatLogHotKeyCategory()
			: base("ChatLogHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x00074C08 File Offset: 0x00072E08
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.Tab),
				new Key(InputKey.ControllerRUp)
			};
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.NumpadEnter)
			};
			list2.Add(new Key(InputKey.ControllerLOption));
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.ControllerRLeft)
			};
			base.RegisterHotKey(new HotKey("CycleChatTypes", "ChatLogHotKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("FinalizeChatAlternative", "ChatLogHotKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("SendMessage", "ChatLogHotKeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x00074CC0 File Offset: 0x00072EC0
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(6, "InitiateAllChat", "ChatLogHotKeyCategory", InputKey.T, GameKeyMainCategories.ChatCategory), true);
			base.RegisterGameKey(new GameKey(7, "InitiateTeamChat", "ChatLogHotKeyCategory", InputKey.Y, GameKeyMainCategories.ChatCategory), true);
			base.RegisterGameKey(new GameKey(8, "FinalizeChat", "ChatLogHotKeyCategory", InputKey.Enter, InputKey.ControllerLOption, GameKeyMainCategories.ChatCategory), true);
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x00074D2C File Offset: 0x00072F2C
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000B6B RID: 2923
		public const string CategoryId = "ChatLogHotKeyCategory";

		// Token: 0x04000B6C RID: 2924
		public const int InitiateAllChat = 6;

		// Token: 0x04000B6D RID: 2925
		public const int InitiateTeamChat = 7;

		// Token: 0x04000B6E RID: 2926
		public const int FinalizeChat = 8;

		// Token: 0x04000B6F RID: 2927
		public const string CycleChatTypes = "CycleChatTypes";

		// Token: 0x04000B70 RID: 2928
		public const string FinalizeChatAlternative = "FinalizeChatAlternative";

		// Token: 0x04000B71 RID: 2929
		public const string SendMessage = "SendMessage";
	}
}
