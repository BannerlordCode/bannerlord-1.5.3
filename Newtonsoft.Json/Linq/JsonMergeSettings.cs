using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C2 RID: 194
	public class JsonMergeSettings
	{
		// Token: 0x06000AB0 RID: 2736 RVA: 0x0002A7A5 File Offset: 0x000289A5
		public JsonMergeSettings()
		{
			this._propertyNameComparison = StringComparison.Ordinal;
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0002A7B4 File Offset: 0x000289B4
		// (set) Token: 0x06000AB2 RID: 2738 RVA: 0x0002A7BC File Offset: 0x000289BC
		public MergeArrayHandling MergeArrayHandling
		{
			get
			{
				return this._mergeArrayHandling;
			}
			set
			{
				if (value < MergeArrayHandling.Concat || value > MergeArrayHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeArrayHandling = value;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0002A7D8 File Offset: 0x000289D8
		// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x0002A7E0 File Offset: 0x000289E0
		public MergeNullValueHandling MergeNullValueHandling
		{
			get
			{
				return this._mergeNullValueHandling;
			}
			set
			{
				if (value < MergeNullValueHandling.Ignore || value > MergeNullValueHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeNullValueHandling = value;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x0002A7FC File Offset: 0x000289FC
		// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x0002A804 File Offset: 0x00028A04
		public StringComparison PropertyNameComparison
		{
			get
			{
				return this._propertyNameComparison;
			}
			set
			{
				if (value < StringComparison.CurrentCulture || value > StringComparison.OrdinalIgnoreCase)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._propertyNameComparison = value;
			}
		}

		// Token: 0x0400038B RID: 907
		private MergeArrayHandling _mergeArrayHandling;

		// Token: 0x0400038C RID: 908
		private MergeNullValueHandling _mergeNullValueHandling;

		// Token: 0x0400038D RID: 909
		private StringComparison _propertyNameComparison;
	}
}
