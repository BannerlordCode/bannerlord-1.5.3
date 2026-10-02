using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000057 RID: 87
	[Serializable]
	public class MapListResponse
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000C0AE File Offset: 0x0000A2AE
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x0000C0B6 File Offset: 0x0000A2B6
		public string CurrentlyPlaying { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000C0BF File Offset: 0x0000A2BF
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x0000C0C7 File Offset: 0x0000A2C7
		public List<MapListItemResponse> Maps { get; private set; }

		// Token: 0x060002C8 RID: 712 RVA: 0x0000C0D0 File Offset: 0x0000A2D0
		[JsonConstructor]
		public MapListResponse(string currentlyPlaying, List<MapListItemResponse> maps)
		{
			this.CurrentlyPlaying = currentlyPlaying;
			this.Maps = maps;
		}
	}
}
