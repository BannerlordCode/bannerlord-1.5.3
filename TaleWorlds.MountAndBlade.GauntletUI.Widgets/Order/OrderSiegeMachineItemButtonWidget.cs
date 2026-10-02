using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Order
{
	// Token: 0x02000074 RID: 116
	public class OrderSiegeMachineItemButtonWidget : ButtonWidget
	{
		// Token: 0x06000642 RID: 1602 RVA: 0x000128AF File Offset: 0x00010AAF
		public OrderSiegeMachineItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x000128C6 File Offset: 0x00010AC6
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isVisualsDirty)
			{
				this.MachineIconWidgetChanged();
				this.UpdateMachineIcon();
				this.UpdateRemainingCount();
				this._isVisualsDirty = false;
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x000128F0 File Offset: 0x00010AF0
		private void MachineIconWidgetChanged()
		{
			if (this.MachineIconWidget == null)
			{
				return;
			}
			this.MachineIconWidget.RegisterBrushStatesOfWidget();
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00012908 File Offset: 0x00010B08
		private void UpdateMachineIcon()
		{
			if (this.MachineIconWidget == null)
			{
				return;
			}
			this._isRemainingCountVisible = true;
			string machineClass = this.MachineClass;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(machineClass);
			if (num <= 1056090379U)
			{
				if (num <= 390431385U)
				{
					if (num <= 6339497U)
					{
						if (num != 0U)
						{
							if (num == 6339497U)
							{
								if (machineClass == "ladder")
								{
									this.MachineIconWidget.SetState("Ladder");
									return;
								}
							}
						}
						else if (machineClass != null)
						{
						}
					}
					else if (num != 354578048U)
					{
						if (num == 390431385U)
						{
							if (machineClass == "bricole")
							{
								this.MachineIconWidget.SetState("Bricole");
								return;
							}
						}
					}
					else if (machineClass == "Mangonel")
					{
						this.MachineIconWidget.SetState("Mangonel");
						return;
					}
				}
				else if (num <= 729368230U)
				{
					if (num != 616782878U)
					{
						if (num == 729368230U)
						{
							if (machineClass == "siege_tower_level1")
							{
								this.MachineIconWidget.SetState("SiegeTower");
								return;
							}
						}
					}
					else if (machineClass == "improved_ram")
					{
						this.MachineIconWidget.SetState("ImprovedRam");
						return;
					}
				}
				else if (num != 808481256U)
				{
					if (num == 1056090379U)
					{
						if (machineClass == "preparations")
						{
							this.MachineIconWidget.SetState("Preparations");
							return;
						}
					}
				}
				else if (machineClass == "fire_ballista")
				{
					this.MachineIconWidget.SetState("FireBallista");
					return;
				}
			}
			else if (num <= 1839032341U)
			{
				if (num <= 1748194790U)
				{
					if (num != 1241455715U)
					{
						if (num == 1748194790U)
						{
							if (machineClass == "fire_catapult")
							{
								this.MachineIconWidget.SetState("FireCatapult");
								return;
							}
						}
					}
					else if (machineClass == "ram")
					{
						this.MachineIconWidget.SetState("Ram");
						return;
					}
				}
				else if (num != 1820818168U)
				{
					if (num == 1839032341U)
					{
						if (machineClass == "trebuchet")
						{
							this.MachineIconWidget.SetState("Trebuchet");
							return;
						}
					}
				}
				else if (machineClass == "fire_onager")
				{
					this.MachineIconWidget.SetState("FireOnager");
					return;
				}
			}
			else if (num <= 1898442385U)
			{
				if (num != 1844264380U)
				{
					if (num == 1898442385U)
					{
						if (machineClass == "catapult")
						{
							this.MachineIconWidget.SetState("Catapult");
							return;
						}
					}
				}
				else if (machineClass == "FireMangonel")
				{
					this.MachineIconWidget.SetState("FireMangonel");
					return;
				}
			}
			else if (num != 2166136261U)
			{
				if (num != 2806198843U)
				{
					if (num == 4036530155U)
					{
						if (machineClass == "ballista")
						{
							this.MachineIconWidget.SetState("Ballista");
							return;
						}
					}
				}
				else if (machineClass == "onager")
				{
					this.MachineIconWidget.SetState("Onager");
					return;
				}
			}
			else if (machineClass != null && machineClass.Length != 0)
			{
			}
			this.MachineIconWidget.SetState("None");
			this._isRemainingCountVisible = false;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00012CB8 File Offset: 0x00010EB8
		private void UpdateRemainingCount()
		{
			if (this.RemainingCountWidget == null)
			{
				return;
			}
			base.IsDisabled = this.RemainingCount == 0;
			this.RemainingCountWidget.IsVisible = this._isRemainingCountVisible;
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00012CE3 File Offset: 0x00010EE3
		// (set) Token: 0x06000648 RID: 1608 RVA: 0x00012CEB File Offset: 0x00010EEB
		[Editor(false)]
		public int RemainingCount
		{
			get
			{
				return this._remainingCount;
			}
			set
			{
				if (this._remainingCount != value)
				{
					this._remainingCount = value;
					base.OnPropertyChanged(value, "RemainingCount");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x00012D10 File Offset: 0x00010F10
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x00012D18 File Offset: 0x00010F18
		[Editor(false)]
		public TextWidget RemainingCountWidget
		{
			get
			{
				return this._remainingCountWidget;
			}
			set
			{
				if (this._remainingCountWidget != value)
				{
					this._remainingCountWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "RemainingCountWidget");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00012D3D File Offset: 0x00010F3D
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x00012D45 File Offset: 0x00010F45
		[Editor(false)]
		public string MachineClass
		{
			get
			{
				return this._machineClass;
			}
			set
			{
				if (this._machineClass != value)
				{
					this._machineClass = value;
					base.OnPropertyChanged<string>(value, "MachineClass");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00012D6F File Offset: 0x00010F6F
		// (set) Token: 0x0600064E RID: 1614 RVA: 0x00012D77 File Offset: 0x00010F77
		[Editor(false)]
		public Widget MachineIconWidget
		{
			get
			{
				return this._machineIconWidget;
			}
			set
			{
				if (this._machineIconWidget != value)
				{
					this._machineIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "MachineIconWidget");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x040002AC RID: 684
		private bool _isRemainingCountVisible = true;

		// Token: 0x040002AD RID: 685
		private bool _isVisualsDirty = true;

		// Token: 0x040002AE RID: 686
		private int _remainingCount;

		// Token: 0x040002AF RID: 687
		private TextWidget _remainingCountWidget;

		// Token: 0x040002B0 RID: 688
		private string _machineClass;

		// Token: 0x040002B1 RID: 689
		private Widget _machineIconWidget;
	}
}
