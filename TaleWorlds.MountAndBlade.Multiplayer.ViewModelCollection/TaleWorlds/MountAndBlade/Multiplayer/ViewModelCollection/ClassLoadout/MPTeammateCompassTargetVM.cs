using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.Compass;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000AC RID: 172
	public class MPTeammateCompassTargetVM : CompassTargetVM
	{
		// Token: 0x06001089 RID: 4233 RVA: 0x00033624 File Offset: 0x00031824
		public MPTeammateCompassTargetVM(TargetIconType iconType, uint color, uint color2, Banner banner, bool isAlly)
			: base(iconType, color, color2, banner, false, isAlly)
		{
			base.IconType = iconType.ToString();
			base.IsFlag = false;
			base.Banner = ((banner != null) ? new BannerImageIdentifierVM(banner, false) : new BannerImageIdentifierVM(null, false));
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00033674 File Offset: 0x00031874
		public void RefreshTargetIconType(TargetIconType targetIconType)
		{
			base.IconType = targetIconType.ToString();
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00033689 File Offset: 0x00031889
		public void RefreshTeam(Banner banner, bool isAlly)
		{
			base.Banner = ((banner != null) ? new BannerImageIdentifierVM(banner, false) : new BannerImageIdentifierVM(null, false));
			base.IsEnemy = !isAlly;
		}
	}
}
