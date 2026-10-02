using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200000E RID: 14
	public class ClickableCharacterTableauWidget : CharacterTableauWidget
	{
		// Token: 0x060000AF RID: 175 RVA: 0x00003B34 File Offset: 0x00001D34
		public ClickableCharacterTableauWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003B48 File Offset: 0x00001D48
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._isMouseDown && !this._isDragging && (this._mousePressPos - base.EventManager.MousePosition).LengthSquared >= this._dragThresholdSqr)
			{
				this._isDragging = true;
				base.SetTextureProviderProperty("CurrentlyRotating", true);
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003BAF File Offset: 0x00001DAF
		protected override void OnMousePressed()
		{
			this._isMouseDown = true;
			this._mousePressPos = base.EventManager.MousePosition;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003BCE File Offset: 0x00001DCE
		protected override void OnMouseReleased(bool isFromInput)
		{
			base.SetTextureProviderProperty("CurrentlyRotating", false);
			if (!this._isDragging && isFromInput)
			{
				base.EventFired("Click", Array.Empty<object>());
			}
			this._isDragging = false;
			this._isMouseDown = false;
		}

		// Token: 0x04000052 RID: 82
		private const float DragThreshold = 5f;

		// Token: 0x04000053 RID: 83
		private float _dragThresholdSqr = 25f;

		// Token: 0x04000054 RID: 84
		private bool _isMouseDown;

		// Token: 0x04000055 RID: 85
		private bool _isDragging;

		// Token: 0x04000056 RID: 86
		private Vec2 _mousePressPos;
	}
}
