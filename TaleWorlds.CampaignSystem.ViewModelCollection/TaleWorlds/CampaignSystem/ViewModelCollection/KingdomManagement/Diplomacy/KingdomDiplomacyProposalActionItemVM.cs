using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x02000074 RID: 116
	public class KingdomDiplomacyProposalActionItemVM : ViewModel
	{
		// Token: 0x06000901 RID: 2305 RVA: 0x000285B8 File Offset: 0x000267B8
		public KingdomDiplomacyProposalActionItemVM(TextObject nameText, TextObject explanationText, int influenceCost, bool isEnabled, TextObject hintText, Action action)
		{
			this._nameText = nameText;
			this._explanationText = explanationText;
			this._action = action;
			this.InfluenceCost = influenceCost;
			this.IsEnabled = isEnabled;
			this.Hint = new HintViewModel(hintText, null);
			this.RefreshValues();
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00028604 File Offset: 0x00026804
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._nameText.ToString();
			this.Explanation = this._explanationText.ToString();
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0002862E File Offset: 0x0002682E
		public void ExecuteAction()
		{
			Action action = this._action;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00028640 File Offset: 0x00026840
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x00028648 File Offset: 0x00026848
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

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x0002866B File Offset: 0x0002686B
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x00028673 File Offset: 0x00026873
		[DataSourceProperty]
		public string Explanation
		{
			get
			{
				return this._explanation;
			}
			set
			{
				if (value != this._explanation)
				{
					this._explanation = value;
					base.OnPropertyChangedWithValue<string>(value, "Explanation");
				}
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x00028696 File Offset: 0x00026896
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x0002869E File Offset: 0x0002689E
		[DataSourceProperty]
		public int InfluenceCost
		{
			get
			{
				return this._influenceCost;
			}
			set
			{
				if (value != this._influenceCost)
				{
					this._influenceCost = value;
					base.OnPropertyChangedWithValue(value, "InfluenceCost");
				}
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x000286BC File Offset: 0x000268BC
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x000286C4 File Offset: 0x000268C4
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

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x000286E2 File Offset: 0x000268E2
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x000286EA File Offset: 0x000268EA
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x040003ED RID: 1005
		private readonly TextObject _nameText;

		// Token: 0x040003EE RID: 1006
		private readonly TextObject _explanationText;

		// Token: 0x040003EF RID: 1007
		private readonly Action _action;

		// Token: 0x040003F0 RID: 1008
		private string _name;

		// Token: 0x040003F1 RID: 1009
		private string _explanation;

		// Token: 0x040003F2 RID: 1010
		private bool _isEnabled;

		// Token: 0x040003F3 RID: 1011
		private int _influenceCost;

		// Token: 0x040003F4 RID: 1012
		private HintViewModel _hint;
	}
}
