using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000080 RID: 128
	public enum SpectatorCameraTypes
	{
		// Token: 0x04000445 RID: 1093
		Invalid = -1,
		// Token: 0x04000446 RID: 1094
		Free,
		// Token: 0x04000447 RID: 1095
		LockToMainPlayer,
		// Token: 0x04000448 RID: 1096
		LockToAnyAgent,
		// Token: 0x04000449 RID: 1097
		LockToAnyPlayer,
		// Token: 0x0400044A RID: 1098
		LockToPlayerFormation,
		// Token: 0x0400044B RID: 1099
		LockToTeamMembers,
		// Token: 0x0400044C RID: 1100
		LockToTeamMembersView,
		// Token: 0x0400044D RID: 1101
		LockToPosition,
		// Token: 0x0400044E RID: 1102
		OrbitAroundTarget,
		// Token: 0x0400044F RID: 1103
		Count
	}
}
