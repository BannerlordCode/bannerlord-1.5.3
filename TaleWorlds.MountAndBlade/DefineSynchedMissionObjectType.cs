using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002F7 RID: 759
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	internal sealed class DefineSynchedMissionObjectType : Attribute
	{
		// Token: 0x06002B83 RID: 11139 RVA: 0x000A7FB6 File Offset: 0x000A61B6
		public DefineSynchedMissionObjectType(Type type)
		{
			this.Type = type;
		}

		// Token: 0x040010F8 RID: 4344
		public readonly Type Type;
	}
}
