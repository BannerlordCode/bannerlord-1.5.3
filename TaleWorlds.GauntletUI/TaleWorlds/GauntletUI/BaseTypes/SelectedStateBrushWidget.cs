using System;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000066 RID: 102
	public class SelectedStateBrushWidget : BrushWidget
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x0001E5C4 File Offset: 0x0001C7C4
		public SelectedStateBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001E5E0 File Offset: 0x0001C7E0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isBrushStatesRegistered)
			{
				this.RegisterBrushStatesOfWidget();
				this._isBrushStatesRegistered = true;
			}
			if (this._isDirty)
			{
				this.SetState(this.SelectedState ?? "Default");
				this._isDirty = false;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x0001E62D File Offset: 0x0001C82D
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x0001E635 File Offset: 0x0001C835
		[Editor(false)]
		public string SelectedState
		{
			get
			{
				return this._selectedState;
			}
			set
			{
				if (this._selectedState != value)
				{
					this._selectedState = value;
					base.OnPropertyChanged<string>(value, "SelectedState");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x04000349 RID: 841
		private bool _isDirty = true;

		// Token: 0x0400034A RID: 842
		private bool _isBrushStatesRegistered;

		// Token: 0x0400034B RID: 843
		private string _selectedState = "Default";
	}
}
