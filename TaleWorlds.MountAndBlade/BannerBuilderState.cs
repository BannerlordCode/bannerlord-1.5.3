using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000241 RID: 577
	public class BannerBuilderState : GameState
	{
		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x060021A1 RID: 8609 RVA: 0x00076F09 File Offset: 0x00075109
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x060021A2 RID: 8610 RVA: 0x00076F0C File Offset: 0x0007510C
		public string DefaultBannerKey { get; }

		// Token: 0x060021A3 RID: 8611 RVA: 0x00076F14 File Offset: 0x00075114
		public BannerBuilderState()
		{
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x00076F1C File Offset: 0x0007511C
		public BannerBuilderState(string defaultBannerKey)
		{
			this.DefaultBannerKey = defaultBannerKey;
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x00076F2B File Offset: 0x0007512B
		protected override void OnActivate()
		{
			base.OnActivate();
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x00076F33 File Offset: 0x00075133
		protected override void OnFinalize()
		{
			base.OnFinalize();
		}
	}
}
