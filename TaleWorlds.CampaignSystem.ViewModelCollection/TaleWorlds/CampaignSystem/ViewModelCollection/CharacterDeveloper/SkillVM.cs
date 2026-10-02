using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper
{
	// Token: 0x0200014B RID: 331
	public class SkillVM : ViewModel
	{
		// Token: 0x06002034 RID: 8244 RVA: 0x00074228 File Offset: 0x00072428
		public SkillVM(SkillObject skill, CharacterDeveloperHeroItemVM heroItem, Action<PerkVM> onStartPerkSelection)
		{
			SkillVM <>4__this = this;
			this._heroItem = heroItem;
			this.Skill = skill;
			this.MaxLevel = 300;
			this.SkillId = skill.StringId;
			this._onStartPerkSelection = onStartPerkSelection;
			this.IsInspected = false;
			this.SkillEffects = new MBBindingList<BindingListStringItem>();
			this.Perks = new MBBindingList<PerkVM>();
			this.AddFocusHint = new HintViewModel();
			this.LearningRateTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetLearningRateTooltip(<>4__this._heroItem.CharacterAttributes, <>4__this.CurrentFocusLevel, heroItem.Hero.GetSkillValue(skill), <>4__this.Skill));
			this.LearningLimitTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetLearningLimitTooltip(<>4__this._heroItem.CharacterAttributes, <>4__this.CurrentFocusLevel, <>4__this.Skill));
			this.InitializeValues();
			this._focusConceptObj = Concept.All.SingleOrDefault<Concept>((Concept c) => c.StringId == "str_game_objects_skill_focus");
			this._skillConceptObj = Concept.All.SingleOrDefault<Concept>((Concept c) => c.StringId == "str_game_objects_skills");
			this.RefreshValues();
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x00074358 File Offset: 0x00072558
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.AddFocusText = GameTexts.FindText("str_add_focus", null).ToString();
			this.HowToLearnText = this.Skill.HowToLearnSkillText.ToString();
			this.HowToLearnTitle = GameTexts.FindText("str_how_to_learn", null).ToString();
			this.DescriptionText = this.Skill.Description.ToString();
			this.NameText = this.Skill.Name.ToString();
			this.AttributesText = GameTexts.GameTextHelper.MergeTextObjectsWithComma(this.Skill.Attributes.Select<CharacterAttribute, TextObject>((CharacterAttribute x) => x.Abbreviation).ToList<TextObject>(), false).ToString();
			this.InitializeValues();
			this.RefreshWithCurrentValues();
			this.SkillEffects.ApplyActionOnAllItems(delegate(BindingListStringItem x)
			{
				x.RefreshValues();
			});
			this.Perks.ApplyActionOnAllItems(delegate(PerkVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x00074480 File Offset: 0x00072680
		public void InitializeValues()
		{
			if (this._heroItem.HeroDeveloper == null)
			{
				this.Level = 0;
			}
			else
			{
				this.Level = this._heroItem.HeroDeveloper.Hero.GetSkillValue(this.Skill);
				this.NextLevel = this.Level + 1;
				this.CurrentSkillXP = this._heroItem.HeroDeveloper.GetSkillXpProgress(this.Skill);
				this.XpRequiredForNextLevel = Campaign.Current.Models.CharacterDevelopmentModel.GetXpRequiredForSkillLevel(this.Level + 1) - Campaign.Current.Models.CharacterDevelopmentModel.GetXpRequiredForSkillLevel(this.Level);
				this.ProgressPercentage = 100.0 * (double)this._currentSkillXP / (double)this.XpRequiredForNextLevel;
				this.ProgressHint = new BasicTooltipViewModel(delegate
				{
					GameTexts.SetVariable("CURRENT_XP", this.CurrentSkillXP.ToString());
					GameTexts.SetVariable("LEVEL_MAX_XP", this.XpRequiredForNextLevel.ToString());
					return GameTexts.FindText("str_current_xp_over_max", null).ToString();
				});
				GameTexts.SetVariable("CURRENT_XP", this.CurrentSkillXP.ToString());
				GameTexts.SetVariable("LEVEL_MAX_XP", this.XpRequiredForNextLevel.ToString());
				this.ProgressText = GameTexts.FindText("str_current_xp_over_max", null).ToString();
				this.SkillXPHint = new BasicTooltipViewModel(delegate
				{
					GameTexts.SetVariable("REQUIRED_XP_FOR_NEXT_LEVEL", this.XpRequiredForNextLevel - this.CurrentSkillXP);
					return GameTexts.FindText("str_skill_xp_hint", null).ToString();
				});
			}
			this._orgFocusAmount = this._heroItem.HeroDeveloper.GetFocus(this.Skill);
			this.CurrentFocusLevel = this._orgFocusAmount;
			this.CreateLists();
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x000745F0 File Offset: 0x000727F0
		public void RefreshWithCurrentValues()
		{
			float resultNumber = Campaign.Current.Models.CharacterDevelopmentModel.CalculateLearningRate(this._heroItem.CharacterAttributes, this.CurrentFocusLevel, this._heroItem.Hero.GetSkillValue(this.Skill), this.Skill, false).ResultNumber;
			GameTexts.SetVariable("COUNT", resultNumber.ToString("0.00"));
			this.CurrentLearningRateText = GameTexts.FindText("str_learning_rate_COUNT", null).ToString();
			this.CanLearnSkill = Math.Round((double)resultNumber, 2) > 0.0;
			this.LearningRate = resultNumber;
			this.FullLearningRateLevel = MathF.Round(Campaign.Current.Models.CharacterDevelopmentModel.CalculateLearningLimit(this._heroItem.CharacterAttributes, this.CurrentFocusLevel, this.Skill, false).ResultNumber);
			int requiredFocusPointsToAddFocusWithCurrentFocus = this._heroItem.GetRequiredFocusPointsToAddFocusWithCurrentFocus(this.Skill);
			GameTexts.SetVariable("COSTAMOUNT", requiredFocusPointsToAddFocusWithCurrentFocus);
			this.FocusCostText = requiredFocusPointsToAddFocusWithCurrentFocus.ToString();
			GameTexts.SetVariable("COUNT", requiredFocusPointsToAddFocusWithCurrentFocus);
			GameTexts.SetVariable("RIGHT", "");
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_cost_COUNT", null));
			MBTextManager.SetTextVariable("FOCUS_ICON", "{=!}<img src=\"CharacterDeveloper\\cp_icon\">", false);
			this.NextLevelCostText = GameTexts.FindText("str_sf_text_with_focus_icon", null).ToString();
			this.RefreshCanAddFocus();
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x00074758 File Offset: 0x00072958
		public void CreateLists()
		{
			this.SkillEffects.Clear();
			this.Perks.Clear();
			int skillValue = this._heroItem.HeroDeveloper.Hero.GetSkillValue(this.Skill);
			foreach (SkillEffect skillEffect in SkillEffect.All.Where<SkillEffect>((SkillEffect x) => x.EffectedSkill == this.Skill))
			{
				this.SkillEffects.Add(new BindingListStringItem(CampaignUIHelper.GetSkillEffectText(skillEffect, skillValue)));
			}
			foreach (PerkObject perkObject in from p in PerkObject.All
				where p.Skill == this.Skill
				orderby p.RequiredSkillValue
				select p)
			{
				PerkVM.PerkAlternativeType perkAlternativeType = ((perkObject.AlternativePerk == null) ? PerkVM.PerkAlternativeType.NoAlternative : ((perkObject.StringId.CompareTo(perkObject.AlternativePerk.StringId) < 0) ? PerkVM.PerkAlternativeType.FirstAlternative : PerkVM.PerkAlternativeType.SecondAlternative));
				PerkVM perkVM = new PerkVM(perkObject, this.IsPerkAvailable(perkObject), perkAlternativeType, new Action<PerkVM>(this.OnStartPerkSelection), new Action<PerkVM>(this.OnPerkSelectionOver), new Func<PerkObject, bool>(this.IsPerkSelected), new Func<PerkObject, bool>(this.IsPreviousPerkSelected));
				this.Perks.Add(perkVM);
			}
			this.RefreshNumOfUnopenedPerks();
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x000748EC File Offset: 0x00072AEC
		public void RefreshLists(SkillObject skill = null)
		{
			if (skill != null && skill != this.Skill)
			{
				return;
			}
			foreach (PerkVM perkVM in this.Perks)
			{
				perkVM.RefreshState();
			}
			this.RefreshNumOfUnopenedPerks();
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x0007494C File Offset: 0x00072B4C
		private void RefreshNumOfUnopenedPerks()
		{
			int num = 0;
			foreach (PerkVM perkVM in this.Perks)
			{
				if ((perkVM.CurrentState == PerkVM.PerkStates.EarnedButNotSelected || perkVM.CurrentState == PerkVM.PerkStates.EarnedPreviousPerkNotSelected) && (perkVM.AlternativeType == 1 || perkVM.AlternativeType == 0))
				{
					num++;
				}
			}
			this.NumOfUnopenedPerks = num;
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x000749C4 File Offset: 0x00072BC4
		private bool IsPerkSelected(PerkObject perk)
		{
			return this._heroItem.HeroDeveloper.GetPerkValue(perk) || this._heroItem.PerkSelection.IsPerkSelected(perk);
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x000749EC File Offset: 0x00072BEC
		private bool IsPreviousPerkSelected(PerkObject perk)
		{
			IEnumerable<PerkObject> enumerable = PerkObject.All.Where<PerkObject>((PerkObject p) => p.Skill == perk.Skill && p.RequiredSkillValue < perk.RequiredSkillValue);
			if (!enumerable.Any<PerkObject>())
			{
				return true;
			}
			PerkObject perkObject = enumerable.MaxBy<PerkObject, float>((PerkObject p) => p.RequiredSkillValue - perk.RequiredSkillValue);
			return this.IsPerkSelected(perkObject) || (perkObject.AlternativePerk != null && this.IsPerkSelected(perkObject.AlternativePerk));
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x00074A5B File Offset: 0x00072C5B
		private bool IsPerkAvailable(PerkObject perk)
		{
			return perk.RequiredSkillValue <= (float)this.Level;
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x00074A70 File Offset: 0x00072C70
		public void RefreshCanAddFocus()
		{
			bool flag = this._heroItem.UnspentCharacterPoints >= this._heroItem.GetRequiredFocusPointsToAddFocusWithCurrentFocus(this.Skill);
			bool flag2 = this._currentFocusLevel >= Campaign.Current.Models.CharacterDevelopmentModel.MaxFocusPerSkill;
			string addFocusHintString = CampaignUIHelper.GetAddFocusHintString(flag, flag2, this.CurrentFocusLevel);
			this.AddFocusHint.HintText = (string.IsNullOrEmpty(addFocusHintString) ? TextObject.GetEmpty() : new TextObject("{=!}" + addFocusHintString, null));
			this.CanAddFocus = this._heroItem.CanAddFocusToSkillWithFocusAmount(this._currentFocusLevel);
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x00074B10 File Offset: 0x00072D10
		public void ExecuteAddFocus()
		{
			if (this.CanAddFocus)
			{
				this._heroItem.UnspentCharacterPoints -= this._heroItem.GetRequiredFocusPointsToAddFocusWithCurrentFocus(this.Skill);
				int currentFocusLevel = this.CurrentFocusLevel;
				this.CurrentFocusLevel = currentFocusLevel + 1;
				this._heroItem.RefreshCharacterValues();
				this.RefreshWithCurrentValues();
				MBInformationManager.HideInformations();
				Game.Current.EventManager.TriggerEvent<FocusAddedByPlayerEvent>(new FocusAddedByPlayerEvent(this._heroItem.Hero, this.Skill));
			}
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x00074B93 File Offset: 0x00072D93
		public void ExecuteShowFocusConcept()
		{
			if (this._focusConceptObj != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._focusConceptObj.EncyclopediaLink);
				return;
			}
			Debug.FailedAssert("Couldn't find Focus encyclopedia page", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\CharacterDeveloper\\SkillVM.cs", "ExecuteShowFocusConcept", 246);
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x00074BD1 File Offset: 0x00072DD1
		public void ExecuteShowSkillConcept()
		{
			if (this._focusConceptObj != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._skillConceptObj.EncyclopediaLink);
				return;
			}
			Debug.FailedAssert("Couldn't find Focus encyclopedia page", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\CharacterDeveloper\\SkillVM.cs", "ExecuteShowSkillConcept", 258);
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x00074C0F File Offset: 0x00072E0F
		public void ExecuteInspect()
		{
			this._heroItem.SetCurrentSkill(this);
			this.RefreshCanAddFocus();
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x00074C23 File Offset: 0x00072E23
		public void ResetChanges()
		{
			this.CurrentFocusLevel = this._orgFocusAmount;
			this.Perks.ApplyActionOnAllItems(delegate(PerkVM p)
			{
				p.RefreshState();
			});
			this.RefreshNumOfUnopenedPerks();
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x00074C61 File Offset: 0x00072E61
		public bool IsThereAnyChanges()
		{
			return this.CurrentFocusLevel != this._orgFocusAmount;
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x00074C74 File Offset: 0x00072E74
		public void ApplyChanges()
		{
			for (int i = 0; i < this.CurrentFocusLevel - this._orgFocusAmount; i++)
			{
				this._heroItem.HeroDeveloper.AddFocus(this.Skill, 1, true);
			}
			this._orgFocusAmount = this.CurrentFocusLevel;
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x00074CC0 File Offset: 0x00072EC0
		private void OnStartPerkSelection(PerkVM perk)
		{
			this._onStartPerkSelection(perk);
			if (perk.AlternativeType != 0)
			{
				this.Perks.SingleOrDefault<PerkVM>((PerkVM p) => p.Perk == perk.Perk.AlternativePerk);
			}
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x00074D10 File Offset: 0x00072F10
		private void OnPerkSelectionOver(PerkVM perk)
		{
			if (perk.AlternativeType != 0)
			{
				this.Perks.SingleOrDefault<PerkVM>((PerkVM p) => p.Perk == perk.Perk.AlternativePerk);
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06002048 RID: 8264 RVA: 0x00074D4F File Offset: 0x00072F4F
		// (set) Token: 0x06002049 RID: 8265 RVA: 0x00074D57 File Offset: 0x00072F57
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x0600204A RID: 8266 RVA: 0x00074D7A File Offset: 0x00072F7A
		// (set) Token: 0x0600204B RID: 8267 RVA: 0x00074D82 File Offset: 0x00072F82
		[DataSourceProperty]
		public string HowToLearnText
		{
			get
			{
				return this._howToLearnText;
			}
			set
			{
				if (value != this._howToLearnText)
				{
					this._howToLearnText = value;
					base.OnPropertyChangedWithValue<string>(value, "HowToLearnText");
				}
			}
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x0600204C RID: 8268 RVA: 0x00074DA5 File Offset: 0x00072FA5
		// (set) Token: 0x0600204D RID: 8269 RVA: 0x00074DAD File Offset: 0x00072FAD
		[DataSourceProperty]
		public string HowToLearnTitle
		{
			get
			{
				return this._howToLearnTitle;
			}
			set
			{
				if (value != this._howToLearnTitle)
				{
					this._howToLearnTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "HowToLearnTitle");
				}
			}
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x0600204E RID: 8270 RVA: 0x00074DD0 File Offset: 0x00072FD0
		// (set) Token: 0x0600204F RID: 8271 RVA: 0x00074DD8 File Offset: 0x00072FD8
		[DataSourceProperty]
		public string AttributesText
		{
			get
			{
				return this._attributesText;
			}
			set
			{
				if (value != this._attributesText)
				{
					this._attributesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AttributesText");
				}
			}
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06002050 RID: 8272 RVA: 0x00074DFB File Offset: 0x00072FFB
		// (set) Token: 0x06002051 RID: 8273 RVA: 0x00074E03 File Offset: 0x00073003
		[DataSourceProperty]
		public bool CanAddFocus
		{
			get
			{
				return this._canAddFocus;
			}
			set
			{
				if (value != this._canAddFocus)
				{
					this._canAddFocus = value;
					base.OnPropertyChangedWithValue(value, "CanAddFocus");
				}
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06002052 RID: 8274 RVA: 0x00074E21 File Offset: 0x00073021
		// (set) Token: 0x06002053 RID: 8275 RVA: 0x00074E29 File Offset: 0x00073029
		[DataSourceProperty]
		public bool CanLearnSkill
		{
			get
			{
				return this._canLearnSkill;
			}
			set
			{
				if (value != this._canLearnSkill)
				{
					this._canLearnSkill = value;
					base.OnPropertyChangedWithValue(value, "CanLearnSkill");
				}
			}
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06002054 RID: 8276 RVA: 0x00074E47 File Offset: 0x00073047
		// (set) Token: 0x06002055 RID: 8277 RVA: 0x00074E4F File Offset: 0x0007304F
		[DataSourceProperty]
		public string NextLevelLearningRateText
		{
			get
			{
				return this._nextLevelLearningRateText;
			}
			set
			{
				if (value != this._nextLevelLearningRateText)
				{
					this._nextLevelLearningRateText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextLevelLearningRateText");
				}
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06002056 RID: 8278 RVA: 0x00074E72 File Offset: 0x00073072
		// (set) Token: 0x06002057 RID: 8279 RVA: 0x00074E7A File Offset: 0x0007307A
		[DataSourceProperty]
		public string NextLevelCostText
		{
			get
			{
				return this._nextLevelCostText;
			}
			set
			{
				if (value != this._nextLevelCostText)
				{
					this._nextLevelCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextLevelCostText");
				}
			}
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06002058 RID: 8280 RVA: 0x00074E9D File Offset: 0x0007309D
		// (set) Token: 0x06002059 RID: 8281 RVA: 0x00074EA5 File Offset: 0x000730A5
		[DataSourceProperty]
		public BasicTooltipViewModel ProgressHint
		{
			get
			{
				return this._progressHint;
			}
			set
			{
				if (value != this._progressHint)
				{
					this._progressHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ProgressHint");
				}
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x0600205A RID: 8282 RVA: 0x00074EC3 File Offset: 0x000730C3
		// (set) Token: 0x0600205B RID: 8283 RVA: 0x00074ECB File Offset: 0x000730CB
		[DataSourceProperty]
		public BasicTooltipViewModel SkillXPHint
		{
			get
			{
				return this._skillXPHint;
			}
			set
			{
				if (value != this._skillXPHint)
				{
					this._skillXPHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "SkillXPHint");
				}
			}
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x0600205C RID: 8284 RVA: 0x00074EE9 File Offset: 0x000730E9
		// (set) Token: 0x0600205D RID: 8285 RVA: 0x00074EF1 File Offset: 0x000730F1
		[DataSourceProperty]
		public HintViewModel AddFocusHint
		{
			get
			{
				return this._addFocusHint;
			}
			set
			{
				if (value != this._addFocusHint)
				{
					this._addFocusHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AddFocusHint");
				}
			}
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x0600205E RID: 8286 RVA: 0x00074F0F File Offset: 0x0007310F
		// (set) Token: 0x0600205F RID: 8287 RVA: 0x00074F17 File Offset: 0x00073117
		[DataSourceProperty]
		public BasicTooltipViewModel LearningLimitTooltip
		{
			get
			{
				return this._learningLimitTooltip;
			}
			set
			{
				if (value != this._learningLimitTooltip)
				{
					this._learningLimitTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "LearningLimitTooltip");
				}
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06002060 RID: 8288 RVA: 0x00074F35 File Offset: 0x00073135
		// (set) Token: 0x06002061 RID: 8289 RVA: 0x00074F3D File Offset: 0x0007313D
		[DataSourceProperty]
		public BasicTooltipViewModel LearningRateTooltip
		{
			get
			{
				return this._learningRateTooltip;
			}
			set
			{
				if (value != this._learningRateTooltip)
				{
					this._learningRateTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "LearningRateTooltip");
				}
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06002062 RID: 8290 RVA: 0x00074F5B File Offset: 0x0007315B
		// (set) Token: 0x06002063 RID: 8291 RVA: 0x00074F63 File Offset: 0x00073163
		[DataSourceProperty]
		public double ProgressPercentage
		{
			get
			{
				return this._progressPercentage;
			}
			set
			{
				if (value != this._progressPercentage)
				{
					this._progressPercentage = value;
					base.OnPropertyChangedWithValue(value, "ProgressPercentage");
				}
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06002064 RID: 8292 RVA: 0x00074F81 File Offset: 0x00073181
		// (set) Token: 0x06002065 RID: 8293 RVA: 0x00074F89 File Offset: 0x00073189
		[DataSourceProperty]
		public float LearningRate
		{
			get
			{
				return this._learningRate;
			}
			set
			{
				if (value != this._learningRate)
				{
					this._learningRate = value;
					base.OnPropertyChangedWithValue(value, "LearningRate");
				}
			}
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06002066 RID: 8294 RVA: 0x00074FA7 File Offset: 0x000731A7
		// (set) Token: 0x06002067 RID: 8295 RVA: 0x00074FAF File Offset: 0x000731AF
		[DataSourceProperty]
		public int CurrentSkillXP
		{
			get
			{
				return this._currentSkillXP;
			}
			set
			{
				if (value != this._currentSkillXP)
				{
					this._currentSkillXP = value;
					base.OnPropertyChangedWithValue(value, "CurrentSkillXP");
				}
			}
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x00074FCD File Offset: 0x000731CD
		// (set) Token: 0x06002069 RID: 8297 RVA: 0x00074FD5 File Offset: 0x000731D5
		[DataSourceProperty]
		public int NextLevel
		{
			get
			{
				return this._nextLevel;
			}
			set
			{
				if (value != this._nextLevel)
				{
					this._nextLevel = value;
					base.OnPropertyChangedWithValue(value, "NextLevel");
				}
			}
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x00074FF3 File Offset: 0x000731F3
		// (set) Token: 0x0600206B RID: 8299 RVA: 0x00074FFB File Offset: 0x000731FB
		[DataSourceProperty]
		public int FullLearningRateLevel
		{
			get
			{
				return this._fullLearningRateLevel;
			}
			set
			{
				if (value != this._fullLearningRateLevel)
				{
					this._fullLearningRateLevel = value;
					base.OnPropertyChangedWithValue(value, "FullLearningRateLevel");
				}
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x0600206C RID: 8300 RVA: 0x00075019 File Offset: 0x00073219
		// (set) Token: 0x0600206D RID: 8301 RVA: 0x00075021 File Offset: 0x00073221
		[DataSourceProperty]
		public int XpRequiredForNextLevel
		{
			get
			{
				return this._xpRequiredForNextLevel;
			}
			set
			{
				if (value != this._xpRequiredForNextLevel)
				{
					this._xpRequiredForNextLevel = value;
					base.OnPropertyChangedWithValue(value, "XpRequiredForNextLevel");
				}
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x0600206E RID: 8302 RVA: 0x0007503F File Offset: 0x0007323F
		// (set) Token: 0x0600206F RID: 8303 RVA: 0x00075047 File Offset: 0x00073247
		[DataSourceProperty]
		public int NumOfUnopenedPerks
		{
			get
			{
				return this._numOfUnopenedPerks;
			}
			set
			{
				if (value != this._numOfUnopenedPerks)
				{
					this._numOfUnopenedPerks = value;
					base.OnPropertyChangedWithValue(value, "NumOfUnopenedPerks");
				}
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06002070 RID: 8304 RVA: 0x00075065 File Offset: 0x00073265
		// (set) Token: 0x06002071 RID: 8305 RVA: 0x0007506D File Offset: 0x0007326D
		[DataSourceProperty]
		public string ProgressText
		{
			get
			{
				return this._progressText;
			}
			set
			{
				if (value != this._progressText)
				{
					this._progressText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProgressText");
				}
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06002072 RID: 8306 RVA: 0x00075090 File Offset: 0x00073290
		// (set) Token: 0x06002073 RID: 8307 RVA: 0x00075098 File Offset: 0x00073298
		[DataSourceProperty]
		public string FocusCostText
		{
			get
			{
				return this._focusCostText;
			}
			set
			{
				if (value != this._focusCostText)
				{
					this._focusCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "FocusCostText");
				}
			}
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06002074 RID: 8308 RVA: 0x000750BB File Offset: 0x000732BB
		// (set) Token: 0x06002075 RID: 8309 RVA: 0x000750C3 File Offset: 0x000732C3
		[DataSourceProperty]
		public MBBindingList<PerkVM> Perks
		{
			get
			{
				return this._perks;
			}
			set
			{
				if (value != this._perks)
				{
					this._perks = value;
					base.OnPropertyChangedWithValue<MBBindingList<PerkVM>>(value, "Perks");
				}
			}
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06002076 RID: 8310 RVA: 0x000750E1 File Offset: 0x000732E1
		// (set) Token: 0x06002077 RID: 8311 RVA: 0x000750E9 File Offset: 0x000732E9
		[DataSourceProperty]
		public MBBindingList<BindingListStringItem> SkillEffects
		{
			get
			{
				return this._skillEffects;
			}
			set
			{
				if (value != this._skillEffects)
				{
					this._skillEffects = value;
					base.OnPropertyChangedWithValue<MBBindingList<BindingListStringItem>>(value, "SkillEffects");
				}
			}
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x00075107 File Offset: 0x00073307
		// (set) Token: 0x06002079 RID: 8313 RVA: 0x0007510F File Offset: 0x0007330F
		[DataSourceProperty]
		public int MaxLevel
		{
			get
			{
				return this._maxLevel;
			}
			set
			{
				if (value != this._maxLevel)
				{
					this._maxLevel = value;
					base.OnPropertyChangedWithValue(value, "MaxLevel");
				}
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x0600207A RID: 8314 RVA: 0x0007512D File Offset: 0x0007332D
		// (set) Token: 0x0600207B RID: 8315 RVA: 0x00075135 File Offset: 0x00073335
		[DataSourceProperty]
		public string CurrentLearningRateText
		{
			get
			{
				return this._currentLearningRateText;
			}
			set
			{
				if (value != this._currentLearningRateText)
				{
					this._currentLearningRateText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentLearningRateText");
				}
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x00075158 File Offset: 0x00073358
		// (set) Token: 0x0600207D RID: 8317 RVA: 0x00075160 File Offset: 0x00073360
		[DataSourceProperty]
		public int CurrentFocusLevel
		{
			get
			{
				return this._currentFocusLevel;
			}
			set
			{
				if (value != this._currentFocusLevel)
				{
					this._currentFocusLevel = value;
					base.OnPropertyChangedWithValue(value, "CurrentFocusLevel");
				}
			}
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x0007517E File Offset: 0x0007337E
		// (set) Token: 0x0600207F RID: 8319 RVA: 0x00075186 File Offset: 0x00073386
		[DataSourceProperty]
		public string AddFocusText
		{
			get
			{
				return this._addFocusText;
			}
			set
			{
				if (value != this._addFocusText)
				{
					this._addFocusText = value;
					base.OnPropertyChangedWithValue<string>(value, "AddFocusText");
				}
			}
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06002080 RID: 8320 RVA: 0x000751A9 File Offset: 0x000733A9
		// (set) Token: 0x06002081 RID: 8321 RVA: 0x000751B1 File Offset: 0x000733B1
		[DataSourceProperty]
		public string SkillId
		{
			get
			{
				return this._skillId;
			}
			set
			{
				if (value != this._skillId)
				{
					this._skillId = value;
					base.OnPropertyChangedWithValue<string>(value, "SkillId");
				}
			}
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06002082 RID: 8322 RVA: 0x000751D4 File Offset: 0x000733D4
		// (set) Token: 0x06002083 RID: 8323 RVA: 0x000751DC File Offset: 0x000733DC
		[DataSourceProperty]
		public bool IsInspected
		{
			get
			{
				return this._isInspected;
			}
			set
			{
				if (value != this._isInspected)
				{
					this._isInspected = value;
					base.OnPropertyChangedWithValue(value, "IsInspected");
				}
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06002084 RID: 8324 RVA: 0x000751FA File Offset: 0x000733FA
		// (set) Token: 0x06002085 RID: 8325 RVA: 0x00075202 File Offset: 0x00073402
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

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06002086 RID: 8326 RVA: 0x00075225 File Offset: 0x00073425
		// (set) Token: 0x06002087 RID: 8327 RVA: 0x0007522D File Offset: 0x0007342D
		[DataSourceProperty]
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (value != this._level)
				{
					this._level = value;
					base.OnPropertyChangedWithValue(value, "Level");
				}
			}
		}

		// Token: 0x04000EC3 RID: 3779
		public const int MAX_SKILL_LEVEL = 300;

		// Token: 0x04000EC4 RID: 3780
		public readonly SkillObject Skill;

		// Token: 0x04000EC5 RID: 3781
		private readonly CharacterDeveloperHeroItemVM _heroItem;

		// Token: 0x04000EC6 RID: 3782
		private readonly Concept _focusConceptObj;

		// Token: 0x04000EC7 RID: 3783
		private readonly Concept _skillConceptObj;

		// Token: 0x04000EC8 RID: 3784
		private readonly Action<PerkVM> _onStartPerkSelection;

		// Token: 0x04000EC9 RID: 3785
		private int _orgFocusAmount;

		// Token: 0x04000ECA RID: 3786
		private MBBindingList<BindingListStringItem> _skillEffects;

		// Token: 0x04000ECB RID: 3787
		private MBBindingList<PerkVM> _perks;

		// Token: 0x04000ECC RID: 3788
		private BasicTooltipViewModel _progressHint;

		// Token: 0x04000ECD RID: 3789
		private HintViewModel _addFocusHint;

		// Token: 0x04000ECE RID: 3790
		private BasicTooltipViewModel _skillXPHint;

		// Token: 0x04000ECF RID: 3791
		private BasicTooltipViewModel _learningLimitTooltip;

		// Token: 0x04000ED0 RID: 3792
		private BasicTooltipViewModel _learningRateTooltip;

		// Token: 0x04000ED1 RID: 3793
		private string _nameText;

		// Token: 0x04000ED2 RID: 3794
		private string _skillId;

		// Token: 0x04000ED3 RID: 3795
		private string _addFocusText;

		// Token: 0x04000ED4 RID: 3796
		private string _focusCostText;

		// Token: 0x04000ED5 RID: 3797
		private string _currentLearningRateText;

		// Token: 0x04000ED6 RID: 3798
		private string _nextLevelLearningRateText;

		// Token: 0x04000ED7 RID: 3799
		private string _nextLevelCostText;

		// Token: 0x04000ED8 RID: 3800
		private string _howToLearnText;

		// Token: 0x04000ED9 RID: 3801
		private string _howToLearnTitle;

		// Token: 0x04000EDA RID: 3802
		private string _progressText;

		// Token: 0x04000EDB RID: 3803
		private string _descriptionText;

		// Token: 0x04000EDC RID: 3804
		private string _attributesText;

		// Token: 0x04000EDD RID: 3805
		private int _level = -1;

		// Token: 0x04000EDE RID: 3806
		private int _maxLevel;

		// Token: 0x04000EDF RID: 3807
		private int _currentFocusLevel;

		// Token: 0x04000EE0 RID: 3808
		private int _currentSkillXP;

		// Token: 0x04000EE1 RID: 3809
		private int _xpRequiredForNextLevel;

		// Token: 0x04000EE2 RID: 3810
		private int _nextLevel;

		// Token: 0x04000EE3 RID: 3811
		private int _fullLearningRateLevel;

		// Token: 0x04000EE4 RID: 3812
		private int _numOfUnopenedPerks;

		// Token: 0x04000EE5 RID: 3813
		private bool _isInspected;

		// Token: 0x04000EE6 RID: 3814
		private bool _canAddFocus;

		// Token: 0x04000EE7 RID: 3815
		private bool _canLearnSkill;

		// Token: 0x04000EE8 RID: 3816
		private float _learningRate;

		// Token: 0x04000EE9 RID: 3817
		private double _progressPercentage;
	}
}
