using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000188 RID: 392
	public struct TacticalDecision
	{
		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x0600150C RID: 5388 RVA: 0x0004D763 File Offset: 0x0004B963
		// (set) Token: 0x0600150D RID: 5389 RVA: 0x0004D76B File Offset: 0x0004B96B
		public TacticComponent DecidingComponent { get; private set; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x0600150E RID: 5390 RVA: 0x0004D774 File Offset: 0x0004B974
		// (set) Token: 0x0600150F RID: 5391 RVA: 0x0004D77C File Offset: 0x0004B97C
		public byte DecisionCode { get; private set; }

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001510 RID: 5392 RVA: 0x0004D785 File Offset: 0x0004B985
		// (set) Token: 0x06001511 RID: 5393 RVA: 0x0004D78D File Offset: 0x0004B98D
		public Formation SubjectFormation { get; private set; }

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001512 RID: 5394 RVA: 0x0004D796 File Offset: 0x0004B996
		// (set) Token: 0x06001513 RID: 5395 RVA: 0x0004D79E File Offset: 0x0004B99E
		public Formation TargetFormation { get; private set; }

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001514 RID: 5396 RVA: 0x0004D7A7 File Offset: 0x0004B9A7
		// (set) Token: 0x06001515 RID: 5397 RVA: 0x0004D7AF File Offset: 0x0004B9AF
		public WorldPosition? TargetPosition { get; private set; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001516 RID: 5398 RVA: 0x0004D7B8 File Offset: 0x0004B9B8
		// (set) Token: 0x06001517 RID: 5399 RVA: 0x0004D7C0 File Offset: 0x0004B9C0
		public MissionObject TargetObject { get; private set; }

		// Token: 0x06001518 RID: 5400 RVA: 0x0004D7C9 File Offset: 0x0004B9C9
		public TacticalDecision(TacticComponent decidingComponent, byte decisionCode, Formation subjectFormation = null, Formation targetFormation = null, WorldPosition? targetPosition = null, MissionObject targetObject = null)
		{
			this.DecidingComponent = decidingComponent;
			this.DecisionCode = decisionCode;
			this.SubjectFormation = subjectFormation;
			this.TargetFormation = targetFormation;
			this.TargetPosition = targetPosition;
			this.TargetObject = targetObject;
		}
	}
}
