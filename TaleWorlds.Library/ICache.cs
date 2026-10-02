using System;
using System.Threading.Tasks;

namespace TaleWorlds.Library
{
	// Token: 0x02000037 RID: 55
	public interface ICache
	{
		// Token: 0x060001C3 RID: 451
		Task<TItem> GetOrUpdate<TItem>(string key, Func<Task<TItem>> factory, TimeSpan absoluteExpirationRelativeToNow, bool getFromFactoryIfCacheFails = true);

		// Token: 0x060001C4 RID: 452
		Task SetString(string key, string value, TimeSpan? absoluteExpirationRelativeToNow);

		// Token: 0x060001C5 RID: 453
		Task<string> GetString(string key);
	}
}
