using System;
using System.Diagnostics;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200007F RID: 127
	[EngineClass("rglResource")]
	public abstract class Resource : NativeObject
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x0000B202 File Offset: 0x00009402
		public bool IsValid
		{
			get
			{
				return base.Pointer != UIntPtr.Zero;
			}
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0000B214 File Offset: 0x00009414
		protected Resource()
		{
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0000B21C File Offset: 0x0000941C
		internal Resource(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0000B22B File Offset: 0x0000942B
		[Conditional("_RGL_KEEP_ASSERTS")]
		protected void CheckResourceParameter(Resource param, string paramName = "")
		{
			if (param == null)
			{
				throw new NullReferenceException(paramName);
			}
			if (!param.IsValid)
			{
				throw new ArgumentException(paramName);
			}
		}
	}
}
