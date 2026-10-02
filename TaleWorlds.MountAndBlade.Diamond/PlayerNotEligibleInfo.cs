using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000108 RID: 264
	[Serializable]
	public class PlayerNotEligibleInfo
	{
		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x000071FC File Offset: 0x000053FC
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x00007204 File Offset: 0x00005404
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0000720D File Offset: 0x0000540D
		// (set) Token: 0x060005AE RID: 1454 RVA: 0x00007215 File Offset: 0x00005415
		[JsonProperty]
		public PlayerNotEligibleError[] Errors { get; private set; }

		// Token: 0x060005AF RID: 1455 RVA: 0x0000721E File Offset: 0x0000541E
		public PlayerNotEligibleInfo(PlayerId playerId, PlayerNotEligibleError[] errors)
		{
			this.PlayerId = playerId;
			this.Errors = errors;
		}
	}
}
