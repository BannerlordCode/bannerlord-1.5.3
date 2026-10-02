using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011E RID: 286
	public class DefaultFleetManagementModel : FleetManagementModel
	{
		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001895 RID: 6293 RVA: 0x00076C51 File Offset: 0x00074E51
		public override int MinimumTroopCountRequiredToSendShips
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x00076C54 File Offset: 0x00074E54
		public override bool CanSendShipToPlayerClan(Ship ship, int playerShipsCount, int troopsCountToSend, out TextObject hint)
		{
			hint = TextObject.GetEmpty();
			return false;
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x00076C5F File Offset: 0x00074E5F
		public override bool CanTroopsReturn()
		{
			return false;
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x00076C62 File Offset: 0x00074E62
		public override CampaignTime GetReturnTimeForTroops(Ship ship)
		{
			return CampaignTime.Never;
		}
	}
}
