using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TaleWorlds.Library
{
	// Token: 0x02000090 RID: 144
	public static class SRTHelper
	{
		// Token: 0x020000E5 RID: 229
		public static class SrtParser
		{
			// Token: 0x060007AB RID: 1963 RVA: 0x00019570 File Offset: 0x00017770
			public static List<SRTHelper.SubtitleItem> ParseStream(Stream subtitleStream, Encoding encoding)
			{
				if (!subtitleStream.CanRead || !subtitleStream.CanSeek)
				{
					throw new ArgumentException("Given subtitle file is not readable.");
				}
				subtitleStream.Position = 0L;
				TextReader textReader = new StreamReader(subtitleStream, encoding, true);
				List<SRTHelper.SubtitleItem> list = new List<SRTHelper.SubtitleItem>();
				List<string> list2 = SRTHelper.SrtParser.GetSrtSubTitleParts(textReader).ToList<string>();
				if (list2.Count <= 0)
				{
					throw new FormatException("Parsing as srt returned no srt part.");
				}
				foreach (string text in list2)
				{
					List<string> list3 = (from s in text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
						select s.Trim() into l
						where !string.IsNullOrEmpty(l)
						select l).ToList<string>();
					SRTHelper.SubtitleItem subtitleItem = new SRTHelper.SubtitleItem();
					foreach (string text2 in list3)
					{
						if (subtitleItem.StartTime == 0 && subtitleItem.EndTime == 0)
						{
							int num;
							int num2;
							if (SRTHelper.SrtParser.TryParseTimecodeLine(text2, out num, out num2))
							{
								subtitleItem.StartTime = num;
								subtitleItem.EndTime = num2;
							}
						}
						else
						{
							subtitleItem.Lines.Add(text2);
						}
					}
					if ((subtitleItem.StartTime != 0 || subtitleItem.EndTime != 0) && subtitleItem.Lines.Count > 0)
					{
						list.Add(subtitleItem);
					}
				}
				if (list.Count > 0)
				{
					return list;
				}
				throw new ArgumentException("Stream is not in a valid Srt format");
			}

			// Token: 0x060007AC RID: 1964 RVA: 0x0001973C File Offset: 0x0001793C
			private static IEnumerable<string> GetSrtSubTitleParts(TextReader reader)
			{
				MBStringBuilder sb = default(MBStringBuilder);
				sb.Initialize(16, "GetSrtSubTitleParts");
				string text;
				while ((text = reader.ReadLine()) != null)
				{
					if (string.IsNullOrEmpty(text.Trim()))
					{
						string text2 = sb.ToStringAndRelease().TrimEnd(Array.Empty<char>());
						if (!string.IsNullOrEmpty(text2))
						{
							yield return text2;
						}
						sb.Initialize(16, "GetSrtSubTitleParts");
					}
					else
					{
						sb.AppendLine<string>(text);
					}
				}
				if (sb.Length > 0)
				{
					yield return sb.ToStringAndRelease();
				}
				else
				{
					sb.Release();
				}
				yield break;
			}

			// Token: 0x060007AD RID: 1965 RVA: 0x0001974C File Offset: 0x0001794C
			private static bool TryParseTimecodeLine(string line, out int startTc, out int endTc)
			{
				string[] array = line.Split(SRTHelper.SrtParser._delimiters, StringSplitOptions.None);
				if (array.Length != 2)
				{
					startTc = -1;
					endTc = -1;
					return false;
				}
				startTc = SRTHelper.SrtParser.ParseSrtTimecode(array[0]);
				endTc = SRTHelper.SrtParser.ParseSrtTimecode(array[1]);
				return true;
			}

			// Token: 0x060007AE RID: 1966 RVA: 0x0001978C File Offset: 0x0001798C
			private static int ParseSrtTimecode(string s)
			{
				Match match = Regex.Match(s, "[0-9]+:[0-9]+:[0-9]+([,\\.][0-9]+)?");
				if (match.Success)
				{
					s = match.Value;
					TimeSpan timeSpan;
					if (TimeSpan.TryParse(s.Replace(',', '.'), out timeSpan))
					{
						return (int)timeSpan.TotalMilliseconds;
					}
				}
				return -1;
			}

			// Token: 0x040002F7 RID: 759
			private static readonly string[] _delimiters = new string[] { "-->", "- >", "->" };
		}

		// Token: 0x020000E6 RID: 230
		public static class StreamHelpers
		{
			// Token: 0x060007B0 RID: 1968 RVA: 0x000197F8 File Offset: 0x000179F8
			public static Stream CopyStream(Stream inputStream)
			{
				MemoryStream memoryStream = new MemoryStream();
				int num;
				do
				{
					byte[] array = new byte[1024];
					num = inputStream.Read(array, 0, 1024);
					memoryStream.Write(array, 0, num);
				}
				while (inputStream.CanRead && num > 0);
				memoryStream.ToArray();
				return memoryStream;
			}
		}

		// Token: 0x020000E7 RID: 231
		public class SubtitleItem
		{
			// Token: 0x17000104 RID: 260
			// (get) Token: 0x060007B1 RID: 1969 RVA: 0x00019841 File Offset: 0x00017A41
			// (set) Token: 0x060007B2 RID: 1970 RVA: 0x00019849 File Offset: 0x00017A49
			public int StartTime { get; set; }

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00019852 File Offset: 0x00017A52
			// (set) Token: 0x060007B4 RID: 1972 RVA: 0x0001985A File Offset: 0x00017A5A
			public int EndTime { get; set; }

			// Token: 0x17000106 RID: 262
			// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00019863 File Offset: 0x00017A63
			// (set) Token: 0x060007B6 RID: 1974 RVA: 0x0001986B File Offset: 0x00017A6B
			public List<string> Lines { get; set; }

			// Token: 0x060007B7 RID: 1975 RVA: 0x00019874 File Offset: 0x00017A74
			public SubtitleItem()
			{
				this.Lines = new List<string>();
			}

			// Token: 0x060007B8 RID: 1976 RVA: 0x00019888 File Offset: 0x00017A88
			public override string ToString()
			{
				TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, this.StartTime);
				TimeSpan timeSpan2 = new TimeSpan(0, 0, 0, 0, this.EndTime);
				return string.Format("{0} --> {1}: {2}", timeSpan.ToString("G"), timeSpan2.ToString("G"), string.Join(Environment.NewLine, this.Lines));
			}
		}
	}
}
