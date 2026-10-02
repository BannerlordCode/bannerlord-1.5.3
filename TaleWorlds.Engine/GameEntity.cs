using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200004B RID: 75
	[EngineClass("rglEntity")]
	public sealed class GameEntity : NativeObject
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x000042B0 File Offset: 0x000024B0
		public Scene Scene
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetScene(base.Pointer);
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000042C2 File Offset: 0x000024C2
		private GameEntity()
		{
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x000042CA File Offset: 0x000024CA
		internal GameEntity(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x000042D9 File Offset: 0x000024D9
		public WeakGameEntity WeakEntity
		{
			get
			{
				return new WeakGameEntity(base.Pointer);
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x000042E6 File Offset: 0x000024E6
		public static GameEntity CreateFromWeakEntity(WeakGameEntity weakEntity)
		{
			if (!weakEntity.IsValid)
			{
				return null;
			}
			return new GameEntity(weakEntity.Pointer);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x000042FF File Offset: 0x000024FF
		public UIntPtr GetScenePointer()
		{
			return EngineApplicationInterface.IGameEntity.GetScenePointer(base.Pointer);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00004314 File Offset: 0x00002514
		public override string ToString()
		{
			return base.Pointer.ToString();
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0000432F File Offset: 0x0000252F
		public void ClearEntityComponents(bool resetAll, bool removeScripts, bool deleteChildEntities)
		{
			EngineApplicationInterface.IGameEntity.ClearEntityComponents(base.Pointer, resetAll, removeScripts, deleteChildEntities);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00004344 File Offset: 0x00002544
		public void ClearComponents()
		{
			EngineApplicationInterface.IGameEntity.ClearComponents(base.Pointer);
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00004356 File Offset: 0x00002556
		public void ClearOnlyOwnComponents()
		{
			EngineApplicationInterface.IGameEntity.ClearOnlyOwnComponents(base.Pointer);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00004368 File Offset: 0x00002568
		public bool CheckResources(bool addToQueue, bool checkFaceResources)
		{
			return EngineApplicationInterface.IGameEntity.CheckResources(base.Pointer, addToQueue, checkFaceResources);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0000437C File Offset: 0x0000257C
		public void SetMobility(GameEntity.Mobility mobility)
		{
			EngineApplicationInterface.IGameEntity.SetMobility(base.Pointer, mobility);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0000438F File Offset: 0x0000258F
		public GameEntity.Mobility GetMobility()
		{
			return EngineApplicationInterface.IGameEntity.GetMobility(base.Pointer);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x000043A1 File Offset: 0x000025A1
		public void AddMesh(Mesh mesh, bool recomputeBoundingBox = true)
		{
			EngineApplicationInterface.IGameEntity.AddMesh(base.Pointer, mesh.Pointer, recomputeBoundingBox);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x000043BA File Offset: 0x000025BA
		public void AddMultiMeshToSkeleton(MetaMesh metaMesh)
		{
			EngineApplicationInterface.IGameEntity.AddMultiMeshToSkeleton(base.Pointer, metaMesh.Pointer);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x000043D2 File Offset: 0x000025D2
		public void AddMultiMeshToSkeletonBone(MetaMesh metaMesh, sbyte boneIndex)
		{
			EngineApplicationInterface.IGameEntity.AddMultiMeshToSkeletonBone(base.Pointer, metaMesh.Pointer, boneIndex);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x000043EB File Offset: 0x000025EB
		public void SetColorToAllMeshesWithTagRecursive(uint color, string tag)
		{
			EngineApplicationInterface.IGameEntity.SetColorToAllMeshesWithTagRecursive(base.Pointer, color, tag);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x000043FF File Offset: 0x000025FF
		public IEnumerable<Mesh> GetAllMeshesWithTag(string tag)
		{
			List<GameEntity> list = new List<GameEntity>();
			this.GetChildrenRecursive(ref list);
			list.Add(this);
			foreach (GameEntity entity in list)
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
				entity = null;
			}
			List<GameEntity>.Enumerator enumerator = default(List<GameEntity>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00004418 File Offset: 0x00002618
		public void SetColor(uint color1, uint color2, string meshTag)
		{
			foreach (Mesh mesh in this.GetAllMeshesWithTag(meshTag))
			{
				mesh.Color = color1;
				mesh.Color2 = color2;
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0000446C File Offset: 0x0000266C
		public uint GetFactorColor()
		{
			return EngineApplicationInterface.IGameEntity.GetFactorColor(base.Pointer);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0000447E File Offset: 0x0000267E
		public void SetFactorColor(uint color)
		{
			EngineApplicationInterface.IGameEntity.SetFactorColor(base.Pointer, color);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00004491 File Offset: 0x00002691
		public void SetAsReplayEntity()
		{
			EngineApplicationInterface.IGameEntity.SetAsReplayEntity(base.Pointer);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x000044A3 File Offset: 0x000026A3
		public void SetClothMaxDistanceMultiplier(float multiplier)
		{
			EngineApplicationInterface.IGameEntity.SetClothMaxDistanceMultiplier(base.Pointer, multiplier);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x000044B6 File Offset: 0x000026B6
		public void RemoveMultiMeshFromSkeleton(MetaMesh metaMesh)
		{
			EngineApplicationInterface.IGameEntity.RemoveMultiMeshFromSkeleton(base.Pointer, metaMesh.Pointer);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000044CE File Offset: 0x000026CE
		public void RemoveMultiMeshFromSkeletonBone(MetaMesh metaMesh, sbyte boneIndex)
		{
			EngineApplicationInterface.IGameEntity.RemoveMultiMeshFromSkeletonBone(base.Pointer, metaMesh.Pointer, boneIndex);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x000044E7 File Offset: 0x000026E7
		public bool RemoveComponentWithMesh(Mesh mesh)
		{
			return EngineApplicationInterface.IGameEntity.RemoveComponentWithMesh(base.Pointer, mesh.Pointer);
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x000044FF File Offset: 0x000026FF
		public void AddComponent(GameEntityComponent component)
		{
			EngineApplicationInterface.IGameEntity.AddComponent(base.Pointer, component.Pointer);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00004517 File Offset: 0x00002717
		public bool HasComponent(GameEntityComponent component)
		{
			return EngineApplicationInterface.IGameEntity.HasComponent(base.Pointer, component.Pointer);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0000452F File Offset: 0x0000272F
		public bool IsInEditorScene()
		{
			return false;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00004532 File Offset: 0x00002732
		public bool RemoveComponent(GameEntityComponent component)
		{
			return EngineApplicationInterface.IGameEntity.RemoveComponent(base.Pointer, component.Pointer);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0000454A File Offset: 0x0000274A
		public string GetGuid()
		{
			return EngineApplicationInterface.IGameEntity.GetGuid(base.Pointer);
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0000455C File Offset: 0x0000275C
		public bool IsGuidValid()
		{
			return EngineApplicationInterface.IGameEntity.IsGuidValid(base.Pointer);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0000456E File Offset: 0x0000276E
		public void SetEnforcedMaximumLodLevel(int lodLevel)
		{
			EngineApplicationInterface.IGameEntity.SetEnforcedMaximumLodLevel(base.Pointer, lodLevel);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00004581 File Offset: 0x00002781
		public float GetLodLevelForDistanceSq(float distSq)
		{
			return EngineApplicationInterface.IGameEntity.GetLodLevelForDistanceSq(base.Pointer, distSq);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00004594 File Offset: 0x00002794
		public void GetQuickBoneEntitialFrame(sbyte index, out MatrixFrame frame)
		{
			EngineApplicationInterface.IGameEntity.GetQuickBoneEntitialFrame(base.Pointer, index, out frame);
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x000045A8 File Offset: 0x000027A8
		public void UpdateVisibilityMask()
		{
			EngineApplicationInterface.IGameEntity.UpdateVisibilityMask(base.Pointer);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x000045BA File Offset: 0x000027BA
		public static GameEntity CreateEmpty(Scene scene, bool isModifiableFromEditor = true, bool createPhysics = true, bool callScriptCallbacks = true)
		{
			return EngineApplicationInterface.IGameEntity.CreateEmpty(scene.Pointer, isModifiableFromEditor, (UIntPtr)0UL, createPhysics, callScriptCallbacks);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x000045D6 File Offset: 0x000027D6
		public static GameEntity CreateEmptyDynamic(Scene scene, bool isModifiableFromEditor = true)
		{
			GameEntity gameEntity = GameEntity.CreateEmpty(scene, isModifiableFromEditor, true, true);
			gameEntity.SetMobility(GameEntity.Mobility.Dynamic);
			return gameEntity;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x000045E8 File Offset: 0x000027E8
		public static GameEntity CreateEmptyWithoutScene()
		{
			return EngineApplicationInterface.IGameEntity.CreateEmptyWithoutScene();
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x000045F4 File Offset: 0x000027F4
		public static GameEntity CopyFrom(Scene scene, GameEntity entity, bool createPhysics = true, bool callScriptCallbacks = true)
		{
			return EngineApplicationInterface.IGameEntity.CreateEmpty(scene.Pointer, false, entity.Pointer, createPhysics, callScriptCallbacks);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0000460F File Offset: 0x0000280F
		public static GameEntity CopyFrom(Scene scene, WeakGameEntity entity, bool createPhysics = true, bool callScriptCallbacks = true)
		{
			return EngineApplicationInterface.IGameEntity.CreateEmpty(scene.Pointer, false, entity.Pointer, createPhysics, callScriptCallbacks);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0000462C File Offset: 0x0000282C
		public static GameEntity Instantiate(Scene scene, string prefabName, bool callScriptCallbacks, bool createPhysics = true, string scriptInclusingTag = "")
		{
			uint num = uint.MaxValue;
			if (scriptInclusingTag.Length > 0)
			{
				num = Managed.GetStringHashCode(scriptInclusingTag);
			}
			if (scene != null)
			{
				return EngineApplicationInterface.IGameEntity.CreateFromPrefab(scene.Pointer, prefabName, callScriptCallbacks, createPhysics, num);
			}
			return EngineApplicationInterface.IGameEntity.CreateFromPrefab(new UIntPtr(0U), prefabName, callScriptCallbacks, createPhysics, num);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0000467F File Offset: 0x0000287F
		public void CallScriptCallbacks(bool registerScriptComponents)
		{
			EngineApplicationInterface.IGameEntity.CallScriptCallbacks(base.Pointer, registerScriptComponents);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00004694 File Offset: 0x00002894
		public static GameEntity Instantiate(Scene scene, string prefabName, MatrixFrame frame, bool callScriptCallbacks = true)
		{
			return EngineApplicationInterface.IGameEntity.CreateFromPrefabWithInitialFrameAndRestOffset(scene.Pointer, prefabName, true, ref frame, false, 0f, callScriptCallbacks, uint.MaxValue);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x000046C0 File Offset: 0x000028C0
		public static GameEntity InstantiateWithRestOffset(Scene scene, string prefabName, bool createPhysics, MatrixFrame frame, float restOffset, bool callScriptCallbacks = true, string scriptInclusingTag = "")
		{
			uint num = uint.MaxValue;
			if (scriptInclusingTag.Length > 0)
			{
				num = Managed.GetStringHashCode(scriptInclusingTag);
			}
			return EngineApplicationInterface.IGameEntity.CreateFromPrefabWithInitialFrameAndRestOffset(scene.Pointer, prefabName, createPhysics, ref frame, true, restOffset, callScriptCallbacks, num);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x000046FB File Offset: 0x000028FB
		private int ScriptCount
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetScriptComponentCount(base.Pointer);
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0000470D File Offset: 0x0000290D
		public bool IsGhostObject()
		{
			return EngineApplicationInterface.IGameEntity.IsGhostObject(base.Pointer);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0000471F File Offset: 0x0000291F
		public void CreateAndAddScriptComponent(string name, bool callScriptCallbacks)
		{
			EngineApplicationInterface.IGameEntity.CreateAndAddScriptComponent(base.Pointer, name, callScriptCallbacks);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00004733 File Offset: 0x00002933
		public static bool PrefabExists(string name)
		{
			return EngineApplicationInterface.IGameEntity.PrefabExists(name);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00004740 File Offset: 0x00002940
		public void RemoveScriptComponent(UIntPtr scriptComponent, int removeReason)
		{
			EngineApplicationInterface.IGameEntity.RemoveScriptComponent(base.Pointer, scriptComponent, removeReason);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00004754 File Offset: 0x00002954
		public void SetEntityEnvMapVisibility(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetEntityEnvMapVisibility(base.Pointer, value);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00004767 File Offset: 0x00002967
		internal ScriptComponentBehavior GetScriptAtIndex(int index)
		{
			return EngineApplicationInterface.IGameEntity.GetScriptComponentAtIndex(base.Pointer, index);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0000477A File Offset: 0x0000297A
		public bool HasScene()
		{
			return EngineApplicationInterface.IGameEntity.HasScene(base.Pointer);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0000478C File Offset: 0x0000298C
		public bool HasScriptComponent(string scName)
		{
			return EngineApplicationInterface.IGameEntity.HasScriptComponent(base.Pointer, scName);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0000479F File Offset: 0x0000299F
		public IEnumerable<ScriptComponentBehavior> GetScriptComponents()
		{
			int count = this.ScriptCount;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return this.GetScriptAtIndex(i);
				num = i;
			}
			yield break;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x000047AF File Offset: 0x000029AF
		public IEnumerable<T> GetScriptComponents<T>() where T : ScriptComponentBehavior
		{
			int count = this.ScriptCount;
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

		// Token: 0x06000737 RID: 1847 RVA: 0x000047C0 File Offset: 0x000029C0
		public bool HasScriptOfType<T>() where T : ScriptComponentBehavior
		{
			int scriptCount = this.ScriptCount;
			for (int i = 0; i < scriptCount; i++)
			{
				if (this.GetScriptAtIndex(i) is T)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x000047F4 File Offset: 0x000029F4
		public GameEntity GetFirstChildEntityWithTag(string tag)
		{
			foreach (GameEntity gameEntity in this.GetChildren())
			{
				if (gameEntity.HasTag(tag))
				{
					return gameEntity;
				}
			}
			return null;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0000484C File Offset: 0x00002A4C
		public bool HasScriptOfType(Type t)
		{
			return this.GetScriptComponents().Any<ScriptComponentBehavior>((ScriptComponentBehavior sc) => sc.GetType().IsAssignableFrom(t));
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00004880 File Offset: 0x00002A80
		public T GetFirstScriptOfTypeInFamily<T>() where T : ScriptComponentBehavior
		{
			T t = this.GetFirstScriptOfType<T>();
			WeakGameEntity weakGameEntity = this.WeakEntity;
			while (t == null && weakGameEntity.Parent != null)
			{
				weakGameEntity = weakGameEntity.Parent;
				t = weakGameEntity.GetFirstScriptOfType<T>();
			}
			return t;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x000048C8 File Offset: 0x00002AC8
		public T GetFirstScriptOfType<T>() where T : ScriptComponentBehavior
		{
			int scriptCount = this.ScriptCount;
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

		// Token: 0x0600073C RID: 1852 RVA: 0x00004910 File Offset: 0x00002B10
		public T GetFirstScriptOfTypeRecursive<T>() where T : ScriptComponentBehavior
		{
			int num = this.ScriptCount;
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

		// Token: 0x0600073D RID: 1853 RVA: 0x00004984 File Offset: 0x00002B84
		public int GetScriptCountOfTypeRecursive<T>() where T : ScriptComponentBehavior
		{
			int num = this.ScriptCount;
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

		// Token: 0x0600073E RID: 1854 RVA: 0x000049DB File Offset: 0x00002BDB
		internal static GameEntity GetFirstEntityWithName(Scene scene, string entityName)
		{
			return EngineApplicationInterface.IGameEntity.FindWithName(scene.Pointer, entityName);
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x000049EE File Offset: 0x00002BEE
		// (set) Token: 0x06000740 RID: 1856 RVA: 0x00004A00 File Offset: 0x00002C00
		public string Name
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetName(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.IGameEntity.SetName(base.Pointer, value);
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00004A13 File Offset: 0x00002C13
		public void SetAlpha(float alpha)
		{
			EngineApplicationInterface.IGameEntity.SetAlpha(base.Pointer, alpha);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00004A26 File Offset: 0x00002C26
		public void SetVisibilityExcludeParents(bool visible)
		{
			EngineApplicationInterface.IGameEntity.SetVisibilityExcludeParents(base.Pointer, visible);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00004A39 File Offset: 0x00002C39
		public void SetReadyToRender(bool ready)
		{
			EngineApplicationInterface.IGameEntity.SetReadyToRender(base.Pointer, ready);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00004A4C File Offset: 0x00002C4C
		public bool GetVisibilityExcludeParents()
		{
			return EngineApplicationInterface.IGameEntity.GetVisibilityExcludeParents(base.Pointer);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00004A5E File Offset: 0x00002C5E
		public bool IsVisibleIncludeParents()
		{
			return EngineApplicationInterface.IGameEntity.IsVisibleIncludeParents(base.Pointer);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00004A70 File Offset: 0x00002C70
		public uint GetVisibilityLevelMaskIncludingParents()
		{
			return EngineApplicationInterface.IGameEntity.GetVisibilityLevelMaskIncludingParents(base.Pointer);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00004A82 File Offset: 0x00002C82
		public bool GetEditModeLevelVisibility()
		{
			return EngineApplicationInterface.IGameEntity.GetEditModeLevelVisibility(base.Pointer);
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00004A94 File Offset: 0x00002C94
		public void Remove(int removeReason)
		{
			EngineApplicationInterface.IGameEntity.Remove(base.Pointer, removeReason);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00004AA8 File Offset: 0x00002CA8
		internal static GameEntity GetFirstEntityWithTag(Scene scene, string tag)
		{
			UIntPtr firstEntityWithTag = EngineApplicationInterface.IGameEntity.GetFirstEntityWithTag(scene.Pointer, tag);
			if (firstEntityWithTag != UIntPtr.Zero)
			{
				return new GameEntity(firstEntityWithTag);
			}
			return null;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00004ADC File Offset: 0x00002CDC
		internal static GameEntity GetNextEntityWithTag(Scene scene, GameEntity startEntity, string tag)
		{
			if (!(startEntity != null))
			{
				return GameEntity.GetFirstEntityWithTag(scene, tag);
			}
			UIntPtr nextEntityWithTag = EngineApplicationInterface.IGameEntity.GetNextEntityWithTag(startEntity.Pointer, tag);
			if (nextEntityWithTag != UIntPtr.Zero)
			{
				return new GameEntity(nextEntityWithTag);
			}
			return null;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00004B24 File Offset: 0x00002D24
		internal static GameEntity GetFirstEntityWithTagExpression(Scene scene, string tagExpression)
		{
			UIntPtr firstEntityWithTagExpression = EngineApplicationInterface.IGameEntity.GetFirstEntityWithTagExpression(scene.Pointer, tagExpression);
			if (!(firstEntityWithTagExpression != UIntPtr.Zero))
			{
				return null;
			}
			return new GameEntity(firstEntityWithTagExpression);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00004B58 File Offset: 0x00002D58
		internal static GameEntity GetNextEntityWithTagExpression(Scene scene, GameEntity startEntity, string tagExpression)
		{
			if (startEntity == null)
			{
				return GameEntity.GetFirstEntityWithTagExpression(scene, tagExpression);
			}
			UIntPtr nextEntityWithTagExpression = EngineApplicationInterface.IGameEntity.GetNextEntityWithTagExpression(startEntity.Pointer, tagExpression);
			if (nextEntityWithTagExpression != UIntPtr.Zero)
			{
				return new GameEntity(nextEntityWithTagExpression);
			}
			return null;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00004B9D File Offset: 0x00002D9D
		internal static GameEntity GetNextPrefab(GameEntity current)
		{
			return EngineApplicationInterface.IGameEntity.GetNextPrefab((current == null) ? new UIntPtr(0U) : current.Pointer);
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00004BC0 File Offset: 0x00002DC0
		public static GameEntity CopyFromPrefab(GameEntity prefab)
		{
			if (!(prefab != null))
			{
				return null;
			}
			return EngineApplicationInterface.IGameEntity.CopyFromPrefab(prefab.Pointer);
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00004BDD File Offset: 0x00002DDD
		public static GameEntity CopyFromPrefab(WeakGameEntity prefab)
		{
			if (!(prefab != null))
			{
				return null;
			}
			return EngineApplicationInterface.IGameEntity.CopyFromPrefab(prefab.Pointer);
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00004BFB File Offset: 0x00002DFB
		public void SetUpgradeLevelMask(GameEntity.UpgradeLevelMask mask)
		{
			EngineApplicationInterface.IGameEntity.SetUpgradeLevelMask(base.Pointer, (uint)mask);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00004C0E File Offset: 0x00002E0E
		public GameEntity.UpgradeLevelMask GetUpgradeLevelMask()
		{
			return (GameEntity.UpgradeLevelMask)EngineApplicationInterface.IGameEntity.GetUpgradeLevelMask(base.Pointer);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00004C20 File Offset: 0x00002E20
		public GameEntity.UpgradeLevelMask GetUpgradeLevelMaskCumulative()
		{
			return (GameEntity.UpgradeLevelMask)EngineApplicationInterface.IGameEntity.GetUpgradeLevelMaskCumulative(base.Pointer);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00004C34 File Offset: 0x00002E34
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

		// Token: 0x06000754 RID: 1876 RVA: 0x00004C69 File Offset: 0x00002E69
		public string GetOldPrefabName()
		{
			return EngineApplicationInterface.IGameEntity.GetOldPrefabName(base.Pointer);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00004C7B File Offset: 0x00002E7B
		public string GetPrefabName()
		{
			return EngineApplicationInterface.IGameEntity.GetPrefabName(base.Pointer);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00004C8D File Offset: 0x00002E8D
		public void CopyScriptComponentFromAnotherEntity(GameEntity otherEntity, string scriptName)
		{
			EngineApplicationInterface.IGameEntity.CopyScriptComponentFromAnotherEntity(base.Pointer, otherEntity.Pointer, scriptName);
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00004CA6 File Offset: 0x00002EA6
		internal static IEnumerable<GameEntity> GetEntitiesWithTag(Scene scene, string tag)
		{
			GameEntity entity = GameEntity.GetFirstEntityWithTag(scene, tag);
			while (entity != null)
			{
				yield return entity;
				entity = GameEntity.GetNextEntityWithTag(scene, entity, tag);
			}
			yield break;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00004CBD File Offset: 0x00002EBD
		internal static IEnumerable<GameEntity> GetEntitiesWithTagExpression(Scene scene, string tagExpression)
		{
			GameEntity entity = GameEntity.GetFirstEntityWithTagExpression(scene, tagExpression);
			while (entity != null)
			{
				yield return entity;
				entity = GameEntity.GetNextEntityWithTagExpression(scene, entity, tagExpression);
			}
			yield break;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00004CD4 File Offset: 0x00002ED4
		public void SetFrame(ref MatrixFrame frame, bool isTeleportation = true)
		{
			this.SetLocalFrame(ref frame, isTeleportation);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00004CDE File Offset: 0x00002EDE
		public void SetLocalFrame(ref MatrixFrame frame, bool isTeleportation)
		{
			EngineApplicationInterface.IGameEntity.SetLocalFrame(base.Pointer, ref frame, isTeleportation);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00004CF2 File Offset: 0x00002EF2
		public void SetClothComponentKeepState(MetaMesh metaMesh, bool state)
		{
			EngineApplicationInterface.IGameEntity.SetClothComponentKeepState(base.Pointer, metaMesh.Pointer, state);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00004D0B File Offset: 0x00002F0B
		public void SetClothComponentKeepStateOfAllMeshes(bool state)
		{
			EngineApplicationInterface.IGameEntity.SetClothComponentKeepStateOfAllMeshes(base.Pointer, state);
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00004D1E File Offset: 0x00002F1E
		public void SetPreviousFrameInvalid()
		{
			EngineApplicationInterface.IGameEntity.SetPreviousFrameInvalid(base.Pointer);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00004D30 File Offset: 0x00002F30
		public MatrixFrame GetFrame()
		{
			return this.GetLocalFrame();
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00004D38 File Offset: 0x00002F38
		public void GetLocalFrame(out MatrixFrame frame)
		{
			EngineApplicationInterface.IGameEntity.GetLocalFrame(base.Pointer, out frame);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00004D4C File Offset: 0x00002F4C
		public MatrixFrame GetLocalFrame()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetLocalFrame(base.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00004D6C File Offset: 0x00002F6C
		public MatrixFrame GetGlobalFrame()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetGlobalFrame(base.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00004D8C File Offset: 0x00002F8C
		public MatrixFrame GetGlobalFrameImpreciseForFixedTick()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetGlobalFrameImpreciseForFixedTick(base.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00004DAC File Offset: 0x00002FAC
		public MatrixFrame ComputePreciseGlobalFrameForFixedTickSlow()
		{
			MatrixFrame matrixFrame = this.GetLocalFrame();
			WeakGameEntity weakGameEntity = this.WeakEntity.Parent;
			while (weakGameEntity.Parent != null)
			{
				matrixFrame = weakGameEntity.GetLocalFrame().TransformToParent(in matrixFrame);
				weakGameEntity = weakGameEntity.Parent;
			}
			matrixFrame = weakGameEntity.GetBodyWorldTransform().TransformToParent(in matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00004E0D File Offset: 0x0000300D
		public void SetGlobalFrame(in MatrixFrame frame, bool isTeleportation = true)
		{
			EngineApplicationInterface.IGameEntity.SetGlobalFrame(base.Pointer, in frame, isTeleportation);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00004E24 File Offset: 0x00003024
		public MatrixFrame GetPreviousGlobalFrame()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetPreviousGlobalFrame(base.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00004E44 File Offset: 0x00003044
		public MatrixFrame GetBodyWorldTransform()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetBodyWorldTransform(base.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00004E64 File Offset: 0x00003064
		public MatrixFrame GetBodyVisualWorldTransform()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetBodyVisualWorldTransform(base.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00004E84 File Offset: 0x00003084
		public void SetLocalPosition(Vec3 position)
		{
			EngineApplicationInterface.IGameEntity.SetLocalPosition(base.Pointer, position);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00004E97 File Offset: 0x00003097
		public void UpdateTriadFrameForEditor()
		{
			EngineApplicationInterface.IGameEntity.UpdateTriadFrameForEditor(base.Pointer);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00004EAC File Offset: 0x000030AC
		public void UpdateTriadFrameForEditorForAllChildren()
		{
			this.UpdateTriadFrameForEditor();
			List<GameEntity> list = new List<GameEntity>();
			this.GetChildrenRecursive(ref list);
			foreach (GameEntity gameEntity in list)
			{
				EngineApplicationInterface.IGameEntity.UpdateTriadFrameForEditor(gameEntity.Pointer);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x00004F18 File Offset: 0x00003118
		// (set) Token: 0x0600076C RID: 1900 RVA: 0x00004F2A File Offset: 0x0000312A
		public EntityFlags EntityFlags
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetEntityFlags(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.IGameEntity.SetEntityFlags(base.Pointer, value);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x00004F3D File Offset: 0x0000313D
		// (set) Token: 0x0600076E RID: 1902 RVA: 0x00004F4F File Offset: 0x0000314F
		public EntityVisibilityFlags EntityVisibilityFlags
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetEntityVisibilityFlags(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.IGameEntity.SetEntityVisibilityFlags(base.Pointer, value);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x00004F62 File Offset: 0x00003162
		// (set) Token: 0x06000770 RID: 1904 RVA: 0x00004F74 File Offset: 0x00003174
		public BodyFlags BodyFlag
		{
			get
			{
				return (BodyFlags)EngineApplicationInterface.IGameEntity.GetBodyFlags(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.IGameEntity.SetBodyFlags(base.Pointer, (uint)value);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x00004F87 File Offset: 0x00003187
		public BodyFlags PhysicsDescBodyFlag
		{
			get
			{
				return (BodyFlags)EngineApplicationInterface.IGameEntity.GetPhysicsDescBodyFlags(base.Pointer);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x00004F99 File Offset: 0x00003199
		public float Mass
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetMass(base.Pointer);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x00004FAB File Offset: 0x000031AB
		public Vec3 CenterOfMass
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetCenterOfMass(base.Pointer);
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00004FBD File Offset: 0x000031BD
		public PhysicsMaterial GetPhysicsMaterial()
		{
			return PhysicsMaterial.GetFromIndex(EngineApplicationInterface.IGameEntity.GetPhysicsMaterialIndex(base.Pointer));
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00004FD4 File Offset: 0x000031D4
		public void SetBodyFlags(BodyFlags bodyFlags)
		{
			this.BodyFlag = bodyFlags;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00004FDD File Offset: 0x000031DD
		public void SetBodyFlagsRecursive(BodyFlags bodyFlags)
		{
			EngineApplicationInterface.IGameEntity.SetBodyFlagsRecursive(base.Pointer, (uint)bodyFlags);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00004FF0 File Offset: 0x000031F0
		public void AddBodyFlags(BodyFlags bodyFlags, bool applyToChildren = true)
		{
			this.BodyFlag |= bodyFlags;
			if (applyToChildren)
			{
				foreach (GameEntity gameEntity in this.GetChildren())
				{
					gameEntity.AddBodyFlags(bodyFlags, true);
				}
			}
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00005050 File Offset: 0x00003250
		public void RemoveBodyFlags(BodyFlags bodyFlags, bool applyToChildren = true)
		{
			this.BodyFlag &= ~bodyFlags;
			if (applyToChildren)
			{
				foreach (GameEntity gameEntity in this.GetChildren())
				{
					gameEntity.RemoveBodyFlags(bodyFlags, true);
				}
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x000050B0 File Offset: 0x000032B0
		public Vec3 GetGlobalScale()
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalScale(base.Pointer);
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x000050C4 File Offset: 0x000032C4
		public Vec3 GetLocalScale()
		{
			return this.GetLocalFrame().rotation.GetScaleVector();
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x000050E4 File Offset: 0x000032E4
		public Vec3 GlobalPosition
		{
			get
			{
				return this.GetGlobalFrame().origin;
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x000050F4 File Offset: 0x000032F4
		public void SetAnimationSoundActivation(bool activate)
		{
			EngineApplicationInterface.IGameEntity.SetAnimationSoundActivation(base.Pointer, activate);
			foreach (GameEntity gameEntity in this.GetChildren())
			{
				gameEntity.SetAnimationSoundActivation(activate);
			}
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00005150 File Offset: 0x00003350
		public void CopyComponentsToSkeleton()
		{
			EngineApplicationInterface.IGameEntity.CopyComponentsToSkeleton(base.Pointer);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00005162 File Offset: 0x00003362
		public void AddMeshToBone(sbyte boneIndex, Mesh mesh)
		{
			EngineApplicationInterface.IGameEntity.AddMeshToBone(base.Pointer, mesh.Pointer, boneIndex);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0000517B File Offset: 0x0000337B
		public void ActivateRagdoll()
		{
			EngineApplicationInterface.IGameEntity.ActivateRagdoll(base.Pointer);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0000518D File Offset: 0x0000338D
		public void PauseSkeletonAnimation()
		{
			EngineApplicationInterface.IGameEntity.Freeze(base.Pointer, true);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x000051A0 File Offset: 0x000033A0
		public void ResumeSkeletonAnimation()
		{
			EngineApplicationInterface.IGameEntity.Freeze(base.Pointer, false);
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x000051B3 File Offset: 0x000033B3
		public bool IsSkeletonAnimationPaused()
		{
			return EngineApplicationInterface.IGameEntity.IsFrozen(base.Pointer);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x000051C5 File Offset: 0x000033C5
		public sbyte GetBoneCount()
		{
			return EngineApplicationInterface.IGameEntity.GetBoneCount(base.Pointer);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x000051D7 File Offset: 0x000033D7
		public float GetWaterLevelAtPosition(Vec2 position, bool useWaterRenderer, bool checkWaterBodyEntities)
		{
			return EngineApplicationInterface.IGameEntity.GetWaterLevelAtPosition(base.Pointer, in position, useWaterRenderer, checkWaterBodyEntities);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x000051F0 File Offset: 0x000033F0
		public MatrixFrame GetBoneEntitialFrameWithIndex(sbyte boneIndex)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.IGameEntity.GetBoneEntitialFrameWithIndex(base.Pointer, boneIndex, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0000521C File Offset: 0x0000341C
		public MatrixFrame GetBoneEntitialFrameWithName(string boneName)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.IGameEntity.GetBoneEntitialFrameWithName(base.Pointer, boneName, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x00005245 File Offset: 0x00003445
		public string[] Tags
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetTags(base.Pointer).Split(new char[] { ' ' });
			}
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00005267 File Offset: 0x00003467
		public void AddTag(string tag)
		{
			EngineApplicationInterface.IGameEntity.AddTag(base.Pointer, tag);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0000527A File Offset: 0x0000347A
		public void RemoveTag(string tag)
		{
			EngineApplicationInterface.IGameEntity.RemoveTag(base.Pointer, tag);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0000528D File Offset: 0x0000348D
		public bool HasTag(string tag)
		{
			return EngineApplicationInterface.IGameEntity.HasTag(base.Pointer, tag);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x000052A0 File Offset: 0x000034A0
		public void AddChild(GameEntity gameEntity, bool autoLocalizeFrame = false)
		{
			EngineApplicationInterface.IGameEntity.AddChild(base.Pointer, gameEntity.Pointer, autoLocalizeFrame);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x000052B9 File Offset: 0x000034B9
		public void RemoveChild(GameEntity childEntity, bool keepPhysics, bool keepScenePointer, bool callScriptCallbacks, int removeReason)
		{
			EngineApplicationInterface.IGameEntity.RemoveChild(base.Pointer, childEntity.Pointer, keepPhysics, keepScenePointer, callScriptCallbacks, removeReason);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x000052D7 File Offset: 0x000034D7
		public void BreakPrefab()
		{
			EngineApplicationInterface.IGameEntity.BreakPrefab(base.Pointer);
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x000052E9 File Offset: 0x000034E9
		public int ChildCount
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetChildCount(base.Pointer);
			}
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x000052FB File Offset: 0x000034FB
		public GameEntity GetChild(int index)
		{
			return EngineApplicationInterface.IGameEntity.GetChild(base.Pointer, index);
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x0000530E File Offset: 0x0000350E
		public GameEntity Parent
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetParent(base.Pointer);
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00005320 File Offset: 0x00003520
		public bool HasComplexAnimTree()
		{
			return EngineApplicationInterface.IGameEntity.HasComplexAnimTree(base.Pointer);
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x00005334 File Offset: 0x00003534
		public GameEntity Root
		{
			get
			{
				UIntPtr rootParentPointer = EngineApplicationInterface.IGameEntity.GetRootParentPointer(base.Pointer);
				if (!(rootParentPointer != UIntPtr.Zero))
				{
					return null;
				}
				return new GameEntity(rootParentPointer);
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00005367 File Offset: 0x00003567
		public void AddMultiMesh(MetaMesh metaMesh, bool updateVisMask = true)
		{
			EngineApplicationInterface.IGameEntity.AddMultiMesh(base.Pointer, metaMesh.Pointer, updateVisMask);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00005380 File Offset: 0x00003580
		public bool RemoveMultiMesh(MetaMesh metaMesh)
		{
			return EngineApplicationInterface.IGameEntity.RemoveMultiMesh(base.Pointer, metaMesh.Pointer);
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x00005398 File Offset: 0x00003598
		public int MultiMeshComponentCount
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetComponentCount(base.Pointer, GameEntity.ComponentType.MetaMesh);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x000053AB File Offset: 0x000035AB
		public int ClothSimulatorComponentCount
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetComponentCount(base.Pointer, GameEntity.ComponentType.ClothSimulator);
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x000053BE File Offset: 0x000035BE
		public int GetComponentCount(GameEntity.ComponentType componentType)
		{
			return EngineApplicationInterface.IGameEntity.GetComponentCount(base.Pointer, componentType);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x000053D1 File Offset: 0x000035D1
		public void AddAllMeshesOfGameEntity(GameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.AddAllMeshesOfGameEntity(base.Pointer, gameEntity.Pointer);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x000053E9 File Offset: 0x000035E9
		public void SetFrameChanged()
		{
			EngineApplicationInterface.IGameEntity.SetFrameChanged(base.Pointer);
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x000053FB File Offset: 0x000035FB
		public GameEntityComponent GetComponentAtIndex(int index, GameEntity.ComponentType componentType)
		{
			return EngineApplicationInterface.IGameEntity.GetComponentAtIndex(base.Pointer, componentType, index);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0000540F File Offset: 0x0000360F
		public MetaMesh GetMetaMesh(int metaMeshIndex)
		{
			return (MetaMesh)EngineApplicationInterface.IGameEntity.GetComponentAtIndex(base.Pointer, GameEntity.ComponentType.MetaMesh, metaMeshIndex);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00005428 File Offset: 0x00003628
		public ClothSimulatorComponent GetClothSimulator(int clothSimulatorIndex)
		{
			return (ClothSimulatorComponent)EngineApplicationInterface.IGameEntity.GetComponentAtIndex(base.Pointer, GameEntity.ComponentType.ClothSimulator, clothSimulatorIndex);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00005441 File Offset: 0x00003641
		public void SetVectorArgument(float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			EngineApplicationInterface.IGameEntity.SetVectorArgument(base.Pointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00005458 File Offset: 0x00003658
		public void SetMaterialForAllMeshes(Material material)
		{
			EngineApplicationInterface.IGameEntity.SetMaterialForAllMeshes(base.Pointer, material.Pointer);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00005470 File Offset: 0x00003670
		public bool AddLight(Light light)
		{
			return EngineApplicationInterface.IGameEntity.AddLight(base.Pointer, light.Pointer);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00005488 File Offset: 0x00003688
		public Light GetLight()
		{
			return EngineApplicationInterface.IGameEntity.GetLight(base.Pointer);
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0000549A File Offset: 0x0000369A
		public void AddParticleSystemComponent(string particleid)
		{
			EngineApplicationInterface.IGameEntity.AddParticleSystemComponent(base.Pointer, particleid);
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x000054AD File Offset: 0x000036AD
		public void RemoveAllParticleSystems()
		{
			EngineApplicationInterface.IGameEntity.RemoveAllParticleSystems(base.Pointer);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x000054BF File Offset: 0x000036BF
		public bool CheckPointWithOrientedBoundingBox(Vec3 point)
		{
			return EngineApplicationInterface.IGameEntity.CheckPointWithOrientedBoundingBox(base.Pointer, point);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x000054D2 File Offset: 0x000036D2
		public void PauseParticleSystem(bool doChildren)
		{
			EngineApplicationInterface.IGameEntity.PauseParticleSystem(base.Pointer, doChildren);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000054E5 File Offset: 0x000036E5
		public void ResumeParticleSystem(bool doChildren)
		{
			EngineApplicationInterface.IGameEntity.ResumeParticleSystem(base.Pointer, doChildren);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x000054F8 File Offset: 0x000036F8
		public void BurstEntityParticle(bool doChildren)
		{
			EngineApplicationInterface.IGameEntity.BurstEntityParticle(base.Pointer, doChildren);
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0000550B File Offset: 0x0000370B
		public void SetRuntimeEmissionRateMultiplier(float emissionRateMultiplier)
		{
			EngineApplicationInterface.IGameEntity.SetRuntimeEmissionRateMultiplier(base.Pointer, emissionRateMultiplier);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0000551E File Offset: 0x0000371E
		public BoundingBox GetLocalBoundingBox()
		{
			return EngineApplicationInterface.IGameEntity.GetLocalBoundingBox(base.Pointer);
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00005530 File Offset: 0x00003730
		public BoundingBox GetGlobalBoundingBox()
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalBoundingBox(base.Pointer);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00005542 File Offset: 0x00003742
		public Vec3 GetBoundingBoxMin()
		{
			return EngineApplicationInterface.IGameEntity.GetBoundingBoxMin(base.Pointer);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00005554 File Offset: 0x00003754
		public void SetHasCustomBoundingBoxValidationSystem(bool hasCustomBoundingBox)
		{
			EngineApplicationInterface.IGameEntity.SetHasCustomBoundingBoxValidationSystem(base.Pointer, hasCustomBoundingBox);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00005567 File Offset: 0x00003767
		public void ValidateBoundingBox()
		{
			EngineApplicationInterface.IGameEntity.ValidateBoundingBox(base.Pointer);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00005579 File Offset: 0x00003779
		public Vec3 GetBoundingBoxMax()
		{
			return EngineApplicationInterface.IGameEntity.GetBoundingBoxMax(base.Pointer);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0000558B File Offset: 0x0000378B
		public void UpdateGlobalBounds()
		{
			EngineApplicationInterface.IGameEntity.UpdateGlobalBounds(base.Pointer);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0000559D File Offset: 0x0000379D
		public void RecomputeBoundingBox()
		{
			EngineApplicationInterface.IGameEntity.RecomputeBoundingBox(base.Pointer);
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x000055AF File Offset: 0x000037AF
		public float GetBoundingBoxRadius()
		{
			return EngineApplicationInterface.IGameEntity.GetRadius(base.Pointer);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x000055C1 File Offset: 0x000037C1
		public void SetBoundingboxDirty()
		{
			EngineApplicationInterface.IGameEntity.SetBoundingboxDirty(base.Pointer);
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x000055D3 File Offset: 0x000037D3
		public Vec3 GlobalBoxMax
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetGlobalBoxMax(base.Pointer);
			}
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x000055E8 File Offset: 0x000037E8
		public ValueTuple<Vec3, Vec3> ComputeGlobalPhysicsBoundingBoxMinMax()
		{
			MatrixFrame globalFrame = this.GetGlobalFrame();
			BoundingBox localPhysicsBoundingBox = this.GetLocalPhysicsBoundingBox(true);
			Vec3 vec = globalFrame.TransformToParent(in localPhysicsBoundingBox.min);
			Vec3 vec2 = globalFrame.TransformToParent(in localPhysicsBoundingBox.max);
			return new ValueTuple<Vec3, Vec3>(vec, vec2);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00005628 File Offset: 0x00003828
		public void SetContourColor(uint? color, bool alwaysVisible = true)
		{
			if (color != null)
			{
				EngineApplicationInterface.IGameEntity.SetAsContourEntity(base.Pointer, color.Value);
				EngineApplicationInterface.IGameEntity.SetContourState(base.Pointer, alwaysVisible);
				return;
			}
			EngineApplicationInterface.IGameEntity.DisableContour(base.Pointer);
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00005677 File Offset: 0x00003877
		public Vec3 GlobalBoxMin
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetGlobalBoxMin(base.Pointer);
			}
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00005689 File Offset: 0x00003889
		public bool GetHasFrameChanged()
		{
			return EngineApplicationInterface.IGameEntity.HasFrameChanged(base.Pointer);
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0000569B File Offset: 0x0000389B
		public Mesh GetFirstMesh()
		{
			return EngineApplicationInterface.IGameEntity.GetFirstMesh(base.Pointer);
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x000056AD File Offset: 0x000038AD
		public int GetAttachedNavmeshFaceCount()
		{
			return EngineApplicationInterface.IGameEntity.GetAttachedNavmeshFaceCount(base.Pointer);
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x000056BF File Offset: 0x000038BF
		public void GetAttachedNavmeshFaceRecords(PathFaceRecord[] faceRecords)
		{
			EngineApplicationInterface.IGameEntity.GetAttachedNavmeshFaceRecords(base.Pointer, faceRecords);
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x000056D2 File Offset: 0x000038D2
		public void SetExternalReferencesUsage(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetExternalReferencesUsage(base.Pointer, value);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x000056E5 File Offset: 0x000038E5
		public void SetMorphFrameOfComponents(float value)
		{
			EngineApplicationInterface.IGameEntity.SetMorphFrameOfComponents(base.Pointer, value);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x000056F8 File Offset: 0x000038F8
		public void AddEditDataUserToAllMeshes(bool entityComponents, bool skeletonComponents)
		{
			EngineApplicationInterface.IGameEntity.AddEditDataUserToAllMeshes(base.Pointer, entityComponents, skeletonComponents);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0000570C File Offset: 0x0000390C
		public void ReleaseEditDataUserToAllMeshes(bool entityComponents, bool skeletonComponents)
		{
			EngineApplicationInterface.IGameEntity.ReleaseEditDataUserToAllMeshes(base.Pointer, entityComponents, skeletonComponents);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00005720 File Offset: 0x00003920
		public void GetCameraParamsFromCameraScript(Camera cam, ref Vec3 dofParams)
		{
			EngineApplicationInterface.IGameEntity.GetCameraParamsFromCameraScript(base.Pointer, cam.Pointer, ref dofParams);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00005739 File Offset: 0x00003939
		public void GetMeshBendedFrame(MatrixFrame worldSpacePosition, ref MatrixFrame output)
		{
			EngineApplicationInterface.IGameEntity.GetMeshBendedPosition(base.Pointer, ref worldSpacePosition, ref output);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0000574E File Offset: 0x0000394E
		public void ComputeTrajectoryVolume(float missileSpeed, float verticalAngleMaxInDegrees, float verticalAngleMinInDegrees, float horizontalAngleRangeInDegrees, float airFrictionConstant)
		{
			EngineApplicationInterface.IGameEntity.ComputeTrajectoryVolume(base.Pointer, missileSpeed, verticalAngleMaxInDegrees, verticalAngleMinInDegrees, horizontalAngleRangeInDegrees, airFrictionConstant);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00005767 File Offset: 0x00003967
		public void SetAnimTreeChannelParameterForceUpdate(float phase, int channelNo)
		{
			EngineApplicationInterface.IGameEntity.SetAnimTreeChannelParameter(base.Pointer, phase, channelNo);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0000577B File Offset: 0x0000397B
		public void ChangeMetaMeshOrRemoveItIfNotExists(MetaMesh entityMetaMesh, MetaMesh newMetaMesh)
		{
			EngineApplicationInterface.IGameEntity.ChangeMetaMeshOrRemoveItIfNotExists(base.Pointer, (entityMetaMesh != null) ? entityMetaMesh.Pointer : UIntPtr.Zero, (newMetaMesh != null) ? newMetaMesh.Pointer : UIntPtr.Zero);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x000057B9 File Offset: 0x000039B9
		public void SetUpdateValidtyOnFrameChangedOfFacesWithId(int faceGroupId, bool updateValidity)
		{
			EngineApplicationInterface.IGameEntity.SetUpdateValidityOnFrameChangedOfFacesWithId(base.Pointer, faceGroupId, updateValidity);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x000057CD File Offset: 0x000039CD
		public void AttachNavigationMeshFaces(int faceGroupId, bool isConnected, bool isBlocker = false, bool autoLocalize = false, bool finalizeBlockerConvexHullComputation = false, bool updateEntityFrame = true)
		{
			EngineApplicationInterface.IGameEntity.AttachNavigationMeshFaces(base.Pointer, faceGroupId, isConnected, isBlocker, autoLocalize, finalizeBlockerConvexHullComputation, updateEntityFrame);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x000057E8 File Offset: 0x000039E8
		public void DetachAllAttachedNavigationMeshFaces()
		{
			EngineApplicationInterface.IGameEntity.DetachAllAttachedNavigationMeshFaces(base.Pointer);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x000057FA File Offset: 0x000039FA
		public void UpdateAttachedNavigationMeshFaces()
		{
			EngineApplicationInterface.IGameEntity.UpdateAttachedNavigationMeshFaces(base.Pointer);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0000580C File Offset: 0x00003A0C
		public void RemoveSkeleton()
		{
			this.Skeleton = null;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x00005815 File Offset: 0x00003A15
		// (set) Token: 0x060007C9 RID: 1993 RVA: 0x00005827 File Offset: 0x00003A27
		public Skeleton Skeleton
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetSkeleton(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.IGameEntity.SetSkeleton(base.Pointer, (value != null) ? value.Pointer : UIntPtr.Zero);
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00005849 File Offset: 0x00003A49
		public void RemoveAllChildren()
		{
			EngineApplicationInterface.IGameEntity.RemoveAllChildren(base.Pointer);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0000585B File Offset: 0x00003A5B
		public IEnumerable<GameEntity> GetChildren()
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

		// Token: 0x060007CC RID: 1996 RVA: 0x0000586B File Offset: 0x00003A6B
		public IEnumerable<GameEntity> GetEntityAndChildren()
		{
			yield return this;
			int count = this.ChildCount;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return this.GetChild(i);
				num = i;
			}
			yield break;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0000587C File Offset: 0x00003A7C
		public void GetChildrenRecursive(ref List<GameEntity> children)
		{
			int childCount = this.ChildCount;
			for (int i = 0; i < childCount; i++)
			{
				GameEntity child = this.GetChild(i);
				children.Add(child);
				child.GetChildrenRecursive(ref children);
			}
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x000058B4 File Offset: 0x00003AB4
		public void GetChildrenWithTagRecursive(List<GameEntity> children, string tag)
		{
			int childCount = this.ChildCount;
			for (int i = 0; i < childCount; i++)
			{
				GameEntity child = this.GetChild(i);
				if (child.HasTag(tag))
				{
					children.Add(child);
				}
				child.GetChildrenWithTagRecursive(children, tag);
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x000058F4 File Offset: 0x00003AF4
		public bool IsSelectedOnEditor()
		{
			return EngineApplicationInterface.IGameEntity.IsEntitySelectedOnEditor(base.Pointer);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00005906 File Offset: 0x00003B06
		public void SelectEntityOnEditor()
		{
			EngineApplicationInterface.IGameEntity.SelectEntityOnEditor(base.Pointer);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00005918 File Offset: 0x00003B18
		public void DeselectEntityOnEditor()
		{
			EngineApplicationInterface.IGameEntity.DeselectEntityOnEditor(base.Pointer);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0000592A File Offset: 0x00003B2A
		public void SetAsPredisplayEntity()
		{
			EngineApplicationInterface.IGameEntity.SetAsPredisplayEntity(base.Pointer);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0000593C File Offset: 0x00003B3C
		public void RemoveFromPredisplayEntity()
		{
			EngineApplicationInterface.IGameEntity.RemoveFromPredisplayEntity(base.Pointer);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0000594E File Offset: 0x00003B4E
		public void SetNativeScriptComponentVariable(string className, string fieldName, ref ScriptComponentFieldHolder data, RglScriptFieldType variableType)
		{
			EngineApplicationInterface.IGameEntity.SetNativeScriptComponentVariable(base.Pointer, className, fieldName, ref data, variableType);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00005965 File Offset: 0x00003B65
		public void SetManualGlobalBoundingBox(Vec3 boundingBoxStartGlobal, Vec3 boundingBoxEndGlobal)
		{
			EngineApplicationInterface.IGameEntity.SetManualGlobalBoundingBox(base.Pointer, boundingBoxStartGlobal, boundingBoxEndGlobal);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00005979 File Offset: 0x00003B79
		public bool RayHitEntity(Vec3 rayOrigin, Vec3 rayDirection, float maxLength, ref float resultLength)
		{
			return EngineApplicationInterface.IGameEntity.RayHitEntity(base.Pointer, in rayOrigin, in rayDirection, maxLength, ref resultLength);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00005992 File Offset: 0x00003B92
		public bool RayHitEntityWithNormal(Vec3 rayOrigin, Vec3 rayDirection, float maxLength, ref Vec3 resultNormal, ref float resultLength)
		{
			return EngineApplicationInterface.IGameEntity.RayHitEntityWithNormal(base.Pointer, in rayOrigin, in rayDirection, maxLength, ref resultNormal, ref resultLength);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x000059AD File Offset: 0x00003BAD
		public void GetNativeScriptComponentVariable(string className, string fieldName, ref ScriptComponentFieldHolder data, RglScriptFieldType variableType)
		{
			EngineApplicationInterface.IGameEntity.GetNativeScriptComponentVariable(base.Pointer, className, fieldName, ref data, variableType);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x000059C4 File Offset: 0x00003BC4
		public void SetCustomClipPlane(Vec3 clipPosition, Vec3 clipNormal, bool setForChildren)
		{
			EngineApplicationInterface.IGameEntity.SetCustomClipPlane(base.Pointer, clipPosition, clipNormal, setForChildren);
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x000059D9 File Offset: 0x00003BD9
		public float GetBoundingBoxLongestHalfDimension()
		{
			return BoundingBox.GetLongestHalfDimensionOfBoundingBox(this.GetLocalBoundingBox());
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x000059E8 File Offset: 0x00003BE8
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

		// Token: 0x060007DC RID: 2012 RVA: 0x00005A4C File Offset: 0x00003C4C
		public BoundingBox ComputeBoundingBoxIncludeChildren()
		{
			BoundingBox boundingBox = default(BoundingBox);
			boundingBox.BeginRelaxation();
			foreach (GameEntity gameEntity in this.GetChildren())
			{
				gameEntity.ValidateBoundingBox();
				BoundingBox localBoundingBox = gameEntity.GetLocalBoundingBox();
				boundingBox.RelaxWithChildBoundingBox(localBoundingBox, gameEntity.GetLocalFrame());
			}
			boundingBox.RecomputeRadius();
			return boundingBox;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00005AC4 File Offset: 0x00003CC4
		public void SetManualLocalBoundingBox(in BoundingBox boundingBox)
		{
			EngineApplicationInterface.IGameEntity.SetManualLocalBoundingBox(base.Pointer, in boundingBox);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00005AD7 File Offset: 0x00003CD7
		public void RelaxLocalBoundingBox(in BoundingBox boundingBox)
		{
			EngineApplicationInterface.IGameEntity.RelaxLocalBoundingBox(base.Pointer, in boundingBox);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00005AEA File Offset: 0x00003CEA
		public void SetCullMode(MBMeshCullingMode cullMode)
		{
			EngineApplicationInterface.IGameEntity.SetCullMode(base.Pointer, cullMode);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00005B00 File Offset: 0x00003D00
		public GameEntity GetFirstChildEntityWithTagRecursive(string tag)
		{
			UIntPtr firstChildWithTagRecursive = EngineApplicationInterface.IGameEntity.GetFirstChildWithTagRecursive(base.Pointer, tag);
			if (firstChildWithTagRecursive != UIntPtr.Zero)
			{
				return new GameEntity(firstChildWithTagRecursive);
			}
			return null;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00005B34 File Offset: 0x00003D34
		public override bool Equals(object obj)
		{
			GameEntity gameEntity = (GameEntity)obj;
			return ((gameEntity != null) ? gameEntity.Pointer : UIntPtr.Zero) == ((this != null) ? base.Pointer : UIntPtr.Zero);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00005B64 File Offset: 0x00003D64
		public override int GetHashCode()
		{
			return base.Pointer.GetHashCode();
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00005B7F File Offset: 0x00003D7F
		public static bool operator ==(GameEntity gameEntity, WeakGameEntity weakGameEntity)
		{
			return weakGameEntity.Pointer == ((gameEntity != null) ? gameEntity.Pointer : UIntPtr.Zero);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00005B9D File Offset: 0x00003D9D
		public static bool operator !=(GameEntity gameEntity, WeakGameEntity weakGameEntity)
		{
			return weakGameEntity.Pointer != ((gameEntity != null) ? gameEntity.Pointer : UIntPtr.Zero);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00005BBB File Offset: 0x00003DBB
		public void SetDoNotCheckVisibility(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetDoNotCheckVisibility(base.Pointer, value);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00005BCE File Offset: 0x00003DCE
		public static bool operator !=(GameEntity gameEntity1, GameEntity gameEntity2)
		{
			return ((gameEntity1 != null) ? gameEntity1.Pointer : UIntPtr.Zero) != ((gameEntity2 != null) ? gameEntity2.Pointer : UIntPtr.Zero);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00005BF5 File Offset: 0x00003DF5
		public static bool operator ==(GameEntity gameEntity1, GameEntity gameEntity2)
		{
			return ((gameEntity1 != null) ? gameEntity1.Pointer : UIntPtr.Zero) == ((gameEntity2 != null) ? gameEntity2.Pointer : UIntPtr.Zero);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00005C1C File Offset: 0x00003E1C
		public void SetBoneFrameToAllMeshes(int boneIndex, in MatrixFrame frame)
		{
			EngineApplicationInterface.IGameEntity.SetBoneFrameToAllMeshes(base.Pointer, boneIndex, in frame);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00005C30 File Offset: 0x00003E30
		public Vec2 GetGlobalWindStrengthVectorOfScene()
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalWindStrengthVectorOfScene(base.Pointer);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00005C42 File Offset: 0x00003E42
		public Vec2 GetGlobalWindVelocityOfScene()
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalWindVelocityOfScene(base.Pointer);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00005C54 File Offset: 0x00003E54
		public Vec3 GetLastFinalRenderCameraPositionOfScene()
		{
			return EngineApplicationInterface.IGameEntity.GetLastFinalRenderCameraPositionOfScene(base.Pointer);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00005C66 File Offset: 0x00003E66
		public void SetForceDecalsToRender(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetForceDecalsToRender(base.Pointer, value);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00005C79 File Offset: 0x00003E79
		public void SetForceNotAffectedBySeason(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetForceNotAffectedBySeason(base.Pointer, value);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00005C8C File Offset: 0x00003E8C
		public bool CheckIsPrefabLinkRootPrefab(int depth)
		{
			return EngineApplicationInterface.IGameEntity.CheckIsPrefabLinkRootPrefab(base.Pointer, depth);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00005C9F File Offset: 0x00003E9F
		public void SetupAdditionalBoneBufferForMeshes(int boneCount)
		{
			EngineApplicationInterface.IGameEntity.SetupAdditionalBoneBufferForMeshes(base.Pointer, boneCount);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00005CB2 File Offset: 0x00003EB2
		public static UIntPtr CreatePhysxCookingInstance()
		{
			return EngineApplicationInterface.IGameEntity.CreatePhysxCookingInstance();
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00005CBE File Offset: 0x00003EBE
		public static void DeletePhysxCookingInstance(UIntPtr pointer)
		{
			EngineApplicationInterface.IGameEntity.DeletePhysxCookingInstance(pointer);
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00005CCB File Offset: 0x00003ECB
		public void DeleteEmptyShape(UIntPtr shape1, UIntPtr shape2)
		{
			EngineApplicationInterface.IGameEntity.DeleteEmptyShape(base.Pointer, shape1, shape2);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00005CDF File Offset: 0x00003EDF
		public UIntPtr CreateEmptyPhysxShape(bool isVariable, int physxMaterialIndex)
		{
			return EngineApplicationInterface.IGameEntity.CreateEmptyPhysxShape(base.Pointer, isVariable, physxMaterialIndex);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00005CF3 File Offset: 0x00003EF3
		public void SwapPhysxShapeInEntity(UIntPtr oldShape, UIntPtr newShape, bool isVariable)
		{
			EngineApplicationInterface.IGameEntity.SwapPhysxShapeInEntity(base.Pointer, oldShape, newShape, isVariable);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00005D08 File Offset: 0x00003F08
		public static void CookTrianglePhysxMesh(UIntPtr cookingInstancePointer, UIntPtr shapePointer, UIntPtr quadPinnedPointer, int physicsMaterial, int numberOfVertices, UIntPtr indicesPinnedPointer, int numberOfIndices)
		{
			EngineApplicationInterface.IGameEntity.CookTrianglePhysxMesh(cookingInstancePointer, shapePointer, quadPinnedPointer, physicsMaterial, numberOfVertices, indicesPinnedPointer, numberOfIndices);
		}

		// Token: 0x020000B5 RID: 181
		[EngineStruct("rglEntity_component_type", true, "rgl_ecomp", false)]
		public enum ComponentType : uint
		{
			// Token: 0x0400035F RID: 863
			MetaMesh,
			// Token: 0x04000360 RID: 864
			Light,
			// Token: 0x04000361 RID: 865
			CompositeComponent,
			// Token: 0x04000362 RID: 866
			ClothSimulator,
			// Token: 0x04000363 RID: 867
			ParticleSystemInstanced,
			// Token: 0x04000364 RID: 868
			TownIcon,
			// Token: 0x04000365 RID: 869
			CustomType1,
			// Token: 0x04000366 RID: 870
			Decal
		}

		// Token: 0x020000B6 RID: 182
		[EngineStruct("rglEntity::Mobility", false, null)]
		public enum Mobility : sbyte
		{
			// Token: 0x04000368 RID: 872
			Stationary,
			// Token: 0x04000369 RID: 873
			Dynamic,
			// Token: 0x0400036A RID: 874
			DynamicForced
		}

		// Token: 0x020000B7 RID: 183
		[Flags]
		public enum UpgradeLevelMask
		{
			// Token: 0x0400036C RID: 876
			None = 0,
			// Token: 0x0400036D RID: 877
			Level0 = 1,
			// Token: 0x0400036E RID: 878
			Level1 = 2,
			// Token: 0x0400036F RID: 879
			Level2 = 4,
			// Token: 0x04000370 RID: 880
			Level3 = 8,
			// Token: 0x04000371 RID: 881
			LevelAll = 15
		}
	}
}
