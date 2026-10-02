using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map.DistanceCache
{
	// Token: 0x02000231 RID: 561
	public readonly struct NavigationCacheElement<T> : IEquatable<NavigationCacheElement<T>> where T : ISettlementDataHolder
	{
		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x060021BD RID: 8637 RVA: 0x000961D8 File Offset: 0x000943D8
		public CampaignVec2 PortPosition
		{
			get
			{
				T settlement = this.Settlement;
				return settlement.PortPosition;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x060021BE RID: 8638 RVA: 0x000961FC File Offset: 0x000943FC
		public CampaignVec2 GatePosition
		{
			get
			{
				T settlement = this.Settlement;
				return settlement.GatePosition;
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x060021BF RID: 8639 RVA: 0x00096220 File Offset: 0x00094420
		public string StringId
		{
			get
			{
				T settlement = this.Settlement;
				return settlement.StringId;
			}
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x00096241 File Offset: 0x00094441
		public NavigationCacheElement(T settlement, bool isPortUsed)
		{
			this.Settlement = settlement;
			this.IsPortUsed = isPortUsed;
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x00096254 File Offset: 0x00094454
		public static void Sort(ref NavigationCacheElement<T> settlement1, ref NavigationCacheElement<T> settlement2, out bool isPairChanged)
		{
			isPairChanged = false;
			int num = string.Compare(settlement1.StringId, settlement2.StringId, StringComparison.Ordinal);
			if (num < 0 || (num == 0 && settlement1.IsPortUsed))
			{
				return;
			}
			NavigationCacheElement<T> navigationCacheElement = settlement2;
			NavigationCacheElement<T> navigationCacheElement2 = settlement1;
			settlement1 = navigationCacheElement;
			settlement2 = navigationCacheElement2;
			isPairChanged = true;
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x000962A6 File Offset: 0x000944A6
		public override int GetHashCode()
		{
			return this.StringId.GetDeterministicHashCode() * 2 + (this.IsPortUsed ? 1 : 0);
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x000962C4 File Offset: 0x000944C4
		public override bool Equals(object obj)
		{
			if (obj is NavigationCacheElement<T>)
			{
				NavigationCacheElement<T> navigationCacheElement = (NavigationCacheElement<T>)obj;
				return this.StringId == navigationCacheElement.StringId && this.IsPortUsed == navigationCacheElement.IsPortUsed;
			}
			return false;
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x0009630A File Offset: 0x0009450A
		public bool Equals(NavigationCacheElement<T> other)
		{
			return EqualityComparer<T>.Default.Equals(this.Settlement, other.Settlement) && this.IsPortUsed == other.IsPortUsed;
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x00096334 File Offset: 0x00094534
		public static bool operator ==(NavigationCacheElement<T> left, NavigationCacheElement<T> right)
		{
			return left.Equals(right);
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x0009633E File Offset: 0x0009453E
		public static bool operator !=(NavigationCacheElement<T> left, NavigationCacheElement<T> right)
		{
			return !left.Equals(right);
		}

		// Token: 0x040009D9 RID: 2521
		public readonly T Settlement;

		// Token: 0x040009DA RID: 2522
		public readonly bool IsPortUsed;
	}
}
