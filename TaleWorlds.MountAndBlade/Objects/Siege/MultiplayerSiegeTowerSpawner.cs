using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003C1 RID: 961
	public class MultiplayerSiegeTowerSpawner : SiegeTowerSpawner
	{
		// Token: 0x06003628 RID: 13864 RVA: 0x000DF5B7 File Offset: 0x000DD7B7
		public override void AssignParameters(SpawnerEntityMissionHelper _spawnerMissionHelper)
		{
			base.AssignParameters(_spawnerMissionHelper);
			_spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<DestructableComponent>().MaxHitPoint = 15000f;
			SiegeTower firstScriptOfType = _spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<SiegeTower>();
			firstScriptOfType.MaxSpeed = 1f;
			firstScriptOfType.MinSpeed = 0.5f;
		}

		// Token: 0x0400172C RID: 5932
		private const float MaxHitPoint = 15000f;

		// Token: 0x0400172D RID: 5933
		private const float MinimumSpeed = 0.5f;

		// Token: 0x0400172E RID: 5934
		private const float MaximumSpeed = 1f;
	}
}
