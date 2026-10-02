using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200014A RID: 330
	public class TransposedLineFormation : LineFormation
	{
		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001118 RID: 4376 RVA: 0x00030D2B File Offset: 0x0002EF2B
		public override float IntervalMultiplier
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x00030D32 File Offset: 0x0002EF32
		public override float DistanceMultiplier
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00030D39 File Offset: 0x0002EF39
		public TransposedLineFormation(IFormation owner)
			: base(owner, true)
		{
			base.IsStaggered = false;
			this.IsTransforming = true;
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00030D51 File Offset: 0x0002EF51
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new TransposedLineFormation(formation);
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00030D5C File Offset: 0x0002EF5C
		public override void RearrangeFrom(IFormationArrangement arrangement)
		{
			ColumnFormation columnFormation;
			if ((columnFormation = arrangement as ColumnFormation) != null)
			{
				base.FormFromFlankWidth(columnFormation.ColumnCount, false);
			}
			else
			{
				int num = MathF.Ceiling(MathF.Sqrt((float)(arrangement.UnitCount / ColumnFormation.ArrangementAspectRatio)));
				if (num > 0 && (float)(arrangement.UnitCount / num - 1) * (base.UnitDiameter + base.Distance) + base.UnitDiameter > 100f)
				{
					(this.owner as Formation).SetPositioning(null, null, new int?(0));
				}
				base.FormFromFlankWidth(num, false);
			}
			base.RearrangeFrom(arrangement);
		}
	}
}
