using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions.Sound.Components
{
	// Token: 0x02000030 RID: 48
	public class MusicArenaPracticeMissionView : MissionView, IMusicHandler
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0001214F File Offset: 0x0001034F
		bool IMusicHandler.IsPausable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00012152 File Offset: 0x00010352
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			MBMusicManager.Current.DeactivateCurrentMode();
			MBMusicManager.Current.ActivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerInit(this);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00012179 File Offset: 0x00010379
		public override void EarlyStart()
		{
			this._allOneShotSoundEventsAreDisabled = false;
			this._arenaSoundEntity = base.Mission.Scene.FindEntityWithTag("arena_sound");
			SoundManager.SetGlobalParameter("ArenaIntensity", 0f);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000121AC File Offset: 0x000103AC
		public override void OnMissionScreenFinalize()
		{
			SoundManager.SetGlobalParameter("ArenaIntensity", 0f);
			MBMusicManager.Current.DeactivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerFinalize();
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000121D4 File Offset: 0x000103D4
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectedAgent != null && affectorAgent != null && affectorAgent.IsMainAgent && (agentState == AgentState.Killed || agentState == AgentState.Unconscious))
			{
				if (affectedAgent.Team != affectorAgent.Team)
				{
					if (affectedAgent.IsHuman)
					{
						this._currentTournamentIntensity++;
						if (affectedAgent.HasMount)
						{
							this._currentTournamentIntensity++;
						}
						if (killingBlow.OverrideKillInfo == Agent.KillInfo.Headshot)
						{
							this._currentTournamentIntensity += 3;
						}
						if (killingBlow.IsMissile)
						{
							this._currentTournamentIntensity++;
						}
						else
						{
							this._currentTournamentIntensity += 2;
						}
					}
					else if (affectedAgent.RiderAgent != null)
					{
						this._currentTournamentIntensity += 3;
					}
				}
				this.UpdateAudienceIntensity();
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0001229C File Offset: 0x0001049C
		public override void OnMissionTick(float dt)
		{
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000122A0 File Offset: 0x000104A0
		public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
			if (affectorAgent != null && affectedAgent != null && affectorAgent.IsMainAgent && affectedAgent.IsHuman && affectedAgent.Position.Distance(affectorAgent.Position) >= 15f && (blow.VictimBodyPart == BoneBodyPartType.Head || blow.VictimBodyPart == BoneBodyPartType.Neck))
			{
				this._currentTournamentIntensity += 3;
				this.UpdateAudienceIntensity();
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00012305 File Offset: 0x00010505
		public override void OnMissileHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
		{
			if (!isCanceled && attacker != null && victim != null && attacker.IsMainAgent && victim.IsHuman && collisionData.IsShieldBroken)
			{
				this._currentTournamentIntensity += 2;
				this.UpdateAudienceIntensity();
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0001233D File Offset: 0x0001053D
		public override void OnMeleeHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
		{
			if (!isCanceled && attacker != null && victim != null && attacker.IsMainAgent && victim.IsHuman && collisionData.IsShieldBroken)
			{
				this._currentTournamentIntensity += 2;
				this.UpdateAudienceIntensity();
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00012378 File Offset: 0x00010578
		private void UpdateAudienceIntensity()
		{
			bool flag = false;
			if (this._currentTournamentIntensity > 60)
			{
				flag = this._currentArenaIntensityLevel != MusicArenaPracticeMissionView.ArenaIntensityLevel.High;
				this._currentArenaIntensityLevel = MusicArenaPracticeMissionView.ArenaIntensityLevel.High;
			}
			else if (this._currentTournamentIntensity > 30)
			{
				flag = this._currentArenaIntensityLevel != MusicArenaPracticeMissionView.ArenaIntensityLevel.Mid;
				this._currentArenaIntensityLevel = MusicArenaPracticeMissionView.ArenaIntensityLevel.Mid;
			}
			else if (this._currentTournamentIntensity <= 30)
			{
				flag = this._currentArenaIntensityLevel != MusicArenaPracticeMissionView.ArenaIntensityLevel.Low;
				this._currentArenaIntensityLevel = MusicArenaPracticeMissionView.ArenaIntensityLevel.Low;
			}
			if (flag)
			{
				SoundManager.SetGlobalParameter("ArenaIntensity", (float)this._currentArenaIntensityLevel);
			}
			if (!this._allOneShotSoundEventsAreDisabled)
			{
				this.Cheer();
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00012408 File Offset: 0x00010608
		private void Cheer()
		{
			string text = "event:/mission/ambient/arena/reaction";
			Vec3 globalPosition = this._arenaSoundEntity.GlobalPosition;
			SoundManager.StartOneShotEvent(text, in globalPosition);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0001242E File Offset: 0x0001062E
		public void OnUpdated(float dt)
		{
		}

		// Token: 0x040000CC RID: 204
		private const string ArenaSoundTag = "arena_sound";

		// Token: 0x040000CD RID: 205
		private const string ArenaIntensityParameterId = "ArenaIntensity";

		// Token: 0x040000CE RID: 206
		private const string ArenaPositiveReactionsSoundId = "event:/mission/ambient/arena/reaction";

		// Token: 0x040000CF RID: 207
		private const int MainAgentKnocksDownAnOpponentBaseIntensityChange = 1;

		// Token: 0x040000D0 RID: 208
		private const int MainAgentKnocksDownAnOpponentHeadShotIntensityChange = 3;

		// Token: 0x040000D1 RID: 209
		private const int MainAgentKnocksDownAnOpponentMountedTargetIntensityChange = 1;

		// Token: 0x040000D2 RID: 210
		private const int MainAgentKnocksDownAnOpponentRangedHitIntensityChange = 1;

		// Token: 0x040000D3 RID: 211
		private const int MainAgentKnocksDownAnOpponentMeleeHitIntensityChange = 2;

		// Token: 0x040000D4 RID: 212
		private const int MainAgentHeadShotFrom15MetersRangeIntensityChange = 3;

		// Token: 0x040000D5 RID: 213
		private const int MainAgentDismountsAnOpponentIntensityChange = 3;

		// Token: 0x040000D6 RID: 214
		private const int MainAgentBreaksAShieldIntensityChange = 2;

		// Token: 0x040000D7 RID: 215
		private int _currentTournamentIntensity;

		// Token: 0x040000D8 RID: 216
		private MusicArenaPracticeMissionView.ArenaIntensityLevel _currentArenaIntensityLevel;

		// Token: 0x040000D9 RID: 217
		private bool _allOneShotSoundEventsAreDisabled;

		// Token: 0x040000DA RID: 218
		private GameEntity _arenaSoundEntity;

		// Token: 0x020000A0 RID: 160
		private enum ArenaIntensityLevel
		{
			// Token: 0x04000324 RID: 804
			None,
			// Token: 0x04000325 RID: 805
			Low,
			// Token: 0x04000326 RID: 806
			Mid,
			// Token: 0x04000327 RID: 807
			High
		}
	}
}
