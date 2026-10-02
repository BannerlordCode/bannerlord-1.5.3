using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000320 RID: 800
	public static class SpectatorHelper
	{
		// Token: 0x06002DDC RID: 11740 RVA: 0x000B2770 File Offset: 0x000B0970
		public static bool IsPeerSpectator(NetworkCommunicator networkPeer)
		{
			if (networkPeer == null)
			{
				return false;
			}
			if (networkPeer.IsSpectator)
			{
				return true;
			}
			Mission mission = Mission.Current;
			Team team = ((mission != null) ? mission.SpectatorTeam : null);
			if (team == null)
			{
				return false;
			}
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			return component != null && component.Team == team;
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x000B27B8 File Offset: 0x000B09B8
		public static bool IsLocalPeerSpectator()
		{
			return SpectatorHelper.IsPeerSpectator(GameNetwork.MyPeer);
		}
	}
}
