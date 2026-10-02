using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection
{
	// Token: 0x02000020 RID: 32
	public static class UIColors
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00012DB3 File Offset: 0x00010FB3
		public static Color PositiveIndicator
		{
			get
			{
				return UIColors._positiveIndicator;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00012DBA File Offset: 0x00010FBA
		public static Color NegativeIndicator
		{
			get
			{
				return UIColors._negativeIndicator;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00012DC1 File Offset: 0x00010FC1
		public static Color Gold
		{
			get
			{
				return UIColors._gold;
			}
		}

		// Token: 0x040000DF RID: 223
		private static Color _positiveIndicator = Color.FromUint(4285250886U);

		// Token: 0x040000E0 RID: 224
		private static Color _negativeIndicator = Color.FromUint(4290070086U);

		// Token: 0x040000E1 RID: 225
		private static Color _gold = Color.FromUint(4294957447U);
	}
}
