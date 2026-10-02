using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003A2 RID: 930
	public class BarberState : GameState
	{
		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x0600369F RID: 13983 RVA: 0x000DF127 File Offset: 0x000DD327
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x060036A0 RID: 13984 RVA: 0x000DF12A File Offset: 0x000DD32A
		// (set) Token: 0x060036A1 RID: 13985 RVA: 0x000DF132 File Offset: 0x000DD332
		public IFaceGeneratorCustomFilter Filter { get; private set; }

		// Token: 0x060036A2 RID: 13986 RVA: 0x000DF13B File Offset: 0x000DD33B
		public BarberState()
		{
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x000DF143 File Offset: 0x000DD343
		public BarberState(BasicCharacterObject character, IFaceGeneratorCustomFilter filter)
		{
			this.Character = character;
			this.Filter = filter;
		}

		// Token: 0x04000F5A RID: 3930
		public BasicCharacterObject Character;
	}
}
