using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000BB RID: 187
	[MenuOverlay("EncounterMenuOverlay")]
	public class EncounterMenuOverlayVM : GameMenuOverlay
	{
		// Token: 0x06001207 RID: 4615 RVA: 0x0004805C File Offset: 0x0004625C
		public EncounterMenuOverlayVM()
		{
			this.AttackerPartyList = new MBBindingList<GameMenuPartyItemVM>();
			this.DefenderPartyList = new MBBindingList<GameMenuPartyItemVM>();
			base.CurrentOverlayType = 1;
			this.AttackerMoraleHint = new BasicTooltipViewModel(() => this.GetEncounterSideMoraleTooltip(BattleSideEnum.Attacker));
			this.DefenderMoraleHint = new BasicTooltipViewModel(() => this.GetEncounterSideMoraleTooltip(BattleSideEnum.Defender));
			this.AttackerFoodHint = new BasicTooltipViewModel(() => this.GetEncounterSideFoodTooltip(BattleSideEnum.Attacker));
			this.DefenderFoodHint = new BasicTooltipViewModel(() => this.GetEncounterSideFoodTooltip(BattleSideEnum.Defender));
			this.AttackerTroopNumHint = new BasicTooltipViewModel(() => this.GetEncounterSideTroopsTooltip(BattleSideEnum.Attacker));
			this.DefenderTroopNumHint = new BasicTooltipViewModel(() => this.GetEncounterSideTroopsTooltip(BattleSideEnum.Defender));
			this.AttackerShipNumHint = new BasicTooltipViewModel(() => this.GetEncounterSideShipsTooltip(BattleSideEnum.Attacker));
			this.DefenderShipNumHint = new BasicTooltipViewModel(() => this.GetEncounterSideShipsTooltip(BattleSideEnum.Defender));
			this.DefenderWallHint = new BasicTooltipViewModel();
			base.IsInitializationOver = false;
			this.UpdateLists();
			this.UpdateProperties();
			base.IsInitializationOver = true;
			this.RefreshValues();
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x00048170 File Offset: 0x00046370
		private void SetAttackerAndDefenderParties(out bool attackerChanged, out bool defenderChanged)
		{
			attackerChanged = false;
			defenderChanged = false;
			if (MobileParty.MainParty.MapEvent != null)
			{
				PartyBase leaderParty = MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker);
				if (leaderParty.IsSettlement)
				{
					if (this._attackerLeadingParty == null || this._attackerLeadingParty.Party != leaderParty)
					{
						attackerChanged = true;
						this._attackerLeadingParty = new GameMenuPartyItemVM(new Action<GameMenuPartyItemVM>(this.ExecuteOnSetAsActiveContextMenuItem), leaderParty.Settlement);
					}
				}
				else if (this._attackerLeadingParty == null || this._attackerLeadingParty.Party != leaderParty)
				{
					attackerChanged = true;
					this._attackerLeadingParty = new GameMenuPartyItemVM(new Action<GameMenuPartyItemVM>(this.ExecuteOnSetAsActiveContextMenuItem), leaderParty, false);
				}
				PartyBase leaderParty2 = MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender);
				if (leaderParty2.IsSettlement)
				{
					if (this._defenderLeadingParty == null || this._defenderLeadingParty.Party != leaderParty2)
					{
						defenderChanged = true;
						this._defenderLeadingParty = new GameMenuPartyItemVM(new Action<GameMenuPartyItemVM>(this.ExecuteOnSetAsActiveContextMenuItem), leaderParty2.Settlement);
						return;
					}
				}
				else if (this._defenderLeadingParty == null || this._defenderLeadingParty.Party != leaderParty2)
				{
					defenderChanged = true;
					this._defenderLeadingParty = new GameMenuPartyItemVM(new Action<GameMenuPartyItemVM>(this.ExecuteOnSetAsActiveContextMenuItem), leaderParty2, false);
					return;
				}
			}
			else
			{
				Settlement settlement = Settlement.CurrentSettlement ?? PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
				SiegeEvent siegeEvent = settlement.SiegeEvent;
				if (siegeEvent != null)
				{
					if (this._defenderLeadingParty == null || this._defenderLeadingParty.Settlement != settlement)
					{
						defenderChanged = true;
						this._defenderLeadingParty = new GameMenuPartyItemVM(new Action<GameMenuPartyItemVM>(this.ExecuteOnSetAsActiveContextMenuItem), settlement);
					}
					if (this._attackerLeadingParty == null || this._attackerLeadingParty.Party != siegeEvent.BesiegerCamp.LeaderParty.Party)
					{
						attackerChanged = true;
						this._attackerLeadingParty = new GameMenuPartyItemVM(new Action<GameMenuPartyItemVM>(this.ExecuteOnSetAsActiveContextMenuItem), siegeEvent.BesiegerCamp.LeaderParty.Party, false);
					}
					this.DefenderWallHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetSiegeWallTooltip(settlement.Town.GetWallLevel(), MathF.Ceiling(settlement.SettlementTotalWallHitPoints)));
					return;
				}
				Debug.FailedAssert("Encounter overlay is open but MapEvent AND SiegeEvent is null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\Overlay\\EncounterMenuOverlayVM.cs", "SetAttackerAndDefenderParties", 121);
			}
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x00048390 File Offset: 0x00046590
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.AttackerBannerHint = new HintViewModel(GameTexts.FindText("str_attacker_banner", null), null);
			this.DefenderBannerHint = new HintViewModel(GameTexts.FindText("str_defender_banner", null), null);
			base.ContextList.Add(new StringItemWithEnabledAndHintVM(new Action<object>(base.ExecuteTroopAction), GameTexts.FindText("str_menu_overlay_context_list", GameMenuOverlay.MenuOverlayContextList.Encyclopedia.ToString()).ToString(), true, GameMenuOverlay.MenuOverlayContextList.Encyclopedia, null));
			this.AttackerPartyList.ApplyActionOnAllItems(delegate(GameMenuPartyItemVM x)
			{
				x.RefreshValues();
			});
			this.DefenderPartyList.ApplyActionOnAllItems(delegate(GameMenuPartyItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x00048468 File Offset: 0x00046668
		public override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (MobileParty.MainParty.MapEvent != null && this.AttackerPartyList.Count + this.DefenderPartyList.Count != MobileParty.MainParty.MapEvent.InvolvedParties.Count<PartyBase>())
			{
				this.UpdateLists();
			}
			this.AttackerPartyList.ApplyActionOnAllItems(delegate(GameMenuPartyItemVM ap)
			{
				ap.RefreshCounts();
			});
			this.DefenderPartyList.ApplyActionOnAllItems(delegate(GameMenuPartyItemVM dp)
			{
				dp.RefreshCounts();
			});
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x0004850F File Offset: 0x0004670F
		public override void Refresh()
		{
			base.IsInitializationOver = false;
			this.UpdateLists();
			this.UpdateProperties();
			base.IsInitializationOver = true;
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0004852C File Offset: 0x0004672C
		private void UpdateProperties()
		{
			if (this.IsSiege)
			{
				GameMenuPartyItemVM defenderLeadingParty = this._defenderLeadingParty;
				bool flag = ((defenderLeadingParty != null) ? defenderLeadingParty.Settlement : null) != null;
				float num = 0f;
				float num2 = 0f;
				if (flag)
				{
					ValueTuple<int, int> townFoodAndMarketStocks = TownHelpers.GetTownFoodAndMarketStocks((flag ? this._defenderLeadingParty : this._attackerLeadingParty).Settlement.Town);
					num2 = (float)(townFoodAndMarketStocks.Item1 + townFoodAndMarketStocks.Item2) / -this._defenderLeadingParty.Settlement.Town.FoodChangeWithoutMarketStocks;
				}
				foreach (GameMenuPartyItemVM gameMenuPartyItemVM in this.DefenderPartyList)
				{
					num += gameMenuPartyItemVM.Party.MobileParty.Morale;
					if (!flag)
					{
						num2 += gameMenuPartyItemVM.Party.MobileParty.Food / -gameMenuPartyItemVM.Party.MobileParty.FoodChange;
					}
				}
				num /= (float)this.DefenderPartyList.Count;
				if (!flag)
				{
					num2 /= (float)this.DefenderPartyList.Count;
				}
				num2 = (float)Math.Max((int)Math.Ceiling((double)num2), 0);
				MBTextManager.SetTextVariable("DAY_NUM", num2.ToString(), false);
				MBTextManager.SetTextVariable("PLURAL", (num2 > 1f) ? 1 : 0);
				this.DefenderPartyFood = GameTexts.FindText("str_party_food_left", null).ToString();
				this.DefenderPartyMorale = num.ToString("0.0");
				num = 0f;
				num2 = 0f;
				if (!flag)
				{
					if (this._attackerLeadingParty.Settlement != null)
					{
						num2 = this._attackerLeadingParty.Settlement.Town.FoodStocks / this._attackerLeadingParty.Settlement.Town.FoodChangeWithoutMarketStocks;
					}
					else if (this._attackerLeadingParty.Party.MobileParty.CurrentSettlement != null)
					{
						num2 = this._attackerLeadingParty.Party.MobileParty.CurrentSettlement.Town.FoodStocks / this._attackerLeadingParty.Party.MobileParty.CurrentSettlement.Town.FoodChangeWithoutMarketStocks;
					}
					else
					{
						Settlement currentSettlement = Settlement.CurrentSettlement;
						if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) != null)
						{
							num2 = Settlement.CurrentSettlement.Town.FoodStocks / Settlement.CurrentSettlement.Town.FoodChangeWithoutMarketStocks;
						}
						else
						{
							Debug.FailedAssert("There are no settlements involved in the siege", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\Overlay\\EncounterMenuOverlayVM.cs", "UpdateProperties", 217);
						}
					}
				}
				else
				{
					Settlement currentSettlement2 = Settlement.CurrentSettlement;
					if (((currentSettlement2 != null) ? currentSettlement2.SiegeEvent : null) != null)
					{
						num2 = Settlement.CurrentSettlement.Town.FoodStocks / Settlement.CurrentSettlement.Town.FoodChangeWithoutMarketStocks;
					}
				}
				foreach (GameMenuPartyItemVM gameMenuPartyItemVM2 in this.AttackerPartyList)
				{
					num += gameMenuPartyItemVM2.Party.MobileParty.Morale;
					if (flag)
					{
						num2 += gameMenuPartyItemVM2.Party.MobileParty.Food / -gameMenuPartyItemVM2.Party.MobileParty.FoodChange;
					}
				}
				num /= (float)this.AttackerPartyList.Count;
				if (flag)
				{
					num2 /= (float)this.AttackerPartyList.Count;
				}
				num2 = (float)Math.Max((int)Math.Ceiling((double)num2), 0);
				MBTextManager.SetTextVariable("DAY_NUM", num2.ToString(), false);
				MBTextManager.SetTextVariable("PLURAL", (num2 > 1f) ? 1 : 0);
				this.AttackerPartyFood = GameTexts.FindText("str_party_food_left", null).ToString();
				this.AttackerPartyMorale = num.ToString("0.0");
				Settlement settlement;
				if ((settlement = Settlement.CurrentSettlement) == null)
				{
					SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
					settlement = ((playerSiegeEvent != null) ? playerSiegeEvent.BesiegedSettlement : null);
				}
				Settlement settlement2 = settlement;
				if (settlement2 != null)
				{
					this.DefenderWallHitPoints = MathF.Ceiling(settlement2.SettlementTotalWallHitPoints).ToString();
				}
			}
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00048914 File Offset: 0x00046B14
		private void UpdateLists()
		{
			if (MobileParty.MainParty.MapEvent == null)
			{
				Settlement settlement;
				if ((settlement = Settlement.CurrentSettlement) == null)
				{
					SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
					settlement = ((playerSiegeEvent != null) ? playerSiegeEvent.BesiegedSettlement : null);
				}
				if (settlement == null)
				{
					return;
				}
			}
			bool flag;
			bool flag2;
			this.SetAttackerAndDefenderParties(out flag, out flag2);
			if (this._defenderLeadingParty != null && flag2)
			{
				int num = this.DefenderPartyList.FindIndex<GameMenuPartyItemVM>((GameMenuPartyItemVM x) => x.Party == this._defenderLeadingParty.Party);
				if (num != -1)
				{
					this.DefenderPartyList.RemoveAt(num);
				}
				this.DefenderPartyList.Insert(0, this._defenderLeadingParty);
			}
			if (this._attackerLeadingParty != null && flag)
			{
				int num2 = this.AttackerPartyList.FindIndex<GameMenuPartyItemVM>((GameMenuPartyItemVM x) => x.Party == this._attackerLeadingParty.Party);
				if (num2 != -1)
				{
					this.AttackerPartyList.RemoveAt(num2);
				}
				this.AttackerPartyList.Insert(0, this._attackerLeadingParty);
			}
			List<PartyBase> list = new List<PartyBase>();
			List<PartyBase> list2 = new List<PartyBase>();
			List<int> list3 = new List<int>();
			List<int> list4 = new List<int>();
			List<PartyBase> list5 = new List<PartyBase>();
			if (MobileParty.MainParty.MapEvent != null)
			{
				list5.AddRange(MobileParty.MainParty.MapEvent.InvolvedParties);
				this.IsSiege = false;
				this.IsNaval = MobileParty.MainParty.MapEvent.IsNavalMapEvent;
			}
			else
			{
				Settlement settlement2 = Settlement.CurrentSettlement ?? PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
				if (settlement2.SiegeEvent == null)
				{
					this.PowerComparer = new PowerLevelComparer(1.0, 1.0);
					return;
				}
				SiegeEvent siegeEvent = settlement2.SiegeEvent;
				list5.AddRange(siegeEvent.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege));
				this.IsSiege = true;
				this.IsNaval = false;
			}
			foreach (PartyBase partyBase in list5)
			{
				bool flag3;
				if (MobileParty.MainParty.MapEvent != null)
				{
					flag3 = partyBase.Side == BattleSideEnum.Defender;
				}
				else
				{
					flag3 = (Settlement.CurrentSettlement ?? PlayerSiege.PlayerSiegeEvent.BesiegedSettlement).SiegeEvent.GetSiegeEventSide(BattleSideEnum.Defender).HasInvolvedPartyForEventType(partyBase, MapEvent.BattleTypes.Siege);
				}
				List<PartyBase> list6 = (flag3 ? list2 : list);
				List<int> list7 = (flag3 ? list4 : list3);
				if (partyBase.IsActive && partyBase.MemberRoster.Count > 0)
				{
					int numberOfHealthyMembers = partyBase.NumberOfHealthyMembers;
					int num3 = 0;
					while (num3 < list7.Count && numberOfHealthyMembers <= list7[num3])
					{
						num3++;
					}
					list7.Add(partyBase.NumberOfHealthyMembers);
					list6.Insert(num3, partyBase);
				}
			}
			float num4 = list2.Sum<PartyBase>((PartyBase party) => party.CalculateCurrentStrength());
			float num5 = list.Sum<PartyBase>((PartyBase party) => party.CalculateCurrentStrength());
			if (list5.AnyQ<PartyBase>(delegate(PartyBase p)
			{
				MobileParty mobileParty = p.MobileParty;
				return mobileParty != null && mobileParty.IsInfoHidden;
			}))
			{
				num4 = 1f;
				num5 = 0f;
			}
			if (this.PowerComparer == null)
			{
				this.PowerComparer = new PowerLevelComparer((double)num4, (double)num5);
			}
			else
			{
				this.PowerComparer.Update((double)num4, (double)num5, (double)num4, (double)num5);
			}
			List<PartyBase> list8 = list.OrderByDescending<PartyBase, int>((PartyBase p) => p.NumberOfAllMembers).ToList<PartyBase>();
			List<PartyBase> list9 = this.AttackerPartyList.Select<GameMenuPartyItemVM, PartyBase>((GameMenuPartyItemVM enemy) => enemy.Party).ToList<PartyBase>();
			List<PartyBase> list10 = list8.Except<PartyBase>(list9).ToList<PartyBase>();
			list10.Remove(this._attackerLeadingParty.Party);
			foreach (PartyBase partyBase2 in list9.Except<PartyBase>(list8).ToList<PartyBase>())
			{
				for (int i = this.AttackerPartyList.Count - 1; i >= 0; i--)
				{
					if (this.AttackerPartyList[i].Party == partyBase2)
					{
						this.AttackerPartyList.RemoveAt(i);
					}
				}
			}
			if (this.IsSiege)
			{
				list10 = list10.Where<PartyBase>((PartyBase x) => x.MemberRoster.TotalHealthyCount > 0).ToList<PartyBase>();
			}
			foreach (PartyBase partyBase3 in list10)
			{
				this.AttackerPartyList.Add(new GameMenuPartyItemVM(new Action<GameMenuPartyItemVM>(this.ExecuteOnSetAsActiveContextMenuItem), partyBase3, false));
			}
			List<PartyBase> list11 = list2.OrderByDescending<PartyBase, int>((PartyBase p) => p.NumberOfAllMembers).ToList<PartyBase>();
			List<PartyBase> list12 = this.DefenderPartyList.Select<GameMenuPartyItemVM, PartyBase>((GameMenuPartyItemVM ally) => ally.Party).ToList<PartyBase>();
			List<PartyBase> list13 = list11.Except<PartyBase>(list12).ToList<PartyBase>();
			list13.Remove(this._defenderLeadingParty.Party);
			foreach (PartyBase partyBase4 in list12.Except<PartyBase>(list11).ToList<PartyBase>())
			{
				for (int j = this.DefenderPartyList.Count - 1; j >= 0; j--)
				{
					if (this.DefenderPartyList[j].Party == partyBase4)
					{
						this.DefenderPartyList.RemoveAt(j);
					}
				}
			}
			if (this.IsSiege)
			{
				list13 = list13.Where<PartyBase>((PartyBase x) => x.MemberRoster.TotalHealthyCount > 0).ToList<PartyBase>();
			}
			foreach (PartyBase partyBase5 in list13)
			{
				this.DefenderPartyList.Add(new GameMenuPartyItemVM(new Action<GameMenuPartyItemVM>(this.ExecuteOnSetAsActiveContextMenuItem), partyBase5, false));
			}
			foreach (GameMenuPartyItemVM gameMenuPartyItemVM in this.DefenderPartyList)
			{
				gameMenuPartyItemVM.RefreshProperties();
			}
			foreach (GameMenuPartyItemVM gameMenuPartyItemVM2 in this.AttackerPartyList)
			{
				gameMenuPartyItemVM2.RefreshProperties();
			}
			this.DefenderPartyCount = this.DefenderPartyList.Sum<GameMenuPartyItemVM>(delegate(GameMenuPartyItemVM p)
			{
				int? num8;
				if (p == null)
				{
					num8 = null;
				}
				else
				{
					PartyBase party = p.Party;
					num8 = ((party != null) ? new int?(party.NumberOfHealthyMembers) : null);
				}
				int? num9 = num8;
				if (num9 == null)
				{
					return 0;
				}
				return num9.GetValueOrDefault();
			});
			this.DefenderPartyCountLbl = (this.DefenderPartyList.AnyQ<GameMenuPartyItemVM>(delegate(GameMenuPartyItemVM p)
			{
				MobileParty mobileParty2 = p.Party.MobileParty;
				return mobileParty2 != null && mobileParty2.IsInfoHidden;
			}) ? "?" : this.DefenderPartyCount.ToString());
			this.DefenderShipCount = this.DefenderPartyList.Sum<GameMenuPartyItemVM>(delegate(GameMenuPartyItemVM p)
			{
				int? num10;
				if (p == null)
				{
					num10 = null;
				}
				else
				{
					PartyBase party2 = p.Party;
					num10 = ((party2 != null) ? new int?(party2.Ships.Count) : null);
				}
				int? num11 = num10;
				if (num11 == null)
				{
					return 0;
				}
				return num11.GetValueOrDefault();
			});
			this.AttackerPartyCount = this.AttackerPartyList.Sum<GameMenuPartyItemVM>(delegate(GameMenuPartyItemVM p)
			{
				int? num12;
				if (p == null)
				{
					num12 = null;
				}
				else
				{
					PartyBase party3 = p.Party;
					num12 = ((party3 != null) ? new int?(party3.NumberOfHealthyMembers) : null);
				}
				int? num13 = num12;
				if (num13 == null)
				{
					return 0;
				}
				return num13.GetValueOrDefault();
			});
			this.AttackerPartyCountLbl = (this.AttackerPartyList.AnyQ<GameMenuPartyItemVM>(delegate(GameMenuPartyItemVM p)
			{
				MobileParty mobileParty3 = p.Party.MobileParty;
				return mobileParty3 != null && mobileParty3.IsInfoHidden;
			}) ? "?" : this.AttackerPartyCount.ToString());
			this.AttackerShipCount = this.AttackerPartyList.Sum<GameMenuPartyItemVM>(delegate(GameMenuPartyItemVM p)
			{
				int? num14;
				if (p == null)
				{
					num14 = null;
				}
				else
				{
					PartyBase party4 = p.Party;
					num14 = ((party4 != null) ? new int?(party4.Ships.Count) : null);
				}
				int? num15 = num14;
				if (num15 == null)
				{
					return 0;
				}
				return num15.GetValueOrDefault();
			});
			if (MobileParty.MainParty.MapEvent != null)
			{
				PartyBase leaderParty = MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker);
				PartyBase leaderParty2 = MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender);
				if (this._attackerLeadingParty.Party == leaderParty2 || this._defenderLeadingParty.Party == leaderParty)
				{
					GameMenuPartyItemVM attackerLeadingParty = this._attackerLeadingParty;
					this._attackerLeadingParty = this._defenderLeadingParty;
					this._defenderLeadingParty = attackerLeadingParty;
				}
			}
			this.TitleText = (this.IsSiege ? GameTexts.FindText("str_siege", null).ToString() : (this.TitleText = GameTexts.FindText("str_battle", null).ToString()));
			IFaction faction = ((this._defenderLeadingParty.Party == null) ? this._defenderLeadingParty.Settlement.MapFaction : this._defenderLeadingParty.Party.MapFaction);
			IFaction faction2 = ((this._attackerLeadingParty.Party == null) ? this._attackerLeadingParty.Settlement.MapFaction : this._attackerLeadingParty.Party.MapFaction);
			Banner banner = ((this._defenderLeadingParty.Party == null) ? this._defenderLeadingParty.Settlement.OwnerClan.Banner : this._defenderLeadingParty.Party.Banner);
			Banner banner2 = ((this._attackerLeadingParty.Party == null) ? this._attackerLeadingParty.Settlement.OwnerClan.Banner : this._attackerLeadingParty.Party.Banner);
			this.DefenderPartyBanner = new BannerImageIdentifierVM(banner, true);
			this.AttackerPartyBanner = new BannerImageIdentifierVM(banner2, true);
			string text;
			if (faction != null && faction is Kingdom)
			{
				text = Color.FromUint(((Kingdom)faction).PrimaryBannerColor).ToString();
			}
			else
			{
				uint? num6;
				if (faction == null)
				{
					num6 = null;
				}
				else
				{
					Banner banner3 = faction.Banner;
					num6 = ((banner3 != null) ? new uint?(banner3.GetPrimaryColor()) : null);
				}
				text = Color.FromUint(num6 ?? Color.White.ToUnsignedInteger()).ToString();
			}
			string text2;
			if (faction2 != null && faction2 is Kingdom)
			{
				text2 = Color.FromUint(((Kingdom)faction2).PrimaryBannerColor).ToString();
			}
			else
			{
				uint? num7;
				if (faction2 == null)
				{
					num7 = null;
				}
				else
				{
					Banner banner4 = faction2.Banner;
					num7 = ((banner4 != null) ? new uint?(banner4.GetPrimaryColor()) : null);
				}
				text2 = Color.FromUint(num7 ?? Color.White.ToUnsignedInteger()).ToString();
			}
			this.PowerComparer.SetColors(text, text2);
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00049420 File Offset: 0x00047620
		private List<TooltipProperty> GetEncounterSideFoodTooltip(BattleSideEnum side)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			GameMenuPartyItemVM defenderLeadingParty = this._defenderLeadingParty;
			bool flag = ((defenderLeadingParty != null) ? defenderLeadingParty.Settlement : null) != null;
			bool flag2 = (flag && flag && side == BattleSideEnum.Defender) || (!flag && side == BattleSideEnum.Attacker);
			if (this.IsSiege && flag2)
			{
				list.Add(new TooltipProperty(new TextObject("{=OSsSBHKe}Settlement's Food", null).ToString(), "", 0, false, TooltipProperty.TooltipPropertyFlags.Title));
				GameMenuPartyItemVM gameMenuPartyItemVM = (flag ? this._defenderLeadingParty : this._attackerLeadingParty);
				Town town;
				if (gameMenuPartyItemVM == null)
				{
					town = null;
				}
				else
				{
					Settlement settlement = gameMenuPartyItemVM.Settlement;
					town = ((settlement != null) ? settlement.Town : null);
				}
				Town town2 = town;
				float num = ((town2 != null) ? town2.FoodChangeWithoutMarketStocks : 0f);
				ValueTuple<int, int> townFoodAndMarketStocks = TownHelpers.GetTownFoodAndMarketStocks(town2);
				list.Add(new TooltipProperty(new TextObject("{=EkFDvG7z}Settlement Food Stocks", null).ToString(), townFoodAndMarketStocks.Item1.ToString("F0"), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				if (townFoodAndMarketStocks.Item2 != 0)
				{
					list.Add(new TooltipProperty(new TextObject("{=HTtWslIx}Market Food Stocks", null).ToString(), townFoodAndMarketStocks.Item2.ToString("F0"), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
				list.Add(new TooltipProperty(new TextObject("{=laznt9ZK}Settlement Food Change", null).ToString(), num.ToString("F2"), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				list.Add(new TooltipProperty("", string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.RundownSeperator));
				list.Add(new TooltipProperty(new TextObject("{=DNXD37JL}Settlement's Days Until Food Runs Out", null).ToString(), CampaignUIHelper.GetDaysUntilNoFood((float)(townFoodAndMarketStocks.Item1 + townFoodAndMarketStocks.Item2), num), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				if (((town2 != null) ? town2.Settlement : null) != null && SettlementHelper.IsGarrisonStarving(town2.Settlement))
				{
					list.Add(new TooltipProperty(new TextObject("{=0rmpC7jf}The Garrison is Starving", null).ToString(), string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			else
			{
				list.Add(new TooltipProperty(new TextObject("{=Q8dhryRX}Parties' Food", null).ToString(), "", 0, false, TooltipProperty.TooltipPropertyFlags.Title));
				MBBindingList<GameMenuPartyItemVM> mbbindingList = ((side == BattleSideEnum.Attacker) ? this.AttackerPartyList : this.DefenderPartyList);
				double num2 = 0.0;
				foreach (GameMenuPartyItemVM gameMenuPartyItemVM2 in mbbindingList)
				{
					float num3 = gameMenuPartyItemVM2.Party.MobileParty.Food / -gameMenuPartyItemVM2.Party.MobileParty.FoodChange;
					num2 += (double)Math.Max(num3, 0f);
					string daysUntilNoFood = CampaignUIHelper.GetDaysUntilNoFood(gameMenuPartyItemVM2.Party.MobileParty.Food, gameMenuPartyItemVM2.Party.MobileParty.FoodChange);
					list.Add(new TooltipProperty(gameMenuPartyItemVM2.Party.MobileParty.Name.ToString(), daysUntilNoFood, 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
				list.Add(new TooltipProperty("", string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.RundownSeperator));
				list.Add(new TooltipProperty(new TextObject("{=rwKBR4NE}Average Days Until Food Runs Out", null).ToString(), MathF.Ceiling(num2 / (double)mbbindingList.Count).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			return list;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00049764 File Offset: 0x00047964
		private List<TooltipProperty> GetEncounterSideTroopsTooltip(BattleSideEnum side)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			MBBindingList<GameMenuPartyItemVM> sideList = ((side == BattleSideEnum.Attacker) ? this.AttackerPartyList : this.DefenderPartyList);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (GameMenuPartyItemVM gameMenuPartyItemVM in sideList)
			{
				for (int i = 0; i < gameMenuPartyItemVM.Party.MemberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = gameMenuPartyItemVM.Party.MemberRoster.GetElementCopyAtIndex(i);
					troopRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, 0, true, -1);
				}
			}
			Func<TroopRoster> getTempRoster = delegate
			{
				TroopRoster troopRoster4 = TroopRoster.CreateDummyTroopRoster();
				foreach (GameMenuPartyItemVM gameMenuPartyItemVM2 in sideList)
				{
					for (int m = 0; m < gameMenuPartyItemVM2.Party.MemberRoster.Count; m++)
					{
						TroopRosterElement elementCopyAtIndex4 = gameMenuPartyItemVM2.Party.MemberRoster.GetElementCopyAtIndex(m);
						troopRoster4.AddToCounts(elementCopyAtIndex4.Character, elementCopyAtIndex4.Number, false, elementCopyAtIndex4.WoundedNumber, 0, true, -1);
					}
				}
				return troopRoster4;
			};
			Dictionary<FormationClass, Tuple<int, int>> dictionary = new Dictionary<FormationClass, Tuple<int, int>>();
			for (int j = 0; j < troopRoster.Count; j++)
			{
				TroopRosterElement elementCopyAtIndex2 = troopRoster.GetElementCopyAtIndex(j);
				if (dictionary.ContainsKey(elementCopyAtIndex2.Character.DefaultFormationClass))
				{
					Tuple<int, int> tuple = dictionary[elementCopyAtIndex2.Character.DefaultFormationClass];
					dictionary[elementCopyAtIndex2.Character.DefaultFormationClass] = new Tuple<int, int>(tuple.Item1 + elementCopyAtIndex2.Number - elementCopyAtIndex2.WoundedNumber, tuple.Item2 + elementCopyAtIndex2.WoundedNumber);
				}
				else
				{
					dictionary.Add(elementCopyAtIndex2.Character.DefaultFormationClass, new Tuple<int, int>(elementCopyAtIndex2.Number - elementCopyAtIndex2.WoundedNumber, elementCopyAtIndex2.WoundedNumber));
				}
			}
			foreach (KeyValuePair<FormationClass, Tuple<int, int>> keyValuePair in dictionary.OrderBy<KeyValuePair<FormationClass, Tuple<int, int>>, FormationClass>((KeyValuePair<FormationClass, Tuple<int, int>> x) => x.Key))
			{
				TextObject textObject = new TextObject("{=Dqydb21E} {PARTY_SIZE}", null);
				textObject.SetTextVariable("PARTY_SIZE", PartyBaseHelper.GetPartySizeText(keyValuePair.Value.Item1, keyValuePair.Value.Item2, true));
				TextObject textObject2 = GameTexts.FindText("str_troop_type_name", keyValuePair.Key.GetName());
				list.Add(new TooltipProperty(textObject2.ToString(), textObject.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			list.Add(new TooltipProperty(string.Empty, string.Empty, -1, true, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty(GameTexts.FindText("str_troop_types", null).ToString(), " ", 0, true, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty("", "", 0, true, TooltipProperty.TooltipPropertyFlags.DefaultSeperator));
			for (int k = 0; k < troopRoster.Count; k++)
			{
				TroopRosterElement elementCopyAtIndex3 = troopRoster.GetElementCopyAtIndex(k);
				if (elementCopyAtIndex3.Character.IsHero)
				{
					CharacterObject hero = elementCopyAtIndex3.Character;
					list.Add(new TooltipProperty(elementCopyAtIndex3.Character.Name.ToString(), delegate
					{
						TroopRoster troopRoster2 = ((getTempRoster != null) ? getTempRoster() : troopRoster);
						int num2 = troopRoster2.FindIndexOfTroop(hero);
						if (num2 == -1)
						{
							return string.Empty;
						}
						TroopRosterElement elementCopyAtIndex5 = troopRoster2.GetElementCopyAtIndex(num2);
						TextObject textObject3 = GameTexts.FindText("str_NUMBER_percent", null);
						textObject3.SetTextVariable("NUMBER", elementCopyAtIndex5.Character.HeroObject.HitPoints * 100 / elementCopyAtIndex5.Character.MaxHitPoints());
						return textObject3.ToString();
					}, 0, true, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			for (int l = 0; l < troopRoster.Count; l++)
			{
				int num = l;
				CharacterObject character = troopRoster.GetElementCopyAtIndex(num).Character;
				if (!character.IsHero)
				{
					list.Add(new TooltipProperty(character.Name.ToString(), delegate
					{
						TroopRoster troopRoster3 = ((getTempRoster != null) ? getTempRoster() : troopRoster);
						int num3 = troopRoster3.FindIndexOfTroop(character);
						if (num3 != -1)
						{
							if (num3 > troopRoster3.Count)
							{
								return string.Empty;
							}
							TroopRosterElement elementCopyAtIndex6 = troopRoster3.GetElementCopyAtIndex(num3);
							if (elementCopyAtIndex6.Character == null)
							{
								return string.Empty;
							}
							CharacterObject character2 = elementCopyAtIndex6.Character;
							if (character2 != null && !character2.IsHero)
							{
								TextObject textObject4 = new TextObject("{=!}{PARTY_SIZE}", null);
								textObject4.SetTextVariable("PARTY_SIZE", PartyBaseHelper.GetPartySizeText(elementCopyAtIndex6.Number - elementCopyAtIndex6.WoundedNumber, elementCopyAtIndex6.WoundedNumber, true));
								return textObject4.ToString();
							}
						}
						return string.Empty;
					}, 0, true, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			return list;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00049B38 File Offset: 0x00047D38
		private List<TooltipProperty> GetEncounterSideShipsTooltip(BattleSideEnum side)
		{
			Dictionary<ShipHull.ShipType, int> dictionary = new Dictionary<ShipHull.ShipType, int>();
			Dictionary<ShipHull, int> dictionary2 = new Dictionary<ShipHull, int>();
			int num = 0;
			List<TooltipProperty> list = new List<TooltipProperty>();
			foreach (GameMenuPartyItemVM gameMenuPartyItemVM in ((side == BattleSideEnum.Attacker) ? this.AttackerPartyList : this.DefenderPartyList))
			{
				if (gameMenuPartyItemVM.Party.MobileParty.Ships.Count > 0)
				{
					num += gameMenuPartyItemVM.Party.MobileParty.Ships.Count;
					for (int i = 0; i < gameMenuPartyItemVM.Party.MobileParty.Ships.Count; i++)
					{
						Ship ship = gameMenuPartyItemVM.Party.MobileParty.Ships[i];
						ShipHull shipHull = ship.ShipHull;
						ShipHull.ShipType type = ship.ShipHull.Type;
						if (dictionary.ContainsKey(type))
						{
							Dictionary<ShipHull.ShipType, int> dictionary3 = dictionary;
							ShipHull.ShipType shipType = type;
							int num2 = dictionary3[shipType];
							dictionary3[shipType] = num2 + 1;
						}
						else
						{
							dictionary[type] = 1;
						}
						if (dictionary2.ContainsKey(shipHull))
						{
							Dictionary<ShipHull, int> dictionary4 = dictionary2;
							ShipHull shipHull2 = shipHull;
							int num2 = dictionary4[shipHull2];
							dictionary4[shipHull2] = num2 + 1;
						}
						else
						{
							dictionary2[shipHull] = 1;
						}
					}
				}
			}
			list.Add(new TooltipProperty("", "", 0, false, TooltipProperty.TooltipPropertyFlags.RundownSeperator));
			foreach (KeyValuePair<ShipHull.ShipType, int> keyValuePair in dictionary.OrderBy<KeyValuePair<ShipHull.ShipType, int>, ShipHull.ShipType>((KeyValuePair<ShipHull.ShipType, int> x) => x.Key))
			{
				TextObject textObject = new TextObject("{=Dqydb21E} {PARTY_SIZE}", null);
				textObject.SetTextVariable("PARTY_SIZE", keyValuePair.Value);
				TextObject textObject2 = GameTexts.FindText("str_ship_type", keyValuePair.Key.ToString().ToLower());
				list.Add(new TooltipProperty(textObject2.ToString(), textObject.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			list.Add(new TooltipProperty(string.Empty, string.Empty, -1, true, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty(GameTexts.FindText("str_ship_types", null).ToString(), " ", 0, true, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty("", "", 0, true, TooltipProperty.TooltipPropertyFlags.DefaultSeperator));
			foreach (KeyValuePair<ShipHull, int> keyValuePair2 in dictionary2)
			{
				ShipHull key = keyValuePair2.Key;
				int value = keyValuePair2.Value;
				list.Add(new TooltipProperty(key.Name.ToString(), value.ToString(), 0, true, TooltipProperty.TooltipPropertyFlags.None));
			}
			return list;
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00049E58 File Offset: 0x00048058
		private List<TooltipProperty> GetEncounterSideMoraleTooltip(BattleSideEnum side)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			list.Add(new TooltipProperty(new TextObject("{=QBB0KQ2Z}Parties' Average Morale", null).ToString(), "", 0, false, TooltipProperty.TooltipPropertyFlags.Title));
			MBBindingList<GameMenuPartyItemVM> mbbindingList = ((side == BattleSideEnum.Attacker) ? this.AttackerPartyList : this.DefenderPartyList);
			double num = 0.0;
			foreach (GameMenuPartyItemVM gameMenuPartyItemVM in mbbindingList)
			{
				list.Add(new TooltipProperty(gameMenuPartyItemVM.Party.MobileParty.Name.ToString(), gameMenuPartyItemVM.Party.MobileParty.Morale.ToString("0.0"), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				num += (double)gameMenuPartyItemVM.Party.MobileParty.Morale;
			}
			list.Add(new TooltipProperty("", string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.RundownSeperator));
			list.Add(new TooltipProperty(new TextObject("{=eoVW9z54}Average Morale", null).ToString(), (num / (double)mbbindingList.Count).ToString("0.0"), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			return list;
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001212 RID: 4626 RVA: 0x00049F90 File Offset: 0x00048190
		// (set) Token: 0x06001213 RID: 4627 RVA: 0x00049F98 File Offset: 0x00048198
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001214 RID: 4628 RVA: 0x00049FBB File Offset: 0x000481BB
		// (set) Token: 0x06001215 RID: 4629 RVA: 0x00049FC3 File Offset: 0x000481C3
		[DataSourceProperty]
		public BannerImageIdentifierVM DefenderPartyBanner
		{
			get
			{
				return this._defenderPartyBanner;
			}
			set
			{
				if (value != this._defenderPartyBanner)
				{
					this._defenderPartyBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "DefenderPartyBanner");
				}
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001216 RID: 4630 RVA: 0x00049FE1 File Offset: 0x000481E1
		// (set) Token: 0x06001217 RID: 4631 RVA: 0x00049FE9 File Offset: 0x000481E9
		[DataSourceProperty]
		public BannerImageIdentifierVM AttackerPartyBanner
		{
			get
			{
				return this._attackerPartyBanner;
			}
			set
			{
				if (value != this._attackerPartyBanner)
				{
					this._attackerPartyBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "AttackerPartyBanner");
				}
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001218 RID: 4632 RVA: 0x0004A007 File Offset: 0x00048207
		// (set) Token: 0x06001219 RID: 4633 RVA: 0x0004A00F File Offset: 0x0004820F
		[DataSourceProperty]
		public PowerLevelComparer PowerComparer
		{
			get
			{
				return this._powerComparer;
			}
			set
			{
				if (value != this._powerComparer)
				{
					this._powerComparer = value;
					base.OnPropertyChangedWithValue<PowerLevelComparer>(value, "PowerComparer");
				}
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x0600121A RID: 4634 RVA: 0x0004A02D File Offset: 0x0004822D
		// (set) Token: 0x0600121B RID: 4635 RVA: 0x0004A035 File Offset: 0x00048235
		[DataSourceProperty]
		public MBBindingList<GameMenuPartyItemVM> AttackerPartyList
		{
			get
			{
				return this._attackerPartyList;
			}
			set
			{
				if (value != this._attackerPartyList)
				{
					this._attackerPartyList = value;
					base.OnPropertyChangedWithValue<MBBindingList<GameMenuPartyItemVM>>(value, "AttackerPartyList");
				}
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x0600121C RID: 4636 RVA: 0x0004A053 File Offset: 0x00048253
		// (set) Token: 0x0600121D RID: 4637 RVA: 0x0004A05B File Offset: 0x0004825B
		[DataSourceProperty]
		public MBBindingList<GameMenuPartyItemVM> DefenderPartyList
		{
			get
			{
				return this._defenderPartyList;
			}
			set
			{
				if (value != this._defenderPartyList)
				{
					this._defenderPartyList = value;
					base.OnPropertyChangedWithValue<MBBindingList<GameMenuPartyItemVM>>(value, "DefenderPartyList");
				}
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x0600121E RID: 4638 RVA: 0x0004A079 File Offset: 0x00048279
		// (set) Token: 0x0600121F RID: 4639 RVA: 0x0004A081 File Offset: 0x00048281
		[DataSourceProperty]
		public string DefenderPartyMorale
		{
			get
			{
				return this._defenderPartyMorale;
			}
			set
			{
				if (value != this._defenderPartyMorale)
				{
					this._defenderPartyMorale = value;
					base.OnPropertyChangedWithValue<string>(value, "DefenderPartyMorale");
				}
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001220 RID: 4640 RVA: 0x0004A0A4 File Offset: 0x000482A4
		// (set) Token: 0x06001221 RID: 4641 RVA: 0x0004A0AC File Offset: 0x000482AC
		[DataSourceProperty]
		public string AttackerPartyMorale
		{
			get
			{
				return this._attackerPartyMorale;
			}
			set
			{
				if (value != this._attackerPartyMorale)
				{
					this._attackerPartyMorale = value;
					base.OnPropertyChangedWithValue<string>(value, "AttackerPartyMorale");
				}
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001222 RID: 4642 RVA: 0x0004A0CF File Offset: 0x000482CF
		// (set) Token: 0x06001223 RID: 4643 RVA: 0x0004A0D7 File Offset: 0x000482D7
		[DataSourceProperty]
		public int DefenderPartyCount
		{
			get
			{
				return this._defenderPartyCount;
			}
			set
			{
				if (value != this._defenderPartyCount)
				{
					this._defenderPartyCount = value;
					base.OnPropertyChangedWithValue(value, "DefenderPartyCount");
				}
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001224 RID: 4644 RVA: 0x0004A0F5 File Offset: 0x000482F5
		// (set) Token: 0x06001225 RID: 4645 RVA: 0x0004A0FD File Offset: 0x000482FD
		[DataSourceProperty]
		public int AttackerPartyCount
		{
			get
			{
				return this._attackerPartyCount;
			}
			set
			{
				if (value != this._attackerPartyCount)
				{
					this._attackerPartyCount = value;
					base.OnPropertyChangedWithValue(value, "AttackerPartyCount");
				}
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001226 RID: 4646 RVA: 0x0004A11B File Offset: 0x0004831B
		// (set) Token: 0x06001227 RID: 4647 RVA: 0x0004A123 File Offset: 0x00048323
		[DataSourceProperty]
		public int DefenderShipCount
		{
			get
			{
				return this._defenderShipCount;
			}
			set
			{
				if (value != this._defenderShipCount)
				{
					this._defenderShipCount = value;
					base.OnPropertyChangedWithValue(value, "DefenderShipCount");
				}
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x0004A141 File Offset: 0x00048341
		// (set) Token: 0x06001229 RID: 4649 RVA: 0x0004A149 File Offset: 0x00048349
		[DataSourceProperty]
		public int AttackerShipCount
		{
			get
			{
				return this._attackerShipCount;
			}
			set
			{
				if (value != this._attackerShipCount)
				{
					this._attackerShipCount = value;
					base.OnPropertyChangedWithValue(value, "AttackerShipCount");
				}
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x0600122A RID: 4650 RVA: 0x0004A167 File Offset: 0x00048367
		// (set) Token: 0x0600122B RID: 4651 RVA: 0x0004A16F File Offset: 0x0004836F
		[DataSourceProperty]
		public string DefenderPartyFood
		{
			get
			{
				return this._defenderPartyFood;
			}
			set
			{
				if (value != this._defenderPartyFood)
				{
					this._defenderPartyFood = value;
					base.OnPropertyChangedWithValue<string>(value, "DefenderPartyFood");
				}
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x0600122C RID: 4652 RVA: 0x0004A192 File Offset: 0x00048392
		// (set) Token: 0x0600122D RID: 4653 RVA: 0x0004A19A File Offset: 0x0004839A
		[DataSourceProperty]
		public string AttackerPartyFood
		{
			get
			{
				return this._attackerPartyFood;
			}
			set
			{
				if (value != this._attackerPartyFood)
				{
					this._attackerPartyFood = value;
					base.OnPropertyChangedWithValue<string>(value, "AttackerPartyFood");
				}
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x0600122E RID: 4654 RVA: 0x0004A1BD File Offset: 0x000483BD
		// (set) Token: 0x0600122F RID: 4655 RVA: 0x0004A1C5 File Offset: 0x000483C5
		public string DefenderWallHitPoints
		{
			get
			{
				return this._defenderWallHitPoints;
			}
			set
			{
				if (value != this._defenderWallHitPoints)
				{
					this._defenderWallHitPoints = value;
					base.OnPropertyChangedWithValue<string>(value, "DefenderWallHitPoints");
				}
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x0004A1E8 File Offset: 0x000483E8
		// (set) Token: 0x06001231 RID: 4657 RVA: 0x0004A1F0 File Offset: 0x000483F0
		[DataSourceProperty]
		public bool IsNaval
		{
			get
			{
				return this._isNaval;
			}
			set
			{
				if (value != this._isNaval)
				{
					this._isNaval = value;
					base.OnPropertyChangedWithValue(value, "IsNaval");
				}
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001232 RID: 4658 RVA: 0x0004A20E File Offset: 0x0004840E
		// (set) Token: 0x06001233 RID: 4659 RVA: 0x0004A216 File Offset: 0x00048416
		[DataSourceProperty]
		public bool IsSiege
		{
			get
			{
				return this._isSiege;
			}
			set
			{
				if (value != this._isSiege)
				{
					this._isSiege = value;
					base.OnPropertyChangedWithValue(value, "IsSiege");
				}
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x0004A234 File Offset: 0x00048434
		// (set) Token: 0x06001235 RID: 4661 RVA: 0x0004A23C File Offset: 0x0004843C
		[DataSourceProperty]
		public string DefenderPartyCountLbl
		{
			get
			{
				return this._defenderPartyCountLbl;
			}
			set
			{
				if (value != this._defenderPartyCountLbl)
				{
					this._defenderPartyCountLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "DefenderPartyCountLbl");
				}
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x0004A25F File Offset: 0x0004845F
		// (set) Token: 0x06001237 RID: 4663 RVA: 0x0004A267 File Offset: 0x00048467
		[DataSourceProperty]
		public string AttackerPartyCountLbl
		{
			get
			{
				return this._attackerPartyCountLbl;
			}
			set
			{
				if (value != this._attackerPartyCountLbl)
				{
					this._attackerPartyCountLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "AttackerPartyCountLbl");
				}
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001238 RID: 4664 RVA: 0x0004A28A File Offset: 0x0004848A
		// (set) Token: 0x06001239 RID: 4665 RVA: 0x0004A292 File Offset: 0x00048492
		[DataSourceProperty]
		public HintViewModel AttackerBannerHint
		{
			get
			{
				return this._attackerBannerHint;
			}
			set
			{
				if (value != this._attackerBannerHint)
				{
					this._attackerBannerHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AttackerBannerHint");
				}
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x0600123A RID: 4666 RVA: 0x0004A2B0 File Offset: 0x000484B0
		// (set) Token: 0x0600123B RID: 4667 RVA: 0x0004A2B8 File Offset: 0x000484B8
		[DataSourceProperty]
		public HintViewModel DefenderBannerHint
		{
			get
			{
				return this._defenderBannerHint;
			}
			set
			{
				if (value != this._defenderBannerHint)
				{
					this._defenderBannerHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DefenderBannerHint");
				}
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x0600123C RID: 4668 RVA: 0x0004A2D6 File Offset: 0x000484D6
		// (set) Token: 0x0600123D RID: 4669 RVA: 0x0004A2DE File Offset: 0x000484DE
		[DataSourceProperty]
		public BasicTooltipViewModel AttackerTroopNumHint
		{
			get
			{
				return this._attackerTroopNumHint;
			}
			set
			{
				if (value != this._attackerTroopNumHint)
				{
					this._attackerTroopNumHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "AttackerTroopNumHint");
				}
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x0600123E RID: 4670 RVA: 0x0004A2FC File Offset: 0x000484FC
		// (set) Token: 0x0600123F RID: 4671 RVA: 0x0004A304 File Offset: 0x00048504
		[DataSourceProperty]
		public BasicTooltipViewModel DefenderTroopNumHint
		{
			get
			{
				return this._defenderTroopNumHint;
			}
			set
			{
				if (value != this._defenderTroopNumHint)
				{
					this._defenderTroopNumHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "DefenderTroopNumHint");
				}
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001240 RID: 4672 RVA: 0x0004A322 File Offset: 0x00048522
		// (set) Token: 0x06001241 RID: 4673 RVA: 0x0004A32A File Offset: 0x0004852A
		[DataSourceProperty]
		public BasicTooltipViewModel AttackerShipNumHint
		{
			get
			{
				return this._attackerShipNumHint;
			}
			set
			{
				if (value != this._attackerShipNumHint)
				{
					this._attackerShipNumHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "AttackerShipNumHint");
				}
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001242 RID: 4674 RVA: 0x0004A348 File Offset: 0x00048548
		// (set) Token: 0x06001243 RID: 4675 RVA: 0x0004A350 File Offset: 0x00048550
		[DataSourceProperty]
		public BasicTooltipViewModel DefenderShipNumHint
		{
			get
			{
				return this._defenderShipNumHint;
			}
			set
			{
				if (value != this._defenderShipNumHint)
				{
					this._defenderShipNumHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "DefenderShipNumHint");
				}
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001244 RID: 4676 RVA: 0x0004A36E File Offset: 0x0004856E
		// (set) Token: 0x06001245 RID: 4677 RVA: 0x0004A376 File Offset: 0x00048576
		[DataSourceProperty]
		public BasicTooltipViewModel DefenderWallHint
		{
			get
			{
				return this._defenderWallHint;
			}
			set
			{
				if (value != this._defenderWallHint)
				{
					this._defenderWallHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "DefenderWallHint");
				}
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001246 RID: 4678 RVA: 0x0004A394 File Offset: 0x00048594
		// (set) Token: 0x06001247 RID: 4679 RVA: 0x0004A39C File Offset: 0x0004859C
		[DataSourceProperty]
		public BasicTooltipViewModel DefenderFoodHint
		{
			get
			{
				return this._defenderFoodHint;
			}
			set
			{
				if (value != this._defenderFoodHint)
				{
					this._defenderFoodHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "DefenderFoodHint");
				}
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001248 RID: 4680 RVA: 0x0004A3BA File Offset: 0x000485BA
		// (set) Token: 0x06001249 RID: 4681 RVA: 0x0004A3C2 File Offset: 0x000485C2
		[DataSourceProperty]
		public BasicTooltipViewModel AttackerFoodHint
		{
			get
			{
				return this._attackerFoodHint;
			}
			set
			{
				if (value != this._attackerFoodHint)
				{
					this._attackerFoodHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "AttackerFoodHint");
				}
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x0600124A RID: 4682 RVA: 0x0004A3E0 File Offset: 0x000485E0
		// (set) Token: 0x0600124B RID: 4683 RVA: 0x0004A3E8 File Offset: 0x000485E8
		[DataSourceProperty]
		public BasicTooltipViewModel AttackerMoraleHint
		{
			get
			{
				return this._attackerMoraleHint;
			}
			set
			{
				if (value != this._attackerMoraleHint)
				{
					this._attackerMoraleHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "AttackerMoraleHint");
				}
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x0600124C RID: 4684 RVA: 0x0004A406 File Offset: 0x00048606
		// (set) Token: 0x0600124D RID: 4685 RVA: 0x0004A40E File Offset: 0x0004860E
		[DataSourceProperty]
		public BasicTooltipViewModel DefenderMoraleHint
		{
			get
			{
				return this._defenderMoraleHint;
			}
			set
			{
				if (value != this._defenderMoraleHint)
				{
					this._defenderMoraleHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "DefenderMoraleHint");
				}
			}
		}

		// Token: 0x04000832 RID: 2098
		private GameMenuPartyItemVM _defenderLeadingParty;

		// Token: 0x04000833 RID: 2099
		private GameMenuPartyItemVM _attackerLeadingParty;

		// Token: 0x04000834 RID: 2100
		private string _titleText;

		// Token: 0x04000835 RID: 2101
		private BannerImageIdentifierVM _defenderPartyBanner;

		// Token: 0x04000836 RID: 2102
		private BannerImageIdentifierVM _attackerPartyBanner;

		// Token: 0x04000837 RID: 2103
		private MBBindingList<GameMenuPartyItemVM> _attackerPartyList;

		// Token: 0x04000838 RID: 2104
		private MBBindingList<GameMenuPartyItemVM> _defenderPartyList;

		// Token: 0x04000839 RID: 2105
		private string _attackerPartyMorale;

		// Token: 0x0400083A RID: 2106
		private string _defenderPartyMorale;

		// Token: 0x0400083B RID: 2107
		private int _attackerPartyCount;

		// Token: 0x0400083C RID: 2108
		private int _defenderPartyCount;

		// Token: 0x0400083D RID: 2109
		private int _defenderShipCount;

		// Token: 0x0400083E RID: 2110
		private int _attackerShipCount;

		// Token: 0x0400083F RID: 2111
		private string _attackerPartyFood;

		// Token: 0x04000840 RID: 2112
		private string _defenderPartyFood;

		// Token: 0x04000841 RID: 2113
		private string _defenderWallHitPoints;

		// Token: 0x04000842 RID: 2114
		private string _defenderPartyCountLbl;

		// Token: 0x04000843 RID: 2115
		private string _attackerPartyCountLbl;

		// Token: 0x04000844 RID: 2116
		private bool _isNaval;

		// Token: 0x04000845 RID: 2117
		private bool _isSiege;

		// Token: 0x04000846 RID: 2118
		private PowerLevelComparer _powerComparer;

		// Token: 0x04000847 RID: 2119
		private HintViewModel _attackerBannerHint;

		// Token: 0x04000848 RID: 2120
		private HintViewModel _defenderBannerHint;

		// Token: 0x04000849 RID: 2121
		private BasicTooltipViewModel _attackerTroopNumHint;

		// Token: 0x0400084A RID: 2122
		private BasicTooltipViewModel _defenderTroopNumHint;

		// Token: 0x0400084B RID: 2123
		private BasicTooltipViewModel _attackerShipNumHint;

		// Token: 0x0400084C RID: 2124
		private BasicTooltipViewModel _defenderShipNumHint;

		// Token: 0x0400084D RID: 2125
		private BasicTooltipViewModel _defenderWallHint;

		// Token: 0x0400084E RID: 2126
		private BasicTooltipViewModel _defenderFoodHint;

		// Token: 0x0400084F RID: 2127
		private BasicTooltipViewModel _attackerFoodHint;

		// Token: 0x04000850 RID: 2128
		private BasicTooltipViewModel _attackerMoraleHint;

		// Token: 0x04000851 RID: 2129
		private BasicTooltipViewModel _defenderMoraleHint;
	}
}
