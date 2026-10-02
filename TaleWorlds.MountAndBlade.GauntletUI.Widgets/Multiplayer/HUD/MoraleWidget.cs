using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000C6 RID: 198
	public class MoraleWidget : Widget
	{
		// Token: 0x06000A5A RID: 2650 RVA: 0x0001D2EA File Offset: 0x0001B4EA
		public MoraleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x0001D2F3 File Offset: 0x0001B4F3
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			this._moraleItemWidgets = this.CreateItemWidgets(this.ItemContainer);
			this.SetItemWidgetColors(this._teamColor);
			this.SetItemGlowWidgetColors(this._teamColorSecondary);
			this.RestartAnimations();
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0001D32C File Offset: 0x0001B52C
		protected override void OnUpdate(float dt)
		{
			if (this._triggerAnimations)
			{
				if (this._animWaitFrame >= 1)
				{
					this.HandleAnimation();
					this._triggerAnimations = false;
					this._animWaitFrame = 0;
				}
				else
				{
					this._animWaitFrame++;
				}
			}
			if (!this._initialized)
			{
				this.FlowArrowWidget.LeftSideArrow = this.ExtendToLeft;
				this._initialized = true;
			}
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0001D38E File Offset: 0x0001B58E
		private void RestartAnimations()
		{
			this._triggerAnimations = true;
			this._animWaitFrame = 0;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0001D39E File Offset: 0x0001B59E
		private void UpdateArrows(int flowLevel)
		{
			if (this.Container == null || this.FlowArrowWidget == null)
			{
				return;
			}
			this.FlowArrowWidget.SetFlowLevel(flowLevel);
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0001D3C0 File Offset: 0x0001B5C0
		private void UpdateMoraleMask()
		{
			int num = MathF.Floor((float)this.MoralePercentage / 100f * 10f);
			for (int i = 0; i < this._moraleItemWidgets.Length; i++)
			{
				MoraleWidget.MoraleItemWidget moraleItemWidget = this._moraleItemWidgets[i];
				float num2 = 0f;
				if (i < num)
				{
					num2 = 1f;
					if (!moraleItemWidget.ItemGlowWidget.IsVisible)
					{
						this.RestartAnimations();
					}
				}
				else if (i == num)
				{
					float num3 = 10f;
					num2 = ((float)this.MoralePercentage - (float)num * num3) / num3;
					if (!moraleItemWidget.ItemWidget.IsVisible && !MBMath.ApproximatelyEquals(num2, 0f, 1E-05f))
					{
						this.RestartAnimations();
					}
				}
				moraleItemWidget.SetFillAmount(num2, 12);
			}
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0001D474 File Offset: 0x0001B674
		private string GetCurrentStateName()
		{
			string text;
			if (this.MoralePercentage < 20)
			{
				text = "IsCriticalAnim";
			}
			else if (this.IncreaseLevel > 0)
			{
				text = "IncreaseAnim";
			}
			else
			{
				text = "Default";
			}
			return text;
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0001D4AC File Offset: 0x0001B6AC
		private void HandleAnimation()
		{
			for (int i = 0; i < this._moraleItemWidgets.Length; i++)
			{
				MoraleWidget.MoraleItemWidget moraleItemWidget = this._moraleItemWidgets[i];
				moraleItemWidget.ItemWidget.SetState(this._currentStateName);
				moraleItemWidget.ItemWidget.BrushRenderer.RestartAnimation();
				moraleItemWidget.ItemGlowWidget.SetState(this._currentStateName);
				moraleItemWidget.ItemGlowWidget.BrushRenderer.RestartAnimation();
			}
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0001D518 File Offset: 0x0001B718
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.UpdateMoraleMask();
			string currentStateName = this.GetCurrentStateName();
			if (this._currentStateName != currentStateName)
			{
				this._currentStateName = currentStateName;
				this.RestartAnimations();
			}
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0001D554 File Offset: 0x0001B754
		private MoraleWidget.MoraleItemWidget[] CreateItemWidgets(Widget containerWidget)
		{
			MoraleWidget.MoraleItemWidget[] array = new MoraleWidget.MoraleItemWidget[10];
			for (int i = 0; i < 10; i++)
			{
				Widget widget = new Widget(base.Context);
				widget.UpdateChildrenStates = true;
				widget.WidthSizePolicy = SizePolicy.Fixed;
				widget.HeightSizePolicy = SizePolicy.Fixed;
				widget.SuggestedWidth = 39f;
				widget.SuggestedHeight = 38f;
				if (this.ExtendToLeft)
				{
					widget.HorizontalAlignment = HorizontalAlignment.Right;
					widget.MarginRight = (float)i * 28f;
				}
				else
				{
					widget.HorizontalAlignment = HorizontalAlignment.Left;
					widget.MarginLeft = (float)i * 28f;
				}
				widget.AddState("IncreaseAnim");
				widget.AddState("IsCriticalAnim");
				containerWidget.AddChild(widget);
				Widget widget2 = new Widget(base.Context);
				widget2.ClipContents = true;
				widget2.UpdateChildrenStates = true;
				widget2.WidthSizePolicy = SizePolicy.StretchToParent;
				widget2.HeightSizePolicy = SizePolicy.Fixed;
				widget2.VerticalAlignment = VerticalAlignment.Bottom;
				widget.AddChild(widget2);
				BrushWidget brushWidget = new BrushWidget(base.Context);
				brushWidget.WidthSizePolicy = SizePolicy.Fixed;
				brushWidget.HeightSizePolicy = SizePolicy.Fixed;
				brushWidget.VerticalAlignment = VerticalAlignment.Bottom;
				brushWidget.Brush = this.ItemGlowBrush;
				brushWidget.SuggestedWidth = 39f;
				brushWidget.SuggestedHeight = 38f;
				brushWidget.AddState("IncreaseAnim");
				brushWidget.AddState("IsCriticalAnim");
				widget2.AddChild(brushWidget);
				BrushWidget brushWidget2 = new BrushWidget(base.Context);
				brushWidget2.WidthSizePolicy = SizePolicy.StretchToParent;
				brushWidget2.HeightSizePolicy = SizePolicy.StretchToParent;
				brushWidget2.Brush = this.ItemBackgroundBrush;
				widget.AddChild(brushWidget2);
				BrushWidget brushWidget3 = new BrushWidget(base.Context);
				brushWidget3.WidthSizePolicy = SizePolicy.Fixed;
				brushWidget3.HeightSizePolicy = SizePolicy.Fixed;
				brushWidget3.VerticalAlignment = VerticalAlignment.Bottom;
				brushWidget3.Brush = this.ItemBrush;
				brushWidget3.SuggestedWidth = 39f;
				brushWidget3.SuggestedHeight = 38f;
				brushWidget3.AddState("IncreaseAnim");
				brushWidget3.AddState("IsCriticalAnim");
				widget2.AddChild(brushWidget3);
				array[i] = new MoraleWidget.MoraleItemWidget(widget, widget2, brushWidget3, brushWidget, brushWidget2);
			}
			return array;
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0001D754 File Offset: 0x0001B954
		private void SetItemWidgetColors(Color color)
		{
			if (this._moraleItemWidgets != null)
			{
				foreach (MoraleWidget.MoraleItemWidget moraleItemWidget in this._moraleItemWidgets)
				{
					this.SetSingleItemWidgetColor(moraleItemWidget, color);
				}
			}
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0001D78C File Offset: 0x0001B98C
		private void SetSingleItemWidgetColor(MoraleWidget.MoraleItemWidget widget, Color color)
		{
			widget.ItemWidget.Brush.Color = color;
			foreach (Style style in widget.ItemWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Color = color;
				}
			}
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0001D810 File Offset: 0x0001BA10
		private void SetItemGlowWidgetColors(Color color)
		{
			if (this._moraleItemWidgets != null)
			{
				foreach (MoraleWidget.MoraleItemWidget moraleItemWidget in this._moraleItemWidgets)
				{
					this.SetSingleItemGlowWidgetColor(moraleItemWidget, color);
				}
			}
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0001D848 File Offset: 0x0001BA48
		private void SetSingleItemGlowWidgetColor(MoraleWidget.MoraleItemWidget widget, Color color)
		{
			widget.ItemGlowWidget.Brush.Color = color;
			foreach (Style style in widget.ItemGlowWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Color = color;
				}
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x0001D8CC File Offset: 0x0001BACC
		// (set) Token: 0x06000A69 RID: 2665 RVA: 0x0001D8D4 File Offset: 0x0001BAD4
		[DataSourceProperty]
		public int IncreaseLevel
		{
			get
			{
				return this._increaseLevel;
			}
			set
			{
				if (this._increaseLevel != value)
				{
					this._increaseLevel = value;
					base.OnPropertyChanged(value, "IncreaseLevel");
					this.UpdateArrows(this._increaseLevel);
				}
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0001D8FE File Offset: 0x0001BAFE
		// (set) Token: 0x06000A6B RID: 2667 RVA: 0x0001D906 File Offset: 0x0001BB06
		[DataSourceProperty]
		public int MoralePercentage
		{
			get
			{
				return this._moralePercentage;
			}
			set
			{
				if (this._moralePercentage != value)
				{
					this._moralePercentage = value;
					base.OnPropertyChanged(value, "MoralePercentage");
				}
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0001D924 File Offset: 0x0001BB24
		// (set) Token: 0x06000A6D RID: 2669 RVA: 0x0001D92C File Offset: 0x0001BB2C
		[DataSourceProperty]
		public Widget Container
		{
			get
			{
				return this._container;
			}
			set
			{
				if (this._container != value)
				{
					this._container = value;
					base.OnPropertyChanged<Widget>(value, "Container");
				}
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0001D94A File Offset: 0x0001BB4A
		// (set) Token: 0x06000A6F RID: 2671 RVA: 0x0001D952 File Offset: 0x0001BB52
		[DataSourceProperty]
		public Widget ItemContainer
		{
			get
			{
				return this._itemContainer;
			}
			set
			{
				if (this._itemContainer != value)
				{
					this._itemContainer = value;
					base.OnPropertyChanged<Widget>(value, "ItemContainer");
				}
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x0001D970 File Offset: 0x0001BB70
		// (set) Token: 0x06000A71 RID: 2673 RVA: 0x0001D978 File Offset: 0x0001BB78
		[DataSourceProperty]
		public Brush ItemBrush
		{
			get
			{
				return this._itemBrush;
			}
			set
			{
				if (this._itemBrush != value)
				{
					this._itemBrush = value;
					base.OnPropertyChanged<Brush>(value, "ItemBrush");
				}
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x0001D996 File Offset: 0x0001BB96
		// (set) Token: 0x06000A73 RID: 2675 RVA: 0x0001D99E File Offset: 0x0001BB9E
		[DataSourceProperty]
		public Brush ItemGlowBrush
		{
			get
			{
				return this._itemGlowBrush;
			}
			set
			{
				if (this._itemGlowBrush != value)
				{
					this._itemGlowBrush = value;
					base.OnPropertyChanged<Brush>(value, "ItemGlowBrush");
				}
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x0001D9BC File Offset: 0x0001BBBC
		// (set) Token: 0x06000A75 RID: 2677 RVA: 0x0001D9C4 File Offset: 0x0001BBC4
		[DataSourceProperty]
		public Brush ItemBackgroundBrush
		{
			get
			{
				return this._itemBackgroundBrush;
			}
			set
			{
				if (this._itemBackgroundBrush != value)
				{
					this._itemBackgroundBrush = value;
					base.OnPropertyChanged<Brush>(value, "ItemBackgroundBrush");
				}
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x0001D9E2 File Offset: 0x0001BBE2
		// (set) Token: 0x06000A77 RID: 2679 RVA: 0x0001D9EA File Offset: 0x0001BBEA
		[DataSourceProperty]
		public string TeamColorAsStr
		{
			get
			{
				return this._teamColorAsStr;
			}
			set
			{
				if (this._teamColorAsStr != value && value != null)
				{
					this._teamColorAsStr = value;
					base.OnPropertyChanged<string>(value, "TeamColorAsStr");
					this._teamColor = Color.ConvertStringToColor(value);
					this.SetItemWidgetColors(this._teamColor);
				}
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x0001DA28 File Offset: 0x0001BC28
		// (set) Token: 0x06000A79 RID: 2681 RVA: 0x0001DA30 File Offset: 0x0001BC30
		[DataSourceProperty]
		public string TeamColorAsStrSecondary
		{
			get
			{
				return this._teamColorAsStrSecondary;
			}
			set
			{
				if (this._teamColorAsStrSecondary != value && value != null)
				{
					this._teamColorAsStrSecondary = value;
					base.OnPropertyChanged<string>(value, "TeamColorAsStrSecondary");
					this._teamColorSecondary = Color.ConvertStringToColor(value);
					this.SetItemGlowWidgetColors(this._teamColorSecondary);
				}
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x0001DA6E File Offset: 0x0001BC6E
		// (set) Token: 0x06000A7B RID: 2683 RVA: 0x0001DA76 File Offset: 0x0001BC76
		[DataSourceProperty]
		public MoraleArrowBrushWidget FlowArrowWidget
		{
			get
			{
				return this._flowArrowWidget;
			}
			set
			{
				if (this._flowArrowWidget != value)
				{
					this._flowArrowWidget = value;
					base.OnPropertyChanged<MoraleArrowBrushWidget>(value, "FlowArrowWidget");
				}
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x0001DA94 File Offset: 0x0001BC94
		// (set) Token: 0x06000A7D RID: 2685 RVA: 0x0001DA9C File Offset: 0x0001BC9C
		[DataSourceProperty]
		public bool ExtendToLeft
		{
			get
			{
				return this._extendToLeft;
			}
			set
			{
				if (this._extendToLeft != value)
				{
					this._extendToLeft = value;
					base.OnPropertyChanged(value, "ExtendToLeft");
				}
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x0001DABA File Offset: 0x0001BCBA
		// (set) Token: 0x06000A7F RID: 2687 RVA: 0x0001DAC2 File Offset: 0x0001BCC2
		[DataSourceProperty]
		public bool AreMoralesIndependent
		{
			get
			{
				return this._areMoralesIndependent;
			}
			set
			{
				if (this._areMoralesIndependent != value)
				{
					this._areMoralesIndependent = value;
					base.OnPropertyChanged(value, "AreMoralesIndependent");
				}
			}
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0001DAE0 File Offset: 0x0001BCE0
		private float PingPong(float min, float max, float time)
		{
			float num = max - min;
			bool flag = (int)(time / num) % 2 == 0;
			float num2 = time % num;
			if (!flag)
			{
				return max - num2;
			}
			return num2 + min;
		}

		// Token: 0x040004B1 RID: 1201
		private const int ItemCount = 10;

		// Token: 0x040004B2 RID: 1202
		private const float ItemDistance = 28f;

		// Token: 0x040004B3 RID: 1203
		private const int ItemWidth = 39;

		// Token: 0x040004B4 RID: 1204
		private const int ItemHeight = 38;

		// Token: 0x040004B5 RID: 1205
		private const int FillMargin = 12;

		// Token: 0x040004B6 RID: 1206
		private MoraleWidget.MoraleItemWidget[] _moraleItemWidgets;

		// Token: 0x040004B7 RID: 1207
		private bool _initialized;

		// Token: 0x040004B8 RID: 1208
		private bool _triggerAnimations;

		// Token: 0x040004B9 RID: 1209
		private int _animWaitFrame;

		// Token: 0x040004BA RID: 1210
		private string _currentStateName;

		// Token: 0x040004BB RID: 1211
		private Color _teamColor;

		// Token: 0x040004BC RID: 1212
		private Color _teamColorSecondary;

		// Token: 0x040004BD RID: 1213
		private int _increaseLevel;

		// Token: 0x040004BE RID: 1214
		private int _moralePercentage;

		// Token: 0x040004BF RID: 1215
		private Widget _container;

		// Token: 0x040004C0 RID: 1216
		private Widget _itemContainer;

		// Token: 0x040004C1 RID: 1217
		private Brush _itemBrush;

		// Token: 0x040004C2 RID: 1218
		private Brush _itemGlowBrush;

		// Token: 0x040004C3 RID: 1219
		private Brush _itemBackgroundBrush;

		// Token: 0x040004C4 RID: 1220
		private MoraleArrowBrushWidget _flowArrowWidget;

		// Token: 0x040004C5 RID: 1221
		private bool _extendToLeft;

		// Token: 0x040004C6 RID: 1222
		private bool _areMoralesIndependent;

		// Token: 0x040004C7 RID: 1223
		private string _teamColorAsStr;

		// Token: 0x040004C8 RID: 1224
		private string _teamColorAsStrSecondary;

		// Token: 0x020001C0 RID: 448
		private class MoraleItemWidget
		{
			// Token: 0x17000784 RID: 1924
			// (get) Token: 0x06001588 RID: 5512 RVA: 0x0003AAB1 File Offset: 0x00038CB1
			// (set) Token: 0x06001589 RID: 5513 RVA: 0x0003AAB9 File Offset: 0x00038CB9
			public Widget ParentWidget { get; private set; }

			// Token: 0x17000785 RID: 1925
			// (get) Token: 0x0600158A RID: 5514 RVA: 0x0003AAC2 File Offset: 0x00038CC2
			// (set) Token: 0x0600158B RID: 5515 RVA: 0x0003AACA File Offset: 0x00038CCA
			public Widget MaskWidget { get; private set; }

			// Token: 0x17000786 RID: 1926
			// (get) Token: 0x0600158C RID: 5516 RVA: 0x0003AAD3 File Offset: 0x00038CD3
			// (set) Token: 0x0600158D RID: 5517 RVA: 0x0003AADB File Offset: 0x00038CDB
			public BrushWidget ItemWidget { get; private set; }

			// Token: 0x17000787 RID: 1927
			// (get) Token: 0x0600158E RID: 5518 RVA: 0x0003AAE4 File Offset: 0x00038CE4
			// (set) Token: 0x0600158F RID: 5519 RVA: 0x0003AAEC File Offset: 0x00038CEC
			public BrushWidget ItemGlowWidget { get; private set; }

			// Token: 0x17000788 RID: 1928
			// (get) Token: 0x06001590 RID: 5520 RVA: 0x0003AAF5 File Offset: 0x00038CF5
			// (set) Token: 0x06001591 RID: 5521 RVA: 0x0003AAFD File Offset: 0x00038CFD
			public Widget ItemBackgroundWidget { get; private set; }

			// Token: 0x06001592 RID: 5522 RVA: 0x0003AB06 File Offset: 0x00038D06
			public MoraleItemWidget(Widget parentWidget, Widget maskWidget, BrushWidget itemWidget, BrushWidget itemGlowWidget, Widget itemBackgroundWidget)
			{
				this.ParentWidget = parentWidget;
				this.MaskWidget = maskWidget;
				this.ItemWidget = itemWidget;
				this.ItemGlowWidget = itemGlowWidget;
				this.ItemBackgroundWidget = itemBackgroundWidget;
			}

			// Token: 0x06001593 RID: 5523 RVA: 0x0003AB34 File Offset: 0x00038D34
			public void SetFillAmount(float fill, int fillMargin)
			{
				bool flag = MBMath.ApproximatelyEquals(fill, 0f, 1E-05f);
				bool flag2 = MBMath.ApproximatelyEquals(fill, 1f, 1E-05f);
				if (flag)
				{
					this.MaskWidget.SuggestedHeight = 0f;
				}
				else if (flag2)
				{
					this.MaskWidget.SuggestedHeight = this.ItemWidget.SuggestedHeight;
				}
				else
				{
					int num = MathF.Floor(this.ItemWidget.SuggestedHeight - (float)(fillMargin * 2));
					this.MaskWidget.SuggestedHeight = (float)fillMargin + (float)num * fill;
				}
				this.ItemWidget.IsVisible = !flag;
				this.ItemGlowWidget.IsVisible = flag2;
			}
		}
	}
}
