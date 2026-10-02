using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Supporters
{
	// Token: 0x02000133 RID: 307
	public class ClanSupporterGroupVM : ViewModel
	{
		// Token: 0x06001CBA RID: 7354 RVA: 0x00068D2F File Offset: 0x00066F2F
		public ClanSupporterGroupVM(TextObject groupName, float influenceBonus, Action<ClanSupporterGroupVM> onSelection)
		{
			this._groupNameText = groupName;
			this._influenceBonus = influenceBonus;
			this._onSelection = onSelection;
			this.Supporters = new MBBindingList<ClanSupporterItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x00068D5D File Offset: 0x00066F5D
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Refresh();
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x00068D6C File Offset: 0x00066F6C
		public void AddSupporter(Hero hero)
		{
			if (!this.Supporters.Any<ClanSupporterItemVM>((ClanSupporterItemVM x) => x.Hero.Hero == hero))
			{
				this.Supporters.Add(new ClanSupporterItemVM(hero));
			}
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x00068DB8 File Offset: 0x00066FB8
		public void Refresh()
		{
			TextObject textObject = GameTexts.FindText("str_amount_with_influence_icon", null);
			this.TotalInfluenceBonus = (float)this.Supporters.Count * this._influenceBonus;
			TextObject textObject2 = GameTexts.FindText("str_plus_with_number", null);
			textObject2.SetTextVariable("NUMBER", this.TotalInfluenceBonus.ToString("F2"));
			textObject.SetTextVariable("AMOUNT", textObject2.ToString());
			textObject.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
			this.TotalInfluence = textObject.ToString();
			TextObject textObject3 = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null);
			textObject3.SetTextVariable("RANK", this._groupNameText.ToString());
			textObject3.SetTextVariable("NUMBER", this.Supporters.Count);
			this.Name = textObject3.ToString();
			TextObject textObject4 = new TextObject("{=cZCOa00c}{SUPPORTER_RANK} Supporters ({NUM})", null);
			textObject4.SetTextVariable("SUPPORTER_RANK", this._groupNameText.ToString());
			textObject4.SetTextVariable("NUM", this.Supporters.Count);
			this.TitleText = textObject4.ToString();
			TextObject textObject5 = new TextObject("{=jdbT6nc9}Each {SUPPORTER_RANK} supporter provides {INFLUENCE_BONUS} per day.", null);
			textObject5.SetTextVariable("SUPPORTER_RANK", this._groupNameText.ToString());
			textObject5.SetTextVariable("INFLUENCE_BONUS", this._influenceBonus.ToString("F2") + "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
			this.InfluenceBonusDescription = textObject5.ToString();
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x00068F2A File Offset: 0x0006712A
		public void ExecuteSelect()
		{
			Action<ClanSupporterGroupVM> onSelection = this._onSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this);
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06001CBF RID: 7359 RVA: 0x00068F3D File Offset: 0x0006713D
		// (set) Token: 0x06001CC0 RID: 7360 RVA: 0x00068F45 File Offset: 0x00067145
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x00068F68 File Offset: 0x00067168
		// (set) Token: 0x06001CC2 RID: 7362 RVA: 0x00068F70 File Offset: 0x00067170
		[DataSourceProperty]
		public float TotalInfluenceBonus
		{
			get
			{
				return this._totalInfluenceBonus;
			}
			private set
			{
				if (value != this._totalInfluenceBonus)
				{
					this._totalInfluenceBonus = value;
					base.OnPropertyChangedWithValue(value, "TotalInfluenceBonus");
				}
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06001CC3 RID: 7363 RVA: 0x00068F8E File Offset: 0x0006718E
		// (set) Token: 0x06001CC4 RID: 7364 RVA: 0x00068F96 File Offset: 0x00067196
		[DataSourceProperty]
		public string InfluenceBonusDescription
		{
			get
			{
				return this._influenceBonusDescription;
			}
			set
			{
				if (value != this._influenceBonusDescription)
				{
					this._influenceBonusDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "InfluenceBonusDescription");
				}
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06001CC5 RID: 7365 RVA: 0x00068FB9 File Offset: 0x000671B9
		// (set) Token: 0x06001CC6 RID: 7366 RVA: 0x00068FC1 File Offset: 0x000671C1
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06001CC7 RID: 7367 RVA: 0x00068FE4 File Offset: 0x000671E4
		// (set) Token: 0x06001CC8 RID: 7368 RVA: 0x00068FEC File Offset: 0x000671EC
		[DataSourceProperty]
		public string TotalInfluence
		{
			get
			{
				return this._totalInfluence;
			}
			set
			{
				if (value != this._totalInfluence)
				{
					this._totalInfluence = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalInfluence");
				}
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06001CC9 RID: 7369 RVA: 0x0006900F File Offset: 0x0006720F
		// (set) Token: 0x06001CCA RID: 7370 RVA: 0x00069017 File Offset: 0x00067217
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

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06001CCB RID: 7371 RVA: 0x00069035 File Offset: 0x00067235
		// (set) Token: 0x06001CCC RID: 7372 RVA: 0x0006903D File Offset: 0x0006723D
		[DataSourceProperty]
		public MBBindingList<ClanSupporterItemVM> Supporters
		{
			get
			{
				return this._supporters;
			}
			set
			{
				if (value != this._supporters)
				{
					this._supporters = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanSupporterItemVM>>(value, "Supporters");
				}
			}
		}

		// Token: 0x04000D20 RID: 3360
		private TextObject _groupNameText;

		// Token: 0x04000D21 RID: 3361
		private float _influenceBonus;

		// Token: 0x04000D22 RID: 3362
		private Action<ClanSupporterGroupVM> _onSelection;

		// Token: 0x04000D23 RID: 3363
		private string _titleText;

		// Token: 0x04000D24 RID: 3364
		private string _influenceBonusDescription;

		// Token: 0x04000D25 RID: 3365
		private string _name;

		// Token: 0x04000D26 RID: 3366
		private string _totalInfluence;

		// Token: 0x04000D27 RID: 3367
		private bool _isSelected;

		// Token: 0x04000D28 RID: 3368
		private MBBindingList<ClanSupporterItemVM> _supporters;

		// Token: 0x04000D29 RID: 3369
		private float _totalInfluenceBonus;
	}
}
