using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000239 RID: 569
	public sealed class MissionOrderHotkeyCategory : GameKeyContext
	{
		// Token: 0x0600215F RID: 8543 RVA: 0x00076328 File Offset: 0x00074528
		public MissionOrderHotkeyCategory()
			: base("MissionOrderHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x0007634A File Offset: 0x0007454A
		private void RegisterHotKeys()
		{
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x0007634C File Offset: 0x0007454C
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(68, "ViewOrders", "MissionOrderHotkeyCategory", InputKey.BackSpace, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(69, "SelectOrder1", "MissionOrderHotkeyCategory", InputKey.F1, InputKey.ControllerRLeft, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(70, "SelectOrder2", "MissionOrderHotkeyCategory", InputKey.F2, InputKey.ControllerRDown, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(71, "SelectOrder3", "MissionOrderHotkeyCategory", InputKey.F3, InputKey.ControllerRRight, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(72, "SelectOrder4", "MissionOrderHotkeyCategory", InputKey.F4, InputKey.ControllerRUp, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(73, "SelectOrder5", "MissionOrderHotkeyCategory", InputKey.F5, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(74, "SelectOrder6", "MissionOrderHotkeyCategory", InputKey.F6, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(75, "SelectOrder7", "MissionOrderHotkeyCategory", InputKey.F7, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(76, "SelectOrder8", "MissionOrderHotkeyCategory", InputKey.F8, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(77, "SelectOrderReturn", "MissionOrderHotkeyCategory", InputKey.F9, InputKey.ControllerROption, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(78, "EveryoneHear", "MissionOrderHotkeyCategory", InputKey.D0, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(79, "Group0Hear", "MissionOrderHotkeyCategory", InputKey.D1, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(80, "Group1Hear", "MissionOrderHotkeyCategory", InputKey.D2, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(81, "Group2Hear", "MissionOrderHotkeyCategory", InputKey.D3, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(82, "Group3Hear", "MissionOrderHotkeyCategory", InputKey.D4, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(83, "Group4Hear", "MissionOrderHotkeyCategory", InputKey.D5, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(84, "Group5Hear", "MissionOrderHotkeyCategory", InputKey.D6, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(85, "Group6Hear", "MissionOrderHotkeyCategory", InputKey.D7, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(86, "Group7Hear", "MissionOrderHotkeyCategory", InputKey.D8, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(87, "HoldOrder", "MissionOrderHotkeyCategory", InputKey.Invalid, InputKey.ControllerLBumper, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(88, "SelectLeftFormation", "MissionOrderHotkeyCategory", InputKey.Invalid, InputKey.ControllerLLeft, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(89, "SelectRightFormation", "MissionOrderHotkeyCategory", InputKey.Invalid, InputKey.ControllerLRight, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(90, "ApplySelection", "MissionOrderHotkeyCategory", InputKey.Invalid, InputKey.ControllerLDown, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(91, "ToggleSelection", "MissionOrderHotkeyCategory", InputKey.Invalid, InputKey.ControllerLUp, GameKeyMainCategories.OrderMenuCategory), true);
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x00076667 File Offset: 0x00074867
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000C7E RID: 3198
		public const string CategoryId = "MissionOrderHotkeyCategory";

		// Token: 0x04000C7F RID: 3199
		public const int ViewOrders = 68;

		// Token: 0x04000C80 RID: 3200
		public const int SelectOrder1 = 69;

		// Token: 0x04000C81 RID: 3201
		public const int SelectOrder2 = 70;

		// Token: 0x04000C82 RID: 3202
		public const int SelectOrder3 = 71;

		// Token: 0x04000C83 RID: 3203
		public const int SelectOrder4 = 72;

		// Token: 0x04000C84 RID: 3204
		public const int SelectOrder5 = 73;

		// Token: 0x04000C85 RID: 3205
		public const int SelectOrder6 = 74;

		// Token: 0x04000C86 RID: 3206
		public const int SelectOrder7 = 75;

		// Token: 0x04000C87 RID: 3207
		public const int SelectOrder8 = 76;

		// Token: 0x04000C88 RID: 3208
		public const int SelectOrderReturn = 77;

		// Token: 0x04000C89 RID: 3209
		public const int EveryoneHear = 78;

		// Token: 0x04000C8A RID: 3210
		public const int Group0Hear = 79;

		// Token: 0x04000C8B RID: 3211
		public const int Group1Hear = 80;

		// Token: 0x04000C8C RID: 3212
		public const int Group2Hear = 81;

		// Token: 0x04000C8D RID: 3213
		public const int Group3Hear = 82;

		// Token: 0x04000C8E RID: 3214
		public const int Group4Hear = 83;

		// Token: 0x04000C8F RID: 3215
		public const int Group5Hear = 84;

		// Token: 0x04000C90 RID: 3216
		public const int Group6Hear = 85;

		// Token: 0x04000C91 RID: 3217
		public const int Group7Hear = 86;

		// Token: 0x04000C92 RID: 3218
		public const int HoldOrder = 87;

		// Token: 0x04000C93 RID: 3219
		public const int SelectLeftFormation = 88;

		// Token: 0x04000C94 RID: 3220
		public const int SelectRightFormation = 89;

		// Token: 0x04000C95 RID: 3221
		public const int ApplySelection = 90;

		// Token: 0x04000C96 RID: 3222
		public const int ToggleSelection = 91;
	}
}
