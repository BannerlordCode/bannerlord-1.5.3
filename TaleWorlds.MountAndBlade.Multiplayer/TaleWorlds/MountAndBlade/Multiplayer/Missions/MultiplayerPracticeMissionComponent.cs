using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.Missions
{
	// Token: 0x02000068 RID: 104
	public class MultiplayerPracticeMissionComponent : MissionLogic
	{
		// Token: 0x06000332 RID: 818 RVA: 0x0000E9ED File Offset: 0x0000CBED
		public override void AfterStart()
		{
			base.AfterStart();
			this._lobbyClient = NetworkMain.GameClient;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000EA00 File Offset: 0x0000CC00
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			this._lastMessagePrintPassedTime += dt;
			if (this._shutDownMissionTriggered)
			{
				this._shutDownMissionTimer += dt;
				if (this._shutDownMissionTimer >= 1f)
				{
					this._shutDownMissionTimer -= 1f;
					this._shutDownMissionCount++;
					if (this._shutDownMissionCount >= 3)
					{
						base.Mission.EndMission();
						return;
					}
					this.InformMissionDuration();
					return;
				}
			}
			else if (this._lobbyClient.CurrentState == LobbyClient.State.SearchingBattle)
			{
				if (this._lastMessagePrintPassedTime > 5f)
				{
					InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=MrEhLbht}Still searching for a battle...", null).ToString()));
					this._lastMessagePrintPassedTime = 0f;
					return;
				}
			}
			else if (this._lobbyClient.CurrentState == LobbyClient.State.AtBattle && !this._shutDownMissionTriggered)
			{
				this._shutDownMissionTriggered = true;
				InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=BN1Pmhho}Found a battle by matchmaker!", null).ToString()));
				this.InformMissionDuration();
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000EB04 File Offset: 0x0000CD04
		private void InformMissionDuration()
		{
			int num = 3 - this._shutDownMissionCount;
			TextObject textObject = new TextObject("{=aNMmlya4}Shutting down mission in {REMAINING_SECONDS_TO_SHUT_DOWN_MISSION} seconds!", null);
			textObject.SetTextVariable("REMAINING_SECONDS_TO_SHUT_DOWN_MISSION", num.ToString());
			InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
		}

		// Token: 0x040000F9 RID: 249
		private LobbyClient _lobbyClient;

		// Token: 0x040000FA RID: 250
		private float _lastMessagePrintPassedTime;

		// Token: 0x040000FB RID: 251
		private bool _shutDownMissionTriggered;

		// Token: 0x040000FC RID: 252
		private float _shutDownMissionTimer;

		// Token: 0x040000FD RID: 253
		private int _shutDownMissionCount;

		// Token: 0x040000FE RID: 254
		private const int ShutDownDurationInSeconds = 3;
	}
}
