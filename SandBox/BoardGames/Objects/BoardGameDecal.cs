using System;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Objects
{
	// Token: 0x02000102 RID: 258
	public class BoardGameDecal : ScriptComponentBehavior
	{
		// Token: 0x06000CEC RID: 3308 RVA: 0x0005EE82 File Offset: 0x0005D082
		protected override void OnInit()
		{
			base.OnInit();
			this.SetAlpha(0f);
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0005EE98 File Offset: 0x0005D098
		public void SetAlpha(float alpha)
		{
			base.GameEntity.SetAlpha(alpha);
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0005EEB4 File Offset: 0x0005D0B4
		protected override bool MovesEntity()
		{
			return false;
		}
	}
}
