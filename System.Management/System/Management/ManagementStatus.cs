using System;

namespace System.Management
{
	// Token: 0x02000022 RID: 34
	public enum ManagementStatus
	{
		// Token: 0x04000033 RID: 51
		Failed = -2147217407,
		// Token: 0x04000034 RID: 52
		NotFound,
		// Token: 0x04000035 RID: 53
		AccessDenied,
		// Token: 0x04000036 RID: 54
		ProviderFailure,
		// Token: 0x04000037 RID: 55
		TypeMismatch,
		// Token: 0x04000038 RID: 56
		OutOfMemory,
		// Token: 0x04000039 RID: 57
		InvalidContext,
		// Token: 0x0400003A RID: 58
		InvalidParameter,
		// Token: 0x0400003B RID: 59
		NotAvailable,
		// Token: 0x0400003C RID: 60
		CriticalError,
		// Token: 0x0400003D RID: 61
		InvalidStream,
		// Token: 0x0400003E RID: 62
		NotSupported,
		// Token: 0x0400003F RID: 63
		InvalidSuperclass,
		// Token: 0x04000040 RID: 64
		InvalidNamespace,
		// Token: 0x04000041 RID: 65
		InvalidObject,
		// Token: 0x04000042 RID: 66
		InvalidClass,
		// Token: 0x04000043 RID: 67
		ProviderNotFound,
		// Token: 0x04000044 RID: 68
		InvalidProviderRegistration,
		// Token: 0x04000045 RID: 69
		ProviderLoadFailure,
		// Token: 0x04000046 RID: 70
		InitializationFailure,
		// Token: 0x04000047 RID: 71
		TransportFailure,
		// Token: 0x04000048 RID: 72
		InvalidOperation,
		// Token: 0x04000049 RID: 73
		InvalidQuery,
		// Token: 0x0400004A RID: 74
		InvalidQueryType,
		// Token: 0x0400004B RID: 75
		AlreadyExists,
		// Token: 0x0400004C RID: 76
		OverrideNotAllowed,
		// Token: 0x0400004D RID: 77
		PropagatedQualifier,
		// Token: 0x0400004E RID: 78
		PropagatedProperty,
		// Token: 0x0400004F RID: 79
		Unexpected,
		// Token: 0x04000050 RID: 80
		IllegalOperation,
		// Token: 0x04000051 RID: 81
		CannotBeKey,
		// Token: 0x04000052 RID: 82
		IncompleteClass,
		// Token: 0x04000053 RID: 83
		InvalidSyntax,
		// Token: 0x04000054 RID: 84
		NondecoratedObject,
		// Token: 0x04000055 RID: 85
		ReadOnly,
		// Token: 0x04000056 RID: 86
		ProviderNotCapable,
		// Token: 0x04000057 RID: 87
		ClassHasChildren,
		// Token: 0x04000058 RID: 88
		ClassHasInstances,
		// Token: 0x04000059 RID: 89
		QueryNotImplemented,
		// Token: 0x0400005A RID: 90
		IllegalNull,
		// Token: 0x0400005B RID: 91
		InvalidQualifierType,
		// Token: 0x0400005C RID: 92
		InvalidPropertyType,
		// Token: 0x0400005D RID: 93
		ValueOutOfRange,
		// Token: 0x0400005E RID: 94
		CannotBeSingleton,
		// Token: 0x0400005F RID: 95
		InvalidCimType,
		// Token: 0x04000060 RID: 96
		InvalidMethod,
		// Token: 0x04000061 RID: 97
		InvalidMethodParameters,
		// Token: 0x04000062 RID: 98
		SystemProperty,
		// Token: 0x04000063 RID: 99
		InvalidProperty,
		// Token: 0x04000064 RID: 100
		CallCanceled,
		// Token: 0x04000065 RID: 101
		ShuttingDown,
		// Token: 0x04000066 RID: 102
		PropagatedMethod,
		// Token: 0x04000067 RID: 103
		UnsupportedParameter,
		// Token: 0x04000068 RID: 104
		MissingParameterID,
		// Token: 0x04000069 RID: 105
		InvalidParameterID,
		// Token: 0x0400006A RID: 106
		NonconsecutiveParameterIDs,
		// Token: 0x0400006B RID: 107
		ParameterIDOnRetval,
		// Token: 0x0400006C RID: 108
		InvalidObjectPath,
		// Token: 0x0400006D RID: 109
		OutOfDiskSpace,
		// Token: 0x0400006E RID: 110
		BufferTooSmall,
		// Token: 0x0400006F RID: 111
		UnsupportedPutExtension,
		// Token: 0x04000070 RID: 112
		UnknownObjectType,
		// Token: 0x04000071 RID: 113
		UnknownPacketType,
		// Token: 0x04000072 RID: 114
		MarshalVersionMismatch,
		// Token: 0x04000073 RID: 115
		MarshalInvalidSignature,
		// Token: 0x04000074 RID: 116
		InvalidQualifier,
		// Token: 0x04000075 RID: 117
		InvalidDuplicateParameter,
		// Token: 0x04000076 RID: 118
		TooMuchData,
		// Token: 0x04000077 RID: 119
		ServerTooBusy,
		// Token: 0x04000078 RID: 120
		InvalidFlavor,
		// Token: 0x04000079 RID: 121
		CircularReference,
		// Token: 0x0400007A RID: 122
		UnsupportedClassUpdate,
		// Token: 0x0400007B RID: 123
		CannotChangeKeyInheritance,
		// Token: 0x0400007C RID: 124
		CannotChangeIndexInheritance = -2147217328,
		// Token: 0x0400007D RID: 125
		TooManyProperties,
		// Token: 0x0400007E RID: 126
		UpdateTypeMismatch,
		// Token: 0x0400007F RID: 127
		UpdateOverrideNotAllowed,
		// Token: 0x04000080 RID: 128
		UpdatePropagatedMethod,
		// Token: 0x04000081 RID: 129
		MethodNotImplemented,
		// Token: 0x04000082 RID: 130
		MethodDisabled,
		// Token: 0x04000083 RID: 131
		RefresherBusy,
		// Token: 0x04000084 RID: 132
		UnparsableQuery,
		// Token: 0x04000085 RID: 133
		NotEventClass,
		// Token: 0x04000086 RID: 134
		MissingGroupWithin,
		// Token: 0x04000087 RID: 135
		MissingAggregationList,
		// Token: 0x04000088 RID: 136
		PropertyNotAnObject,
		// Token: 0x04000089 RID: 137
		AggregatingByObject,
		// Token: 0x0400008A RID: 138
		UninterpretableProviderQuery = -2147217313,
		// Token: 0x0400008B RID: 139
		BackupRestoreWinmgmtRunning,
		// Token: 0x0400008C RID: 140
		QueueOverflow,
		// Token: 0x0400008D RID: 141
		PrivilegeNotHeld,
		// Token: 0x0400008E RID: 142
		InvalidOperator,
		// Token: 0x0400008F RID: 143
		LocalCredentials,
		// Token: 0x04000090 RID: 144
		CannotBeAbstract,
		// Token: 0x04000091 RID: 145
		AmendedObject,
		// Token: 0x04000092 RID: 146
		ClientTooSlow,
		// Token: 0x04000093 RID: 147
		RegistrationTooBroad = -2147213311,
		// Token: 0x04000094 RID: 148
		RegistrationTooPrecise,
		// Token: 0x04000095 RID: 149
		NoError = 0,
		// Token: 0x04000096 RID: 150
		False,
		// Token: 0x04000097 RID: 151
		ResetToDefault = 262146,
		// Token: 0x04000098 RID: 152
		Different,
		// Token: 0x04000099 RID: 153
		Timedout,
		// Token: 0x0400009A RID: 154
		NoMoreData,
		// Token: 0x0400009B RID: 155
		OperationCanceled,
		// Token: 0x0400009C RID: 156
		Pending,
		// Token: 0x0400009D RID: 157
		DuplicateObjects,
		// Token: 0x0400009E RID: 158
		PartialResults = 262160
	}
}
