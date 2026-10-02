using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200009E RID: 158
	public struct WeakGameEntity
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x0000FEB4 File Offset: 0x0000E0B4
		// (set) Token: 0x06000E0B RID: 3595 RVA: 0x0000FEBC File Offset: 0x0000E0BC
		public UIntPtr Pointer { get; private set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0000FEC5 File Offset: 0x0000E0C5
		public bool IsValid
		{
			get
			{
				return this.Pointer != UIntPtr.Zero;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000E0D RID: 3597 RVA: 0x0000FED7 File Offset: 0x0000E0D7
		public string Name
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetName(this.Pointer);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x0000FEE9 File Offset: 0x0000E0E9
		public Scene Scene
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetScene(this.Pointer);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000E0F RID: 3599 RVA: 0x0000FEFB File Offset: 0x0000E0FB
		public EntityFlags EntityFlags
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetEntityFlags(this.Pointer);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x0000FF0D File Offset: 0x0000E10D
		public EntityVisibilityFlags EntityVisibilityFlags
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetEntityVisibilityFlags(this.Pointer);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000E11 RID: 3601 RVA: 0x0000FF1F File Offset: 0x0000E11F
		public BodyFlags BodyFlag
		{
			get
			{
				return (BodyFlags)EngineApplicationInterface.IGameEntity.GetBodyFlags(this.Pointer);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x0000FF31 File Offset: 0x0000E131
		public BodyFlags PhysicsDescBodyFlag
		{
			get
			{
				return (BodyFlags)EngineApplicationInterface.IGameEntity.GetPhysicsDescBodyFlags(this.Pointer);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000E13 RID: 3603 RVA: 0x0000FF43 File Offset: 0x0000E143
		public float Mass
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetMass(this.Pointer);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000E14 RID: 3604 RVA: 0x0000FF55 File Offset: 0x0000E155
		public Vec3 CenterOfMass
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetCenterOfMass(this.Pointer);
			}
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0000FF67 File Offset: 0x0000E167
		internal WeakGameEntity(UIntPtr pointer)
		{
			this.Pointer = pointer;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0000FF70 File Offset: 0x0000E170
		public void Invalidate()
		{
			this.Pointer = (UIntPtr)0UL;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0000FF7F File Offset: 0x0000E17F
		public UIntPtr GetScenePointer()
		{
			return EngineApplicationInterface.IGameEntity.GetScenePointer(this.Pointer);
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0000FF94 File Offset: 0x0000E194
		public override string ToString()
		{
			return this.Pointer.ToString();
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0000FFAF File Offset: 0x0000E1AF
		public void ClearEntityComponents(bool resetAll, bool removeScripts, bool deleteChildEntities)
		{
			EngineApplicationInterface.IGameEntity.ClearEntityComponents(this.Pointer, resetAll, removeScripts, deleteChildEntities);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0000FFC4 File Offset: 0x0000E1C4
		public void ClearComponents()
		{
			EngineApplicationInterface.IGameEntity.ClearComponents(this.Pointer);
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0000FFD6 File Offset: 0x0000E1D6
		public void ClearOnlyOwnComponents()
		{
			EngineApplicationInterface.IGameEntity.ClearOnlyOwnComponents(this.Pointer);
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0000FFE8 File Offset: 0x0000E1E8
		public bool CheckResources(bool addToQueue, bool checkFaceResources)
		{
			return EngineApplicationInterface.IGameEntity.CheckResources(this.Pointer, addToQueue, checkFaceResources);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0000FFFC File Offset: 0x0000E1FC
		public void SetMobility(GameEntity.Mobility mobility)
		{
			EngineApplicationInterface.IGameEntity.SetMobility(this.Pointer, mobility);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0001000F File Offset: 0x0000E20F
		public GameEntity.Mobility GetMobility()
		{
			return EngineApplicationInterface.IGameEntity.GetMobility(this.Pointer);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00010021 File Offset: 0x0000E221
		public void AddMesh(Mesh mesh, bool recomputeBoundingBox = true)
		{
			EngineApplicationInterface.IGameEntity.AddMesh(this.Pointer, mesh.Pointer, recomputeBoundingBox);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0001003A File Offset: 0x0000E23A
		public void AddMultiMeshToSkeleton(MetaMesh metaMesh)
		{
			EngineApplicationInterface.IGameEntity.AddMultiMeshToSkeleton(this.Pointer, metaMesh.Pointer);
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00010052 File Offset: 0x0000E252
		public void AddMultiMeshToSkeletonBone(MetaMesh metaMesh, sbyte boneIndex)
		{
			EngineApplicationInterface.IGameEntity.AddMultiMeshToSkeletonBone(this.Pointer, metaMesh.Pointer, boneIndex);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0001006B File Offset: 0x0000E26B
		public void SetColorToAllMeshesWithTagRecursive(uint color, string tag)
		{
			EngineApplicationInterface.IGameEntity.SetColorToAllMeshesWithTagRecursive(this.Pointer, color, tag);
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0001007F File Offset: 0x0000E27F
		public IEnumerable<Mesh> GetAllMeshesWithTag(string tag)
		{
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			this.GetChildrenRecursive(ref list);
			list.Add(ref this);
			foreach (WeakGameEntity entity in list)
			{
				int num;
				for (int i = 0; i < entity.MultiMeshComponentCount; i = num + 1)
				{
					MetaMesh multiMesh = entity.GetMetaMesh(i);
					for (int j = 0; j < multiMesh.MeshCount; j = num + 1)
					{
						Mesh meshAtIndex = multiMesh.GetMeshAtIndex(j);
						if (meshAtIndex.HasTag(tag))
						{
							yield return meshAtIndex;
						}
						num = j;
					}
					multiMesh = null;
					num = i;
				}
				for (int i = 0; i < entity.ClothSimulatorComponentCount; i = num + 1)
				{
					ClothSimulatorComponent clothSimulator = entity.GetClothSimulator(i);
					MetaMesh multiMesh = clothSimulator.GetFirstMetaMesh();
					for (int j = 0; j < multiMesh.MeshCount; j = num + 1)
					{
						Mesh meshAtIndex2 = multiMesh.GetMeshAtIndex(j);
						if (meshAtIndex2.HasTag(tag))
						{
							yield return meshAtIndex2;
						}
						num = j;
					}
					multiMesh = null;
					num = i;
				}
			}
			List<WeakGameEntity>.Enumerator enumerator = default(List<WeakGameEntity>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0001009B File Offset: 0x0000E29B
		public void SetName(string name)
		{
			EngineApplicationInterface.IGameEntity.SetName(this.Pointer, name);
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x000100AE File Offset: 0x0000E2AE
		public void SetEntityFlags(EntityFlags flags)
		{
			EngineApplicationInterface.IGameEntity.SetEntityFlags(this.Pointer, flags);
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x000100C1 File Offset: 0x0000E2C1
		public void SetEntityVisibilityFlags(EntityVisibilityFlags flags)
		{
			EngineApplicationInterface.IGameEntity.SetEntityVisibilityFlags(this.Pointer, flags);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x000100D4 File Offset: 0x0000E2D4
		public PhysicsMaterial GetPhysicsMaterial()
		{
			return PhysicsMaterial.GetFromIndex(EngineApplicationInterface.IGameEntity.GetPhysicsMaterialIndex(this.Pointer));
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x000100EB File Offset: 0x0000E2EB
		public void SetBodyFlags(BodyFlags flags)
		{
			EngineApplicationInterface.IGameEntity.SetBodyFlags(this.Pointer, (uint)flags);
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x000100FE File Offset: 0x0000E2FE
		public void SetBodyFlagsRecursive(BodyFlags bodyFlags)
		{
			EngineApplicationInterface.IGameEntity.SetBodyFlagsRecursive(this.Pointer, (uint)bodyFlags);
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00010114 File Offset: 0x0000E314
		public void AddBodyFlags(BodyFlags bodyFlags, bool applyToChildren = true)
		{
			this.SetBodyFlags(this.BodyFlag | bodyFlags);
			if (applyToChildren)
			{
				foreach (WeakGameEntity weakGameEntity in this.GetChildren())
				{
					weakGameEntity.AddBodyFlags(bodyFlags, true);
				}
			}
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00010174 File Offset: 0x0000E374
		internal static WeakGameEntity GetFirstEntityWithTag(Scene scene, string tag)
		{
			return new WeakGameEntity(EngineApplicationInterface.IGameEntity.GetFirstEntityWithTag(scene.Pointer, tag));
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0001018C File Offset: 0x0000E38C
		internal static WeakGameEntity GetNextEntityWithTag(Scene scene, WeakGameEntity startEntity, string tag)
		{
			if (!(startEntity == null))
			{
				return new WeakGameEntity(EngineApplicationInterface.IGameEntity.GetNextEntityWithTag(startEntity.Pointer, tag));
			}
			return WeakGameEntity.GetFirstEntityWithTag(scene, tag);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x000101B6 File Offset: 0x0000E3B6
		internal static WeakGameEntity GetFirstEntityWithTagExpression(Scene scene, string tagExpression)
		{
			return new WeakGameEntity(EngineApplicationInterface.IGameEntity.GetFirstEntityWithTagExpression(scene.Pointer, tagExpression));
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x000101D0 File Offset: 0x0000E3D0
		internal static WeakGameEntity GetNextEntityWithTagExpression(Scene scene, WeakGameEntity startEntity, string tagExpression)
		{
			if (startEntity == null)
			{
				return WeakGameEntity.GetFirstEntityWithTagExpression(scene, tagExpression);
			}
			UIntPtr nextEntityWithTagExpression = EngineApplicationInterface.IGameEntity.GetNextEntityWithTagExpression(startEntity.Pointer, tagExpression);
			if (nextEntityWithTagExpression != UIntPtr.Zero)
			{
				return new WeakGameEntity(nextEntityWithTagExpression);
			}
			return WeakGameEntity.Invalid;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0001021A File Offset: 0x0000E41A
		internal static IEnumerable<WeakGameEntity> GetEntitiesWithTag(Scene scene, string tag)
		{
			WeakGameEntity entity = WeakGameEntity.GetFirstEntityWithTag(scene, tag);
			while (entity != null)
			{
				yield return entity;
				entity = WeakGameEntity.GetNextEntityWithTag(scene, entity, tag);
			}
			yield break;
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00010231 File Offset: 0x0000E431
		internal static IEnumerable<WeakGameEntity> GetEntitiesWithTagExpression(Scene scene, string tagExpression)
		{
			WeakGameEntity entity = WeakGameEntity.GetFirstEntityWithTagExpression(scene, tagExpression);
			while (entity != null)
			{
				yield return entity;
				entity = WeakGameEntity.GetNextEntityWithTagExpression(scene, entity, tagExpression);
			}
			yield break;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00010248 File Offset: 0x0000E448
		public void RemoveBodyFlags(BodyFlags bodyFlags, bool applyToChildren = true)
		{
			this.SetBodyFlags(this.BodyFlag & ~bodyFlags);
			if (applyToChildren)
			{
				foreach (WeakGameEntity weakGameEntity in this.GetChildren())
				{
					weakGameEntity.RemoveBodyFlags(bodyFlags, true);
				}
			}
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x000102AC File Offset: 0x0000E4AC
		public void SetLocalPosition(Vec3 position)
		{
			EngineApplicationInterface.IGameEntity.SetLocalPosition(this.Pointer, position);
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x000102BF File Offset: 0x0000E4BF
		public void SetGlobalPosition(Vec3 position)
		{
			EngineApplicationInterface.IGameEntity.SetGlobalPosition(this.Pointer, in position);
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x000102D4 File Offset: 0x0000E4D4
		public void SetColor(uint color1, uint color2, string meshTag)
		{
			foreach (Mesh mesh in this.GetAllMeshesWithTag(meshTag))
			{
				mesh.Color = color1;
				mesh.Color2 = color2;
			}
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00010328 File Offset: 0x0000E528
		public uint GetFactorColor()
		{
			return EngineApplicationInterface.IGameEntity.GetFactorColor(this.Pointer);
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x0001033A File Offset: 0x0000E53A
		public void SetFactorColor(uint color)
		{
			EngineApplicationInterface.IGameEntity.SetFactorColor(this.Pointer, color);
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0001034D File Offset: 0x0000E54D
		public void SetAsReplayEntity()
		{
			EngineApplicationInterface.IGameEntity.SetAsReplayEntity(this.Pointer);
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0001035F File Offset: 0x0000E55F
		public void SetClothMaxDistanceMultiplier(float multiplier)
		{
			EngineApplicationInterface.IGameEntity.SetClothMaxDistanceMultiplier(this.Pointer, multiplier);
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00010372 File Offset: 0x0000E572
		public void RemoveMultiMeshFromSkeleton(MetaMesh metaMesh)
		{
			EngineApplicationInterface.IGameEntity.RemoveMultiMeshFromSkeleton(this.Pointer, metaMesh.Pointer);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x0001038A File Offset: 0x0000E58A
		public void RemoveMultiMeshFromSkeletonBone(MetaMesh metaMesh, sbyte boneIndex)
		{
			EngineApplicationInterface.IGameEntity.RemoveMultiMeshFromSkeletonBone(this.Pointer, metaMesh.Pointer, boneIndex);
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x000103A3 File Offset: 0x0000E5A3
		public bool RemoveComponentWithMesh(Mesh mesh)
		{
			return EngineApplicationInterface.IGameEntity.RemoveComponentWithMesh(this.Pointer, mesh.Pointer);
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x000103BB File Offset: 0x0000E5BB
		public void AddComponent(GameEntityComponent component)
		{
			EngineApplicationInterface.IGameEntity.AddComponent(this.Pointer, component.Pointer);
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x000103D3 File Offset: 0x0000E5D3
		public bool HasComponent(GameEntityComponent component)
		{
			return EngineApplicationInterface.IGameEntity.HasComponent(this.Pointer, component.Pointer);
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x000103EB File Offset: 0x0000E5EB
		public bool IsInEditorScene()
		{
			return false;
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x000103EE File Offset: 0x0000E5EE
		public bool RemoveComponent(GameEntityComponent component)
		{
			return EngineApplicationInterface.IGameEntity.RemoveComponent(this.Pointer, component.Pointer);
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00010406 File Offset: 0x0000E606
		public string GetGuid()
		{
			return EngineApplicationInterface.IGameEntity.GetGuid(this.Pointer);
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00010418 File Offset: 0x0000E618
		public bool IsGuidValid()
		{
			return EngineApplicationInterface.IGameEntity.IsGuidValid(this.Pointer);
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0001042A File Offset: 0x0000E62A
		public void SetEnforcedMaximumLodLevel(int lodLevel)
		{
			EngineApplicationInterface.IGameEntity.SetEnforcedMaximumLodLevel(this.Pointer, lodLevel);
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0001043D File Offset: 0x0000E63D
		public float GetLodLevelForDistanceSq(float distSq)
		{
			return EngineApplicationInterface.IGameEntity.GetLodLevelForDistanceSq(this.Pointer, distSq);
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00010450 File Offset: 0x0000E650
		public void GetQuickBoneEntitialFrame(sbyte index, out MatrixFrame frame)
		{
			EngineApplicationInterface.IGameEntity.GetQuickBoneEntitialFrame(this.Pointer, index, out frame);
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00010464 File Offset: 0x0000E664
		public void UpdateVisibilityMask()
		{
			EngineApplicationInterface.IGameEntity.UpdateVisibilityMask(this.Pointer);
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00010476 File Offset: 0x0000E676
		public void CallScriptCallbacks(bool registerScriptComponents)
		{
			EngineApplicationInterface.IGameEntity.CallScriptCallbacks(this.Pointer, registerScriptComponents);
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00010489 File Offset: 0x0000E689
		public int GetScriptCount()
		{
			return EngineApplicationInterface.IGameEntity.GetScriptComponentCount(this.Pointer);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x0001049B File Offset: 0x0000E69B
		public bool IsGhostObject()
		{
			return EngineApplicationInterface.IGameEntity.IsGhostObject(this.Pointer);
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x000104AD File Offset: 0x0000E6AD
		public void CreateAndAddScriptComponent(string name, bool callScriptCallbacks)
		{
			EngineApplicationInterface.IGameEntity.CreateAndAddScriptComponent(this.Pointer, name, callScriptCallbacks);
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x000104C1 File Offset: 0x0000E6C1
		public void RemoveScriptComponent(UIntPtr scriptComponent, int removeReason)
		{
			EngineApplicationInterface.IGameEntity.RemoveScriptComponent(this.Pointer, scriptComponent, removeReason);
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x000104D5 File Offset: 0x0000E6D5
		public void SetEntityEnvMapVisibility(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetEntityEnvMapVisibility(this.Pointer, value);
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x000104E8 File Offset: 0x0000E6E8
		public ScriptComponentBehavior GetScriptAtIndex(int index)
		{
			return EngineApplicationInterface.IGameEntity.GetScriptComponentAtIndex(this.Pointer, index);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x000104FB File Offset: 0x0000E6FB
		internal int GetScriptComponentIndex(uint nameHash)
		{
			return EngineApplicationInterface.IGameEntity.GetScriptComponentIndex(this.Pointer, nameHash);
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0001050E File Offset: 0x0000E70E
		public bool HasScene()
		{
			return EngineApplicationInterface.IGameEntity.HasScene(this.Pointer);
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00010520 File Offset: 0x0000E720
		public bool HasScriptComponent(string scName)
		{
			return EngineApplicationInterface.IGameEntity.HasScriptComponent(this.Pointer, scName);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00010533 File Offset: 0x0000E733
		public bool HasScriptComponent(uint scNameHash)
		{
			return EngineApplicationInterface.IGameEntity.HasScriptComponentHash(this.Pointer, scNameHash);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00010546 File Offset: 0x0000E746
		public IEnumerable<ScriptComponentBehavior> GetScriptComponents()
		{
			int count = this.GetScriptCount();
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return this.GetScriptAtIndex(i);
				num = i;
			}
			yield break;
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0001055B File Offset: 0x0000E75B
		public IEnumerable<T> GetScriptComponents<T>() where T : ScriptComponentBehavior
		{
			int count = this.GetScriptCount();
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				T t;
				if ((t = this.GetScriptAtIndex(i) as T) != null)
				{
					yield return t;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00010570 File Offset: 0x0000E770
		public bool HasScriptOfType<T>() where T : ScriptComponentBehavior
		{
			int scriptCount = this.GetScriptCount();
			for (int i = 0; i < scriptCount; i++)
			{
				if (this.GetScriptAtIndex(i) is T)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x000105A4 File Offset: 0x0000E7A4
		public bool HasScriptWithInterfaceOfType<T>()
		{
			int scriptCount = this.GetScriptCount();
			for (int i = 0; i < scriptCount; i++)
			{
				if (this.GetScriptAtIndex(i) is T)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x000105D8 File Offset: 0x0000E7D8
		public T GetFirstScriptOfTypeInFamily<T>() where T : ScriptComponentBehavior
		{
			T t = this.GetFirstScriptOfType<T>();
			WeakGameEntity weakGameEntity = this;
			while (t == null)
			{
				WeakGameEntity parent = weakGameEntity.Parent;
				if (!parent.IsValid)
				{
					break;
				}
				weakGameEntity = parent;
				t = weakGameEntity.GetFirstScriptOfType<T>();
			}
			return t;
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0001061C File Offset: 0x0000E81C
		public ScriptComponentBehavior GetFirstScriptWithNameHash(uint nameHash)
		{
			int scriptComponentIndex = this.GetScriptComponentIndex(nameHash);
			if (scriptComponentIndex != -1)
			{
				return this.GetScriptAtIndex(scriptComponentIndex);
			}
			return null;
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00010640 File Offset: 0x0000E840
		public T GetFirstScriptOfType<T>() where T : ScriptComponentBehavior
		{
			int scriptCount = this.GetScriptCount();
			for (int i = 0; i < scriptCount; i++)
			{
				T t;
				if ((t = this.GetScriptAtIndex(i) as T) != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00010688 File Offset: 0x0000E888
		public T GetFirstScriptWithInterfaceOfType<T>() where T : class
		{
			int scriptCount = this.GetScriptCount();
			for (int i = 0; i < scriptCount; i++)
			{
				T t;
				if ((t = this.GetScriptAtIndex(i) as T) != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x000106D0 File Offset: 0x0000E8D0
		public T GetFirstScriptOfTypeRecursive<T>() where T : ScriptComponentBehavior
		{
			int num = this.GetScriptCount();
			for (int i = 0; i < num; i++)
			{
				T t;
				if ((t = this.GetScriptAtIndex(i) as T) != null)
				{
					return t;
				}
			}
			num = this.ChildCount;
			for (int j = 0; j < num; j++)
			{
				T firstScriptOfTypeRecursive = this.GetChild(j).GetFirstScriptOfTypeRecursive<T>();
				if (firstScriptOfTypeRecursive != null)
				{
					return firstScriptOfTypeRecursive;
				}
			}
			return default(T);
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00010748 File Offset: 0x0000E948
		public WeakGameEntity GetFirstChildEntityWithTag(string tag)
		{
			foreach (WeakGameEntity weakGameEntity in this.GetChildren())
			{
				if (weakGameEntity.HasTag(tag))
				{
					return weakGameEntity;
				}
			}
			return WeakGameEntity.Invalid;
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x000107A4 File Offset: 0x0000E9A4
		public int GetScriptCountOfType<T>() where T : ScriptComponentBehavior
		{
			int scriptCount = this.GetScriptCount();
			int num = 0;
			for (int i = 0; i < scriptCount; i++)
			{
				if (this.GetScriptAtIndex(i) is T)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x000107DC File Offset: 0x0000E9DC
		public int GetScriptCountOfTypeRecursive<T>() where T : ScriptComponentBehavior
		{
			int num = this.GetScriptCount();
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (this.GetScriptAtIndex(i) is T)
				{
					num2++;
				}
			}
			num = this.ChildCount;
			for (int j = 0; j < num; j++)
			{
				num2 += this.GetChild(j).GetScriptCountOfTypeRecursive<T>();
			}
			return num2;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00010837 File Offset: 0x0000EA37
		internal static GameEntity GetFirstEntityWithName(Scene scene, string entityName)
		{
			return EngineApplicationInterface.IGameEntity.FindWithName(scene.Pointer, entityName);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0001084A File Offset: 0x0000EA4A
		public void SetAlpha(float alpha)
		{
			EngineApplicationInterface.IGameEntity.SetAlpha(this.Pointer, alpha);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0001085D File Offset: 0x0000EA5D
		public void SetVisibilityExcludeParents(bool visible)
		{
			EngineApplicationInterface.IGameEntity.SetVisibilityExcludeParents(this.Pointer, visible);
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00010870 File Offset: 0x0000EA70
		public void SetReadyToRender(bool ready)
		{
			EngineApplicationInterface.IGameEntity.SetReadyToRender(this.Pointer, ready);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00010883 File Offset: 0x0000EA83
		public bool GetVisibilityExcludeParents()
		{
			return EngineApplicationInterface.IGameEntity.GetVisibilityExcludeParents(this.Pointer);
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00010895 File Offset: 0x0000EA95
		public bool IsVisibleIncludeParents()
		{
			return EngineApplicationInterface.IGameEntity.IsVisibleIncludeParents(this.Pointer);
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x000108A7 File Offset: 0x0000EAA7
		public uint GetVisibilityLevelMaskIncludingParents()
		{
			return EngineApplicationInterface.IGameEntity.GetVisibilityLevelMaskIncludingParents(this.Pointer);
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x000108B9 File Offset: 0x0000EAB9
		public bool GetEditModeLevelVisibility()
		{
			return EngineApplicationInterface.IGameEntity.GetEditModeLevelVisibility(this.Pointer);
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x000108CB File Offset: 0x0000EACB
		public void Remove(int removeReason)
		{
			EngineApplicationInterface.IGameEntity.Remove(this.Pointer, removeReason);
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x000108DE File Offset: 0x0000EADE
		public void SetUpgradeLevelMask(GameEntity.UpgradeLevelMask mask)
		{
			EngineApplicationInterface.IGameEntity.SetUpgradeLevelMask(this.Pointer, (uint)mask);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x000108F1 File Offset: 0x0000EAF1
		public GameEntity.UpgradeLevelMask GetUpgradeLevelMask()
		{
			return (GameEntity.UpgradeLevelMask)EngineApplicationInterface.IGameEntity.GetUpgradeLevelMask(this.Pointer);
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00010903 File Offset: 0x0000EB03
		public GameEntity.UpgradeLevelMask GetUpgradeLevelMaskCumulative()
		{
			return (GameEntity.UpgradeLevelMask)EngineApplicationInterface.IGameEntity.GetUpgradeLevelMaskCumulative(this.Pointer);
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00010918 File Offset: 0x0000EB18
		public int GetUpgradeLevelOfEntity()
		{
			int upgradeLevelMask = (int)this.GetUpgradeLevelMask();
			if ((upgradeLevelMask & 1) > 0)
			{
				return 0;
			}
			if ((upgradeLevelMask & 2) > 0)
			{
				return 1;
			}
			if ((upgradeLevelMask & 4) > 0)
			{
				return 2;
			}
			if ((upgradeLevelMask & 8) > 0)
			{
				return 3;
			}
			return -1;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0001094D File Offset: 0x0000EB4D
		public string GetOldPrefabName()
		{
			return EngineApplicationInterface.IGameEntity.GetOldPrefabName(this.Pointer);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0001095F File Offset: 0x0000EB5F
		public string GetPrefabName()
		{
			return EngineApplicationInterface.IGameEntity.GetPrefabName(this.Pointer);
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00010971 File Offset: 0x0000EB71
		public void RefreshMeshesToRenderToHullWater(UIntPtr visualRecord, string entityTag)
		{
			EngineApplicationInterface.IGameEntity.RefreshMeshesToRenderToHullWater(this.Pointer, visualRecord, entityTag);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00010985 File Offset: 0x0000EB85
		public void DeRegisterWaterMeshMaterials(UIntPtr visualRecord)
		{
			EngineApplicationInterface.IGameEntity.DeRegisterWaterMeshMaterials(this.Pointer, visualRecord);
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00010998 File Offset: 0x0000EB98
		public void SetVisualRecordWakeParams(UIntPtr visualRecord, Vec3 wakeParams)
		{
			EngineApplicationInterface.IGameEntity.SetVisualRecordWakeParams(visualRecord, in wakeParams);
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x000109A7 File Offset: 0x0000EBA7
		public void ChangeResolutionMultiplierOfWaterVisual(UIntPtr visualRecord, float multiplier, in Vec3 waterEffectsBB)
		{
			EngineApplicationInterface.IGameEntity.ChangeResolutionMultiplierOfWaterVisual(visualRecord, multiplier, in waterEffectsBB);
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x000109B6 File Offset: 0x0000EBB6
		public void ResetHullWater(UIntPtr visualRecord)
		{
			EngineApplicationInterface.IGameEntity.ResetHullWater(visualRecord);
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x000109C3 File Offset: 0x0000EBC3
		public void SetWaterVisualRecordFrameAndDt(UIntPtr visualRecord, MatrixFrame frame, float dt)
		{
			EngineApplicationInterface.IGameEntity.SetWaterVisualRecordFrameAndDt(this.Pointer, visualRecord, in frame, dt);
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x000109D9 File Offset: 0x0000EBD9
		public void AddSplashPositionToWaterVisualRecord(UIntPtr visualRecord, Vec3 position)
		{
			EngineApplicationInterface.IGameEntity.AddSplashPositionToWaterVisualRecord(this.Pointer, visualRecord, in position);
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x000109EE File Offset: 0x0000EBEE
		public void UpdateHullWaterEffectFrames(UIntPtr visualRecord)
		{
			EngineApplicationInterface.IGameEntity.UpdateHullWaterEffectFrames(this.Pointer, visualRecord);
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x00010A01 File Offset: 0x0000EC01
		public void CopyScriptComponentFromAnotherEntity(GameEntity otherEntity, string scriptName)
		{
			EngineApplicationInterface.IGameEntity.CopyScriptComponentFromAnotherEntity(this.Pointer, otherEntity.Pointer, scriptName);
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x00010A1A File Offset: 0x0000EC1A
		public void SetFrame(ref MatrixFrame frame, bool isTeleportation = true)
		{
			EngineApplicationInterface.IGameEntity.SetLocalFrame(this.Pointer, ref frame, isTeleportation);
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00010A2E File Offset: 0x0000EC2E
		public void SetLocalFrame(ref MatrixFrame frame, bool isTeleportation)
		{
			EngineApplicationInterface.IGameEntity.SetLocalFrame(this.Pointer, ref frame, isTeleportation);
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00010A42 File Offset: 0x0000EC42
		public void SetClothComponentKeepState(MetaMesh metaMesh, bool state)
		{
			EngineApplicationInterface.IGameEntity.SetClothComponentKeepState(this.Pointer, metaMesh.Pointer, state);
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00010A5B File Offset: 0x0000EC5B
		public void SetClothComponentKeepStateOfAllMeshes(bool state)
		{
			EngineApplicationInterface.IGameEntity.SetClothComponentKeepStateOfAllMeshes(this.Pointer, state);
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00010A6E File Offset: 0x0000EC6E
		public void SetPreviousFrameInvalid()
		{
			EngineApplicationInterface.IGameEntity.SetPreviousFrameInvalid(this.Pointer);
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00010A80 File Offset: 0x0000EC80
		public MatrixFrame GetFrame()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetLocalFrame(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00010AA0 File Offset: 0x0000ECA0
		public void GetLocalFrame(out MatrixFrame frame)
		{
			EngineApplicationInterface.IGameEntity.GetLocalFrame(this.Pointer, out frame);
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00010AB3 File Offset: 0x0000ECB3
		public bool HasBatchedKinematicPhysicsFlag()
		{
			return EngineApplicationInterface.IGameEntity.HasBatchedKinematicPhysicsFlag(this.Pointer);
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00010AC5 File Offset: 0x0000ECC5
		public bool HasBatchedRayCastPhysicsFlag()
		{
			return EngineApplicationInterface.IGameEntity.HasBatchedRayCastPhysicsFlag(this.Pointer);
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00010AD8 File Offset: 0x0000ECD8
		public MatrixFrame GetLocalFrame()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetLocalFrame(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00010AF8 File Offset: 0x0000ECF8
		public MatrixFrame GetGlobalFrame()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetGlobalFrame(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00010B18 File Offset: 0x0000ED18
		public void SetWaterSDFClipData(int slotIndex, in MatrixFrame frame, bool visibility)
		{
			EngineApplicationInterface.IGameEntity.SetWaterSDFClipData(this.Pointer, slotIndex, in frame, visibility);
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00010B2D File Offset: 0x0000ED2D
		public int RegisterWaterSDFClip(Texture sdfTexture)
		{
			return EngineApplicationInterface.IGameEntity.RegisterWaterSDFClip(this.Pointer, (sdfTexture != null) ? sdfTexture.Pointer : UIntPtr.Zero);
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00010B55 File Offset: 0x0000ED55
		public void DeRegisterWaterSDFClip(int slot)
		{
			EngineApplicationInterface.IGameEntity.DeRegisterWaterSDFClip(this.Pointer, slot);
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x00010B68 File Offset: 0x0000ED68
		public MatrixFrame GetGlobalFrameImpreciseForFixedTick()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetGlobalFrameImpreciseForFixedTick(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00010B88 File Offset: 0x0000ED88
		public MatrixFrame ComputePreciseGlobalFrameForFixedTickSlow()
		{
			MatrixFrame matrixFrame = this.GetLocalFrame();
			WeakGameEntity weakGameEntity = this.Parent;
			while (weakGameEntity.Parent != null)
			{
				matrixFrame = weakGameEntity.GetLocalFrame().TransformToParent(in matrixFrame);
				weakGameEntity = weakGameEntity.Parent;
			}
			matrixFrame = weakGameEntity.GetBodyWorldTransform().TransformToParent(in matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00010BE1 File Offset: 0x0000EDE1
		public void SetGlobalFrame(in MatrixFrame frame, bool isTeleportation = true)
		{
			EngineApplicationInterface.IGameEntity.SetGlobalFrame(this.Pointer, in frame, isTeleportation);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00010BF8 File Offset: 0x0000EDF8
		public MatrixFrame GetPreviousGlobalFrame()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetPreviousGlobalFrame(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00010C18 File Offset: 0x0000EE18
		public MatrixFrame GetBodyWorldTransform()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetBodyWorldTransform(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00010C38 File Offset: 0x0000EE38
		public MatrixFrame GetBodyVisualWorldTransform()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetBodyVisualWorldTransform(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00010C58 File Offset: 0x0000EE58
		public void UpdateTriadFrameForEditor()
		{
			EngineApplicationInterface.IGameEntity.UpdateTriadFrameForEditor(this.Pointer);
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00010C6C File Offset: 0x0000EE6C
		public void UpdateTriadFrameForEditorForAllChildren()
		{
			this.UpdateTriadFrameForEditor();
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			this.GetChildrenRecursive(ref list);
			foreach (WeakGameEntity weakGameEntity in list)
			{
				EngineApplicationInterface.IGameEntity.UpdateTriadFrameForEditor(weakGameEntity.Pointer);
			}
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00010CD8 File Offset: 0x0000EED8
		public Vec3 GetGlobalScale()
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalScale(this.Pointer);
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00010CEC File Offset: 0x0000EEEC
		public Vec3 GetLocalScale()
		{
			return this.GetFrame().rotation.GetScaleVector();
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000E8D RID: 3725 RVA: 0x00010D0C File Offset: 0x0000EF0C
		public Vec3 GlobalPosition
		{
			get
			{
				return this.GetGlobalFrame().origin;
			}
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00010D1C File Offset: 0x0000EF1C
		public void SetAnimationSoundActivation(bool activate)
		{
			EngineApplicationInterface.IGameEntity.SetAnimationSoundActivation(this.Pointer, activate);
			foreach (WeakGameEntity weakGameEntity in this.GetChildren())
			{
				weakGameEntity.SetAnimationSoundActivation(activate);
			}
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00010D7C File Offset: 0x0000EF7C
		public void CopyComponentsToSkeleton()
		{
			EngineApplicationInterface.IGameEntity.CopyComponentsToSkeleton(this.Pointer);
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00010D8E File Offset: 0x0000EF8E
		public void AddMeshToBone(sbyte boneIndex, Mesh mesh)
		{
			EngineApplicationInterface.IGameEntity.AddMeshToBone(this.Pointer, mesh.Pointer, boneIndex);
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00010DA7 File Offset: 0x0000EFA7
		public void ActivateRagdoll()
		{
			EngineApplicationInterface.IGameEntity.ActivateRagdoll(this.Pointer);
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00010DB9 File Offset: 0x0000EFB9
		public void PauseSkeletonAnimation()
		{
			EngineApplicationInterface.IGameEntity.Freeze(this.Pointer, true);
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00010DCC File Offset: 0x0000EFCC
		public void ResumeSkeletonAnimation()
		{
			EngineApplicationInterface.IGameEntity.Freeze(this.Pointer, false);
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00010DDF File Offset: 0x0000EFDF
		public bool IsSkeletonAnimationPaused()
		{
			return EngineApplicationInterface.IGameEntity.IsFrozen(this.Pointer);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00010DF1 File Offset: 0x0000EFF1
		public sbyte GetBoneCount()
		{
			return EngineApplicationInterface.IGameEntity.GetBoneCount(this.Pointer);
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00010E03 File Offset: 0x0000F003
		public float GetWaterLevelAtPosition(Vec2 position, bool useWaterRenderer, bool checkWaterBodyEntities)
		{
			return EngineApplicationInterface.IGameEntity.GetWaterLevelAtPosition(this.Pointer, in position, useWaterRenderer, checkWaterBodyEntities);
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00010E1C File Offset: 0x0000F01C
		public MatrixFrame GetBoneEntitialFrameWithIndex(sbyte boneIndex)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.IGameEntity.GetBoneEntitialFrameWithIndex(this.Pointer, boneIndex, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00010E48 File Offset: 0x0000F048
		public MatrixFrame GetBoneEntitialFrameWithName(string boneName)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.IGameEntity.GetBoneEntitialFrameWithName(this.Pointer, boneName, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000E99 RID: 3737 RVA: 0x00010E71 File Offset: 0x0000F071
		public string[] Tags
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetTags(this.Pointer).Split(new char[] { ' ' });
			}
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00010E93 File Offset: 0x0000F093
		public void AddTag(string tag)
		{
			EngineApplicationInterface.IGameEntity.AddTag(this.Pointer, tag);
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00010EA6 File Offset: 0x0000F0A6
		public void RemoveTag(string tag)
		{
			EngineApplicationInterface.IGameEntity.RemoveTag(this.Pointer, tag);
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00010EB9 File Offset: 0x0000F0B9
		public bool HasTag(string tag)
		{
			return EngineApplicationInterface.IGameEntity.HasTag(this.Pointer, tag);
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00010ECC File Offset: 0x0000F0CC
		public void AddChild(WeakGameEntity gameEntity, bool autoLocalizeFrame = false)
		{
			EngineApplicationInterface.IGameEntity.AddChild(this.Pointer, gameEntity.Pointer, autoLocalizeFrame);
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00010EE6 File Offset: 0x0000F0E6
		public void RemoveChild(WeakGameEntity childEntity, bool keepPhysics, bool keepScenePointer, bool callScriptCallbacks, int removeReason)
		{
			EngineApplicationInterface.IGameEntity.RemoveChild(this.Pointer, childEntity.Pointer, keepPhysics, keepScenePointer, callScriptCallbacks, removeReason);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00010F05 File Offset: 0x0000F105
		public void BreakPrefab()
		{
			EngineApplicationInterface.IGameEntity.BreakPrefab(this.Pointer);
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x00010F17 File Offset: 0x0000F117
		public int ChildCount
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetChildCount(this.Pointer);
			}
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00010F2C File Offset: 0x0000F12C
		public WeakGameEntity GetChild(int index)
		{
			UIntPtr childPointer = EngineApplicationInterface.IGameEntity.GetChildPointer(this.Pointer, index);
			if (!(childPointer != UIntPtr.Zero))
			{
				return new WeakGameEntity(UIntPtr.Zero);
			}
			return new WeakGameEntity(childPointer);
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x00010F6C File Offset: 0x0000F16C
		public WeakGameEntity Parent
		{
			get
			{
				UIntPtr parentPointer = EngineApplicationInterface.IGameEntity.GetParentPointer(this.Pointer);
				if (!(parentPointer != UIntPtr.Zero))
				{
					return new WeakGameEntity(UIntPtr.Zero);
				}
				return new WeakGameEntity(parentPointer);
			}
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x00010FA8 File Offset: 0x0000F1A8
		public bool HasComplexAnimTree()
		{
			return EngineApplicationInterface.IGameEntity.HasComplexAnimTree(this.Pointer);
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x00010FBA File Offset: 0x0000F1BA
		public WeakGameEntity Root
		{
			get
			{
				return new WeakGameEntity(EngineApplicationInterface.IGameEntity.GetRootParentPointer(this.Pointer));
			}
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00010FD1 File Offset: 0x0000F1D1
		public void AddMultiMesh(MetaMesh metaMesh, bool updateVisMask = true)
		{
			EngineApplicationInterface.IGameEntity.AddMultiMesh(this.Pointer, metaMesh.Pointer, updateVisMask);
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00010FEA File Offset: 0x0000F1EA
		public bool RemoveMultiMesh(MetaMesh metaMesh)
		{
			return EngineApplicationInterface.IGameEntity.RemoveMultiMesh(this.Pointer, metaMesh.Pointer);
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x00011002 File Offset: 0x0000F202
		public int MultiMeshComponentCount
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetComponentCount(this.Pointer, GameEntity.ComponentType.MetaMesh);
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x00011015 File Offset: 0x0000F215
		public int ClothSimulatorComponentCount
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetComponentCount(this.Pointer, GameEntity.ComponentType.ClothSimulator);
			}
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00011028 File Offset: 0x0000F228
		public int GetComponentCount(GameEntity.ComponentType componentType)
		{
			return EngineApplicationInterface.IGameEntity.GetComponentCount(this.Pointer, componentType);
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0001103B File Offset: 0x0000F23B
		public void AddAllMeshesOfGameEntity(GameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.AddAllMeshesOfGameEntity(this.Pointer, gameEntity.Pointer);
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00011053 File Offset: 0x0000F253
		public void SetFrameChanged()
		{
			EngineApplicationInterface.IGameEntity.SetFrameChanged(this.Pointer);
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00011065 File Offset: 0x0000F265
		public GameEntityComponent GetComponentAtIndex(int index, GameEntity.ComponentType componentType)
		{
			return EngineApplicationInterface.IGameEntity.GetComponentAtIndex(this.Pointer, componentType, index);
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00011079 File Offset: 0x0000F279
		public MetaMesh GetMetaMesh(int metaMeshIndex)
		{
			return (MetaMesh)EngineApplicationInterface.IGameEntity.GetComponentAtIndex(this.Pointer, GameEntity.ComponentType.MetaMesh, metaMeshIndex);
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00011092 File Offset: 0x0000F292
		public ClothSimulatorComponent GetClothSimulator(int clothSimulatorIndex)
		{
			return (ClothSimulatorComponent)EngineApplicationInterface.IGameEntity.GetComponentAtIndex(this.Pointer, GameEntity.ComponentType.ClothSimulator, clothSimulatorIndex);
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x000110AB File Offset: 0x0000F2AB
		public void SetVectorArgument(float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			EngineApplicationInterface.IGameEntity.SetVectorArgument(this.Pointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x000110C2 File Offset: 0x0000F2C2
		public void SetMaterialForAllMeshes(Material material)
		{
			EngineApplicationInterface.IGameEntity.SetMaterialForAllMeshes(this.Pointer, material.Pointer);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x000110DA File Offset: 0x0000F2DA
		public bool AddLight(Light light)
		{
			return EngineApplicationInterface.IGameEntity.AddLight(this.Pointer, light.Pointer);
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x000110F2 File Offset: 0x0000F2F2
		public Light GetLight()
		{
			return EngineApplicationInterface.IGameEntity.GetLight(this.Pointer);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00011104 File Offset: 0x0000F304
		public void AddParticleSystemComponent(string particleid)
		{
			EngineApplicationInterface.IGameEntity.AddParticleSystemComponent(this.Pointer, particleid);
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00011117 File Offset: 0x0000F317
		public void RemoveAllParticleSystems()
		{
			EngineApplicationInterface.IGameEntity.RemoveAllParticleSystems(this.Pointer);
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00011129 File Offset: 0x0000F329
		public bool CheckPointWithOrientedBoundingBox(Vec3 point)
		{
			return EngineApplicationInterface.IGameEntity.CheckPointWithOrientedBoundingBox(this.Pointer, point);
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0001113C File Offset: 0x0000F33C
		public void PauseParticleSystem(bool doChildren)
		{
			EngineApplicationInterface.IGameEntity.PauseParticleSystem(this.Pointer, doChildren);
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0001114F File Offset: 0x0000F34F
		public void ResumeParticleSystem(bool doChildren)
		{
			EngineApplicationInterface.IGameEntity.ResumeParticleSystem(this.Pointer, doChildren);
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00011162 File Offset: 0x0000F362
		public void BurstEntityParticle(bool doChildren)
		{
			EngineApplicationInterface.IGameEntity.BurstEntityParticle(this.Pointer, doChildren);
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x00011175 File Offset: 0x0000F375
		public void SetRuntimeEmissionRateMultiplier(float emissionRateMultiplier)
		{
			EngineApplicationInterface.IGameEntity.SetRuntimeEmissionRateMultiplier(this.Pointer, emissionRateMultiplier);
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x00011188 File Offset: 0x0000F388
		public BoundingBox GetLocalBoundingBox()
		{
			return EngineApplicationInterface.IGameEntity.GetLocalBoundingBox(this.Pointer);
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x0001119A File Offset: 0x0000F39A
		public BoundingBox GetGlobalBoundingBox()
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalBoundingBox(this.Pointer);
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x000111AC File Offset: 0x0000F3AC
		public Vec3 GetBoundingBoxMin()
		{
			return EngineApplicationInterface.IGameEntity.GetBoundingBoxMin(this.Pointer);
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x000111BE File Offset: 0x0000F3BE
		public void SetHasCustomBoundingBoxValidationSystem(bool hasCustomBoundingBox)
		{
			EngineApplicationInterface.IGameEntity.SetHasCustomBoundingBoxValidationSystem(this.Pointer, hasCustomBoundingBox);
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x000111D1 File Offset: 0x0000F3D1
		public void ValidateBoundingBox()
		{
			EngineApplicationInterface.IGameEntity.ValidateBoundingBox(this.Pointer);
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x000111E3 File Offset: 0x0000F3E3
		public Vec3 GetBoundingBoxMax()
		{
			return EngineApplicationInterface.IGameEntity.GetBoundingBoxMax(this.Pointer);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x000111F5 File Offset: 0x0000F3F5
		public void UpdateGlobalBounds()
		{
			EngineApplicationInterface.IGameEntity.UpdateGlobalBounds(this.Pointer);
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00011207 File Offset: 0x0000F407
		public void RecomputeBoundingBox()
		{
			EngineApplicationInterface.IGameEntity.RecomputeBoundingBox(this.Pointer);
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00011219 File Offset: 0x0000F419
		public float GetBoundingBoxRadius()
		{
			return EngineApplicationInterface.IGameEntity.GetRadius(this.Pointer);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0001122B File Offset: 0x0000F42B
		public void SetBoundingboxDirty()
		{
			EngineApplicationInterface.IGameEntity.SetBoundingboxDirty(this.Pointer);
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x0001123D File Offset: 0x0000F43D
		public Vec3 GlobalBoxMax
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetGlobalBoxMax(this.Pointer);
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00011250 File Offset: 0x0000F450
		public ValueTuple<Vec3, Vec3> ComputeGlobalPhysicsBoundingBoxMinMax()
		{
			MatrixFrame globalFrame = this.GetGlobalFrame();
			BoundingBox localPhysicsBoundingBox = this.GetLocalPhysicsBoundingBox(true);
			Vec3 vec = globalFrame.TransformToParent(in localPhysicsBoundingBox.min);
			Vec3 vec2 = globalFrame.TransformToParent(in localPhysicsBoundingBox.max);
			return new ValueTuple<Vec3, Vec3>(vec, vec2);
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00011294 File Offset: 0x0000F494
		public Vec3 ComputeGlobalPhysicsBoundingBoxCenter()
		{
			return this.GetGlobalFrame().TransformToParent(in this.GetLocalPhysicsBoundingBox(true).center);
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x000112C4 File Offset: 0x0000F4C4
		public void SetContourColor(uint? color, bool alwaysVisible = true)
		{
			if (color != null)
			{
				EngineApplicationInterface.IGameEntity.SetAsContourEntity(this.Pointer, color.Value);
				EngineApplicationInterface.IGameEntity.SetContourState(this.Pointer, alwaysVisible);
				return;
			}
			EngineApplicationInterface.IGameEntity.DisableContour(this.Pointer);
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x00011313 File Offset: 0x0000F513
		public Vec3 GlobalBoxMin
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetGlobalBoxMin(this.Pointer);
			}
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00011325 File Offset: 0x0000F525
		public bool GetHasFrameChanged()
		{
			return EngineApplicationInterface.IGameEntity.HasFrameChanged(this.Pointer);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00011337 File Offset: 0x0000F537
		public Mesh GetFirstMesh()
		{
			return EngineApplicationInterface.IGameEntity.GetFirstMesh(this.Pointer);
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00011349 File Offset: 0x0000F549
		public int GetAttachedNavmeshFaceCount()
		{
			return EngineApplicationInterface.IGameEntity.GetAttachedNavmeshFaceCount(this.Pointer);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0001135B File Offset: 0x0000F55B
		public void GetAttachedNavmeshFaceRecords(PathFaceRecord[] faceRecords)
		{
			EngineApplicationInterface.IGameEntity.GetAttachedNavmeshFaceRecords(this.Pointer, faceRecords);
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0001136E File Offset: 0x0000F56E
		public void GetAttachedNavmeshFaceVertexIndices(in PathFaceRecord faceRecord, int[] indices)
		{
			EngineApplicationInterface.IGameEntity.GetAttachedNavmeshFaceVertexIndices(this.Pointer, in faceRecord, indices);
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00011382 File Offset: 0x0000F582
		public void SetCustomVertexPositionEnabled(bool customVertexPositionEnabled)
		{
			EngineApplicationInterface.IGameEntity.SetCustomVertexPositionEnabled(this.Pointer, customVertexPositionEnabled);
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00011395 File Offset: 0x0000F595
		public void SetPositionsForAttachedNavmeshVertices(int[] vertices, int indexCount, Vec3[] positions)
		{
			EngineApplicationInterface.IGameEntity.SetPositionsForAttachedNavmeshVertices(this.Pointer, vertices, indexCount, positions);
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x000113AA File Offset: 0x0000F5AA
		public void SetCostAdderForAttachedFaces(float costs)
		{
			EngineApplicationInterface.IGameEntity.SetCostAdderForAttachedFaces(this.Pointer, costs);
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x000113BD File Offset: 0x0000F5BD
		public void SetExternalReferencesUsage(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetExternalReferencesUsage(this.Pointer, value);
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x000113D0 File Offset: 0x0000F5D0
		public void SetMorphFrameOfComponents(float value)
		{
			EngineApplicationInterface.IGameEntity.SetMorphFrameOfComponents(this.Pointer, value);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x000113E3 File Offset: 0x0000F5E3
		public void AddEditDataUserToAllMeshes(bool entityComponents, bool skeletonComponents)
		{
			EngineApplicationInterface.IGameEntity.AddEditDataUserToAllMeshes(this.Pointer, entityComponents, skeletonComponents);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x000113F7 File Offset: 0x0000F5F7
		public void ReleaseEditDataUserToAllMeshes(bool entityComponents, bool skeletonComponents)
		{
			EngineApplicationInterface.IGameEntity.ReleaseEditDataUserToAllMeshes(this.Pointer, entityComponents, skeletonComponents);
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0001140B File Offset: 0x0000F60B
		public void GetCameraParamsFromCameraScript(Camera cam, ref Vec3 dofParams)
		{
			EngineApplicationInterface.IGameEntity.GetCameraParamsFromCameraScript(this.Pointer, cam.Pointer, ref dofParams);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00011424 File Offset: 0x0000F624
		public void GetMeshBendedFrame(MatrixFrame worldSpacePosition, ref MatrixFrame output)
		{
			EngineApplicationInterface.IGameEntity.GetMeshBendedPosition(this.Pointer, ref worldSpacePosition, ref output);
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00011439 File Offset: 0x0000F639
		public void ComputeTrajectoryVolume(float missileSpeed, float verticalAngleMaxInDegrees, float verticalAngleMinInDegrees, float horizontalAngleRangeInDegrees, float airFrictionConstant)
		{
			EngineApplicationInterface.IGameEntity.ComputeTrajectoryVolume(this.Pointer, missileSpeed, verticalAngleMaxInDegrees, verticalAngleMinInDegrees, horizontalAngleRangeInDegrees, airFrictionConstant);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x00011452 File Offset: 0x0000F652
		public void SetAnimTreeChannelParameterForceUpdate(float phase, int channelNo)
		{
			EngineApplicationInterface.IGameEntity.SetAnimTreeChannelParameter(this.Pointer, phase, channelNo);
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x00011466 File Offset: 0x0000F666
		public void ChangeMetaMeshOrRemoveItIfNotExists(MetaMesh entityMetaMesh, MetaMesh newMetaMesh)
		{
			EngineApplicationInterface.IGameEntity.ChangeMetaMeshOrRemoveItIfNotExists(this.Pointer, (entityMetaMesh != null) ? entityMetaMesh.Pointer : UIntPtr.Zero, (newMetaMesh != null) ? newMetaMesh.Pointer : UIntPtr.Zero);
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x000114A4 File Offset: 0x0000F6A4
		public void SetUpdateValidtyOnFrameChangedOfFacesWithId(int faceGroupId, bool updateValidity)
		{
			EngineApplicationInterface.IGameEntity.SetUpdateValidityOnFrameChangedOfFacesWithId(this.Pointer, faceGroupId, updateValidity);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x000114B8 File Offset: 0x0000F6B8
		public void AttachNavigationMeshFaces(int faceGroupId, bool isConnected, bool isBlocker = false, bool autoLocalize = false, bool finalizeBlockerConvexHullComputation = false, bool updateEntityFrame = true)
		{
			EngineApplicationInterface.IGameEntity.AttachNavigationMeshFaces(this.Pointer, faceGroupId, isConnected, isBlocker, autoLocalize, finalizeBlockerConvexHullComputation, updateEntityFrame);
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x000114D3 File Offset: 0x0000F6D3
		public void DetachAllAttachedNavigationMeshFaces()
		{
			EngineApplicationInterface.IGameEntity.DetachAllAttachedNavigationMeshFaces(this.Pointer);
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x000114E5 File Offset: 0x0000F6E5
		public void UpdateAttachedNavigationMeshFaces()
		{
			EngineApplicationInterface.IGameEntity.UpdateAttachedNavigationMeshFaces(this.Pointer);
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x000114F7 File Offset: 0x0000F6F7
		public void RemoveSkeleton()
		{
			this.Skeleton = null;
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x00011500 File Offset: 0x0000F700
		// (set) Token: 0x06000EE0 RID: 3808 RVA: 0x00011512 File Offset: 0x0000F712
		public Skeleton Skeleton
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetSkeleton(this.Pointer);
			}
			set
			{
				EngineApplicationInterface.IGameEntity.SetSkeleton(this.Pointer, (value != null) ? value.Pointer : UIntPtr.Zero);
			}
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00011534 File Offset: 0x0000F734
		public void RemoveAllChildren()
		{
			EngineApplicationInterface.IGameEntity.RemoveAllChildren(this.Pointer);
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00011546 File Offset: 0x0000F746
		public IEnumerable<WeakGameEntity> GetChildren()
		{
			int count = this.ChildCount;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return this.GetChild(i);
				num = i;
			}
			yield break;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0001155B File Offset: 0x0000F75B
		public IEnumerable<WeakGameEntity> GetEntityAndChildren()
		{
			yield return ref this;
			int count = this.ChildCount;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return this.GetChild(i);
				num = i;
			}
			yield break;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00011570 File Offset: 0x0000F770
		public void GetChildrenRecursive(ref List<WeakGameEntity> children)
		{
			int childCount = this.ChildCount;
			for (int i = 0; i < childCount; i++)
			{
				WeakGameEntity child = this.GetChild(i);
				children.Add(child);
				child.GetChildrenRecursive(ref children);
			}
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x000115A8 File Offset: 0x0000F7A8
		public void GetChildrenWithTagRecursive(List<WeakGameEntity> children, string tag)
		{
			int childCount = this.ChildCount;
			for (int i = 0; i < childCount; i++)
			{
				WeakGameEntity child = this.GetChild(i);
				if (child.HasTag(tag))
				{
					children.Add(child);
				}
				child.GetChildrenWithTagRecursive(children, tag);
			}
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x000115EA File Offset: 0x0000F7EA
		public bool IsSelectedOnEditor()
		{
			return EngineApplicationInterface.IGameEntity.IsEntitySelectedOnEditor(this.Pointer);
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x000115FC File Offset: 0x0000F7FC
		public void SelectEntityOnEditor()
		{
			EngineApplicationInterface.IGameEntity.SelectEntityOnEditor(this.Pointer);
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0001160E File Offset: 0x0000F80E
		public void DeselectEntityOnEditor()
		{
			EngineApplicationInterface.IGameEntity.DeselectEntityOnEditor(this.Pointer);
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00011620 File Offset: 0x0000F820
		public void SetAsPredisplayEntity()
		{
			EngineApplicationInterface.IGameEntity.SetAsPredisplayEntity(this.Pointer);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00011632 File Offset: 0x0000F832
		public void RemoveFromPredisplayEntity()
		{
			EngineApplicationInterface.IGameEntity.RemoveFromPredisplayEntity(this.Pointer);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00011644 File Offset: 0x0000F844
		public void SetNativeScriptComponentVariable(string className, string fieldName, ref ScriptComponentFieldHolder data, RglScriptFieldType variableType)
		{
			EngineApplicationInterface.IGameEntity.SetNativeScriptComponentVariable(this.Pointer, className, fieldName, ref data, variableType);
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0001165B File Offset: 0x0000F85B
		public void SetManualGlobalBoundingBox(Vec3 boundingBoxStartGlobal, Vec3 boundingBoxEndGlobal)
		{
			EngineApplicationInterface.IGameEntity.SetManualGlobalBoundingBox(this.Pointer, boundingBoxStartGlobal, boundingBoxEndGlobal);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0001166F File Offset: 0x0000F86F
		public bool RayHitEntityWithNormal(Vec3 rayOrigin, Vec3 rayDirection, float maxLength, ref Vec3 resultNormal, ref float resultLength)
		{
			return EngineApplicationInterface.IGameEntity.RayHitEntityWithNormal(this.Pointer, in rayOrigin, in rayDirection, maxLength, ref resultNormal, ref resultLength);
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0001168A File Offset: 0x0000F88A
		public bool RayHitEntity(Vec3 rayOrigin, Vec3 rayDirection, float maxLength, ref float resultLength)
		{
			return EngineApplicationInterface.IGameEntity.RayHitEntity(this.Pointer, in rayOrigin, in rayDirection, maxLength, ref resultLength);
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x000116A3 File Offset: 0x0000F8A3
		public void GetNativeScriptComponentVariable(string className, string fieldName, ref ScriptComponentFieldHolder data, RglScriptFieldType variableType)
		{
			EngineApplicationInterface.IGameEntity.GetNativeScriptComponentVariable(this.Pointer, className, fieldName, ref data, variableType);
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x000116BA File Offset: 0x0000F8BA
		public void SetCustomClipPlane(Vec3 clipPosition, Vec3 clipNormal, bool setForChildren)
		{
			EngineApplicationInterface.IGameEntity.SetCustomClipPlane(this.Pointer, clipPosition, clipNormal, setForChildren);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x000116CF File Offset: 0x0000F8CF
		public float GetBoundingBoxLongestHalfDimension()
		{
			return BoundingBox.GetLongestHalfDimensionOfBoundingBox(this.GetLocalBoundingBox());
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x000116DC File Offset: 0x0000F8DC
		public BoundingBox ComputeBoundingBoxFromLongestHalfDimension(float longestHalfDimensionCoefficient)
		{
			BoundingBox localBoundingBox = this.GetLocalBoundingBox();
			BoundingBox boundingBox = default(BoundingBox);
			float num = this.GetBoundingBoxLongestHalfDimension() * longestHalfDimensionCoefficient;
			Vec3 vec = new Vec3(num, num, num, -1f);
			boundingBox.min = localBoundingBox.center - vec;
			boundingBox.max = localBoundingBox.center + vec;
			boundingBox.RecomputeRadius();
			return boundingBox;
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00011740 File Offset: 0x0000F940
		public BoundingBox ComputeBoundingBoxIncludeChildren()
		{
			BoundingBox boundingBox = default(BoundingBox);
			boundingBox.BeginRelaxation();
			foreach (WeakGameEntity weakGameEntity in this.GetChildren())
			{
				weakGameEntity.ValidateBoundingBox();
				BoundingBox localBoundingBox = weakGameEntity.GetLocalBoundingBox();
				boundingBox.RelaxWithChildBoundingBox(localBoundingBox, weakGameEntity.GetFrame());
			}
			boundingBox.RecomputeRadius();
			return boundingBox;
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x000117BC File Offset: 0x0000F9BC
		public void SetManualLocalBoundingBox(in BoundingBox boundingBox)
		{
			EngineApplicationInterface.IGameEntity.SetManualLocalBoundingBox(this.Pointer, in boundingBox);
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x000117CF File Offset: 0x0000F9CF
		public void RelaxLocalBoundingBox(in BoundingBox boundingBox)
		{
			EngineApplicationInterface.IGameEntity.RelaxLocalBoundingBox(this.Pointer, in boundingBox);
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x000117E2 File Offset: 0x0000F9E2
		public void SetCullMode(MBMeshCullingMode cullMode)
		{
			EngineApplicationInterface.IGameEntity.SetCullMode(this.Pointer, cullMode);
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x000117F8 File Offset: 0x0000F9F8
		public WeakGameEntity GetFirstChildEntityWithTagRecursive(string tag)
		{
			UIntPtr firstChildWithTagRecursive = EngineApplicationInterface.IGameEntity.GetFirstChildWithTagRecursive(this.Pointer, tag);
			if (firstChildWithTagRecursive != UIntPtr.Zero)
			{
				return new WeakGameEntity(firstChildWithTagRecursive);
			}
			return WeakGameEntity.Invalid;
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00011830 File Offset: 0x0000FA30
		public override bool Equals(object obj)
		{
			return ((WeakGameEntity)obj).Pointer == this.Pointer;
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00011858 File Offset: 0x0000FA58
		public override int GetHashCode()
		{
			return this.Pointer.GetHashCode();
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00011873 File Offset: 0x0000FA73
		public static bool operator ==(WeakGameEntity weakGameEntity, GameEntity entity)
		{
			return weakGameEntity.Pointer == ((entity != null) ? entity.Pointer : UIntPtr.Zero);
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00011891 File Offset: 0x0000FA91
		public static bool operator !=(WeakGameEntity weakGameEntity, GameEntity entity)
		{
			return weakGameEntity.Pointer != ((entity != null) ? entity.Pointer : UIntPtr.Zero);
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x000118AF File Offset: 0x0000FAAF
		public static bool operator ==(WeakGameEntity weakGameEntity1, WeakGameEntity weakGameEntity2)
		{
			return weakGameEntity1.Pointer == weakGameEntity2.Pointer;
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x000118C4 File Offset: 0x0000FAC4
		public static bool operator !=(WeakGameEntity weakGameEntity1, WeakGameEntity weakGameEntity2)
		{
			return weakGameEntity1.Pointer != weakGameEntity2.Pointer;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x000118DC File Offset: 0x0000FADC
		public List<WeakGameEntity> CollectChildrenEntitiesWithTag(string tag)
		{
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			foreach (WeakGameEntity weakGameEntity in this.GetChildren())
			{
				if (weakGameEntity.HasTag(tag))
				{
					list.Add(weakGameEntity);
				}
				if (weakGameEntity.ChildCount > 0)
				{
					list.AddRange(weakGameEntity.CollectChildrenEntitiesWithTag(tag));
				}
			}
			return list;
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00011954 File Offset: 0x0000FB54
		public IEnumerable<WeakGameEntity> CollectChildrenEntitiesWithTagAsEnumarable(string tag)
		{
			foreach (WeakGameEntity child in this.GetChildren())
			{
				if (child.HasTag(tag))
				{
					yield return child;
				}
				if (child.ChildCount > 0)
				{
					foreach (WeakGameEntity weakGameEntity in child.CollectChildrenEntitiesWithTagAsEnumarable(tag))
					{
						yield return weakGameEntity;
					}
					IEnumerator<WeakGameEntity> enumerator2 = null;
				}
			}
			IEnumerator<WeakGameEntity> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00011970 File Offset: 0x0000FB70
		public void SetDoNotCheckVisibility(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetDoNotCheckVisibility(this.Pointer, value);
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00011983 File Offset: 0x0000FB83
		public void SetBoneFrameToAllMeshes(int boneIndex, in MatrixFrame frame)
		{
			EngineApplicationInterface.IGameEntity.SetBoneFrameToAllMeshes(this.Pointer, boneIndex, in frame);
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00011997 File Offset: 0x0000FB97
		public Vec2 GetGlobalWindStrengthVectorOfScene()
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalWindStrengthVectorOfScene(this.Pointer);
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x000119A9 File Offset: 0x0000FBA9
		public Vec2 GetGlobalWindVelocityOfScene()
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalWindVelocityOfScene(this.Pointer);
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x000119BB File Offset: 0x0000FBBB
		public Vec3 GetLastFinalRenderCameraPositionOfScene()
		{
			return EngineApplicationInterface.IGameEntity.GetLastFinalRenderCameraPositionOfScene(this.Pointer);
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x000119CD File Offset: 0x0000FBCD
		public Vec2 GetGlobalWindVelocityWithGustNoiseOfScene(float globalTime)
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalWindVelocityWithGustNoiseOfScene(this.Pointer, globalTime);
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x000119E0 File Offset: 0x0000FBE0
		public void SetForceDecalsToRender(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetForceDecalsToRender(this.Pointer, value);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x000119F3 File Offset: 0x0000FBF3
		public UIntPtr CreateEmptyPhysxShape(bool isVariable, int physxMaterialIndex)
		{
			return EngineApplicationInterface.IGameEntity.CreateEmptyPhysxShape(this.Pointer, isVariable, physxMaterialIndex);
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00011A07 File Offset: 0x0000FC07
		public void SetForceNotAffectedBySeason(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetForceNotAffectedBySeason(this.Pointer, value);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x00011A1A File Offset: 0x0000FC1A
		public bool CheckIsPrefabLinkRootPrefab(int depth)
		{
			return EngineApplicationInterface.IGameEntity.CheckIsPrefabLinkRootPrefab(this.Pointer, depth);
		}

		// Token: 0x04000206 RID: 518
		public static readonly WeakGameEntity Invalid = new WeakGameEntity(UIntPtr.Zero);
	}
}
