using System;
using TaleWorlds.Library.Http;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002EB RID: 747
	public class CommunityClient
	{
		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06002B65 RID: 11109 RVA: 0x000A77F9 File Offset: 0x000A59F9
		// (set) Token: 0x06002B66 RID: 11110 RVA: 0x000A7801 File Offset: 0x000A5A01
		public bool IsInGame { get; private set; }

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06002B67 RID: 11111 RVA: 0x000A780A File Offset: 0x000A5A0A
		// (set) Token: 0x06002B68 RID: 11112 RVA: 0x000A7812 File Offset: 0x000A5A12
		public ICommunityClientHandler Handler { get; set; }

		// Token: 0x06002B69 RID: 11113 RVA: 0x000A781B File Offset: 0x000A5A1B
		public CommunityClient()
		{
			this._httpDriver = HttpDriverManager.GetDefaultHttpDriver();
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x000A782E File Offset: 0x000A5A2E
		public void QuitFromGame()
		{
			if (this.IsInGame)
			{
				this.IsInGame = false;
				ICommunityClientHandler handler = this.Handler;
				if (handler == null)
				{
					return;
				}
				handler.OnQuitFromGame();
			}
		}

		// Token: 0x0400107F RID: 4223
		private IHttpDriver _httpDriver;
	}
}
