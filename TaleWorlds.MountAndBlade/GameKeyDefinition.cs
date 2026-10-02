using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000230 RID: 560
	public enum GameKeyDefinition
	{
		// Token: 0x04000BB7 RID: 2999
		Up,
		// Token: 0x04000BB8 RID: 3000
		Down,
		// Token: 0x04000BB9 RID: 3001
		Left,
		// Token: 0x04000BBA RID: 3002
		Right,
		// Token: 0x04000BBB RID: 3003
		Leave,
		// Token: 0x04000BBC RID: 3004
		ShowIndicators,
		// Token: 0x04000BBD RID: 3005
		InitiateAllChat,
		// Token: 0x04000BBE RID: 3006
		InitiateTeamChat,
		// Token: 0x04000BBF RID: 3007
		FinalizeChat,
		// Token: 0x04000BC0 RID: 3008
		Attack,
		// Token: 0x04000BC1 RID: 3009
		Defend,
		// Token: 0x04000BC2 RID: 3010
		EquipPrimaryWeapon,
		// Token: 0x04000BC3 RID: 3011
		EquipSecondaryWeapon,
		// Token: 0x04000BC4 RID: 3012
		Action,
		// Token: 0x04000BC5 RID: 3013
		Jump,
		// Token: 0x04000BC6 RID: 3014
		Crouch,
		// Token: 0x04000BC7 RID: 3015
		Kick,
		// Token: 0x04000BC8 RID: 3016
		ToggleWeaponMode,
		// Token: 0x04000BC9 RID: 3017
		EquipWeapon1,
		// Token: 0x04000BCA RID: 3018
		EquipWeapon2,
		// Token: 0x04000BCB RID: 3019
		EquipWeapon3,
		// Token: 0x04000BCC RID: 3020
		EquipWeapon4,
		// Token: 0x04000BCD RID: 3021
		DropWeapon,
		// Token: 0x04000BCE RID: 3022
		SheathWeapon,
		// Token: 0x04000BCF RID: 3023
		Zoom,
		// Token: 0x04000BD0 RID: 3024
		ViewCharacter,
		// Token: 0x04000BD1 RID: 3025
		LockTarget,
		// Token: 0x04000BD2 RID: 3026
		CameraToggle,
		// Token: 0x04000BD3 RID: 3027
		MissionScreenHotkeyCameraZoomIn,
		// Token: 0x04000BD4 RID: 3028
		MissionScreenHotkeyCameraZoomOut,
		// Token: 0x04000BD5 RID: 3029
		ToggleWalkMode,
		// Token: 0x04000BD6 RID: 3030
		Cheer,
		// Token: 0x04000BD7 RID: 3031
		Taunt,
		// Token: 0x04000BD8 RID: 3032
		PushToTalk,
		// Token: 0x04000BD9 RID: 3033
		EquipmentSwitch,
		// Token: 0x04000BDA RID: 3034
		ShowMouse,
		// Token: 0x04000BDB RID: 3035
		BannerWindow,
		// Token: 0x04000BDC RID: 3036
		CharacterWindow,
		// Token: 0x04000BDD RID: 3037
		InventoryWindow,
		// Token: 0x04000BDE RID: 3038
		EncyclopediaWindow,
		// Token: 0x04000BDF RID: 3039
		KingdomWindow,
		// Token: 0x04000BE0 RID: 3040
		ClanWindow,
		// Token: 0x04000BE1 RID: 3041
		QuestsWindow,
		// Token: 0x04000BE2 RID: 3042
		PartyWindow,
		// Token: 0x04000BE3 RID: 3043
		FacegenWindow,
		// Token: 0x04000BE4 RID: 3044
		ManageFleetWindow,
		// Token: 0x04000BE5 RID: 3045
		MapMoveUp,
		// Token: 0x04000BE6 RID: 3046
		MapMoveDown,
		// Token: 0x04000BE7 RID: 3047
		MapMoveLeft,
		// Token: 0x04000BE8 RID: 3048
		MapMoveRight,
		// Token: 0x04000BE9 RID: 3049
		PartyMoveUp,
		// Token: 0x04000BEA RID: 3050
		PartyMoveDown,
		// Token: 0x04000BEB RID: 3051
		PartyMoveLeft,
		// Token: 0x04000BEC RID: 3052
		PartyMoveRight,
		// Token: 0x04000BED RID: 3053
		QuickSave,
		// Token: 0x04000BEE RID: 3054
		MapFastMove,
		// Token: 0x04000BEF RID: 3055
		MapZoomIn,
		// Token: 0x04000BF0 RID: 3056
		MapZoomOut,
		// Token: 0x04000BF1 RID: 3057
		MapRotateLeft,
		// Token: 0x04000BF2 RID: 3058
		MapRotateRight,
		// Token: 0x04000BF3 RID: 3059
		MapTimeStop,
		// Token: 0x04000BF4 RID: 3060
		MapTimeNormal,
		// Token: 0x04000BF5 RID: 3061
		MapTimeFastForward,
		// Token: 0x04000BF6 RID: 3062
		MapTimeTogglePause,
		// Token: 0x04000BF7 RID: 3063
		MapCameraFollowMode,
		// Token: 0x04000BF8 RID: 3064
		MapToggleFastForward,
		// Token: 0x04000BF9 RID: 3065
		MapTrackSettlement,
		// Token: 0x04000BFA RID: 3066
		MapGoToEncylopedia,
		// Token: 0x04000BFB RID: 3067
		ViewOrders,
		// Token: 0x04000BFC RID: 3068
		SelectOrder1,
		// Token: 0x04000BFD RID: 3069
		SelectOrder2,
		// Token: 0x04000BFE RID: 3070
		SelectOrder3,
		// Token: 0x04000BFF RID: 3071
		SelectOrder4,
		// Token: 0x04000C00 RID: 3072
		SelectOrder5,
		// Token: 0x04000C01 RID: 3073
		SelectOrder6,
		// Token: 0x04000C02 RID: 3074
		SelectOrder7,
		// Token: 0x04000C03 RID: 3075
		SelectOrder8,
		// Token: 0x04000C04 RID: 3076
		SelectOrderReturn,
		// Token: 0x04000C05 RID: 3077
		EveryoneHear,
		// Token: 0x04000C06 RID: 3078
		Group0Hear,
		// Token: 0x04000C07 RID: 3079
		Group1Hear,
		// Token: 0x04000C08 RID: 3080
		Group2Hear,
		// Token: 0x04000C09 RID: 3081
		Group3Hear,
		// Token: 0x04000C0A RID: 3082
		Group4Hear,
		// Token: 0x04000C0B RID: 3083
		Group5Hear,
		// Token: 0x04000C0C RID: 3084
		Group6Hear,
		// Token: 0x04000C0D RID: 3085
		Group7Hear,
		// Token: 0x04000C0E RID: 3086
		HoldOrder,
		// Token: 0x04000C0F RID: 3087
		SelectLeftFormation,
		// Token: 0x04000C10 RID: 3088
		SelectRightFormation,
		// Token: 0x04000C11 RID: 3089
		ApplySelection,
		// Token: 0x04000C12 RID: 3090
		ToggleSelection,
		// Token: 0x04000C13 RID: 3091
		HideUI,
		// Token: 0x04000C14 RID: 3092
		CameraRollLeft,
		// Token: 0x04000C15 RID: 3093
		CameraRollRight,
		// Token: 0x04000C16 RID: 3094
		TakePicture,
		// Token: 0x04000C17 RID: 3095
		TakePictureWithAdditionalPasses,
		// Token: 0x04000C18 RID: 3096
		ToggleCameraFollowMode,
		// Token: 0x04000C19 RID: 3097
		ToggleMouse,
		// Token: 0x04000C1A RID: 3098
		ToggleVignette,
		// Token: 0x04000C1B RID: 3099
		ToggleCharacters,
		// Token: 0x04000C1C RID: 3100
		IncreaseFocus,
		// Token: 0x04000C1D RID: 3101
		DecreaseFocus,
		// Token: 0x04000C1E RID: 3102
		IncreaseFocusStart,
		// Token: 0x04000C1F RID: 3103
		DecreaseFocusStart,
		// Token: 0x04000C20 RID: 3104
		IncreaseFocusEnd,
		// Token: 0x04000C21 RID: 3105
		DecreaseFocusEnd,
		// Token: 0x04000C22 RID: 3106
		Reset,
		// Token: 0x04000C23 RID: 3107
		AcceptPoll,
		// Token: 0x04000C24 RID: 3108
		DeclinePoll,
		// Token: 0x04000C25 RID: 3109
		ToggleSail,
		// Token: 0x04000C26 RID: 3110
		ToggleOarsmen,
		// Token: 0x04000C27 RID: 3111
		ChangeShipCamera,
		// Token: 0x04000C28 RID: 3112
		SelectShip,
		// Token: 0x04000C29 RID: 3113
		AttemptBoarding,
		// Token: 0x04000C2A RID: 3114
		ToggleRangedWeaponOrderMode,
		// Token: 0x04000C2B RID: 3115
		TotalGameKeyCount
	}
}
