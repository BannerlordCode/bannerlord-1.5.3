using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003B RID: 59
	public class CampaignEntityComponent : IEntityComponent
	{
		// Token: 0x060003EE RID: 1006 RVA: 0x0001F60F File Offset: 0x0001D80F
		void IEntityComponent.OnInitialize()
		{
			this.OnInitialize();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0001F617 File Offset: 0x0001D817
		void IEntityComponent.OnFinalize()
		{
			this.OnFinalize();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001F61F File Offset: 0x0001D81F
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001F621 File Offset: 0x0001D821
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0001F623 File Offset: 0x0001D823
		public virtual void OnTick(float realDt, float dt)
		{
		}
	}
}
