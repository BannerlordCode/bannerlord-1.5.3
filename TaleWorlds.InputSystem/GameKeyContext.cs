using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.InputSystem
{
	// Token: 0x02000006 RID: 6
	public abstract class GameKeyContext
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000074 RID: 116 RVA: 0x0000295E File Offset: 0x00000B5E
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00002966 File Offset: 0x00000B66
		public string GameKeyCategoryId { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000076 RID: 118 RVA: 0x0000296F File Offset: 0x00000B6F
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00002977 File Offset: 0x00000B77
		public GameKeyContext.GameKeyContextType Type { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002980 File Offset: 0x00000B80
		public MBReadOnlyList<GameKey> RegisteredGameKeys
		{
			get
			{
				return this._registeredGameKeys;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002988 File Offset: 0x00000B88
		public Dictionary<string, HotKey>.ValueCollection RegisteredHotKeys
		{
			get
			{
				return this._registeredHotKeys.Values;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002995 File Offset: 0x00000B95
		public Dictionary<string, GameAxisKey>.ValueCollection RegisteredGameAxisKeys
		{
			get
			{
				return this._registeredAxisKeys.Values;
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000029A4 File Offset: 0x00000BA4
		protected GameKeyContext(string id, int gameKeysCount, GameKeyContext.GameKeyContextType type = GameKeyContext.GameKeyContextType.Default)
		{
			this.GameKeyCategoryId = id;
			this.Type = type;
			this._registeredHotKeys = new Dictionary<string, HotKey>();
			this._registeredAxisKeys = new Dictionary<string, GameAxisKey>();
			this._registeredGameKeys = new MBList<GameKey>(gameKeysCount);
			for (int i = 0; i < gameKeysCount; i++)
			{
				this._registeredGameKeys.Add(null);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002A00 File Offset: 0x00000C00
		protected internal void RegisterHotKey(HotKey gameKey, bool addIfMissing = true)
		{
			if (GameKeyContext._isRDownSwappedWithRRight)
			{
				for (int i = 0; i < gameKey.Keys.Count; i++)
				{
					Key key = gameKey.Keys[i];
					if (key != null && key.InputKey == InputKey.ControllerRDown)
					{
						key.ChangeKey(InputKey.ControllerRRight);
					}
					else if (key != null && key.InputKey == InputKey.ControllerRRight)
					{
						key.ChangeKey(InputKey.ControllerRDown);
					}
				}
			}
			if (this._registeredHotKeys.ContainsKey(gameKey.Id))
			{
				this._registeredHotKeys[gameKey.Id] = gameKey;
				return;
			}
			if (addIfMissing)
			{
				this._registeredHotKeys.Add(gameKey.Id, gameKey);
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002AAC File Offset: 0x00000CAC
		protected internal void RegisterGameKey(GameKey gameKey, bool addIfMissing = true)
		{
			if (GameKeyContext._isRDownSwappedWithRRight)
			{
				Key controllerKey = gameKey.ControllerKey;
				if (controllerKey != null && controllerKey.InputKey == InputKey.ControllerRDown)
				{
					controllerKey.ChangeKey(InputKey.ControllerRRight);
				}
				else if (controllerKey != null && controllerKey.InputKey == InputKey.ControllerRRight)
				{
					controllerKey.ChangeKey(InputKey.ControllerRDown);
				}
			}
			this._registeredGameKeys[gameKey.Id] = gameKey;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002B11 File Offset: 0x00000D11
		protected internal void RegisterGameAxisKey(GameAxisKey gameKey, bool addIfMissing = true)
		{
			if (this._registeredAxisKeys.ContainsKey(gameKey.Id))
			{
				this._registeredAxisKeys[gameKey.Id] = gameKey;
				return;
			}
			if (addIfMissing)
			{
				this._registeredAxisKeys.Add(gameKey.Id, gameKey);
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002B4E File Offset: 0x00000D4E
		internal static void SetIsRDownSwappedWithRRight(bool value)
		{
			GameKeyContext._isRDownSwappedWithRRight = value;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002B58 File Offset: 0x00000D58
		public HotKey GetHotKey(string hotKeyId)
		{
			HotKey hotKey = null;
			this._registeredHotKeys.TryGetValue(hotKeyId, out hotKey);
			return hotKey;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002B78 File Offset: 0x00000D78
		public GameKey GetGameKey(int gameKeyId)
		{
			for (int i = 0; i < this._registeredGameKeys.Count; i++)
			{
				GameKey gameKey = this._registeredGameKeys[i];
				if (gameKey != null && gameKey.Id == gameKeyId)
				{
					return gameKey;
				}
			}
			Debug.FailedAssert(string.Format("Couldn't find {0} in {1}", gameKeyId, this.GameKeyCategoryId), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.InputSystem\\GameKeyContext.cs", "GetGameKey", 125);
			return null;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002BE0 File Offset: 0x00000DE0
		internal GameKey GetGameKey(string gameKeyId)
		{
			for (int i = 0; i < this._registeredGameKeys.Count; i++)
			{
				GameKey gameKey = this._registeredGameKeys[i];
				if (gameKey != null && gameKey.StringId == gameKeyId)
				{
					return gameKey;
				}
			}
			Debug.FailedAssert("Couldn't find " + gameKeyId + " in " + this.GameKeyCategoryId, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.InputSystem\\GameKeyContext.cs", "GetGameKey", 140);
			return null;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002C50 File Offset: 0x00000E50
		internal GameAxisKey GetGameAxisKey(string axisKeyId)
		{
			GameAxisKey gameAxisKey;
			this._registeredAxisKeys.TryGetValue(axisKeyId, out gameAxisKey);
			return gameAxisKey;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002C70 File Offset: 0x00000E70
		public string GetHotKeyId(string hotKeyId)
		{
			HotKey hotKey;
			if (this._registeredHotKeys.TryGetValue(hotKeyId, out hotKey))
			{
				return hotKey.ToString();
			}
			GameAxisKey gameAxisKey;
			if (this._registeredAxisKeys.TryGetValue(hotKeyId, out gameAxisKey))
			{
				return gameAxisKey.ToString();
			}
			Debug.FailedAssert("HotKey with id: " + hotKeyId + " is not registered.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.InputSystem\\GameKeyContext.cs", "GetHotKeyId", 163);
			return "";
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002CD4 File Offset: 0x00000ED4
		public string GetHotKeyId(int gameKeyId)
		{
			GameKey gameKey = this._registeredGameKeys[gameKeyId];
			if (gameKey != null)
			{
				return gameKey.ToString();
			}
			Debug.FailedAssert("GameKey with id: " + gameKeyId + " is not registered.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.InputSystem\\GameKeyContext.cs", "GetHotKeyId", 175);
			return "";
		}

		// Token: 0x04000012 RID: 18
		private readonly Dictionary<string, HotKey> _registeredHotKeys;

		// Token: 0x04000013 RID: 19
		private readonly MBList<GameKey> _registeredGameKeys;

		// Token: 0x04000014 RID: 20
		private readonly Dictionary<string, GameAxisKey> _registeredAxisKeys;

		// Token: 0x04000015 RID: 21
		private static bool _isRDownSwappedWithRRight = true;

		// Token: 0x02000012 RID: 18
		public enum GameKeyContextType
		{
			// Token: 0x04000159 RID: 345
			Default,
			// Token: 0x0400015A RID: 346
			AuxiliaryNotSerialized,
			// Token: 0x0400015B RID: 347
			AuxiliarySerialized,
			// Token: 0x0400015C RID: 348
			AuxiliarySerializedAndShownInOptions
		}
	}
}
