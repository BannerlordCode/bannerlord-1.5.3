using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200004F RID: 79
	public static class GameEntityPhysicsExtensions
	{
		// Token: 0x060007FF RID: 2047 RVA: 0x00005DE2 File Offset: 0x00003FE2
		public static bool HasBody(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.HasBody(gameEntity.Pointer);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00005DF4 File Offset: 0x00003FF4
		public static bool HasBody(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.HasBody(gameEntity.Pointer);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00005E07 File Offset: 0x00004007
		public static void AddSphereAsBody(this GameEntity gameEntity, Vec3 sphere, float radius, BodyFlags bodyFlags)
		{
			EngineApplicationInterface.IGameEntity.AddSphereAsBody(gameEntity.Pointer, sphere, radius, (uint)bodyFlags);
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00005E1C File Offset: 0x0000401C
		public static void AddCapsuleAsBody(this GameEntity gameEntity, Vec3 p1, Vec3 p2, float radius, BodyFlags bodyFlags, string physicsMaterialName = "")
		{
			EngineApplicationInterface.IGameEntity.AddCapsuleAsBody(gameEntity.Pointer, p1, p2, radius, (uint)bodyFlags, physicsMaterialName);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00005E35 File Offset: 0x00004035
		public static void UpdateBodyRestOffset(this WeakGameEntity gameEntity, float restOffset)
		{
			EngineApplicationInterface.IGameEntity.UpdateBodyRestOffset(gameEntity.Pointer, restOffset);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00005E49 File Offset: 0x00004049
		public static void PushCapsuleShapeToEntityBody(this WeakGameEntity gameEntity, Vec3 p1, Vec3 p2, float radius, string physicsMaterialName)
		{
			EngineApplicationInterface.IGameEntity.PushCapsuleShapeToEntityBody(gameEntity.Pointer, p1, p2, radius, physicsMaterialName);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00005E61 File Offset: 0x00004061
		public static void AddSphereAsBody(this WeakGameEntity gameEntity, Vec3 sphere, float radius, BodyFlags bodyFlags)
		{
			EngineApplicationInterface.IGameEntity.AddSphereAsBody(gameEntity.Pointer, sphere, radius, (uint)bodyFlags);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00005E77 File Offset: 0x00004077
		public static void AddCapsuleAsBody(this WeakGameEntity gameEntity, Vec3 p1, Vec3 p2, float radius, BodyFlags bodyFlags, string physicsMaterialName = "")
		{
			EngineApplicationInterface.IGameEntity.AddCapsuleAsBody(gameEntity.Pointer, p1, p2, radius, (uint)bodyFlags, physicsMaterialName);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00005E91 File Offset: 0x00004091
		public static void PopCapsuleShapeFromEntityBody(this WeakGameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.PopCapsuleShapeFromEntityBody(gameEntity.Pointer);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00005EA4 File Offset: 0x000040A4
		public static void RemovePhysics(this GameEntity gameEntity, bool clearingTheScene = false)
		{
			EngineApplicationInterface.IGameEntity.RemovePhysics(gameEntity.Pointer, clearingTheScene);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00005EB7 File Offset: 0x000040B7
		public static void RemovePhysics(this WeakGameEntity gameEntity, bool clearingTheScene = false)
		{
			EngineApplicationInterface.IGameEntity.RemovePhysics(gameEntity.Pointer, clearingTheScene);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00005ECB File Offset: 0x000040CB
		public static bool GetPhysicsState(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetPhysicsState(gameEntity.Pointer);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00005EDD File Offset: 0x000040DD
		public static bool GetPhysicsState(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetPhysicsState(gameEntity.Pointer);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00005EF0 File Offset: 0x000040F0
		public static int GetPhysicsTriangleCount(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetPhysicsTriangleCount(gameEntity.Pointer);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00005F03 File Offset: 0x00004103
		public static int GetPhysicsTriangleCount(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetPhysicsTriangleCount(gameEntity.Pointer);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00005F15 File Offset: 0x00004115
		public static bool HasPhysicsDefinitionWithoutFlags(this GameEntity gameEntity, int excludeFlags)
		{
			return EngineApplicationInterface.IGameEntity.HasPhysicsDefinition(gameEntity.Pointer, excludeFlags);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00005F28 File Offset: 0x00004128
		public static bool HasPhysicsDefinitionWithoutFlags(this WeakGameEntity gameEntity, int excludeFlags)
		{
			return EngineApplicationInterface.IGameEntity.HasPhysicsDefinition(gameEntity.Pointer, excludeFlags);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00005F3C File Offset: 0x0000413C
		public static bool HasPhysicsBody(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.HasPhysicsBody(gameEntity.Pointer);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00005F4E File Offset: 0x0000414E
		public static bool HasPhysicsBody(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.HasPhysicsBody(gameEntity.Pointer);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00005F61 File Offset: 0x00004161
		public static bool HasDynamicRigidBody(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.HasDynamicRigidBody(gameEntity.Pointer);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00005F73 File Offset: 0x00004173
		public static bool HasDynamicRigidBody(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.HasDynamicRigidBody(gameEntity.Pointer);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x00005F86 File Offset: 0x00004186
		public static bool HasKinematicRigidBody(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.HasKinematicRigidBody(gameEntity.Pointer);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00005F98 File Offset: 0x00004198
		public static bool HasKinematicRigidBody(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.HasKinematicRigidBody(gameEntity.Pointer);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00005FAB File Offset: 0x000041AB
		public static bool HasStaticPhysicsBody(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.HasStaticPhysicsBody(gameEntity.Pointer);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00005FBD File Offset: 0x000041BD
		public static bool HasStaticPhysicsBody(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.HasStaticPhysicsBody(gameEntity.Pointer);
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00005FD0 File Offset: 0x000041D0
		public static bool HasDynamicRigidBodyAndActiveSimulation(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.HasDynamicRigidBodyAndActiveSimulation(gameEntity.Pointer);
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00005FE2 File Offset: 0x000041E2
		public static bool HasDynamicRigidBodyAndActiveSimulation(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.HasDynamicRigidBodyAndActiveSimulation(gameEntity.Pointer);
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00005FF5 File Offset: 0x000041F5
		public static void CreateVariableRatePhysics(this GameEntity gameEntity, bool forChildren)
		{
			EngineApplicationInterface.IGameEntity.CreateVariableRatePhysics(gameEntity.Pointer, forChildren);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00006008 File Offset: 0x00004208
		public static void CreateVariableRatePhysics(this WeakGameEntity gameEntity, bool forChildren)
		{
			EngineApplicationInterface.IGameEntity.CreateVariableRatePhysics(gameEntity.Pointer, forChildren);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0000601C File Offset: 0x0000421C
		public static void SetPhysicsState(this GameEntity gameEntity, bool isEnabled, bool setChildren)
		{
			EngineApplicationInterface.IGameEntity.SetPhysicsState(gameEntity.Pointer, isEnabled, setChildren);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00006030 File Offset: 0x00004230
		public static void SetPhysicsState(this WeakGameEntity gameEntity, bool isEnabled, bool setChildren)
		{
			EngineApplicationInterface.IGameEntity.SetPhysicsState(gameEntity.Pointer, isEnabled, setChildren);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00006045 File Offset: 0x00004245
		public static void SetPhysicsStateOnlyVariable(this GameEntity gameEntity, bool isEnabled, bool setChildren)
		{
			EngineApplicationInterface.IGameEntity.SetPhysicsStateOnlyVariable(gameEntity.Pointer, isEnabled, setChildren);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00006059 File Offset: 0x00004259
		public static void SetPhysicsStateOnlyVariable(this WeakGameEntity gameEntity, bool isEnabled, bool setChildren)
		{
			EngineApplicationInterface.IGameEntity.SetPhysicsStateOnlyVariable(gameEntity.Pointer, isEnabled, setChildren);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0000606E File Offset: 0x0000426E
		public static void RemoveEnginePhysics(this GameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.RemoveEnginePhysics(gameEntity.Pointer);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00006080 File Offset: 0x00004280
		public static void RemoveEnginePhysics(this WeakGameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.RemoveEnginePhysics(gameEntity.Pointer);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00006093 File Offset: 0x00004293
		public static bool IsEngineBodySleeping(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.IsEngineBodySleeping(gameEntity.Pointer);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x000060A5 File Offset: 0x000042A5
		public static bool IsEngineBodySleeping(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.IsEngineBodySleeping(gameEntity.Pointer);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x000060B8 File Offset: 0x000042B8
		public static bool IsDynamicBodyStationary(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.IsDynamicBodyStationary(gameEntity.Pointer);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x000060CA File Offset: 0x000042CA
		public static bool IsDynamicBodyStationary(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.IsDynamicBodyStationary(gameEntity.Pointer);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x000060DD File Offset: 0x000042DD
		public static bool IsDynamicBodyStationaryMT(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.IsDynamicBodyStationary(gameEntity.Pointer);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x000060EF File Offset: 0x000042EF
		public static bool IsDynamicBodyStationaryMT(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.IsDynamicBodyStationary(gameEntity.Pointer);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00006104 File Offset: 0x00004304
		public static void ReplacePhysicsBodyWithQuadPhysicsBody(this GameEntity gameEntity, UIntPtr vertices, int numberOfVertices, PhysicsMaterial physicsMaterial, BodyFlags bodyFlags, UIntPtr indices, int numberOfIndices, bool replaceTrianglemeshDescriptions = false)
		{
			EngineApplicationInterface.IGameEntity.ReplacePhysicsBodyWithQuadPhysicsBody(gameEntity.Pointer, vertices, physicsMaterial.Index, bodyFlags, numberOfVertices, indices, numberOfIndices, replaceTrianglemeshDescriptions);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00006134 File Offset: 0x00004334
		public static void ReplacePhysicsBodyWithQuadPhysicsBody(this WeakGameEntity gameEntity, UIntPtr vertices, int numberOfVertices, PhysicsMaterial physicsMaterial, BodyFlags bodyFlags, UIntPtr indices, int numberOfIndices, bool replaceTrianglemeshDescriptions = false)
		{
			EngineApplicationInterface.IGameEntity.ReplacePhysicsBodyWithQuadPhysicsBody(gameEntity.Pointer, vertices, physicsMaterial.Index, bodyFlags, numberOfVertices, indices, numberOfIndices, replaceTrianglemeshDescriptions);
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00006162 File Offset: 0x00004362
		public static PhysicsShape GetBodyShape(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetBodyShape(gameEntity.Pointer);
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00006174 File Offset: 0x00004374
		public static PhysicsShape GetBodyShape(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetBodyShape(gameEntity.Pointer);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00006187 File Offset: 0x00004387
		public static void SetBodyShape(this GameEntity gameEntity, PhysicsShape shape)
		{
			EngineApplicationInterface.IGameEntity.SetBodyShape(gameEntity.Pointer, (shape == null) ? ((UIntPtr)0UL) : shape.Pointer);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000061B1 File Offset: 0x000043B1
		public static void SetBodyShape(this WeakGameEntity gameEntity, PhysicsShape shape)
		{
			EngineApplicationInterface.IGameEntity.SetBodyShape(gameEntity.Pointer, (shape == null) ? ((UIntPtr)0UL) : shape.Pointer);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x000061DC File Offset: 0x000043DC
		public static void AddPhysics(this GameEntity gameEntity, float mass, Vec3 localCenterOfMass, PhysicsShape body, Vec3 initialGlobalVelocity, Vec3 angularGlobalVelocity, PhysicsMaterial physicsMaterial, bool isStatic, int collisionGroupID)
		{
			EngineApplicationInterface.IGameEntity.AddPhysics(gameEntity.Pointer, (body != null) ? body.Pointer : UIntPtr.Zero, mass, ref localCenterOfMass, ref initialGlobalVelocity, ref angularGlobalVelocity, physicsMaterial.Index, isStatic, collisionGroupID);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00006224 File Offset: 0x00004424
		public static void AddPhysics(this WeakGameEntity gameEntity, float mass, Vec3 localCenterOfMass, PhysicsShape body, Vec3 initialVelocity, Vec3 angularVelocity, PhysicsMaterial physicsMaterial, bool isStatic, int collisionGroupID)
		{
			EngineApplicationInterface.IGameEntity.AddPhysics(gameEntity.Pointer, (body != null) ? body.Pointer : UIntPtr.Zero, mass, ref localCenterOfMass, ref initialVelocity, ref angularVelocity, physicsMaterial.Index, isStatic, collisionGroupID);
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0000626A File Offset: 0x0000446A
		public static void SetVelocityLimits(this GameEntity gameEntity, float maxLinearVelocity, float maxAngularVelocity)
		{
			EngineApplicationInterface.IGameEntity.SetVelocityLimits(gameEntity.Pointer, maxLinearVelocity, maxAngularVelocity);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0000627E File Offset: 0x0000447E
		public static void SetVelocityLimits(this WeakGameEntity gameEntity, float maxLinearVelocity, float maxAngularVelocity)
		{
			EngineApplicationInterface.IGameEntity.SetVelocityLimits(gameEntity.Pointer, maxLinearVelocity, maxAngularVelocity);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00006293 File Offset: 0x00004493
		public static void SetMaxDepenetrationVelocity(this GameEntity gameEntity, float maxDepenetrationVelocity)
		{
			EngineApplicationInterface.IGameEntity.SetMaxDepenetrationVelocity(gameEntity.Pointer, maxDepenetrationVelocity);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x000062A6 File Offset: 0x000044A6
		public static void SetMaxDepenetrationVelocity(this WeakGameEntity gameEntity, float maxDepenetrationVelocity)
		{
			EngineApplicationInterface.IGameEntity.SetMaxDepenetrationVelocity(gameEntity.Pointer, maxDepenetrationVelocity);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x000062BA File Offset: 0x000044BA
		public static void SetSolverIterationCounts(this GameEntity gameEntity, int positionIterationCount, int velocityIterationCount)
		{
			EngineApplicationInterface.IGameEntity.SetSolverIterationCounts(gameEntity.Pointer, positionIterationCount, velocityIterationCount);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x000062CE File Offset: 0x000044CE
		public static void SetSolverIterationCounts(this WeakGameEntity gameEntity, int positionIterationCount, int velocityIterationCount)
		{
			EngineApplicationInterface.IGameEntity.SetSolverIterationCounts(gameEntity.Pointer, positionIterationCount, velocityIterationCount);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x000062E3 File Offset: 0x000044E3
		public static void ApplyLocalImpulseToDynamicBody(this GameEntity gameEntity, Vec3 localPosition, Vec3 impulse)
		{
			EngineApplicationInterface.IGameEntity.ApplyLocalImpulseToDynamicBody(gameEntity.Pointer, ref localPosition, ref impulse);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x000062F9 File Offset: 0x000044F9
		public static void ApplyLocalImpulseToDynamicBody(this WeakGameEntity gameEntity, Vec3 localPosition, Vec3 impulse)
		{
			EngineApplicationInterface.IGameEntity.ApplyLocalImpulseToDynamicBody(gameEntity.Pointer, ref localPosition, ref impulse);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00006310 File Offset: 0x00004510
		public static void ApplyForceToDynamicBody(this GameEntity gameEntity, Vec3 force, GameEntityPhysicsExtensions.ForceMode forceMode)
		{
			EngineApplicationInterface.IGameEntity.ApplyForceToDynamicBody(gameEntity.Pointer, ref force, forceMode);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00006325 File Offset: 0x00004525
		public static void ApplyForceToDynamicBody(this WeakGameEntity gameEntity, Vec3 force, GameEntityPhysicsExtensions.ForceMode forceMode)
		{
			EngineApplicationInterface.IGameEntity.ApplyForceToDynamicBody(gameEntity.Pointer, ref force, forceMode);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0000633B File Offset: 0x0000453B
		public static void ApplyGlobalForceAtLocalPosToDynamicBody(this GameEntity gameEntity, Vec3 localPosition, Vec3 globalForce, GameEntityPhysicsExtensions.ForceMode forceMode)
		{
			EngineApplicationInterface.IGameEntity.ApplyGlobalForceAtLocalPosToDynamicBody(gameEntity.Pointer, ref localPosition, ref globalForce, forceMode);
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00006352 File Offset: 0x00004552
		public static void ApplyGlobalForceAtLocalPosToDynamicBody(this WeakGameEntity gameEntity, Vec3 localPosition, Vec3 globalForce, GameEntityPhysicsExtensions.ForceMode forceMode)
		{
			EngineApplicationInterface.IGameEntity.ApplyGlobalForceAtLocalPosToDynamicBody(gameEntity.Pointer, ref localPosition, ref globalForce, forceMode);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0000636A File Offset: 0x0000456A
		public static void ApplyTorqueToDynamicBody(this GameEntity gameEntity, Vec3 torque, GameEntityPhysicsExtensions.ForceMode forceMode)
		{
			EngineApplicationInterface.IGameEntity.ApplyTorqueToDynamicBody(gameEntity.Pointer, ref torque, forceMode);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0000637F File Offset: 0x0000457F
		public static void ApplyTorqueToDynamicBody(this WeakGameEntity gameEntity, Vec3 torque, GameEntityPhysicsExtensions.ForceMode forceMode)
		{
			EngineApplicationInterface.IGameEntity.ApplyTorqueToDynamicBody(gameEntity.Pointer, ref torque, forceMode);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00006395 File Offset: 0x00004595
		public static void ApplyLocalForceAtLocalPosToDynamicBody(this GameEntity gameEntity, Vec3 localPosition, Vec3 localForce, GameEntityPhysicsExtensions.ForceMode forceMode)
		{
			EngineApplicationInterface.IGameEntity.ApplyLocalForceAtLocalPosToDynamicBody(gameEntity.Pointer, ref localPosition, ref localForce, forceMode);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x000063AC File Offset: 0x000045AC
		public static void ApplyLocalForceAtLocalPosToDynamicBody(this WeakGameEntity gameEntity, Vec3 localPosition, Vec3 localForce, GameEntityPhysicsExtensions.ForceMode forceMode)
		{
			EngineApplicationInterface.IGameEntity.ApplyLocalForceAtLocalPosToDynamicBody(gameEntity.Pointer, ref localPosition, ref localForce, forceMode);
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x000063C4 File Offset: 0x000045C4
		public static void ApplyAccelerationToDynamicBody(this GameEntity gameEntity, Vec3 acceleration)
		{
			EngineApplicationInterface.IGameEntity.ApplyAccelerationToDynamicBody(gameEntity.Pointer, ref acceleration);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x000063D8 File Offset: 0x000045D8
		public static void ApplyAccelerationToDynamicBody(this WeakGameEntity gameEntity, Vec3 acceleration)
		{
			EngineApplicationInterface.IGameEntity.ApplyAccelerationToDynamicBody(gameEntity.Pointer, ref acceleration);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x000063ED File Offset: 0x000045ED
		public static void DisableDynamicBodySimulation(this GameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.DisableDynamicBodySimulation(gameEntity.Pointer);
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x000063FF File Offset: 0x000045FF
		public static void DisableDynamicBodySimulation(this WeakGameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.DisableDynamicBodySimulation(gameEntity.Pointer);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00006412 File Offset: 0x00004612
		public static void DisableDynamicBodySimulationMT(this GameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.DisableDynamicBodySimulation(gameEntity.Pointer);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00006424 File Offset: 0x00004624
		public static void DisableDynamicBodySimulationMT(this WeakGameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.DisableDynamicBodySimulation(gameEntity.Pointer);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00006437 File Offset: 0x00004637
		public static void ConvertDynamicBodyToRayCast(this GameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.ConvertDynamicBodyToRayCast(gameEntity.Pointer);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00006449 File Offset: 0x00004649
		public static void ConvertDynamicBodyToRayCast(this WeakGameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.ConvertDynamicBodyToRayCast(gameEntity.Pointer);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0000645C File Offset: 0x0000465C
		public static void SetPhysicsMoveToBatched(this GameEntity gameEntity, bool value)
		{
			EngineApplicationInterface.IGameEntity.SetPhysicsMoveToBatched(gameEntity.Pointer, value);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0000646F File Offset: 0x0000466F
		public static void SetPhysicsMoveToBatched(this WeakGameEntity gameEntity, bool value)
		{
			EngineApplicationInterface.IGameEntity.SetPhysicsMoveToBatched(gameEntity.Pointer, value);
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00006483 File Offset: 0x00004683
		public static void EnableDynamicBody(this GameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.EnableDynamicBody(gameEntity.Pointer);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00006495 File Offset: 0x00004695
		public static void EnableDynamicBody(this WeakGameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.EnableDynamicBody(gameEntity.Pointer);
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x000064A8 File Offset: 0x000046A8
		public static float GetMass(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetMass(gameEntity.Pointer);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x000064BA File Offset: 0x000046BA
		public static float GetMass(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetMass(gameEntity.Pointer);
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x000064CD File Offset: 0x000046CD
		public static void SetMassAndUpdateInertiaAndCenterOfMass(this GameEntity gameEntity, float mass)
		{
			EngineApplicationInterface.IGameEntity.SetMassAndUpdateInertiaAndCenterOfMass(gameEntity.Pointer, mass);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x000064E0 File Offset: 0x000046E0
		public static void SetMassAndUpdateInertiaAndCenterOfMass(this WeakGameEntity gameEntity, float mass)
		{
			EngineApplicationInterface.IGameEntity.SetMassAndUpdateInertiaAndCenterOfMass(gameEntity.Pointer, mass);
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x000064F4 File Offset: 0x000046F4
		public static void SetCenterOfMass(this GameEntity gameEntity, Vec3 localCenterOfMass)
		{
			EngineApplicationInterface.IGameEntity.SetCenterOfMass(gameEntity.Pointer, ref localCenterOfMass);
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00006508 File Offset: 0x00004708
		public static void SetCenterOfMass(this WeakGameEntity gameEntity, Vec3 centerOfMass)
		{
			EngineApplicationInterface.IGameEntity.SetCenterOfMass(gameEntity.Pointer, ref centerOfMass);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0000651D File Offset: 0x0000471D
		public static Vec3 GetMassSpaceInertia(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetMassSpaceInertia(gameEntity.Pointer);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0000652F File Offset: 0x0000472F
		public static Vec3 GetMassSpaceInertia(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetMassSpaceInertia(gameEntity.Pointer);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00006542 File Offset: 0x00004742
		public static Vec3 GetMassSpaceInverseInertia(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetMassSpaceInverseInertia(gameEntity.Pointer);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00006554 File Offset: 0x00004754
		public static Vec3 GetMassSpaceInverseInertia(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetMassSpaceInverseInertia(gameEntity.Pointer);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00006567 File Offset: 0x00004767
		public static void SetMassSpaceInertia(this GameEntity gameEntity, Vec3 inertia)
		{
			EngineApplicationInterface.IGameEntity.SetMassSpaceInertia(gameEntity.Pointer, ref inertia);
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0000657B File Offset: 0x0000477B
		public static void SetMassSpaceInertia(this WeakGameEntity gameEntity, Vec3 inertia)
		{
			EngineApplicationInterface.IGameEntity.SetMassSpaceInertia(gameEntity.Pointer, ref inertia);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00006590 File Offset: 0x00004790
		public static void SetDamping(this GameEntity gameEntity, float linearDamping, float angularDamping)
		{
			EngineApplicationInterface.IGameEntity.SetDamping(gameEntity.Pointer, linearDamping, angularDamping);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x000065A4 File Offset: 0x000047A4
		public static void SetDamping(this WeakGameEntity gameEntity, float linearDamping, float angularDamping)
		{
			EngineApplicationInterface.IGameEntity.SetDamping(gameEntity.Pointer, linearDamping, angularDamping);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x000065B9 File Offset: 0x000047B9
		public static void SetDampingMT(this GameEntity gameEntity, float linearDamping, float angularDamping)
		{
			EngineApplicationInterface.IGameEntity.SetDamping(gameEntity.Pointer, linearDamping, angularDamping);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x000065CD File Offset: 0x000047CD
		public static void SetDampingMT(this WeakGameEntity gameEntity, float linearDamping, float angularDamping)
		{
			EngineApplicationInterface.IGameEntity.SetDamping(gameEntity.Pointer, linearDamping, angularDamping);
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x000065E2 File Offset: 0x000047E2
		public static void DisableGravity(this GameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.DisableGravity(gameEntity.Pointer);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x000065F4 File Offset: 0x000047F4
		public static void DisableGravity(this WeakGameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.DisableGravity(gameEntity.Pointer);
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00006607 File Offset: 0x00004807
		public static bool IsGravityDisabled(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.IsGravityDisabled(gameEntity.Pointer);
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00006619 File Offset: 0x00004819
		public static bool IsGravityDisabled(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.IsGravityDisabled(gameEntity.Pointer);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0000662C File Offset: 0x0000482C
		public static Vec3 GetLinearVelocity(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetLinearVelocity(gameEntity.Pointer);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0000663E File Offset: 0x0000483E
		public static Vec3 GetLinearVelocity(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetLinearVelocity(gameEntity.Pointer);
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00006651 File Offset: 0x00004851
		public static void SetLinearVelocity(this GameEntity gameEntity, Vec3 newLinearVelocity)
		{
			EngineApplicationInterface.IGameEntity.SetLinearVelocity(gameEntity.Pointer, newLinearVelocity);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00006664 File Offset: 0x00004864
		public static void SetLinearVelocity(this WeakGameEntity gameEntity, Vec3 newLinearVelocity)
		{
			EngineApplicationInterface.IGameEntity.SetLinearVelocity(gameEntity.Pointer, newLinearVelocity);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00006678 File Offset: 0x00004878
		public static Vec3 GetLinearVelocityMT(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetLinearVelocity(gameEntity.Pointer);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0000668A File Offset: 0x0000488A
		public static Vec3 GetLinearVelocityMT(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetLinearVelocity(gameEntity.Pointer);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0000669D File Offset: 0x0000489D
		public static Vec3 GetAngularVelocity(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetAngularVelocity(gameEntity.Pointer);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x000066AF File Offset: 0x000048AF
		public static Vec3 GetAngularVelocity(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetAngularVelocity(gameEntity.Pointer);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x000066C2 File Offset: 0x000048C2
		public static Vec3 GetAngularVelocityMT(this GameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetAngularVelocity(gameEntity.Pointer);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x000066D4 File Offset: 0x000048D4
		public static Vec3 GetAngularVelocityMT(this WeakGameEntity gameEntity)
		{
			return EngineApplicationInterface.IGameEntity.GetAngularVelocity(gameEntity.Pointer);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x000066E7 File Offset: 0x000048E7
		public static void SetAngularVelocity(this GameEntity gameEntity, Vec3 newAngularVelocity)
		{
			EngineApplicationInterface.IGameEntity.SetAngularVelocity(gameEntity.Pointer, in newAngularVelocity);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x000066FB File Offset: 0x000048FB
		public static void SetAngularVelocity(this WeakGameEntity gameEntity, Vec3 newAngularVelocity)
		{
			EngineApplicationInterface.IGameEntity.SetAngularVelocity(gameEntity.Pointer, in newAngularVelocity);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00006710 File Offset: 0x00004910
		public static void GetPhysicsMinMax(this GameEntity gameEntity, bool includeChildren, out Vec3 bbmin, out Vec3 bbmax, bool returnLocal)
		{
			bbmin = Vec3.Zero;
			bbmax = Vec3.Zero;
			EngineApplicationInterface.IGameEntity.GetPhysicsMinMax(gameEntity.Pointer, includeChildren, ref bbmin, ref bbmax, returnLocal);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0000673D File Offset: 0x0000493D
		public static void GetPhysicsMinMax(this WeakGameEntity gameEntity, bool includeChildren, out Vec3 bbmin, out Vec3 bbmax, bool returnLocal)
		{
			bbmin = Vec3.Zero;
			bbmax = Vec3.Zero;
			EngineApplicationInterface.IGameEntity.GetPhysicsMinMax(gameEntity.Pointer, includeChildren, ref bbmin, ref bbmax, returnLocal);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0000676C File Offset: 0x0000496C
		public static BoundingBox GetLocalPhysicsBoundingBox(this GameEntity gameEntity, bool includeChildren)
		{
			BoundingBox boundingBox;
			EngineApplicationInterface.IGameEntity.GetLocalPhysicsBoundingBox(gameEntity.Pointer, includeChildren, out boundingBox);
			return boundingBox;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00006790 File Offset: 0x00004990
		public static BoundingBox GetLocalPhysicsBoundingBox(this WeakGameEntity gameEntity, bool includeChildren)
		{
			BoundingBox boundingBox;
			EngineApplicationInterface.IGameEntity.GetLocalPhysicsBoundingBox(gameEntity.Pointer, includeChildren, out boundingBox);
			return boundingBox;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x000067B4 File Offset: 0x000049B4
		public static Vec3 GetLinearVelocityAtGlobalPointForEntityWithDynamicBody(this WeakGameEntity entity, Vec3 globalPoint)
		{
			MatrixFrame bodyWorldTransform = entity.GetBodyWorldTransform();
			Vec3 centerOfMass = entity.CenterOfMass;
			Vec3 vec = globalPoint - bodyWorldTransform.TransformToParent(in centerOfMass);
			Vec3 vec2 = Vec3.CrossProduct(entity.GetAngularVelocity(), vec);
			return entity.GetLinearVelocity() + vec2;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x000067FC File Offset: 0x000049FC
		public static Vec3 GetLinearVelocityAtGlobalPointForEntityWithDynamicBody(this GameEntity entity, Vec3 globalPoint)
		{
			MatrixFrame bodyWorldTransform = entity.GetBodyWorldTransform();
			Vec3 centerOfMass = entity.CenterOfMass;
			Vec3 vec = globalPoint - bodyWorldTransform.TransformToParent(in centerOfMass);
			Vec3 vec2 = Vec3.CrossProduct(entity.GetAngularVelocity(), vec);
			return entity.GetLinearVelocity() + vec2;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00006840 File Offset: 0x00004A40
		public static void ComputeVelocityDeltaFromImpulse(this WeakGameEntity gameEntity, in Vec3 impulseGlobal, in Vec3 impulsiveTorqueGlobal, out Vec3 deltaGlobalLinearVelocity, out Vec3 deltaGlobalAngularVelocity)
		{
			EngineApplicationInterface.IGameEntity.ComputeVelocityDeltaFromImpulse(gameEntity.Pointer, in impulseGlobal, in impulsiveTorqueGlobal, out deltaGlobalLinearVelocity, out deltaGlobalAngularVelocity);
		}

		// Token: 0x020000C0 RID: 192
		[EngineStruct("rglPhysics_engine_body::Force_mode", false, null)]
		public enum ForceMode : sbyte
		{
			// Token: 0x040003A7 RID: 935
			Force,
			// Token: 0x040003A8 RID: 936
			Impulse,
			// Token: 0x040003A9 RID: 937
			VelocityChange,
			// Token: 0x040003AA RID: 938
			Acceleration
		}
	}
}
