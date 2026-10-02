using System;
using System.Collections.Generic;

namespace psai.net
{
	// Token: 0x0200001C RID: 28
	public class PsaiCore
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000945D File Offset: 0x0000765D
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00009475 File Offset: 0x00007675
		public static PsaiCore Instance
		{
			get
			{
				if (PsaiCore.s_singleton == null)
				{
					PsaiCore.s_singleton = new PsaiCore();
				}
				return PsaiCore.s_singleton;
			}
			set
			{
				PsaiCore.s_singleton = null;
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000947D File Offset: 0x0000767D
		public static bool IsInstanceInitialized()
		{
			return PsaiCore.s_singleton != null;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009487 File Offset: 0x00007687
		public PsaiCore()
		{
			this.m_logik = Logik.Instance;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000949A File Offset: 0x0000769A
		public PsaiResult SetMaximumLatencyNeededByPlatformToBufferSounddata(int latencyInMilliseconds)
		{
			return this.m_logik.SetMaximumLatencyNeededByPlatformToBufferSounddata(latencyInMilliseconds);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000094A8 File Offset: 0x000076A8
		public PsaiResult SetMaximumLatencyNeededByPlatformToPlayBackBufferedSounddata(int latencyInMilliseconds)
		{
			return this.m_logik.SetMaximumLatencyNeededByPlatformToPlayBackBufferedSounds(latencyInMilliseconds);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000094B6 File Offset: 0x000076B6
		public PsaiResult LoadSoundtrackFromProjectFile(List<string> pathToProjectFiles)
		{
			return this.m_logik.LoadSoundtrackFromProjectFile(pathToProjectFiles);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000094C4 File Offset: 0x000076C4
		public PsaiResult TriggerMusicTheme(int themeId, float intensity)
		{
			return this.m_logik.TriggerMusicTheme(themeId, intensity);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000094D3 File Offset: 0x000076D3
		public PsaiResult TriggerMusicTheme(int themeId, float intensity, int musicDurationInSeconds)
		{
			return this.m_logik.TriggerMusicTheme(themeId, intensity, musicDurationInSeconds);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000094E3 File Offset: 0x000076E3
		public PsaiResult AddToCurrentIntensity(float deltaIntensity)
		{
			return this.m_logik.AddToCurrentIntensity(deltaIntensity, false);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000094F2 File Offset: 0x000076F2
		public PsaiResult StopMusic(bool immediately)
		{
			return this.m_logik.StopMusic(immediately);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009500 File Offset: 0x00007700
		public PsaiResult StopMusic(bool immediately, float fadeOutSeconds)
		{
			return this.m_logik.StopMusic(immediately, (int)(fadeOutSeconds * 1000f));
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00009516 File Offset: 0x00007716
		public PsaiResult ReturnToLastBasicMood(bool immediately)
		{
			return this.m_logik.ReturnToLastBasicMood(immediately);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00009524 File Offset: 0x00007724
		public PsaiResult GoToRest(bool immediately, float fadeOutSeconds)
		{
			return this.m_logik.GoToRest(immediately, (int)(fadeOutSeconds * 1000f));
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000953A File Offset: 0x0000773A
		public PsaiResult GoToRest(bool immediately, float fadeOutSeconds, int restTimeMin, int restTimeMax)
		{
			return this.m_logik.GoToRest(immediately, (int)(fadeOutSeconds * 1000f), restTimeMin, restTimeMax);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00009553 File Offset: 0x00007753
		public PsaiResult HoldCurrentIntensity(bool hold)
		{
			return this.m_logik.HoldCurrentIntensity(hold);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00009561 File Offset: 0x00007761
		public string GetVersion()
		{
			return this.m_logik.getVersion();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000956E File Offset: 0x0000776E
		public PsaiResult Update()
		{
			return this.m_logik.Update();
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000957B File Offset: 0x0000777B
		public float GetVolume()
		{
			return this.m_logik.getVolume();
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00009588 File Offset: 0x00007788
		public void SetVolume(float volume)
		{
			this.m_logik.setVolume(volume);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00009596 File Offset: 0x00007796
		public void SetPaused(bool setPaused)
		{
			this.m_logik.setPaused(setPaused);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000095A5 File Offset: 0x000077A5
		public float GetCurrentIntensity()
		{
			return this.m_logik.getCurrentIntensity();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000095B2 File Offset: 0x000077B2
		public PsaiInfo GetPsaiInfo()
		{
			return this.m_logik.getPsaiInfo();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000095BF File Offset: 0x000077BF
		public SoundtrackInfo GetSoundtrackInfo()
		{
			return this.m_logik.m_soundtrack.getSoundtrackInfo();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000095D1 File Offset: 0x000077D1
		public ThemeInfo GetThemeInfo(int themeId)
		{
			return this.m_logik.m_soundtrack.getThemeInfo(themeId);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x000095E4 File Offset: 0x000077E4
		public SegmentInfo GetSegmentInfo(int segmentId)
		{
			return this.m_logik.m_soundtrack.getSegmentInfo(segmentId);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000095F7 File Offset: 0x000077F7
		public int GetCurrentSegmentId()
		{
			return this.m_logik.getCurrentSnippetId();
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00009604 File Offset: 0x00007804
		public int GetCurrentThemeId()
		{
			return this.m_logik.getEffectiveThemeId();
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00009611 File Offset: 0x00007811
		public int GetRemainingMillisecondsOfCurrentSegmentPlayback()
		{
			return this.m_logik.GetRemainingMillisecondsOfCurrentSegmentPlayback();
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000961E File Offset: 0x0000781E
		public int GetRemainingMillisecondsUntilNextSegmentStart()
		{
			return this.m_logik.GetRemainingMillisecondsUntilNextSegmentStart();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000962B File Offset: 0x0000782B
		public PsaiResult MenuModeEnter(int menuThemeId, float menuThemeIntensity)
		{
			return this.m_logik.MenuModeEnter(menuThemeId, menuThemeIntensity);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000963A File Offset: 0x0000783A
		public PsaiResult MenuModeLeave()
		{
			return this.m_logik.MenuModeLeave();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00009647 File Offset: 0x00007847
		public bool MenuModeIsActive()
		{
			return this.m_logik.menuModeIsActive();
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00009654 File Offset: 0x00007854
		public bool CutSceneIsActive()
		{
			return this.m_logik.cutSceneIsActive();
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00009661 File Offset: 0x00007861
		public PsaiResult CutSceneEnter(int themeId, float intensity)
		{
			return this.m_logik.CutSceneEnter(themeId, intensity);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00009670 File Offset: 0x00007870
		public PsaiResult CutSceneLeave(bool immediately, bool reset)
		{
			return this.m_logik.CutSceneLeave(immediately, reset);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000967F File Offset: 0x0000787F
		public PsaiResult PlaySegment(int segmentId)
		{
			return this.m_logik.PlaySegmentLayeredAndImmediately(segmentId);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000968D File Offset: 0x0000788D
		public bool CheckIfAtLeastOneDirectTransitionOrLayeringIsPossible(int sourceSegmentId, int targetThemeId)
		{
			return this.m_logik.CheckIfAtLeastOneDirectTransitionOrLayeringIsPossible(sourceSegmentId, targetThemeId);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000969C File Offset: 0x0000789C
		public void SetLastBasicMood(int themeId)
		{
			this.m_logik.SetLastBasicMood(themeId);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x000096AA File Offset: 0x000078AA
		public void Release()
		{
			this.m_logik.Release();
			this.m_logik = null;
		}

		// Token: 0x04000119 RID: 281
		private Logik m_logik;

		// Token: 0x0400011A RID: 282
		private static PsaiCore s_singleton;
	}
}
