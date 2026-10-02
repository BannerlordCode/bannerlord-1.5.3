using System;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000217 RID: 535
	public abstract class CharacterCreationStageBase
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x060020B3 RID: 8371 RVA: 0x00093D79 File Offset: 0x00091F79
		// (set) Token: 0x060020B4 RID: 8372 RVA: 0x00093D81 File Offset: 0x00091F81
		public ICharacterCreationStageListener Listener { get; set; }

		// Token: 0x060020B5 RID: 8373 RVA: 0x00093D8A File Offset: 0x00091F8A
		protected internal virtual void OnFinalize()
		{
			ICharacterCreationStageListener listener = this.Listener;
			if (listener == null)
			{
				return;
			}
			listener.OnStageFinalize();
		}
	}
}
