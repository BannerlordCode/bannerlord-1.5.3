using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.Source.Missions
{
	// Token: 0x020003DD RID: 989
	public class EquipmentTestMissionController : MissionLogic
	{
		// Token: 0x06003738 RID: 14136 RVA: 0x000E5630 File Offset: 0x000E3830
		public override void AfterStart()
		{
			base.AfterStart();
			WeakGameEntity weakGameEntity = base.Mission.Scene.FindWeakEntityWithTag("spawnpoint_player");
			base.Mission.SpawnAgent(new AgentBuildData(Game.Current.PlayerTroop).Team(base.Mission.AttackerTeam).InitialFrameFromSpawnPointEntity(weakGameEntity).CivilianEquipment(false)
				.Controller(AgentControllerType.Player), false, null, null);
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x000E5699 File Offset: 0x000E3899
		public override void OnAfterMissionLoadingFinished()
		{
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}
	}
}
