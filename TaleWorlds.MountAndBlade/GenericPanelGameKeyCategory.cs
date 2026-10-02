using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000235 RID: 565
	public class GenericPanelGameKeyCategory : GameKeyContext
	{
		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x0600214F RID: 8527 RVA: 0x00075B0D File Offset: 0x00073D0D
		// (set) Token: 0x06002150 RID: 8528 RVA: 0x00075B14 File Offset: 0x00073D14
		public static GenericPanelGameKeyCategory Current { get; private set; }

		// Token: 0x06002151 RID: 8529 RVA: 0x00075B1C File Offset: 0x00073D1C
		public GenericPanelGameKeyCategory(string categoryId = "GenericPanelGameKeyCategory")
			: base(categoryId, 116, GameKeyContext.GameKeyContextType.Default)
		{
			GenericPanelGameKeyCategory.Current = this;
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x00075B40 File Offset: 0x00073D40
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.Escape),
				new Key(InputKey.ControllerRRight)
			};
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.Enter),
				new Key(InputKey.NumpadEnter),
				new Key(InputKey.ControllerRLeft)
			};
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.ControllerRUp)
			};
			List<Key> list4 = new List<Key>
			{
				new Key(InputKey.Escape),
				new Key(InputKey.ControllerROption)
			};
			List<Key> list5 = new List<Key>
			{
				new Key(InputKey.Q),
				new Key(InputKey.ControllerLBumper)
			};
			List<Key> list6 = new List<Key>
			{
				new Key(InputKey.E),
				new Key(InputKey.ControllerRBumper)
			};
			List<Key> list7 = new List<Key>
			{
				new Key(InputKey.D),
				new Key(InputKey.ControllerRTrigger)
			};
			List<Key> list8 = new List<Key>
			{
				new Key(InputKey.A),
				new Key(InputKey.ControllerLTrigger)
			};
			List<Key> list9 = new List<Key>
			{
				new Key(InputKey.R),
				new Key(InputKey.ControllerRUp)
			};
			List<Key> list10 = new List<Key>
			{
				new Key(InputKey.ControllerROption)
			};
			List<Key> list11 = new List<Key>
			{
				new Key(InputKey.Delete),
				new Key(InputKey.ControllerRUp)
			};
			List<Key> list12 = new List<Key>
			{
				new Key(InputKey.Escape),
				new Key(InputKey.ControllerRUp)
			};
			List<Key> list13 = new List<Key>
			{
				new Key(InputKey.Enter),
				new Key(InputKey.NumpadEnter),
				new Key(InputKey.ControllerRDown)
			};
			base.RegisterHotKey(new HotKey("Exit", "GenericPanelGameKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Confirm", "GenericPanelGameKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Reset", "GenericPanelGameKeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ToggleEscapeMenu", "GenericPanelGameKeyCategory", list4, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("SwitchToPreviousTab", "GenericPanelGameKeyCategory", list5, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("SwitchToNextTab", "GenericPanelGameKeyCategory", list6, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("GiveAll", "GenericPanelGameKeyCategory", list7, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("TakeAll", "GenericPanelGameKeyCategory", list8, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Randomize", "GenericPanelGameKeyCategory", list9, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Start", "GenericPanelGameKeyCategory", list10, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Delete", "GenericPanelGameKeyCategory", list11, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("SelectProfile", "GenericPanelGameKeyCategory", list12, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Play", "GenericPanelGameKeyCategory", list13, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x00075E71 File Offset: 0x00074071
		private void RegisterGameKeys()
		{
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x00075E73 File Offset: 0x00074073
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000C4E RID: 3150
		public const string CategoryId = "GenericPanelGameKeyCategory";

		// Token: 0x04000C4F RID: 3151
		public const string Exit = "Exit";

		// Token: 0x04000C50 RID: 3152
		public const string Confirm = "Confirm";

		// Token: 0x04000C51 RID: 3153
		public const string ResetChanges = "Reset";

		// Token: 0x04000C52 RID: 3154
		public const string ToggleEscapeMenu = "ToggleEscapeMenu";

		// Token: 0x04000C53 RID: 3155
		public const string SwitchToPreviousTab = "SwitchToPreviousTab";

		// Token: 0x04000C54 RID: 3156
		public const string SwitchToNextTab = "SwitchToNextTab";

		// Token: 0x04000C55 RID: 3157
		public const string GiveAll = "GiveAll";

		// Token: 0x04000C56 RID: 3158
		public const string TakeAll = "TakeAll";

		// Token: 0x04000C57 RID: 3159
		public const string Randomize = "Randomize";

		// Token: 0x04000C58 RID: 3160
		public const string Start = "Start";

		// Token: 0x04000C59 RID: 3161
		public const string Delete = "Delete";

		// Token: 0x04000C5A RID: 3162
		public const string SelectProfile = "SelectProfile";

		// Token: 0x04000C5B RID: 3163
		public const string Play = "Play";
	}
}
