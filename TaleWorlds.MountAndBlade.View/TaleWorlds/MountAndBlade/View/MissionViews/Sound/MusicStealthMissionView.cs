using System;
using System.Collections.Generic;
using psai.net;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Sound
{
	// Token: 0x02000087 RID: 135
	public class MusicStealthMissionView : MissionView, IMusicHandler
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x00026AFE File Offset: 0x00024CFE
		bool IMusicHandler.IsPausable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00026B04 File Offset: 0x00024D04
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._cautiousAgents = new List<Agent>();
			this._patrollingCautiousAgents = new List<Agent>();
			this._detectedAgents = new List<Agent>();
			this._combatAgents = new List<Agent>();
			this._stealthNotificationSoundEvents = new Dictionary<Agent, SoundEvent>();
			MBMusicManager.Current.DeactivateCurrentMode();
			MBMusicManager.Current.ActivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerInit(this);
			this._warningSoundState = MusicStealthMissionView.WarningSoundState.None;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00026B74 File Offset: 0x00024D74
		public override void OnMissionScreenFinalize()
		{
			MBMusicManager.Current.DeactivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerFinalize();
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00026B8A File Offset: 0x00024D8A
		void IMusicHandler.OnUpdated(float dt)
		{
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00026B8C File Offset: 0x00024D8C
		public override void AfterStart()
		{
			base.AfterStart();
			MBMusicManager.Current.StartTheme(MusicTheme.StealthA, 0.25f, false);
			PsaiCore.Instance.HoldCurrentIntensity(true);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00026BB4 File Offset: 0x00024DB4
		public override void OnAgentAlarmedStateChanged(Agent agent, Agent.AIStateFlag flag)
		{
			object lockObject = MusicStealthMissionView._lockObject;
			lock (lockObject)
			{
				this._cautiousAgents.Remove(agent);
				this._patrollingCautiousAgents.Remove(agent);
				this._detectedAgents.Remove(agent);
				switch (flag)
				{
				case Agent.AIStateFlag.Cautious:
					this._cautiousAgents.Add(agent);
					break;
				case Agent.AIStateFlag.PatrollingCautious:
					this._patrollingCautiousAgents.Add(agent);
					break;
				case Agent.AIStateFlag.Alarmed:
					this._detectedAgents.Add(agent);
					break;
				}
				this.CheckIntensityChange();
				this.CheckWarningSoundStateChange(agent);
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00026C64 File Offset: 0x00024E64
		public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
			base.OnAgentHit(affectedAgent, affectorAgent, in affectorWeapon, in blow, in attackCollisionData);
			if (affectedAgent.IsMainAgent && affectorAgent != null && affectorAgent != affectedAgent && affectorAgent.IsEnemyOf(Agent.Main) && !this._combatAgents.Contains(affectorAgent))
			{
				this._combatAgents.Add(affectorAgent);
				this.CheckIntensityChange();
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00026CBC File Offset: 0x00024EBC
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
			this._cautiousAgents.Remove(affectedAgent);
			this._patrollingCautiousAgents.Remove(affectedAgent);
			this._detectedAgents.Remove(affectedAgent);
			this._combatAgents.Remove(affectedAgent);
			this.CheckIntensityChange();
			this.CheckWarningSoundStateChange(affectedAgent);
			if (this._stealthNotificationSoundEvents.ContainsKey(affectedAgent) && !affectedAgent.IsActive())
			{
				this._stealthNotificationSoundEvents.Remove(affectedAgent);
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00026D38 File Offset: 0x00024F38
		private void CheckIntensityChange()
		{
			float num;
			if (this._combatAgents.Count > 0)
			{
				num = 1f;
			}
			else if (this._detectedAgents.Count > 0)
			{
				num = 0.75f;
			}
			else if (this._cautiousAgents.Count > 0 || this._patrollingCautiousAgents.Count > 0)
			{
				num = 0.5f;
			}
			else
			{
				num = 0.25f;
			}
			float num2 = num - PsaiCore.Instance.GetCurrentIntensity();
			if (Math.Abs(num2) > 1E-05f)
			{
				PsaiCore.Instance.HoldCurrentIntensity(false);
				PsaiCore.Instance.AddToCurrentIntensity(num2);
				PsaiCore.Instance.HoldCurrentIntensity(true);
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00026DDC File Offset: 0x00024FDC
		private void CheckWarningSoundStateChange(Agent relatedAgent)
		{
			MusicStealthMissionView.WarningSoundState intendedWarningSoundState = this.GetIntendedWarningSoundState();
			if (intendedWarningSoundState != this._warningSoundState)
			{
				this.ChangeWarningSoundState(intendedWarningSoundState, relatedAgent);
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00026E01 File Offset: 0x00025001
		private MusicStealthMissionView.WarningSoundState GetIntendedWarningSoundState()
		{
			if (this._detectedAgents.Count > 0)
			{
				return MusicStealthMissionView.WarningSoundState.Alarmed;
			}
			if (this._patrollingCautiousAgents.Count > 0)
			{
				return MusicStealthMissionView.WarningSoundState.PatrollingCautious;
			}
			if (this._cautiousAgents.Count > 0)
			{
				return MusicStealthMissionView.WarningSoundState.Cautious;
			}
			return MusicStealthMissionView.WarningSoundState.Neutral;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00026E34 File Offset: 0x00025034
		private void ChangeWarningSoundState(MusicStealthMissionView.WarningSoundState newState, Agent relatedAgent)
		{
			SoundEvent soundEvent;
			if (!this._stealthNotificationSoundEvents.TryGetValue(relatedAgent, out soundEvent))
			{
				soundEvent = SoundEvent.CreateEventFromString("event:/ui/stealth/stealth_notification_b", base.Mission.Scene);
				this._stealthNotificationSoundEvents.Add(relatedAgent, soundEvent);
			}
			this._warningSoundState = newState;
			soundEvent.SetPosition(relatedAgent.Position);
			float num = (float)this._warningSoundState;
			soundEvent.SetParameter("agent_state", num);
			if (!soundEvent.IsPlaying())
			{
				soundEvent.Play();
			}
		}

		// Token: 0x040002F0 RID: 752
		private const string StealthNotificationSoundEventId = "event:/ui/stealth/stealth_notification_b";

		// Token: 0x040002F1 RID: 753
		private const string AgentStateParameterName = "agent_state";

		// Token: 0x040002F2 RID: 754
		private static object _lockObject = new object();

		// Token: 0x040002F3 RID: 755
		private List<Agent> _cautiousAgents;

		// Token: 0x040002F4 RID: 756
		private List<Agent> _patrollingCautiousAgents;

		// Token: 0x040002F5 RID: 757
		private List<Agent> _detectedAgents;

		// Token: 0x040002F6 RID: 758
		private List<Agent> _combatAgents;

		// Token: 0x040002F7 RID: 759
		private Dictionary<Agent, SoundEvent> _stealthNotificationSoundEvents;

		// Token: 0x040002F8 RID: 760
		private MusicStealthMissionView.WarningSoundState _warningSoundState;

		// Token: 0x020000E8 RID: 232
		private enum WarningSoundState
		{
			// Token: 0x04000408 RID: 1032
			None,
			// Token: 0x04000409 RID: 1033
			Neutral,
			// Token: 0x0400040A RID: 1034
			Cautious,
			// Token: 0x0400040B RID: 1035
			PatrollingCautious,
			// Token: 0x0400040C RID: 1036
			Alarmed
		}
	}
}
