using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000012 RID: 18
	public static class CrashInformationCollector
	{
		// Token: 0x06000096 RID: 150 RVA: 0x000036B4 File Offset: 0x000018B4
		[EngineCallback(null, false)]
		public static string CollectInformation()
		{
			List<CrashInformationCollector.CrashInformation> list = new List<CrashInformationCollector.CrashInformation>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				try
				{
					Type[] types = assembly.GetTypes();
					for (int j = 0; j < types.Length; j++)
					{
						foreach (MethodInfo methodInfo in types[j].GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
						{
							object[] customAttributesSafe = methodInfo.GetCustomAttributesSafe(typeof(CrashInformationCollector.CrashInformationProvider), false);
							if (customAttributesSafe != null && customAttributesSafe.Length != 0 && customAttributesSafe[0] is CrashInformationCollector.CrashInformationProvider)
							{
								CrashInformationCollector.CrashInformation crashInformation = methodInfo.Invoke(null, new object[0]) as CrashInformationCollector.CrashInformation;
								if (crashInformation != null)
								{
									list.Add(crashInformation);
								}
							}
						}
					}
				}
				catch (ReflectionTypeLoadException ex)
				{
					foreach (Exception ex2 in ex.LoaderExceptions)
					{
						MBDebug.Print("Unable to load types from assembly: " + ex2.Message, 0, Debug.DebugColor.White, 17592186044416UL);
					}
				}
				catch (Exception ex3)
				{
					MBDebug.Print("Exception while collecting crash information : " + ex3.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				}
			}
			string text = "";
			foreach (CrashInformationCollector.CrashInformation crashInformation2 in list)
			{
				foreach (ValueTuple<string, string> valueTuple in crashInformation2.Lines)
				{
					text = string.Concat(new string[] { text, "[", crashInformation2.Id, "][", valueTuple.Item1, "][", valueTuple.Item2, "]\n" });
				}
			}
			return text;
		}

		// Token: 0x020000B3 RID: 179
		public class CrashInformation
		{
			// Token: 0x06000FE1 RID: 4065 RVA: 0x00014052 File Offset: 0x00012252
			public CrashInformation(string id, MBReadOnlyList<ValueTuple<string, string>> lines)
			{
				this.Id = id;
				this.Lines = lines;
			}

			// Token: 0x0400035C RID: 860
			public readonly string Id;

			// Token: 0x0400035D RID: 861
			public readonly MBReadOnlyList<ValueTuple<string, string>> Lines;
		}

		// Token: 0x020000B4 RID: 180
		[AttributeUsage(AttributeTargets.Method)]
		public class CrashInformationProvider : Attribute
		{
		}
	}
}
