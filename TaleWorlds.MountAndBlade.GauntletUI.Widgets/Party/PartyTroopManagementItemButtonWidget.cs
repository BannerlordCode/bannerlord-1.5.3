using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x0200006A RID: 106
	public class PartyTroopManagementItemButtonWidget : ButtonWidget
	{
		// Token: 0x060005CF RID: 1487 RVA: 0x00011631 File Offset: 0x0000F831
		public PartyTroopManagementItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001163C File Offset: 0x0000F83C
		public Widget GetActionButtonAtIndex(int index)
		{
			if (this.ActionButtonsContainer != null)
			{
				int num = 0;
				List<Widget> allChildrenRecursive = this.ActionButtonsContainer.GetAllChildrenRecursive(null);
				for (int i = 0; i < allChildrenRecursive.Count; i++)
				{
					if (allChildrenRecursive[i].Id == "ActionButton")
					{
						if (num == index)
						{
							return allChildrenRecursive[i];
						}
						num++;
					}
				}
			}
			return null;
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x0001169A File Offset: 0x0000F89A
		// (set) Token: 0x060005D2 RID: 1490 RVA: 0x000116A2 File Offset: 0x0000F8A2
		public Widget ActionButtonsContainer
		{
			get
			{
				return this._actionButtonsContainer;
			}
			set
			{
				if (value != this._actionButtonsContainer)
				{
					this._actionButtonsContainer = value;
					base.OnPropertyChanged<Widget>(value, "ActionButtonsContainer");
				}
			}
		}

		// Token: 0x0400027B RID: 635
		private Widget _actionButtonsContainer;
	}
}
