using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.Source.Missions;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200027D RID: 637
	public class BattleEndLogic : MissionLogic, IBattleEndLogic
	{
		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060023CB RID: 9163 RVA: 0x0007F59E File Offset: 0x0007D79E
		public bool PlayerVictory
		{
			get
			{
				return (this._isEnemySideRetreating || this._isEnemySideDepleted) && !this._isEnemyDefenderPulledBack;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060023CC RID: 9164 RVA: 0x0007F5BB File Offset: 0x0007D7BB
		public bool EnemyVictory
		{
			get
			{
				return this._isPlayerSideRetreating || this._isPlayerSideDepleted;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060023CD RID: 9165 RVA: 0x0007F5CD File Offset: 0x0007D7CD
		public bool IsEnemySideRetreating
		{
			get
			{
				return this._isEnemySideRetreating;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060023CE RID: 9166 RVA: 0x0007F5D5 File Offset: 0x0007D7D5
		// (set) Token: 0x060023CF RID: 9167 RVA: 0x0007F5DD File Offset: 0x0007D7DD
		private bool _notificationsDisabled { get; set; }

		// Token: 0x060023D0 RID: 9168 RVA: 0x0007F5E6 File Offset: 0x0007D7E6
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._checkRetreatingTimer = new BasicMissionTimer();
			this._missionAgentSpawnLogic = base.Mission.GetMissionBehavior<IMissionAgentSpawnLogic>();
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x0007F60C File Offset: 0x0007D80C
		public override void OnMissionTick(float dt)
		{
			if (base.Mission.IsMissionEnding)
			{
				if (this._notificationsDisabled)
				{
					this._scoreBoardOpenedOnceOnMissionEnd = true;
				}
				if (this._missionEndedMessageShown && !this._scoreBoardOpenedOnceOnMissionEnd)
				{
					if (this._checkRetreatingTimer.ElapsedTime > 7f)
					{
						this.CheckIsEnemySideRetreatingOrOneSideDepleted();
						this._checkRetreatingTimer.Reset();
						if (base.Mission.MissionResult != null && base.Mission.MissionResult.PlayerDefeated)
						{
							GameTexts.SetVariable("leave_key", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("Generic", 4), 1f));
							MBInformationManager.AddQuickInformation(GameTexts.FindText("str_battle_lost_press_tab_to_view_results", null), 0, null, null, "");
						}
						else if (base.Mission.MissionResult != null && base.Mission.MissionResult.PlayerVictory)
						{
							if (this._isEnemySideDepleted)
							{
								GameTexts.SetVariable("leave_key", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("Generic", 4), 1f));
								MBInformationManager.AddQuickInformation(GameTexts.FindText("str_battle_won_press_tab_to_view_results", null), 0, null, null, "");
							}
						}
						else
						{
							GameTexts.SetVariable("leave_key", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("Generic", 4), 1f));
							MBInformationManager.AddQuickInformation(GameTexts.FindText("str_battle_finished_press_tab_to_view_results", null), 0, null, null, "");
						}
					}
				}
				else if (this._checkRetreatingTimer.ElapsedTime > 3f && !this._scoreBoardOpenedOnceOnMissionEnd)
				{
					if (base.Mission.MissionResult != null && base.Mission.MissionResult.PlayerDefeated)
					{
						if (this._isPlayerSideDepleted)
						{
							MBInformationManager.AddQuickInformation(GameTexts.FindText("str_battle_lost", null), 0, null, null, "");
						}
						else if (this._isPlayerSideRetreating)
						{
							MBInformationManager.AddQuickInformation(GameTexts.FindText("str_friendlies_are_fleeing_you_lost", null), 0, null, null, "");
						}
					}
					else if (base.Mission.MissionResult != null && base.Mission.MissionResult.PlayerVictory)
					{
						if (this._isEnemySideDepleted)
						{
							MBInformationManager.AddQuickInformation(GameTexts.FindText("str_battle_won", null), 0, null, null, "");
						}
						else if (this._isEnemySideRetreating)
						{
							MBInformationManager.AddQuickInformation(GameTexts.FindText("str_enemies_are_fleeing_you_won", null), 0, null, null, "");
						}
					}
					else
					{
						MBInformationManager.AddQuickInformation(GameTexts.FindText("str_battle_finished", null), 0, null, null, "");
					}
					this._missionEndedMessageShown = true;
					this._checkRetreatingTimer.Reset();
				}
				if (!this._victoryReactionsActivated)
				{
					AgentVictoryLogic missionBehavior = base.Mission.GetMissionBehavior<AgentVictoryLogic>();
					if (missionBehavior != null)
					{
						this.CheckIsEnemySideRetreatingOrOneSideDepleted();
						if (this._isEnemySideDepleted)
						{
							missionBehavior.SetTimersOfVictoryReactionsOnBattleEnd(base.Mission.PlayerTeam.Side);
							this._victoryReactionsActivated = true;
							return;
						}
						if (this._isPlayerSideDepleted)
						{
							missionBehavior.SetTimersOfVictoryReactionsOnBattleEnd(base.Mission.PlayerEnemyTeam.Side);
							this._victoryReactionsActivated = true;
							return;
						}
						if (this._isEnemySideRetreating && !this._victoryReactionsActivatedForRetreating)
						{
							missionBehavior.SetTimersOfVictoryReactionsOnRetreat(base.Mission.PlayerTeam.Side);
							this._victoryReactionsActivatedForRetreating = true;
							return;
						}
						if (this._isPlayerSideRetreating && !this._victoryReactionsActivatedForRetreating)
						{
							missionBehavior.SetTimersOfVictoryReactionsOnRetreat(base.Mission.PlayerEnemyTeam.Side);
							this._victoryReactionsActivatedForRetreating = true;
							return;
						}
					}
				}
			}
			else if (this._checkRetreatingTimer.ElapsedTime > 1f)
			{
				this.CheckIsEnemySideRetreatingOrOneSideDepleted();
				this._checkRetreatingTimer.Reset();
			}
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x0007F978 File Offset: 0x0007DB78
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (this._enemyDefenderPullbackEnabled && this._troopNumberNeededForEnemyDefenderPullBack > 0 && affectedAgent.IsHuman && agentState == AgentState.Routed && affectedAgent.Team != null && affectedAgent.Team.Side == BattleSideEnum.Defender && affectedAgent.Team.Side != base.Mission.PlayerTeam.Side)
			{
				this._troopNumberNeededForEnemyDefenderPullBack--;
				this._isEnemyDefenderPulledBack = this._troopNumberNeededForEnemyDefenderPullBack <= 0;
			}
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x0007F9F4 File Offset: 0x0007DBF4
		public override bool MissionEnded(ref MissionResult missionResult)
		{
			bool flag = false;
			if (this._isEnemySideDepleted && this._isEnemyDefenderPulledBack)
			{
				missionResult = MissionResult.CreateDefenderPushedBack();
				flag = true;
			}
			else if (this._isEnemySideRetreating || this._isEnemySideDepleted)
			{
				missionResult = MissionResult.CreateSuccessful(base.Mission, this._isEnemySideRetreating);
				flag = true;
			}
			else if (this._isPlayerSideRetreating || this._isPlayerSideDepleted)
			{
				missionResult = MissionResult.CreateDefeated(base.Mission);
				flag = true;
			}
			if (flag)
			{
				this._missionAgentSpawnLogic.StopSpawner(BattleSideEnum.Attacker);
				this._missionAgentSpawnLogic.StopSpawner(BattleSideEnum.Defender);
			}
			return flag;
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x0007FA80 File Offset: 0x0007DC80
		protected override void OnEndMission()
		{
			if (this._isEnemySideRetreating)
			{
				foreach (Agent agent in base.Mission.PlayerEnemyTeam.ActiveAgents)
				{
					bool flag = agent.GetMorale() < 0.01f;
					IAgentOriginBase origin = agent.Origin;
					if (origin != null)
					{
						origin.SetRouted(!flag);
					}
				}
			}
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x0007FB00 File Offset: 0x0007DD00
		public void ChangeCanCheckForEndCondition(bool canCheckForEndCondition)
		{
			this._canCheckForEndCondition = canCheckForEndCondition;
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x0007FB0C File Offset: 0x0007DD0C
		public BattleEndLogic.ExitResult TryExit()
		{
			if (GameNetwork.IsClientOrReplay)
			{
				return BattleEndLogic.ExitResult.False;
			}
			if (base.Mission.MissionEnded || (!this.PlayerVictory && !this.EnemyVictory))
			{
				Agent mainAgent = base.Mission.MainAgent;
				if (mainAgent == null || !mainAgent.IsActive() || !base.Mission.IsPlayerCloseToAnEnemy(5f))
				{
					if (base.Mission.MissionEnded || this._isEnemySideRetreating)
					{
						base.Mission.EndMission();
						return BattleEndLogic.ExitResult.True;
					}
					if (Mission.Current.IsSiegeBattle && base.Mission.PlayerTeam.IsDefender)
					{
						return BattleEndLogic.ExitResult.SurrenderSiege;
					}
					return BattleEndLogic.ExitResult.NeedsPlayerConfirmation;
				}
			}
			return BattleEndLogic.ExitResult.False;
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x0007FBAF File Offset: 0x0007DDAF
		public void EnableEnemyDefenderPullBack(int neededTroopNumber)
		{
			this._enemyDefenderPullbackEnabled = true;
			this._troopNumberNeededForEnemyDefenderPullBack = neededTroopNumber;
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x0007FBBF File Offset: 0x0007DDBF
		public void SetNotificationDisabled(bool value)
		{
			this._notificationsDisabled = value;
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x0007FBC8 File Offset: 0x0007DDC8
		private void CheckIsEnemySideRetreatingOrOneSideDepleted()
		{
			if (!this._canCheckForEndConditionSiege)
			{
				this._canCheckForEndConditionSiege = base.Mission.GetMissionBehavior<BattleDeploymentHandler>() == null;
				return;
			}
			if (this._canCheckForEndCondition)
			{
				BattleSideEnum side = base.Mission.PlayerTeam.Side;
				BattleSideEnum oppositeSide = side.GetOppositeSide();
				this._isPlayerSideDepleted = this._missionAgentSpawnLogic.IsSideDepleted(side);
				this._isEnemySideDepleted = this._missionAgentSpawnLogic.IsSideDepleted(oppositeSide);
				if (!this._isEnemySideDepleted && !this._isPlayerSideDepleted && base.Mission.GetMissionBehavior<HideoutPhasedMissionController>() == null)
				{
					float num = this._missionAgentSpawnLogic.GetReinforcementInterval(side) + 3f;
					if (base.Mission.MainAgent != null && base.Mission.MainAgent.IsPlayerControlled && base.Mission.MainAgent.IsActive())
					{
						this._playerSideNotYetRetreatingTime = MissionTime.Now;
					}
					else
					{
						bool flag = true;
						foreach (Team team in base.Mission.Teams)
						{
							if (team.IsFriendOf(base.Mission.PlayerTeam))
							{
								using (List<Agent>.Enumerator enumerator2 = team.ActiveAgents.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										if (!enumerator2.Current.IsRunningAway)
										{
											flag = false;
											break;
										}
									}
								}
							}
						}
						if (!flag)
						{
							this._playerSideNotYetRetreatingTime = MissionTime.Now;
						}
					}
					if (this._playerSideNotYetRetreatingTime.ElapsedSeconds > num)
					{
						this._isPlayerSideRetreating = true;
					}
					if (oppositeSide != BattleSideEnum.Defender || !this._enemyDefenderPullbackEnabled)
					{
						float num2 = this._missionAgentSpawnLogic.GetReinforcementInterval(oppositeSide) + 3f;
						bool flag2 = true;
						foreach (Team team2 in base.Mission.Teams)
						{
							if (team2.IsEnemyOf(base.Mission.PlayerTeam))
							{
								using (List<Agent>.Enumerator enumerator2 = team2.ActiveAgents.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										if (!enumerator2.Current.IsRunningAway)
										{
											flag2 = false;
											break;
										}
									}
								}
							}
						}
						if (!flag2)
						{
							this._enemySideNotYetRetreatingTime = MissionTime.Now;
						}
						if (this._enemySideNotYetRetreatingTime.ElapsedSeconds > num2)
						{
							this._isEnemySideRetreating = true;
						}
					}
				}
			}
		}

		// Token: 0x04000DBB RID: 3515
		private IMissionAgentSpawnLogic _missionAgentSpawnLogic;

		// Token: 0x04000DBC RID: 3516
		private MissionTime _enemySideNotYetRetreatingTime;

		// Token: 0x04000DBD RID: 3517
		private MissionTime _playerSideNotYetRetreatingTime;

		// Token: 0x04000DBE RID: 3518
		private BasicMissionTimer _checkRetreatingTimer;

		// Token: 0x04000DBF RID: 3519
		private bool _isEnemySideRetreating;

		// Token: 0x04000DC0 RID: 3520
		private bool _isPlayerSideRetreating;

		// Token: 0x04000DC1 RID: 3521
		private bool _isEnemySideDepleted;

		// Token: 0x04000DC2 RID: 3522
		private bool _isPlayerSideDepleted;

		// Token: 0x04000DC3 RID: 3523
		private bool _isEnemyDefenderPulledBack;

		// Token: 0x04000DC4 RID: 3524
		private bool _canCheckForEndCondition = true;

		// Token: 0x04000DC5 RID: 3525
		private bool _canCheckForEndConditionSiege;

		// Token: 0x04000DC6 RID: 3526
		private bool _enemyDefenderPullbackEnabled;

		// Token: 0x04000DC7 RID: 3527
		private int _troopNumberNeededForEnemyDefenderPullBack;

		// Token: 0x04000DC8 RID: 3528
		private bool _missionEndedMessageShown;

		// Token: 0x04000DC9 RID: 3529
		private bool _victoryReactionsActivated;

		// Token: 0x04000DCA RID: 3530
		private bool _victoryReactionsActivatedForRetreating;

		// Token: 0x04000DCB RID: 3531
		private bool _scoreBoardOpenedOnceOnMissionEnd;

		// Token: 0x0200055A RID: 1370
		public enum ExitResult
		{
			// Token: 0x04001E59 RID: 7769
			False,
			// Token: 0x04001E5A RID: 7770
			NeedsPlayerConfirmation,
			// Token: 0x04001E5B RID: 7771
			SurrenderSiege,
			// Token: 0x04001E5C RID: 7772
			True
		}
	}
}
