using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200029F RID: 671
	public class SallyOutMissionNotificationsHandler
	{
		// Token: 0x06002550 RID: 9552 RVA: 0x00087D68 File Offset: 0x00085F68
		public SallyOutMissionNotificationsHandler(DefaultBattleMissionAgentSpawnLogic spawnLogic, SallyOutMissionController sallyOutController)
		{
			this._spawnLogic = spawnLogic;
			this._sallyOutController = sallyOutController;
			this._spawnLogic.OnReinforcementsSpawned += this.OnReinforcementsSpawned;
			this._spawnLogic.OnInitialTroopsSpawned += this.OnInitialTroopsSpawned;
			this._besiegerSpawnedTroopCount = 0;
			this._notificationTimer = new BasicMissionTimer();
			this._notificationsQueue = new Queue<SallyOutMissionNotificationsHandler.NotificationType>();
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x00087DDC File Offset: 0x00085FDC
		public void OnBesiegedSideFallsbackToKeep()
		{
			if (this._isPlayerBesieged)
			{
				if (Mission.Current.PlayerTeam.FormationsIncludingEmpty.Any<Formation>((Formation f) => f.IsAIControlled && f.CountOfUnits > 0))
				{
					this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.BesiegedSideTacticalRetreat);
					if (Mission.Current.MainAgent != null && Mission.Current.MainAgent.IsActive())
					{
						this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.BesiegedSidePlayerPullbackRequest);
						return;
					}
				}
			}
			else
			{
				this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.BesiegedSideTacticalRetreat);
			}
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x00087E68 File Offset: 0x00086068
		public void OnAfterStart()
		{
			this._isPlayerBesieged = Mission.Current.PlayerTeam.Side == BattleSideEnum.Defender;
			this.SetNotificationTimerEnabled(false, true);
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x00087E8A File Offset: 0x0008608A
		public void OnMissionEnd()
		{
			this._spawnLogic.OnReinforcementsSpawned -= this.OnReinforcementsSpawned;
			this._spawnLogic.OnInitialTroopsSpawned -= this.OnInitialTroopsSpawned;
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x00087EBA File Offset: 0x000860BA
		public void OnDeploymentFinished()
		{
			this.SetNotificationTimerEnabled(true, true);
			this._besiegerSiegeEngines = this._sallyOutController.BesiegerSiegeEngines;
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x00087ED8 File Offset: 0x000860D8
		public void OnMissionTick(float dt)
		{
			if (this._notificationTimerEnabled && this._notificationTimer.ElapsedTime >= 5f)
			{
				this.CheckPeriodicNotifications();
				if (!this._notificationsQueue.IsEmpty<SallyOutMissionNotificationsHandler.NotificationType>())
				{
					SallyOutMissionNotificationsHandler.NotificationType notificationType = this._notificationsQueue.Dequeue();
					this.SendNotification(notificationType);
				}
				this._notificationTimer.Reset();
			}
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x00087F30 File Offset: 0x00086130
		private void SetNotificationTimerEnabled(bool value, bool resetTimer = true)
		{
			this._notificationTimerEnabled = value;
			if (resetTimer)
			{
				this._notificationTimer.Reset();
			}
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x00087F48 File Offset: 0x00086148
		private void CheckPeriodicNotifications()
		{
			if (!this._objectiveMessageSent)
			{
				this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.SallyOutObjective);
				this._objectiveMessageSent = true;
			}
			if (!this._siegeEnginesDestroyedMessageSent && this.IsSiegeEnginesDestroyed())
			{
				this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.SiegeEnginesDestroyed);
				this._siegeEnginesDestroyedMessageSent = true;
			}
			if (!this._besiegersStrengtheningMessageSent && this._spawnLogic.NumberOfRemainingDefenderTroops == 0 && this._besiegerSpawnedTroopCount >= this._spawnLogic.NumberOfActiveDefenderTroops)
			{
				this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.BesiegerSideStrenghtening);
				this._besiegersStrengtheningMessageSent = true;
			}
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x00087FD0 File Offset: 0x000861D0
		private void SendNotification(SallyOutMissionNotificationsHandler.NotificationType type)
		{
			int num = -1;
			if (this._isPlayerBesieged)
			{
				switch (type)
				{
				case SallyOutMissionNotificationsHandler.NotificationType.SallyOutObjective:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_besieged_objective_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/move");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.BesiegerSideStrenghtening:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_enemy_becoming_strong_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/retreat");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.BesiegerSideReinforcementsSpawned:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_enemy_reinforcements_arrived", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/reinforcements");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.BesiegedSideTacticalRetreat:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_allied_troops_tactical_retreat_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/retreat");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.BesiegedSidePlayerPullbackRequest:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_allied_pullback_or_take_command_message", null), 0, null, null, "");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.SiegeEnginesDestroyed:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_enemy_siege_engines_destroyed_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/move");
					break;
				}
			}
			else
			{
				switch (type)
				{
				case SallyOutMissionNotificationsHandler.NotificationType.SallyOutObjective:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_besieger_objective_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/move");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.BesiegerSideReinforcementsSpawned:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_allied_reinforcements_arrived", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/reinforcements");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.BesiegedSideTacticalRetreat:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_enemy_troops_fall_back_to_keep_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/move");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.SiegeEnginesDestroyed:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_allied_siege_engines_destroyed_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/move");
					break;
				}
			}
			if (num >= 0)
			{
				this.PlayNotificationSound(num);
			}
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000881B4 File Offset: 0x000863B4
		private void PlayNotificationSound(int soundId)
		{
			MatrixFrame cameraFrame = Mission.Current.GetCameraFrame();
			Vec3 vec = cameraFrame.origin + cameraFrame.rotation.u;
			MBSoundEvent.PlaySound(soundId, vec);
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000881EB File Offset: 0x000863EB
		private void OnInitialTroopsSpawned(BattleSideEnum battleSide, int numberOfTroopsSpawned)
		{
			if (battleSide == BattleSideEnum.Attacker)
			{
				this._besiegerSpawnedTroopCount += numberOfTroopsSpawned;
			}
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x000881FF File Offset: 0x000863FF
		private void OnReinforcementsSpawned(BattleSideEnum battleSide, int numberOfTroopsSpawned)
		{
			if (battleSide == BattleSideEnum.Attacker)
			{
				this._besiegerSpawnedTroopCount += numberOfTroopsSpawned;
				this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.BesiegerSideReinforcementsSpawned);
			}
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x0008821F File Offset: 0x0008641F
		private bool IsSiegeEnginesDestroyed()
		{
			if (this._besiegerSiegeEngines != null)
			{
				return this._besiegerSiegeEngines.All<SiegeWeapon>((SiegeWeapon siegeEngine) => siegeEngine.DestructionComponent.IsDestroyed);
			}
			return false;
		}

		// Token: 0x04000E70 RID: 3696
		private const float NotificationCheckInterval = 5f;

		// Token: 0x04000E71 RID: 3697
		private DefaultBattleMissionAgentSpawnLogic _spawnLogic;

		// Token: 0x04000E72 RID: 3698
		private SallyOutMissionController _sallyOutController;

		// Token: 0x04000E73 RID: 3699
		private bool _isPlayerBesieged;

		// Token: 0x04000E74 RID: 3700
		private MBReadOnlyList<SiegeWeapon> _besiegerSiegeEngines;

		// Token: 0x04000E75 RID: 3701
		private Queue<SallyOutMissionNotificationsHandler.NotificationType> _notificationsQueue;

		// Token: 0x04000E76 RID: 3702
		private BasicMissionTimer _notificationTimer;

		// Token: 0x04000E77 RID: 3703
		private bool _notificationTimerEnabled = true;

		// Token: 0x04000E78 RID: 3704
		private bool _objectiveMessageSent;

		// Token: 0x04000E79 RID: 3705
		private bool _siegeEnginesDestroyedMessageSent;

		// Token: 0x04000E7A RID: 3706
		private bool _besiegersStrengtheningMessageSent;

		// Token: 0x04000E7B RID: 3707
		private int _besiegerSpawnedTroopCount;

		// Token: 0x02000578 RID: 1400
		private enum NotificationType
		{
			// Token: 0x04001EAC RID: 7852
			SallyOutObjective,
			// Token: 0x04001EAD RID: 7853
			BesiegerSideStrenghtening,
			// Token: 0x04001EAE RID: 7854
			BesiegerSideReinforcementsSpawned,
			// Token: 0x04001EAF RID: 7855
			BesiegedSideTacticalRetreat,
			// Token: 0x04001EB0 RID: 7856
			BesiegedSidePlayerPullbackRequest,
			// Token: 0x04001EB1 RID: 7857
			SiegeEnginesDestroyed
		}
	}
}
