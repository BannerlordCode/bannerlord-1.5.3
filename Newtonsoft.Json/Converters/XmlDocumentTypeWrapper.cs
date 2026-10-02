using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F0 RID: 240
	[NullableContext(2)]
	[Nullable(0)]
	internal class XmlDocumentTypeWrapper : XmlNodeWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x06000C9F RID: 3231 RVA: 0x000329B5 File Offset: 0x00030BB5
		[NullableContext(1)]
		public XmlDocumentTypeWrapper(XmlDocumentType documentType)
			: base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x000329C5 File Offset: 0x00030BC5
		[Nullable(1)]
		public string Name
		{
			[NullableContext(1)]
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x000329D2 File Offset: 0x00030BD2
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x000329DF File Offset: 0x00030BDF
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x000329EC File Offset: 0x00030BEC
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x000329F9 File Offset: 0x00030BF9
		public override string LocalName
		{
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x04000405 RID: 1029
		[Nullable(1)]
		private readonly XmlDocumentType _documentType;
	}
}
