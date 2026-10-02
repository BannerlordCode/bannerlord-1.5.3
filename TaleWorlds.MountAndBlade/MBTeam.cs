using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E5 RID: 485
	public struct MBTeam
	{
		// Token: 0x06001CB4 RID: 7348 RVA: 0x0006212B File Offset: 0x0006032B
		internal MBTeam(Mission mission, int index)
		{
			this._mission = mission;
			this.Index = index;
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001CB5 RID: 7349 RVA: 0x0006213B File Offset: 0x0006033B
		public static MBTeam InvalidTeam
		{
			get
			{
				return new MBTeam(null, -1);
			}
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x00062144 File Offset: 0x00060344
		public override int GetHashCode()
		{
			return this.Index;
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x0006214C File Offset: 0x0006034C
		public override bool Equals(object obj)
		{
			return ((MBTeam)obj).Index == this.Index;
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x00062161 File Offset: 0x00060361
		public static bool operator ==(MBTeam team1, MBTeam team2)
		{
			return team1.Index == team2.Index;
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x00062171 File Offset: 0x00060371
		public static bool operator !=(MBTeam team1, MBTeam team2)
		{
			return team1.Index != team2.Index;
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001CBA RID: 7354 RVA: 0x00062184 File Offset: 0x00060384
		public bool IsValid
		{
			get
			{
				return this.Index >= 0;
			}
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x00062192 File Offset: 0x00060392
		public bool IsEnemyOf(MBTeam otherTeam)
		{
			return MBAPI.IMBTeam.IsEnemy(this._mission.Pointer, this.Index, otherTeam.Index);
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x000621B5 File Offset: 0x000603B5
		public void SetIsEnemyOf(MBTeam otherTeam, bool isEnemyOf)
		{
			MBAPI.IMBTeam.SetIsEnemy(this._mission.Pointer, this.Index, otherTeam.Index, isEnemyOf);
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x000621D9 File Offset: 0x000603D9
		public override string ToString()
		{
			return "Mission Team: " + this.Index;
		}

		// Token: 0x040009A8 RID: 2472
		public readonly int Index;

		// Token: 0x040009A9 RID: 2473
		private readonly Mission _mission;
	}
}
