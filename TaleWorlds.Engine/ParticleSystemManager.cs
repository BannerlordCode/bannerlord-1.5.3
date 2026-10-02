using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000074 RID: 116
	public sealed class ParticleSystemManager
	{
		// Token: 0x06000A95 RID: 2709 RVA: 0x0000ABFA File Offset: 0x00008DFA
		public static int GetRuntimeIdByName(string particleSystemName)
		{
			return EngineApplicationInterface.IParticleSystem.GetRuntimeIdByName(particleSystemName);
		}
	}
}
