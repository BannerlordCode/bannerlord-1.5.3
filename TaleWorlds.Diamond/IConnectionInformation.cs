using System;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000015 RID: 21
	public interface IConnectionInformation
	{
		// Token: 0x06000079 RID: 121
		string GetAddress(bool isIpv6Compatible = false);

		// Token: 0x0600007A RID: 122
		string GetCountry();
	}
}
