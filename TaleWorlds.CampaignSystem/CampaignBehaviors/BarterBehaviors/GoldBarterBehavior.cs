using System;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.BarterBehaviors
{
	// Token: 0x0200048A RID: 1162
	public class GoldBarterBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004B09 RID: 19209 RVA: 0x0017AA24 File Offset: 0x00178C24
		public override void RegisterEvents()
		{
			CampaignEvents.BarterablesRequested.AddNonSerializedListener(this, new Action<BarterData>(this.CheckForBarters));
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x0017AA3D File Offset: 0x00178C3D
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004B0B RID: 19211 RVA: 0x0017AA40 File Offset: 0x00178C40
		public void CheckForBarters(BarterData args)
		{
			if ((args.OffererHero != null && args.OtherHero != null && args.OffererHero.Clan != args.OtherHero.Clan) || (args.OffererHero == null && args.OffererParty != null) || (args.OtherHero == null && args.OtherParty != null))
			{
				int num = ((args.OffererHero != null) ? args.OffererHero.Gold : args.OffererParty.MobileParty.PartyTradeGold);
				int num2 = ((args.OtherHero != null) ? args.OtherHero.Gold : args.OtherParty.MobileParty.PartyTradeGold);
				Barterable barterable = new GoldBarterable(args.OffererHero, args.OtherHero, args.OffererParty, args.OtherParty, num);
				args.AddBarterable<GoldBarterGroup>(barterable, false);
				Barterable barterable2 = new GoldBarterable(args.OtherHero, args.OffererHero, args.OtherParty, args.OffererParty, num2);
				args.AddBarterable<GoldBarterGroup>(barterable2, false);
			}
		}
	}
}
