using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000337 RID: 823
	[Obsolete]
	public class DestructedPrefabInfoMissionObject : MissionObject
	{
		// Token: 0x04001279 RID: 4729
		public string DestructedPrefabName;

		// Token: 0x0400127A RID: 4730
		public Vec3 Translate = new Vec3(0f, 0f, 0f, -1f);

		// Token: 0x0400127B RID: 4731
		public Vec3 Rotation = new Vec3(0f, 0f, 0f, -1f);

		// Token: 0x0400127C RID: 4732
		public Vec3 Scale = new Vec3(1f, 1f, 1f, -1f);
	}
}
