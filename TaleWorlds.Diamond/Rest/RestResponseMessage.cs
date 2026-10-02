using System;
using System.Runtime.Serialization;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x02000037 RID: 55
	[DataContract]
	[Serializable]
	public abstract class RestResponseMessage : RestData
	{
		// Token: 0x06000143 RID: 323
		public abstract Message GetMessage();
	}
}
