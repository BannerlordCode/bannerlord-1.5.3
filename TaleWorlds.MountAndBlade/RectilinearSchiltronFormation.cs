using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200014B RID: 331
	public class RectilinearSchiltronFormation : SquareFormation
	{
		// Token: 0x0600111D RID: 4381 RVA: 0x00030DFC File Offset: 0x0002EFFC
		public RectilinearSchiltronFormation(IFormation owner)
			: base(owner)
		{
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00030E05 File Offset: 0x0002F005
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new RectilinearSchiltronFormation(formation);
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x0600111F RID: 4383 RVA: 0x00030E10 File Offset: 0x0002F010
		public override float MaximumWidth
		{
			get
			{
				int num;
				int maximumRankCount = SquareFormation.GetMaximumRankCount(base.GetUnitCountWithOverride(), out num);
				return SquareFormation.GetSideWidthFromUnitCount(base.GetUnitsPerSideFromRankCount(maximumRankCount), this.owner.MaximumInterval, base.UnitDiameter);
			}
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00030E48 File Offset: 0x0002F048
		public void Form()
		{
			int num;
			int maximumRankCount = SquareFormation.GetMaximumRankCount(base.GetUnitCountWithOverride(), out num);
			base.FormFromRankCount(maximumRankCount);
		}
	}
}
