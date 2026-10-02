using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F8 RID: 248
	[NullableContext(2)]
	[Nullable(0)]
	internal class XDocumentTypeWrapper : XObjectWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x06000CDE RID: 3294 RVA: 0x00032D21 File Offset: 0x00030F21
		[NullableContext(1)]
		public XDocumentTypeWrapper(XDocumentType documentType)
			: base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x00032D31 File Offset: 0x00030F31
		[Nullable(1)]
		public string Name
		{
			[NullableContext(1)]
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x00032D3E File Offset: 0x00030F3E
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x00032D4B File Offset: 0x00030F4B
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x00032D58 File Offset: 0x00030F58
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x00032D65 File Offset: 0x00030F65
		public override string LocalName
		{
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x0400040A RID: 1034
		[Nullable(1)]
		private readonly XDocumentType _documentType;
	}
}
