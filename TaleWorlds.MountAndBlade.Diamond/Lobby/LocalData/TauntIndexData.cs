using System;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x0200017A RID: 378
	public struct TauntIndexData
	{
		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00011763 File Offset: 0x0000F963
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x0001176B File Offset: 0x0000F96B
		public string TauntId { get; set; }

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x00011774 File Offset: 0x0000F974
		// (set) Token: 0x06000ABB RID: 2747 RVA: 0x0001177C File Offset: 0x0000F97C
		public int TauntIndex { get; set; }

		// Token: 0x06000ABC RID: 2748 RVA: 0x00011785 File Offset: 0x0000F985
		public TauntIndexData(string tauntId, int tauntIndex)
		{
			this.TauntId = tauntId;
			this.TauntIndex = tauntIndex;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00011798 File Offset: 0x0000F998
		public override bool Equals(object obj)
		{
			if (obj is TauntIndexData)
			{
				TauntIndexData tauntIndexData = (TauntIndexData)obj;
				return this.TauntId == tauntIndexData.TauntId && this.TauntIndex == tauntIndexData.TauntIndex;
			}
			return false;
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x000117E0 File Offset: 0x0000F9E0
		public override int GetHashCode()
		{
			return (this.TauntId.GetHashCode() * 397) ^ this.TauntIndex.GetHashCode();
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0001180D File Offset: 0x0000FA0D
		public static bool operator ==(TauntIndexData first, TauntIndexData second)
		{
			return first.TauntId == second.TauntId && first.TauntIndex == second.TauntIndex;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00011836 File Offset: 0x0000FA36
		public static bool operator !=(TauntIndexData first, TauntIndexData second)
		{
			return !(first == second);
		}
	}
}
