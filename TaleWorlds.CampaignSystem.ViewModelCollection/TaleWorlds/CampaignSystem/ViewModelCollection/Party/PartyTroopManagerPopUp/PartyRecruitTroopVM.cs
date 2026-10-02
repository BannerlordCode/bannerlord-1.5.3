using System;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party.PartyTroopManagerPopUp
{
	// Token: 0x02000032 RID: 50
	public class PartyRecruitTroopVM : PartyTroopManagerVM
	{
		// Token: 0x060004F5 RID: 1269 RVA: 0x0001C6E0 File Offset: 0x0001A8E0
		public PartyRecruitTroopVM(PartyVM partyVM)
			: base(partyVM)
		{
			this.RefreshValues();
			base.IsUpgradePopUp = false;
			this._openButtonEnabledHint = new TextObject("{=tnbCJyax}Some of your prisoners are recruitable.", null);
			this._openButtonNoTroopsHint = new TextObject("{=1xf8rHLH}You don't have any recruitable prisoners.", null);
			this._openButtonIrrelevantScreenHint = new TextObject("{=zduu7dpz}Prisoners are not recruitable in this screen.", null);
			this._openButtonUpgradesDisabledHint = new TextObject("{=HfsUngkh}Recruitment is currently disabled.", null);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001C748 File Offset: 0x0001A948
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.TitleText = new TextObject("{=b8CqpGHx}Recruit Prisoners", null).ToString();
			this.EffectText = new TextObject("{=opVqBNLh}Effect", null).ToString();
			this.RecruitText = new TextObject("{=recruitVerb}Recruit", null).ToString();
			this.RecruitAllText = new TextObject("{=YJaNtktT}Recruit All", null).ToString();
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001C7B4 File Offset: 0x0001A9B4
		public void OnTroopRecruited(PartyCharacterVM recruitedCharacter)
		{
			if (!base.IsOpen)
			{
				return;
			}
			this._hasMadeChanges = true;
			PartyTroopManagerItemVM partyTroopManagerItemVM = base.Troops.FirstOrDefault<PartyTroopManagerItemVM>((PartyTroopManagerItemVM x) => x.PartyCharacter == recruitedCharacter);
			recruitedCharacter.UpdateRecruitable();
			if (!recruitedCharacter.IsTroopRecruitable)
			{
				base.Troops.Remove(partyTroopManagerItemVM);
			}
			base.UpdateLabels();
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001C821 File Offset: 0x0001AA21
		public override void OpenPopUp()
		{
			base.OpenPopUp();
			this.PopulateTroops();
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001C82F File Offset: 0x0001AA2F
		public override void ExecuteDone()
		{
			base.ExecuteDone();
			this._partyVM.OnRecruitPopUpClosed(false);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001C843 File Offset: 0x0001AA43
		public override void ExecuteCancel()
		{
			base.ShowCancelInquiry(new Action(this.ConfirmCancel));
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001C858 File Offset: 0x0001AA58
		protected override void ConfirmCancel()
		{
			base.ConfirmCancel();
			this._partyVM.OnRecruitPopUpClosed(true);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001C86C File Offset: 0x0001AA6C
		private void PopulateTroops()
		{
			base.Troops = new MBBindingList<PartyTroopManagerItemVM>();
			foreach (PartyCharacterVM partyCharacterVM in this._partyVM.MainPartyPrisoners)
			{
				if (partyCharacterVM.IsTroopRecruitable)
				{
					base.Troops.Add(new PartyTroopManagerItemVM(partyCharacterVM, new Action<PartyTroopManagerItemVM>(base.SetFocusedCharacter)));
				}
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001C8E8 File Offset: 0x0001AAE8
		public override void ExecuteItemPrimaryAction()
		{
			PartyTroopManagerItemVM focusedTroop = base.FocusedTroop;
			if (focusedTroop == null)
			{
				return;
			}
			focusedTroop.PartyCharacter.ExecuteRecruitTroop();
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001C900 File Offset: 0x0001AB00
		public void ExecuteRecruitAll()
		{
			for (int i = base.Troops.Count - 1; i >= 0; i--)
			{
				PartyCharacterVM partyCharacter = base.Troops[i].PartyCharacter;
				if (partyCharacter != null)
				{
					partyCharacter.RecruitAll();
				}
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0001C941 File Offset: 0x0001AB41
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x0001C949 File Offset: 0x0001AB49
		[DataSourceProperty]
		public string EffectText
		{
			get
			{
				return this._effectText;
			}
			set
			{
				if (value != this._effectText)
				{
					this._effectText = value;
					base.OnPropertyChangedWithValue<string>(value, "EffectText");
				}
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0001C96C File Offset: 0x0001AB6C
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x0001C974 File Offset: 0x0001AB74
		[DataSourceProperty]
		public string RecruitText
		{
			get
			{
				return this._recruitText;
			}
			set
			{
				if (value != this._recruitText)
				{
					this._recruitText = value;
					base.OnPropertyChangedWithValue<string>(value, "RecruitText");
				}
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x0001C997 File Offset: 0x0001AB97
		// (set) Token: 0x06000504 RID: 1284 RVA: 0x0001C99F File Offset: 0x0001AB9F
		[DataSourceProperty]
		public string RecruitAllText
		{
			get
			{
				return this._recruitAllText;
			}
			set
			{
				if (value != this._recruitAllText)
				{
					this._recruitAllText = value;
					base.OnPropertyChangedWithValue<string>(value, "RecruitAllText");
				}
			}
		}

		// Token: 0x04000227 RID: 551
		private string _effectText;

		// Token: 0x04000228 RID: 552
		private string _recruitText;

		// Token: 0x04000229 RID: 553
		private string _recruitAllText;
	}
}
