using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000131 RID: 305
	public class ClanRoleItemVM : ViewModel
	{
		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06001C68 RID: 7272 RVA: 0x000679CB File Offset: 0x00065BCB
		// (set) Token: 0x06001C69 RID: 7273 RVA: 0x000679D3 File Offset: 0x00065BD3
		public PartyRole Role { get; private set; }

		// Token: 0x06001C6A RID: 7274 RVA: 0x000679DC File Offset: 0x00065BDC
		public ClanRoleItemVM(MobileParty party, PartyRole role, MBBindingList<ClanPartyMemberItemVM> heroMembers, Action<ClanRoleItemVM> onRoleSelectionToggled)
		{
			this.Role = role;
			this._party = party;
			this._onRoleSelectionToggled = onRoleSelectionToggled;
			this._heroMembers = heroMembers;
			this.NotAssignedHint = new HintViewModel(new TextObject("{=S1iS3OYj}Party leader is default for unassigned roles", null), null);
			this.DisabledHint = new HintViewModel();
			this.IsEnabled = true;
			this.RoleId = ClanRoleItemVM.GetRoleIdentifier(role);
			this.Refresh();
			this.RefreshValues();
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x00067A50 File Offset: 0x00065C50
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = GameTexts.FindText("role", this.Role.ToString()).ToString();
			this.NoEffectText = GameTexts.FindText("str_clan_role_no_effect", null).ToString();
			this.AssignedMemberEffects = ((this.EffectiveOwner != null) ? this.GetEffectsList(this.EffectiveOwner.HeroObject, this.Role) : "");
			this.HasEffects = !string.IsNullOrEmpty(this.AssignedMemberEffects);
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x00067AE4 File Offset: 0x00065CE4
		private static string GetRoleIdentifier(PartyRole role)
		{
			switch (role)
			{
			case PartyRole.Ruler:
				return "rule";
			case PartyRole.ClanLeader:
				return "clan_leader";
			case PartyRole.Governor:
				return "governor";
			case PartyRole.ArmyCommander:
				return "commander";
			case PartyRole.PartyLeader:
				return "party_leader";
			case PartyRole.PartyOwner:
				return "party_owner";
			case PartyRole.Surgeon:
				return "surgeon";
			case PartyRole.Engineer:
				return "engineer";
			case PartyRole.Scout:
				return "scout";
			case PartyRole.Quartermaster:
				return "quartermaser";
			case PartyRole.PartyMember:
				return "member";
			case PartyRole.Personal:
				return "personal";
			case PartyRole.Captain:
				return "captain";
			case PartyRole.FirstMate:
				return "first_mate";
			case PartyRole.Navigator:
				return "navigator";
			}
			return string.Empty;
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x00067B98 File Offset: 0x00065D98
		public void Refresh()
		{
			if (this._party == null)
			{
				this.EffectiveOwner = this._heroMembers.FirstOrDefault<ClanPartyMemberItemVM>();
				this.IsNotAssigned = false;
			}
			else
			{
				Hero hero;
				Hero effectiveRoleOwner;
				this.GetMemberAssignedToRole(this._party, this.Role, out hero, out effectiveRoleOwner);
				this.EffectiveOwner = this._heroMembers.FirstOrDefault<ClanPartyMemberItemVM>((ClanPartyMemberItemVM x) => x.HeroObject == effectiveRoleOwner);
				this.IsNotAssigned = hero == null;
			}
			this.RefreshValues();
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x00067C14 File Offset: 0x00065E14
		public void ExecuteToggleRoleSelection()
		{
			Action<ClanRoleItemVM> onRoleSelectionToggled = this._onRoleSelectionToggled;
			if (onRoleSelectionToggled == null)
			{
				return;
			}
			onRoleSelectionToggled(this);
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x00067C28 File Offset: 0x00065E28
		private void GetMemberAssignedToRole(MobileParty party, PartyRole role, out Hero roleOwner, out Hero effectiveRoleOwner)
		{
			roleOwner = party.GetRoleHolder(role);
			switch (role)
			{
			case PartyRole.Surgeon:
				effectiveRoleOwner = party.EffectiveSurgeon;
				return;
			case PartyRole.Engineer:
				effectiveRoleOwner = party.EffectiveEngineer;
				return;
			case PartyRole.Scout:
				effectiveRoleOwner = party.EffectiveScout;
				return;
			case PartyRole.Quartermaster:
				effectiveRoleOwner = party.EffectiveQuartermaster;
				return;
			case PartyRole.FirstMate:
				effectiveRoleOwner = party.EffectiveFirstMate;
				return;
			case PartyRole.Navigator:
				effectiveRoleOwner = party.EffectiveNavigator;
				return;
			}
			effectiveRoleOwner = party.LeaderHero;
			roleOwner = party.LeaderHero;
			Debug.FailedAssert("Given party role is not valid.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\ClanManagement\\ClanRoleItemVM.cs", "GetMemberAssignedToRole", 138);
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x00067CD2 File Offset: 0x00065ED2
		public void SetEnabled(bool enabled, TextObject disabledHint)
		{
			this.IsEnabled = enabled;
			this.DisabledHint.HintText = disabledHint;
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x00067CE8 File Offset: 0x00065EE8
		private string GetEffectsList(Hero hero, PartyRole role)
		{
			IEnumerable<SkillEffect> enumerable = SkillEffect.All.Where<SkillEffect>((SkillEffect x) => x.Role == role);
			StringBuilder stringBuilder = new StringBuilder();
			if (SkillHelper.GetHeroRelevantSkillValueForPartyRole(hero, role) > 0)
			{
				foreach (SkillEffect skillEffect in enumerable)
				{
					stringBuilder.AppendLine(SkillHelper.GetEffectDescriptionForSkillLevel(skillEffect, SkillHelper.GetHeroRelevantSkillValueForPartyRole(hero, role)).ToString());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06001C72 RID: 7282 RVA: 0x00067D88 File Offset: 0x00065F88
		// (set) Token: 0x06001C73 RID: 7283 RVA: 0x00067D90 File Offset: 0x00065F90
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

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06001C74 RID: 7284 RVA: 0x00067DAE File Offset: 0x00065FAE
		// (set) Token: 0x06001C75 RID: 7285 RVA: 0x00067DB6 File Offset: 0x00065FB6
		[DataSourceProperty]
		public ClanPartyMemberItemVM EffectiveOwner
		{
			get
			{
				return this._effectiveOwner;
			}
			set
			{
				if (value != this._effectiveOwner)
				{
					this._effectiveOwner = value;
					base.OnPropertyChangedWithValue<ClanPartyMemberItemVM>(value, "EffectiveOwner");
				}
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06001C76 RID: 7286 RVA: 0x00067DD4 File Offset: 0x00065FD4
		// (set) Token: 0x06001C77 RID: 7287 RVA: 0x00067DDC File Offset: 0x00065FDC
		[DataSourceProperty]
		public HintViewModel NotAssignedHint
		{
			get
			{
				return this._notAssignedHint;
			}
			set
			{
				if (value != this._notAssignedHint)
				{
					this._notAssignedHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "NotAssignedHint");
				}
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06001C78 RID: 7288 RVA: 0x00067DFA File Offset: 0x00065FFA
		// (set) Token: 0x06001C79 RID: 7289 RVA: 0x00067E02 File Offset: 0x00066002
		[DataSourceProperty]
		public HintViewModel DisabledHint
		{
			get
			{
				return this._disabledHint;
			}
			set
			{
				if (value != this._disabledHint)
				{
					this._disabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DisabledHint");
				}
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06001C7A RID: 7290 RVA: 0x00067E20 File Offset: 0x00066020
		// (set) Token: 0x06001C7B RID: 7291 RVA: 0x00067E28 File Offset: 0x00066028
		[DataSourceProperty]
		public bool IsNotAssigned
		{
			get
			{
				return this._isNotAssigned;
			}
			set
			{
				if (value != this._isNotAssigned)
				{
					this._isNotAssigned = value;
					base.OnPropertyChangedWithValue(value, "IsNotAssigned");
				}
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06001C7C RID: 7292 RVA: 0x00067E46 File Offset: 0x00066046
		// (set) Token: 0x06001C7D RID: 7293 RVA: 0x00067E4E File Offset: 0x0006604E
		[DataSourceProperty]
		public string RoleId
		{
			get
			{
				return this._roleId;
			}
			set
			{
				if (value != this._roleId)
				{
					this._roleId = value;
					base.OnPropertyChangedWithValue<string>(value, "RoleId");
				}
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06001C7E RID: 7294 RVA: 0x00067E71 File Offset: 0x00066071
		// (set) Token: 0x06001C7F RID: 7295 RVA: 0x00067E79 File Offset: 0x00066079
		[DataSourceProperty]
		public bool HasEffects
		{
			get
			{
				return this._hasEffects;
			}
			set
			{
				if (value != this._hasEffects)
				{
					this._hasEffects = value;
					base.OnPropertyChangedWithValue(value, "HasEffects");
				}
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x00067E97 File Offset: 0x00066097
		// (set) Token: 0x06001C81 RID: 7297 RVA: 0x00067E9F File Offset: 0x0006609F
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

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06001C82 RID: 7298 RVA: 0x00067EC2 File Offset: 0x000660C2
		// (set) Token: 0x06001C83 RID: 7299 RVA: 0x00067ECA File Offset: 0x000660CA
		[DataSourceProperty]
		public string AssignedMemberEffects
		{
			get
			{
				return this._assignedMemberEffects;
			}
			set
			{
				if (value != this._assignedMemberEffects)
				{
					this._assignedMemberEffects = value;
					base.OnPropertyChangedWithValue<string>(value, "AssignedMemberEffects");
				}
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06001C84 RID: 7300 RVA: 0x00067EED File Offset: 0x000660ED
		// (set) Token: 0x06001C85 RID: 7301 RVA: 0x00067EF5 File Offset: 0x000660F5
		[DataSourceProperty]
		public string NoEffectText
		{
			get
			{
				return this._noEffectText;
			}
			set
			{
				if (value != this._noEffectText)
				{
					this._noEffectText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoEffectText");
				}
			}
		}

		// Token: 0x04000CFB RID: 3323
		private Action<ClanRoleItemVM> _onRoleSelectionToggled;

		// Token: 0x04000CFC RID: 3324
		private MBBindingList<ClanPartyMemberItemVM> _heroMembers;

		// Token: 0x04000CFD RID: 3325
		private MobileParty _party;

		// Token: 0x04000CFE RID: 3326
		private bool _isEnabled;

		// Token: 0x04000CFF RID: 3327
		private ClanPartyMemberItemVM _effectiveOwner;

		// Token: 0x04000D00 RID: 3328
		private HintViewModel _notAssignedHint;

		// Token: 0x04000D01 RID: 3329
		private HintViewModel _disabledHint;

		// Token: 0x04000D02 RID: 3330
		private bool _isNotAssigned;

		// Token: 0x04000D03 RID: 3331
		private bool _hasEffects;

		// Token: 0x04000D04 RID: 3332
		private string _roleId;

		// Token: 0x04000D05 RID: 3333
		private string _name;

		// Token: 0x04000D06 RID: 3334
		private string _assignedMemberEffects;

		// Token: 0x04000D07 RID: 3335
		private string _noEffectText;
	}
}
