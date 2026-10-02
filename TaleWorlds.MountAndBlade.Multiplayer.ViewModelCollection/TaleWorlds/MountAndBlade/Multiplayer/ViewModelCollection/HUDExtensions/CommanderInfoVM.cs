using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x02000093 RID: 147
	public class CommanderInfoVM : ViewModel
	{
		// Token: 0x06000E2B RID: 3627 RVA: 0x0002B924 File Offset: 0x00029B24
		public CommanderInfoVM(MissionRepresentativeBase missionRepresentative)
		{
			this._missionRepresentative = missionRepresentative;
			this.AllyControlPoints = new MBBindingList<CapturePointVM>();
			this.NeutralControlPoints = new MBBindingList<CapturePointVM>();
			this.EnemyControlPoints = new MBBindingList<CapturePointVM>();
			this._gameMode = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this._missionScoreboardComponent = Mission.Current.GetMissionBehavior<MissionScoreboardComponent>();
			this._commanderInfo = Mission.Current.GetMissionBehavior<ICommanderInfo>();
			this.ShowTacticalInfo = true;
			if (this._gameMode != null)
			{
				this.UpdateWarmupDependentFlags(this._gameMode.IsInWarmup);
				this.UsePowerComparer = this._gameMode.GameType == MultiplayerGameType.Battle && this._gameMode.ScoreboardComponent != null;
				if (this.UsePowerComparer)
				{
					this.PowerLevelComparer = new PowerLevelComparer(1.0, 1.0);
				}
				if (this.UseMoraleComparer)
				{
					this.RegisterMoraleEvents();
				}
			}
			this._siegeClient = Mission.Current.GetMissionBehavior<MissionMultiplayerSiegeClient>();
			if (this._siegeClient != null)
			{
				this._siegeClient.OnCapturePointRemainingMoraleGainsChangedEvent += this.OnCapturePointRemainingMoraleGainsChanged;
			}
			Mission.Current.OnMissionReset += this.OnMissionReset;
			MultiplayerMissionAgentVisualSpawnComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
			missionBehavior.OnMyAgentSpawnedFromVisual += this.OnPreparationEnded;
			missionBehavior.OnMyAgentVisualSpawned += this.OnRoundStarted;
			this.OnTeamChanged();
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0002BA80 File Offset: 0x00029C80
		private void OnRoundStarted()
		{
			this.OnTeamChanged();
			if (this.UsePowerComparer)
			{
				this._attackerTeamInitialMemberCount = this._missionScoreboardComponent.Sides[1].Players.Count<MissionPeer>();
				this._defenderTeamInitialMemberCount = this._missionScoreboardComponent.Sides[0].Players.Count<MissionPeer>();
			}
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0002BAD8 File Offset: 0x00029CD8
		private void RegisterMoraleEvents()
		{
			if (!this._areMoraleEventsRegistered)
			{
				this._commanderInfo.OnMoraleChangedEvent += this.OnUpdateMorale;
				this._commanderInfo.OnFlagNumberChangedEvent += this.OnNumberOfCapturePointsChanged;
				this._commanderInfo.OnCapturePointOwnerChangedEvent += this.OnCapturePointOwnerChanged;
				this.AreMoralesIndependent = this._commanderInfo.AreMoralesIndependent;
				this.ResetCapturePointLists();
				this.InitCapturePoints();
				this._areMoraleEventsRegistered = true;
			}
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0002BB56 File Offset: 0x00029D56
		private void OnPreparationEnded()
		{
			this.ShowTacticalInfo = true;
			this.OnTeamChanged();
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0002BB68 File Offset: 0x00029D68
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this._commanderInfo != null)
			{
				this._commanderInfo.OnMoraleChangedEvent -= this.OnUpdateMorale;
				this._commanderInfo.OnFlagNumberChangedEvent -= this.OnNumberOfCapturePointsChanged;
				this._commanderInfo.OnCapturePointOwnerChangedEvent -= this.OnCapturePointOwnerChanged;
			}
			Mission.Current.OnMissionReset -= this.OnMissionReset;
			MultiplayerMissionAgentVisualSpawnComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
			missionBehavior.OnMyAgentSpawnedFromVisual -= this.OnPreparationEnded;
			missionBehavior.OnMyAgentVisualSpawned -= this.OnRoundStarted;
			if (this._siegeClient != null)
			{
				this._siegeClient.OnCapturePointRemainingMoraleGainsChangedEvent -= this.OnCapturePointRemainingMoraleGainsChanged;
			}
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0002BC2A File Offset: 0x00029E2A
		public void UpdateWarmupDependentFlags(bool isInWarmup)
		{
			this.UseMoraleComparer = !isInWarmup && this._gameMode.IsGameModeTactical && this._commanderInfo != null;
			this.ShowControlPointStatus = !isInWarmup;
			if (!isInWarmup && this.UseMoraleComparer)
			{
				this.RegisterMoraleEvents();
			}
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0002BC6C File Offset: 0x00029E6C
		public void OnUpdateMorale(BattleSideEnum side, float morale)
		{
			if (this._allyTeam != null && this._allyTeam.Side == side)
			{
				this.AllyMoralePercentage = MathF.Round(MathF.Abs(morale * 100f));
				return;
			}
			if (this._enemyTeam != null && this._enemyTeam.Side == side)
			{
				this.EnemyMoralePercentage = MathF.Round(MathF.Abs(morale * 100f));
			}
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0002BCD4 File Offset: 0x00029ED4
		private void OnMissionReset(object sender, PropertyChangedEventArgs e)
		{
			if (this.UseMoraleComparer)
			{
				this.AllyMoralePercentage = 50;
				this.EnemyMoralePercentage = 50;
			}
			if (this.UsePowerComparer)
			{
				this.PowerLevelComparer.Update(1.0, 1.0, 1.0, 1.0);
			}
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0002BD30 File Offset: 0x00029F30
		internal void Tick(float dt)
		{
			foreach (CapturePointVM capturePointVM in this.AllyControlPoints)
			{
				capturePointVM.Refresh(0f, 0f, 0f);
			}
			foreach (CapturePointVM capturePointVM2 in this.EnemyControlPoints)
			{
				capturePointVM2.Refresh(0f, 0f, 0f);
			}
			foreach (CapturePointVM capturePointVM3 in this.NeutralControlPoints)
			{
				capturePointVM3.Refresh(0f, 0f, 0f);
			}
			if (this._allyTeam != null && this.UsePowerComparer)
			{
				int count = Mission.Current.AttackerTeam.ActiveAgents.Count;
				int count2 = Mission.Current.DefenderTeam.ActiveAgents.Count;
				this.AllyMemberCount = ((this._allyTeam.Side == BattleSideEnum.Attacker) ? count : count2);
				this.EnemyMemberCount = ((this._allyTeam.Side == BattleSideEnum.Attacker) ? count2 : count);
				int num = ((this._allyTeam.Side == BattleSideEnum.Attacker) ? this._attackerTeamInitialMemberCount : this._defenderTeamInitialMemberCount);
				Team allyTeam = this._allyTeam;
				int num2 = ((allyTeam != null && allyTeam.Side == BattleSideEnum.Attacker) ? this._defenderTeamInitialMemberCount : this._attackerTeamInitialMemberCount);
				if (num2 == 0 && num == 0)
				{
					this.PowerLevelComparer.Update(1.0, 1.0, 1.0, 1.0);
					return;
				}
				this.PowerLevelComparer.Update((double)this.EnemyMemberCount, (double)this.AllyMemberCount, (double)num2, (double)num);
			}
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x0002BF24 File Offset: 0x0002A124
		private void OnCapturePointOwnerChanged(FlagCapturePoint target, Team newOwnerTeam)
		{
			CapturePointVM capturePointVM = this.FindCapturePointInLists(target);
			if (capturePointVM != null)
			{
				this.RemoveFlagFromLists(capturePointVM);
				this.HandleAddNewCapturePoint(capturePointVM);
				capturePointVM.OnOwnerChanged(newOwnerTeam);
			}
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x0002BF54 File Offset: 0x0002A154
		private void OnCapturePointRemainingMoraleGainsChanged(int[] remainingMoraleArr)
		{
			foreach (CapturePointVM capturePointVM in this.AllyControlPoints)
			{
				int flagIndex = capturePointVM.Target.FlagIndex;
				if (flagIndex >= 0 && remainingMoraleArr.Length > flagIndex)
				{
					capturePointVM.OnRemainingMoraleChanged(remainingMoraleArr[flagIndex]);
				}
			}
			foreach (CapturePointVM capturePointVM2 in this.EnemyControlPoints)
			{
				int flagIndex2 = capturePointVM2.Target.FlagIndex;
				if (flagIndex2 >= 0 && remainingMoraleArr.Length > flagIndex2)
				{
					capturePointVM2.OnRemainingMoraleChanged(remainingMoraleArr[flagIndex2]);
				}
			}
			foreach (CapturePointVM capturePointVM3 in this.NeutralControlPoints)
			{
				int flagIndex3 = capturePointVM3.Target.FlagIndex;
				if (flagIndex3 >= 0 && remainingMoraleArr.Length > flagIndex3)
				{
					capturePointVM3.OnRemainingMoraleChanged(remainingMoraleArr[flagIndex3]);
				}
			}
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x0002C06C File Offset: 0x0002A26C
		private void OnNumberOfCapturePointsChanged()
		{
			this.ResetCapturePointLists();
			this.InitCapturePoints();
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0002C07C File Offset: 0x0002A27C
		private void InitCapturePoints()
		{
			if (this._commanderInfo != null)
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				bool flag;
				if (myPeer == null)
				{
					flag = null != null;
				}
				else
				{
					MissionPeer component = myPeer.GetComponent<MissionPeer>();
					flag = ((component != null) ? component.Team : null) != null;
				}
				if (flag)
				{
					foreach (FlagCapturePoint flagCapturePoint in this._commanderInfo.AllCapturePoints.Where<FlagCapturePoint>((FlagCapturePoint c) => !c.IsDeactivated).ToArray<FlagCapturePoint>())
					{
						CapturePointVM capturePointVM = new CapturePointVM(flagCapturePoint, TargetIconType.Flag_A + flagCapturePoint.FlagIndex);
						this.HandleAddNewCapturePoint(capturePointVM);
					}
					this.RefreshMoraleIncreaseLevels();
				}
			}
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0002C118 File Offset: 0x0002A318
		private void HandleAddNewCapturePoint(CapturePointVM capturePointVM)
		{
			this.RemoveFlagFromLists(capturePointVM);
			if (this._allyTeam == null)
			{
				return;
			}
			Team team = this._commanderInfo.GetFlagOwner(capturePointVM.Target);
			if (team != null && (team.Side == BattleSideEnum.None || team.Side == BattleSideEnum.NumSides))
			{
				team = null;
			}
			capturePointVM.OnOwnerChanged(team);
			bool isDeactivated = capturePointVM.Target.IsDeactivated;
			if ((team == null || team.TeamIndex == -1) && !isDeactivated)
			{
				int num = MathF.Min(this.NeutralControlPoints.Count, capturePointVM.Target.FlagIndex);
				this.NeutralControlPoints.Insert(num, capturePointVM);
			}
			else if (this._allyTeam == team)
			{
				int num2 = MathF.Min(this.AllyControlPoints.Count, capturePointVM.Target.FlagIndex);
				this.AllyControlPoints.Insert(num2, capturePointVM);
			}
			else if (this._allyTeam != team)
			{
				int num3 = MathF.Min(this.EnemyControlPoints.Count, capturePointVM.Target.FlagIndex);
				this.EnemyControlPoints.Insert(num3, capturePointVM);
			}
			else if (team.Side != BattleSideEnum.None)
			{
				Debug.FailedAssert("Incorrect flag team state", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\HUDExtensions\\CommanderInfoVM.cs", "HandleAddNewCapturePoint", 321);
			}
			this.RefreshMoraleIncreaseLevels();
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0002C240 File Offset: 0x0002A440
		private void RefreshMoraleIncreaseLevels()
		{
			this.AllyMoraleIncreaseLevel = MathF.Max(0, this.AllyControlPoints.Count - this.EnemyControlPoints.Count);
			this.EnemyMoraleIncreaseLevel = MathF.Max(0, this.EnemyControlPoints.Count - this.AllyControlPoints.Count);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x0002C294 File Offset: 0x0002A494
		private void RemoveFlagFromLists(CapturePointVM capturePoint)
		{
			if (this.AllyControlPoints.Contains(capturePoint))
			{
				this.AllyControlPoints.Remove(capturePoint);
				return;
			}
			if (this.NeutralControlPoints.Contains(capturePoint))
			{
				this.NeutralControlPoints.Remove(capturePoint);
				return;
			}
			if (this.EnemyControlPoints.Contains(capturePoint))
			{
				this.EnemyControlPoints.Remove(capturePoint);
			}
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0002C2F4 File Offset: 0x0002A4F4
		public void OnTeamChanged()
		{
			if (!GameNetwork.IsMyPeerReady || !this.ShowTacticalInfo)
			{
				return;
			}
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			this._allyTeam = component.Team;
			if (this._allyTeam == null)
			{
				return;
			}
			IEnumerable<Team> enumerable = Mission.Current.Teams.Where<Team>((Team t) => t.IsEnemyOf(this._allyTeam));
			this._enemyTeam = enumerable.FirstOrDefault<Team>();
			if (this._allyTeam.Side == BattleSideEnum.None)
			{
				this._allyTeam = Mission.Current.AttackerTeam;
				return;
			}
			this.ResetCapturePointLists();
			this.InitCapturePoints();
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0002C384 File Offset: 0x0002A584
		private void ResetCapturePointLists()
		{
			this.AllyControlPoints.Clear();
			this.NeutralControlPoints.Clear();
			this.EnemyControlPoints.Clear();
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0002C3A8 File Offset: 0x0002A5A8
		private CapturePointVM FindCapturePointInLists(FlagCapturePoint target)
		{
			CapturePointVM capturePointVM = this.AllyControlPoints.SingleOrDefault<CapturePointVM>((CapturePointVM c) => c.Target == target);
			if (capturePointVM != null)
			{
				return capturePointVM;
			}
			CapturePointVM capturePointVM2 = this.EnemyControlPoints.SingleOrDefault<CapturePointVM>((CapturePointVM c) => c.Target == target);
			if (capturePointVM2 != null)
			{
				return capturePointVM2;
			}
			CapturePointVM capturePointVM3 = this.NeutralControlPoints.SingleOrDefault<CapturePointVM>((CapturePointVM c) => c.Target == target);
			if (capturePointVM3 != null)
			{
				return capturePointVM3;
			}
			return null;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0002C41A File Offset: 0x0002A61A
		public void RefreshColors(string allyTeamColor, string allyTeamColorSecondary, string enemyTeamColor, string enemyTeamColorSecondary)
		{
			this.AllyTeamColor = allyTeamColor;
			this.AllyTeamColorSecondary = allyTeamColorSecondary;
			this.EnemyTeamColor = enemyTeamColor;
			this.EnemyTeamColorSecondary = enemyTeamColorSecondary;
			if (this.UsePowerComparer)
			{
				this.PowerLevelComparer.SetColors(this.EnemyTeamColor, this.AllyTeamColor);
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06000E3F RID: 3647 RVA: 0x0002C458 File Offset: 0x0002A658
		// (set) Token: 0x06000E40 RID: 3648 RVA: 0x0002C460 File Offset: 0x0002A660
		[DataSourceProperty]
		public MBBindingList<CapturePointVM> AllyControlPoints
		{
			get
			{
				return this._allyControlPoints;
			}
			set
			{
				if (value != this._allyControlPoints)
				{
					this._allyControlPoints = value;
					base.OnPropertyChangedWithValue<MBBindingList<CapturePointVM>>(value, "AllyControlPoints");
				}
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x0002C47E File Offset: 0x0002A67E
		// (set) Token: 0x06000E42 RID: 3650 RVA: 0x0002C486 File Offset: 0x0002A686
		[DataSourceProperty]
		public MBBindingList<CapturePointVM> NeutralControlPoints
		{
			get
			{
				return this._neutralControlPoints;
			}
			set
			{
				if (value != this._neutralControlPoints)
				{
					this._neutralControlPoints = value;
					base.OnPropertyChangedWithValue<MBBindingList<CapturePointVM>>(value, "NeutralControlPoints");
				}
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x0002C4A4 File Offset: 0x0002A6A4
		// (set) Token: 0x06000E44 RID: 3652 RVA: 0x0002C4AC File Offset: 0x0002A6AC
		[DataSourceProperty]
		public MBBindingList<CapturePointVM> EnemyControlPoints
		{
			get
			{
				return this._enemyControlPoints;
			}
			set
			{
				if (value != this._enemyControlPoints)
				{
					this._enemyControlPoints = value;
					base.OnPropertyChangedWithValue<MBBindingList<CapturePointVM>>(value, "EnemyControlPoints");
				}
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x0002C4CA File Offset: 0x0002A6CA
		// (set) Token: 0x06000E46 RID: 3654 RVA: 0x0002C4D2 File Offset: 0x0002A6D2
		[DataSourceProperty]
		public string AllyTeamColor
		{
			get
			{
				return this._allyTeamColor;
			}
			set
			{
				if (value != this._allyTeamColor)
				{
					this._allyTeamColor = value;
					base.OnPropertyChangedWithValue<string>(value, "AllyTeamColor");
				}
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06000E47 RID: 3655 RVA: 0x0002C4F5 File Offset: 0x0002A6F5
		// (set) Token: 0x06000E48 RID: 3656 RVA: 0x0002C4FD File Offset: 0x0002A6FD
		[DataSourceProperty]
		public string AllyTeamColorSecondary
		{
			get
			{
				return this._allyTeamColorSecondary;
			}
			set
			{
				if (value != this._allyTeamColorSecondary)
				{
					this._allyTeamColorSecondary = value;
					base.OnPropertyChangedWithValue<string>(value, "AllyTeamColorSecondary");
				}
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x0002C520 File Offset: 0x0002A720
		// (set) Token: 0x06000E4A RID: 3658 RVA: 0x0002C528 File Offset: 0x0002A728
		[DataSourceProperty]
		public string EnemyTeamColor
		{
			get
			{
				return this._enemyTeamColor;
			}
			set
			{
				if (value != this._enemyTeamColor)
				{
					this._enemyTeamColor = value;
					base.OnPropertyChangedWithValue<string>(value, "EnemyTeamColor");
				}
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x0002C54B File Offset: 0x0002A74B
		// (set) Token: 0x06000E4C RID: 3660 RVA: 0x0002C553 File Offset: 0x0002A753
		[DataSourceProperty]
		public string EnemyTeamColorSecondary
		{
			get
			{
				return this._enemyTeamColorSecondary;
			}
			set
			{
				if (value != this._enemyTeamColorSecondary)
				{
					this._enemyTeamColorSecondary = value;
					base.OnPropertyChangedWithValue<string>(value, "EnemyTeamColorSecondary");
				}
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x0002C576 File Offset: 0x0002A776
		// (set) Token: 0x06000E4E RID: 3662 RVA: 0x0002C57E File Offset: 0x0002A77E
		[DataSourceProperty]
		public int AllyMoraleIncreaseLevel
		{
			get
			{
				return this._allyMoraleIncreaseLevel;
			}
			set
			{
				if (value != this._allyMoraleIncreaseLevel)
				{
					this._allyMoraleIncreaseLevel = value;
					base.OnPropertyChangedWithValue(value, "AllyMoraleIncreaseLevel");
				}
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x0002C59C File Offset: 0x0002A79C
		// (set) Token: 0x06000E50 RID: 3664 RVA: 0x0002C5A4 File Offset: 0x0002A7A4
		[DataSourceProperty]
		public int EnemyMoraleIncreaseLevel
		{
			get
			{
				return this._enemyMoraleIncreaseLevel;
			}
			set
			{
				if (value != this._enemyMoraleIncreaseLevel)
				{
					this._enemyMoraleIncreaseLevel = value;
					base.OnPropertyChangedWithValue(value, "EnemyMoraleIncreaseLevel");
				}
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x0002C5C2 File Offset: 0x0002A7C2
		// (set) Token: 0x06000E52 RID: 3666 RVA: 0x0002C5CA File Offset: 0x0002A7CA
		[DataSourceProperty]
		public int AllyMoralePercentage
		{
			get
			{
				return this._allyMoralePercentage;
			}
			set
			{
				if (value != this._allyMoralePercentage)
				{
					this._allyMoralePercentage = value;
					base.OnPropertyChangedWithValue(value, "AllyMoralePercentage");
				}
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x0002C5E8 File Offset: 0x0002A7E8
		// (set) Token: 0x06000E54 RID: 3668 RVA: 0x0002C5F0 File Offset: 0x0002A7F0
		[DataSourceProperty]
		public int EnemyMoralePercentage
		{
			get
			{
				return this._enemyMoralePercentage;
			}
			set
			{
				if (value != this._enemyMoralePercentage)
				{
					this._enemyMoralePercentage = value;
					base.OnPropertyChangedWithValue(value, "EnemyMoralePercentage");
				}
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x0002C60E File Offset: 0x0002A80E
		// (set) Token: 0x06000E56 RID: 3670 RVA: 0x0002C616 File Offset: 0x0002A816
		[DataSourceProperty]
		public int AllyMemberCount
		{
			get
			{
				return this._allyMemberCount;
			}
			set
			{
				if (value != this._allyMemberCount)
				{
					this._allyMemberCount = value;
					base.OnPropertyChangedWithValue(value, "AllyMemberCount");
				}
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0002C634 File Offset: 0x0002A834
		// (set) Token: 0x06000E58 RID: 3672 RVA: 0x0002C63C File Offset: 0x0002A83C
		[DataSourceProperty]
		public int EnemyMemberCount
		{
			get
			{
				return this._enemyMemberCount;
			}
			set
			{
				if (value != this._enemyMemberCount)
				{
					this._enemyMemberCount = value;
					base.OnPropertyChangedWithValue(value, "EnemyMemberCount");
				}
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0002C65A File Offset: 0x0002A85A
		// (set) Token: 0x06000E5A RID: 3674 RVA: 0x0002C662 File Offset: 0x0002A862
		[DataSourceProperty]
		public PowerLevelComparer PowerLevelComparer
		{
			get
			{
				return this._powerLevelComparer;
			}
			set
			{
				if (value != this._powerLevelComparer)
				{
					this._powerLevelComparer = value;
					base.OnPropertyChangedWithValue<PowerLevelComparer>(value, "PowerLevelComparer");
				}
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x0002C680 File Offset: 0x0002A880
		// (set) Token: 0x06000E5C RID: 3676 RVA: 0x0002C688 File Offset: 0x0002A888
		[DataSourceProperty]
		public bool UsePowerComparer
		{
			get
			{
				return this._usePowerComparer;
			}
			set
			{
				if (value != this._usePowerComparer)
				{
					this._usePowerComparer = value;
					base.OnPropertyChangedWithValue(value, "UsePowerComparer");
				}
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06000E5D RID: 3677 RVA: 0x0002C6A6 File Offset: 0x0002A8A6
		// (set) Token: 0x06000E5E RID: 3678 RVA: 0x0002C6AE File Offset: 0x0002A8AE
		[DataSourceProperty]
		public bool UseMoraleComparer
		{
			get
			{
				return this._useMoraleComparer;
			}
			set
			{
				if (value != this._useMoraleComparer)
				{
					this._useMoraleComparer = value;
					base.OnPropertyChangedWithValue(value, "UseMoraleComparer");
				}
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x0002C6CC File Offset: 0x0002A8CC
		// (set) Token: 0x06000E60 RID: 3680 RVA: 0x0002C6D4 File Offset: 0x0002A8D4
		[DataSourceProperty]
		public bool ShowTacticalInfo
		{
			get
			{
				return this._showTacticalInfo;
			}
			set
			{
				if (value != this._showTacticalInfo)
				{
					this._showTacticalInfo = value;
					base.OnPropertyChangedWithValue(value, "ShowTacticalInfo");
				}
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x0002C6F2 File Offset: 0x0002A8F2
		// (set) Token: 0x06000E62 RID: 3682 RVA: 0x0002C6FA File Offset: 0x0002A8FA
		[DataSourceProperty]
		public bool AreMoralesIndependent
		{
			get
			{
				return this._areMoralesIndependent;
			}
			set
			{
				if (value != this._areMoralesIndependent)
				{
					this._areMoralesIndependent = value;
					base.OnPropertyChangedWithValue(value, "AreMoralesIndependent");
				}
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x0002C718 File Offset: 0x0002A918
		// (set) Token: 0x06000E64 RID: 3684 RVA: 0x0002C720 File Offset: 0x0002A920
		[DataSourceProperty]
		public bool ShowControlPointStatus
		{
			get
			{
				return this._showControlPointStatus;
			}
			set
			{
				if (value != this._showControlPointStatus)
				{
					this._showControlPointStatus = value;
					base.OnPropertyChangedWithValue(value, "ShowControlPointStatus");
				}
			}
		}

		// Token: 0x04000678 RID: 1656
		private readonly MissionRepresentativeBase _missionRepresentative;

		// Token: 0x04000679 RID: 1657
		private readonly MissionMultiplayerGameModeBaseClient _gameMode;

		// Token: 0x0400067A RID: 1658
		private readonly MissionMultiplayerSiegeClient _siegeClient;

		// Token: 0x0400067B RID: 1659
		private readonly MissionScoreboardComponent _missionScoreboardComponent;

		// Token: 0x0400067C RID: 1660
		private const float InitialArmyStrength = 1f;

		// Token: 0x0400067D RID: 1661
		private int _attackerTeamInitialMemberCount;

		// Token: 0x0400067E RID: 1662
		private int _defenderTeamInitialMemberCount;

		// Token: 0x0400067F RID: 1663
		private Team _allyTeam;

		// Token: 0x04000680 RID: 1664
		private Team _enemyTeam;

		// Token: 0x04000681 RID: 1665
		private ICommanderInfo _commanderInfo;

		// Token: 0x04000682 RID: 1666
		private bool _areMoraleEventsRegistered;

		// Token: 0x04000683 RID: 1667
		private MBBindingList<CapturePointVM> _allyControlPoints;

		// Token: 0x04000684 RID: 1668
		private MBBindingList<CapturePointVM> _neutralControlPoints;

		// Token: 0x04000685 RID: 1669
		private MBBindingList<CapturePointVM> _enemyControlPoints;

		// Token: 0x04000686 RID: 1670
		private int _allyMoraleIncreaseLevel;

		// Token: 0x04000687 RID: 1671
		private int _enemyMoraleIncreaseLevel;

		// Token: 0x04000688 RID: 1672
		private int _allyMoralePercentage;

		// Token: 0x04000689 RID: 1673
		private int _enemyMoralePercentage;

		// Token: 0x0400068A RID: 1674
		private int _allyMemberCount;

		// Token: 0x0400068B RID: 1675
		private int _enemyMemberCount;

		// Token: 0x0400068C RID: 1676
		private PowerLevelComparer _powerLevelComparer;

		// Token: 0x0400068D RID: 1677
		private bool _showTacticalInfo;

		// Token: 0x0400068E RID: 1678
		private bool _usePowerComparer;

		// Token: 0x0400068F RID: 1679
		private bool _useMoraleComparer;

		// Token: 0x04000690 RID: 1680
		private bool _areMoralesIndependent;

		// Token: 0x04000691 RID: 1681
		private bool _showControlPointStatus;

		// Token: 0x04000692 RID: 1682
		private string _allyTeamColor;

		// Token: 0x04000693 RID: 1683
		private string _allyTeamColorSecondary;

		// Token: 0x04000694 RID: 1684
		private string _enemyTeamColor;

		// Token: 0x04000695 RID: 1685
		private string _enemyTeamColorSecondary;
	}
}
