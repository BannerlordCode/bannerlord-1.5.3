using System;
using System.Collections.Generic;
using SandBox.GauntletUI.BannerEditor;
using SandBox.View.CharacterCreation;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.CharacterCreation
{
	// Token: 0x02000048 RID: 72
	[CharacterCreationStageView(typeof(CharacterCreationBannerEditorStage))]
	public class CharacterCreationBannerEditorView : CharacterCreationStageViewBase
	{
		// Token: 0x06000347 RID: 839 RVA: 0x00013B5C File Offset: 0x00011D5C
		public CharacterCreationBannerEditorView(CharacterCreationManager characterCreationManager, ControlCharacterCreationStage affirmativeAction, TextObject affirmativeActionText, ControlCharacterCreationStage negativeAction, TextObject negativeActionText, ControlCharacterCreationStage onRefresh = null, ControlCharacterCreationStageReturnInt getCurrentStageIndexAction = null, ControlCharacterCreationStageReturnInt getTotalStageCountAction = null, ControlCharacterCreationStageReturnInt getFurthestIndexAction = null, ControlCharacterCreationStageWithInt goToIndexAction = null)
			: this(CharacterObject.PlayerCharacter, Clan.PlayerClan.Banner, affirmativeAction, affirmativeActionText, negativeAction, negativeActionText, onRefresh, getCurrentStageIndexAction, getTotalStageCountAction, getFurthestIndexAction, goToIndexAction)
		{
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00013B90 File Offset: 0x00011D90
		public CharacterCreationBannerEditorView(BasicCharacterObject character, Banner banner, ControlCharacterCreationStage affirmativeAction, TextObject affirmativeActionText, ControlCharacterCreationStage negativeAction, TextObject negativeActionText, ControlCharacterCreationStage onRefresh = null, ControlCharacterCreationStageReturnInt getCurrentStageIndexAction = null, ControlCharacterCreationStageReturnInt getTotalStageCountAction = null, ControlCharacterCreationStageReturnInt getFurthestIndexAction = null, ControlCharacterCreationStageWithInt goToIndexAction = null)
			: base(affirmativeAction, negativeAction, onRefresh, getTotalStageCountAction, getCurrentStageIndexAction, getFurthestIndexAction, goToIndexAction)
		{
			this._bannerEditorView = new BannerEditorView(character, banner, new ControlCharacterCreationStage(this.AffirmativeAction), affirmativeActionText, negativeAction, negativeActionText, onRefresh, getCurrentStageIndexAction, getTotalStageCountAction, getFurthestIndexAction, goToIndexAction);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00013BD9 File Offset: 0x00011DD9
		public override IEnumerable<ScreenLayer> GetLayers()
		{
			return new List<ScreenLayer>
			{
				this._bannerEditorView.SceneLayer,
				this._bannerEditorView.GauntletLayer
			};
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00013C02 File Offset: 0x00011E02
		public override void PreviousStage()
		{
			this._bannerEditorView.Exit(true);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00013C10 File Offset: 0x00011E10
		public override void NextStage()
		{
			this._bannerEditorView.Exit(false);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00013C1E File Offset: 0x00011E1E
		public override void Tick(float dt)
		{
			if (!this._isFinalized)
			{
				this._bannerEditorView.OnTick(dt);
				if (this._isFinalized)
				{
					return;
				}
				base.HandleEscapeMenu(this, this._bannerEditorView.SceneLayer);
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00013C4F File Offset: 0x00011E4F
		public override int GetVirtualStageCount()
		{
			return 1;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00013C52 File Offset: 0x00011E52
		public override void GoToIndex(int index)
		{
			this._bannerEditorView.GoToIndex(index);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00013C60 File Offset: 0x00011E60
		protected override void OnFinalize()
		{
			this._bannerEditorView.OnDeactivate();
			this._bannerEditorView.OnFinalize();
			this._isFinalized = true;
			base.OnFinalize();
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00013C88 File Offset: 0x00011E88
		private void AffirmativeAction()
		{
			uint primaryColor = this._bannerEditorView.Banner.GetPrimaryColor();
			uint firstIconColor = this._bannerEditorView.Banner.GetFirstIconColor();
			Clan playerClan = Clan.PlayerClan;
			playerClan.Color = primaryColor;
			playerClan.Color2 = firstIconColor;
			playerClan.UpdateBannerColor(primaryColor, firstIconColor);
			(GameStateManager.Current.ActiveState as CharacterCreationState).CharacterCreationManager.CharacterCreationContent.SetMainClanBanner(this._bannerEditorView.Banner);
			this._affirmativeAction();
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00013D05 File Offset: 0x00011F05
		public override void LoadEscapeMenuMovie()
		{
			this._escapeMenuDatasource = new EscapeMenuVM(base.GetEscapeMenuItems(this), null);
			this._escapeMenuMovie = this._bannerEditorView.GauntletLayer.LoadMovie("EscapeMenu", this._escapeMenuDatasource);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00013D3B File Offset: 0x00011F3B
		public override void ReleaseEscapeMenuMovie()
		{
			this._bannerEditorView.GauntletLayer.ReleaseMovie(this._escapeMenuMovie);
			this._escapeMenuDatasource = null;
			this._escapeMenuMovie = null;
		}

		// Token: 0x04000148 RID: 328
		private readonly BannerEditorView _bannerEditorView;

		// Token: 0x04000149 RID: 329
		private bool _isFinalized;

		// Token: 0x0400014A RID: 330
		private EscapeMenuVM _escapeMenuDatasource;

		// Token: 0x0400014B RID: 331
		private GauntletMovieIdentifier _escapeMenuMovie;
	}
}
