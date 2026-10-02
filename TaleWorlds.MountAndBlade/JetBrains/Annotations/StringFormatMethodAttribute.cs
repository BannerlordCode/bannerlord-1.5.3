using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000E3 RID: 227
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class StringFormatMethodAttribute : Attribute
	{
		// Token: 0x06000935 RID: 2357 RVA: 0x0000FA37 File Offset: 0x0000DC37
		public StringFormatMethodAttribute(string formatParameterName)
		{
			this.FormatParameterName = formatParameterName;
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x0000FA46 File Offset: 0x0000DC46
		// (set) Token: 0x06000937 RID: 2359 RVA: 0x0000FA4E File Offset: 0x0000DC4E
		[UsedImplicitly]
		public string FormatParameterName { get; private set; }
	}
}
