using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200034F RID: 847
	public class FireBallista : Ballista
	{
		// Token: 0x06003010 RID: 12304 RVA: 0x000BCF2B File Offset: 0x000BB12B
		public override SiegeEngineType GetSiegeEngineType()
		{
			return DefaultSiegeEngineTypes.FireBallista;
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x000BCF34 File Offset: 0x000BB134
		public override float ProcessTargetValue(float baseValue, TargetFlags flags)
		{
			if (flags.HasAnyFlag(TargetFlags.NotAThreat))
			{
				return -1000f;
			}
			if (flags.HasAllFlags(TargetFlags.IsSiegeEngine | TargetFlags.IsAttacker))
			{
				baseValue *= 1.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSiegeEngine))
			{
				baseValue *= 2.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsStructure))
			{
				baseValue *= 0.1f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsFlammable))
			{
				baseValue *= 2f;
			}
			if (flags.HasAnyFlag(TargetFlags.DebugThreat))
			{
				baseValue *= 1000f;
			}
			return baseValue;
		}
	}
}
