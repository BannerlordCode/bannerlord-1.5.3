using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000FC RID: 252
	public class EducationVM : ViewModel
	{
		// Token: 0x06001664 RID: 5732 RVA: 0x00057CB0 File Offset: 0x00055EB0
		public EducationVM(Hero child, Action<bool> onDone, Action<EducationCampaignBehavior.EducationCharacterProperties[]> onOptionSelect, Action<List<BasicCharacterObject>, List<Equipment>> sendPossibleCharactersAndEquipment)
		{
			this._onDone = onDone;
			this._onOptionSelect = onOptionSelect;
			this._sendPossibleCharactersAndEquipment = sendPossibleCharactersAndEquipment;
			this._child = child;
			this._educationBehavior = Campaign.Current.GetCampaignBehavior<IEducationLogic>();
			int num;
			this._educationBehavior.GetStageProperties(this._child, out num);
			this._pageCount = num + 1;
			this.GainedPropertiesController = new EducationGainedPropertiesVM(this._child, this._pageCount);
			this.Options = new MBBindingList<EducationOptionVM>();
			this.Review = new EducationReviewVM(this._pageCount);
			this.CanGoBack = true;
			this.InitWithStageIndex(0);
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x00057D7C File Offset: 0x00055F7C
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject currentPageTitleTextObj = this._currentPageTitleTextObj;
			this.StageTitleText = ((currentPageTitleTextObj != null) ? currentPageTitleTextObj.ToString() : null) ?? "";
			TextObject currentPageDescriptionTextObj = this._currentPageDescriptionTextObj;
			this.PageDescriptionText = ((currentPageDescriptionTextObj != null) ? currentPageDescriptionTextObj.ToString() : null) ?? "";
			TextObject currentPageInstructionTextObj = this._currentPageInstructionTextObj;
			this.ChooseText = ((currentPageInstructionTextObj != null) ? currentPageInstructionTextObj.ToString() : null) ?? "";
			this.Options.ApplyActionOnAllItems(delegate(EducationOptionVM o)
			{
				o.RefreshValues();
			});
			foreach (EducationOptionVM educationOptionVM in this.Options)
			{
				if (educationOptionVM.IsSelected)
				{
					this.OptionEffectText = educationOptionVM.OptionEffect;
					this.OptionDescriptionText = educationOptionVM.OptionDescription;
				}
			}
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00057E78 File Offset: 0x00056078
		private void InitWithStageIndex(int index)
		{
			this._latestOptionId = null;
			this.CanAdvance = false;
			this._currentPageIndex = index;
			this.OptionEffectText = "";
			this.OptionDescriptionText = "";
			this.Options.Clear();
			if (index < this._pageCount - 1)
			{
				List<BasicCharacterObject> list = new List<BasicCharacterObject>();
				List<Equipment> list2 = new List<Equipment>();
				TextObject textObject;
				TextObject textObject2;
				TextObject textObject3;
				EducationCampaignBehavior.EducationCharacterProperties[] array;
				string[] array2;
				this._educationBehavior.GetPageProperties(this._child, this._selectedOptions.Take<string>(index).ToList<string>(), out textObject, out textObject2, out textObject3, out array, out array2);
				this._currentPageTitleTextObj = textObject;
				this._currentPageDescriptionTextObj = textObject2;
				this._currentPageInstructionTextObj = textObject3;
				for (int i = 0; i < array2.Length; i++)
				{
					TextObject textObject4;
					TextObject textObject5;
					TextObject textObject6;
					ValueTuple<CharacterAttribute, int>[] array3;
					ValueTuple<SkillObject, int>[] array4;
					ValueTuple<SkillObject, int>[] array5;
					EducationCampaignBehavior.EducationCharacterProperties[] array6;
					this._educationBehavior.GetOptionProperties(this._child, array2[i], this._selectedOptions, out textObject4, out textObject5, out textObject6, out array3, out array4, out array5, out array6);
					this.Options.Add(new EducationOptionVM(new Action<object>(this.OnOptionSelect), array2[i], textObject4, textObject5, textObject6, false, array3, array4, array5, array6));
					foreach (EducationCampaignBehavior.EducationCharacterProperties educationCharacterProperties in array6)
					{
						if (educationCharacterProperties.Character != null && !list.Contains(educationCharacterProperties.Character))
						{
							list.Add(educationCharacterProperties.Character);
						}
						if (educationCharacterProperties.Equipment != null && !list2.Contains(educationCharacterProperties.Equipment))
						{
							list2.Add(educationCharacterProperties.Equipment);
						}
					}
				}
				this.OnlyHasOneOption = this.Options.Count == 1;
				if (this._selectedOptions.Count > index)
				{
					string text = this._selectedOptions[index];
					int num = array2.IndexOf(text);
					if (num >= 0)
					{
						Action<EducationCampaignBehavior.EducationCharacterProperties[]> onOptionSelect = this._onOptionSelect;
						if (onOptionSelect != null)
						{
							onOptionSelect(this.Options[num].CharacterProperties);
						}
						if (index == this._currentPageIndex)
						{
							this.Options[num].ExecuteAction();
							this.CanAdvance = true;
						}
					}
				}
				else
				{
					EducationCampaignBehavior.EducationCharacterProperties[] array8 = new EducationCampaignBehavior.EducationCharacterProperties[(array != null) ? array.Length : 1];
					for (int k = 0; k < ((array != null) ? array.Length : 0); k++)
					{
						array8[k] = array[k];
						if (array8[k].Character != null && !list.Contains(array8[k].Character))
						{
							list.Add(array8[k].Character);
						}
						if (array8[k].Equipment != null && !list2.Contains(array8[k].Equipment))
						{
							list2.Add(array8[k].Equipment);
						}
					}
					Action<EducationCampaignBehavior.EducationCharacterProperties[]> onOptionSelect2 = this._onOptionSelect;
					if (onOptionSelect2 != null)
					{
						onOptionSelect2(array8);
					}
				}
				if (this.OnlyHasOneOption)
				{
					this.Options[0].ExecuteAction();
				}
				this._sendPossibleCharactersAndEquipment(list, list2);
			}
			else
			{
				this._currentPageTitleTextObj = new TextObject("{=Ck9HT8fQ}Summary", null);
				this._currentPageInstructionTextObj = null;
				this._currentPageDescriptionTextObj = null;
				this.OnlyHasOneOption = false;
				this.CanAdvance = true;
			}
			TextObject currentPageTitleTextObj = this._currentPageTitleTextObj;
			this.StageTitleText = ((currentPageTitleTextObj != null) ? currentPageTitleTextObj.ToString() : null) ?? "";
			TextObject currentPageInstructionTextObj = this._currentPageInstructionTextObj;
			this.ChooseText = ((currentPageInstructionTextObj != null) ? currentPageInstructionTextObj.ToString() : null) ?? "";
			TextObject currentPageDescriptionTextObj = this._currentPageDescriptionTextObj;
			this.PageDescriptionText = ((currentPageDescriptionTextObj != null) ? currentPageDescriptionTextObj.ToString() : null) ?? "";
			if (this._currentPageIndex == 0)
			{
				this.NextText = this._nextPageTextObj.ToString();
				this.PreviousText = GameTexts.FindText("str_exit", null).ToString();
			}
			else if (this._currentPageIndex == this._pageCount - 1)
			{
				this.NextText = GameTexts.FindText("str_done", null).ToString();
				this.PreviousText = this._previousPageTextObj.ToString();
			}
			else
			{
				this.NextText = this._nextPageTextObj.ToString();
				this.PreviousText = this._previousPageTextObj.ToString();
			}
			this.UpdateGainedProperties();
			this.Review.SetCurrentPage(this._currentPageIndex);
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x000582AC File Offset: 0x000564AC
		private void OnOptionSelect(object optionIdAsObj)
		{
			if (optionIdAsObj != this._latestOptionId)
			{
				string optionId = (string)optionIdAsObj;
				EducationOptionVM educationOptionVM = this.Options.FirstOrDefault<EducationOptionVM>((EducationOptionVM o) => (string)o.Identifier == optionId);
				this.Options.ApplyActionOnAllItems(delegate(EducationOptionVM o)
				{
					o.IsSelected = false;
				});
				educationOptionVM.IsSelected = true;
				string actionText = educationOptionVM.ActionText;
				if (this._currentPageIndex == this._selectedOptions.Count)
				{
					this._selectedOptions.Add(optionId);
				}
				else if (this._currentPageIndex < this._selectedOptions.Count)
				{
					this._selectedOptions[this._currentPageIndex] = optionId;
				}
				else
				{
					Debug.FailedAssert("Skipped a stage for education!!!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Education\\EducationVM.cs", "OnOptionSelect", 210);
				}
				this.OptionEffectText = educationOptionVM.OptionEffect;
				this.OptionDescriptionText = educationOptionVM.OptionDescription;
				Action<EducationCampaignBehavior.EducationCharacterProperties[]> onOptionSelect = this._onOptionSelect;
				if (onOptionSelect != null)
				{
					onOptionSelect(educationOptionVM.CharacterProperties);
				}
				this.UpdateGainedProperties();
				this.CanAdvance = true;
				this._latestOptionId = optionIdAsObj;
				this.Review.SetGainForStage(this._currentPageIndex, this.OptionEffectText);
			}
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x000583EC File Offset: 0x000565EC
		private void UpdateGainedProperties()
		{
			this.GainedPropertiesController.UpdateWithSelections(this._selectedOptions, this._currentPageIndex);
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x00058408 File Offset: 0x00056608
		public void ExecuteNextStage()
		{
			if (this._currentPageIndex + 1 < this._pageCount)
			{
				this.InitWithStageIndex(this._currentPageIndex + 1);
				return;
			}
			this._educationBehavior.Finalize(this._child, this._selectedOptions);
			Action<bool> onDone = this._onDone;
			if (onDone == null)
			{
				return;
			}
			onDone(false);
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x0005845C File Offset: 0x0005665C
		public void ExecutePreviousStage()
		{
			if (this._currentPageIndex > 0)
			{
				this.InitWithStageIndex(this._currentPageIndex - 1);
				return;
			}
			if (this._currentPageIndex == 0)
			{
				Action<bool> onDone = this._onDone;
				if (onDone == null)
				{
					return;
				}
				onDone(true);
			}
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x0005848F File Offset: 0x0005668F
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.OnFinalize();
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x000584B8 File Offset: 0x000566B8
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x000584C7 File Offset: 0x000566C7
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x0600166E RID: 5742 RVA: 0x000584D6 File Offset: 0x000566D6
		// (set) Token: 0x0600166F RID: 5743 RVA: 0x000584DE File Offset: 0x000566DE
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x000584FC File Offset: 0x000566FC
		// (set) Token: 0x06001671 RID: 5745 RVA: 0x00058504 File Offset: 0x00056704
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06001672 RID: 5746 RVA: 0x00058522 File Offset: 0x00056722
		// (set) Token: 0x06001673 RID: 5747 RVA: 0x0005852A File Offset: 0x0005672A
		[DataSourceProperty]
		public string StageTitleText
		{
			get
			{
				return this._stageTitleText;
			}
			set
			{
				if (value != this._stageTitleText)
				{
					this._stageTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "StageTitleText");
				}
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06001674 RID: 5748 RVA: 0x0005854D File Offset: 0x0005674D
		// (set) Token: 0x06001675 RID: 5749 RVA: 0x00058555 File Offset: 0x00056755
		[DataSourceProperty]
		public string ChooseText
		{
			get
			{
				return this._chooseText;
			}
			set
			{
				if (value != this._chooseText)
				{
					this._chooseText = value;
					base.OnPropertyChangedWithValue<string>(value, "ChooseText");
				}
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06001676 RID: 5750 RVA: 0x00058578 File Offset: 0x00056778
		// (set) Token: 0x06001677 RID: 5751 RVA: 0x00058580 File Offset: 0x00056780
		[DataSourceProperty]
		public string PageDescriptionText
		{
			get
			{
				return this._pageDescriptionText;
			}
			set
			{
				if (value != this._pageDescriptionText)
				{
					this._pageDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "PageDescriptionText");
				}
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x000585A3 File Offset: 0x000567A3
		// (set) Token: 0x06001679 RID: 5753 RVA: 0x000585AB File Offset: 0x000567AB
		[DataSourceProperty]
		public string OptionEffectText
		{
			get
			{
				return this._optionEffectText;
			}
			set
			{
				if (value != this._optionEffectText)
				{
					this._optionEffectText = value;
					base.OnPropertyChangedWithValue<string>(value, "OptionEffectText");
				}
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x0600167A RID: 5754 RVA: 0x000585CE File Offset: 0x000567CE
		// (set) Token: 0x0600167B RID: 5755 RVA: 0x000585D6 File Offset: 0x000567D6
		[DataSourceProperty]
		public string OptionDescriptionText
		{
			get
			{
				return this._optionDescriptionText;
			}
			set
			{
				if (value != this._optionDescriptionText)
				{
					this._optionDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "OptionDescriptionText");
				}
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x0600167C RID: 5756 RVA: 0x000585F9 File Offset: 0x000567F9
		// (set) Token: 0x0600167D RID: 5757 RVA: 0x00058601 File Offset: 0x00056801
		[DataSourceProperty]
		public string NextText
		{
			get
			{
				return this._nextText;
			}
			set
			{
				if (value != this._nextText)
				{
					this._nextText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextText");
				}
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x0600167E RID: 5758 RVA: 0x00058624 File Offset: 0x00056824
		// (set) Token: 0x0600167F RID: 5759 RVA: 0x0005862C File Offset: 0x0005682C
		[DataSourceProperty]
		public string PreviousText
		{
			get
			{
				return this._previousText;
			}
			set
			{
				if (value != this._previousText)
				{
					this._previousText = value;
					base.OnPropertyChangedWithValue<string>(value, "PreviousText");
				}
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06001680 RID: 5760 RVA: 0x0005864F File Offset: 0x0005684F
		// (set) Token: 0x06001681 RID: 5761 RVA: 0x00058657 File Offset: 0x00056857
		[DataSourceProperty]
		public bool CanAdvance
		{
			get
			{
				return this._canAdvance;
			}
			set
			{
				if (value != this._canAdvance)
				{
					this._canAdvance = value;
					base.OnPropertyChangedWithValue(value, "CanAdvance");
				}
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06001682 RID: 5762 RVA: 0x00058675 File Offset: 0x00056875
		// (set) Token: 0x06001683 RID: 5763 RVA: 0x0005867D File Offset: 0x0005687D
		[DataSourceProperty]
		public bool CanGoBack
		{
			get
			{
				return this._canGoBack;
			}
			set
			{
				if (value != this._canGoBack)
				{
					this._canGoBack = value;
					base.OnPropertyChangedWithValue(value, "CanGoBack");
				}
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06001684 RID: 5764 RVA: 0x0005869B File Offset: 0x0005689B
		// (set) Token: 0x06001685 RID: 5765 RVA: 0x000586A3 File Offset: 0x000568A3
		[DataSourceProperty]
		public bool OnlyHasOneOption
		{
			get
			{
				return this._onlyHasOneOption;
			}
			set
			{
				if (value != this._onlyHasOneOption)
				{
					this._onlyHasOneOption = value;
					base.OnPropertyChangedWithValue(value, "OnlyHasOneOption");
				}
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06001686 RID: 5766 RVA: 0x000586C1 File Offset: 0x000568C1
		// (set) Token: 0x06001687 RID: 5767 RVA: 0x000586C9 File Offset: 0x000568C9
		[DataSourceProperty]
		public MBBindingList<EducationOptionVM> Options
		{
			get
			{
				return this._options;
			}
			set
			{
				if (value != this._options)
				{
					this._options = value;
					base.OnPropertyChangedWithValue<MBBindingList<EducationOptionVM>>(value, "Options");
				}
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06001688 RID: 5768 RVA: 0x000586E7 File Offset: 0x000568E7
		// (set) Token: 0x06001689 RID: 5769 RVA: 0x000586EF File Offset: 0x000568EF
		[DataSourceProperty]
		public EducationGainedPropertiesVM GainedPropertiesController
		{
			get
			{
				return this._gainedPropertiesController;
			}
			set
			{
				if (value != this._gainedPropertiesController)
				{
					this._gainedPropertiesController = value;
					base.OnPropertyChangedWithValue<EducationGainedPropertiesVM>(value, "GainedPropertiesController");
				}
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x0600168A RID: 5770 RVA: 0x0005870D File Offset: 0x0005690D
		// (set) Token: 0x0600168B RID: 5771 RVA: 0x00058715 File Offset: 0x00056915
		[DataSourceProperty]
		public EducationReviewVM Review
		{
			get
			{
				return this._review;
			}
			set
			{
				if (value != this._review)
				{
					this._review = value;
					base.OnPropertyChangedWithValue<EducationReviewVM>(value, "Review");
				}
			}
		}

		// Token: 0x04000A22 RID: 2594
		private readonly Action<bool> _onDone;

		// Token: 0x04000A23 RID: 2595
		private readonly Action<EducationCampaignBehavior.EducationCharacterProperties[]> _onOptionSelect;

		// Token: 0x04000A24 RID: 2596
		private readonly Action<List<BasicCharacterObject>, List<Equipment>> _sendPossibleCharactersAndEquipment;

		// Token: 0x04000A25 RID: 2597
		private readonly IEducationLogic _educationBehavior;

		// Token: 0x04000A26 RID: 2598
		private readonly Hero _child;

		// Token: 0x04000A27 RID: 2599
		private readonly TextObject _nextPageTextObj = new TextObject("{=Rvr1bcu8}Next", null);

		// Token: 0x04000A28 RID: 2600
		private readonly TextObject _previousPageTextObj = new TextObject("{=WXAaWZVf}Previous", null);

		// Token: 0x04000A29 RID: 2601
		private readonly int _pageCount;

		// Token: 0x04000A2A RID: 2602
		private readonly List<string> _selectedOptions = new List<string>();

		// Token: 0x04000A2B RID: 2603
		private TextObject _currentPageTitleTextObj;

		// Token: 0x04000A2C RID: 2604
		private TextObject _currentPageDescriptionTextObj;

		// Token: 0x04000A2D RID: 2605
		private TextObject _currentPageInstructionTextObj;

		// Token: 0x04000A2E RID: 2606
		private object _latestOptionId;

		// Token: 0x04000A2F RID: 2607
		private int _currentPageIndex;

		// Token: 0x04000A30 RID: 2608
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000A31 RID: 2609
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000A32 RID: 2610
		private string _stageTitleText;

		// Token: 0x04000A33 RID: 2611
		private string _chooseText;

		// Token: 0x04000A34 RID: 2612
		private string _pageDescriptionText;

		// Token: 0x04000A35 RID: 2613
		private string _optionEffectText;

		// Token: 0x04000A36 RID: 2614
		private string _optionDescriptionText;

		// Token: 0x04000A37 RID: 2615
		private string _nextText;

		// Token: 0x04000A38 RID: 2616
		private string _previousText;

		// Token: 0x04000A39 RID: 2617
		private bool _canAdvance;

		// Token: 0x04000A3A RID: 2618
		private bool _canGoBack;

		// Token: 0x04000A3B RID: 2619
		private bool _onlyHasOneOption;

		// Token: 0x04000A3C RID: 2620
		private MBBindingList<EducationOptionVM> _options;

		// Token: 0x04000A3D RID: 2621
		private EducationGainedPropertiesVM _gainedPropertiesController;

		// Token: 0x04000A3E RID: 2622
		private EducationReviewVM _review;
	}
}
