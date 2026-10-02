using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200004A RID: 74
	public struct PropertyChangedWithBoolValueEventArgs
	{
		// Token: 0x06000266 RID: 614 RVA: 0x00007CFB File Offset: 0x00005EFB
		public PropertyChangedWithBoolValueEventArgs(string propertyName, bool value)
		{
			this.PropertyName = propertyName;
			this.Value = value;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00007D0B File Offset: 0x00005F0B
		public string PropertyName { get; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00007D13 File Offset: 0x00005F13
		public bool Value { get; }
	}
}
