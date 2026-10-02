using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Quest
{
	// Token: 0x02000060 RID: 96
	public class QuestStageItemWidget : Widget
	{
		// Token: 0x06000546 RID: 1350 RVA: 0x00010056 File Offset: 0x0000E256
		public QuestStageItemWidget(UIContext context)
			: base(context)
		{
			this._firstFrame = true;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00010068 File Offset: 0x0000E268
		protected override void OnParallelUpdate(float dt)
		{
			base.OnParallelUpdate(dt);
			this._previousHoverBegan = this._hoverBegan;
			if (!this._firstFrame && this.IsNew)
			{
				bool flag = this.IsMouseOverWidget();
				if (flag && !this._hoverBegan)
				{
					this._hoverBegan = true;
				}
				else if (!flag && this._hoverBegan)
				{
					this._hoverBegan = false;
				}
			}
			this._firstFrame = false;
			if (this._previousHoverBegan && !this._hoverBegan)
			{
				base.EventFired("ResetGlow", Array.Empty<object>());
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x000100EC File Offset: 0x0000E2EC
		private bool IsMouseOverWidget()
		{
			Vector2 globalPosition = base.GlobalPosition;
			return this.IsBetween(base.EventManager.MousePosition.X, globalPosition.X, globalPosition.X + base.Size.X) && this.IsBetween(base.EventManager.MousePosition.Y, globalPosition.Y, globalPosition.Y + base.Size.Y);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00010160 File Offset: 0x0000E360
		private bool IsBetween(float number, float min, float max)
		{
			return number >= min && number <= max;
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x0001016F File Offset: 0x0000E36F
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x00010177 File Offset: 0x0000E377
		[Editor(false)]
		public bool IsNew
		{
			get
			{
				return this._isNew;
			}
			set
			{
				if (this._isNew != value)
				{
					this._isNew = value;
					base.OnPropertyChanged(value, "IsNew");
				}
			}
		}

		// Token: 0x04000241 RID: 577
		private bool _firstFrame;

		// Token: 0x04000242 RID: 578
		private bool _previousHoverBegan;

		// Token: 0x04000243 RID: 579
		private bool _hoverBegan;

		// Token: 0x04000244 RID: 580
		private bool _isNew;
	}
}
