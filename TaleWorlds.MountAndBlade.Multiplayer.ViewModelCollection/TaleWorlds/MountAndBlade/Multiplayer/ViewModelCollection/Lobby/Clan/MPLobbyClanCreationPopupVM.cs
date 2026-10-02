using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Messages.FromLobbyServer.ToClient;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x0200006A RID: 106
	public class MPLobbyClanCreationPopupVM : ViewModel
	{
		// Token: 0x06000A42 RID: 2626 RVA: 0x0001FBE5 File Offset: 0x0001DDE5
		public MPLobbyClanCreationPopupVM()
		{
			this.PartyMembersList = new MBBindingList<MPLobbyClanMemberItemVM>();
			this.PrepareFactionsList();
			this.PrepareSigilIconsList();
			this.RefreshValues();
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0001FC0C File Offset: 0x0001DE0C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CreateClanText = new TextObject("{=ECb8IPbA}Create Clan", null).ToString();
			this.NameText = new TextObject("{=PDdh1sBj}Name", null).ToString();
			this.TagText = new TextObject("{=OUvFT99g}Tag", null).ToString();
			this.FactionText = new TextObject("{=PUjDWe5j}Culture", null).ToString();
			this.SigilText = new TextObject("{=P5Z9owOy}Sigil", null).ToString();
			this.CreateText = new TextObject("{=65oGXBYQ}Create", null).ToString();
			this.CancelText = new TextObject("{=3CpNUnVl}Cancel", null).ToString();
			this.WaitingForConfirmationText = new TextObject("{=08KLQa3P}Waiting For Party Members", null).ToString();
			this.ResetAll();
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0001FCD5 File Offset: 0x0001DED5
		private void ResetAll()
		{
			this.ResetErrorTexts();
			this.ResetUserInputs();
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0001FCE3 File Offset: 0x0001DEE3
		private void ResetErrorTexts()
		{
			this.NameErrorText = "";
			this.TagErrorText = "";
			this.FactionErrorText = "";
			this.SigilIconErrorText = "";
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0001FD11 File Offset: 0x0001DF11
		private void ResetUserInputs()
		{
			this.NameInputText = "";
			this.TagInputText = "";
			this.OnFactionSelection(null);
			this.OnSigilIconSelection(null);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0001FD37 File Offset: 0x0001DF37
		public void ExecuteOpenPopup()
		{
			this.RefreshValues();
			this.IsEnabled = true;
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0001FD46 File Offset: 0x0001DF46
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0001FD50 File Offset: 0x0001DF50
		private void PrepareFactionsList()
		{
			this._selectedFaction = null;
			this.FactionsList = new MBBindingList<MPCultureItemVM>
			{
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("vlandia").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("sturgia").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("battania").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("khuzait").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("aserai").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection))
			};
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0001FE90 File Offset: 0x0001E090
		private void PrepareSigilIconsList()
		{
			this.IconsList = new MBBindingList<MPLobbySigilItemVM>();
			this._selectedSigilIcon = null;
			foreach (BannerIconGroup bannerIconGroup in BannerManager.Instance.BannerIconGroups)
			{
				if (!bannerIconGroup.IsPattern)
				{
					foreach (KeyValuePair<int, BannerIconData> keyValuePair in bannerIconGroup.AvailableIcons)
					{
						MPLobbySigilItemVM mplobbySigilItemVM = new MPLobbySigilItemVM(keyValuePair.Key, new Action<MPLobbySigilItemVM>(this.OnSigilIconSelection));
						this.IconsList.Add(mplobbySigilItemVM);
					}
				}
			}
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0001FF5C File Offset: 0x0001E15C
		private void PreparePartyMembersList()
		{
			this.PartyMembersList.Clear();
			foreach (PartyPlayerInLobbyClient partyPlayerInLobbyClient in NetworkMain.GameClient.PlayersInParty)
			{
				if (partyPlayerInLobbyClient.PlayerId != NetworkMain.GameClient.PlayerID)
				{
					MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = new MPLobbyClanMemberItemVM(partyPlayerInLobbyClient.PlayerId);
					mplobbyClanMemberItemVM.InviteAcceptInfo = new TextObject("{=c0ZdKSkn}Waiting", null).ToString();
					this.PartyMembersList.Add(mplobbyClanMemberItemVM);
				}
			}
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0001FFFC File Offset: 0x0001E1FC
		private void OnFactionSelection(MPCultureItemVM faction)
		{
			if (faction != this._selectedFaction)
			{
				if (this._selectedFaction != null)
				{
					this._selectedFaction.IsSelected = false;
				}
				this._selectedFaction = faction;
				if (this._selectedFaction != null)
				{
					this._selectedFaction.IsSelected = true;
					this.FactionErrorText = "";
				}
			}
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0002004C File Offset: 0x0001E24C
		private void OnSigilIconSelection(MPLobbySigilItemVM sigilIcon)
		{
			if (sigilIcon != this._selectedSigilIcon)
			{
				if (this._selectedSigilIcon != null)
				{
					this._selectedSigilIcon.IsSelected = false;
				}
				this._selectedSigilIcon = sigilIcon;
				if (this._selectedSigilIcon != null)
				{
					this._selectedSigilIcon.IsSelected = true;
					this.SigilIconErrorText = "";
				}
			}
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0002009C File Offset: 0x0001E29C
		private void UpdateNameErrorText(StringValidationError error)
		{
			this.NameErrorText = "";
			if (error == StringValidationError.InvalidLength)
			{
				this.NameErrorText = new TextObject("{=bExIl1A2}Name Length Is Invalid", null).ToString();
				return;
			}
			if (error == StringValidationError.AlreadyExists)
			{
				this.NameErrorText = new TextObject("{=Agtv9l7S}This Name Already Exists", null).ToString();
				return;
			}
			if (error == StringValidationError.HasNonLettersCharacters)
			{
				this.NameErrorText = new TextObject("{=lO1hok44}Name Has Invalid Characters In It", null).ToString();
				return;
			}
			if (error == StringValidationError.ContainsProfanity)
			{
				this.NameErrorText = new TextObject("{=cl2DnRYR}Name Should Not Contain Offensive Words", null).ToString();
				return;
			}
			if (error == StringValidationError.Unspecified)
			{
				this.NameErrorText = new TextObject("{=UEgS8RcB}Name Has Invalid Content", null).ToString();
			}
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0002013C File Offset: 0x0001E33C
		private void UpdateTagErrorText(StringValidationError error)
		{
			this.TagErrorText = "";
			if (error == StringValidationError.InvalidLength)
			{
				this.TagErrorText = new TextObject("{=MjnlWhih}Tag Length Is Invalid", null).ToString();
				return;
			}
			if (error == StringValidationError.AlreadyExists)
			{
				this.TagErrorText = new TextObject("{=ulzyykHO}This Tag Already Exists", null).ToString();
				return;
			}
			if (error == StringValidationError.HasNonLettersCharacters)
			{
				this.TagErrorText = new TextObject("{=FjmxNxZJ}Tag Has Invalid Characters In It", null).ToString();
				return;
			}
			if (error == StringValidationError.ContainsProfanity)
			{
				this.TagErrorText = new TextObject("{=jyJXcOLe}Tag Should Not Contain Offensive Words", null).ToString();
				return;
			}
			if (error == StringValidationError.Unspecified)
			{
				this.TagErrorText = new TextObject("{=hCNnqVgK}Tag Has Invalid Content", null).ToString();
			}
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000201D9 File Offset: 0x0001E3D9
		public void UpdateFactionErrorText()
		{
			this.FactionErrorText = "";
			this.FactionErrorText = new TextObject("{=p83IO9ls}You must select a culture", null).ToString();
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x000201FC File Offset: 0x0001E3FC
		public void UpdateSigilIconErrorText()
		{
			this.SigilIconErrorText = "";
			this.SigilIconErrorText = new TextObject("{=uOrwqeQl}You must select a sigil icon", null).ToString();
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00020220 File Offset: 0x0001E420
		public void UpdateConfirmation(PlayerId playerId, ClanCreationAnswer answer)
		{
			foreach (MPLobbyClanMemberItemVM mplobbyClanMemberItemVM in this.PartyMembersList)
			{
				if (mplobbyClanMemberItemVM.ProvidedID == playerId)
				{
					if (answer == ClanCreationAnswer.Accepted)
					{
						mplobbyClanMemberItemVM.InviteAcceptInfo = new TextObject("{=JTMegIk4}Accepted", null).ToString();
					}
					else if (answer == ClanCreationAnswer.Declined)
					{
						mplobbyClanMemberItemVM.InviteAcceptInfo = new TextObject("{=FgaORzy5}Declined", null).ToString();
					}
				}
			}
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x000202AC File Offset: 0x0001E4AC
		private BasicCultureObject GetSelectedCulture()
		{
			return Game.Current.ObjectManager.GetObject<BasicCultureObject>(this._selectedFaction.CultureCode);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x000202C8 File Offset: 0x0001E4C8
		private Banner GetCreatedClanSigil()
		{
			BasicCultureObject selectedCulture = this.GetSelectedCulture();
			Banner banner = new Banner(selectedCulture.Banner, selectedCulture.BackgroundColor1, selectedCulture.ForegroundColor1);
			banner.SetIconMeshId(this._selectedSigilIcon.IconID);
			return banner;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00020304 File Offset: 0x0001E504
		private async void ExecuteTryCreateClan()
		{
			bool areAllInputsValid = true;
			this.ResetErrorTexts();
			CheckClanParameterValidResult checkClanParameterValidResult = await NetworkMain.GameClient.ClanNameExists(this.NameInputText);
			if (!checkClanParameterValidResult.IsValid)
			{
				areAllInputsValid = false;
				this.UpdateNameErrorText(checkClanParameterValidResult.Error);
			}
			TaskAwaiter<bool> taskAwaiter = PlatformServices.Instance.VerifyString(this.NameInputText).GetAwaiter();
			TaskAwaiter<bool> taskAwaiter2;
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				areAllInputsValid = false;
				this.UpdateNameErrorText(StringValidationError.Unspecified);
			}
			CheckClanParameterValidResult checkClanParameterValidResult2 = await NetworkMain.GameClient.ClanTagExists(this.TagInputText);
			if (!checkClanParameterValidResult2.IsValid)
			{
				areAllInputsValid = false;
				this.UpdateTagErrorText(checkClanParameterValidResult2.Error);
			}
			taskAwaiter = PlatformServices.Instance.VerifyString(this.TagInputText).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				areAllInputsValid = false;
				this.UpdateTagErrorText(StringValidationError.Unspecified);
			}
			if (this._selectedFaction == null)
			{
				areAllInputsValid = false;
				this.UpdateFactionErrorText();
			}
			if (this._selectedSigilIcon == null)
			{
				areAllInputsValid = false;
				this.UpdateSigilIconErrorText();
			}
			if (areAllInputsValid)
			{
				this.HasCreationStarted = true;
				NetworkMain.GameClient.SendCreateClanMessage(this.NameInputText, this.TagInputText, this.GetSelectedCulture().StringId, this.GetCreatedClanSigil().Serialize());
			}
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0002033D File Offset: 0x0001E53D
		public void ExecuteSwitchToWaiting()
		{
			this.PreparePartyMembersList();
			this.IsWaiting = true;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0002034C File Offset: 0x0001E54C
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey == null)
			{
				return;
			}
			cancelInputKey.OnFinalize();
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00020364 File Offset: 0x0001E564
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x00020373 File Offset: 0x0001E573
		// (set) Token: 0x06000A5A RID: 2650 RVA: 0x0002037B File Offset: 0x0001E57B
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
					base.OnPropertyChanged("CancelInputKey");
				}
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x00020398 File Offset: 0x0001E598
		// (set) Token: 0x06000A5C RID: 2652 RVA: 0x000203A0 File Offset: 0x0001E5A0
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
					base.OnPropertyChanged("IsEnabled");
				}
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x000203BD File Offset: 0x0001E5BD
		// (set) Token: 0x06000A5E RID: 2654 RVA: 0x000203C5 File Offset: 0x0001E5C5
		[DataSourceProperty]
		public bool HasCreationStarted
		{
			get
			{
				return this._hasCreationStarted;
			}
			set
			{
				if (value != this._hasCreationStarted)
				{
					this._hasCreationStarted = value;
					base.OnPropertyChanged("HasCreationStarted");
				}
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x000203E2 File Offset: 0x0001E5E2
		// (set) Token: 0x06000A60 RID: 2656 RVA: 0x000203EA File Offset: 0x0001E5EA
		[DataSourceProperty]
		public bool IsWaiting
		{
			get
			{
				return this._isWaiting;
			}
			set
			{
				if (value != this._isWaiting)
				{
					this._isWaiting = value;
					base.OnPropertyChanged("IsWaiting");
				}
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x00020407 File Offset: 0x0001E607
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x0002040F File Offset: 0x0001E60F
		[DataSourceProperty]
		public string CreateClanText
		{
			get
			{
				return this._createClanText;
			}
			set
			{
				if (value != this._createClanText)
				{
					this._createClanText = value;
					base.OnPropertyChanged("CreateClanText");
				}
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x00020431 File Offset: 0x0001E631
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x00020439 File Offset: 0x0001E639
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
					base.OnPropertyChanged("NameText");
				}
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0002045B File Offset: 0x0001E65B
		// (set) Token: 0x06000A66 RID: 2662 RVA: 0x00020463 File Offset: 0x0001E663
		[DataSourceProperty]
		public string NameErrorText
		{
			get
			{
				return this._nameErrorText;
			}
			set
			{
				if (value != this._nameErrorText)
				{
					this._nameErrorText = value;
					base.OnPropertyChanged("NameErrorText");
				}
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x00020485 File Offset: 0x0001E685
		// (set) Token: 0x06000A68 RID: 2664 RVA: 0x0002048D File Offset: 0x0001E68D
		[DataSourceProperty]
		public string TagText
		{
			get
			{
				return this._tagText;
			}
			set
			{
				if (value != this._tagText)
				{
					this._tagText = value;
					base.OnPropertyChanged("TagText");
				}
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x000204AF File Offset: 0x0001E6AF
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x000204B7 File Offset: 0x0001E6B7
		[DataSourceProperty]
		public string TagErrorText
		{
			get
			{
				return this._tagErrorText;
			}
			set
			{
				if (value != this._tagErrorText)
				{
					this._tagErrorText = value;
					base.OnPropertyChanged("TagErrorText");
				}
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x000204D9 File Offset: 0x0001E6D9
		// (set) Token: 0x06000A6C RID: 2668 RVA: 0x000204E1 File Offset: 0x0001E6E1
		[DataSourceProperty]
		public string FactionText
		{
			get
			{
				return this._factionText;
			}
			set
			{
				if (value != this._factionText)
				{
					this._factionText = value;
					base.OnPropertyChanged("FactionText");
				}
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00020503 File Offset: 0x0001E703
		// (set) Token: 0x06000A6E RID: 2670 RVA: 0x0002050B File Offset: 0x0001E70B
		[DataSourceProperty]
		public string FactionErrorText
		{
			get
			{
				return this._factionErrorText;
			}
			set
			{
				if (value != this._factionErrorText)
				{
					this._factionErrorText = value;
					base.OnPropertyChanged("FactionErrorText");
				}
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0002052D File Offset: 0x0001E72D
		// (set) Token: 0x06000A70 RID: 2672 RVA: 0x00020535 File Offset: 0x0001E735
		[DataSourceProperty]
		public string SigilText
		{
			get
			{
				return this._sigilText;
			}
			set
			{
				if (value != this._sigilText)
				{
					this._sigilText = value;
					base.OnPropertyChanged("SigilText");
				}
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00020557 File Offset: 0x0001E757
		// (set) Token: 0x06000A72 RID: 2674 RVA: 0x0002055F File Offset: 0x0001E75F
		[DataSourceProperty]
		public string SigilIconErrorText
		{
			get
			{
				return this._sigilIconErrorText;
			}
			set
			{
				if (value != this._sigilIconErrorText)
				{
					this._sigilIconErrorText = value;
					base.OnPropertyChanged("SigilIconErrorText");
				}
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x00020581 File Offset: 0x0001E781
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x00020589 File Offset: 0x0001E789
		[DataSourceProperty]
		public string CreateText
		{
			get
			{
				return this._createText;
			}
			set
			{
				if (value != this._createText)
				{
					this._createText = value;
					base.OnPropertyChanged("CreateText");
				}
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x000205AB File Offset: 0x0001E7AB
		// (set) Token: 0x06000A76 RID: 2678 RVA: 0x000205B3 File Offset: 0x0001E7B3
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChanged("CancelText");
				}
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x000205D5 File Offset: 0x0001E7D5
		// (set) Token: 0x06000A78 RID: 2680 RVA: 0x000205DD File Offset: 0x0001E7DD
		[DataSourceProperty]
		public string NameInputText
		{
			get
			{
				return this._nameInputText;
			}
			set
			{
				if (value != this._nameInputText)
				{
					this._nameInputText = value;
					base.OnPropertyChanged("NameInputText");
					this.NameErrorText = "";
				}
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0002060A File Offset: 0x0001E80A
		// (set) Token: 0x06000A7A RID: 2682 RVA: 0x00020612 File Offset: 0x0001E812
		[DataSourceProperty]
		public string TagInputText
		{
			get
			{
				return this._tagInputText;
			}
			set
			{
				if (value != this._tagInputText)
				{
					this._tagInputText = value;
					base.OnPropertyChanged("TagInputText");
					this.TagErrorText = "";
				}
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0002063F File Offset: 0x0001E83F
		// (set) Token: 0x06000A7C RID: 2684 RVA: 0x00020647 File Offset: 0x0001E847
		[DataSourceProperty]
		public string WaitingForConfirmationText
		{
			get
			{
				return this._waitingForConfirmationText;
			}
			set
			{
				if (value != this._waitingForConfirmationText)
				{
					this._waitingForConfirmationText = value;
					base.OnPropertyChanged("WaitingForConfirmationText");
				}
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x00020669 File Offset: 0x0001E869
		// (set) Token: 0x06000A7E RID: 2686 RVA: 0x00020671 File Offset: 0x0001E871
		[DataSourceProperty]
		public MBBindingList<MPCultureItemVM> FactionsList
		{
			get
			{
				return this._factionsList;
			}
			set
			{
				if (value != this._factionsList)
				{
					this._factionsList = value;
					base.OnPropertyChanged("FactionsList");
				}
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x0002068E File Offset: 0x0001E88E
		// (set) Token: 0x06000A80 RID: 2688 RVA: 0x00020696 File Offset: 0x0001E896
		[DataSourceProperty]
		public MBBindingList<MPLobbySigilItemVM> IconsList
		{
			get
			{
				return this._iconsList;
			}
			set
			{
				if (value != this._iconsList)
				{
					this._iconsList = value;
					base.OnPropertyChanged("IconsList");
				}
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x000206B3 File Offset: 0x0001E8B3
		// (set) Token: 0x06000A82 RID: 2690 RVA: 0x000206BB File Offset: 0x0001E8BB
		[DataSourceProperty]
		public MBBindingList<MPLobbyClanMemberItemVM> PartyMembersList
		{
			get
			{
				return this._partyMembersList;
			}
			set
			{
				if (value != this._partyMembersList)
				{
					this._partyMembersList = value;
					base.OnPropertyChanged("PartyMembersList");
				}
			}
		}

		// Token: 0x040004B1 RID: 1201
		private MPCultureItemVM _selectedFaction;

		// Token: 0x040004B2 RID: 1202
		private MPLobbySigilItemVM _selectedSigilIcon;

		// Token: 0x040004B3 RID: 1203
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040004B4 RID: 1204
		private bool _isEnabled;

		// Token: 0x040004B5 RID: 1205
		private bool _hasCreationStarted;

		// Token: 0x040004B6 RID: 1206
		private bool _isWaiting;

		// Token: 0x040004B7 RID: 1207
		private string _createClanText;

		// Token: 0x040004B8 RID: 1208
		private string _nameText;

		// Token: 0x040004B9 RID: 1209
		private string _nameErrorText;

		// Token: 0x040004BA RID: 1210
		private string _tagText;

		// Token: 0x040004BB RID: 1211
		private string _tagErrorText;

		// Token: 0x040004BC RID: 1212
		private string _factionText;

		// Token: 0x040004BD RID: 1213
		private string _factionErrorText;

		// Token: 0x040004BE RID: 1214
		private string _sigilText;

		// Token: 0x040004BF RID: 1215
		private string _sigilIconErrorText;

		// Token: 0x040004C0 RID: 1216
		private string _createText;

		// Token: 0x040004C1 RID: 1217
		private string _cancelText;

		// Token: 0x040004C2 RID: 1218
		private string _nameInputText;

		// Token: 0x040004C3 RID: 1219
		private string _tagInputText;

		// Token: 0x040004C4 RID: 1220
		private string _waitingForConfirmationText;

		// Token: 0x040004C5 RID: 1221
		private MBBindingList<MPCultureItemVM> _factionsList;

		// Token: 0x040004C6 RID: 1222
		private MBBindingList<MPLobbySigilItemVM> _iconsList;

		// Token: 0x040004C7 RID: 1223
		private MBBindingList<MPLobbyClanMemberItemVM> _partyMembersList;
	}
}
