using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200001E RID: 30
	public class FiefStatTypeVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000172 RID: 370 RVA: 0x00006141 File Offset: 0x00004341
		public FiefStatTypeVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00006151 File Offset: 0x00004351
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._determinedVisual)
			{
				this.RegisterBrushStatesOfWidget();
				this.UpdateVisual(this.Type);
				this._determinedVisual = true;
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000617C File Offset: 0x0000437C
		private void UpdateVisual(int type)
		{
			switch (type)
			{
			case 0:
				this.SetState("None");
				return;
			case 1:
				this.SetState("Wall");
				return;
			case 2:
				this.SetState("Garrison");
				return;
			case 3:
				this.SetState("Militia");
				return;
			case 4:
				this.SetState("Prosperity");
				return;
			case 5:
				this.SetState("Food");
				return;
			case 6:
				this.SetState("Loyalty");
				return;
			case 7:
				this.SetState("Security");
				return;
			case 8:
				this.SetState("Shipyard");
				return;
			case 9:
				this.SetState("Patrol");
				return;
			case 10:
				this.SetState("CoastalPatrol");
				return;
			default:
				return;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000175 RID: 373 RVA: 0x0000623F File Offset: 0x0000443F
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00006247 File Offset: 0x00004447
		[Editor(false)]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._type != value)
				{
					this._type = value;
					base.OnPropertyChanged(value, "Type");
				}
			}
		}

		// Token: 0x040000AA RID: 170
		private bool _determinedVisual;

		// Token: 0x040000AB RID: 171
		private int _type = -1;
	}
}
