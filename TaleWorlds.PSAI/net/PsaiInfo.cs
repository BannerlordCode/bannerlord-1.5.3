using System;

namespace psai.net
{
	// Token: 0x02000018 RID: 24
	public struct PsaiInfo
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000929D File Offset: 0x0000749D
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x000092A5 File Offset: 0x000074A5
		public PsaiState psaiState { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x000092AE File Offset: 0x000074AE
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x000092B6 File Offset: 0x000074B6
		public PsaiState upcomingPsaiState { get; private set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x000092BF File Offset: 0x000074BF
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x000092C7 File Offset: 0x000074C7
		public int lastBasicMoodThemeId { get; private set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x000092D0 File Offset: 0x000074D0
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x000092D8 File Offset: 0x000074D8
		public int effectiveThemeId { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x000092E1 File Offset: 0x000074E1
		// (set) Token: 0x060001CA RID: 458 RVA: 0x000092E9 File Offset: 0x000074E9
		public int upcomingThemeId { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000092F2 File Offset: 0x000074F2
		// (set) Token: 0x060001CC RID: 460 RVA: 0x000092FA File Offset: 0x000074FA
		public float currentIntensity { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00009303 File Offset: 0x00007503
		// (set) Token: 0x060001CE RID: 462 RVA: 0x0000930B File Offset: 0x0000750B
		public float upcomingIntensity { get; private set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00009314 File Offset: 0x00007514
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000931C File Offset: 0x0000751C
		public int themesQueued { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00009325 File Offset: 0x00007525
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000932D File Offset: 0x0000752D
		public int targetSegmentId { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00009336 File Offset: 0x00007536
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x0000933E File Offset: 0x0000753E
		public bool intensityIsHeld { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00009347 File Offset: 0x00007547
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x0000934F File Offset: 0x0000754F
		public bool returningToLastBasicMood { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00009358 File Offset: 0x00007558
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00009360 File Offset: 0x00007560
		public int remainingMillisecondsInRestMode { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00009369 File Offset: 0x00007569
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00009371 File Offset: 0x00007571
		public bool paused { get; private set; }

		// Token: 0x060001DB RID: 475 RVA: 0x0000937C File Offset: 0x0000757C
		public PsaiInfo(PsaiState m_psaiState, PsaiState m_upcomingPsaiState, int m_lastBasicMoodThemeId, int m_effectiveThemeId, int m_upcomingThemeId, float m_currentIntensity, float m_upcomingIntensity, int m_themesQueued, int m_targetSegmentId, bool m_intensityIsHeld, bool m_returningToLastBasicMood, int m_remainingMillisecondsInRestMode, bool m_paused)
		{
			this.psaiState = m_psaiState;
			this.upcomingPsaiState = m_upcomingPsaiState;
			this.lastBasicMoodThemeId = m_lastBasicMoodThemeId;
			this.effectiveThemeId = m_effectiveThemeId;
			this.upcomingThemeId = m_upcomingThemeId;
			this.currentIntensity = m_currentIntensity;
			this.upcomingIntensity = m_upcomingIntensity;
			this.themesQueued = m_themesQueued;
			this.targetSegmentId = m_targetSegmentId;
			this.intensityIsHeld = m_intensityIsHeld;
			this.returningToLastBasicMood = m_returningToLastBasicMood;
			this.remainingMillisecondsInRestMode = m_remainingMillisecondsInRestMode;
			this.paused = m_paused;
		}
	}
}
