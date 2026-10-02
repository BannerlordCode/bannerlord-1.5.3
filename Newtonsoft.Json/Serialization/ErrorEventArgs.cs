using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000079 RID: 121
	[NullableContext(1)]
	[Nullable(0)]
	public class ErrorEventArgs : EventArgs
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x0001B59F File Offset: 0x0001979F
		[Nullable(2)]
		public object CurrentObject
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x0001B5A7 File Offset: 0x000197A7
		public ErrorContext ErrorContext { get; }

		// Token: 0x06000664 RID: 1636 RVA: 0x0001B5AF File Offset: 0x000197AF
		public ErrorEventArgs([Nullable(2)] object currentObject, ErrorContext errorContext)
		{
			this.CurrentObject = currentObject;
			this.ErrorContext = errorContext;
		}
	}
}
