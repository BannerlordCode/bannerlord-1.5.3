using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200023B RID: 571
	public sealed class PartyHotKeyCategory : GameKeyContext
	{
		// Token: 0x06002168 RID: 8552 RVA: 0x000769AC File Offset: 0x00074BAC
		public PartyHotKeyCategory()
			: base("PartyHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x000769D0 File Offset: 0x00074BD0
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.Q),
				new Key(InputKey.ControllerLTrigger)
			};
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.E),
				new Key(InputKey.ControllerRTrigger)
			};
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.A),
				new Key(InputKey.ControllerLBumper)
			};
			List<Key> list4 = new List<Key>
			{
				new Key(InputKey.D),
				new Key(InputKey.ControllerRBumper)
			};
			List<Key> list5 = new List<Key>
			{
				new Key(InputKey.ControllerLBumper)
			};
			List<Key> list6 = new List<Key>
			{
				new Key(InputKey.ControllerRBumper)
			};
			List<Key> list7 = new List<Key>
			{
				new Key(InputKey.ControllerLThumb)
			};
			List<Key> list8 = new List<Key>
			{
				new Key(InputKey.ControllerRThumb)
			};
			base.RegisterHotKey(new HotKey("TakeAllTroops", "PartyHotKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("GiveAllTroops", "PartyHotKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("TakeAllPrisoners", "PartyHotKeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("GiveAllPrisoners", "PartyHotKeyCategory", list4, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("OpenUpgradePopup", "PartyHotKeyCategory", list7, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("OpenRecruitPopup", "PartyHotKeyCategory", list8, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("PopupItemPrimaryAction", "PartyHotKeyCategory", list5, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("PopupItemSecondaryAction", "PartyHotKeyCategory", list6, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x00076B91 File Offset: 0x00074D91
		private void RegisterGameKeys()
		{
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x00076B93 File Offset: 0x00074D93
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000CB6 RID: 3254
		public const string CategoryId = "PartyHotKeyCategory";

		// Token: 0x04000CB7 RID: 3255
		public const string TakeAllTroops = "TakeAllTroops";

		// Token: 0x04000CB8 RID: 3256
		public const string GiveAllTroops = "GiveAllTroops";

		// Token: 0x04000CB9 RID: 3257
		public const string TakeAllPrisoners = "TakeAllPrisoners";

		// Token: 0x04000CBA RID: 3258
		public const string GiveAllPrisoners = "GiveAllPrisoners";

		// Token: 0x04000CBB RID: 3259
		public const string PopupItemPrimaryAction = "PopupItemPrimaryAction";

		// Token: 0x04000CBC RID: 3260
		public const string PopupItemSecondaryAction = "PopupItemSecondaryAction";

		// Token: 0x04000CBD RID: 3261
		public const string OpenUpgradePopup = "OpenUpgradePopup";

		// Token: 0x04000CBE RID: 3262
		public const string OpenRecruitPopup = "OpenRecruitPopup";
	}
}
