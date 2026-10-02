using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000237 RID: 567
	public sealed class MapHotKeyCategory : GameKeyContext
	{
		// Token: 0x06002159 RID: 8537 RVA: 0x00075EB7 File Offset: 0x000740B7
		public MapHotKeyCategory()
			: base("MapHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x00075EDC File Offset: 0x000740DC
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRDown)
			};
			base.RegisterHotKey(new HotKey("MapClick", "MapHotKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.ControllerLOptionTap)
			};
			base.RegisterHotKey(new HotKey("MapTouchpadClick", "MapHotKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.LeftAlt),
				new Key(InputKey.ControllerLBumper)
			};
			base.RegisterHotKey(new HotKey("MapFollowModifier", "MapHotKeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list4 = new List<Key>
			{
				new Key(InputKey.ControllerRRight)
			};
			base.RegisterHotKey(new HotKey("MapChangeCursorMode", "MapHotKeyCategory", list4, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x00075FC4 File Offset: 0x000741C4
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(50, "PartyMoveUp", "MapHotKeyCategory", InputKey.Up, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(51, "PartyMoveDown", "MapHotKeyCategory", InputKey.Down, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(52, "PartyMoveLeft", "MapHotKeyCategory", InputKey.Left, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(53, "PartyMoveRight", "MapHotKeyCategory", InputKey.Right, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(54, "QuickSave", "MapHotKeyCategory", InputKey.F5, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(55, "MapFastMove", "MapHotKeyCategory", InputKey.LeftShift, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(56, "MapZoomIn", "MapHotKeyCategory", InputKey.MouseScrollUp, InputKey.ControllerRTrigger, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(57, "MapZoomOut", "MapHotKeyCategory", InputKey.MouseScrollDown, InputKey.ControllerLTrigger, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(58, "MapRotateLeft", "MapHotKeyCategory", InputKey.Q, InputKey.Invalid, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(59, "MapRotateRight", "MapHotKeyCategory", InputKey.E, InputKey.Invalid, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(60, "MapTimeStop", "MapHotKeyCategory", InputKey.D1, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(61, "MapTimeNormal", "MapHotKeyCategory", InputKey.D2, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(62, "MapTimeFastForward", "MapHotKeyCategory", InputKey.D3, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(63, "MapTimeTogglePause", "MapHotKeyCategory", InputKey.Space, InputKey.ControllerRLeft, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(64, "MapCameraFollowMode", "MapHotKeyCategory", InputKey.Invalid, InputKey.ControllerLThumb, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(65, "MapToggleFastForward", "MapHotKeyCategory", InputKey.Invalid, InputKey.ControllerRBumper, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(66, "MapTrackSettlement", "MapHotKeyCategory", InputKey.Invalid, InputKey.ControllerRThumb, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(67, "MapGoToEncylopedia", "MapHotKeyCategory", InputKey.Invalid, InputKey.ControllerLOption, GameKeyMainCategories.CampaignMapCategory), true);
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x00076230 File Offset: 0x00074430
		private void RegisterGameAxisKeys()
		{
			GameKey gameKey = new GameKey(46, "MapMoveUp", "MapHotKeyCategory", InputKey.W, GameKeyMainCategories.CampaignMapCategory);
			GameKey gameKey2 = new GameKey(47, "MapMoveDown", "MapHotKeyCategory", InputKey.S, GameKeyMainCategories.CampaignMapCategory);
			GameKey gameKey3 = new GameKey(48, "MapMoveLeft", "MapHotKeyCategory", InputKey.A, GameKeyMainCategories.CampaignMapCategory);
			GameKey gameKey4 = new GameKey(49, "MapMoveRight", "MapHotKeyCategory", InputKey.D, GameKeyMainCategories.CampaignMapCategory);
			base.RegisterGameKey(gameKey, true);
			base.RegisterGameKey(gameKey2, true);
			base.RegisterGameKey(gameKey3, true);
			base.RegisterGameKey(gameKey4, true);
			base.RegisterGameAxisKey(new GameAxisKey("MapMovementAxisX", InputKey.ControllerLStick, gameKey4, gameKey3, GameAxisKey.AxisType.X), true);
			base.RegisterGameAxisKey(new GameAxisKey("MapMovementAxisY", InputKey.ControllerLStick, gameKey, gameKey2, GameAxisKey.AxisType.Y), true);
		}

		// Token: 0x04000C5F RID: 3167
		public const string CategoryId = "MapHotKeyCategory";

		// Token: 0x04000C60 RID: 3168
		public const int QuickSave = 54;

		// Token: 0x04000C61 RID: 3169
		public const int PartyMoveUp = 50;

		// Token: 0x04000C62 RID: 3170
		public const int PartyMoveLeft = 52;

		// Token: 0x04000C63 RID: 3171
		public const int PartyMoveDown = 51;

		// Token: 0x04000C64 RID: 3172
		public const int PartyMoveRight = 53;

		// Token: 0x04000C65 RID: 3173
		public const int MapMoveUp = 46;

		// Token: 0x04000C66 RID: 3174
		public const int MapMoveDown = 47;

		// Token: 0x04000C67 RID: 3175
		public const int MapMoveLeft = 48;

		// Token: 0x04000C68 RID: 3176
		public const int MapMoveRight = 49;

		// Token: 0x04000C69 RID: 3177
		public const string MovementAxisX = "MapMovementAxisX";

		// Token: 0x04000C6A RID: 3178
		public const string MovementAxisY = "MapMovementAxisY";

		// Token: 0x04000C6B RID: 3179
		public const int MapFastMove = 55;

		// Token: 0x04000C6C RID: 3180
		public const int MapZoomIn = 56;

		// Token: 0x04000C6D RID: 3181
		public const int MapZoomOut = 57;

		// Token: 0x04000C6E RID: 3182
		public const int MapRotateLeft = 58;

		// Token: 0x04000C6F RID: 3183
		public const int MapRotateRight = 59;

		// Token: 0x04000C70 RID: 3184
		public const int MapCameraFollowMode = 64;

		// Token: 0x04000C71 RID: 3185
		public const int MapToggleFastForward = 65;

		// Token: 0x04000C72 RID: 3186
		public const int MapTrackSettlement = 66;

		// Token: 0x04000C73 RID: 3187
		public const int MapGoToEncylopedia = 67;

		// Token: 0x04000C74 RID: 3188
		public const string MapClick = "MapClick";

		// Token: 0x04000C75 RID: 3189
		public const string MapTouchpadClick = "MapTouchpadClick";

		// Token: 0x04000C76 RID: 3190
		public const string MapFollowModifier = "MapFollowModifier";

		// Token: 0x04000C77 RID: 3191
		public const string MapChangeCursorMode = "MapChangeCursorMode";

		// Token: 0x04000C78 RID: 3192
		public const int MapTimeStop = 60;

		// Token: 0x04000C79 RID: 3193
		public const int MapTimeNormal = 61;

		// Token: 0x04000C7A RID: 3194
		public const int MapTimeFastForward = 62;

		// Token: 0x04000C7B RID: 3195
		public const int MapTimeTogglePause = 63;
	}
}
