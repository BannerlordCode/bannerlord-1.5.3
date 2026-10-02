using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000AD RID: 173
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class InitializeSession : LoginMessage
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000312 RID: 786 RVA: 0x000041C1 File Offset: 0x000023C1
		// (set) Token: 0x06000313 RID: 787 RVA: 0x000041C9 File Offset: 0x000023C9
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000314 RID: 788 RVA: 0x000041D2 File Offset: 0x000023D2
		// (set) Token: 0x06000315 RID: 789 RVA: 0x000041DA File Offset: 0x000023DA
		[JsonProperty]
		public string PlayerName { get; private set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000316 RID: 790 RVA: 0x000041E3 File Offset: 0x000023E3
		// (set) Token: 0x06000317 RID: 791 RVA: 0x000041EB File Offset: 0x000023EB
		[JsonProperty]
		public ApplicationVersion ApplicationVersion { get; private set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000318 RID: 792 RVA: 0x000041F4 File Offset: 0x000023F4
		// (set) Token: 0x06000319 RID: 793 RVA: 0x000041FC File Offset: 0x000023FC
		[JsonProperty]
		public string ConnectionPassword { get; private set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600031A RID: 794 RVA: 0x00004205 File Offset: 0x00002405
		// (set) Token: 0x0600031B RID: 795 RVA: 0x0000420D File Offset: 0x0000240D
		[JsonProperty]
		public ModuleInfoModel[] LoadedModules { get; private set; }

		// Token: 0x0600031C RID: 796 RVA: 0x00004216 File Offset: 0x00002416
		public InitializeSession()
		{
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000421E File Offset: 0x0000241E
		public InitializeSession(PlayerId playerId, string playerName, AccessObject accessObject, ApplicationVersion applicationVersion, string connectionPassword, ModuleInfoModel[] loadedModules)
			: base(playerId.ConvertToPeerId(), accessObject)
		{
			this.PlayerId = playerId;
			this.PlayerName = playerName;
			this.ApplicationVersion = applicationVersion;
			this.ConnectionPassword = connectionPassword;
			this.LoadedModules = loadedModules;
		}
	}
}
