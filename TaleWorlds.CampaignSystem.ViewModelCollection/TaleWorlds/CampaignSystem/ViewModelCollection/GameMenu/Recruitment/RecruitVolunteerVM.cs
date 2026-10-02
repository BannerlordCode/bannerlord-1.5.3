using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Recruitment
{
	// Token: 0x020000B9 RID: 185
	public class RecruitVolunteerVM : ViewModel
	{
		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x060011C4 RID: 4548 RVA: 0x00047097 File Offset: 0x00045297
		// (set) Token: 0x060011C5 RID: 4549 RVA: 0x0004709F File Offset: 0x0004529F
		public Hero OwnerHero { get; private set; }

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x060011C6 RID: 4550 RVA: 0x000470A8 File Offset: 0x000452A8
		// (set) Token: 0x060011C7 RID: 4551 RVA: 0x000470B0 File Offset: 0x000452B0
		public List<CharacterObject> VolunteerTroops { get; private set; }

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x060011C8 RID: 4552 RVA: 0x000470B9 File Offset: 0x000452B9
		public int GoldCost { get; }

		// Token: 0x060011C9 RID: 4553 RVA: 0x000470C4 File Offset: 0x000452C4
		public RecruitVolunteerVM(Hero owner, List<CharacterObject> troops, Action<RecruitVolunteerVM, RecruitVolunteerTroopVM> onRecruit, Action<RecruitVolunteerVM, RecruitVolunteerTroopVM> onRemoveFromCart)
		{
			this.OwnerHero = owner;
			this.VolunteerTroops = troops;
			this._onRecruit = onRecruit;
			this._onRemoveFromCart = onRemoveFromCart;
			this.Owner = new RecruitVolunteerOwnerVM(owner, (int)owner.GetRelationWithPlayer());
			this.Troops = new MBBindingList<RecruitVolunteerTroopVM>();
			int num = 0;
			foreach (CharacterObject characterObject in troops)
			{
				RecruitVolunteerTroopVM recruitVolunteerTroopVM = new RecruitVolunteerTroopVM(this, characterObject, num, new Action<RecruitVolunteerTroopVM>(this.ExecuteRecruit), new Action<RecruitVolunteerTroopVM>(this.ExecuteRemoveFromCart));
				recruitVolunteerTroopVM.CanBeRecruited = false;
				recruitVolunteerTroopVM.PlayerHasEnoughRelation = false;
				if (HeroHelper.HeroCanRecruitFromHero(Hero.MainHero, this.OwnerHero, num))
				{
					recruitVolunteerTroopVM.PlayerHasEnoughRelation = true;
					if (characterObject != null)
					{
						recruitVolunteerTroopVM.CanBeRecruited = true;
					}
				}
				num++;
				this.Troops.Add(recruitVolunteerTroopVM);
			}
			this.RecruitHint = new HintViewModel();
			this.RefreshProperties();
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x000471C4 File Offset: 0x000453C4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RefreshProperties();
			RecruitVolunteerOwnerVM owner = this.Owner;
			if (owner != null)
			{
				owner.RefreshValues();
			}
			this.Troops.ApplyActionOnAllItems(delegate(RecruitVolunteerTroopVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x00047218 File Offset: 0x00045418
		public void ExecuteRecruit(RecruitVolunteerTroopVM troop)
		{
			this._onRecruit(this, troop);
			this.RefreshProperties();
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0004722D File Offset: 0x0004542D
		public void ExecuteRemoveFromCart(RecruitVolunteerTroopVM troop)
		{
			this._onRemoveFromCart(this, troop);
			this.RefreshProperties();
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00047244 File Offset: 0x00045444
		private void RefreshProperties()
		{
			this.RecruitText = this.GoldCost.ToString();
			if (this.RecruitableNumber == 0)
			{
				this.QuantityText = GameTexts.FindText("str_none", null).ToString();
				return;
			}
			GameTexts.SetVariable("QUANTITY", this.RecruitableNumber.ToString());
			this.QuantityText = GameTexts.FindText("str_x_quantity", null).ToString();
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x000472B0 File Offset: 0x000454B0
		public void OnRecruitMoveToCart(RecruitVolunteerTroopVM troop)
		{
			MBInformationManager.HideInformations();
			this.Troops.RemoveAt(troop.Index);
			RecruitVolunteerTroopVM recruitVolunteerTroopVM = new RecruitVolunteerTroopVM(this, null, troop.Index, new Action<RecruitVolunteerTroopVM>(this.ExecuteRecruit), new Action<RecruitVolunteerTroopVM>(this.ExecuteRemoveFromCart));
			recruitVolunteerTroopVM.IsTroopEmpty = true;
			recruitVolunteerTroopVM.PlayerHasEnoughRelation = true;
			this.Troops.Insert(troop.Index, recruitVolunteerTroopVM);
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x00047319 File Offset: 0x00045519
		public void OnRecruitRemovedFromCart(RecruitVolunteerTroopVM troop)
		{
			this.Troops.RemoveAt(troop.Index);
			this.Troops.Insert(troop.Index, troop);
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x0004733E File Offset: 0x0004553E
		// (set) Token: 0x060011D1 RID: 4561 RVA: 0x00047346 File Offset: 0x00045546
		[DataSourceProperty]
		public MBBindingList<RecruitVolunteerTroopVM> Troops
		{
			get
			{
				return this._troops;
			}
			set
			{
				if (value != this._troops)
				{
					this._troops = value;
					base.OnPropertyChangedWithValue<MBBindingList<RecruitVolunteerTroopVM>>(value, "Troops");
				}
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x00047364 File Offset: 0x00045564
		// (set) Token: 0x060011D3 RID: 4563 RVA: 0x0004736C File Offset: 0x0004556C
		[DataSourceProperty]
		public RecruitVolunteerOwnerVM Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				if (value != this._owner)
				{
					this._owner = value;
					base.OnPropertyChangedWithValue<RecruitVolunteerOwnerVM>(value, "Owner");
				}
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x0004738A File Offset: 0x0004558A
		// (set) Token: 0x060011D5 RID: 4565 RVA: 0x00047392 File Offset: 0x00045592
		[DataSourceProperty]
		public bool CanRecruit
		{
			get
			{
				return this._canRecruit;
			}
			set
			{
				if (value != this._canRecruit)
				{
					this._canRecruit = value;
					base.OnPropertyChangedWithValue(value, "CanRecruit");
				}
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x060011D6 RID: 4566 RVA: 0x000473B0 File Offset: 0x000455B0
		// (set) Token: 0x060011D7 RID: 4567 RVA: 0x000473B8 File Offset: 0x000455B8
		[DataSourceProperty]
		public bool ButtonIsVisible
		{
			get
			{
				return this._buttonIsVisible;
			}
			set
			{
				if (value != this._buttonIsVisible)
				{
					this._buttonIsVisible = value;
					base.OnPropertyChangedWithValue(value, "ButtonIsVisible");
				}
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x060011D8 RID: 4568 RVA: 0x000473D6 File Offset: 0x000455D6
		// (set) Token: 0x060011D9 RID: 4569 RVA: 0x000473DE File Offset: 0x000455DE
		[DataSourceProperty]
		public string QuantityText
		{
			get
			{
				return this._quantityText;
			}
			set
			{
				if (value != this._quantityText)
				{
					this._quantityText = value;
					base.OnPropertyChangedWithValue<string>(value, "QuantityText");
				}
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x00047401 File Offset: 0x00045601
		// (set) Token: 0x060011DB RID: 4571 RVA: 0x00047409 File Offset: 0x00045609
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

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x0004742C File Offset: 0x0004562C
		// (set) Token: 0x060011DD RID: 4573 RVA: 0x00047434 File Offset: 0x00045634
		[DataSourceProperty]
		public HintViewModel RecruitHint
		{
			get
			{
				return this._recruitHint;
			}
			set
			{
				if (value != this._recruitHint)
				{
					this._recruitHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RecruitHint");
				}
			}
		}

		// Token: 0x04000816 RID: 2070
		public int RecruitableNumber;

		// Token: 0x04000817 RID: 2071
		private readonly Action<RecruitVolunteerVM, RecruitVolunteerTroopVM> _onRecruit;

		// Token: 0x04000818 RID: 2072
		private readonly Action<RecruitVolunteerVM, RecruitVolunteerTroopVM> _onRemoveFromCart;

		// Token: 0x04000819 RID: 2073
		private string _quantityText;

		// Token: 0x0400081A RID: 2074
		private string _recruitText;

		// Token: 0x0400081B RID: 2075
		private bool _canRecruit;

		// Token: 0x0400081C RID: 2076
		private bool _buttonIsVisible;

		// Token: 0x0400081D RID: 2077
		private HintViewModel _recruitHint;

		// Token: 0x0400081E RID: 2078
		private RecruitVolunteerOwnerVM _owner;

		// Token: 0x0400081F RID: 2079
		private MBBindingList<RecruitVolunteerTroopVM> _troops;
	}
}
