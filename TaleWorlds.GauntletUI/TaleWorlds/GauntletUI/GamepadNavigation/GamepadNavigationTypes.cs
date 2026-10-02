using System;

namespace TaleWorlds.GauntletUI.GamepadNavigation
{
	// Token: 0x0200004F RID: 79
	[Flags]
	public enum GamepadNavigationTypes
	{
		// Token: 0x04000274 RID: 628
		None = 0,
		// Token: 0x04000275 RID: 629
		Up = 1,
		// Token: 0x04000276 RID: 630
		Down = 2,
		// Token: 0x04000277 RID: 631
		Vertical = 3,
		// Token: 0x04000278 RID: 632
		Left = 4,
		// Token: 0x04000279 RID: 633
		Right = 8,
		// Token: 0x0400027A RID: 634
		Horizontal = 12
	}
}
