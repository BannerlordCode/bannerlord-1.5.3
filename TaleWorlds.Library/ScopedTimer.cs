using System;
using System.Diagnostics;

namespace TaleWorlds.Library
{
	// Token: 0x0200008C RID: 140
	public class ScopedTimer : IDisposable
	{
		// Token: 0x0600050B RID: 1291 RVA: 0x000124AF File Offset: 0x000106AF
		public ScopedTimer(string scopeName)
		{
			this.scopeName_ = scopeName;
			this.watch_ = new Stopwatch();
			this.watch_.Start();
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x000124D4 File Offset: 0x000106D4
		public void Dispose()
		{
			this.watch_.Stop();
			Console.WriteLine(string.Concat(new object[]
			{
				"ScopedTimer: ",
				this.scopeName_,
				" elapsed ms: ",
				this.watch_.Elapsed.TotalMilliseconds
			}));
		}

		// Token: 0x04000191 RID: 401
		private readonly Stopwatch watch_;

		// Token: 0x04000192 RID: 402
		private readonly string scopeName_;
	}
}
