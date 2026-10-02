using System;
using System.Collections.Concurrent;

namespace TaleWorlds.Library.Http
{
	// Token: 0x020000AF RID: 175
	public static class HttpDriverManager
	{
		// Token: 0x0600069B RID: 1691 RVA: 0x00016F2D File Offset: 0x0001512D
		public static void AddHttpDriver(string name, IHttpDriver driver)
		{
			if (HttpDriverManager._httpDrivers.Count == 0)
			{
				HttpDriverManager._defaultHttpDriver = name;
			}
			HttpDriverManager._httpDrivers[name] = driver;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00016F4D File Offset: 0x0001514D
		public static void SetDefault(string name)
		{
			if (HttpDriverManager.GetHttpDriver(name) != null)
			{
				HttpDriverManager._defaultHttpDriver = name;
			}
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00016F60 File Offset: 0x00015160
		public static IHttpDriver GetHttpDriver(string name)
		{
			IHttpDriver httpDriver;
			HttpDriverManager._httpDrivers.TryGetValue(name, out httpDriver);
			if (httpDriver == null)
			{
				Debug.Print("HTTP driver not found:" + (name ?? "not set"), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			return httpDriver;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00016FA4 File Offset: 0x000151A4
		public static IHttpDriver GetDefaultHttpDriver()
		{
			if (HttpDriverManager._defaultHttpDriver == null)
			{
				HttpDriverManager.AddHttpDriver("DotNet", new DotNetHttpDriver());
			}
			return HttpDriverManager.GetHttpDriver(HttpDriverManager._defaultHttpDriver);
		}

		// Token: 0x040001FC RID: 508
		private static ConcurrentDictionary<string, IHttpDriver> _httpDrivers = new ConcurrentDictionary<string, IHttpDriver>();

		// Token: 0x040001FD RID: 509
		private static string _defaultHttpDriver;
	}
}
