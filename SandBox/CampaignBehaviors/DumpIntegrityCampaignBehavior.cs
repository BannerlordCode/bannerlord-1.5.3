using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AdvancedStartOptions;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000DC RID: 220
	public class DumpIntegrityCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000A21 RID: 2593 RVA: 0x0004BDE8 File Offset: 0x00049FE8
		public override void SyncData(IDataStore dataStore)
		{
			TextObject textObject;
			DumpIntegrityCampaignBehavior.IsGameIntegrityAchieved(out textObject);
			this.UpdateDumpInfo();
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0004BE03 File Offset: 0x0004A003
		public override void RegisterEvents()
		{
			CampaignEvents.OnConfigChangedEvent.AddNonSerializedListener(this, new Action(this.OnConfigChanged));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0004BE34 File Offset: 0x0004A034
		private void OnConfigChanged()
		{
			TextObject textObject;
			DumpIntegrityCampaignBehavior.IsGameIntegrityAchieved(out textObject);
			this.UpdateDumpInfo();
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0004BE50 File Offset: 0x0004A050
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter campaignGameStarter)
		{
			TextObject textObject;
			DumpIntegrityCampaignBehavior.IsGameIntegrityAchieved(out textObject);
			this.UpdateDumpInfo();
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0004BE6C File Offset: 0x0004A06C
		private void UpdateDumpInfo()
		{
			this._saveIntegrityDumpInfo.Clear();
			this._usedModulesDumpInfo.Clear();
			this._usedVersionsDumpInfo.Clear();
			this._usedAdvancedStartOptionsDumpInfo.Clear();
			if (Campaign.Current.NewGameVersion != null)
			{
				this._saveIntegrityDumpInfo.Add(new KeyValuePair<string, string>("New Game Version", Campaign.Current.NewGameVersion));
			}
			this._saveIntegrityDumpInfo.Add(new KeyValuePair<string, string>("Has Used Cheats", (!DumpIntegrityCampaignBehavior.CheckCheatUsage()).ToString()));
			this._saveIntegrityDumpInfo.Add(new KeyValuePair<string, string>("Is Advanced Start Options Used", DumpIntegrityCampaignBehavior.IsAdvancedStartOptionsUsed(this._usedAdvancedStartOptionsDumpInfo).ToString()));
			Campaign campaign = Campaign.Current;
			if (((campaign != null) ? campaign.PreviouslyUsedModules : null) != null && Campaign.Current.UsedGameVersions != null)
			{
				string text;
				if (DumpIntegrityCampaignBehavior.CheckIfModulesAreDefault(out text))
				{
					this._saveIntegrityDumpInfo.Add(new KeyValuePair<string, string>("Has Installed Unofficial Modules", "False"));
				}
				else
				{
					this._saveIntegrityDumpInfo.Add(new KeyValuePair<string, string>("Has Installed Unofficial Modules", text));
				}
				string text2;
				DumpIntegrityCampaignBehavior.CheckIfVersionIntegrityIsAchieved(out text2);
				this._saveIntegrityDumpInfo.Add(new KeyValuePair<string, string>("Has Reverted to Older Versions", text2));
				TextObject textObject;
				this._saveIntegrityDumpInfo.Add(new KeyValuePair<string, string>("Game Integrity is Achieved", DumpIntegrityCampaignBehavior.IsGameIntegrityAchieved(out textObject).ToString()));
			}
			Campaign campaign2 = Campaign.Current;
			if (((campaign2 != null) ? campaign2.PreviouslyUsedModules : null) != null && Campaign.Current.PreviouslyUsedModules.Count > 0)
			{
				foreach (string text3 in Campaign.Current.PreviouslyUsedModules.Last<string>().Split(new char[] { MBSaveLoad.ModuleCodeSeperator }))
				{
					string text4 = text3.Split(new char[] { MBSaveLoad.ModuleVersionSeperator })[0];
					string text5 = text3.Split(new char[] { MBSaveLoad.ModuleVersionSeperator })[1];
					ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo(text4);
					string text6 = "Does not exist";
					if (moduleInfo != null)
					{
						text6 = "Exists But Inactive";
						if (moduleInfo.IsActive)
						{
							text6 = "Exists and Active";
						}
					}
					text6 = text6 + " (Version: " + text5 + ")";
					this._usedModulesDumpInfo.Add(new KeyValuePair<string, string>(text4, text6));
				}
			}
			if (Campaign.Current.UsedGameVersions != null)
			{
				foreach (string text7 in Campaign.Current.UsedGameVersions)
				{
					string text8 = ((text7 == MBSaveLoad.CurrentVersion.ToString()) ? "1" : "0");
					this._usedVersionsDumpInfo.Add(new KeyValuePair<string, string>(text7, text8));
				}
			}
			this.SendDataToWatchdog();
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0004C148 File Offset: 0x0004A348
		private void SendDataToWatchdog()
		{
			foreach (KeyValuePair<string, string> keyValuePair in this._saveIntegrityDumpInfo)
			{
				Utilities.SetWatchdogValue("crash_tags.txt", "Campaign Dump Integrity", keyValuePair.Key, keyValuePair.Value);
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in this._usedModulesDumpInfo)
			{
				Utilities.SetWatchdogValue("crash_tags.txt", "Used Modules", keyValuePair2.Key, keyValuePair2.Value);
			}
			foreach (KeyValuePair<string, string> keyValuePair3 in this._usedVersionsDumpInfo)
			{
				Utilities.SetWatchdogValue("crash_tags.txt", "Used Versions", keyValuePair3.Key, keyValuePair3.Value);
			}
			foreach (KeyValuePair<string, string> keyValuePair4 in this._usedAdvancedStartOptionsDumpInfo)
			{
				Utilities.SetWatchdogValue("crash_tags.txt", "Used Advanced Start Options", keyValuePair4.Key, keyValuePair4.Value);
			}
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0004C2BC File Offset: 0x0004A4BC
		public static bool IsGameIntegrityAchieved(out TextObject reason)
		{
			bool flag = true;
			string text;
			if (!DumpIntegrityCampaignBehavior.CheckCheatUsage())
			{
				reason = new TextObject("{=sO8Zh3ZH}Achievements are disabled due to cheat usage.", null);
				flag = false;
			}
			else if (!DumpIntegrityCampaignBehavior.CheckIfModulesAreDefault(out text))
			{
				reason = new TextObject("{=R0AbAxqX}Achievements are disabled due to unofficial modules.", null);
				flag = false;
			}
			else if (!DumpIntegrityCampaignBehavior.CheckIfVersionIntegrityIsAchieved(out text))
			{
				reason = new TextObject("{=dt00CQCM}Achievements are disabled due to version downgrade.", null);
				flag = false;
			}
			else if (DumpIntegrityCampaignBehavior.IsAdvancedStartOptionsUsed(new List<KeyValuePair<string, string>>()))
			{
				reason = new TextObject("{=ARfa9HpP}Achievements are disabled due to advanced startup options.", null);
				flag = false;
			}
			else
			{
				reason = null;
			}
			return flag;
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0004C338 File Offset: 0x0004A538
		private static bool IsAdvancedStartOptionsUsed(List<KeyValuePair<string, string>> usedOptions)
		{
			AdvancedStartOptionsData advancedStartData = Campaign.Current.AdvancedStartData;
			bool flag = false;
			if (advancedStartData != null)
			{
				foreach (AdvancedStartData advancedStartData2 in advancedStartData.GetOptions())
				{
					if (advancedStartData2.StringId != "Seed")
					{
						flag = true;
					}
					usedOptions.Add(new KeyValuePair<string, string>(advancedStartData2.StringId, advancedStartData2.GetData().ToString()));
				}
			}
			return flag;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0004C3C0 File Offset: 0x0004A5C0
		private static bool CheckIfVersionIntegrityIsAchieved(out string message)
		{
			message = "False";
			MBReadOnlyList<string> usedGameVersions = Campaign.Current.UsedGameVersions;
			for (int i = 0; i < usedGameVersions.Count; i++)
			{
				if (i < usedGameVersions.Count - 1 && ApplicationVersion.FromString(usedGameVersions[i], 0).IsNewerThan(ApplicationVersion.FromString(usedGameVersions[i + 1], 0)))
				{
					message = "Version downgrade from " + usedGameVersions[i + 1] + " to " + usedGameVersions[i];
					Debug.Print("Dump integrity is compromised due to version downgrade from " + usedGameVersions[i + 1] + " to " + usedGameVersions[i], 0, Debug.DebugColor.DarkRed, 17592186044416UL);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0004C47C File Offset: 0x0004A67C
		private static bool CheckIfModulesAreDefault(out string unofficialModulesCode)
		{
			MBList<string> officialModuleIds = ModuleHelper.GetOfficialModuleIds();
			MBList<string> mblist = new MBList<string>();
			foreach (string text in Campaign.Current.PreviouslyUsedModules)
			{
				string[] array = text.Split(new char[] { MBSaveLoad.ModuleCodeSeperator });
				for (int i = 0; i < array.Length; i++)
				{
					string text2 = array[i];
					string moduleId = text2.Split(new char[] { MBSaveLoad.ModuleVersionSeperator })[0];
					if (!officialModuleIds.Any<string>((string x) => moduleId.Equals(x, StringComparison.InvariantCultureIgnoreCase)) && !mblist.Contains(moduleId))
					{
						mblist.Add(moduleId);
					}
				}
			}
			unofficialModulesCode = string.Join(MBSaveLoad.ModuleCodeSeperator.ToString(), mblist);
			if (mblist.Count > 0)
			{
				Debug.Print("Unofficial modules are used: " + unofficialModulesCode, 0, Debug.DebugColor.DarkRed, 17592186044416UL);
			}
			return mblist.Count == 0;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0004C5A4 File Offset: 0x0004A7A4
		private static bool CheckCheatUsage()
		{
			if (!Campaign.Current.EnabledCheatsBefore && Game.Current.CheatMode)
			{
				Campaign.Current.EnabledCheatsBefore = Game.Current.CheatMode;
			}
			if (Campaign.Current.EnabledCheatsBefore)
			{
				Debug.Print("Dump integrity is compromised due to cheat usage", 0, Debug.DebugColor.DarkRed, 17592186044416UL);
			}
			return !Campaign.Current.EnabledCheatsBefore;
		}

		// Token: 0x0400049B RID: 1179
		private readonly List<KeyValuePair<string, string>> _saveIntegrityDumpInfo = new List<KeyValuePair<string, string>>();

		// Token: 0x0400049C RID: 1180
		private readonly List<KeyValuePair<string, string>> _usedModulesDumpInfo = new List<KeyValuePair<string, string>>();

		// Token: 0x0400049D RID: 1181
		private readonly List<KeyValuePair<string, string>> _usedVersionsDumpInfo = new List<KeyValuePair<string, string>>();

		// Token: 0x0400049E RID: 1182
		private readonly List<KeyValuePair<string, string>> _usedAdvancedStartOptionsDumpInfo = new List<KeyValuePair<string, string>>();
	}
}
