using System;
using TaleWorlds.Library;

namespace TaleWorlds.InputSystem
{
	// Token: 0x0200000B RID: 11
	public static class Input
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x00003C78 File Offset: 0x00001E78
		public static bool IsPlaystation(this Input.ControllerTypes controllerType)
		{
			return controllerType.HasAnyFlag((Input.ControllerTypes)6);
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00003C81 File Offset: 0x00001E81
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00003C88 File Offset: 0x00001E88
		public static InputState InputState { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00003C90 File Offset: 0x00001E90
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00003C97 File Offset: 0x00001E97
		public static IInputContext DebugInput { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00003C9F File Offset: 0x00001E9F
		public static IInputManager InputManager
		{
			get
			{
				if (Input.IsOnScreenKeyboardActive)
				{
					return Input._emptyInputManager;
				}
				return Input._inputManager;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00003CB3 File Offset: 0x00001EB3
		public static Vec2 Resolution
		{
			get
			{
				return Input._inputManager.GetResolution();
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00003CBF File Offset: 0x00001EBF
		public static Vec2 DesktopResolution
		{
			get
			{
				return Input._inputManager.GetDesktopResolution();
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00003CCB File Offset: 0x00001ECB
		public static void Initialize(IInputManager inputManager, IInputContext debugInput)
		{
			Input._emptyInputManager = new EmptyInputManager();
			Input._inputManager = inputManager;
			Input.InputState = new InputState();
			Input.keyData = new byte[256];
			Input.DebugInput = new EmptyInputContext();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00003D00 File Offset: 0x00001F00
		public static void UpdateKeyData(byte[] keyData)
		{
			Input.InputManager.UpdateKeyData(keyData);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00003D0D File Offset: 0x00001F0D
		public static float GetMouseMoveX()
		{
			return Input.InputManager.GetMouseMoveX();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00003D19 File Offset: 0x00001F19
		public static float GetMouseMoveY()
		{
			return Input.InputManager.GetMouseMoveY();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00003D25 File Offset: 0x00001F25
		public static float GetNormalizedMouseMoveX()
		{
			return Input.InputManager.GetNormalizedMouseMoveX();
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00003D31 File Offset: 0x00001F31
		public static float GetNormalizedMouseMoveY()
		{
			return Input.InputManager.GetNormalizedMouseMoveY();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00003D3D File Offset: 0x00001F3D
		public static float GetGyroX()
		{
			return Input.InputManager.GetGyroX();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00003D49 File Offset: 0x00001F49
		public static float GetGyroY()
		{
			return Input.InputManager.GetGyroY();
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00003D55 File Offset: 0x00001F55
		public static float GetGyroZ()
		{
			return Input.InputManager.GetGyroZ();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00003D61 File Offset: 0x00001F61
		public static Vec2 GetKeyState(InputKey key)
		{
			return Input.InputManager.GetKeyState(key);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00003D6E File Offset: 0x00001F6E
		public static bool IsKeyPressed(InputKey key)
		{
			return Input.InputManager.IsKeyPressed(key);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00003D7B File Offset: 0x00001F7B
		public static bool IsKeyDown(InputKey key)
		{
			return Input.InputManager.IsKeyDown(key);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00003D88 File Offset: 0x00001F88
		public static bool IsKeyDownImmediate(InputKey key)
		{
			return Input.InputManager.IsKeyDownImmediate(key);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00003D95 File Offset: 0x00001F95
		public static bool IsKeyReleased(InputKey key)
		{
			return Input.InputManager.IsKeyReleased(key);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00003DA2 File Offset: 0x00001FA2
		public static bool IsControlOrShiftNotDown()
		{
			return !InputKey.LeftControl.IsDown() && !InputKey.RightControl.IsDown() && !InputKey.LeftShift.IsDown() && !InputKey.RightShift.IsDown();
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00003DCE File Offset: 0x00001FCE
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00003DD5 File Offset: 0x00001FD5
		public static bool IsOnScreenKeyboardActive { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00003DDD File Offset: 0x00001FDD
		public static bool IsMouseActive
		{
			get
			{
				return Input.InputManager.IsMouseActive();
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00003DE9 File Offset: 0x00001FE9
		public static bool IsControllerConnected
		{
			get
			{
				return Input.InputManager.IsControllerConnected();
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00003DF5 File Offset: 0x00001FF5
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00003DFC File Offset: 0x00001FFC
		public static bool IsGamepadActive
		{
			get
			{
				return Input._isGamepadActive;
			}
			private set
			{
				if (value != Input._isGamepadActive)
				{
					Input._isGamepadActive = value;
					Action onGamepadActiveStateChanged = Input.OnGamepadActiveStateChanged;
					if (onGamepadActiveStateChanged == null)
					{
						return;
					}
					onGamepadActiveStateChanged();
				}
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00003E1B File Offset: 0x0000201B
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00003E22 File Offset: 0x00002022
		public static bool IsAnyTouchActive
		{
			get
			{
				return Input._isAnyTouchActive;
			}
			private set
			{
				if (value != Input._isAnyTouchActive)
				{
					Input._isAnyTouchActive = value;
				}
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00003E32 File Offset: 0x00002032
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00003E39 File Offset: 0x00002039
		public static Input.ControllerTypes ControllerType
		{
			get
			{
				return Input._controllerType;
			}
			private set
			{
				if (value != Input._controllerType)
				{
					Input._controllerType = value;
					Action<Input.ControllerTypes> onControllerTypeChanged = Input.OnControllerTypeChanged;
					if (onControllerTypeChanged == null)
					{
						return;
					}
					onControllerTypeChanged(value);
				}
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00003E59 File Offset: 0x00002059
		public static Input.ControllerTypes GetPrimaryControllerType()
		{
			return Input.ControllerTypes.Xbox;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00003E5C File Offset: 0x0000205C
		public static int GetFirstKeyPressedInRange(int startKeyNo)
		{
			int num = -1;
			for (int i = startKeyNo; i < 256; i++)
			{
				if (Input.IsKeyPressed((InputKey)i))
				{
					num = i;
					break;
				}
			}
			return num;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00003E88 File Offset: 0x00002088
		public static int GetFirstKeyDownInRange(int startKeyNo)
		{
			int num = -1;
			for (int i = startKeyNo; i < 256; i++)
			{
				if (Input.IsKeyDown((InputKey)i))
				{
					num = i;
					break;
				}
			}
			return num;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00003EB4 File Offset: 0x000020B4
		public static int GetFirstKeyReleasedInRange(int startKeyNo)
		{
			int num = -1;
			for (int i = startKeyNo; i < 256; i++)
			{
				if (Input.IsKeyReleased((InputKey)i))
				{
					num = i;
					break;
				}
			}
			return num;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00003EE0 File Offset: 0x000020E0
		public static void PressKey(InputKey key)
		{
			Input.InputManager.PressKey(key);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00003EED File Offset: 0x000020ED
		public static void ClearKeys()
		{
			Input.InputManager.ClearKeys();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00003EF9 File Offset: 0x000020F9
		public static int GetVirtualKeyCode(InputKey key)
		{
			return Input.InputManager.GetVirtualKeyCode(key);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00003F06 File Offset: 0x00002106
		public static bool IsDown(this InputKey key)
		{
			return Input.IsKeyDown(key);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00003F0E File Offset: 0x0000210E
		public static bool IsPressed(this InputKey key)
		{
			return Input.IsKeyPressed(key);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00003F16 File Offset: 0x00002116
		public static bool IsReleased(this InputKey key)
		{
			return Input.IsKeyReleased(key);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00003F1E File Offset: 0x0000211E
		public static void SetClipboardText(string text)
		{
			Input.InputManager.SetClipboardText(text);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00003F2B File Offset: 0x0000212B
		public static string GetClipboardText()
		{
			return Input.InputManager.GetClipboardText();
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00003F37 File Offset: 0x00002137
		public static float MouseMoveX
		{
			get
			{
				return Input.InputManager.GetMouseMoveX();
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00003F43 File Offset: 0x00002143
		public static float MouseMoveY
		{
			get
			{
				return Input.InputManager.GetMouseMoveY();
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00003F4F File Offset: 0x0000214F
		public static float GyroX
		{
			get
			{
				return Input.InputManager.GetGyroX();
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00003F5B File Offset: 0x0000215B
		public static float GyroY
		{
			get
			{
				return Input.InputManager.GetGyroY();
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00003F67 File Offset: 0x00002167
		public static float GyroZ
		{
			get
			{
				return Input.InputManager.GetGyroZ();
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00003F73 File Offset: 0x00002173
		public static float MouseSensitivity
		{
			get
			{
				return Input.InputManager.GetMouseSensitivity();
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00003F7F File Offset: 0x0000217F
		public static float DeltaMouseScroll
		{
			get
			{
				return Input.InputManager.GetMouseDeltaZ();
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00003F8B File Offset: 0x0000218B
		public static Vec2 MousePositionRanged
		{
			get
			{
				return Input.InputState.MousePositionRanged;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00003F97 File Offset: 0x00002197
		public static Vec2 MousePositionPixel
		{
			get
			{
				return Input.InputState.MousePositionPixel;
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00003FA4 File Offset: 0x000021A4
		public static void Update()
		{
			if (Input.IsOnScreenKeyboardActive)
			{
				return;
			}
			float mousePositionX = Input.InputManager.GetMousePositionX();
			float mousePositionY = Input.InputManager.GetMousePositionY();
			float mouseScrollValue = Input.InputManager.GetMouseScrollValue();
			Input.IsMousePositionUpdated = Input.InputState.UpdateMousePosition(mousePositionX, mousePositionY);
			Input.IsMouseScrollChanged = Input.InputState.UpdateMouseScroll(mouseScrollValue);
			Input.IsGamepadActive = Input.IsControllerConnected && !Input.IsMouseActive;
			Input.IsAnyTouchActive = Input.InputManager.IsAnyTouchActive();
			Input.ControllerType = Input.InputManager.GetControllerType();
			Input.UpdateKeyData(Input.keyData);
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600012B RID: 299 RVA: 0x0000403A File Offset: 0x0000223A
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00004041 File Offset: 0x00002241
		public static bool IsMousePositionUpdated { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00004049 File Offset: 0x00002249
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00004050 File Offset: 0x00002250
		public static bool IsMouseScrollChanged { get; private set; }

		// Token: 0x0600012F RID: 303 RVA: 0x00004058 File Offset: 0x00002258
		public static bool IsControllerKey(InputKey key)
		{
			switch (key)
			{
			case InputKey.Escape:
			case InputKey.D1:
			case InputKey.D2:
			case InputKey.D3:
			case InputKey.D4:
			case InputKey.D5:
			case InputKey.D6:
			case InputKey.D7:
			case InputKey.D8:
			case InputKey.D9:
			case InputKey.D0:
			case InputKey.Minus:
			case InputKey.Equals:
			case InputKey.BackSpace:
			case InputKey.Tab:
			case InputKey.Q:
			case InputKey.W:
			case InputKey.E:
			case InputKey.R:
			case InputKey.T:
			case InputKey.Y:
			case InputKey.U:
			case InputKey.I:
			case InputKey.O:
			case InputKey.P:
			case InputKey.OpenBraces:
			case InputKey.CloseBraces:
			case InputKey.Enter:
			case InputKey.LeftControl:
			case InputKey.A:
			case InputKey.S:
			case InputKey.D:
			case InputKey.F:
			case InputKey.G:
			case InputKey.H:
			case InputKey.J:
			case InputKey.K:
			case InputKey.L:
			case InputKey.SemiColon:
			case InputKey.Apostrophe:
			case InputKey.Tilde:
			case InputKey.LeftShift:
			case InputKey.BackSlash:
			case InputKey.Z:
			case InputKey.X:
			case InputKey.C:
			case InputKey.V:
			case InputKey.B:
			case InputKey.N:
			case InputKey.M:
			case InputKey.Comma:
			case InputKey.Period:
			case InputKey.Slash:
			case InputKey.RightShift:
			case InputKey.NumpadMultiply:
			case InputKey.LeftAlt:
			case InputKey.Space:
			case InputKey.CapsLock:
			case InputKey.F1:
			case InputKey.F2:
			case InputKey.F3:
			case InputKey.F4:
			case InputKey.F5:
			case InputKey.F6:
			case InputKey.F7:
			case InputKey.F8:
			case InputKey.F9:
			case InputKey.F10:
			case InputKey.Numpad7:
			case InputKey.Numpad8:
			case InputKey.Numpad9:
			case InputKey.NumpadMinus:
			case InputKey.Numpad4:
			case InputKey.Numpad5:
			case InputKey.Numpad6:
			case InputKey.NumpadPlus:
			case InputKey.Numpad1:
			case InputKey.Numpad2:
			case InputKey.Numpad3:
			case InputKey.Numpad0:
			case InputKey.NumpadPeriod:
			case InputKey.Extended:
			case InputKey.F11:
			case InputKey.F12:
			case InputKey.NumpadEnter:
			case InputKey.RightControl:
			case InputKey.NumpadSlash:
			case InputKey.RightAlt:
			case InputKey.NumLock:
			case InputKey.Home:
			case InputKey.Up:
			case InputKey.PageUp:
			case InputKey.Left:
			case InputKey.Right:
			case InputKey.End:
			case InputKey.Down:
			case InputKey.PageDown:
			case InputKey.Insert:
			case InputKey.Delete:
			case InputKey.LeftMouseButton:
			case InputKey.RightMouseButton:
			case InputKey.MiddleMouseButton:
			case InputKey.X1MouseButton:
			case InputKey.X2MouseButton:
			case InputKey.MouseScrollUp:
			case InputKey.MouseScrollDown:
				return false;
			}
			return true;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000446E File Offset: 0x0000266E
		public static void SetMousePosition(int x, int y)
		{
			Input.InputManager.SetCursorPosition(x, y);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000447C File Offset: 0x0000267C
		public static void SetCursorFriction(float frictionValue)
		{
			Input.InputManager.SetCursorFriction(frictionValue);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00004489 File Offset: 0x00002689
		public static InputKey[] GetClickKeys()
		{
			return Input.InputManager.GetClickKeys();
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00004495 File Offset: 0x00002695
		public static void SetRumbleEffect(float[] lowFrequencyLevels, float[] lowFrequencyDurations, int numLowFrequencyElements, float[] highFrequencyLevels, float[] highFrequencyDurations, int numHighFrequencyElements)
		{
			Input.InputManager.SetRumbleEffect(lowFrequencyLevels, lowFrequencyDurations, numLowFrequencyElements, highFrequencyLevels, highFrequencyDurations, numHighFrequencyElements);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000044A9 File Offset: 0x000026A9
		public static void SetTriggerFeedback(byte leftTriggerPosition, byte leftTriggerStrength, byte rightTriggerPosition, byte rightTriggerStrength)
		{
			Input.InputManager.SetTriggerFeedback(leftTriggerPosition, leftTriggerStrength, rightTriggerPosition, rightTriggerStrength);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000044B9 File Offset: 0x000026B9
		public static void SetTriggerWeaponEffect(byte leftStartPosition, byte leftEnd_position, byte leftStrength, byte rightStartPosition, byte rightEndPosition, byte rightStrength)
		{
			Input.InputManager.SetTriggerWeaponEffect(leftStartPosition, leftEnd_position, leftStrength, rightStartPosition, rightEndPosition, rightStrength);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000044D0 File Offset: 0x000026D0
		public static void SetTriggerVibration(float[] leftTriggerAmplitudes, float[] leftTriggerFrequencies, float[] leftTriggerDurations, int numLeftTriggerElements, float[] rightTriggerAmplitudes, float[] rightTriggerFrequencies, float[] rightTriggerDurations, int numRightTriggerElements)
		{
			Input.InputManager.SetTriggerVibration(leftTriggerAmplitudes, leftTriggerFrequencies, leftTriggerDurations, numLeftTriggerElements, rightTriggerAmplitudes, rightTriggerFrequencies, rightTriggerDurations, numRightTriggerElements);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000044F3 File Offset: 0x000026F3
		public static void SetLightbarColor(float red, float green, float blue)
		{
			Input.InputManager.SetLightbarColor(red, green, blue);
		}

		// Token: 0x0400002C RID: 44
		public const int NumberOfKeys = 256;

		// Token: 0x0400002D RID: 45
		private static byte[] keyData;

		// Token: 0x0400002E RID: 46
		private static IInputManager _emptyInputManager;

		// Token: 0x0400002F RID: 47
		private static IInputManager _inputManager;

		// Token: 0x04000031 RID: 49
		public static Action OnGamepadActiveStateChanged;

		// Token: 0x04000032 RID: 50
		private static bool _isGamepadActive;

		// Token: 0x04000033 RID: 51
		private static bool _isAnyTouchActive;

		// Token: 0x04000034 RID: 52
		public static Action<Input.ControllerTypes> OnControllerTypeChanged;

		// Token: 0x04000035 RID: 53
		private static Input.ControllerTypes _controllerType;

		// Token: 0x02000018 RID: 24
		public enum ControllerTypes
		{
			// Token: 0x0400016D RID: 365
			None,
			// Token: 0x0400016E RID: 366
			Xbox,
			// Token: 0x0400016F RID: 367
			PlayStationDualShock,
			// Token: 0x04000170 RID: 368
			PlayStationDualSense = 4
		}
	}
}
