using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000D8 RID: 216
	public class AgentLockVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000B17 RID: 2839 RVA: 0x0001F3B6 File Offset: 0x0001D5B6
		public AgentLockVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0001F3C8 File Offset: 0x0001D5C8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.ScaledPositionXOffset = this.Position.X - base.Size.X / 2f;
			base.ScaledPositionYOffset = this.Position.Y - base.Size.Y / 2f;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0001F428 File Offset: 0x0001D628
		private void UpdateVisualState(int lockState)
		{
			if (lockState == 0)
			{
				this.SetState("Possible");
				return;
			}
			if (lockState != 1)
			{
				return;
			}
			this.SetState("Active");
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x0001F449 File Offset: 0x0001D649
		// (set) Token: 0x06000B1B RID: 2843 RVA: 0x0001F451 File Offset: 0x0001D651
		[Editor(false)]
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position != value)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x0001F474 File Offset: 0x0001D674
		// (set) Token: 0x06000B1D RID: 2845 RVA: 0x0001F47C File Offset: 0x0001D67C
		[Editor(false)]
		public int LockState
		{
			get
			{
				return this._lockState;
			}
			set
			{
				if (this._lockState != value)
				{
					this._lockState = value;
					base.OnPropertyChanged(value, "LockState");
					this.UpdateVisualState(value);
				}
			}
		}

		// Token: 0x04000507 RID: 1287
		private Vec2 _position;

		// Token: 0x04000508 RID: 1288
		private int _lockState = -1;
	}
}
