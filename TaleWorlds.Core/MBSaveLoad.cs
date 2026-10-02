using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Load;

namespace TaleWorlds.Core
{
	// Token: 0x020000B7 RID: 183
	public static class MBSaveLoad
	{
		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x0001EDBA File Offset: 0x0001CFBA
		public static char ModuleVersionSeperator
		{
			get
			{
				return ':';
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x0001EDBE File Offset: 0x0001CFBE
		public static char ModuleCodeSeperator
		{
			get
			{
				return ';';
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x0001EDC2 File Offset: 0x0001CFC2
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x0001EDC9 File Offset: 0x0001CFC9
		public static ApplicationVersion LastLoadedGameVersion { get; private set; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0001EDD1 File Offset: 0x0001CFD1
		public static ApplicationVersion CurrentVersion { get; } = ApplicationVersion.FromParametersFile(null);

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x0001EDD8 File Offset: 0x0001CFD8
		public static bool IsUpdatingGameVersion
		{
			get
			{
				return MBSaveLoad.LastLoadedGameVersion.IsOlderThan(MBSaveLoad.CurrentVersion);
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x0001EDF7 File Offset: 0x0001CFF7
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x0001EDFE File Offset: 0x0001CFFE
		public static int NumberOfCurrentSaves { get; private set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x0001EE06 File Offset: 0x0001D006
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x0001EE0D File Offset: 0x0001D00D
		public static string ActiveSaveSlotName { get; private set; } = null;

		// Token: 0x0600097F RID: 2431 RVA: 0x0001EE15 File Offset: 0x0001D015
		private static string GetAutoSaveName()
		{
			return MBSaveLoad.AutoSaveNamePrefix + MBSaveLoad.AutoSaveIndex;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0001EE2B File Offset: 0x0001D02B
		private static void IncrementAutoSaveIndex()
		{
			MBSaveLoad.AutoSaveIndex++;
			if (MBSaveLoad.AutoSaveIndex > 3)
			{
				MBSaveLoad.AutoSaveIndex = 1;
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0001EE48 File Offset: 0x0001D048
		private static void InitializeAutoSaveIndex(string saveName)
		{
			string text = string.Empty;
			if (saveName.StartsWith(MBSaveLoad.AutoSaveNamePrefix))
			{
				text = saveName;
			}
			else
			{
				foreach (string text2 in MBSaveLoad.GetSaveFileNames())
				{
					if (text2.StartsWith(MBSaveLoad.AutoSaveNamePrefix))
					{
						text = text2;
						break;
					}
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				MBSaveLoad.AutoSaveIndex = 1;
				return;
			}
			int num;
			if (int.TryParse(text.Substring(MBSaveLoad.AutoSaveNamePrefix.Length), out num) && num > 0 && num <= 3)
			{
				MBSaveLoad.AutoSaveIndex = num;
			}
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0001EECE File Offset: 0x0001D0CE
		public static void SetSaveDriver(ISaveDriver saveDriver)
		{
			MBSaveLoad._saveDriver = saveDriver;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0001EED8 File Offset: 0x0001D0D8
		public static SaveGameFileInfo[] GetSaveFiles(Func<SaveGameFileInfo, bool> condition = null)
		{
			SaveGameFileInfo[] saveGameFileInfos = MBSaveLoad._saveDriver.GetSaveGameFileInfos();
			MBSaveLoad.NumberOfCurrentSaves = saveGameFileInfos.Length;
			List<SaveGameFileInfo> list = new List<SaveGameFileInfo>();
			foreach (SaveGameFileInfo saveGameFileInfo in saveGameFileInfos)
			{
				if (condition == null || condition(saveGameFileInfo))
				{
					list.Add(saveGameFileInfo);
				}
			}
			return list.OrderByDescending<SaveGameFileInfo, DateTime>((SaveGameFileInfo info) => info.MetaData.GetCreationTime()).ToArray<SaveGameFileInfo>();
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0001EF4D File Offset: 0x0001D14D
		public static bool IsSaveGameFileExists(string saveFileName)
		{
			return MBSaveLoad._saveDriver.IsSaveGameFileExists(saveFileName);
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0001EF5A File Offset: 0x0001D15A
		public static string[] GetSaveFileNames()
		{
			return MBSaveLoad._saveDriver.GetSaveGameFileNames();
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0001EF68 File Offset: 0x0001D168
		public static LoadResult LoadSaveGameData(string saveName)
		{
			MBSaveLoad.InitializeAutoSaveIndex(saveName);
			ISaveDriver saveDriver = MBSaveLoad._saveDriver;
			LoadResult loadResult = SaveManager.Load(saveName, saveDriver, true);
			if (loadResult.Successful)
			{
				MBSaveLoad.ActiveSaveSlotName = saveName;
				return loadResult;
			}
			Debug.Print("Error: Could not load the game!", 0, Debug.DebugColor.White, 17592186044416UL);
			return null;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0001EFB4 File Offset: 0x0001D1B4
		public static SaveGameFileInfo GetSaveFileWithName(string saveName)
		{
			SaveGameFileInfo[] saveFiles = MBSaveLoad.GetSaveFiles((SaveGameFileInfo x) => x.Name.Equals(saveName));
			if (saveFiles.Length == 0)
			{
				return null;
			}
			return saveFiles[0];
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0001EFE7 File Offset: 0x0001D1E7
		public static void QuickSaveCurrentGame(CampaignSaveMetaDataArgs campaignMetaData, Action<ValueTuple<SaveResult, string>> onSaveCompleted)
		{
			if (MBSaveLoad.ActiveSaveSlotName == null)
			{
				MBSaveLoad.ActiveSaveSlotName = MBSaveLoad.GetNextAvailableSaveName();
			}
			Debug.Print("QuickSaveCurrentGame: " + MBSaveLoad.ActiveSaveSlotName, 0, Debug.DebugColor.White, 17592186044416UL);
			MBSaveLoad.OverwriteSaveAux(campaignMetaData, MBSaveLoad.ActiveSaveSlotName, onSaveCompleted);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0001F028 File Offset: 0x0001D228
		public static void AutoSaveCurrentGame(CampaignSaveMetaDataArgs campaignMetaData, Action<ValueTuple<SaveResult, string>> onSaveCompleted)
		{
			MBSaveLoad.IncrementAutoSaveIndex();
			string autoSaveName = MBSaveLoad.GetAutoSaveName();
			Debug.Print("AutoSaveCurrentGame: " + autoSaveName, 0, Debug.DebugColor.White, 17592186044416UL);
			MBSaveLoad.OverwriteSaveAux(campaignMetaData, autoSaveName, onSaveCompleted);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0001F064 File Offset: 0x0001D264
		public static void SaveAsCurrentGame(CampaignSaveMetaDataArgs campaignMetaData, string saveName, Action<ValueTuple<SaveResult, string>> onSaveCompleted)
		{
			MBSaveLoad.ActiveSaveSlotName = saveName;
			Debug.Print("SaveAsCurrentGame: " + saveName, 0, Debug.DebugColor.White, 17592186044416UL);
			MBSaveLoad.OverwriteSaveAux(campaignMetaData, saveName, onSaveCompleted);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0001F090 File Offset: 0x0001D290
		private static void OverwriteSaveAux(CampaignSaveMetaDataArgs campaignMetaData, string saveName, Action<ValueTuple<SaveResult, string>> onSaveCompleted)
		{
			MetaData saveMetaData = MBSaveLoad.GetSaveMetaData(campaignMetaData);
			bool isOverwritingExistingSave = MBSaveLoad.IsSaveGameFileExists(saveName);
			if (!MBSaveLoad.IsMaxNumberOfSavesReached() || isOverwritingExistingSave)
			{
				MBSaveLoad.OverwriteSaveFile(saveMetaData, saveName, delegate(SaveResult r)
				{
					Action<ValueTuple<SaveResult, string>> onSaveCompleted3 = onSaveCompleted;
					if (onSaveCompleted3 != null)
					{
						onSaveCompleted3(new ValueTuple<SaveResult, string>(r, saveName));
					}
					if (r == SaveResult.Success && !isOverwritingExistingSave)
					{
						MBSaveLoad.NumberOfCurrentSaves++;
					}
				});
				return;
			}
			Action<ValueTuple<SaveResult, string>> onSaveCompleted2 = onSaveCompleted;
			if (onSaveCompleted2 == null)
			{
				return;
			}
			onSaveCompleted2(new ValueTuple<SaveResult, string>(SaveResult.SaveLimitReached, string.Empty));
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0001F10C File Offset: 0x0001D30C
		public static bool DeleteSaveGame(string saveName)
		{
			bool flag = MBSaveLoad._saveDriver.Delete(saveName);
			if (flag)
			{
				if (MBSaveLoad.NumberOfCurrentSaves > 0)
				{
					MBSaveLoad.NumberOfCurrentSaves--;
				}
				if (MBSaveLoad.ActiveSaveSlotName == saveName)
				{
					MBSaveLoad.ActiveSaveSlotName = null;
				}
			}
			return flag;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0001F143 File Offset: 0x0001D343
		public static void Initialize(GameTextManager localizedTextProvider)
		{
			MBSaveLoad._textProvider = localizedTextProvider;
			MBSaveLoad.NumberOfCurrentSaves = MBSaveLoad._saveDriver.GetSaveGameFileInfos().Length;
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0001F15C File Offset: 0x0001D35C
		public static void OnNewGame()
		{
			MBSaveLoad.ActiveSaveSlotName = null;
			MBSaveLoad.LastLoadedGameVersion = MBSaveLoad.CurrentVersion;
			MBSaveLoad.AutoSaveIndex = 0;
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0001F174 File Offset: 0x0001D374
		public static void OnGameDestroy()
		{
			MBSaveLoad.LastLoadedGameVersion = ApplicationVersion.Empty;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0001F180 File Offset: 0x0001D380
		public static void OnStartGame(LoadResult loadResult)
		{
			MBSaveLoad.LastLoadedGameVersion = loadResult.MetaData.GetApplicationVersion();
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0001F194 File Offset: 0x0001D394
		public static bool IsSaveFileNameReserved(string name)
		{
			for (int i = 1; i <= 3; i++)
			{
				if (name == MBSaveLoad.AutoSaveNamePrefix + i)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0001F1C8 File Offset: 0x0001D3C8
		private static string GetNextAvailableSaveName()
		{
			uint num = 0U;
			foreach (string text in MBSaveLoad.GetSaveFileNames())
			{
				uint num2;
				if (text.StartsWith(MBSaveLoad.DefaultSaveGamePrefix) && uint.TryParse(text.Substring(MBSaveLoad.DefaultSaveGamePrefix.Length), out num2) && num2 > num)
				{
					num = num2;
				}
			}
			num += 1U;
			return MBSaveLoad.DefaultSaveGamePrefix + num.ToString("000");
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0001F238 File Offset: 0x0001D438
		private static void OverwriteSaveFile(MetaData metaData, string name, Action<SaveResult> onSaveCompleted)
		{
			try
			{
				MBSaveLoad.SaveGame(name, metaData, delegate(SaveResult r)
				{
					onSaveCompleted(r);
					MBSaveLoad.ShowErrorFromResult(r);
				});
			}
			catch
			{
				MBSaveLoad.ShowErrorFromResult(SaveResult.GeneralFailure);
			}
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0001F280 File Offset: 0x0001D480
		private static void ShowErrorFromResult(SaveResult result)
		{
			if (result != SaveResult.Success)
			{
				if (result == SaveResult.PlatformFileHelperFailure)
				{
					Debug.FailedAssert("Save Failed:\n" + Common.PlatformFileHelper.GetError(), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\MBSaveLoad.cs", "ShowErrorFromResult", 314);
				}
				if (!MBSaveLoad.DoNotShowSaveErrorAgain)
				{
					InformationManager.ShowInquiry(new InquiryData(MBSaveLoad._textProvider.FindText("str_save_unsuccessful_title", null).ToString(), MBSaveLoad._textProvider.FindText("str_game_save_result", result.ToString()).ToString(), true, false, MBSaveLoad._textProvider.FindText("str_ok", null).ToString(), MBSaveLoad._textProvider.FindText("str_dont_show_again", null).ToString(), null, delegate
					{
						MBSaveLoad.DoNotShowSaveErrorAgain = true;
					}, "", 0f, null, null, null), false, false);
				}
			}
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0001F364 File Offset: 0x0001D564
		private static void SaveGame(string saveName, MetaData metaData, Action<SaveResult> onSaveCompleted)
		{
			ISaveDriver saveDriver = MBSaveLoad._saveDriver;
			try
			{
				Game.Current.Save(metaData, saveName, saveDriver, onSaveCompleted);
			}
			catch (Exception ex)
			{
				Debug.Print("Unable to create save game data", 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print(ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.SilentAssert(ModuleHelper.GetModules(null).Any<ModuleInfo>((ModuleInfo m) => !m.IsOfficial), ex.Message, false, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\MBSaveLoad.cs", "SaveGame", 347);
			}
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0001F40C File Offset: 0x0001D60C
		private static MetaData GetSaveMetaData(CampaignSaveMetaDataArgs data)
		{
			MetaData metaData = new MetaData();
			List<ModuleInfo> moduleInfos = ModuleHelper.GetModuleInfos(data.ModuleNames);
			metaData["Modules"] = string.Join(MBSaveLoad.ModuleCodeSeperator.ToString(), moduleInfos.Select<ModuleInfo, string>((ModuleInfo q) => q.Id));
			foreach (ModuleInfo moduleInfo in moduleInfos)
			{
				metaData["Module_" + moduleInfo.Id] = moduleInfo.Version.ToString();
			}
			metaData.Add("ApplicationVersion", MBSaveLoad.CurrentVersion.ToString());
			metaData.Add("CreationTime", DateTime.Now.Ticks.ToString());
			foreach (KeyValuePair<string, string> keyValuePair in data.OtherData)
			{
				metaData[keyValuePair.Key] = keyValuePair.Value;
			}
			return metaData;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0001F554 File Offset: 0x0001D754
		public static int GetMaxNumberOfSaves()
		{
			return int.MaxValue;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0001F55B File Offset: 0x0001D75B
		public static bool IsMaxNumberOfSavesReached()
		{
			return MBSaveLoad.NumberOfCurrentSaves >= MBSaveLoad.GetMaxNumberOfSaves();
		}

		// Token: 0x0400053B RID: 1339
		private const int MaxNumberOfAutoSaveFiles = 3;

		// Token: 0x0400053C RID: 1340
		private static ISaveDriver _saveDriver = new FileDriver();

		// Token: 0x0400053D RID: 1341
		private static int AutoSaveIndex = 0;

		// Token: 0x0400053E RID: 1342
		private static string DefaultSaveGamePrefix = "save";

		// Token: 0x0400053F RID: 1343
		private static string AutoSaveNamePrefix = MBSaveLoad.DefaultSaveGamePrefix + "auto";

		// Token: 0x04000541 RID: 1345
		private static GameTextManager _textProvider;

		// Token: 0x04000542 RID: 1346
		private static bool DoNotShowSaveErrorAgain = false;
	}
}
