using System;

namespace System.Management
{
	// Token: 0x02000004 RID: 4
	public enum AuthenticationLevel
	{
		// Token: 0x04000003 RID: 3
		Unchanged = -1,
		// Token: 0x04000004 RID: 4
		Default,
		// Token: 0x04000005 RID: 5
		None,
		// Token: 0x04000006 RID: 6
		Connect,
		// Token: 0x04000007 RID: 7
		Call,
		// Token: 0x04000008 RID: 8
		Packet,
		// Token: 0x04000009 RID: 9
		PacketIntegrity,
		// Token: 0x0400000A RID: 10
		PacketPrivacy
	}
}
