using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000E0 RID: 224
	public class FormationFocusedMarkerWidget : BrushWidget
	{
		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x0002028D File Offset: 0x0001E48D
		// (set) Token: 0x06000B84 RID: 2948 RVA: 0x00020295 File Offset: 0x0001E495
		public int NormalSize { get; set; } = 55;

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x0002029E File Offset: 0x0001E49E
		// (set) Token: 0x06000B86 RID: 2950 RVA: 0x000202A6 File Offset: 0x0001E4A6
		public int FocusedSize { get; set; } = 60;

		// Token: 0x06000B87 RID: 2951 RVA: 0x000202AF File Offset: 0x0001E4AF
		public FormationFocusedMarkerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x000202C8 File Offset: 0x0001E4C8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.UpdateVisibility();
			if (base.IsVisible)
			{
				this.UpdateSize();
			}
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x000202E5 File Offset: 0x0001E4E5
		private void UpdateVisibility()
		{
			base.IsVisible = this.IsTargetingAFormation || (this.IsFormationTargetRelevant && this.IsCenterOfFocus);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002030C File Offset: 0x0001E50C
		private void UpdateSize()
		{
			float num4;
			if (this.IsCenterOfFocus)
			{
				float num = (float)(this.IsTargetingAFormation ? (this.FocusedSize + 3) : this.FocusedSize);
				float num2 = MathF.Sin(base.EventManager.Time * 5f);
				num2 = (num2 + 1f) / 2f;
				float num3 = (num - (float)this.NormalSize) * num2;
				num4 = (float)this.NormalSize + num3;
			}
			else
			{
				num4 = (float)this.NormalSize;
			}
			base.ScaledSuggestedHeight = num4 * base._scaleToUse;
			base.ScaledSuggestedWidth = num4 * base._scaleToUse;
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00020399 File Offset: 0x0001E599
		private void UpdateState()
		{
			this.SetState(this.IsTargetingAFormation ? "Targeting" : "Default");
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x000203B5 File Offset: 0x0001E5B5
		// (set) Token: 0x06000B8D RID: 2957 RVA: 0x000203BD File Offset: 0x0001E5BD
		public bool IsCenterOfFocus
		{
			get
			{
				return this._isCenterOfFocus;
			}
			set
			{
				if (this._isCenterOfFocus != value)
				{
					this._isCenterOfFocus = value;
					base.OnPropertyChanged(value, "IsCenterOfFocus");
				}
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x000203DB File Offset: 0x0001E5DB
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x000203E3 File Offset: 0x0001E5E3
		public bool IsFormationTargetRelevant
		{
			get
			{
				return this._isFormationTargetRelevant;
			}
			set
			{
				if (this._isFormationTargetRelevant != value)
				{
					this._isFormationTargetRelevant = value;
					base.OnPropertyChanged(value, "IsFormationTargetRelevant");
				}
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x00020401 File Offset: 0x0001E601
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x00020409 File Offset: 0x0001E609
		public bool IsTargetingAFormation
		{
			get
			{
				return this._isTargetingAFormation;
			}
			set
			{
				if (this._isTargetingAFormation != value)
				{
					this._isTargetingAFormation = value;
					base.OnPropertyChanged(value, "IsTargetingAFormation");
					this.UpdateState();
				}
			}
		}

		// Token: 0x04000533 RID: 1331
		private bool _isCenterOfFocus;

		// Token: 0x04000534 RID: 1332
		private bool _isFormationTargetRelevant;

		// Token: 0x04000535 RID: 1333
		private bool _isTargetingAFormation;
	}
}
