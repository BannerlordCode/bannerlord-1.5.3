using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.CampaignSystem.Map.DistanceCache
{
	// Token: 0x0200022F RID: 559
	public abstract class NavigationCache<T> where T : ISettlementDataHolder
	{
		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x0600218F RID: 8591 RVA: 0x00094E79 File Offset: 0x00093079
		// (set) Token: 0x06002190 RID: 8592 RVA: 0x00094E81 File Offset: 0x00093081
		public float MaximumDistanceBetweenTwoConnectedSettlements { get; protected set; }

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002191 RID: 8593 RVA: 0x00094E8A File Offset: 0x0009308A
		// (set) Token: 0x06002192 RID: 8594 RVA: 0x00094E92 File Offset: 0x00093092
		private protected MobileParty.NavigationType _navigationType { protected get; private set; }

		// Token: 0x06002193 RID: 8595 RVA: 0x00094E9B File Offset: 0x0009309B
		protected NavigationCache(MobileParty.NavigationType navigationType)
		{
			this._navigationType = navigationType;
			this._settlementToSettlementDistanceWithLandRatio = new Dictionary<NavigationCacheElement<T>, Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>>();
			this._fortificationNeighbors = new Dictionary<T, MBReadOnlyList<T>>();
			this._closestSettlementsToFaceIndices = new Dictionary<int, NavigationCacheElement<T>>();
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x00094ECC File Offset: 0x000930CC
		protected void FinalizeCacheInitialization()
		{
			if (this._fortificationNeighbors != null)
			{
				if (!this._fortificationNeighbors.AnyQ<KeyValuePair<T, MBReadOnlyList<T>>>((KeyValuePair<T, MBReadOnlyList<T>> x) => x.Value.Count == 0))
				{
					return;
				}
			}
			Debug.FailedAssert("There is settlement with zero neighbor in neighbor cache, this should not be happening, check here", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Map\\DistanceCache\\NavigationCache.cs", "FinalizeCacheInitialization", 44);
			this.GenerateNeighborSettlementsCache();
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x00094F2C File Offset: 0x0009312C
		public static void CopyTo<T1>(NavigationCache<T1> source, NavigationCache<T> target) where T1 : ISettlementDataHolder
		{
			target._navigationType = source._navigationType;
			target.MaximumDistanceBetweenTwoConnectedSettlements = source.MaximumDistanceBetweenTwoConnectedSettlements;
			target._settlementToSettlementDistanceWithLandRatio = new Dictionary<NavigationCacheElement<T>, Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>>(source._settlementToSettlementDistanceWithLandRatio.Count);
			foreach (KeyValuePair<NavigationCacheElement<T1>, Dictionary<NavigationCacheElement<T1>, ValueTuple<float, float>>> keyValuePair in source._settlementToSettlementDistanceWithLandRatio)
			{
				NavigationCacheElement<T> cacheElement = target.GetCacheElement(target.GetCacheElement(keyValuePair.Key.StringId), keyValuePair.Key.IsPortUsed);
				Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>> dictionary = new Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>(keyValuePair.Value.Count);
				target._settlementToSettlementDistanceWithLandRatio.Add(cacheElement, dictionary);
				foreach (KeyValuePair<NavigationCacheElement<T1>, ValueTuple<float, float>> keyValuePair2 in keyValuePair.Value)
				{
					NavigationCacheElement<T> cacheElement2 = target.GetCacheElement(target.GetCacheElement(keyValuePair2.Key.StringId), keyValuePair2.Key.IsPortUsed);
					dictionary.Add(cacheElement2, keyValuePair2.Value);
				}
			}
			target._fortificationNeighbors = new Dictionary<T, MBReadOnlyList<T>>(source._fortificationNeighbors.Count);
			foreach (KeyValuePair<T1, MBReadOnlyList<T1>> keyValuePair3 in source._fortificationNeighbors)
			{
				T1 key = keyValuePair3.Key;
				T cacheElement3 = target.GetCacheElement(key.StringId);
				List<T> list = new List<T>(keyValuePair3.Value.Count);
				target._fortificationNeighbors.Add(cacheElement3, list.ToMBList<T>());
				foreach (T1 t in keyValuePair3.Value)
				{
					T cacheElement4 = target.GetCacheElement(t.StringId);
					list.Add(cacheElement4);
				}
			}
			target._closestSettlementsToFaceIndices = new Dictionary<int, NavigationCacheElement<T>>();
			foreach (KeyValuePair<int, NavigationCacheElement<T1>> keyValuePair4 in source._closestSettlementsToFaceIndices)
			{
				NavigationCacheElement<T> cacheElement5 = target.GetCacheElement(target.GetCacheElement(keyValuePair4.Value.StringId), keyValuePair4.Value.IsPortUsed);
				target._closestSettlementsToFaceIndices.Add(keyValuePair4.Key, cacheElement5);
			}
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x000951EC File Offset: 0x000933EC
		public MBReadOnlyList<T> GetNeighbors(T settlement)
		{
			MBReadOnlyList<T> mbreadOnlyList;
			if (!this._fortificationNeighbors.TryGetValue(settlement, out mbreadOnlyList))
			{
				mbreadOnlyList = new MBReadOnlyList<T>();
			}
			return mbreadOnlyList;
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x00095210 File Offset: 0x00093410
		public T GetClosestSettlementToFaceIndex(int faceId, out bool isAtSea)
		{
			NavigationCacheElement<T> navigationCacheElement;
			if (this._closestSettlementsToFaceIndices.TryGetValue(faceId, out navigationCacheElement))
			{
				isAtSea = navigationCacheElement.IsPortUsed;
				return navigationCacheElement.Settlement;
			}
			isAtSea = false;
			return default(T);
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x00095248 File Offset: 0x00093448
		public void GenerateCacheData()
		{
			this.GenerateClosestSettlementToFaceCache();
			this.GenerateSettlementToSettlementDistanceCache();
			this.GenerateNeighborSettlementsCache();
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x0009525C File Offset: 0x0009345C
		protected float GetSettlementToSettlementDistanceWithLandRatio(NavigationCacheElement<T> settlement1, NavigationCacheElement<T> settlement2, out float landRatio)
		{
			bool flag;
			NavigationCacheElement<T>.Sort(ref settlement1, ref settlement2, out flag);
			Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>> dictionary;
			if (!this._settlementToSettlementDistanceWithLandRatio.TryGetValue(settlement1, out dictionary))
			{
				dictionary = new Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>();
				this._settlementToSettlementDistanceWithLandRatio.Add(settlement1, dictionary);
			}
			ValueTuple<float, float> valueTuple;
			if (!dictionary.TryGetValue(settlement2, out valueTuple))
			{
				float realDistanceAndLandRatioBetweenSettlements = this.GetRealDistanceAndLandRatioBetweenSettlements(settlement1, settlement2, out landRatio);
				this.SetSettlementToSettlementDistanceWithLandRatio(settlement1, settlement2, realDistanceAndLandRatioBetweenSettlements, landRatio);
				valueTuple = new ValueTuple<float, float>(realDistanceAndLandRatioBetweenSettlements, landRatio);
			}
			landRatio = valueTuple.Item2;
			return valueTuple.Item1;
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x000952D0 File Offset: 0x000934D0
		protected void SetSettlementToSettlementDistanceWithLandRatio(NavigationCacheElement<T> settlement1, NavigationCacheElement<T> settlement2, float distance, float landRatio)
		{
			bool flag;
			NavigationCacheElement<T>.Sort(ref settlement1, ref settlement2, out flag);
			Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>> dictionary;
			if (!this._settlementToSettlementDistanceWithLandRatio.TryGetValue(settlement1, out dictionary))
			{
				dictionary = new Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>();
				this._settlementToSettlementDistanceWithLandRatio.Add(settlement1, dictionary);
			}
			ValueTuple<float, float> valueTuple;
			if (dictionary.TryGetValue(settlement2, out valueTuple))
			{
				Debug.FailedAssert("Element already exists", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Map\\DistanceCache\\NavigationCache.cs", "SetSettlementToSettlementDistanceWithLandRatio", 215);
			}
			else
			{
				dictionary.Add(settlement2, new ValueTuple<float, float>(distance, landRatio));
			}
			if (distance < 100000000f && distance > this.MaximumDistanceBetweenTwoConnectedSettlements)
			{
				this.MaximumDistanceBetweenTwoConnectedSettlements = distance;
			}
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x00095358 File Offset: 0x00093558
		protected void AddNeighbor(T settlement1, T settlement2)
		{
			bool flag = false;
			foreach (KeyValuePair<T, MBReadOnlyList<T>> keyValuePair in this._fortificationNeighbors)
			{
				T t = keyValuePair.Key;
				if (!t.StringId.Equals(settlement1.StringId) || !keyValuePair.Value.Contains(settlement2))
				{
					t = keyValuePair.Key;
					if (!t.StringId.Equals(settlement2.StringId) || !keyValuePair.Value.Contains(settlement1))
					{
						continue;
					}
				}
				flag = true;
				break;
			}
			if (!flag)
			{
				MBReadOnlyList<T> mbreadOnlyList;
				if (!this._fortificationNeighbors.TryGetValue(settlement1, out mbreadOnlyList))
				{
					this._fortificationNeighbors.Add(settlement1, new MBReadOnlyList<T>());
				}
				MBList<T> mblist;
				if (mbreadOnlyList != null)
				{
					mblist = new MBList<T>(mbreadOnlyList.Count + 1);
					mblist.AddRange(mbreadOnlyList);
				}
				else
				{
					mblist = new MBList<T>(1);
				}
				mblist.Add(settlement2);
				this._fortificationNeighbors[settlement1] = mblist;
				MBReadOnlyList<T> mbreadOnlyList2;
				if (!this._fortificationNeighbors.TryGetValue(settlement2, out mbreadOnlyList2))
				{
					this._fortificationNeighbors.Add(settlement2, new MBReadOnlyList<T>());
				}
				if (mbreadOnlyList2 != null)
				{
					mblist = new MBList<T>(mbreadOnlyList2.Count + 1);
					mblist.AddRange(mbreadOnlyList2);
				}
				else
				{
					mblist = new MBList<T>(1);
				}
				mblist.Add(settlement1);
				this._fortificationNeighbors[settlement2] = mblist;
			}
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x000954E0 File Offset: 0x000936E0
		protected void SetClosestSettlementToFaceIndex(int faceId, NavigationCacheElement<T> settlement)
		{
			this._closestSettlementsToFaceIndices.Add(faceId, settlement);
		}

		// Token: 0x0600219D RID: 8605
		protected abstract float GetRealDistanceAndLandRatioBetweenSettlements(NavigationCacheElement<T> settlement1, NavigationCacheElement<T> settlement2, out float landRatio);

		// Token: 0x0600219E RID: 8606
		protected abstract T GetCacheElement(string settlementId);

		// Token: 0x0600219F RID: 8607
		protected abstract NavigationCacheElement<T> GetCacheElement(T settlement, bool isPortUsed);

		// Token: 0x060021A0 RID: 8608 RVA: 0x000954F0 File Offset: 0x000936F0
		protected float GetLandRatioOfPath(NavigationPath path, Vec2 startPosition)
		{
			float num = 0f;
			float num2 = 0f;
			if (path.Size > 1)
			{
				List<Vec2> list = new List<Vec2>(path.PathPoints);
				list.Insert(0, startPosition);
				for (int i = 0; i < list.Count - 1; i++)
				{
					Vec2 vec = list[i];
					Vec2 vec2 = list[i + 1];
					if (vec2 == Vec2.Zero)
					{
						IL_015F:
						return MBMath.ClampFloat(num / num2, 0f, 1f);
					}
					Vec2 vec3 = vec2 - vec;
					float num3 = vec3.Length / 0.5f;
					vec3.Normalize();
					int num4 = 0;
					while ((float)num4 < num3 - 1f)
					{
						Vec2 vec4 = vec + vec3 * (float)num4 * 0.5f;
						Vec2 vec5 = vec + vec3 * (float)(num4 + 1) * 0.5f;
						bool flag;
						this.GetFaceRecordForPoint(vec4, out flag);
						bool flag2;
						this.GetFaceRecordForPoint(vec5, out flag2);
						float num5 = vec4.Distance(vec5);
						if (flag2 && flag)
						{
							num += num5;
						}
						else if (flag2 != flag)
						{
							num += num5 / 2f;
						}
						num2 += num5;
						num4++;
					}
				}
				goto IL_015F;
			}
			bool flag3;
			this.GetFaceRecordForPoint(startPosition, out flag3);
			bool flag4;
			this.GetFaceRecordForPoint(path[0], out flag4);
			if (flag4 != flag3)
			{
				return 0.5f;
			}
			if (flag4)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x060021A1 RID: 8609
		protected abstract void GetFaceRecordForPoint(Vec2 position, out bool isOnRegion1);

		// Token: 0x060021A2 RID: 8610 RVA: 0x00095670 File Offset: 0x00093870
		protected void GenerateClosestSettlementToFaceCache()
		{
			int navMeshFaceCount = this.GetNavMeshFaceCount();
			for (int i = 0; i < navMeshFaceCount; i++)
			{
				Debug.Print(string.Format("Face-Settlement cache creation progress % {0}     {1}", i * 100 / navMeshFaceCount, this._navigationType), 0, Debug.DebugColor.White, 17592186044416UL);
				Vec2 navMeshFaceCenterPosition = this.GetNavMeshFaceCenterPosition(i);
				PathFaceRecord faceRecordAtIndex = this.GetFaceRecordAtIndex(i);
				bool flag = false;
				T closestSettlementToPosition = this.GetClosestSettlementToPosition(navMeshFaceCenterPosition, faceRecordAtIndex, this.GetExcludedFaceIds(), this.GetAllRegisteredSettlements(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1(), float.MaxValue, out flag, false);
				if (!object.Equals(closestSettlementToPosition, default(T)))
				{
					this.SetClosestSettlementToFaceIndex(i, new NavigationCacheElement<T>(closestSettlementToPosition, flag));
				}
			}
		}

		// Token: 0x060021A3 RID: 8611
		protected abstract int GetNavMeshFaceCount();

		// Token: 0x060021A4 RID: 8612
		protected abstract Vec2 GetNavMeshFaceCenterPosition(int faceIndex);

		// Token: 0x060021A5 RID: 8613
		protected abstract PathFaceRecord GetFaceRecordAtIndex(int faceIndex);

		// Token: 0x060021A6 RID: 8614
		protected abstract int[] GetExcludedFaceIds();

		// Token: 0x060021A7 RID: 8615
		protected abstract int GetRegionSwitchCostTo0();

		// Token: 0x060021A8 RID: 8616
		protected abstract int GetRegionSwitchCostTo1();

		// Token: 0x060021A9 RID: 8617 RVA: 0x00095734 File Offset: 0x00093934
		protected void GenerateSettlementToSettlementDistanceCache()
		{
			List<T> allRegisteredSettlements = this.GetAllRegisteredSettlements();
			for (int i = 0; i < allRegisteredSettlements.Count; i++)
			{
				Debug.Print(string.Format("Settlement to settlement cache creation index {0},    total count: {1}     {2}", i, allRegisteredSettlements.Count, this._navigationType), 0, Debug.DebugColor.White, 17592186044416UL);
				T t = allRegisteredSettlements[i];
				for (int j = ((this._navigationType == MobileParty.NavigationType.All) ? i : (i + 1)); j < allRegisteredSettlements.Count; j++)
				{
					T t2 = allRegisteredSettlements[j];
					if (this._navigationType == MobileParty.NavigationType.Default)
					{
						this.AddClosestEntrancePairBase(t, false, t2, false);
					}
					else if (this._navigationType == MobileParty.NavigationType.Naval)
					{
						if (t.HasPort && t2.HasPort)
						{
							this.AddClosestEntrancePairBase(t, true, t2, true);
						}
					}
					else if (this._navigationType == MobileParty.NavigationType.All)
					{
						this.AddClosestEntrancePairBase(t, false, t2, false);
						if (t.HasPort && t2.HasPort)
						{
							this.AddClosestEntrancePairBase(t, true, t2, true);
						}
						if (t2.HasPort)
						{
							this.AddClosestEntrancePairBase(t, false, t2, true);
						}
						if (t.HasPort && i != j)
						{
							this.AddClosestEntrancePairBase(t, true, t2, false);
						}
					}
				}
			}
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x00095894 File Offset: 0x00093A94
		private void AddClosestEntrancePairBase(T settlement1, bool isPort1, T settlement2, bool isPort2)
		{
			NavigationCacheElement<T> cacheElement = this.GetCacheElement(settlement1, isPort1);
			NavigationCacheElement<T> cacheElement2 = this.GetCacheElement(settlement2, isPort2);
			float num;
			float realDistanceAndLandRatioBetweenSettlements = this.GetRealDistanceAndLandRatioBetweenSettlements(cacheElement, cacheElement2, out num);
			float num2;
			float realDistanceAndLandRatioBetweenSettlements2 = this.GetRealDistanceAndLandRatioBetweenSettlements(cacheElement2, cacheElement, out num2);
			float num3 = (realDistanceAndLandRatioBetweenSettlements + realDistanceAndLandRatioBetweenSettlements2) * 0.5f;
			if (num3 > 0f)
			{
				float num4 = 1f;
				if (this._navigationType == MobileParty.NavigationType.Naval)
				{
					num4 = 0f;
				}
				else if (this._navigationType == MobileParty.NavigationType.All)
				{
					num4 = num;
				}
				bool flag;
				NavigationCacheElement<T>.Sort(ref cacheElement, ref cacheElement2, out flag);
				if (flag)
				{
					num4 = num2;
				}
				this.SetSettlementToSettlementDistanceWithLandRatio(cacheElement, cacheElement2, num3, num4);
			}
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x00095920 File Offset: 0x00093B20
		protected void GenerateNeighborSettlementsCache()
		{
			this._fortificationNeighbors.Clear();
			List<T> updatedSettlementsForNeighborDetection = this.GetUpdatedSettlementsForNeighborDetection(this.GetAllRegisteredSettlements());
			for (int i = 0; i < updatedSettlementsForNeighborDetection.Count - 1; i++)
			{
				Debug.Print(string.Format("Neighbor cache progress for navigation {0}, current index: {1}  - total count: {2}", this._navigationType, i, updatedSettlementsForNeighborDetection.Count), 0, Debug.DebugColor.White, 17592186044416UL);
				T t = updatedSettlementsForNeighborDetection[i];
				if (t.IsFortification)
				{
					for (int j = i + 1; j < updatedSettlementsForNeighborDetection.Count; j++)
					{
						T t2 = updatedSettlementsForNeighborDetection[j];
						if (t2.IsFortification && this.CheckBeingNeighbor(updatedSettlementsForNeighborDetection, t, t2))
						{
							this.AddNeighbor(t, t2);
						}
					}
				}
			}
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x000959EC File Offset: 0x00093BEC
		private void CheckNeighbourAux(List<T> settlementsToConsider, T settlement1, T settlement2, bool useGate1, bool useGate2, ref float distance, ref bool isNeighbour)
		{
			float num;
			bool flag = this.CheckBeingNeighbor(settlementsToConsider, settlement1, settlement2, useGate1, useGate2, out num);
			if (num < distance)
			{
				distance = num;
				isNeighbour = flag;
			}
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x00095A18 File Offset: 0x00093C18
		protected bool CheckBeingNeighbor(List<T> settlementsToConsider, T settlement1, T settlement2)
		{
			float maxValue = float.MaxValue;
			bool flag = false;
			if (this._navigationType == MobileParty.NavigationType.Default || this._navigationType == MobileParty.NavigationType.All)
			{
				this.CheckNeighbourAux(settlementsToConsider, settlement1, settlement2, true, true, ref maxValue, ref flag);
				this.CheckNeighbourAux(settlementsToConsider, settlement2, settlement1, true, true, ref maxValue, ref flag);
			}
			if (this._navigationType == MobileParty.NavigationType.Naval || this._navigationType == MobileParty.NavigationType.All)
			{
				bool hasPort = settlement1.HasPort;
				bool hasPort2 = settlement2.HasPort;
				if (hasPort)
				{
					this.CheckNeighbourAux(settlementsToConsider, settlement1, settlement2, false, true, ref maxValue, ref flag);
					this.CheckNeighbourAux(settlementsToConsider, settlement2, settlement1, true, false, ref maxValue, ref flag);
				}
				if (hasPort2)
				{
					this.CheckNeighbourAux(settlementsToConsider, settlement1, settlement2, true, false, ref maxValue, ref flag);
					this.CheckNeighbourAux(settlementsToConsider, settlement2, settlement1, false, true, ref maxValue, ref flag);
				}
				if (hasPort2 && hasPort)
				{
					this.CheckNeighbourAux(settlementsToConsider, settlement1, settlement2, false, false, ref maxValue, ref flag);
					this.CheckNeighbourAux(settlementsToConsider, settlement2, settlement1, false, false, ref maxValue, ref flag);
				}
			}
			return flag;
		}

		// Token: 0x060021AE RID: 8622
		protected abstract List<T> GetAllRegisteredSettlements();

		// Token: 0x060021AF RID: 8623 RVA: 0x00095AF0 File Offset: 0x00093CF0
		protected List<T> GetUpdatedSettlementsForNeighborDetection(List<T> settlements)
		{
			if (this._navigationType == MobileParty.NavigationType.Naval)
			{
				return settlements.Where<T>((T x) => x.IsFortification && x.HasPort).ToList<T>();
			}
			return settlements.Where<T>((T x) => x.IsFortification).ToList<T>();
		}

		// Token: 0x060021B0 RID: 8624
		protected abstract bool CheckBeingNeighbor(List<T> settlementsToConsider, T settlement1, T settlement2, bool useGate1, bool useGate2, out float foundDistance);

		// Token: 0x060021B1 RID: 8625
		protected abstract float GetRealPathDistanceFromPositionToSettlement(Vec2 checkPosition, PathFaceRecord currentFaceRecord, float maxDistanceToLookForPathDetection, T currentSettlementToLook, out bool isPort);

		// Token: 0x060021B2 RID: 8626 RVA: 0x00095B5C File Offset: 0x00093D5C
		protected T GetClosestSettlementToPosition(Vec2 checkPosition, PathFaceRecord currentFaceRecord, int[] excludedFaceIds, List<T> settlementRecords, int regionSwitchCostTo0, int regionSwitchCostTo1, float minPathScoreEverFound, out bool isPort, bool useEarlyOut = false)
		{
			isPort = false;
			T t = default(T);
			foreach (T t2 in this.GetClosestSettlementsToPositionInCache(checkPosition, settlementRecords))
			{
				if (useEarlyOut)
				{
					CampaignVec2 campaignVec;
					if (this._navigationType == MobileParty.NavigationType.Naval && t2.HasPort)
					{
						campaignVec = t2.PortPosition;
					}
					else if (this._navigationType == MobileParty.NavigationType.All && t2.HasPort)
					{
						float num = t2.GatePosition.DistanceSquared(checkPosition);
						campaignVec = ((t2.PortPosition.DistanceSquared(checkPosition) < num) ? t2.PortPosition : t2.GatePosition);
					}
					else
					{
						campaignVec = t2.GatePosition;
					}
					float num2 = minPathScoreEverFound + 25f;
					if (campaignVec.DistanceSquared(checkPosition) > num2 * num2)
					{
						break;
					}
				}
				bool flag;
				float realPathDistanceFromPositionToSettlement = this.GetRealPathDistanceFromPositionToSettlement(checkPosition, currentFaceRecord, minPathScoreEverFound * 2f, t2, out flag);
				if (realPathDistanceFromPositionToSettlement < minPathScoreEverFound)
				{
					minPathScoreEverFound = realPathDistanceFromPositionToSettlement;
					t = t2;
					isPort = flag;
				}
			}
			return t;
		}

		// Token: 0x060021B3 RID: 8627
		protected abstract IEnumerable<T> GetClosestSettlementsToPositionInCache(Vec2 checkPosition, List<T> settlements);

		// Token: 0x060021B4 RID: 8628
		public abstract void GetSceneXmlCrcValues(out uint sceneXmlCrc, out uint sceneNavigationMeshCrc);

		// Token: 0x060021B5 RID: 8629 RVA: 0x00095CB0 File Offset: 0x00093EB0
		public bool GetSettlementsDistanceCacheFileForCapability(string moduleId, out string filePath)
		{
			string text = ModuleHelper.GetModuleFullPath(moduleId) + "ModuleData/DistanceCaches";
			string text2 = this._navigationType.ToString();
			filePath = text + "/settlements_distance_cache_" + text2 + ".bin";
			bool flag = File.Exists(filePath);
			if (flag)
			{
				Debug.Print(string.Format("Found distance cache at: {0}, {1}, {2}", moduleId, text, this._navigationType), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			return flag;
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x00095D28 File Offset: 0x00093F28
		public void Serialize(string path)
		{
			BinaryWriter binaryWriter = new BinaryWriter(File.Open(path, FileMode.Create));
			uint num;
			uint num2;
			this.GetSceneXmlCrcValues(out num, out num2);
			binaryWriter.Write(num);
			binaryWriter.Write(num2);
			binaryWriter.Write(this._settlementToSettlementDistanceWithLandRatio.Count);
			foreach (KeyValuePair<NavigationCacheElement<T>, Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>> keyValuePair in this._settlementToSettlementDistanceWithLandRatio)
			{
				binaryWriter.Write(keyValuePair.Key.StringId);
				binaryWriter.Write(keyValuePair.Key.IsPortUsed);
				binaryWriter.Write(keyValuePair.Value.Count);
				foreach (KeyValuePair<NavigationCacheElement<T>, ValueTuple<float, float>> keyValuePair2 in keyValuePair.Value)
				{
					binaryWriter.Write(keyValuePair2.Key.StringId);
					binaryWriter.Write(keyValuePair2.Key.IsPortUsed);
					binaryWriter.Write(keyValuePair2.Value.Item1);
					if (this._navigationType == MobileParty.NavigationType.All)
					{
						binaryWriter.Write(keyValuePair2.Value.Item2);
					}
				}
			}
			binaryWriter.Write(this._fortificationNeighbors.SumQ<KeyValuePair<T, MBReadOnlyList<T>>>((KeyValuePair<T, MBReadOnlyList<T>> x) => x.Value.Count));
			foreach (KeyValuePair<T, MBReadOnlyList<T>> keyValuePair3 in this._fortificationNeighbors)
			{
				T key = keyValuePair3.Key;
				string stringId = key.StringId;
				foreach (T t in keyValuePair3.Value)
				{
					binaryWriter.Write(stringId);
					binaryWriter.Write(t.StringId);
				}
			}
			binaryWriter.Write(this._closestSettlementsToFaceIndices.Count);
			foreach (KeyValuePair<int, NavigationCacheElement<T>> keyValuePair4 in this._closestSettlementsToFaceIndices)
			{
				binaryWriter.Write(keyValuePair4.Key);
				binaryWriter.Write(keyValuePair4.Value.StringId);
				binaryWriter.Write(keyValuePair4.Value.IsPortUsed);
			}
			binaryWriter.Close();
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x00095FE8 File Offset: 0x000941E8
		public void Deserialize(string path)
		{
			Debug.Print("Reading SettlementsDistanceCacheFilePath: " + path, 0, Debug.DebugColor.White, 17592186044416UL);
			BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read));
			binaryReader.ReadUInt32();
			binaryReader.ReadUInt32();
			Campaign.Current.MapSceneWrapper.GetSceneXmlCrc();
			Campaign.Current.MapSceneWrapper.GetSceneNavigationMeshCrc();
			int num = binaryReader.ReadInt32();
			this._settlementToSettlementDistanceWithLandRatio = new Dictionary<NavigationCacheElement<T>, Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>>(num);
			for (int i = 0; i < num; i++)
			{
				T cacheElement = this.GetCacheElement(binaryReader.ReadString());
				bool flag = binaryReader.ReadBoolean();
				NavigationCacheElement<T> cacheElement2 = this.GetCacheElement(cacheElement, flag);
				int num2 = binaryReader.ReadInt32();
				this._settlementToSettlementDistanceWithLandRatio.Add(cacheElement2, new Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>(num2));
				for (int j = 0; j < num2; j++)
				{
					T cacheElement3 = this.GetCacheElement(binaryReader.ReadString());
					bool flag2 = binaryReader.ReadBoolean();
					NavigationCacheElement<T> cacheElement4 = this.GetCacheElement(cacheElement3, flag2);
					bool flag3;
					NavigationCacheElement<T>.Sort(ref cacheElement2, ref cacheElement4, out flag3);
					float num3 = binaryReader.ReadSingle();
					float num4 = ((this._navigationType == MobileParty.NavigationType.Naval) ? 0f : 1f);
					if (this._navigationType == MobileParty.NavigationType.All)
					{
						num4 = binaryReader.ReadSingle();
					}
					this.SetSettlementToSettlementDistanceWithLandRatio(cacheElement2, cacheElement4, num3, num4);
				}
			}
			int num5 = binaryReader.ReadInt32();
			this._fortificationNeighbors = new Dictionary<T, MBReadOnlyList<T>>(num5);
			for (int k = 0; k < num5; k++)
			{
				T cacheElement5 = this.GetCacheElement(binaryReader.ReadString());
				T cacheElement6 = this.GetCacheElement(binaryReader.ReadString());
				this.AddNeighbor(cacheElement5, cacheElement6);
			}
			int num6 = binaryReader.ReadInt32();
			this._closestSettlementsToFaceIndices = new Dictionary<int, NavigationCacheElement<T>>(num6);
			for (int l = 0; l < num6; l++)
			{
				int num7 = binaryReader.ReadInt32();
				T cacheElement7 = this.GetCacheElement(binaryReader.ReadString());
				bool flag4 = binaryReader.ReadBoolean();
				NavigationCacheElement<T> cacheElement8 = this.GetCacheElement(cacheElement7, flag4);
				this.SetClosestSettlementToFaceIndex(num7, cacheElement8);
			}
			binaryReader.Close();
		}

		// Token: 0x040009D3 RID: 2515
		private Dictionary<NavigationCacheElement<T>, Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>> _settlementToSettlementDistanceWithLandRatio;

		// Token: 0x040009D4 RID: 2516
		private Dictionary<T, MBReadOnlyList<T>> _fortificationNeighbors;

		// Token: 0x040009D5 RID: 2517
		private Dictionary<int, NavigationCacheElement<T>> _closestSettlementsToFaceIndices;

		// Token: 0x040009D6 RID: 2518
		protected const float AgentRadius = 0.3f;

		// Token: 0x040009D7 RID: 2519
		protected const float ExtraCostMultiplierForNeighborDetection = 2f;
	}
}
