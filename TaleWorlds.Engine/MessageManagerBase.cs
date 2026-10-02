using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200006B RID: 107
	public abstract class MessageManagerBase : DotNetObject
	{
		// Token: 0x060009FD RID: 2557
		[EngineCallback(null, false)]
		protected internal abstract void PostWarningLine(string text);

		// Token: 0x060009FE RID: 2558
		[EngineCallback(null, false)]
		protected internal abstract void PostSuccessLine(string text);

		// Token: 0x060009FF RID: 2559
		[EngineCallback(null, false)]
		protected internal abstract void PostMessageLineFormatted(string text, uint color);

		// Token: 0x06000A00 RID: 2560
		[EngineCallback(null, false)]
		protected internal abstract void PostMessageLine(string text, uint color);
	}
}
