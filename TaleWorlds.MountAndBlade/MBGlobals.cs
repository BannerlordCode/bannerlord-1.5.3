using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D1 RID: 465
	public static class MBGlobals
	{
		// Token: 0x06001C05 RID: 7173 RVA: 0x00061207 File Offset: 0x0005F407
		public static void InitializeReferences()
		{
			if (!MBGlobals._initialized)
			{
				MBGlobals._actionSets = new Dictionary<string, MBActionSet>();
				MBGlobals._initialized = true;
			}
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x00061220 File Offset: 0x0005F420
		public static MBActionSet GetActionSetWithSuffix(Monster monster, bool isFemale, string suffix)
		{
			return MBGlobals.GetActionSet(ActionSetCode.GenerateActionSetNameWithSuffix(monster, isFemale, suffix));
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x00061230 File Offset: 0x0005F430
		public static MBActionSet GetActionSet(string actionSetCode)
		{
			MBActionSet actionSet;
			if (!MBGlobals._actionSets.TryGetValue(actionSetCode, out actionSet))
			{
				actionSet = MBActionSet.GetActionSet(actionSetCode);
				if (!actionSet.IsValid)
				{
					Debug.FailedAssert("No action set found with action set code: " + actionSetCode, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Base\\MBGlobals.cs", "GetActionSet", 40);
					throw new Exception("Invalid action set code");
				}
				MBGlobals._actionSets[actionSetCode] = actionSet;
			}
			return actionSet;
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x00061290 File Offset: 0x0005F490
		public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
		{
			return ((MemberExpression)memberExpression.Body).Member.Name;
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x000612A7 File Offset: 0x0005F4A7
		public static string GetMethodName<T>(Expression<Func<T>> memberExpression)
		{
			return ((MethodCallExpression)memberExpression.Body).Method.Name;
		}

		// Token: 0x04000929 RID: 2345
		public const float Gravity = 9.806f;

		// Token: 0x0400092A RID: 2346
		public static readonly Vec3 GravitationalAcceleration = new Vec3(0f, 0f, -9.806f, -1f);

		// Token: 0x0400092B RID: 2347
		private static bool _initialized;

		// Token: 0x0400092C RID: 2348
		private static Dictionary<string, MBActionSet> _actionSets;
	}
}
