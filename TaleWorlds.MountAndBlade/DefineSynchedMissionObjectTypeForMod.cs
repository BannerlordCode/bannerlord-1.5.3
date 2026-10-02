using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002F6 RID: 758
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public sealed class DefineSynchedMissionObjectTypeForMod : Attribute
	{
		// Token: 0x06002B82 RID: 11138 RVA: 0x000A7FA7 File Offset: 0x000A61A7
		public DefineSynchedMissionObjectTypeForMod(Type type)
		{
			this.Type = type;
		}

		// Token: 0x040010F7 RID: 4343
		public readonly Type Type;
	}
}
