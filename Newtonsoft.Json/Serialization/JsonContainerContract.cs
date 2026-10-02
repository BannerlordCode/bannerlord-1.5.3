using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000082 RID: 130
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonContainerContract : JsonContract
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x0001BDAE File Offset: 0x00019FAE
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x0001BDB6 File Offset: 0x00019FB6
		internal JsonContract ItemContract
		{
			get
			{
				return this._itemContract;
			}
			set
			{
				this._itemContract = value;
				if (this._itemContract != null)
				{
					this._finalItemContract = (this._itemContract.UnderlyingType.IsSealed() ? this._itemContract : null);
					return;
				}
				this._finalItemContract = null;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x0001BDF0 File Offset: 0x00019FF0
		internal JsonContract FinalItemContract
		{
			get
			{
				return this._finalItemContract;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x0001BDF8 File Offset: 0x00019FF8
		// (set) Token: 0x06000689 RID: 1673 RVA: 0x0001BE00 File Offset: 0x0001A000
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001BE09 File Offset: 0x0001A009
		// (set) Token: 0x0600068B RID: 1675 RVA: 0x0001BE11 File Offset: 0x0001A011
		public bool? ItemIsReference { get; set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0001BE1A File Offset: 0x0001A01A
		// (set) Token: 0x0600068D RID: 1677 RVA: 0x0001BE22 File Offset: 0x0001A022
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x0001BE2B File Offset: 0x0001A02B
		// (set) Token: 0x0600068F RID: 1679 RVA: 0x0001BE33 File Offset: 0x0001A033
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x06000690 RID: 1680 RVA: 0x0001BE3C File Offset: 0x0001A03C
		[NullableContext(1)]
		internal JsonContainerContract(Type underlyingType)
			: base(underlyingType)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(underlyingType);
			if (cachedAttribute != null)
			{
				if (cachedAttribute.ItemConverterType != null)
				{
					this.ItemConverter = JsonTypeReflector.CreateJsonConverterInstance(cachedAttribute.ItemConverterType, cachedAttribute.ItemConverterParameters);
				}
				this.ItemIsReference = cachedAttribute._itemIsReference;
				this.ItemReferenceLoopHandling = cachedAttribute._itemReferenceLoopHandling;
				this.ItemTypeNameHandling = cachedAttribute._itemTypeNameHandling;
			}
		}

		// Token: 0x04000255 RID: 597
		private JsonContract _itemContract;

		// Token: 0x04000256 RID: 598
		private JsonContract _finalItemContract;
	}
}
