using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200022D RID: 557
	public sealed class ConversationHotKeyCategory : GameKeyContext
	{
		// Token: 0x06002134 RID: 8500 RVA: 0x00075327 File Offset: 0x00073527
		public ConversationHotKeyCategory()
			: base("ConversationHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x0007534C File Offset: 0x0007354C
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.Space),
				new Key(InputKey.Enter),
				new Key(InputKey.NumpadEnter)
			};
			base.RegisterHotKey(new HotKey("ContinueKey", "ConversationHotKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRDown)
			};
			base.RegisterHotKey(new HotKey("ContinueClick", "ConversationHotKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000753E1 File Offset: 0x000735E1
		private void RegisterGameKeys()
		{
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x000753E3 File Offset: 0x000735E3
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000B9E RID: 2974
		public const string CategoryId = "ConversationHotKeyCategory";

		// Token: 0x04000B9F RID: 2975
		public const string ContinueKey = "ContinueKey";

		// Token: 0x04000BA0 RID: 2976
		public const string ContinueClick = "ContinueClick";
	}
}
