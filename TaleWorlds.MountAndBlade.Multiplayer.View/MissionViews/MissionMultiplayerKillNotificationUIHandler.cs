using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews
{
	// Token: 0x0200000B RID: 11
	public class MissionMultiplayerKillNotificationUIHandler : MissionView
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000020A0 File Offset: 0x000002A0
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			if (GameNetwork.IsDedicatedServer || !affectedAgent.IsHuman)
			{
				return;
			}
			string text = ((affectorAgent == null) ? string.Empty : ((affectorAgent.MissionPeer != null) ? affectorAgent.MissionPeer.DisplayedName : affectorAgent.Name));
			string text2 = ((affectedAgent.MissionPeer != null) ? affectedAgent.MissionPeer.DisplayedName : affectedAgent.Name);
			uint num = 4291306250U;
			MissionPeer missionPeer = null;
			if (GameNetwork.MyPeer != null)
			{
				missionPeer = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			}
			if (missionPeer != null && ((missionPeer.Team != base.Mission.SpectatorTeam && missionPeer.Team != affectedAgent.Team) || (affectorAgent != null && affectorAgent.MissionPeer == missionPeer)))
			{
				num = 4281589009U;
			}
			TextObject textObject;
			if (affectorAgent != null)
			{
				textObject = new TextObject("{=2ZarUUbw}{KILLERPLAYERNAME} has killed {KILLEDPLAYERNAME}!", null);
				textObject.SetTextVariable("KILLERPLAYERNAME", text);
			}
			else
			{
				textObject = new TextObject("{=9CnRKZOb}{KILLEDPLAYERNAME} has died!", null);
			}
			textObject.SetTextVariable("KILLEDPLAYERNAME", text2);
			MessageManager.DisplayMessage(textObject.ToString(), num);
		}
	}
}
