using System;
using TaleWorlds.Engine.Options;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.Engine.InputSystem
{
	// Token: 0x020000B1 RID: 177
	public class EngineInputManager : IInputManager
	{
		// Token: 0x06000FB8 RID: 4024 RVA: 0x00013C0C File Offset: 0x00011E0C
		float IInputManager.GetMousePositionX()
		{
			return EngineApplicationInterface.IInput.GetMousePositionX();
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00013C18 File Offset: 0x00011E18
		float IInputManager.GetMousePositionY()
		{
			return EngineApplicationInterface.IInput.GetMousePositionY();
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00013C24 File Offset: 0x00011E24
		float IInputManager.GetMouseScrollValue()
		{
			return EngineApplicationInterface.IInput.GetMouseScrollValue();
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00013C30 File Offset: 0x00011E30
		bool IInputManager.IsMouseActive()
		{
			return EngineApplicationInterface.IInput.IsMouseActive();
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00013C3C File Offset: 0x00011E3C
		bool IInputManager.IsControllerConnected()
		{
			return EngineApplicationInterface.IInput.IsControllerConnected();
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x00013C48 File Offset: 0x00011E48
		void IInputManager.PressKey(InputKey key)
		{
			EngineApplicationInterface.IInput.PressKey(key);
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00013C55 File Offset: 0x00011E55
		void IInputManager.ClearKeys()
		{
			EngineApplicationInterface.IInput.ClearKeys();
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x00013C61 File Offset: 0x00011E61
		int IInputManager.GetVirtualKeyCode(InputKey key)
		{
			return EngineApplicationInterface.IInput.GetVirtualKeyCode(key);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00013C6E File Offset: 0x00011E6E
		void IInputManager.SetClipboardText(string text)
		{
			EngineApplicationInterface.IInput.SetClipboardText(text);
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00013C7B File Offset: 0x00011E7B
		string IInputManager.GetClipboardText()
		{
			return EngineApplicationInterface.IInput.GetClipboardText();
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00013C87 File Offset: 0x00011E87
		float IInputManager.GetMouseMoveX()
		{
			return EngineApplicationInterface.IInput.GetMouseMoveX();
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x00013C93 File Offset: 0x00011E93
		float IInputManager.GetMouseMoveY()
		{
			return EngineApplicationInterface.IInput.GetMouseMoveY();
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00013C9F File Offset: 0x00011E9F
		float IInputManager.GetNormalizedMouseMoveX()
		{
			return EngineApplicationInterface.IInput.GetMouseMoveX() / Screen.RealScreenResolutionWidth;
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00013CB1 File Offset: 0x00011EB1
		float IInputManager.GetNormalizedMouseMoveY()
		{
			return EngineApplicationInterface.IInput.GetMouseMoveY() / Screen.RealScreenResolutionHeight;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x00013CC3 File Offset: 0x00011EC3
		float IInputManager.GetGyroX()
		{
			return EngineApplicationInterface.IInput.GetGyroX();
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x00013CCF File Offset: 0x00011ECF
		float IInputManager.GetGyroY()
		{
			return EngineApplicationInterface.IInput.GetGyroY();
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00013CDB File Offset: 0x00011EDB
		float IInputManager.GetGyroZ()
		{
			return EngineApplicationInterface.IInput.GetGyroZ();
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00013CE7 File Offset: 0x00011EE7
		float IInputManager.GetMouseSensitivity()
		{
			return EngineApplicationInterface.IInput.GetMouseSensitivity();
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00013CF3 File Offset: 0x00011EF3
		float IInputManager.GetMouseDeltaZ()
		{
			return EngineApplicationInterface.IInput.GetMouseDeltaZ();
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00013CFF File Offset: 0x00011EFF
		void IInputManager.UpdateKeyData(byte[] keyData)
		{
			EngineApplicationInterface.IInput.UpdateKeyData(keyData);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00013D0C File Offset: 0x00011F0C
		Vec2 IInputManager.GetKeyState(InputKey key)
		{
			return EngineApplicationInterface.IInput.GetKeyState(key);
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00013D19 File Offset: 0x00011F19
		bool IInputManager.IsKeyPressed(InputKey key)
		{
			return EngineApplicationInterface.IInput.IsKeyPressed(key);
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x00013D26 File Offset: 0x00011F26
		bool IInputManager.IsKeyDown(InputKey key)
		{
			return EngineApplicationInterface.IInput.IsKeyDown(key);
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00013D33 File Offset: 0x00011F33
		bool IInputManager.IsKeyDownImmediate(InputKey key)
		{
			return EngineApplicationInterface.IInput.IsKeyDownImmediate(key);
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00013D40 File Offset: 0x00011F40
		bool IInputManager.IsKeyReleased(InputKey key)
		{
			return EngineApplicationInterface.IInput.IsKeyReleased(key);
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00013D4D File Offset: 0x00011F4D
		Vec2 IInputManager.GetResolution()
		{
			return Screen.RealScreenResolution;
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00013D54 File Offset: 0x00011F54
		Vec2 IInputManager.GetDesktopResolution()
		{
			return Screen.DesktopResolution;
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00013D5C File Offset: 0x00011F5C
		void IInputManager.SetCursorPosition(int x, int y)
		{
			float num = 1f;
			float num2 = 1f;
			if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.DisplayMode) != 0f)
			{
				num = Input.DesktopResolution.X / Input.Resolution.X;
				num2 = Input.DesktopResolution.Y / Input.Resolution.Y;
			}
			EngineApplicationInterface.IInput.SetCursorPosition((int)((float)x * num), (int)((float)y * num2));
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00013DD1 File Offset: 0x00011FD1
		void IInputManager.SetCursorFriction(float frictionValue)
		{
			EngineApplicationInterface.IInput.SetCursorFrictionValue(frictionValue);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00013DE0 File Offset: 0x00011FE0
		InputKey[] IInputManager.GetClickKeys()
		{
			InputKey inputKey = (EngineApplicationInterface.IScreen.IsEnterButtonCross() ? InputKey.ControllerRDown : InputKey.ControllerRRight);
			if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.EnableTouchpadMouse) != 0f)
			{
				return new InputKey[]
				{
					InputKey.LeftMouseButton,
					inputKey,
					InputKey.ControllerLOptionTap
				};
			}
			return new InputKey[]
			{
				InputKey.LeftMouseButton,
				inputKey
			};
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00013E41 File Offset: 0x00012041
		public void SetRumbleEffect(float[] lowFrequencyLevels, float[] lowFrequencyDurations, int numLowFrequencyElements, float[] highFrequencyLevels, float[] highFrequencyDurations, int numHighFrequencyElements)
		{
			EngineApplicationInterface.IInput.SetRumbleEffect(lowFrequencyLevels, lowFrequencyDurations, numLowFrequencyElements, highFrequencyLevels, highFrequencyDurations, numHighFrequencyElements);
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00013E56 File Offset: 0x00012056
		public void SetTriggerFeedback(byte leftTriggerPosition, byte leftTriggerStrength, byte rightTriggerPosition, byte rightTriggerStrength)
		{
			EngineApplicationInterface.IInput.SetTriggerFeedback(leftTriggerPosition, leftTriggerStrength, rightTriggerPosition, rightTriggerStrength);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x00013E67 File Offset: 0x00012067
		public void SetTriggerWeaponEffect(byte leftStartPosition, byte leftEnd_position, byte leftStrength, byte rightStartPosition, byte rightEndPosition, byte rightStrength)
		{
			EngineApplicationInterface.IInput.SetTriggerWeaponEffect(leftStartPosition, leftEnd_position, leftStrength, rightStartPosition, rightEndPosition, rightStrength);
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x00013E7C File Offset: 0x0001207C
		public void SetTriggerVibration(float[] leftTriggerAmplitudes, float[] leftTriggerFrequencies, float[] leftTriggerDurations, int numLeftTriggerElements, float[] rightTriggerAmplitudes, float[] rightTriggerFrequencies, float[] rightTriggerDurations, int numRightTriggerElements)
		{
			EngineApplicationInterface.IInput.SetTriggerVibration(leftTriggerAmplitudes, leftTriggerFrequencies, leftTriggerDurations, numLeftTriggerElements, rightTriggerAmplitudes, rightTriggerFrequencies, rightTriggerDurations, numRightTriggerElements);
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x00013EA0 File Offset: 0x000120A0
		public void SetLightbarColor(float red, float green, float blue)
		{
			EngineApplicationInterface.IInput.SetLightbarColor(red, green, blue);
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x00013EAF File Offset: 0x000120AF
		Input.ControllerTypes IInputManager.GetControllerType()
		{
			return (Input.ControllerTypes)EngineApplicationInterface.IInput.GetControllerType();
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x00013EBB File Offset: 0x000120BB
		bool IInputManager.IsAnyTouchActive()
		{
			return EngineApplicationInterface.IInput.IsAnyTouchActive();
		}
	}
}
