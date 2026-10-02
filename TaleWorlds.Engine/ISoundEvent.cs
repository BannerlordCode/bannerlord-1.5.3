using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000042 RID: 66
	[ApplicationInterfaceBase]
	internal interface ISoundEvent
	{
		// Token: 0x060006AE RID: 1710
		[EngineMethod("create_event_from_string", false, null, false)]
		int CreateEventFromString(string eventName, UIntPtr scene);

		// Token: 0x060006AF RID: 1711
		[EngineMethod("get_event_id_from_string", false, null, false)]
		int GetEventIdFromString(string eventName);

		// Token: 0x060006B0 RID: 1712
		[EngineMethod("play_sound_2d", false, null, false)]
		bool PlaySound2D(int fmodEventIndex);

		// Token: 0x060006B1 RID: 1713
		[EngineMethod("get_total_event_count", false, null, false)]
		int GetTotalEventCount();

		// Token: 0x060006B2 RID: 1714
		[EngineMethod("set_event_min_max_distance", false, null, false)]
		void SetEventMinMaxDistance(int fmodEventIndex, Vec3 radius);

		// Token: 0x060006B3 RID: 1715
		[EngineMethod("create_event", false, null, false)]
		int CreateEvent(int fmodEventIndex, UIntPtr scene);

		// Token: 0x060006B4 RID: 1716
		[EngineMethod("release_event", false, null, false)]
		void ReleaseEvent(int eventId);

		// Token: 0x060006B5 RID: 1717
		[EngineMethod("set_event_parameter_from_string", false, null, false)]
		void SetEventParameterFromString(int eventId, string name, float value);

		// Token: 0x060006B6 RID: 1718
		[EngineMethod("get_event_min_max_distance", false, null, false)]
		Vec3 GetEventMinMaxDistance(int eventId);

		// Token: 0x060006B7 RID: 1719
		[EngineMethod("set_event_position", false, null, true)]
		void SetEventPosition(int eventId, ref Vec3 position);

		// Token: 0x060006B8 RID: 1720
		[EngineMethod("set_event_velocity", false, null, false)]
		void SetEventVelocity(int eventId, ref Vec3 velocity);

		// Token: 0x060006B9 RID: 1721
		[EngineMethod("start_event", false, null, false)]
		bool StartEvent(int eventId);

		// Token: 0x060006BA RID: 1722
		[EngineMethod("start_event_in_position", false, null, false)]
		bool StartEventInPosition(int eventId, ref Vec3 position);

		// Token: 0x060006BB RID: 1723
		[EngineMethod("stop_event", false, null, false)]
		void StopEvent(int eventId);

		// Token: 0x060006BC RID: 1724
		[EngineMethod("pause_event", false, null, false)]
		void PauseEvent(int eventId);

		// Token: 0x060006BD RID: 1725
		[EngineMethod("resume_event", false, null, false)]
		void ResumeEvent(int eventId);

		// Token: 0x060006BE RID: 1726
		[EngineMethod("play_extra_event", false, null, false)]
		void PlayExtraEvent(int soundId, string eventName);

		// Token: 0x060006BF RID: 1727
		[EngineMethod("set_switch", false, null, false)]
		void SetSwitch(int soundId, string switchGroupName, string newSwitchStateName);

		// Token: 0x060006C0 RID: 1728
		[EngineMethod("trigger_cue", false, null, false)]
		void TriggerCue(int eventId);

		// Token: 0x060006C1 RID: 1729
		[EngineMethod("set_event_parameter_at_index", false, null, false)]
		void SetEventParameterAtIndex(int soundId, int parameterIndex, float value);

		// Token: 0x060006C2 RID: 1730
		[EngineMethod("is_playing", false, null, false)]
		bool IsPlaying(int eventId);

		// Token: 0x060006C3 RID: 1731
		[EngineMethod("is_stopped", false, null, false)]
		bool IsStopped(int eventId);

		// Token: 0x060006C4 RID: 1732
		[EngineMethod("is_paused", false, null, false)]
		bool IsPaused(int eventId);

		// Token: 0x060006C5 RID: 1733
		[EngineMethod("is_valid", false, null, true)]
		bool IsValid(int eventId);

		// Token: 0x060006C6 RID: 1734
		[EngineMethod("create_event_from_external_file", false, null, false)]
		int CreateEventFromExternalFile(string programmerSoundEventName, string filePath, UIntPtr scene, bool is3d, bool isBlocking);

		// Token: 0x060006C7 RID: 1735
		[EngineMethod("create_event_from_sound_buffer", false, null, false)]
		int CreateEventFromSoundBuffer(string programmerSoundEventName, byte[] soundBuffer, UIntPtr scene, bool is3d, bool isBlocking);
	}
}
