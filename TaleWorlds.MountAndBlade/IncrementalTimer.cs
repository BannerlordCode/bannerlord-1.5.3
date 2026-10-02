using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000250 RID: 592
	public class IncrementalTimer
	{
		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002220 RID: 8736 RVA: 0x00077CAA File Offset: 0x00075EAA
		// (set) Token: 0x06002221 RID: 8737 RVA: 0x00077CB2 File Offset: 0x00075EB2
		public float TimerCounter { get; private set; }

		// Token: 0x06002222 RID: 8738 RVA: 0x00077CBC File Offset: 0x00075EBC
		public IncrementalTimer(float totalDuration, float tickInterval)
		{
			this._tickInterval = MathF.Max(tickInterval, 0.01f);
			this._totalDuration = MathF.Max(totalDuration, 0.01f);
			this.TimerCounter = 0f;
			this._timer = new Timer(Mission.Current.CurrentTime, this._tickInterval, true);
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x00077D18 File Offset: 0x00075F18
		public bool Check()
		{
			if (this._timer.Check(Mission.Current.CurrentTime))
			{
				this.TimerCounter += this._tickInterval / this._totalDuration;
				return true;
			}
			return false;
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x00077D4E File Offset: 0x00075F4E
		public bool HasEnded()
		{
			return this.TimerCounter >= 1f;
		}

		// Token: 0x04000D1C RID: 3356
		private readonly float _totalDuration;

		// Token: 0x04000D1D RID: 3357
		private readonly float _tickInterval;

		// Token: 0x04000D1E RID: 3358
		private readonly Timer _timer;
	}
}
