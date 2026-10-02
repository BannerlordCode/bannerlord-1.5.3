using System;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Sound
{
	// Token: 0x02000086 RID: 134
	public class MusicSilencedMissionView : MissionView, IMusicHandler
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00026AC8 File Offset: 0x00024CC8
		bool IMusicHandler.IsPausable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00026ACB File Offset: 0x00024CCB
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			MBMusicManager.Current.DeactivateCurrentMode();
			MBMusicManager.Current.OnSilencedMusicHandlerInit(this);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00026AE8 File Offset: 0x00024CE8
		public override void OnMissionScreenFinalize()
		{
			MBMusicManager.Current.OnSilencedMusicHandlerFinalize();
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00026AF4 File Offset: 0x00024CF4
		void IMusicHandler.OnUpdated(float dt)
		{
		}
	}
}
