using System;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003B4 RID: 948
	public class PartyState : PlayerGameState
	{
		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06003747 RID: 14151 RVA: 0x000DFA3B File Offset: 0x000DDC3B
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06003748 RID: 14152 RVA: 0x000DFA3E File Offset: 0x000DDC3E
		// (set) Token: 0x06003749 RID: 14153 RVA: 0x000DFA46 File Offset: 0x000DDC46
		public PartyScreenLogic PartyScreenLogic { get; set; }

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x0600374A RID: 14154 RVA: 0x000DFA4F File Offset: 0x000DDC4F
		// (set) Token: 0x0600374B RID: 14155 RVA: 0x000DFA57 File Offset: 0x000DDC57
		public PartyScreenHelper.PartyScreenMode PartyScreenMode { get; set; }

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x0600374C RID: 14156 RVA: 0x000DFA60 File Offset: 0x000DDC60
		// (set) Token: 0x0600374D RID: 14157 RVA: 0x000DFA68 File Offset: 0x000DDC68
		public bool IsDonating { get; set; }

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x0600374E RID: 14158 RVA: 0x000DFA71 File Offset: 0x000DDC71
		// (set) Token: 0x0600374F RID: 14159 RVA: 0x000DFA79 File Offset: 0x000DDC79
		public IPartyScreenLogicHandler Handler { get; set; }

		// Token: 0x06003750 RID: 14160 RVA: 0x000DFA82 File Offset: 0x000DDC82
		public void RequestUserInput(string text, Action accept, Action cancel)
		{
			if (this.Handler != null)
			{
				this.Handler.RequestUserInput(text, accept, cancel);
			}
		}
	}
}
