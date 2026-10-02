using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x02000027 RID: 39
	public class PartyCharacterVM : ViewModel
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000295 RID: 661 RVA: 0x000148E5 File Offset: 0x00012AE5
		// (set) Token: 0x06000296 RID: 662 RVA: 0x000148ED File Offset: 0x00012AED
		public TroopRoster Troops { get; private set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000297 RID: 663 RVA: 0x000148F6 File Offset: 0x00012AF6
		// (set) Token: 0x06000298 RID: 664 RVA: 0x000148FE File Offset: 0x00012AFE
		public string StringId { get; private set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000299 RID: 665 RVA: 0x00014907 File Offset: 0x00012B07
		// (set) Token: 0x0600029A RID: 666 RVA: 0x00014910 File Offset: 0x00012B10
		public TroopRosterElement Troop
		{
			get
			{
				return this._troop;
			}
			set
			{
				this._troop = value;
				this.Character = value.Character;
				this.TroopID = this.Character.StringId;
				this.CheckTransferAmountDefaultValue();
				this.TroopXPTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetTroopXPTooltip(value));
				this.TroopConformityTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetTroopConformityTooltip(value));
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0001498C File Offset: 0x00012B8C
		// (set) Token: 0x0600029C RID: 668 RVA: 0x00014994 File Offset: 0x00012B94
		public CharacterObject Character
		{
			get
			{
				return this._character;
			}
			set
			{
				if (this._character != value)
				{
					this._character = value;
					CharacterCode characterCode = this.GetCharacterCode(value, this.Type, this.Side);
					this.Code = new CharacterImageIdentifierVM(characterCode);
					CharacterObject[] upgradeTargets = this._character.UpgradeTargets;
					if (upgradeTargets != null && upgradeTargets.Length != 0)
					{
						this.Upgrades = new MBBindingList<UpgradeTargetVM>();
						for (int i = 0; i < this._character.UpgradeTargets.Length; i++)
						{
							CharacterCode characterCode2 = this.GetCharacterCode(this._character.UpgradeTargets[i], this.Type, this.Side);
							this.Upgrades.Add(new UpgradeTargetVM(i, value, characterCode2, new Action<int, int>(this.Upgrade), new Action<UpgradeTargetVM>(this.FocusUpgrade)));
						}
					}
				}
				this.CheckTransferAmountDefaultValue();
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00014A60 File Offset: 0x00012C60
		public PartyCharacterVM(PartyScreenLogic partyScreenLogic, PartyVM partyVm, TroopRoster troops, int index, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, bool isTroopTransferrable)
		{
			this.Upgrades = new MBBindingList<UpgradeTargetVM>();
			this._partyScreenLogic = partyScreenLogic;
			this._partyVm = partyVm;
			this.Troops = troops;
			this.Side = side;
			this.Type = type;
			this.Troop = troops.GetElementCopyAtIndex(index);
			this.Index = index;
			this.IsHero = this.Troop.Character.IsHero;
			this.IsMainHero = Hero.MainHero.CharacterObject == this.Troop.Character;
			this.IsPrisoner = this.Type == PartyScreenLogic.TroopType.Prisoner;
			this.TierIconData = CampaignUIHelper.GetCharacterTierData(this.Troop.Character, true);
			this.TypeIconData = CampaignUIHelper.GetCharacterTypeData(this.Troop.Character, false);
			this.StringId = CampaignUIHelper.GetTroopLockStringID(this.Troop);
			this._initIsTroopTransferable = isTroopTransferrable;
			this.IsTroopTransferrable = this._initIsTroopTransferable;
			this.TradeData = new PartyTradeVM(partyScreenLogic, this.Troop, this.Side, this.IsTroopTransferrable, this.IsPrisoner, new Action<int, bool>(this.OnTradeApplyTransaction));
			this.IsPrisonerOfPlayer = this.IsPrisoner && this.Side == PartyScreenLogic.PartyRosterSide.Right;
			this.IsHeroPrisonerOfPlayer = this.IsPrisonerOfPlayer && this.Character.IsHero;
			this.IsExecutable = this._partyScreenLogic.IsExecutable(this.Type, this.Character, this.Side);
			this.IsUpgradableTroop = this.Side == PartyScreenLogic.PartyRosterSide.Right && !this.IsHero && !this.IsPrisoner && this.Character.UpgradeTargets.Length != 0;
			this.InitializeUpgrades();
			this.ThrowOnPropertyChanged();
			this.CheckTransferAmountDefaultValue();
			this.UpdateRecruitable();
			this.RefreshValues();
			this.SetMoraleCost();
			this.UpdateTalkable();
			this.TransferHint = new BasicTooltipViewModel(() => this.GetTransferHint());
			this.RecruitPrisonerHint = new BasicTooltipViewModel(() => this.GetRecruitHint());
			this.ExecutePrisonerHint = new BasicTooltipViewModel(() => this._partyScreenLogic.GetExecutableReasonString(this.Troop.Character, this.IsExecutable));
			this.HeroHealthHint = (this.Troop.Character.IsHero ? new BasicTooltipViewModel(() => CampaignUIHelper.GetHeroHealthTooltip(this.Troop.Character.HeroObject)) : null);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00014CB4 File Offset: 0x00012EB4
		public void UpdateTalkable()
		{
			bool flag = this.Side == PartyScreenLogic.PartyRosterSide.Right;
			bool flag2 = this.Troop.Character != CharacterObject.PlayerCharacter;
			bool isHero = this.Troop.Character.IsHero;
			this.IsTalkableCharacter = flag2 && flag && isHero;
			if (this.TalkHint == null)
			{
				this.TalkHint = new HintViewModel();
			}
			if (this.IsTalkableCharacter)
			{
				this._partyCharacterTalkPermission = null;
				Game.Current.EventManager.TriggerEvent<PartyScreenCharacterTalkPermissionEvent>(new PartyScreenCharacterTalkPermissionEvent(this.Character.HeroObject, new Action<bool, TextObject>(this.OnPartyCharacterTalkPermissionResult)));
				if (this._partyCharacterTalkPermission != null && !this._partyCharacterTalkPermission.Item1)
				{
					this.CanTalk = false;
					this.TalkHint.HintText = this._partyCharacterTalkPermission.Item2;
					if (this.TalkHint.HintText.IsEmpty())
					{
						this.TalkHint.HintText = new TextObject("{=epQYhd1A}Cannot talk to hero right now", null);
						return;
					}
				}
				else
				{
					CanTalkToHeroDelegate canTalkToHeroDelegate = this._partyVm.PartyScreenLogic.CanTalkToHeroDelegate;
					this.CanTalk = (canTalkToHeroDelegate == null || canTalkToHeroDelegate(this.Character.HeroObject, this.Type, this.Side, this._partyScreenLogic.LeftOwnerParty, out this.TalkHint.HintText)) && CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out this.TalkHint.HintText);
					if (this.CanTalk)
					{
						this.TalkHint.HintText = GameTexts.FindText("str_talk_button", null);
						return;
					}
					if (this.TalkHint.HintText.IsEmpty())
					{
						this.TalkHint.HintText = new TextObject("{=epQYhd1A}Cannot talk to hero right now", null);
						return;
					}
				}
			}
			else
			{
				this.TalkHint.HintText = TextObject.GetEmpty();
				this.CanTalk = false;
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00014E70 File Offset: 0x00013070
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.Troop.Character.Name.ToString();
			this.LockHint = new HintViewModel(GameTexts.FindText("str_lock_in_party", null).SetTextVariable("TRANSFERABLE", this.IsPrisoner ? GameTexts.FindText("str_prisoners", null).ToString() : GameTexts.FindText("str_troops", null).ToString()), null);
			MBBindingList<UpgradeTargetVM> upgrades = this.Upgrades;
			if (upgrades != null)
			{
				upgrades.ApplyActionOnAllItems(delegate(UpgradeTargetVM x)
				{
					x.RefreshValues();
				});
			}
			PartyTradeVM tradeData = this.TradeData;
			if (tradeData == null)
			{
				return;
			}
			tradeData.RefreshValues();
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00014F29 File Offset: 0x00013129
		private void OnPartyCharacterTalkPermissionResult(bool isAvailable, TextObject reasonStr)
		{
			this._partyCharacterTalkPermission = new Tuple<bool, TextObject>(isAvailable, reasonStr);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00014F38 File Offset: 0x00013138
		private string GetTransferHint()
		{
			string text = GameTexts.FindText("str_transfer", null).ToString();
			string stackModifierString = CampaignUIHelper.GetStackModifierString(GameTexts.FindText("str_entire_stack_shortcut_transfer", null), GameTexts.FindText("str_five_stack_shortcut_transfer", null), this.Troop.Number >= 5);
			if (string.IsNullOrEmpty(stackModifierString))
			{
				return text;
			}
			return GameTexts.FindText("str_string_newline_string", null).SetTextVariable("STR1", text).SetTextVariable("STR2", stackModifierString)
				.ToString();
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00014FB8 File Offset: 0x000131B8
		private string GetRecruitHint()
		{
			bool flag;
			string recruitableReasonString = this._partyScreenLogic.GetRecruitableReasonString(this.Troop.Character, this.IsTroopRecruitable, this.Troop.Number, out flag);
			string stackModifierString = CampaignUIHelper.GetStackModifierString(GameTexts.FindText("str_entire_stack_shortcut_recruit_units", null), GameTexts.FindText("str_five_stack_shortcut_recruit_units", null), this.Troop.Number >= 5);
			if (string.IsNullOrEmpty(stackModifierString) || !flag)
			{
				return recruitableReasonString;
			}
			return GameTexts.FindText("str_string_newline_string", null).SetTextVariable("STR1", recruitableReasonString).SetTextVariable("STR2", stackModifierString)
				.ToString();
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00015058 File Offset: 0x00013258
		private void CheckTransferAmountDefaultValue()
		{
			if (this.TransferAmount == 0 && this.Troop.Character != null && this.Troop.Number > 0)
			{
				this.TransferAmount = 1;
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00015092 File Offset: 0x00013292
		public void ExecuteSetSelected()
		{
			if (this.Character != null)
			{
				PartyCharacterVM.SetSelected(this);
			}
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x000150A7 File Offset: 0x000132A7
		public void ExecuteTalk()
		{
			PartyVM partyVm = this._partyVm;
			if (partyVm == null)
			{
				return;
			}
			partyVm.ExecuteTalk();
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000150B9 File Offset: 0x000132B9
		public void UpdateTradeData()
		{
			PartyTradeVM tradeData = this.TradeData;
			if (tradeData == null)
			{
				return;
			}
			tradeData.UpdateTroopData(this.Troop, this.Side, true);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x000150D8 File Offset: 0x000132D8
		public void UpdateRecruitable()
		{
			this.MaxConformity = this.Troop.Character.ConformityNeededToRecruitPrisoner;
			int elementXp = PartyBase.MainParty.PrisonRoster.GetElementXp(this.Troop.Character);
			this.CurrentConformity = ((elementXp >= this.Troop.Number * this.MaxConformity) ? this.MaxConformity : (elementXp % this.MaxConformity));
			this.IsRecruitablePrisoner = !this._character.IsHero && this.Type == PartyScreenLogic.TroopType.Prisoner;
			this.IsTroopRecruitable = this._partyScreenLogic.IsPrisonerRecruitable(this.Type, this.Character, this.Side) && !this._partyScreenLogic.IsTroopUpgradesDisabled;
			this.NumOfRecruitablePrisoners = this._partyScreenLogic.GetTroopRecruitableAmount(this.Character);
			GameTexts.SetVariable("LEFT", this.NumOfRecruitablePrisoners);
			GameTexts.SetVariable("RIGHT", this.Troop.Number);
			this.StrNumOfRecruitableTroop = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000151F0 File Offset: 0x000133F0
		private void OnTradeApplyTransaction(int amount, bool isIncreasing)
		{
			this.TransferAmount = amount;
			PartyScreenLogic.PartyRosterSide partyRosterSide = (isIncreasing ? PartyScreenLogic.PartyRosterSide.Left : PartyScreenLogic.PartyRosterSide.Right);
			this.ApplyTransfer(this.TransferAmount, partyRosterSide);
			this.IsExecutable = this._partyScreenLogic.IsExecutable(this.Type, this.Character, this.Side) && this.Troop.Number > 0;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00015254 File Offset: 0x00013454
		public void InitializeUpgrades()
		{
			if (this.IsUpgradableTroop)
			{
				for (int i = 0; i < this.Character.UpgradeTargets.Length; i++)
				{
					CharacterObject characterObject = this.Character.UpgradeTargets[i];
					int level = characterObject.Level;
					int upgradeGoldCost = this.Character.GetUpgradeGoldCost(PartyBase.MainParty, i);
					if (!this.Character.Culture.IsBandit)
					{
						int level2 = this.Character.Level;
					}
					else
					{
						int level3 = this.Character.Level;
					}
					PerkObject perkObject;
					bool flag = Campaign.Current.Models.PartyTroopUpgradeModel.DoesPartyHaveRequiredPerksForUpgrade(PartyBase.MainParty, this.Character, characterObject, out perkObject);
					int num = (flag ? this.Troop.Number : 0);
					bool flag2 = true;
					int numOfCategoryItemPartyHas = this.GetNumOfCategoryItemPartyHas(this._partyScreenLogic.RightOwnerParty.ItemRoster, characterObject.UpgradeRequiresItemFromCategory);
					if (characterObject.UpgradeRequiresItemFromCategory != null)
					{
						flag2 = numOfCategoryItemPartyHas > 0;
					}
					bool flag3 = Hero.MainHero.Gold + this._partyScreenLogic.CurrentData.PartyGoldChangeAmount >= upgradeGoldCost;
					bool flag4 = level >= this.Character.Level && this.Troop.Xp >= this.Character.GetUpgradeXpCost(PartyBase.MainParty, i);
					bool flag5 = !flag2 || !flag3;
					int num2 = this.Troop.Number;
					if (upgradeGoldCost > 0)
					{
						num2 = (int)MathF.Clamp((float)MathF.Floor((float)(Hero.MainHero.Gold + this._partyScreenLogic.CurrentData.PartyGoldChangeAmount) / (float)upgradeGoldCost), 0f, (float)this.Troop.Number);
					}
					int num3 = ((characterObject.UpgradeRequiresItemFromCategory != null) ? numOfCategoryItemPartyHas : this.Troop.Number);
					int num4 = (flag4 ? ((int)MathF.Clamp((float)MathF.Floor((float)this.Troop.Xp / (float)this.Character.GetUpgradeXpCost(PartyBase.MainParty, i)), 0f, (float)this.Troop.Number)) : 0);
					int num5 = MathF.Min(MathF.Min(num2, num3), MathF.Min(num4, num));
					if (this.Character.Culture.IsBandit)
					{
						flag5 = flag5 || !Campaign.Current.Models.PartyTroopUpgradeModel.CanPartyUpgradeTroopToTarget(PartyBase.MainParty, this.Character, characterObject);
						num5 = ((!flag4) ? 0 : num5);
					}
					flag4 = flag4 && !this._partyVm.PartyScreenLogic.IsTroopUpgradesDisabled;
					string upgradeHint = CampaignUIHelper.GetUpgradeHint(i, numOfCategoryItemPartyHas, num5, upgradeGoldCost, flag, perkObject, this.Character, this.Troop, this._partyScreenLogic.CurrentData.PartyGoldChangeAmount, this._partyVm.PartyScreenLogic.IsTroopUpgradesDisabled);
					this.Upgrades[i].Refresh(num5, flag4, flag5, flag2, flag, upgradeHint, !this.Character.IsHero && this.Character.UpgradeTargets[i].IsMariner);
					if (i == 0)
					{
						this.UpgradeCostText = upgradeGoldCost.ToString();
						this.HasEnoughGold = flag3;
						this.NumOfReadyToUpgradeTroops = num4;
						this.MaxXP = this.Character.GetUpgradeXpCost(PartyBase.MainParty, i);
						this.CurrentXP = ((this.Troop.Xp >= this.Troop.Number * this.MaxXP) ? this.MaxXP : (this.Troop.Xp % this.MaxXP));
					}
				}
				this.AnyUpgradeHasRequirement = this.Upgrades.Any<UpgradeTargetVM>((UpgradeTargetVM x) => x.Requirements.HasItemRequirement || x.Requirements.HasPerkRequirement);
			}
			int num6 = 0;
			foreach (UpgradeTargetVM upgradeTargetVM in this.Upgrades)
			{
				if (upgradeTargetVM.AvailableUpgrades > num6)
				{
					num6 = upgradeTargetVM.AvailableUpgrades;
				}
			}
			this.NumOfUpgradeableTroops = num6;
			this.IsTroopUpgradable = this.NumOfUpgradeableTroops > 0 && !this._partyVm.PartyScreenLogic.IsTroopUpgradesDisabled;
			GameTexts.SetVariable("LEFT", this.NumOfReadyToUpgradeTroops);
			GameTexts.SetVariable("RIGHT", this.Troop.Number);
			this.StrNumOfUpgradableTroop = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
			base.OnPropertyChanged("AmountOfUpgrades");
		}

		// Token: 0x060002AA RID: 682 RVA: 0x000156F0 File Offset: 0x000138F0
		public void OnTransferred()
		{
			if (this.Side != PartyScreenLogic.PartyRosterSide.Left || this.IsPrisoner)
			{
				this.InitializeUpgrades();
				return;
			}
			PartyCharacterVM partyCharacterVM = this._partyVm.MainPartyTroops.FirstOrDefault<PartyCharacterVM>((PartyCharacterVM x) => x.Character == this.Character);
			if (partyCharacterVM == null)
			{
				return;
			}
			partyCharacterVM.InitializeUpgrades();
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00015730 File Offset: 0x00013930
		public void ThrowOnPropertyChanged()
		{
			base.OnPropertyChanged("Name");
			base.OnPropertyChanged("Number");
			base.OnPropertyChanged("WoundedCount");
			base.OnPropertyChanged("IsTroopTransferrable");
			base.OnPropertyChanged("MaxCount");
			base.OnPropertyChanged("AmountOfUpgrades");
			base.OnPropertyChanged("Level");
			base.OnPropertyChanged("PartyIndex");
			base.OnPropertyChanged("Index");
			base.OnPropertyChanged("TroopNum");
			base.OnPropertyChanged("TransferString");
		}

		// Token: 0x060002AC RID: 684 RVA: 0x000157B8 File Offset: 0x000139B8
		public override bool Equals(object obj)
		{
			PartyCharacterVM partyCharacterVM;
			return obj != null && (partyCharacterVM = obj as PartyCharacterVM) != null && ((partyCharacterVM.Character == null && this.Code == null) || partyCharacterVM.Character == this.Character);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000157F6 File Offset: 0x000139F6
		private void ApplyTransfer(int transferAmount, PartyScreenLogic.PartyRosterSide side)
		{
			PartyCharacterVM.OnTransfer(this, -1, transferAmount, side);
			this.ThrowOnPropertyChanged();
			this.UpdateTalkable();
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00015812 File Offset: 0x00013A12
		private void ExecuteTransfer()
		{
			this.ApplyTransfer(this.TransferAmount, this.Side);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00015828 File Offset: 0x00013A28
		private void ExecuteTransferAll()
		{
			this.ApplyTransfer(this.Troop.Number, this.Side);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0001584F File Offset: 0x00013A4F
		public void ExecuteSetFocused()
		{
			Action<PartyCharacterVM> onFocus = PartyCharacterVM.OnFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(this);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00015861 File Offset: 0x00013A61
		public void ExecuteSetUnfocused()
		{
			Action<PartyCharacterVM> onFocus = PartyCharacterVM.OnFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(null);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00015874 File Offset: 0x00013A74
		public void ExecuteTransferSingle()
		{
			int num = 1;
			if (this._partyVm.IsEntireStackModifierActive)
			{
				num = this.Troop.Number;
			}
			else if (this._partyVm.IsFiveStackModifierActive)
			{
				num = MathF.Min(5, this.Troop.Number);
			}
			this.ApplyTransfer(num, this.Side);
			this._partyVm.ExecuteRemoveZeroCounts();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x000158DB File Offset: 0x00013ADB
		public void ExecuteResetTrade()
		{
			this.TradeData.ExecuteReset();
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x000158E8 File Offset: 0x00013AE8
		public void Upgrade(int upgradeIndex, int maxUpgradeCount)
		{
			PartyVM partyVm = this._partyVm;
			if (partyVm == null)
			{
				return;
			}
			partyVm.ExecuteUpgrade(this, upgradeIndex, maxUpgradeCount);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x000158FD File Offset: 0x00013AFD
		public void FocusUpgrade(UpgradeTargetVM upgrade)
		{
			this._partyVm.CurrentFocusedUpgrade = upgrade;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0001590B File Offset: 0x00013B0B
		public void RecruitAll()
		{
			if (this.IsTroopRecruitable)
			{
				this._partyVm.ExecuteRecruit(this, true);
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00015922 File Offset: 0x00013B22
		public void ExecuteRecruitTroop()
		{
			if (this.IsTroopRecruitable)
			{
				this._partyVm.ExecuteRecruit(this, false);
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0001593C File Offset: 0x00013B3C
		public void ExecuteExecuteTroop()
		{
			if (this.IsExecutable)
			{
				if (FaceGen.GetMaturityTypeWithAge(this.Character.HeroObject.BodyProperties.Age) <= BodyMeshMaturityType.Tween)
				{
					return;
				}
				MBInformationManager.ShowSceneNotification(HeroExecutionSceneNotificationData.CreateForPlayerExecutingHero(this.Character.HeroObject, delegate
				{
					this._partyVm.ExecuteExecution();
				}, SceneNotificationData.RelevantContextType.Any, true, null));
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00015998 File Offset: 0x00013B98
		public void ExecuteOpenTroopEncyclopedia()
		{
			if (!this.Troop.Character.IsHero)
			{
				if (Campaign.Current.EncyclopediaManager.GetPageOf(typeof(CharacterObject)).IsValidEncyclopediaItem(this.Troop.Character))
				{
					Campaign.Current.EncyclopediaManager.GoToLink(this.Troop.Character.EncyclopediaLink);
					return;
				}
			}
			else if (Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero)).IsValidEncyclopediaItem(this.Troop.Character.HeroObject))
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Troop.Character.HeroObject.EncyclopediaLink);
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00015A58 File Offset: 0x00013C58
		private CharacterCode GetCharacterCode(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side)
		{
			IFaction faction = null;
			if (type != PartyScreenLogic.TroopType.Prisoner)
			{
				if (side == PartyScreenLogic.PartyRosterSide.Left && this._partyScreenLogic.LeftOwnerParty != null)
				{
					faction = this._partyScreenLogic.LeftOwnerParty.MapFaction;
				}
				else if (this.Side == PartyScreenLogic.PartyRosterSide.Right && this._partyScreenLogic.RightOwnerParty != null)
				{
					faction = this._partyScreenLogic.RightOwnerParty.MapFaction;
				}
			}
			uint num = Color.White.ToUnsignedInteger();
			uint num2 = Color.White.ToUnsignedInteger();
			if (faction != null)
			{
				num = faction.Color;
				num2 = faction.Color2;
			}
			else if (character.Culture != null)
			{
				num = character.Culture.Color;
				num2 = character.Culture.Color2;
			}
			Equipment equipment = character.Equipment;
			string text = ((equipment != null) ? equipment.CalculateEquipmentCode() : null);
			BodyProperties bodyProperties = character.GetBodyProperties(character.Equipment, -1);
			return CharacterCode.CreateFrom(text, bodyProperties, character.IsFemale, character.IsHero, num, num2, character.DefaultFormationClass, character.Race);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00015B48 File Offset: 0x00013D48
		private void SetMoraleCost()
		{
			if (this.IsTroopRecruitable)
			{
				this.RecruitMoraleCostText = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetPrisonerRecruitmentMoraleEffect(this._partyScreenLogic.RightOwnerParty, this.Character, 1).ToString();
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00015B94 File Offset: 0x00013D94
		public void SetIsUpgradeButtonHighlighted(bool isHighlighted)
		{
			MBBindingList<UpgradeTargetVM> upgrades = this.Upgrades;
			if (upgrades == null)
			{
				return;
			}
			upgrades.ApplyActionOnAllItems(delegate(UpgradeTargetVM x)
			{
				x.IsHighlighted = isHighlighted;
			});
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00015BCC File Offset: 0x00013DCC
		public int GetNumOfCategoryItemPartyHas(ItemRoster items, ItemCategory itemCategory)
		{
			int num = 0;
			foreach (ItemRosterElement itemRosterElement in items)
			{
				if (itemRosterElement.EquipmentElement.Item.ItemCategory == itemCategory)
				{
					num += itemRosterElement.Amount;
				}
			}
			return num;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00015C34 File Offset: 0x00013E34
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002BF RID: 703 RVA: 0x00015C3C File Offset: 0x00013E3C
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x00015C44 File Offset: 0x00013E44
		[DataSourceProperty]
		public bool IsFormationEnabled
		{
			get
			{
				return this._isFormationEnabled;
			}
			set
			{
				if (this._isFormationEnabled != value)
				{
					this._isFormationEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsFormationEnabled");
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x00015C64 File Offset: 0x00013E64
		[DataSourceProperty]
		public string TransferString
		{
			get
			{
				return this.TransferAmount.ToString() + "/" + this.Number.ToString();
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x00015C97 File Offset: 0x00013E97
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x00015C9F File Offset: 0x00013E9F
		[DataSourceProperty]
		public bool IsTroopUpgradable
		{
			get
			{
				return this._isTroopUpgradable;
			}
			set
			{
				if (value != this._isTroopUpgradable)
				{
					this._isTroopUpgradable = value;
					base.OnPropertyChangedWithValue(value, "IsTroopUpgradable");
				}
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x00015CBD File Offset: 0x00013EBD
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x00015CC5 File Offset: 0x00013EC5
		[DataSourceProperty]
		public bool IsTroopRecruitable
		{
			get
			{
				return this._isTroopRecruitable;
			}
			set
			{
				if (value != this._isTroopRecruitable)
				{
					this._isTroopRecruitable = value;
					base.OnPropertyChangedWithValue(value, "IsTroopRecruitable");
				}
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x00015CE3 File Offset: 0x00013EE3
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x00015CEB File Offset: 0x00013EEB
		[DataSourceProperty]
		public bool IsRecruitablePrisoner
		{
			get
			{
				return this._isRecruitablePrisoner;
			}
			set
			{
				if (value != this._isRecruitablePrisoner)
				{
					this._isRecruitablePrisoner = value;
					base.OnPropertyChangedWithValue(value, "IsRecruitablePrisoner");
				}
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x00015D09 File Offset: 0x00013F09
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x00015D11 File Offset: 0x00013F11
		[DataSourceProperty]
		public bool IsUpgradableTroop
		{
			get
			{
				return this._isUpgradableTroop;
			}
			set
			{
				if (value != this._isUpgradableTroop)
				{
					this._isUpgradableTroop = value;
					base.OnPropertyChangedWithValue(value, "IsUpgradableTroop");
				}
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00015D2F File Offset: 0x00013F2F
		// (set) Token: 0x060002CB RID: 715 RVA: 0x00015D37 File Offset: 0x00013F37
		[DataSourceProperty]
		public bool IsExecutable
		{
			get
			{
				return this._isExecutable;
			}
			set
			{
				if (value != this._isExecutable)
				{
					this._isExecutable = value;
					base.OnPropertyChangedWithValue(value, "IsExecutable");
				}
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00015D55 File Offset: 0x00013F55
		// (set) Token: 0x060002CD RID: 717 RVA: 0x00015D5D File Offset: 0x00013F5D
		[DataSourceProperty]
		public int NumOfReadyToUpgradeTroops
		{
			get
			{
				return this._numOfReadyToUpgradeTroops;
			}
			set
			{
				if (value != this._numOfReadyToUpgradeTroops)
				{
					this._numOfReadyToUpgradeTroops = value;
					base.OnPropertyChangedWithValue(value, "NumOfReadyToUpgradeTroops");
				}
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060002CE RID: 718 RVA: 0x00015D7B File Offset: 0x00013F7B
		// (set) Token: 0x060002CF RID: 719 RVA: 0x00015D83 File Offset: 0x00013F83
		[DataSourceProperty]
		public int NumOfUpgradeableTroops
		{
			get
			{
				return this._numOfUpgradeableTroops;
			}
			set
			{
				if (value != this._numOfUpgradeableTroops)
				{
					this._numOfUpgradeableTroops = value;
					base.OnPropertyChangedWithValue(value, "NumOfUpgradeableTroops");
				}
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00015DA1 File Offset: 0x00013FA1
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x00015DA9 File Offset: 0x00013FA9
		[DataSourceProperty]
		public int NumOfRecruitablePrisoners
		{
			get
			{
				return this._numOfRecruitablePrisoners;
			}
			set
			{
				if (value != this._numOfRecruitablePrisoners)
				{
					this._numOfRecruitablePrisoners = value;
					base.OnPropertyChangedWithValue(value, "NumOfRecruitablePrisoners");
				}
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x00015DC7 File Offset: 0x00013FC7
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x00015DCF File Offset: 0x00013FCF
		[DataSourceProperty]
		public int MaxXP
		{
			get
			{
				return this._maxXP;
			}
			set
			{
				if (value != this._maxXP)
				{
					this._maxXP = value;
					base.OnPropertyChangedWithValue(value, "MaxXP");
				}
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00015DED File Offset: 0x00013FED
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x00015DF5 File Offset: 0x00013FF5
		[DataSourceProperty]
		public int CurrentXP
		{
			get
			{
				return this._currentXP;
			}
			set
			{
				if (value != this._currentXP)
				{
					this._currentXP = value;
					base.OnPropertyChangedWithValue(value, "CurrentXP");
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x00015E13 File Offset: 0x00014013
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x00015E1B File Offset: 0x0001401B
		[DataSourceProperty]
		public int CurrentConformity
		{
			get
			{
				return this._currentConformity;
			}
			set
			{
				if (value != this._currentConformity)
				{
					this._currentConformity = value;
					base.OnPropertyChangedWithValue(value, "CurrentConformity");
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x00015E39 File Offset: 0x00014039
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x00015E41 File Offset: 0x00014041
		[DataSourceProperty]
		public int MaxConformity
		{
			get
			{
				return this._maxConformity;
			}
			set
			{
				if (value != this._maxConformity)
				{
					this._maxConformity = value;
					base.OnPropertyChangedWithValue(value, "MaxConformity");
				}
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002DA RID: 730 RVA: 0x00015E5F File Offset: 0x0001405F
		// (set) Token: 0x060002DB RID: 731 RVA: 0x00015E67 File Offset: 0x00014067
		[DataSourceProperty]
		public BasicTooltipViewModel TroopXPTooltip
		{
			get
			{
				return this._troopXPTooltip;
			}
			set
			{
				if (value != this._troopXPTooltip)
				{
					this._troopXPTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "TroopXPTooltip");
				}
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00015E85 File Offset: 0x00014085
		// (set) Token: 0x060002DD RID: 733 RVA: 0x00015E8D File Offset: 0x0001408D
		[DataSourceProperty]
		public BasicTooltipViewModel TroopConformityTooltip
		{
			get
			{
				return this._troopConformityTooltip;
			}
			set
			{
				if (value != this._troopConformityTooltip)
				{
					this._troopConformityTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "TroopConformityTooltip");
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00015EAB File Offset: 0x000140AB
		// (set) Token: 0x060002DF RID: 735 RVA: 0x00015EB3 File Offset: 0x000140B3
		[DataSourceProperty]
		public BasicTooltipViewModel TransferHint
		{
			get
			{
				return this._transferHint;
			}
			set
			{
				if (value != this._transferHint)
				{
					this._transferHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "TransferHint");
				}
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00015ED1 File Offset: 0x000140D1
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x00015ED9 File Offset: 0x000140D9
		[DataSourceProperty]
		public bool IsRecruitButtonsHiglighted
		{
			get
			{
				return this._isRecruitButtonsHiglighted;
			}
			set
			{
				if (value != this._isRecruitButtonsHiglighted)
				{
					this._isRecruitButtonsHiglighted = value;
					base.OnPropertyChangedWithValue(value, "IsRecruitButtonsHiglighted");
				}
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00015EF7 File Offset: 0x000140F7
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x00015EFF File Offset: 0x000140FF
		[DataSourceProperty]
		public bool IsTransferButtonHiglighted
		{
			get
			{
				return this._isTransferButtonHiglighted;
			}
			set
			{
				if (value != this._isTransferButtonHiglighted)
				{
					this._isTransferButtonHiglighted = value;
					base.OnPropertyChangedWithValue(value, "IsTransferButtonHiglighted");
				}
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00015F1D File Offset: 0x0001411D
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x00015F25 File Offset: 0x00014125
		[DataSourceProperty]
		public string StrNumOfUpgradableTroop
		{
			get
			{
				return this._strNumOfUpgradableTroop;
			}
			set
			{
				if (value != this._strNumOfUpgradableTroop)
				{
					this._strNumOfUpgradableTroop = value;
					base.OnPropertyChangedWithValue<string>(value, "StrNumOfUpgradableTroop");
				}
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x00015F48 File Offset: 0x00014148
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x00015F50 File Offset: 0x00014150
		[DataSourceProperty]
		public string StrNumOfRecruitableTroop
		{
			get
			{
				return this._strNumOfRecruitableTroop;
			}
			set
			{
				if (value != this._strNumOfRecruitableTroop)
				{
					this._strNumOfRecruitableTroop = value;
					base.OnPropertyChangedWithValue<string>(value, "StrNumOfRecruitableTroop");
				}
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x00015F73 File Offset: 0x00014173
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x00015F7B File Offset: 0x0001417B
		[DataSourceProperty]
		public string TroopID
		{
			get
			{
				return this._troopID;
			}
			set
			{
				if (value != this._troopID)
				{
					this._troopID = value;
					base.OnPropertyChangedWithValue<string>(value, "TroopID");
				}
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00015F9E File Offset: 0x0001419E
		// (set) Token: 0x060002EB RID: 747 RVA: 0x00015FA6 File Offset: 0x000141A6
		[DataSourceProperty]
		public string UpgradeCostText
		{
			get
			{
				return this._upgradeCostText;
			}
			set
			{
				if (value != this._upgradeCostText)
				{
					this._upgradeCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "UpgradeCostText");
				}
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00015FC9 File Offset: 0x000141C9
		// (set) Token: 0x060002ED RID: 749 RVA: 0x00015FD1 File Offset: 0x000141D1
		[DataSourceProperty]
		public string RecruitMoraleCostText
		{
			get
			{
				return this._recruitMoraleCostText;
			}
			set
			{
				if (value != this._recruitMoraleCostText)
				{
					this._recruitMoraleCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "RecruitMoraleCostText");
				}
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00015FF4 File Offset: 0x000141F4
		// (set) Token: 0x060002EF RID: 751 RVA: 0x00015FFC File Offset: 0x000141FC
		[DataSourceProperty]
		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				if (this._index != value)
				{
					this._index = value;
					base.OnPropertyChangedWithValue(value, "Index");
				}
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0001601A File Offset: 0x0001421A
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x00016022 File Offset: 0x00014222
		[DataSourceProperty]
		public int TransferAmount
		{
			get
			{
				return this._transferAmount;
			}
			set
			{
				if (value <= 0)
				{
					value = 1;
				}
				if (this._transferAmount != value)
				{
					this._transferAmount = value;
					base.OnPropertyChangedWithValue(value, "TransferAmount");
					base.OnPropertyChanged("TransferString");
				}
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x00016052 File Offset: 0x00014252
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0001605A File Offset: 0x0001425A
		[DataSourceProperty]
		public bool IsTroopTransferrable
		{
			get
			{
				return this._isTroopTransferrable;
			}
			set
			{
				if (this.Character != CharacterObject.PlayerCharacter)
				{
					this._isTroopTransferrable = value;
					base.OnPropertyChangedWithValue(value, "IsTroopTransferrable");
				}
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0001607C File Offset: 0x0001427C
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x00016084 File Offset: 0x00014284
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x000160A8 File Offset: 0x000142A8
		[DataSourceProperty]
		public string TroopNum
		{
			get
			{
				if (this.Character != null && this.Character.IsHero)
				{
					return "1";
				}
				if (this.Troop.Character == null)
				{
					return "-1";
				}
				int num = this.Troop.Number - this.Troop.WoundedNumber;
				string text = GameTexts.FindText("str_party_nameplate_wounded_abbr", null).ToString();
				if (num != this.Troop.Number && this.Type != PartyScreenLogic.TroopType.Prisoner)
				{
					return string.Concat(new object[]
					{
						num,
						"+",
						this.Troop.WoundedNumber,
						text
					});
				}
				return this.Troop.Number.ToString();
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0001617C File Offset: 0x0001437C
		[DataSourceProperty]
		public bool IsHeroWounded
		{
			get
			{
				CharacterObject character = this.Character;
				return character != null && character.IsHero && this.Character.HeroObject.IsWounded;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x000161A4 File Offset: 0x000143A4
		[DataSourceProperty]
		public int HeroHealth
		{
			get
			{
				CharacterObject character = this.Character;
				if (character != null && character.IsHero)
				{
					return MathF.Ceiling((float)this.Character.HeroObject.HitPoints * 100f / (float)this.Character.MaxHitPoints());
				}
				return 0;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x000161F0 File Offset: 0x000143F0
		[DataSourceProperty]
		public int Number
		{
			get
			{
				this.IsTroopTransferrable = this._initIsTroopTransferable && this.Troop.Number > 0;
				return this.Troop.Number;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00016230 File Offset: 0x00014430
		[DataSourceProperty]
		public int WoundedCount
		{
			get
			{
				if (this.Troop.Character == null)
				{
					return 0;
				}
				return this.Troop.WoundedNumber;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0001625A File Offset: 0x0001445A
		// (set) Token: 0x060002FC RID: 764 RVA: 0x00016262 File Offset: 0x00014462
		[DataSourceProperty]
		public BasicTooltipViewModel RecruitPrisonerHint
		{
			get
			{
				return this._recruitPrisonerHint;
			}
			set
			{
				if (value != this._recruitPrisonerHint)
				{
					this._recruitPrisonerHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "RecruitPrisonerHint");
				}
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00016280 File Offset: 0x00014480
		// (set) Token: 0x060002FE RID: 766 RVA: 0x00016288 File Offset: 0x00014488
		[DataSourceProperty]
		public CharacterImageIdentifierVM Code
		{
			get
			{
				return this._code;
			}
			set
			{
				if (value != this._code)
				{
					this._code = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Code");
				}
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002FF RID: 767 RVA: 0x000162A6 File Offset: 0x000144A6
		// (set) Token: 0x06000300 RID: 768 RVA: 0x000162AE File Offset: 0x000144AE
		[DataSourceProperty]
		public BasicTooltipViewModel ExecutePrisonerHint
		{
			get
			{
				return this._executePrisonerHint;
			}
			set
			{
				if (value != this._executePrisonerHint)
				{
					this._executePrisonerHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ExecutePrisonerHint");
				}
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000301 RID: 769 RVA: 0x000162CC File Offset: 0x000144CC
		// (set) Token: 0x06000302 RID: 770 RVA: 0x000162D4 File Offset: 0x000144D4
		[DataSourceProperty]
		public MBBindingList<UpgradeTargetVM> Upgrades
		{
			get
			{
				return this._upgrades;
			}
			set
			{
				if (value != this._upgrades)
				{
					this._upgrades = value;
					base.OnPropertyChangedWithValue<MBBindingList<UpgradeTargetVM>>(value, "Upgrades");
				}
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000303 RID: 771 RVA: 0x000162F2 File Offset: 0x000144F2
		// (set) Token: 0x06000304 RID: 772 RVA: 0x000162FA File Offset: 0x000144FA
		[DataSourceProperty]
		public BasicTooltipViewModel HeroHealthHint
		{
			get
			{
				return this._heroHealthHint;
			}
			set
			{
				if (value != this._heroHealthHint)
				{
					this._heroHealthHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "HeroHealthHint");
				}
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00016318 File Offset: 0x00014518
		// (set) Token: 0x06000306 RID: 774 RVA: 0x00016320 File Offset: 0x00014520
		[DataSourceProperty]
		public bool IsHero
		{
			get
			{
				return this._isHero;
			}
			set
			{
				if (value != this._isHero)
				{
					this._isHero = value;
					base.OnPropertyChangedWithValue(value, "IsHero");
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0001633E File Offset: 0x0001453E
		// (set) Token: 0x06000308 RID: 776 RVA: 0x00016346 File Offset: 0x00014546
		[DataSourceProperty]
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (value != this._isMainHero)
				{
					this._isMainHero = value;
					base.OnPropertyChangedWithValue(value, "IsMainHero");
				}
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00016364 File Offset: 0x00014564
		// (set) Token: 0x0600030A RID: 778 RVA: 0x0001636C File Offset: 0x0001456C
		[DataSourceProperty]
		public bool IsPrisoner
		{
			get
			{
				return this._isPrisoner;
			}
			set
			{
				if (value != this._isPrisoner)
				{
					this._isPrisoner = value;
					base.OnPropertyChangedWithValue(value, "IsPrisoner");
				}
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600030B RID: 779 RVA: 0x0001638A File Offset: 0x0001458A
		// (set) Token: 0x0600030C RID: 780 RVA: 0x00016392 File Offset: 0x00014592
		[DataSourceProperty]
		public bool IsPrisonerOfPlayer
		{
			get
			{
				return this._isPrisonerOfPlayer;
			}
			set
			{
				if (value != this._isPrisonerOfPlayer)
				{
					this._isPrisonerOfPlayer = value;
					base.OnPropertyChangedWithValue(value, "IsPrisonerOfPlayer");
				}
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600030D RID: 781 RVA: 0x000163B0 File Offset: 0x000145B0
		// (set) Token: 0x0600030E RID: 782 RVA: 0x000163B8 File Offset: 0x000145B8
		[DataSourceProperty]
		public bool IsHeroPrisonerOfPlayer
		{
			get
			{
				return this._isHeroPrisonerOfPlayer;
			}
			set
			{
				if (value != this._isHeroPrisonerOfPlayer)
				{
					this._isHeroPrisonerOfPlayer = value;
					base.OnPropertyChangedWithValue(value, "IsHeroPrisonerOfPlayer");
				}
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600030F RID: 783 RVA: 0x000163D6 File Offset: 0x000145D6
		// (set) Token: 0x06000310 RID: 784 RVA: 0x000163DE File Offset: 0x000145DE
		[DataSourceProperty]
		public bool AnyUpgradeHasRequirement
		{
			get
			{
				return this._anyUpgradeHasRequirement;
			}
			set
			{
				if (value != this._anyUpgradeHasRequirement)
				{
					this._anyUpgradeHasRequirement = value;
					base.OnPropertyChangedWithValue(value, "AnyUpgradeHasRequirement");
				}
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000311 RID: 785 RVA: 0x000163FC File Offset: 0x000145FC
		// (set) Token: 0x06000312 RID: 786 RVA: 0x00016404 File Offset: 0x00014604
		[DataSourceProperty]
		public StringItemWithHintVM TierIconData
		{
			get
			{
				return this._tierIconData;
			}
			set
			{
				if (value != this._tierIconData)
				{
					this._tierIconData = value;
					base.OnPropertyChangedWithValue<StringItemWithHintVM>(value, "TierIconData");
				}
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00016422 File Offset: 0x00014622
		// (set) Token: 0x06000314 RID: 788 RVA: 0x0001642A File Offset: 0x0001462A
		[DataSourceProperty]
		public StringItemWithHintVM TypeIconData
		{
			get
			{
				return this._typeIconData;
			}
			set
			{
				if (value != this._typeIconData)
				{
					this._typeIconData = value;
					base.OnPropertyChangedWithValue<StringItemWithHintVM>(value, "TypeIconData");
				}
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00016448 File Offset: 0x00014648
		// (set) Token: 0x06000316 RID: 790 RVA: 0x00016450 File Offset: 0x00014650
		[DataSourceProperty]
		public bool HasEnoughGold
		{
			get
			{
				return this._hasEnoughGold;
			}
			set
			{
				if (value != this._hasEnoughGold)
				{
					this._hasEnoughGold = value;
					base.OnPropertyChangedWithValue(value, "HasEnoughGold");
				}
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0001646E File Offset: 0x0001466E
		// (set) Token: 0x06000318 RID: 792 RVA: 0x00016476 File Offset: 0x00014676
		[DataSourceProperty]
		public bool IsTalkableCharacter
		{
			get
			{
				return this._isTalkableCharacter;
			}
			set
			{
				if (value != this._isTalkableCharacter)
				{
					this._isTalkableCharacter = value;
					base.OnPropertyChangedWithValue(value, "IsTalkableCharacter");
				}
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00016494 File Offset: 0x00014694
		// (set) Token: 0x0600031A RID: 794 RVA: 0x0001649C File Offset: 0x0001469C
		[DataSourceProperty]
		public bool CanTalk
		{
			get
			{
				return this._canTalk;
			}
			set
			{
				if (value != this._canTalk)
				{
					this._canTalk = value;
					base.OnPropertyChangedWithValue(value, "CanTalk");
				}
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600031B RID: 795 RVA: 0x000164BA File Offset: 0x000146BA
		// (set) Token: 0x0600031C RID: 796 RVA: 0x000164C2 File Offset: 0x000146C2
		[DataSourceProperty]
		public HintViewModel TalkHint
		{
			get
			{
				return this._talkHint;
			}
			set
			{
				if (value != this._talkHint)
				{
					this._talkHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "TalkHint");
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600031D RID: 797 RVA: 0x000164E0 File Offset: 0x000146E0
		// (set) Token: 0x0600031E RID: 798 RVA: 0x000164E8 File Offset: 0x000146E8
		[DataSourceProperty]
		public PartyTradeVM TradeData
		{
			get
			{
				return this._tradeData;
			}
			set
			{
				if (value != this._tradeData)
				{
					this._tradeData = value;
					base.OnPropertyChangedWithValue<PartyTradeVM>(value, "TradeData");
				}
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00016506 File Offset: 0x00014706
		// (set) Token: 0x06000320 RID: 800 RVA: 0x0001650E File Offset: 0x0001470E
		[DataSourceProperty]
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
			set
			{
				if (value != this._isLocked)
				{
					this._isLocked = value;
					base.OnPropertyChangedWithValue(value, "IsLocked");
					Action<PartyCharacterVM, bool> processCharacterLock = PartyCharacterVM.ProcessCharacterLock;
					if (processCharacterLock == null)
					{
						return;
					}
					processCharacterLock(this, value);
				}
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000321 RID: 801 RVA: 0x0001653D File Offset: 0x0001473D
		// (set) Token: 0x06000322 RID: 802 RVA: 0x00016545 File Offset: 0x00014745
		[DataSourceProperty]
		public HintViewModel LockHint
		{
			get
			{
				return this._lockHint;
			}
			set
			{
				if (value != this._lockHint)
				{
					this._lockHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LockHint");
				}
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000323 RID: 803 RVA: 0x00016563 File Offset: 0x00014763
		// (set) Token: 0x06000324 RID: 804 RVA: 0x0001656B File Offset: 0x0001476B
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

		// Token: 0x0400012C RID: 300
		public static bool IsShiftingDisabled;

		// Token: 0x0400012D RID: 301
		public static Action<PartyCharacterVM, bool> ProcessCharacterLock;

		// Token: 0x0400012E RID: 302
		public static Action<PartyCharacterVM> SetSelected;

		// Token: 0x0400012F RID: 303
		public static Action<PartyCharacterVM, int, int, PartyScreenLogic.PartyRosterSide> OnTransfer;

		// Token: 0x04000130 RID: 304
		public static Action<PartyCharacterVM> OnShift;

		// Token: 0x04000131 RID: 305
		public static Action<PartyCharacterVM> OnFocus;

		// Token: 0x04000132 RID: 306
		public readonly PartyScreenLogic.PartyRosterSide Side;

		// Token: 0x04000133 RID: 307
		public readonly PartyScreenLogic.TroopType Type;

		// Token: 0x04000134 RID: 308
		protected readonly PartyVM _partyVm;

		// Token: 0x04000135 RID: 309
		protected readonly PartyScreenLogic _partyScreenLogic;

		// Token: 0x04000136 RID: 310
		protected readonly bool _initIsTroopTransferable;

		// Token: 0x04000137 RID: 311
		private Tuple<bool, TextObject> _partyCharacterTalkPermission;

		// Token: 0x0400013A RID: 314
		private TroopRosterElement _troop;

		// Token: 0x0400013B RID: 315
		private CharacterObject _character;

		// Token: 0x0400013C RID: 316
		private string _name;

		// Token: 0x0400013D RID: 317
		private string _strNumOfUpgradableTroop;

		// Token: 0x0400013E RID: 318
		private string _strNumOfRecruitableTroop;

		// Token: 0x0400013F RID: 319
		private string _troopID;

		// Token: 0x04000140 RID: 320
		private string _upgradeCostText;

		// Token: 0x04000141 RID: 321
		private string _recruitMoraleCostText;

		// Token: 0x04000142 RID: 322
		private MBBindingList<UpgradeTargetVM> _upgrades;

		// Token: 0x04000143 RID: 323
		private CharacterImageIdentifierVM _code;

		// Token: 0x04000144 RID: 324
		private BasicTooltipViewModel _transferHint;

		// Token: 0x04000145 RID: 325
		private BasicTooltipViewModel _recruitPrisonerHint;

		// Token: 0x04000146 RID: 326
		private BasicTooltipViewModel _executePrisonerHint;

		// Token: 0x04000147 RID: 327
		private BasicTooltipViewModel _heroHealthHint;

		// Token: 0x04000148 RID: 328
		private HintViewModel _talkHint;

		// Token: 0x04000149 RID: 329
		private int _transferAmount = 1;

		// Token: 0x0400014A RID: 330
		private int _index = -2;

		// Token: 0x0400014B RID: 331
		private int _numOfReadyToUpgradeTroops;

		// Token: 0x0400014C RID: 332
		private int _numOfUpgradeableTroops;

		// Token: 0x0400014D RID: 333
		private int _numOfRecruitablePrisoners;

		// Token: 0x0400014E RID: 334
		private int _maxXP;

		// Token: 0x0400014F RID: 335
		private int _currentXP;

		// Token: 0x04000150 RID: 336
		private int _maxConformity;

		// Token: 0x04000151 RID: 337
		private int _currentConformity;

		// Token: 0x04000152 RID: 338
		private BasicTooltipViewModel _troopXPTooltip;

		// Token: 0x04000153 RID: 339
		private BasicTooltipViewModel _troopConformityTooltip;

		// Token: 0x04000154 RID: 340
		private bool _isHero;

		// Token: 0x04000155 RID: 341
		private bool _isMainHero;

		// Token: 0x04000156 RID: 342
		private bool _isPrisoner;

		// Token: 0x04000157 RID: 343
		private bool _isPrisonerOfPlayer;

		// Token: 0x04000158 RID: 344
		private bool _isRecruitablePrisoner;

		// Token: 0x04000159 RID: 345
		private bool _isUpgradableTroop;

		// Token: 0x0400015A RID: 346
		private bool _isTroopTransferrable;

		// Token: 0x0400015B RID: 347
		private bool _isHeroPrisonerOfPlayer;

		// Token: 0x0400015C RID: 348
		private bool _isTroopUpgradable;

		// Token: 0x0400015D RID: 349
		private StringItemWithHintVM _tierIconData;

		// Token: 0x0400015E RID: 350
		private bool _hasEnoughGold;

		// Token: 0x0400015F RID: 351
		private bool _anyUpgradeHasRequirement;

		// Token: 0x04000160 RID: 352
		private StringItemWithHintVM _typeIconData;

		// Token: 0x04000161 RID: 353
		private bool _isRecruitButtonsHiglighted;

		// Token: 0x04000162 RID: 354
		private bool _isTransferButtonHiglighted;

		// Token: 0x04000163 RID: 355
		private bool _isFormationEnabled;

		// Token: 0x04000164 RID: 356
		private PartyTradeVM _tradeData;

		// Token: 0x04000165 RID: 357
		private bool _isTroopRecruitable;

		// Token: 0x04000166 RID: 358
		private bool _isExecutable;

		// Token: 0x04000167 RID: 359
		private bool _isLocked;

		// Token: 0x04000168 RID: 360
		private HintViewModel _lockHint;

		// Token: 0x04000169 RID: 361
		private bool _isTalkableCharacter;

		// Token: 0x0400016A RID: 362
		private bool _canTalk;

		// Token: 0x0400016B RID: 363
		private bool _isSelected;
	}
}
