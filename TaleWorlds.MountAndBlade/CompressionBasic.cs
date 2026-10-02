using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002EE RID: 750
	public static class CompressionBasic
	{
		// Token: 0x04001082 RID: 4226
		public const float MaxPossibleAbsValueForSecondMaxQuaternionComponent = 0.7071068f;

		// Token: 0x04001083 RID: 4227
		public const float MaxPositionZForCompression = 2521f;

		// Token: 0x04001084 RID: 4228
		public const float MaxPositionForCompression = 10385f;

		// Token: 0x04001085 RID: 4229
		public const float MinPositionForCompression = -100f;

		// Token: 0x04001086 RID: 4230
		public static CompressionInfo.Integer PingValueCompressionInfo = new CompressionInfo.Integer(0, 1023, true);

		// Token: 0x04001087 RID: 4231
		public static CompressionInfo.Integer LossValueCompressionInfo = new CompressionInfo.Integer(0, 100, true);

		// Token: 0x04001088 RID: 4232
		public static CompressionInfo.Integer ServerPerformanceStateCompressionInfo = new CompressionInfo.Integer(0, 2, true);

		// Token: 0x04001089 RID: 4233
		public static CompressionInfo.UnsignedInteger ColorCompressionInfo = new CompressionInfo.UnsignedInteger(0U, 32);

		// Token: 0x0400108A RID: 4234
		public static CompressionInfo.Integer ItemDataValueCompressionInfo = new CompressionInfo.Integer(0, 16);

		// Token: 0x0400108B RID: 4235
		public static CompressionInfo.Integer RandomSeedCompressionInfo = new CompressionInfo.Integer(0, 2000, true);

		// Token: 0x0400108C RID: 4236
		public static CompressionInfo.Float PositionCompressionInfo = new CompressionInfo.Float(-100f, 10385f, 22);

		// Token: 0x0400108D RID: 4237
		public static CompressionInfo.Float LocalPositionCompressionInfo = new CompressionInfo.Float(-32f, 32f, 16);

		// Token: 0x0400108E RID: 4238
		public static CompressionInfo.Float LowResLocalPositionCompressionInfo = new CompressionInfo.Float(-32f, 32f, 12);

		// Token: 0x0400108F RID: 4239
		public static CompressionInfo.Float BigRangeLowResLocalPositionCompressionInfo = new CompressionInfo.Float(-1000f, 1000f, 16);

		// Token: 0x04001090 RID: 4240
		public static CompressionInfo.Integer PlayerCompressionInfo = new CompressionInfo.Integer(-1, 1022, true);

		// Token: 0x04001091 RID: 4241
		public static CompressionInfo.UnsignedInteger PeerComponentCompressionInfo = new CompressionInfo.UnsignedInteger(0U, 32);

		// Token: 0x04001092 RID: 4242
		public static CompressionInfo.UnsignedInteger GUIDCompressionInfo = new CompressionInfo.UnsignedInteger(0U, 32);

		// Token: 0x04001093 RID: 4243
		public static CompressionInfo.Integer FlagsCompressionInfo = new CompressionInfo.Integer(0, 30);

		// Token: 0x04001094 RID: 4244
		public static CompressionInfo.Integer GUIDIntCompressionInfo = new CompressionInfo.Integer(-1, 31);

		// Token: 0x04001095 RID: 4245
		public static CompressionInfo.Integer MissionObjectIDCompressionInfo = new CompressionInfo.Integer(-1, 8190, true);

		// Token: 0x04001096 RID: 4246
		public static CompressionInfo.Float UnitVectorCompressionInfo = new CompressionInfo.Float(-1.024f, 10, 0.002f);

		// Token: 0x04001097 RID: 4247
		public static CompressionInfo.Float LowResRadianCompressionInfo = new CompressionInfo.Float(-3.1515927f, 3.1515927f, 8);

		// Token: 0x04001098 RID: 4248
		public static CompressionInfo.Float RadianCompressionInfo = new CompressionInfo.Float(-3.1515927f, 3.1515927f, 10);

		// Token: 0x04001099 RID: 4249
		public static CompressionInfo.Float HighResRadianCompressionInfo = new CompressionInfo.Float(-3.1515927f, 3.1515927f, 13);

		// Token: 0x0400109A RID: 4250
		public static CompressionInfo.Float ScaleCompressionInfo = new CompressionInfo.Float(-0.001f, 10, 0.01f);

		// Token: 0x0400109B RID: 4251
		public static CompressionInfo.Float LowResQuaternionCompressionInfo = new CompressionInfo.Float(-0.7071068f, 0.7071068f, 6);

		// Token: 0x0400109C RID: 4252
		public static CompressionInfo.Integer OmittedQuaternionComponentIndexCompressionInfo = new CompressionInfo.Integer(0, 3, true);

		// Token: 0x0400109D RID: 4253
		public static CompressionInfo.Float ImpulseCompressionInfo = new CompressionInfo.Float(-500f, 16, 0.0153f);

		// Token: 0x0400109E RID: 4254
		public static CompressionInfo.Integer AnimationKeyCompressionInfo = new CompressionInfo.Integer(0, 8000, true);

		// Token: 0x0400109F RID: 4255
		public static CompressionInfo.Float AnimationSpeedCompressionInfo = new CompressionInfo.Float(0f, 9, 0.01f);

		// Token: 0x040010A0 RID: 4256
		public static CompressionInfo.Float AnimationProgressCompressionInfo = new CompressionInfo.Float(0f, 1f, 9);

		// Token: 0x040010A1 RID: 4257
		public static CompressionInfo.Float VertexAnimationSpeedCompressionInfo = new CompressionInfo.Float(0f, 9, 0.1f);

		// Token: 0x040010A2 RID: 4258
		public static CompressionInfo.Integer PercentageCompressionInfo = new CompressionInfo.Integer(0, 100, true);

		// Token: 0x040010A3 RID: 4259
		public static CompressionInfo.Integer EntityChildCountCompressionInfo = new CompressionInfo.Integer(0, 8);

		// Token: 0x040010A4 RID: 4260
		public static CompressionInfo.Integer AgentHitDamageCompressionInfo = new CompressionInfo.Integer(0, 2000, true);

		// Token: 0x040010A5 RID: 4261
		public static CompressionInfo.Integer AgentHitModifiedDamageCompressionInfo = new CompressionInfo.Integer(-2000, 2000, true);

		// Token: 0x040010A6 RID: 4262
		public static CompressionInfo.Float AgentHitRelativeSpeedCompressionInfo = new CompressionInfo.Float(0f, 17, 0.01f);

		// Token: 0x040010A7 RID: 4263
		public static CompressionInfo.Integer AgentHitArmorCompressionInfo = new CompressionInfo.Integer(0, 200, true);

		// Token: 0x040010A8 RID: 4264
		public static CompressionInfo.Integer AgentHitBoneIndexCompressionInfo = new CompressionInfo.Integer(-1, 63, true);

		// Token: 0x040010A9 RID: 4265
		public static CompressionInfo.Integer AgentHitBodyPartCompressionInfo = new CompressionInfo.Integer(-1, 8, true);

		// Token: 0x040010AA RID: 4266
		public static CompressionInfo.Integer AgentHitDamageTypeCompressionInfo = new CompressionInfo.Integer(-1, 2, true);

		// Token: 0x040010AB RID: 4267
		public static CompressionInfo.Integer RoundGoldAmountCompressionInfo = new CompressionInfo.Integer(-1, 2000, true);

		// Token: 0x040010AC RID: 4268
		public static CompressionInfo.Integer DebugIntNonCompressionInfo = new CompressionInfo.Integer(int.MinValue, 32);

		// Token: 0x040010AD RID: 4269
		public static CompressionInfo.UnsignedLongInteger DebugULongNonCompressionInfo = new CompressionInfo.UnsignedLongInteger(0UL, 64);

		// Token: 0x040010AE RID: 4270
		public static CompressionInfo.Float AgentAgeCompressionInfo = new CompressionInfo.Float(0f, 128f, 10);

		// Token: 0x040010AF RID: 4271
		public static CompressionInfo.Float FaceKeyDataCompressionInfo = new CompressionInfo.Float(0f, 1f, 10);

		// Token: 0x040010B0 RID: 4272
		public static CompressionInfo.Integer PlayerChosenBadgeCompressionInfo = new CompressionInfo.Integer(-1, 8);

		// Token: 0x040010B1 RID: 4273
		public static CompressionInfo.Integer MaxNumberOfPlayersCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.MaxNumberOfPlayers.GetMinimumValue(), MultiplayerOptions.OptionType.MaxNumberOfPlayers.GetMaximumValue(), true);

		// Token: 0x040010B2 RID: 4274
		public static CompressionInfo.Integer MinNumberOfPlayersForMatchStartCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.MinNumberOfPlayersForMatchStart.GetMinimumValue(), MultiplayerOptions.OptionType.MinNumberOfPlayersForMatchStart.GetMaximumValue(), true);

		// Token: 0x040010B3 RID: 4275
		public static CompressionInfo.Integer MapTimeLimitCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.MapTimeLimit.GetMinimumValue(), MultiplayerOptions.OptionType.MapTimeLimit.GetMaximumValue(), true);

		// Token: 0x040010B4 RID: 4276
		public static CompressionInfo.Integer RoundTotalCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.RoundTotal.GetMinimumValue(), MultiplayerOptions.OptionType.RoundTotal.GetMaximumValue(), true);

		// Token: 0x040010B5 RID: 4277
		public static CompressionInfo.Integer RoundTimeLimitCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.RoundTimeLimit.GetMinimumValue(), MultiplayerOptions.OptionType.RoundTimeLimit.GetMaximumValue(), true);

		// Token: 0x040010B6 RID: 4278
		public static CompressionInfo.Integer WarmupTimeLimitCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.GetMinimumValue(), MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.GetMaximumValue(), true);

		// Token: 0x040010B7 RID: 4279
		public static CompressionInfo.Integer RoundPreparationTimeLimitCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.RoundPreparationTimeLimit.GetMinimumValue(), MultiplayerOptions.OptionType.RoundPreparationTimeLimit.GetMaximumValue(), true);

		// Token: 0x040010B8 RID: 4280
		public static CompressionInfo.Integer RespawnPeriodCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.RespawnPeriodTeam1.GetMinimumValue(), MultiplayerOptions.OptionType.RespawnPeriodTeam1.GetMaximumValue(), true);

		// Token: 0x040010B9 RID: 4281
		public static CompressionInfo.Integer GoldGainChangePercentageCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.GetMinimumValue(), MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.GetMaximumValue(), true);

		// Token: 0x040010BA RID: 4282
		public static CompressionInfo.Integer SpectatorCameraTypeCompressionInfo = new CompressionInfo.Integer(-1, 8, true);

		// Token: 0x040010BB RID: 4283
		public static CompressionInfo.Integer PollAcceptThresholdCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.PollAcceptThreshold.GetMinimumValue(), MultiplayerOptions.OptionType.PollAcceptThreshold.GetMaximumValue(), true);

		// Token: 0x040010BC RID: 4284
		public static CompressionInfo.Integer NumberOfBotsTeamCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetMinimumValue(), MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetMaximumValue(), true);

		// Token: 0x040010BD RID: 4285
		public static CompressionInfo.Integer NumberOfBotsPerFormationCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetMinimumValue(), MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetMaximumValue(), true);

		// Token: 0x040010BE RID: 4286
		public static CompressionInfo.Integer AutoTeamBalanceLimitCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetMinimumValue(), MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetMaximumValue(), true);

		// Token: 0x040010BF RID: 4287
		public static CompressionInfo.Integer FriendlyFireDamageCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.GetMinimumValue(), MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.GetMaximumValue(), true);

		// Token: 0x040010C0 RID: 4288
		public static CompressionInfo.Integer ForcedAvatarIndexCompressionInfo = new CompressionInfo.Integer(-1, 99, true);

		// Token: 0x040010C1 RID: 4289
		public static CompressionInfo.Integer IntermissionStateCompressionInfo = new CompressionInfo.Integer(0, Enum.GetNames(typeof(MultiplayerIntermissionState)).Length - 1, false);

		// Token: 0x040010C2 RID: 4290
		public static CompressionInfo.Float IntermissionTimerCompressionInfo = new CompressionInfo.Float(0f, 240f, 14);

		// Token: 0x040010C3 RID: 4291
		public static CompressionInfo.Integer IntermissionMapVoteItemCountCompressionInfo = new CompressionInfo.Integer(0, 99, true);

		// Token: 0x040010C4 RID: 4292
		public static CompressionInfo.Integer IntermissionVoterCountCompressionInfo = new CompressionInfo.Integer(0, 1022, true);

		// Token: 0x040010C5 RID: 4293
		public static CompressionInfo.Integer ActionCodeCompressionInfo;

		// Token: 0x040010C6 RID: 4294
		public static CompressionInfo.Integer AnimationIndexCompressionInfo;

		// Token: 0x040010C7 RID: 4295
		public static CompressionInfo.Integer CultureIndexCompressionInfo;

		// Token: 0x040010C8 RID: 4296
		public static CompressionInfo.Integer SoundEventsCompressionInfo;

		// Token: 0x040010C9 RID: 4297
		public static CompressionInfo.Integer NetworkComponentEventTypeFromServerCompressionInfo;

		// Token: 0x040010CA RID: 4298
		public static CompressionInfo.Integer NetworkComponentEventTypeFromClientCompressionInfo;

		// Token: 0x040010CB RID: 4299
		public static CompressionInfo.Integer TroopTypeCompressionInfo = new CompressionInfo.Integer(-1, 2, true);

		// Token: 0x040010CC RID: 4300
		public static CompressionInfo.Integer BannerDataCountCompressionInfo = new CompressionInfo.Integer(0, 31, true);

		// Token: 0x040010CD RID: 4301
		public static CompressionInfo.Integer BannerDataMeshIdCompressionInfo = new CompressionInfo.Integer(0, 13);

		// Token: 0x040010CE RID: 4302
		public static CompressionInfo.Integer BannerDataColorIndexCompressionInfo = new CompressionInfo.Integer(0, 10);

		// Token: 0x040010CF RID: 4303
		public static CompressionInfo.Integer BannerDataSizeCompressionInfo = new CompressionInfo.Integer(-8000, 8000, true);

		// Token: 0x040010D0 RID: 4304
		public static CompressionInfo.Integer BannerDataRotationCompressionInfo = new CompressionInfo.Integer(0, 360, true);
	}
}
