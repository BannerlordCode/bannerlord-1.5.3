using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection
{
	// Token: 0x0200014E RID: 334
	public class PerkSelectionVM : ViewModel
	{
		// Token: 0x0600209C RID: 8348 RVA: 0x0007546D File Offset: 0x0007366D
		public PerkSelectionVM(HeroDeveloper developer, Action<SkillObject> refreshPerksOf, Action onPerkSelection)
		{
			this._developer = developer;
			this._refreshPerksOf = refreshPerksOf;
			this._onPerkSelection = onPerkSelection;
			this._selectedPerks = new List<PerkObject>();
			this.AvailablePerks = new MBBindingList<PerkSelectionItemVM>();
			this.IsActive = false;
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x000754A8 File Offset: 0x000736A8
		public void SetCurrentSelectionPerk(PerkVM perk)
		{
			if (this.AvailablePerks.Count > 0 || this.IsActive)
			{
				this.ExecuteDeactivate();
			}
			this.AvailablePerks.Clear();
			this._currentInitialPerk = perk;
			this.AvailablePerks.Add(new PerkSelectionItemVM(perk.Perk, new Action<PerkSelectionItemVM>(this.OnSelectPerk)));
			if (perk.AlternativeType == 2)
			{
				this.AvailablePerks.Insert(0, new PerkSelectionItemVM(perk.Perk.AlternativePerk, new Action<PerkSelectionItemVM>(this.OnSelectPerk)));
			}
			else if (perk.AlternativeType == 1)
			{
				this.AvailablePerks.Add(new PerkSelectionItemVM(perk.Perk.AlternativePerk, new Action<PerkSelectionItemVM>(this.OnSelectPerk)));
			}
			this.IsActive = true;
			this.OnSelectPerk(perk);
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x00075578 File Offset: 0x00073778
		private void OnSelectPerk(PerkVM selectedPerk)
		{
			this._selectedPerks.Add(selectedPerk.Perk);
			this._refreshPerksOf(selectedPerk.Perk.Skill);
			this.IsActive = false;
			Game.Current.EventManager.TriggerEvent<PerkSelectedByPlayerEvent>(new PerkSelectedByPlayerEvent(selectedPerk.Perk));
			Action onPerkSelection = this._onPerkSelection;
			if (onPerkSelection == null)
			{
				return;
			}
			onPerkSelection();
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x000755E0 File Offset: 0x000737E0
		private void OnSelectPerk(PerkSelectionItemVM selectedPerk)
		{
			this._selectedPerks.Add(selectedPerk.Perk);
			this._refreshPerksOf(selectedPerk.Perk.Skill);
			this.IsActive = false;
			Game.Current.EventManager.TriggerEvent<PerkSelectedByPlayerEvent>(new PerkSelectedByPlayerEvent(selectedPerk.Perk));
			Action onPerkSelection = this._onPerkSelection;
			if (onPerkSelection == null)
			{
				return;
			}
			onPerkSelection();
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x00075648 File Offset: 0x00073848
		public void ResetSelectedPerks()
		{
			foreach (PerkObject perkObject in this._selectedPerks)
			{
				this._refreshPerksOf(perkObject.Skill);
			}
			this._selectedPerks.Clear();
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x000756B0 File Offset: 0x000738B0
		public void ApplySelectedPerks()
		{
			foreach (PerkObject perkObject in this._selectedPerks.ToList<PerkObject>())
			{
				this._developer.AddPerk(perkObject);
				this._selectedPerks.Remove(perkObject);
			}
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x0007571C File Offset: 0x0007391C
		public bool IsPerkSelected(PerkObject perk)
		{
			return this._selectedPerks.Contains(perk);
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x0007572A File Offset: 0x0007392A
		public bool IsAnyPerkSelected()
		{
			return this._selectedPerks.Count > 0;
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x0007573A File Offset: 0x0007393A
		public void ExecuteDeactivate()
		{
			this.IsActive = false;
			this._refreshPerksOf(this._currentInitialPerk.Perk.Skill);
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x060020A5 RID: 8357 RVA: 0x0007575E File Offset: 0x0007395E
		// (set) Token: 0x060020A6 RID: 8358 RVA: 0x00075766 File Offset: 0x00073966
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
					Game.Current.EventManager.TriggerEvent<PerkSelectionToggleEvent>(new PerkSelectionToggleEvent(this.IsActive));
				}
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x060020A7 RID: 8359 RVA: 0x0007579E File Offset: 0x0007399E
		// (set) Token: 0x060020A8 RID: 8360 RVA: 0x000757A6 File Offset: 0x000739A6
		[DataSourceProperty]
		public MBBindingList<PerkSelectionItemVM> AvailablePerks
		{
			get
			{
				return this._availablePerks;
			}
			set
			{
				if (value != this._availablePerks)
				{
					this._availablePerks = value;
					base.OnPropertyChangedWithValue<MBBindingList<PerkSelectionItemVM>>(value, "AvailablePerks");
				}
			}
		}

		// Token: 0x04000EF2 RID: 3826
		private readonly HeroDeveloper _developer;

		// Token: 0x04000EF3 RID: 3827
		private readonly List<PerkObject> _selectedPerks;

		// Token: 0x04000EF4 RID: 3828
		private readonly Action<SkillObject> _refreshPerksOf;

		// Token: 0x04000EF5 RID: 3829
		private readonly Action _onPerkSelection;

		// Token: 0x04000EF6 RID: 3830
		private PerkVM _currentInitialPerk;

		// Token: 0x04000EF7 RID: 3831
		private bool _isActive;

		// Token: 0x04000EF8 RID: 3832
		private MBBindingList<PerkSelectionItemVM> _availablePerks;
	}
}
