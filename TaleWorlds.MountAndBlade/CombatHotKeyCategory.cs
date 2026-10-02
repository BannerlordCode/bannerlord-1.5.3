using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200022C RID: 556
	public sealed class CombatHotKeyCategory : GameKeyContext
	{
		// Token: 0x06002130 RID: 8496 RVA: 0x00074D2E File Offset: 0x00072F2E
		public CombatHotKeyCategory()
			: base("CombatHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x00074D50 File Offset: 0x00072F50
		private void RegisterHotKeys()
		{
			base.RegisterHotKey(new HotKey("DeploymentCameraIsActive", "CombatHotKeyCategory", InputKey.MiddleMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ToggleZoom", "CombatHotKeyCategory", InputKey.ControllerRThumb, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerEquipDropWeapon1", "CombatHotKeyCategory", InputKey.ControllerRRight, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerEquipDropWeapon2", "CombatHotKeyCategory", InputKey.ControllerRUp, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerEquipDropWeapon3", "CombatHotKeyCategory", InputKey.ControllerRLeft, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerEquipDropWeapon4", "CombatHotKeyCategory", InputKey.ControllerRDown, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerEquipDropExtraWeapon", "CombatHotKeyCategory", InputKey.ControllerRThumb, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkSelectFirstCategory", "CombatHotKeyCategory", new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRLeft)
			}, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkSelectSecondCategory", "CombatHotKeyCategory", new List<Key>
			{
				new Key(InputKey.RightMouseButton),
				new Key(InputKey.ControllerRRight)
			}, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkCloseMenu", "CombatHotKeyCategory", InputKey.ControllerRThumb, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkItem1", "CombatHotKeyCategory", InputKey.ControllerRUp, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkItem2", "CombatHotKeyCategory", InputKey.ControllerRRight, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkItem3", "CombatHotKeyCategory", InputKey.ControllerRDown, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkItem4", "CombatHotKeyCategory", InputKey.ControllerRLeft, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ForfeitSpawn", "CombatHotKeyCategory", new List<Key>
			{
				new Key(InputKey.X),
				new Key(InputKey.ControllerRLeft)
			}, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControlModeToggle", "CombatHotKeyCategory", InputKey.ControllerLDown, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerToggleWalk", "CombatHotKeyCategory", InputKey.ControllerRUp, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerToggleCrouch", "CombatHotKeyCategory", InputKey.ControllerRDown, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x00074FC4 File Offset: 0x000731C4
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(9, "Attack", "CombatHotKeyCategory", InputKey.LeftMouseButton, InputKey.ControllerRTrigger, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(10, "Defend", "CombatHotKeyCategory", InputKey.RightMouseButton, InputKey.ControllerLTrigger, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(11, "EquipPrimaryWeapon", "CombatHotKeyCategory", InputKey.MouseScrollUp, InputKey.Invalid, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(12, "EquipSecondaryWeapon", "CombatHotKeyCategory", InputKey.MouseScrollDown, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(13, "Action", "CombatHotKeyCategory", InputKey.F, InputKey.ControllerRUp, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(14, "Jump", "CombatHotKeyCategory", InputKey.Space, InputKey.ControllerRDown, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(15, "Crouch", "CombatHotKeyCategory", InputKey.Z, InputKey.ControllerLDown, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(16, "Kick", "CombatHotKeyCategory", InputKey.E, InputKey.ControllerRLeft, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(17, "ToggleWeaponMode", "CombatHotKeyCategory", InputKey.X, InputKey.Invalid, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(18, "EquipWeapon1", "CombatHotKeyCategory", InputKey.Numpad1, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(19, "EquipWeapon2", "CombatHotKeyCategory", InputKey.Numpad2, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(20, "EquipWeapon3", "CombatHotKeyCategory", InputKey.Numpad3, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(21, "EquipWeapon4", "CombatHotKeyCategory", InputKey.Numpad4, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(22, "DropWeapon", "CombatHotKeyCategory", InputKey.G, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(23, "SheathWeapon", "CombatHotKeyCategory", InputKey.BackSlash, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(24, "Zoom", "CombatHotKeyCategory", InputKey.LeftShift, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(25, "ViewCharacter", "CombatHotKeyCategory", InputKey.Tilde, InputKey.ControllerLLeft, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(26, "LockTarget", "CombatHotKeyCategory", InputKey.MiddleMouseButton, InputKey.ControllerRThumb, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(27, "CameraToggle", "CombatHotKeyCategory", InputKey.R, InputKey.ControllerLThumb, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(28, "MissionScreenHotkeyCameraZoomIn", "CombatHotKeyCategory", InputKey.NumpadPlus, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(29, "MissionScreenHotkeyCameraZoomOut", "CombatHotKeyCategory", InputKey.NumpadMinus, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(30, "ToggleWalkMode", "CombatHotKeyCategory", InputKey.CapsLock, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(31, "Cheer", "CombatHotKeyCategory", InputKey.O, InputKey.ControllerLUp, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(33, "PushToTalk", "CombatHotKeyCategory", InputKey.V, InputKey.ControllerLRight, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(34, "EquipmentSwitch", "CombatHotKeyCategory", InputKey.U, InputKey.ControllerRBumper, GameKeyMainCategories.ActionCategory), true);
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x00075325 File Offset: 0x00073525
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000B72 RID: 2930
		public const string CategoryId = "CombatHotKeyCategory";

		// Token: 0x04000B73 RID: 2931
		public const int MissionScreenHotkeyCameraZoomIn = 28;

		// Token: 0x04000B74 RID: 2932
		public const int MissionScreenHotkeyCameraZoomOut = 29;

		// Token: 0x04000B75 RID: 2933
		public const int Action = 13;

		// Token: 0x04000B76 RID: 2934
		public const int Jump = 14;

		// Token: 0x04000B77 RID: 2935
		public const int Crouch = 15;

		// Token: 0x04000B78 RID: 2936
		public const int Attack = 9;

		// Token: 0x04000B79 RID: 2937
		public const int Defend = 10;

		// Token: 0x04000B7A RID: 2938
		public const int Kick = 16;

		// Token: 0x04000B7B RID: 2939
		public const int ToggleWeaponMode = 17;

		// Token: 0x04000B7C RID: 2940
		public const int ToggleWalkMode = 30;

		// Token: 0x04000B7D RID: 2941
		public const int EquipWeapon1 = 18;

		// Token: 0x04000B7E RID: 2942
		public const int EquipWeapon2 = 19;

		// Token: 0x04000B7F RID: 2943
		public const int EquipWeapon3 = 20;

		// Token: 0x04000B80 RID: 2944
		public const int EquipWeapon4 = 21;

		// Token: 0x04000B81 RID: 2945
		public const int EquipPrimaryWeapon = 11;

		// Token: 0x04000B82 RID: 2946
		public const int EquipSecondaryWeapon = 12;

		// Token: 0x04000B83 RID: 2947
		public const int DropWeapon = 22;

		// Token: 0x04000B84 RID: 2948
		public const int SheathWeapon = 23;

		// Token: 0x04000B85 RID: 2949
		public const int Zoom = 24;

		// Token: 0x04000B86 RID: 2950
		public const int ViewCharacter = 25;

		// Token: 0x04000B87 RID: 2951
		public const int LockTarget = 26;

		// Token: 0x04000B88 RID: 2952
		public const int CameraToggle = 27;

		// Token: 0x04000B89 RID: 2953
		public const int Cheer = 31;

		// Token: 0x04000B8A RID: 2954
		public const int PushToTalk = 33;

		// Token: 0x04000B8B RID: 2955
		public const int EquipmentSwitch = 34;

		// Token: 0x04000B8C RID: 2956
		public const string DeploymentCameraIsActive = "DeploymentCameraIsActive";

		// Token: 0x04000B8D RID: 2957
		public const string ToggleZoom = "ToggleZoom";

		// Token: 0x04000B8E RID: 2958
		public const string ControllerEquipDropRRight = "ControllerEquipDropWeapon1";

		// Token: 0x04000B8F RID: 2959
		public const string ControllerEquipDropRUp = "ControllerEquipDropWeapon2";

		// Token: 0x04000B90 RID: 2960
		public const string ControllerEquipDropRLeft = "ControllerEquipDropWeapon3";

		// Token: 0x04000B91 RID: 2961
		public const string ControllerEquipDropRDown = "ControllerEquipDropWeapon4";

		// Token: 0x04000B92 RID: 2962
		public const string ControllerEquipDropRThumb = "ControllerEquipDropExtraWeapon";

		// Token: 0x04000B93 RID: 2963
		public const string CheerBarkSelectFirstCategory = "CheerBarkSelectFirstCategory";

		// Token: 0x04000B94 RID: 2964
		public const string CheerBarkSelectSecondCategory = "CheerBarkSelectSecondCategory";

		// Token: 0x04000B95 RID: 2965
		public const string CheerBarkCloseMenu = "CheerBarkCloseMenu";

		// Token: 0x04000B96 RID: 2966
		public const string CheerBarkItem1 = "CheerBarkItem1";

		// Token: 0x04000B97 RID: 2967
		public const string CheerBarkItem2 = "CheerBarkItem2";

		// Token: 0x04000B98 RID: 2968
		public const string CheerBarkItem3 = "CheerBarkItem3";

		// Token: 0x04000B99 RID: 2969
		public const string CheerBarkItem4 = "CheerBarkItem4";

		// Token: 0x04000B9A RID: 2970
		public const string ControlModeToggle = "ControlModeToggle";

		// Token: 0x04000B9B RID: 2971
		public const string ControllerToggleWalk = "ControllerToggleWalk";

		// Token: 0x04000B9C RID: 2972
		public const string ControllerToggleCrouch = "ControllerToggleCrouch";

		// Token: 0x04000B9D RID: 2973
		public const string ForfeitSpawn = "ForfeitSpawn";
	}
}
