using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000D7 RID: 215
	public class Timer
	{
		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x00024AC5 File Offset: 0x00022CC5
		// (set) Token: 0x06000B3B RID: 2875 RVA: 0x00024ACD File Offset: 0x00022CCD
		public float StartTime { get; protected set; }

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x00024AD6 File Offset: 0x00022CD6
		// (set) Token: 0x06000B3D RID: 2877 RVA: 0x00024ADE File Offset: 0x00022CDE
		public float Duration { get; protected set; }

		// Token: 0x06000B3E RID: 2878 RVA: 0x00024AE7 File Offset: 0x00022CE7
		public Timer(float gameTime, float duration, bool autoReset = true)
		{
			this.StartTime = gameTime;
			this._latestGameTime = gameTime;
			this._autoReset = autoReset;
			this.Duration = duration;
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00024B0C File Offset: 0x00022D0C
		public virtual bool Check(float gameTime)
		{
			this._latestGameTime = gameTime;
			if (this.Duration <= 0f)
			{
				this.PreviousDeltaTime = this.ElapsedTime();
				this.StartTime = gameTime;
				return true;
			}
			bool flag = false;
			if (this.ElapsedTime() >= this.Duration)
			{
				this.PreviousDeltaTime = this.ElapsedTime();
				if (this._autoReset)
				{
					while (this.ElapsedTime() >= this.Duration)
					{
						this.StartTime += this.Duration;
					}
				}
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00024B8C File Offset: 0x00022D8C
		public float ElapsedTime()
		{
			return this._latestGameTime - this.StartTime;
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x00024B9B File Offset: 0x00022D9B
		// (set) Token: 0x06000B42 RID: 2882 RVA: 0x00024BA3 File Offset: 0x00022DA3
		public float PreviousDeltaTime { get; private set; }

		// Token: 0x06000B43 RID: 2883 RVA: 0x00024BAC File Offset: 0x00022DAC
		public void Reset(float gameTime)
		{
			this.Reset(gameTime, this.Duration);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00024BBB File Offset: 0x00022DBB
		public void Reset(float gameTime, float newDuration)
		{
			this.StartTime = gameTime;
			this._latestGameTime = gameTime;
			this.Duration = newDuration;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00024BD2 File Offset: 0x00022DD2
		public void AdjustStartTime(float deltaTime)
		{
			this.StartTime += deltaTime;
		}

		// Token: 0x0400065D RID: 1629
		private float _latestGameTime;

		// Token: 0x0400065E RID: 1630
		private bool _autoReset;
	}
}
