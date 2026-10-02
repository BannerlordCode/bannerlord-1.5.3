using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B6 RID: 694
	public abstract class MissionMultiplayerGameModeBaseClient : MissionNetwork, ICameraModeLogic
	{
		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002721 RID: 10017 RVA: 0x000913AD File Offset: 0x0008F5AD
		// (set) Token: 0x06002722 RID: 10018 RVA: 0x000913B5 File Offset: 0x0008F5B5
		public MissionLobbyComponent MissionLobbyComponent { get; private set; }

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06002723 RID: 10019 RVA: 0x000913BE File Offset: 0x0008F5BE
		// (set) Token: 0x06002724 RID: 10020 RVA: 0x000913C6 File Offset: 0x0008F5C6
		public MissionNetworkComponent MissionNetworkComponent { get; private set; }

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002725 RID: 10021 RVA: 0x000913CF File Offset: 0x0008F5CF
		// (set) Token: 0x06002726 RID: 10022 RVA: 0x000913D7 File Offset: 0x0008F5D7
		public MissionScoreboardComponent ScoreboardComponent { get; private set; }

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002727 RID: 10023 RVA: 0x000913E0 File Offset: 0x0008F5E0
		// (set) Token: 0x06002728 RID: 10024 RVA: 0x000913E8 File Offset: 0x0008F5E8
		public MultiplayerGameNotificationsComponent NotificationsComponent { get; private set; }

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002729 RID: 10025 RVA: 0x000913F1 File Offset: 0x0008F5F1
		// (set) Token: 0x0600272A RID: 10026 RVA: 0x000913F9 File Offset: 0x0008F5F9
		public MultiplayerWarmupComponent WarmupComponent { get; private set; }

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x0600272B RID: 10027 RVA: 0x00091402 File Offset: 0x0008F602
		// (set) Token: 0x0600272C RID: 10028 RVA: 0x0009140A File Offset: 0x0008F60A
		public IRoundComponent RoundComponent { get; private set; }

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x0600272D RID: 10029 RVA: 0x00091413 File Offset: 0x0008F613
		// (set) Token: 0x0600272E RID: 10030 RVA: 0x0009141B File Offset: 0x0008F61B
		public MultiplayerTimerComponent TimerComponent { get; private set; }

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x0600272F RID: 10031
		public abstract bool IsGameModeUsingGold { get; }

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06002730 RID: 10032
		public abstract bool IsGameModeTactical { get; }

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002731 RID: 10033 RVA: 0x00091424 File Offset: 0x0008F624
		public virtual bool IsGameModeUsingCasualGold
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002732 RID: 10034
		public abstract bool IsGameModeUsingRoundCountdown { get; }

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002733 RID: 10035 RVA: 0x00091427 File Offset: 0x0008F627
		public virtual bool IsGameModeUsingAllowCultureChange
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002734 RID: 10036 RVA: 0x0009142A File Offset: 0x0008F62A
		public virtual bool IsGameModeUsingAllowTroopChange
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06002735 RID: 10037
		public abstract MultiplayerGameType GameType { get; }

		// Token: 0x06002736 RID: 10038
		public abstract int GetGoldAmount();

		// Token: 0x06002737 RID: 10039 RVA: 0x0009142D File Offset: 0x0008F62D
		public virtual SpectatorCameraTypes GetMissionCameraLockMode(bool lockedToMainPlayer)
		{
			return SpectatorCameraTypes.Invalid;
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x00091430 File Offset: 0x0008F630
		public bool IsRoundInProgress
		{
			get
			{
				IRoundComponent roundComponent = this.RoundComponent;
				return roundComponent != null && roundComponent.CurrentRoundState == MultiplayerRoundState.InProgress;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06002739 RID: 10041 RVA: 0x00091446 File Offset: 0x0008F646
		public bool IsInWarmup
		{
			get
			{
				return this.MissionLobbyComponent.IsInWarmup;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x0600273A RID: 10042 RVA: 0x00091453 File Offset: 0x0008F653
		public float RemainingTime
		{
			get
			{
				return this.TimerComponent.GetRemainingTime(GameNetwork.IsClientOrReplay);
			}
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x00091468 File Offset: 0x0008F668
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this.MissionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this.MissionNetworkComponent = base.Mission.GetMissionBehavior<MissionNetworkComponent>();
			this.ScoreboardComponent = base.Mission.GetMissionBehavior<MissionScoreboardComponent>();
			this.NotificationsComponent = base.Mission.GetMissionBehavior<MultiplayerGameNotificationsComponent>();
			this.WarmupComponent = base.Mission.GetMissionBehavior<MultiplayerWarmupComponent>();
			this.RoundComponent = base.Mission.GetMissionBehavior<IRoundComponent>();
			this.TimerComponent = base.Mission.GetMissionBehavior<MultiplayerTimerComponent>();
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000914F2 File Offset: 0x0008F6F2
		public override void EarlyStart()
		{
			this.MissionLobbyComponent.MissionType = this.GameType;
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x00091508 File Offset: 0x0008F708
		public bool CheckTimer(out int remainingTime, out int remainingWarningTime, bool forceUpdate = false)
		{
			bool flag = false;
			float num = 0f;
			if (this.WarmupComponent != null && this.MissionLobbyComponent.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
			{
				flag = !this.WarmupComponent.IsInWarmup;
			}
			else if (this.RoundComponent != null)
			{
				flag = !this.RoundComponent.CurrentRoundState.StateHasVisualTimer();
				num = this.RoundComponent.LastRoundEndRemainingTime;
			}
			if (forceUpdate || !flag)
			{
				if (flag)
				{
					remainingTime = MathF.Ceiling(num);
				}
				else
				{
					remainingTime = MathF.Ceiling(this.RemainingTime);
				}
				remainingWarningTime = this.GetWarningTimer();
				return true;
			}
			remainingTime = 0;
			remainingWarningTime = 0;
			return false;
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x0009159C File Offset: 0x0008F79C
		protected virtual int GetWarningTimer()
		{
			return 0;
		}

		// Token: 0x0600273F RID: 10047
		public abstract void OnGoldAmountChangedForRepresentative(MissionRepresentativeBase representative, int goldAmount);

		// Token: 0x06002740 RID: 10048 RVA: 0x0009159F File Offset: 0x0008F79F
		public virtual bool CanRequestTroopChange()
		{
			return false;
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000915A2 File Offset: 0x0008F7A2
		public virtual bool CanRequestCultureChange()
		{
			return false;
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x000915A8 File Offset: 0x0008F7A8
		public bool IsClassAvailable(MultiplayerClassDivisions.MPHeroClass heroClass)
		{
			FormationClass formationClass;
			if (Enum.TryParse<FormationClass>(heroClass.ClassGroup.StringId, out formationClass))
			{
				return this.MissionLobbyComponent.IsClassAvailable(formationClass);
			}
			Debug.FailedAssert("\"" + heroClass.ClassGroup.StringId + "\" does not match with any FormationClass.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MultiplayerGameModeLogics\\ClientGameModeLogics\\MissionMultiplayerGameModeBaseClient.cs", "IsClassAvailable", 116);
			return false;
		}
	}
}
