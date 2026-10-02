using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200025A RID: 602
	public struct CompassItemUpdateParams
	{
		// Token: 0x06002251 RID: 8785 RVA: 0x00078D20 File Offset: 0x00076F20
		public CompassItemUpdateParams(object item, TargetIconType targetType, Vec3 worldPosition, uint color, uint color2)
		{
			this = default(CompassItemUpdateParams);
			this.Item = item;
			this.TargetType = targetType;
			this.WorldPosition = worldPosition;
			this.Color = color;
			this.Color2 = color2;
			this.IsAttacker = false;
			this.IsAlly = false;
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x00078D5C File Offset: 0x00076F5C
		public CompassItemUpdateParams(object item, TargetIconType targetType, Vec3 worldPosition, Banner banner, bool isAttacker, bool isAlly)
		{
			this = default(CompassItemUpdateParams);
			this.Item = item;
			this.TargetType = targetType;
			this.WorldPosition = worldPosition;
			this.Banner = banner;
			this.IsAttacker = isAttacker;
			this.IsAlly = isAlly;
		}

		// Token: 0x04000D3E RID: 3390
		public readonly object Item;

		// Token: 0x04000D3F RID: 3391
		public readonly TargetIconType TargetType;

		// Token: 0x04000D40 RID: 3392
		public readonly Vec3 WorldPosition;

		// Token: 0x04000D41 RID: 3393
		public readonly uint Color;

		// Token: 0x04000D42 RID: 3394
		public readonly uint Color2;

		// Token: 0x04000D43 RID: 3395
		public readonly Banner Banner;

		// Token: 0x04000D44 RID: 3396
		public readonly bool IsAttacker;

		// Token: 0x04000D45 RID: 3397
		public readonly bool IsAlly;
	}
}
