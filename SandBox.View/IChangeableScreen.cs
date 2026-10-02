using System;

namespace SandBox.View
{
	// Token: 0x02000006 RID: 6
	public interface IChangeableScreen
	{
		// Token: 0x0600000E RID: 14
		bool AnyUnsavedChanges();

		// Token: 0x0600000F RID: 15
		bool CanChangesBeApplied();

		// Token: 0x06000010 RID: 16
		void ApplyChanges();

		// Token: 0x06000011 RID: 17
		void ResetChanges();
	}
}
