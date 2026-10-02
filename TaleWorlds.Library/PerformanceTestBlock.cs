using System;
using System.Diagnostics;

namespace TaleWorlds.Library
{
	// Token: 0x0200007B RID: 123
	public class PerformanceTestBlock : IDisposable
	{
		// Token: 0x06000461 RID: 1121 RVA: 0x0000F81C File Offset: 0x0000DA1C
		public PerformanceTestBlock(string name)
		{
			this._name = name;
			Debug.Print(this._name + " block is started.", 0, Debug.DebugColor.White, 17592186044416UL);
			this._stopwatch = new Stopwatch();
			this._stopwatch.Start();
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000F870 File Offset: 0x0000DA70
		void IDisposable.Dispose()
		{
			float num = (float)this._stopwatch.ElapsedMilliseconds / 1000f;
			Debug.Print(string.Concat(new object[] { this._name, " completed in ", num, " seconds." }), 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x0400015D RID: 349
		private readonly string _name;

		// Token: 0x0400015E RID: 350
		private readonly Stopwatch _stopwatch;
	}
}
