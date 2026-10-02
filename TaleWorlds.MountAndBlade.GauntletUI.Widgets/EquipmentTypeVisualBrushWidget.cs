using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200001C RID: 28
	public class EquipmentTypeVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000168 RID: 360 RVA: 0x00005FE6 File Offset: 0x000041E6
		public EquipmentTypeVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00005FFA File Offset: 0x000041FA
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._hasVisualDetermined)
			{
				this.RegisterBrushStatesOfWidget();
				this.UpdateVisual(this.Type);
				this._hasVisualDetermined = true;
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006024 File Offset: 0x00004224
		private void UpdateVisual(string type)
		{
			if (base.ContainsState(type))
			{
				this.SetState(type);
				return;
			}
			this.SetState("Invalid");
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00006042 File Offset: 0x00004242
		// (set) Token: 0x0600016C RID: 364 RVA: 0x0000604A File Offset: 0x0000424A
		[Editor(false)]
		public string Type
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
					base.OnPropertyChanged<string>(value, "Type");
				}
			}
		}

		// Token: 0x040000A6 RID: 166
		private bool _hasVisualDetermined;

		// Token: 0x040000A7 RID: 167
		private string _type = "";
	}
}
