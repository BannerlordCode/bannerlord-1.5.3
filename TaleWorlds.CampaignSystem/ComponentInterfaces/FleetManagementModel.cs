using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A7 RID: 423
	public abstract class FleetManagementModel : MBGameModel<FleetManagementModel>
	{
		// Token: 0x06001D33 RID: 7475
		public abstract bool CanTroopsReturn();

		// Token: 0x06001D34 RID: 7476
		public abstract CampaignTime GetReturnTimeForTroops(Ship ship);

		// Token: 0x06001D35 RID: 7477
		public abstract bool CanSendShipToPlayerClan(Ship ship, int playerShipsCount, int troopsCountToSend, out TextObject hint);

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06001D36 RID: 7478
		public abstract int MinimumTroopCountRequiredToSendShips { get; }
	}
}
