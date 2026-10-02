using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A7 RID: 679
	public struct MissionTime : IComparable<MissionTime>
	{
		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x060025A7 RID: 9639 RVA: 0x0008950F File Offset: 0x0008770F
		public long NumberOfTicks
		{
			get
			{
				return this._numberOfTicks;
			}
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x00089517 File Offset: 0x00087717
		public MissionTime(long numberOfTicks)
		{
			this._numberOfTicks = numberOfTicks;
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060025A9 RID: 9641 RVA: 0x00089520 File Offset: 0x00087720
		private static long CurrentNumberOfTicks
		{
			get
			{
				return Mission.Current.MissionTimeTracker.NumberOfTicks;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060025AA RID: 9642 RVA: 0x00089531 File Offset: 0x00087731
		public static MissionTime DeltaTime
		{
			get
			{
				return new MissionTime(Mission.Current.MissionTimeTracker.DeltaTimeInTicks);
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060025AB RID: 9643 RVA: 0x00089547 File Offset: 0x00087747
		private static long DeltaTimeInTicks
		{
			get
			{
				return Mission.Current.MissionTimeTracker.DeltaTimeInTicks;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060025AC RID: 9644 RVA: 0x00089558 File Offset: 0x00087758
		public static MissionTime Now
		{
			get
			{
				return new MissionTime(Mission.Current.MissionTimeTracker.NumberOfTicks);
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x060025AD RID: 9645 RVA: 0x0008956E File Offset: 0x0008776E
		public bool IsFuture
		{
			get
			{
				return MissionTime.CurrentNumberOfTicks < this._numberOfTicks;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x060025AE RID: 9646 RVA: 0x0008957D File Offset: 0x0008777D
		public bool IsPast
		{
			get
			{
				return MissionTime.CurrentNumberOfTicks > this._numberOfTicks;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x0008958C File Offset: 0x0008778C
		public bool IsNow
		{
			get
			{
				return MissionTime.CurrentNumberOfTicks == this._numberOfTicks;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x060025B0 RID: 9648 RVA: 0x0008959B File Offset: 0x0008779B
		public float ElapsedHours
		{
			get
			{
				return (float)(MissionTime.CurrentNumberOfTicks - this._numberOfTicks) / 3.6E+10f;
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x060025B1 RID: 9649 RVA: 0x000895B0 File Offset: 0x000877B0
		public float ElapsedSeconds
		{
			get
			{
				return (float)(MissionTime.CurrentNumberOfTicks - this._numberOfTicks) * 1E-07f;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x060025B2 RID: 9650 RVA: 0x000895C5 File Offset: 0x000877C5
		public float ElapsedMilliseconds
		{
			get
			{
				return (float)(MissionTime.CurrentNumberOfTicks - this._numberOfTicks) / 10000f;
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x060025B3 RID: 9651 RVA: 0x000895DA File Offset: 0x000877DA
		public double ToHours
		{
			get
			{
				return (double)this._numberOfTicks / 36000000000.0;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x060025B4 RID: 9652 RVA: 0x000895ED File Offset: 0x000877ED
		public double ToMinutes
		{
			get
			{
				return (double)this._numberOfTicks / 600000000.0;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x060025B5 RID: 9653 RVA: 0x00089600 File Offset: 0x00087800
		public double ToSeconds
		{
			get
			{
				return (double)this._numberOfTicks * 1.0000000116860974E-07;
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x00089613 File Offset: 0x00087813
		public double ToMilliseconds
		{
			get
			{
				return (double)this._numberOfTicks / 10000.0;
			}
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x00089626 File Offset: 0x00087826
		public static MissionTime MillisecondsFromNow(float valueInMilliseconds)
		{
			return new MissionTime((long)(valueInMilliseconds * 10000f + (float)MissionTime.CurrentNumberOfTicks));
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x0008963C File Offset: 0x0008783C
		public static MissionTime SecondsFromNow(float valueInSeconds)
		{
			return new MissionTime((long)(valueInSeconds * 10000000f + (float)MissionTime.CurrentNumberOfTicks));
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x00089652 File Offset: 0x00087852
		public bool Equals(MissionTime other)
		{
			return this._numberOfTicks == other._numberOfTicks;
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x00089662 File Offset: 0x00087862
		public override bool Equals(object obj)
		{
			return obj != null && obj is MissionTime && this.Equals((MissionTime)obj);
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x00089680 File Offset: 0x00087880
		public override int GetHashCode()
		{
			return this._numberOfTicks.GetHashCode();
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x0008969B File Offset: 0x0008789B
		public int CompareTo(MissionTime other)
		{
			if (this._numberOfTicks == other._numberOfTicks)
			{
				return 0;
			}
			if (this._numberOfTicks > other._numberOfTicks)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x000896BE File Offset: 0x000878BE
		public static bool operator <(MissionTime x, MissionTime y)
		{
			return x._numberOfTicks < y._numberOfTicks;
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x000896CE File Offset: 0x000878CE
		public static bool operator >(MissionTime x, MissionTime y)
		{
			return x._numberOfTicks > y._numberOfTicks;
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x000896DE File Offset: 0x000878DE
		public static bool operator ==(MissionTime x, MissionTime y)
		{
			return x._numberOfTicks == y._numberOfTicks;
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x000896EE File Offset: 0x000878EE
		public static bool operator !=(MissionTime x, MissionTime y)
		{
			return !(x == y);
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x000896FA File Offset: 0x000878FA
		public static bool operator <=(MissionTime x, MissionTime y)
		{
			return x._numberOfTicks <= y._numberOfTicks;
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x0008970D File Offset: 0x0008790D
		public static bool operator >=(MissionTime x, MissionTime y)
		{
			return x._numberOfTicks >= y._numberOfTicks;
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x00089720 File Offset: 0x00087920
		public static MissionTime Milliseconds(float valueInMilliseconds)
		{
			return new MissionTime((long)(valueInMilliseconds * 10000f));
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x0008972F File Offset: 0x0008792F
		public static MissionTime Seconds(float valueInSeconds)
		{
			return new MissionTime((long)(valueInSeconds * 10000000f));
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x0008973E File Offset: 0x0008793E
		public static MissionTime Minutes(float valueInMinutes)
		{
			return new MissionTime((long)(valueInMinutes * 600000000f));
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x0008974D File Offset: 0x0008794D
		public static MissionTime Hours(float valueInHours)
		{
			return new MissionTime((long)(valueInHours * 3.6E+10f));
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x060025C7 RID: 9671 RVA: 0x0008975C File Offset: 0x0008795C
		public static MissionTime Zero
		{
			get
			{
				return new MissionTime(0L);
			}
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x00089765 File Offset: 0x00087965
		public static MissionTime operator +(MissionTime g1, MissionTime g2)
		{
			return new MissionTime(g1._numberOfTicks + g2._numberOfTicks);
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x00089779 File Offset: 0x00087979
		public static MissionTime operator -(MissionTime g1, MissionTime g2)
		{
			return new MissionTime(g1._numberOfTicks - g2._numberOfTicks);
		}

		// Token: 0x04000E90 RID: 3728
		public const long TimeTicksPerMilliSecond = 10000L;

		// Token: 0x04000E91 RID: 3729
		public const long TimeTicksPerSecond = 10000000L;

		// Token: 0x04000E92 RID: 3730
		public const long TimeTicksPerMinute = 600000000L;

		// Token: 0x04000E93 RID: 3731
		public const long TimeTicksPerHour = 36000000000L;

		// Token: 0x04000E94 RID: 3732
		public const float InvTimeTicksPerSecond = 1E-07f;

		// Token: 0x04000E95 RID: 3733
		private readonly long _numberOfTicks;
	}
}
