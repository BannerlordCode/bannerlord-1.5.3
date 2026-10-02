using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000350 RID: 848
	public class FireMangonel : Mangonel
	{
		// Token: 0x06003013 RID: 12307 RVA: 0x000BCFB9 File Offset: 0x000BB1B9
		public override SiegeEngineType GetSiegeEngineType()
		{
			if (this.DefaultSide != BattleSideEnum.Attacker)
			{
				return DefaultSiegeEngineTypes.FireCatapult;
			}
			return DefaultSiegeEngineTypes.FireOnager;
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x000BCFD0 File Offset: 0x000BB1D0
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
				baseValue *= 12f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsStructure))
			{
				baseValue *= 1.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSmall))
			{
				baseValue *= 8f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsMoving))
			{
				baseValue *= 12f;
			}
			if (flags.HasAnyFlag(TargetFlags.DebugThreat))
			{
				baseValue *= 10f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSiegeTower))
			{
				baseValue *= 12f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsFlammable))
			{
				baseValue *= 100f;
			}
			return baseValue;
		}
	}
}
