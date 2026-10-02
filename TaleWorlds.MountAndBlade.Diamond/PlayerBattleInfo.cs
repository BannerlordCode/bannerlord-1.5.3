using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000142 RID: 322
	[Serializable]
	public class PlayerBattleInfo
	{
		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0000CE8C File Offset: 0x0000B08C
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x0000CE94 File Offset: 0x0000B094
		public PlayerId PlayerId { get; set; }

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x0000CE9D File Offset: 0x0000B09D
		// (set) Token: 0x060008B6 RID: 2230 RVA: 0x0000CEA5 File Offset: 0x0000B0A5
		public string Name { get; set; }

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x0000CEAE File Offset: 0x0000B0AE
		// (set) Token: 0x060008B8 RID: 2232 RVA: 0x0000CEB6 File Offset: 0x0000B0B6
		public int TeamNo { get; set; }

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x0000CEBF File Offset: 0x0000B0BF
		public bool Fled
		{
			get
			{
				return this._state == PlayerBattleInfo.State.Fled;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x0000CECA File Offset: 0x0000B0CA
		public bool Disconnected
		{
			get
			{
				return this._state == PlayerBattleInfo.State.Disconnected;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x0000CED5 File Offset: 0x0000B0D5
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x0000CEDD File Offset: 0x0000B0DD
		public BattleJoinType JoinType { get; set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0000CEE6 File Offset: 0x0000B0E6
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x0000CEEE File Offset: 0x0000B0EE
		public bool IsSpectator { get; set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x0000CEF7 File Offset: 0x0000B0F7
		// (set) Token: 0x060008C0 RID: 2240 RVA: 0x0000CEFF File Offset: 0x0000B0FF
		public int PeerIndex { get; set; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0000CF08 File Offset: 0x0000B108
		public PlayerBattleInfo.State CurrentState
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0000CF10 File Offset: 0x0000B110
		public PlayerBattleInfo()
		{
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0000CF18 File Offset: 0x0000B118
		public PlayerBattleInfo(PlayerId playerId, string name, int teamNo, bool isSpectator)
		{
			this.PlayerId = playerId;
			this.Name = name;
			this.TeamNo = teamNo;
			this.IsSpectator = isSpectator;
			this.PeerIndex = -1;
			this._state = PlayerBattleInfo.State.AssignedToBattle;
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0000CF4B File Offset: 0x0000B14B
		public PlayerBattleInfo(PlayerId playerId, string name, int teamNo, int peerIndex, PlayerBattleInfo.State state, bool isSpectator)
		{
			this.PlayerId = playerId;
			this.Name = name;
			this.TeamNo = teamNo;
			this.IsSpectator = isSpectator;
			this.PeerIndex = peerIndex;
			this._state = state;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0000CF80 File Offset: 0x0000B180
		public void Flee()
		{
			if (this._state != PlayerBattleInfo.State.Disconnected && this._state != PlayerBattleInfo.State.AtBattle)
			{
				throw new Exception("PlayerBattleInfo incorrect state, expected AtBattle or Disconnected; got " + this._state);
			}
			this._state = PlayerBattleInfo.State.Fled;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0000CFB6 File Offset: 0x0000B1B6
		public void Disconnect()
		{
			if (this._state != PlayerBattleInfo.State.AtBattle)
			{
				throw new Exception("PlayerBattleInfo incorrect state, expected AtBattle got " + this._state);
			}
			this._state = PlayerBattleInfo.State.Disconnected;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0000CFE3 File Offset: 0x0000B1E3
		public void Initialize(int peerIndex)
		{
			if (this._state != PlayerBattleInfo.State.AssignedToBattle)
			{
				throw new Exception("PlayerBattleInfo incorrect state, expected AssignedToBattle got " + this._state);
			}
			this.PeerIndex = peerIndex;
			this._state = PlayerBattleInfo.State.AtBattle;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0000D017 File Offset: 0x0000B217
		public void RejoinBattle(int teamNo)
		{
			if (this._state != PlayerBattleInfo.State.Disconnected)
			{
				throw new Exception("PlayerBattleInfo incorrect state, expected Disconnected got " + this._state);
			}
			this.TeamNo = teamNo;
			this.PeerIndex = -1;
			this._state = PlayerBattleInfo.State.AssignedToBattle;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0000D052 File Offset: 0x0000B252
		public PlayerBattleInfo Clone()
		{
			return new PlayerBattleInfo(this.PlayerId, this.Name, this.TeamNo, this.PeerIndex, this._state, this.IsSpectator);
		}

		// Token: 0x040003CC RID: 972
		public const int SpectatorTeamNo = -1;

		// Token: 0x040003D3 RID: 979
		private PlayerBattleInfo.State _state;

		// Token: 0x020001DA RID: 474
		public enum State
		{
			// Token: 0x04000727 RID: 1831
			Created,
			// Token: 0x04000728 RID: 1832
			AssignedToBattle,
			// Token: 0x04000729 RID: 1833
			AtBattle,
			// Token: 0x0400072A RID: 1834
			Disconnected,
			// Token: 0x0400072B RID: 1835
			Fled
		}
	}
}
