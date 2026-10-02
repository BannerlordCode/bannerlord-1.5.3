using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x0200004C RID: 76
	public class TutorialObjectiveItemWidget : Widget
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x0000D32A File Offset: 0x0000B52A
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x0000D332 File Offset: 0x0000B532
		public InputKeyVisualWidget KeyPressWidget { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x0000D33B File Offset: 0x0000B53B
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x0000D343 File Offset: 0x0000B543
		public TutorialObjectiveMouseParentWidget MouseMoveWidget { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x0000D34C File Offset: 0x0000B54C
		// (set) Token: 0x06000433 RID: 1075 RVA: 0x0000D354 File Offset: 0x0000B554
		public TutorialObjectiveStickParentWidget StickMoveWidget { get; set; }

		// Token: 0x06000434 RID: 1076 RVA: 0x0000D35D File Offset: 0x0000B55D
		public TutorialObjectiveItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000D368 File Offset: 0x0000B568
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.KeyPressWidget.IsVisible = this.InputType == 1;
			this.MouseMoveWidget.IsVisible = this.InputType == 0;
			this.StickMoveWidget.IsVisible = this.InputType == 2;
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0000D3B8 File Offset: 0x0000B5B8
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x0000D3C0 File Offset: 0x0000B5C0
		[Editor(false)]
		public int MovementType
		{
			get
			{
				return this._movementType;
			}
			set
			{
				if (value != this._movementType)
				{
					this._movementType = value;
					base.OnPropertyChanged(value, "MovementType");
				}
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0000D3DE File Offset: 0x0000B5DE
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x0000D3E6 File Offset: 0x0000B5E6
		[Editor(false)]
		public int InputType
		{
			get
			{
				return this._inputType;
			}
			set
			{
				if (value != this._inputType)
				{
					this._inputType = value;
					base.OnPropertyChanged(value, "InputType");
				}
			}
		}

		// Token: 0x040001C1 RID: 449
		private int _movementType;

		// Token: 0x040001C2 RID: 450
		private int _inputType;

		// Token: 0x020001A9 RID: 425
		public enum InputTypes
		{
			// Token: 0x040009F0 RID: 2544
			MouseAndClick,
			// Token: 0x040009F1 RID: 2545
			Key,
			// Token: 0x040009F2 RID: 2546
			ControllerStick
		}
	}
}
