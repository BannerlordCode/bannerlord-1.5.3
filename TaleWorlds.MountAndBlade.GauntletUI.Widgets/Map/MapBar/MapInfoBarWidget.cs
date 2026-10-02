using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapBar
{
	// Token: 0x02000131 RID: 305
	public class MapInfoBarWidget : Widget
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06001004 RID: 4100 RVA: 0x0002C5B0 File Offset: 0x0002A7B0
		// (remove) Token: 0x06001005 RID: 4101 RVA: 0x0002C5E8 File Offset: 0x0002A7E8
		public event MapInfoBarWidget.MapBarExtendStateChangeEvent OnMapInfoBarExtendStateChange;

		// Token: 0x06001006 RID: 4102 RVA: 0x0002C61D File Offset: 0x0002A81D
		public MapInfoBarWidget(UIContext context)
			: base(context)
		{
			base.AddState("Disabled");
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0002C631 File Offset: 0x0002A831
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.RefreshBarExtendState();
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0002C640 File Offset: 0x0002A840
		private void OnExtendButtonClick(Widget widget)
		{
			this.IsInfoBarExtended = !this.IsInfoBarExtended;
			this.RefreshBarExtendState();
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0002C658 File Offset: 0x0002A858
		private void RefreshBarExtendState()
		{
			if (this.IsInfoBarExtended && base.CurrentState != "Extended")
			{
				this.SetState("Extended");
				this.RefreshVerticalVisual();
				return;
			}
			if (!this.IsInfoBarExtended && base.CurrentState != "Default")
			{
				this.SetState("Default");
				this.RefreshVerticalVisual();
			}
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0002C6BC File Offset: 0x0002A8BC
		private void RefreshVerticalVisual()
		{
			foreach (Style style in this.ExtendButtonWidget.Brush.Styles)
			{
				for (int i = 0; i < style.LayerCount; i++)
				{
					style.GetLayer(i).VerticalFlip = this.IsInfoBarExtended;
				}
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x0600100B RID: 4107 RVA: 0x0002C738 File Offset: 0x0002A938
		// (set) Token: 0x0600100C RID: 4108 RVA: 0x0002C740 File Offset: 0x0002A940
		[Editor(false)]
		public ButtonWidget ExtendButtonWidget
		{
			get
			{
				return this._extendButtonWidget;
			}
			set
			{
				if (this._extendButtonWidget != value)
				{
					this._extendButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ExtendButtonWidget");
					if (!this._extendButtonWidget.ClickEventHandlers.Contains(new Action<Widget>(this.OnExtendButtonClick)))
					{
						this._extendButtonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnExtendButtonClick));
					}
				}
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x0600100D RID: 4109 RVA: 0x0002C7A3 File Offset: 0x0002A9A3
		// (set) Token: 0x0600100E RID: 4110 RVA: 0x0002C7AB File Offset: 0x0002A9AB
		[Editor(false)]
		public bool IsInfoBarExtended
		{
			get
			{
				return this._isInfoBarExtended;
			}
			set
			{
				if (this._isInfoBarExtended != value)
				{
					this._isInfoBarExtended = value;
					base.OnPropertyChanged(value, "IsInfoBarExtended");
					MapInfoBarWidget.MapBarExtendStateChangeEvent onMapInfoBarExtendStateChange = this.OnMapInfoBarExtendStateChange;
					if (onMapInfoBarExtendStateChange == null)
					{
						return;
					}
					onMapInfoBarExtendStateChange(this.IsInfoBarExtended);
				}
			}
		}

		// Token: 0x04000750 RID: 1872
		private ButtonWidget _extendButtonWidget;

		// Token: 0x04000751 RID: 1873
		private bool _isInfoBarExtended;

		// Token: 0x020001CE RID: 462
		// (Invoke) Token: 0x060015A7 RID: 5543
		public delegate void MapBarExtendStateChangeEvent(bool newState);
	}
}
