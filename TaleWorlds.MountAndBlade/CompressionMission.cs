using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000307 RID: 775
	public static class CompressionMission
	{
		// Token: 0x04001133 RID: 4403
		public static CompressionInfo.Float DebugScaleValueCompressionInfo = new CompressionInfo.Float(0.5f, 1.5f, 13);

		// Token: 0x04001134 RID: 4404
		public static CompressionInfo.Integer AgentCompressionInfo = new CompressionInfo.Integer(-1, 11);

		// Token: 0x04001135 RID: 4405
		public static CompressionInfo.Integer WeaponAttachmentIndexCompressionInfo = new CompressionInfo.Integer(0, 8);

		// Token: 0x04001136 RID: 4406
		public static CompressionInfo.Integer AgentOffsetCompressionInfo = new CompressionInfo.Integer(0, 8);

		// Token: 0x04001137 RID: 4407
		public static CompressionInfo.Integer AgentHealthCompressionInfo = new CompressionInfo.Integer(-1, 11);

		// Token: 0x04001138 RID: 4408
		public static CompressionInfo.Integer AgentControllerCompressionInfo = new CompressionInfo.Integer(0, 2, true);

		// Token: 0x04001139 RID: 4409
		public static CompressionInfo.Integer TeamCompressionInfo = new CompressionInfo.Integer(-1, 10);

		// Token: 0x0400113A RID: 4410
		public static CompressionInfo.Integer TeamSideCompressionInfo = new CompressionInfo.Integer(-1, 4);

		// Token: 0x0400113B RID: 4411
		public static CompressionInfo.Integer RoundEndReasonCompressionInfo = new CompressionInfo.Integer(-1, 2, true);

		// Token: 0x0400113C RID: 4412
		public static CompressionInfo.Integer TeamScoreCompressionInfo = new CompressionInfo.Integer(-1023000, 1023000, true);

		// Token: 0x0400113D RID: 4413
		public static CompressionInfo.Integer FactionCompressionInfo = new CompressionInfo.Integer(0, 4);

		// Token: 0x0400113E RID: 4414
		public static CompressionInfo.Integer MissionOrderTypeCompressionInfo = new CompressionInfo.Integer(-1, 5);

		// Token: 0x0400113F RID: 4415
		public static CompressionInfo.Integer MissionRoundCountCompressionInfo = new CompressionInfo.Integer(-1, 7);

		// Token: 0x04001140 RID: 4416
		public static CompressionInfo.Integer MissionRoundStateCompressionInfo = new CompressionInfo.Integer(-1, 5, true);

		// Token: 0x04001141 RID: 4417
		public static CompressionInfo.Integer RoundTimeCompressionInfo = new CompressionInfo.Integer(0, MultiplayerOptions.OptionType.RoundTimeLimit.GetMaximumValue(), true);

		// Token: 0x04001142 RID: 4418
		public static CompressionInfo.Integer SelectedTroopIndexCompressionInfo = new CompressionInfo.Integer(-1, 15, true);

		// Token: 0x04001143 RID: 4419
		public static CompressionInfo.Integer MissileCompressionInfo = new CompressionInfo.Integer(0, 10);

		// Token: 0x04001144 RID: 4420
		public static CompressionInfo.Float MissileSpeedCompressionInfo = new CompressionInfo.Float(0f, 12, 0.05f);

		// Token: 0x04001145 RID: 4421
		public static CompressionInfo.Integer MissileCollisionReactionCompressionInfo = new CompressionInfo.Integer(0, 3, true);

		// Token: 0x04001146 RID: 4422
		public static CompressionInfo.Integer FlagCapturePointIndexCompressionInfo = new CompressionInfo.Integer(0, 3);

		// Token: 0x04001147 RID: 4423
		public static CompressionInfo.Integer FlagpoleIndexCompressionInfo = new CompressionInfo.Integer(0, 5, true);

		// Token: 0x04001148 RID: 4424
		public static CompressionInfo.Float FlagCapturePointDurationCompressionInfo = new CompressionInfo.Float(-1f, 14, 0.01f);

		// Token: 0x04001149 RID: 4425
		public static CompressionInfo.Float FlagProgressCompressionInfo = new CompressionInfo.Float(-1f, 1f, 12);

		// Token: 0x0400114A RID: 4426
		public static CompressionInfo.Float FlagClassicProgressCompressionInfo = new CompressionInfo.Float(0f, 1f, 11);

		// Token: 0x0400114B RID: 4427
		public static CompressionInfo.Integer FlagDirectionEnumCompressionInfo = new CompressionInfo.Integer(-1, 2, true);

		// Token: 0x0400114C RID: 4428
		public static CompressionInfo.Float FlagSpeedCompressionInfo = new CompressionInfo.Float(-1f, 14, 0.01f);

		// Token: 0x0400114D RID: 4429
		public static CompressionInfo.Integer FlagCaptureResultCompressionInfo = new CompressionInfo.Integer(0, 3, true);

		// Token: 0x0400114E RID: 4430
		public static CompressionInfo.Integer UsableGameObjectDestructionStateCompressionInfo = new CompressionInfo.Integer(0, 3);

		// Token: 0x0400114F RID: 4431
		public static CompressionInfo.Float UsableGameObjectHealthCompressionInfo = new CompressionInfo.Float(-1f, 18, 0.1f);

		// Token: 0x04001150 RID: 4432
		public static CompressionInfo.Float UsableGameObjectBlowMagnitude = new CompressionInfo.Float(0f, DestructableComponent.MaxBlowMagnitude, 8);

		// Token: 0x04001151 RID: 4433
		public static CompressionInfo.Float UsableGameObjectBlowDirection = new CompressionInfo.Float(-1f, 1f, 7);

		// Token: 0x04001152 RID: 4434
		public static CompressionInfo.Float CapturePointProgressCompressionInfo = new CompressionInfo.Float(0f, 1f, 10);

		// Token: 0x04001153 RID: 4435
		public static CompressionInfo.Integer ItemSlotCompressionInfo = new CompressionInfo.Integer(0, 4, true);

		// Token: 0x04001154 RID: 4436
		public static CompressionInfo.Integer WieldSlotCompressionInfo = new CompressionInfo.Integer(-1, 4, true);

		// Token: 0x04001155 RID: 4437
		public static CompressionInfo.Integer ItemDataCompressionInfo = new CompressionInfo.Integer(0, 10);

		// Token: 0x04001156 RID: 4438
		public static CompressionInfo.Integer WeaponReloadPhaseCompressionInfo = new CompressionInfo.Integer(0, 9, true);

		// Token: 0x04001157 RID: 4439
		public static CompressionInfo.Integer WeaponUsageIndexCompressionInfo = new CompressionInfo.Integer(0, 2);

		// Token: 0x04001158 RID: 4440
		public static CompressionInfo.Integer WeaponClassCompressionInfo = new CompressionInfo.Integer(0, 30, true);

		// Token: 0x04001159 RID: 4441
		public static CompressionInfo.Integer TauntIndexCompressionInfo = new CompressionInfo.Integer(0, TauntUsageManager.Instance.GetTauntItemCount() - 1, true);

		// Token: 0x0400115A RID: 4442
		public static CompressionInfo.Integer BarkIndexCompressionInfo = new CompressionInfo.Integer(0, SkinVoiceManager.VoiceType.MpBarks.Length - 1, true);

		// Token: 0x0400115B RID: 4443
		public static CompressionInfo.Integer UsageDirectionCompressionInfo = new CompressionInfo.Integer(-1, 9, true);

		// Token: 0x0400115C RID: 4444
		public static CompressionInfo.Float SpawnedItemVelocityCompressionInfo = new CompressionInfo.Float(-50f, 50f, 12);

		// Token: 0x0400115D RID: 4445
		public static CompressionInfo.Float SpawnedItemAngularVelocityCompressionInfo = new CompressionInfo.Float(-10f, 10f, 12);

		// Token: 0x0400115E RID: 4446
		public static CompressionInfo.UnsignedInteger SpawnedItemWeaponSpawnFlagCompressionInfo = new CompressionInfo.UnsignedInteger(0U, EnumHelper.GetCombinedUIntEnumFlagsValue(typeof(Mission.WeaponSpawnFlags)), true);

		// Token: 0x0400115F RID: 4447
		public static CompressionInfo.Integer RangedSiegeWeaponAmmoCompressionInfo = new CompressionInfo.Integer(0, 7);

		// Token: 0x04001160 RID: 4448
		public static CompressionInfo.Integer RangedSiegeWeaponAmmoIndexCompressionInfo = new CompressionInfo.Integer(0, 3);

		// Token: 0x04001161 RID: 4449
		public static CompressionInfo.Integer RangedSiegeWeaponStateCompressionInfo = new CompressionInfo.Integer(0, 8, true);

		// Token: 0x04001162 RID: 4450
		public static CompressionInfo.Integer SiegeLadderStateCompressionInfo = new CompressionInfo.Integer(0, 9, true);

		// Token: 0x04001163 RID: 4451
		public static CompressionInfo.Integer BatteringRamStateCompressionInfo = new CompressionInfo.Integer(0, 2, true);

		// Token: 0x04001164 RID: 4452
		public static CompressionInfo.Integer SiegeLadderAnimationStateCompressionInfo = new CompressionInfo.Integer(0, 2, true);

		// Token: 0x04001165 RID: 4453
		public static CompressionInfo.Float SiegeMachineComponentAngularSpeedCompressionInfo = new CompressionInfo.Float(-20f, 20f, 12);

		// Token: 0x04001166 RID: 4454
		public static CompressionInfo.Integer SiegeTowerGateStateCompressionInfo = new CompressionInfo.Integer(0, 3, true);

		// Token: 0x04001167 RID: 4455
		public static CompressionInfo.Integer NumberOfPacesCompressionInfo = new CompressionInfo.Integer(0, 3);

		// Token: 0x04001168 RID: 4456
		public static CompressionInfo.Float WalkingSpeedLimitCompressionInfo = new CompressionInfo.Float(-0.01f, 9, 0.01f);

		// Token: 0x04001169 RID: 4457
		public static CompressionInfo.Float StepSizeCompressionInfo = new CompressionInfo.Float(-0.01f, 7, 0.01f);

		// Token: 0x0400116A RID: 4458
		public static CompressionInfo.Integer BoneIndexCompressionInfo = new CompressionInfo.Integer(0, 63, true);

		// Token: 0x0400116B RID: 4459
		public static CompressionInfo.Integer AgentPrefabComponentIndexCompressionInfo = new CompressionInfo.Integer(0, 4);

		// Token: 0x0400116C RID: 4460
		public static CompressionInfo.Integer AttachedWeaponsCompressionInfo = new CompressionInfo.Integer(-1, 11);

		// Token: 0x0400116D RID: 4461
		public static CompressionInfo.Integer MultiplayerPollRejectReasonCompressionInfo = new CompressionInfo.Integer(0, 3, true);

		// Token: 0x0400116E RID: 4462
		public static CompressionInfo.Integer MultiplayerNotificationCompressionInfo = new CompressionInfo.Integer(0, MultiplayerGameNotificationsComponent.NotificationCount, true);

		// Token: 0x0400116F RID: 4463
		public static CompressionInfo.Integer MultiplayerNotificationParameterCompressionInfo = new CompressionInfo.Integer(-1, 8);

		// Token: 0x04001170 RID: 4464
		public static CompressionInfo.Integer PerkListIndexCompressionInfo = new CompressionInfo.Integer(0, 2);

		// Token: 0x04001171 RID: 4465
		public static CompressionInfo.Integer PerkIndexCompressionInfo = new CompressionInfo.Integer(0, 4);

		// Token: 0x04001172 RID: 4466
		public static CompressionInfo.Float FlagDominationMoraleCompressionInfo = new CompressionInfo.Float(-1f, 8, 0.01f);

		// Token: 0x04001173 RID: 4467
		public static CompressionInfo.Integer TdmGoldChangeCompressionInfo = new CompressionInfo.Integer(0, 2000, true);

		// Token: 0x04001174 RID: 4468
		public static CompressionInfo.Integer TdmGoldGainTypeCompressionInfo = new CompressionInfo.Integer(0, 12);

		// Token: 0x04001175 RID: 4469
		public static CompressionInfo.Integer DuelAreaIndexCompressionInfo = new CompressionInfo.Integer(0, 4);

		// Token: 0x04001176 RID: 4470
		public static CompressionInfo.Integer AutomatedBattleIndexCompressionInfo = new CompressionInfo.Integer(0, 10, true);

		// Token: 0x04001177 RID: 4471
		public static CompressionInfo.Integer SiegeMoraleCompressionInfo = new CompressionInfo.Integer(0, 1440, true);

		// Token: 0x04001178 RID: 4472
		public static CompressionInfo.Integer SiegeMoralePerFlagCompressionInfo = new CompressionInfo.Integer(0, 90, true);

		// Token: 0x04001179 RID: 4473
		public static CompressionInfo.Integer ActionSetCompressionInfo;

		// Token: 0x0400117A RID: 4474
		public static CompressionInfo.Integer MonsterUsageSetCompressionInfo;

		// Token: 0x0400117B RID: 4475
		public static CompressionInfo.Integer OrderTypeCompressionInfo = new CompressionInfo.Integer(0, 41, true);

		// Token: 0x0400117C RID: 4476
		public static CompressionInfo.Integer FormationClassCompressionInfo = new CompressionInfo.Integer(-1, 10, true);

		// Token: 0x0400117D RID: 4477
		public static CompressionInfo.Float OrderPositionCompressionInfo = new CompressionInfo.Float(-100000f, 100000f, 24);

		// Token: 0x0400117E RID: 4478
		public static CompressionInfo.Integer SynchedMissionObjectReadableRecordTypeIndex = new CompressionInfo.Integer(-1, 8);
	}
}
