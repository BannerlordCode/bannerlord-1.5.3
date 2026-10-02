using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000233 RID: 563
	public sealed class GenericCampaignPanelsGameKeyCategory : GameKeyContext
	{
		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002144 RID: 8516 RVA: 0x0007578A File Offset: 0x0007398A
		// (set) Token: 0x06002145 RID: 8517 RVA: 0x00075791 File Offset: 0x00073991
		public static GenericCampaignPanelsGameKeyCategory Current { get; private set; }

		// Token: 0x06002146 RID: 8518 RVA: 0x00075799 File Offset: 0x00073999
		public GenericCampaignPanelsGameKeyCategory(string categoryId = "GenericCampaignPanelsGameKeyCategory")
			: base(categoryId, 116, GameKeyContext.GameKeyContextType.Default)
		{
			GenericCampaignPanelsGameKeyCategory.Current = this;
			this.RegisterHotKeys();
			this.RegisterGameKeys();
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x000757B8 File Offset: 0x000739B8
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.LeftShift),
				new Key(InputKey.RightShift)
			};
			base.RegisterHotKey(new HotKey("FiveStackModifier", "GenericCampaignPanelsGameKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.LeftControl),
				new Key(InputKey.RightControl)
			};
			base.RegisterHotKey(new HotKey("EntireStackModifier", "GenericCampaignPanelsGameKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x0007583C File Offset: 0x00073A3C
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(36, "BannerWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.B, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(37, "CharacterWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.C, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(38, "InventoryWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.I, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(39, "EncyclopediaWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.N, InputKey.ControllerLOption, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(40, "KingdomWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.K, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(41, "ClanWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.L, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(42, "QuestsWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.J, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(43, "PartyWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.P, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(44, "FacegenWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.V, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(45, "ManageFleetWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.U, GameKeyMainCategories.MenuShortcutCategory), true);
		}

		// Token: 0x04000C34 RID: 3124
		public const string CategoryId = "GenericCampaignPanelsGameKeyCategory";

		// Token: 0x04000C35 RID: 3125
		public const string FiveStackModifier = "FiveStackModifier";

		// Token: 0x04000C36 RID: 3126
		public const string EntireStackModifier = "EntireStackModifier";

		// Token: 0x04000C37 RID: 3127
		public const int BannerWindow = 36;

		// Token: 0x04000C38 RID: 3128
		public const int CharacterWindow = 37;

		// Token: 0x04000C39 RID: 3129
		public const int InventoryWindow = 38;

		// Token: 0x04000C3A RID: 3130
		public const int EncyclopediaWindow = 39;

		// Token: 0x04000C3B RID: 3131
		public const int PartyWindow = 43;

		// Token: 0x04000C3C RID: 3132
		public const int KingdomWindow = 40;

		// Token: 0x04000C3D RID: 3133
		public const int ClanWindow = 41;

		// Token: 0x04000C3E RID: 3134
		public const int QuestsWindow = 42;

		// Token: 0x04000C3F RID: 3135
		public const int FacegenWindow = 44;

		// Token: 0x04000C40 RID: 3136
		public const int ManageFleetWindow = 45;
	}
}
