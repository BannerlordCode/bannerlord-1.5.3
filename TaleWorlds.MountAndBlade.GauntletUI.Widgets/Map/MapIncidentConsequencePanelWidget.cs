using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map
{
	// Token: 0x0200011C RID: 284
	public class MapIncidentConsequencePanelWidget : CircularAutoScrollablePanelWidget
	{
		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x00029C9D File Offset: 0x00027E9D
		// (set) Token: 0x06000F20 RID: 3872 RVA: 0x00029CA5 File Offset: 0x00027EA5
		public Widget OptionsList { get; set; }

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x00029CAE File Offset: 0x00027EAE
		// (set) Token: 0x06000F22 RID: 3874 RVA: 0x00029CB6 File Offset: 0x00027EB6
		public float AnimationSpeed { get; set; } = 10f;

		// Token: 0x06000F23 RID: 3875 RVA: 0x00029CBF File Offset: 0x00027EBF
		public MapIncidentConsequencePanelWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00029CDC File Offset: 0x00027EDC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isFirstFrame)
			{
				this._initialHeight = base.SuggestedHeight;
				this._isFirstFrame = false;
			}
			Widget activeOption = this.GetActiveOption();
			if (activeOption != this._previousActiveOption)
			{
				this._previousActiveOption = activeOption;
				return;
			}
			float num = base.InnerPanel.Size.Y * base._inverseScaleToUse + base.ClipRect.MarginTop + base.ClipRect.MarginBottom;
			float num2 = base.ParentWidget.Size.Y * base._inverseScaleToUse;
			bool flag = num > this._initialHeight && (base.IsHovered || (activeOption != null && activeOption.IsHovered));
			float num3 = (flag ? MathF.Clamp(num, this._initialHeight, num2) : this._initialHeight);
			if (flag && num3 == num)
			{
				base.StopScrolling();
			}
			base.SuggestedHeight = MathF.Lerp(base.SuggestedHeight, num3, MathF.Min(this.AnimationSpeed * dt, 1f), 0.01f);
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00029DE0 File Offset: 0x00027FE0
		private Widget GetActiveOption()
		{
			Widget widget = null;
			for (int i = 0; i < this.OptionsList.ChildCount; i++)
			{
				Widget widget2 = this.OptionsList.Children[i];
				if (widget2.IsHovered)
				{
					return widget2;
				}
				ButtonWidget buttonWidget;
				if ((buttonWidget = widget2 as ButtonWidget) != null && buttonWidget.IsSelected)
				{
					widget = widget2;
				}
			}
			return widget;
		}

		// Token: 0x040006EB RID: 1771
		private float _initialHeight;

		// Token: 0x040006EC RID: 1772
		private bool _isFirstFrame = true;

		// Token: 0x040006ED RID: 1773
		private Widget _previousActiveOption;
	}
}
