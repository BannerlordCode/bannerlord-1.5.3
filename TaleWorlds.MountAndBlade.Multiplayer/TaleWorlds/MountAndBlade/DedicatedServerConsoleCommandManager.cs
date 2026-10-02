using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000006 RID: 6
	public static class DedicatedServerConsoleCommandManager
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002416 File Offset: 0x00000616
		static DedicatedServerConsoleCommandManager()
		{
			DedicatedServerConsoleCommandManager.AddType(typeof(DedicatedServerConsoleCommandManager));
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002431 File Offset: 0x00000631
		public static void AddType(Type type)
		{
			DedicatedServerConsoleCommandManager._commandHandlerTypes.Add(type);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002440 File Offset: 0x00000640
		internal static void HandleConsoleCommand(string command)
		{
			int num = command.IndexOf(' ');
			string text = "";
			string text2;
			if (num > 0)
			{
				text2 = command.Substring(0, num);
				text = command.Substring(num + 1);
			}
			else
			{
				text2 = command;
			}
			bool flag = false;
			MultiplayerOptions.OptionType optionType;
			MultiplayerOptionsProperty multiplayerOptionsProperty;
			if (MultiplayerOptions.TryGetOptionTypeFromString(text2, out optionType, out multiplayerOptionsProperty))
			{
				if (text == "?")
				{
					Debug.Print(string.Concat(new object[] { "--", optionType, ": ", multiplayerOptionsProperty.Description }), 0, Debug.DebugColor.White, 17179869184UL);
					Debug.Print("--" + (multiplayerOptionsProperty.HasBounds ? string.Concat(new object[] { "Min: ", multiplayerOptionsProperty.BoundsMin, ", Max: ", multiplayerOptionsProperty.BoundsMax, ". " }) : "") + "Current value: " + optionType.GetValueText(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions), 0, Debug.DebugColor.White, 17179869184UL);
				}
				else if (text != "")
				{
					bool flag2 = true;
					string text3 = null;
					if (multiplayerOptionsProperty.OptionValueType == MultiplayerOptions.OptionValueType.String)
					{
						optionType.SetValue(text, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					}
					else if (multiplayerOptionsProperty.OptionValueType == MultiplayerOptions.OptionValueType.Integer || multiplayerOptionsProperty.OptionValueType == MultiplayerOptions.OptionValueType.Enum)
					{
						int num2;
						if (int.TryParse(text.Trim(), out num2))
						{
							optionType.SetValue(num2, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
						}
						else
						{
							flag2 = false;
							text3 = (multiplayerOptionsProperty.HasBounds ? string.Concat(new object[] { "a whole number between ", multiplayerOptionsProperty.BoundsMin, " and ", multiplayerOptionsProperty.BoundsMax }) : "a whole number");
						}
					}
					else if (multiplayerOptionsProperty.OptionValueType == MultiplayerOptions.OptionValueType.Bool)
					{
						bool flag3;
						if (bool.TryParse(text.Trim(), out flag3))
						{
							optionType.SetValue(flag3, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
						}
						else
						{
							flag2 = false;
							text3 = "True or False";
						}
					}
					else
					{
						flag2 = false;
						Debug.FailedAssert("No valid type found for multiplayer option.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\DedicatedServerConsoleCommandManager.cs", "HandleConsoleCommand", 91);
					}
					if (flag2)
					{
						Debug.Print(string.Concat(new object[]
						{
							"--Changed: ",
							optionType,
							", to: ",
							optionType.GetValueText(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
						}), 0, Debug.DebugColor.White, 17179869184UL);
					}
					else
					{
						string text4 = string.Concat(new object[]
						{
							"--Ignored: ",
							optionType,
							". Could not read '",
							text,
							"'",
							(text3 != null) ? ("; expected " + text3) : "",
							". Value left at: ",
							optionType.GetValueText(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
						});
						Debug.Print(text4, 0, Debug.DebugColor.Yellow, 17179869184UL);
						Console.WriteLine(text4);
					}
				}
				else
				{
					Debug.Print(string.Concat(new object[]
					{
						"--Value of: ",
						optionType,
						", is: ",
						optionType.GetValueText(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
					}), 0, Debug.DebugColor.White, 17179869184UL);
				}
				flag = true;
			}
			if (!flag)
			{
				foreach (Type type in DedicatedServerConsoleCommandManager._commandHandlerTypes)
				{
					foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
					{
						object[] customAttributesSafe = methodInfo.GetCustomAttributesSafe(false);
						for (int j = 0; j < customAttributesSafe.Length; j++)
						{
							ConsoleCommandMethod consoleCommandMethod = customAttributesSafe[j] as ConsoleCommandMethod;
							if (consoleCommandMethod != null && consoleCommandMethod.CommandName.Equals(text2))
							{
								if (text == "?")
								{
									Debug.Print("--" + consoleCommandMethod.CommandName + ": " + consoleCommandMethod.Description, 0, Debug.DebugColor.White, 17179869184UL);
								}
								else
								{
									List<object> list;
									if (!string.IsNullOrEmpty(text))
									{
										(list = new List<object>()).Add(text);
									}
									else
									{
										list = null;
									}
									List<object> list2 = list;
									methodInfo.Invoke(null, (list2 != null) ? list2.ToArray() : null);
								}
								flag = true;
							}
						}
					}
				}
			}
			if (!flag)
			{
				bool flag4;
				string text5 = CommandLineFunctionality.CallFunction(text2, text, out flag4);
				if (flag4)
				{
					Debug.Print(text5, 0, Debug.DebugColor.White, 17179869184UL);
					flag = true;
				}
			}
			if (!flag)
			{
				Debug.Print("--Invalid command is given.", 0, Debug.DebugColor.White, 17179869184UL);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000028B8 File Offset: 0x00000AB8
		[UsedImplicitly]
		[ConsoleCommandMethod("list", "Displays a list of all multiplayer options, their values and other possible commands")]
		private static void ListAllCommands()
		{
			Debug.Print("--List of all multiplayer command and their current values:", 0, Debug.DebugColor.White, 17179869184UL);
			for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				Debug.Print(string.Concat(new object[]
				{
					"----",
					optionType,
					": ",
					optionType.GetValueText(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
				}), 0, Debug.DebugColor.White, 17179869184UL);
			}
			Debug.Print("--List of additional commands:", 0, Debug.DebugColor.White, 17179869184UL);
			foreach (Type type in DedicatedServerConsoleCommandManager._commandHandlerTypes)
			{
				MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
				for (int i = 0; i < methods.Length; i++)
				{
					object[] customAttributesSafe = methods[i].GetCustomAttributesSafe(false);
					for (int j = 0; j < customAttributesSafe.Length; j++)
					{
						ConsoleCommandMethod consoleCommandMethod = customAttributesSafe[j] as ConsoleCommandMethod;
						if (consoleCommandMethod != null)
						{
							Debug.Print("----" + consoleCommandMethod.CommandName, 0, Debug.DebugColor.White, 17179869184UL);
						}
					}
				}
			}
			Debug.Print("--Add '?' after a command to get a more detailed description.", 0, Debug.DebugColor.White, 17179869184UL);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000029F8 File Offset: 0x00000BF8
		[UsedImplicitly]
		[ConsoleCommandMethod("set_winner_team", "Sets the winner team of flag domination based multiplayer missions.")]
		private static void SetWinnerTeam(string winnerTeamAsString)
		{
			MissionMultiplayerFlagDomination.SetWinnerTeam(int.Parse(winnerTeamAsString));
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002A05 File Offset: 0x00000C05
		[UsedImplicitly]
		[ConsoleCommandMethod("set_server_bandwidth_limit_in_mbps", "Overrides server's older bandwidth limit in megabit(s) per second.")]
		private static void SetServerBandwidthLimitInMbps(string bandwidthLimitAsString)
		{
			GameNetwork.SetServerBandwidthLimitInMbps(double.Parse(bandwidthLimitAsString));
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002A12 File Offset: 0x00000C12
		[UsedImplicitly]
		[ConsoleCommandMethod("set_server_tickrate", "Overrides server's older tickrate setting.")]
		private static void SetServerTickRate(string tickrateAsString)
		{
			double num = double.Parse(tickrateAsString);
			GameNetwork.SetServerTickRate(num);
			GameNetwork.SetServerFrameRate(num);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002A28 File Offset: 0x00000C28
		[UsedImplicitly]
		[ConsoleCommandMethod("stats", "Displays some game statistics, like FPS and players on the server.")]
		private static void ShowStats()
		{
			Debug.Print("--Current FPS: " + Utilities.GetFps(), 0, Debug.DebugColor.White, 17179869184UL);
			Debug.Print("--Active Players: " + GameNetwork.NetworkPeers.Count<NetworkCommunicator>((NetworkCommunicator x) => x.IsSynchronized), 0, Debug.DebugColor.White, 17179869184UL);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002AA3 File Offset: 0x00000CA3
		[UsedImplicitly]
		[ConsoleCommandMethod("open_monitor", "Opens up the monitor window with continuous data-representations on server performance and network usage.")]
		private static void OpenMonitor()
		{
			DebugNetworkEventStatistics.ControlActivate();
			DebugNetworkEventStatistics.OpenExternalMonitor();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002AAF File Offset: 0x00000CAF
		[UsedImplicitly]
		[ConsoleCommandMethod("crash_game", "Crashes the game process.")]
		private static void CrashGame()
		{
			Debug.Print("Crashing the process...", 0, Debug.DebugColor.White, 17179869184UL);
			throw new Exception("Game crashed by user command");
		}

		// Token: 0x04000001 RID: 1
		private static readonly List<Type> _commandHandlerTypes = new List<Type>();
	}
}
