using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000CD RID: 205
	public class MultiplayerClassLoadoutItemTabControllerButtonWidget : ButtonWidget
	{
		// Token: 0x06000ABA RID: 2746 RVA: 0x0001E24D File Offset: 0x0001C44D
		public MultiplayerClassLoadoutItemTabControllerButtonWidget(UIContext context)
			: base(context)
		{
			this._itemTabs = new List<MultiplayerItemTabButtonWidget>();
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0001E264 File Offset: 0x0001C464
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			this._itemTabs.Clear();
			for (int i = 0; i < this.ItemTabList.ChildCount; i++)
			{
				MultiplayerItemTabButtonWidget multiplayerItemTabButtonWidget = (MultiplayerItemTabButtonWidget)this.ItemTabList.GetChild(i);
				multiplayerItemTabButtonWidget.boolPropertyChanged += this.TabWidgetPropertyChanged;
				this._itemTabs.Add(multiplayerItemTabButtonWidget);
			}
			this.ItemTabList.OnInitialized += this.ItemTabListInitialized;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0001E2E0 File Offset: 0x0001C4E0
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			for (int i = 0; i < this._itemTabs.Count; i++)
			{
				this._itemTabs[i].boolPropertyChanged -= this.TabWidgetPropertyChanged;
			}
			this._itemTabs.Clear();
			this.ItemTabList.OnInitialized -= this.ItemTabListInitialized;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0001E348 File Offset: 0x0001C548
		protected override void OnUpdate(float dt)
		{
			if (this.CursorWidget == null || float.IsNaN(this._targetPositionXOffset) || this.AnimationSpeed <= 0f || MathF.Abs(this.CursorWidget.PositionXOffset - this._targetPositionXOffset) <= 1E-05f)
			{
				return;
			}
			int num = MathF.Sign(this._targetPositionXOffset - this.CursorWidget.PositionXOffset);
			float num2 = MathF.Min(this.AnimationSpeed * dt, 1f);
			this.CursorWidget.PositionXOffset = MathF.Lerp(this.CursorWidget.PositionXOffset, this._targetPositionXOffset, num2, 1E-05f);
			if ((num < 0 && this.CursorWidget.PositionXOffset < this._targetPositionXOffset) || (num > 0 && this.CursorWidget.PositionXOffset > this._targetPositionXOffset))
			{
				this.CursorWidget.PositionXOffset = this._targetPositionXOffset;
			}
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0001E427 File Offset: 0x0001C627
		private void TabWidgetPropertyChanged(PropertyOwnerObject sender, string propertyName, bool value)
		{
			if (propertyName == "IsSelected" && value)
			{
				this.SelectedTabChanged(null);
			}
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0001E43F File Offset: 0x0001C63F
		private void ItemTabListInitialized()
		{
			this.SelectedTabChanged(null);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0001E448 File Offset: 0x0001C648
		private void SelectedTabChanged(Widget widget)
		{
			if (this.CursorWidget == null || this.ItemTabList.IntValue < 0)
			{
				return;
			}
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < this.ItemTabList.ChildCount; i++)
			{
				ButtonWidget buttonWidget = (ButtonWidget)this.ItemTabList.GetChild(i);
				if (buttonWidget.IsVisible)
				{
					num2++;
					if (buttonWidget.IsSelected)
					{
						num = num2 - 1;
					}
				}
			}
			this.CalculateTargetPosition(num, num2);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0001E4B8 File Offset: 0x0001C6B8
		private void CalculateTargetPosition(int selectedIndex, int activeTabCount)
		{
			float num = this.ItemTabList.Size.X / base._scaleToUse;
			float num2 = num / (float)activeTabCount;
			float num3 = (float)selectedIndex * num2 + num2 / 2f;
			this._targetPositionXOffset = num3 - num / 2f;
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x0001E4FE File Offset: 0x0001C6FE
		// (set) Token: 0x06000AC3 RID: 2755 RVA: 0x0001E508 File Offset: 0x0001C708
		[DataSourceProperty]
		public MultiplayerClassLoadoutItemTabListPanel ItemTabList
		{
			get
			{
				return this._itemTabList;
			}
			set
			{
				if (value != this._itemTabList)
				{
					MultiplayerClassLoadoutItemTabListPanel itemTabList = this._itemTabList;
					if (itemTabList != null)
					{
						itemTabList.SelectEventHandlers.Remove(new Action<Widget>(this.SelectedTabChanged));
					}
					this._itemTabList = value;
					MultiplayerClassLoadoutItemTabListPanel itemTabList2 = this._itemTabList;
					if (itemTabList2 != null)
					{
						itemTabList2.SelectEventHandlers.Add(new Action<Widget>(this.SelectedTabChanged));
					}
					base.OnPropertyChanged<MultiplayerClassLoadoutItemTabListPanel>(value, "ItemTabList");
				}
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0001E576 File Offset: 0x0001C776
		// (set) Token: 0x06000AC5 RID: 2757 RVA: 0x0001E57E File Offset: 0x0001C77E
		[DataSourceProperty]
		public Widget CursorWidget
		{
			get
			{
				return this._cursorWidget;
			}
			set
			{
				if (value != this._cursorWidget)
				{
					this._cursorWidget = value;
					base.OnPropertyChanged<Widget>(value, "CursorWidget");
				}
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0001E59C File Offset: 0x0001C79C
		// (set) Token: 0x06000AC7 RID: 2759 RVA: 0x0001E5A4 File Offset: 0x0001C7A4
		[DataSourceProperty]
		public float AnimationSpeed
		{
			get
			{
				return this._animationSpeed;
			}
			set
			{
				if (value != this._animationSpeed)
				{
					this._animationSpeed = value;
					base.OnPropertyChanged(value, "AnimationSpeed");
				}
			}
		}

		// Token: 0x040004E4 RID: 1252
		private readonly List<MultiplayerItemTabButtonWidget> _itemTabs;

		// Token: 0x040004E5 RID: 1253
		private float _targetPositionXOffset;

		// Token: 0x040004E6 RID: 1254
		private MultiplayerClassLoadoutItemTabListPanel _itemTabList;

		// Token: 0x040004E7 RID: 1255
		private Widget _cursorWidget;

		// Token: 0x040004E8 RID: 1256
		private float _animationSpeed;
	}
}
