using System;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x0200002F RID: 47
	public class SaveError
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000A8ED File Offset: 0x00008AED
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x0000A8F5 File Offset: 0x00008AF5
		public string Message { get; private set; }

		// Token: 0x060001F5 RID: 501 RVA: 0x0000A8FE File Offset: 0x00008AFE
		internal SaveError(string message)
		{
			this.Message = message;
		}
	}
}
