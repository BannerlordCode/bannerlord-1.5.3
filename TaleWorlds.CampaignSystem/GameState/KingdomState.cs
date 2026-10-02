using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003B0 RID: 944
	public class KingdomState : GameState
	{
		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x060036F1 RID: 14065 RVA: 0x000DF3F8 File Offset: 0x000DD5F8
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x060036F2 RID: 14066 RVA: 0x000DF3FB File Offset: 0x000DD5FB
		// (set) Token: 0x060036F3 RID: 14067 RVA: 0x000DF403 File Offset: 0x000DD603
		public Army InitialSelectedArmy { get; private set; }

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x060036F4 RID: 14068 RVA: 0x000DF40C File Offset: 0x000DD60C
		// (set) Token: 0x060036F5 RID: 14069 RVA: 0x000DF414 File Offset: 0x000DD614
		public Settlement InitialSelectedSettlement { get; private set; }

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x060036F6 RID: 14070 RVA: 0x000DF41D File Offset: 0x000DD61D
		// (set) Token: 0x060036F7 RID: 14071 RVA: 0x000DF425 File Offset: 0x000DD625
		public Clan InitialSelectedClan { get; private set; }

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x060036F8 RID: 14072 RVA: 0x000DF42E File Offset: 0x000DD62E
		// (set) Token: 0x060036F9 RID: 14073 RVA: 0x000DF436 File Offset: 0x000DD636
		public PolicyObject InitialSelectedPolicy { get; private set; }

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x060036FA RID: 14074 RVA: 0x000DF43F File Offset: 0x000DD63F
		// (set) Token: 0x060036FB RID: 14075 RVA: 0x000DF447 File Offset: 0x000DD647
		public Kingdom InitialSelectedKingdom { get; private set; }

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x060036FC RID: 14076 RVA: 0x000DF450 File Offset: 0x000DD650
		// (set) Token: 0x060036FD RID: 14077 RVA: 0x000DF458 File Offset: 0x000DD658
		public KingdomDecision InitialSelectedDecision { get; private set; }

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x060036FE RID: 14078 RVA: 0x000DF461 File Offset: 0x000DD661
		// (set) Token: 0x060036FF RID: 14079 RVA: 0x000DF469 File Offset: 0x000DD669
		public IKingdomStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x000DF472 File Offset: 0x000DD672
		public KingdomState()
		{
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x000DF47A File Offset: 0x000DD67A
		public KingdomState(KingdomDecision initialSelectedDecision)
		{
			this.InitialSelectedDecision = initialSelectedDecision;
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x000DF489 File Offset: 0x000DD689
		public KingdomState(Army initialSelectedArmy)
		{
			this.InitialSelectedArmy = initialSelectedArmy;
		}

		// Token: 0x06003703 RID: 14083 RVA: 0x000DF498 File Offset: 0x000DD698
		public KingdomState(Settlement initialSelectedSettlement)
		{
			this.InitialSelectedSettlement = initialSelectedSettlement;
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x000DF4A8 File Offset: 0x000DD6A8
		public KingdomState(IFaction initialSelectedFaction)
		{
			Clan clan;
			if ((clan = initialSelectedFaction as Clan) != null)
			{
				this.InitialSelectedClan = clan;
				return;
			}
			Kingdom kingdom;
			if ((kingdom = initialSelectedFaction as Kingdom) != null)
			{
				this.InitialSelectedKingdom = kingdom;
			}
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x000DF4DE File Offset: 0x000DD6DE
		public KingdomState(PolicyObject initialSelectedPolicy)
		{
			this.InitialSelectedPolicy = initialSelectedPolicy;
		}

		// Token: 0x04000F74 RID: 3956
		private IKingdomStateHandler _handler;
	}
}
