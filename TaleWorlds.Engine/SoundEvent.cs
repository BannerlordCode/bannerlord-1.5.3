using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200008E RID: 142
	public class SoundEvent
	{
		// Token: 0x06000CB9 RID: 3257 RVA: 0x0000E422 File Offset: 0x0000C622
		public int GetSoundId()
		{
			return this._soundId;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0000E42A File Offset: 0x0000C62A
		private SoundEvent(int soundId)
		{
			this._soundId = soundId;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0000E43C File Offset: 0x0000C63C
		public static SoundEvent CreateEventFromString(string eventId, Scene scene)
		{
			UIntPtr uintPtr = ((scene == null) ? UIntPtr.Zero : scene.Pointer);
			return new SoundEvent(EngineApplicationInterface.ISoundEvent.CreateEventFromString(eventId, uintPtr));
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0000E471 File Offset: 0x0000C671
		public void SetEventMinMaxDistance(Vec3 newRadius)
		{
			EngineApplicationInterface.ISoundEvent.SetEventMinMaxDistance(this._soundId, newRadius);
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0000E484 File Offset: 0x0000C684
		public static int GetEventIdFromString(string name)
		{
			return EngineApplicationInterface.ISoundEvent.GetEventIdFromString(name);
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0000E491 File Offset: 0x0000C691
		public static bool PlaySound2D(int soundCodeId)
		{
			return EngineApplicationInterface.ISoundEvent.PlaySound2D(soundCodeId);
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0000E49E File Offset: 0x0000C69E
		public static bool PlaySound2D(string soundName)
		{
			return SoundEvent.PlaySound2D(SoundEvent.GetEventIdFromString(soundName));
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0000E4AB File Offset: 0x0000C6AB
		public static int GetTotalEventCount()
		{
			return EngineApplicationInterface.ISoundEvent.GetTotalEventCount();
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0000E4B7 File Offset: 0x0000C6B7
		public static SoundEvent CreateEvent(int soundCodeId, Scene scene)
		{
			return new SoundEvent(EngineApplicationInterface.ISoundEvent.CreateEvent(soundCodeId, scene.Pointer));
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0000E4CF File Offset: 0x0000C6CF
		public bool IsNullSoundEvent()
		{
			return this == SoundEvent.NullSoundEvent;
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0000E4D9 File Offset: 0x0000C6D9
		public bool IsValid
		{
			get
			{
				return this._soundId != -1 && EngineApplicationInterface.ISoundEvent.IsValid(this._soundId);
			}
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0000E4F6 File Offset: 0x0000C6F6
		public bool Play()
		{
			return EngineApplicationInterface.ISoundEvent.StartEvent(this._soundId);
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0000E508 File Offset: 0x0000C708
		public void Pause()
		{
			EngineApplicationInterface.ISoundEvent.PauseEvent(this._soundId);
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0000E51A File Offset: 0x0000C71A
		public void Resume()
		{
			EngineApplicationInterface.ISoundEvent.ResumeEvent(this._soundId);
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0000E52C File Offset: 0x0000C72C
		public void PlayExtraEvent(string eventName)
		{
			EngineApplicationInterface.ISoundEvent.PlayExtraEvent(this._soundId, eventName);
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0000E53F File Offset: 0x0000C73F
		public void SetSwitch(string switchGroupName, string newSwitchStateName)
		{
			EngineApplicationInterface.ISoundEvent.SetSwitch(this._soundId, switchGroupName, newSwitchStateName);
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0000E553 File Offset: 0x0000C753
		public void TriggerCue()
		{
			EngineApplicationInterface.ISoundEvent.TriggerCue(this._soundId);
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0000E565 File Offset: 0x0000C765
		public bool PlayInPosition(Vec3 position)
		{
			return EngineApplicationInterface.ISoundEvent.StartEventInPosition(this._soundId, ref position);
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0000E579 File Offset: 0x0000C779
		public void Stop()
		{
			if (!this.IsValid)
			{
				return;
			}
			EngineApplicationInterface.ISoundEvent.StopEvent(this._soundId);
			this._soundId = -1;
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0000E59B File Offset: 0x0000C79B
		public void SetParameter(string parameterName, float value)
		{
			EngineApplicationInterface.ISoundEvent.SetEventParameterFromString(this._soundId, parameterName, value);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0000E5AF File Offset: 0x0000C7AF
		public void SetParameter(int parameterIndex, float value)
		{
			EngineApplicationInterface.ISoundEvent.SetEventParameterAtIndex(this._soundId, parameterIndex, value);
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0000E5C3 File Offset: 0x0000C7C3
		public Vec3 GetEventMinMaxDistance()
		{
			return EngineApplicationInterface.ISoundEvent.GetEventMinMaxDistance(this._soundId);
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0000E5D5 File Offset: 0x0000C7D5
		public void SetPosition(Vec3 vec)
		{
			if (!this.IsValid)
			{
				return;
			}
			EngineApplicationInterface.ISoundEvent.SetEventPosition(this._soundId, ref vec);
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0000E5F2 File Offset: 0x0000C7F2
		public void SetVelocity(Vec3 vec)
		{
			if (!this.IsValid)
			{
				return;
			}
			EngineApplicationInterface.ISoundEvent.SetEventVelocity(this._soundId, ref vec);
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x0000E610 File Offset: 0x0000C810
		public void Release()
		{
			MBDebug.Print("Release Sound Event " + this._soundId, 0, Debug.DebugColor.Red, 17592186044416UL);
			if (this.IsValid)
			{
				if (this.IsPlaying())
				{
					this.Stop();
				}
				EngineApplicationInterface.ISoundEvent.ReleaseEvent(this._soundId);
			}
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0000E668 File Offset: 0x0000C868
		public bool IsPlaying()
		{
			return EngineApplicationInterface.ISoundEvent.IsPlaying(this._soundId);
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0000E67A File Offset: 0x0000C87A
		public bool IsPaused()
		{
			return EngineApplicationInterface.ISoundEvent.IsPaused(this._soundId);
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0000E68C File Offset: 0x0000C88C
		public bool IsStopped()
		{
			return EngineApplicationInterface.ISoundEvent.IsStopped(this._soundId);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0000E69E File Offset: 0x0000C89E
		public static SoundEvent CreateEventFromSoundBuffer(string eventId, byte[] soundData, Scene scene, bool is3d, bool isBlocking)
		{
			return new SoundEvent(EngineApplicationInterface.ISoundEvent.CreateEventFromSoundBuffer(eventId, soundData, (scene != null) ? scene.Pointer : UIntPtr.Zero, is3d, isBlocking));
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0000E6CA File Offset: 0x0000C8CA
		public static SoundEvent CreateEventFromExternalFile(string programmerEventName, string soundFilePath, Scene scene, bool is3d, bool isBlocking)
		{
			return new SoundEvent(EngineApplicationInterface.ISoundEvent.CreateEventFromExternalFile(programmerEventName, soundFilePath, (scene != null) ? scene.Pointer : UIntPtr.Zero, is3d, isBlocking));
		}

		// Token: 0x040001C7 RID: 455
		private const int NullSoundId = -1;

		// Token: 0x040001C8 RID: 456
		private static readonly SoundEvent NullSoundEvent = new SoundEvent(-1);

		// Token: 0x040001C9 RID: 457
		private int _soundId;
	}
}
