using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions
{
	// Token: 0x02000061 RID: 97
	public class StealthFailCounterMissionLogic : MissionLogic
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00015F1A File Offset: 0x0001411A
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x00015F22 File Offset: 0x00014122
		public bool IsActive { get; private set; } = true;

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00015F2B File Offset: 0x0001412B
		public float FailCounterElapsedTime
		{
			get
			{
				if (!this.IsActive || this._failCounter == null)
				{
					return -1f;
				}
				return this._failCounter.ElapsedTime();
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00015F50 File Offset: 0x00014150
		public override void OnAgentAlarmedStateChanged(Agent agent, Agent.AIStateFlag flag)
		{
			base.OnAgentAlarmedStateChanged(agent, flag);
			if (agent.Team != null && !agent.Team.IsPlayerAlly)
			{
				if (agent.IsAlarmed() && !this._alarmedAgents.Contains(agent))
				{
					this._alarmedAgents.Add(agent);
				}
				else if (!agent.IsAlarmed() && this._alarmedAgents.Contains(agent))
				{
					this._alarmedAgents.Remove(agent);
				}
				if (this._alarmedAgents.Count == 0)
				{
					this.IsActive = false;
					return;
				}
				this.IsActive = true;
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00015FDD File Offset: 0x000141DD
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
			this._alarmedAgents.Remove(affectedAgent);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00015FF8 File Offset: 0x000141F8
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this.IsActive)
			{
				if (base.Mission.Mode == MissionMode.Stealth)
				{
					if (this._failCounter == null && !this._alarmedAgents.IsEmpty<Agent>())
					{
						this._failCounter = new Timer(base.Mission.CurrentTime, this.FailCounterSeconds, true);
					}
					if (this._failCounter != null)
					{
						if (this._alarmedAgents.IsEmpty<Agent>())
						{
							this._failCounter = null;
							return;
						}
						if (this._failCounter.Check(base.Mission.CurrentTime))
						{
							this.ShowMissionFailedPopup();
							return;
						}
					}
				}
				else
				{
					this.IsActive = false;
				}
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0001609A File Offset: 0x0001429A
		public void SetFailTexts(TextObject title, TextObject description)
		{
			this._popupTitle = title;
			this._popupDescription = description;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x000160AC File Offset: 0x000142AC
		private void ShowMissionFailedPopup()
		{
			this.IsActive = false;
			object obj = (TextObject.IsNullOrEmpty(this._popupTitle) ? new TextObject("{=wQbfWNZO}Mission Failed!", null) : this._popupTitle);
			TextObject textObject = (TextObject.IsNullOrEmpty(this._popupDescription) ? new TextObject("{=5R0TauYV}You have been compromised.", null) : this._popupDescription);
			TextObject textObject2 = new TextObject("{=DM6luo3c}Continue", null);
			InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, false, textObject2.ToString(), null, delegate
			{
				Game.Current.EventManager.TriggerEvent<OnStealthMissionCounterFailedEvent>(new OnStealthMissionCounterFailedEvent());
				Mission.Current.EndMission();
			}, null, "", 0f, null, null, null), Campaign.Current.GameMode == CampaignGameMode.Campaign, false);
		}

		// Token: 0x040001FD RID: 509
		private readonly List<Agent> _alarmedAgents = new List<Agent>();

		// Token: 0x040001FE RID: 510
		private Timer _failCounter;

		// Token: 0x040001FF RID: 511
		public float FailCounterSeconds = 5f;

		// Token: 0x04000201 RID: 513
		private TextObject _popupTitle;

		// Token: 0x04000202 RID: 514
		private TextObject _popupDescription;
	}
}
