using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003B8 RID: 952
	public class FireTrebuchet : Trebuchet
	{
		// Token: 0x06003615 RID: 13845 RVA: 0x000DF33A File Offset: 0x000DD53A
		public override SiegeEngineType GetSiegeEngineType()
		{
			return DefaultSiegeEngineTypes.FireTrebuchet;
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x000DF344 File Offset: 0x000DD544
		public override float ProcessTargetValue(float baseValue, TargetFlags flags)
		{
			if (flags.HasAnyFlag(TargetFlags.NotAThreat))
			{
				return -1000f;
			}
			if (flags.HasAllFlags(TargetFlags.IsFlammable | TargetFlags.IsSiegeEngine))
			{
				baseValue *= 1.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSiegeEngine))
			{
				baseValue *= 2f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsStructure))
			{
				baseValue *= 1.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSmall))
			{
				baseValue *= 0.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsMoving))
			{
				baseValue *= 0.8f;
			}
			if (flags.HasAnyFlag(TargetFlags.DebugThreat))
			{
				baseValue *= 10000f;
			}
			return baseValue;
		}
	}
}
