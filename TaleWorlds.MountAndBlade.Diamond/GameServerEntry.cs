using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200011D RID: 285
	[Serializable]
	public class GameServerEntry
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00008210 File Offset: 0x00006410
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x00008218 File Offset: 0x00006418
		[JsonProperty]
		public CustomBattleId Id { get; private set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00008221 File Offset: 0x00006421
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x00008229 File Offset: 0x00006429
		[JsonProperty]
		public string Address { get; private set; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00008232 File Offset: 0x00006432
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x0000823A File Offset: 0x0000643A
		[JsonProperty]
		public int Port { get; private set; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x00008243 File Offset: 0x00006443
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x0000824B File Offset: 0x0000644B
		[JsonProperty]
		public string Region { get; private set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x00008254 File Offset: 0x00006454
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x0000825C File Offset: 0x0000645C
		[JsonProperty]
		public int PlayerCount { get; private set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x00008265 File Offset: 0x00006465
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x0000826D File Offset: 0x0000646D
		[JsonProperty]
		public int MaxPlayerCount { get; private set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x00008276 File Offset: 0x00006476
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x0000827E File Offset: 0x0000647E
		[JsonProperty]
		public string ServerName { get; private set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x00008287 File Offset: 0x00006487
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x0000828F File Offset: 0x0000648F
		[JsonProperty]
		public string GameModule { get; private set; }

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x00008298 File Offset: 0x00006498
		// (set) Token: 0x06000659 RID: 1625 RVA: 0x000082A0 File Offset: 0x000064A0
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x000082A9 File Offset: 0x000064A9
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x000082B1 File Offset: 0x000064B1
		[JsonProperty]
		public string Map { get; private set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x000082BA File Offset: 0x000064BA
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x000082C2 File Offset: 0x000064C2
		[JsonProperty]
		public string UniqueMapId { get; private set; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x000082CB File Offset: 0x000064CB
		// (set) Token: 0x0600065F RID: 1631 RVA: 0x000082D3 File Offset: 0x000064D3
		[JsonProperty]
		public int Ping { get; private set; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x000082DC File Offset: 0x000064DC
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x000082E4 File Offset: 0x000064E4
		[JsonProperty]
		public bool IsOfficial { get; private set; }

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x000082ED File Offset: 0x000064ED
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x000082F5 File Offset: 0x000064F5
		[JsonProperty]
		public bool ByOfficialProvider { get; private set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x000082FE File Offset: 0x000064FE
		// (set) Token: 0x06000665 RID: 1637 RVA: 0x00008306 File Offset: 0x00006506
		[JsonProperty]
		public bool PasswordProtected { get; private set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0000830F File Offset: 0x0000650F
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x00008317 File Offset: 0x00006517
		[JsonProperty]
		public int Permission { get; private set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x00008320 File Offset: 0x00006520
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x00008328 File Offset: 0x00006528
		[JsonProperty]
		public bool CrossplayEnabled { get; private set; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x00008331 File Offset: 0x00006531
		// (set) Token: 0x0600066B RID: 1643 RVA: 0x00008339 File Offset: 0x00006539
		[JsonProperty]
		public PlayerId HostId { get; private set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x00008342 File Offset: 0x00006542
		// (set) Token: 0x0600066D RID: 1645 RVA: 0x0000834A File Offset: 0x0000654A
		[JsonProperty]
		public string HostName { get; private set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x00008353 File Offset: 0x00006553
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x0000835B File Offset: 0x0000655B
		[JsonProperty]
		public List<ModuleInfoModel> LoadedModules { get; private set; }

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x00008364 File Offset: 0x00006564
		// (set) Token: 0x06000671 RID: 1649 RVA: 0x0000836C File Offset: 0x0000656C
		[JsonProperty]
		public bool AllowsOptionalModules { get; private set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x00008375 File Offset: 0x00006575
		// (set) Token: 0x06000673 RID: 1651 RVA: 0x0000837D File Offset: 0x0000657D
		[JsonProperty]
		public int SpectatorCount { get; private set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x00008386 File Offset: 0x00006586
		// (set) Token: 0x06000675 RID: 1653 RVA: 0x0000838E File Offset: 0x0000658E
		[JsonProperty]
		public int MaxSpectatorCount { get; private set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x00008397 File Offset: 0x00006597
		// (set) Token: 0x06000677 RID: 1655 RVA: 0x0000839F File Offset: 0x0000659F
		[JsonProperty]
		public bool SpectatorPasswordProtected { get; private set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x000083A8 File Offset: 0x000065A8
		// (set) Token: 0x06000679 RID: 1657 RVA: 0x000083B0 File Offset: 0x000065B0
		[JsonProperty]
		public bool EnableSpectators { get; private set; }

		// Token: 0x0600067A RID: 1658 RVA: 0x000083B9 File Offset: 0x000065B9
		public GameServerEntry()
		{
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x000083C4 File Offset: 0x000065C4
		public GameServerEntry(CustomBattleId id, string serverName, string address, int port, string region, string gameModule, string gameType, string map, string uniqueMapId, int playerCount, int maxPlayerCount, bool isOfficial, bool byOfficialProvider, bool crossplayEnabled, PlayerId hostId, string hostName, List<ModuleInfoModel> loadedModules, bool allowsOptionalModules, bool passwordProtected = false, int permission = 0, int spectatorCount = 0, int maxSpectatorCount = 0, bool spectatorPasswordProtected = false, bool enableSpectators = false)
		{
			this.Id = id;
			this.ServerName = serverName;
			this.Address = address;
			this.GameModule = gameModule;
			this.GameType = gameType;
			this.Map = map;
			this.UniqueMapId = uniqueMapId;
			this.PlayerCount = playerCount;
			this.MaxPlayerCount = maxPlayerCount;
			this.Port = port;
			this.Region = region;
			this.IsOfficial = isOfficial;
			this.ByOfficialProvider = byOfficialProvider;
			this.CrossplayEnabled = crossplayEnabled;
			this.HostId = hostId;
			this.HostName = hostName;
			this.LoadedModules = loadedModules;
			this.AllowsOptionalModules = allowsOptionalModules;
			this.PasswordProtected = passwordProtected;
			this.Permission = permission;
			this.SpectatorCount = spectatorCount;
			this.MaxSpectatorCount = maxSpectatorCount;
			this.SpectatorPasswordProtected = spectatorPasswordProtected;
			this.EnableSpectators = enableSpectators;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00008494 File Offset: 0x00006694
		public static void FilterGameServerEntriesBasedOnCrossplay(ref List<GameServerEntry> serverList, bool hasCrossplayPrivilege)
		{
			bool flag = ApplicationPlatform.CurrentPlatform == Platform.GDKDesktop;
			if (flag && !hasCrossplayPrivilege)
			{
				serverList.RemoveAll((GameServerEntry s) => s.CrossplayEnabled);
				return;
			}
			if (!flag)
			{
				serverList.RemoveAll((GameServerEntry s) => !s.CrossplayEnabled);
			}
		}
	}
}
