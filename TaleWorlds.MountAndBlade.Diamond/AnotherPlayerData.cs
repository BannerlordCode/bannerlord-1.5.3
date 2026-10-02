using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200012A RID: 298
	[Serializable]
	public class AnotherPlayerData
	{
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0000BC8F File Offset: 0x00009E8F
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x0000BC97 File Offset: 0x00009E97
		public AnotherPlayerState PlayerState { get; set; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x0000BCA0 File Offset: 0x00009EA0
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x0000BCA8 File Offset: 0x00009EA8
		public int Experience { get; set; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x0000BCB1 File Offset: 0x00009EB1
		// (set) Token: 0x060007C2 RID: 1986 RVA: 0x0000BCB9 File Offset: 0x00009EB9
		public CustomBattleId? SpectatableCustomBattleId { get; set; }

		// Token: 0x060007C3 RID: 1987 RVA: 0x0000BCC2 File Offset: 0x00009EC2
		public AnotherPlayerData()
		{
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0000BCCA File Offset: 0x00009ECA
		public AnotherPlayerData(AnotherPlayerState anotherPlayerState, int anotherPlayerExperience)
		{
			this.PlayerState = anotherPlayerState;
			this.Experience = anotherPlayerExperience;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0000BCE0 File Offset: 0x00009EE0
		public AnotherPlayerData(AnotherPlayerState anotherPlayerState, int anotherPlayerExperience, CustomBattleId? spectatableCustomBattleId)
		{
			this.PlayerState = anotherPlayerState;
			this.Experience = anotherPlayerExperience;
			this.SpectatableCustomBattleId = spectatableCustomBattleId;
		}
	}
}
