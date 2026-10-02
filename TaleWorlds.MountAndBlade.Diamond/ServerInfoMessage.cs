using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200015A RID: 346
	public enum ServerInfoMessage
	{
		// Token: 0x04000442 RID: 1090
		Success,
		// Token: 0x04000443 RID: 1091
		LoginMuted,
		// Token: 0x04000444 RID: 1092
		DestroySessionPremadeGameCancellation,
		// Token: 0x04000445 RID: 1093
		DestroySessionPartyInvitationCancellation,
		// Token: 0x04000446 RID: 1094
		DestroySessionPartyAutoDisband,
		// Token: 0x04000447 RID: 1095
		PlayerNotFound,
		// Token: 0x04000448 RID: 1096
		PlayerNotInLobby,
		// Token: 0x04000449 RID: 1097
		MustBeInLobby,
		// Token: 0x0400044A RID: 1098
		NoTextGiven,
		// Token: 0x0400044B RID: 1099
		TextTooLong,
		// Token: 0x0400044C RID: 1100
		FindGameBlockedFromMatchmaking,
		// Token: 0x0400044D RID: 1101
		FindGamePartyMemberBlockedFromMatchmaking,
		// Token: 0x0400044E RID: 1102
		FindGameNoGameTypeSelected,
		// Token: 0x0400044F RID: 1103
		FindGameDisabledGameTypesSelected,
		// Token: 0x04000450 RID: 1104
		FindGamePlayerCountNotAllowed,
		// Token: 0x04000451 RID: 1105
		FindGameNotPartyLeader,
		// Token: 0x04000452 RID: 1106
		FindGameNotAllPlayersReady,
		// Token: 0x04000453 RID: 1107
		FindGameRegionNotAvailable,
		// Token: 0x04000454 RID: 1108
		FindGamePunished,
		// Token: 0x04000455 RID: 1109
		RejoinGame,
		// Token: 0x04000456 RID: 1110
		RejoinGameNotFound,
		// Token: 0x04000457 RID: 1111
		RejoinGameNotAllowed,
		// Token: 0x04000458 RID: 1112
		AddFriendCantAddSelf,
		// Token: 0x04000459 RID: 1113
		AddFriendRequestSent,
		// Token: 0x0400045A RID: 1114
		AddFriendRequestReceived,
		// Token: 0x0400045B RID: 1115
		AddFriendAlreadyFriends,
		// Token: 0x0400045C RID: 1116
		AddFriendRequestPending,
		// Token: 0x0400045D RID: 1117
		AddFriendRequestAccepted,
		// Token: 0x0400045E RID: 1118
		AddFriendRequestDeclined,
		// Token: 0x0400045F RID: 1119
		AddFriendRequestBlocked,
		// Token: 0x04000460 RID: 1120
		RemoveFriendSuccess,
		// Token: 0x04000461 RID: 1121
		FriendRequestAccepted,
		// Token: 0x04000462 RID: 1122
		FriendRequestDeclined,
		// Token: 0x04000463 RID: 1123
		FriendRequestNotFound,
		// Token: 0x04000464 RID: 1124
		MustBeInParty,
		// Token: 0x04000465 RID: 1125
		MustBePartyLeader,
		// Token: 0x04000466 RID: 1126
		InvitePartyHasModules,
		// Token: 0x04000467 RID: 1127
		InvitePartyOtherPlayerHasModules,
		// Token: 0x04000468 RID: 1128
		InvitePartyCantInviteSelf,
		// Token: 0x04000469 RID: 1129
		InvitePartyOtherPlayerAlreadyInParty,
		// Token: 0x0400046A RID: 1130
		InvitePartyPartyIsFull,
		// Token: 0x0400046B RID: 1131
		InvitePartyOnlyLeaderCanInvite,
		// Token: 0x0400046C RID: 1132
		InvitePartySuccess,
		// Token: 0x0400046D RID: 1133
		SuggestPartyMustBeInParty,
		// Token: 0x0400046E RID: 1134
		SuggestPartyMustBeMember,
		// Token: 0x0400046F RID: 1135
		SuggestPartyCantSuggestSelf,
		// Token: 0x04000470 RID: 1136
		SuggestPartyOtherPlayerAlreadyInParty,
		// Token: 0x04000471 RID: 1137
		SuggestPartySuccess,
		// Token: 0x04000472 RID: 1138
		DisbandPartySuccess,
		// Token: 0x04000473 RID: 1139
		KickPlayerOtherPlayerMustBeInParty,
		// Token: 0x04000474 RID: 1140
		KickPartyPlayerMustBeLeader,
		// Token: 0x04000475 RID: 1141
		PromotePartyLeaderOngoingClanCreation,
		// Token: 0x04000476 RID: 1142
		PromotePartyLeaderCantPromoteSelf,
		// Token: 0x04000477 RID: 1143
		PromotePartyLeaderCantPromoteNonMember,
		// Token: 0x04000478 RID: 1144
		PromotePartyLeaderMustBeLeader,
		// Token: 0x04000479 RID: 1145
		PromotePartyLeaderSuccess,
		// Token: 0x0400047A RID: 1146
		PromotePartyLeaderAuto,
		// Token: 0x0400047B RID: 1147
		MustBeInClan,
		// Token: 0x0400047C RID: 1148
		MustBeClanLeader,
		// Token: 0x0400047D RID: 1149
		MustBePrivilegedClanMember,
		// Token: 0x0400047E RID: 1150
		ClanCreationNameIsInvalid,
		// Token: 0x0400047F RID: 1151
		ClanCreationTagIsInvalid,
		// Token: 0x04000480 RID: 1152
		ClanCreationSigilIsInvalid,
		// Token: 0x04000481 RID: 1153
		ClanCreationCultureIsInvalid,
		// Token: 0x04000482 RID: 1154
		ClanCreationNotAllPlayersReady,
		// Token: 0x04000483 RID: 1155
		ClanCreationNotEnoughPlayers,
		// Token: 0x04000484 RID: 1156
		ClanCreationAlreadyInAClan,
		// Token: 0x04000485 RID: 1157
		ClanCreationHaveToBeInAParty,
		// Token: 0x04000486 RID: 1158
		SetClanInformationSuccess,
		// Token: 0x04000487 RID: 1159
		AddClanAnnouncementSuccess,
		// Token: 0x04000488 RID: 1160
		EditClanAnnouncementNotFound,
		// Token: 0x04000489 RID: 1161
		EditClanAnnouncementSuccess,
		// Token: 0x0400048A RID: 1162
		DeleteClanAnnouncementNotFound,
		// Token: 0x0400048B RID: 1163
		DeleteClanAnnouncementSuccess,
		// Token: 0x0400048C RID: 1164
		ChangeClanSigilInvalid,
		// Token: 0x0400048D RID: 1165
		ChangeClanSigilSuccess,
		// Token: 0x0400048E RID: 1166
		ChangeClanCultureSuccess,
		// Token: 0x0400048F RID: 1167
		InviteClanPlayerAlreadyInvited,
		// Token: 0x04000490 RID: 1168
		InviteClanPlayerAlreadyInClan,
		// Token: 0x04000491 RID: 1169
		InviteClanPlayerIsNotOnline,
		// Token: 0x04000492 RID: 1170
		InviteClanPlayerFeatureNotSupported,
		// Token: 0x04000493 RID: 1171
		InviteClanCantInviteSelf,
		// Token: 0x04000494 RID: 1172
		InviteClanSuccess,
		// Token: 0x04000495 RID: 1173
		AcceptClanInvitationSuccess,
		// Token: 0x04000496 RID: 1174
		DeclineClanInvitationSuccess,
		// Token: 0x04000497 RID: 1175
		PromoteClanRolePlayerNotInClan,
		// Token: 0x04000498 RID: 1176
		PromoteClanLeaderCantPromoteSelf,
		// Token: 0x04000499 RID: 1177
		PromoteClanLeaderSuccess,
		// Token: 0x0400049A RID: 1178
		PromoteClanOfficerRoleLimitReached,
		// Token: 0x0400049B RID: 1179
		PromoteClanOfficerCantPromoteSelf,
		// Token: 0x0400049C RID: 1180
		PromoteClanOfficerSuccess,
		// Token: 0x0400049D RID: 1181
		RemoveClanOfficerMustBeOfficerToMember,
		// Token: 0x0400049E RID: 1182
		RemoveClanOfficerMustBeOfficerToLeader,
		// Token: 0x0400049F RID: 1183
		RemoveClanOfficerSuccessFromLeader,
		// Token: 0x040004A0 RID: 1184
		RemoveClanOfficerSuccessFromMember,
		// Token: 0x040004A1 RID: 1185
		RemoveClanMemberToMember,
		// Token: 0x040004A2 RID: 1186
		RemoveClanMemberToLeader,
		// Token: 0x040004A3 RID: 1187
		RemoveClanMemberLeaderCantLeave,
		// Token: 0x040004A4 RID: 1188
		PremadeGameCreationCanceled,
		// Token: 0x040004A5 RID: 1189
		PremadeGameCreationMustBeCreating,
		// Token: 0x040004A6 RID: 1190
		PremadeGameCreationMapNotAvailable,
		// Token: 0x040004A7 RID: 1191
		PremadeGameCreationPartyNotEligible,
		// Token: 0x040004A8 RID: 1192
		PremadeGameCreationInvalidGameType,
		// Token: 0x040004A9 RID: 1193
		PremadeGameJoinIncorrectPassword,
		// Token: 0x040004AA RID: 1194
		PremadeGameJoinGameNotFound,
		// Token: 0x040004AB RID: 1195
		PremadeGameJoinPartyNotEligible,
		// Token: 0x040004AC RID: 1196
		GetPremadeGameListNotEligible,
		// Token: 0x040004AD RID: 1197
		ReportPlayerGameNotFound,
		// Token: 0x040004AE RID: 1198
		ReportPlayerPlayerNotFound,
		// Token: 0x040004AF RID: 1199
		ReportPlayerServerIsUnofficial,
		// Token: 0x040004B0 RID: 1200
		ReportPlayerSuccess,
		// Token: 0x040004B1 RID: 1201
		ChangeBannerlordIDFailure,
		// Token: 0x040004B2 RID: 1202
		ChangeBannerlordIDSuccess,
		// Token: 0x040004B3 RID: 1203
		ChangeBannerlordIDEmpty,
		// Token: 0x040004B4 RID: 1204
		ChangeBannerlordIDTooShort,
		// Token: 0x040004B5 RID: 1205
		ChangeBannerlordIDTooLong,
		// Token: 0x040004B6 RID: 1206
		ChangeBannerlordIDInvalidCharacters,
		// Token: 0x040004B7 RID: 1207
		ChangeBannerlordIDProfanity,
		// Token: 0x040004B8 RID: 1208
		GameInvitationCantInviteSelf,
		// Token: 0x040004B9 RID: 1209
		GameInvitationPlayerAlreadyInGame,
		// Token: 0x040004BA RID: 1210
		GameInvitationSuccess,
		// Token: 0x040004BB RID: 1211
		ChangeRegionFailed,
		// Token: 0x040004BC RID: 1212
		ChangeGameModeFailed,
		// Token: 0x040004BD RID: 1213
		BattleServerKickFriendlyFire,
		// Token: 0x040004BE RID: 1214
		ChatServerDisconnectedFromRoom,
		// Token: 0x040004BF RID: 1215
		CustomizationServiceIsUnavailable,
		// Token: 0x040004C0 RID: 1216
		CustomizationNotEnoughLoot,
		// Token: 0x040004C1 RID: 1217
		CustomizationItemIsUnavailable,
		// Token: 0x040004C2 RID: 1218
		CustomizationItemIsFree,
		// Token: 0x040004C3 RID: 1219
		CustomizationItemAlreadyOwned,
		// Token: 0x040004C4 RID: 1220
		CustomizationItemIsNotOwned,
		// Token: 0x040004C5 RID: 1221
		CustomizationChangeSigilSuccess,
		// Token: 0x040004C6 RID: 1222
		CustomizationTroopIsNotValid,
		// Token: 0x040004C7 RID: 1223
		CustomizationCantUseMoreThanOneForSingleSlot,
		// Token: 0x040004C8 RID: 1224
		CustomizationCantUpdateBadge,
		// Token: 0x040004C9 RID: 1225
		CustomizationInvalidBadge,
		// Token: 0x040004CA RID: 1226
		CustomizationCantDowngradeBadge,
		// Token: 0x040004CB RID: 1227
		CustomizationBadgeNotAvailable,
		// Token: 0x040004CC RID: 1228
		PremadeGameJoinIncorrectStateForSpectator,
		// Token: 0x040004CD RID: 1229
		PremadeGameJoinSpectatorCapacityIsFull
	}
}
