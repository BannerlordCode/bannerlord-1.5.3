using System;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000010 RID: 16
	public class GauntletGamepadCursor : GlobalLayer
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00004DB4 File Offset: 0x00002FB4
		public GauntletGamepadCursor()
		{
			this._dataSource = new GamepadCursorViewModel();
			this._layer = new GauntletLayer("GamepadCusor", 115001, false);
			this._layer.LoadMovie("GamepadCursor", this._dataSource);
			this._layer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Invalid);
			base.Layer = this._layer;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004E1D File Offset: 0x0000301D
		public static void Initialize()
		{
			if (GauntletGamepadCursor._current == null)
			{
				GauntletGamepadCursor._current = new GauntletGamepadCursor();
				ScreenManager.AddGlobalLayer(GauntletGamepadCursor._current, false);
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004E3C File Offset: 0x0000303C
		protected override void OnLateTick(float dt)
		{
			base.OnLateTick(dt);
			if (ScreenManager.IsMouseCursorHidden())
			{
				this._dataSource.IsGamepadCursorVisible = true;
				this._dataSource.IsConsoleMouseVisible = false;
				Vec2 cursorPosition = this.GetCursorPosition();
				this._dataSource.CursorPositionX = cursorPosition.X;
				this._dataSource.CursorPositionY = cursorPosition.Y;
				return;
			}
			this._dataSource.IsGamepadCursorVisible = false;
			this._dataSource.IsConsoleMouseVisible = false;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004EB4 File Offset: 0x000030B4
		private Vec2 GetCursorPosition()
		{
			Vec2 mousePositionPixel = Input.MousePositionPixel;
			Vec2 vec = Vec2.One - ScreenManager.UsableArea;
			float num = vec.x * Screen.RealScreenResolution.x / 2f;
			float num2 = vec.y * Screen.RealScreenResolution.y / 2f;
			return new Vec2(mousePositionPixel.X - num, mousePositionPixel.Y - num2);
		}

		// Token: 0x04000058 RID: 88
		private GamepadCursorViewModel _dataSource;

		// Token: 0x04000059 RID: 89
		private GauntletLayer _layer;

		// Token: 0x0400005A RID: 90
		private static GauntletGamepadCursor _current;
	}
}
