using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.ExtraWidgets.Graph
{
	// Token: 0x0200001B RID: 27
	public class GraphLineWidget : Widget
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000076E2 File Offset: 0x000058E2
		// (set) Token: 0x0600015D RID: 349 RVA: 0x000076EA File Offset: 0x000058EA
		public string LineBrushStateName { get; set; }

		// Token: 0x0600015E RID: 350 RVA: 0x000076F3 File Offset: 0x000058F3
		public GraphLineWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000076FC File Offset: 0x000058FC
		private void OnPointContainerEventFire(Widget widget, string eventName, object[] eventArgs)
		{
			GraphLinePointWidget graphLinePointWidget;
			if (eventName == "ItemAdd" && eventArgs.Length != 0 && (graphLinePointWidget = eventArgs[0] as GraphLinePointWidget) != null)
			{
				Action<GraphLineWidget, GraphLinePointWidget> onPointAdded = this.OnPointAdded;
				if (onPointAdded == null)
				{
					return;
				}
				onPointAdded(this, graphLinePointWidget);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00007738 File Offset: 0x00005938
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00007740 File Offset: 0x00005940
		public Widget PointContainerWidget
		{
			get
			{
				return this._pointContainerWidget;
			}
			set
			{
				if (value != this._pointContainerWidget)
				{
					if (this._pointContainerWidget != null)
					{
						this._pointContainerWidget.EventFire -= this.OnPointContainerEventFire;
					}
					this._pointContainerWidget = value;
					if (this._pointContainerWidget != null)
					{
						this._pointContainerWidget.EventFire += this.OnPointContainerEventFire;
					}
					base.OnPropertyChanged<Widget>(value, "PointContainerWidget");
				}
			}
		}

		// Token: 0x040000A8 RID: 168
		public Action<GraphLineWidget, GraphLinePointWidget> OnPointAdded;

		// Token: 0x040000A9 RID: 169
		private Widget _pointContainerWidget;
	}
}
