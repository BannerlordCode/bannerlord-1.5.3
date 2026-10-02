using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200016D RID: 365
	public abstract class TacticComponent
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x0003DB04 File Offset: 0x0003BD04
		// (set) Token: 0x06001302 RID: 4866 RVA: 0x0003DB0C File Offset: 0x0003BD0C
		public Team Team { get; protected set; }

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001303 RID: 4867 RVA: 0x0003DB15 File Offset: 0x0003BD15
		protected MBList<Formation> FormationsIncludingSpecialAndEmpty
		{
			get
			{
				return this.Team.FormationsIncludingSpecialAndEmpty;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001304 RID: 4868 RVA: 0x0003DB22 File Offset: 0x0003BD22
		protected MBList<Formation> FormationsIncludingEmpty
		{
			get
			{
				return this.Team.FormationsIncludingEmpty;
			}
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0003DB2F File Offset: 0x0003BD2F
		protected TacticComponent(Team team)
		{
			this.Team = team;
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0003DB6D File Offset: 0x0003BD6D
		protected internal virtual void OnCancel()
		{
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0003DB6F File Offset: 0x0003BD6F
		protected internal virtual void OnApply()
		{
			this.IsTacticReapplyNeeded = true;
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0003DB78 File Offset: 0x0003BD78
		public virtual void TickOccasionally()
		{
			TeamAIComponent teamAI = this.Team.TeamAI;
			if (teamAI.GetIsFirstTacticChosen)
			{
				teamAI.OnTacticAppliedForFirstTime();
			}
			if (Mission.Current.IsMissionEnding)
			{
				this.StopUsingAllMachines();
				if (teamAI.HasStrategicAreas)
				{
					teamAI.RemoveAllStrategicAreas();
				}
			}
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x0003DBC0 File Offset: 0x0003BDC0
		private static void GetUnitCountByAttackType(Formation formation, out int unitCount, out int rangedCount, out int semiRangedCount, out int nonRangedCount)
		{
			FormationQuerySystem querySystem = formation.QuerySystem;
			unitCount = formation.CountOfUnits;
			rangedCount = (int)(querySystem.RangedUnitRatio * (float)unitCount);
			semiRangedCount = 0;
			nonRangedCount = unitCount - rangedCount;
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x0003DBF4 File Offset: 0x0003BDF4
		protected static float GetFormationGroupEffectivenessOverOrder(IEnumerable<Formation> formationGroup, OrderType orderType, IOrderable targetObject = null)
		{
			int num;
			int num2;
			int num3;
			int num4;
			TacticComponent.GetUnitCountByAttackType(formationGroup.FirstOrDefault<Formation>(), out num, out num2, out num3, out num4);
			if (orderType == OrderType.PointDefence)
			{
				float num5 = ((float)num4 * 0.1f + (float)num3 * 0.3f + (float)num2 * 1f) / (float)num;
				int num6 = (targetObject as IPointDefendable).DefencePoints.Count<DefencePoint>() * 3;
				float num7 = MathF.Min((float)num * 1f / (float)num6, 1f);
				return num5 * num7;
			}
			if (orderType == OrderType.Use)
			{
				float num8 = ((float)num4 * 1f + (float)num3 * 0.9f + (float)num2 * 0.3f) / (float)num;
				int maxUserCount = (targetObject as UsableMachine).MaxUserCount;
				float num9 = MathF.Min((float)num * 1f / (float)maxUserCount, 1f);
				return num8 * num9;
			}
			if (orderType == OrderType.Charge)
			{
				return ((float)num4 * 1f + (float)num3 * 0.9f + (float)num2 * 0.3f) / (float)num;
			}
			return 1f;
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x0003DCD8 File Offset: 0x0003BED8
		protected static float GetFormationEffectivenessOverOrder(Formation formation, OrderType orderType, IOrderable targetObject = null)
		{
			int num;
			int num2;
			int num3;
			int num4;
			TacticComponent.GetUnitCountByAttackType(formation, out num, out num2, out num3, out num4);
			if (orderType == OrderType.PointDefence)
			{
				float num5 = ((float)num4 * 0.1f + (float)num3 * 0.3f + (float)num2 * 1f) / (float)num;
				int num6 = (targetObject as IPointDefendable).DefencePoints.Count<DefencePoint>() * 3;
				float num7 = MathF.Min((float)num * 1f / (float)num6, 1f);
				return num5 * num7;
			}
			if (orderType == OrderType.Use)
			{
				float minDistanceSquared = float.MaxValue;
				formation.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					minDistanceSquared = MathF.Min(agent.Position.DistanceSquared((targetObject as UsableMachine).GameEntity.GlobalPosition), minDistanceSquared);
				}, null);
				return 1f / MBMath.ClampFloat(minDistanceSquared, 1f, float.MaxValue);
			}
			if (orderType == OrderType.Charge)
			{
				return ((float)num4 * 1f + (float)num3 * 0.9f + (float)num2 * 0.3f) / (float)num;
			}
			return 1f;
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0003DDD1 File Offset: 0x0003BFD1
		[Conditional("DEBUG")]
		protected internal virtual void DebugTick(float dt)
		{
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0003DDD4 File Offset: 0x0003BFD4
		private static int GetAvailableUnitCount(Formation formation, IEnumerable<ValueTuple<Formation, UsableMachine, int>> appliedCombinations)
		{
			int num = appliedCombinations.Where<ValueTuple<Formation, UsableMachine, int>>((ValueTuple<Formation, UsableMachine, int> c) => c.Item1 == formation).Sum<ValueTuple<Formation, UsableMachine, int>>((ValueTuple<Formation, UsableMachine, int> c) => c.Item3);
			int num2 = 0;
			return formation.CountOfUnits - (num + num2);
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0003DE38 File Offset: 0x0003C038
		private static int GetVacantSlotCount(UsableMachine weapon, IEnumerable<ValueTuple<Formation, UsableMachine, int>> appliedCombinations)
		{
			int num = appliedCombinations.Where<ValueTuple<Formation, UsableMachine, int>>((ValueTuple<Formation, UsableMachine, int> c) => c.Item2 == weapon).Sum<ValueTuple<Formation, UsableMachine, int>>((ValueTuple<Formation, UsableMachine, int> c) => c.Item3);
			return weapon.MaxUserCount - num;
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0003DE98 File Offset: 0x0003C098
		protected List<Formation> ConsolidateFormations(List<Formation> formationsToBeConsolidated, int neededCount)
		{
			if (formationsToBeConsolidated.Count <= neededCount || neededCount <= 0)
			{
				return formationsToBeConsolidated;
			}
			List<Formation> list = formationsToBeConsolidated.OrderByDescending<Formation, int>((Formation f) => f.CountOfUnits + ((!f.IsAIControlled) ? 10000 : 0)).ToList<Formation>();
			List<Formation> list2 = list.Take<Formation>(neededCount).Reverse<Formation>().ToList<Formation>();
			list.RemoveRange(0, neededCount);
			Queue<Formation> queue = new Queue<Formation>(list2);
			List<Formation> list3 = new List<Formation>();
			foreach (Formation formation in list)
			{
				if (!formation.IsAIControlled)
				{
					list3.Add(formation);
				}
				else
				{
					if (queue.IsEmpty<Formation>())
					{
						queue = new Queue<Formation>(list2);
					}
					Formation formation2 = queue.Dequeue();
					formation.TransferUnits(formation2, formation.CountOfUnits);
				}
			}
			return list2.Concat<Formation>(list3).ToList<Formation>();
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0003DF88 File Offset: 0x0003C188
		protected static float CalculateNotEngagingTacticalAdvantage(TeamQuerySystem team)
		{
			float num = team.CavalryRatio + team.RangedCavalryRatio;
			float num2 = team.EnemyCavalryRatio + team.EnemyRangedCavalryRatio;
			return MathF.Pow(MBMath.ClampFloat((num > 0f) ? (num2 / num) : 1.5f, 1f, 1.5f), 1.5f * MathF.Max(num, num2));
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0003DFE4 File Offset: 0x0003C1E4
		protected void SplitFormationClassIntoGivenNumber(Func<Formation, bool> formationClass, int count)
		{
			List<Formation> list = new List<Formation>();
			List<int> list2 = new List<int>();
			int num = 0;
			List<int> list3 = new List<int>();
			int num2 = 0;
			int i = 0;
			int num3 = 0;
			foreach (Formation formation in this.Team.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits == 0)
				{
					list.Add(formation);
					i++;
				}
				else if (formationClass(formation))
				{
					list.Add(formation);
					if (formation.IsAIOwned)
					{
						list2.Add(i);
						num += formation.CountOfUnits;
						if (formation.IsConvenientForTransfer)
						{
							list3.Add(i);
							num2 += formation.CountOfUnits;
						}
					}
					else
					{
						num3++;
					}
					i++;
				}
			}
			int num4 = count - num3;
			int count2 = list3.Count;
			int count3 = list2.Count;
			if (count3 > 0)
			{
				List<int> list4 = new List<int>();
				if (num4 > count3 && count2 > 0)
				{
					int num5 = num4 - count3;
					float num6 = (float)num / (float)num4;
					double num7 = Math.Ceiling((double)((float)num2 / (float)(list3.Count + num5)));
					List<int> list5 = new List<int>();
					int num8 = 0;
					for (i = 0; i < count3; i++)
					{
						if (list[list2[i]].IsConvenientForTransfer || (double)list[list2[i]].CountOfUnits <= num7)
						{
							list5.Add(i);
							num8 += list[list2[i]].CountOfUnits;
						}
					}
					double num9 = Math.Ceiling((double)((float)num8 / (float)(list5.Count + num5)));
					List<int> list6 = new List<int>();
					for (i = 0; i < list.Count; i++)
					{
						if (list[i].CountOfUnits == 0 && list6.Count < num5)
						{
							list6.Add(i);
						}
					}
					for (i = 0; i < count2; i++)
					{
						Formation formation2 = list[list3[i]];
						int num10 = (int)((double)formation2.CountOfUnits - num9);
						int num11 = 0;
						while (num10 >= 1 && num11 < list5.Count)
						{
							Formation formation3 = list[list5[num11]];
							if (formation2 != formation3)
							{
								int num12 = (int)(num9 - (double)formation3.CountOfUnits);
								if (num12 >= 1)
								{
									int num13 = MathF.Min(num10, num12);
									formation2.TransferUnits(formation3, num13);
									if (!list4.Contains(list3[i]))
									{
										list4.Add(list3[i]);
									}
									if (!list4.Contains(list5[num11]))
									{
										list4.Add(list5[num11]);
									}
									num10 -= num13;
								}
							}
							num11++;
						}
						if (num10 >= 1)
						{
							int num14 = 0;
							while (num10 >= 1 && num14 < list6.Count)
							{
								Formation formation4 = list[list6[num14]];
								int num15 = (int)(num7 - (double)formation4.CountOfUnits);
								if (num15 >= 1)
								{
									int num16 = MathF.Min(num10, num15);
									formation2.TransferUnits(formation4, num16);
									if (!list4.Contains(list3[i]))
									{
										list4.Add(list3[i]);
									}
									if (!list4.Contains(list6[num14]))
									{
										list4.Add(list6[num14]);
									}
									num10 -= num16;
								}
								num14++;
							}
						}
					}
				}
				else if (num4 < count3 && count3 != 1)
				{
					if (num4 < 1)
					{
						num4 = 1;
					}
					float num17 = (float)num / (float)num4;
					int num18 = 0;
					List<int> list7 = new List<int>();
					int num19 = 0;
					int num20 = 0;
					for (i = 0; i < list2.Count; i++)
					{
						Formation formation5 = list[list2[i]];
						bool flag = false;
						if (!formation5.IsConvenientForTransfer)
						{
							num18++;
							if ((float)formation5.CountOfUnits < num17)
							{
								flag = true;
							}
							else
							{
								num20++;
							}
						}
						else
						{
							flag = true;
						}
						if (flag)
						{
							list7.Add(list2[i]);
							num19 += formation5.CountOfUnits;
						}
					}
					if (num4 < 1)
					{
						num4 = 1;
					}
					else if (num4 < num18)
					{
						num4 = num18;
					}
					float num21 = (float)num19 / (float)(num4 - num20);
					List<int> list8 = new List<int>();
					int num22 = count3 - num4;
					while (list8.Count < num22 && count2 > 0)
					{
						int num23 = int.MaxValue;
						int num24 = -1;
						for (i = 0; i < list3.Count; i++)
						{
							Formation formation6 = list[list3[i]];
							if (formation6.CountOfUnits < num23)
							{
								num23 = formation6.CountOfUnits;
								num24 = list3[i];
							}
						}
						list3.Remove(num24);
						list8.Add(num24);
					}
					for (i = 0; i < list8.Count + list3.Count; i++)
					{
						bool flag2 = i < list8.Count;
						int num25;
						if (flag2)
						{
							num25 = list8[i];
						}
						else
						{
							num25 = list3[i - list8.Count];
						}
						Formation formation7 = list[num25];
						int num26;
						if (flag2)
						{
							num26 = formation7.CountOfUnits;
						}
						else
						{
							num26 = (int)((float)formation7.CountOfUnits - num21);
						}
						int num27 = 0;
						while (num26 >= 1 && num27 < list7.Count)
						{
							Formation formation8 = list[list7[num27]];
							if (formation7 != formation8 && formation8.CountOfUnits != 0)
							{
								int num28 = (int)Math.Ceiling((double)(num21 - (float)formation8.CountOfUnits));
								if (num28 >= 1)
								{
									int num29 = MathF.Min(num26, num28);
									formation7.TransferUnits(formation8, num29);
									if (!list4.Contains(num25))
									{
										list4.Add(num25);
									}
									if (!list4.Contains(list7[num27]))
									{
										list4.Add(list7[num27]);
									}
									num26 -= num29;
								}
							}
							num27++;
						}
					}
				}
				if (num4 > 1 && list4.Count > 1)
				{
					List<Formation> list9 = new List<Formation>();
					for (i = 0; i < list4.Count; i++)
					{
						Formation formation9 = list[list4[i]];
						if (formation9.CountOfUnits > 0)
						{
							formation9.AI.Side = FormationAI.BehaviorSide.BehaviorSideNotSet;
							formation9.AI.IsMainFormation = false;
							formation9.AI.ResetBehaviorWeights();
							TacticComponent.SetDefaultBehaviorWeights(formation9);
							list9.Add(formation9);
						}
					}
					this.IsTacticReapplyNeeded = true;
				}
			}
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x0003E65C File Offset: 0x0003C85C
		protected virtual bool CheckAndSetAvailableFormationsChanged()
		{
			return false;
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001314 RID: 4884 RVA: 0x0003E660 File Offset: 0x0003C860
		protected bool AreFormationsCreated
		{
			get
			{
				if (this._areFormationsCreated)
				{
					return true;
				}
				if (this.FormationsIncludingEmpty.AnyQ<Formation>((Formation f) => f.CountOfUnits > 0))
				{
					this._areFormationsCreated = true;
					this.ManageFormationCounts();
					this.CheckAndSetAvailableFormationsChanged();
					this.IsTacticReapplyNeeded = true;
					return true;
				}
				return false;
			}
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0003E6C1 File Offset: 0x0003C8C1
		public void ResetTactic()
		{
			this.ManageFormationCounts();
			this.CheckAndSetAvailableFormationsChanged();
			this.IsTacticReapplyNeeded = true;
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0003E6D8 File Offset: 0x0003C8D8
		protected void AssignTacticFormations1121()
		{
			this.ManageFormationCounts(1, 1, 2, 1);
			this._mainInfantry = TacticComponent.ChooseAndSortByPriority(this.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsInfantryFormation, (Formation f) => f.IsAIControlled, (Formation f) => f.QuerySystem.FormationPower).FirstOrDefault<Formation>();
			if (this._mainInfantry != null)
			{
				this._mainInfantry.AI.IsMainFormation = true;
				this._mainInfantry.AI.Side = FormationAI.BehaviorSide.Middle;
			}
			this._archers = TacticComponent.ChooseAndSortByPriority(this.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsRangedFormation, (Formation f) => f.IsAIControlled, (Formation f) => f.QuerySystem.FormationPower).FirstOrDefault<Formation>();
			List<Formation> list = TacticComponent.ChooseAndSortByPriority(this.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsCavalryFormation, (Formation f) => f.IsAIControlled, (Formation f) => f.QuerySystem.FormationPower);
			if (list.Count > 0)
			{
				this._leftCavalry = list[0];
				this._leftCavalry.AI.Side = FormationAI.BehaviorSide.Left;
				if (list.Count > 1)
				{
					this._rightCavalry = list[1];
					this._rightCavalry.AI.Side = FormationAI.BehaviorSide.Right;
				}
				else
				{
					this._rightCavalry = null;
				}
			}
			else
			{
				this._leftCavalry = null;
				this._rightCavalry = null;
			}
			this._rangedCavalry = TacticComponent.ChooseAndSortByPriority(this.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0 && f.QuerySystem.IsRangedCavalryFormation, (Formation f) => f.IsAIControlled, (Formation f) => f.QuerySystem.FormationPower).FirstOrDefault<Formation>();
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0003E944 File Offset: 0x0003CB44
		protected static List<Formation> ChooseAndSortByPriority(IEnumerable<Formation> formations, Func<Formation, bool> isEligible, Func<Formation, bool> isPrioritized, Func<Formation, float> score)
		{
			formations = formations.Where<Formation>(isEligible);
			IOrderedEnumerable<Formation> orderedEnumerable = formations.Where<Formation>(isPrioritized).OrderByDescending<Formation, float>(score);
			IOrderedEnumerable<Formation> orderedEnumerable2 = formations.Except<Formation>(orderedEnumerable).OrderByDescending<Formation, float>(score);
			return orderedEnumerable.Concat<Formation>(orderedEnumerable2).ToList<Formation>();
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0003E982 File Offset: 0x0003CB82
		protected virtual void ManageFormationCounts()
		{
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0003E984 File Offset: 0x0003CB84
		protected void ManageFormationCounts(int infantryCount, int rangedCount, int cavalryCount, int rangedCavalryCount)
		{
			this.SplitFormationClassIntoGivenNumber((Formation f) => f.QuerySystem.IsInfantryFormation, infantryCount);
			this.SplitFormationClassIntoGivenNumber((Formation f) => f.QuerySystem.IsRangedFormation, rangedCount);
			this.SplitFormationClassIntoGivenNumber((Formation f) => f.QuerySystem.IsCavalryFormation, cavalryCount);
			this.SplitFormationClassIntoGivenNumber((Formation f) => f.QuerySystem.IsRangedCavalryFormation, rangedCavalryCount);
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0003EA2C File Offset: 0x0003CC2C
		protected virtual void StopUsingAllMachines()
		{
			TeamAISiegeComponent teamAISiegeComponent;
			if ((teamAISiegeComponent = this.Team.TeamAI as TeamAISiegeComponent) != null)
			{
				foreach (SiegeWeapon siegeWeapon in teamAISiegeComponent.SceneSiegeWeapons)
				{
					if (siegeWeapon.Side == this.Team.Side)
					{
						siegeWeapon.SetForcedUse(false);
						for (int i = siegeWeapon.UserFormations.Count - 1; i >= 0; i--)
						{
							Formation formation = siegeWeapon.UserFormations[i];
							if (formation.Team == this.Team)
							{
								formation.StopUsingMachine(siegeWeapon, false);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0003EAE8 File Offset: 0x0003CCE8
		protected void StopUsingAllRangedSiegeWeapons()
		{
			foreach (Formation formation in this.FormationsIncludingSpecialAndEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					for (int i = formation.Detachments.Count - 1; i >= 0; i--)
					{
						RangedSiegeWeapon rangedSiegeWeapon;
						if ((rangedSiegeWeapon = formation.Detachments[i] as RangedSiegeWeapon) != null)
						{
							formation.StopUsingMachine(rangedSiegeWeapon, false);
							rangedSiegeWeapon.SetForcedUse(false);
						}
					}
				}
			}
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0003EB7C File Offset: 0x0003CD7C
		protected void SoundTacticalHorn(int soundCode)
		{
			float currentTime = Mission.Current.CurrentTime;
			if (currentTime > this._hornCooldownExpireTime)
			{
				this._hornCooldownExpireTime = currentTime + 10f;
				SoundEvent.PlaySound2D(soundCode);
			}
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x0003EBB4 File Offset: 0x0003CDB4
		public static void SetDefaultBehaviorWeights(Formation f)
		{
			f.AI.SetBehaviorWeight<BehaviorCharge>(1f);
			f.AI.SetBehaviorWeight<BehaviorPullBack>(1f);
			f.AI.SetBehaviorWeight<BehaviorStop>(1f);
			f.AI.SetBehaviorWeight<BehaviorReserve>(1f);
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0003EC05 File Offset: 0x0003CE05
		protected internal virtual float GetTacticWeight()
		{
			return 0f;
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0003EC0C File Offset: 0x0003CE0C
		protected bool CheckAndDetermineFormation(ref Formation formation, Func<Formation, bool> isEligible)
		{
			if (formation != null && formation.CountOfUnits != 0 && isEligible(formation))
			{
				return true;
			}
			List<Formation> list = this.FormationsIncludingEmpty.Where<Formation>(isEligible).ToList<Formation>();
			if (list.Count > 0)
			{
				formation = list.MaxBy<Formation, int>((Formation f) => f.CountOfUnits);
				this.IsTacticReapplyNeeded = true;
				return true;
			}
			if (formation != null)
			{
				formation = null;
				this.IsTacticReapplyNeeded = true;
			}
			return false;
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0003EC8C File Offset: 0x0003CE8C
		protected internal virtual bool ResetTacticalPositions()
		{
			return false;
		}

		// Token: 0x040004C1 RID: 1217
		public static readonly int MoveHornSoundIndex = SoundEvent.GetEventIdFromString("event:/ui/mission/horns/move");

		// Token: 0x040004C2 RID: 1218
		public static readonly int AttackHornSoundIndex = SoundEvent.GetEventIdFromString("event:/ui/mission/horns/attack");

		// Token: 0x040004C3 RID: 1219
		public static readonly int RetreatHornSoundIndex = SoundEvent.GetEventIdFromString("event:/ui/mission/horns/retreat");

		// Token: 0x040004C5 RID: 1221
		protected int _AIControlledFormationCount;

		// Token: 0x040004C6 RID: 1222
		protected bool IsTacticReapplyNeeded;

		// Token: 0x040004C7 RID: 1223
		private bool _areFormationsCreated;

		// Token: 0x040004C8 RID: 1224
		protected Formation _mainInfantry;

		// Token: 0x040004C9 RID: 1225
		protected Formation _archers;

		// Token: 0x040004CA RID: 1226
		protected Formation _leftCavalry;

		// Token: 0x040004CB RID: 1227
		protected Formation _rightCavalry;

		// Token: 0x040004CC RID: 1228
		protected Formation _rangedCavalry;

		// Token: 0x040004CD RID: 1229
		private float _hornCooldownExpireTime;

		// Token: 0x040004CE RID: 1230
		private const float HornCooldownTime = 10f;
	}
}
