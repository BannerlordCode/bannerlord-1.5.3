using System;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000016 RID: 22
	public sealed class InnerProcessConnectionInformation : IConnectionInformation
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00002A45 File Offset: 0x00000C45
		string IConnectionInformation.GetAddress(bool isIpv6Compatible)
		{
			return "InnerProcess";
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002A4C File Offset: 0x00000C4C
		string IConnectionInformation.GetCountry()
		{
			return "TR";
		}
	}
}
