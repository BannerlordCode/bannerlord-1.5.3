using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting
{
	// Token: 0x02000102 RID: 258
	public class CraftingResourceItemVM : ViewModel
	{
		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x060016E6 RID: 5862 RVA: 0x0005903B File Offset: 0x0005723B
		// (set) Token: 0x060016E7 RID: 5863 RVA: 0x00059043 File Offset: 0x00057243
		public ItemObject ResourceItem { get; private set; }

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x060016E8 RID: 5864 RVA: 0x0005904C File Offset: 0x0005724C
		// (set) Token: 0x060016E9 RID: 5865 RVA: 0x00059054 File Offset: 0x00057254
		public CraftingMaterials ResourceMaterial { get; private set; }

		// Token: 0x060016EA RID: 5866 RVA: 0x00059060 File Offset: 0x00057260
		public CraftingResourceItemVM(CraftingMaterials material, int amount, int changeAmount = 0)
		{
			this.ResourceMaterial = material;
			Campaign campaign = Campaign.Current;
			ItemObject itemObject;
			if (campaign == null)
			{
				itemObject = null;
			}
			else
			{
				GameModels models = campaign.Models;
				if (models == null)
				{
					itemObject = null;
				}
				else
				{
					SmithingModel smithingModel = models.SmithingModel;
					itemObject = ((smithingModel != null) ? smithingModel.GetCraftingMaterialItem(material) : null);
				}
			}
			this.ResourceItem = itemObject;
			ItemObject resourceItem = this.ResourceItem;
			string text;
			if (resourceItem == null)
			{
				text = null;
			}
			else
			{
				TextObject name = resourceItem.Name;
				text = ((name != null) ? name.ToString() : null);
			}
			this.ResourceName = text ?? "none";
			this.ResourceHint = new HintViewModel(new TextObject("{=!}" + this.ResourceName, null), null);
			this.ResourceAmount = amount;
			ItemObject resourceItem2 = this.ResourceItem;
			this.ResourceItemStringId = ((resourceItem2 != null) ? resourceItem2.StringId : null) ?? "none";
			this.ResourceMaterialTypeAsStr = this.ResourceMaterial.ToString();
			this.ResourceChangeAmount = changeAmount;
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x060016EB RID: 5867 RVA: 0x00059149 File Offset: 0x00057349
		// (set) Token: 0x060016EC RID: 5868 RVA: 0x00059151 File Offset: 0x00057351
		[DataSourceProperty]
		public string ResourceName
		{
			get
			{
				return this._resourceName;
			}
			set
			{
				if (value != this._resourceName)
				{
					this._resourceName = value;
					base.OnPropertyChangedWithValue<string>(value, "ResourceName");
				}
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x060016ED RID: 5869 RVA: 0x00059174 File Offset: 0x00057374
		// (set) Token: 0x060016EE RID: 5870 RVA: 0x0005917C File Offset: 0x0005737C
		[DataSourceProperty]
		public HintViewModel ResourceHint
		{
			get
			{
				return this._resourceHint;
			}
			set
			{
				if (value != this._resourceHint)
				{
					this._resourceHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ResourceHint");
				}
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x060016EF RID: 5871 RVA: 0x0005919A File Offset: 0x0005739A
		// (set) Token: 0x060016F0 RID: 5872 RVA: 0x000591A2 File Offset: 0x000573A2
		[DataSourceProperty]
		public string ResourceMaterialTypeAsStr
		{
			get
			{
				return this._resourceMaterialTypeAsStr;
			}
			set
			{
				if (value != this._resourceMaterialTypeAsStr)
				{
					this._resourceMaterialTypeAsStr = value;
					base.OnPropertyChangedWithValue<string>(value, "ResourceMaterialTypeAsStr");
				}
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x060016F1 RID: 5873 RVA: 0x000591C5 File Offset: 0x000573C5
		// (set) Token: 0x060016F2 RID: 5874 RVA: 0x000591CD File Offset: 0x000573CD
		[DataSourceProperty]
		public int ResourceAmount
		{
			get
			{
				return this._resourceUsageAmount;
			}
			set
			{
				if (value != this._resourceUsageAmount)
				{
					this._resourceUsageAmount = value;
					base.OnPropertyChangedWithValue(value, "ResourceAmount");
				}
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x060016F3 RID: 5875 RVA: 0x000591EB File Offset: 0x000573EB
		// (set) Token: 0x060016F4 RID: 5876 RVA: 0x000591F3 File Offset: 0x000573F3
		[DataSourceProperty]
		public int ResourceChangeAmount
		{
			get
			{
				return this._resourceChangeAmount;
			}
			set
			{
				if (value != this._resourceChangeAmount)
				{
					this._resourceChangeAmount = value;
					base.OnPropertyChangedWithValue(value, "ResourceChangeAmount");
				}
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x060016F5 RID: 5877 RVA: 0x00059211 File Offset: 0x00057411
		// (set) Token: 0x060016F6 RID: 5878 RVA: 0x00059219 File Offset: 0x00057419
		[DataSourceProperty]
		public string ResourceItemStringId
		{
			get
			{
				return this._resourceItemStringId;
			}
			set
			{
				if (value != this._resourceItemStringId)
				{
					this._resourceItemStringId = value;
					base.OnPropertyChangedWithValue<string>(value, "ResourceItemStringId");
				}
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x060016F7 RID: 5879 RVA: 0x0005923C File Offset: 0x0005743C
		// (set) Token: 0x060016F8 RID: 5880 RVA: 0x00059244 File Offset: 0x00057444
		[DataSourceProperty]
		public bool IsResourceAvailable
		{
			get
			{
				return this._isResourceAvailable;
			}
			set
			{
				if (value != this._isResourceAvailable)
				{
					this._isResourceAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsResourceAvailable");
				}
			}
		}

		// Token: 0x04000A6E RID: 2670
		private string _resourceName;

		// Token: 0x04000A6F RID: 2671
		private string _resourceItemStringId;

		// Token: 0x04000A70 RID: 2672
		private int _resourceUsageAmount;

		// Token: 0x04000A71 RID: 2673
		private int _resourceChangeAmount;

		// Token: 0x04000A72 RID: 2674
		private string _resourceMaterialTypeAsStr;

		// Token: 0x04000A73 RID: 2675
		private HintViewModel _resourceHint;

		// Token: 0x04000A74 RID: 2676
		private bool _isResourceAvailable = true;
	}
}
