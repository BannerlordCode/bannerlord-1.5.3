using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x020000A3 RID: 163
	public static class EngineExtensions
	{
		// Token: 0x06000F44 RID: 3908 RVA: 0x00012111 File Offset: 0x00010311
		public static WorldPosition ToWorldPosition(this Vec3 vec3, Scene scene)
		{
			return new WorldPosition(scene, UIntPtr.Zero, vec3, false);
		}
	}
}
