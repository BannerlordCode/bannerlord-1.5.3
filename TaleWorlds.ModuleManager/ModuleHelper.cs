using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;

namespace TaleWorlds.ModuleManager
{
	// Token: 0x02000006 RID: 6
	public static class ModuleHelper
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000211C File Offset: 0x0000031C
		private static string _pathPrefix
		{
			get
			{
				return BasePath.Name + "Modules/";
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000212D File Offset: 0x0000032D
		public static string GetModuleFullPath(string moduleId)
		{
			return ModuleHelper._loadedModules[moduleId.ToLower()].FolderPath + "/";
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002150 File Offset: 0x00000350
		public static ModuleInfo GetModuleInfo(string moduleId)
		{
			string text = moduleId.ToLower();
			if (ModuleHelper._loadedModules.ContainsKey(text))
			{
				return ModuleHelper._loadedModules[text];
			}
			return null;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002180 File Offset: 0x00000380
		public static void OnModuleDeactivated(string id)
		{
			ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo(id);
			if (moduleInfo != null && moduleInfo.IsActive)
			{
				moduleInfo.DeactivateModule();
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000021A8 File Offset: 0x000003A8
		public static void OnModuleActivated(string id)
		{
			ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo(id);
			if (moduleInfo != null && !moduleInfo.IsActive)
			{
				moduleInfo.ActivateModule();
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000021D0 File Offset: 0x000003D0
		public static void InitializeModules(string[] loadedModuleIds, string[] platformModulePaths = null)
		{
			ModuleHelper._loadedModules = new Dictionary<string, ModuleInfo>();
			List<ModuleInfo> list = new List<ModuleInfo>();
			List<ModuleInfo> physicalModules = ModuleHelper.GetPhysicalModules(false);
			List<ModuleInfo> platformModules = ModuleHelper.GetPlatformModules(platformModulePaths);
			list.AddRange(physicalModules);
			list.AddRange(platformModules);
			foreach (ModuleInfo moduleInfo in list)
			{
				if (moduleInfo.Name == "NavalDLC")
				{
					if (moduleInfo.RequiredBaseVersion != ApplicationVersion.FromParametersFile(null))
					{
						string text = "NavalDLC version is not matched with base game! Please verify game files if the problem persists.";
						string text2 = "ERROR";
						Debug.ShowMessageBox(text, text2, 4U);
						Environment.Exit(0);
					}
					VirtualFolders.PlatformDLCPaths.Add("NavalDLC", moduleInfo.FolderPath);
				}
			}
			List<ModuleInfo> list2 = new List<ModuleInfo>();
			for (int i = 0; i < loadedModuleIds.Length; i++)
			{
				string moduleId = loadedModuleIds[i];
				ModuleInfo moduleInfo2 = list.Find((ModuleInfo x) => x.Id.ToLower().Equals(moduleId.ToLower()));
				if (moduleInfo2 != null)
				{
					if (moduleInfo2.IsOfficial)
					{
						list2.Add(moduleInfo2);
						moduleInfo2.UpdateVersionChangeSet();
					}
					if (!ModuleHelper._loadedModules.ContainsKey(moduleInfo2.Id.ToLower()))
					{
						ModuleHelper._loadedModules.Add(moduleInfo2.Id.ToLower(), moduleInfo2);
					}
				}
			}
			foreach (ModuleInfo moduleInfo3 in ModuleHelper._loadedModules.Values)
			{
				using (List<DependedModule>.Enumerator enumerator3 = moduleInfo3.DependedModules.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						DependedModule dependedModule = enumerator3.Current;
						if (list2.Any<ModuleInfo>((ModuleInfo m) => m.Id == dependedModule.ModuleId))
						{
							dependedModule.UpdateVersionChangeSet();
						}
					}
				}
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000023D8 File Offset: 0x000005D8
		public static ModuleInfo InitializeSingleModule(string modulePath)
		{
			Debug.Print("###Trying to load Module:  " + modulePath, 0, Debug.DebugColor.White, 17592186044416UL);
			ModuleInfo moduleInfo = new ModuleInfo();
			try
			{
				moduleInfo.LoadWithFullPath(modulePath);
			}
			catch (Exception ex)
			{
				string text = string.Concat(new string[]
				{
					"Module ",
					modulePath,
					" can't be loaded, there are some errors exception follows:.",
					Environment.NewLine,
					ex.Message
				}) + ex.StackTrace;
				string text2 = "ERROR";
				Debug.ShowMessageBox(text, text2, 4U);
			}
			if (moduleInfo.Name == "NavalDLC")
			{
				VirtualFolders.PlatformDLCPaths.Add("NavalDLC", moduleInfo.FolderPath);
			}
			if (!ModuleHelper._loadedModules.ContainsKey(moduleInfo.Id.ToLower()))
			{
				ModuleHelper._loadedModules.Add(moduleInfo.Id.ToLower(), moduleInfo);
			}
			return moduleInfo;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000024C0 File Offset: 0x000006C0
		public static bool IsModuleActive(string moduleId)
		{
			bool flag = false;
			ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo(moduleId);
			if (moduleInfo != null)
			{
				flag = moduleInfo.IsActive;
			}
			return flag;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000024E1 File Offset: 0x000006E1
		public static void InitializePlatformModuleExtension(IPlatformModuleExtension moduleExtension, List<string> args)
		{
			ModuleHelper._platformModuleExtension = moduleExtension;
			ModuleHelper._platformModuleExtension.Initialize(args);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000024F4 File Offset: 0x000006F4
		public static void ClearPlatformModuleExtension()
		{
			if (ModuleHelper._platformModuleExtension != null)
			{
				ModuleHelper._platformModuleExtension.Destroy();
				ModuleHelper._platformModuleExtension = null;
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002510 File Offset: 0x00000710
		public static List<ModuleInfo> GetModuleInfos(string[] moduleIds)
		{
			List<ModuleInfo> list = new List<ModuleInfo>();
			for (int i = 0; i < moduleIds.Length; i++)
			{
				ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo(moduleIds[i]);
				if (moduleInfo != null)
				{
					list.Add(moduleInfo);
				}
			}
			return list;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002548 File Offset: 0x00000748
		public static List<ModuleInfo> GetModules(Func<ModuleInfo, bool> cond = null)
		{
			List<ModuleInfo> list = new List<ModuleInfo>();
			foreach (ModuleInfo moduleInfo in ModuleHelper._loadedModules.Values)
			{
				if (cond == null || cond(moduleInfo))
				{
					list.Add(moduleInfo);
				}
			}
			return list;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000025B4 File Offset: 0x000007B4
		public static Dictionary<string, ModuleInfo>.ValueCollection GetAllModules()
		{
			return ModuleHelper._loadedModules.Values;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000025C0 File Offset: 0x000007C0
		public static List<ModuleInfo> GetActiveModules()
		{
			return ModuleHelper.GetModules((ModuleInfo x) => x.IsActive);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000025E8 File Offset: 0x000007E8
		public static string GetMbprojPath(string id)
		{
			string text = id.ToLower();
			if (ModuleHelper._loadedModules.ContainsKey(text))
			{
				return ModuleHelper._loadedModules[text].FolderPath + "/ModuleData/project.mbproj";
			}
			return "";
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002629 File Offset: 0x00000829
		public static string GetXmlPathForNative(string moduleId, string xmlName)
		{
			return ModuleHelper.GetModuleFullPath(moduleId) + xmlName;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002637 File Offset: 0x00000837
		public static string GetXmlPathForNativeWBase(string moduleId, string xmlName)
		{
			return "$BASE/Modules/" + moduleId + "/" + xmlName;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000264A File Offset: 0x0000084A
		public static string GetXsltPathForNative(string moduleId, string xsltName)
		{
			xsltName = xsltName.Remove(xsltName.Length - 4);
			return ModuleHelper.GetModuleFullPath(moduleId) + xsltName + ".xsl";
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000266D File Offset: 0x0000086D
		public static string GetPath(string id)
		{
			return ModuleHelper.GetModuleFullPath(id) + "SubModule.xml";
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000267F File Offset: 0x0000087F
		public static string GetXmlPath(string moduleId, string xmlName)
		{
			return ModuleHelper.GetModuleFullPath(moduleId) + "ModuleData/" + xmlName + ".xml";
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002697 File Offset: 0x00000897
		public static string GetXsltPath(string moduleId, string xmlName)
		{
			return ModuleHelper.GetModuleFullPath(moduleId) + "ModuleData/" + xmlName + ".xsl";
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000026AF File Offset: 0x000008AF
		public static string GetXsdPathForModules(string moduleId, string xsdName)
		{
			return ModuleHelper.GetModuleFullPath(moduleId) + "ModuleData/XmlSchemas/" + xsdName + ".xsd";
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000026C7 File Offset: 0x000008C7
		public static string GetXsdPath(string xmlInfoId)
		{
			return BasePath.Name + "XmlSchemas/" + xmlInfoId + ".xsd";
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000026DE File Offset: 0x000008DE
		public static IEnumerable<ModuleInfo> GetDependentModulesOf(IEnumerable<ModuleInfo> source, ModuleInfo module)
		{
			using (List<DependedModule>.Enumerator enumerator = module.DependedModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DependedModule item = enumerator.Current;
					ModuleInfo moduleInfo = source.FirstOrDefault<ModuleInfo>((ModuleInfo i) => i.Id == item.ModuleId);
					if (moduleInfo != null)
					{
						yield return moduleInfo;
					}
				}
			}
			List<DependedModule>.Enumerator enumerator = default(List<DependedModule>.Enumerator);
			Func<DependedModule, bool> <>9__1;
			foreach (ModuleInfo moduleInfo2 in source)
			{
				IEnumerable<DependedModule> modulesToLoadAfterThis = moduleInfo2.ModulesToLoadAfterThis;
				Func<DependedModule, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = (DependedModule m) => m.ModuleId == module.Id);
				}
				if (modulesToLoadAfterThis.Any<DependedModule>(func))
				{
					yield return moduleInfo2;
				}
			}
			IEnumerator<ModuleInfo> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000026F8 File Offset: 0x000008F8
		public static List<ModuleInfo> GetSortedModules(string[] moduleIDs)
		{
			List<ModuleInfo> modules = ModuleHelper.GetModuleInfos(moduleIDs);
			IList<ModuleInfo> list = MBMath.TopologySort<ModuleInfo>(modules, (ModuleInfo module) => ModuleHelper.GetDependentModulesOf(modules, module));
			List<ModuleInfo> list2;
			if ((list2 = list as List<ModuleInfo>) == null)
			{
				return list.ToList<ModuleInfo>();
			}
			return list2;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002744 File Offset: 0x00000944
		public static List<ModuleInfo> GetModulesForLauncher()
		{
			if (ModuleHelper.results == null)
			{
				ModuleHelper.results = new List<ModuleInfo>();
				List<ModuleInfo> physicalModules = ModuleHelper.GetPhysicalModules(true);
				List<ModuleInfo> platformModules = ModuleHelper.GetPlatformModules(null);
				ModuleHelper.results.AddRange(physicalModules);
				ModuleHelper.results.AddRange(platformModules);
				List<ModuleInfo> list = new List<ModuleInfo>();
				foreach (ModuleInfo moduleInfo in ModuleHelper.results)
				{
					if (moduleInfo.IsOfficial)
					{
						list.Add(moduleInfo);
						moduleInfo.UpdateVersionChangeSet();
					}
				}
				foreach (ModuleInfo moduleInfo2 in ModuleHelper.results)
				{
					using (List<DependedModule>.Enumerator enumerator2 = moduleInfo2.DependedModules.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							DependedModule dependedModule = enumerator2.Current;
							if (list.Any<ModuleInfo>((ModuleInfo m) => m.Id == dependedModule.ModuleId))
							{
								dependedModule.UpdateVersionChangeSet();
							}
						}
					}
				}
			}
			return ModuleHelper.results;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002890 File Offset: 0x00000A90
		public static MBList<string> GetOfficialModuleIds()
		{
			return new MBList<string> { "Native", "Multiplayer", "SandBoxCore", "Sandbox", "CustomBattle", "StoryMode", "NavalDLC", "BirthAndDeath", "FastMode" };
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002908 File Offset: 0x00000B08
		private static List<ModuleInfo> GetPhysicalModules(bool isLauncher)
		{
			List<ModuleInfo> list = new List<ModuleInfo>();
			foreach (string text in Directory.GetDirectories(ModuleHelper._pathPrefix))
			{
				try
				{
					string text2 = Path.Combine(text, "SubModule.xml");
					if (File.Exists(text2))
					{
						ModuleInfo moduleInfo = new ModuleInfo();
						string directoryName = Path.GetDirectoryName(text2);
						moduleInfo.LoadWithFullPath(directoryName);
						if (moduleInfo.Name == "NavalDLC" && ModuleHelper._platformModuleExtension != null)
						{
							if (!ModuleHelper._platformModuleExtension.CheckEntitlement("Mount & Blade II: Bannerlord - War Sails"))
							{
								goto IL_0105;
							}
							if (moduleInfo.RequiredBaseVersion != ApplicationVersion.FromParametersFile(null))
							{
								string text3 = "NavalDLC version is not matched with base game! Please verify game files if the problem persists.";
								string text4 = "ERROR";
								Debug.ShowMessageBox(text3, text4, 4U);
								if (!isLauncher)
								{
									Environment.Exit(0);
								}
							}
						}
						list.Add(moduleInfo);
					}
				}
				catch (Exception ex)
				{
					string text5 = string.Concat(new string[]
					{
						"Module ",
						text,
						" can't be loaded, there are some errors.",
						Environment.NewLine,
						Environment.NewLine,
						ex.Message
					});
					string text6 = "ERROR";
					Debug.ShowMessageBox(text5, text6, 4U);
				}
				IL_0105:;
			}
			return list;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A38 File Offset: 0x00000C38
		public static MBList<Assembly> GetActiveGameAssemblies()
		{
			HashSet<Assembly> hashSet = new HashSet<Assembly>();
			Dictionary<string, Assembly> dictionary = new Dictionary<string, Assembly>();
			Assembly[] array;
			try
			{
				array = AppDomain.CurrentDomain.GetAssemblies();
			}
			catch (Exception ex)
			{
				Debug.FailedAssert(ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.ModuleManager\\ModuleHelper.cs", "GetActiveGameAssemblies", 443);
				array = new Assembly[0];
			}
			if (ModuleHelper.IsTestMode)
			{
				return array.ToMBList<Assembly>();
			}
			foreach (Assembly assembly in array)
			{
				dictionary[assembly.GetName().Name] = assembly;
			}
			MBList<Assembly> mblist = new MBList<Assembly>();
			List<ModuleInfo> activeModules = ModuleHelper.GetActiveModules();
			for (int j = 0; j < activeModules.Count; j++)
			{
				ModuleInfo moduleInfo = activeModules[j];
				foreach (Assembly assembly2 in array)
				{
					if (ModuleHelper.IsAssemblyDirectlyReferencedInModule(assembly2, moduleInfo))
					{
						ModuleHelper.CollectAssemblyRecursive(assembly2, dictionary, hashSet, mblist);
					}
				}
			}
			return mblist;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B30 File Offset: 0x00000D30
		private static void CollectAssemblyRecursive(Assembly assembly, Dictionary<string, Assembly> assemblyLookup, HashSet<Assembly> visited, MBList<Assembly> result)
		{
			if (!visited.Add(assembly))
			{
				return;
			}
			foreach (AssemblyName assemblyName in assembly.GetReferencedAssembliesSafe())
			{
				Assembly assembly2;
				if (assemblyLookup.TryGetValue(assemblyName.Name, out assembly2) && ModuleHelper.IsGameAssembly(assembly2))
				{
					ModuleHelper.CollectAssemblyRecursive(assembly2, assemblyLookup, visited, result);
				}
			}
			result.Add(assembly);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002B88 File Offset: 0x00000D88
		private static bool IsAssemblyDirectlyReferencedInModule(Assembly assembly, ModuleInfo moduleInfo)
		{
			string text = assembly.GetName().Name + ".dll";
			foreach (SubModuleInfo subModuleInfo in moduleInfo.SubModules)
			{
				if (subModuleInfo.DLLName == text)
				{
					return true;
				}
				using (List<string>.Enumerator enumerator2 = subModuleInfo.Assemblies.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current == text)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002C48 File Offset: 0x00000E48
		private static bool IsGameAssembly(Assembly assembly)
		{
			AssemblyName name = assembly.GetName();
			return !name.Name.StartsWith("System") && !name.Name.StartsWith("Microsoft") && !name.Name.StartsWith("mscorlib") && !name.Name.StartsWith("netstandard");
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002CA8 File Offset: 0x00000EA8
		private static List<ModuleInfo> GetPlatformModules(string[] platformModulePaths = null)
		{
			List<ModuleInfo> list = new List<ModuleInfo>();
			if (platformModulePaths != null)
			{
				Debug.Print("GetPlatformModules platformModulePaths != null", 0, Debug.DebugColor.White, 17592186044416UL);
				foreach (string text in platformModulePaths)
				{
					ModuleInfo moduleInfo = new ModuleInfo();
					try
					{
						moduleInfo.LoadWithFullPath(text);
						Debug.Print("dir " + text, 0, Debug.DebugColor.White, 17592186044416UL);
						list.Add(moduleInfo);
					}
					catch (Exception ex)
					{
						string text2 = string.Concat(new string[]
						{
							"Module ",
							moduleInfo.Name,
							" with dir: ",
							text,
							" can't be loaded, there are some errors.",
							Environment.NewLine,
							Environment.NewLine,
							ex.ToString()
						});
						string text3 = "ERROR";
						Debug.ShowMessageBox(text2, text3, 4U);
					}
				}
			}
			if (ModuleHelper._platformModuleExtension != null)
			{
				foreach (string text4 in ModuleHelper._platformModuleExtension.GetModulePaths())
				{
					ModuleInfo moduleInfo2 = new ModuleInfo();
					try
					{
						moduleInfo2.LoadWithFullPath(text4);
						list.Add(moduleInfo2);
					}
					catch (Exception ex2)
					{
						string text5 = string.Concat(new string[]
						{
							"Module ",
							moduleInfo2.Name,
							" with dir: ",
							text4,
							" can't be loaded, there are some errors.",
							Environment.NewLine,
							Environment.NewLine,
							ex2.ToString()
						});
						string text6 = "ERROR";
						Debug.ShowMessageBox(text5, text6, 4U);
					}
				}
			}
			return list;
		}

		// Token: 0x04000009 RID: 9
		public const char ModuleVersionSeperator = ':';

		// Token: 0x0400000A RID: 10
		public static bool IsTestMode = false;

		// Token: 0x0400000B RID: 11
		public const char ModuleCodeSeperator = ';';

		// Token: 0x0400000C RID: 12
		public static readonly MBList<string> ModulesDisablingLoadingAfterBeingRemoved = new MBList<string> { "StoryMode", "NavalDLC" };

		// Token: 0x0400000D RID: 13
		public static readonly MBList<string> ModulesDisablingLoadingAfterBeingAdded = new MBList<string> { "NavalDLC" };

		// Token: 0x0400000E RID: 14
		private static IPlatformModuleExtension _platformModuleExtension;

		// Token: 0x0400000F RID: 15
		private static Dictionary<string, ModuleInfo> _loadedModules;

		// Token: 0x04000010 RID: 16
		private static List<ModuleInfo> results = null;
	}
}
