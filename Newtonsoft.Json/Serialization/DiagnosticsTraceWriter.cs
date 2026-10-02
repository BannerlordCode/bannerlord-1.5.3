using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000077 RID: 119
	public class DiagnosticsTraceWriter : ITraceWriter
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x0001B42C File Offset: 0x0001962C
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x0001B434 File Offset: 0x00019634
		public TraceLevel LevelFilter { get; set; }

		// Token: 0x06000656 RID: 1622 RVA: 0x0001B43D File Offset: 0x0001963D
		private TraceEventType GetTraceEventType(TraceLevel level)
		{
			switch (level)
			{
			case TraceLevel.Error:
				return TraceEventType.Error;
			case TraceLevel.Warning:
				return TraceEventType.Warning;
			case TraceLevel.Info:
				return TraceEventType.Information;
			case TraceLevel.Verbose:
				return TraceEventType.Verbose;
			default:
				throw new ArgumentOutOfRangeException("level");
			}
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0001B46C File Offset: 0x0001966C
		[NullableContext(1)]
		public void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex)
		{
			if (level == TraceLevel.Off)
			{
				return;
			}
			TraceEventCache traceEventCache = new TraceEventCache();
			TraceEventType traceEventType = this.GetTraceEventType(level);
			foreach (object obj in global::System.Diagnostics.Trace.Listeners)
			{
				TraceListener traceListener = (TraceListener)obj;
				if (!traceListener.IsThreadSafe)
				{
					TraceListener traceListener2 = traceListener;
					lock (traceListener2)
					{
						traceListener.TraceEvent(traceEventCache, "Newtonsoft.Json", traceEventType, 0, message);
						goto IL_006E;
					}
					goto IL_005F;
				}
				goto IL_005F;
				IL_006E:
				if (global::System.Diagnostics.Trace.AutoFlush)
				{
					traceListener.Flush();
					continue;
				}
				continue;
				IL_005F:
				traceListener.TraceEvent(traceEventCache, "Newtonsoft.Json", traceEventType, 0, message);
				goto IL_006E;
			}
		}
	}
}
