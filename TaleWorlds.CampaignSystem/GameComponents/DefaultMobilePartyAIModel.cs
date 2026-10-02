using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000135 RID: 309
	public class DefaultMobilePartyAIModel : MobilePartyAIModel
	{
		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x0600197D RID: 6525 RVA: 0x0007D424 File Offset: 0x0007B624
		public override float AiCheckInterval
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x0600197E RID: 6526 RVA: 0x0007D42B File Offset: 0x0007B62B
		public override float FleeToNearbyPartyRadius
		{
			get
			{
				return Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius * Campaign.Current.EstimatedMaximumLordPartySpeedExceptPlayer * this.AiCheckInterval * 1.5f;
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x0600197F RID: 6527 RVA: 0x0007D459 File Offset: 0x0007B659
		public override float FleeToNearbySettlementRadius
		{
			get
			{
				return this.FleeToNearbyPartyRadius * 2f;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x0007D467 File Offset: 0x0007B667
		public override float HideoutPatrolDistanceAsDays
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06001981 RID: 6529 RVA: 0x0007D46E File Offset: 0x0007B66E
		public override float FortificationPatrolDistanceAsDays
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06001982 RID: 6530 RVA: 0x0007D475 File Offset: 0x0007B675
		public override float FortificationPortPatrolDistanceAsDays
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06001983 RID: 6531 RVA: 0x0007D47C File Offset: 0x0007B67C
		public override float VillagePatrolDistanceAsDays
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x0007D484 File Offset: 0x0007B684
		public override bool ShouldConsiderAttacking(MobileParty party, MobileParty targetParty)
		{
			bool flag = targetParty != MobileParty.MainParty || !MobileParty.MainParty.ShouldBeIgnored;
			bool flag2 = targetParty != MobileParty.MainParty || party.Ai.DoNotAttackMainPartyUntil.IsPast;
			bool flag3 = party.IsCurrentlyAtSea == targetParty.IsCurrentlyAtSea;
			bool flag4 = targetParty.IsCurrentlyAtSea && party.CurrentSettlement != null && party.CurrentSettlement.IsFortification && party.CurrentSettlement.HasPort && party.HasNavalNavigationCapability;
			return flag && flag2 && (flag3 || flag4) && MobilePartyHelper.CanPartyAttackWithCurrentMorale(party);
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06001985 RID: 6533 RVA: 0x0007D51C File Offset: 0x0007B71C
		public override float SettlementDefendingNearbyPartyCheckRadius
		{
			get
			{
				return this.SettlementDefendingWaitingPositionRadius * 3f;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06001986 RID: 6534 RVA: 0x0007D52A File Offset: 0x0007B72A
		public override float SettlementDefendingWaitingPositionRadius
		{
			get
			{
				return Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius * 0.8f;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06001987 RID: 6535 RVA: 0x0007D546 File Offset: 0x0007B746
		public override float NeededFoodsInDaysThresholdForSiege
		{
			get
			{
				return 12f;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06001988 RID: 6536 RVA: 0x0007D54D File Offset: 0x0007B74D
		public override float NeededFoodsInDaysThresholdForRaid
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0007D554 File Offset: 0x0007B754
		public override bool ShouldConsiderAvoiding(MobileParty party, MobileParty targetParty)
		{
			return (targetParty.SiegeEvent == null || !targetParty.SiegeEvent.BesiegedSettlement.HasPort || targetParty.SiegeEvent.IsBlockadeActive || !party.IsTargetingPort) && (targetParty.IsMainParty || MobilePartyHelper.CanPartyAttackWithCurrentMorale(targetParty)) && ((targetParty.Aggressiveness > 0.01f && !targetParty.IsInNavalAutoTravel) || targetParty.IsGarrison);
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x0007D5C4 File Offset: 0x0007B7C4
		public override float GetPatrolRadius(MobileParty mobileParty, CampaignVec2 patrolPoint)
		{
			float num = 0f;
			if (mobileParty.TargetSettlement != null)
			{
				num = this.GetPatrolRadiusInternal(mobileParty.TargetSettlement, mobileParty._lastCalculatedSpeed, !patrolPoint.IsOnLand);
				if (mobileParty.IsPatrolParty)
				{
					num *= 0.5f;
				}
			}
			return num;
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x0007D60C File Offset: 0x0007B80C
		public override float GetSettlementNearbyThreatAndAllyCheckRadius(Settlement settlement, bool isPort)
		{
			float num = (isPort ? Campaign.Current.EstimatedAverageLordPartyNavalSpeed : Campaign.Current.EstimatedAverageLordPartySpeed);
			return this.GetPatrolRadiusInternal(settlement, num * 1.3f, isPort);
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x0007D644 File Offset: 0x0007B844
		public override bool ShouldPartyCheckInitiativeBehavior(MobileParty mobileParty)
		{
			return (mobileParty.CurrentSettlement == null || (!mobileParty.IsGarrison && !mobileParty.IsMilitia && !mobileParty.IsBandit && (!mobileParty.IsLordParty || mobileParty.LeaderHero != null))) && (mobileParty != MobileParty.MainParty && mobileParty.BesiegedSettlement == null) && (mobileParty.Army == null || !mobileParty.Army.LeaderParty.AttachedParties.Contains(mobileParty));
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x0007D6B8 File Offset: 0x0007B8B8
		public override void GetBestInitiativeBehavior(MobileParty mobileParty, out AiBehavior bestInitiativeBehavior, out MobileParty bestInitiativeTargetParty, out float bestInitiativeBehaviorScore, out Vec2 averageEnemyVec)
		{
			MobilePartyAi.DangerousPartiesAndTheirVecs.Clear();
			bestInitiativeBehaviorScore = 0f;
			bestInitiativeTargetParty = null;
			bestInitiativeBehavior = AiBehavior.None;
			averageEnemyVec = Vec2.Zero;
			float getEncounterJoiningRadius = Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius;
			float num = 2f;
			if (mobileParty.DefaultBehavior == AiBehavior.PatrolAroundPoint && !mobileParty.AiBehaviorTarget.IsOnLand && mobileParty.IsCurrentlyAtSea)
			{
				num *= 2f;
			}
			float num2 = getEncounterJoiningRadius * (num + 1f);
			CampaignVec2 campaignVec = mobileParty.Position;
			if (mobileParty.CurrentSettlement != null)
			{
				campaignVec = mobileParty.CurrentSettlement.Position;
			}
			LocatableSearchData<MobileParty> locatableSearchData = MobileParty.StartFindingLocatablesAroundPosition(campaignVec.ToVec2(), num2);
			MobileParty mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
			while (mobileParty2 != null)
			{
				if (mobileParty2.MapEvent != null && MobileParty.MainParty.MapEvent == mobileParty2.MapEvent && (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty) && mobileParty2 != MobileParty.MainParty)
				{
					mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
				}
				else
				{
					if (!mobileParty2.IsGarrison)
					{
						if ((mobileParty.CurrentSettlement == null || !mobileParty.CurrentSettlement.HasPort) && mobileParty.IsCurrentlyAtSea != mobileParty2.IsCurrentlyAtSea)
						{
							mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
							continue;
						}
						if ((mobileParty2.IsCurrentlyAtSea && !mobileParty.HasNavalNavigationCapability) || (!mobileParty2.IsCurrentlyAtSea && !mobileParty.HasLandNavigationCapability))
						{
							mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
							continue;
						}
					}
					if (mobileParty.IsLordParty && mobileParty2.IsBandit && mobileParty.DefaultBehavior == AiBehavior.PatrolAroundPoint && !mobileParty.TargetPosition.IsOnLand && !mobileParty.IsCurrentlyAtSea)
					{
						mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
					}
					else
					{
						if (mobileParty2 != mobileParty && mobileParty2.IsActive && this.IsEnemy(mobileParty2.Party, mobileParty) && !mobileParty2.ShouldBeIgnored && (mobileParty2.CurrentSettlement == null || mobileParty2.IsGarrison || mobileParty2.IsLordParty))
						{
							Settlement currentSettlement = mobileParty.CurrentSettlement;
							if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) == null && (!mobileParty2.IsGarrison || mobileParty.IsBandit) && (mobileParty2.BesiegerCamp == null || mobileParty2.BesiegerCamp.LeaderParty == mobileParty2) && (mobileParty2.Army == null || mobileParty2.Army.LeaderParty == mobileParty2 || mobileParty2.AttachedTo == null) && (mobileParty2.MapEvent == null || mobileParty2 == MobileParty.MainParty || mobileParty2.Party.MapEvent.MapEventSettlement != null || mobileParty2.Party == mobileParty2.Party.MapEvent.GetLeaderParty(BattleSideEnum.Attacker) || mobileParty2.Party == mobileParty2.Party.MapEvent.GetLeaderParty(BattleSideEnum.Defender)) && (mobileParty2.MapEvent == null || this.IsEnemy(mobileParty2.MapEvent.AttackerSide.LeaderParty, mobileParty) != this.IsEnemy(mobileParty2.MapEvent.DefenderSide.LeaderParty, mobileParty)) && (mobileParty2.CurrentSettlement == null || !mobileParty2.CurrentSettlement.IsHideout || !mobileParty.IsBandit))
							{
								if (mobileParty.Army != null && mobileParty.AttachedTo == null && mobileParty.Army.LeaderParty != mobileParty && mobileParty2.MapEvent != null && mobileParty2.MapEventSide.OtherSide.LeaderParty.IsMobile && mobileParty2.MapEventSide.OtherSide.LeaderParty.MobileParty.Army != null && mobileParty2.MapEventSide.OtherSide.LeaderParty.MobileParty.Army == mobileParty.Army)
								{
									mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
									continue;
								}
								CampaignVec2 campaignVec2;
								if (mobileParty.DefaultBehavior == AiBehavior.DefendSettlement && mobileParty.IsCurrentlyAtSea && mobileParty.IsTargetingPort)
								{
									campaignVec2 = mobileParty.TargetSettlement.PortPosition;
									num = Campaign.Current.Models.MobilePartyAIModel.SettlementDefendingNearbyPartyCheckRadius;
								}
								else
								{
									campaignVec2 = mobileParty2.Position;
									float num3;
									float num4;
									if (!DistanceHelper.FindClosestDistanceFromMobilePartyToMobileParty(mobileParty, mobileParty2, mobileParty.NavigationCapability, getEncounterJoiningRadius * num * 10f, out num3, out num4))
									{
										mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
										continue;
									}
								}
								float num5 = mobileParty.Position.Distance(campaignVec2);
								if (bestInitiativeTargetParty != null && mobileParty.IsLordParty && !mobileParty2.IsLordParty && bestInitiativeBehavior == AiBehavior.EngageParty && bestInitiativeTargetParty.IsLordParty)
								{
									mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
									continue;
								}
								if (mobileParty2.SiegeEvent != null && mobileParty2.MapEvent != null && mobileParty2.MapEvent.IsBlockade && !mobileParty.IsCurrentlyAtSea)
								{
									mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
									continue;
								}
								if (mobileParty.Army != null && mobileParty.AttachedTo == null)
								{
									if (mobileParty.Army.LeaderParty.DefaultBehavior == AiBehavior.DefendSettlement && mobileParty2.SiegeEvent != null && mobileParty2.SiegeEvent.BesiegedSettlement == mobileParty.Army.LeaderParty.TargetSettlement)
									{
										mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
										continue;
									}
									if (mobileParty2.IsBandit)
									{
										mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
										continue;
									}
									MobileParty mobileParty3 = mobileParty2.AttachedTo ?? mobileParty2;
									if (mobileParty.Army.LeaderParty != mobileParty)
									{
										if (!mobileParty3.IsFleeing() || mobileParty3.ShortTermTargetParty != mobileParty.Army.LeaderParty)
										{
											Army army = mobileParty3.Army;
											if (((army != null) ? army.EstimatedStrength : mobileParty3.Party.EstimatedStrength) >= mobileParty.Army.EstimatedStrength)
											{
												goto IL_058B;
											}
										}
										mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
										continue;
									}
								}
								IL_058B:
								float num6 = 1f + MathF.Max(0f, (num5 - 1f) / ((getEncounterJoiningRadius - 1f) * 2f));
								num6 = ((num6 > num) ? num : num6);
								float num7 = ((mobileParty.Army != null && (mobileParty.AttachedTo != null || mobileParty.Army.LeaderParty == mobileParty)) ? mobileParty.Army.EstimatedStrength : mobileParty.Party.EstimatedStrength) + 0.01f;
								if (mobileParty2.IsCurrentlyAtSea != mobileParty.IsCurrentlyAtSea)
								{
									num7 = ((mobileParty.Army != null && (mobileParty.AttachedTo != null || mobileParty.Army.LeaderParty == mobileParty)) ? mobileParty.Army.GetCustomStrength(BattleSideEnum.Attacker, MapEvent.PowerCalculationContext.SeaBattle) : mobileParty.Party.GetCustomStrength(BattleSideEnum.Attacker, MapEvent.PowerCalculationContext.SeaBattle)) + 0.01f;
								}
								float num8 = mobileParty.Aggressiveness;
								float num9 = 0f;
								float num10 = 0.01f;
								if (mobileParty2.BesiegerCamp != null)
								{
									bool isCurrentlyAtSea = mobileParty.IsCurrentlyAtSea;
									MapEvent.PowerCalculationContext powerCalculationContext = (isCurrentlyAtSea ? MapEvent.PowerCalculationContext.SeaBattle : Campaign.Current.Models.MilitaryPowerModel.GetContextForPosition(mobileParty2.SiegeEvent.BesiegerCamp.LeaderParty.Position));
									using (IEnumerator<PartyBase> enumerator = mobileParty2.SiegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType(isCurrentlyAtSea ? MapEvent.BattleTypes.BlockadeBattle : MapEvent.BattleTypes.Siege).GetEnumerator())
									{
										while (enumerator.MoveNext())
										{
											PartyBase partyBase = enumerator.Current;
											num10 += partyBase.GetCustomStrength(BattleSideEnum.Defender, powerCalculationContext);
										}
										goto IL_0758;
									}
									goto IL_0700;
								}
								goto IL_0700;
								IL_0758:
								bool flag = false;
								LocatableSearchData<MobileParty> locatableSearchData2 = MobileParty.StartFindingLocatablesAroundPosition(mobileParty.Position.ToVec2(), getEncounterJoiningRadius * (num + 1f));
								MobileParty mobileParty4 = MobileParty.FindNextLocatable(ref locatableSearchData2);
								float num11 = 0f;
								while (mobileParty4 != null)
								{
									if ((mobileParty.MapFaction == mobileParty4.MapFaction && mobileParty4.BesiegedSettlement != null) || (mobileParty4.MapEvent != null && mobileParty4.MapEvent != mobileParty2.MapEvent))
									{
										mobileParty4 = MobileParty.FindNextLocatable(ref locatableSearchData2);
									}
									else if (mobileParty4.AttachedTo != null)
									{
										mobileParty4 = MobileParty.FindNextLocatable(ref locatableSearchData2);
									}
									else if (mobileParty4.IsCurrentlyAtSea != mobileParty.IsCurrentlyAtSea)
									{
										mobileParty4 = MobileParty.FindNextLocatable(ref locatableSearchData2);
									}
									else if (mobileParty4.IsInNavalAutoTravel)
									{
										mobileParty4 = MobileParty.FindNextLocatable(ref locatableSearchData2);
									}
									else if (mobileParty4.CurrentSettlement != null && mobileParty4.CurrentSettlement.SiegeEvent != null)
									{
										mobileParty4 = MobileParty.FindNextLocatable(ref locatableSearchData2);
									}
									else
									{
										if ((mobileParty4.IsMainParty && mobileParty4.Party.EstimatedStrength > mobileParty.Party.EstimatedStrength) || (mobileParty4.ShortTermBehavior == AiBehavior.EngageParty && mobileParty4.ShortTermTargetParty == mobileParty && mobileParty4.MapFaction != mobileParty2.MapFaction))
										{
											flag = true;
											break;
										}
										if (mobileParty4 != mobileParty && mobileParty4 != mobileParty2)
										{
											Vec2 vec = ((mobileParty4.BesiegedSettlement != null) ? mobileParty4.VisualPosition2DWithoutError : mobileParty4.Position.ToVec2());
											float num12 = ((mobileParty4 != mobileParty2) ? vec.Distance(campaignVec2.ToVec2()) : mobileParty.Position.Distance(vec));
											if (num12 > num * getEncounterJoiningRadius)
											{
												mobileParty4 = MobileParty.FindNextLocatable(ref locatableSearchData2);
												continue;
											}
											if (mobileParty4.BesiegerCamp != null && mobileParty4.BesiegerCamp.LeaderParty != mobileParty4)
											{
												mobileParty4 = MobileParty.FindNextLocatable(ref locatableSearchData2);
												continue;
											}
											if (mobileParty4.IsGarrison || mobileParty4.IsMilitia)
											{
												mobileParty4 = MobileParty.FindNextLocatable(ref locatableSearchData2);
												continue;
											}
											PartyBase partyBase2 = mobileParty4.Ai.AiBehaviorPartyBase;
											if (mobileParty4.Army != null)
											{
												partyBase2 = mobileParty4.Army.LeaderParty.Ai.AiBehaviorPartyBase;
											}
											bool flag2 = partyBase2 != null && (partyBase2 == mobileParty2.Party || (partyBase2.MapEvent != null && partyBase2.MapEvent == mobileParty2.Party.MapEvent));
											bool flag3 = (mobileParty.Army != null && mobileParty.Army == mobileParty4.Army && mobileParty.Army.DoesLeaderPartyAndAttachedPartiesContain(mobileParty)) || (mobileParty2.Army != null && mobileParty2.Army == mobileParty4.Army) || (mobileParty2.BesiegedSettlement != null && mobileParty2.BesiegedSettlement == mobileParty4.BesiegedSettlement) || (num5 > getEncounterJoiningRadius && flag2) || (num12 > getEncounterJoiningRadius && flag2 && mobileParty2 != MobileParty.MainParty && (MobileParty.MainParty.Army == null || mobileParty2 != MobileParty.MainParty.Army.LeaderParty));
											if (flag3 || num12 < getEncounterJoiningRadius * num6)
											{
												float num13 = (flag3 ? 1f : ((num12 < getEncounterJoiningRadius) ? 1f : (1f - (num12 - getEncounterJoiningRadius) / (getEncounterJoiningRadius * (num6 - 1f)))));
												num13 = MathF.Min(1f, num13);
												bool flag4 = mobileParty2.MapEvent != null && mobileParty2.MapEvent == mobileParty4.MapEvent;
												float num14 = ((mobileParty4.Army != null && (mobileParty4.AttachedTo != null || mobileParty4.Army.LeaderParty == mobileParty4)) ? mobileParty4.Army.EstimatedStrength : mobileParty4.Party.EstimatedStrength);
												if (mobileParty4.IsGarrison && !mobileParty.IsLordParty)
												{
													num11 += MathF.Max(mobileParty4.Party.EstimatedStrength, 250f);
												}
												if ((mobileParty4.Aggressiveness > 0.01f || mobileParty4.IsGarrison || flag4) && mobileParty4.MapFaction == mobileParty2.MapFaction)
												{
													if (mobileParty4.BesiegerCamp != null)
													{
														using (IEnumerator<PartyBase> enumerator = mobileParty4.SiegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType(mobileParty.IsCurrentlyAtSea ? MapEvent.BattleTypes.BlockadeBattle : MapEvent.BattleTypes.Siege).GetEnumerator())
														{
															while (enumerator.MoveNext())
															{
																PartyBase partyBase3 = enumerator.Current;
																bool flag5 = mobileParty.DefaultBehavior == AiBehavior.DefendSettlement && partyBase3.SiegeEvent.BesiegedSettlement == mobileParty.TargetSettlement;
																num11 += partyBase3.EstimatedStrength * (flag5 ? 0.2f : 1f);
															}
															goto IL_0BF1;
														}
													}
													num11 += num14 * num13;
												}
												IL_0BF1:
												if (mobileParty.MapFaction == mobileParty4.MapFaction && !mobileParty4.IsMainParty)
												{
													bool flag6 = mobileParty4.Aggressiveness > 0.01f || (mobileParty4.CurrentSettlement != null && mobileParty4.CurrentSettlement == mobileParty2.CurrentSettlement);
													bool flag7 = mobileParty2 != MobileParty.MainParty || Campaign.Current.Models.MobilePartyAIModel.ShouldConsiderAttacking(mobileParty4, MobileParty.MainParty);
													bool flag8 = mobileParty4.CurrentSettlement == null || !mobileParty4.CurrentSettlement.IsHideout;
													if (flag4 || (flag6 && flag7 && flag8))
													{
														Settlement currentSettlement2 = mobileParty4.CurrentSettlement;
														if (((currentSettlement2 != null) ? currentSettlement2.SiegeEvent : null) == null || mobileParty2 != mobileParty4.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty)
														{
															if (mobileParty4.BesiegerCamp != null)
															{
																using (IEnumerator<PartyBase> enumerator = mobileParty4.SiegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege).GetEnumerator())
																{
																	while (enumerator.MoveNext())
																	{
																		PartyBase partyBase4 = enumerator.Current;
																		num7 += partyBase4.EstimatedStrength;
																		if (partyBase4.MobileParty.Aggressiveness > num8)
																		{
																			num8 = partyBase4.MobileParty.Aggressiveness;
																		}
																	}
																	goto IL_0D66;
																}
															}
															num7 += num14 * num13;
															if (mobileParty4.Aggressiveness > num8)
															{
																num8 = mobileParty4.Aggressiveness;
															}
															if (mobileParty4.CurrentSettlement != null)
															{
																num9 += num14 * num13;
															}
														}
													}
												}
											}
										}
										IL_0D66:
										mobileParty4 = MobileParty.FindNextLocatable(ref locatableSearchData2);
									}
								}
								num10 += num11 * 0.9f;
								if (mobileParty.CurrentSettlement != null)
								{
									num7 -= num9;
								}
								if (mobileParty2.LastVisitedSettlement != null && mobileParty2.LastVisitedSettlement.IsVillage && mobileParty2.Position.DistanceSquared(mobileParty2.LastVisitedSettlement.Position) < 1f && mobileParty2.LastVisitedSettlement.MapFaction.IsAtWarWith(mobileParty.MapFaction))
								{
									num10 += 20f;
								}
								float num15 = num7 / num10;
								num15 *= (((mobileParty.IsCaravan || mobileParty.IsVillager) && mobileParty2 == MobileParty.MainParty) ? 0.6f : 1f);
								num15 *= (mobileParty.IsPatrolParty ? (mobileParty2.IsBandit ? 1.2f : (mobileParty2.IsLordParty ? 0.9f : (mobileParty2.IsPatrolParty ? 0.8f : 1f))) : 1f);
								if (mobileParty2.IsCaravan && mobileParty.LeaderHero != null && mobileParty.LeaderHero.IsMinorFactionHero)
								{
									num15 *= 1.5f;
								}
								if (mobileParty2.MapEvent != null && mobileParty2.MapEvent.IsSiegeAssault && mobileParty2 == mobileParty2.MapEvent.AttackerSide.LeaderParty.MobileParty)
								{
									float settlementAdvantage = Campaign.Current.Models.CombatSimulationModel.GetSettlementAdvantage(mobileParty2.MapEvent.MapEventSettlement);
									if (num9 * MathF.Sqrt(settlementAdvantage) > num10)
									{
										mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
										continue;
									}
								}
								if (num15 > 1f && num5 >= getEncounterJoiningRadius * num * 3f)
								{
									mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
									continue;
								}
								float num16;
								float num17;
								this.CalculateInitiativeScoresForEnemy(mobileParty, mobileParty2, out num16, out num17, num15, num8);
								if (flag)
								{
									num17 = 0f;
								}
								if (mobileParty2.CurrentSettlement != null && mobileParty2.MapEvent == null)
								{
									num17 = 0f;
								}
								if (num15 > 2f && mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty && mobileParty2.AttachedParties.Count == 0 && !mobileParty.Army.IsWaitingForArmyMembers() && (mobileParty.DefaultBehavior != AiBehavior.GoAroundParty || mobileParty.TargetParty != mobileParty2))
								{
									num17 = 0f;
									num16 = 0f;
								}
								if (num16 > 1f)
								{
									MobilePartyAi.DangerousPartiesAndTheirVecs.Add(new ValueTuple<float, Vec2>(num16, (campaignVec2.ToVec2() - mobileParty.Position.ToVec2()).Normalized()));
								}
								if (num16 > bestInitiativeBehaviorScore || (num16 * 0.75f > bestInitiativeBehaviorScore && bestInitiativeBehavior == AiBehavior.EngageParty))
								{
									bestInitiativeBehavior = AiBehavior.FleeToPoint;
									bestInitiativeTargetParty = mobileParty2;
									bestInitiativeBehaviorScore = num16;
								}
								if (num17 > bestInitiativeBehaviorScore && (bestInitiativeBehaviorScore < num17 * 0.75f || bestInitiativeBehavior == AiBehavior.EngageParty))
								{
									bestInitiativeBehavior = AiBehavior.EngageParty;
									bestInitiativeTargetParty = mobileParty2;
									bestInitiativeBehaviorScore = num17;
								}
								mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
								continue;
								IL_0700:
								if (mobileParty2.CurrentSettlement == null || !mobileParty2.CurrentSettlement.IsUnderSiege)
								{
									num10 += ((mobileParty2.Army != null && (mobileParty2.AttachedTo != null || mobileParty2.Army.LeaderParty == mobileParty2)) ? mobileParty2.Army.EstimatedStrength : mobileParty2.Party.EstimatedStrength);
									goto IL_0758;
								}
								goto IL_0758;
							}
						}
						mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
					}
				}
			}
			if (bestInitiativeBehavior == AiBehavior.FleeToPoint || bestInitiativeBehavior == AiBehavior.FleeToGate)
			{
				float num18 = 0f;
				for (int i = 0; i < 8; i++)
				{
					Vec2 vec2 = new Vec2(MathF.Sin((float)i / 8f * 3.1415927f * 2f), MathF.Cos((float)i / 8f * 3.1415927f * 2f));
					float num19 = 0f;
					for (int j = 0; j < MobilePartyAi.DangerousPartiesAndTheirVecs.Count; j++)
					{
						Vec2 item = MobilePartyAi.DangerousPartiesAndTheirVecs[j].Item2;
						float num20 = item.DistanceSquared(vec2);
						if (num20 > 1f)
						{
							num20 = 1f + (num20 - 1f) * 0.5f;
						}
						num19 += num20 * MobilePartyAi.DangerousPartiesAndTheirVecs[j].Item1;
					}
					if (num19 > num18)
					{
						averageEnemyVec = -vec2;
						num18 = num19;
					}
				}
			}
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x0007E834 File Offset: 0x0007CA34
		private bool IsEnemy(PartyBase party, MobileParty mobileParty)
		{
			return FactionManager.IsAtWarAgainstFaction(party.MapFaction, mobileParty.MapFaction);
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x0007E848 File Offset: 0x0007CA48
		private void CalculateInitiativeScoresForEnemy(MobileParty mobileParty, MobileParty enemyParty, out float avoidScore, out float attackScore, float localAdvantage, float maxAggressiveness)
		{
			attackScore = 0f;
			avoidScore = 0f;
			float num = Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius * 1.2f;
			CampaignVec2 campaignVec = mobileParty.Position;
			if (!mobileParty.IsCurrentlyAtSea && enemyParty.IsCurrentlyAtSea)
			{
				campaignVec = mobileParty.CurrentSettlement.PortPosition;
			}
			float length = (enemyParty.Position.ToVec2() - campaignVec.ToVec2()).Length;
			float num2 = this.CalculateStanceScore(mobileParty, enemyParty);
			float num3 = MBMath.ClampFloat(0.5f * (1f + localAdvantage), 0.05f, 3f);
			float num4 = MBMath.ClampFloat((localAdvantage < 1f) ? MBMath.ClampFloat(1f / localAdvantage, 0.05f, 3f) : 0f, 0.05f, 3f);
			if (Campaign.Current.Models.MobilePartyAIModel.ShouldConsiderAttacking(mobileParty, enemyParty) && num3 > num4)
			{
				float num5 = 1f;
				float initiativeDistanceForAttack = this.GetInitiativeDistanceForAttack(mobileParty, enemyParty, num);
				float num6 = ((mobileParty.IsBandit && mobileParty.HasNavalNavigationCapability) ? 10f : 5f);
				if (mobileParty.IsCurrentlyAtSea && mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty)
				{
					num6 *= 0.5f;
				}
				float num7 = (mobileParty.IsCurrentlyAtSea ? Campaign.Current.Models.EncounterModel.NeededMaximumNavalDistanceForEncounteringMobileParty : Campaign.Current.Models.EncounterModel.NeededMaximumLandDistanceForEncounteringMobileParty);
				if (length < num7 * num6 || (mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty && enemyParty.Army != null && enemyParty.Army.LeaderParty == enemyParty && initiativeDistanceForAttack * 2f > length))
				{
					num5 = 100f;
				}
				else if (enemyParty.IsMoving && enemyParty.SiegeEvent == null && enemyParty.MapEvent == null)
				{
					float num8 = mobileParty.LastCalculatedBaseSpeed - enemyParty.LastCalculatedBaseSpeed;
					if (num8 > 0.01f)
					{
						float num9 = initiativeDistanceForAttack / num8;
						float num10 = (float)CampaignTime.HoursInDay * 0.75f;
						if (num9 < num10)
						{
							num5 = num10 / num9;
						}
					}
					else
					{
						num5 = 0f;
					}
				}
				float num11 = ((enemyParty.IsLordParty && enemyParty.LeaderHero != null && enemyParty.LeaderHero.IsLord) ? 1f : mobileParty.Ai.AttackInitiative);
				if ((double)mobileParty.Aggressiveness < 0.01)
				{
					maxAggressiveness = mobileParty.Aggressiveness;
				}
				float num12 = ((enemyParty.MapEvent != null && maxAggressiveness > 0.1f) ? MathF.Max(1f + (enemyParty.MapEvent.IsSallyOut ? 0.3f : 0f), maxAggressiveness) : maxAggressiveness);
				float num13 = ((mobileParty.DefaultBehavior == AiBehavior.DefendSettlement && ((enemyParty.BesiegedSettlement != null && mobileParty.Ai.AiBehaviorPartyBase == enemyParty.BesiegedSettlement.Party) || (enemyParty.MapEvent != null && enemyParty.MapEvent.MapEventSettlement != null && mobileParty.Ai.AiBehaviorPartyBase == enemyParty.MapEvent.MapEventSettlement.Party))) ? 1.1f : 1f);
				float num14 = 1f;
				if (mobileParty.IsLordParty && mobileParty.DefaultBehavior == AiBehavior.PatrolAroundPoint && num3 * 0.8f > num4)
				{
					MobileParty.NavigationType navigationType = (mobileParty.HasNavalNavigationCapability ? MobileParty.NavigationType.All : MobileParty.NavigationType.Default);
					num14 += 0.2f * (Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(navigationType) * 0.5f) / mobileParty.AiBehaviorTarget.Distance(mobileParty.Position);
				}
				float num15 = ((enemyParty.MapEvent != null && enemyParty.MapEventSide.OtherSide.Parties.ContainsQ<MapEventParty>((MapEventParty x) => x.Party.IsMobile && x.Party.MapFaction == mobileParty.MapFaction)) ? 1.2f : 1f);
				attackScore = 1.06f * num13 * num3 * num2 * num5 * num12 * num14 * num11 * num15;
			}
			if (attackScore < 1f)
			{
				if (enemyParty.IsGarrison)
				{
					attackScore = 0f;
					if (enemyParty == mobileParty.ShortTermTargetParty)
					{
						mobileParty.RecalculateShortTermBehavior();
					}
				}
				if (Campaign.Current.Models.MobilePartyAIModel.ShouldConsiderAvoiding(mobileParty, enemyParty))
				{
					float num16 = ((mobileParty.IsCaravan || mobileParty.IsVillager) ? 0.9f : ((enemyParty.IsGarrison || enemyParty.IsMilitia || enemyParty.CurrentSettlement != null) ? 0.4f : 0.7f));
					float num17 = num * num16;
					if (enemyParty.MapEvent != null || enemyParty.BesiegedSettlement != null || (mobileParty.DefaultBehavior == AiBehavior.EngageParty && mobileParty.TargetParty == enemyParty) || (mobileParty.DefaultBehavior == AiBehavior.GoAroundParty && mobileParty.TargetParty == enemyParty))
					{
						num17 = num * 0.6f;
					}
					num17 *= (1f + mobileParty.Ai.AvoidInitiative) / 2f;
					float num18 = 1f;
					if (length < num17 * 4f)
					{
						float num19 = length / (num17 + 1E-05f);
						num18 = 4f - num19;
					}
					float num20 = ((enemyParty.IsLordParty && enemyParty.LeaderHero != null && enemyParty.LeaderHero.IsLord) ? 1f : mobileParty.Ai.AvoidInitiative);
					avoidScore = 0.9433963f * num20 * num18 * ((num2 > 0.01f) ? 1f : 0f) * num4;
				}
			}
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0007EE70 File Offset: 0x0007D070
		private float GetInitiativeDistanceForAttack(MobileParty mobileParty, MobileParty enemyParty, float reasonableDistance)
		{
			float num = 1f;
			if (enemyParty.IsCaravan)
			{
				num = (mobileParty.IsBandit ? 2f : ((mobileParty.Army == null) ? 1.5f : 1f));
			}
			else if (enemyParty.Aggressiveness < 0.1f)
			{
				num = 0.7f;
			}
			else if (enemyParty.IsBandit || enemyParty.IsLordParty)
			{
				num = ((mobileParty.DefaultBehavior == AiBehavior.PatrolAroundPoint) ? 3.5f : 1f);
			}
			else if ((mobileParty.DefaultBehavior == AiBehavior.GoAroundParty || mobileParty.ShortTermBehavior == AiBehavior.GoAroundParty) && enemyParty != mobileParty.TargetParty)
			{
				num = 0.7f;
			}
			if (enemyParty.MapEvent == null && mobileParty._lastCalculatedSpeed < enemyParty._lastCalculatedSpeed * 1.1f && (mobileParty.DefaultBehavior != AiBehavior.GoAroundParty || mobileParty.TargetParty != enemyParty) && (mobileParty.DefaultBehavior != AiBehavior.DefendSettlement || enemyParty != mobileParty.TargetSettlement.LastAttackerParty))
			{
				float num2 = MathF.Max(0.5f, (mobileParty._lastCalculatedSpeed + 0.1f) / (enemyParty._lastCalculatedSpeed + 0.1f)) / 1.1f;
				num *= MathF.Max(0.8f, num2) * MathF.Max(0.8f, num2);
			}
			float num3 = reasonableDistance * num;
			num3 *= (1f + ((mobileParty.Army != null && mobileParty.Army.LeaderParty != null && (enemyParty.BesiegedSettlement == mobileParty.Army.LeaderParty.TargetSettlement || (mobileParty.Army.LeaderParty.TargetSettlement != null && enemyParty == mobileParty.Army.LeaderParty.TargetSettlement.LastAttackerParty))) ? 1f : mobileParty.Ai.AttackInitiative)) / 2f;
			num3 *= ((enemyParty.Army != null) ? MathF.Pow((float)enemyParty.Army.Parties.Count, 0.33f) : 1f);
			if (enemyParty.MapEvent != null || enemyParty.BesiegedSettlement != null || (mobileParty.DefaultBehavior == AiBehavior.EngageParty && mobileParty.TargetParty == enemyParty) || (mobileParty.DefaultBehavior == AiBehavior.GoAroundParty && mobileParty.TargetParty == enemyParty))
			{
				num3 = reasonableDistance * 1.5f;
			}
			return num3;
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0007F089 File Offset: 0x0007D289
		private float CalculateStanceScore(MobileParty mobileParty, MobileParty otherParty)
		{
			if (FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, otherParty.MapFaction))
			{
				return 1f;
			}
			if (DiplomacyHelper.IsSameFactionAndNotEliminated(mobileParty.MapFaction, otherParty.MapFaction))
			{
				return -1f;
			}
			return 0f;
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x0007F0C4 File Offset: 0x0007D2C4
		private float GetPatrolRadiusInternal(Settlement settlement, float speed, bool isPort)
		{
			if (settlement.IsHideout)
			{
				return Campaign.Current.Models.MobilePartyAIModel.HideoutPatrolDistanceAsDays * speed * (float)CampaignTime.HoursInDay;
			}
			if (settlement.IsFortification)
			{
				return (isPort ? Campaign.Current.Models.MobilePartyAIModel.FortificationPortPatrolDistanceAsDays : Campaign.Current.Models.MobilePartyAIModel.FortificationPatrolDistanceAsDays) * speed * (float)CampaignTime.HoursInDay;
			}
			if (settlement.IsVillage)
			{
				return Campaign.Current.Models.MobilePartyAIModel.VillagePatrolDistanceAsDays * speed * (float)CampaignTime.HoursInDay;
			}
			return 0f;
		}
	}
}
