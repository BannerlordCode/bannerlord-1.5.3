using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C1 RID: 193
	public class JsonLoadSettings
	{
		// Token: 0x06000AA9 RID: 2729 RVA: 0x0002A71C File Offset: 0x0002891C
		public JsonLoadSettings()
		{
			this._lineInfoHandling = LineInfoHandling.Load;
			this._commentHandling = CommentHandling.Ignore;
			this._duplicatePropertyNameHandling = DuplicatePropertyNameHandling.Replace;
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x0002A739 File Offset: 0x00028939
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x0002A741 File Offset: 0x00028941
		public CommentHandling CommentHandling
		{
			get
			{
				return this._commentHandling;
			}
			set
			{
				if (value < CommentHandling.Ignore || value > CommentHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._commentHandling = value;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x0002A75D File Offset: 0x0002895D
		// (set) Token: 0x06000AAD RID: 2733 RVA: 0x0002A765 File Offset: 0x00028965
		public LineInfoHandling LineInfoHandling
		{
			get
			{
				return this._lineInfoHandling;
			}
			set
			{
				if (value < LineInfoHandling.Ignore || value > LineInfoHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._lineInfoHandling = value;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0002A781 File Offset: 0x00028981
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x0002A789 File Offset: 0x00028989
		public DuplicatePropertyNameHandling DuplicatePropertyNameHandling
		{
			get
			{
				return this._duplicatePropertyNameHandling;
			}
			set
			{
				if (value < DuplicatePropertyNameHandling.Replace || value > DuplicatePropertyNameHandling.Error)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._duplicatePropertyNameHandling = value;
			}
		}

		// Token: 0x04000388 RID: 904
		private CommentHandling _commentHandling;

		// Token: 0x04000389 RID: 905
		private LineInfoHandling _lineInfoHandling;

		// Token: 0x0400038A RID: 906
		private DuplicatePropertyNameHandling _duplicatePropertyNameHandling;
	}
}
