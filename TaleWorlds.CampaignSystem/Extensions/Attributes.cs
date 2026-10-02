using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x02000175 RID: 373
	public static class Attributes
	{
		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06001BDC RID: 7132 RVA: 0x000904F5 File Offset: 0x0008E6F5
		public static MBReadOnlyList<CharacterAttribute> All
		{
			get
			{
				return Campaign.Current.AllCharacterAttributes;
			}
		}
	}
}
