using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000203 RID: 515
	public abstract class CampaignTimeModel : MBGameModel<CampaignTimeModel>
	{
		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x0600201D RID: 8221
		public abstract CampaignTime CampaignStartTime { get; }

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x0600201E RID: 8222
		public abstract int SunRise { get; }

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x0600201F RID: 8223
		public abstract int SunSet { get; }

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06002020 RID: 8224
		public abstract long TimeTicksPerMillisecond { get; }

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06002021 RID: 8225
		public abstract int MillisecondInSecond { get; }

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06002022 RID: 8226
		public abstract int SecondsInMinute { get; }

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06002023 RID: 8227
		public abstract int MinutesInHour { get; }

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06002024 RID: 8228
		public abstract int HoursInDay { get; }

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06002025 RID: 8229
		public abstract int DaysInWeek { get; }

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06002026 RID: 8230
		public abstract int WeeksInSeason { get; }

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06002027 RID: 8231
		public abstract int SeasonsInYear { get; }
	}
}
