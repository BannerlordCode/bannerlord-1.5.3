using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;
using TaleWorlds.SaveSystem.Load;
using TaleWorlds.SaveSystem.Save;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000022 RID: 34
	public static class SaveManager
	{
		// Token: 0x060000BE RID: 190 RVA: 0x00004BC8 File Offset: 0x00002DC8
		public static void InitializeGlobalDefinitionContext()
		{
			SaveManager._definitionContext = new DefinitionContext();
			SaveManager._definitionContext.FillWithCurrentTypes();
			foreach (string text in SaveManager._definitionContext.Errors)
			{
				Debug.Print(text, 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004C38 File Offset: 0x00002E38
		public static List<Type> CheckSaveableTypes()
		{
			List<Type> list = new List<Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				foreach (Type type in assemblies[i].GetTypesSafe(null))
				{
					PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
					{
						Attribute[] array = fieldInfo.GetCustomAttributesSafe(typeof(SaveableFieldAttribute)).ToArray<Attribute>();
						if (array.Length != 0)
						{
							SaveableFieldAttribute saveableFieldAttribute = (SaveableFieldAttribute)array[0];
							Type fieldType = fieldInfo.FieldType;
							if (!SaveManager._definitionContext.HasDefinition(fieldType) && !list.Contains(fieldType) && !fieldType.IsInterface && fieldType.FullName != null)
							{
								list.Add(fieldType);
							}
						}
					}
					foreach (PropertyInfo propertyInfo in properties)
					{
						Attribute[] array3 = propertyInfo.GetCustomAttributesSafe(typeof(SaveablePropertyAttribute)).ToArray<Attribute>();
						if (array3.Length != 0)
						{
							SaveablePropertyAttribute saveablePropertyAttribute = (SaveablePropertyAttribute)array3[0];
							Type propertyType = propertyInfo.PropertyType;
							if (!SaveManager._definitionContext.HasDefinition(propertyType) && !list.Contains(propertyType) && !propertyType.IsInterface && propertyType.FullName != null)
							{
								list.Add(propertyType);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004DD0 File Offset: 0x00002FD0
		public static SaveOutput Save(object target, MetaData metaData, string saveName, ISaveDriver driver)
		{
			SaveManager._isLoading = false;
			SaveManager.OperatingVersion = metaData.GetApplicationVersion();
			if (SaveManager._definitionContext == null)
			{
				SaveManager.InitializeGlobalDefinitionContext();
			}
			SaveOutput saveOutput = null;
			if (SaveManager._definitionContext.GotError)
			{
				List<SaveError> list = new List<SaveError>();
				foreach (string text in SaveManager._definitionContext.Errors)
				{
					list.Add(new SaveError(text));
				}
				saveOutput = SaveOutput.CreateFailed(list, SaveResult.GeneralFailure);
			}
			else
			{
				Debug.Print("------Saving with new context. Save name: " + saveName + "------", 0, Debug.DebugColor.White, 17592186044416UL);
				ISaveContext saveContext = new SaveContext(SaveManager._definitionContext);
				string text2;
				if (saveContext.Save(target, metaData, out text2))
				{
					try
					{
						Task<SaveResultWithMessage> task = driver.Save(saveName, 1, metaData, saveContext.SaveData);
						if (task.IsCompleted)
						{
							if (task.Result.SaveResult == SaveResult.Success)
							{
								saveOutput = SaveOutput.CreateSuccessful(saveContext.SaveData);
							}
							else
							{
								saveOutput = SaveOutput.CreateFailed(new SaveError[]
								{
									new SaveError(task.Result.Message)
								}, task.Result.SaveResult);
							}
						}
						else
						{
							saveOutput = SaveOutput.CreateContinuing(task);
						}
						goto IL_015B;
					}
					catch (Exception ex)
					{
						saveOutput = SaveOutput.CreateFailed(new SaveError[]
						{
							new SaveError(ex.Message)
						}, SaveResult.GeneralFailure);
						goto IL_015B;
					}
				}
				saveOutput = SaveOutput.CreateFailed(new SaveError[]
				{
					new SaveError(text2)
				}, SaveResult.GeneralFailure);
			}
			IL_015B:
			SaveManager.OperatingVersion = ApplicationVersion.Empty;
			return saveOutput;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004F60 File Offset: 0x00003160
		public static bool ShouldResolveConflicts()
		{
			return SaveManager._isLoading;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004F67 File Offset: 0x00003167
		public static MetaData LoadMetaData(string saveName, ISaveDriver driver)
		{
			return driver.LoadMetaData(saveName);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004F70 File Offset: 0x00003170
		public static LoadResult Load(string saveName, ISaveDriver driver)
		{
			return SaveManager.Load(saveName, driver, false);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004F7C File Offset: 0x0000317C
		public static LoadResult Load(string saveName, ISaveDriver driver, bool loadAsLateInitialize)
		{
			SaveManager._isLoading = true;
			DefinitionContext definitionContext = new DefinitionContext();
			definitionContext.FillWithCurrentTypes();
			LoadData loadData = driver.Load(saveName);
			SaveManager.OperatingVersion = loadData.MetaData.GetApplicationVersion();
			LoadContext loadContext = new LoadContext(definitionContext, driver);
			LoadResult loadResult;
			if (loadContext.Load(loadData, loadAsLateInitialize))
			{
				LoadCallbackInitializator loadCallbackInitializator = null;
				if (loadAsLateInitialize)
				{
					loadCallbackInitializator = loadContext.CreateLoadCallbackInitializator(loadData);
				}
				loadResult = LoadResult.CreateSuccessful(loadContext.RootObject, loadData.MetaData, loadCallbackInitializator);
			}
			else
			{
				loadResult = LoadResult.CreateFailed(new LoadError[]
				{
					new LoadError("Not implemented")
				});
			}
			SaveManager._isLoading = false;
			SaveManager.OperatingVersion = ApplicationVersion.Empty;
			return loadResult;
		}

		// Token: 0x0400004F RID: 79
		public const string SaveFileExtension = "sav";

		// Token: 0x04000050 RID: 80
		private const int CurrentVersion = 1;

		// Token: 0x04000051 RID: 81
		private static DefinitionContext _definitionContext;

		// Token: 0x04000052 RID: 82
		internal static ApplicationVersion OperatingVersion = ApplicationVersion.Empty;

		// Token: 0x04000053 RID: 83
		private static bool _isLoading = false;
	}
}
