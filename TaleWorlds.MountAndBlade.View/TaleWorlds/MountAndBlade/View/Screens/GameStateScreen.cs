using System;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x02000055 RID: 85
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class GameStateScreen : Attribute
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00011FE8 File Offset: 0x000101E8
		// (set) Token: 0x060002BE RID: 702 RVA: 0x00011FF0 File Offset: 0x000101F0
		public Type GameStateType { get; private set; }

		// Token: 0x060002BF RID: 703 RVA: 0x00011FF9 File Offset: 0x000101F9
		public GameStateScreen(Type gameStateType)
		{
			this.GameStateType = gameStateType;
		}
	}
}
