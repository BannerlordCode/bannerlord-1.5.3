using System;
using TaleWorlds.Diamond.Rest;
using TaleWorlds.Library;
using TaleWorlds.Library.Http;

namespace TaleWorlds.Diamond.ClientApplication
{
	// Token: 0x02000046 RID: 70
	public class ThreadedRestSessionFactory : IClientSessionFactory
	{
		// Token: 0x060001DA RID: 474 RVA: 0x00005D8E File Offset: 0x00003F8E
		public ThreadedRestSessionFactory(string address, IHttpDriver httpDriver, ParameterContainer parameters)
		{
			this._address = address;
			this._httpDriver = httpDriver;
			this._parameters = parameters;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00005DAC File Offset: 0x00003FAC
		public IClientSession CreateSession(int aliveCheckInterval)
		{
			int num;
			if (!this._parameters.TryGetParameterAsInt("ThreadedClientSession.ThreadSleepTime", out num))
			{
				num = 100;
			}
			return new ThreadedClientSession(new ClientRestSession(this._address, this._httpDriver, aliveCheckInterval), num);
		}

		// Token: 0x040000B0 RID: 176
		public const int DefaultThreadSleepTime = 100;

		// Token: 0x040000B1 RID: 177
		private readonly string _address;

		// Token: 0x040000B2 RID: 178
		private readonly IHttpDriver _httpDriver;

		// Token: 0x040000B3 RID: 179
		private readonly ParameterContainer _parameters;
	}
}
