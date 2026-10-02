using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI
{
	// Token: 0x02000002 RID: 2
	[GameStateScreen(typeof(LobbyPracticeState))]
	public class LobbyPracticeStateGauntletScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
		public GauntletLayer Layer { get; private set; }

		// Token: 0x06000003 RID: 3 RVA: 0x0000205C File Offset: 0x0000025C
		public LobbyPracticeStateGauntletScreen(LobbyPracticeState gameState)
		{
			this._dataSource = new MPPracticeVM();
			this.Layer = new GauntletLayer("LobbyPracticeScreen", 100, false);
			this.Layer.IsFocusLayer = true;
			base.AddLayer(this.Layer);
			this.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this.Layer.LoadMovie("MultiplayerPractice", this._dataSource);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020D6 File Offset: 0x000002D6
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this.Layer.InputRestrictions.ResetInputRestrictions();
			this.Layer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002107 File Offset: 0x00000307
		void IGameStateListener.OnActivate()
		{
			this.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			ScreenManager.TrySetFocus(this.Layer);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002126 File Offset: 0x00000326
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002128 File Offset: 0x00000328
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000212A File Offset: 0x0000032A
		void IGameStateListener.OnFinalize()
		{
		}

		// Token: 0x04000002 RID: 2
		private MPPracticeVM _dataSource;
	}
}
