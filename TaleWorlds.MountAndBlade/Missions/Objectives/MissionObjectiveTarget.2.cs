using System;

namespace TaleWorlds.MountAndBlade.Missions.Objectives
{
	// Token: 0x020003F1 RID: 1009
	public abstract class MissionObjectiveTarget<T> : MissionObjectiveTarget
	{
		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x060037DA RID: 14298 RVA: 0x000E7B2C File Offset: 0x000E5D2C
		public T Target { get; }

		// Token: 0x060037DB RID: 14299 RVA: 0x000E7B34 File Offset: 0x000E5D34
		public MissionObjectiveTarget(T target)
		{
			this.Target = target;
		}
	}
}
