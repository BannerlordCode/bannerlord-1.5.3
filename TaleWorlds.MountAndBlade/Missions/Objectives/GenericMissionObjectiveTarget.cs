using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Missions.Objectives
{
	// Token: 0x020003F2 RID: 1010
	internal class GenericMissionObjectiveTarget<T> : MissionObjectiveTarget<T>
	{
		// Token: 0x060037DC RID: 14300 RVA: 0x000E7B43 File Offset: 0x000E5D43
		public GenericMissionObjectiveTarget(T target)
			: base(target)
		{
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x000E7B4C File Offset: 0x000E5D4C
		public override bool IsActive()
		{
			return this.IsActiveCallback == null || this.IsActiveCallback(base.Target);
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x000E7B69 File Offset: 0x000E5D69
		public override TextObject GetName()
		{
			if (this.GetNameCallback != null)
			{
				return this.GetNameCallback(base.Target);
			}
			if (this.Name != null)
			{
				return this.Name;
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x000E7BA0 File Offset: 0x000E5DA0
		public override Vec3 GetGlobalPosition()
		{
			if (this.GetGlobalPositionCallback != null)
			{
				return this.GetGlobalPositionCallback(base.Target);
			}
			if (this.StaticPosition.IsValid)
			{
				return this.StaticPosition;
			}
			Debug.FailedAssert("Static target position or position getter callback was not set for generic mission objective target.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Objectives\\MissionObjectiveTarget.cs", "GetGlobalPosition", 74);
			return Vec3.Zero;
		}

		// Token: 0x04001822 RID: 6178
		internal Func<T, bool> IsActiveCallback;

		// Token: 0x04001823 RID: 6179
		internal Func<T, TextObject> GetNameCallback;

		// Token: 0x04001824 RID: 6180
		internal Func<T, Vec3> GetGlobalPositionCallback;

		// Token: 0x04001825 RID: 6181
		internal TextObject Name;

		// Token: 0x04001826 RID: 6182
		internal Vec3 StaticPosition;
	}
}
