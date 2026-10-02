using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003BC RID: 956
	public class MultiplayerBatteringRamSpawner : BatteringRamSpawner
	{
		// Token: 0x0600361E RID: 13854 RVA: 0x000DF4FC File Offset: 0x000DD6FC
		public override void AssignParameters(SpawnerEntityMissionHelper _spawnerMissionHelper)
		{
			base.AssignParameters(_spawnerMissionHelper);
			_spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<DestructableComponent>().MaxHitPoint = 12000f;
			BatteringRam firstScriptOfType = _spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<BatteringRam>();
			firstScriptOfType.MaxSpeed *= 1f;
			firstScriptOfType.MinSpeed *= 1f;
		}

		// Token: 0x0400172A RID: 5930
		private const float MaxHitPoint = 12000f;

		// Token: 0x0400172B RID: 5931
		private const float SpeedMultiplier = 1f;
	}
}
