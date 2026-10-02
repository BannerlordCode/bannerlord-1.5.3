using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x020002E9 RID: 745
	public class TournamentParticipant
	{
		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06002887 RID: 10375 RVA: 0x000A9008 File Offset: 0x000A7208
		// (set) Token: 0x06002888 RID: 10376 RVA: 0x000A9010 File Offset: 0x000A7210
		public int Score { get; private set; }

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06002889 RID: 10377 RVA: 0x000A9019 File Offset: 0x000A7219
		// (set) Token: 0x0600288A RID: 10378 RVA: 0x000A9021 File Offset: 0x000A7221
		public CharacterObject Character { get; private set; }

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x0600288B RID: 10379 RVA: 0x000A902A File Offset: 0x000A722A
		// (set) Token: 0x0600288C RID: 10380 RVA: 0x000A9032 File Offset: 0x000A7232
		public UniqueTroopDescriptor Descriptor { get; private set; }

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600288D RID: 10381 RVA: 0x000A903B File Offset: 0x000A723B
		// (set) Token: 0x0600288E RID: 10382 RVA: 0x000A9043 File Offset: 0x000A7243
		public TournamentTeam Team { get; private set; }

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x0600288F RID: 10383 RVA: 0x000A904C File Offset: 0x000A724C
		// (set) Token: 0x06002890 RID: 10384 RVA: 0x000A9054 File Offset: 0x000A7254
		public Equipment MatchEquipment { get; set; }

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06002891 RID: 10385 RVA: 0x000A905D File Offset: 0x000A725D
		// (set) Token: 0x06002892 RID: 10386 RVA: 0x000A9065 File Offset: 0x000A7265
		public bool IsAssigned { get; set; }

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06002893 RID: 10387 RVA: 0x000A906E File Offset: 0x000A726E
		public bool IsPlayer
		{
			get
			{
				CharacterObject character = this.Character;
				return character != null && character.IsPlayerCharacter;
			}
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000A9081 File Offset: 0x000A7281
		public TournamentParticipant(CharacterObject character, UniqueTroopDescriptor descriptor = default(UniqueTroopDescriptor))
		{
			this.Character = character;
			this.Descriptor = (descriptor.IsValid ? descriptor : new UniqueTroopDescriptor(Game.Current.NextUniqueTroopSeed));
			this.Team = null;
			this.IsAssigned = false;
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x000A90BF File Offset: 0x000A72BF
		public void SetTeam(TournamentTeam team)
		{
			this.Team = team;
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000A90C8 File Offset: 0x000A72C8
		public int AddScore(int score)
		{
			this.Score += score;
			return this.Score;
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000A90DE File Offset: 0x000A72DE
		public void ResetScore()
		{
			this.Score = 0;
		}
	}
}
