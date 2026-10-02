using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000228 RID: 552
	public static class GameEntityExtensions
	{
		// Token: 0x0600211C RID: 8476 RVA: 0x00074940 File Offset: 0x00072B40
		public static GameEntity Instantiate(Scene scene, MissionWeapon weapon, bool showHolsterWithWeapon, bool needBatchedVersion)
		{
			WeaponData weaponData = weapon.GetWeaponData(needBatchedVersion);
			WeaponStatsData[] weaponStatsData = weapon.GetWeaponStatsData();
			WeaponData ammoWeaponData = weapon.GetAmmoWeaponData(needBatchedVersion);
			WeaponStatsData[] ammoWeaponStatsData = weapon.GetAmmoWeaponStatsData();
			GameEntity gameEntity = MBAPI.IMBGameEntityExtensions.CreateFromWeapon(scene.Pointer, in weaponData, weaponStatsData, weaponStatsData.Length, in ammoWeaponData, ammoWeaponStatsData, ammoWeaponStatsData.Length, showHolsterWithWeapon);
			weaponData.DeinitializeManagedPointers();
			return gameEntity;
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x00074993 File Offset: 0x00072B93
		public static void CreateSimpleSkeleton(this GameEntity gameEntity, string skeletonName)
		{
			gameEntity.Skeleton = MBAPI.IMBSkeletonExtensions.CreateSimpleSkeleton(skeletonName);
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x000749A6 File Offset: 0x00072BA6
		public static void CreateSimpleSkeleton(this WeakGameEntity gameEntity, string skeletonName)
		{
			gameEntity.Skeleton = MBAPI.IMBSkeletonExtensions.CreateSimpleSkeleton(skeletonName);
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x000749BC File Offset: 0x00072BBC
		public static void CreateAgentSkeleton(this GameEntity gameEntity, string skeletonName, bool isHumanoid, MBActionSet actionSet, string monsterUsageSetName, Monster monster)
		{
			AnimationSystemData animationSystemData = monster.FillAnimationSystemData(actionSet, 1f, false);
			gameEntity.Skeleton = MBAPI.IMBSkeletonExtensions.CreateAgentSkeleton(skeletonName, isHumanoid, actionSet.Index, monsterUsageSetName, ref animationSystemData);
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x000749F4 File Offset: 0x00072BF4
		public static void CreateAgentSkeleton(this WeakGameEntity gameEntity, string skeletonName, bool isHumanoid, MBActionSet actionSet, string monsterUsageSetName, Monster monster)
		{
			AnimationSystemData animationSystemData = monster.FillAnimationSystemData(actionSet, 1f, false);
			gameEntity.Skeleton = MBAPI.IMBSkeletonExtensions.CreateAgentSkeleton(skeletonName, isHumanoid, actionSet.Index, monsterUsageSetName, ref animationSystemData);
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x00074A2D File Offset: 0x00072C2D
		public static void CreateSkeletonWithActionSet(this GameEntity gameEntity, ref AnimationSystemData animationSystemData)
		{
			gameEntity.Skeleton = MBSkeletonExtensions.CreateWithActionSet(ref animationSystemData);
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x00074A3B File Offset: 0x00072C3B
		public static void CreateSkeletonWithActionSet(this WeakGameEntity gameEntity, ref AnimationSystemData animationSystemData)
		{
			gameEntity.Skeleton = MBSkeletonExtensions.CreateWithActionSet(ref animationSystemData);
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x00074A4A File Offset: 0x00072C4A
		public static void FadeOut(this GameEntity gameEntity, float interval, bool isRemovingFromScene)
		{
			MBAPI.IMBGameEntityExtensions.FadeOut(gameEntity.Pointer, interval, isRemovingFromScene);
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x00074A5E File Offset: 0x00072C5E
		public static void FadeIn(this GameEntity gameEntity, bool resetAlpha = true)
		{
			MBAPI.IMBGameEntityExtensions.FadeIn(gameEntity.Pointer, resetAlpha);
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x00074A71 File Offset: 0x00072C71
		public static void HideIfNotFadingOut(this GameEntity gameEntity)
		{
			MBAPI.IMBGameEntityExtensions.HideIfNotFadingOut(gameEntity.Pointer);
		}
	}
}
