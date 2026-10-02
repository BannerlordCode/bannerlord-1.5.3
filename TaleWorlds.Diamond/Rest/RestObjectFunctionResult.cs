using System;
using System.Runtime.Serialization;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x02000035 RID: 53
	[DataContract]
	[Serializable]
	public class RestObjectFunctionResult : RestFunctionResult
	{
		// Token: 0x0600013D RID: 317 RVA: 0x00003F14 File Offset: 0x00002114
		public override FunctionResult GetFunctionResult()
		{
			return this._functionResult;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00003F1C File Offset: 0x0000211C
		public RestObjectFunctionResult()
		{
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00003F24 File Offset: 0x00002124
		public RestObjectFunctionResult(FunctionResult functionResult)
		{
			this._functionResult = functionResult;
		}

		// Token: 0x04000063 RID: 99
		[DataMember]
		private FunctionResult _functionResult;
	}
}
