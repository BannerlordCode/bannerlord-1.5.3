using System;
using System.Collections.Generic;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000012 RID: 18
	public class GauntletGameVersionView : GlobalLayer
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00004FD2 File Offset: 0x000031D2
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00004FD9 File Offset: 0x000031D9
		public static GauntletGameVersionView Current { get; private set; }

		// Token: 0x06000093 RID: 147 RVA: 0x00004FE4 File Offset: 0x000031E4
		public GauntletGameVersionView()
		{
			this._dataSource = new GameVersionVM(new Func<List<string>>(GauntletGameVersionView.CollectAllVersionTexts));
			GauntletLayer gauntletLayer = new GauntletLayer("MainMenuGameVersion", 15001, false);
			gauntletLayer.LoadMovie("GameVersion", this._dataSource);
			base.Layer = gauntletLayer;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000503F File Offset: 0x0000323F
		public static void Initialize()
		{
			if (GauntletGameVersionView.Current == null)
			{
				GauntletGameVersionView.Current = new GauntletGameVersionView();
				ScreenManager.AddGlobalLayer(GauntletGameVersionView.Current, false);
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000505D File Offset: 0x0000325D
		public static void Refresh()
		{
			GauntletGameVersionView gauntletGameVersionView = GauntletGameVersionView.Current;
			if (gauntletGameVersionView == null)
			{
				return;
			}
			GameVersionVM dataSource = gauntletGameVersionView._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.RefreshValues();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005078 File Offset: 0x00003278
		public static void AddModuleVersionInfo(string title, string versionStr)
		{
			GauntletGameVersionView._versionTexts.Add(new Tuple<string, string>(title, versionStr));
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000508C File Offset: 0x0000328C
		public static void RemoveModuleVersionInfo(string title)
		{
			GauntletGameVersionView._versionTexts.RemoveAll((Tuple<string, string> x) => x.Item1 == title);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000050C0 File Offset: 0x000032C0
		private static List<string> CollectAllVersionTexts()
		{
			List<string> list = new List<string>();
			foreach (Tuple<string, string> tuple in GauntletGameVersionView._versionTexts)
			{
				list.Add(tuple.Item1 + ": " + tuple.Item2);
			}
			return list;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005130 File Offset: 0x00003330
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			bool flag = ScreenManager.TopScreen is GauntletInitialScreen || ScreenManager.TopScreen is GauntletOptionsScreen;
			this.SetEnabled(flag);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005168 File Offset: 0x00003368
		private void SetEnabled(bool isEnabled)
		{
			if (this._isEnabled != isEnabled)
			{
				this._isEnabled = isEnabled;
				ScreenManager.SetSuspendLayer(base.Layer, !this._isEnabled);
				if (this._isEnabled)
				{
					GauntletGameVersionView.Refresh();
				}
			}
		}

		// Token: 0x04000060 RID: 96
		private static readonly List<Tuple<string, string>> _versionTexts = new List<Tuple<string, string>>();

		// Token: 0x04000061 RID: 97
		private GameVersionVM _dataSource;

		// Token: 0x04000062 RID: 98
		private bool _isEnabled = true;
	}
}
