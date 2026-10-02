using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200009B RID: 155
	public static class Utilities
	{
		// Token: 0x06000D55 RID: 3413 RVA: 0x0000F1FC File Offset: 0x0000D3FC
		public static void ConstructMainThreadJob(Delegate function, params object[] parameters)
		{
			Utilities.MainThreadJob mainThreadJob = new Utilities.MainThreadJob(function, parameters);
			Utilities.jobs.Enqueue(mainThreadJob);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0000F21C File Offset: 0x0000D41C
		public static void ConstructMainThreadJob(Semaphore semaphore, Delegate function, params object[] parameters)
		{
			Utilities.MainThreadJob mainThreadJob = new Utilities.MainThreadJob(semaphore, function, parameters);
			Utilities.jobs.Enqueue(mainThreadJob);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0000F240 File Offset: 0x0000D440
		public static void RunJobs()
		{
			Utilities.MainThreadJob mainThreadJob;
			while (Utilities.jobs.TryDequeue(out mainThreadJob))
			{
				mainThreadJob.Invoke();
			}
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0000F263 File Offset: 0x0000D463
		public static void WaitJobs()
		{
			while (!Utilities.jobs.IsEmpty)
			{
			}
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0000F271 File Offset: 0x0000D471
		public static void OutputBenchmarkValuesToPerformanceReporter()
		{
			EngineApplicationInterface.IUtil.OutputBenchmarkValuesToPerformanceReporter();
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0000F27D File Offset: 0x0000D47D
		public static void SetLoadingScreenPercentage(float value)
		{
			EngineApplicationInterface.IUtil.SetLoadingScreenPercentage(value);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0000F28A File Offset: 0x0000D48A
		public static void SetFixedDt(bool enabled, float dt)
		{
			EngineApplicationInterface.IUtil.SetFixedDt(enabled, dt);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0000F298 File Offset: 0x0000D498
		public static void SetBenchmarkStatus(int status, string def)
		{
			EngineApplicationInterface.IUtil.SetBenchmarkStatus(status, def);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0000F2A6 File Offset: 0x0000D4A6
		public static int GetBenchmarkStatus()
		{
			return EngineApplicationInterface.IUtil.GetBenchmarkStatus();
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0000F2B2 File Offset: 0x0000D4B2
		public static string GetApplicationMemoryStatistics()
		{
			return EngineApplicationInterface.IUtil.GetApplicationMemoryStatistics();
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0000F2BE File Offset: 0x0000D4BE
		public static bool IsBenchmarkQuited()
		{
			return EngineApplicationInterface.IUtil.IsBenchmarkQuited();
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0000F2CA File Offset: 0x0000D4CA
		public static string GetNativeMemoryStatistics()
		{
			return EngineApplicationInterface.IUtil.GetNativeMemoryStatistics();
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0000F2D6 File Offset: 0x0000D4D6
		public static bool CommandLineArgumentExists(string str)
		{
			return EngineApplicationInterface.IUtil.CommandLineArgumentExists(str);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0000F2E3 File Offset: 0x0000D4E3
		public static string GetConsoleHostMachine()
		{
			return EngineApplicationInterface.IUtil.GetConsoleHostMachine();
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0000F2EF File Offset: 0x0000D4EF
		public static string ExportNavMeshFaceMarks(string file_name)
		{
			return EngineApplicationInterface.IUtil.ExportNavMeshFaceMarks(file_name);
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0000F2FC File Offset: 0x0000D4FC
		public static string TakeSSFromTop(string file_name)
		{
			return EngineApplicationInterface.IUtil.TakeSSFromTop(file_name);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0000F309 File Offset: 0x0000D509
		public static void CheckIfAssetsAndSourcesAreSame()
		{
			EngineApplicationInterface.IUtil.CheckIfAssetsAndSourcesAreSame();
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0000F315 File Offset: 0x0000D515
		public static void DisableCoreGame()
		{
			EngineApplicationInterface.IUtil.DisableCoreGame();
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0000F321 File Offset: 0x0000D521
		public static float GetApplicationMemory()
		{
			return EngineApplicationInterface.IUtil.GetApplicationMemory();
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0000F32D File Offset: 0x0000D52D
		public static void GatherCoreGameReferences(string scene_names)
		{
			EngineApplicationInterface.IUtil.GatherCoreGameReferences(scene_names);
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0000F33A File Offset: 0x0000D53A
		public static bool IsOnlyCoreContentEnabled()
		{
			return EngineApplicationInterface.IUtil.GetCoreGameState() != 0;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0000F349 File Offset: 0x0000D549
		public static void FindMeshesWithoutLods(string module_name)
		{
			EngineApplicationInterface.IUtil.FindMeshesWithoutLods(module_name);
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0000F356 File Offset: 0x0000D556
		public static void SetDisableDumpGeneration(bool value)
		{
			EngineApplicationInterface.IUtil.SetDisableDumpGeneration(value);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0000F363 File Offset: 0x0000D563
		public static void SetPrintCallstackAtCrahses(bool value)
		{
			EngineApplicationInterface.IUtil.SetPrintCallstackAtCrahses(value);
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0000F370 File Offset: 0x0000D570
		public static string[] GetModulesNames()
		{
			return EngineApplicationInterface.IUtil.GetModulesCode().Split(new char[] { '*' });
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0000F38C File Offset: 0x0000D58C
		public static string GetFullFilePathOfScene(string sceneName)
		{
			string fullFilePathOfScene = EngineApplicationInterface.IUtil.GetFullFilePathOfScene(sceneName);
			if (fullFilePathOfScene == "SCENE_NOT_FOUND")
			{
				throw new Exception("Scene '" + sceneName + "' was not found!");
			}
			return fullFilePathOfScene.Replace("$BASE/", Utilities.GetBasePath());
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0000F3CC File Offset: 0x0000D5CC
		public static bool TryGetFullFilePathOfScene(string sceneName, out string fullPath)
		{
			bool flag;
			try
			{
				fullPath = Utilities.GetFullFilePathOfScene(sceneName);
				flag = true;
			}
			catch (Exception)
			{
				fullPath = null;
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0000F400 File Offset: 0x0000D600
		public static bool TryGetUniqueIdentifiersForScene(string sceneName, out UniqueSceneId identifiers)
		{
			identifiers = null;
			string text;
			return Utilities.TryGetFullFilePathOfScene(sceneName, out text) && Utilities.TryGetUniqueIdentifiersForSceneFile(text, out identifiers);
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0000F424 File Offset: 0x0000D624
		public static bool TryGetUniqueIdentifiersForSceneFile(string xsceneFilePath, out UniqueSceneId identifiers)
		{
			identifiers = null;
			using (XmlReader xmlReader = XmlReader.Create(new StreamReader(xsceneFilePath)))
			{
				string attribute;
				string attribute2;
				if (xmlReader.MoveToContent() == XmlNodeType.Element && xmlReader.Name == "scene" && (attribute = xmlReader.GetAttribute("unique_token")) != null && (attribute2 = xmlReader.GetAttribute("revision")) != null)
				{
					identifiers = new UniqueSceneId(attribute, attribute2);
				}
			}
			return identifiers != null;
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0000F4A4 File Offset: 0x0000D6A4
		public static void PairSceneNameToModuleName(string sceneName, string moduleName)
		{
			EngineApplicationInterface.IUtil.PairSceneNameToModuleName(sceneName, moduleName);
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0000F4B2 File Offset: 0x0000D6B2
		public static string[] GetSingleModuleScenesOfModule(string moduleName)
		{
			return EngineApplicationInterface.IUtil.GetSingleModuleScenesOfModule(moduleName).Split(new char[] { '*' });
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0000F4CF File Offset: 0x0000D6CF
		public static string GetFullCommandLineString()
		{
			return EngineApplicationInterface.IUtil.GetFullCommandLineString();
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0000F4DB File Offset: 0x0000D6DB
		public static void SetScreenTextRenderingState(bool state)
		{
			EngineApplicationInterface.IUtil.SetScreenTextRenderingState(state);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0000F4E8 File Offset: 0x0000D6E8
		public static void SetMessageLineRenderingState(bool state)
		{
			EngineApplicationInterface.IUtil.SetMessageLineRenderingState(state);
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0000F4F5 File Offset: 0x0000D6F5
		public static bool CheckIfTerrainShaderHeaderGenerationFinished()
		{
			return EngineApplicationInterface.IUtil.CheckIfTerrainShaderHeaderGenerationFinished();
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0000F501 File Offset: 0x0000D701
		public static void GenerateTerrainShaderHeaders(string targetPlatform, string targetConfig, string output_path)
		{
			EngineApplicationInterface.IUtil.GenerateTerrainShaderHeaders(targetPlatform, targetConfig, output_path);
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0000F510 File Offset: 0x0000D710
		public static void CompileTerrainShadersDist(string targetPlatform, string targetConfig, string output_path)
		{
			EngineApplicationInterface.IUtil.CompileTerrainShadersDist(targetPlatform, targetConfig, output_path);
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0000F51F File Offset: 0x0000D71F
		public static void SetCrashOnAsserts(bool val)
		{
			EngineApplicationInterface.IUtil.SetCrashOnAsserts(val);
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0000F52C File Offset: 0x0000D72C
		public static void SetCrashOnWarnings(bool val)
		{
			EngineApplicationInterface.IUtil.SetCrashOnWarnings(val);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0000F539 File Offset: 0x0000D739
		public static void SetCreateDumpOnWarnings(bool val)
		{
			EngineApplicationInterface.IUtil.SetCreateDumpOnWarnings(val);
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0000F546 File Offset: 0x0000D746
		public static void ToggleRender()
		{
			EngineApplicationInterface.IUtil.ToggleRender();
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0000F552 File Offset: 0x0000D752
		public static void SetRenderAgents(bool value)
		{
			EngineApplicationInterface.IUtil.SetRenderAgents(value);
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0000F55F File Offset: 0x0000D75F
		public static bool CheckShaderCompilation()
		{
			return EngineApplicationInterface.IUtil.CheckShaderCompilation();
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0000F56B File Offset: 0x0000D76B
		public static void CompileAllShaders(string targetPlatform)
		{
			EngineApplicationInterface.IUtil.CompileAllShaders(targetPlatform);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0000F578 File Offset: 0x0000D778
		public static string GetExecutableWorkingDirectory()
		{
			return EngineApplicationInterface.IUtil.GetExecutableWorkingDirectory();
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0000F584 File Offset: 0x0000D784
		public static void SetDumpFolderPath(string path)
		{
			EngineApplicationInterface.IUtil.SetDumpFolderPath(path);
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0000F591 File Offset: 0x0000D791
		public static void CheckSceneForProblems(string sceneName)
		{
			EngineApplicationInterface.IUtil.CheckSceneForProblems(sceneName);
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0000F59E File Offset: 0x0000D79E
		public static void SetCoreGameState(int state)
		{
			EngineApplicationInterface.IUtil.SetCoreGameState(state);
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0000F5AB File Offset: 0x0000D7AB
		public static int GetCoreGameState()
		{
			return EngineApplicationInterface.IUtil.GetCoreGameState();
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0000F5B7 File Offset: 0x0000D7B7
		public static string ExecuteCommandLineCommand(string command)
		{
			return EngineApplicationInterface.IUtil.ExecuteCommandLineCommand(command);
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0000F5C4 File Offset: 0x0000D7C4
		public static void QuitGame()
		{
			EngineApplicationInterface.IUtil.QuitGame();
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0000F5D0 File Offset: 0x0000D7D0
		public static void ExitProcess(int exitCode)
		{
			EngineApplicationInterface.IUtil.ExitProcess(exitCode);
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0000F5DD File Offset: 0x0000D7DD
		public static string GetBasePath()
		{
			return EngineApplicationInterface.IUtil.GetBaseDirectory();
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0000F5E9 File Offset: 0x0000D7E9
		public static string GetVisualTestsValidatePath()
		{
			return EngineApplicationInterface.IUtil.GetVisualTestsValidatePath();
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0000F5F5 File Offset: 0x0000D7F5
		public static string GetVisualTestsTestFilesPath()
		{
			return EngineApplicationInterface.IUtil.GetVisualTestsTestFilesPath();
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0000F601 File Offset: 0x0000D801
		public static string GetAttachmentsPath()
		{
			return EngineApplicationInterface.IUtil.GetAttachmentsPath();
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0000F60D File Offset: 0x0000D80D
		public static void StartScenePerformanceReport(string folderPath)
		{
			EngineApplicationInterface.IUtil.StartScenePerformanceReport(folderPath);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0000F61A File Offset: 0x0000D81A
		public static bool IsSceneReportFinished()
		{
			return EngineApplicationInterface.IUtil.IsSceneReportFinished();
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0000F626 File Offset: 0x0000D826
		public static float GetFps()
		{
			return EngineApplicationInterface.IUtil.GetFps();
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0000F632 File Offset: 0x0000D832
		public static float GetMainFps()
		{
			return EngineApplicationInterface.IUtil.GetMainFps();
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0000F63E File Offset: 0x0000D83E
		public static float GetRendererFps()
		{
			return EngineApplicationInterface.IUtil.GetRendererFps();
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0000F64A File Offset: 0x0000D84A
		public static void EnableSingleGPUQueryPerFrame()
		{
			EngineApplicationInterface.IUtil.EnableSingleGPUQueryPerFrame();
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0000F656 File Offset: 0x0000D856
		public static void ClearDecalAtlas(DecalAtlasGroup atlasGroup)
		{
			EngineApplicationInterface.IUtil.clear_decal_atlas(atlasGroup);
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0000F663 File Offset: 0x0000D863
		public static void FlushManagedObjectsMemory()
		{
			Common.MemoryCleanupGC(false);
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0000F66B File Offset: 0x0000D86B
		public static void OnLoadingWindowEnabled()
		{
			EngineApplicationInterface.IUtil.OnLoadingWindowEnabled();
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0000F677 File Offset: 0x0000D877
		public static void DebugSetGlobalLoadingWindowState(bool newState)
		{
			EngineApplicationInterface.IUtil.DebugSetGlobalLoadingWindowState(newState);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0000F684 File Offset: 0x0000D884
		public static void OnLoadingWindowDisabled()
		{
			EngineApplicationInterface.IUtil.OnLoadingWindowDisabled();
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0000F690 File Offset: 0x0000D890
		public static void DisableGlobalLoadingWindow()
		{
			EngineApplicationInterface.IUtil.DisableGlobalLoadingWindow();
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0000F69C File Offset: 0x0000D89C
		public static void EnableGlobalLoadingWindow()
		{
			EngineApplicationInterface.IUtil.EnableGlobalLoadingWindow();
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0000F6A8 File Offset: 0x0000D8A8
		public static void EnableGlobalEditDataCacher()
		{
			EngineApplicationInterface.IUtil.EnableGlobalEditDataCacher();
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0000F6B4 File Offset: 0x0000D8B4
		public static void DoFullBakeAllLevelsAutomated(string module, string scene)
		{
			EngineApplicationInterface.IUtil.DoFullBakeAllLevelsAutomated(module, scene);
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0000F6C2 File Offset: 0x0000D8C2
		public static int GetReturnCode()
		{
			return EngineApplicationInterface.IUtil.GetReturnCode();
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0000F6CE File Offset: 0x0000D8CE
		public static int GetUniqueAssertCount()
		{
			return EngineApplicationInterface.IUtil.GetUniqueAssertCount();
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0000F6DA File Offset: 0x0000D8DA
		public static int GetUniqueWarningCount()
		{
			return EngineApplicationInterface.IUtil.GetUniqueWarningCount();
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0000F6E6 File Offset: 0x0000D8E6
		public static void DisableGlobalEditDataCacher()
		{
			EngineApplicationInterface.IUtil.DisableGlobalEditDataCacher();
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0000F6F2 File Offset: 0x0000D8F2
		public static void DoFullBakeSingleLevelAutomated(string module, string scene)
		{
			EngineApplicationInterface.IUtil.DoFullBakeSingleLevelAutomated(module, scene);
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0000F700 File Offset: 0x0000D900
		public static void DoLightOnlyBakeSingleLevelAutomated(string module, string scene)
		{
			EngineApplicationInterface.IUtil.DoLightOnlyBakeSingleLevelAutomated(module, scene);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0000F70E File Offset: 0x0000D90E
		public static void DoLightOnlyBakeAllLevelsAutomated(string module, string scene)
		{
			EngineApplicationInterface.IUtil.DoLightOnlyBakeAllLevelsAutomated(module, scene);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0000F71C File Offset: 0x0000D91C
		public static bool DidAutomatedGIBakeFinished()
		{
			return EngineApplicationInterface.IUtil.DidAutomatedGIBakeFinished();
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0000F728 File Offset: 0x0000D928
		public static void GetSelectedEntities(ref List<GameEntity> gameEntities)
		{
			int editorSelectedEntityCount = EngineApplicationInterface.IUtil.GetEditorSelectedEntityCount();
			UIntPtr[] array = new UIntPtr[editorSelectedEntityCount];
			EngineApplicationInterface.IUtil.GetEditorSelectedEntities(array);
			for (int i = 0; i < editorSelectedEntityCount; i++)
			{
				gameEntities.Add(new GameEntity(array[i]));
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0000F770 File Offset: 0x0000D970
		public static void DeleteEntitiesInEditorScene(List<GameEntity> gameEntities)
		{
			int count = gameEntities.Count;
			UIntPtr[] array = new UIntPtr[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = gameEntities[i].Pointer;
			}
			EngineApplicationInterface.IUtil.DeleteEntitiesInEditorScene(array, count);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0000F7B4 File Offset: 0x0000D9B4
		public static void CreateSelectionInEditor(List<GameEntity> gameEntities, string name)
		{
			int count = gameEntities.Count;
			UIntPtr[] array = new UIntPtr[gameEntities.Count];
			for (int i = 0; i < count; i++)
			{
				array[i] = gameEntities[i].Pointer;
			}
			EngineApplicationInterface.IUtil.CreateSelectionInEditor(array, count, name);
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0000F7FC File Offset: 0x0000D9FC
		public static void SelectEntities(List<GameEntity> gameEntities)
		{
			int count = gameEntities.Count;
			UIntPtr[] array = new UIntPtr[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = gameEntities[i].Pointer;
			}
			EngineApplicationInterface.IUtil.SelectEntities(array, count);
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0000F840 File Offset: 0x0000DA40
		public static void GetEntitiesOfSelectionSet(string selectionSetName, ref List<GameEntity> gameEntities)
		{
			int entityCountOfSelectionSet = EngineApplicationInterface.IUtil.GetEntityCountOfSelectionSet(selectionSetName);
			UIntPtr[] array = new UIntPtr[entityCountOfSelectionSet];
			EngineApplicationInterface.IUtil.GetEntitiesOfSelectionSet(selectionSetName, array);
			for (int i = 0; i < entityCountOfSelectionSet; i++)
			{
				gameEntities.Add(new GameEntity(array[i]));
			}
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0000F887 File Offset: 0x0000DA87
		public static void AddCommandLineFunction(string concatName)
		{
			EngineApplicationInterface.IUtil.AddCommandLineFunction(concatName);
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0000F894 File Offset: 0x0000DA94
		public static int GetNumberOfShaderCompilationsInProgress()
		{
			return EngineApplicationInterface.IUtil.GetNumberOfShaderCompilationsInProgress();
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0000F8A0 File Offset: 0x0000DAA0
		public static int IsDetailedSoundLogOn()
		{
			return EngineApplicationInterface.IUtil.IsDetailedSoundLogOn();
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0000F8AC File Offset: 0x0000DAAC
		public static ulong GetCurrentCpuMemoryUsageMB()
		{
			return EngineApplicationInterface.IUtil.GetCurrentCpuMemoryUsage();
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0000F8B8 File Offset: 0x0000DAB8
		public static ulong GetGpuMemoryOfAllocationGroup(string name)
		{
			return EngineApplicationInterface.IUtil.GetGpuMemoryOfAllocationGroup(name);
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0000F8C5 File Offset: 0x0000DAC5
		public static void GetGPUMemoryStats(ref float totalMemory, ref float renderTargetMemory, ref float depthTargetMemory, ref float srvMemory, ref float bufferMemory)
		{
			EngineApplicationInterface.IUtil.GetGPUMemoryStats(ref totalMemory, ref renderTargetMemory, ref depthTargetMemory, ref srvMemory, ref bufferMemory);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0000F8D7 File Offset: 0x0000DAD7
		public static void GetDetailedGPUMemoryData(ref int totalMemoryAllocated, ref int totalMemoryUsed, ref int emptyChunkTotalSize)
		{
			EngineApplicationInterface.IUtil.GetDetailedGPUBufferMemoryStats(ref totalMemoryAllocated, ref totalMemoryUsed, ref emptyChunkTotalSize);
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0000F8E6 File Offset: 0x0000DAE6
		public static void SetRenderMode(Utilities.EngineRenderDisplayMode mode)
		{
			EngineApplicationInterface.IUtil.SetRenderMode((int)mode);
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0000F8F3 File Offset: 0x0000DAF3
		public static void SetForceDrawEntityID(bool value)
		{
			EngineApplicationInterface.IUtil.SetForceDrawEntityID(value);
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0000F900 File Offset: 0x0000DB00
		public static void AddPerformanceReportToken(string performance_type, string name, float loading_time)
		{
			EngineApplicationInterface.IUtil.AddPerformanceReportToken(performance_type, name, loading_time);
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0000F90F File Offset: 0x0000DB0F
		public static void AddSceneObjectReport(string scene_name, string report_name, float report_value)
		{
			EngineApplicationInterface.IUtil.AddSceneObjectReport(scene_name, report_name, report_value);
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0000F91E File Offset: 0x0000DB1E
		public static void OutputPerformanceReports()
		{
			EngineApplicationInterface.IUtil.OutputPerformanceReports();
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x0000F92A File Offset: 0x0000DB2A
		public static int EngineFrameNo
		{
			get
			{
				return EngineApplicationInterface.IUtil.GetEngineFrameNo();
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x0000F936 File Offset: 0x0000DB36
		public static bool EditModeEnabled
		{
			get
			{
				return EngineApplicationInterface.IUtil.IsEditModeEnabled();
			}
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0000F942 File Offset: 0x0000DB42
		public static void TakeScreenshot(PlatformFilePath path)
		{
			EngineApplicationInterface.IUtil.TakeScreenshotFromPlatformPath(path);
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0000F94F File Offset: 0x0000DB4F
		public static void TakeScreenshot(string path)
		{
			EngineApplicationInterface.IUtil.TakeScreenshotFromStringPath(path);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0000F95C File Offset: 0x0000DB5C
		public static void TakeScreenshotAsPng(string path)
		{
			EngineApplicationInterface.IUtil.TakeScreenshotAsPng(path);
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0000F969 File Offset: 0x0000DB69
		public static void SetAllocationAlwaysValidScene(Scene scene)
		{
			EngineApplicationInterface.IUtil.SetAllocationAlwaysValidScene((scene != null) ? scene.Pointer : UIntPtr.Zero);
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0000F98B File Offset: 0x0000DB8B
		public static void CheckResourceModifications()
		{
			EngineApplicationInterface.IUtil.CheckResourceModifications();
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0000F997 File Offset: 0x0000DB97
		public static void SetGraphicsPreset(int preset)
		{
			EngineApplicationInterface.IUtil.SetGraphicsPreset(preset);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0000F9A4 File Offset: 0x0000DBA4
		public static string GetLocalOutputPath()
		{
			return EngineApplicationInterface.IUtil.GetLocalOutputPath();
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0000F9B0 File Offset: 0x0000DBB0
		public static string GetPCInfo()
		{
			return EngineApplicationInterface.IUtil.GetPCInfo();
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0000F9BC File Offset: 0x0000DBBC
		public static int GetGPUMemoryMB()
		{
			return EngineApplicationInterface.IUtil.GetGPUMemoryMB();
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0000F9C8 File Offset: 0x0000DBC8
		public static int GetCurrentEstimatedGPUMemoryCostMB()
		{
			return EngineApplicationInterface.IUtil.GetCurrentEstimatedGPUMemoryCostMB();
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0000F9D4 File Offset: 0x0000DBD4
		public static void DumpGPUMemoryStatistics(string filePath)
		{
			EngineApplicationInterface.IUtil.DumpGPUMemoryStatistics(filePath);
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0000F9E1 File Offset: 0x0000DBE1
		public static int SaveDataAsTexture(string path, int width, int height, float[] data)
		{
			return EngineApplicationInterface.IUtil.SaveDataAsTexture(path, width, height, data);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0000F9F1 File Offset: 0x0000DBF1
		public static void ClearOldResourcesAndObjects()
		{
			EngineApplicationInterface.IUtil.ClearOldResourcesAndObjects();
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0000F9FD File Offset: 0x0000DBFD
		public static void LoadVirtualTextureTileset(string name)
		{
			EngineApplicationInterface.IUtil.LoadVirtualTextureTileset(name);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0000FA0A File Offset: 0x0000DC0A
		public static float GetDeltaTime(int timerId)
		{
			return EngineApplicationInterface.IUtil.GetDeltaTime(timerId);
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0000FA17 File Offset: 0x0000DC17
		public static void LoadSkyBoxes()
		{
			EngineApplicationInterface.IUtil.LoadSkyBoxes();
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0000FA23 File Offset: 0x0000DC23
		public static string GetApplicationName()
		{
			return EngineApplicationInterface.IUtil.GetApplicationName();
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0000FA2F File Offset: 0x0000DC2F
		public static void OpenConsoleStorePage(string productId)
		{
			EngineApplicationInterface.IUtil.OpenConsoleStorePage(productId);
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0000FA3C File Offset: 0x0000DC3C
		public static void SetWindowTitle(string title)
		{
			EngineApplicationInterface.IUtil.SetWindowTitle(title);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0000FA49 File Offset: 0x0000DC49
		public static string ProcessWindowTitle(string title)
		{
			return EngineApplicationInterface.IUtil.ProcessWindowTitle(title);
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0000FA56 File Offset: 0x0000DC56
		public static uint GetCurrentProcessID()
		{
			return EngineApplicationInterface.IUtil.GetCurrentProcessID();
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0000FA62 File Offset: 0x0000DC62
		public static void DoDelayedexit(int returnCode)
		{
			EngineApplicationInterface.IUtil.DoDelayedexit(returnCode);
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0000FA6F File Offset: 0x0000DC6F
		public static void SetAssertionsAndWarningsSetExitCode(bool value)
		{
			EngineApplicationInterface.IUtil.SetAssertionsAndWarningsSetExitCode(value);
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0000FA7C File Offset: 0x0000DC7C
		public static void SetReportMode(bool reportMode)
		{
			EngineApplicationInterface.IUtil.SetReportMode(reportMode);
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0000FA89 File Offset: 0x0000DC89
		public static void SetAssertionAtShaderCompile(bool value)
		{
			EngineApplicationInterface.IUtil.SetAssertionAtShaderCompile(value);
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0000FA96 File Offset: 0x0000DC96
		public static void SetCrashReportCustomString(string customString)
		{
			EngineApplicationInterface.IUtil.SetCrashReportCustomString(customString);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0000FAA3 File Offset: 0x0000DCA3
		public static void SetCrashReportCustomStack(string customStack)
		{
			EngineApplicationInterface.IUtil.SetCrashReportCustomStack(customStack);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0000FAB0 File Offset: 0x0000DCB0
		public static int GetSteamAppId()
		{
			return EngineApplicationInterface.IUtil.GetSteamAppId();
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0000FABC File Offset: 0x0000DCBC
		public static void SetForceVsync(bool value)
		{
			Debug.Print("Force VSync State is now " + (value ? "ACTIVE" : "DEACTIVATED"), 0, Debug.DebugColor.DarkBlue, 17592186044416UL);
			EngineApplicationInterface.IUtil.SetForceVsync(value);
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x0000FAF2 File Offset: 0x0000DCF2
		private static PlatformFilePath DefaultBannerlordConfigFullPath
		{
			get
			{
				return new PlatformFilePath(EngineFilePaths.ConfigsPath, "BannerlordConfig.txt");
			}
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0000FB04 File Offset: 0x0000DD04
		public static string LoadBannerlordConfigFile()
		{
			PlatformFilePath defaultBannerlordConfigFullPath = Utilities.DefaultBannerlordConfigFullPath;
			if (!FileHelper.FileExists(defaultBannerlordConfigFullPath))
			{
				return "";
			}
			return FileHelper.GetFileContentString(defaultBannerlordConfigFullPath);
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0000FB2C File Offset: 0x0000DD2C
		public static SaveResult SaveConfigFile(string configProperties)
		{
			PlatformFilePath defaultBannerlordConfigFullPath = Utilities.DefaultBannerlordConfigFullPath;
			SaveResult saveResult;
			try
			{
				string text = configProperties.Substring(0, configProperties.Length - 1);
				FileHelper.SaveFileString(defaultBannerlordConfigFullPath, text);
				saveResult = SaveResult.Success;
			}
			catch
			{
				Debug.Print("Could not create Bannerlord Config file", 0, Debug.DebugColor.White, 17592186044416UL);
				saveResult = SaveResult.ConfigFileFailure;
			}
			return saveResult;
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0000FB88 File Offset: 0x0000DD88
		public static void OpenOnscreenKeyboard(string initialText, string descriptionText, int maxLength, int keyboardTypeEnum)
		{
			EngineApplicationInterface.IUtil.OpenOnscreenKeyboard(initialText, descriptionText, maxLength, keyboardTypeEnum);
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0000FB98 File Offset: 0x0000DD98
		public static string GetSystemLanguage()
		{
			return EngineApplicationInterface.IUtil.GetSystemLanguage();
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0000FBA4 File Offset: 0x0000DDA4
		public static int RegisterGPUAllocationGroup(string name)
		{
			return EngineApplicationInterface.IUtil.RegisterGPUAllocationGroup(name);
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0000FBB1 File Offset: 0x0000DDB1
		public static int GetMemoryUsageOfCategory(int category)
		{
			return EngineApplicationInterface.IUtil.GetMemoryUsageOfCategory(category);
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0000FBBE File Offset: 0x0000DDBE
		public static string GetDetailedXBOXMemoryInfo()
		{
			return EngineApplicationInterface.IUtil.GetDetailedXBOXMemoryInfo();
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0000FBCA File Offset: 0x0000DDCA
		public static void SetFrameLimiterWithSleep(bool value)
		{
			EngineApplicationInterface.IUtil.SetFrameLimiterWithSleep(value);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0000FBD7 File Offset: 0x0000DDD7
		public static bool GetFrameLimiterWithSleep()
		{
			return EngineApplicationInterface.IUtil.GetFrameLimiterWithSleep();
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0000FBE3 File Offset: 0x0000DDE3
		public static string GetPossibleCommandLineStartingWith(string command, int index)
		{
			return EngineApplicationInterface.IUtil.GetPossibleCommandLineStartingWith(command, index);
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0000FBF1 File Offset: 0x0000DDF1
		public static bool IsDevkit()
		{
			return EngineApplicationInterface.IUtil.IsDevkit();
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0000FBFD File Offset: 0x0000DDFD
		public static bool IsLockhartPlatform()
		{
			return EngineApplicationInterface.IUtil.IsLockhartPlatform();
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0000FC09 File Offset: 0x0000DE09
		public static int GetVertexBufferChunkSystemMemoryUsage()
		{
			return EngineApplicationInterface.IUtil.GetVertexBufferChunkSystemMemoryUsage();
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0000FC15 File Offset: 0x0000DE15
		public static int GetBuildNumber()
		{
			return EngineApplicationInterface.IUtil.GetBuildNumber();
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0000FC21 File Offset: 0x0000DE21
		public static ApplicationVersion GetApplicationVersionWithBuildNumber()
		{
			return ApplicationVersion.FromParametersFile(null);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0000FC29 File Offset: 0x0000DE29
		public static void ParallelFor(int startIndex, int endIndex, long curKey, int grainSize)
		{
			EngineApplicationInterface.IUtil.ManagedParallelFor(startIndex, endIndex, curKey, grainSize);
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0000FC39 File Offset: 0x0000DE39
		public static void ParallelForWithDt(int startIndex, int endIndex, long curKey, int grainSize)
		{
			EngineApplicationInterface.IUtil.ManagedParallelForWithDt(startIndex, endIndex, curKey, grainSize);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0000FC49 File Offset: 0x0000DE49
		public static void ParallelForWithoutRenderThread(int startIndex, int endIndex, long curKey, int grainSize)
		{
			EngineApplicationInterface.IUtil.ManagedParallelForWithoutRenderThread(startIndex, endIndex, curKey, grainSize);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0000FC59 File Offset: 0x0000DE59
		public static void ParallelForWithoutRenderThreadDt(int startIndex, int endIndex, long curKey, int grainSize)
		{
			EngineApplicationInterface.IUtil.ManagedParallelForWithoutRenderThreadDt(startIndex, endIndex, curKey, grainSize);
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0000FC69 File Offset: 0x0000DE69
		public static void ClearShaderMemory()
		{
			EngineApplicationInterface.IUtil.ClearShaderMemory();
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0000FC75 File Offset: 0x0000DE75
		public static void RegisterMeshForGPUMorph(string metaMeshName)
		{
			EngineApplicationInterface.IUtil.RegisterMeshForGPUMorph(metaMeshName);
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0000FC82 File Offset: 0x0000DE82
		public static ulong GetMainThreadId()
		{
			return EngineApplicationInterface.IUtil.GetMainThreadId();
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0000FC8E File Offset: 0x0000DE8E
		public static ulong GetCurrentThreadId()
		{
			return EngineApplicationInterface.IUtil.GetCurrentThreadId();
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0000FC9A File Offset: 0x0000DE9A
		public static void SetWatchdogValue(string fileName, string groupName, string key, string value)
		{
			EngineApplicationInterface.IUtil.SetWatchdogValue(fileName, groupName, key, value);
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0000FCAA File Offset: 0x0000DEAA
		public static void SetWatchdogAutoreport(bool enabled)
		{
			EngineApplicationInterface.IUtil.SetWatchdogAutoreport(enabled);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0000FCB7 File Offset: 0x0000DEB7
		public static void DetachWatchdog()
		{
			EngineApplicationInterface.IUtil.DetachWatchdog();
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0000FCC3 File Offset: 0x0000DEC3
		public static string GetPlatformModulePaths()
		{
			return EngineApplicationInterface.IUtil.GetPlatformModulePaths();
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0000FCCF File Offset: 0x0000DECF
		public static bool IsAsyncPhysicsThread()
		{
			return EngineApplicationInterface.IUtil.IsAsyncPhysicsThread();
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0000FCDB File Offset: 0x0000DEDB
		public static void StartLoadingStuckCheckState(float timeoutThresholdSeconds)
		{
			EngineApplicationInterface.IUtil.StartLoadingStuckCheckState(timeoutThresholdSeconds);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0000FCE8 File Offset: 0x0000DEE8
		public static void EndLoadingStuckCheckState()
		{
			EngineApplicationInterface.IUtil.EndLoadingStuckCheckState();
		}

		// Token: 0x04000204 RID: 516
		private static ConcurrentQueue<Utilities.MainThreadJob> jobs = new ConcurrentQueue<Utilities.MainThreadJob>();

		// Token: 0x04000205 RID: 517
		public static bool renderingActive = true;

		// Token: 0x020000D8 RID: 216
		public enum EngineRenderDisplayMode
		{
			// Token: 0x04000472 RID: 1138
			ShowNone,
			// Token: 0x04000473 RID: 1139
			ShowAlbedo,
			// Token: 0x04000474 RID: 1140
			ShowNormals,
			// Token: 0x04000475 RID: 1141
			ShowVertexNormals,
			// Token: 0x04000476 RID: 1142
			ShowSpecular,
			// Token: 0x04000477 RID: 1143
			ShowGloss,
			// Token: 0x04000478 RID: 1144
			ShowOcclusion,
			// Token: 0x04000479 RID: 1145
			ShowGbufferShadowMask,
			// Token: 0x0400047A RID: 1146
			ShowTranslucency,
			// Token: 0x0400047B RID: 1147
			ShowMotionVector,
			// Token: 0x0400047C RID: 1148
			ShowVertexColor,
			// Token: 0x0400047D RID: 1149
			ShowDepth,
			// Token: 0x0400047E RID: 1150
			ShowTiledLightOverdraw,
			// Token: 0x0400047F RID: 1151
			ShowTiledDecalOverdraw,
			// Token: 0x04000480 RID: 1152
			ShowMeshId,
			// Token: 0x04000481 RID: 1153
			ShowDisableSunLighting,
			// Token: 0x04000482 RID: 1154
			ShowDebugTexture,
			// Token: 0x04000483 RID: 1155
			ShowTextureDensity,
			// Token: 0x04000484 RID: 1156
			ShowOverdraw,
			// Token: 0x04000485 RID: 1157
			ShowVsComplexity,
			// Token: 0x04000486 RID: 1158
			ShowPsComplexity,
			// Token: 0x04000487 RID: 1159
			ShowDisableAmbientLighting,
			// Token: 0x04000488 RID: 1160
			ShowEntityId,
			// Token: 0x04000489 RID: 1161
			ShowPrtDiffuseAmbient,
			// Token: 0x0400048A RID: 1162
			ShowLightDebugMode,
			// Token: 0x0400048B RID: 1163
			ShowParticleShadingAtlas,
			// Token: 0x0400048C RID: 1164
			ShowTerrainAngle,
			// Token: 0x0400048D RID: 1165
			ShowParallaxDebug,
			// Token: 0x0400048E RID: 1166
			ShowAlbedoValidation,
			// Token: 0x0400048F RID: 1167
			NumDebugModes
		}

		// Token: 0x020000D9 RID: 217
		private class MainThreadJob
		{
			// Token: 0x06001044 RID: 4164 RVA: 0x00015226 File Offset: 0x00013426
			internal MainThreadJob(Delegate function, object[] parameters)
			{
				this._function = function;
				this._parameters = parameters;
				this.wait_handle = null;
			}

			// Token: 0x06001045 RID: 4165 RVA: 0x00015243 File Offset: 0x00013443
			internal MainThreadJob(Semaphore sema, Delegate function, object[] parameters)
			{
				this._function = function;
				this._parameters = parameters;
				this.wait_handle = sema;
			}

			// Token: 0x06001046 RID: 4166 RVA: 0x00015260 File Offset: 0x00013460
			internal void Invoke()
			{
				this._function.DynamicInvoke(this._parameters);
				if (this.wait_handle != null)
				{
					this.wait_handle.Release();
				}
			}

			// Token: 0x04000490 RID: 1168
			private Delegate _function;

			// Token: 0x04000491 RID: 1169
			private object[] _parameters;

			// Token: 0x04000492 RID: 1170
			private Semaphore wait_handle;
		}

		// Token: 0x020000DA RID: 218
		public class MainThreadPerformanceQuery : IDisposable
		{
			// Token: 0x06001047 RID: 4167 RVA: 0x00015288 File Offset: 0x00013488
			public MainThreadPerformanceQuery(string parent, string name)
			{
				this._name = name;
				this._parent = parent;
				this._stopWatch = new Stopwatch();
				this._stopWatch.Start();
			}

			// Token: 0x06001048 RID: 4168 RVA: 0x000152B4 File Offset: 0x000134B4
			public void Dispose()
			{
				this._stopWatch.Stop();
				float num = (float)this._stopWatch.Elapsed.TotalMilliseconds;
				num /= 1000f;
				EngineApplicationInterface.IUtil.AddMainThreadPerformanceQuery(this._parent, this._name, num);
			}

			// Token: 0x04000493 RID: 1171
			private string _name;

			// Token: 0x04000494 RID: 1172
			private string _parent;

			// Token: 0x04000495 RID: 1173
			private Stopwatch _stopWatch;
		}
	}
}
