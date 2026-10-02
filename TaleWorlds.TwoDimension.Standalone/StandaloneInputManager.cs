using System;
using System.Drawing;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x0200000F RID: 15
	public class StandaloneInputManager : IInputManager
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00005DF1 File Offset: 0x00003FF1
		public StandaloneInputManager(GraphicsForm graphicsForm)
		{
			this._graphicsForm = graphicsForm;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005E00 File Offset: 0x00004000
		float IInputManager.GetMousePositionX()
		{
			return this._graphicsForm.MousePosition().X / (float)this._graphicsForm.Width;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005E1F File Offset: 0x0000401F
		float IInputManager.GetMousePositionY()
		{
			return this._graphicsForm.MousePosition().Y / (float)this._graphicsForm.Height;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005E3E File Offset: 0x0000403E
		float IInputManager.GetMouseScrollValue()
		{
			return 0f;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00005E45 File Offset: 0x00004045
		bool IInputManager.IsMouseActive()
		{
			return true;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005E48 File Offset: 0x00004048
		bool IInputManager.IsAnyTouchActive()
		{
			return false;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005E4B File Offset: 0x0000404B
		bool IInputManager.IsControllerConnected()
		{
			return false;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005E4E File Offset: 0x0000404E
		void IInputManager.PressKey(InputKey key)
		{
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005E50 File Offset: 0x00004050
		void IInputManager.ClearKeys()
		{
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005E52 File Offset: 0x00004052
		int IInputManager.GetVirtualKeyCode(InputKey key)
		{
			return -1;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005E55 File Offset: 0x00004055
		void IInputManager.SetClipboardText(string text)
		{
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005E57 File Offset: 0x00004057
		string IInputManager.GetClipboardText()
		{
			return "";
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005E5E File Offset: 0x0000405E
		float IInputManager.GetMouseMoveX()
		{
			return 0f;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005E65 File Offset: 0x00004065
		float IInputManager.GetMouseMoveY()
		{
			return 0f;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005E6C File Offset: 0x0000406C
		float IInputManager.GetNormalizedMouseMoveX()
		{
			return 0f;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005E73 File Offset: 0x00004073
		float IInputManager.GetNormalizedMouseMoveY()
		{
			return 0f;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005E7A File Offset: 0x0000407A
		float IInputManager.GetGyroX()
		{
			return 0f;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005E81 File Offset: 0x00004081
		float IInputManager.GetGyroY()
		{
			return 0f;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005E88 File Offset: 0x00004088
		float IInputManager.GetGyroZ()
		{
			return 0f;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005E8F File Offset: 0x0000408F
		float IInputManager.GetMouseSensitivity()
		{
			return 1f;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00005E96 File Offset: 0x00004096
		float IInputManager.GetMouseDeltaZ()
		{
			return this._graphicsForm.GetMouseDeltaZ();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005EA3 File Offset: 0x000040A3
		void IInputManager.UpdateKeyData(byte[] keyData)
		{
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005EA5 File Offset: 0x000040A5
		Vec2 IInputManager.GetKeyState(InputKey key)
		{
			if (!this._graphicsForm.GetKey(key))
			{
				return new Vec2(0f, 0f);
			}
			return new Vec2(1f, 0f);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005ED4 File Offset: 0x000040D4
		bool IInputManager.IsKeyPressed(InputKey key)
		{
			return this._graphicsForm.GetKeyDown(key);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005EE2 File Offset: 0x000040E2
		bool IInputManager.IsKeyDown(InputKey key)
		{
			return this._graphicsForm.GetKey(key);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005EF0 File Offset: 0x000040F0
		bool IInputManager.IsKeyDownImmediate(InputKey key)
		{
			return this._graphicsForm.GetKey(key);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005EFE File Offset: 0x000040FE
		bool IInputManager.IsKeyReleased(InputKey key)
		{
			return this._graphicsForm.GetKeyUp(key);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005F0C File Offset: 0x0000410C
		Vec2 IInputManager.GetResolution()
		{
			return new Vec2((float)this._graphicsForm.Width, (float)this._graphicsForm.Height);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005F2C File Offset: 0x0000412C
		Vec2 IInputManager.GetDesktopResolution()
		{
			Rectangle rectangle;
			User32.GetClientRect(User32.GetDesktopWindow(), out rectangle);
			return new Vec2((float)rectangle.Width, (float)rectangle.Height);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005F5B File Offset: 0x0000415B
		void IInputManager.SetCursorPosition(int x, int y)
		{
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005F5D File Offset: 0x0000415D
		void IInputManager.SetCursorFriction(float frictionValue)
		{
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005F5F File Offset: 0x0000415F
		InputKey[] IInputManager.GetClickKeys()
		{
			return new InputKey[]
			{
				InputKey.LeftMouseButton,
				InputKey.ControllerRDown
			};
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005F77 File Offset: 0x00004177
		public void SetRumbleEffect(float[] lowFrequencyLevels, float[] lowFrequencyDurations, int numLowFrequencyElements, float[] highFrequencyLevels, float[] highFrequencyDurations, int numHighFrequencyElements)
		{
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005F79 File Offset: 0x00004179
		public void SetTriggerFeedback(byte leftTriggerPosition, byte leftTriggerStrength, byte rightTriggerPosition, byte rightTriggerStrength)
		{
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005F7B File Offset: 0x0000417B
		public void SetTriggerWeaponEffect(byte leftStartPosition, byte leftEnd_position, byte leftStrength, byte rightStartPosition, byte rightEndPosition, byte rightStrength)
		{
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005F7D File Offset: 0x0000417D
		public void SetTriggerVibration(float[] leftTriggerAmplitudes, float[] leftTriggerFrequencies, float[] leftTriggerDurations, int numLeftTriggerElements, float[] rightTriggerAmplitudes, float[] rightTriggerFrequencies, float[] rightTriggerDurations, int numRightTriggerElements)
		{
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005F7F File Offset: 0x0000417F
		public void SetLightbarColor(float red, float green, float blue)
		{
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005F81 File Offset: 0x00004181
		Input.ControllerTypes IInputManager.GetControllerType()
		{
			return Input.ControllerTypes.Xbox;
		}

		// Token: 0x04000057 RID: 87
		private GraphicsForm _graphicsForm;
	}
}
