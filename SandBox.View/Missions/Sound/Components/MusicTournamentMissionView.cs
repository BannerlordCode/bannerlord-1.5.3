using System;
using psai.net;
using SandBox.Tournaments.MissionLogics;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions.Sound.Components
{
	// Token: 0x02000031 RID: 49
	public class MusicTournamentMissionView : MissionView, IMusicHandler
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00012438 File Offset: 0x00010638
		bool IMusicHandler.IsPausable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0001243C File Offset: 0x0001063C
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			MBMusicManager.Current.DeactivateCurrentMode();
			MBMusicManager.Current.ActivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerInit(this);
			this._startTimer = new Timer(Mission.Current.CurrentTime, 3f, true);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0001248C File Offset: 0x0001068C
		public override void EarlyStart()
		{
			this._allOneShotSoundEventsAreDisabled = false;
			this._tournamentBehavior = Mission.Current.GetMissionBehavior<TournamentBehavior>();
			this._currentMatch = null;
			this._lastMatch = null;
			this._arenaSoundEntity = base.Mission.Scene.FindEntityWithTag("arena_sound");
			SoundManager.SetGlobalParameter("ArenaIntensity", 0f);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000124E8 File Offset: 0x000106E8
		public override void OnMissionScreenFinalize()
		{
			SoundManager.SetGlobalParameter("ArenaIntensity", 0f);
			MBMusicManager.Current.DeactivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerFinalize();
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00012510 File Offset: 0x00010710
		private void CheckIntensityFall()
		{
			PsaiInfo psaiInfo = PsaiCore.Instance.GetPsaiInfo();
			if (psaiInfo.effectiveThemeId >= 0)
			{
				if (float.IsNaN(psaiInfo.currentIntensity))
				{
					MBMusicManager.Current.ChangeCurrentThemeIntensity(MusicParameters.MinIntensity);
					return;
				}
				if (psaiInfo.currentIntensity < MusicParameters.MinIntensity)
				{
					MBMusicManager.Current.ChangeCurrentThemeIntensity(MusicParameters.MinIntensity - psaiInfo.currentIntensity);
				}
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00012578 File Offset: 0x00010778
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (this._fightStarted)
			{
				bool flag = affectedAgent.IsMine || (affectedAgent.RiderAgent != null && affectedAgent.RiderAgent.IsMine);
				Team team = affectedAgent.Team;
				BattleSideEnum battleSideEnum = ((team != null) ? team.Side : BattleSideEnum.None);
				bool flag2;
				if (!flag)
				{
					if (battleSideEnum != BattleSideEnum.None)
					{
						Team playerTeam = Mission.Current.PlayerTeam;
						flag2 = ((playerTeam != null) ? playerTeam.Side : BattleSideEnum.None) == battleSideEnum;
					}
					else
					{
						flag2 = false;
					}
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				if ((affectedAgent.IsHuman && affectedAgent.State != AgentState.Routed) || flag)
				{
					float num = (flag3 ? MusicParameters.FriendlyTroopDeadEffectOnIntensity : MusicParameters.EnemyTroopDeadEffectOnIntensity);
					if (flag)
					{
						num *= MusicParameters.PlayerTroopDeadEffectMultiplierOnIntensity;
					}
					MBMusicManager.Current.ChangeCurrentThemeIntensity(num);
				}
			}
			if (affectedAgent != null && affectorAgent != null && affectorAgent.IsMainAgent && (agentState == AgentState.Killed || agentState == AgentState.Unconscious))
			{
				int num2 = 0;
				if (affectedAgent.Team == affectorAgent.Team)
				{
					if (affectedAgent.IsHuman)
					{
						num2 += -30;
					}
					else
					{
						num2 += -20;
					}
				}
				else if (affectedAgent.IsHuman)
				{
					num2++;
					if (affectedAgent.HasMount)
					{
						num2++;
					}
					if (killingBlow.OverrideKillInfo == Agent.KillInfo.Headshot)
					{
						num2 += 3;
					}
					if (killingBlow.IsMissile)
					{
						num2++;
					}
					else
					{
						num2 += 2;
					}
				}
				else if (affectedAgent.RiderAgent != null)
				{
					num2 += 3;
				}
				this.UpdateAudienceIntensity(num2, false);
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000126D8 File Offset: 0x000108D8
		void IMusicHandler.OnUpdated(float dt)
		{
			if (!this._fightStarted && Agent.Main != null && Agent.Main.IsActive() && this._startTimer.Check(Mission.Current.CurrentTime))
			{
				this._fightStarted = true;
				MBMusicManager.Current.StartTheme(MusicTheme.BattleSmall, 0.5f, false);
			}
			if (this._fightStarted)
			{
				this.CheckIntensityFall();
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00012740 File Offset: 0x00010940
		public override void OnMissionTick(float dt)
		{
			if (this._tournamentBehavior != null)
			{
				if (this._currentMatch != this._tournamentBehavior.CurrentMatch)
				{
					TournamentMatch currentMatch = this._tournamentBehavior.CurrentMatch;
					if (currentMatch != null && currentMatch.IsPlayerParticipating())
					{
						Agent main = Agent.Main;
						if (main != null && main.IsActive())
						{
							this._currentMatch = this._tournamentBehavior.CurrentMatch;
							this.OnTournamentRoundBegin(this._tournamentBehavior.NextRound == null);
						}
					}
				}
				if (this._lastMatch != this._tournamentBehavior.LastMatch)
				{
					this._lastMatch = this._tournamentBehavior.LastMatch;
					if (this._tournamentBehavior.NextRound == null || this._tournamentBehavior.LastMatch.IsPlayerParticipating())
					{
						this.OnTournamentRoundEnd();
					}
				}
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00012808 File Offset: 0x00010A08
		public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
			if (affectorAgent != null && affectedAgent != null && affectorAgent.IsMainAgent && affectedAgent.IsHuman && affectedAgent.Position.Distance(affectorAgent.Position) >= 15f && (blow.VictimBodyPart == BoneBodyPartType.Head || blow.VictimBodyPart == BoneBodyPartType.Neck))
			{
				this.UpdateAudienceIntensity(3, false);
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00012861 File Offset: 0x00010A61
		public override void OnMissileHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
		{
			if (!isCanceled && attacker != null && victim != null && attacker.IsMainAgent && victim.IsHuman && collisionData.IsShieldBroken)
			{
				this.UpdateAudienceIntensity(2, false);
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0001288D File Offset: 0x00010A8D
		public override void OnMeleeHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
		{
			if (!isCanceled && attacker != null && victim != null && attacker.IsMainAgent && victim.IsHuman && collisionData.IsShieldBroken)
			{
				this.UpdateAudienceIntensity(2, false);
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000128BC File Offset: 0x00010ABC
		private void UpdateAudienceIntensity(int intensityChangeAmount, bool isEnd = false)
		{
			MusicTournamentMissionView.ReactionType reactionType;
			if (!isEnd)
			{
				reactionType = ((intensityChangeAmount >= 0) ? MusicTournamentMissionView.ReactionType.Positive : MusicTournamentMissionView.ReactionType.Negative);
			}
			else
			{
				reactionType = MusicTournamentMissionView.ReactionType.End;
			}
			this._currentTournamentIntensity += intensityChangeAmount;
			bool flag = false;
			if (this._currentTournamentIntensity > 60)
			{
				flag = this._arenaIntensityLevel != MusicTournamentMissionView.ArenaIntensityLevel.High;
				this._arenaIntensityLevel = MusicTournamentMissionView.ArenaIntensityLevel.High;
			}
			else if (this._currentTournamentIntensity > 30)
			{
				flag = this._arenaIntensityLevel != MusicTournamentMissionView.ArenaIntensityLevel.Mid;
				this._arenaIntensityLevel = MusicTournamentMissionView.ArenaIntensityLevel.Mid;
			}
			else if (this._currentTournamentIntensity <= 30)
			{
				flag = this._arenaIntensityLevel != MusicTournamentMissionView.ArenaIntensityLevel.Low;
				this._arenaIntensityLevel = MusicTournamentMissionView.ArenaIntensityLevel.Low;
			}
			if (flag)
			{
				SoundManager.SetGlobalParameter("ArenaIntensity", (float)this._arenaIntensityLevel);
			}
			if (!this._allOneShotSoundEventsAreDisabled)
			{
				this.Cheer(reactionType);
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0001296C File Offset: 0x00010B6C
		private void Cheer(MusicTournamentMissionView.ReactionType reactionType)
		{
			string text = null;
			switch (reactionType)
			{
			case MusicTournamentMissionView.ReactionType.Positive:
				text = "event:/mission/ambient/arena/reaction";
				break;
			case MusicTournamentMissionView.ReactionType.Negative:
				text = "event:/mission/ambient/arena/negative_reaction";
				break;
			case MusicTournamentMissionView.ReactionType.End:
				text = "event:/mission/ambient/arena/reaction";
				break;
			}
			if (text != null)
			{
				string text2 = text;
				Vec3 globalPosition = this._arenaSoundEntity.GlobalPosition;
				SoundManager.StartOneShotEvent(text2, in globalPosition);
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000129BD File Offset: 0x00010BBD
		public void OnTournamentRoundBegin(bool isFinalRound)
		{
			this._isFinalRound = isFinalRound;
			this.UpdateAudienceIntensity(0, false);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000129D0 File Offset: 0x00010BD0
		public void OnTournamentRoundEnd()
		{
			int num = 10;
			if (this._lastMatch.IsPlayerWinner())
			{
				num += 10;
			}
			this.UpdateAudienceIntensity(num, this._isFinalRound);
		}

		// Token: 0x040000DB RID: 219
		private const string ArenaSoundTag = "arena_sound";

		// Token: 0x040000DC RID: 220
		private const string ArenaIntensityParameterId = "ArenaIntensity";

		// Token: 0x040000DD RID: 221
		private const string ArenaPositiveReactionsSoundId = "event:/mission/ambient/arena/reaction";

		// Token: 0x040000DE RID: 222
		private const string ArenaNegativeReactionsSoundId = "event:/mission/ambient/arena/negative_reaction";

		// Token: 0x040000DF RID: 223
		private const string ArenaTournamentEndSoundId = "event:/mission/ambient/arena/reaction";

		// Token: 0x040000E0 RID: 224
		private const int MainAgentKnocksDownAnOpponentBaseIntensityChange = 1;

		// Token: 0x040000E1 RID: 225
		private const int MainAgentKnocksDownAnOpponentHeadShotIntensityChange = 3;

		// Token: 0x040000E2 RID: 226
		private const int MainAgentKnocksDownAnOpponentMountedTargetIntensityChange = 1;

		// Token: 0x040000E3 RID: 227
		private const int MainAgentKnocksDownAnOpponentRangedHitIntensityChange = 1;

		// Token: 0x040000E4 RID: 228
		private const int MainAgentKnocksDownAnOpponentMeleeHitIntensityChange = 2;

		// Token: 0x040000E5 RID: 229
		private const int MainAgentHeadShotFrom15MetersRangeIntensityChange = 3;

		// Token: 0x040000E6 RID: 230
		private const int MainAgentDismountsAnOpponentIntensityChange = 3;

		// Token: 0x040000E7 RID: 231
		private const int MainAgentBreaksAShieldIntensityChange = 2;

		// Token: 0x040000E8 RID: 232
		private const int MainAgentWinsTournamentRoundIntensityChange = 10;

		// Token: 0x040000E9 RID: 233
		private const int RoundEndIntensityChange = 10;

		// Token: 0x040000EA RID: 234
		private const int MainAgentKnocksDownATeamMateIntensityChange = -30;

		// Token: 0x040000EB RID: 235
		private const int MainAgentKnocksDownAFriendlyHorseIntensityChange = -20;

		// Token: 0x040000EC RID: 236
		private int _currentTournamentIntensity;

		// Token: 0x040000ED RID: 237
		private MusicTournamentMissionView.ArenaIntensityLevel _arenaIntensityLevel;

		// Token: 0x040000EE RID: 238
		private bool _allOneShotSoundEventsAreDisabled;

		// Token: 0x040000EF RID: 239
		private TournamentBehavior _tournamentBehavior;

		// Token: 0x040000F0 RID: 240
		private TournamentMatch _currentMatch;

		// Token: 0x040000F1 RID: 241
		private TournamentMatch _lastMatch;

		// Token: 0x040000F2 RID: 242
		private GameEntity _arenaSoundEntity;

		// Token: 0x040000F3 RID: 243
		private bool _isFinalRound;

		// Token: 0x040000F4 RID: 244
		private bool _fightStarted;

		// Token: 0x040000F5 RID: 245
		private Timer _startTimer;

		// Token: 0x020000A1 RID: 161
		private enum ArenaIntensityLevel
		{
			// Token: 0x04000329 RID: 809
			None,
			// Token: 0x0400032A RID: 810
			Low,
			// Token: 0x0400032B RID: 811
			Mid,
			// Token: 0x0400032C RID: 812
			High
		}

		// Token: 0x020000A2 RID: 162
		private enum ReactionType
		{
			// Token: 0x0400032E RID: 814
			Positive,
			// Token: 0x0400032F RID: 815
			Negative,
			// Token: 0x04000330 RID: 816
			End
		}
	}
}
