using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000029 RID: 41
	public static class PortStateHelper
	{
		// Token: 0x06000182 RID: 386 RVA: 0x00011A88 File Offset: 0x0000FC88
		public static void OpenAsTrade(Town town)
		{
			PortState portState = GameStateManager.Current.CreateState<PortState>(new object[]
			{
				town.Settlement.Party,
				PartyBase.MainParty,
				PortScreenModes.TradeMode
			});
			GameStateManager.Current.PushState(portState, 0);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00011AD4 File Offset: 0x0000FCD4
		public static void OpenAsLoot(MBReadOnlyList<Ship> lootShips, Action onEndAction = null)
		{
			PortState portState = GameStateManager.Current.CreateState<PortState>(new object[]
			{
				null,
				PartyBase.MainParty,
				lootShips,
				PartyBase.MainParty.Ships,
				onEndAction,
				PortScreenModes.LootMode
			});
			GameStateManager.Current.PushState(portState, 0);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00011B24 File Offset: 0x0000FD24
		public static void OpenAsRestricted(Town town, TextObject restrictedReason)
		{
			PortState portState = GameStateManager.Current.CreateState<PortState>(new object[]
			{
				town.Settlement.Party,
				PartyBase.MainParty,
				PortScreenModes.Restricted
			});
			GameStateManager.Current.PushState(portState, 0);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00011B70 File Offset: 0x0000FD70
		public static void OpenAsStoryMode(Settlement settlement)
		{
			PortState portState = GameStateManager.Current.CreateState<PortState>(new object[]
			{
				settlement,
				PartyBase.MainParty,
				PortScreenModes.Story
			});
			GameStateManager.Current.PushState(portState, 0);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00011BB0 File Offset: 0x0000FDB0
		public static void OpenAsManageFleet(MBReadOnlyList<Ship> leftShips)
		{
			PortState portState = GameStateManager.Current.CreateState<PortState>(new object[]
			{
				null,
				PartyBase.MainParty,
				leftShips,
				PartyBase.MainParty.Ships,
				PortScreenModes.Manage
			});
			GameStateManager.Current.PushState(portState, 0);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00011BFC File Offset: 0x0000FDFC
		public static void OpenAsManageOtherFleet(PartyBase other, Action onEndAction)
		{
			PortState portState = GameStateManager.Current.CreateState<PortState>(new object[]
			{
				other,
				PartyBase.MainParty,
				onEndAction,
				PortScreenModes.ManageOther
			});
			GameStateManager.Current.PushState(portState, 0);
		}
	}
}
