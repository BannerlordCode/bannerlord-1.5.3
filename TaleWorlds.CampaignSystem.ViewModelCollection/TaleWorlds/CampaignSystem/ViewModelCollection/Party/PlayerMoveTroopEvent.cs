using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x0200002F RID: 47
	public class PlayerMoveTroopEvent : EventBase
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x0001C012 File Offset: 0x0001A212
		// (set) Token: 0x060004B5 RID: 1205 RVA: 0x0001C01A File Offset: 0x0001A21A
		public CharacterObject Troop { get; private set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x0001C023 File Offset: 0x0001A223
		// (set) Token: 0x060004B7 RID: 1207 RVA: 0x0001C02B File Offset: 0x0001A22B
		public int Amount { get; private set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x0001C034 File Offset: 0x0001A234
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x0001C03C File Offset: 0x0001A23C
		public bool IsPrisoner { get; private set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x0001C045 File Offset: 0x0001A245
		// (set) Token: 0x060004BB RID: 1211 RVA: 0x0001C04D File Offset: 0x0001A24D
		public PartyScreenLogic.PartyRosterSide FromSide { get; private set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0001C056 File Offset: 0x0001A256
		// (set) Token: 0x060004BD RID: 1213 RVA: 0x0001C05E File Offset: 0x0001A25E
		public PartyScreenLogic.PartyRosterSide ToSide { get; private set; }

		// Token: 0x060004BE RID: 1214 RVA: 0x0001C067 File Offset: 0x0001A267
		public PlayerMoveTroopEvent(CharacterObject troop, PartyScreenLogic.PartyRosterSide fromSide, PartyScreenLogic.PartyRosterSide toSide, int amount, bool isPrisoner)
		{
			this.Troop = troop;
			this.FromSide = fromSide;
			this.ToSide = toSide;
			this.IsPrisoner = isPrisoner;
			this.Amount = amount;
		}
	}
}
