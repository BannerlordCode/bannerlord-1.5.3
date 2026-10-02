using System;
using System.Collections.Generic;
using SandBox.View.CharacterCreation;
using SandBox.View.Missions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI.CharacterCreation
{
	// Token: 0x0200004A RID: 74
	[CharacterCreationStageView(typeof(CharacterCreationCultureStage))]
	public class CharacterCreationCultureStageView : CharacterCreationStageViewBase
	{
		// Token: 0x0600036A RID: 874 RVA: 0x00014BA8 File Offset: 0x00012DA8
		public CharacterCreationCultureStageView(CharacterCreationManager characterCreationManager, ControlCharacterCreationStage affirmativeAction, TextObject affirmativeActionText, ControlCharacterCreationStage negativeAction, TextObject negativeActionText, ControlCharacterCreationStage onRefresh, ControlCharacterCreationStageReturnInt getCurrentStageIndexAction, ControlCharacterCreationStageReturnInt getTotalStageCountAction, ControlCharacterCreationStageReturnInt getFurthestIndexAction, ControlCharacterCreationStageWithInt goToIndexAction)
			: base(affirmativeAction, negativeAction, onRefresh, getCurrentStageIndexAction, getTotalStageCountAction, getFurthestIndexAction, goToIndexAction)
		{
			this._characterCreationManager = characterCreationManager;
			this.GauntletLayer = new GauntletLayer("CharacterCreationCulture", 1, true)
			{
				IsFocusLayer = true
			};
			this.GauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this.GauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			ScreenManager.TrySetFocus(this.GauntletLayer);
			this._dataSource = new CharacterCreationCultureStageVM(this._characterCreationManager, new Action(this.NextStage), affirmativeActionText, new Action(this.PreviousStage), negativeActionText, new Action<CultureObject>(this.OnCultureSelected));
			this._movie = this.GauntletLayer.LoadMovie("CharacterCreationCultureStage", this._dataSource);
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._characterCreationCategory = UIResourceManager.LoadSpriteCategory("ui_charactercreation");
			if (this._characterCreationManager.GetStage<CharacterCreationBannerEditorStage>() != null)
			{
				this._bannerEditorCategory = UIResourceManager.LoadSpriteCategory("ui_bannericons");
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00014CE0 File Offset: 0x00012EE0
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this.GauntletLayer = null;
			CharacterCreationCultureStageVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._dataSource = null;
			this._characterCreationCategory.Unload();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00014D14 File Offset: 0x00012F14
		private void HandleLayerInput()
		{
			if (this.GauntletLayer.Input.IsHotKeyReleased("Exit"))
			{
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
				this._dataSource.OnPreviousStage();
				return;
			}
			if (this.GauntletLayer.Input.IsHotKeyReleased("Confirm") && this._dataSource.CanAdvance)
			{
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
				this._dataSource.OnNextStage();
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00014D87 File Offset: 0x00012F87
		public override void Tick(float dt)
		{
			base.Tick(dt);
			if (this._dataSource.IsActive)
			{
				base.HandleEscapeMenu(this, this.GauntletLayer);
				this.HandleLayerInput();
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00014DB0 File Offset: 0x00012FB0
		public override void NextStage()
		{
			this._characterCreationManager.CharacterCreationContent.SetMainCharacterName(NameGenerator.Current.GenerateFirstNameForPlayer(this._dataSource.CurrentSelectedCulture.Culture, Hero.MainHero.IsFemale).ToString());
			this._affirmativeAction();
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00014E04 File Offset: 0x00013004
		private void OnCultureSelected(CultureObject culture)
		{
			MissionSoundParametersView.SoundParameterMissionCulture soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.None;
			if (culture.StringId == "aserai")
			{
				soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Aserai;
			}
			else if (culture.StringId == "khuzait")
			{
				soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Khuzait;
			}
			else if (culture.StringId == "vlandia")
			{
				soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Vlandia;
			}
			else if (culture.StringId == "sturgia")
			{
				soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Sturgia;
			}
			else if (culture.StringId == "battania")
			{
				soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Battania;
			}
			else if (culture.StringId == "empire")
			{
				soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Empire;
			}
			else if (culture.StringId == "nord")
			{
				soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Nord;
			}
			SoundManager.SetGlobalParameter("MissionCulture", (float)soundParameterMissionCulture);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00014EBA File Offset: 0x000130BA
		public override void PreviousStage()
		{
			Game.Current.GameStateManager.PopState(0);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00014ECC File Offset: 0x000130CC
		public override int GetVirtualStageCount()
		{
			return 1;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00014ECF File Offset: 0x000130CF
		public override IEnumerable<ScreenLayer> GetLayers()
		{
			return new List<ScreenLayer> { this.GauntletLayer };
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00014EE2 File Offset: 0x000130E2
		public override void LoadEscapeMenuMovie()
		{
			this._escapeMenuDatasource = new EscapeMenuVM(base.GetEscapeMenuItems(this), null);
			this._escapeMenuMovie = this.GauntletLayer.LoadMovie("EscapeMenu", this._escapeMenuDatasource);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00014F13 File Offset: 0x00013113
		public override void ReleaseEscapeMenuMovie()
		{
			this.GauntletLayer.ReleaseMovie(this._escapeMenuMovie);
			this._escapeMenuDatasource = null;
			this._escapeMenuMovie = null;
		}

		// Token: 0x04000163 RID: 355
		private const string CultureParameterId = "MissionCulture";

		// Token: 0x04000164 RID: 356
		private readonly GauntletMovieIdentifier _movie;

		// Token: 0x04000165 RID: 357
		private GauntletLayer GauntletLayer;

		// Token: 0x04000166 RID: 358
		private CharacterCreationCultureStageVM _dataSource;

		// Token: 0x04000167 RID: 359
		private SpriteCategory _characterCreationCategory;

		// Token: 0x04000168 RID: 360
		private SpriteCategory _bannerEditorCategory;

		// Token: 0x04000169 RID: 361
		private readonly CharacterCreationManager _characterCreationManager;

		// Token: 0x0400016A RID: 362
		private EscapeMenuVM _escapeMenuDatasource;

		// Token: 0x0400016B RID: 363
		private GauntletMovieIdentifier _escapeMenuMovie;
	}
}
