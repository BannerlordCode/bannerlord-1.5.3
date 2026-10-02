using System;
using System.Security;
using System.Text;

namespace System.Globalization
{
	// Token: 0x020003D6 RID: 982
	internal static class TimeSpanFormat
	{
		// Token: 0x060031EF RID: 12783 RVA: 0x000BEF83 File Offset: 0x000BD183
		[SecuritySafeCritical]
		private static string IntToString(int n, int digits)
		{
			return ParseNumbers.IntToString(n, 10, digits, '0', 0);
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x000BEF94 File Offset: 0x000BD194
		internal static string Format(TimeSpan value, string format, IFormatProvider formatProvider)
		{
			if (format == null || format.Length == 0)
			{
				format = "c";
			}
			if (format.Length != 1)
			{
				return TimeSpanFormat.FormatCustomized(value, format, DateTimeFormatInfo.GetInstance(formatProvider));
			}
			char c = format[0];
			if (c == 'c' || c == 't' || c == 'T')
			{
				return TimeSpanFormat.FormatStandard(value, true, format, TimeSpanFormat.Pattern.Minimum);
			}
			if (c == 'g' || c == 'G')
			{
				DateTimeFormatInfo instance = DateTimeFormatInfo.GetInstance(formatProvider);
				if (value._ticks < 0L)
				{
					format = instance.FullTimeSpanNegativePattern;
				}
				else
				{
					format = instance.FullTimeSpanPositivePattern;
				}
				TimeSpanFormat.Pattern pattern;
				if (c == 'g')
				{
					pattern = TimeSpanFormat.Pattern.Minimum;
				}
				else
				{
					pattern = TimeSpanFormat.Pattern.Full;
				}
				return TimeSpanFormat.FormatStandard(value, false, format, pattern);
			}
			throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x000BF03C File Offset: 0x000BD23C
		private static string FormatStandard(TimeSpan value, bool isInvariant, string format, TimeSpanFormat.Pattern pattern)
		{
			StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
			int num = (int)(value._ticks / 864000000000L);
			long num2 = value._ticks % 864000000000L;
			if (value._ticks < 0L)
			{
				num = -num;
				num2 = -num2;
			}
			int num3 = (int)(num2 / 36000000000L % 24L);
			int num4 = (int)(num2 / 600000000L % 60L);
			int num5 = (int)(num2 / 10000000L % 60L);
			int num6 = (int)(num2 % 10000000L);
			TimeSpanFormat.FormatLiterals formatLiterals;
			if (isInvariant)
			{
				if (value._ticks < 0L)
				{
					formatLiterals = TimeSpanFormat.NegativeInvariantFormatLiterals;
				}
				else
				{
					formatLiterals = TimeSpanFormat.PositiveInvariantFormatLiterals;
				}
			}
			else
			{
				formatLiterals = default(TimeSpanFormat.FormatLiterals);
				formatLiterals.Init(format, pattern == TimeSpanFormat.Pattern.Full);
			}
			if (num6 != 0)
			{
				num6 = (int)((long)num6 / (long)Math.Pow(10.0, (double)(7 - formatLiterals.ff)));
			}
			stringBuilder.Append(formatLiterals.Start);
			if (pattern == TimeSpanFormat.Pattern.Full || num != 0)
			{
				stringBuilder.Append(num);
				stringBuilder.Append(formatLiterals.DayHourSep);
			}
			stringBuilder.Append(TimeSpanFormat.IntToString(num3, formatLiterals.hh));
			stringBuilder.Append(formatLiterals.HourMinuteSep);
			stringBuilder.Append(TimeSpanFormat.IntToString(num4, formatLiterals.mm));
			stringBuilder.Append(formatLiterals.MinuteSecondSep);
			stringBuilder.Append(TimeSpanFormat.IntToString(num5, formatLiterals.ss));
			if (!isInvariant && pattern == TimeSpanFormat.Pattern.Minimum)
			{
				int num7 = formatLiterals.ff;
				while (num7 > 0 && num6 % 10 == 0)
				{
					num6 /= 10;
					num7--;
				}
				if (num7 > 0)
				{
					stringBuilder.Append(formatLiterals.SecondFractionSep);
					stringBuilder.Append(num6.ToString(DateTimeFormat.fixedNumberFormats[num7 - 1], CultureInfo.InvariantCulture));
				}
			}
			else if (pattern == TimeSpanFormat.Pattern.Full || num6 != 0)
			{
				stringBuilder.Append(formatLiterals.SecondFractionSep);
				stringBuilder.Append(TimeSpanFormat.IntToString(num6, formatLiterals.ff));
			}
			stringBuilder.Append(formatLiterals.End);
			return StringBuilderCache.GetStringAndRelease(stringBuilder);
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x000BF238 File Offset: 0x000BD438
		internal static string FormatCustomized(TimeSpan value, string format, DateTimeFormatInfo dtfi)
		{
			int num = (int)(value._ticks / 864000000000L);
			long num2 = value._ticks % 864000000000L;
			if (value._ticks < 0L)
			{
				num = -num;
				num2 = -num2;
			}
			int num3 = (int)(num2 / 36000000000L % 24L);
			int num4 = (int)(num2 / 600000000L % 60L);
			int num5 = (int)(num2 / 10000000L % 60L);
			int num6 = (int)(num2 % 10000000L);
			int i = 0;
			StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
			while (i < format.Length)
			{
				char c = format[i];
				int num8;
				if (c <= 'F')
				{
					if (c <= '%')
					{
						if (c != '"')
						{
							if (c != '%')
							{
								goto IL_034D;
							}
							int num7 = DateTimeFormat.ParseNextChar(format, i);
							if (num7 >= 0 && num7 != 37)
							{
								stringBuilder.Append(TimeSpanFormat.FormatCustomized(value, ((char)num7).ToString(), dtfi));
								num8 = 2;
								goto IL_035D;
							}
							throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
						}
					}
					else if (c != '\'')
					{
						if (c != 'F')
						{
							goto IL_034D;
						}
						num8 = DateTimeFormat.ParseRepeatPattern(format, i, c);
						if (num8 > 7)
						{
							throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
						}
						long num9 = (long)num6;
						num9 /= (long)Math.Pow(10.0, (double)(7 - num8));
						int num10 = num8;
						while (num10 > 0 && num9 % 10L == 0L)
						{
							num9 /= 10L;
							num10--;
						}
						if (num10 > 0)
						{
							stringBuilder.Append(num9.ToString(DateTimeFormat.fixedNumberFormats[num10 - 1], CultureInfo.InvariantCulture));
							goto IL_035D;
						}
						goto IL_035D;
					}
					StringBuilder stringBuilder2 = new StringBuilder();
					num8 = DateTimeFormat.ParseQuoteString(format, i, stringBuilder2);
					stringBuilder.Append(stringBuilder2);
				}
				else if (c <= 'h')
				{
					if (c != '\\')
					{
						switch (c)
						{
						case 'd':
							num8 = DateTimeFormat.ParseRepeatPattern(format, i, c);
							if (num8 > 8)
							{
								throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
							}
							DateTimeFormat.FormatDigits(stringBuilder, num, num8, true);
							break;
						case 'e':
						case 'g':
							goto IL_034D;
						case 'f':
						{
							num8 = DateTimeFormat.ParseRepeatPattern(format, i, c);
							if (num8 > 7)
							{
								throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
							}
							long num9 = (long)num6;
							stringBuilder.Append((num9 / (long)Math.Pow(10.0, (double)(7 - num8))).ToString(DateTimeFormat.fixedNumberFormats[num8 - 1], CultureInfo.InvariantCulture));
							break;
						}
						case 'h':
							num8 = DateTimeFormat.ParseRepeatPattern(format, i, c);
							if (num8 > 2)
							{
								throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
							}
							DateTimeFormat.FormatDigits(stringBuilder, num3, num8);
							break;
						default:
							goto IL_034D;
						}
					}
					else
					{
						int num7 = DateTimeFormat.ParseNextChar(format, i);
						if (num7 < 0)
						{
							throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
						}
						stringBuilder.Append((char)num7);
						num8 = 2;
					}
				}
				else if (c != 'm')
				{
					if (c != 's')
					{
						goto IL_034D;
					}
					num8 = DateTimeFormat.ParseRepeatPattern(format, i, c);
					if (num8 > 2)
					{
						throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
					}
					DateTimeFormat.FormatDigits(stringBuilder, num5, num8);
				}
				else
				{
					num8 = DateTimeFormat.ParseRepeatPattern(format, i, c);
					if (num8 > 2)
					{
						throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
					}
					DateTimeFormat.FormatDigits(stringBuilder, num4, num8);
				}
				IL_035D:
				i += num8;
				continue;
				IL_034D:
				throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
			}
			return StringBuilderCache.GetStringAndRelease(stringBuilder);
		}

		// Token: 0x04001541 RID: 5441
		internal static readonly TimeSpanFormat.FormatLiterals PositiveInvariantFormatLiterals = TimeSpanFormat.FormatLiterals.InitInvariant(false);

		// Token: 0x04001542 RID: 5442
		internal static readonly TimeSpanFormat.FormatLiterals NegativeInvariantFormatLiterals = TimeSpanFormat.FormatLiterals.InitInvariant(true);

		// Token: 0x02000B73 RID: 2931
		internal enum Pattern
		{
			// Token: 0x0400347A RID: 13434
			None,
			// Token: 0x0400347B RID: 13435
			Minimum,
			// Token: 0x0400347C RID: 13436
			Full
		}

		// Token: 0x02000B74 RID: 2932
		internal struct FormatLiterals
		{
			// Token: 0x1700124E RID: 4686
			// (get) Token: 0x06006C2E RID: 27694 RVA: 0x00176495 File Offset: 0x00174695
			internal string Start
			{
				get
				{
					return this.literals[0];
				}
			}

			// Token: 0x1700124F RID: 4687
			// (get) Token: 0x06006C2F RID: 27695 RVA: 0x0017649F File Offset: 0x0017469F
			internal string DayHourSep
			{
				get
				{
					return this.literals[1];
				}
			}

			// Token: 0x17001250 RID: 4688
			// (get) Token: 0x06006C30 RID: 27696 RVA: 0x001764A9 File Offset: 0x001746A9
			internal string HourMinuteSep
			{
				get
				{
					return this.literals[2];
				}
			}

			// Token: 0x17001251 RID: 4689
			// (get) Token: 0x06006C31 RID: 27697 RVA: 0x001764B3 File Offset: 0x001746B3
			internal string MinuteSecondSep
			{
				get
				{
					return this.literals[3];
				}
			}

			// Token: 0x17001252 RID: 4690
			// (get) Token: 0x06006C32 RID: 27698 RVA: 0x001764BD File Offset: 0x001746BD
			internal string SecondFractionSep
			{
				get
				{
					return this.literals[4];
				}
			}

			// Token: 0x17001253 RID: 4691
			// (get) Token: 0x06006C33 RID: 27699 RVA: 0x001764C7 File Offset: 0x001746C7
			internal string End
			{
				get
				{
					return this.literals[5];
				}
			}

			// Token: 0x06006C34 RID: 27700 RVA: 0x001764D4 File Offset: 0x001746D4
			internal static TimeSpanFormat.FormatLiterals InitInvariant(bool isNegative)
			{
				TimeSpanFormat.FormatLiterals formatLiterals = new TimeSpanFormat.FormatLiterals
				{
					literals = new string[6]
				};
				formatLiterals.literals[0] = (isNegative ? "-" : string.Empty);
				formatLiterals.literals[1] = ".";
				formatLiterals.literals[2] = ":";
				formatLiterals.literals[3] = ":";
				formatLiterals.literals[4] = ".";
				formatLiterals.literals[5] = string.Empty;
				formatLiterals.AppCompatLiteral = ":.";
				formatLiterals.dd = 2;
				formatLiterals.hh = 2;
				formatLiterals.mm = 2;
				formatLiterals.ss = 2;
				formatLiterals.ff = 7;
				return formatLiterals;
			}

			// Token: 0x06006C35 RID: 27701 RVA: 0x00176584 File Offset: 0x00174784
			internal void Init(string format, bool useInvariantFieldLengths)
			{
				this.literals = new string[6];
				for (int i = 0; i < this.literals.Length; i++)
				{
					this.literals[i] = string.Empty;
				}
				this.dd = 0;
				this.hh = 0;
				this.mm = 0;
				this.ss = 0;
				this.ff = 0;
				StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
				bool flag = false;
				char c = '\'';
				int num = 0;
				int j = 0;
				while (j < format.Length)
				{
					char c2 = format[j];
					if (c2 <= 'F')
					{
						if (c2 <= '%')
						{
							if (c2 != '"')
							{
								if (c2 != '%')
								{
									goto IL_01AF;
								}
								goto IL_01AF;
							}
						}
						else if (c2 != '\'')
						{
							if (c2 != 'F')
							{
								goto IL_01AF;
							}
							goto IL_019A;
						}
						if (flag && c == format[j])
						{
							if (num < 0 || num > 5)
							{
								return;
							}
							this.literals[num] = stringBuilder.ToString();
							stringBuilder.Length = 0;
							flag = false;
						}
						else if (!flag)
						{
							c = format[j];
							flag = true;
						}
					}
					else if (c2 <= 'h')
					{
						if (c2 != '\\')
						{
							switch (c2)
							{
							case 'd':
								if (!flag)
								{
									num = 1;
									this.dd++;
								}
								break;
							case 'e':
							case 'g':
								goto IL_01AF;
							case 'f':
								goto IL_019A;
							case 'h':
								if (!flag)
								{
									num = 2;
									this.hh++;
								}
								break;
							default:
								goto IL_01AF;
							}
						}
						else
						{
							if (flag)
							{
								goto IL_01AF;
							}
							j++;
						}
					}
					else if (c2 != 'm')
					{
						if (c2 != 's')
						{
							goto IL_01AF;
						}
						if (!flag)
						{
							num = 4;
							this.ss++;
						}
					}
					else if (!flag)
					{
						num = 3;
						this.mm++;
					}
					IL_01BE:
					j++;
					continue;
					IL_019A:
					if (!flag)
					{
						num = 5;
						this.ff++;
						goto IL_01BE;
					}
					goto IL_01BE;
					IL_01AF:
					stringBuilder.Append(format[j]);
					goto IL_01BE;
				}
				this.AppCompatLiteral = this.MinuteSecondSep + this.SecondFractionSep;
				if (useInvariantFieldLengths)
				{
					this.dd = 2;
					this.hh = 2;
					this.mm = 2;
					this.ss = 2;
					this.ff = 7;
				}
				else
				{
					if (this.dd < 1 || this.dd > 2)
					{
						this.dd = 2;
					}
					if (this.hh < 1 || this.hh > 2)
					{
						this.hh = 2;
					}
					if (this.mm < 1 || this.mm > 2)
					{
						this.mm = 2;
					}
					if (this.ss < 1 || this.ss > 2)
					{
						this.ss = 2;
					}
					if (this.ff < 1 || this.ff > 7)
					{
						this.ff = 7;
					}
				}
				StringBuilderCache.Release(stringBuilder);
			}

			// Token: 0x0400347D RID: 13437
			internal string AppCompatLiteral;

			// Token: 0x0400347E RID: 13438
			internal int dd;

			// Token: 0x0400347F RID: 13439
			internal int hh;

			// Token: 0x04003480 RID: 13440
			internal int mm;

			// Token: 0x04003481 RID: 13441
			internal int ss;

			// Token: 0x04003482 RID: 13442
			internal int ff;

			// Token: 0x04003483 RID: 13443
			private string[] literals;
		}
	}
}
