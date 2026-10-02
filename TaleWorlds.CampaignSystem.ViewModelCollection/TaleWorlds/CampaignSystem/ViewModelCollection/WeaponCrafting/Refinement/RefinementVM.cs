using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Refinement
{
	// Token: 0x0200011B RID: 283
	public class RefinementVM : ViewModel
	{
		// Token: 0x06001997 RID: 6551 RVA: 0x000616BC File Offset: 0x0005F8BC
		public RefinementVM(Action onRefinementSelectionChange, Func<CraftingAvailableHeroItemVM> getCurrentHero)
		{
			this._onRefinementSelectionChange = onRefinementSelectionChange;
			this._craftingBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			this._getCurrentHero = getCurrentHero;
			this.AvailableRefinementActions = new MBBindingList<RefinementActionItemVM>();
			this.SetupRefinementActionsList(this._getCurrentHero().Hero);
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0006170E File Offset: 0x0005F90E
		private void SetupRefinementActionsList(Hero craftingHero)
		{
			this.UpdateRefinementFormulas(craftingHero);
			this.RefreshRefinementActionsList(craftingHero);
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0006171E File Offset: 0x0005F91E
		internal void OnCraftingHeroChanged(CraftingAvailableHeroItemVM newHero)
		{
			this.SetupRefinementActionsList(this._getCurrentHero().Hero);
			this.SelectDefaultAction();
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x0006173C File Offset: 0x0005F93C
		private void UpdateRefinementFormulas(Hero hero)
		{
			this.AvailableRefinementActions.Clear();
			foreach (Crafting.RefiningFormula refiningFormula in Campaign.Current.Models.SmithingModel.GetRefiningFormulas(hero))
			{
				this.AvailableRefinementActions.Add(new RefinementActionItemVM(refiningFormula, new Action<RefinementActionItemVM>(this.OnSelectAction)));
			}
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x000617BC File Offset: 0x0005F9BC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RefinementText = new TextObject("{=p7raHA9x}Refinement", null).ToString();
			this.AvailableRefinementActions.ApplyActionOnAllItems(delegate(RefinementActionItemVM x)
			{
				x.RefreshValues();
			});
			RefinementActionItemVM currentSelectedAction = this.CurrentSelectedAction;
			if (currentSelectedAction == null)
			{
				return;
			}
			currentSelectedAction.RefreshValues();
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x00061820 File Offset: 0x0005FA20
		public void ExecuteSelectedRefinement(Hero currentCraftingHero)
		{
			if (this.CurrentSelectedAction != null)
			{
				ICraftingCampaignBehavior craftingBehavior = this._craftingBehavior;
				if (craftingBehavior != null)
				{
					craftingBehavior.DoRefinement(currentCraftingHero, this.CurrentSelectedAction.RefineFormula);
				}
				this.RefreshRefinementActionsList(currentCraftingHero);
				if (!this.CurrentSelectedAction.IsEnabled)
				{
					this.OnSelectAction(null);
				}
			}
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00061870 File Offset: 0x0005FA70
		public void RefreshRefinementActionsList(Hero craftingHero)
		{
			foreach (RefinementActionItemVM refinementActionItemVM in this.AvailableRefinementActions)
			{
				refinementActionItemVM.RefreshDynamicProperties();
			}
			if (this.CurrentSelectedAction == null)
			{
				this.SelectDefaultAction();
			}
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x000618C8 File Offset: 0x0005FAC8
		private void SelectDefaultAction()
		{
			RefinementActionItemVM refinementActionItemVM = this.AvailableRefinementActions.FirstOrDefault<RefinementActionItemVM>((RefinementActionItemVM a) => a.IsEnabled);
			if (refinementActionItemVM != null)
			{
				this.OnSelectAction(refinementActionItemVM);
			}
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0006190A File Offset: 0x0005FB0A
		private void OnSelectAction(RefinementActionItemVM selectedAction)
		{
			if (this.CurrentSelectedAction != null)
			{
				this.CurrentSelectedAction.IsSelected = false;
			}
			this.CurrentSelectedAction = selectedAction;
			this._onRefinementSelectionChange();
			if (this.CurrentSelectedAction != null)
			{
				this.CurrentSelectedAction.IsSelected = true;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x060019A0 RID: 6560 RVA: 0x00061946 File Offset: 0x0005FB46
		// (set) Token: 0x060019A1 RID: 6561 RVA: 0x0006194E File Offset: 0x0005FB4E
		[DataSourceProperty]
		public RefinementActionItemVM CurrentSelectedAction
		{
			get
			{
				return this._currentSelectedAction;
			}
			set
			{
				if (value != this._currentSelectedAction)
				{
					this._currentSelectedAction = value;
					base.OnPropertyChangedWithValue<RefinementActionItemVM>(value, "CurrentSelectedAction");
					this.IsValidRefinementActionSelected = value != null;
				}
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x060019A2 RID: 6562 RVA: 0x00061976 File Offset: 0x0005FB76
		// (set) Token: 0x060019A3 RID: 6563 RVA: 0x0006197E File Offset: 0x0005FB7E
		[DataSourceProperty]
		public bool IsValidRefinementActionSelected
		{
			get
			{
				return this._isValidRefinementActionSelected;
			}
			set
			{
				if (value != this._isValidRefinementActionSelected)
				{
					this._isValidRefinementActionSelected = value;
					base.OnPropertyChangedWithValue(value, "IsValidRefinementActionSelected");
				}
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x060019A4 RID: 6564 RVA: 0x0006199C File Offset: 0x0005FB9C
		// (set) Token: 0x060019A5 RID: 6565 RVA: 0x000619A4 File Offset: 0x0005FBA4
		[DataSourceProperty]
		public MBBindingList<RefinementActionItemVM> AvailableRefinementActions
		{
			get
			{
				return this._availableRefinementActions;
			}
			set
			{
				if (value != this._availableRefinementActions)
				{
					this._availableRefinementActions = value;
					base.OnPropertyChangedWithValue<MBBindingList<RefinementActionItemVM>>(value, "AvailableRefinementActions");
				}
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x060019A6 RID: 6566 RVA: 0x000619C2 File Offset: 0x0005FBC2
		// (set) Token: 0x060019A7 RID: 6567 RVA: 0x000619CA File Offset: 0x0005FBCA
		[DataSourceProperty]
		public string RefinementText
		{
			get
			{
				return this._refinementText;
			}
			set
			{
				if (value != this._refinementText)
				{
					this._refinementText = value;
					base.OnPropertyChangedWithValue<string>(value, "RefinementText");
				}
			}
		}

		// Token: 0x04000BB7 RID: 2999
		private readonly Action _onRefinementSelectionChange;

		// Token: 0x04000BB8 RID: 3000
		private readonly ICraftingCampaignBehavior _craftingBehavior;

		// Token: 0x04000BB9 RID: 3001
		private readonly Func<CraftingAvailableHeroItemVM> _getCurrentHero;

		// Token: 0x04000BBA RID: 3002
		private RefinementActionItemVM _currentSelectedAction;

		// Token: 0x04000BBB RID: 3003
		private bool _isValidRefinementActionSelected;

		// Token: 0x04000BBC RID: 3004
		private MBBindingList<RefinementActionItemVM> _availableRefinementActions;

		// Token: 0x04000BBD RID: 3005
		private string _refinementText;
	}
}
