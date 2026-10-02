using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000148 RID: 328
	public interface IFormationUnit
	{
		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001070 RID: 4208
		IFormationArrangement Formation { get; }

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001071 RID: 4209
		// (set) Token: 0x06001072 RID: 4210
		int FormationFileIndex { get; set; }

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001073 RID: 4211
		// (set) Token: 0x06001074 RID: 4212
		int FormationRankIndex { get; set; }

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001075 RID: 4213
		IFormationUnit FollowedUnit { get; }

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001076 RID: 4214
		bool IsShieldUsageEncouraged { get; }

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001077 RID: 4215
		bool IsPlayerUnit { get; }
	}
}
