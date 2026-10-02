using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000014 RID: 20
	public class StateSyncWidget : BrushWidget
	{
		// Token: 0x0600012A RID: 298 RVA: 0x00006B98 File Offset: 0x00004D98
		public StateSyncWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006BA1 File Offset: 0x00004DA1
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			Widget widget = this.TargetWidget ?? this;
			Widget sourceWidget = this.SourceWidget;
			widget.SetState(((sourceWidget != null) ? sourceWidget.CurrentState : null) ?? "Default");
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00006BD5 File Offset: 0x00004DD5
		// (set) Token: 0x0600012D RID: 301 RVA: 0x00006BDD File Offset: 0x00004DDD
		[Editor(false)]
		public Widget SourceWidget
		{
			get
			{
				return this._sourceWidget;
			}
			set
			{
				if (this._sourceWidget != value)
				{
					this._sourceWidget = value;
					base.OnPropertyChanged<Widget>(value, "SourceWidget");
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00006BFB File Offset: 0x00004DFB
		// (set) Token: 0x0600012F RID: 303 RVA: 0x00006C03 File Offset: 0x00004E03
		[Editor(false)]
		public Widget TargetWidget
		{
			get
			{
				return this._targetWidget;
			}
			set
			{
				if (this._targetWidget != value)
				{
					this._targetWidget = value;
					base.OnPropertyChanged<Widget>(value, "TargetWidget");
				}
			}
		}

		// Token: 0x0400008C RID: 140
		private Widget _sourceWidget;

		// Token: 0x0400008D RID: 141
		private Widget _targetWidget;
	}
}
