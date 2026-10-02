using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000046 RID: 70
	[Serializable]
	public class GetUserCosmeticsInfoMessageResult : FunctionResult
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00002FC6 File Offset: 0x000011C6
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00002FCE File Offset: 0x000011CE
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00002FD7 File Offset: 0x000011D7
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00002FDF File Offset: 0x000011DF
		[JsonProperty]
		public List<string> OwnedCosmetics { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00002FE8 File Offset: 0x000011E8
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00002FF0 File Offset: 0x000011F0
		[JsonProperty]
		public Dictionary<string, List<string>> UsedCosmetics { get; private set; }

		// Token: 0x06000165 RID: 357 RVA: 0x00002FF9 File Offset: 0x000011F9
		public GetUserCosmeticsInfoMessageResult()
		{
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00003001 File Offset: 0x00001201
		public GetUserCosmeticsInfoMessageResult(bool successful, List<string> ownedCosmetics, Dictionary<string, List<string>> usedCosmetics)
		{
			this.Successful = successful;
			this.OwnedCosmetics = ownedCosmetics;
			this.UsedCosmetics = usedCosmetics;
		}
	}
}
