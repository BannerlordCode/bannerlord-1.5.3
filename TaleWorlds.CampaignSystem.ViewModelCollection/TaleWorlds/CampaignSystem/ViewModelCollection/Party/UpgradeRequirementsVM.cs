using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x02000031 RID: 49
	public class UpgradeRequirementsVM : ViewModel
	{
		// Token: 0x060004DE RID: 1246 RVA: 0x0001C44E File Offset: 0x0001A64E
		public UpgradeRequirementsVM()
		{
			this.IsItemRequirementMet = true;
			this.IsPerkRequirementMet = true;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001C47A File Offset: 0x0001A67A
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.UpdateItemRequirementHint();
			this.UpdatePerkRequirementHint();
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001C48E File Offset: 0x0001A68E
		public void SetItemRequirement(ItemCategory category)
		{
			if (category != null)
			{
				this.HasItemRequirement = true;
				this._category = category;
				this.ItemRequirement = category.StringId.ToLower();
				this.UpdateItemRequirementHint();
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001C4B8 File Offset: 0x0001A6B8
		public void SetPerkRequirement(PerkObject perk)
		{
			if (perk != null)
			{
				this.HasPerkRequirement = true;
				this._perk = perk;
				this.PerkRequirement = perk.Skill.StringId.ToLower();
				this.UpdatePerkRequirementHint();
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001C4E7 File Offset: 0x0001A6E7
		public void SetRequirementsMet(bool isItemRequirementMet, bool isPerkRequirementMet)
		{
			this.IsItemRequirementMet = !this.HasItemRequirement || isItemRequirementMet;
			this.IsPerkRequirementMet = !this.HasPerkRequirement || isPerkRequirementMet;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001C50C File Offset: 0x0001A70C
		private void UpdateItemRequirementHint()
		{
			if (this._category == null)
			{
				return;
			}
			TextObject textObject = new TextObject("{=Q0j1umAt}Requirement: {REQUIREMENT_NAME}", null);
			textObject.SetTextVariable("REQUIREMENT_NAME", this._category.GetName().ToString());
			this.ItemRequirementHint = new HintViewModel(textObject, null);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001C558 File Offset: 0x0001A758
		private void UpdatePerkRequirementHint()
		{
			if (this._perk == null)
			{
				return;
			}
			TextObject textObject = new TextObject("{=Q0j1umAt}Requirement: {REQUIREMENT_NAME}", null);
			textObject.SetTextVariable("REQUIREMENT_NAME", this._perk.Name.ToString());
			this.PerkRequirementHint = new HintViewModel(textObject, null);
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0001C5A3 File Offset: 0x0001A7A3
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x0001C5AB File Offset: 0x0001A7AB
		[DataSourceProperty]
		public bool IsItemRequirementMet
		{
			get
			{
				return this._isItemRequirementMet;
			}
			set
			{
				if (value != this._isItemRequirementMet)
				{
					this._isItemRequirementMet = value;
					base.OnPropertyChangedWithValue(value, "IsItemRequirementMet");
				}
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0001C5C9 File Offset: 0x0001A7C9
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x0001C5D1 File Offset: 0x0001A7D1
		[DataSourceProperty]
		public bool IsPerkRequirementMet
		{
			get
			{
				return this._isPerkRequirementMet;
			}
			set
			{
				if (value != this._isPerkRequirementMet)
				{
					this._isPerkRequirementMet = value;
					base.OnPropertyChangedWithValue(value, "IsPerkRequirementMet");
				}
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0001C5EF File Offset: 0x0001A7EF
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x0001C5F7 File Offset: 0x0001A7F7
		[DataSourceProperty]
		public bool HasItemRequirement
		{
			get
			{
				return this._hasItemRequirement;
			}
			set
			{
				if (value != this._hasItemRequirement)
				{
					this._hasItemRequirement = value;
					base.OnPropertyChangedWithValue(value, "HasItemRequirement");
				}
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0001C615 File Offset: 0x0001A815
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x0001C61D File Offset: 0x0001A81D
		[DataSourceProperty]
		public bool HasPerkRequirement
		{
			get
			{
				return this._hasPerkRequirement;
			}
			set
			{
				if (value != this._hasPerkRequirement)
				{
					this._hasPerkRequirement = value;
					base.OnPropertyChangedWithValue(value, "HasPerkRequirement");
				}
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0001C63B File Offset: 0x0001A83B
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x0001C643 File Offset: 0x0001A843
		[DataSourceProperty]
		public string PerkRequirement
		{
			get
			{
				return this._perkRequirement;
			}
			set
			{
				if (value != this._perkRequirement)
				{
					this._perkRequirement = value;
					base.OnPropertyChangedWithValue<string>(value, "PerkRequirement");
				}
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x0001C666 File Offset: 0x0001A866
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x0001C66E File Offset: 0x0001A86E
		[DataSourceProperty]
		public string ItemRequirement
		{
			get
			{
				return this._itemRequirement;
			}
			set
			{
				if (value != this._itemRequirement)
				{
					this._itemRequirement = value;
					base.OnPropertyChangedWithValue<string>(value, "ItemRequirement");
				}
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0001C691 File Offset: 0x0001A891
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x0001C699 File Offset: 0x0001A899
		[DataSourceProperty]
		public HintViewModel ItemRequirementHint
		{
			get
			{
				return this._itemRequirementHint;
			}
			set
			{
				if (value != this._itemRequirementHint)
				{
					this._itemRequirementHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ItemRequirementHint");
				}
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0001C6B7 File Offset: 0x0001A8B7
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x0001C6BF File Offset: 0x0001A8BF
		[DataSourceProperty]
		public HintViewModel PerkRequirementHint
		{
			get
			{
				return this._perkRequirementHint;
			}
			set
			{
				if (value != this._perkRequirementHint)
				{
					this._perkRequirementHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PerkRequirementHint");
				}
			}
		}

		// Token: 0x0400021D RID: 541
		private ItemCategory _category;

		// Token: 0x0400021E RID: 542
		private PerkObject _perk;

		// Token: 0x0400021F RID: 543
		private bool _isItemRequirementMet;

		// Token: 0x04000220 RID: 544
		private bool _isPerkRequirementMet;

		// Token: 0x04000221 RID: 545
		private bool _hasItemRequirement;

		// Token: 0x04000222 RID: 546
		private bool _hasPerkRequirement;

		// Token: 0x04000223 RID: 547
		private string _perkRequirement = "";

		// Token: 0x04000224 RID: 548
		private string _itemRequirement = "";

		// Token: 0x04000225 RID: 549
		private HintViewModel _itemRequirementHint;

		// Token: 0x04000226 RID: 550
		private HintViewModel _perkRequirementHint;
	}
}
