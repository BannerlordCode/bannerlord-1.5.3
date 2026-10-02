using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Policies
{
	// Token: 0x02000071 RID: 113
	public class KingdomPolicyItemVM : KingdomItemVM
	{
		// Token: 0x060008C0 RID: 2240 RVA: 0x000278DC File Offset: 0x00025ADC
		public KingdomPolicyItemVM(PolicyObject policy, Action<KingdomPolicyItemVM> onSelect, Func<PolicyObject, bool> getIsPolicyActive)
		{
			this._onSelect = onSelect;
			this._policy = policy;
			this._getIsPolicyActive = getIsPolicyActive;
			this.Name = policy.Name.ToString();
			this.Explanation = policy.Description.ToString();
			this.LikelihoodHint = new HintViewModel();
			this.PolicyEffectList = new MBBindingList<StringItemWithHintVM>();
			foreach (string text in policy.SecondaryEffects.ToString().Split(new char[] { '\n' }))
			{
				this.PolicyEffectList.Add(new StringItemWithHintVM(text, TextObject.GetEmpty()));
			}
			this.RefreshValues();
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00027988 File Offset: 0x00025B88
		public override void RefreshValues()
		{
			base.RefreshValues();
			Func<PolicyObject, bool> getIsPolicyActive = this._getIsPolicyActive;
			this.PolicyAcceptanceText = ((getIsPolicyActive != null && getIsPolicyActive(this.Policy)) ? GameTexts.FindText("str_policy_support_for_abolishing", null).ToString() : GameTexts.FindText("str_policy_support_for_enacting", null).ToString());
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x000279DD File Offset: 0x00025BDD
		protected override void OnSelect()
		{
			base.OnSelect();
			this._onSelect(this);
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x000279F1 File Offset: 0x00025BF1
		// (set) Token: 0x060008C4 RID: 2244 RVA: 0x000279F9 File Offset: 0x00025BF9
		[DataSourceProperty]
		public string PolicyAcceptanceText
		{
			get
			{
				return this._policyAcceptanceText;
			}
			set
			{
				if (value != this._policyAcceptanceText)
				{
					this._policyAcceptanceText = value;
					base.OnPropertyChangedWithValue<string>(value, "PolicyAcceptanceText");
				}
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x00027A1C File Offset: 0x00025C1C
		// (set) Token: 0x060008C6 RID: 2246 RVA: 0x00027A24 File Offset: 0x00025C24
		[DataSourceProperty]
		public MBBindingList<StringItemWithHintVM> PolicyEffectList
		{
			get
			{
				return this._policyEffectList;
			}
			set
			{
				if (value != this._policyEffectList)
				{
					this._policyEffectList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringItemWithHintVM>>(value, "PolicyEffectList");
				}
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00027A42 File Offset: 0x00025C42
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x00027A4A File Offset: 0x00025C4A
		[DataSourceProperty]
		public string PolicyLikelihoodText
		{
			get
			{
				return this._policyLikelihoodText;
			}
			set
			{
				if (value != this._policyLikelihoodText)
				{
					this._policyLikelihoodText = value;
					base.OnPropertyChangedWithValue<string>(value, "PolicyLikelihoodText");
				}
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x00027A6D File Offset: 0x00025C6D
		// (set) Token: 0x060008CA RID: 2250 RVA: 0x00027A75 File Offset: 0x00025C75
		[DataSourceProperty]
		public HintViewModel LikelihoodHint
		{
			get
			{
				return this._likelihoodHint;
			}
			set
			{
				if (value != this._likelihoodHint)
				{
					this._likelihoodHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LikelihoodHint");
				}
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00027A93 File Offset: 0x00025C93
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x00027A9B File Offset: 0x00025C9B
		[DataSourceProperty]
		public PolicyObject Policy
		{
			get
			{
				return this._policy;
			}
			set
			{
				if (value != this._policy)
				{
					this._policy = value;
					base.OnPropertyChangedWithValue<PolicyObject>(value, "Policy");
				}
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x00027AB9 File Offset: 0x00025CB9
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x00027AC1 File Offset: 0x00025CC1
		[DataSourceProperty]
		public int PolicyLikelihood
		{
			get
			{
				return this._policyLikelihood;
			}
			set
			{
				if (value != this._policyLikelihood)
				{
					this._policyLikelihood = value;
					base.OnPropertyChangedWithValue(value, "PolicyLikelihood");
				}
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x00027ADF File Offset: 0x00025CDF
		// (set) Token: 0x060008D0 RID: 2256 RVA: 0x00027AE7 File Offset: 0x00025CE7
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

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x00027B0A File Offset: 0x00025D0A
		// (set) Token: 0x060008D2 RID: 2258 RVA: 0x00027B12 File Offset: 0x00025D12
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

		// Token: 0x040003C5 RID: 965
		private readonly Action<KingdomPolicyItemVM> _onSelect;

		// Token: 0x040003C6 RID: 966
		private readonly Func<PolicyObject, bool> _getIsPolicyActive;

		// Token: 0x040003C7 RID: 967
		private string _name;

		// Token: 0x040003C8 RID: 968
		private string _explanation;

		// Token: 0x040003C9 RID: 969
		private string _policyAcceptanceText;

		// Token: 0x040003CA RID: 970
		private PolicyObject _policy;

		// Token: 0x040003CB RID: 971
		private int _policyLikelihood;

		// Token: 0x040003CC RID: 972
		private string _policyLikelihoodText;

		// Token: 0x040003CD RID: 973
		private HintViewModel _likelihoodHint;

		// Token: 0x040003CE RID: 974
		private MBBindingList<StringItemWithHintVM> _policyEffectList;
	}
}
