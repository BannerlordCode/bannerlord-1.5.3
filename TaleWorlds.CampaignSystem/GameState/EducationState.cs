using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003A9 RID: 937
	public class EducationState : GameState
	{
		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x060036C7 RID: 14023 RVA: 0x000DF2A9 File Offset: 0x000DD4A9
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x060036C8 RID: 14024 RVA: 0x000DF2AC File Offset: 0x000DD4AC
		// (set) Token: 0x060036C9 RID: 14025 RVA: 0x000DF2B4 File Offset: 0x000DD4B4
		public Hero Child { get; private set; }

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x060036CA RID: 14026 RVA: 0x000DF2BD File Offset: 0x000DD4BD
		// (set) Token: 0x060036CB RID: 14027 RVA: 0x000DF2C5 File Offset: 0x000DD4C5
		public IEducationStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x000DF2CE File Offset: 0x000DD4CE
		public EducationState()
		{
			Debug.FailedAssert("Do not use EducationState with default constructor!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameState\\EducationState.cs", ".ctor", 22);
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x000DF2EC File Offset: 0x000DD4EC
		public EducationState(Hero child)
		{
			this.Child = child;
		}

		// Token: 0x04000F67 RID: 3943
		private IEducationStateHandler _handler;
	}
}
