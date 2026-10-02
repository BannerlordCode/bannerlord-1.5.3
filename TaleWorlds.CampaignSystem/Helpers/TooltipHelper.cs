using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000022 RID: 34
	public class TooltipHelper
	{
		// Token: 0x0600011B RID: 283 RVA: 0x0000E418 File Offset: 0x0000C618
		public static TextObject GetSendTroopsPowerContextTooltipForMapEvent()
		{
			MapEvent playerMapEvent = MapEvent.PlayerMapEvent;
			MapEvent.PowerCalculationContext simulationContext = playerMapEvent.SimulationContext;
			string text = simulationContext.ToString();
			if (simulationContext == MapEvent.PowerCalculationContext.Village || simulationContext == MapEvent.PowerCalculationContext.NavalRaid || simulationContext == MapEvent.PowerCalculationContext.RiverCrossingBattle || simulationContext == MapEvent.PowerCalculationContext.Siege)
			{
				text += ((playerMapEvent.PlayerSide == playerMapEvent.AttackerSide.MissionSide) ? "Attacker" : "Defender");
			}
			return GameTexts.FindText("str_simulation_tooltip", text);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000E482 File Offset: 0x0000C682
		public static TextObject GetSendTroopsPowerContextTooltipForSiege()
		{
			return GameTexts.FindText("str_simulation_tooltip", (PlayerSiege.PlayerSide == BattleSideEnum.Attacker) ? "SiegeAttacker" : "SiegeDefender");
		}
	}
}
