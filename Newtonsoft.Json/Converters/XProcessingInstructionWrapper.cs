using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FC RID: 252
	[NullableContext(2)]
	[Nullable(0)]
	internal class XProcessingInstructionWrapper : XObjectWrapper
	{
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00032FBF File Offset: 0x000311BF
		[Nullable(1)]
		private XProcessingInstruction ProcessingInstruction
		{
			[NullableContext(1)]
			get
			{
				return (XProcessingInstruction)base.WrappedNode;
			}
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x00032FCC File Offset: 0x000311CC
		[NullableContext(1)]
		public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction)
			: base(processingInstruction)
		{
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00032FD5 File Offset: 0x000311D5
		public override string LocalName
		{
			get
			{
				return this.ProcessingInstruction.Target;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x00032FE2 File Offset: 0x000311E2
		// (set) Token: 0x06000D04 RID: 3332 RVA: 0x00032FEF File Offset: 0x000311EF
		public override string Value
		{
			get
			{
				return this.ProcessingInstruction.Data;
			}
			set
			{
				this.ProcessingInstruction.Data = value ?? string.Empty;
			}
		}
	}
}
