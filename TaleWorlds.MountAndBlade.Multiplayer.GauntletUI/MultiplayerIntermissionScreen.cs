using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Intermission;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI
{
	// Token: 0x02000006 RID: 6
	[GameStateScreen(typeof(LobbyGameStateCustomGameClient))]
	[GameStateScreen(typeof(LobbyGameStateCommunityClient))]
	public class MultiplayerIntermissionScreen : ScreenBase, IGameStateListener, IChatLogHandlerScreen
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002277 File Offset: 0x00000477
		// (set) Token: 0x06000016 RID: 22 RVA: 0x0000227F File Offset: 0x0000047F
		public GauntletLayer Layer { get; private set; }

		// Token: 0x06000017 RID: 23 RVA: 0x00002288 File Offset: 0x00000488
		public MultiplayerIntermissionScreen(LobbyGameStateCustomGameClient gameState)
		{
			this.Construct();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002296 File Offset: 0x00000496
		public MultiplayerIntermissionScreen(LobbyGameStateCommunityClient gameState)
		{
			this.Construct();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000022A4 File Offset: 0x000004A4
		private void Construct()
		{
			this._customGameClientCategory = UIResourceManager.LoadSpriteCategory("ui_mpintermission");
			this._dataSource = new MPIntermissionVM();
			this.Layer = new GauntletLayer("MultiplayerIntermission", 100, false);
			this.Layer.IsFocusLayer = true;
			base.AddLayer(this.Layer);
			this.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this.Layer.LoadMovie("MultiplayerIntermission", this._dataSource);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002328 File Offset: 0x00000528
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this._dataSource.Tick();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000233C File Offset: 0x0000053C
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._customGameClientCategory.Unload();
			this.Layer.InputRestrictions.ResetInputRestrictions();
			this.Layer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002378 File Offset: 0x00000578
		void IGameStateListener.OnActivate()
		{
			this.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			ScreenManager.TrySetFocus(this.Layer);
			LoadingWindow.EnableGlobalLoadingWindow();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000239C File Offset: 0x0000059C
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000239E File Offset: 0x0000059E
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000023A0 File Offset: 0x000005A0
		void IGameStateListener.OnFinalize()
		{
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000023A2 File Offset: 0x000005A2
		void IChatLogHandlerScreen.TryUpdateChatLogLayerParameters(ref bool isTeamChatAvailable, ref bool inputEnabled, ref bool isToggleChatHintAvailable, ref bool isMouseVisible, ref InputContext inputContext)
		{
			if (this.Layer != null)
			{
				isTeamChatAvailable = false;
				inputEnabled = true;
				inputContext = this.Layer.Input;
			}
		}

		// Token: 0x04000007 RID: 7
		private MPIntermissionVM _dataSource;

		// Token: 0x04000008 RID: 8
		private SpriteCategory _customGameClientCategory;
	}
}
