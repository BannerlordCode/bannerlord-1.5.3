using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Conversation
{
	// Token: 0x0200011F RID: 287
	public class PersuasionOptionVM : ViewModel
	{
		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06001A1A RID: 6682 RVA: 0x000631D8 File Offset: 0x000613D8
		private ConversationSentenceOption _option
		{
			get
			{
				return this._manager.CurOptions[this._index];
			}
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x000631F0 File Offset: 0x000613F0
		public PersuasionOptionVM(ConversationManager manager, int index, Action onReadyToContinue)
		{
			this._index = index;
			this._manager = manager;
			this._onReadyToContinue = onReadyToContinue;
			if (ConversationManager.GetPersuasionIsActive() && this._option.HasPersuasion)
			{
				float num;
				float num2;
				float num3;
				float num4;
				this._manager.GetPersuasionChances(this._option, out num, out num2, out num3, out num4);
				this.CritFailChance = (int)(num3 * 100f);
				this.FailChance = (int)(num4 * 100f);
				this.SuccessChance = (int)(num * 100f);
				this.CritSuccessChance = (int)(num2 * 100f);
				this._args = this._option.PersuationOptionArgs;
			}
			this.RefreshValues();
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x0006329C File Offset: 0x0006149C
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (ConversationManager.GetPersuasionIsActive() && this._option.HasPersuasion)
			{
				GameTexts.SetVariable("NUMBER", this.CritFailChance);
				this.CritFailChanceText = GameTexts.FindText("str_NUMBER_percent", null).ToString();
				GameTexts.SetVariable("NUMBER", this.FailChance);
				this.FailChanceText = GameTexts.FindText("str_NUMBER_percent", null).ToString();
				GameTexts.SetVariable("NUMBER", this.SuccessChance);
				this.SuccessChanceText = GameTexts.FindText("str_NUMBER_percent", null).ToString();
				GameTexts.SetVariable("NUMBER", this.CritSuccessChance);
				this.CritSuccessChanceText = GameTexts.FindText("str_NUMBER_percent", null).ToString();
				this.CritFailHint = new BasicTooltipViewModel(delegate
				{
					GameTexts.SetVariable("LEFT", GameTexts.FindText("str_persuasion_critical_fail", null));
					GameTexts.SetVariable("NUMBER", this.CritFailChance);
					GameTexts.SetVariable("RIGHT", GameTexts.FindText("str_NUMBER_percent", null));
					return GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
				});
				this.FailHint = new BasicTooltipViewModel(delegate
				{
					GameTexts.SetVariable("LEFT", GameTexts.FindText("str_persuasion_fail", null));
					GameTexts.SetVariable("NUMBER", this.FailChance);
					GameTexts.SetVariable("RIGHT", GameTexts.FindText("str_NUMBER_percent", null));
					return GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
				});
				this.SuccessHint = new BasicTooltipViewModel(delegate
				{
					GameTexts.SetVariable("LEFT", GameTexts.FindText("str_persuasion_success", null));
					GameTexts.SetVariable("NUMBER", this.SuccessChance);
					GameTexts.SetVariable("RIGHT", GameTexts.FindText("str_NUMBER_percent", null));
					return GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
				});
				this.CritSuccessHint = new BasicTooltipViewModel(delegate
				{
					GameTexts.SetVariable("LEFT", GameTexts.FindText("str_persuasion_critical_success", null));
					GameTexts.SetVariable("NUMBER", this.CritSuccessChance);
					GameTexts.SetVariable("RIGHT", GameTexts.FindText("str_NUMBER_percent", null));
					return GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
				});
				this.ProgressingOptionHint = new HintViewModel(GameTexts.FindText("str_persuasion_progressing_hint", null), null);
				this.BlockingOptionHint = new HintViewModel(GameTexts.FindText("str_persuasion_blocking_hint", null), null);
				this.IsABlockingOption = this._args.CanBlockOtherOption;
				this.IsAProgressingOption = this._args.CanMoveToTheNextReservation;
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x0006340D File Offset: 0x0006160D
		internal void OnPersuasionProgress(Tuple<PersuasionOptionArgs, PersuasionOptionResult> result)
		{
			this.IsPersuasionResultReady = true;
			if (result.Item1 == this._args)
			{
				this.PersuasionResultIndex = (int)result.Item2;
			}
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x00063430 File Offset: 0x00061630
		public string GetPersuasionAdditionalText()
		{
			string text = null;
			if (this._args != null)
			{
				if (this._args.SkillUsed != null)
				{
					text = ((Hero.MainHero.GetSkillValue(this._args.SkillUsed) <= 50) ? "<a style=\"Conversation.Persuasion.Neutral\"><b>{TEXT}</b></a>" : "<a style=\"Conversation.Persuasion.Positive\"><b>{TEXT}</b></a>").Replace("{TEXT}", this._args.SkillUsed.Name.ToString());
				}
				if (this._args.TraitUsed != null && !this._args.TraitUsed.IsHidden)
				{
					int traitLevel = Hero.MainHero.GetTraitLevel(this._args.TraitUsed);
					string text2;
					if (traitLevel == 0)
					{
						text2 = "<a style=\"Conversation.Persuasion.Neutral\"><b>{TEXT}</b></a>";
					}
					else
					{
						text2 = (((traitLevel > 0 && this._args.TraitEffect == TraitEffect.Positive) || (traitLevel < 0 && this._args.TraitEffect == TraitEffect.Negative)) ? "<a style=\"Conversation.Persuasion.Positive\"><b>{TEXT}</b></a>" : "<a style=\"Conversation.Persuasion.Negative\"><b>{TEXT}</b></a>");
					}
					text2 = text2.Replace("{TEXT}", this._args.TraitUsed.Name.ToString());
					if (text != null)
					{
						GameTexts.SetVariable("LEFT", text);
						GameTexts.SetVariable("RIGHT", text2);
						text = GameTexts.FindText("str_LEFT_comma_RIGHT", null).ToString();
					}
					else
					{
						text = text2;
					}
				}
				if (this._args.TraitCorrelation != null)
				{
					foreach (Tuple<TraitObject, int> tuple in this._args.TraitCorrelation)
					{
						if (tuple.Item2 != 0 && this._args.TraitUsed != tuple.Item1 && !tuple.Item1.IsHidden)
						{
							int traitLevel2 = Hero.MainHero.GetTraitLevel(tuple.Item1);
							string text3;
							if (traitLevel2 == 0)
							{
								text3 = "<a style=\"Conversation.Persuasion.Neutral\"><b>{TEXT}</b></a>";
							}
							else
							{
								text3 = ((traitLevel2 * tuple.Item2 > 0) ? "<a style=\"Conversation.Persuasion.Positive\"><b>{TEXT}</b></a>" : "<a style=\"Conversation.Persuasion.Negative\"><b>{TEXT}</b></a>");
							}
							text3 = text3.Replace("{TEXT}", tuple.Item1.Name.ToString());
							if (text != null)
							{
								GameTexts.SetVariable("LEFT", text);
								GameTexts.SetVariable("RIGHT", text3);
								text = GameTexts.FindText("str_LEFT_comma_RIGHT", null).ToString();
							}
							else
							{
								text = text3;
							}
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				GameTexts.SetVariable("STR", text);
				return GameTexts.FindText("str_STR_in_parentheses", null).ToString();
			}
			return string.Empty;
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x00063691 File Offset: 0x00061891
		public void ExecuteReadyToContinue()
		{
			Action onReadyToContinue = this._onReadyToContinue;
			if (onReadyToContinue == null)
			{
				return;
			}
			onReadyToContinue.DynamicInvokeWithLog(Array.Empty<object>());
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x000636A9 File Offset: 0x000618A9
		// (set) Token: 0x06001A21 RID: 6689 RVA: 0x000636B1 File Offset: 0x000618B1
		[DataSourceProperty]
		public bool IsPersuasionResultReady
		{
			get
			{
				return this._isPersuasionResultReady;
			}
			set
			{
				if (this._isPersuasionResultReady != value)
				{
					this._isPersuasionResultReady = value;
					base.OnPropertyChangedWithValue(value, "IsPersuasionResultReady");
				}
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06001A22 RID: 6690 RVA: 0x000636CF File Offset: 0x000618CF
		// (set) Token: 0x06001A23 RID: 6691 RVA: 0x000636D7 File Offset: 0x000618D7
		[DataSourceProperty]
		public bool IsABlockingOption
		{
			get
			{
				return this._isABlockingOption;
			}
			set
			{
				if (this._isABlockingOption != value)
				{
					this._isABlockingOption = value;
					base.OnPropertyChangedWithValue(value, "IsABlockingOption");
				}
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06001A24 RID: 6692 RVA: 0x000636F5 File Offset: 0x000618F5
		// (set) Token: 0x06001A25 RID: 6693 RVA: 0x000636FD File Offset: 0x000618FD
		[DataSourceProperty]
		public bool IsAProgressingOption
		{
			get
			{
				return this._isAProgressingOption;
			}
			set
			{
				if (this._isAProgressingOption != value)
				{
					this._isAProgressingOption = value;
					base.OnPropertyChangedWithValue(value, "IsAProgressingOption");
				}
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06001A26 RID: 6694 RVA: 0x0006371B File Offset: 0x0006191B
		// (set) Token: 0x06001A27 RID: 6695 RVA: 0x00063723 File Offset: 0x00061923
		[DataSourceProperty]
		public int SuccessChance
		{
			get
			{
				return this._successChance;
			}
			set
			{
				if (this._successChance != value)
				{
					this._successChance = value;
					base.OnPropertyChangedWithValue(value, "SuccessChance");
				}
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06001A28 RID: 6696 RVA: 0x00063741 File Offset: 0x00061941
		// (set) Token: 0x06001A29 RID: 6697 RVA: 0x00063749 File Offset: 0x00061949
		[DataSourceProperty]
		public int PersuasionResultIndex
		{
			get
			{
				return this._persuasionResultIndex;
			}
			set
			{
				if (this._persuasionResultIndex != value)
				{
					this._persuasionResultIndex = value;
					base.OnPropertyChangedWithValue(value, "PersuasionResultIndex");
				}
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06001A2A RID: 6698 RVA: 0x00063767 File Offset: 0x00061967
		// (set) Token: 0x06001A2B RID: 6699 RVA: 0x0006376F File Offset: 0x0006196F
		[DataSourceProperty]
		public int FailChance
		{
			get
			{
				return this._failChance;
			}
			set
			{
				if (this._failChance != value)
				{
					this._failChance = value;
					base.OnPropertyChangedWithValue(value, "FailChance");
				}
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06001A2C RID: 6700 RVA: 0x0006378D File Offset: 0x0006198D
		// (set) Token: 0x06001A2D RID: 6701 RVA: 0x00063795 File Offset: 0x00061995
		[DataSourceProperty]
		public int CritSuccessChance
		{
			get
			{
				return this._critSuccessChance;
			}
			set
			{
				if (this._critSuccessChance != value)
				{
					this._critSuccessChance = value;
					base.OnPropertyChangedWithValue(value, "CritSuccessChance");
				}
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06001A2E RID: 6702 RVA: 0x000637B3 File Offset: 0x000619B3
		// (set) Token: 0x06001A2F RID: 6703 RVA: 0x000637BB File Offset: 0x000619BB
		[DataSourceProperty]
		public int CritFailChance
		{
			get
			{
				return this._critFailChance;
			}
			set
			{
				if (this._critFailChance != value)
				{
					this._critFailChance = value;
					base.OnPropertyChangedWithValue(value, "CritFailChance");
				}
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06001A30 RID: 6704 RVA: 0x000637D9 File Offset: 0x000619D9
		// (set) Token: 0x06001A31 RID: 6705 RVA: 0x000637E1 File Offset: 0x000619E1
		[DataSourceProperty]
		public string FailChanceText
		{
			get
			{
				return this._failChanceText;
			}
			set
			{
				if (this._failChanceText != value)
				{
					this._failChanceText = value;
					base.OnPropertyChangedWithValue<string>(value, "FailChanceText");
				}
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06001A32 RID: 6706 RVA: 0x00063804 File Offset: 0x00061A04
		// (set) Token: 0x06001A33 RID: 6707 RVA: 0x0006380C File Offset: 0x00061A0C
		[DataSourceProperty]
		public string CritFailChanceText
		{
			get
			{
				return this._critFailChanceText;
			}
			set
			{
				if (this._critFailChanceText != value)
				{
					this._critFailChanceText = value;
					base.OnPropertyChangedWithValue<string>(value, "CritFailChanceText");
				}
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06001A34 RID: 6708 RVA: 0x0006382F File Offset: 0x00061A2F
		// (set) Token: 0x06001A35 RID: 6709 RVA: 0x00063837 File Offset: 0x00061A37
		[DataSourceProperty]
		public string SuccessChanceText
		{
			get
			{
				return this._successChanceText;
			}
			set
			{
				if (this._successChanceText != value)
				{
					this._successChanceText = value;
					base.OnPropertyChangedWithValue<string>(value, "SuccessChanceText");
				}
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06001A36 RID: 6710 RVA: 0x0006385A File Offset: 0x00061A5A
		// (set) Token: 0x06001A37 RID: 6711 RVA: 0x00063862 File Offset: 0x00061A62
		[DataSourceProperty]
		public string CritSuccessChanceText
		{
			get
			{
				return this._critSuccessChanceText;
			}
			set
			{
				if (this._critSuccessChanceText != value)
				{
					this._critSuccessChanceText = value;
					base.OnPropertyChangedWithValue<string>(value, "CritSuccessChanceText");
				}
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06001A38 RID: 6712 RVA: 0x00063885 File Offset: 0x00061A85
		// (set) Token: 0x06001A39 RID: 6713 RVA: 0x0006388D File Offset: 0x00061A8D
		[DataSourceProperty]
		public BasicTooltipViewModel CritFailHint
		{
			get
			{
				return this._critFailHint;
			}
			set
			{
				if (this._critFailHint != value)
				{
					this._critFailHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "CritFailHint");
				}
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06001A3A RID: 6714 RVA: 0x000638AB File Offset: 0x00061AAB
		// (set) Token: 0x06001A3B RID: 6715 RVA: 0x000638B3 File Offset: 0x00061AB3
		[DataSourceProperty]
		public BasicTooltipViewModel FailHint
		{
			get
			{
				return this._failHint;
			}
			set
			{
				if (this._failHint != value)
				{
					this._failHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "FailHint");
				}
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06001A3C RID: 6716 RVA: 0x000638D1 File Offset: 0x00061AD1
		// (set) Token: 0x06001A3D RID: 6717 RVA: 0x000638D9 File Offset: 0x00061AD9
		[DataSourceProperty]
		public BasicTooltipViewModel SuccessHint
		{
			get
			{
				return this._successHint;
			}
			set
			{
				if (this._successHint != value)
				{
					this._successHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "SuccessHint");
				}
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06001A3E RID: 6718 RVA: 0x000638F7 File Offset: 0x00061AF7
		// (set) Token: 0x06001A3F RID: 6719 RVA: 0x000638FF File Offset: 0x00061AFF
		[DataSourceProperty]
		public BasicTooltipViewModel CritSuccessHint
		{
			get
			{
				return this._critSuccessHint;
			}
			set
			{
				if (this._critSuccessHint != value)
				{
					this._critSuccessHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "CritSuccessHint");
				}
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06001A40 RID: 6720 RVA: 0x0006391D File Offset: 0x00061B1D
		// (set) Token: 0x06001A41 RID: 6721 RVA: 0x00063925 File Offset: 0x00061B25
		[DataSourceProperty]
		public HintViewModel BlockingOptionHint
		{
			get
			{
				return this._blockingOptionHint;
			}
			set
			{
				if (this._blockingOptionHint != value)
				{
					this._blockingOptionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "BlockingOptionHint");
				}
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06001A42 RID: 6722 RVA: 0x00063943 File Offset: 0x00061B43
		// (set) Token: 0x06001A43 RID: 6723 RVA: 0x0006394B File Offset: 0x00061B4B
		[DataSourceProperty]
		public HintViewModel ProgressingOptionHint
		{
			get
			{
				return this._progressingOptionHint;
			}
			set
			{
				if (this._progressingOptionHint != value)
				{
					this._progressingOptionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ProgressingOptionHint");
				}
			}
		}

		// Token: 0x04000BF1 RID: 3057
		private const int _minSkillValueForPositive = 50;

		// Token: 0x04000BF2 RID: 3058
		private readonly ConversationManager _manager;

		// Token: 0x04000BF3 RID: 3059
		private readonly PersuasionOptionArgs _args;

		// Token: 0x04000BF4 RID: 3060
		private readonly Action _onReadyToContinue;

		// Token: 0x04000BF5 RID: 3061
		private readonly int _index;

		// Token: 0x04000BF6 RID: 3062
		private int _critFailChance;

		// Token: 0x04000BF7 RID: 3063
		private int _failChance;

		// Token: 0x04000BF8 RID: 3064
		private int _successChance;

		// Token: 0x04000BF9 RID: 3065
		private int _critSuccessChance;

		// Token: 0x04000BFA RID: 3066
		private bool _isPersuasionResultReady;

		// Token: 0x04000BFB RID: 3067
		private int _persuasionResultIndex = -1;

		// Token: 0x04000BFC RID: 3068
		private bool _isABlockingOption;

		// Token: 0x04000BFD RID: 3069
		private bool _isAProgressingOption;

		// Token: 0x04000BFE RID: 3070
		private string _critFailChanceText;

		// Token: 0x04000BFF RID: 3071
		private string _failChanceText;

		// Token: 0x04000C00 RID: 3072
		private string _successChanceText;

		// Token: 0x04000C01 RID: 3073
		private string _critSuccessChanceText;

		// Token: 0x04000C02 RID: 3074
		private BasicTooltipViewModel _critFailHint;

		// Token: 0x04000C03 RID: 3075
		private BasicTooltipViewModel _failHint;

		// Token: 0x04000C04 RID: 3076
		private BasicTooltipViewModel _successHint;

		// Token: 0x04000C05 RID: 3077
		private BasicTooltipViewModel _critSuccessHint;

		// Token: 0x04000C06 RID: 3078
		private HintViewModel _progressingOptionHint;

		// Token: 0x04000C07 RID: 3079
		private HintViewModel _blockingOptionHint;
	}
}
