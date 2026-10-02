using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000AE RID: 174
	public class SaveHandler
	{
		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060013D6 RID: 5078 RVA: 0x0005D7AD File Offset: 0x0005B9AD
		// (set) Token: 0x060013D7 RID: 5079 RVA: 0x0005D7B5 File Offset: 0x0005B9B5
		public IMainHeroVisualSupplier MainHeroVisualSupplier { get; set; }

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060013D8 RID: 5080 RVA: 0x0005D7BE File Offset: 0x0005B9BE
		public bool IsSaving
		{
			get
			{
				return !this.SaveArgsQueue.IsEmpty<SaveHandler.SaveArgs>();
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060013D9 RID: 5081 RVA: 0x0005D7CE File Offset: 0x0005B9CE
		public string IronmanModSaveName
		{
			get
			{
				return "Ironman" + Campaign.Current.UniqueGameId;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x0005D7E4 File Offset: 0x0005B9E4
		private bool _isAutoSaveEnabled
		{
			get
			{
				return this.AutoSaveInterval > -1;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060013DB RID: 5083 RVA: 0x0005D7EF File Offset: 0x0005B9EF
		private double _autoSavePriorityTimeLimit
		{
			get
			{
				return (double)this.AutoSaveInterval * 0.75;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060013DC RID: 5084 RVA: 0x0005D802 File Offset: 0x0005BA02
		public int AutoSaveInterval
		{
			get
			{
				ISaveManager sandBoxSaveManager = Campaign.Current.SandBoxManager.SandBoxSaveManager;
				if (sandBoxSaveManager == null)
				{
					return 15;
				}
				return sandBoxSaveManager.GetAutoSaveInterval();
			}
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0005D81F File Offset: 0x0005BA1F
		public void QuickSaveCurrentGame()
		{
			this.SetSaveArgs(SaveHandler.SaveArgs.SaveMode.QuickSave, null);
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0005D829 File Offset: 0x0005BA29
		public void SaveAs(string saveName)
		{
			this.SetSaveArgs(SaveHandler.SaveArgs.SaveMode.SaveAs, saveName);
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0005D834 File Offset: 0x0005BA34
		private void TryAutoSave(bool isPriority)
		{
			MapState mapState;
			if (this._isAutoSaveEnabled && (mapState = GameStateManager.Current.ActiveState as MapState) != null && !mapState.MapConversationActive)
			{
				double totalMinutes = (DateTime.Now - this._lastAutoSaveTime).TotalMinutes;
				double num = (isPriority ? this._autoSavePriorityTimeLimit : ((double)this.AutoSaveInterval));
				if (totalMinutes > num)
				{
					this.SetSaveArgs(SaveHandler.SaveArgs.SaveMode.AutoSave, null);
				}
			}
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0005D89A File Offset: 0x0005BA9A
		public void CampaignTick()
		{
			if (Campaign.Current.TimeControlMode != CampaignTimeControlMode.Stop)
			{
				this.TryAutoSave(false);
			}
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x0005D8B0 File Offset: 0x0005BAB0
		internal void SaveTick()
		{
			if (!this.SaveArgsQueue.IsEmpty<SaveHandler.SaveArgs>())
			{
				switch (this._saveStep)
				{
				case SaveHandler.SaveSteps.PreSave:
					this._saveStep++;
					this.OnSaveStarted();
					return;
				case SaveHandler.SaveSteps.Saving:
				{
					this._saveStep++;
					CampaignEventDispatcher.Instance.OnBeforeSave();
					if (CampaignOptions.IsIronmanMode)
					{
						MBSaveLoad.SaveAsCurrentGame(this.GetSaveMetaData(), this.IronmanModSaveName, new Action<ValueTuple<SaveResult, string>>(this.OnSaveCompleted));
						return;
					}
					SaveHandler.SaveArgs saveArgs = this.SaveArgsQueue.Peek();
					switch (saveArgs.Mode)
					{
					case SaveHandler.SaveArgs.SaveMode.SaveAs:
						MBSaveLoad.SaveAsCurrentGame(this.GetSaveMetaData(), saveArgs.Name, new Action<ValueTuple<SaveResult, string>>(this.OnSaveCompleted));
						return;
					case SaveHandler.SaveArgs.SaveMode.QuickSave:
						MBSaveLoad.QuickSaveCurrentGame(this.GetSaveMetaData(), new Action<ValueTuple<SaveResult, string>>(this.OnSaveCompleted));
						return;
					case SaveHandler.SaveArgs.SaveMode.AutoSave:
						MBSaveLoad.AutoSaveCurrentGame(this.GetSaveMetaData(), new Action<ValueTuple<SaveResult, string>>(this.OnSaveCompleted));
						return;
					default:
						return;
					}
					break;
				}
				case SaveHandler.SaveSteps.AwaitingCompletion:
					return;
				}
				this._saveStep++;
			}
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0005D9C3 File Offset: 0x0005BBC3
		private void OnSaveCompleted(ValueTuple<SaveResult, string> result)
		{
			this._saveStep = SaveHandler.SaveSteps.PreSave;
			if (this.SaveArgsQueue.Dequeue().Mode == SaveHandler.SaveArgs.SaveMode.AutoSave)
			{
				this._lastAutoSaveTime = DateTime.Now;
			}
			this.OnSaveEnded(result.Item1 == SaveResult.Success, result.Item2);
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0005D9FF File Offset: 0x0005BBFF
		public void SignalAutoSave()
		{
			this.TryAutoSave(true);
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0005DA08 File Offset: 0x0005BC08
		private void OnSaveStarted()
		{
			Campaign.Current.WaitAsyncTasks();
			CampaignEventDispatcher.Instance.OnSaveStarted();
			MBInformationManager.HideInformations();
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0005DA24 File Offset: 0x0005BC24
		private void OnSaveEnded(bool isSaveSuccessful, string newSaveGameName)
		{
			ISaveManager sandBoxSaveManager = Campaign.Current.SandBoxManager.SandBoxSaveManager;
			if (sandBoxSaveManager != null)
			{
				sandBoxSaveManager.OnSaveOver(isSaveSuccessful, newSaveGameName);
			}
			CampaignEventDispatcher.Instance.OnSaveOver(isSaveSuccessful, newSaveGameName);
			if (!isSaveSuccessful)
			{
				MBInformationManager.AddQuickInformation(new TextObject("{=u9PPxTNL}Save Error!", null), 0, null, null, "");
			}
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0005DA74 File Offset: 0x0005BC74
		private void SetSaveArgs(SaveHandler.SaveArgs.SaveMode saveType, string saveName = null)
		{
			this.SaveArgsQueue.Enqueue(new SaveHandler.SaveArgs(saveType, saveName));
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0005DA88 File Offset: 0x0005BC88
		public void ForceAutoSave()
		{
			if (!Campaign.Current.SandBoxManager.SandBoxSaveManager.IsAutoSaveDisabled())
			{
				this.SetSaveArgs(SaveHandler.SaveArgs.SaveMode.AutoSave, null);
			}
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0005DAA8 File Offset: 0x0005BCA8
		public CampaignSaveMetaDataArgs GetSaveMetaData()
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			list.Add(new KeyValuePair<string, string>("UniqueGameId", Campaign.Current.UniqueGameId ?? ""));
			list.Add(new KeyValuePair<string, string>("MainHeroLevel", Hero.MainHero.Level.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("MainPartyFood", Campaign.Current.MainParty.Food.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("MainHeroGold", Hero.MainHero.Gold.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("ClanInfluence", Clan.PlayerClan.Influence.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("ClanFiefs", Clan.PlayerClan.Settlements.Count.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("MainPartyShipCount", Campaign.Current.MainParty.Ships.Count.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("MainPartyHealthyMemberCount", Campaign.Current.MainParty.MemberRoster.TotalHealthyCount.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("MainPartyPrisonerMemberCount", Campaign.Current.MainParty.PrisonRoster.TotalManCount.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("MainPartyWoundedMemberCount", Campaign.Current.MainParty.MemberRoster.TotalWounded.ToString(SaveHandler._invariantCulture)));
			string text = "CharacterName";
			TextObject name = Hero.MainHero.Name;
			list.Add(new KeyValuePair<string, string>(text, (name != null) ? name.ToString() : null));
			list.Add(new KeyValuePair<string, string>("DayLong", Campaign.Current.Models.CampaignTimeModel.CampaignStartTime.ElapsedDaysUntilNow.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("ClanBannerCode", Clan.PlayerClan.Banner.Serialize()));
			string text2 = "MainHeroVisual";
			IMainHeroVisualSupplier mainHeroVisualSupplier = this.MainHeroVisualSupplier;
			list.Add(new KeyValuePair<string, string>(text2, ((mainHeroVisualSupplier != null) ? mainHeroVisualSupplier.GetMainHeroVisualCode() : null) ?? string.Empty));
			list.Add(new KeyValuePair<string, string>("IronmanMode", (CampaignOptions.IsIronmanMode ? 1 : 0).ToString()));
			list.Add(new KeyValuePair<string, string>("HealthPercentage", MBMath.ClampInt(Hero.MainHero.HitPoints * 100 / Hero.MainHero.MaxHitPoints, 1, 100).ToString()));
			list.Add(new KeyValuePair<string, string>("NewGameVersion", string.IsNullOrEmpty(Campaign.Current.NewGameVersion) ? string.Empty : Campaign.Current.NewGameVersion));
			List<KeyValuePair<string, string>> list2 = list;
			CampaignEventDispatcher.Instance.CollectMetadataEntries(list2);
			return new CampaignSaveMetaDataArgs((from x in ModuleHelper.GetActiveModules()
				select x.Id).ToArray<string>(), list2.ToArray());
		}

		// Token: 0x04000659 RID: 1625
		private SaveHandler.SaveSteps _saveStep;

		// Token: 0x0400065A RID: 1626
		private static readonly CultureInfo _invariantCulture = CultureInfo.InvariantCulture;

		// Token: 0x0400065C RID: 1628
		private Queue<SaveHandler.SaveArgs> SaveArgsQueue = new Queue<SaveHandler.SaveArgs>();

		// Token: 0x0400065D RID: 1629
		private DateTime _lastAutoSaveTime = DateTime.Now;

		// Token: 0x02000572 RID: 1394
		private readonly struct SaveArgs
		{
			// Token: 0x06005053 RID: 20563 RVA: 0x0018EC03 File Offset: 0x0018CE03
			public SaveArgs(SaveHandler.SaveArgs.SaveMode mode, string name)
			{
				this.Mode = mode;
				this.Name = name;
			}

			// Token: 0x040017C7 RID: 6087
			public readonly SaveHandler.SaveArgs.SaveMode Mode;

			// Token: 0x040017C8 RID: 6088
			public readonly string Name;

			// Token: 0x02000900 RID: 2304
			public enum SaveMode
			{
				// Token: 0x040026DF RID: 9951
				SaveAs,
				// Token: 0x040026E0 RID: 9952
				QuickSave,
				// Token: 0x040026E1 RID: 9953
				AutoSave
			}
		}

		// Token: 0x02000573 RID: 1395
		private enum SaveSteps
		{
			// Token: 0x040017CA RID: 6090
			PreSave,
			// Token: 0x040017CB RID: 6091
			Saving = 2,
			// Token: 0x040017CC RID: 6092
			AwaitingCompletion
		}
	}
}
