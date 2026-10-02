using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x0200002E RID: 46
	public class OrderOfBattleFormationClassSelectorItemVM : SelectorItemVM
	{
		// Token: 0x06000355 RID: 853 RVA: 0x0000C360 File Offset: 0x0000A560
		public OrderOfBattleFormationClassSelectorItemVM(DeploymentFormationClass formationClass)
			: base(formationClass.ToString())
		{
			this.FormationClass = formationClass;
			this.FormationClassInt = (int)formationClass;
			this.RefreshValues();
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000C389 File Offset: 0x0000A589
		public override void RefreshValues()
		{
			base.Hint = new HintViewModel(this.FormationClass.GetClassName(), null);
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000C3A2 File Offset: 0x0000A5A2
		// (set) Token: 0x06000358 RID: 856 RVA: 0x0000C3AA File Offset: 0x0000A5AA
		[DataSourceProperty]
		public int FormationClassInt
		{
			get
			{
				return this._formationClassInt;
			}
			set
			{
				if (value != this._formationClassInt)
				{
					this._formationClassInt = value;
					base.OnPropertyChangedWithValue(value, "FormationClassInt");
				}
			}
		}

		// Token: 0x04000178 RID: 376
		public readonly DeploymentFormationClass FormationClass;

		// Token: 0x04000179 RID: 377
		private int _formationClassInt;
	}
}
