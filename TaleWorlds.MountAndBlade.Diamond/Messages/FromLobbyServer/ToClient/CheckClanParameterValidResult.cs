using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200001D RID: 29
	[Serializable]
	public class CheckClanParameterValidResult : FunctionResult
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000028E7 File Offset: 0x00000AE7
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x000028EF File Offset: 0x00000AEF
		[JsonProperty]
		public bool IsValid { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x000028F8 File Offset: 0x00000AF8
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00002900 File Offset: 0x00000B00
		[JsonProperty]
		public StringValidationError Error { get; private set; }

		// Token: 0x060000B7 RID: 183 RVA: 0x00002909 File Offset: 0x00000B09
		public CheckClanParameterValidResult()
		{
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00002911 File Offset: 0x00000B11
		public CheckClanParameterValidResult(bool isValid, StringValidationError error)
		{
			this.IsValid = isValid;
			this.Error = error;
		}
	}
}
