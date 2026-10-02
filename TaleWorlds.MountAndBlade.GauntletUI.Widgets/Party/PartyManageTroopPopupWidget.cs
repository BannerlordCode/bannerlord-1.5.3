using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x02000067 RID: 103
	public class PartyManageTroopPopupWidget : Widget
	{
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x00010885 File Offset: 0x0000EA85
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x0001088D File Offset: 0x0000EA8D
		public Widget PrimaryInputKeyVisualParent { get; set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x00010896 File Offset: 0x0000EA96
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x0001089E File Offset: 0x0000EA9E
		public Widget SecondaryInputKeyVisualParent { get; set; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x000108A7 File Offset: 0x0000EAA7
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x000108AF File Offset: 0x0000EAAF
		public Widget TertiaryInputKeyVisualParent { get; set; }

		// Token: 0x06000585 RID: 1413 RVA: 0x000108B8 File Offset: 0x0000EAB8
		public PartyManageTroopPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x000108C4 File Offset: 0x0000EAC4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsVisible)
			{
				Widget hoveredWidget = base.EventManager.HoveredWidget;
				if (hoveredWidget != null && this.PrimaryInputKeyVisualParent != null && this.SecondaryInputKeyVisualParent != null && this.TertiaryInputKeyVisualParent != null)
				{
					PartyTroopManagementItemButtonWidget firstParentTupleOfWidget = this.GetFirstParentTupleOfWidget(hoveredWidget);
					if (firstParentTupleOfWidget != null)
					{
						Widget actionButtonAtIndex = firstParentTupleOfWidget.GetActionButtonAtIndex(0);
						if (this.IsPrimaryActionAvailable && actionButtonAtIndex != null)
						{
							this.PrimaryInputKeyVisualParent.IsVisible = true;
							this.PrimaryInputKeyVisualParent.ScaledPositionXOffset = actionButtonAtIndex.GlobalPosition.X;
							this.PrimaryInputKeyVisualParent.ScaledPositionYOffset = actionButtonAtIndex.GlobalPosition.Y - 10f;
						}
						else
						{
							this.PrimaryInputKeyVisualParent.IsVisible = false;
						}
						Widget actionButtonAtIndex2 = firstParentTupleOfWidget.GetActionButtonAtIndex(1);
						if (this.IsSecondaryActionAvailable && actionButtonAtIndex2 != null)
						{
							this.SecondaryInputKeyVisualParent.IsVisible = true;
							this.SecondaryInputKeyVisualParent.ScaledPositionXOffset = actionButtonAtIndex2.GlobalPosition.X;
							this.SecondaryInputKeyVisualParent.ScaledPositionYOffset = actionButtonAtIndex2.GlobalPosition.Y - 10f;
						}
						else
						{
							this.SecondaryInputKeyVisualParent.IsVisible = false;
						}
						Widget actionButtonAtIndex3 = firstParentTupleOfWidget.GetActionButtonAtIndex(2);
						if (this.IsTertiaryActionAvailable && actionButtonAtIndex3 != null)
						{
							this.TertiaryInputKeyVisualParent.IsVisible = true;
							this.TertiaryInputKeyVisualParent.ScaledPositionXOffset = actionButtonAtIndex3.GlobalPosition.X;
							this.TertiaryInputKeyVisualParent.ScaledPositionYOffset = actionButtonAtIndex3.GlobalPosition.Y - 10f;
							return;
						}
						this.TertiaryInputKeyVisualParent.IsVisible = false;
						return;
					}
					else
					{
						this.PrimaryInputKeyVisualParent.IsVisible = false;
						this.SecondaryInputKeyVisualParent.IsVisible = false;
						this.TertiaryInputKeyVisualParent.IsVisible = false;
					}
				}
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00010A6C File Offset: 0x0000EC6C
		private PartyTroopManagementItemButtonWidget GetFirstParentTupleOfWidget(Widget widget)
		{
			for (Widget widget2 = widget; widget2 != null; widget2 = widget2.ParentWidget)
			{
				PartyTroopManagementItemButtonWidget partyTroopManagementItemButtonWidget;
				if ((partyTroopManagementItemButtonWidget = widget2 as PartyTroopManagementItemButtonWidget) != null)
				{
					return partyTroopManagementItemButtonWidget;
				}
			}
			return null;
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x00010A94 File Offset: 0x0000EC94
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x00010A9C File Offset: 0x0000EC9C
		public bool IsPrimaryActionAvailable
		{
			get
			{
				return this._isPrimaryActionAvailable;
			}
			set
			{
				if (value != this._isPrimaryActionAvailable)
				{
					this._isPrimaryActionAvailable = value;
					base.OnPropertyChanged(value, "IsPrimaryActionAvailable");
				}
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x00010ABA File Offset: 0x0000ECBA
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x00010AC2 File Offset: 0x0000ECC2
		public bool IsSecondaryActionAvailable
		{
			get
			{
				return this._isSecondaryActionAvailable;
			}
			set
			{
				if (value != this._isSecondaryActionAvailable)
				{
					this._isSecondaryActionAvailable = value;
					base.OnPropertyChanged(value, "IsSecondaryActionAvailable");
				}
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x00010AE0 File Offset: 0x0000ECE0
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x00010AE8 File Offset: 0x0000ECE8
		public bool IsTertiaryActionAvailable
		{
			get
			{
				return this._isTertiaryActionAvailable;
			}
			set
			{
				if (value != this._isTertiaryActionAvailable)
				{
					this._isTertiaryActionAvailable = value;
					base.OnPropertyChanged(value, "IsTertiaryActionAvailable");
				}
			}
		}

		// Token: 0x0400025C RID: 604
		private bool _isPrimaryActionAvailable;

		// Token: 0x0400025D RID: 605
		private bool _isSecondaryActionAvailable;

		// Token: 0x0400025E RID: 606
		private bool _isTertiaryActionAvailable;
	}
}
