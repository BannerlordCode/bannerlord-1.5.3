using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200005E RID: 94
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PlayersAddedToPartyMessage : Message
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000354C File Offset: 0x0000174C
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x00003554 File Offset: 0x00001754
		[TupleElementNames(new string[] { "PlayerId", "PlayerName", "IsPartyLeader" })]
		[JsonProperty]
		public List<ValueTuple<PlayerId, string, bool>> Players
		{
			[return: TupleElementNames(new string[] { "PlayerId", "PlayerName", "IsPartyLeader" })]
			get;
			[param: TupleElementNames(new string[] { "PlayerId", "PlayerName", "IsPartyLeader" })]
			private set;
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000355D File Offset: 0x0000175D
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x00003565 File Offset: 0x00001765
		[TupleElementNames(new string[] { "PlayerId", "PlayerName" })]
		[JsonProperty]
		public List<ValueTuple<PlayerId, string>> InvitedPlayers
		{
			[return: TupleElementNames(new string[] { "PlayerId", "PlayerName" })]
			get;
			[param: TupleElementNames(new string[] { "PlayerId", "PlayerName" })]
			private set;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000356E File Offset: 0x0000176E
		public PlayersAddedToPartyMessage()
		{
			this.Players = new List<ValueTuple<PlayerId, string, bool>>();
			this.InvitedPlayers = new List<ValueTuple<PlayerId, string>>();
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000358C File Offset: 0x0000178C
		public PlayersAddedToPartyMessage(PlayerId playerId, string playerName, bool isPartyLeader)
			: this()
		{
			this.AddPlayer(playerId, playerName, isPartyLeader);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000359D File Offset: 0x0000179D
		public void AddPlayer(PlayerId playerId, string playerName, bool isPartyLeader)
		{
			this.Players.Add(new ValueTuple<PlayerId, string, bool>(playerId, playerName, isPartyLeader));
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000035B2 File Offset: 0x000017B2
		public void AddInvitedPlayer(PlayerId playerId, string playerName)
		{
			this.InvitedPlayers.Add(new ValueTuple<PlayerId, string>(playerId, playerName));
		}
	}
}
