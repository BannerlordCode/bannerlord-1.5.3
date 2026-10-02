using System;
using System.Runtime.Serialization;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x02000034 RID: 52
	[DataContract]
	[Serializable]
	public abstract class RestFunctionResult : RestData
	{
		// Token: 0x0600013B RID: 315
		public abstract FunctionResult GetFunctionResult();
	}
}
