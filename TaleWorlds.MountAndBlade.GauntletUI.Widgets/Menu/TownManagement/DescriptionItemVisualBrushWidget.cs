using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x02000109 RID: 265
	public class DescriptionItemVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000E28 RID: 3624 RVA: 0x00026EAA File Offset: 0x000250AA
		public DescriptionItemVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00026EBA File Offset: 0x000250BA
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

		// Token: 0x06000E2A RID: 3626 RVA: 0x00026EE4 File Offset: 0x000250E4
		private void UpdateVisual(int type)
		{
			switch (type)
			{
			case 0:
				this.SetState("Gold");
				return;
			case 1:
				this.SetState("Production");
				return;
			case 2:
				this.SetState("Militia");
				return;
			case 3:
				this.SetState("Prosperity");
				return;
			case 4:
				this.SetState("Food");
				return;
			case 5:
				this.SetState("Loyalty");
				return;
			case 6:
				this.SetState("Security");
				return;
			case 7:
				this.SetState("Garrison");
				return;
			default:
				return;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06000E2B RID: 3627 RVA: 0x00026F77 File Offset: 0x00025177
		// (set) Token: 0x06000E2C RID: 3628 RVA: 0x00026F7F File Offset: 0x0002517F
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

		// Token: 0x04000670 RID: 1648
		private bool _determinedVisual;

		// Token: 0x04000671 RID: 1649
		private int _type = -1;
	}
}
