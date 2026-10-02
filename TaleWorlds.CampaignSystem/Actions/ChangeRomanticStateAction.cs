using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C9 RID: 1225
	public static class ChangeRomanticStateAction
	{
		// Token: 0x06004D41 RID: 19777 RVA: 0x00186ED0 File Offset: 0x001850D0
		private static void ApplyInternal(Hero hero1, Hero hero2, Romance.RomanceLevelEnum toWhat)
		{
			Romance.SetRomanticState(hero1, hero2, toWhat);
			CampaignEventDispatcher.Instance.OnRomanticStateChanged(hero1, hero2, toWhat);
		}

		// Token: 0x06004D42 RID: 19778 RVA: 0x00186EE7 File Offset: 0x001850E7
		public static void Apply(Hero person1, Hero person2, Romance.RomanceLevelEnum toWhat)
		{
			ChangeRomanticStateAction.ApplyInternal(person1, person2, toWhat);
		}
	}
}
