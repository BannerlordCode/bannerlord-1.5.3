using System;
using System.Numerics;
using TaleWorlds.InputSystem;

namespace TaleWorlds.GauntletUI.GauntletInput
{
	// Token: 0x02000049 RID: 73
	public interface IReadonlyInputContext
	{
		// Token: 0x06000454 RID: 1108
		bool GetIsMouseActive();

		// Token: 0x06000455 RID: 1109
		Vector2 GetMousePosition();

		// Token: 0x06000456 RID: 1110
		Vector2 GetMouseMovement();

		// Token: 0x06000457 RID: 1111
		InputKey[] GetClickKeys();

		// Token: 0x06000458 RID: 1112
		InputKey[] GetAlternateClickKeys();

		// Token: 0x06000459 RID: 1113
		Vector2 GetControllerLeftStickState();

		// Token: 0x0600045A RID: 1114
		Vector2 GetControllerRightStickState();

		// Token: 0x0600045B RID: 1115
		float GetMouseScrollDelta();
	}
}
