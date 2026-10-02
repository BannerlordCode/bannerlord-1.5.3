using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000CA RID: 202
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class UpdateUsedCosmeticItemsMessage : Message
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x000048C2 File Offset: 0x00002AC2
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x000048CA File Offset: 0x00002ACA
		[JsonProperty]
		public List<CosmeticItemInfo> UsedCosmetics { get; private set; }

		// Token: 0x060003B9 RID: 953 RVA: 0x000048D3 File Offset: 0x00002AD3
		public UpdateUsedCosmeticItemsMessage()
		{
		}

		// Token: 0x060003BA RID: 954 RVA: 0x000048DB File Offset: 0x00002ADB
		public UpdateUsedCosmeticItemsMessage(List<CosmeticItemInfo> usedCosmetics)
		{
			this.UsedCosmetics = usedCosmetics;
		}
	}
}
