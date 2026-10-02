using System;
using TaleWorlds.Core;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper
{
	// Token: 0x0200014C RID: 332
	public class FocusAddedByPlayerEvent : EventBase
	{
		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x000752E2 File Offset: 0x000734E2
		// (set) Token: 0x0600208D RID: 8333 RVA: 0x000752EA File Offset: 0x000734EA
		public Hero AddedPlayer { get; private set; }

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x0600208E RID: 8334 RVA: 0x000752F3 File Offset: 0x000734F3
		// (set) Token: 0x0600208F RID: 8335 RVA: 0x000752FB File Offset: 0x000734FB
		public SkillObject AddedSkill { get; private set; }

		// Token: 0x06002090 RID: 8336 RVA: 0x00075304 File Offset: 0x00073504
		public FocusAddedByPlayerEvent(Hero addedPlayer, SkillObject addedSkill)
		{
			this.AddedPlayer = addedPlayer;
			this.AddedSkill = addedSkill;
		}
	}
}
