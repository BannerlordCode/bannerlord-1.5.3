using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200007F RID: 127
	[NullableContext(1)]
	public interface ITraceWriter
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000671 RID: 1649
		TraceLevel LevelFilter { get; }

		// Token: 0x06000672 RID: 1650
		void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex);
	}
}
