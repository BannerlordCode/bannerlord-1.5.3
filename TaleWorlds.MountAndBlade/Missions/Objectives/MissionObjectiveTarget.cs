using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Missions.Objectives
{
	// Token: 0x020003F0 RID: 1008
	public abstract class MissionObjectiveTarget
	{
		// Token: 0x060037D6 RID: 14294
		public abstract bool IsActive();

		// Token: 0x060037D7 RID: 14295
		public abstract TextObject GetName();

		// Token: 0x060037D8 RID: 14296
		public abstract Vec3 GetGlobalPosition();
	}
}
