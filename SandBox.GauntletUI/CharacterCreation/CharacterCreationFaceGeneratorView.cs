using System;
using System.Collections.Generic;
using SandBox.View.CharacterCreation;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.GauntletUI.BodyGenerator;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using TaleWorlds.ObjectSystem;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.CharacterCreation
{
	// Token: 0x0200004B RID: 75
	[CharacterCreationStageView(typeof(CharacterCreationFaceGeneratorStage))]
	public class CharacterCreationFaceGeneratorView : CharacterCreationStageViewBase
	{
		// Token: 0x06000375 RID: 885 RVA: 0x00014F34 File Offset: 0x00013134
		public CharacterCreationFaceGeneratorView(CharacterCreationManager characterCreationManager, ControlCharacterCreationStage affirmativeAction, TextObject affirmativeActionText, ControlCharacterCreationStage negativeAction, TextObject negativeActionText, ControlCharacterCreationStage onRefresh, ControlCharacterCreationStageReturnInt getCurrentStageIndexAction, ControlCharacterCreationStageReturnInt getTotalStageCountAction, ControlCharacterCreationStageReturnInt getFurthestIndexAction, ControlCharacterCreationStageWithInt goToIndexAction)
			: base(affirmativeAction, negativeAction, onRefresh, getTotalStageCountAction, getCurrentStageIndexAction, getFurthestIndexAction, goToIndexAction)
		{
			this._characterCreationManager = characterCreationManager;
			MBObjectManager objectManager = Game.Current.ObjectManager;
			string text = "player_char_creation_show_";
			CharacterObject playerCharacter = CharacterObject.PlayerCharacter;
			string text2;
			if (playerCharacter == null)
			{
				text2 = null;
			}
			else
			{
				CultureObject culture = playerCharacter.Culture;
				text2 = ((culture != null) ? culture.StringId : null);
			}
			MBEquipmentRoster @object = objectManager.GetObject<MBEquipmentRoster>(text + text2);
			Equipment equipment = ((@object != null) ? @object.DefaultEquipment : null);
			this._faceGeneratorView = new BodyGeneratorView(new ControlCharacterCreationStage(this.NextStage), affirmativeActionText, new ControlCharacterCreationStage(this.PreviousStage), negativeActionText, CharacterObject.PlayerCharacter, false, null, equipment, getCurrentStageIndexAction, getTotalStageCountAction, getFurthestIndexAction, goToIndexAction, this._characterCreationManager.FaceGenHistory);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00014FE1 File Offset: 0x000131E1
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._faceGeneratorView.OnFinalize();
			this._faceGeneratorView = null;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00014FFB File Offset: 0x000131FB
		public override IEnumerable<ScreenLayer> GetLayers()
		{
			return new List<ScreenLayer>
			{
				this._faceGeneratorView.SceneLayer,
				this._faceGeneratorView.GauntletLayer
			};
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00015024 File Offset: 0x00013224
		public override void PreviousStage()
		{
			this._negativeAction();
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00015031 File Offset: 0x00013231
		public override void NextStage()
		{
			this._affirmativeAction();
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0001503E File Offset: 0x0001323E
		public override void Tick(float dt)
		{
			this._faceGeneratorView.OnTick(dt);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001504C File Offset: 0x0001324C
		public override int GetVirtualStageCount()
		{
			return 1;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0001504F File Offset: 0x0001324F
		public override void GoToIndex(int index)
		{
			this._goToIndexAction(index);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0001505D File Offset: 0x0001325D
		public override void LoadEscapeMenuMovie()
		{
			this._escapeMenuDatasource = new EscapeMenuVM(base.GetEscapeMenuItems(this), null);
			this._escapeMenuMovie = this._faceGeneratorView.GauntletLayer.LoadMovie("EscapeMenu", this._escapeMenuDatasource);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00015093 File Offset: 0x00013293
		public override void ReleaseEscapeMenuMovie()
		{
			this._faceGeneratorView.GauntletLayer.ReleaseMovie(this._escapeMenuMovie);
			this._escapeMenuDatasource = null;
			this._escapeMenuMovie = null;
		}

		// Token: 0x0400016C RID: 364
		private BodyGeneratorView _faceGeneratorView;

		// Token: 0x0400016D RID: 365
		private readonly CharacterCreationManager _characterCreationManager;

		// Token: 0x0400016E RID: 366
		private EscapeMenuVM _escapeMenuDatasource;

		// Token: 0x0400016F RID: 367
		private GauntletMovieIdentifier _escapeMenuMovie;
	}
}
