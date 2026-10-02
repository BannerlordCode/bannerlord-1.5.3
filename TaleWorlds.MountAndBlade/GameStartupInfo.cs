using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200023E RID: 574
	public class GameStartupInfo
	{
		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002172 RID: 8562 RVA: 0x00076D68 File Offset: 0x00074F68
		// (set) Token: 0x06002173 RID: 8563 RVA: 0x00076D70 File Offset: 0x00074F70
		public GameStartupType StartupType { get; internal set; }

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002174 RID: 8564 RVA: 0x00076D79 File Offset: 0x00074F79
		// (set) Token: 0x06002175 RID: 8565 RVA: 0x00076D81 File Offset: 0x00074F81
		public DedicatedServerType DedicatedServerType { get; internal set; }

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002176 RID: 8566 RVA: 0x00076D8A File Offset: 0x00074F8A
		// (set) Token: 0x06002177 RID: 8567 RVA: 0x00076D92 File Offset: 0x00074F92
		public bool PlayerHostedDedicatedServer { get; internal set; }

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x00076D9B File Offset: 0x00074F9B
		// (set) Token: 0x06002179 RID: 8569 RVA: 0x00076DA3 File Offset: 0x00074FA3
		public bool IsSinglePlatformServer { get; internal set; }

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x00076DAC File Offset: 0x00074FAC
		// (set) Token: 0x0600217B RID: 8571 RVA: 0x00076DB4 File Offset: 0x00074FB4
		public string CustomServerHostIP { get; internal set; } = string.Empty;

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x00076DBD File Offset: 0x00074FBD
		// (set) Token: 0x0600217D RID: 8573 RVA: 0x00076DC5 File Offset: 0x00074FC5
		public int ServerPort { get; internal set; }

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x0600217E RID: 8574 RVA: 0x00076DCE File Offset: 0x00074FCE
		// (set) Token: 0x0600217F RID: 8575 RVA: 0x00076DD6 File Offset: 0x00074FD6
		public string ServerRegion { get; internal set; }

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x00076DDF File Offset: 0x00074FDF
		// (set) Token: 0x06002181 RID: 8577 RVA: 0x00076DE7 File Offset: 0x00074FE7
		public sbyte ServerPriority { get; internal set; }

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002182 RID: 8578 RVA: 0x00076DF0 File Offset: 0x00074FF0
		// (set) Token: 0x06002183 RID: 8579 RVA: 0x00076DF8 File Offset: 0x00074FF8
		public string ServerGameMode { get; internal set; }

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x00076E01 File Offset: 0x00075001
		// (set) Token: 0x06002185 RID: 8581 RVA: 0x00076E09 File Offset: 0x00075009
		public string CustomGameServerConfigFile { get; internal set; }

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x00076E12 File Offset: 0x00075012
		// (set) Token: 0x06002187 RID: 8583 RVA: 0x00076E1A File Offset: 0x0007501A
		public string CustomGameServerNameOverride { get; internal set; }

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x00076E23 File Offset: 0x00075023
		// (set) Token: 0x06002189 RID: 8585 RVA: 0x00076E2B File Offset: 0x0007502B
		public string CustomGameServerPasswordOverride { get; internal set; }

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x00076E34 File Offset: 0x00075034
		// (set) Token: 0x0600218B RID: 8587 RVA: 0x00076E3C File Offset: 0x0007503C
		public string CustomGameServerAuthToken { get; internal set; }

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x0600218C RID: 8588 RVA: 0x00076E45 File Offset: 0x00075045
		// (set) Token: 0x0600218D RID: 8589 RVA: 0x00076E4D File Offset: 0x0007504D
		public bool CustomGameServerAllowsOptionalModules { get; internal set; } = true;

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x0600218E RID: 8590 RVA: 0x00076E56 File Offset: 0x00075056
		// (set) Token: 0x0600218F RID: 8591 RVA: 0x00076E5E File Offset: 0x0007505E
		public string OverridenUserName { get; internal set; }

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x00076E67 File Offset: 0x00075067
		// (set) Token: 0x06002191 RID: 8593 RVA: 0x00076E6F File Offset: 0x0007506F
		public string PremadeGameType { get; internal set; }

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x00076E78 File Offset: 0x00075078
		// (set) Token: 0x06002193 RID: 8595 RVA: 0x00076E80 File Offset: 0x00075080
		public int Permission { get; internal set; }

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x00076E89 File Offset: 0x00075089
		// (set) Token: 0x06002195 RID: 8597 RVA: 0x00076E91 File Offset: 0x00075091
		public string PlatformInterface { get; internal set; }

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002196 RID: 8598 RVA: 0x00076E9A File Offset: 0x0007509A
		// (set) Token: 0x06002197 RID: 8599 RVA: 0x00076EA2 File Offset: 0x000750A2
		public string EpicUserId { get; internal set; }

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002198 RID: 8600 RVA: 0x00076EAB File Offset: 0x000750AB
		// (set) Token: 0x06002199 RID: 8601 RVA: 0x00076EB3 File Offset: 0x000750B3
		public string EpicUserName { get; internal set; }

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x0600219A RID: 8602 RVA: 0x00076EBC File Offset: 0x000750BC
		// (set) Token: 0x0600219B RID: 8603 RVA: 0x00076EC4 File Offset: 0x000750C4
		public bool IsContinueGame { get; internal set; }

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x0600219C RID: 8604 RVA: 0x00076ECD File Offset: 0x000750CD
		// (set) Token: 0x0600219D RID: 8605 RVA: 0x00076ED5 File Offset: 0x000750D5
		public double ServerBandwidthLimitInMbps { get; internal set; }

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x0600219E RID: 8606 RVA: 0x00076EDE File Offset: 0x000750DE
		// (set) Token: 0x0600219F RID: 8607 RVA: 0x00076EE6 File Offset: 0x000750E6
		public int ServerTickRate { get; internal set; }
	}
}
