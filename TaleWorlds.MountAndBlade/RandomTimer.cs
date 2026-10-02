using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200038F RID: 911
	public class RandomTimer : Timer
	{
		// Token: 0x0600349F RID: 13471 RVA: 0x000DA093 File Offset: 0x000D8293
		public RandomTimer(float gameTime, float durationMin, float durationMax)
			: base(gameTime, MBRandom.RandomFloatRanged(durationMin, durationMax), true)
		{
			this.durationMin = durationMin;
			this.durationMax = durationMax;
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x000DA0B4 File Offset: 0x000D82B4
		public override bool Check(float gameTime)
		{
			bool flag = false;
			bool flag2;
			do
			{
				flag2 = base.Check(gameTime);
				if (flag2)
				{
					this.RecomputeDuration();
					flag = true;
				}
			}
			while (flag2);
			return flag;
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x000DA0DA File Offset: 0x000D82DA
		public void ChangeDuration(float min, float max)
		{
			this.durationMin = min;
			this.durationMax = max;
			this.RecomputeDuration();
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x000DA0F0 File Offset: 0x000D82F0
		public void RecomputeDuration()
		{
			base.Duration = MBRandom.RandomFloatRanged(this.durationMin, this.durationMax);
		}

		// Token: 0x0400165A RID: 5722
		private float durationMin;

		// Token: 0x0400165B RID: 5723
		private float durationMax;
	}
}
