using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E3 RID: 739
	public class MonsterMissionData : IMonsterMissionData
	{
		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06002B2F RID: 11055 RVA: 0x000A6D52 File Offset: 0x000A4F52
		// (set) Token: 0x06002B30 RID: 11056 RVA: 0x000A6D5A File Offset: 0x000A4F5A
		public Monster Monster { get; private set; }

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002B31 RID: 11057 RVA: 0x000A6D63 File Offset: 0x000A4F63
		public CapsuleData BodyCapsule
		{
			get
			{
				return new CapsuleData(this.Monster.BodyCapsuleRadius, this.Monster.BodyCapsulePoint1, this.Monster.BodyCapsulePoint2);
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002B32 RID: 11058 RVA: 0x000A6D8B File Offset: 0x000A4F8B
		public CapsuleData CrouchedBodyCapsule
		{
			get
			{
				return new CapsuleData(this.Monster.CrouchedBodyCapsuleRadius, this.Monster.CrouchedBodyCapsulePoint1, this.Monster.CrouchedBodyCapsulePoint2);
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06002B33 RID: 11059 RVA: 0x000A6DB3 File Offset: 0x000A4FB3
		public MBActionSet ActionSet
		{
			get
			{
				if (!this._actionSet.IsValid && !string.IsNullOrEmpty(this.Monster.ActionSetCode))
				{
					this._actionSet = MBActionSet.GetActionSet(this.Monster.ActionSetCode);
				}
				return this._actionSet;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06002B34 RID: 11060 RVA: 0x000A6DF0 File Offset: 0x000A4FF0
		public MBActionSet FemaleActionSet
		{
			get
			{
				if (!this._femaleActionSet.IsValid && !string.IsNullOrEmpty(this.Monster.FemaleActionSetCode))
				{
					this._femaleActionSet = MBActionSet.GetActionSet(this.Monster.FemaleActionSetCode);
				}
				return this._femaleActionSet;
			}
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000A6E2D File Offset: 0x000A502D
		public MonsterMissionData(Monster monster)
		{
			this._actionSet = MBActionSet.InvalidActionSet;
			this._femaleActionSet = MBActionSet.InvalidActionSet;
			this.Monster = monster;
		}

		// Token: 0x0400106E RID: 4206
		private MBActionSet _actionSet;

		// Token: 0x0400106F RID: 4207
		private MBActionSet _femaleActionSet;
	}
}
