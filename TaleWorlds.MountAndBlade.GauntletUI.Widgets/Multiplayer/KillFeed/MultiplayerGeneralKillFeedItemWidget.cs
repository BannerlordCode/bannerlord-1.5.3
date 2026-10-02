using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.KillFeed
{
	// Token: 0x020000BF RID: 191
	public class MultiplayerGeneralKillFeedItemWidget : Widget
	{
		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0001C4D1 File Offset: 0x0001A6D1
		// (set) Token: 0x06000A13 RID: 2579 RVA: 0x0001C4D9 File Offset: 0x0001A6D9
		public float TimeSinceCreation { get; private set; }

		// Token: 0x06000A14 RID: 2580 RVA: 0x0001C4E2 File Offset: 0x0001A6E2
		public MultiplayerGeneralKillFeedItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0001C4F8 File Offset: 0x0001A6F8
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				this.SetGlobalAlphaRecursively(0f);
				this._initialized = true;
			}
			this.TimeSinceCreation += dt * this._speedModifier;
			if (this.TimeSinceCreation <= 0.15f)
			{
				this.SetGlobalAlphaRecursively(Mathf.Lerp(base.AlphaFactor, 1f, this.TimeSinceCreation / 0.15f));
				return;
			}
			if (this.TimeSinceCreation - 0.15f <= 3.5f)
			{
				this.SetGlobalAlphaRecursively(1f);
				return;
			}
			if (this.TimeSinceCreation - 3.65f <= 1f)
			{
				this.SetGlobalAlphaRecursively(Mathf.Lerp(base.AlphaFactor, 0f, (this.TimeSinceCreation - 3.65f) / 1f));
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

		// Token: 0x06000A16 RID: 2582 RVA: 0x0001C5F3 File Offset: 0x0001A7F3
		public void SetSpeedModifier(float newSpeed)
		{
			if (newSpeed > this._speedModifier)
			{
				this._speedModifier = newSpeed;
			}
		}

		// Token: 0x0400048B RID: 1163
		private const float FadeInTime = 0.15f;

		// Token: 0x0400048C RID: 1164
		private const float StayTime = 3.5f;

		// Token: 0x0400048D RID: 1165
		private const float FadeOutTime = 1f;

		// Token: 0x0400048E RID: 1166
		private float _speedModifier = 1f;

		// Token: 0x04000490 RID: 1168
		private bool _initialized;
	}
}
