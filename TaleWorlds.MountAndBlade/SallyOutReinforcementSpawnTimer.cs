using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A0 RID: 672
	public class SallyOutReinforcementSpawnTimer : ICustomReinforcementSpawnTimer
	{
		// Token: 0x0600255D RID: 9565 RVA: 0x00088255 File Offset: 0x00086455
		public SallyOutReinforcementSpawnTimer(float besiegedInterval, float besiegerInterval, float besiegerIntervalChange, int besiegerIntervalChangeCount)
		{
			this._besiegedSideTimer = new BasicMissionTimer();
			this._besiegedInterval = besiegedInterval;
			this._besiegerSideTimer = new BasicMissionTimer();
			this._besiegerInterval = besiegerInterval;
			this._besiegerIntervalChange = besiegerIntervalChange;
			this._besiegerRemainingIntervalChanges = besiegerIntervalChangeCount;
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x00088290 File Offset: 0x00086490
		public bool Check(BattleSideEnum side)
		{
			if (side == BattleSideEnum.Attacker)
			{
				if (this._besiegerSideTimer.ElapsedTime >= this._besiegerInterval)
				{
					if (this._besiegerRemainingIntervalChanges > 0)
					{
						this._besiegerInterval -= this._besiegerIntervalChange;
						this._besiegerRemainingIntervalChanges--;
					}
					this._besiegerSideTimer.Reset();
					return true;
				}
			}
			else if (side == BattleSideEnum.Defender && this._besiegedSideTimer.ElapsedTime >= this._besiegedInterval)
			{
				this._besiegedSideTimer.Reset();
				return true;
			}
			return false;
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x0008830F File Offset: 0x0008650F
		public void ResetTimer(BattleSideEnum side)
		{
			if (side == BattleSideEnum.Attacker)
			{
				this._besiegerSideTimer.Reset();
				return;
			}
			if (side == BattleSideEnum.Defender)
			{
				this._besiegedSideTimer.Reset();
			}
		}

		// Token: 0x04000E7C RID: 3708
		private BasicMissionTimer _besiegedSideTimer;

		// Token: 0x04000E7D RID: 3709
		private BasicMissionTimer _besiegerSideTimer;

		// Token: 0x04000E7E RID: 3710
		private float _besiegedInterval;

		// Token: 0x04000E7F RID: 3711
		private float _besiegerInterval;

		// Token: 0x04000E80 RID: 3712
		private float _besiegerIntervalChange;

		// Token: 0x04000E81 RID: 3713
		private int _besiegerRemainingIntervalChanges;
	}
}
