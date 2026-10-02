using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200001D RID: 29
	public class FiefProfitTypeVisualBrushWidget : BrushWidget
	{
		// Token: 0x0600016D RID: 365 RVA: 0x0000606D File Offset: 0x0000426D
		public FiefProfitTypeVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000607D File Offset: 0x0000427D
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

		// Token: 0x0600016F RID: 367 RVA: 0x000060A8 File Offset: 0x000042A8
		private void UpdateVisual(int type)
		{
			switch (type)
			{
			case 0:
				this.SetState("None");
				return;
			case 1:
				this.SetState("Tax");
				return;
			case 2:
				this.SetState("Tariff");
				return;
			case 3:
				this.SetState("Garrison");
				return;
			case 4:
				this.SetState("Village");
				return;
			case 5:
				this.SetState("Governor");
				return;
			default:
				return;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0000611B File Offset: 0x0000431B
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00006123 File Offset: 0x00004323
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

		// Token: 0x040000A8 RID: 168
		private bool _determinedVisual;

		// Token: 0x040000A9 RID: 169
		private int _type = -1;
	}
}
