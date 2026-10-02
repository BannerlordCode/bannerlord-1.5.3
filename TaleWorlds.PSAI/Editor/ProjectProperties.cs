using System;

namespace psai.Editor
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public class ProjectProperties : ICloneable
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00004E96 File Offset: 0x00003096
		// (set) Token: 0x0600008A RID: 138 RVA: 0x00004E9E File Offset: 0x0000309E
		public int WarningThresholdPreBeatMillis { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00004EA7 File Offset: 0x000030A7
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00004EAF File Offset: 0x000030AF
		public bool DefaultCalculatePostAndPrebeatLengthBasedOnBeats { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00004EB8 File Offset: 0x000030B8
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00004EC0 File Offset: 0x000030C0
		public int DefaultSegmentSuitabilites { get; set; }

		// Token: 0x0600008F RID: 143 RVA: 0x00004ECC File Offset: 0x000030CC
		public ProjectProperties()
		{
			this.DefaultBpm = 100f;
			this.DefaultPostbeats = 4f;
			this.DefaultPrebeats = 1f;
			this.WarningThresholdPreBeatMillis = 1500;
			this.DefaultSegmentSuitabilites = 3;
			this.ForceFullRebuild = true;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00004F21 File Offset: 0x00003121
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00004F29 File Offset: 0x00003129
		public bool ForceFullRebuild { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004F32 File Offset: 0x00003132
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00004F3A File Offset: 0x0000313A
		public string ModuleIdPrefix { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00004F43 File Offset: 0x00003143
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00004F4B File Offset: 0x0000314B
		public float VolumeBoost
		{
			get
			{
				return this._volumeBoost;
			}
			set
			{
				if (value >= 0f && value <= 600f)
				{
					this._volumeBoost = value;
					return;
				}
				Console.Out.WriteLine("invalid value for VolumeBoost");
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00004F74 File Offset: 0x00003174
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00004F7C File Offset: 0x0000317C
		public int ExportSoundQualityInPercent
		{
			get
			{
				return this._exportSoundQualityInPercent;
			}
			set
			{
				if (value >= 1 && value <= 100)
				{
					this._exportSoundQualityInPercent = value;
				}
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004F8E File Offset: 0x0000318E
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00004F96 File Offset: 0x00003196
		public float DefaultPrebeats { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004F9F File Offset: 0x0000319F
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00004FA7 File Offset: 0x000031A7
		public float DefaultPostbeats { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00004FB0 File Offset: 0x000031B0
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00004FB8 File Offset: 0x000031B8
		public float DefaultBpm { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004FC1 File Offset: 0x000031C1
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00004FC9 File Offset: 0x000031C9
		public int DefaultPrebeatLengthInSamples { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004FD2 File Offset: 0x000031D2
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00004FDA File Offset: 0x000031DA
		public int DefaultPostbeatLengthInSamples { get; set; }

		// Token: 0x060000A2 RID: 162 RVA: 0x00004FE3 File Offset: 0x000031E3
		public ProjectProperties ShallowCopy()
		{
			return (ProjectProperties)base.MemberwiseClone();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004FF0 File Offset: 0x000031F0
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x0400003A RID: 58
		private float _volumeBoost;

		// Token: 0x0400003B RID: 59
		private int _exportSoundQualityInPercent = 100;
	}
}
