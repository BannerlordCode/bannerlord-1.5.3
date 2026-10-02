using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Source.Missions
{
	// Token: 0x020003E1 RID: 993
	public class SimpleMountedPlayerMissionController : MissionLogic
	{
		// Token: 0x0600374E RID: 14158 RVA: 0x000E5AC8 File Offset: 0x000E3CC8
		public override void EarlyStart()
		{
			base.EarlyStart();
			foreach (MissionObject missionObject in base.Mission.ActiveMissionObjects.ToList<MissionObject>())
			{
				missionObject.SetDisabled(true);
			}
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x000E5B2C File Offset: 0x000E3D2C
		public override void AfterStart()
		{
			BasicCharacterObject @object = this._game.ObjectManager.GetObject<BasicCharacterObject>("aserai_tribal_horseman");
			WeakGameEntity weakGameEntity = Mission.Current.Scene.FindWeakEntityWithTag("spawnpoint_player_test");
			if (!weakGameEntity.IsValid)
			{
				weakGameEntity = Mission.Current.Scene.FindWeakEntityWithTag("spawnpoint_player");
			}
			MatrixFrame matrixFrame = (weakGameEntity.IsValid ? weakGameEntity.GetGlobalFrame() : MatrixFrame.Identity);
			AgentBuildData agentBuildData = new AgentBuildData(new BasicBattleAgentOrigin(@object));
			AgentBuildData agentBuildData2 = agentBuildData.InitialPosition(in matrixFrame.origin);
			Vec2 vec = matrixFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			agentBuildData2.InitialDirection(in vec).Controller(AgentControllerType.Player);
			base.Mission.SpawnAgent(agentBuildData, false, null, null).WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x000E5BEF File Offset: 0x000E3DEF
		public override void OnAfterMissionLoadingFinished()
		{
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x000E5BFD File Offset: 0x000E3DFD
		public override bool MissionEnded(ref MissionResult missionResult)
		{
			return base.Mission.InputManager.IsGameKeyPressed(4);
		}

		// Token: 0x040017E2 RID: 6114
		private const string TestPlayerSpawnPoint = "spawnpoint_player_test";

		// Token: 0x040017E3 RID: 6115
		private const string PlayerSpawnPoint = "spawnpoint_player";

		// Token: 0x040017E4 RID: 6116
		private readonly Game _game = Game.Current;
	}
}
