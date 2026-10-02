using System;
using System.Collections.Generic;
using System.Linq;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002F1 RID: 753
	public class CustomGameUsableMap
	{
		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002B77 RID: 11127 RVA: 0x000A7E32 File Offset: 0x000A6032
		// (set) Token: 0x06002B78 RID: 11128 RVA: 0x000A7E3A File Offset: 0x000A603A
		public string Map { get; private set; }

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002B79 RID: 11129 RVA: 0x000A7E43 File Offset: 0x000A6043
		// (set) Token: 0x06002B7A RID: 11130 RVA: 0x000A7E4B File Offset: 0x000A604B
		public bool IsCompatibleWithAllGameTypes { get; private set; }

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06002B7B RID: 11131 RVA: 0x000A7E54 File Offset: 0x000A6054
		// (set) Token: 0x06002B7C RID: 11132 RVA: 0x000A7E5C File Offset: 0x000A605C
		public List<string> CompatibleGameTypes { get; private set; }

		// Token: 0x06002B7D RID: 11133 RVA: 0x000A7E65 File Offset: 0x000A6065
		public CustomGameUsableMap(string map, bool isCompatibleWithAllGameTypes, List<string> compatibleGameTypes)
		{
			this.Map = map;
			this.IsCompatibleWithAllGameTypes = isCompatibleWithAllGameTypes;
			this.CompatibleGameTypes = compatibleGameTypes;
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x000A7E84 File Offset: 0x000A6084
		public override bool Equals(object obj)
		{
			CustomGameUsableMap customGameUsableMap;
			if ((customGameUsableMap = obj as CustomGameUsableMap) != null)
			{
				return !(customGameUsableMap.Map != this.Map) && customGameUsableMap.IsCompatibleWithAllGameTypes == this.IsCompatibleWithAllGameTypes && (((this.CompatibleGameTypes == null || this.CompatibleGameTypes.Count == 0) && (customGameUsableMap.CompatibleGameTypes == null || customGameUsableMap.CompatibleGameTypes.Count == 0)) || (this.CompatibleGameTypes != null && this.CompatibleGameTypes.Count != 0 && customGameUsableMap.CompatibleGameTypes != null && customGameUsableMap.CompatibleGameTypes.Count != 0 && this.CompatibleGameTypes.SequenceEqual<string>(customGameUsableMap.CompatibleGameTypes)));
			}
			return base.Equals(obj);
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x000A7F34 File Offset: 0x000A6134
		public override int GetHashCode()
		{
			return (((((this.Map != null) ? this.Map.GetHashCode() : 0) * 397) ^ this.IsCompatibleWithAllGameTypes.GetHashCode()) * 397) ^ ((this.CompatibleGameTypes != null) ? this.CompatibleGameTypes.GetHashCode() : 0);
		}
	}
}
