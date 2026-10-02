using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.InitialMenu
{
	// Token: 0x0200004A RID: 74
	public class InitialMenuAnnouncementVM : ViewModel
	{
		// Token: 0x0600062C RID: 1580 RVA: 0x00016B90 File Offset: 0x00014D90
		public InitialMenuAnnouncementVM()
		{
			this.Refresh();
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00016BA0 File Offset: 0x00014DA0
		public void Tick()
		{
			if (this._needsRefresh && !this._isFetchingData)
			{
				try
				{
					this.SetDataFromAnnouncementInfo();
				}
				catch (Exception)
				{
					this._announcementInfo = null;
					this.ImageSourcePath = null;
					this._clickLink = null;
				}
				this.IsVisible = !string.IsNullOrEmpty(this.ImageSourcePath);
				this.IsLinkAvailable = !string.IsNullOrEmpty(this._clickLink);
				this._needsRefresh = false;
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00016C1C File Offset: 0x00014E1C
		private void SetDataFromAnnouncementInfo()
		{
			Dictionary<string, InitialMenuAnnouncementVM.AnnouncementInformation> dictionary;
			if (this._announcementInfo == null || !this._announcementInfo.TryGetValue(this.GetPlatformString(), out dictionary) || dictionary == null || dictionary.Count <= 0)
			{
				this.ImageSourcePath = null;
				this._clickLink = null;
				return;
			}
			InitialMenuAnnouncementVM.AnnouncementInformation announcementInformation;
			if (!dictionary.TryGetValue(this.GetLanguageString(), out announcementInformation) && !dictionary.TryGetValue("en", out announcementInformation))
			{
				announcementInformation = dictionary.Values.FirstOrDefault<InitialMenuAnnouncementVM.AnnouncementInformation>();
			}
			bool flag = true;
			List<string> excludedModules = announcementInformation.ExcludedModules;
			if (excludedModules != null && excludedModules.Count > 0)
			{
				if (excludedModules.TrueForAll((string module) => ModuleHelper.IsModuleActive(module)))
				{
					flag = false;
				}
			}
			if (flag)
			{
				this.ImageSourcePath = announcementInformation.ImageUrl;
				this._clickLink = announcementInformation.LinkUrl;
				return;
			}
			this.ImageSourcePath = null;
			this._clickLink = null;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00016D03 File Offset: 0x00014F03
		public void Refresh()
		{
			if (this._isFetchingData)
			{
				return;
			}
			if (this._announcementInfo != null)
			{
				this._needsRefresh = true;
				return;
			}
			this._isFetchingData = true;
			this.RefreshAux();
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00016D2C File Offset: 0x00014F2C
		private async void RefreshAux()
		{
			try
			{
				string text = await HttpHelper.DownloadStringTaskAsync("https://taleworldswebsiteassets.blob.core.windows.net/upload/upsell/data.json");
				this._announcementInfo = Common.DeserializeObjectFromJson<Dictionary<string, Dictionary<string, InitialMenuAnnouncementVM.AnnouncementInformation>>>(text);
			}
			catch (Exception)
			{
			}
			this._isFetchingData = false;
			this._needsRefresh = true;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00016D68 File Offset: 0x00014F68
		public void ExecuteNavigateToLink()
		{
			if (!this.IsLinkAvailable || string.IsNullOrEmpty(this._clickLink))
			{
				return;
			}
			if (ApplicationPlatform.CurrentPlatform == Platform.Durango || ApplicationPlatform.CurrentPlatform == Platform.Orbis)
			{
				Utilities.OpenConsoleStorePage(this._clickLink);
				return;
			}
			if (!PlatformServices.Instance.ShowOverlayForWebPage(this._clickLink).Result)
			{
				Process.Start(new ProcessStartInfo(this._clickLink)
				{
					UseShellExecute = true
				});
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00016DD8 File Offset: 0x00014FD8
		private string GetPlatformString()
		{
			switch (ApplicationPlatform.CurrentPlatform)
			{
			case Platform.WindowsSteam:
				return "steam";
			case Platform.WindowsEpic:
				return "epic";
			case Platform.Orbis:
				return "ps4";
			case Platform.Durango:
				return "xbone";
			case Platform.WindowsGOG:
				return "gog";
			case Platform.GDKDesktop:
				return "gdk";
			}
			return string.Empty;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00016E40 File Offset: 0x00015040
		private string GetLanguageString()
		{
			string language = BannerlordConfig.Language;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(language);
			if (num <= 1760062771U)
			{
				if (num <= 463134907U)
				{
					if (num != 134208041U)
					{
						if (num != 425085109U)
						{
							if (num == 463134907U)
							{
								if (language == "English")
								{
									return "en";
								}
							}
						}
						else if (language == "Português (BR)")
						{
							return "br";
						}
					}
					else if (language == "Polski")
					{
						return "pl";
					}
				}
				else if (num != 1161419880U)
				{
					if (num != 1409693518U)
					{
						if (num == 1760062771U)
						{
							if (language == "Русский")
							{
								return "ru";
							}
						}
					}
					else if (language == "日本語")
					{
						return "jp";
					}
				}
				else if (language == "繁體中文")
				{
					return "cnt";
				}
			}
			else if (num <= 2613828866U)
			{
				if (num != 1833040324U)
				{
					if (num != 1856371212U)
					{
						if (num == 2613828866U)
						{
							if (language == "한국어")
							{
								return "kr";
							}
						}
					}
					else if (language == "Italiano")
					{
						return "it";
					}
				}
				else if (language == "简体中文")
				{
					return "cns";
				}
			}
			else if (num <= 3399016062U)
			{
				if (num != 2616412764U)
				{
					if (num == 3399016062U)
					{
						if (language == "Türkçe")
						{
							return "tr";
						}
					}
				}
				else if (language == "Español (LA)")
				{
					return "sp";
				}
			}
			else if (num != 4095678947U)
			{
				if (num == 4176589014U)
				{
					if (language == "Français")
					{
						return "fr";
					}
				}
			}
			else if (language == "Deutsch")
			{
				return "de";
			}
			return "en";
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0001705F File Offset: 0x0001525F
		// (set) Token: 0x06000635 RID: 1589 RVA: 0x00017067 File Offset: 0x00015267
		[DataSourceProperty]
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (value != this._isVisible)
				{
					this._isVisible = value;
					base.OnPropertyChangedWithValue(value, "IsVisible");
				}
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x00017085 File Offset: 0x00015285
		// (set) Token: 0x06000637 RID: 1591 RVA: 0x0001708D File Offset: 0x0001528D
		[DataSourceProperty]
		public bool IsLinkAvailable
		{
			get
			{
				return this._isLinkAvailable;
			}
			set
			{
				if (value != this._isLinkAvailable)
				{
					this._isLinkAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsLinkAvailable");
				}
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x000170AB File Offset: 0x000152AB
		// (set) Token: 0x06000639 RID: 1593 RVA: 0x000170B3 File Offset: 0x000152B3
		[DataSourceProperty]
		public string ImageSourcePath
		{
			get
			{
				return this._imageSourcePath;
			}
			set
			{
				if (value != this._imageSourcePath)
				{
					this._imageSourcePath = value;
					base.OnPropertyChangedWithValue<string>(value, "ImageSourcePath");
				}
			}
		}

		// Token: 0x040002C3 RID: 707
		private bool _isFetchingData;

		// Token: 0x040002C4 RID: 708
		private bool _needsRefresh;

		// Token: 0x040002C5 RID: 709
		private Dictionary<string, Dictionary<string, InitialMenuAnnouncementVM.AnnouncementInformation>> _announcementInfo;

		// Token: 0x040002C6 RID: 710
		private string _clickLink;

		// Token: 0x040002C7 RID: 711
		private bool _isVisible;

		// Token: 0x040002C8 RID: 712
		private bool _isLinkAvailable;

		// Token: 0x040002C9 RID: 713
		private string _imageSourcePath;

		// Token: 0x020000E0 RID: 224
		private struct AnnouncementInformation
		{
			// Token: 0x1700038D RID: 909
			// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x00029CA2 File Offset: 0x00027EA2
			// (set) Token: 0x06000CE6 RID: 3302 RVA: 0x00029CAA File Offset: 0x00027EAA
			public string ImageUrl { get; set; }

			// Token: 0x1700038E RID: 910
			// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00029CB3 File Offset: 0x00027EB3
			// (set) Token: 0x06000CE8 RID: 3304 RVA: 0x00029CBB File Offset: 0x00027EBB
			public string LinkUrl { get; set; }

			// Token: 0x1700038F RID: 911
			// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00029CC4 File Offset: 0x00027EC4
			// (set) Token: 0x06000CEA RID: 3306 RVA: 0x00029CCC File Offset: 0x00027ECC
			public List<string> ExcludedModules { get; set; }
		}
	}
}
