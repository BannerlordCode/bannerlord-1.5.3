using System;

namespace TaleWorlds.Engine.Options
{
	// Token: 0x020000A7 RID: 167
	public interface IOptionData
	{
		// Token: 0x06000F6A RID: 3946
		float GetDefaultValue();

		// Token: 0x06000F6B RID: 3947
		void Commit();

		// Token: 0x06000F6C RID: 3948
		float GetValue(bool forceRefresh);

		// Token: 0x06000F6D RID: 3949
		void SetValue(float value);

		// Token: 0x06000F6E RID: 3950
		object GetOptionType();

		// Token: 0x06000F6F RID: 3951
		bool IsNative();

		// Token: 0x06000F70 RID: 3952
		bool IsAction();

		// Token: 0x06000F71 RID: 3953
		ValueTuple<string, bool> GetIsDisabledAndReasonID();
	}
}
