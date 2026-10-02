using System;

namespace TaleWorlds.MountAndBlade.Missions.Objectives
{
	// Token: 0x020003EF RID: 1007
	public struct MissionObjectiveProgressInfo
	{
		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x060037D5 RID: 14293 RVA: 0x000E7B19 File Offset: 0x000E5D19
		public bool HasProgress
		{
			get
			{
				return this.RequiredProgressAmount > 0;
			}
		}

		// Token: 0x0400181F RID: 6175
		public int RequiredProgressAmount;

		// Token: 0x04001820 RID: 6176
		public int CurrentProgressAmount;
	}
}
