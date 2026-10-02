using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000038 RID: 56
	public class RadioContainerWidget : Widget
	{
		// Token: 0x0600034D RID: 845 RVA: 0x0000A9EB File Offset: 0x00008BEB
		public RadioContainerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000A9F4 File Offset: 0x00008BF4
		private void ContainerOnPropertyChanged(PropertyOwnerObject owner, string propertyName, int value)
		{
			if (propertyName == "IntValue")
			{
				this.SelectedIndex = this.Container.IntValue;
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000AA14 File Offset: 0x00008C14
		private void ContainerOnEventFire(Widget owner, string eventName, object[] arguments)
		{
			if (eventName == "ItemAdd" || eventName == "ItemRemove")
			{
				this.Container.IntValue = this.SelectedIndex;
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000AA44 File Offset: 0x00008C44
		private void ContainerUpdated(Container newContainer)
		{
			if (this.Container != null)
			{
				this.Container.intPropertyChanged -= this.ContainerOnPropertyChanged;
				this.Container.EventFire -= this.ContainerOnEventFire;
			}
			if (newContainer != null)
			{
				newContainer.intPropertyChanged += this.ContainerOnPropertyChanged;
				newContainer.EventFire += this.ContainerOnEventFire;
				newContainer.IntValue = this.SelectedIndex;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000AABA File Offset: 0x00008CBA
		// (set) Token: 0x06000352 RID: 850 RVA: 0x0000AAC2 File Offset: 0x00008CC2
		[Editor(false)]
		public int SelectedIndex
		{
			get
			{
				return this._selectedIndex;
			}
			set
			{
				if (this._selectedIndex != value)
				{
					this._selectedIndex = value;
					base.OnPropertyChanged(value, "SelectedIndex");
					if (this.Container != null)
					{
						this.Container.IntValue = this._selectedIndex;
					}
				}
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0000AAF9 File Offset: 0x00008CF9
		// (set) Token: 0x06000354 RID: 852 RVA: 0x0000AB01 File Offset: 0x00008D01
		[Editor(false)]
		public Container Container
		{
			get
			{
				return this._container;
			}
			set
			{
				if (this._container != value)
				{
					this.ContainerUpdated(value);
					this._container = value;
					base.OnPropertyChanged<Container>(value, "Container");
				}
			}
		}

		// Token: 0x04000154 RID: 340
		private int _selectedIndex;

		// Token: 0x04000155 RID: 341
		private Container _container;
	}
}
