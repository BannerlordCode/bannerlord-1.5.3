using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000AB RID: 171
	public class SettlementGovernorSelectionVM : ViewModel
	{
		// Token: 0x0600101F RID: 4127 RVA: 0x00042620 File Offset: 0x00040820
		public SettlementGovernorSelectionVM(Settlement settlement, Action<Hero> onDone)
		{
			this._settlement = settlement;
			this._onDone = onDone;
			this.AvailableGovernors = new MBBindingList<SettlementGovernorSelectionItemVM>
			{
				new SettlementGovernorSelectionItemVM(null, new Action<SettlementGovernorSelectionItemVM>(this.OnSelection))
			};
			if (((settlement != null) ? settlement.OwnerClan : null) != null)
			{
				using (List<Hero>.Enumerator enumerator = settlement.OwnerClan.Heroes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Hero hero = enumerator.Current;
						if (Campaign.Current.Models.ClanPoliticsModel.CanHeroBeGovernor(hero) && !this.AvailableGovernors.Any<SettlementGovernorSelectionItemVM>((SettlementGovernorSelectionItemVM G) => G.Governor == hero) && (hero.GovernorOf == this._settlement.Town || hero.GovernorOf == null))
						{
							this.AvailableGovernors.Add(new SettlementGovernorSelectionItemVM(hero, new Action<SettlementGovernorSelectionItemVM>(this.OnSelection)));
						}
					}
				}
			}
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00042750 File Offset: 0x00040950
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.AvailableGovernors.ApplyActionOnAllItems(delegate(SettlementGovernorSelectionItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x00042782 File Offset: 0x00040982
		private void OnSelection(SettlementGovernorSelectionItemVM item)
		{
			Action<Hero> onDone = this._onDone;
			if (onDone == null)
			{
				return;
			}
			onDone(item.Governor);
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001022 RID: 4130 RVA: 0x0004279A File Offset: 0x0004099A
		// (set) Token: 0x06001023 RID: 4131 RVA: 0x000427A2 File Offset: 0x000409A2
		[DataSourceProperty]
		public MBBindingList<SettlementGovernorSelectionItemVM> AvailableGovernors
		{
			get
			{
				return this._availableGovernors;
			}
			set
			{
				if (value != this._availableGovernors)
				{
					this._availableGovernors = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementGovernorSelectionItemVM>>(value, "AvailableGovernors");
				}
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x000427C0 File Offset: 0x000409C0
		// (set) Token: 0x06001025 RID: 4133 RVA: 0x000427C8 File Offset: 0x000409C8
		[DataSourceProperty]
		public int CurrentGovernorIndex
		{
			get
			{
				return this._currentGovernorIndex;
			}
			set
			{
				if (value != this._currentGovernorIndex)
				{
					this._currentGovernorIndex = value;
					base.OnPropertyChangedWithValue(value, "CurrentGovernorIndex");
				}
			}
		}

		// Token: 0x0400074F RID: 1871
		private readonly Settlement _settlement;

		// Token: 0x04000750 RID: 1872
		private readonly Action<Hero> _onDone;

		// Token: 0x04000751 RID: 1873
		private MBBindingList<SettlementGovernorSelectionItemVM> _availableGovernors;

		// Token: 0x04000752 RID: 1874
		private int _currentGovernorIndex;
	}
}
