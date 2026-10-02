using System;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000219 RID: 537
	public interface ICharacterCreationContentHandler
	{
		// Token: 0x060020C1 RID: 8385
		void InitializeContent(CharacterCreationManager characterCreationManager);

		// Token: 0x060020C2 RID: 8386
		void AfterInitializeContent(CharacterCreationManager characterCreationManager);

		// Token: 0x060020C3 RID: 8387
		void OnStageCompleted(CharacterCreationStageBase stage);

		// Token: 0x060020C4 RID: 8388
		void OnCharacterCreationFinalize(CharacterCreationManager characterCreationManager);
	}
}
