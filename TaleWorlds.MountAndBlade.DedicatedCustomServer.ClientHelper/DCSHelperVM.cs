using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.DedicatedCustomServer.ClientHelper
{
	// Token: 0x02000003 RID: 3
	public class DCSHelperVM : ViewModel
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002368 File Offset: 0x00000568
		public IEnumerable<DCSHelperMapItemVM> SelectedMaps
		{
			get
			{
				return this.MapList.Where<DCSHelperMapItemVM>((DCSHelperMapItemVM map) => map.IsSelected);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002394 File Offset: 0x00000594
		public IEnumerable<DCSHelperMapItemVM> SelectableMaps
		{
			get
			{
				return this.MapList.Where<DCSHelperMapItemVM>((DCSHelperMapItemVM map) => !map.IsSelected);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000023C0 File Offset: 0x000005C0
		public DCSHelperVM(string hostAddress, string fullName = null)
		{
			this._hostAddress = hostAddress;
			this._fullName = fullName;
			this._texts = new DCSHelperVM.Texts();
			this.IsDownloading = false;
			this.ShowProgress = false;
			this.PanelTitleText = this._texts.DownloadPanel;
			this.DownloadButtonText = this._texts.Download;
			this.CloseButtonText = this._texts.Close;
			this.HostAddressText = this._texts.GetPanelSubtitle(this.Truncate(this._fullName, 40) ?? this._hostAddress);
			this.ToggleSelectionButtonText = this._texts.SelectAll;
			this.MapList = new MBBindingList<DCSHelperMapItemVM>();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002472 File Offset: 0x00000672
		public DCSHelperVM(GameServerEntry server)
			: this(string.Format("{0}:{1}", server.Address, server.Port), server.ServerName)
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000249C File Offset: 0x0000069C
		private string Truncate(string str, int maxLength = 40)
		{
			if (str == null)
			{
				return str;
			}
			string text = str.Trim();
			if (text.Length > maxLength)
			{
				return text.Substring(0, maxLength).TrimEnd(Array.Empty<char>()) + "...";
			}
			return text;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000024DC File Offset: 0x000006DC
		private async Task RefreshMapList()
		{
			this.MapList.Clear();
			MapListResponse mapListResponse = await DedicatedCustomServerClientHelperSubModule.Instance.GetMapListFromHost(this._hostAddress);
			foreach (MapListItemResponse mapListItemResponse in mapListResponse.Maps)
			{
				UniqueSceneId uniqueSceneId = ((mapListItemResponse.UniqueToken != null && mapListItemResponse.Revision != null) ? new UniqueSceneId(mapListItemResponse.UniqueToken, mapListItemResponse.Revision) : null);
				DCSHelperMapItemVM dcshelperMapItemVM = new DCSHelperMapItemVM(mapListItemResponse.Name, delegate(DCSHelperMapItemVM map)
				{
					this.OnMapSelected(map, false);
				}, mapListItemResponse.Name == mapListResponse.CurrentlyPlaying, uniqueSceneId);
				dcshelperMapItemVM.RefreshLocalMapData();
				this.MapList.Add(dcshelperMapItemVM);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002524 File Offset: 0x00000724
		private void OnMapSelected(DCSHelperMapItemVM mapItem, bool forceSelection = false)
		{
			if (!this.IsDownloading || forceSelection)
			{
				bool readyToDownload = this.ReadyToDownload;
				mapItem.IsSelected = !mapItem.IsSelected;
				this.ToggleSelectionButtonText = (this.SelectedMaps.Any<DCSHelperMapItemVM>() ? this._texts.UnselectAll : this._texts.SelectAll);
				if (readyToDownload != this.ReadyToDownload)
				{
					base.OnPropertyChangedWithValue(this.ReadyToDownload, "ReadyToDownload");
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002598 File Offset: 0x00000798
		private void ToggleSelection()
		{
			IEnumerable<DCSHelperMapItemVM> enumerable = this.SelectedMaps;
			if (!enumerable.Any<DCSHelperMapItemVM>())
			{
				enumerable = this.SelectableMaps;
			}
			foreach (DCSHelperMapItemVM dcshelperMapItemVM in enumerable)
			{
				dcshelperMapItemVM.ExecuteToggleSelection();
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000025F4 File Offset: 0x000007F4
		public async Task OpenPopup()
		{
			this._gauntletLayer = new GauntletLayer("DCSHelper", 20, false);
			this._gauntletLayer.LoadMovie("DCSHelper", this);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TopScreen.AddLayer(this._gauntletLayer);
			ScreenManager.TrySetFocus(this._gauntletLayer);
			this.IsLoading = true;
			try
			{
				await this.RefreshMapList();
				this.IsLoading = false;
			}
			catch (Exception ex)
			{
				this.ShowFailedToRetrieveInquiry(ex.Message);
				this.ExecuteClosePopup();
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000263C File Offset: 0x0000083C
		public async Task ExecuteDownloadMap()
		{
			Queue<DCSHelperMapItemVM> remainingMaps = new Queue<DCSHelperMapItemVM>(this.SelectedMaps);
			int totalMapCount = remainingMaps.Count;
			if (totalMapCount != 0)
			{
				List<DCSHelperMapItemVM> downloadedMaps = new List<DCSHelperMapItemVM>();
				this.IsDownloading = true;
				this.ShowProgress = false;
				this.DownloadButtonText = this._texts.Downloading;
				this.CloseButtonText = this._texts.Cancel;
				while (remainingMaps.Any<DCSHelperMapItemVM>())
				{
					DCSHelperVM.<>c__DisplayClass17_0 CS$<>8__locals1 = new DCSHelperVM.<>c__DisplayClass17_0();
					CS$<>8__locals1.<>4__this = this;
					CS$<>8__locals1.mapItem = remainingMaps.Dequeue();
					ModLogger.Log(string.Concat(new string[]
					{
						"Download Panel: Downloading map '",
						CS$<>8__locals1.mapItem.MapName,
						"' from host '",
						this._hostAddress,
						"'"
					}), 0, Debug.DebugColor.Green);
					this.ProgressCounterText = this._texts.GetProgressCounter(downloadedMaps.Count + 1, totalMapCount);
					try
					{
						bool flag = !ModHelpers.DoesSceneFolderAlreadyExist(CS$<>8__locals1.mapItem.MapName);
						if (!flag)
						{
							flag = await this.WaitForConfirmationToReplace(CS$<>8__locals1.mapItem.MapName);
						}
						if (flag)
						{
							CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
							this._cancellationTokenSource = cancellationTokenSource;
							using (cancellationTokenSource)
							{
								await Task.Run(delegate
								{
									DCSHelperVM.<>c__DisplayClass17_0.<<ExecuteDownloadMap>b__0>d <<ExecuteDownloadMap>b__0>d;
									<<ExecuteDownloadMap>b__0>d.<>4__this = CS$<>8__locals1;
									<<ExecuteDownloadMap>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
									<<ExecuteDownloadMap>b__0>d.<>1__state = -1;
									AsyncTaskMethodBuilder <>t__builder = <<ExecuteDownloadMap>b__0>d.<>t__builder;
									<>t__builder.Start<DCSHelperVM.<>c__DisplayClass17_0.<<ExecuteDownloadMap>b__0>d>(ref <<ExecuteDownloadMap>b__0>d);
									return <<ExecuteDownloadMap>b__0>d.<>t__builder.Task;
								});
							}
							CancellationTokenSource cancellationTokenSource2 = null;
							downloadedMaps.Add(CS$<>8__locals1.mapItem);
							this.OnMapSelected(CS$<>8__locals1.mapItem, true);
							CS$<>8__locals1.mapItem.RefreshLocalMapData();
						}
						if (!remainingMaps.Any<DCSHelperMapItemVM>())
						{
							this.ShowDownloadCompleteInquiry(downloadedMaps);
						}
					}
					catch (Exception ex)
					{
						remainingMaps.Clear();
						CancellationTokenSource cancellationTokenSource3 = this._cancellationTokenSource;
						if (cancellationTokenSource3 != null && !cancellationTokenSource3.IsCancellationRequested)
						{
							this.ShowDownloadFailedInquiry(ex.Message);
						}
					}
					this._cancellationTokenSource = null;
					CS$<>8__locals1 = null;
				}
				this.IsDownloading = false;
				this.DownloadButtonText = this._texts.Download;
				this.CloseButtonText = this._texts.Close;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002684 File Offset: 0x00000884
		private Task<bool> WaitForConfirmationToReplace(string mapName)
		{
			DCSHelperVM.<>c__DisplayClass18_0 CS$<>8__locals1 = new DCSHelperVM.<>c__DisplayClass18_0();
			CS$<>8__locals1.taskSource = new TaskCompletionSource<bool>();
			InformationManager.ShowInquiry(new InquiryData(this._texts.DownloadPanel, this._texts.GetReplacementConfirmationMessage(mapName), true, true, this._texts.Yes, this._texts.No, CS$<>8__locals1.<WaitForConfirmationToReplace>g__getAction|0(true), CS$<>8__locals1.<WaitForConfirmationToReplace>g__getAction|0(false), "", 0f, null, null, null), false, false);
			return CS$<>8__locals1.taskSource.Task;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002704 File Offset: 0x00000904
		private void ShowDownloadCompleteInquiry(List<DCSHelperMapItemVM> downloadedMaps)
		{
			InformationManager.ShowInquiry(new InquiryData(this._texts.DownloadComplete, (downloadedMaps.Count == 1) ? this._texts.GetDownloadCompleteMessageSingular(downloadedMaps.Single<DCSHelperMapItemVM>().MapName) : this._texts.GetDownloadCompleteMessagePlural(downloadedMaps.Count), true, false, this._texts.Dismiss, "", null, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000277C File Offset: 0x0000097C
		private void ShowDownloadFailedInquiry(string reason)
		{
			InformationManager.ShowInquiry(new InquiryData(this._texts.DownloadFailed, reason, false, true, "", this._texts.Dismiss, null, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000027C4 File Offset: 0x000009C4
		private void ShowFailedToRetrieveInquiry(string reason)
		{
			InformationManager.ShowInquiry(new InquiryData(this._texts.DownloadPanel, reason, false, true, "", this._texts.Dismiss, null, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000280A File Offset: 0x00000A0A
		public void ExecuteCloseOrCancel()
		{
			if (this.IsDownloading)
			{
				this.ExecuteCancelDownload();
				return;
			}
			this.ExecuteClosePopup();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002821 File Offset: 0x00000A21
		public void ExecuteClosePopup()
		{
			ScreenManager.TryLoseFocus(this._gauntletLayer);
			ScreenManager.TopScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002848 File Offset: 0x00000A48
		public void ExecuteCancelDownload()
		{
			if (this._cancellationTokenSource != null && !this._cancellationTokenSource.IsCancellationRequested)
			{
				try
				{
					this._cancellationTokenSource.Cancel();
				}
				catch (Exception ex)
				{
					ModLogger.Warn("Failed to cancel download: " + ex.Message);
				}
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000028A0 File Offset: 0x00000AA0
		private void OnProgressUpdate(ProgressUpdate update)
		{
			this.ProgressText = string.Concat(new string[]
			{
				update.MegaBytesRead.ToString("0.##"),
				" MB / ",
				update.TotalMegaBytes.ToString("0.##"),
				" MB (",
				(update.ProgressRatio * 100f).ToString("0.##"),
				"%)"
			});
			this.DownloadRatio = update.ProgressRatio;
			this.ShowProgress = true;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002931 File Offset: 0x00000B31
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002939 File Offset: 0x00000B39
		[DataSourceProperty]
		public bool IsLoading
		{
			get
			{
				return this._isLoading;
			}
			set
			{
				if (value != this._isLoading)
				{
					this._isLoading = value;
					base.OnPropertyChangedWithValue(value, "IsLoading");
				}
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002957 File Offset: 0x00000B57
		// (set) Token: 0x0600002C RID: 44 RVA: 0x0000295F File Offset: 0x00000B5F
		[DataSourceProperty]
		public bool IsDownloading
		{
			get
			{
				return this._isDownloading;
			}
			set
			{
				if (value != this._isDownloading)
				{
					this._isDownloading = value;
					base.OnPropertyChangedWithValue(value, "IsDownloading");
					base.OnPropertyChangedWithValue(this.ReadyToDownload, "ReadyToDownload");
				}
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002D RID: 45 RVA: 0x0000298E File Offset: 0x00000B8E
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002996 File Offset: 0x00000B96
		[DataSourceProperty]
		public bool ShowProgress
		{
			get
			{
				return this._showProgress;
			}
			set
			{
				if (value != this._showProgress)
				{
					this._showProgress = value;
					base.OnPropertyChangedWithValue(value, "ShowProgress");
				}
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000029B4 File Offset: 0x00000BB4
		[DataSourceProperty]
		public bool ReadyToDownload
		{
			get
			{
				return !this._isDownloading && this.SelectedMaps.Count<DCSHelperMapItemVM>() != 0;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000029CE File Offset: 0x00000BCE
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000029D6 File Offset: 0x00000BD6
		[DataSourceProperty]
		public string PanelTitleText
		{
			get
			{
				return this._panelTitleText;
			}
			set
			{
				if (value != this._panelTitleText)
				{
					this._panelTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "PanelTitleText");
				}
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000029F9 File Offset: 0x00000BF9
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002A01 File Offset: 0x00000C01
		[DataSourceProperty]
		public string DownloadButtonText
		{
			get
			{
				return this._downloadButtonText;
			}
			set
			{
				if (value != this._downloadButtonText)
				{
					this._downloadButtonText = value;
					base.OnPropertyChangedWithValue<string>(value, "DownloadButtonText");
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002A24 File Offset: 0x00000C24
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002A2C File Offset: 0x00000C2C
		[DataSourceProperty]
		public string CloseButtonText
		{
			get
			{
				return this._closeButtonText;
			}
			set
			{
				if (value != this._closeButtonText)
				{
					this._closeButtonText = value;
					base.OnPropertyChangedWithValue<string>(value, "CloseButtonText");
				}
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002A4F File Offset: 0x00000C4F
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002A57 File Offset: 0x00000C57
		[DataSourceProperty]
		public string ToggleSelectionButtonText
		{
			get
			{
				return this._toggleSelectionButtonText;
			}
			set
			{
				if (value != this._toggleSelectionButtonText)
				{
					this._toggleSelectionButtonText = value;
					base.OnPropertyChangedWithValue<string>(value, "ToggleSelectionButtonText");
				}
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002A7A File Offset: 0x00000C7A
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002A82 File Offset: 0x00000C82
		[DataSourceProperty]
		public string HostAddressText
		{
			get
			{
				return this._hostAddressText;
			}
			set
			{
				if (value != this._hostAddressText)
				{
					this._hostAddressText = value;
					base.OnPropertyChangedWithValue<string>(value, "HostAddressText");
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002AA5 File Offset: 0x00000CA5
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002AAD File Offset: 0x00000CAD
		[DataSourceProperty]
		public string ProgressCounterText
		{
			get
			{
				return this._progressCounterText;
			}
			set
			{
				if (value != this._progressCounterText)
				{
					this._progressCounterText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProgressCounterText");
				}
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002AD0 File Offset: 0x00000CD0
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002AD8 File Offset: 0x00000CD8
		[DataSourceProperty]
		public string ProgressText
		{
			get
			{
				return this._progressText;
			}
			set
			{
				if (value != this._progressText)
				{
					this._progressText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProgressText");
				}
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002AFB File Offset: 0x00000CFB
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002B03 File Offset: 0x00000D03
		[DataSourceProperty]
		public float DownloadRatio
		{
			get
			{
				return this._downloadRatio;
			}
			set
			{
				if (value != this._downloadRatio)
				{
					this._downloadRatio = value;
					base.OnPropertyChangedWithValue(value, "DownloadRatio");
				}
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002B21 File Offset: 0x00000D21
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002B29 File Offset: 0x00000D29
		[DataSourceProperty]
		public MBBindingList<DCSHelperMapItemVM> MapList
		{
			get
			{
				return this._mapList;
			}
			set
			{
				if (value != this._mapList)
				{
					this._mapList = value;
					base.OnPropertyChangedWithValue<MBBindingList<DCSHelperMapItemVM>>(value, "MapList");
				}
			}
		}

		// Token: 0x0400000B RID: 11
		private readonly string _hostAddress;

		// Token: 0x0400000C RID: 12
		private readonly string _fullName;

		// Token: 0x0400000D RID: 13
		private readonly DCSHelperVM.Texts _texts;

		// Token: 0x0400000E RID: 14
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400000F RID: 15
		private CancellationTokenSource _cancellationTokenSource;

		// Token: 0x04000010 RID: 16
		private bool _isLoading;

		// Token: 0x04000011 RID: 17
		private bool _isDownloading;

		// Token: 0x04000012 RID: 18
		private bool _showProgress;

		// Token: 0x04000013 RID: 19
		private string _panelTitleText;

		// Token: 0x04000014 RID: 20
		private string _downloadButtonText;

		// Token: 0x04000015 RID: 21
		private string _closeButtonText;

		// Token: 0x04000016 RID: 22
		private string _toggleSelectionButtonText;

		// Token: 0x04000017 RID: 23
		private string _hostAddressText;

		// Token: 0x04000018 RID: 24
		private string _progressCounterText;

		// Token: 0x04000019 RID: 25
		private string _progressText;

		// Token: 0x0400001A RID: 26
		private float _downloadRatio;

		// Token: 0x0400001B RID: 27
		private MBBindingList<DCSHelperMapItemVM> _mapList;

		// Token: 0x02000008 RID: 8
		private class Texts
		{
			// Token: 0x1700001F RID: 31
			// (get) Token: 0x0600005F RID: 95 RVA: 0x00003094 File Offset: 0x00001294
			// (set) Token: 0x06000060 RID: 96 RVA: 0x0000309C File Offset: 0x0000129C
			public string Download { get; private set; }

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x06000061 RID: 97 RVA: 0x000030A5 File Offset: 0x000012A5
			// (set) Token: 0x06000062 RID: 98 RVA: 0x000030AD File Offset: 0x000012AD
			public string Downloading { get; private set; }

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x06000063 RID: 99 RVA: 0x000030B6 File Offset: 0x000012B6
			// (set) Token: 0x06000064 RID: 100 RVA: 0x000030BE File Offset: 0x000012BE
			public string Cancel { get; private set; }

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x06000065 RID: 101 RVA: 0x000030C7 File Offset: 0x000012C7
			// (set) Token: 0x06000066 RID: 102 RVA: 0x000030CF File Offset: 0x000012CF
			public string Close { get; private set; }

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x06000067 RID: 103 RVA: 0x000030D8 File Offset: 0x000012D8
			// (set) Token: 0x06000068 RID: 104 RVA: 0x000030E0 File Offset: 0x000012E0
			public string Dismiss { get; private set; }

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x06000069 RID: 105 RVA: 0x000030E9 File Offset: 0x000012E9
			// (set) Token: 0x0600006A RID: 106 RVA: 0x000030F1 File Offset: 0x000012F1
			public string SelectAll { get; private set; }

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x0600006B RID: 107 RVA: 0x000030FA File Offset: 0x000012FA
			// (set) Token: 0x0600006C RID: 108 RVA: 0x00003102 File Offset: 0x00001302
			public string UnselectAll { get; private set; }

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x0600006D RID: 109 RVA: 0x0000310B File Offset: 0x0000130B
			// (set) Token: 0x0600006E RID: 110 RVA: 0x00003113 File Offset: 0x00001313
			public string DownloadPanel { get; private set; }

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x0600006F RID: 111 RVA: 0x0000311C File Offset: 0x0000131C
			// (set) Token: 0x06000070 RID: 112 RVA: 0x00003124 File Offset: 0x00001324
			public string DownloadComplete { get; private set; }

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x06000071 RID: 113 RVA: 0x0000312D File Offset: 0x0000132D
			// (set) Token: 0x06000072 RID: 114 RVA: 0x00003135 File Offset: 0x00001335
			public string DownloadFailed { get; private set; }

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x06000073 RID: 115 RVA: 0x0000313E File Offset: 0x0000133E
			// (set) Token: 0x06000074 RID: 116 RVA: 0x00003146 File Offset: 0x00001346
			public string Yes { get; private set; }

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x06000075 RID: 117 RVA: 0x0000314F File Offset: 0x0000134F
			// (set) Token: 0x06000076 RID: 118 RVA: 0x00003157 File Offset: 0x00001357
			public string No { get; private set; }

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x06000077 RID: 119 RVA: 0x00003160 File Offset: 0x00001360
			private TextObject PanelSubtitle
			{
				get
				{
					return new TextObject("{=GkwbPV4s}Maps available for '{SERVER_NAME}'", null);
				}
			}

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x06000078 RID: 120 RVA: 0x0000316D File Offset: 0x0000136D
			private TextObject ProgressCounter
			{
				get
				{
					return new TextObject("{=qMfaQ3fz}{DOWNLOADED_COUNT} of {TOTAL_COUNT}", null);
				}
			}

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x06000079 RID: 121 RVA: 0x0000317A File Offset: 0x0000137A
			private TextObject DownloadCompleteMessageSingular
			{
				get
				{
					return new TextObject("{=wdxXylLz}The map '{MAP_NAME}' has been successfully downloaded.", null).SetTextVariable("MODULE_NAME", "Multiplayer");
				}
			}

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x0600007A RID: 122 RVA: 0x00003196 File Offset: 0x00001396
			private TextObject DownloadCompleteMessagePlural
			{
				get
				{
					return new TextObject("{=zifpttFx}{MAP_COUNT} maps have been successfully downloaded.", null).SetTextVariable("MODULE_NAME", "Multiplayer");
				}
			}

			// Token: 0x1700002F RID: 47
			// (get) Token: 0x0600007B RID: 123 RVA: 0x000031B2 File Offset: 0x000013B2
			private TextObject ReplacementConfirmationMessage
			{
				get
				{
					return new TextObject("{=DluuLzfU}'{MAP_NAME}' already exists, should it be deleted and replaced? This action is IRREVERSIBLE.", null).SetTextVariable("MODULE_NAME", "Multiplayer");
				}
			}

			// Token: 0x0600007C RID: 124 RVA: 0x000031CE File Offset: 0x000013CE
			public Texts()
			{
				this.Refresh();
			}

			// Token: 0x0600007D RID: 125 RVA: 0x000031DC File Offset: 0x000013DC
			public void Refresh()
			{
				this.Download = new TextObject("{=a9HJ7K6I}Download", null).ToString();
				this.Downloading = new TextObject("{=adg8E1oP}Downloading...", null).ToString();
				this.Cancel = GameTexts.FindText("str_cancel", null).ToString();
				this.Close = GameTexts.FindText("str_close", null).ToString();
				this.Dismiss = GameTexts.FindText("str_dismiss", null).ToString();
				this.SelectAll = new TextObject("{=977S9OkT}Select all", null).ToString();
				this.UnselectAll = new TextObject("{=dOoPRBjm}Unselect all", null).ToString();
				this.DownloadPanel = new TextObject("{=vLSXeRnK}Download Panel", null).ToString();
				this.DownloadComplete = new TextObject("{=qhrPpmhu}Download Complete", null).ToString();
				this.DownloadFailed = new TextObject("{=7DKw0JRu}Download Failed", null).ToString();
				this.Yes = GameTexts.FindText("str_yes", null).ToString();
				this.No = GameTexts.FindText("str_no", null).ToString();
			}

			// Token: 0x0600007E RID: 126 RVA: 0x000032F1 File Offset: 0x000014F1
			public string GetPanelSubtitle(string serverName)
			{
				return this.PanelSubtitle.SetTextVariable("SERVER_NAME", serverName).ToString();
			}

			// Token: 0x0600007F RID: 127 RVA: 0x00003309 File Offset: 0x00001509
			public string GetProgressCounter(int downloadedCount, int totalCount)
			{
				return this.ProgressCounter.SetTextVariable("DOWNLOADED_COUNT", downloadedCount).SetTextVariable("TOTAL_COUNT", totalCount).ToString();
			}

			// Token: 0x06000080 RID: 128 RVA: 0x0000332C File Offset: 0x0000152C
			public string GetDownloadCompleteMessageSingular(string mapName)
			{
				return this.DownloadCompleteMessageSingular.SetTextVariable("MAP_NAME", mapName).ToString();
			}

			// Token: 0x06000081 RID: 129 RVA: 0x00003344 File Offset: 0x00001544
			public string GetDownloadCompleteMessagePlural(int mapCount)
			{
				return this.DownloadCompleteMessagePlural.SetTextVariable("MAP_COUNT", mapCount).ToString();
			}

			// Token: 0x06000082 RID: 130 RVA: 0x0000335C File Offset: 0x0000155C
			public string GetReplacementConfirmationMessage(string mapName)
			{
				return this.ReplacementConfirmationMessage.SetTextVariable("MAP_NAME", mapName).ToString();
			}
		}
	}
}
