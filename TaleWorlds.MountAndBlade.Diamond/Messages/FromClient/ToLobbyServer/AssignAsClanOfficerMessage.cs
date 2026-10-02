using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000079 RID: 121
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class AssignAsClanOfficerMessage : Message
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000262 RID: 610 RVA: 0x00003AAD File Offset: 0x00001CAD
		// (set) Token: 0x06000263 RID: 611 RVA: 0x00003AB5 File Offset: 0x00001CB5
		[JsonProperty]
		public PlayerId AssignedPlayerId { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00003ABE File Offset: 0x00001CBE
		// (set) Token: 0x06000265 RID: 613 RVA: 0x00003AC6 File Offset: 0x00001CC6
		[JsonProperty]
		public bool DontUseNameForUnknownPlayer { get; private set; }

		// Token: 0x06000266 RID: 614 RVA: 0x00003ACF File Offset: 0x00001CCF
		public AssignAsClanOfficerMessage()
		{
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00003AD7 File Offset: 0x00001CD7
		public AssignAsClanOfficerMessage(PlayerId assignedPlayerId, bool dontUseNameForUnknownPlayer)
		{
			this.AssignedPlayerId = assignedPlayerId;
			this.DontUseNameForUnknownPlayer = dontUseNameForUnknownPlayer;
		}
	}
}
