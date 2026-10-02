using System;

namespace TaleWorlds.MountAndBlade.DividableTasks
{
	// Token: 0x0200040D RID: 1037
	public class FormationSearchThreatTask : DividableTask
	{
		// Token: 0x060038A1 RID: 14497 RVA: 0x000E9998 File Offset: 0x000E7B98
		protected override bool UpdateExtra()
		{
			this._result = this._formation.HasUnitWithConditionLimitedRandom((Agent agent) => this._weapon.CanShootAtAgent(agent, 5), this._storedIndex, this._checkCountPerTick, out this._targetAgent);
			this._storedIndex += this._checkCountPerTick;
			return this._storedIndex >= this._formation.CountOfUnits || this._result;
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x000E9A02 File Offset: 0x000E7C02
		public void Prepare(Formation formation, RangedSiegeWeapon weapon)
		{
			base.ResetTaskStatus();
			this._formation = formation;
			this._weapon = weapon;
			this._storedIndex = 0;
			this._checkCountPerTick = (int)((float)this._formation.CountOfUnits * 0.1f) + 1;
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x000E9A3A File Offset: 0x000E7C3A
		public bool GetResult(out Agent targetAgent)
		{
			targetAgent = this._targetAgent;
			return this._result;
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x000E9A4A File Offset: 0x000E7C4A
		public FormationSearchThreatTask()
			: base(null)
		{
		}

		// Token: 0x04001859 RID: 6233
		private Agent _targetAgent;

		// Token: 0x0400185A RID: 6234
		private const float CheckCountRatio = 0.1f;

		// Token: 0x0400185B RID: 6235
		private RangedSiegeWeapon _weapon;

		// Token: 0x0400185C RID: 6236
		private Formation _formation;

		// Token: 0x0400185D RID: 6237
		private int _storedIndex;

		// Token: 0x0400185E RID: 6238
		private int _checkCountPerTick;

		// Token: 0x0400185F RID: 6239
		private bool _result;
	}
}
