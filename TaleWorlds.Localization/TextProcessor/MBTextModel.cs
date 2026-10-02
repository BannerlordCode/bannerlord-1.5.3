using System;
using TaleWorlds.Library;
using TaleWorlds.Localization.Expressions;

namespace TaleWorlds.Localization.TextProcessor
{
	// Token: 0x02000026 RID: 38
	public class MBTextModel
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000ED RID: 237 RVA: 0x0000525A File Offset: 0x0000345A
		internal MBReadOnlyList<TextExpression> RootExpressions
		{
			get
			{
				return this._rootExpressions;
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005275 File Offset: 0x00003475
		internal void AddRootExpression(TextExpression newExp)
		{
			this._rootExpressions.Add(newExp);
		}

		// Token: 0x04000059 RID: 89
		internal MBList<TextExpression> _rootExpressions = new MBList<TextExpression>();
	}
}
