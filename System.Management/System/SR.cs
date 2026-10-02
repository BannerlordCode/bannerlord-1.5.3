using System;
using System.Resources;
using System.Runtime.CompilerServices;
using FxResources.System.Management;

namespace System
{
	// Token: 0x02000003 RID: 3
	internal static class SR
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool UsingResourceKeys()
		{
			return false;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002054 File Offset: 0x00000254
		internal static string GetResourceString(string resourceKey, string defaultString = null)
		{
			if (global::System.SR.UsingResourceKeys())
			{
				return defaultString ?? resourceKey;
			}
			string text = null;
			try
			{
				text = global::System.SR.ResourceManager.GetString(resourceKey);
			}
			catch (MissingManifestResourceException)
			{
			}
			if (defaultString != null && resourceKey.Equals(text))
			{
				return defaultString;
			}
			return text;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020A4 File Offset: 0x000002A4
		internal static string Format(string resourceFormat, object p1)
		{
			if (global::System.SR.UsingResourceKeys())
			{
				return string.Join(", ", new object[] { resourceFormat, p1 });
			}
			return string.Format(resourceFormat, p1);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020CD File Offset: 0x000002CD
		internal static string Format(string resourceFormat, object p1, object p2)
		{
			if (global::System.SR.UsingResourceKeys())
			{
				return string.Join(", ", new object[] { resourceFormat, p1, p2 });
			}
			return string.Format(resourceFormat, p1, p2);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020FB File Offset: 0x000002FB
		internal static string Format(string resourceFormat, object p1, object p2, object p3)
		{
			if (global::System.SR.UsingResourceKeys())
			{
				return string.Join(", ", new object[] { resourceFormat, p1, p2, p3 });
			}
			return string.Format(resourceFormat, p1, p2, p3);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000212E File Offset: 0x0000032E
		internal static string Format(string resourceFormat, params object[] args)
		{
			if (args == null)
			{
				return resourceFormat;
			}
			if (global::System.SR.UsingResourceKeys())
			{
				return resourceFormat + ", " + string.Join(", ", args);
			}
			return string.Format(resourceFormat, args);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000215A File Offset: 0x0000035A
		internal static string Format(IFormatProvider provider, string resourceFormat, object p1)
		{
			if (global::System.SR.UsingResourceKeys())
			{
				return string.Join(", ", new object[] { resourceFormat, p1 });
			}
			return string.Format(provider, resourceFormat, p1);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002184 File Offset: 0x00000384
		internal static string Format(IFormatProvider provider, string resourceFormat, object p1, object p2)
		{
			if (global::System.SR.UsingResourceKeys())
			{
				return string.Join(", ", new object[] { resourceFormat, p1, p2 });
			}
			return string.Format(provider, resourceFormat, p1, p2);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021B3 File Offset: 0x000003B3
		internal static string Format(IFormatProvider provider, string resourceFormat, object p1, object p2, object p3)
		{
			if (global::System.SR.UsingResourceKeys())
			{
				return string.Join(", ", new object[] { resourceFormat, p1, p2, p3 });
			}
			return string.Format(provider, resourceFormat, p1, p2, p3);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021E9 File Offset: 0x000003E9
		internal static string Format(IFormatProvider provider, string resourceFormat, params object[] args)
		{
			if (args == null)
			{
				return resourceFormat;
			}
			if (global::System.SR.UsingResourceKeys())
			{
				return resourceFormat + ", " + string.Join(", ", args);
			}
			return string.Format(provider, resourceFormat, args);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002216 File Offset: 0x00000416
		internal static ResourceManager ResourceManager
		{
			get
			{
				ResourceManager resourceManager;
				if ((resourceManager = global::System.SR.s_resourceManager) == null)
				{
					resourceManager = (global::System.SR.s_resourceManager = new ResourceManager(typeof(FxResources.System.Management.SR)));
				}
				return resourceManager;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002236 File Offset: 0x00000436
		internal static string InvalidQuery
		{
			get
			{
				return global::System.SR.GetResourceString("InvalidQuery", null);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002243 File Offset: 0x00000443
		internal static string InvalidQueryDuplicatedToken
		{
			get
			{
				return global::System.SR.GetResourceString("InvalidQueryDuplicatedToken", null);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002250 File Offset: 0x00000450
		internal static string InvalidQueryNullToken
		{
			get
			{
				return global::System.SR.GetResourceString("InvalidQueryNullToken", null);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000225D File Offset: 0x0000045D
		internal static string WorkerThreadWakeupFailed
		{
			get
			{
				return global::System.SR.GetResourceString("WorkerThreadWakeupFailed", null);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000226A File Offset: 0x0000046A
		internal static string ClassNameNotInitializedException
		{
			get
			{
				return global::System.SR.GetResourceString("ClassNameNotInitializedException", null);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002277 File Offset: 0x00000477
		internal static string ClassNameNotFoundException
		{
			get
			{
				return global::System.SR.GetResourceString("ClassNameNotFoundException", null);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002284 File Offset: 0x00000484
		internal static string CommentAttributeProperty
		{
			get
			{
				return global::System.SR.GetResourceString("CommentAttributeProperty", null);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002291 File Offset: 0x00000491
		internal static string CommentAutoCommitProperty
		{
			get
			{
				return global::System.SR.GetResourceString("CommentAutoCommitProperty", null);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000014 RID: 20 RVA: 0x0000229E File Offset: 0x0000049E
		internal static string CommentClassBegin
		{
			get
			{
				return global::System.SR.GetResourceString("CommentClassBegin", null);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000022AB File Offset: 0x000004AB
		internal static string CommentConstructors
		{
			get
			{
				return global::System.SR.GetResourceString("CommentConstructors", null);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000022B8 File Offset: 0x000004B8
		internal static string CommentCreatedClass
		{
			get
			{
				return global::System.SR.GetResourceString("CommentCreatedClass", null);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000022C5 File Offset: 0x000004C5
		internal static string CommentCreatedWmiNamespace
		{
			get
			{
				return global::System.SR.GetResourceString("CommentCreatedWmiNamespace", null);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000022D2 File Offset: 0x000004D2
		internal static string CommentCurrentObject
		{
			get
			{
				return global::System.SR.GetResourceString("CommentCurrentObject", null);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000022DF File Offset: 0x000004DF
		internal static string CommentDateConversionFunction
		{
			get
			{
				return global::System.SR.GetResourceString("CommentDateConversionFunction", null);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000022EC File Offset: 0x000004EC
		internal static string CommentEmbeddedObject
		{
			get
			{
				return global::System.SR.GetResourceString("CommentEmbeddedObject", null);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000022F9 File Offset: 0x000004F9
		internal static string CommentEnumeratorImplementation
		{
			get
			{
				return global::System.SR.GetResourceString("CommentEnumeratorImplementation", null);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002306 File Offset: 0x00000506
		internal static string CommentFlagForEmbedded
		{
			get
			{
				return global::System.SR.GetResourceString("CommentFlagForEmbedded", null);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002313 File Offset: 0x00000513
		internal static string CommentGetInstances
		{
			get
			{
				return global::System.SR.GetResourceString("CommentGetInstances", null);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002320 File Offset: 0x00000520
		internal static string CommentIsPropNull
		{
			get
			{
				return global::System.SR.GetResourceString("CommentIsPropNull", null);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000232D File Offset: 0x0000052D
		internal static string CommentLateBoundObject
		{
			get
			{
				return global::System.SR.GetResourceString("CommentLateBoundObject", null);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000233A File Offset: 0x0000053A
		internal static string CommentLateBoundProperty
		{
			get
			{
				return global::System.SR.GetResourceString("CommentLateBoundProperty", null);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002347 File Offset: 0x00000547
		internal static string CommentManagementPath
		{
			get
			{
				return global::System.SR.GetResourceString("CommentManagementPath", null);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002354 File Offset: 0x00000554
		internal static string CommentManagementScope
		{
			get
			{
				return global::System.SR.GetResourceString("CommentManagementScope", null);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002361 File Offset: 0x00000561
		internal static string CommentOriginNamespace
		{
			get
			{
				return global::System.SR.GetResourceString("CommentOriginNamespace", null);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000236E File Offset: 0x0000056E
		internal static string CommentPrivateAutoCommit
		{
			get
			{
				return global::System.SR.GetResourceString("CommentPrivateAutoCommit", null);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000237B File Offset: 0x0000057B
		internal static string CommentPrototypeConverter
		{
			get
			{
				return global::System.SR.GetResourceString("CommentPrototypeConverter", null);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002388 File Offset: 0x00000588
		internal static string CommentResetProperty
		{
			get
			{
				return global::System.SR.GetResourceString("CommentResetProperty", null);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002395 File Offset: 0x00000595
		internal static string CommentShouldSerialize
		{
			get
			{
				return global::System.SR.GetResourceString("CommentShouldSerialize", null);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000023A2 File Offset: 0x000005A2
		internal static string CommentStaticManagementScope
		{
			get
			{
				return global::System.SR.GetResourceString("CommentStaticManagementScope", null);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000023AF File Offset: 0x000005AF
		internal static string CommentStaticScopeProperty
		{
			get
			{
				return global::System.SR.GetResourceString("CommentStaticScopeProperty", null);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000023BC File Offset: 0x000005BC
		internal static string CommentSystemObject
		{
			get
			{
				return global::System.SR.GetResourceString("CommentSystemObject", null);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000023C9 File Offset: 0x000005C9
		internal static string CommentSystemPropertiesClass
		{
			get
			{
				return global::System.SR.GetResourceString("CommentSystemPropertiesClass", null);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000023D6 File Offset: 0x000005D6
		internal static string CommentTimeSpanConvertionFunction
		{
			get
			{
				return global::System.SR.GetResourceString("CommentTimeSpanConvertionFunction", null);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000023E3 File Offset: 0x000005E3
		internal static string CommentToDateTime
		{
			get
			{
				return global::System.SR.GetResourceString("CommentToDateTime", null);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000023F0 File Offset: 0x000005F0
		internal static string CommentToDmtfDateTime
		{
			get
			{
				return global::System.SR.GetResourceString("CommentToDmtfDateTime", null);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000023FD File Offset: 0x000005FD
		internal static string CommentToDmtfTimeInterval
		{
			get
			{
				return global::System.SR.GetResourceString("CommentToDmtfTimeInterval", null);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000030 RID: 48 RVA: 0x0000240A File Offset: 0x0000060A
		internal static string CommentToTimeSpan
		{
			get
			{
				return global::System.SR.GetResourceString("CommentToTimeSpan", null);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002417 File Offset: 0x00000617
		internal static string EmbeddedComment
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedComment", null);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002424 File Offset: 0x00000624
		internal static string EmbeddedComment2
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedComment2", null);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002431 File Offset: 0x00000631
		internal static string EmbeddedComment3
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedComment3", null);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000034 RID: 52 RVA: 0x0000243E File Offset: 0x0000063E
		internal static string EmbeddedComment4
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedComment4", null);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000035 RID: 53 RVA: 0x0000244B File Offset: 0x0000064B
		internal static string EmbeddedComment5
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedComment5", null);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002458 File Offset: 0x00000658
		internal static string EmbeddedComment6
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedComment6", null);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002465 File Offset: 0x00000665
		internal static string EmbeddedComment7
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedComment7", null);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002472 File Offset: 0x00000672
		internal static string EmbeddedComment8
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedComment8", null);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000039 RID: 57 RVA: 0x0000247F File Offset: 0x0000067F
		internal static string EmbeddedCSharpComment1
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment1", null);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600003A RID: 58 RVA: 0x0000248C File Offset: 0x0000068C
		internal static string EmbeddedCSharpComment10
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment10", null);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002499 File Offset: 0x00000699
		internal static string EmbeddedCSharpComment11
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment11", null);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600003C RID: 60 RVA: 0x000024A6 File Offset: 0x000006A6
		internal static string EmbeddedCSharpComment12
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment12", null);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600003D RID: 61 RVA: 0x000024B3 File Offset: 0x000006B3
		internal static string EmbeddedCSharpComment13
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment13", null);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600003E RID: 62 RVA: 0x000024C0 File Offset: 0x000006C0
		internal static string EmbeddedCSharpComment14
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment14", null);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000024CD File Offset: 0x000006CD
		internal static string EmbeddedCSharpComment15
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment15", null);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000024DA File Offset: 0x000006DA
		internal static string EmbeddedCSharpComment2
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment2", null);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000024E7 File Offset: 0x000006E7
		internal static string EmbeddedCSharpComment3
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment3", null);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000024F4 File Offset: 0x000006F4
		internal static string EmbeddedCSharpComment4
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment4", null);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002501 File Offset: 0x00000701
		internal static string EmbeddedCSharpComment5
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment5", null);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000044 RID: 68 RVA: 0x0000250E File Offset: 0x0000070E
		internal static string EmbeddedCSharpComment6
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment6", null);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000045 RID: 69 RVA: 0x0000251B File Offset: 0x0000071B
		internal static string EmbeddedCSharpComment7
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment7", null);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002528 File Offset: 0x00000728
		internal static string EmbeddedCSharpComment8
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment8", null);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002535 File Offset: 0x00000735
		internal static string EmbeddedCSharpComment9
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedCSharpComment9", null);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002542 File Offset: 0x00000742
		internal static string EmbeddedVisualBasicComment1
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedVisualBasicComment1", null);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000049 RID: 73 RVA: 0x0000254F File Offset: 0x0000074F
		internal static string EmbeddedVisualBasicComment10
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedVisualBasicComment10", null);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600004A RID: 74 RVA: 0x0000255C File Offset: 0x0000075C
		internal static string EmbeddedVisualBasicComment2
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedVisualBasicComment2", null);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002569 File Offset: 0x00000769
		internal static string EmbeddedVisualBasicComment3
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedVisualBasicComment3", null);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002576 File Offset: 0x00000776
		internal static string EmbeddedVisualBasicComment4
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedVisualBasicComment4", null);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002583 File Offset: 0x00000783
		internal static string EmbeddedVisualBasicComment5
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedVisualBasicComment5", null);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002590 File Offset: 0x00000790
		internal static string EmbeddedVisualBasicComment6
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedVisualBasicComment6", null);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600004F RID: 79 RVA: 0x0000259D File Offset: 0x0000079D
		internal static string EmbeddedVisualBasicComment7
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedVisualBasicComment7", null);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000025AA File Offset: 0x000007AA
		internal static string EmbeddedVisualBasicComment8
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedVisualBasicComment8", null);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000025B7 File Offset: 0x000007B7
		internal static string EmbeddedVisualBasicComment9
		{
			get
			{
				return global::System.SR.GetResourceString("EmbeddedVisualBasicComment9", null);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000025C4 File Offset: 0x000007C4
		internal static string EmptyFilePathException
		{
			get
			{
				return global::System.SR.GetResourceString("EmptyFilePathException", null);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000053 RID: 83 RVA: 0x000025D1 File Offset: 0x000007D1
		internal static string NamespaceNotInitializedException
		{
			get
			{
				return global::System.SR.GetResourceString("NamespaceNotInitializedException", null);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000025DE File Offset: 0x000007DE
		internal static string NullFilePathException
		{
			get
			{
				return global::System.SR.GetResourceString("NullFilePathException", null);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000055 RID: 85 RVA: 0x000025EB File Offset: 0x000007EB
		internal static string UnableToCreateCodeGeneratorException
		{
			get
			{
				return global::System.SR.GetResourceString("UnableToCreateCodeGeneratorException", null);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000025F8 File Offset: 0x000007F8
		internal static string PlatformNotSupported_SystemManagement
		{
			get
			{
				return global::System.SR.GetResourceString("PlatformNotSupported_SystemManagement", null);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002605 File Offset: 0x00000805
		internal static string PlatformNotSupported_FullFrameworkRequired
		{
			get
			{
				return global::System.SR.GetResourceString("PlatformNotSupported_FullFrameworkRequired", null);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002612 File Offset: 0x00000812
		internal static string LoadLibraryFailed
		{
			get
			{
				return global::System.SR.GetResourceString("LoadLibraryFailed", null);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000059 RID: 89 RVA: 0x0000261F File Offset: 0x0000081F
		internal static string PlatformNotSupported_FrameworkUpdatedRequired
		{
			get
			{
				return global::System.SR.GetResourceString("PlatformNotSupported_FrameworkUpdatedRequired", null);
			}
		}

		// Token: 0x04000001 RID: 1
		private static ResourceManager s_resourceManager;
	}
}
