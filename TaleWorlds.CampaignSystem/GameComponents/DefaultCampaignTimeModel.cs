using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000106 RID: 262
	public class DefaultCampaignTimeModel : CampaignTimeModel
	{
		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x0006C734 File Offset: 0x0006A934
		public override CampaignTime CampaignStartTime
		{
			get
			{
				return CampaignTime.Years(1084f) + CampaignTime.Weeks((float)CampaignTime.WeeksInSeason) + CampaignTime.Hours(9f);
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x0006C75F File Offset: 0x0006A95F
		public override int SunRise
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x0006C762 File Offset: 0x0006A962
		public override int SunSet
		{
			get
			{
				return 22;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x0006C766 File Offset: 0x0006A966
		public override long TimeTicksPerMillisecond
		{
			get
			{
				return 10L;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001751 RID: 5969 RVA: 0x0006C76B File Offset: 0x0006A96B
		public override int MillisecondInSecond
		{
			get
			{
				return 1000;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001752 RID: 5970 RVA: 0x0006C772 File Offset: 0x0006A972
		public override int SecondsInMinute
		{
			get
			{
				return 60;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001753 RID: 5971 RVA: 0x0006C776 File Offset: 0x0006A976
		public override int MinutesInHour
		{
			get
			{
				return 60;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001754 RID: 5972 RVA: 0x0006C77A File Offset: 0x0006A97A
		public override int HoursInDay
		{
			get
			{
				return 24;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06001755 RID: 5973 RVA: 0x0006C77E File Offset: 0x0006A97E
		public override int DaysInWeek
		{
			get
			{
				if (Campaign.Current.Options.AccelerationMode != GameAccelerationMode.Fast)
				{
					return 7;
				}
				return 3;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001756 RID: 5974 RVA: 0x0006C795 File Offset: 0x0006A995
		public override int WeeksInSeason
		{
			get
			{
				if (Campaign.Current.Options.AccelerationMode != GameAccelerationMode.Fast)
				{
					return 3;
				}
				return 2;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001757 RID: 5975 RVA: 0x0006C7AC File Offset: 0x0006A9AC
		public override int SeasonsInYear
		{
			get
			{
				return 4;
			}
		}
	}
}
