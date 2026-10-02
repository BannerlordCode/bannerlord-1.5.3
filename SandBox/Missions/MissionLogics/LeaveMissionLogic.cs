using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000070 RID: 112
	public class LeaveMissionLogic : MissionLogic
	{
		// Token: 0x06000480 RID: 1152 RVA: 0x0001B2C5 File Offset: 0x000194C5
		public LeaveMissionLogic(string leaveMenuId = "settlement_player_unconscious")
		{
			this._menuId = leaveMenuId;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001B2D4 File Offset: 0x000194D4
		public override bool MissionEnded(ref MissionResult missionResult)
		{
			return base.Mission.MainAgent != null && !base.Mission.MainAgent.IsActive();
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0001B2F8 File Offset: 0x000194F8
		public override void OnMissionTick(float dt)
		{
			if (Agent.Main == null || !Agent.Main.IsActive())
			{
				if (this._isAgentDeadTimer == null)
				{
					this._isAgentDeadTimer = new Timer(Mission.Current.CurrentTime, 5f, true);
				}
				if (this._isAgentDeadTimer.Check(Mission.Current.CurrentTime))
				{
					Mission.Current.NextCheckTimeEndMission = 0f;
					Mission.Current.EndMission();
					Campaign.Current.GameMenuManager.SetNextMenu(this._menuId);
					return;
				}
			}
			else if (this._isAgentDeadTimer != null)
			{
				this._isAgentDeadTimer = null;
			}
		}

		// Token: 0x04000268 RID: 616
		private string _menuId;

		// Token: 0x04000269 RID: 617
		private Timer _isAgentDeadTimer;
	}
}
