using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000135 RID: 309
	public class KingdomCardItemContainerWidget : Widget
	{
		// Token: 0x06001039 RID: 4153 RVA: 0x0002CC3F File Offset: 0x0002AE3F
		public KingdomCardItemContainerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0002CC5E File Offset: 0x0002AE5E
		protected override void OnBeforeChildRemoved(Widget child)
		{
			base.OnBeforeChildRemoved(child);
			child.EventFire -= this.ChildrenWidgetEventFired;
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0002CC79 File Offset: 0x0002AE79
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.EventFire += this.ChildrenWidgetEventFired;
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0002CC94 File Offset: 0x0002AE94
		private void ChildrenWidgetEventFired(Widget widget, string eventName, object[] args)
		{
			if (eventName == "HoverBegin")
			{
				this._isMouseOverChildren = true;
				widget.RenderLate = true;
				return;
			}
			if (eventName == "HoverEnd")
			{
				this._isMouseOverChildren = false;
				widget.RenderLate = false;
			}
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0002CCD0 File Offset: 0x0002AED0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			float num = 0f;
			float num2 = 0f;
			if (base.ChildCount > 0)
			{
				num = base.GetChild(0).Size.X * (float)base.ChildCount;
				num2 = this._defaultXOffset * base._inverseScaleToUse * (float)(base.ChildCount - 1) + base.GetChild(0).Size.X;
				base.IsEnabled = true;
			}
			else
			{
				base.IsEnabled = false;
			}
			for (int i = 0; i < base.ChildCount; i++)
			{
				Widget child = base.GetChild(i);
				if (this._isMouseOverChildren || this._isMouseOverSelf)
				{
					if (base.ChildCount > 1)
					{
						if (num < base.Size.X)
						{
							float num3 = base.Size.X / 2f - num / 2f;
							this._targetXOffset = (float)i * child.Size.X + num3;
						}
						else
						{
							this._targetXOffset = (float)i / ((float)base.ChildCount - 1f) * (base.Size.X - child.Size.X);
						}
					}
					else if (base.ChildCount == 1)
					{
						this._targetXOffset = base.Size.X / 2f - child.Size.X / 2f;
					}
				}
				else if (base.ChildCount > 1)
				{
					float num4 = this._defaultXOffset;
					while (num2 > base.Size.X && num4 > 5f)
					{
						num4 -= 0.5f;
						num2 = num4 * (float)(base.ChildCount - 1) + child.Size.X;
					}
					this._targetXOffset = base.Size.X / 2f - num2 / 2f + num4 * (float)i;
				}
				else if (base.ChildCount == 1)
				{
					this._targetXOffset = base.Size.X / 2f - child.Size.X / 2f;
				}
				child.PositionXOffset = Mathf.Lerp(child.PositionXOffset, this._targetXOffset * base._inverseScaleToUse, dt * this._lerpFactor);
			}
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0002CF06 File Offset: 0x0002B106
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
			this._isMouseOverSelf = true;
			base.RenderLate = true;
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0002CF1C File Offset: 0x0002B11C
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
			this._isMouseOverSelf = false;
			base.RenderLate = false;
		}

		// Token: 0x04000765 RID: 1893
		private float _targetXOffset;

		// Token: 0x04000766 RID: 1894
		private bool _isMouseOverChildren;

		// Token: 0x04000767 RID: 1895
		private bool _isMouseOverSelf;

		// Token: 0x04000768 RID: 1896
		private float _lerpFactor = 15f;

		// Token: 0x04000769 RID: 1897
		private float _defaultXOffset = 20f;
	}
}
