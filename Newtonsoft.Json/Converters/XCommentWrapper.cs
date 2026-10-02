using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FB RID: 251
	[NullableContext(2)]
	[Nullable(0)]
	internal class XCommentWrapper : XObjectWrapper
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x00032F64 File Offset: 0x00031164
		[Nullable(1)]
		private XComment Text
		{
			[NullableContext(1)]
			get
			{
				return (XComment)base.WrappedNode;
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x00032F71 File Offset: 0x00031171
		[NullableContext(1)]
		public XCommentWrapper(XComment text)
			: base(text)
		{
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x00032F7A File Offset: 0x0003117A
		// (set) Token: 0x06000CFE RID: 3326 RVA: 0x00032F87 File Offset: 0x00031187
		public override string Value
		{
			get
			{
				return this.Text.Value;
			}
			set
			{
				this.Text.Value = value ?? string.Empty;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x00032F9E File Offset: 0x0003119E
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Text.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Text.Parent);
			}
		}
	}
}
