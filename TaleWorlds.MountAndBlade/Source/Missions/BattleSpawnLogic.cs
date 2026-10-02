using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.Source.Missions
{
	// Token: 0x020003D9 RID: 985
	public class BattleSpawnLogic : MissionLogic
	{
		// Token: 0x0600372E RID: 14126 RVA: 0x000E4AC8 File Offset: 0x000E2CC8
		public BattleSpawnLogic(string selectedSpawnPointSetTag)
		{
			this._selectedSpawnPointSetTag = selectedSpawnPointSetTag;
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x000E4AD8 File Offset: 0x000E2CD8
		public override void OnPreMissionTick(float dt)
		{
			if (this._isScenePrepared)
			{
				return;
			}
			WeakGameEntity weakGameEntity = base.Mission.Scene.FindWeakEntityWithTag(this._selectedSpawnPointSetTag);
			if (weakGameEntity != null)
			{
				List<WeakGameEntity> list = base.Mission.Scene.FindWeakEntitiesWithTag("spawnpoint_set").ToList<WeakGameEntity>();
				list.Remove(weakGameEntity);
				foreach (WeakGameEntity weakGameEntity2 in list)
				{
					weakGameEntity2.Remove(76);
				}
			}
			this._isScenePrepared = true;
		}

		// Token: 0x040017C9 RID: 6089
		public const string BattleTag = "battle_set";

		// Token: 0x040017CA RID: 6090
		public const string SallyOutTag = "sally_out_set";

		// Token: 0x040017CB RID: 6091
		public const string ReliefForceAttackTag = "relief_force_attack_set";

		// Token: 0x040017CC RID: 6092
		private const string SpawnPointSetCommonTag = "spawnpoint_set";

		// Token: 0x040017CD RID: 6093
		private readonly string _selectedSpawnPointSetTag;

		// Token: 0x040017CE RID: 6094
		private bool _isScenePrepared;
	}
}
