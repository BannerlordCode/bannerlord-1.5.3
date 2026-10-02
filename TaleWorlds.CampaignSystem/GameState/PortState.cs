using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003B9 RID: 953
	public class PortState : GameState
	{
		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06003759 RID: 14169 RVA: 0x000DFAA2 File Offset: 0x000DDCA2
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x000DFAA5 File Offset: 0x000DDCA5
		public PortState()
		{
			Debug.FailedAssert("do not use parameterless constructor.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameState\\PortState.cs", ".ctor", 38);
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x000DFAC4 File Offset: 0x000DDCC4
		public PortState(PartyBase leftOwner, PartyBase rightOwner, PortScreenModes portScreenMode)
		{
			this.PortScreenMode = portScreenMode;
			this.LeftOwner = leftOwner;
			this.RightOwner = rightOwner;
			this.LeftShips = ((leftOwner != null) ? leftOwner.Ships : null);
			this.RightShips = ((rightOwner != null) ? rightOwner.Ships : null);
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x000DFB10 File Offset: 0x000DDD10
		public PortState(PartyBase leftOwner, PartyBase rightOwner, Action onEndAction, PortScreenModes portScreenMode)
		{
			this.PortScreenMode = portScreenMode;
			this.LeftOwner = leftOwner;
			this.RightOwner = rightOwner;
			this.LeftShips = ((leftOwner != null) ? leftOwner.Ships : null);
			this.RightShips = ((rightOwner != null) ? rightOwner.Ships : null);
			this.OnEndAction = onEndAction;
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x000DFB64 File Offset: 0x000DDD64
		public PortState(MBReadOnlyList<Ship> leftShips, MBReadOnlyList<Ship> rightShips, PortScreenModes portScreenMode)
		{
			this.PortScreenMode = portScreenMode;
			this.LeftOwner = null;
			this.RightOwner = null;
			this.LeftShips = leftShips;
			this.RightShips = rightShips;
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x000DFB8F File Offset: 0x000DDD8F
		public PortState(PartyBase leftOwner, PartyBase rightOwner, MBReadOnlyList<Ship> leftShips, MBReadOnlyList<Ship> rightShips, PortScreenModes portScreenMode)
		{
			this.PortScreenMode = portScreenMode;
			this.LeftOwner = leftOwner;
			this.RightOwner = rightOwner;
			this.LeftShips = leftShips;
			this.RightShips = rightShips;
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x000DFBBC File Offset: 0x000DDDBC
		public PortState(PartyBase leftOwner, PartyBase rightOwner, MBReadOnlyList<Ship> leftShips, MBReadOnlyList<Ship> rightShips, Action onEndAction, PortScreenModes portScreenMode)
		{
			this.PortScreenMode = portScreenMode;
			this.LeftOwner = leftOwner;
			this.RightOwner = rightOwner;
			this.LeftShips = leftShips;
			this.RightShips = rightShips;
			this.OnEndAction = onEndAction;
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x000DFBF1 File Offset: 0x000DDDF1
		public PortState(Settlement settlement, PartyBase rightOwner, PortScreenModes portScreenMode)
		{
			this.PortScreenMode = portScreenMode;
			this.LeftOwner = settlement.Party;
			this.RightOwner = rightOwner;
			this.LeftShips = settlement.Party.Ships;
			this.RightShips = rightOwner.Ships;
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x000DFC30 File Offset: 0x000DDE30
		protected override void OnFinalize()
		{
			base.OnFinalize();
			Action onEndAction = this.OnEndAction;
			if (onEndAction == null)
			{
				return;
			}
			onEndAction();
		}

		// Token: 0x04000F86 RID: 3974
		public readonly PortScreenModes PortScreenMode;

		// Token: 0x04000F87 RID: 3975
		public readonly PartyBase LeftOwner;

		// Token: 0x04000F88 RID: 3976
		public readonly PartyBase RightOwner;

		// Token: 0x04000F89 RID: 3977
		public readonly MBReadOnlyList<Ship> LeftShips;

		// Token: 0x04000F8A RID: 3978
		public readonly MBReadOnlyList<Ship> RightShips;

		// Token: 0x04000F8B RID: 3979
		public readonly Action OnEndAction;
	}
}
