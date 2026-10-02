using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000059 RID: 89
	[NullableContext(2)]
	[Nullable(0)]
	internal class FSharpFunction
	{
		// Token: 0x06000518 RID: 1304 RVA: 0x00015C95 File Offset: 0x00013E95
		public FSharpFunction(object instance, [Nullable(new byte[] { 1, 2, 1 })] MethodCall<object, object> invoker)
		{
			this._instance = instance;
			this._invoker = invoker;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00015CAB File Offset: 0x00013EAB
		[NullableContext(1)]
		public object Invoke(params object[] args)
		{
			return this._invoker(this._instance, args);
		}

		// Token: 0x040001E5 RID: 485
		private readonly object _instance;

		// Token: 0x040001E6 RID: 486
		[Nullable(new byte[] { 1, 2, 1 })]
		private readonly MethodCall<object, object> _invoker;
	}
}
