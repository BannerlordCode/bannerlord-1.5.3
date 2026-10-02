using System;
using System.Collections.Generic;
using SandBox.Tournaments.MissionLogics;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000081 RID: 129
	public class SandboxHighlightsController : MissionLogic
	{
		// Token: 0x0600052D RID: 1325 RVA: 0x00022DCC File Offset: 0x00020FCC
		public override void AfterStart()
		{
			this._highlightsController = Mission.Current.GetMissionBehavior<HighlightsController>();
			foreach (HighlightsController.HighlightType highlightType in this._highlightTypes)
			{
				HighlightsController.AddHighlightType(highlightType);
			}
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00022E2C File Offset: 0x0002102C
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectorAgent != null && affectorAgent.IsMainAgent && affectedAgent != null && affectedAgent.IsHuman)
			{
				TournamentBehavior missionBehavior = Mission.Current.GetMissionBehavior<TournamentBehavior>();
				if (missionBehavior != null && missionBehavior.CurrentMatch != null && missionBehavior.NextRound == null)
				{
					foreach (TournamentParticipant tournamentParticipant in missionBehavior.CurrentMatch.Participants)
					{
						if (affectorAgent.Character == tournamentParticipant.Character && affectedAgent.Character != tournamentParticipant.Character)
						{
							HighlightsController.Highlight highlight = default(HighlightsController.Highlight);
							highlight.Start = Mission.Current.CurrentTime;
							highlight.End = Mission.Current.CurrentTime;
							highlight.HighlightType = this._highlightsController.GetHighlightTypeWithId("hlid_tournament_last_match_kill");
							this._highlightsController.SaveHighlight(highlight, affectedAgent.Position);
							break;
						}
					}
				}
			}
		}

		// Token: 0x040002C2 RID: 706
		private List<HighlightsController.HighlightType> _highlightTypes = new List<HighlightsController.HighlightType>
		{
			new HighlightsController.HighlightType("hlid_tournament_last_match_kill", "Champion of the Arena", "grpid_incidents", -5000, 3000, 0f, float.MaxValue, true)
		};

		// Token: 0x040002C3 RID: 707
		private HighlightsController _highlightsController;
	}
}
