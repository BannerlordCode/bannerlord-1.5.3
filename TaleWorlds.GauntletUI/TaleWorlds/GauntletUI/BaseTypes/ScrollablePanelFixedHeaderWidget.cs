using System;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000064 RID: 100
	public class ScrollablePanelFixedHeaderWidget : Widget
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x0001DDAE File Offset: 0x0001BFAE
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x0001DDB6 File Offset: 0x0001BFB6
		public Widget FixedHeader { get; set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0001DDBF File Offset: 0x0001BFBF
		// (set) Token: 0x060006D0 RID: 1744 RVA: 0x0001DDC7 File Offset: 0x0001BFC7
		public float TopOffset { get; set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0001DDD0 File Offset: 0x0001BFD0
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x0001DDD8 File Offset: 0x0001BFD8
		public float BottomOffset { get; set; } = float.MinValue;

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001DDE1 File Offset: 0x0001BFE1
		public ScrollablePanelFixedHeaderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001DDFC File Offset: 0x0001BFFC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isDirty)
			{
				base.EventFired("FixedHeaderPropertyChanged", Array.Empty<object>());
				this._isDirty = false;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0001DE24 File Offset: 0x0001C024
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x0001DE2C File Offset: 0x0001C02C
		public float HeaderHeight
		{
			get
			{
				return this._headerHeight;
			}
			set
			{
				if (value != this._headerHeight)
				{
					this._headerHeight = value;
					base.SuggestedHeight = this._headerHeight;
					this._isDirty = true;
				}
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0001DE51 File Offset: 0x0001C051
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x0001DE59 File Offset: 0x0001C059
		public float AdditionalTopOffset
		{
			get
			{
				return this._additionalTopOffset;
			}
			set
			{
				if (value != this._additionalTopOffset)
				{
					this._additionalTopOffset = value;
					this._isDirty = true;
				}
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0001DE72 File Offset: 0x0001C072
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x0001DE7A File Offset: 0x0001C07A
		public float AdditionalBottomOffset
		{
			get
			{
				return this._additionalBottomOffset;
			}
			set
			{
				if (value != this._additionalBottomOffset)
				{
					this._additionalBottomOffset = value;
					this._isDirty = true;
				}
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0001DE93 File Offset: 0x0001C093
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x0001DE9B File Offset: 0x0001C09B
		[Editor(false)]
		public bool IsRelevant
		{
			get
			{
				return this._isRelevant;
			}
			set
			{
				if (value != this._isRelevant)
				{
					this._isRelevant = value;
					base.IsVisible = value;
					this._isDirty = true;
					base.OnPropertyChanged(value, "IsRelevant");
				}
			}
		}

		// Token: 0x04000333 RID: 819
		private bool _isDirty;

		// Token: 0x04000337 RID: 823
		private float _headerHeight;

		// Token: 0x04000338 RID: 824
		private float _additionalTopOffset;

		// Token: 0x04000339 RID: 825
		private float _additionalBottomOffset;

		// Token: 0x0400033A RID: 826
		private bool _isRelevant = true;
	}
}
