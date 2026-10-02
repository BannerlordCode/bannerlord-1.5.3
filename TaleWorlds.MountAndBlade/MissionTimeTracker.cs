using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A9 RID: 681
	public class MissionTimeTracker
	{
		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x060025D4 RID: 9684 RVA: 0x0008989D File Offset: 0x00087A9D
		// (set) Token: 0x060025D5 RID: 9685 RVA: 0x000898A5 File Offset: 0x00087AA5
		public long NumberOfTicks { get; private set; }

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x060025D6 RID: 9686 RVA: 0x000898AE File Offset: 0x00087AAE
		// (set) Token: 0x060025D7 RID: 9687 RVA: 0x000898B6 File Offset: 0x00087AB6
		public long DeltaTimeInTicks { get; private set; }

		// Token: 0x060025D8 RID: 9688 RVA: 0x000898BF File Offset: 0x00087ABF
		public MissionTimeTracker(MissionTime initialMapTime)
		{
			this.NumberOfTicks = initialMapTime.NumberOfTicks;
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x000898D4 File Offset: 0x00087AD4
		public MissionTimeTracker()
		{
			this.NumberOfTicks = 0L;
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x000898E4 File Offset: 0x00087AE4
		public void Tick(float seconds)
		{
			this.DeltaTimeInTicks = (long)(seconds * 10000000f);
			this.NumberOfTicks += this.DeltaTimeInTicks;
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x00089908 File Offset: 0x00087B08
		public void UpdateSync(float newValue)
		{
			long num = (long)(newValue * 10000000f);
			this._lastSyncDifference = num - this.NumberOfTicks;
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x0008992C File Offset: 0x00087B2C
		public float GetLastSyncDifference()
		{
			return (float)this._lastSyncDifference / 10000000f;
		}

		// Token: 0x04000E9A RID: 3738
		private long _lastSyncDifference;
	}
}
