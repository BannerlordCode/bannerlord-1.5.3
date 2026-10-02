using System;
using System.Collections.Generic;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200002F RID: 47
	public class SoundProperties
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000EF37 File Offset: 0x0000D137
		public IEnumerable<KeyValuePair<string, AudioProperty>> RegisteredStateSounds
		{
			get
			{
				foreach (KeyValuePair<string, AudioProperty> keyValuePair in this._stateSounds)
				{
					yield return keyValuePair;
				}
				Dictionary<string, AudioProperty>.Enumerator enumerator = default(Dictionary<string, AudioProperty>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000EF47 File Offset: 0x0000D147
		public IEnumerable<KeyValuePair<string, AudioProperty>> RegisteredEventSounds
		{
			get
			{
				foreach (KeyValuePair<string, AudioProperty> keyValuePair in this._eventSounds)
				{
					yield return keyValuePair;
				}
				Dictionary<string, AudioProperty>.Enumerator enumerator = default(Dictionary<string, AudioProperty>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000EF57 File Offset: 0x0000D157
		public SoundProperties()
		{
			this._stateSounds = new Dictionary<string, AudioProperty>();
			this._eventSounds = new Dictionary<string, AudioProperty>();
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000EF75 File Offset: 0x0000D175
		public void AddStateSound(string state, AudioProperty audioProperty)
		{
			this._stateSounds.Add(state, audioProperty);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000EF84 File Offset: 0x0000D184
		public void AddEventSound(string state, AudioProperty audioProperty)
		{
			if (this._eventSounds.ContainsKey(state))
			{
				this._eventSounds[state] = audioProperty;
				return;
			}
			this._eventSounds.Add(state, audioProperty);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000EFB0 File Offset: 0x0000D1B0
		public void FillFrom(SoundProperties soundProperties)
		{
			this._stateSounds = new Dictionary<string, AudioProperty>();
			this._eventSounds = new Dictionary<string, AudioProperty>();
			foreach (KeyValuePair<string, AudioProperty> keyValuePair in soundProperties._stateSounds)
			{
				string key = keyValuePair.Key;
				AudioProperty value = keyValuePair.Value;
				AudioProperty audioProperty = new AudioProperty();
				audioProperty.FillFrom(value);
				this._stateSounds.Add(key, audioProperty);
			}
			foreach (KeyValuePair<string, AudioProperty> keyValuePair2 in soundProperties._eventSounds)
			{
				string key2 = keyValuePair2.Key;
				AudioProperty value2 = keyValuePair2.Value;
				AudioProperty audioProperty2 = new AudioProperty();
				audioProperty2.FillFrom(value2);
				this._eventSounds.Add(key2, audioProperty2);
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000F0AC File Offset: 0x0000D2AC
		public AudioProperty GetEventAudioProperty(string eventName)
		{
			if (this._eventSounds.ContainsKey(eventName))
			{
				return this._eventSounds[eventName];
			}
			return null;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000F0CA File Offset: 0x0000D2CA
		public AudioProperty GetStateAudioProperty(string stateName)
		{
			if (this._stateSounds.ContainsKey(stateName))
			{
				return this._stateSounds[stateName];
			}
			return null;
		}

		// Token: 0x040001A6 RID: 422
		private Dictionary<string, AudioProperty> _stateSounds;

		// Token: 0x040001A7 RID: 423
		private Dictionary<string, AudioProperty> _eventSounds;
	}
}
