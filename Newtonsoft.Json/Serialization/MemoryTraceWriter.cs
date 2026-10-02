using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000099 RID: 153
	[NullableContext(1)]
	[Nullable(0)]
	public class MemoryTraceWriter : ITraceWriter
	{
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x0002331C File Offset: 0x0002151C
		// (set) Token: 0x0600080A RID: 2058 RVA: 0x00023324 File Offset: 0x00021524
		public TraceLevel LevelFilter { get; set; }

		// Token: 0x0600080B RID: 2059 RVA: 0x0002332D File Offset: 0x0002152D
		public MemoryTraceWriter()
		{
			this.LevelFilter = TraceLevel.Verbose;
			this._traceMessages = new Queue<string>();
			this._lock = new object();
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00023354 File Offset: 0x00021554
		public void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff", CultureInfo.InvariantCulture));
			stringBuilder.Append(" ");
			stringBuilder.Append(level.ToString("g"));
			stringBuilder.Append(" ");
			stringBuilder.Append(message);
			string text = stringBuilder.ToString();
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._traceMessages.Count >= 1000)
				{
					this._traceMessages.Dequeue();
				}
				this._traceMessages.Enqueue(text);
			}
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00023418 File Offset: 0x00021618
		public IEnumerable<string> GetTraceMessages()
		{
			return this._traceMessages;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00023420 File Offset: 0x00021620
		public override string ToString()
		{
			object @lock = this._lock;
			string text2;
			lock (@lock)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text in this._traceMessages)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
				text2 = stringBuilder.ToString();
			}
			return text2;
		}

		// Token: 0x040002D9 RID: 729
		private readonly Queue<string> _traceMessages;

		// Token: 0x040002DA RID: 730
		private readonly object _lock;
	}
}
