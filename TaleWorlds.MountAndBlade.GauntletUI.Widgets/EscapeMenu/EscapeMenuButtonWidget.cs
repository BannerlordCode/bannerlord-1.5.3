using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.EscapeMenu
{
	// Token: 0x02000159 RID: 345
	public class EscapeMenuButtonWidget : ButtonWidget
	{
		// Token: 0x06001286 RID: 4742 RVA: 0x00033543 File Offset: 0x00031743
		public EscapeMenuButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x0003354C File Offset: 0x0003174C
		private void PositiveBehavioredStateUpdated()
		{
			if (this.IsPositiveBehaviored)
			{
				base.Brush = this.PositiveBehaviorBrush;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06001288 RID: 4744 RVA: 0x00033562 File Offset: 0x00031762
		// (set) Token: 0x06001289 RID: 4745 RVA: 0x0003356A File Offset: 0x0003176A
		[Editor(false)]
		public bool IsPositiveBehaviored
		{
			get
			{
				return this._isPositiveBehaviored;
			}
			set
			{
				if (this._isPositiveBehaviored != value)
				{
					this._isPositiveBehaviored = value;
					base.OnPropertyChanged(value, "IsPositiveBehaviored");
					this.PositiveBehavioredStateUpdated();
				}
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x0600128A RID: 4746 RVA: 0x0003358E File Offset: 0x0003178E
		// (set) Token: 0x0600128B RID: 4747 RVA: 0x00033596 File Offset: 0x00031796
		[Editor(false)]
		public Brush PositiveBehaviorBrush
		{
			get
			{
				return this._positiveBehaviorBrush;
			}
			set
			{
				if (this._positiveBehaviorBrush != value)
				{
					this._positiveBehaviorBrush = value;
					base.OnPropertyChanged<Brush>(value, "PositiveBehaviorBrush");
				}
			}
		}

		// Token: 0x04000879 RID: 2169
		private bool _isPositiveBehaviored;

		// Token: 0x0400087A RID: 2170
		private Brush _positiveBehaviorBrush;
	}
}
