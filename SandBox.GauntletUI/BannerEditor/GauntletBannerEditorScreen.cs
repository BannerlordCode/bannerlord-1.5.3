using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.BannerEditor
{
	// Token: 0x02000050 RID: 80
	[GameStateScreen(typeof(BannerEditorState))]
	public class GauntletBannerEditorScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x060003DE RID: 990 RVA: 0x00018598 File Offset: 0x00016798
		public GauntletBannerEditorScreen(BannerEditorState bannerEditorState)
		{
			LoadingWindow.EnableGlobalLoadingWindow();
			this._clan = bannerEditorState.GetClan();
			this._bannerEditorLayer = new BannerEditorView(bannerEditorState.GetCharacter(), bannerEditorState.GetClan().Banner, new ControlCharacterCreationStage(this.OnDone), new TextObject("{=WiNRdfsm}Done", null), new ControlCharacterCreationStage(this.OnCancel), new TextObject("{=3CpNUnVl}Cancel", null), null, null, null, null, null);
			this._bannerEditorLayer.DataSource.SetClanRelatedRules(bannerEditorState.GetClan().Kingdom == null);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00018629 File Offset: 0x00016829
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this._bannerEditorLayer.OnTick(dt);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00018640 File Offset: 0x00016840
		public void OnDone()
		{
			uint primaryColor = this._bannerEditorLayer.DataSource.BannerVM.Banner.GetPrimaryColor();
			uint firstIconColor = this._bannerEditorLayer.DataSource.BannerVM.Banner.GetFirstIconColor();
			this._clan.Color2 = firstIconColor;
			if (this._bannerEditorLayer.DataSource.CanChangeBackgroundColor)
			{
				this._clan.Color = primaryColor;
				this._clan.UpdateBannerColor(primaryColor, firstIconColor);
			}
			else
			{
				this._clan.UpdateBannerColor(this._clan.Color, firstIconColor);
			}
			Game.Current.GameStateManager.PopState(0);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x000186E3 File Offset: 0x000168E3
		public void OnCancel()
		{
			Game.Current.GameStateManager.PopState(0);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000186F5 File Offset: 0x000168F5
		protected override void OnInitialize()
		{
			base.OnInitialize();
			Game.Current.GameStateManager.RegisterActiveStateDisableRequest(this);
			InformationManager.HideAllMessages();
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00018712 File Offset: 0x00016912
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._bannerEditorLayer.OnFinalize();
			if (LoadingWindow.IsLoadingWindowActive)
			{
				LoadingWindow.DisableGlobalLoadingWindow();
			}
			Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00018741 File Offset: 0x00016941
		protected override void OnActivate()
		{
			base.OnActivate();
			base.AddLayer(this._bannerEditorLayer.GauntletLayer);
			base.AddLayer(this._bannerEditorLayer.SceneLayer);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001876B File Offset: 0x0001696B
		protected override void OnDeactivate()
		{
			this._bannerEditorLayer.OnDeactivate();
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00018778 File Offset: 0x00016978
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001877A File Offset: 0x0001697A
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001877C File Offset: 0x0001697C
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0001877E File Offset: 0x0001697E
		void IGameStateListener.OnFinalize()
		{
		}

		// Token: 0x040001BE RID: 446
		private const int ViewOrderPriority = 15;

		// Token: 0x040001BF RID: 447
		private readonly BannerEditorView _bannerEditorLayer;

		// Token: 0x040001C0 RID: 448
		private readonly Clan _clan;
	}
}
