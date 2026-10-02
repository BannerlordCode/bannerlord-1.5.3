using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000062 RID: 98
	[Serializable]
	public class RegisterCustomGameResult : FunctionResult
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00003687 File Offset: 0x00001887
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000368F File Offset: 0x0000188F
		[JsonProperty]
		public bool Success { get; private set; }

		// Token: 0x060001FE RID: 510 RVA: 0x00003698 File Offset: 0x00001898
		public RegisterCustomGameResult()
		{
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000036A0 File Offset: 0x000018A0
		public RegisterCustomGameResult(bool success)
		{
			this.Success = success;
		}
	}
}
