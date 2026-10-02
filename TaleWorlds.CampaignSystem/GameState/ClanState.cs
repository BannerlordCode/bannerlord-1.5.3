using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003A5 RID: 933
	public class ClanState : GameState
	{
		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x060036AB RID: 13995 RVA: 0x000DF195 File Offset: 0x000DD395
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x060036AC RID: 13996 RVA: 0x000DF198 File Offset: 0x000DD398
		// (set) Token: 0x060036AD RID: 13997 RVA: 0x000DF1A0 File Offset: 0x000DD3A0
		public Hero InitialSelectedHero { get; private set; }

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x060036AE RID: 13998 RVA: 0x000DF1A9 File Offset: 0x000DD3A9
		// (set) Token: 0x060036AF RID: 13999 RVA: 0x000DF1B1 File Offset: 0x000DD3B1
		public PartyBase InitialSelectedParty { get; private set; }

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x060036B0 RID: 14000 RVA: 0x000DF1BA File Offset: 0x000DD3BA
		// (set) Token: 0x060036B1 RID: 14001 RVA: 0x000DF1C2 File Offset: 0x000DD3C2
		public Settlement InitialSelectedSettlement { get; private set; }

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x060036B2 RID: 14002 RVA: 0x000DF1CB File Offset: 0x000DD3CB
		// (set) Token: 0x060036B3 RID: 14003 RVA: 0x000DF1D3 File Offset: 0x000DD3D3
		public Workshop InitialSelectedWorkshop { get; private set; }

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x060036B4 RID: 14004 RVA: 0x000DF1DC File Offset: 0x000DD3DC
		// (set) Token: 0x060036B5 RID: 14005 RVA: 0x000DF1E4 File Offset: 0x000DD3E4
		public Alley InitialSelectedAlley { get; private set; }

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x060036B6 RID: 14006 RVA: 0x000DF1ED File Offset: 0x000DD3ED
		// (set) Token: 0x060036B7 RID: 14007 RVA: 0x000DF1F5 File Offset: 0x000DD3F5
		public IClanStateHandler Handler
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

		// Token: 0x060036B8 RID: 14008 RVA: 0x000DF1FE File Offset: 0x000DD3FE
		public ClanState()
		{
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x000DF206 File Offset: 0x000DD406
		public ClanState(Hero initialSelectedHero)
		{
			this.InitialSelectedHero = initialSelectedHero;
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x000DF215 File Offset: 0x000DD415
		public ClanState(PartyBase initialSelectedParty)
		{
			this.InitialSelectedParty = initialSelectedParty;
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x000DF224 File Offset: 0x000DD424
		public ClanState(Settlement initialSelectedSettlement)
		{
			this.InitialSelectedSettlement = initialSelectedSettlement;
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x000DF233 File Offset: 0x000DD433
		public ClanState(Workshop initialSelectedWorkshop)
		{
			this.InitialSelectedWorkshop = initialSelectedWorkshop;
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x000DF242 File Offset: 0x000DD442
		public ClanState(Alley initialSelectedAlley)
		{
			this.InitialSelectedAlley = initialSelectedAlley;
		}

		// Token: 0x04000F63 RID: 3939
		private IClanStateHandler _handler;
	}
}
