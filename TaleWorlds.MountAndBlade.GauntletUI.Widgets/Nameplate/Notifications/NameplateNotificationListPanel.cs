using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate.Notifications
{
	// Token: 0x02000084 RID: 132
	public class NameplateNotificationListPanel : ListPanel
	{
		// Token: 0x06000790 RID: 1936 RVA: 0x000164BB File Offset: 0x000146BB
		public NameplateNotificationListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x000164EC File Offset: 0x000146EC
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._isFirstFrame)
			{
				switch (this.RelationType)
				{
				case -1:
					this.RelationVisualWidget.Color = NameplateNotificationListPanel.NegativeRelationColor;
					break;
				case 0:
					this.RelationVisualWidget.Color = NameplateNotificationListPanel.NeutralRelationColor;
					break;
				case 1:
					this.RelationVisualWidget.Color = NameplateNotificationListPanel.PositiveRelationColor;
					break;
				}
				this._isFirstFrame = false;
			}
			this._totalDt += dt;
			if (base.AlphaFactor <= 0f || this._totalDt > this._stayAmount + this._fadeTime)
			{
				base.EventFired("OnRemove", Array.Empty<object>());
				return;
			}
			if (this._totalDt > this._stayAmount)
			{
				float num = 1f - (this._totalDt - this._stayAmount) / this._fadeTime;
				this.SetGlobalAlphaRecursively(num);
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x000165D1 File Offset: 0x000147D1
		// (set) Token: 0x06000793 RID: 1939 RVA: 0x000165D9 File Offset: 0x000147D9
		public Widget RelationVisualWidget
		{
			get
			{
				return this._relationVisualWidget;
			}
			set
			{
				if (this._relationVisualWidget != value)
				{
					this._relationVisualWidget = value;
					base.OnPropertyChanged<Widget>(value, "RelationVisualWidget");
				}
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x000165F7 File Offset: 0x000147F7
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x000165FF File Offset: 0x000147FF
		public int RelationType
		{
			get
			{
				return this._relationType;
			}
			set
			{
				if (this._relationType != value)
				{
					this._relationType = value;
					base.OnPropertyChanged(value, "RelationType");
				}
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x0001661D File Offset: 0x0001481D
		// (set) Token: 0x06000797 RID: 1943 RVA: 0x00016625 File Offset: 0x00014825
		public float StayAmount
		{
			get
			{
				return this._stayAmount;
			}
			set
			{
				if (this._stayAmount != value)
				{
					this._stayAmount = value;
					base.OnPropertyChanged(value, "StayAmount");
				}
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x00016643 File Offset: 0x00014843
		// (set) Token: 0x06000799 RID: 1945 RVA: 0x0001664B File Offset: 0x0001484B
		public float FadeTime
		{
			get
			{
				return this._fadeTime;
			}
			set
			{
				if (this._fadeTime != value)
				{
					this._fadeTime = value;
					base.OnPropertyChanged(value, "FadeTime");
				}
			}
		}

		// Token: 0x04000348 RID: 840
		private static readonly Color NegativeRelationColor = Color.ConvertStringToColor("#D6543BFF");

		// Token: 0x04000349 RID: 841
		private static readonly Color NeutralRelationColor = Color.ConvertStringToColor("#ECB05BFF");

		// Token: 0x0400034A RID: 842
		private static readonly Color PositiveRelationColor = Color.ConvertStringToColor("#98CA3AFF");

		// Token: 0x0400034B RID: 843
		private float _totalDt;

		// Token: 0x0400034C RID: 844
		private bool _isFirstFrame = true;

		// Token: 0x0400034D RID: 845
		private Widget _relationVisualWidget;

		// Token: 0x0400034E RID: 846
		private float _stayAmount = 2f;

		// Token: 0x0400034F RID: 847
		private float _fadeTime = 1f;

		// Token: 0x04000350 RID: 848
		private int _relationType = -2;
	}
}
