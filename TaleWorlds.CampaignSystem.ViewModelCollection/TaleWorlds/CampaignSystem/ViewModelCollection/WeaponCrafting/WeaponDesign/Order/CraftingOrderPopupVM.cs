using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign.Order
{
	// Token: 0x02000116 RID: 278
	public class CraftingOrderPopupVM : ViewModel
	{
		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x0600192B RID: 6443 RVA: 0x000603DE File Offset: 0x0005E5DE
		public bool HasOrders
		{
			get
			{
				return this.CraftingOrders.Count > 0;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x0600192C RID: 6444 RVA: 0x000603EE File Offset: 0x0005E5EE
		public bool HasEnabledOrders
		{
			get
			{
				return this.CraftingOrders.Count<CraftingOrderItemVM>((CraftingOrderItemVM x) => x.IsEnabled) > 0;
			}
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0006041D File Offset: 0x0005E61D
		public CraftingOrderPopupVM(Action<CraftingOrderItemVM> onDoneAction, Func<CraftingAvailableHeroItemVM> getCurrentCraftingHero, Func<CraftingOrder, IEnumerable<CraftingStatData>> getOrderStatDatas)
		{
			this._onDoneAction = onDoneAction;
			this._getCurrentCraftingHero = getCurrentCraftingHero;
			this._getOrderStatDatas = getOrderStatDatas;
			this._craftingBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			this.CraftingOrders = new MBBindingList<CraftingOrderItemVM>();
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x00060458 File Offset: 0x0005E658
		public void RefreshOrders()
		{
			this.CraftingOrders.Clear();
			if (Campaign.Current.GameMode == CampaignGameMode.Tutorial)
			{
				return;
			}
			IReadOnlyDictionary<Town, CraftingCampaignBehavior.CraftingOrderSlots> craftingOrders = this._craftingBehavior.CraftingOrders;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			CraftingCampaignBehavior.CraftingOrderSlots craftingOrderSlots = craftingOrders[(currentSettlement != null) ? currentSettlement.Town : null];
			if (craftingOrderSlots == null)
			{
				return;
			}
			CraftingOrderPopupVM.OrderComparer orderComparer = new CraftingOrderPopupVM.OrderComparer();
			List<CraftingOrder> list = craftingOrderSlots.CustomOrders.Where<CraftingOrder>((CraftingOrder x) => x != null).ToList<CraftingOrder>();
			list.Sort(orderComparer);
			List<CraftingOrder> list2 = craftingOrderSlots.Slots.Where<CraftingOrder>((CraftingOrder x) => x != null).ToList<CraftingOrder>();
			list2.Sort(orderComparer);
			CampaignUIHelper.IssueQuestFlags issueQuestFlags = CampaignUIHelper.IssueQuestFlags.None;
			for (int i = 0; i < list.Count; i++)
			{
				List<CraftingStatData> list3 = this._getOrderStatDatas(list[i]).ToList<CraftingStatData>();
				CampaignUIHelper.IssueQuestFlags questFlagsForOrder = this.GetQuestFlagsForOrder(list[i]);
				this.CraftingOrders.Add(new CraftingOrderItemVM(list[i], new Action<CraftingOrderItemVM>(this.SelectOrder), this._getCurrentCraftingHero, list3, questFlagsForOrder));
				issueQuestFlags |= questFlagsForOrder;
			}
			this.QuestType = (int)issueQuestFlags;
			for (int j = 0; j < list2.Count; j++)
			{
				List<CraftingStatData> list4 = this._getOrderStatDatas(list2[j]).ToList<CraftingStatData>();
				this.CraftingOrders.Add(new CraftingOrderItemVM(list2[j], new Action<CraftingOrderItemVM>(this.SelectOrder), this._getCurrentCraftingHero, list4, CampaignUIHelper.IssueQuestFlags.None));
			}
			TextObject textObject = new TextObject("{=MkVTRqAw}Orders ({ORDER_COUNT})", null);
			textObject.SetTextVariable("ORDER_COUNT", this.CraftingOrders.Count);
			this.OrderCountText = textObject.ToString();
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x00060625 File Offset: 0x0005E825
		private CampaignUIHelper.IssueQuestFlags GetQuestFlagsForOrder(CraftingOrder order)
		{
			if (Campaign.Current.QuestManager.TrackedObjects.ContainsKey(order))
			{
				return CampaignUIHelper.IssueQuestFlags.ActiveIssue;
			}
			return CampaignUIHelper.IssueQuestFlags.None;
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x00060641 File Offset: 0x0005E841
		public void SelectOrder(CraftingOrderItemVM order)
		{
			if (this.SelectedCraftingOrder != null)
			{
				this.SelectedCraftingOrder.IsSelected = false;
			}
			this.SelectedCraftingOrder = order;
			this.SelectedCraftingOrder.IsSelected = true;
			this._onDoneAction(order);
			this.IsVisible = false;
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x0006067D File Offset: 0x0005E87D
		public void ExecuteOpenPopup()
		{
			this.IsVisible = true;
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x00060686 File Offset: 0x0005E886
		public void ExecuteCloseWithoutSelection()
		{
			this.IsVisible = false;
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06001933 RID: 6451 RVA: 0x0006068F File Offset: 0x0005E88F
		// (set) Token: 0x06001934 RID: 6452 RVA: 0x00060697 File Offset: 0x0005E897
		[DataSourceProperty]
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (value != this._isVisible)
				{
					this._isVisible = value;
					base.OnPropertyChangedWithValue(value, "IsVisible");
					Game game = Game.Current;
					if (game == null)
					{
						return;
					}
					game.EventManager.TriggerEvent<CraftingOrderSelectionOpenedEvent>(new CraftingOrderSelectionOpenedEvent(this._isVisible));
				}
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06001935 RID: 6453 RVA: 0x000606D4 File Offset: 0x0005E8D4
		// (set) Token: 0x06001936 RID: 6454 RVA: 0x000606DC File Offset: 0x0005E8DC
		[DataSourceProperty]
		public int QuestType
		{
			get
			{
				return this._questType;
			}
			set
			{
				if (value != this._questType)
				{
					this._questType = value;
					base.OnPropertyChangedWithValue(value, "QuestType");
				}
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06001937 RID: 6455 RVA: 0x000606FA File Offset: 0x0005E8FA
		// (set) Token: 0x06001938 RID: 6456 RVA: 0x00060702 File Offset: 0x0005E902
		[DataSourceProperty]
		public string OrderCountText
		{
			get
			{
				return this._orderCountText;
			}
			set
			{
				if (value != this._orderCountText)
				{
					this._orderCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderCountText");
				}
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06001939 RID: 6457 RVA: 0x00060725 File Offset: 0x0005E925
		// (set) Token: 0x0600193A RID: 6458 RVA: 0x0006072D File Offset: 0x0005E92D
		[DataSourceProperty]
		public CraftingOrderItemVM SelectedCraftingOrder
		{
			get
			{
				return this._selectedCraftingOrder;
			}
			set
			{
				if (value != this._selectedCraftingOrder)
				{
					this._selectedCraftingOrder = value;
					base.OnPropertyChangedWithValue<CraftingOrderItemVM>(value, "SelectedCraftingOrder");
				}
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x0600193B RID: 6459 RVA: 0x0006074B File Offset: 0x0005E94B
		// (set) Token: 0x0600193C RID: 6460 RVA: 0x00060753 File Offset: 0x0005E953
		[DataSourceProperty]
		public MBBindingList<CraftingOrderItemVM> CraftingOrders
		{
			get
			{
				return this._craftingOrders;
			}
			set
			{
				if (value != this._craftingOrders)
				{
					this._craftingOrders = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingOrderItemVM>>(value, "CraftingOrders");
				}
			}
		}

		// Token: 0x04000B83 RID: 2947
		private Action<CraftingOrderItemVM> _onDoneAction;

		// Token: 0x04000B84 RID: 2948
		private Func<CraftingAvailableHeroItemVM> _getCurrentCraftingHero;

		// Token: 0x04000B85 RID: 2949
		private Func<CraftingOrder, IEnumerable<CraftingStatData>> _getOrderStatDatas;

		// Token: 0x04000B86 RID: 2950
		private readonly ICraftingCampaignBehavior _craftingBehavior;

		// Token: 0x04000B87 RID: 2951
		private bool _isVisible;

		// Token: 0x04000B88 RID: 2952
		private int _questType;

		// Token: 0x04000B89 RID: 2953
		private string _orderCountText;

		// Token: 0x04000B8A RID: 2954
		private MBBindingList<CraftingOrderItemVM> _craftingOrders;

		// Token: 0x04000B8B RID: 2955
		private CraftingOrderItemVM _selectedCraftingOrder;

		// Token: 0x0200027C RID: 636
		private class OrderComparer : IComparer<CraftingOrder>
		{
			// Token: 0x060026C2 RID: 9922 RVA: 0x000838C9 File Offset: 0x00081AC9
			public int Compare(CraftingOrder x, CraftingOrder y)
			{
				return (int)(x.OrderDifficulty - y.OrderDifficulty);
			}
		}
	}
}
