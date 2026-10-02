using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x02000022 RID: 34
	public class OrderTroopItemFormationClassVM : ViewModel
	{
		// Token: 0x060002EC RID: 748 RVA: 0x0000B2EC File Offset: 0x000094EC
		public OrderTroopItemFormationClassVM(Formation formation, FormationClass formationClass)
		{
			this._formation = formation;
			this.FormationClass = formationClass;
			this.FormationClassValue = (int)formationClass;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000B30C File Offset: 0x0000950C
		public void UpdateTroopCount()
		{
			switch (this.FormationClass)
			{
			case FormationClass.Infantry:
				this.TroopCount = this._formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.Infantry);
				return;
			case FormationClass.Ranged:
				this.TroopCount = this._formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.Ranged);
				return;
			case FormationClass.Cavalry:
				this.TroopCount = this._formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.Cavalry);
				return;
			case FormationClass.HorseArcher:
				this.TroopCount = this._formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.HorseArcher);
				return;
			default:
				return;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000B382 File Offset: 0x00009582
		// (set) Token: 0x060002EF RID: 751 RVA: 0x0000B38A File Offset: 0x0000958A
		[DataSourceProperty]
		public int FormationClassValue
		{
			get
			{
				return this._formationClassValue;
			}
			set
			{
				if (value != this._formationClassValue)
				{
					this._formationClassValue = value;
					base.OnPropertyChangedWithValue(value, "FormationClassValue");
				}
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000B3A8 File Offset: 0x000095A8
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000B3B0 File Offset: 0x000095B0
		[DataSourceProperty]
		public int TroopCount
		{
			get
			{
				return this._troopCount;
			}
			set
			{
				if (value != this._troopCount)
				{
					this._troopCount = value;
					base.OnPropertyChangedWithValue(value, "TroopCount");
				}
			}
		}

		// Token: 0x04000147 RID: 327
		public readonly FormationClass FormationClass;

		// Token: 0x04000148 RID: 328
		private readonly Formation _formation;

		// Token: 0x04000149 RID: 329
		private int _formationClassValue;

		// Token: 0x0400014A RID: 330
		private int _troopCount;
	}
}
