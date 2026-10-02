using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200003E RID: 62
	public class SelectorWidget : Widget
	{
		// Token: 0x060003AB RID: 939 RVA: 0x0000BAAD File Offset: 0x00009CAD
		public SelectorWidget(UIContext context)
			: base(context)
		{
			this._listSelectionHandler = new Action<Widget>(this.OnSelectionChanged);
			this._listItemRemovedHandler = new Action<Widget, Widget>(this.OnListChanged);
			this._listItemAddedHandler = new Action<Widget, Widget>(this.OnListChanged);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000BAEC File Offset: 0x00009CEC
		public void OnListChanged(Widget widget)
		{
			this.RefreshSelectedItem();
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000BAF4 File Offset: 0x00009CF4
		public void OnListChanged(Widget parentWidget, Widget addedWidget)
		{
			this.RefreshSelectedItem();
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000BAFC File Offset: 0x00009CFC
		public void OnSelectionChanged(Widget widget)
		{
			this.CurrentSelectedIndex = this.ListPanelValue;
			this.RefreshSelectedItem();
			base.OnPropertyChanged(this.CurrentSelectedIndex, "CurrentSelectedIndex");
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000BB21 File Offset: 0x00009D21
		private void RefreshSelectedItem()
		{
			this.ListPanelValue = this.CurrentSelectedIndex;
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0000BB2F File Offset: 0x00009D2F
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x0000BB46 File Offset: 0x00009D46
		[Editor(false)]
		public int ListPanelValue
		{
			get
			{
				if (this.Container != null)
				{
					return this.Container.IntValue;
				}
				return -1;
			}
			set
			{
				if (this.Container != null && this.Container.IntValue != value)
				{
					this.Container.IntValue = value;
				}
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0000BB6A File Offset: 0x00009D6A
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x0000BB72 File Offset: 0x00009D72
		[Editor(false)]
		public int CurrentSelectedIndex
		{
			get
			{
				return this._currentSelectedIndex;
			}
			set
			{
				if (this._currentSelectedIndex != value && value >= 0)
				{
					this._currentSelectedIndex = value;
					this.RefreshSelectedItem();
				}
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000BB8E File Offset: 0x00009D8E
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x0000BB98 File Offset: 0x00009D98
		[Editor(false)]
		public Container Container
		{
			get
			{
				return this._container;
			}
			set
			{
				if (this._container != null)
				{
					this._container.SelectEventHandlers.Remove(this._listSelectionHandler);
					this._container.ItemAddEventHandlers.Remove(this._listItemAddedHandler);
					this._container.ItemRemoveEventHandlers.Remove(this._listItemRemovedHandler);
				}
				this._container = value;
				if (this._container != null)
				{
					this._container.SelectEventHandlers.Add(this._listSelectionHandler);
					this._container.ItemAddEventHandlers.Add(this._listItemAddedHandler);
					this._container.ItemRemoveEventHandlers.Add(this._listItemRemovedHandler);
				}
				this.RefreshSelectedItem();
			}
		}

		// Token: 0x04000184 RID: 388
		private int _currentSelectedIndex;

		// Token: 0x04000185 RID: 389
		private Action<Widget> _listSelectionHandler;

		// Token: 0x04000186 RID: 390
		private Action<Widget, Widget> _listItemRemovedHandler;

		// Token: 0x04000187 RID: 391
		private Action<Widget, Widget> _listItemAddedHandler;

		// Token: 0x04000188 RID: 392
		private Container _container;
	}
}
