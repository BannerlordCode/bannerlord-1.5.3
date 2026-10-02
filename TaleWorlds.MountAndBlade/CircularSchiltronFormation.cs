using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000142 RID: 322
	public class CircularSchiltronFormation : CircularFormation
	{
		// Token: 0x06000F8F RID: 3983 RVA: 0x0002A509 File Offset: 0x00028709
		public CircularSchiltronFormation(IFormation owner)
			: base(owner)
		{
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0002A512 File Offset: 0x00028712
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new CircularSchiltronFormation(formation);
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x0002A51C File Offset: 0x0002871C
		public override float MaximumWidth
		{
			get
			{
				int unitCountWithOverride = base.GetUnitCountWithOverride();
				int currentMaximumRankCount = base.GetCurrentMaximumRankCount(unitCountWithOverride);
				float num = this.owner.MaximumInterval + base.UnitDiameter;
				float num2 = this.owner.MaximumDistance + base.UnitDiameter;
				return base.GetCircumferenceAux(unitCountWithOverride, currentMaximumRankCount, num, num2) / 3.1415927f;
			}
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0002A570 File Offset: 0x00028770
		public void Form()
		{
			int unitCountWithOverride = base.GetUnitCountWithOverride();
			int currentMaximumRankCount = base.GetCurrentMaximumRankCount(unitCountWithOverride);
			float circumferenceFromRankCount = base.GetCircumferenceFromRankCount(currentMaximumRankCount);
			base.FormFromCircumference(circumferenceFromRankCount);
		}
	}
}
