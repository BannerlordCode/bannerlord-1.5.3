using System;

namespace psai.net
{
	// Token: 0x02000011 RID: 17
	public enum PsaiResult
	{
		// Token: 0x04000084 RID: 132
		none,
		// Token: 0x04000085 RID: 133
		OK,
		// Token: 0x04000086 RID: 134
		alreadyActive,
		// Token: 0x04000087 RID: 135
		badCommand,
		// Token: 0x04000088 RID: 136
		channelAllocFailed,
		// Token: 0x04000089 RID: 137
		channelStolen,
		// Token: 0x0400008A RID: 138
		error_file,
		// Token: 0x0400008B RID: 139
		file_couldNotSeek,
		// Token: 0x0400008C RID: 140
		file_diskEjected,
		// Token: 0x0400008D RID: 141
		file_eof,
		// Token: 0x0400008E RID: 142
		file_notFound,
		// Token: 0x0400008F RID: 143
		format_error,
		// Token: 0x04000090 RID: 144
		initialization_error,
		// Token: 0x04000091 RID: 145
		internal_error,
		// Token: 0x04000092 RID: 146
		invalidHandle,
		// Token: 0x04000093 RID: 147
		invalidParam,
		// Token: 0x04000094 RID: 148
		memory_error,
		// Token: 0x04000095 RID: 149
		notReady,
		// Token: 0x04000096 RID: 150
		error_createBufferFailed,
		// Token: 0x04000097 RID: 151
		output_format_error,
		// Token: 0x04000098 RID: 152
		output_init_failed,
		// Token: 0x04000099 RID: 153
		output_failure,
		// Token: 0x0400009A RID: 154
		update_error,
		// Token: 0x0400009B RID: 155
		error_version,
		// Token: 0x0400009C RID: 156
		unknown_theme,
		// Token: 0x0400009D RID: 157
		essential_segment_missing,
		// Token: 0x0400009E RID: 158
		commandIgnored,
		// Token: 0x0400009F RID: 159
		triggerDenied,
		// Token: 0x040000A0 RID: 160
		triggerIgnoredFollowingThemeAlreadySet,
		// Token: 0x040000A1 RID: 161
		triggerIgnoredLowPriority,
		// Token: 0x040000A2 RID: 162
		commandIgnoredMenuModeActive,
		// Token: 0x040000A3 RID: 163
		commandIgnoredCutsceneActive,
		// Token: 0x040000A4 RID: 164
		no_basicmood_set
	}
}
