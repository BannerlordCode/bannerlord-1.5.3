using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x02000086 RID: 134
	public class PolicyDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x0002E828 File Offset: 0x0002CA28
		public KingdomPolicyDecision PolicyDecision
		{
			get
			{
				KingdomPolicyDecision kingdomPolicyDecision;
				if ((kingdomPolicyDecision = this._policyDecision) == null)
				{
					kingdomPolicyDecision = (this._policyDecision = this._decision as KingdomPolicyDecision);
				}
				return kingdomPolicyDecision;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x0002E853 File Offset: 0x0002CA53
		public PolicyObject Policy
		{
			get
			{
				return this.PolicyDecision.Policy;
			}
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0002E860 File Offset: 0x0002CA60
		public PolicyDecisionItemVM(KingdomPolicyDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			base.DecisionType = 3;
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0002E874 File Offset: 0x0002CA74
		protected override void InitValues()
		{
			base.InitValues();
			base.DecisionType = 3;
			this.NameText = this.Policy.Name.ToString();
			this.PolicyDescriptionText = this.Policy.Description.ToString();
			this.PolicyEffectList = new MBBindingList<StringItemWithHintVM>();
			foreach (string text in this.Policy.SecondaryEffects.ToString().Split(new char[] { '\n' }))
			{
				this.PolicyEffectList.Add(new StringItemWithHintVM(text, TextObject.GetEmpty()));
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0002E90E File Offset: 0x0002CB0E
		// (set) Token: 0x06000AC5 RID: 2757 RVA: 0x0002E916 File Offset: 0x0002CB16
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0002E939 File Offset: 0x0002CB39
		// (set) Token: 0x06000AC7 RID: 2759 RVA: 0x0002E941 File Offset: 0x0002CB41
		[DataSourceProperty]
		public string PolicyDescriptionText
		{
			get
			{
				return this._policyDescriptionText;
			}
			set
			{
				if (value != this._policyDescriptionText)
				{
					this._policyDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "PolicyDescriptionText");
				}
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0002E964 File Offset: 0x0002CB64
		// (set) Token: 0x06000AC9 RID: 2761 RVA: 0x0002E96C File Offset: 0x0002CB6C
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

		// Token: 0x040004BC RID: 1212
		private KingdomPolicyDecision _policyDecision;

		// Token: 0x040004BD RID: 1213
		private MBBindingList<StringItemWithHintVM> _policyEffectList;

		// Token: 0x040004BE RID: 1214
		private string _nameText;

		// Token: 0x040004BF RID: 1215
		private string _policyDescriptionText;
	}
}
