using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Roster;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003AD RID: 941
	public interface IMenuContextHandler
	{
		// Token: 0x060036D8 RID: 14040
		void OnBackgroundMeshNameSet(string name);

		// Token: 0x060036D9 RID: 14041
		void OnOpenTownManagement();

		// Token: 0x060036DA RID: 14042
		void OnOpenRecruitVolunteers();

		// Token: 0x060036DB RID: 14043
		void OnOpenTournamentLeaderboard();

		// Token: 0x060036DC RID: 14044
		void OnOpenTroopSelection(TroopRoster fullRoster, TroopRoster initialSelections, Func<CharacterObject, bool> canChangeStatusOfTroop, Action<TroopRoster> onDone, int maxSelectableTroopCount, int minSelectableTroopCount);

		// Token: 0x060036DD RID: 14045
		void OnOpenNavalTroopSelection(TroopRoster fullRoster, TroopRoster initialTroopSelections, List<Ship> eligibleShips, List<Ship> initialShipSelections, Func<CharacterObject, bool> canChangeStatusOfTroop, Action<TroopRoster, List<Ship>> onDone, int minSelectableTroopCount, int minSelectableShipCount, int maxSelectableShipCount, bool anyOtherPartiesOnPlayerSide);

		// Token: 0x060036DE RID: 14046
		void OnMenuCreate();

		// Token: 0x060036DF RID: 14047
		void OnMenuActivate();

		// Token: 0x060036E0 RID: 14048
		void OnMenuRefresh();

		// Token: 0x060036E1 RID: 14049
		void OnHourlyTick();

		// Token: 0x060036E2 RID: 14050
		void OnPanelSoundIDSet(string panelSoundID);

		// Token: 0x060036E3 RID: 14051
		void OnAmbientSoundIDSet(string ambientSoundID);
	}
}
