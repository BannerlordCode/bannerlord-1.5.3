using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200023C RID: 572
	public sealed class PollHotkeyCategory : GameKeyContext
	{
		// Token: 0x0600216C RID: 8556 RVA: 0x00076B95 File Offset: 0x00074D95
		public PollHotkeyCategory()
			: base("PollHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterGameKeys();
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x00076BAC File Offset: 0x00074DAC
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(108, "AcceptPoll", "PollHotkeyCategory", InputKey.F10, InputKey.ControllerLBumper, GameKeyMainCategories.PollCategory), true);
			base.RegisterGameKey(new GameKey(109, "DeclinePoll", "PollHotkeyCategory", InputKey.F11, InputKey.ControllerRBumper, GameKeyMainCategories.PollCategory), true);
		}

		// Token: 0x04000CBF RID: 3263
		public const string CategoryId = "PollHotkeyCategory";

		// Token: 0x04000CC0 RID: 3264
		public const int AcceptPoll = 108;

		// Token: 0x04000CC1 RID: 3265
		public const int DeclinePoll = 109;
	}
}
