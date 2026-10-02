using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x02000005 RID: 5
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", true)]
	[Serializable]
	public class CustomBattleServerReadyMessage : LoginMessage
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002174 File Offset: 0x00000374
		// (set) Token: 0x06000016 RID: 22 RVA: 0x0000217C File Offset: 0x0000037C
		[JsonProperty]
		public ApplicationVersion ApplicationVersion { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002185 File Offset: 0x00000385
		// (set) Token: 0x06000018 RID: 24 RVA: 0x0000218D File Offset: 0x0000038D
		[JsonProperty]
		public string AuthToken { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002196 File Offset: 0x00000396
		// (set) Token: 0x0600001A RID: 26 RVA: 0x0000219E File Offset: 0x0000039E
		[JsonProperty]
		public ModuleInfoModel[] LoadedModules { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000021A7 File Offset: 0x000003A7
		// (set) Token: 0x0600001C RID: 28 RVA: 0x000021AF File Offset: 0x000003AF
		[JsonProperty]
		public bool AllowsOptionalModules { get; private set; }

		// Token: 0x0600001D RID: 29 RVA: 0x000021B8 File Offset: 0x000003B8
		public CustomBattleServerReadyMessage()
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000021C0 File Offset: 0x000003C0
		public CustomBattleServerReadyMessage(PeerId peerId, ApplicationVersion applicationVersion, string authToken, ModuleInfoModel[] loadedModules, bool allowsOptionalModules)
			: base(peerId, null)
		{
			this.ApplicationVersion = applicationVersion;
			this.AuthToken = authToken;
			this.LoadedModules = loadedModules;
			this.AllowsOptionalModules = allowsOptionalModules;
		}
	}
}
