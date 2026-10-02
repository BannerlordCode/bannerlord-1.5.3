using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.DamageFeed
{
	// Token: 0x02000104 RID: 260
	public class MissionAgentDamageFeedItemWidget : Widget
	{
		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06000E03 RID: 3587 RVA: 0x0002697A File Offset: 0x00024B7A
		// (set) Token: 0x06000E04 RID: 3588 RVA: 0x00026982 File Offset: 0x00024B82
		public float FadeInTime { get; set; } = 0.1f;

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06000E05 RID: 3589 RVA: 0x0002698B File Offset: 0x00024B8B
		// (set) Token: 0x06000E06 RID: 3590 RVA: 0x00026993 File Offset: 0x00024B93
		public float StayTime { get; set; } = 1.5f;

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06000E07 RID: 3591 RVA: 0x0002699C File Offset: 0x00024B9C
		// (set) Token: 0x06000E08 RID: 3592 RVA: 0x000269A4 File Offset: 0x00024BA4
		public float FadeOutTime { get; set; } = 0.3f;

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06000E09 RID: 3593 RVA: 0x000269AD File Offset: 0x00024BAD
		// (set) Token: 0x06000E0A RID: 3594 RVA: 0x000269B5 File Offset: 0x00024BB5
		public float TimeSinceCreation { get; private set; }

		// Token: 0x06000E0B RID: 3595 RVA: 0x000269BE File Offset: 0x00024BBE
		public MissionAgentDamageFeedItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x000269F3 File Offset: 0x00024BF3
		public void ShowFeed()
		{
			this._isShown = true;
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x000269FC File Offset: 0x00024BFC
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._isInitialized)
			{
				this.SetGlobalAlphaRecursively(0f);
				this._isInitialized = true;
			}
			if (this._isShown)
			{
				this.TimeSinceCreation += dt * this._speedModifier;
				if (this.TimeSinceCreation <= this.FadeInTime)
				{
					this.SetGlobalAlphaRecursively(Mathf.Lerp(base.AlphaFactor, 1f, this.TimeSinceCreation / this.FadeInTime));
					return;
				}
				if (this.TimeSinceCreation - this.FadeInTime <= this.StayTime)
				{
					this.SetGlobalAlphaRecursively(1f);
					return;
				}
				if (this.TimeSinceCreation - (this.FadeInTime + this.StayTime) <= this.FadeOutTime)
				{
					this.SetGlobalAlphaRecursively(Mathf.Lerp(base.AlphaFactor, 0f, (this.TimeSinceCreation - (this.FadeInTime + this.StayTime)) / this.FadeOutTime));
					if (base.AlphaFactor <= 0.1f)
					{
						base.EventFired("OnRemove", Array.Empty<object>());
						return;
					}
				}
				else
				{
					base.EventFired("OnRemove", Array.Empty<object>());
				}
			}
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00026B18 File Offset: 0x00024D18
		public void SetSpeedModifier(float newSpeed)
		{
			if (newSpeed > this._speedModifier)
			{
				this._speedModifier = newSpeed;
			}
		}

		// Token: 0x04000661 RID: 1633
		private float _speedModifier = 1f;

		// Token: 0x04000663 RID: 1635
		private bool _isInitialized;

		// Token: 0x04000664 RID: 1636
		private bool _isShown;
	}
}
