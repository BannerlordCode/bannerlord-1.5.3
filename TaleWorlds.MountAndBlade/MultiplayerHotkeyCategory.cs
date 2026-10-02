using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200023A RID: 570
	public sealed class MultiplayerHotkeyCategory : GameKeyContext
	{
		// Token: 0x06002163 RID: 8547 RVA: 0x00076669 File Offset: 0x00074869
		public MultiplayerHotkeyCategory()
			: base("MultiplayerHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x0007668C File Offset: 0x0007488C
		private void RegisterHotKeys()
		{
			for (int i = 0; i < MultiplayerHotkeyCategory.CameraPositionDigitKeys.Length; i++)
			{
				base.RegisterHotKey(new HotKey("StoreCameraPosition" + (i + 1).ToString(), "MultiplayerHotkeyCategory", MultiplayerHotkeyCategory.CameraPositionDigitKeys[i], HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			}
			for (int j = 0; j < MultiplayerHotkeyCategory.CameraPositionDigitKeys.Length; j++)
			{
				base.RegisterHotKey(new HotKey("SpectateCameraPosition" + (j + 1).ToString(), "MultiplayerHotkeyCategory", MultiplayerHotkeyCategory.CameraPositionDigitKeys[j], HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			}
			List<Key> list = new List<Key>
			{
				new Key(InputKey.RightMouseButton),
				new Key(InputKey.ControllerRUp)
			};
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRDown)
			};
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.RightMouseButton),
				new Key(InputKey.ControllerRUp)
			};
			List<Key> list4 = new List<Key>
			{
				new Key(InputKey.F),
				new Key(InputKey.ControllerRLeft)
			};
			List<Key> list5 = new List<Key>
			{
				new Key(InputKey.V),
				new Key(InputKey.ControllerRRight)
			};
			base.RegisterHotKey(new HotKey("PerformActionOnCosmeticItem", "MultiplayerHotkeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("PreviewCosmeticItem", "MultiplayerHotkeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("InspectBadgeProgression", "MultiplayerHotkeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ToggleFriendsList", "MultiplayerHotkeyCategory", list4, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CycleSpectatorCamera", "MultiplayerHotkeyCategory", list5, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list6 = new List<Key>
			{
				new Key(InputKey.Q),
				new Key(InputKey.ControllerLBumper)
			};
			base.RegisterHotKey(new HotKey("CycleSpectatorTargetPrevious", "MultiplayerHotkeyCategory", list6, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list7 = new List<Key>
			{
				new Key(InputKey.E),
				new Key(InputKey.ControllerRBumper)
			};
			base.RegisterHotKey(new HotKey("CycleSpectatorTargetNext", "MultiplayerHotkeyCategory", list7, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x000768DC File Offset: 0x00074ADC
		private void RegisterGameKeys()
		{
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x000768DE File Offset: 0x00074ADE
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000C97 RID: 3223
		public const string CategoryId = "MultiplayerHotkeyCategory";

		// Token: 0x04000C98 RID: 3224
		private const string _storeCameraPositionBase = "StoreCameraPosition";

		// Token: 0x04000C99 RID: 3225
		public const string StoreCameraPosition1 = "StoreCameraPosition1";

		// Token: 0x04000C9A RID: 3226
		public const string StoreCameraPosition2 = "StoreCameraPosition2";

		// Token: 0x04000C9B RID: 3227
		public const string StoreCameraPosition3 = "StoreCameraPosition3";

		// Token: 0x04000C9C RID: 3228
		public const string StoreCameraPosition4 = "StoreCameraPosition4";

		// Token: 0x04000C9D RID: 3229
		public const string StoreCameraPosition5 = "StoreCameraPosition5";

		// Token: 0x04000C9E RID: 3230
		public const string StoreCameraPosition6 = "StoreCameraPosition6";

		// Token: 0x04000C9F RID: 3231
		public const string StoreCameraPosition7 = "StoreCameraPosition7";

		// Token: 0x04000CA0 RID: 3232
		public const string StoreCameraPosition8 = "StoreCameraPosition8";

		// Token: 0x04000CA1 RID: 3233
		public const string StoreCameraPosition9 = "StoreCameraPosition9";

		// Token: 0x04000CA2 RID: 3234
		private const string _spectateCameraPositionBase = "SpectateCameraPosition";

		// Token: 0x04000CA3 RID: 3235
		public const string SpectateCameraPosition1 = "SpectateCameraPosition1";

		// Token: 0x04000CA4 RID: 3236
		public const string SpectateCameraPosition2 = "SpectateCameraPosition2";

		// Token: 0x04000CA5 RID: 3237
		public const string SpectateCameraPosition3 = "SpectateCameraPosition3";

		// Token: 0x04000CA6 RID: 3238
		public const string SpectateCameraPosition4 = "SpectateCameraPosition4";

		// Token: 0x04000CA7 RID: 3239
		public const string SpectateCameraPosition5 = "SpectateCameraPosition5";

		// Token: 0x04000CA8 RID: 3240
		public const string SpectateCameraPosition6 = "SpectateCameraPosition6";

		// Token: 0x04000CA9 RID: 3241
		public const string SpectateCameraPosition7 = "SpectateCameraPosition7";

		// Token: 0x04000CAA RID: 3242
		public const string SpectateCameraPosition8 = "SpectateCameraPosition8";

		// Token: 0x04000CAB RID: 3243
		public const string SpectateCameraPosition9 = "SpectateCameraPosition9";

		// Token: 0x04000CAC RID: 3244
		public const string CycleSpectatorCamera = "CycleSpectatorCamera";

		// Token: 0x04000CAD RID: 3245
		public const string CycleSpectatorTargetPrevious = "CycleSpectatorTargetPrevious";

		// Token: 0x04000CAE RID: 3246
		public const string CycleSpectatorTargetNext = "CycleSpectatorTargetNext";

		// Token: 0x04000CAF RID: 3247
		public const string InspectBadgeProgression = "InspectBadgeProgression";

		// Token: 0x04000CB0 RID: 3248
		public const string PerformActionOnCosmeticItem = "PerformActionOnCosmeticItem";

		// Token: 0x04000CB1 RID: 3249
		public const string PreviewCosmeticItem = "PreviewCosmeticItem";

		// Token: 0x04000CB2 RID: 3250
		public const string ToggleFriendsList = "ToggleFriendsList";

		// Token: 0x04000CB3 RID: 3251
		private static readonly InputKey[] CameraPositionDigitKeys = new InputKey[]
		{
			InputKey.D1,
			InputKey.D2,
			InputKey.D3,
			InputKey.D4,
			InputKey.D5,
			InputKey.D6,
			InputKey.D7,
			InputKey.D8,
			InputKey.D9
		};

		// Token: 0x04000CB4 RID: 3252
		public static readonly string[] StoreCameraPositionHotKeys = new string[] { "StoreCameraPosition1", "StoreCameraPosition2", "StoreCameraPosition3", "StoreCameraPosition4", "StoreCameraPosition5", "StoreCameraPosition6", "StoreCameraPosition7", "StoreCameraPosition8", "StoreCameraPosition9" };

		// Token: 0x04000CB5 RID: 3253
		public static readonly string[] SpectateCameraPositionHotKeys = new string[] { "SpectateCameraPosition1", "SpectateCameraPosition2", "SpectateCameraPosition3", "SpectateCameraPosition4", "SpectateCameraPosition5", "SpectateCameraPosition6", "SpectateCameraPosition7", "SpectateCameraPosition8", "SpectateCameraPosition9" };
	}
}
