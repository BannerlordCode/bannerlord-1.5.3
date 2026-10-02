using System;
using TaleWorlds.Diamond.Rest;
using TaleWorlds.Library.Http;

namespace TaleWorlds.Diamond.ClientApplication
{
	// Token: 0x02000044 RID: 68
	public class GenericRestSessionProvider : IClientSessionFactory
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x00005D64 File Offset: 0x00003F64
		public GenericRestSessionProvider(string address, IHttpDriver httpDriver)
		{
			this._address = address;
			this._httpDriver = httpDriver;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00005D7A File Offset: 0x00003F7A
		public IClientSession CreateSession(int aliveCheckInterval)
		{
			return new ClientRestSession(this._address, this._httpDriver, aliveCheckInterval);
		}

		// Token: 0x040000AA RID: 170
		private string _address;

		// Token: 0x040000AB RID: 171
		private IHttpDriver _httpDriver;
	}
}
