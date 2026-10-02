using System;
using System.Numerics;
using TaleWorlds.InputSystem;

namespace TaleWorlds.GauntletUI.GauntletInput
{
	// Token: 0x0200004A RID: 74
	public class GauntletInputContext : IReadonlyInputContext
	{
		// Token: 0x0600045C RID: 1116 RVA: 0x00011F0F File Offset: 0x0001010F
		public GauntletInputContext(IInputContext inputContext)
		{
			this._inputContext = inputContext;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00011F1E File Offset: 0x0001011E
		public bool GetIsMouseActive()
		{
			return this._inputContext.GetIsMouseActive();
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00011F2B File Offset: 0x0001012B
		public Vector2 GetMousePosition()
		{
			if (this._isMousePositionOverridden)
			{
				return this._overrideMousePosition;
			}
			return this._inputContext.GetPointerPosition();
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00011F47 File Offset: 0x00010147
		public Vector2 GetMouseMovement()
		{
			return new Vector2(this._inputContext.GetMouseMoveX(), this._inputContext.GetMouseMoveY());
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00011F64 File Offset: 0x00010164
		public InputKey[] GetClickKeys()
		{
			return Input.GetClickKeys();
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00011F6B File Offset: 0x0001016B
		public InputKey[] GetAlternateClickKeys()
		{
			return new InputKey[] { InputKey.RightMouseButton };
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00011F7B File Offset: 0x0001017B
		public float GetMouseScrollDelta()
		{
			return this._inputContext.GetDeltaMouseScroll();
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00011F88 File Offset: 0x00010188
		public Vector2 GetControllerLeftStickState()
		{
			return (Vector2)this._inputContext.GetControllerLeftStickState();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00011F9A File Offset: 0x0001019A
		public Vector2 GetControllerRightStickState()
		{
			return (Vector2)this._inputContext.GetControllerRightStickState();
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00011FAC File Offset: 0x000101AC
		public void SetMousePositionOverride(Vector2 mousePosition)
		{
			this._isMousePositionOverridden = true;
			this._overrideMousePosition = mousePosition;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00011FBC File Offset: 0x000101BC
		public void ResetMousePositionOverride()
		{
			this._isMousePositionOverridden = false;
		}

		// Token: 0x0400022B RID: 555
		private readonly IInputContext _inputContext;

		// Token: 0x0400022C RID: 556
		private bool _isMousePositionOverridden;

		// Token: 0x0400022D RID: 557
		private Vector2 _overrideMousePosition;
	}
}
