using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000DF RID: 223
	public class DisguiseMarkerBrushWidget : BrushWidget
	{
		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x000201D9 File Offset: 0x0001E3D9
		// (set) Token: 0x06000B7D RID: 2941 RVA: 0x000201E1 File Offset: 0x0001E3E1
		public Vec2 Position { get; set; }

		// Token: 0x06000B7E RID: 2942 RVA: 0x000201EA File Offset: 0x0001E3EA
		public DisguiseMarkerBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x000201F4 File Offset: 0x0001E3F4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.ScaledPositionYOffset = this.Position.y - base.Size.Y / 2f;
			base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0002024E File Offset: 0x0001E44E
		private void UpdateState()
		{
			this.SetState(this.OffenseTypeIdentifier);
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x0002025C File Offset: 0x0001E45C
		// (set) Token: 0x06000B82 RID: 2946 RVA: 0x00020264 File Offset: 0x0001E464
		public string OffenseTypeIdentifier
		{
			get
			{
				return this._offenseTypeIdentifier;
			}
			set
			{
				if (value != this._offenseTypeIdentifier)
				{
					this._offenseTypeIdentifier = value;
					base.OnPropertyChanged<string>(value, "OffenseTypeIdentifier");
					this.UpdateState();
				}
			}
		}

		// Token: 0x04000530 RID: 1328
		private string _offenseTypeIdentifier;
	}
}
