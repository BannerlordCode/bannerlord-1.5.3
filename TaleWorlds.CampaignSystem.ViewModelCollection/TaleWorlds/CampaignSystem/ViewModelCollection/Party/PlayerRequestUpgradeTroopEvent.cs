using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x0200002D RID: 45
	public class PlayerRequestUpgradeTroopEvent : EventBase
	{
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x0001BFA2 File Offset: 0x0001A1A2
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x0001BFAA File Offset: 0x0001A1AA
		public CharacterObject SourceTroop { get; private set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x0001BFB3 File Offset: 0x0001A1B3
		// (set) Token: 0x060004AD RID: 1197 RVA: 0x0001BFBB File Offset: 0x0001A1BB
		public CharacterObject TargetTroop { get; private set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x0001BFC4 File Offset: 0x0001A1C4
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x0001BFCC File Offset: 0x0001A1CC
		public int Number { get; private set; }

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001BFD5 File Offset: 0x0001A1D5
		public PlayerRequestUpgradeTroopEvent(CharacterObject sourceTroop, CharacterObject targetTroop, int num)
		{
			this.SourceTroop = sourceTroop;
			this.TargetTroop = targetTroop;
			this.Number = num;
		}
	}
}
