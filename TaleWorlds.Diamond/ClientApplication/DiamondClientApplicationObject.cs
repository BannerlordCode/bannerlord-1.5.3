using System;
using TaleWorlds.Library;

namespace TaleWorlds.Diamond.ClientApplication
{
	// Token: 0x02000043 RID: 67
	public abstract class DiamondClientApplicationObject
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00005D40 File Offset: 0x00003F40
		public DiamondClientApplication Application
		{
			get
			{
				return this._application;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x00005D48 File Offset: 0x00003F48
		public ApplicationVersion ApplicationVersion
		{
			get
			{
				return this.Application.ApplicationVersion;
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00005D55 File Offset: 0x00003F55
		protected DiamondClientApplicationObject(DiamondClientApplication application)
		{
			this._application = application;
		}

		// Token: 0x040000A9 RID: 169
		private DiamondClientApplication _application;
	}
}
