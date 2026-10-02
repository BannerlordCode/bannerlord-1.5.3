using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer
{
	// Token: 0x02000092 RID: 146
	public class MissionCustomBattlePreloadView : MissionView
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x000276C0 File Offset: 0x000258C0
		public override void OnPreMissionTick(float dt)
		{
			if (!this._preloadDone)
			{
				MissionCombatantsLogic missionBehavior = base.Mission.GetMissionBehavior<MissionCombatantsLogic>();
				List<BasicCharacterObject> list = new List<BasicCharacterObject>();
				foreach (IBattleCombatant battleCombatant in missionBehavior.GetAllCombatants())
				{
					list.AddRange(((CustomBattleCombatant)battleCombatant).Characters);
				}
				this._helperInstance.PreloadCharacters(list);
				SiegeDeploymentMissionController missionBehavior2 = Mission.Current.GetMissionBehavior<SiegeDeploymentMissionController>();
				if (missionBehavior2 != null)
				{
					this._helperInstance.PreloadItems(missionBehavior2.GetSiegeMissiles());
				}
				this._preloadDone = true;
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00027764 File Offset: 0x00025964
		public override void OnSceneRenderingStarted()
		{
			this._helperInstance.WaitForMeshesToBeLoaded();
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00027771 File Offset: 0x00025971
		public override void OnMissionStateDeactivated()
		{
			base.OnMissionStateDeactivated();
			this._helperInstance.Clear();
		}

		// Token: 0x040002FF RID: 767
		private PreloadHelper _helperInstance = new PreloadHelper();

		// Token: 0x04000300 RID: 768
		private bool _preloadDone;
	}
}
