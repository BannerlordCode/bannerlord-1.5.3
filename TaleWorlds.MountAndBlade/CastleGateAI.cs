using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200015E RID: 350
	public class CastleGateAI : UsableMachineAIBase
	{
		// Token: 0x06001270 RID: 4720 RVA: 0x00039B86 File Offset: 0x00037D86
		public void ResetInitialGateState(CastleGate.GateState newInitialState)
		{
			this._initialState = newInitialState;
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00039B8F File Offset: 0x00037D8F
		public CastleGateAI(CastleGate gate)
			: base(gate)
		{
			this._initialState = gate.State;
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x00039BA4 File Offset: 0x00037DA4
		public override bool HasActionCompleted
		{
			get
			{
				return ((CastleGate)this.UsableMachine).State != this._initialState;
			}
		}

		// Token: 0x04000482 RID: 1154
		private CastleGate.GateState _initialState;
	}
}
