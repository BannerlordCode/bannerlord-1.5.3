using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;

namespace SandBox
{
	// Token: 0x0200001B RID: 27
	public class MapScene : IMapScene
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00004179 File Offset: 0x00002379
		public Scene Scene
		{
			get
			{
				return this._scene;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004181 File Offset: 0x00002381
		public MapScene()
		{
			this._sharedLock = new ReaderWriterLockSlim();
			this._sceneLevels = new Dictionary<string, uint>();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000419F File Offset: 0x0000239F
		public Vec2 GetTerrainSize()
		{
			return this._terrainSize;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000041A8 File Offset: 0x000023A8
		public uint GetSceneLevel(string name)
		{
			this._sharedLock.EnterReadLock();
			uint num;
			bool flag = this._sceneLevels.TryGetValue(name, out num) && num != 2147483647U;
			this._sharedLock.ExitReadLock();
			if (flag)
			{
				return num;
			}
			uint upgradeLevelMaskOfLevelName = this._scene.GetUpgradeLevelMaskOfLevelName(name);
			this._sharedLock.EnterWriteLock();
			this._sceneLevels[name] = upgradeLevelMaskOfLevelName;
			this._sharedLock.ExitWriteLock();
			return upgradeLevelMaskOfLevelName;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004220 File Offset: 0x00002420
		public void SetSceneLevels(List<string> levels)
		{
			foreach (string text in levels)
			{
				this._sceneLevels.Add(text, 2147483647U);
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004278 File Offset: 0x00002478
		public List<AtmosphereState> GetAtmosphereStates()
		{
			List<AtmosphereState> list = new List<AtmosphereState>();
			foreach (GameEntity gameEntity in this.Scene.FindEntitiesWithTag("atmosphere_probe"))
			{
				MapAtmosphereProbe firstScriptOfType = gameEntity.GetFirstScriptOfType<MapAtmosphereProbe>();
				Vec3 globalPosition = gameEntity.GlobalPosition;
				AtmosphereState atmosphereState = new AtmosphereState
				{
					Position = globalPosition,
					HumidityAverage = firstScriptOfType.rainDensity,
					HumidityVariance = 5f,
					TemperatureAverage = firstScriptOfType.temperature,
					TemperatureVariance = 5f,
					distanceForMaxWeight = firstScriptOfType.minRadius,
					distanceForMinWeight = firstScriptOfType.maxRadius,
					ColorGradeTexture = firstScriptOfType.colorGrade
				};
				list.Add(atmosphereState);
			}
			return list;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004348 File Offset: 0x00002548
		public void ValidateAgentVisualsReseted()
		{
			if (this._scene != null && this._agentRendererSceneController != null)
			{
				MBAgentRendererSceneController.ValidateAgentVisualsReseted(this._scene, this._agentRendererSceneController);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004371 File Offset: 0x00002571
		public void SetAtmosphereColorgrade(TerrainType terrainType)
		{
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004374 File Offset: 0x00002574
		public void AddNewEntityToMapScene(string entityId, in CampaignVec2 position)
		{
			GameEntity gameEntity = GameEntity.Instantiate(this._scene, entityId, true, true, "");
			if (gameEntity != null)
			{
				GameEntity gameEntity2 = gameEntity;
				CampaignVec2 campaignVec = position;
				gameEntity2.SetLocalPosition(campaignVec.AsVec3());
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000043B2 File Offset: 0x000025B2
		public void GetMapBorders(out Vec2 minimumPosition, out Vec2 maximumPosition, out float maximumHeight)
		{
			minimumPosition = this._minimumPositionCache;
			maximumPosition = this._maximumPositionCache;
			maximumHeight = this._maximumHeightCache;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000043D4 File Offset: 0x000025D4
		public void Load()
		{
			Debug.Print("Creating map scene", 0, Debug.DebugColor.White, 17592186044416UL);
			this._scene = Scene.CreateNewScene(false, true, DecalAtlasGroup.Worldmap, "MapScene");
			this._scene.SetClothSimulationState(true);
			this._agentRendererSceneController = MBAgentRendererSceneController.CreateNewAgentRendererSceneController(this._scene);
			this._agentRendererSceneController.SetDoTimerBasedForcedSkeletonUpdates(false);
			this._scene.SetOcclusionMode(true);
			SceneInitializationData sceneInitializationData = new SceneInitializationData(true);
			sceneInitializationData.UsePhysicsMaterials = false;
			sceneInitializationData.EnableFloraPhysics = false;
			sceneInitializationData.UseTerrainMeshBlending = false;
			sceneInitializationData.CreateOros = false;
			this._scene.SetFetchCrcInfoOfScene(true);
			this.DisableUnwalkableNavigationMeshes();
			bool[] regionMapping = SandBoxHelpers.MapSceneHelper.GetRegionMapping(Campaign.Current.Models.PartyNavigationModel);
			this._scene.SetNavMeshRegionMap(regionMapping);
			ModuleInfo mainMapModule = this.GetMainMapModule();
			if (mainMapModule != null)
			{
				this._scene.Read("Main_map", mainMapModule.Id, ref sceneInitializationData, "");
			}
			else
			{
				this._scene.Read("Main_map", ref sceneInitializationData, "");
			}
			Utilities.SetAllocationAlwaysValidScene(this._scene);
			GameEntity firstEntityWithName = this._scene.GetFirstEntityWithName("border_min");
			GameEntity firstEntityWithName2 = this._scene.GetFirstEntityWithName("border_max");
			MatrixFrame matrixFrame = firstEntityWithName.GetGlobalFrame();
			this._minimumPositionCache = matrixFrame.origin.AsVec2;
			matrixFrame = firstEntityWithName2.GetGlobalFrame();
			this._maximumPositionCache = matrixFrame.origin.AsVec2;
			this._maximumHeightCache = firstEntityWithName2.GetGlobalFrame().origin.z;
			this._scene.DisableStaticShadows(true);
			this._scene.InvalidateTerrainPhysicsMaterials();
			this._scene.SetDontLoadInvisibleEntities(true);
			GameEntity firstEntityWithName3 = this._scene.GetFirstEntityWithName("medit_water_plane");
			if (firstEntityWithName3 != null)
			{
				firstEntityWithName3.SetDoNotCheckVisibility(true);
			}
			if (this._scene.GetFirstEntityWithScriptComponent("Town Scene Manager") == null)
			{
				MBDebug.ShowWarning("Main map scene must have an entity with 'Town Scene Manager' script for mesh memory optimization.");
			}
			this.LoadAtmosphereData(this._scene);
			Campaign.Current.Models.MapWeatherModel.InitializeCaches();
			MBMapScene.ValidateTerrainSoundIds();
			this._scene.OptimizeScene(true, false);
			Vec2i vec2i;
			float num;
			int num2;
			int num3;
			this._scene.GetTerrainData(out vec2i, out num, out num2, out num3);
			this._terrainSize.x = (float)vec2i.X * num;
			this._terrainSize.y = (float)vec2i.Y * num;
			MBMapScene.GetBattleSceneIndexMap(this._scene, ref this._battleTerrainIndexMap, ref this._battleTerrainIndexMapWidth, ref this._battleTerrainIndexMapHeight);
			Debug.Print("Ticking map scene for first initialization", 0, Debug.DebugColor.White, 17592186044416UL);
			this._scene.Tick(0.1f);
			AsyncTask asyncTask = AsyncTask.CreateWithDelegate(new ManagedDelegate
			{
				Instance = new ManagedDelegate.DelegateDefinition(Campaign.LateAITick)
			}, false);
			Campaign.Current.CampaignLateAITickTask = asyncTask;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000469C File Offset: 0x0000289C
		public void SetSnowAndRainDataWithDimension(Texture snowRainTexture, int weatherNodeGridWidthAndHeight)
		{
			this._scene.CreateDynamicRainTexture(weatherNodeGridWidthAndHeight, weatherNodeGridWidthAndHeight);
			this._snowAndRainDataTextureWidth = snowRainTexture.Width;
			this._snowAndRainDataTextureHeight = snowRainTexture.Height;
			this._snowAndRainData = new byte[this._snowAndRainDataTextureWidth * this._snowAndRainDataTextureHeight * 2];
			snowRainTexture.GetPixelData(this._snowAndRainData);
			this._scene.SetDynamicSnowTexture(snowRainTexture);
			Campaign.Current.DefaultWeatherNodeDimension = weatherNodeGridWidthAndHeight;
			this._windFlowMapData = new float[524288];
			this._scene.GetWindFlowMapData(this._windFlowMapData);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000472C File Offset: 0x0000292C
		public void AfterLoad()
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004730 File Offset: 0x00002930
		private ModuleInfo GetMainMapModule()
		{
			ModuleInfo moduleInfo = null;
			foreach (ModuleInfo moduleInfo2 in ModuleHelper.GetActiveModules())
			{
				if (moduleInfo2.IsActive && File.Exists(Path.Combine(moduleInfo2.FolderPath, "SceneObj", "Main_map", "scene.xscene")))
				{
					moduleInfo = moduleInfo2;
				}
			}
			return moduleInfo;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000047AC File Offset: 0x000029AC
		public void Destroy()
		{
			MBAgentRendererSceneController.DestructAgentRendererSceneController(this._scene, this._agentRendererSceneController, false);
			this._agentRendererSceneController = null;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000047C8 File Offset: 0x000029C8
		public void DisableUnwalkableNavigationMeshes()
		{
			foreach (int num in Campaign.Current.Models.PartyNavigationModel.GetInvalidTerrainTypesForNavigationType(MobileParty.NavigationType.All))
			{
				this.Scene.SetAbilityOfFacesWithId(num, false);
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000480C File Offset: 0x00002A0C
		public PathFaceRecord GetFaceIndex(in CampaignVec2 vec2)
		{
			PathFaceRecord pathFaceRecord = new PathFaceRecord(-1, -1, -1);
			Scene scene = this._scene;
			CampaignVec2 campaignVec = vec2;
			scene.GetNavMeshFaceIndex(ref pathFaceRecord, campaignVec.ToVec2(), vec2.IsOnLand, false, true);
			return pathFaceRecord;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004847 File Offset: 0x00002A47
		private void LoadAtmosphereData(Scene mapScene)
		{
			MBMapScene.LoadAtmosphereData(mapScene);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004850 File Offset: 0x00002A50
		public TerrainType GetTerrainTypeAtPosition(in CampaignVec2 position)
		{
			CampaignVec2 campaignVec = position;
			PathFaceRecord face = campaignVec.Face;
			return this.GetFaceTerrainType(face);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004873 File Offset: 0x00002A73
		public TerrainType GetFaceTerrainType(PathFaceRecord navMeshFace)
		{
			if (!navMeshFace.IsValid())
			{
				Debug.FailedAssert("Null nav mesh face tried to get terrain type.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\MapScene.cs", "GetFaceTerrainType", 338);
				return TerrainType.Plain;
			}
			return (TerrainType)navMeshFace.FaceGroupIndex;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000048A0 File Offset: 0x00002AA0
		public CampaignVec2 GetNearestFaceCenterForPosition(in CampaignVec2 position, int[] excludedFaceIds)
		{
			Scene scene = this._scene;
			CampaignVec2 campaignVec = position;
			return new CampaignVec2(MBMapScene.GetNearestFaceCenterForPosition(scene, campaignVec.ToVec2(), position.IsOnLand, excludedFaceIds), position.IsOnLand);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000048D8 File Offset: 0x00002AD8
		public CampaignVec2 GetNearestFaceCenterForPositionWithPath(PathFaceRecord pathFaceRecord, bool targetIsLand, float maxDist, int[] excludedFaceIds)
		{
			return new CampaignVec2(MBMapScene.GetNearestFaceCenterForPositionWithPath(this._scene, pathFaceRecord, targetIsLand, maxDist, excludedFaceIds), targetIsLand);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000048F0 File Offset: 0x00002AF0
		public List<TerrainType> GetEnvironmentTerrainTypes(in CampaignVec2 originPosition)
		{
			List<TerrainType> list = new List<TerrainType>();
			Vec2 vec = new Vec2(1f, 0f);
			CampaignVec2 campaignVec = originPosition;
			list.Add(this.GetTerrainTypeAtPosition(in campaignVec));
			for (int i = 0; i < 8; i++)
			{
				vec.RotateCCW(0.7853982f * (float)i);
				for (int j = 1; j < 7; j++)
				{
					campaignVec += (float)j * vec;
					TerrainType terrainTypeAtPosition = this.GetTerrainTypeAtPosition(in campaignVec);
					if (!list.Contains(terrainTypeAtPosition))
					{
						list.Add(terrainTypeAtPosition);
					}
				}
			}
			return list;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004984 File Offset: 0x00002B84
		public List<TerrainType> GetEnvironmentTerrainTypesCount(in CampaignVec2 originPosition, out TerrainType currentPositionTerrainType)
		{
			List<TerrainType> list = new List<TerrainType>();
			Vec2 vec = new Vec2(1f, 0f);
			CampaignVec2 campaignVec = originPosition;
			currentPositionTerrainType = this.GetTerrainTypeAtPosition(in campaignVec);
			list.Add(currentPositionTerrainType);
			for (int i = 0; i < 8; i++)
			{
				vec.RotateCCW(0.7853982f * (float)i);
				for (int j = 1; j < 7; j++)
				{
					campaignVec += (float)j * vec;
					if (campaignVec.Face.IsValid())
					{
						TerrainType faceTerrainType = this.GetFaceTerrainType(campaignVec.Face);
						list.Add(faceTerrainType);
					}
				}
			}
			return list;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004A28 File Offset: 0x00002C28
		public MapPatchData GetMapPatchAtPosition(in CampaignVec2 position)
		{
			if (this._battleTerrainIndexMap != null)
			{
				CampaignVec2 campaignVec = position;
				int num = MathF.Floor(campaignVec.X / this._terrainSize.X * (float)this._battleTerrainIndexMapWidth);
				campaignVec = position;
				int num2 = MathF.Floor(campaignVec.Y / this._terrainSize.Y * (float)this._battleTerrainIndexMapHeight);
				num = MBMath.ClampIndex(num, 0, this._battleTerrainIndexMapWidth);
				int num3 = (MBMath.ClampIndex(num2, 0, this._battleTerrainIndexMapHeight) * this._battleTerrainIndexMapWidth + num) * 2;
				byte b = this._battleTerrainIndexMap[num3];
				byte b2 = this._battleTerrainIndexMap[num3 + 1];
				Vec2 vec = new Vec2((float)(b2 & 15) / 15f, (float)((b2 >> 4) & 15) / 15f);
				return new MapPatchData
				{
					sceneIndex = (int)b,
					normalizedCoordinates = vec
				};
			}
			return default(MapPatchData);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004B10 File Offset: 0x00002D10
		public CampaignVec2 GetAccessiblePointNearPosition(in CampaignVec2 pos, float radius)
		{
			Scene scene = this._scene;
			CampaignVec2 campaignVec = pos;
			return new CampaignVec2(MBMapScene.GetAccessiblePointNearPosition(scene, campaignVec.ToVec2(), pos.IsOnLand, radius), pos.IsOnLand);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004B48 File Offset: 0x00002D48
		public bool GetPathBetweenAIFaces(PathFaceRecord startingFace, PathFaceRecord endingFace, Vec2 startingPosition, Vec2 endingPosition, float agentRadius, NavigationPath path, int[] excludedFaceIds, float extraCostMultiplier, int regionSwitchCostFromLandToSea, int regionSwitchCostFromSeaToLand)
		{
			if (regionSwitchCostFromLandToSea == 0 && regionSwitchCostFromSeaToLand == 0)
			{
				return this._scene.GetPathBetweenAIFaces(startingFace.FaceIndex, endingFace.FaceIndex, startingPosition, endingPosition, agentRadius, path, excludedFaceIds, extraCostMultiplier);
			}
			return this._scene.GetPathBetweenAIFaces(startingFace.FaceIndex, endingFace.FaceIndex, startingPosition, endingPosition, agentRadius, path, excludedFaceIds, extraCostMultiplier, regionSwitchCostFromLandToSea, regionSwitchCostFromSeaToLand);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004BA8 File Offset: 0x00002DA8
		public bool GetPathBetweenAIFaces(PathFaceRecord startingFace, PathFaceRecord endingFace, Vec2 startingPosition, Vec2 endingPosition, float agentRadius, NavigationPath path, int[] excludedFaceIds, float extraCostMultiplier, int regionSwitchCostFromLandToSea, int regionSwitchCostFromSeaToLand, int excludedFaceIndex)
		{
			if (regionSwitchCostFromLandToSea == 0 && regionSwitchCostFromSeaToLand == 0)
			{
				return this._scene.GetPathBetweenAIFaces(startingFace.FaceIndex, endingFace.FaceIndex, startingPosition, endingPosition, agentRadius, path, excludedFaceIds, extraCostMultiplier, excludedFaceIndex);
			}
			return this._scene.GetPathBetweenAIFaces(startingFace.FaceIndex, endingFace.FaceIndex, startingPosition, endingPosition, agentRadius, path, excludedFaceIds, extraCostMultiplier, regionSwitchCostFromLandToSea, regionSwitchCostFromSeaToLand, excludedFaceIndex);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004C0C File Offset: 0x00002E0C
		public bool GetPathDistanceBetweenAIFaces(PathFaceRecord startingAiFace, PathFaceRecord endingAiFace, Vec2 startingPosition, Vec2 endingPosition, float agentRadius, float distanceLimit, out float distance, int[] excludedFaceIds, int regionSwitchCostFromLandToSea, int regionSwitchCostFromSeaToLand)
		{
			return this._scene.GetPathDistanceBetweenAIFaces(startingAiFace.FaceIndex, endingAiFace.FaceIndex, startingPosition, endingPosition, agentRadius, distanceLimit, out distance, excludedFaceIds, regionSwitchCostFromLandToSea, regionSwitchCostFromSeaToLand);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004C3F File Offset: 0x00002E3F
		public bool IsLineToPointClear(PathFaceRecord startingFace, Vec2 position, Vec2 destination, float agentRadius)
		{
			return this._scene.IsLineToPointClear(startingFace.FaceIndex, position, destination, agentRadius);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004C56 File Offset: 0x00002E56
		public Vec2 GetLastPointOnNavigationMeshFromPositionToDestination(PathFaceRecord startingFace, Vec2 position, Vec2 destination, int[] excludedFaceIds = null)
		{
			return this._scene.GetLastPointOnNavigationMeshFromPositionToDestination(startingFace.FaceIndex, position, destination, excludedFaceIds);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004C6D File Offset: 0x00002E6D
		public Vec2 GetLastPositionOnNavMeshFaceForPointAndDirection(PathFaceRecord startingFace, Vec2 position, Vec2 destination)
		{
			return this._scene.GetLastPositionOnNavMeshFaceForPointAndDirection(startingFace, position, destination);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004C80 File Offset: 0x00002E80
		public Vec2 GetNavigationMeshCenterPosition(PathFaceRecord face)
		{
			Vec3 zero = Vec3.Zero;
			this._scene.GetNavMeshCenterPosition(face.FaceIndex, ref zero);
			return zero.AsVec2;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004CB0 File Offset: 0x00002EB0
		public Vec2 GetNavigationMeshCenterPosition(int faceIndex)
		{
			Vec3 zero = Vec3.Zero;
			this._scene.GetNavMeshCenterPosition(faceIndex, ref zero);
			return zero.AsVec2;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004CD8 File Offset: 0x00002ED8
		public int GetNumberOfNavigationMeshFaces()
		{
			return this._scene.GetNavMeshFaceCount();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004CE5 File Offset: 0x00002EE5
		public PathFaceRecord GetFaceAtIndex(int faceIndex)
		{
			return this._scene.GetNavMeshPathFaceRecord(faceIndex);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004CF4 File Offset: 0x00002EF4
		public bool GetHeightAtPoint(in CampaignVec2 point, ref float height)
		{
			BodyFlags bodyFlags = BodyFlags.Moveable;
			bodyFlags |= (point.IsOnLand ? BodyFlags.OnlyCollideWithRaycast : BodyFlags.CommonCollisionExcludeFlags);
			Scene scene = this._scene;
			CampaignVec2 campaignVec = point;
			return scene.GetHeightAtPoint(campaignVec.ToVec2(), bodyFlags, ref height);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004D36 File Offset: 0x00002F36
		public float GetWinterTimeFactor()
		{
			return this._scene.GetWinterTimeFactor();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004D43 File Offset: 0x00002F43
		public float GetFaceVertexZ(PathFaceRecord navMeshFace)
		{
			return this._scene.GetNavMeshFaceFirstVertexZ(navMeshFace.FaceIndex);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004D56 File Offset: 0x00002F56
		public Vec3 GetGroundNormal(Vec2 position)
		{
			return this._scene.GetNormalAt(position);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004D64 File Offset: 0x00002F64
		public void GetSiegeCampFrames(Settlement settlement, out List<MatrixFrame> siegeCamp1GlobalFrames, out List<MatrixFrame> siegeCamp2GlobalFrames)
		{
			siegeCamp1GlobalFrames = new List<MatrixFrame>();
			siegeCamp2GlobalFrames = new List<MatrixFrame>();
			GameEntity campaignEntityWithName = this._scene.GetCampaignEntityWithName(settlement.Party.Id);
			if (campaignEntityWithName != null && settlement.IsFortification)
			{
				List<GameEntity> list = new List<GameEntity>();
				campaignEntityWithName.GetChildrenRecursive(ref list);
				foreach (GameEntity gameEntity in list)
				{
					if (gameEntity.HasTag("map_camp_area_1"))
					{
						siegeCamp1GlobalFrames.Add(gameEntity.GetGlobalFrame());
					}
					else if (gameEntity.HasTag("map_camp_area_2"))
					{
						siegeCamp2GlobalFrames.Add(gameEntity.GetGlobalFrame());
					}
				}
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004E28 File Offset: 0x00003028
		public void GetTerrainHeightAndNormal(Vec2 position, out float height, out Vec3 normal)
		{
			this._scene.GetTerrainHeightAndNormal(position, out height, out normal);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004E38 File Offset: 0x00003038
		public string GetTerrainTypeName(TerrainType type)
		{
			string text = "Invalid";
			switch (type)
			{
			case TerrainType.Plain:
				text = "Plain";
				break;
			case TerrainType.Desert:
				text = "Desert";
				break;
			case TerrainType.Snow:
				text = "Snow";
				break;
			case TerrainType.Forest:
				text = "Forest";
				break;
			case TerrainType.Steppe:
				text = "Steppe";
				break;
			case TerrainType.Fording:
				text = "Fording";
				break;
			case TerrainType.Mountain:
				text = "Mountain";
				break;
			case TerrainType.Lake:
				text = "Lake";
				break;
			case TerrainType.Water:
				text = "Water";
				break;
			case TerrainType.River:
				text = "River";
				break;
			case TerrainType.Canyon:
				text = "Canyon";
				break;
			case TerrainType.Swamp:
				text = "Swamp";
				break;
			case TerrainType.Dune:
				text = "Dune";
				break;
			case TerrainType.Bridge:
				text = "Bridge";
				break;
			}
			return text;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004F08 File Offset: 0x00003108
		public uint GetSceneXmlCrc()
		{
			return this._scene.GetSceneXMLCRC();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004F15 File Offset: 0x00003115
		public uint GetSceneNavigationMeshCrc()
		{
			return this._scene.GetNavigationMeshCRC();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004F24 File Offset: 0x00003124
		public Vec2 GetWindAtPosition(Vec2 position)
		{
			int textureDataIndexForPosition = this.GetTextureDataIndexForPosition(position, 512, 512);
			return new Vec2(this._windFlowMapData[textureDataIndexForPosition * 2], this._windFlowMapData[textureDataIndexForPosition * 2 + 1]);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004F60 File Offset: 0x00003160
		public float GetSnowAmountAtPosition(Vec2 position)
		{
			int textureDataIndexForPosition = this.GetTextureDataIndexForPosition(position, this._snowAndRainDataTextureWidth, this._snowAndRainDataTextureHeight);
			return (float)this._snowAndRainData[textureDataIndexForPosition * 2];
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004F8C File Offset: 0x0000318C
		public float GetRainAmountAtPosition(Vec2 position)
		{
			int textureDataIndexForPosition = this.GetTextureDataIndexForPosition(position, this._snowAndRainDataTextureWidth, this._snowAndRainDataTextureHeight);
			return (float)this._snowAndRainData[textureDataIndexForPosition * 2 + 1];
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004FBC File Offset: 0x000031BC
		private int GetTextureDataIndexForPosition(Vec2 position, int dimensionX, int dimensionY)
		{
			Vec2 terrainSize = this.GetTerrainSize();
			int num = MathF.Floor(position.x / terrainSize.X * (float)dimensionX);
			int num2 = MathF.Floor(position.y / terrainSize.Y * (float)dimensionY);
			num = MBMath.ClampIndex(num, 0, dimensionX);
			return MBMath.ClampIndex(num2, 0, dimensionY) * dimensionX + num;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005011 File Offset: 0x00003211
		public void SetupWaterWake(float wakeWorldSize, float wakeCameraOffset)
		{
			this._scene.EnsureWaterWakeRenderer();
			this._scene.SetWaterWakeWorldSize(wakeWorldSize, 0.994f);
			this._scene.SetWaterWakeCameraOffset(wakeCameraOffset);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000503B File Offset: 0x0000323B
		PathFaceRecord IMapScene.GetFaceIndex(in CampaignVec2 vec2)
		{
			return this.GetFaceIndex(in vec2);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005044 File Offset: 0x00003244
		TerrainType IMapScene.GetTerrainTypeAtPosition(in CampaignVec2 vec2)
		{
			return this.GetTerrainTypeAtPosition(in vec2);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000504D File Offset: 0x0000324D
		List<TerrainType> IMapScene.GetEnvironmentTerrainTypes(in CampaignVec2 vec2)
		{
			return this.GetEnvironmentTerrainTypes(in vec2);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005056 File Offset: 0x00003256
		List<TerrainType> IMapScene.GetEnvironmentTerrainTypesCount(in CampaignVec2 vec2, out TerrainType currentPositionTerrainType)
		{
			return this.GetEnvironmentTerrainTypesCount(in vec2, out currentPositionTerrainType);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005060 File Offset: 0x00003260
		MapPatchData IMapScene.GetMapPatchAtPosition(in CampaignVec2 position)
		{
			return this.GetMapPatchAtPosition(in position);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00005069 File Offset: 0x00003269
		CampaignVec2 IMapScene.GetNearestFaceCenterForPosition(in CampaignVec2 vec2, int[] excludedFaceIds)
		{
			return this.GetNearestFaceCenterForPosition(in vec2, excludedFaceIds);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005073 File Offset: 0x00003273
		CampaignVec2 IMapScene.GetAccessiblePointNearPosition(in CampaignVec2 vec2, float radius)
		{
			return this.GetAccessiblePointNearPosition(in vec2, radius);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000507D File Offset: 0x0000327D
		bool IMapScene.GetHeightAtPoint(in CampaignVec2 point, ref float height)
		{
			return this.GetHeightAtPoint(in point, ref height);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005087 File Offset: 0x00003287
		void IMapScene.AddNewEntityToMapScene(string entityId, in CampaignVec2 position)
		{
			this.AddNewEntityToMapScene(entityId, in position);
		}

		// Token: 0x04000028 RID: 40
		private int _snowAndRainDataTextureWidth;

		// Token: 0x04000029 RID: 41
		private int _snowAndRainDataTextureHeight;

		// Token: 0x0400002A RID: 42
		public const int FlowMapTextureDimension = 512;

		// Token: 0x0400002B RID: 43
		private const string MapCampArea1Tag = "map_camp_area_1";

		// Token: 0x0400002C RID: 44
		private const string MapCampArea2Tag = "map_camp_area_2";

		// Token: 0x0400002D RID: 45
		private Scene _scene;

		// Token: 0x0400002E RID: 46
		private MBAgentRendererSceneController _agentRendererSceneController;

		// Token: 0x0400002F RID: 47
		private byte[] _snowAndRainData;

		// Token: 0x04000030 RID: 48
		private float[] _windFlowMapData;

		// Token: 0x04000031 RID: 49
		private Vec2 _minimumPositionCache;

		// Token: 0x04000032 RID: 50
		private Vec2 _maximumPositionCache;

		// Token: 0x04000033 RID: 51
		private float _maximumHeightCache;

		// Token: 0x04000034 RID: 52
		private Dictionary<string, uint> _sceneLevels;

		// Token: 0x04000035 RID: 53
		private int _battleTerrainIndexMapWidth;

		// Token: 0x04000036 RID: 54
		private int _battleTerrainIndexMapHeight;

		// Token: 0x04000037 RID: 55
		private byte[] _battleTerrainIndexMap;

		// Token: 0x04000038 RID: 56
		private Vec2 _terrainSize;

		// Token: 0x04000039 RID: 57
		private ReaderWriterLockSlim _sharedLock;
	}
}
