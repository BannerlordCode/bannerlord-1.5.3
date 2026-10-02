using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000296 RID: 662
	public class MissionHardBorderPlacer : MissionLogic
	{
		// Token: 0x06002500 RID: 9472 RVA: 0x00086F30 File Offset: 0x00085130
		public override void EarlyStart()
		{
			base.EarlyStart();
			Scene scene = base.Mission.Scene;
			GameEntity gameEntity = GameEntity.CreateEmpty(scene, true, true, true);
			scene.FillEntityWithHardBorderPhysicsBarrier(gameEntity);
		}
	}
}
