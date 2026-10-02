using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003A3 RID: 931
	public class CharacterDeveloperState : GameState
	{
		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x060036A4 RID: 13988 RVA: 0x000DF159 File Offset: 0x000DD359
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x060036A5 RID: 13989 RVA: 0x000DF15C File Offset: 0x000DD35C
		// (set) Token: 0x060036A6 RID: 13990 RVA: 0x000DF164 File Offset: 0x000DD364
		public Hero InitialSelectedHero { get; private set; }

		// Token: 0x060036A7 RID: 13991 RVA: 0x000DF16D File Offset: 0x000DD36D
		public CharacterDeveloperState()
		{
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x000DF175 File Offset: 0x000DD375
		public CharacterDeveloperState(Hero initialSelectedHero)
		{
			this.InitialSelectedHero = initialSelectedHero;
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x060036A9 RID: 13993 RVA: 0x000DF184 File Offset: 0x000DD384
		// (set) Token: 0x060036AA RID: 13994 RVA: 0x000DF18C File Offset: 0x000DD38C
		public ICharacterDeveloperStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x04000F5D RID: 3933
		private ICharacterDeveloperStateHandler _handler;
	}
}
