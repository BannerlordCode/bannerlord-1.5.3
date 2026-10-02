using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Refinement
{
	// Token: 0x0200011A RID: 282
	public class RefinementActionItemVM : ViewModel
	{
		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06001989 RID: 6537 RVA: 0x000613F0 File Offset: 0x0005F5F0
		public Crafting.RefiningFormula RefineFormula { get; }

		// Token: 0x0600198A RID: 6538 RVA: 0x000613F8 File Offset: 0x0005F5F8
		public RefinementActionItemVM(Crafting.RefiningFormula refineFormula, Action<RefinementActionItemVM> onSelect)
		{
			this._onSelect = onSelect;
			this.RefineFormula = refineFormula;
			this.InputMaterials = new MBBindingList<CraftingResourceItemVM>();
			this.OutputMaterials = new MBBindingList<CraftingResourceItemVM>();
			SmithingModel smithingModel = Campaign.Current.Models.SmithingModel;
			if (this.RefineFormula.Input1Count > 0)
			{
				this.InputMaterials.Add(new CraftingResourceItemVM(this.RefineFormula.Input1, this.RefineFormula.Input1Count, 0));
			}
			if (this.RefineFormula.Input2Count > 0)
			{
				this.InputMaterials.Add(new CraftingResourceItemVM(this.RefineFormula.Input2, this.RefineFormula.Input2Count, 0));
			}
			if (this.RefineFormula.OutputCount > 0)
			{
				this.OutputMaterials.Add(new CraftingResourceItemVM(this.RefineFormula.Output, this.RefineFormula.OutputCount, 0));
			}
			if (this.RefineFormula.Output2Count > 0)
			{
				this.OutputMaterials.Add(new CraftingResourceItemVM(this.RefineFormula.Output2, this.RefineFormula.Output2Count, 0));
			}
			this.RefreshDynamicProperties();
			this.RefreshValues();
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x00061520 File Offset: 0x0005F720
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.InputMaterials.ApplyActionOnAllItems(delegate(CraftingResourceItemVM m)
			{
				m.RefreshValues();
			});
			this.OutputMaterials.ApplyActionOnAllItems(delegate(CraftingResourceItemVM m)
			{
				m.RefreshValues();
			});
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x00061587 File Offset: 0x0005F787
		public void RefreshDynamicProperties()
		{
			this.IsEnabled = this.UpdateInputAvailabilities();
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x00061598 File Offset: 0x0005F798
		private bool UpdateInputAvailabilities()
		{
			bool flag = true;
			ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
			foreach (CraftingResourceItemVM craftingResourceItemVM in this.InputMaterials)
			{
				if (itemRoster.GetItemNumber(craftingResourceItemVM.ResourceItem) < craftingResourceItemVM.ResourceAmount)
				{
					flag = false;
					craftingResourceItemVM.IsResourceAvailable = false;
				}
				else
				{
					craftingResourceItemVM.IsResourceAvailable = true;
				}
			}
			return flag;
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x00061614 File Offset: 0x0005F814
		public void ExecuteSelectAction()
		{
			this._onSelect(this);
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x0600198F RID: 6543 RVA: 0x00061622 File Offset: 0x0005F822
		// (set) Token: 0x06001990 RID: 6544 RVA: 0x0006162A File Offset: 0x0005F82A
		[DataSourceProperty]
		public MBBindingList<CraftingResourceItemVM> InputMaterials
		{
			get
			{
				return this._inputMaterials;
			}
			set
			{
				if (value != this._inputMaterials)
				{
					this._inputMaterials = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingResourceItemVM>>(value, "InputMaterials");
				}
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06001991 RID: 6545 RVA: 0x00061648 File Offset: 0x0005F848
		// (set) Token: 0x06001992 RID: 6546 RVA: 0x00061650 File Offset: 0x0005F850
		[DataSourceProperty]
		public MBBindingList<CraftingResourceItemVM> OutputMaterials
		{
			get
			{
				return this._outputMaterials;
			}
			set
			{
				if (value != this._outputMaterials)
				{
					this._outputMaterials = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingResourceItemVM>>(value, "OutputMaterials");
				}
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06001993 RID: 6547 RVA: 0x0006166E File Offset: 0x0005F86E
		// (set) Token: 0x06001994 RID: 6548 RVA: 0x00061676 File Offset: 0x0005F876
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06001995 RID: 6549 RVA: 0x00061694 File Offset: 0x0005F894
		// (set) Token: 0x06001996 RID: 6550 RVA: 0x0006169C File Offset: 0x0005F89C
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x04000BB1 RID: 2993
		private readonly Action<RefinementActionItemVM> _onSelect;

		// Token: 0x04000BB3 RID: 2995
		private MBBindingList<CraftingResourceItemVM> _inputMaterials;

		// Token: 0x04000BB4 RID: 2996
		private MBBindingList<CraftingResourceItemVM> _outputMaterials;

		// Token: 0x04000BB5 RID: 2997
		private bool _isSelected;

		// Token: 0x04000BB6 RID: 2998
		private bool _isEnabled;
	}
}
