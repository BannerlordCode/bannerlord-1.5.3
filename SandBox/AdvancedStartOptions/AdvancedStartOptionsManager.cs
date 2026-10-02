using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace SandBox.AdvancedStartOptions
{
	// Token: 0x02000113 RID: 275
	public static class AdvancedStartOptionsManager
	{
		// Token: 0x06000DA5 RID: 3493 RVA: 0x000625D8 File Offset: 0x000607D8
		private static void Initialize()
		{
			AdvancedStartOptionsManager._providers.Clear();
			MBList<Assembly> activeGameAssemblies = ModuleHelper.GetActiveGameAssemblies();
			for (int i = 0; i < activeGameAssemblies.Count; i++)
			{
				List<Type> typesSafe = activeGameAssemblies[i].GetTypesSafe(null);
				for (int j = 0; j < typesSafe.Count; j++)
				{
					Type type = typesSafe[j];
					if (!(type == null))
					{
						foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
						{
							object[] customAttributesSafe = methodInfo.GetCustomAttributesSafe(typeof(StartOptionsProviderAttribute), false);
							if (customAttributesSafe != null && customAttributesSafe.Length != 0)
							{
								try
								{
									AdvancedStartOptionsManager.StartOptionsProviderDelegate startOptionsProviderDelegate = Delegate.CreateDelegate(typeof(AdvancedStartOptionsManager.StartOptionsProviderDelegate), methodInfo) as AdvancedStartOptionsManager.StartOptionsProviderDelegate;
									if (startOptionsProviderDelegate != null)
									{
										AdvancedStartOptionsManager._providers.Add(startOptionsProviderDelegate);
									}
									else
									{
										Debug.FailedAssert(string.Concat(new string[] { "Start options provider ", type.Name, ".", methodInfo.Name, " does not match the expected signature 'static void (CampaignStartOptions)' and will be ignored" }), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\AdvancedStartOptions\\AdvancedStartOptionsManager.cs", "Initialize", 50);
									}
								}
								catch (Exception ex)
								{
									Debug.FailedAssert(string.Concat(new string[] { "Error when creating start options provider ", type.Name, ".", methodInfo.Name, ": ", ex.Message }), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\AdvancedStartOptions\\AdvancedStartOptionsManager.cs", "Initialize", 55);
									Debug.Print("Error when creating start options provider: " + ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00062794 File Offset: 0x00060994
		public static AdvancedStartOptions CreateCampaignStartOptions()
		{
			AdvancedStartOptionsManager.Initialize();
			AdvancedStartOptions advancedStartOptions = new AdvancedStartOptions();
			for (int i = 0; i < AdvancedStartOptionsManager._providers.Count; i++)
			{
				AdvancedStartOptionsManager._providers[i](advancedStartOptions);
			}
			return advancedStartOptions;
		}

		// Token: 0x040005CD RID: 1485
		private static readonly List<AdvancedStartOptionsManager.StartOptionsProviderDelegate> _providers = new List<AdvancedStartOptionsManager.StartOptionsProviderDelegate>();

		// Token: 0x0200023E RID: 574
		// (Invoke) Token: 0x06001483 RID: 5251
		private delegate void StartOptionsProviderDelegate(AdvancedStartOptions options);
	}
}
