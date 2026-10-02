using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Policies
{
	// Token: 0x02000070 RID: 112
	public class KingdomPoliciesVM : KingdomCategoryVM
	{
		// Token: 0x06000886 RID: 2182 RVA: 0x00026D7C File Offset: 0x00024F7C
		public KingdomPoliciesVM(Action<KingdomDecision> forceDecide)
		{
			this._forceDecide = forceDecide;
			this.ActivePolicies = new MBBindingList<KingdomPolicyItemVM>();
			this.OtherPolicies = new MBBindingList<KingdomPolicyItemVM>();
			this.DoneHint = new HintViewModel();
			this._playerKingdom = Hero.MainHero.MapFaction as Kingdom;
			this.ProposalAndDisavowalCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfPolicyProposalAndDisavowal(Clan.PlayerClan);
			base.IsAcceptableItemSelected = false;
			this.RefreshValues();
			this.ExecuteSwitchMode();
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00026E08 File Offset: 0x00025008
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PoliciesText = GameTexts.FindText("str_policies", null).ToString();
			this.ActivePoliciesText = GameTexts.FindText("str_active_policies", null).ToString();
			this.OtherPoliciesText = GameTexts.FindText("str_other_policies", null).ToString();
			this.ProposeNewPolicyText = GameTexts.FindText("str_propose_new_policy", null).ToString();
			this.DisavowPolicyText = GameTexts.FindText("str_disavow_a_policy", null).ToString();
			base.NoItemSelectedText = GameTexts.FindText("str_kingdom_no_policy_selected", null).ToString();
			base.CategoryNameText = new TextObject("{=Sls0KQVn}Elections", null).ToString();
			this.RefreshPolicyList();
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00026EBC File Offset: 0x000250BC
		public void SelectPolicy(PolicyObject policy)
		{
			bool flag = false;
			foreach (KingdomPolicyItemVM kingdomPolicyItemVM in this.ActivePolicies)
			{
				if (kingdomPolicyItemVM.Policy == policy)
				{
					this.OnPolicySelect(kingdomPolicyItemVM);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				foreach (KingdomPolicyItemVM kingdomPolicyItemVM2 in this.OtherPolicies)
				{
					if (kingdomPolicyItemVM2.Policy == policy)
					{
						this.OnPolicySelect(kingdomPolicyItemVM2);
						flag = true;
						break;
					}
				}
			}
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00026F64 File Offset: 0x00025164
		private void OnPolicySelect(KingdomPolicyItemVM policy)
		{
			if (this.CurrentSelectedPolicy != policy)
			{
				if (this.CurrentSelectedPolicy != null)
				{
					this.CurrentSelectedPolicy.IsSelected = false;
				}
				this.CurrentSelectedPolicy = policy;
				if (this.CurrentSelectedPolicy != null)
				{
					this.CurrentSelectedPolicy.IsSelected = true;
					this._currentSelectedPolicyObject = policy.Policy;
					this._currentItemsUnresolvedDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision d)
					{
						KingdomPolicyDecision kingdomPolicyDecision;
						return (kingdomPolicyDecision = d as KingdomPolicyDecision) != null && kingdomPolicyDecision.Policy == this._currentSelectedPolicyObject && !d.ShouldBeCancelled();
					});
					if (this._currentItemsUnresolvedDecision != null)
					{
						TextObject textObject;
						this.CanProposeOrDisavowPolicy = this.GetCanProposeOrDisavowPolicyWithReason(true, out textObject);
						this.DoneHint.HintText = textObject;
						this.ProposeOrDisavowText = GameTexts.FindText("str_resolve", null).ToString();
						this.ProposeActionExplanationText = GameTexts.FindText("str_resolve_explanation", null).ToString();
						this.PolicyLikelihood = KingdomPoliciesVM.CalculateLikelihood(policy.Policy);
					}
					else
					{
						float influence = Clan.PlayerClan.Influence;
						int proposalAndDisavowalCost = this.ProposalAndDisavowalCost;
						bool isUnderMercenaryService = Clan.PlayerClan.IsUnderMercenaryService;
						TextObject textObject2;
						this.CanProposeOrDisavowPolicy = this.GetCanProposeOrDisavowPolicyWithReason(false, out textObject2);
						this.DoneHint.HintText = textObject2;
						if (this.IsPolicyActive(policy.Policy))
						{
							this.ProposeActionExplanationText = GameTexts.FindText("str_policy_propose_again_action_explanation", null).SetTextVariable("SUPPORT", KingdomPoliciesVM.GetSupportText(policy.Policy)).ToString();
						}
						else
						{
							this.ProposeActionExplanationText = GameTexts.FindText("str_policy_propose_action_explanation", null).SetTextVariable("SUPPORT", KingdomPoliciesVM.GetSupportText(policy.Policy)).ToString();
						}
						this.ProposeOrDisavowText = ((this._playerKingdom.Clans.Count > 1) ? GameTexts.FindText("str_policy_propose", null).ToString() : GameTexts.FindText("str_policy_enact", null).ToString());
						base.NotificationCount = Clan.PlayerClan.Kingdom.UnresolvedDecisions.Count<KingdomDecision>((KingdomDecision d) => !d.ShouldBeCancelled());
						this.PolicyLikelihood = KingdomPoliciesVM.CalculateLikelihood(policy.Policy);
					}
					GameTexts.SetVariable("NUMBER", this.PolicyLikelihood);
					this.PolicyLikelihoodText = GameTexts.FindText("str_NUMBER_percent", null).ToString();
				}
				base.IsAcceptableItemSelected = this.CurrentSelectedPolicy != null;
			}
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x000271A0 File Offset: 0x000253A0
		private bool GetCanProposeOrDisavowPolicyWithReason(bool hasUnresolvedDecision, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				disabledReason = GameTexts.FindText("str_mercenaries_cannot_propose_policies", null);
				return false;
			}
			if (!hasUnresolvedDecision && Clan.PlayerClan.Influence < (float)this.ProposalAndDisavowalCost)
			{
				disabledReason = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null);
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00027204 File Offset: 0x00025404
		public void RefreshPolicyList()
		{
			this.ActivePolicies.Clear();
			this.OtherPolicies.Clear();
			if (this._playerKingdom != null)
			{
				foreach (PolicyObject policyObject in this._playerKingdom.ActivePolicies)
				{
					this.ActivePolicies.Add(new KingdomPolicyItemVM(policyObject, new Action<KingdomPolicyItemVM>(this.OnPolicySelect), new Func<PolicyObject, bool>(this.IsPolicyActive)));
				}
				foreach (PolicyObject policyObject2 in PolicyObject.All.Where<PolicyObject>((PolicyObject p) => !this.IsPolicyActive(p)))
				{
					this.OtherPolicies.Add(new KingdomPolicyItemVM(policyObject2, new Action<KingdomPolicyItemVM>(this.OnPolicySelect), new Func<PolicyObject, bool>(this.IsPolicyActive)));
				}
			}
			GameTexts.SetVariable("STR", this.ActivePolicies.Count);
			this.NumOfActivePoliciesText = GameTexts.FindText("str_STR_in_parentheses", null).ToString();
			GameTexts.SetVariable("STR", this.OtherPolicies.Count);
			this.NumOfOtherPoliciesText = GameTexts.FindText("str_STR_in_parentheses", null).ToString();
			this.SetDefaultSelectedPolicy();
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00027364 File Offset: 0x00025564
		private bool IsPolicyActive(PolicyObject policy)
		{
			return this._playerKingdom.ActivePolicies.Contains(policy);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00027378 File Offset: 0x00025578
		private void SetDefaultSelectedPolicy()
		{
			KingdomPolicyItemVM kingdomPolicyItemVM = (this.IsInProposeMode ? this.OtherPolicies.FirstOrDefault<KingdomPolicyItemVM>() : this.ActivePolicies.FirstOrDefault<KingdomPolicyItemVM>());
			this.OnPolicySelect(kingdomPolicyItemVM);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x000273B0 File Offset: 0x000255B0
		private void ExecuteSwitchMode()
		{
			this.IsInProposeMode = !this.IsInProposeMode;
			this.CurrentActiveModeText = (this.IsInProposeMode ? this.OtherPoliciesText : this.ActivePoliciesText);
			this.CurrentActionText = (this.IsInProposeMode ? this.DisavowPolicyText : this.ProposeNewPolicyText);
			this.SetDefaultSelectedPolicy();
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0002740C File Offset: 0x0002560C
		private void ExecuteProposeOrDisavow()
		{
			if (this._currentItemsUnresolvedDecision != null)
			{
				this._forceDecide(this._currentItemsUnresolvedDecision);
				return;
			}
			if (this.CanProposeOrDisavowPolicy)
			{
				KingdomDecision kingdomDecision = new KingdomPolicyDecision(Clan.PlayerClan, this._currentSelectedPolicyObject, this.IsPolicyActive(this._currentSelectedPolicyObject));
				Clan.PlayerClan.Kingdom.AddDecision(kingdomDecision, false);
				this._forceDecide(kingdomDecision);
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x00027475 File Offset: 0x00025675
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x0002747D File Offset: 0x0002567D
		[DataSourceProperty]
		public HintViewModel DoneHint
		{
			get
			{
				return this._doneHint;
			}
			set
			{
				if (value != this._doneHint)
				{
					this._doneHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DoneHint");
				}
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x0002749B File Offset: 0x0002569B
		// (set) Token: 0x06000893 RID: 2195 RVA: 0x000274A3 File Offset: 0x000256A3
		[DataSourceProperty]
		public MBBindingList<KingdomPolicyItemVM> ActivePolicies
		{
			get
			{
				return this._activePolicies;
			}
			set
			{
				if (value != this._activePolicies)
				{
					this._activePolicies = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomPolicyItemVM>>(value, "ActivePolicies");
				}
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x000274C1 File Offset: 0x000256C1
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x000274C9 File Offset: 0x000256C9
		[DataSourceProperty]
		public MBBindingList<KingdomPolicyItemVM> OtherPolicies
		{
			get
			{
				return this._otherPolicies;
			}
			set
			{
				if (value != this._otherPolicies)
				{
					this._otherPolicies = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomPolicyItemVM>>(value, "OtherPolicies");
				}
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x000274E7 File Offset: 0x000256E7
		// (set) Token: 0x06000897 RID: 2199 RVA: 0x000274EF File Offset: 0x000256EF
		[DataSourceProperty]
		public KingdomPolicyItemVM CurrentSelectedPolicy
		{
			get
			{
				return this._currentSelectedPolicy;
			}
			set
			{
				if (value != this._currentSelectedPolicy)
				{
					this._currentSelectedPolicy = value;
					base.OnPropertyChangedWithValue<KingdomPolicyItemVM>(value, "CurrentSelectedPolicy");
				}
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x0002750D File Offset: 0x0002570D
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x00027515 File Offset: 0x00025715
		[DataSourceProperty]
		public bool CanProposeOrDisavowPolicy
		{
			get
			{
				return this._canProposeOrDisavowPolicy;
			}
			set
			{
				if (value != this._canProposeOrDisavowPolicy)
				{
					this._canProposeOrDisavowPolicy = value;
					base.OnPropertyChangedWithValue(value, "CanProposeOrDisavowPolicy");
				}
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x00027533 File Offset: 0x00025733
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x0002753B File Offset: 0x0002573B
		[DataSourceProperty]
		public int ProposalAndDisavowalCost
		{
			get
			{
				return this._proposalAndDisavowalCost;
			}
			set
			{
				if (value != this._proposalAndDisavowalCost)
				{
					this._proposalAndDisavowalCost = value;
					base.OnPropertyChangedWithValue(value, "ProposalAndDisavowalCost");
				}
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00027559 File Offset: 0x00025759
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x00027561 File Offset: 0x00025761
		[DataSourceProperty]
		public string NumOfActivePoliciesText
		{
			get
			{
				return this._numOfActivePoliciesText;
			}
			set
			{
				if (value != this._numOfActivePoliciesText)
				{
					this._numOfActivePoliciesText = value;
					base.OnPropertyChangedWithValue<string>(value, "NumOfActivePoliciesText");
				}
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00027584 File Offset: 0x00025784
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x0002758C File Offset: 0x0002578C
		[DataSourceProperty]
		public string NumOfOtherPoliciesText
		{
			get
			{
				return this._numOfOtherPoliciesText;
			}
			set
			{
				if (value != this._numOfOtherPoliciesText)
				{
					this._numOfOtherPoliciesText = value;
					base.OnPropertyChangedWithValue<string>(value, "NumOfOtherPoliciesText");
				}
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x000275AF File Offset: 0x000257AF
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x000275B7 File Offset: 0x000257B7
		[DataSourceProperty]
		public bool IsInProposeMode
		{
			get
			{
				return this._isInProposeMode;
			}
			set
			{
				if (value != this._isInProposeMode)
				{
					this._isInProposeMode = value;
					base.OnPropertyChangedWithValue(value, "IsInProposeMode");
				}
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x000275D5 File Offset: 0x000257D5
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x000275DD File Offset: 0x000257DD
		[DataSourceProperty]
		public string DisavowPolicyText
		{
			get
			{
				return this._disavowPolicyText;
			}
			set
			{
				if (value != this._disavowPolicyText)
				{
					this._disavowPolicyText = value;
					base.OnPropertyChangedWithValue<string>(value, "DisavowPolicyText");
				}
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00027600 File Offset: 0x00025800
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x00027608 File Offset: 0x00025808
		[DataSourceProperty]
		public string CurrentActiveModeText
		{
			get
			{
				return this._currentActiveModeText;
			}
			set
			{
				if (value != this._currentActiveModeText)
				{
					this._currentActiveModeText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentActiveModeText");
				}
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x0002762B File Offset: 0x0002582B
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x00027633 File Offset: 0x00025833
		[DataSourceProperty]
		public string CurrentActionText
		{
			get
			{
				return this._currentActionText;
			}
			set
			{
				if (value != this._currentActionText)
				{
					this._currentActionText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentActionText");
				}
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00027656 File Offset: 0x00025856
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x0002765E File Offset: 0x0002585E
		[DataSourceProperty]
		public string ProposeNewPolicyText
		{
			get
			{
				return this._proposeNewPolicyText;
			}
			set
			{
				if (value != this._proposeNewPolicyText)
				{
					this._proposeNewPolicyText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProposeNewPolicyText");
				}
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00027681 File Offset: 0x00025881
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x00027689 File Offset: 0x00025889
		[DataSourceProperty]
		public string BackText
		{
			get
			{
				return this._backText;
			}
			set
			{
				if (value != this._backText)
				{
					this._backText = value;
					base.OnPropertyChangedWithValue<string>(value, "BackText");
				}
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x000276AC File Offset: 0x000258AC
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x000276B4 File Offset: 0x000258B4
		[DataSourceProperty]
		public string PoliciesText
		{
			get
			{
				return this._policiesText;
			}
			set
			{
				if (value != this._policiesText)
				{
					this._policiesText = value;
					base.OnPropertyChangedWithValue<string>(value, "PoliciesText");
				}
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x000276D7 File Offset: 0x000258D7
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x000276DF File Offset: 0x000258DF
		[DataSourceProperty]
		public string ActivePoliciesText
		{
			get
			{
				return this._activePoliciesText;
			}
			set
			{
				if (value != this._activePoliciesText)
				{
					this._activePoliciesText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActivePoliciesText");
				}
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00027702 File Offset: 0x00025902
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x0002770A File Offset: 0x0002590A
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

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0002772D File Offset: 0x0002592D
		// (set) Token: 0x060008B3 RID: 2227 RVA: 0x00027735 File Offset: 0x00025935
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

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x00027753 File Offset: 0x00025953
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x0002775B File Offset: 0x0002595B
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

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x00027779 File Offset: 0x00025979
		// (set) Token: 0x060008B7 RID: 2231 RVA: 0x00027781 File Offset: 0x00025981
		[DataSourceProperty]
		public string OtherPoliciesText
		{
			get
			{
				return this._otherPoliciesText;
			}
			set
			{
				if (value != this._otherPoliciesText)
				{
					this._otherPoliciesText = value;
					base.OnPropertyChangedWithValue<string>(value, "OtherPoliciesText");
				}
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x000277A4 File Offset: 0x000259A4
		// (set) Token: 0x060008B9 RID: 2233 RVA: 0x000277AC File Offset: 0x000259AC
		[DataSourceProperty]
		public string ProposeOrDisavowText
		{
			get
			{
				return this._proposeOrDisavowText;
			}
			set
			{
				if (value != this._proposeOrDisavowText)
				{
					this._proposeOrDisavowText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProposeOrDisavowText");
				}
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x000277CF File Offset: 0x000259CF
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x000277D7 File Offset: 0x000259D7
		[DataSourceProperty]
		public string ProposeActionExplanationText
		{
			get
			{
				return this._proposeActionExplanationText;
			}
			set
			{
				if (value != this._proposeActionExplanationText)
				{
					this._proposeActionExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProposeActionExplanationText");
				}
			}
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x000277FC File Offset: 0x000259FC
		private static int CalculateLikelihood(PolicyObject policy)
		{
			KingdomElection kingdomElection = new KingdomElection(new KingdomPolicyDecision(Clan.PlayerClan, policy, Clan.PlayerClan.Kingdom.ActivePolicies.Contains(policy)));
			kingdomElection.SetupResultWithoutPlayerSupport();
			return MathF.Round(kingdomElection.GetWinChanceForSponsor(Clan.PlayerClan) * 100f);
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0002784C File Offset: 0x00025A4C
		private static TextObject GetSupportText(PolicyObject policy)
		{
			KingdomPolicyDecision kingdomPolicyDecision = new KingdomPolicyDecision(Clan.PlayerClan, policy, Clan.PlayerClan.Kingdom.ActivePolicies.Contains(policy));
			return GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.GetElectionOutcomeSupport(kingdomPolicyDecision, Clan.PlayerClan).ToString());
		}

		// Token: 0x040003AB RID: 939
		private readonly Action<KingdomDecision> _forceDecide;

		// Token: 0x040003AC RID: 940
		private readonly Kingdom _playerKingdom;

		// Token: 0x040003AD RID: 941
		private PolicyObject _currentSelectedPolicyObject;

		// Token: 0x040003AE RID: 942
		private KingdomDecision _currentItemsUnresolvedDecision;

		// Token: 0x040003AF RID: 943
		private MBBindingList<KingdomPolicyItemVM> _activePolicies;

		// Token: 0x040003B0 RID: 944
		private MBBindingList<KingdomPolicyItemVM> _otherPolicies;

		// Token: 0x040003B1 RID: 945
		private KingdomPolicyItemVM _currentSelectedPolicy;

		// Token: 0x040003B2 RID: 946
		private bool _canProposeOrDisavowPolicy;

		// Token: 0x040003B3 RID: 947
		private bool _isInProposeMode = true;

		// Token: 0x040003B4 RID: 948
		private string _proposeOrDisavowText;

		// Token: 0x040003B5 RID: 949
		private string _proposeActionExplanationText;

		// Token: 0x040003B6 RID: 950
		private string _activePoliciesText;

		// Token: 0x040003B7 RID: 951
		private string _otherPoliciesText;

		// Token: 0x040003B8 RID: 952
		private string _currentActiveModeText;

		// Token: 0x040003B9 RID: 953
		private string _currentActionText;

		// Token: 0x040003BA RID: 954
		private string _proposeNewPolicyText;

		// Token: 0x040003BB RID: 955
		private string _disavowPolicyText;

		// Token: 0x040003BC RID: 956
		private string _policiesText;

		// Token: 0x040003BD RID: 957
		private string _backText;

		// Token: 0x040003BE RID: 958
		private int _proposalAndDisavowalCost;

		// Token: 0x040003BF RID: 959
		private string _numOfActivePoliciesText;

		// Token: 0x040003C0 RID: 960
		private string _numOfOtherPoliciesText;

		// Token: 0x040003C1 RID: 961
		private HintViewModel _doneHint;

		// Token: 0x040003C2 RID: 962
		private string _policyLikelihoodText;

		// Token: 0x040003C3 RID: 963
		private HintViewModel _likelihoodHint;

		// Token: 0x040003C4 RID: 964
		private int _policyLikelihood;
	}
}
