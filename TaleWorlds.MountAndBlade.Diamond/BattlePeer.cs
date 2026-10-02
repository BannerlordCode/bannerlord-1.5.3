using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F0 RID: 240
	public class BattlePeer
	{
		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x000054D0 File Offset: 0x000036D0
		// (set) Token: 0x060004A6 RID: 1190 RVA: 0x000054D8 File Offset: 0x000036D8
		public int Index { get; private set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x000054E1 File Offset: 0x000036E1
		// (set) Token: 0x060004A8 RID: 1192 RVA: 0x000054E9 File Offset: 0x000036E9
		public string Name { get; private set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x000054F2 File Offset: 0x000036F2
		public PlayerId PlayerId
		{
			get
			{
				return this.PlayerData.PlayerId;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x000054FF File Offset: 0x000036FF
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x00005507 File Offset: 0x00003707
		public int TeamNo { get; private set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00005510 File Offset: 0x00003710
		// (set) Token: 0x060004AD RID: 1197 RVA: 0x00005518 File Offset: 0x00003718
		public BattleJoinType BattleJoinType { get; private set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00005521 File Offset: 0x00003721
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x00005529 File Offset: 0x00003729
		public bool IsSpectator { get; private set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00005532 File Offset: 0x00003732
		public bool Quit
		{
			get
			{
				return this.QuitType > BattlePeerQuitType.None;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0000553D File Offset: 0x0000373D
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x00005545 File Offset: 0x00003745
		public PlayerData PlayerData { get; private set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0000554E File Offset: 0x0000374E
		// (set) Token: 0x060004B4 RID: 1204 RVA: 0x00005556 File Offset: 0x00003756
		public Dictionary<string, List<string>> UsedCosmetics { get; private set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0000555F File Offset: 0x0000375F
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x00005567 File Offset: 0x00003767
		public int SessionKey { get; private set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00005570 File Offset: 0x00003770
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x00005578 File Offset: 0x00003778
		public BattlePeerQuitType QuitType { get; private set; }

		// Token: 0x060004B9 RID: 1209 RVA: 0x00005581 File Offset: 0x00003781
		public BattlePeer(string name, PlayerData playerData, Dictionary<string, List<string>> usedCosmetics, int teamNo, BattleJoinType battleJoinType, bool isSpectator = false)
		{
			this.Index = -1;
			this.Name = name;
			this.PlayerData = playerData;
			this.UsedCosmetics = usedCosmetics;
			this.TeamNo = teamNo;
			this.BattleJoinType = battleJoinType;
			this.IsSpectator = isSpectator;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000055BD File Offset: 0x000037BD
		internal void Flee()
		{
			this.QuitType = BattlePeerQuitType.Fled;
			this.Index = -1;
			this.SessionKey = 0;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000055D4 File Offset: 0x000037D4
		internal void SetPlayerDisconnectdFromLobby()
		{
			this.QuitType = BattlePeerQuitType.DisconnectedFromLobby;
			this.Index = -1;
			this.SessionKey = 0;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x000055EB File Offset: 0x000037EB
		internal void SetPlayerDisconnectdFromGameSession()
		{
			this.QuitType = BattlePeerQuitType.DisconnectedFromGameSession;
			this.Index = -1;
			this.SessionKey = 0;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00005602 File Offset: 0x00003802
		public void Rejoin(int teamNo)
		{
			this.QuitType = BattlePeerQuitType.None;
			this.TeamNo = teamNo;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00005612 File Offset: 0x00003812
		public void InitializeSession(int index, int sessionKey)
		{
			this.Index = index;
			this.SessionKey = sessionKey;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00005622 File Offset: 0x00003822
		internal void SetPlayerKickedDueToFriendlyDamage()
		{
			this.QuitType = BattlePeerQuitType.KickedDueToFriendlyDamage;
			this.Index = -1;
			this.SessionKey = 0;
		}
	}
}
