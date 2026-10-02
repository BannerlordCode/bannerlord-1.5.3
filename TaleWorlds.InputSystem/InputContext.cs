using System;
using System.Collections.Generic;
using System.Numerics;
using TaleWorlds.Library;

namespace TaleWorlds.InputSystem
{
	// Token: 0x0200000C RID: 12
	public class InputContext : IInputContext
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00004502 File Offset: 0x00002702
		// (set) Token: 0x06000139 RID: 313 RVA: 0x0000450A File Offset: 0x0000270A
		public bool IsKeysAllowed { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00004513 File Offset: 0x00002713
		// (set) Token: 0x0600013B RID: 315 RVA: 0x0000451B File Offset: 0x0000271B
		public bool IsMouseButtonAllowed { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00004524 File Offset: 0x00002724
		// (set) Token: 0x0600013D RID: 317 RVA: 0x0000452C File Offset: 0x0000272C
		public bool IsMouseWheelAllowed { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00004535 File Offset: 0x00002735
		public bool IsControllerAllowed
		{
			get
			{
				return this.IsKeysAllowed;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600013F RID: 319 RVA: 0x0000453D File Offset: 0x0000273D
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00004545 File Offset: 0x00002745
		public bool MouseOnMe { get; set; }

		// Token: 0x06000141 RID: 321 RVA: 0x00004550 File Offset: 0x00002750
		public InputContext()
		{
			this._categories = new List<GameKeyContext>();
			this._registeredGameKeys = new List<GameKey>();
			this._registeredHotKeys = new Dictionary<string, HotKey>();
			this._registeredGameAxisKeys = new Dictionary<string, GameAxisKey>();
			this._downInputKeys = new List<Key>();
			this.MouseOnMe = false;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000045AC File Offset: 0x000027AC
		public int GetPointerX()
		{
			float x = Input.Resolution.x;
			return (int)(this.GetMousePositionRanged().x * x);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000045D4 File Offset: 0x000027D4
		public int GetPointerY()
		{
			float y = Input.Resolution.y;
			return (int)(this.GetMousePositionRanged().y * y);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000045FC File Offset: 0x000027FC
		public Vector2 GetPointerPosition()
		{
			Vec2 resolution = Input.Resolution;
			float x = resolution.x;
			float y = resolution.y;
			float num = this.GetMousePositionRanged().x * x;
			float num2 = this.GetMousePositionRanged().y * y;
			return new Vector2(num, num2);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000463C File Offset: 0x0000283C
		public Vec2 GetPointerPositionVec2()
		{
			Vec2 resolution = Input.Resolution;
			float x = resolution.x;
			float y = resolution.y;
			float num = this.GetMousePositionRanged().x * x;
			float num2 = this.GetMousePositionRanged().y * y;
			return new Vec2(num, num2);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000467C File Offset: 0x0000287C
		public void RegisterHotKeyCategory(GameKeyContext category)
		{
			this._categories.Add(category);
			foreach (HotKey hotKey in category.RegisteredHotKeys)
			{
				if (!this._registeredHotKeys.ContainsKey(hotKey.Id))
				{
					this._registeredHotKeys.Add(hotKey.Id, hotKey);
				}
			}
			if (this._registeredGameKeys.Count == 0)
			{
				int count = category.RegisteredGameKeys.Count;
				for (int i = 0; i < count; i++)
				{
					this._registeredGameKeys.Add(null);
				}
			}
			foreach (GameKey gameKey in category.RegisteredGameKeys)
			{
				if (gameKey != null)
				{
					this._registeredGameKeys[gameKey.Id] = gameKey;
				}
			}
			foreach (GameAxisKey gameAxisKey in category.RegisteredGameAxisKeys)
			{
				if (!this._registeredGameAxisKeys.ContainsKey(gameAxisKey.Id))
				{
					this._registeredGameAxisKeys.Add(gameAxisKey.Id, gameAxisKey);
				}
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000047E4 File Offset: 0x000029E4
		public bool IsCategoryRegistered(GameKeyContext category)
		{
			List<GameKeyContext> categories = this._categories;
			return categories != null && categories.Contains(category);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000047F8 File Offset: 0x000029F8
		private List<Key> GetAllAvailableKeys()
		{
			this._allKeysListMemoryCache.Clear();
			for (int i = 0; i < this._registeredGameKeys.Count; i++)
			{
				GameKey gameKey = this._registeredGameKeys[i];
				if (gameKey != null)
				{
					if (gameKey.KeyboardKey != null)
					{
						this._allKeysListMemoryCache.Add(gameKey.KeyboardKey);
					}
					if (gameKey.ControllerKey != null)
					{
						this._allKeysListMemoryCache.Add(gameKey.ControllerKey);
					}
				}
			}
			foreach (HotKey hotKey in this._registeredHotKeys.Values)
			{
				for (int j = 0; j < hotKey.Keys.Count; j++)
				{
					if (hotKey.Keys[j] != null)
					{
						this._allKeysListMemoryCache.Add(hotKey.Keys[j]);
					}
				}
			}
			return this._allKeysListMemoryCache;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00004908 File Offset: 0x00002B08
		public void RegisterDownKeys()
		{
			List<Key> allAvailableKeys = this.GetAllAvailableKeys();
			for (int i = 0; i < allAvailableKeys.Count; i++)
			{
				Key key = allAvailableKeys[i];
				if (key.IsPressed() && !this._downInputKeys.Contains(key))
				{
					this._downInputKeys.Add(key);
				}
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00004958 File Offset: 0x00002B58
		public void UnregisterReleasedKeys()
		{
			List<Key> allAvailableKeys = this.GetAllAvailableKeys();
			for (int i = 0; i < allAvailableKeys.Count; i++)
			{
				Key key = allAvailableKeys[i];
				if (key.IsReleased() && this._downInputKeys.Contains(key))
				{
					this._downInputKeys.Remove(key);
				}
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000049A8 File Offset: 0x00002BA8
		public void ResetLastDownKeys()
		{
			this._downInputKeys.Clear();
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000049B5 File Offset: 0x00002BB5
		private bool IsHotKeyDown(HotKey hotKey)
		{
			return hotKey.IsDown(this.IsKeysAllowed, this.IsMouseButtonAllowed && this.MouseOnMe, this.IsMouseWheelAllowed, this.IsControllerAllowed);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000049E0 File Offset: 0x00002BE0
		public bool IsHotKeyDown(string hotKey)
		{
			HotKey hotKey2;
			return this._registeredHotKeys.TryGetValue(hotKey, out hotKey2) && this.IsHotKeyDown(hotKey2);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00004A06 File Offset: 0x00002C06
		private bool IsGameKeyDown(GameKey gameKey)
		{
			return gameKey.IsDown(this.IsKeysAllowed, this.IsMouseButtonAllowed && this.MouseOnMe, this.IsMouseWheelAllowed, this.IsControllerAllowed, true);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00004A34 File Offset: 0x00002C34
		public bool IsGameKeyDown(int gameKey)
		{
			GameKey gameKey2 = this._registeredGameKeys[gameKey];
			return this.IsGameKeyDown(gameKey2);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00004A55 File Offset: 0x00002C55
		private bool IsGameKeyDownImmediate(GameKey gameKey)
		{
			return gameKey.IsDownImmediate(this.IsKeysAllowed, this.IsMouseButtonAllowed && this.MouseOnMe, this.IsMouseWheelAllowed, this.IsControllerAllowed);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00004A80 File Offset: 0x00002C80
		public bool IsGameKeyDownImmediate(int gameKey)
		{
			GameKey gameKey2 = this._registeredGameKeys[gameKey];
			return this.IsGameKeyDownImmediate(gameKey2);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00004AA1 File Offset: 0x00002CA1
		private bool IsHotKeyPressed(HotKey hotKey)
		{
			return hotKey.IsPressed(this.IsKeysAllowed, this.IsMouseButtonAllowed && this.MouseOnMe, this.IsMouseWheelAllowed, this.IsControllerAllowed);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00004ACC File Offset: 0x00002CCC
		public bool IsHotKeyPressed(string hotKey)
		{
			HotKey hotKey2;
			return this._registeredHotKeys.TryGetValue(hotKey, out hotKey2) && this.IsHotKeyPressed(hotKey2);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00004AF2 File Offset: 0x00002CF2
		private bool IsGameKeyPressed(GameKey gameKey)
		{
			return gameKey.IsPressed(this.IsKeysAllowed, this.IsMouseButtonAllowed && this.MouseOnMe, this.IsMouseWheelAllowed, this.IsControllerAllowed);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00004B20 File Offset: 0x00002D20
		public bool IsGameKeyPressed(int gameKey)
		{
			GameKey gameKey2 = this._registeredGameKeys[gameKey];
			return this.IsGameKeyPressed(gameKey2);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00004B44 File Offset: 0x00002D44
		private bool IsHotKeyReleased(HotKey hotKey)
		{
			for (int i = 0; i < hotKey.Keys.Count; i++)
			{
				if (this._downInputKeys.Contains(hotKey.Keys[i]))
				{
					return hotKey.IsReleased(this.IsKeysAllowed, this.IsMouseButtonAllowed && this.MouseOnMe, this.IsMouseWheelAllowed, this.IsControllerAllowed);
				}
			}
			return false;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00004BAC File Offset: 0x00002DAC
		public bool IsHotKeyReleased(string hotKey)
		{
			HotKey hotKey2;
			return this._registeredHotKeys.TryGetValue(hotKey, out hotKey2) && this.IsHotKeyReleased(hotKey2);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00004BD4 File Offset: 0x00002DD4
		private bool IsGameKeyReleased(GameKey gameKey)
		{
			return (this._downInputKeys.Contains(gameKey.KeyboardKey) || this._downInputKeys.Contains(gameKey.ControllerKey)) && gameKey.IsReleased(this.IsKeysAllowed, this.IsMouseButtonAllowed && this.MouseOnMe, this.IsMouseWheelAllowed, this.IsControllerAllowed);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00004C34 File Offset: 0x00002E34
		public bool IsGameKeyReleased(int gameKey)
		{
			GameKey gameKey2 = this._registeredGameKeys[gameKey];
			return this.IsGameKeyReleased(gameKey2);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00004C55 File Offset: 0x00002E55
		private float GetGameKeyState(GameKey gameKey)
		{
			return gameKey.GetKeyState(this.IsKeysAllowed, this.IsMouseButtonAllowed && this.MouseOnMe, this.IsMouseWheelAllowed, this.IsControllerAllowed);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00004C80 File Offset: 0x00002E80
		public float GetGameKeyState(int gameKey)
		{
			GameKey gameKey2 = this._registeredGameKeys[gameKey];
			return this.GetGameKeyState(gameKey2);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00004CA1 File Offset: 0x00002EA1
		private bool IsHotKeyDoublePressed(HotKey hotKey)
		{
			return hotKey.IsDoublePressed(this.IsKeysAllowed, this.IsMouseButtonAllowed && this.MouseOnMe, this.IsMouseWheelAllowed, this.IsControllerAllowed);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00004CCC File Offset: 0x00002ECC
		public bool IsHotKeyDoublePressed(string hotKey)
		{
			HotKey hotKey2;
			return this._registeredHotKeys.TryGetValue(hotKey, out hotKey2) && this.IsHotKeyDoublePressed(hotKey2);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00004CF2 File Offset: 0x00002EF2
		public float GetGameKeyAxis(GameAxisKey gameKey)
		{
			return gameKey.GetAxisState(this.IsKeysAllowed, this.IsMouseButtonAllowed && this.MouseOnMe, this.IsMouseWheelAllowed, this.IsControllerAllowed);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00004D20 File Offset: 0x00002F20
		public float GetGameKeyAxis(string gameKey)
		{
			GameAxisKey gameAxisKey;
			if (this._registeredGameAxisKeys.TryGetValue(gameKey, out gameAxisKey))
			{
				return this.GetGameKeyAxis(gameAxisKey);
			}
			return 0f;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00004D4C File Offset: 0x00002F4C
		internal bool CanUse(InputKey key)
		{
			InputKey[] clickKeys = Input.GetClickKeys();
			for (int i = 0; i < clickKeys.Length; i++)
			{
				if (clickKeys[i] == key)
				{
					return this.IsMouseButtonAllowed || this.IsControllerAllowed;
				}
			}
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
				return this.IsKeysAllowed;
			case InputKey.ControllerLStick:
			case InputKey.ControllerRStick:
			case InputKey.ControllerLStickUp:
			case InputKey.ControllerLStickDown:
			case InputKey.ControllerLStickLeft:
			case InputKey.ControllerLStickRight:
			case InputKey.ControllerRStickUp:
			case InputKey.ControllerRStickDown:
			case InputKey.ControllerRStickLeft:
			case InputKey.ControllerRStickRight:
			case InputKey.ControllerLUp:
			case InputKey.ControllerLDown:
			case InputKey.ControllerLLeft:
			case InputKey.ControllerLRight:
			case InputKey.ControllerRUp:
			case InputKey.ControllerRDown:
			case InputKey.ControllerRLeft:
			case InputKey.ControllerRRight:
			case InputKey.ControllerLBumper:
			case InputKey.ControllerRBumper:
			case InputKey.ControllerLOption:
			case InputKey.ControllerROption:
			case InputKey.ControllerLTrigger:
			case InputKey.ControllerRTrigger:
				return this.IsControllerAllowed;
			case InputKey.LeftMouseButton:
			case InputKey.RightMouseButton:
			case InputKey.MiddleMouseButton:
			case InputKey.X1MouseButton:
			case InputKey.X2MouseButton:
				return this.IsMouseButtonAllowed;
			case InputKey.MouseScrollUp:
			case InputKey.MouseScrollDown:
				return this.IsMouseWheelAllowed;
			}
			return false;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000051A7 File Offset: 0x000033A7
		public Vec2 GetKeyState(InputKey key)
		{
			if (!this.CanUse(key))
			{
				return new Vec2(0f, 0f);
			}
			return Input.GetKeyState(key);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000051C8 File Offset: 0x000033C8
		protected bool IsMouseButton(InputKey key)
		{
			return key == InputKey.LeftMouseButton || key == InputKey.RightMouseButton || key == InputKey.MiddleMouseButton;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000051E4 File Offset: 0x000033E4
		public bool IsKeyDown(InputKey key)
		{
			if (this.IsMouseButton(key))
			{
				if (!this.MouseOnMe)
				{
					return false;
				}
			}
			else if (!this.CanUse(key))
			{
				return false;
			}
			return Input.IsKeyDown(key);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000520A File Offset: 0x0000340A
		public bool IsKeyPressed(InputKey key)
		{
			return this.CanUse(key) && Input.IsKeyPressed(key);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000521D File Offset: 0x0000341D
		public bool IsKeyReleased(InputKey key)
		{
			if (this.IsMouseButton(key))
			{
				if (!this.MouseOnMe)
				{
					return false;
				}
			}
			else if (!this.CanUse(key))
			{
				return false;
			}
			return Input.IsKeyReleased(key);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005243 File Offset: 0x00003443
		public float GetMouseMoveX()
		{
			return Input.GetMouseMoveX();
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000524A File Offset: 0x0000344A
		public float GetMouseMoveY()
		{
			return Input.GetMouseMoveY();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00005251 File Offset: 0x00003451
		public float GetNormalizedMouseMoveX()
		{
			return Input.GetNormalizedMouseMoveX();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00005258 File Offset: 0x00003458
		public float GetNormalizedMouseMoveY()
		{
			return Input.GetNormalizedMouseMoveY();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000525F File Offset: 0x0000345F
		public Vec2 GetControllerRightStickState()
		{
			return Input.GetKeyState(InputKey.ControllerRStick);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000526B File Offset: 0x0000346B
		public Vec2 GetControllerLeftStickState()
		{
			return Input.GetKeyState(InputKey.ControllerLStick);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00005277 File Offset: 0x00003477
		public bool GetIsMouseActive()
		{
			return Input.IsMouseActive;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000527E File Offset: 0x0000347E
		public bool GetIsMouseDown()
		{
			return Input.IsKeyDown(InputKey.LeftMouseButton) || Input.IsKeyDown(InputKey.RightMouseButton);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00005298 File Offset: 0x00003498
		public Vec2 GetMousePositionPixel()
		{
			return Input.MousePositionPixel;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000529F File Offset: 0x0000349F
		public float GetDeltaMouseScroll()
		{
			if (!this.IsMouseWheelAllowed)
			{
				return 0f;
			}
			return Input.DeltaMouseScroll;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000052B4 File Offset: 0x000034B4
		public bool GetIsControllerConnected()
		{
			return Input.IsControllerConnected;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000052BB File Offset: 0x000034BB
		public Vec2 GetMousePositionRanged()
		{
			return Input.MousePositionRanged;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000052C2 File Offset: 0x000034C2
		public float GetMouseSensitivity()
		{
			return Input.MouseSensitivity;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000052C9 File Offset: 0x000034C9
		public bool IsControlDown()
		{
			return this.IsKeysAllowed && (Input.IsKeyDown(InputKey.LeftControl) || Input.IsKeyDown(InputKey.RightControl));
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000052EA File Offset: 0x000034EA
		public bool IsShiftDown()
		{
			return this.IsKeysAllowed && (Input.IsKeyDown(InputKey.LeftShift) || Input.IsKeyDown(InputKey.RightShift));
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00005308 File Offset: 0x00003508
		public bool IsAltDown()
		{
			return this.IsKeysAllowed && (Input.IsKeyDown(InputKey.LeftAlt) || Input.IsKeyDown(InputKey.RightAlt));
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00005329 File Offset: 0x00003529
		public InputKey[] GetClickKeys()
		{
			return Input.GetClickKeys();
		}

		// Token: 0x04000038 RID: 56
		private List<GameKey> _registeredGameKeys;

		// Token: 0x04000039 RID: 57
		private Dictionary<string, HotKey> _registeredHotKeys;

		// Token: 0x0400003A RID: 58
		private Dictionary<string, GameAxisKey> _registeredGameAxisKeys;

		// Token: 0x0400003B RID: 59
		private List<Key> _downInputKeys;

		// Token: 0x0400003C RID: 60
		private List<Key> _allKeysListMemoryCache = new List<Key>();

		// Token: 0x04000041 RID: 65
		private readonly List<GameKeyContext> _categories;
	}
}
