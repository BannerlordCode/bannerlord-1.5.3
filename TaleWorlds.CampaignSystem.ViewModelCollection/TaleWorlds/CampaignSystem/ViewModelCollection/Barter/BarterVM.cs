using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Helpers;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Barter
{
	// Token: 0x02000160 RID: 352
	public class BarterVM : ViewModel
	{
		// Token: 0x060021C2 RID: 8642 RVA: 0x00078850 File Offset: 0x00076A50
		public BarterVM(BarterData args)
		{
			this._barterData = args;
			if (this._barterData.OtherHero == Hero.MainHero)
			{
				this._otherParty = this._barterData.OffererParty;
				this._otherCharacter = this._barterData.OffererHero.CharacterObject ?? CampaignUIHelper.GetVisualPartyLeader(this._otherParty);
			}
			else if (this._barterData.OtherHero != null)
			{
				this._otherCharacter = this._barterData.OtherHero.CharacterObject;
				this.LeftMaxGold = this._otherCharacter.HeroObject.Gold;
			}
			else
			{
				this._otherParty = this._barterData.OtherParty;
				this._otherCharacter = CampaignUIHelper.GetVisualPartyLeader(this._otherParty);
				this.LeftMaxGold = this._otherParty.MobileParty.PartyTradeGold;
			}
			this._barter = Campaign.Current.BarterManager;
			this._isPlayerOfferer = this._barterData.OffererHero == Hero.MainHero;
			this.AutoBalanceHint = new HintViewModel();
			this.LeftFiefList = new MBBindingList<BarterItemVM>();
			this.RightFiefList = new MBBindingList<BarterItemVM>();
			this.LeftPrisonerList = new MBBindingList<BarterItemVM>();
			this.RightPrisonerList = new MBBindingList<BarterItemVM>();
			this.LeftItemList = new MBBindingList<BarterItemVM>();
			this.RightItemList = new MBBindingList<BarterItemVM>();
			this.LeftOtherList = new MBBindingList<BarterItemVM>();
			this.RightOtherList = new MBBindingList<BarterItemVM>();
			this.LeftDiplomaticList = new MBBindingList<BarterItemVM>();
			this.RightDiplomaticList = new MBBindingList<BarterItemVM>();
			this.LeftGoldList = new MBBindingList<BarterItemVM>();
			this.RightGoldList = new MBBindingList<BarterItemVM>();
			this._leftList = new Dictionary<BarterGroup, MBBindingList<BarterItemVM>>();
			this._rightList = new Dictionary<BarterGroup, MBBindingList<BarterItemVM>>();
			this._barterList = new List<Dictionary<BarterGroup, MBBindingList<BarterItemVM>>>();
			this._offerList = new List<MBBindingList<BarterItemVM>>();
			this.LeftOfferList = new MBBindingList<BarterItemVM>();
			this.RightOfferList = new MBBindingList<BarterItemVM>();
			this.InitBarterList(this._barterData);
			this.OnInitialized();
			this.RightMaxGold = Hero.MainHero.Gold;
			this.LeftHero = new HeroVM(this._otherCharacter.HeroObject, false);
			this.RightHero = new HeroVM(Hero.MainHero, false);
			this.SendOffer();
			this.InitializationIsOver = true;
			this.RefreshValues();
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x00078A8C File Offset: 0x00076C8C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.InitializeStaticContent();
			this.LeftNameLbl = this._otherCharacter.Name.ToString();
			this.RightNameLbl = Hero.MainHero.Name.ToString();
			this.LeftFiefList.ApplyActionOnAllItems(delegate(BarterItemVM x)
			{
				x.RefreshValues();
			});
			this.RightFiefList.ApplyActionOnAllItems(delegate(BarterItemVM x)
			{
				x.RefreshValues();
			});
			this.LeftPrisonerList.ApplyActionOnAllItems(delegate(BarterItemVM x)
			{
				x.RefreshValues();
			});
			this.RightPrisonerList.ApplyActionOnAllItems(delegate(BarterItemVM x)
			{
				x.RefreshValues();
			});
			this.LeftItemList.ApplyActionOnAllItems(delegate(BarterItemVM x)
			{
				x.RefreshValues();
			});
			this.RightItemList.ApplyActionOnAllItems(delegate(BarterItemVM x)
			{
				x.RefreshValues();
			});
			this.LeftOtherList.ApplyActionOnAllItems(delegate(BarterItemVM x)
			{
				x.RefreshValues();
			});
			this.RightOtherList.ApplyActionOnAllItems(delegate(BarterItemVM x)
			{
				x.RefreshValues();
			});
			this.LeftDiplomaticList.ApplyActionOnAllItems(delegate(BarterItemVM x)
			{
				x.RefreshValues();
			});
			this.RightDiplomaticList.ApplyActionOnAllItems(delegate(BarterItemVM x)
			{
				x.RefreshValues();
			});
			this.LeftGoldList.ApplyActionOnAllItems(delegate(BarterItemVM x)
			{
				x.RefreshValues();
			});
			this.RightGoldList.ApplyActionOnAllItems(delegate(BarterItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x00078CC8 File Offset: 0x00076EC8
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.DoneInputKey.OnFinalize();
			this.CancelInputKey.OnFinalize();
			this.ResetInputKey.OnFinalize();
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x00078CF4 File Offset: 0x00076EF4
		private void InitBarterList(BarterData args)
		{
			this._leftList.Add(args.GetBarterGroup<FiefBarterGroup>(), this.LeftFiefList);
			this._leftList.Add(args.GetBarterGroup<PrisonerBarterGroup>(), this.LeftPrisonerList);
			this._leftList.Add(args.GetBarterGroup<ItemBarterGroup>(), this.LeftItemList);
			this._leftList.Add(args.GetBarterGroup<OtherBarterGroup>(), this.LeftOtherList);
			this._leftList.Add(args.GetBarterGroup<GoldBarterGroup>(), this.LeftGoldList);
			this._rightList.Add(args.GetBarterGroup<FiefBarterGroup>(), this.RightFiefList);
			this._rightList.Add(args.GetBarterGroup<PrisonerBarterGroup>(), this.RightPrisonerList);
			this._rightList.Add(args.GetBarterGroup<ItemBarterGroup>(), this.RightItemList);
			this._rightList.Add(args.GetBarterGroup<OtherBarterGroup>(), this.RightOtherList);
			this._rightList.Add(args.GetBarterGroup<GoldBarterGroup>(), this.RightGoldList);
			this._barterList.Add(this._leftList);
			this._barterList.Add(this._rightList);
			this._offerList.Add(this.LeftOfferList);
			this._offerList.Add(this.RightOfferList);
			if (this._barterData.ContextInitializer != null)
			{
				foreach (Barterable barterable in this._barterData.GetBarterables())
				{
					if (barterable.IsContextDependent && this._barterData.ContextInitializer(barterable, this._barterData, null))
					{
						this.ChangeBarterableIsOffered(barterable, true);
					}
				}
			}
			foreach (Barterable barterable2 in args.GetBarterables())
			{
				if (!barterable2.IsOffered && !barterable2.IsContextDependent)
				{
					this._barterList[(barterable2.OriginalOwner == Hero.MainHero) ? 1 : 0][barterable2.Group].Add(new BarterItemVM(barterable2, new BarterItemVM.BarterTransferEventDelegate(this.TransferItem), new Action(this.OnOfferedAmountChange), false));
				}
				else
				{
					BarterItemVM barterItemVM = new BarterItemVM(barterable2, new BarterItemVM.BarterTransferEventDelegate(this.TransferItem), new Action(this.OnOfferedAmountChange), barterable2.IsContextDependent);
					this._offerList[(barterable2.OriginalOwner == Hero.MainHero) ? 1 : 0].Add(barterItemVM);
					this.RefreshCompatibility(barterItemVM, true);
				}
			}
			this._barterData.GetBarterables().Find((Barterable t) => t.Group.GetType() == typeof(GoldBarterGroup) && t.OriginalOwner == Hero.MainHero);
			this._barterData.GetBarterables().Find((Barterable t) => (t.Group.GetType() == typeof(GoldBarterGroup) && this._barterData.OffererHero == Hero.MainHero && t.OriginalOwner == this._barterData.OtherHero) || (this._barterData.OtherHero == Hero.MainHero && t.OriginalOwner == this._barterData.OffererHero));
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x00078FE0 File Offset: 0x000771E0
		private void ChangeBarterableIsOffered(Barterable barterable, bool newState)
		{
			if (barterable.IsOffered != newState)
			{
				barterable.SetIsOffered(newState);
				this.OnTransferItem(barterable, true);
				foreach (Barterable barterable2 in barterable.LinkedBarterables)
				{
					this.OnTransferItem(barterable2, true);
				}
			}
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x0007904C File Offset: 0x0007724C
		public void OnInitialized()
		{
			BarterManager barterManager = Campaign.Current.BarterManager;
			barterManager.Closed = (BarterManager.BarterCloseEventDelegate)Delegate.Combine(barterManager.Closed, new BarterManager.BarterCloseEventDelegate(this.OnClosed));
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x00079079 File Offset: 0x00077279
		private void OnClosed()
		{
			BarterManager barterManager = Campaign.Current.BarterManager;
			barterManager.Closed = (BarterManager.BarterCloseEventDelegate)Delegate.Remove(barterManager.Closed, new BarterManager.BarterCloseEventDelegate(this.OnClosed));
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000790A6 File Offset: 0x000772A6
		public void ExecuteTransferAllLeftFief()
		{
			this.ExecuteTransferAll(this._otherCharacter, this._barterData.GetBarterGroup<FiefBarterGroup>());
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x000790BF File Offset: 0x000772BF
		public void ExecuteAutoBalance()
		{
			this.AutoBalanceAdd();
			this.AutoBalanceRemove();
			this.AutoBalanceAdd();
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x000790D4 File Offset: 0x000772D4
		private void AutoBalanceRemove()
		{
			if ((int)Campaign.Current.BarterManager.GetOfferValue(this._otherCharacter.HeroObject, this._otherParty, this._barterData.OffererParty, this._barterData.GetOfferedBarterables()) > 0)
			{
				List<ValueTuple<Barterable, int>> list = BarterHelper.GetAutoBalanceBarterablesToRemove(this._barterData, this.OtherFaction, Clan.PlayerClan.MapFaction, Hero.MainHero).ToList<ValueTuple<Barterable, int>>();
				List<ValueTuple<BarterItemVM, int>> list2 = new List<ValueTuple<BarterItemVM, int>>();
				this.GetBarterItems(this.RightGoldList, list, list2);
				this.GetBarterItems(this.RightItemList, list, list2);
				this.GetBarterItems(this.RightPrisonerList, list, list2);
				this.GetBarterItems(this.RightFiefList, list, list2);
				foreach (ValueTuple<BarterItemVM, int> valueTuple in list2)
				{
					BarterItemVM item = valueTuple.Item1;
					int item2 = valueTuple.Item2;
					this.OfferItemRemove(item, item2);
				}
			}
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x000791D4 File Offset: 0x000773D4
		private void AutoBalanceAdd()
		{
			if ((int)Campaign.Current.BarterManager.GetOfferValue(this._otherCharacter.HeroObject, this._otherParty, this._barterData.OffererParty, this._barterData.GetOfferedBarterables()) < 0)
			{
				List<ValueTuple<Barterable, int>> list = BarterHelper.GetAutoBalanceBarterablesAdd(this._barterData, this.OtherFaction, Clan.PlayerClan.MapFaction, Hero.MainHero, 1f).ToList<ValueTuple<Barterable, int>>();
				List<ValueTuple<BarterItemVM, int>> list2 = new List<ValueTuple<BarterItemVM, int>>();
				this.GetBarterItems(this.RightGoldList, list, list2);
				this.GetBarterItems(this.RightItemList, list, list2);
				this.GetBarterItems(this.RightPrisonerList, list, list2);
				this.GetBarterItems(this.RightFiefList, list, list2);
				foreach (ValueTuple<BarterItemVM, int> valueTuple in list2)
				{
					BarterItemVM item = valueTuple.Item1;
					int item2 = valueTuple.Item2;
					if (item2 > 0)
					{
						this.OfferItemAdd(item, item2);
					}
				}
			}
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x000792DC File Offset: 0x000774DC
		private void GetBarterItems(MBBindingList<BarterItemVM> itemList, [TupleElementNames(new string[] { "barterable", "count" })] List<ValueTuple<Barterable, int>> newBarterables, List<ValueTuple<BarterItemVM, int>> barterItems)
		{
			foreach (BarterItemVM barterItemVM in itemList)
			{
				foreach (ValueTuple<Barterable, int> valueTuple in newBarterables)
				{
					Barterable item = valueTuple.Item1;
					int item2 = valueTuple.Item2;
					if (item == barterItemVM.Barterable)
					{
						barterItems.Add(new ValueTuple<BarterItemVM, int>(barterItemVM, item2));
					}
				}
			}
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x00079378 File Offset: 0x00077578
		public void ExecuteTransferAllLeftItem()
		{
			this.ExecuteTransferAll(this._otherCharacter, this._barterData.GetBarterGroup<ItemBarterGroup>());
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x00079391 File Offset: 0x00077591
		public void ExecuteTransferAllLeftPrisoner()
		{
			this.ExecuteTransferAll(this._otherCharacter, this._barterData.GetBarterGroup<PrisonerBarterGroup>());
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x000793AA File Offset: 0x000775AA
		public void ExecuteTransferAllLeftOther()
		{
			this.ExecuteTransferAll(this._otherCharacter, this._barterData.GetBarterGroup<OtherBarterGroup>());
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x000793C3 File Offset: 0x000775C3
		public void ExecuteTransferAllRightFief()
		{
			this.ExecuteTransferAll(CharacterObject.PlayerCharacter, this._barterData.GetBarterGroup<FiefBarterGroup>());
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x000793DB File Offset: 0x000775DB
		public void ExecuteTransferAllRightItem()
		{
			this.ExecuteTransferAll(CharacterObject.PlayerCharacter, this._barterData.GetBarterGroup<ItemBarterGroup>());
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000793F3 File Offset: 0x000775F3
		public void ExecuteTransferAllRightPrisoner()
		{
			this.ExecuteTransferAll(CharacterObject.PlayerCharacter, this._barterData.GetBarterGroup<PrisonerBarterGroup>());
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x0007940B File Offset: 0x0007760B
		public void ExecuteTransferAllRightOther()
		{
			this.ExecuteTransferAll(CharacterObject.PlayerCharacter, this._barterData.GetBarterGroup<OtherBarterGroup>());
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x00079424 File Offset: 0x00077624
		private void ExecuteTransferAll(CharacterObject fromCharacter, BarterGroup barterGroup)
		{
			if (barterGroup != null)
			{
				foreach (BarterItemVM barterItemVM in new List<BarterItemVM>(this._barterList[(fromCharacter == CharacterObject.PlayerCharacter) ? 1 : 0][barterGroup].Where<BarterItemVM>((BarterItemVM barterItem) => !barterItem.Barterable.IsOffered)))
				{
					this.TransferItem(barterItemVM, true);
				}
				foreach (BarterItemVM barterItemVM2 in this._barterList[(fromCharacter == CharacterObject.PlayerCharacter) ? 1 : 0][barterGroup])
				{
					barterItemVM2.CurrentOfferedAmount = barterItemVM2.TotalItemCount;
				}
			}
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x00079514 File Offset: 0x00077714
		private void SendOffer()
		{
			this.IsOfferDisabled = !this.IsCurrentOfferAcceptable() || (this.LeftOfferList.Count == 0 && this.RightOfferList.Count == 0);
			this.RefreshResultBar();
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x0007954B File Offset: 0x0007774B
		private bool IsCurrentOfferAcceptable()
		{
			return Campaign.Current.BarterManager.IsOfferAcceptable(this._barterData, this._otherCharacter.HeroObject, this._otherParty);
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x060021D8 RID: 8664 RVA: 0x00079574 File Offset: 0x00077774
		private IFaction OtherFaction
		{
			get
			{
				if (!this._otherCharacter.IsHero)
				{
					return this._otherParty.MapFaction;
				}
				return this._otherCharacter.HeroObject.Clan;
			}
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x000795AC File Offset: 0x000777AC
		private void RefreshResultBar()
		{
			long num = 0L;
			long num2 = 0L;
			IFaction otherFaction = this.OtherFaction;
			foreach (BarterItemVM barterItemVM in this.LeftOfferList)
			{
				int valueForFaction = barterItemVM.Barterable.GetValueForFaction(otherFaction);
				if (valueForFaction < 0)
				{
					num2 += (long)valueForFaction;
				}
				else
				{
					num += (long)valueForFaction;
				}
			}
			foreach (BarterItemVM barterItemVM2 in this.RightOfferList)
			{
				int valueForFaction2 = barterItemVM2.Barterable.GetValueForFaction(otherFaction);
				if (valueForFaction2 < 0)
				{
					num2 += (long)valueForFaction2;
				}
				else
				{
					num += (long)valueForFaction2;
				}
			}
			double num3 = (double)MathF.Max(0f, (float)num);
			double num4 = (double)MathF.Max(1f, (float)(-(float)num2));
			this.ResultBarOtherPercentage = MathF.Round(num3 / num4 * 100.0);
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x000796B4 File Offset: 0x000778B4
		private void ExecuteTransferAllGoldLeft()
		{
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x000796B6 File Offset: 0x000778B6
		private void ExecuteTransferAllGoldRight()
		{
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x000796B8 File Offset: 0x000778B8
		public void ExecuteOffer()
		{
			Campaign.Current.BarterManager.ApplyAndFinalizePlayerBarter(this._barterData.OffererHero, this._barterData.OtherHero, this._barterData);
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x000796E5 File Offset: 0x000778E5
		public void ExecuteCancel()
		{
			Campaign.Current.BarterManager.CancelAndFinalizePlayerBarter(this._barterData.OffererHero, this._barterData.OtherHero, this._barterData);
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x00079714 File Offset: 0x00077914
		public void ExecuteReset()
		{
			this.LeftFiefList.Clear();
			this.RightFiefList.Clear();
			this.LeftPrisonerList.Clear();
			this.RightPrisonerList.Clear();
			this.LeftItemList.Clear();
			this.RightItemList.Clear();
			this.LeftOtherList.Clear();
			this.RightOtherList.Clear();
			this.LeftDiplomaticList.Clear();
			this.RightDiplomaticList.Clear();
			this.LeftGoldList.Clear();
			this.RightGoldList.Clear();
			this._leftList.Clear();
			this._rightList.Clear();
			this._barterList.Clear();
			this.LeftOfferList.Clear();
			this.RightOfferList.Clear();
			this._offerList.Clear();
			foreach (Barterable barterable in this._barterData.GetBarterables())
			{
				if (barterable.IsOffered)
				{
					this.ChangeBarterableIsOffered(barterable, false);
				}
			}
			this.InitBarterList(this._barterData);
			this.SendOffer();
			this.InitializationIsOver = true;
			this.RefreshValues();
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x0007985C File Offset: 0x00077A5C
		private void TransferItem(BarterItemVM item, bool offerAll)
		{
			this.ChangeBarterableIsOffered(item.Barterable, !item.IsOffered);
			if (offerAll)
			{
				item.CurrentOfferedAmount = item.TotalItemCount;
			}
			this.SendOffer();
			this.RefreshCompatibility(item, item.IsOffered);
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x00079898 File Offset: 0x00077A98
		private void OfferItemAdd(BarterItemVM barterItemVM, int count)
		{
			this.ChangeBarterableIsOffered(barterItemVM.Barterable, true);
			barterItemVM.CurrentOfferedAmount = (int)MathF.Clamp((float)(barterItemVM.CurrentOfferedAmount + count), 0f, (float)barterItemVM.TotalItemCount);
			this.SendOffer();
			this.RefreshCompatibility(barterItemVM, barterItemVM.IsOffered);
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x000798E8 File Offset: 0x00077AE8
		private void OfferItemRemove(BarterItemVM barterItemVM, int count)
		{
			if (barterItemVM.CurrentOfferedAmount <= count)
			{
				this.ChangeBarterableIsOffered(barterItemVM.Barterable, false);
			}
			else
			{
				barterItemVM.CurrentOfferedAmount = (int)MathF.Clamp((float)(barterItemVM.CurrentOfferedAmount - count), 0f, (float)barterItemVM.TotalItemCount);
			}
			this.SendOffer();
			this.RefreshCompatibility(barterItemVM, barterItemVM.IsOffered);
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x00079944 File Offset: 0x00077B44
		public void OnTransferItem(Barterable barter, bool isTransferrable)
		{
			int num = ((barter.OriginalOwner == Hero.MainHero) ? 1 : 0);
			if (!this._barterList.IsEmpty<Dictionary<BarterGroup, MBBindingList<BarterItemVM>>>())
			{
				BarterItemVM barterItemVM = this._barterList[num][barter.Group].FirstOrDefault<BarterItemVM>((BarterItemVM i) => i.Barterable == barter);
				if (barterItemVM == null && !this._offerList.IsEmpty<MBBindingList<BarterItemVM>>())
				{
					barterItemVM = this._offerList[num].FirstOrDefault<BarterItemVM>((BarterItemVM i) => i.Barterable == barter);
				}
				if (barterItemVM != null)
				{
					barterItemVM.IsOffered = barter.IsOffered;
					barterItemVM.IsItemTransferrable = isTransferrable;
					if (barterItemVM.IsOffered)
					{
						this._offerList[num].Add(barterItemVM);
						if (barterItemVM.IsMultiple)
						{
							barterItemVM.CurrentOfferedAmount = 1;
							return;
						}
					}
					else
					{
						this._offerList[num].Remove(barterItemVM);
						if (barterItemVM.IsMultiple)
						{
							barterItemVM.CurrentOfferedAmount = 1;
						}
					}
				}
			}
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x00079A48 File Offset: 0x00077C48
		private void OnOfferedAmountChange()
		{
			this.SendOffer();
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x00079A50 File Offset: 0x00077C50
		private void RefreshCompatibility(BarterItemVM lastTransferredItem, bool gotOffered)
		{
			Action<BarterItemVM> <>9__0;
			foreach (MBBindingList<BarterItemVM> mbbindingList in this._leftList.Values)
			{
				List<BarterItemVM> list = mbbindingList.ToList<BarterItemVM>();
				Action<BarterItemVM> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(BarterItemVM b)
					{
						b.RefreshCompabilityWithItem(lastTransferredItem, gotOffered);
					});
				}
				list.ForEach(action);
			}
			Action<BarterItemVM> <>9__1;
			foreach (MBBindingList<BarterItemVM> mbbindingList2 in this._rightList.Values)
			{
				List<BarterItemVM> list2 = mbbindingList2.ToList<BarterItemVM>();
				Action<BarterItemVM> action2;
				if ((action2 = <>9__1) == null)
				{
					action2 = (<>9__1 = delegate(BarterItemVM b)
					{
						b.RefreshCompabilityWithItem(lastTransferredItem, gotOffered);
					});
				}
				list2.ForEach(action2);
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x060021E5 RID: 8677 RVA: 0x00079B48 File Offset: 0x00077D48
		// (set) Token: 0x060021E6 RID: 8678 RVA: 0x00079B50 File Offset: 0x00077D50
		[DataSourceProperty]
		public string FiefLbl
		{
			get
			{
				return this._fiefLbl;
			}
			set
			{
				if (value != this._fiefLbl)
				{
					this._fiefLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "FiefLbl");
				}
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x060021E7 RID: 8679 RVA: 0x00079B73 File Offset: 0x00077D73
		// (set) Token: 0x060021E8 RID: 8680 RVA: 0x00079B7B File Offset: 0x00077D7B
		[DataSourceProperty]
		public string PrisonerLbl
		{
			get
			{
				return this._prisonerLbl;
			}
			set
			{
				if (value != this._prisonerLbl)
				{
					this._prisonerLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "PrisonerLbl");
				}
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x060021E9 RID: 8681 RVA: 0x00079B9E File Offset: 0x00077D9E
		// (set) Token: 0x060021EA RID: 8682 RVA: 0x00079BA6 File Offset: 0x00077DA6
		[DataSourceProperty]
		public string ItemLbl
		{
			get
			{
				return this._itemLbl;
			}
			set
			{
				if (value != this._itemLbl)
				{
					this._itemLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "ItemLbl");
				}
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x060021EB RID: 8683 RVA: 0x00079BC9 File Offset: 0x00077DC9
		// (set) Token: 0x060021EC RID: 8684 RVA: 0x00079BD1 File Offset: 0x00077DD1
		[DataSourceProperty]
		public string OtherLbl
		{
			get
			{
				return this._otherLbl;
			}
			set
			{
				if (value != this._otherLbl)
				{
					this._otherLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "OtherLbl");
				}
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x060021ED RID: 8685 RVA: 0x00079BF4 File Offset: 0x00077DF4
		// (set) Token: 0x060021EE RID: 8686 RVA: 0x00079BFC File Offset: 0x00077DFC
		[DataSourceProperty]
		public string CancelLbl
		{
			get
			{
				return this._cancelLbl;
			}
			set
			{
				if (value != this._cancelLbl)
				{
					this._cancelLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelLbl");
				}
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x060021EF RID: 8687 RVA: 0x00079C1F File Offset: 0x00077E1F
		// (set) Token: 0x060021F0 RID: 8688 RVA: 0x00079C27 File Offset: 0x00077E27
		[DataSourceProperty]
		public string ResetLbl
		{
			get
			{
				return this._resetLbl;
			}
			set
			{
				if (value != this._resetLbl)
				{
					this._resetLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "ResetLbl");
				}
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x060021F1 RID: 8689 RVA: 0x00079C4A File Offset: 0x00077E4A
		// (set) Token: 0x060021F2 RID: 8690 RVA: 0x00079C52 File Offset: 0x00077E52
		[DataSourceProperty]
		public string OfferLbl
		{
			get
			{
				return this._offerLbl;
			}
			set
			{
				if (value != this._offerLbl)
				{
					this._offerLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "OfferLbl");
				}
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x060021F3 RID: 8691 RVA: 0x00079C75 File Offset: 0x00077E75
		// (set) Token: 0x060021F4 RID: 8692 RVA: 0x00079C7D File Offset: 0x00077E7D
		[DataSourceProperty]
		public string DiplomaticLbl
		{
			get
			{
				return this._diplomaticLbl;
			}
			set
			{
				if (value != this._diplomaticLbl)
				{
					this._diplomaticLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "DiplomaticLbl");
				}
			}
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x060021F5 RID: 8693 RVA: 0x00079CA0 File Offset: 0x00077EA0
		// (set) Token: 0x060021F6 RID: 8694 RVA: 0x00079CA8 File Offset: 0x00077EA8
		[DataSourceProperty]
		public HintViewModel AutoBalanceHint
		{
			get
			{
				return this._autoBalanceHint;
			}
			set
			{
				if (value != this._autoBalanceHint)
				{
					this._autoBalanceHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AutoBalanceHint");
				}
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x060021F7 RID: 8695 RVA: 0x00079CC6 File Offset: 0x00077EC6
		// (set) Token: 0x060021F8 RID: 8696 RVA: 0x00079CCE File Offset: 0x00077ECE
		[DataSourceProperty]
		public HeroVM LeftHero
		{
			get
			{
				return this._leftHero;
			}
			set
			{
				if (value != this._leftHero)
				{
					this._leftHero = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "LeftHero");
				}
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x060021F9 RID: 8697 RVA: 0x00079CEC File Offset: 0x00077EEC
		// (set) Token: 0x060021FA RID: 8698 RVA: 0x00079CF4 File Offset: 0x00077EF4
		[DataSourceProperty]
		public HeroVM RightHero
		{
			get
			{
				return this._rightHero;
			}
			set
			{
				if (value != this._rightHero)
				{
					this._rightHero = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "RightHero");
				}
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x060021FB RID: 8699 RVA: 0x00079D12 File Offset: 0x00077F12
		// (set) Token: 0x060021FC RID: 8700 RVA: 0x00079D1A File Offset: 0x00077F1A
		[DataSourceProperty]
		public bool IsOfferDisabled
		{
			get
			{
				return this._isOfferDisabled;
			}
			set
			{
				if (value != this._isOfferDisabled)
				{
					this._isOfferDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsOfferDisabled");
				}
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x060021FD RID: 8701 RVA: 0x00079D38 File Offset: 0x00077F38
		// (set) Token: 0x060021FE RID: 8702 RVA: 0x00079D40 File Offset: 0x00077F40
		[DataSourceProperty]
		public int LeftMaxGold
		{
			get
			{
				return this._leftMaxGold;
			}
			set
			{
				if (value != this._leftMaxGold)
				{
					this._leftMaxGold = value;
					base.OnPropertyChangedWithValue(value, "LeftMaxGold");
				}
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x060021FF RID: 8703 RVA: 0x00079D5E File Offset: 0x00077F5E
		// (set) Token: 0x06002200 RID: 8704 RVA: 0x00079D66 File Offset: 0x00077F66
		[DataSourceProperty]
		public int RightMaxGold
		{
			get
			{
				return this._rightMaxGold;
			}
			set
			{
				if (value != this._rightMaxGold)
				{
					this._rightMaxGold = value;
					base.OnPropertyChangedWithValue(value, "RightMaxGold");
				}
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06002201 RID: 8705 RVA: 0x00079D84 File Offset: 0x00077F84
		// (set) Token: 0x06002202 RID: 8706 RVA: 0x00079D8C File Offset: 0x00077F8C
		[DataSourceProperty]
		public string LeftNameLbl
		{
			get
			{
				return this._leftNameLbl;
			}
			set
			{
				if (value != this._leftNameLbl)
				{
					this._leftNameLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "LeftNameLbl");
				}
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06002203 RID: 8707 RVA: 0x00079DAF File Offset: 0x00077FAF
		// (set) Token: 0x06002204 RID: 8708 RVA: 0x00079DB7 File Offset: 0x00077FB7
		[DataSourceProperty]
		public string RightNameLbl
		{
			get
			{
				return this._rightNameLbl;
			}
			set
			{
				if (value != this._rightNameLbl)
				{
					this._rightNameLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "RightNameLbl");
				}
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06002205 RID: 8709 RVA: 0x00079DDA File Offset: 0x00077FDA
		// (set) Token: 0x06002206 RID: 8710 RVA: 0x00079DE2 File Offset: 0x00077FE2
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> LeftFiefList
		{
			get
			{
				return this._leftFiefList;
			}
			set
			{
				if (value != this._leftFiefList)
				{
					this._leftFiefList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "LeftFiefList");
				}
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06002207 RID: 8711 RVA: 0x00079E00 File Offset: 0x00078000
		// (set) Token: 0x06002208 RID: 8712 RVA: 0x00079E08 File Offset: 0x00078008
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> RightFiefList
		{
			get
			{
				return this._rightFiefList;
			}
			set
			{
				if (value != this._rightFiefList)
				{
					this._rightFiefList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "RightFiefList");
				}
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06002209 RID: 8713 RVA: 0x00079E26 File Offset: 0x00078026
		// (set) Token: 0x0600220A RID: 8714 RVA: 0x00079E2E File Offset: 0x0007802E
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> LeftPrisonerList
		{
			get
			{
				return this._leftPrisonerList;
			}
			set
			{
				if (value != this._leftPrisonerList)
				{
					this._leftPrisonerList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "LeftPrisonerList");
				}
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x0600220B RID: 8715 RVA: 0x00079E4C File Offset: 0x0007804C
		// (set) Token: 0x0600220C RID: 8716 RVA: 0x00079E54 File Offset: 0x00078054
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> RightPrisonerList
		{
			get
			{
				return this._rightPrisonerList;
			}
			set
			{
				if (value != this._rightPrisonerList)
				{
					this._rightPrisonerList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "RightPrisonerList");
				}
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x00079E72 File Offset: 0x00078072
		// (set) Token: 0x0600220E RID: 8718 RVA: 0x00079E7A File Offset: 0x0007807A
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> LeftItemList
		{
			get
			{
				return this._leftItemList;
			}
			set
			{
				if (value != this._leftItemList)
				{
					this._leftItemList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "LeftItemList");
				}
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x0600220F RID: 8719 RVA: 0x00079E98 File Offset: 0x00078098
		// (set) Token: 0x06002210 RID: 8720 RVA: 0x00079EA0 File Offset: 0x000780A0
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> RightItemList
		{
			get
			{
				return this._rightItemList;
			}
			set
			{
				if (value != this._rightItemList)
				{
					this._rightItemList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "RightItemList");
				}
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06002211 RID: 8721 RVA: 0x00079EBE File Offset: 0x000780BE
		// (set) Token: 0x06002212 RID: 8722 RVA: 0x00079EC6 File Offset: 0x000780C6
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> LeftOtherList
		{
			get
			{
				return this._leftOtherList;
			}
			set
			{
				if (value != this._leftOtherList)
				{
					this._leftOtherList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "LeftOtherList");
				}
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06002213 RID: 8723 RVA: 0x00079EE4 File Offset: 0x000780E4
		// (set) Token: 0x06002214 RID: 8724 RVA: 0x00079EEC File Offset: 0x000780EC
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> RightOtherList
		{
			get
			{
				return this._rightOtherList;
			}
			set
			{
				if (value != this._rightOtherList)
				{
					this._rightOtherList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "RightOtherList");
				}
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06002215 RID: 8725 RVA: 0x00079F0A File Offset: 0x0007810A
		// (set) Token: 0x06002216 RID: 8726 RVA: 0x00079F12 File Offset: 0x00078112
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> LeftDiplomaticList
		{
			get
			{
				return this._leftDiplomaticList;
			}
			set
			{
				if (value != this._leftDiplomaticList)
				{
					this._leftDiplomaticList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "LeftDiplomaticList");
				}
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06002217 RID: 8727 RVA: 0x00079F30 File Offset: 0x00078130
		// (set) Token: 0x06002218 RID: 8728 RVA: 0x00079F38 File Offset: 0x00078138
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> RightDiplomaticList
		{
			get
			{
				return this._rightDiplomaticList;
			}
			set
			{
				if (value != this._rightDiplomaticList)
				{
					this._rightDiplomaticList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "RightDiplomaticList");
				}
			}
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06002219 RID: 8729 RVA: 0x00079F56 File Offset: 0x00078156
		// (set) Token: 0x0600221A RID: 8730 RVA: 0x00079F5E File Offset: 0x0007815E
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> LeftOfferList
		{
			get
			{
				return this._leftOfferList;
			}
			set
			{
				if (value != this._leftOfferList)
				{
					this._leftOfferList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "LeftOfferList");
				}
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x0600221B RID: 8731 RVA: 0x00079F7C File Offset: 0x0007817C
		// (set) Token: 0x0600221C RID: 8732 RVA: 0x00079F84 File Offset: 0x00078184
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> RightOfferList
		{
			get
			{
				return this._rightOfferList;
			}
			set
			{
				if (value != this._rightOfferList)
				{
					this._rightOfferList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "RightOfferList");
				}
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x0600221D RID: 8733 RVA: 0x00079FA2 File Offset: 0x000781A2
		// (set) Token: 0x0600221E RID: 8734 RVA: 0x00079FAA File Offset: 0x000781AA
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> RightGoldList
		{
			get
			{
				return this._rightGoldList;
			}
			set
			{
				if (value != this._rightGoldList)
				{
					this._rightGoldList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "RightGoldList");
				}
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x0600221F RID: 8735 RVA: 0x00079FC8 File Offset: 0x000781C8
		// (set) Token: 0x06002220 RID: 8736 RVA: 0x00079FD0 File Offset: 0x000781D0
		[DataSourceProperty]
		public MBBindingList<BarterItemVM> LeftGoldList
		{
			get
			{
				return this._leftGoldList;
			}
			set
			{
				if (value != this._leftGoldList)
				{
					this._leftGoldList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BarterItemVM>>(value, "LeftGoldList");
				}
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06002221 RID: 8737 RVA: 0x00079FEE File Offset: 0x000781EE
		// (set) Token: 0x06002222 RID: 8738 RVA: 0x00079FF6 File Offset: 0x000781F6
		[DataSourceProperty]
		public bool InitializationIsOver
		{
			get
			{
				return this._initializationIsOver;
			}
			set
			{
				this._initializationIsOver = value;
				base.OnPropertyChangedWithValue(value, "InitializationIsOver");
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06002223 RID: 8739 RVA: 0x0007A00B File Offset: 0x0007820B
		// (set) Token: 0x06002224 RID: 8740 RVA: 0x0007A013 File Offset: 0x00078213
		[DataSourceProperty]
		public int ResultBarOtherPercentage
		{
			get
			{
				return this._resultBarOtherPercentage;
			}
			set
			{
				this._resultBarOtherPercentage = value;
				base.OnPropertyChangedWithValue(value, "ResultBarOtherPercentage");
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06002225 RID: 8741 RVA: 0x0007A028 File Offset: 0x00078228
		// (set) Token: 0x06002226 RID: 8742 RVA: 0x0007A030 File Offset: 0x00078230
		[DataSourceProperty]
		public int ResultBarOffererPercentage
		{
			get
			{
				return this._resultBarOffererPercentage;
			}
			set
			{
				this._resultBarOffererPercentage = value;
				base.OnPropertyChangedWithValue(value, "ResultBarOffererPercentage");
			}
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x0007A045 File Offset: 0x00078245
		public void SetResetInputKey(HotKey hotkey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x0007A054 File Offset: 0x00078254
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x0007A063 File Offset: 0x00078263
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x0600222A RID: 8746 RVA: 0x0007A072 File Offset: 0x00078272
		// (set) Token: 0x0600222B RID: 8747 RVA: 0x0007A07A File Offset: 0x0007827A
		[DataSourceProperty]
		public InputKeyItemVM ResetInputKey
		{
			get
			{
				return this._resetInputKey;
			}
			set
			{
				if (value != this._resetInputKey)
				{
					this._resetInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ResetInputKey");
				}
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x0600222C RID: 8748 RVA: 0x0007A098 File Offset: 0x00078298
		// (set) Token: 0x0600222D RID: 8749 RVA: 0x0007A0A0 File Offset: 0x000782A0
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

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x0007A0BE File Offset: 0x000782BE
		// (set) Token: 0x0600222F RID: 8751 RVA: 0x0007A0C6 File Offset: 0x000782C6
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

		// Token: 0x06002230 RID: 8752 RVA: 0x0007A0E4 File Offset: 0x000782E4
		public void InitializeStaticContent()
		{
			this.FiefLbl = GameTexts.FindText("str_fiefs", null).ToString();
			this.PrisonerLbl = GameTexts.FindText("str_prisoner_tag_name", null).ToString();
			this.ItemLbl = GameTexts.FindText("str_item_tag_name", null).ToString();
			this.OtherLbl = GameTexts.FindText("str_other", null).ToString();
			this.CancelLbl = GameTexts.FindText("str_cancel", null).ToString();
			this.OfferLbl = GameTexts.FindText("str_confirm", null).ToString();
			this.ResetLbl = GameTexts.FindText("str_reset", null).ToString();
			this.DiplomaticLbl = GameTexts.FindText("str_diplomatic_group", null).ToString();
			this.AutoBalanceHint.HintText = new TextObject("{=Ve5jkJqf}Auto Offer", null);
		}

		// Token: 0x04000F6F RID: 3951
		private readonly List<Dictionary<BarterGroup, MBBindingList<BarterItemVM>>> _barterList;

		// Token: 0x04000F70 RID: 3952
		private readonly List<MBBindingList<BarterItemVM>> _offerList;

		// Token: 0x04000F71 RID: 3953
		private readonly Dictionary<BarterGroup, MBBindingList<BarterItemVM>> _leftList;

		// Token: 0x04000F72 RID: 3954
		private readonly Dictionary<BarterGroup, MBBindingList<BarterItemVM>> _rightList;

		// Token: 0x04000F73 RID: 3955
		private readonly bool _isPlayerOfferer;

		// Token: 0x04000F74 RID: 3956
		private readonly BarterManager _barter;

		// Token: 0x04000F75 RID: 3957
		private readonly CharacterObject _otherCharacter;

		// Token: 0x04000F76 RID: 3958
		private readonly PartyBase _otherParty;

		// Token: 0x04000F77 RID: 3959
		private readonly BarterData _barterData;

		// Token: 0x04000F78 RID: 3960
		private string _fiefLbl;

		// Token: 0x04000F79 RID: 3961
		private string _prisonerLbl;

		// Token: 0x04000F7A RID: 3962
		private string _itemLbl;

		// Token: 0x04000F7B RID: 3963
		private string _otherLbl;

		// Token: 0x04000F7C RID: 3964
		private string _cancelLbl;

		// Token: 0x04000F7D RID: 3965
		private string _resetLbl;

		// Token: 0x04000F7E RID: 3966
		private string _offerLbl;

		// Token: 0x04000F7F RID: 3967
		private string _diplomaticLbl;

		// Token: 0x04000F80 RID: 3968
		private HintViewModel _autoBalanceHint;

		// Token: 0x04000F81 RID: 3969
		private HeroVM _leftHero;

		// Token: 0x04000F82 RID: 3970
		private HeroVM _rightHero;

		// Token: 0x04000F83 RID: 3971
		private string _leftNameLbl;

		// Token: 0x04000F84 RID: 3972
		private string _rightNameLbl;

		// Token: 0x04000F85 RID: 3973
		private MBBindingList<BarterItemVM> _leftFiefList;

		// Token: 0x04000F86 RID: 3974
		private MBBindingList<BarterItemVM> _rightFiefList;

		// Token: 0x04000F87 RID: 3975
		private MBBindingList<BarterItemVM> _leftPrisonerList;

		// Token: 0x04000F88 RID: 3976
		private MBBindingList<BarterItemVM> _rightPrisonerList;

		// Token: 0x04000F89 RID: 3977
		private MBBindingList<BarterItemVM> _leftItemList;

		// Token: 0x04000F8A RID: 3978
		private MBBindingList<BarterItemVM> _rightItemList;

		// Token: 0x04000F8B RID: 3979
		private MBBindingList<BarterItemVM> _leftOtherList;

		// Token: 0x04000F8C RID: 3980
		private MBBindingList<BarterItemVM> _rightOtherList;

		// Token: 0x04000F8D RID: 3981
		private MBBindingList<BarterItemVM> _leftDiplomaticList;

		// Token: 0x04000F8E RID: 3982
		private MBBindingList<BarterItemVM> _rightDiplomaticList;

		// Token: 0x04000F8F RID: 3983
		private MBBindingList<BarterItemVM> _leftGoldList;

		// Token: 0x04000F90 RID: 3984
		private MBBindingList<BarterItemVM> _rightGoldList;

		// Token: 0x04000F91 RID: 3985
		private MBBindingList<BarterItemVM> _leftOfferList;

		// Token: 0x04000F92 RID: 3986
		private MBBindingList<BarterItemVM> _rightOfferList;

		// Token: 0x04000F93 RID: 3987
		private int _leftMaxGold;

		// Token: 0x04000F94 RID: 3988
		private int _rightMaxGold;

		// Token: 0x04000F95 RID: 3989
		private bool _initializationIsOver;

		// Token: 0x04000F96 RID: 3990
		private bool _isOfferDisabled;

		// Token: 0x04000F97 RID: 3991
		private int _resultBarOffererPercentage = -1;

		// Token: 0x04000F98 RID: 3992
		private int _resultBarOtherPercentage = -1;

		// Token: 0x04000F99 RID: 3993
		private InputKeyItemVM _resetInputKey;

		// Token: 0x04000F9A RID: 3994
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000F9B RID: 3995
		private InputKeyItemVM _cancelInputKey;
	}
}
