using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network
{
	// Token: 0x020003C6 RID: 966
	public static class DebugNetworkEventStatistics
	{
		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x06003644 RID: 13892 RVA: 0x000E0078 File Offset: 0x000DE278
		// (remove) Token: 0x06003645 RID: 13893 RVA: 0x000E00AC File Offset: 0x000DE2AC
		public static event Action<IEnumerable<DebugNetworkEventStatistics.TotalEventData>> OnEventDataUpdated;

		// Token: 0x140000AA RID: 170
		// (add) Token: 0x06003646 RID: 13894 RVA: 0x000E00E0 File Offset: 0x000DE2E0
		// (remove) Token: 0x06003647 RID: 13895 RVA: 0x000E0114 File Offset: 0x000DE314
		public static event Action<DebugNetworkEventStatistics.PerSecondEventData> OnPerSecondEventDataUpdated;

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x06003648 RID: 13896 RVA: 0x000E0148 File Offset: 0x000DE348
		// (remove) Token: 0x06003649 RID: 13897 RVA: 0x000E017C File Offset: 0x000DE37C
		public static event Action<IEnumerable<float>> OnFPSEventUpdated;

		// Token: 0x140000AC RID: 172
		// (add) Token: 0x0600364A RID: 13898 RVA: 0x000E01B0 File Offset: 0x000DE3B0
		// (remove) Token: 0x0600364B RID: 13899 RVA: 0x000E01E4 File Offset: 0x000DE3E4
		public static event Action OnOpenExternalMonitor;

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x0600364C RID: 13900 RVA: 0x000E0217 File Offset: 0x000DE417
		// (set) Token: 0x0600364D RID: 13901 RVA: 0x000E021E File Offset: 0x000DE41E
		public static int SamplesPerSecond
		{
			get
			{
				return DebugNetworkEventStatistics._samplesPerSecond;
			}
			set
			{
				DebugNetworkEventStatistics._samplesPerSecond = value;
				DebugNetworkEventStatistics.MaxGraphPointCount = value * 5;
			}
		} = 10;

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x0600364E RID: 13902 RVA: 0x000E022E File Offset: 0x000DE42E
		// (set) Token: 0x0600364F RID: 13903 RVA: 0x000E0235 File Offset: 0x000DE435
		public static bool IsActive { get; private set; }

		// Token: 0x06003651 RID: 13905 RVA: 0x000E0308 File Offset: 0x000DE508
		internal static void StartEvent(string eventName, int eventType)
		{
			if (!DebugNetworkEventStatistics.IsActive)
			{
				return;
			}
			DebugNetworkEventStatistics._curEventType = eventType;
			if (!DebugNetworkEventStatistics._statistics.ContainsKey(DebugNetworkEventStatistics._curEventType))
			{
				DebugNetworkEventStatistics._statistics.Add(DebugNetworkEventStatistics._curEventType, new DebugNetworkEventStatistics.PerEventData
				{
					Name = eventName
				});
			}
			DebugNetworkEventStatistics._statistics[DebugNetworkEventStatistics._curEventType].Count++;
			DebugNetworkEventStatistics._totalData.TotalCount++;
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x000E0380 File Offset: 0x000DE580
		internal static void EndEvent()
		{
			if (!DebugNetworkEventStatistics.IsActive)
			{
				return;
			}
			DebugNetworkEventStatistics.PerEventData perEventData = DebugNetworkEventStatistics._statistics[DebugNetworkEventStatistics._curEventType];
			perEventData.DataSize = perEventData.TotalDataSize / perEventData.Count;
			DebugNetworkEventStatistics._curEventType = -1;
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x000E03BE File Offset: 0x000DE5BE
		internal static void AddDataToStatistic(int bitCount)
		{
			if (!DebugNetworkEventStatistics.IsActive)
			{
				return;
			}
			DebugNetworkEventStatistics._statistics[DebugNetworkEventStatistics._curEventType].TotalDataSize += bitCount;
			DebugNetworkEventStatistics._totalData.TotalDataSize += bitCount;
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x000E03F6 File Offset: 0x000DE5F6
		public static void OpenExternalMonitor()
		{
			if (DebugNetworkEventStatistics.OnOpenExternalMonitor != null)
			{
				DebugNetworkEventStatistics.OnOpenExternalMonitor();
			}
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x000E0409 File Offset: 0x000DE609
		public static void ControlActivate()
		{
			DebugNetworkEventStatistics.IsActive = true;
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x000E0411 File Offset: 0x000DE611
		public static void ControlDeactivate()
		{
			DebugNetworkEventStatistics.IsActive = false;
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x000E0419 File Offset: 0x000DE619
		public static void ControlJustDump()
		{
			DebugNetworkEventStatistics.DumpData();
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x000E0420 File Offset: 0x000DE620
		public static void ControlDumpAll()
		{
			DebugNetworkEventStatistics.DumpData();
			DebugNetworkEventStatistics.DumpReplicationData();
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x000E042C File Offset: 0x000DE62C
		public static void ControlClear()
		{
			DebugNetworkEventStatistics.Clear();
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x000E0433 File Offset: 0x000DE633
		public static void ClearNetGraphs()
		{
			DebugNetworkEventStatistics._eventSamples.Clear();
			DebugNetworkEventStatistics._lossSamples.Clear();
			DebugNetworkEventStatistics._prevEventData = new DebugNetworkEventStatistics.TotalEventData();
			DebugNetworkEventStatistics._currEventData = new DebugNetworkEventStatistics.TotalEventData();
			DebugNetworkEventStatistics._collectSampleCheck = 0f;
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x000E0467 File Offset: 0x000DE667
		public static void ClearFpsGraph()
		{
			DebugNetworkEventStatistics._fpsSamplesUntilNextSampling.Clear();
			DebugNetworkEventStatistics._fpsSamples.Clear();
			DebugNetworkEventStatistics._collectFpsSampleCheck = 0f;
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x000E0487 File Offset: 0x000DE687
		public static void ControlClearAll()
		{
			DebugNetworkEventStatistics.Clear();
			DebugNetworkEventStatistics.ClearFpsGraph();
			DebugNetworkEventStatistics.ClearNetGraphs();
			DebugNetworkEventStatistics.ClearReplicationData();
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x000E049D File Offset: 0x000DE69D
		public static void ControlDumpReplicationData()
		{
			DebugNetworkEventStatistics.DumpReplicationData();
			DebugNetworkEventStatistics.ClearReplicationData();
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x000E04AC File Offset: 0x000DE6AC
		public static void EndTick(float dt)
		{
			if (DebugNetworkEventStatistics._useImgui && Input.DebugInput.IsHotKeyPressed("DebugNetworkEventStatisticsHotkeyToggleActive"))
			{
				DebugNetworkEventStatistics.ToggleActive();
				if (DebugNetworkEventStatistics.IsActive)
				{
					Imgui.NewFrame();
				}
			}
			if (!DebugNetworkEventStatistics.IsActive)
			{
				return;
			}
			DebugNetworkEventStatistics._totalData.TotalTime += dt;
			DebugNetworkEventStatistics._totalData.TotalFrameCount++;
			if (DebugNetworkEventStatistics._useImgui)
			{
				Imgui.BeginMainThreadScope();
				Imgui.Begin("Network panel");
				if (Imgui.Button("Disable Network Panel"))
				{
					DebugNetworkEventStatistics.ToggleActive();
				}
				Imgui.Separator();
				if (Imgui.Button("Show Upload Data (screen)"))
				{
					DebugNetworkEventStatistics._showUploadDataText = !DebugNetworkEventStatistics._showUploadDataText;
				}
				Imgui.Separator();
				if (Imgui.Button("Clear Data"))
				{
					DebugNetworkEventStatistics.Clear();
				}
				if (Imgui.Button("Dump Data (console)"))
				{
					DebugNetworkEventStatistics.DumpData();
				}
				Imgui.Separator();
				if (Imgui.Button("Clear Replication Data"))
				{
					DebugNetworkEventStatistics.ClearReplicationData();
				}
				if (Imgui.Button("Dump Replication Data (console)"))
				{
					DebugNetworkEventStatistics.DumpReplicationData();
				}
				if (Imgui.Button("Dump & Clear Replication Data (console)"))
				{
					DebugNetworkEventStatistics.DumpReplicationData();
					DebugNetworkEventStatistics.ClearReplicationData();
				}
				if (DebugNetworkEventStatistics._showUploadDataText)
				{
					Imgui.Separator();
					DebugNetworkEventStatistics.ShowUploadData();
				}
				Imgui.End();
			}
			if (!DebugNetworkEventStatistics.IsActive)
			{
				return;
			}
			DebugNetworkEventStatistics.CollectFpsSample(dt);
			DebugNetworkEventStatistics._collectSampleCheck += dt;
			if (DebugNetworkEventStatistics._collectSampleCheck >= 1f / (float)DebugNetworkEventStatistics.SamplesPerSecond)
			{
				DebugNetworkEventStatistics._currEventData = DebugNetworkEventStatistics.GetCurrentEventData();
				if (DebugNetworkEventStatistics._currEventData.HasData && DebugNetworkEventStatistics._prevEventData.HasData && DebugNetworkEventStatistics._currEventData != DebugNetworkEventStatistics._prevEventData)
				{
					DebugNetworkEventStatistics._lossSamples.Enqueue(GameNetwork.GetAveragePacketLossRatio());
					DebugNetworkEventStatistics._eventSamples.Enqueue(DebugNetworkEventStatistics._currEventData - DebugNetworkEventStatistics._prevEventData);
					DebugNetworkEventStatistics._prevEventData = DebugNetworkEventStatistics._currEventData;
					if (DebugNetworkEventStatistics._eventSamples.Count > DebugNetworkEventStatistics.MaxGraphPointCount)
					{
						DebugNetworkEventStatistics._eventSamples.Dequeue();
						DebugNetworkEventStatistics._lossSamples.Dequeue();
					}
					if (DebugNetworkEventStatistics._eventSamples.Count >= DebugNetworkEventStatistics.SamplesPerSecond)
					{
						List<DebugNetworkEventStatistics.TotalEventData> range = DebugNetworkEventStatistics._eventSamples.ToList<DebugNetworkEventStatistics.TotalEventData>().GetRange(DebugNetworkEventStatistics._eventSamples.Count - DebugNetworkEventStatistics.SamplesPerSecond, DebugNetworkEventStatistics.SamplesPerSecond);
						DebugNetworkEventStatistics.UploadPerSecondEventData = new DebugNetworkEventStatistics.PerSecondEventData(range.Sum<DebugNetworkEventStatistics.TotalEventData>((DebugNetworkEventStatistics.TotalEventData x) => x.TotalUpload), range.Sum<DebugNetworkEventStatistics.TotalEventData>((DebugNetworkEventStatistics.TotalEventData x) => x.TotalConstantsUpload), range.Sum<DebugNetworkEventStatistics.TotalEventData>((DebugNetworkEventStatistics.TotalEventData x) => x.TotalReliableUpload), range.Sum<DebugNetworkEventStatistics.TotalEventData>((DebugNetworkEventStatistics.TotalEventData x) => x.TotalReplicationUpload), range.Sum<DebugNetworkEventStatistics.TotalEventData>((DebugNetworkEventStatistics.TotalEventData x) => x.TotalUnreliableUpload), range.Sum<DebugNetworkEventStatistics.TotalEventData>((DebugNetworkEventStatistics.TotalEventData x) => x.TotalOtherUpload));
						if (DebugNetworkEventStatistics.OnPerSecondEventDataUpdated != null)
						{
							DebugNetworkEventStatistics.OnPerSecondEventDataUpdated(DebugNetworkEventStatistics.UploadPerSecondEventData);
						}
					}
					if (DebugNetworkEventStatistics.OnEventDataUpdated != null)
					{
						DebugNetworkEventStatistics.OnEventDataUpdated(DebugNetworkEventStatistics._eventSamples.ToList<DebugNetworkEventStatistics.TotalEventData>());
					}
					DebugNetworkEventStatistics._collectSampleCheck -= 1f / (float)DebugNetworkEventStatistics.SamplesPerSecond;
				}
			}
			if (DebugNetworkEventStatistics._useImgui)
			{
				Imgui.Begin("Network Graph panel");
				float[] array = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalUpload / 8192f).ToArray<float>();
				float num = ((array.Length != 0) ? array.Max() : 0f);
				DebugNetworkEventStatistics._targetMaxGraphHeight = (DebugNetworkEventStatistics._useAbsoluteMaximum ? MathF.Max(num, DebugNetworkEventStatistics._targetMaxGraphHeight) : num);
				float num2 = MBMath.ClampFloat(3f * dt, 0f, 1f);
				DebugNetworkEventStatistics._curMaxGraphHeight = MBMath.Lerp(DebugNetworkEventStatistics._curMaxGraphHeight, DebugNetworkEventStatistics._targetMaxGraphHeight, num2, 1E-05f);
				if (DebugNetworkEventStatistics.UploadPerSecondEventData != null)
				{
					Imgui.Text(string.Concat(new object[]
					{
						"Taking ",
						DebugNetworkEventStatistics.SamplesPerSecond,
						" samples per second. Total KiB per second:",
						(float)DebugNetworkEventStatistics.UploadPerSecondEventData.TotalUploadPerSecond / 8192f
					}));
				}
				float[] array2 = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalConstantsUpload / 8192f).ToArray<float>();
				Imgui.PlotLines("", array2, DebugNetworkEventStatistics._eventSamples.Count, 0, "Constants upload (in KiB)", 0f, DebugNetworkEventStatistics._curMaxGraphHeight, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + DebugNetworkEventStatistics._curMaxGraphHeight);
				float[] array3 = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalReliableUpload / 8192f).ToArray<float>();
				Imgui.PlotLines("", array3, DebugNetworkEventStatistics._eventSamples.Count, 0, "Reliable upload (in KiB)", 0f, DebugNetworkEventStatistics._curMaxGraphHeight, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + DebugNetworkEventStatistics._curMaxGraphHeight);
				float[] array4 = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalReplicationUpload / 8192f).ToArray<float>();
				Imgui.PlotLines("", array4, DebugNetworkEventStatistics._eventSamples.Count, 0, "Replication upload (in KiB)", 0f, DebugNetworkEventStatistics._curMaxGraphHeight, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + DebugNetworkEventStatistics._curMaxGraphHeight);
				float[] array5 = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalUnreliableUpload / 8192f).ToArray<float>();
				Imgui.PlotLines("", array5, DebugNetworkEventStatistics._eventSamples.Count, 0, "Unreliable upload (in KiB)", 0f, DebugNetworkEventStatistics._curMaxGraphHeight, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + DebugNetworkEventStatistics._curMaxGraphHeight);
				float[] array6 = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalOtherUpload / 8192f).ToArray<float>();
				Imgui.PlotLines("", array6, DebugNetworkEventStatistics._eventSamples.Count, 0, "Other upload (in KiB)", 0f, DebugNetworkEventStatistics._curMaxGraphHeight, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + DebugNetworkEventStatistics._curMaxGraphHeight);
				Imgui.Separator();
				float[] array7 = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalUpload / (float)x.TotalPackets / 8f).ToArray<float>();
				Imgui.PlotLines("", array7, DebugNetworkEventStatistics._eventSamples.Count, 0, "Data per package (in B)", 0f, 1400f, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + 1400);
				Imgui.Separator();
				float num3 = ((DebugNetworkEventStatistics._lossSamples.Count > 0) ? DebugNetworkEventStatistics._lossSamples.Max() : 0f);
				DebugNetworkEventStatistics._targetMaxLossGraphHeight = (DebugNetworkEventStatistics._useAbsoluteMaximum ? MathF.Max(num3, DebugNetworkEventStatistics._targetMaxLossGraphHeight) : num3);
				float num4 = MBMath.ClampFloat(3f * dt, 0f, 1f);
				DebugNetworkEventStatistics._currMaxLossGraphHeight = MBMath.Lerp(DebugNetworkEventStatistics._currMaxLossGraphHeight, DebugNetworkEventStatistics._targetMaxLossGraphHeight, num4, 1E-05f);
				Imgui.PlotLines("", DebugNetworkEventStatistics._lossSamples.ToArray(), DebugNetworkEventStatistics._lossSamples.Count, 0, "Averaged loss ratio", 0f, DebugNetworkEventStatistics._currMaxLossGraphHeight, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + DebugNetworkEventStatistics._currMaxLossGraphHeight);
				Imgui.Checkbox("Use absolute Maximum", ref DebugNetworkEventStatistics._useAbsoluteMaximum);
				Imgui.End();
			}
			Imgui.EndMainThreadScope();
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x000E0D18 File Offset: 0x000DEF18
		private static void CollectFpsSample(float dt)
		{
			if (DebugNetworkEventStatistics.TrackFps)
			{
				float fps = Utilities.GetFps();
				if (!float.IsInfinity(fps) && !float.IsNegativeInfinity(fps) && !float.IsNaN(fps))
				{
					DebugNetworkEventStatistics._fpsSamplesUntilNextSampling.Add(fps);
				}
				DebugNetworkEventStatistics._collectFpsSampleCheck += dt;
				if (DebugNetworkEventStatistics._collectFpsSampleCheck >= 1f / (float)DebugNetworkEventStatistics.SamplesPerSecond)
				{
					if (DebugNetworkEventStatistics._fpsSamplesUntilNextSampling.Count > 0)
					{
						DebugNetworkEventStatistics._fpsSamples.Enqueue(DebugNetworkEventStatistics._fpsSamplesUntilNextSampling.Min());
						DebugNetworkEventStatistics._fpsSamplesUntilNextSampling.Clear();
						if (DebugNetworkEventStatistics._fpsSamples.Count > DebugNetworkEventStatistics.MaxGraphPointCount)
						{
							DebugNetworkEventStatistics._fpsSamples.Dequeue();
						}
						Action<IEnumerable<float>> onFPSEventUpdated = DebugNetworkEventStatistics.OnFPSEventUpdated;
						if (onFPSEventUpdated != null)
						{
							onFPSEventUpdated(DebugNetworkEventStatistics._fpsSamples.ToList<float>());
						}
					}
					DebugNetworkEventStatistics._collectFpsSampleCheck -= 1f / (float)DebugNetworkEventStatistics.SamplesPerSecond;
				}
			}
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x000E0DEF File Offset: 0x000DEFEF
		private static void ToggleActive()
		{
			DebugNetworkEventStatistics.IsActive = !DebugNetworkEventStatistics.IsActive;
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x000E0DFE File Offset: 0x000DEFFE
		private static void Clear()
		{
			DebugNetworkEventStatistics._totalData = new DebugNetworkEventStatistics.TotalData();
			DebugNetworkEventStatistics._statistics = new Dictionary<int, DebugNetworkEventStatistics.PerEventData>();
			GameNetwork.ResetDebugUploads();
			DebugNetworkEventStatistics._curEventType = -1;
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x000E0E20 File Offset: 0x000DF020
		private static void DumpData()
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "DumpData");
			mbstringBuilder.AppendLine();
			mbstringBuilder.AppendLine<string>("///GENERAL DATA///");
			mbstringBuilder.AppendLine<string>("Total elapsed time: " + DebugNetworkEventStatistics._totalData.TotalTime + " seconds.");
			mbstringBuilder.AppendLine<string>("Total frame count: " + DebugNetworkEventStatistics._totalData.TotalFrameCount);
			mbstringBuilder.AppendLine<string>("Total avg packet count: " + (int)(DebugNetworkEventStatistics._totalData.TotalTime / 60f));
			mbstringBuilder.AppendLine<string>("Total event data size: " + DebugNetworkEventStatistics._totalData.TotalDataSize + " bits.");
			mbstringBuilder.AppendLine<string>("Total event count: " + DebugNetworkEventStatistics._totalData.TotalCount);
			mbstringBuilder.AppendLine();
			mbstringBuilder.AppendLine<string>("///ALL DATA///");
			List<DebugNetworkEventStatistics.PerEventData> list = new List<DebugNetworkEventStatistics.PerEventData>();
			list.AddRange(DebugNetworkEventStatistics._statistics.Values);
			list.Sort();
			foreach (DebugNetworkEventStatistics.PerEventData perEventData in list)
			{
				mbstringBuilder.AppendLine<string>("Event name: " + perEventData.Name);
				mbstringBuilder.AppendLine<string>("\tEvent size (for one event): " + perEventData.DataSize + " bits.");
				mbstringBuilder.AppendLine<string>("\tTotal count: " + perEventData.Count);
				mbstringBuilder.AppendLine<string>(string.Concat(new object[]
				{
					"\tTotal size: ",
					perEventData.TotalDataSize,
					"bits | ~",
					perEventData.TotalDataSize / 8 + ((perEventData.TotalDataSize % 8 == 0) ? 0 : 1),
					" bytes."
				}));
				mbstringBuilder.AppendLine<string>("\tTotal count per frame: " + (float)perEventData.Count / (float)DebugNetworkEventStatistics._totalData.TotalFrameCount);
				mbstringBuilder.AppendLine<string>("\tTotal size per frame: " + (float)perEventData.TotalDataSize / (float)DebugNetworkEventStatistics._totalData.TotalFrameCount + " bits per frame.");
				mbstringBuilder.AppendLine();
			}
			DebugNetworkEventStatistics.GetFormattedDebugUploadDataOutput(ref mbstringBuilder);
			mbstringBuilder.AppendLine<string>("NetworkEventStaticticsLogLength: " + mbstringBuilder.Length + "\n");
			MBDebug.Print(mbstringBuilder.ToStringAndRelease(), 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x000E10DC File Offset: 0x000DF2DC
		private static void GetFormattedDebugUploadDataOutput(ref MBStringBuilder outStr)
		{
			GameNetwork.DebugNetworkPacketStatisticsStruct debugNetworkPacketStatisticsStruct = default(GameNetwork.DebugNetworkPacketStatisticsStruct);
			GameNetwork.DebugNetworkPositionCompressionStatisticsStruct debugNetworkPositionCompressionStatisticsStruct = default(GameNetwork.DebugNetworkPositionCompressionStatisticsStruct);
			GameNetwork.GetDebugUploadsInBits(ref debugNetworkPacketStatisticsStruct, ref debugNetworkPositionCompressionStatisticsStruct);
			outStr.AppendLine<string>("REAL NETWORK UPLOAD PERCENTS");
			if (debugNetworkPacketStatisticsStruct.TotalUpload == 0)
			{
				outStr.AppendLine<string>("Total Upload is ZERO");
				return;
			}
			int num = debugNetworkPacketStatisticsStruct.TotalUpload - (debugNetworkPacketStatisticsStruct.TotalConstantsUpload + debugNetworkPacketStatisticsStruct.TotalReliableEventUpload + debugNetworkPacketStatisticsStruct.TotalReplicationUpload + debugNetworkPacketStatisticsStruct.TotalUnreliableEventUpload);
			if (num == debugNetworkPacketStatisticsStruct.TotalUpload)
			{
				outStr.AppendLine<string>("USE_DEBUG_NETWORK_PACKET_PERCENTS not defined!");
			}
			else
			{
				outStr.AppendLine<string>("\tAverage Ping: " + debugNetworkPacketStatisticsStruct.AveragePingTime);
				outStr.AppendLine<string>("\tTime out period: " + debugNetworkPacketStatisticsStruct.TimeOutPeriod);
				outStr.AppendLine<string>("\tLost Percent: " + debugNetworkPacketStatisticsStruct.LostPercent);
				outStr.AppendLine<string>("\tlost_count: " + debugNetworkPacketStatisticsStruct.LostCount);
				outStr.AppendLine<string>("\ttotal_count_on_lost_check: " + debugNetworkPacketStatisticsStruct.TotalCountOnLostCheck);
				outStr.AppendLine<string>("\tround_trip_time: " + debugNetworkPacketStatisticsStruct.RoundTripTime);
				float num2 = (float)debugNetworkPacketStatisticsStruct.TotalUpload;
				float num3 = 1f / (float)debugNetworkPacketStatisticsStruct.TotalPackets;
				outStr.AppendLine<string>(string.Concat(new object[]
				{
					"\tConstants Upload: percent: ",
					(float)debugNetworkPacketStatisticsStruct.TotalConstantsUpload / num2 * 100f,
					"; size in bits: ",
					(float)debugNetworkPacketStatisticsStruct.TotalConstantsUpload * num3,
					";"
				}));
				outStr.AppendLine<string>(string.Concat(new object[]
				{
					"\tReliable Upload: percent: ",
					(float)debugNetworkPacketStatisticsStruct.TotalReliableEventUpload / num2 * 100f,
					"; size in bits: ",
					(float)debugNetworkPacketStatisticsStruct.TotalReliableEventUpload * num3,
					";"
				}));
				outStr.AppendLine<string>(string.Concat(new object[]
				{
					"\tReplication Upload: percent: ",
					(float)debugNetworkPacketStatisticsStruct.TotalReplicationUpload / num2 * 100f,
					"; size in bits: ",
					(float)debugNetworkPacketStatisticsStruct.TotalReplicationUpload * num3,
					";"
				}));
				outStr.AppendLine<string>(string.Concat(new object[]
				{
					"\tUnreliable Upload: percent: ",
					(float)debugNetworkPacketStatisticsStruct.TotalUnreliableEventUpload / num2 * 100f,
					"; size in bits: ",
					(float)debugNetworkPacketStatisticsStruct.TotalUnreliableEventUpload * num3,
					";"
				}));
				outStr.AppendLine<string>(string.Concat(new object[]
				{
					"\tOthers (headers, ack etc.) Upload: percent: ",
					(float)num / num2 * 100f,
					"; size in bits: ",
					(float)num * num3,
					";"
				}));
				int num4 = debugNetworkPositionCompressionStatisticsStruct.totalPositionCoarseBitCountX + debugNetworkPositionCompressionStatisticsStruct.totalPositionCoarseBitCountY + debugNetworkPositionCompressionStatisticsStruct.totalPositionCoarseBitCountZ;
				float num5 = 1f / (float)debugNetworkPacketStatisticsStruct.TotalCellPriorityChecks;
				outStr.AppendLine<string>(string.Concat(new object[]
				{
					"\n\tTotal PPS: ",
					(float)debugNetworkPacketStatisticsStruct.TotalPackets / DebugNetworkEventStatistics._totalData.TotalTime,
					"; bps: ",
					(float)debugNetworkPacketStatisticsStruct.TotalUpload / DebugNetworkEventStatistics._totalData.TotalTime,
					";"
				}));
			}
			outStr.AppendLine<string>(string.Concat(new object[]
			{
				"\n\tTotal packets: ",
				debugNetworkPacketStatisticsStruct.TotalPackets,
				"; bits per packet: ",
				(float)debugNetworkPacketStatisticsStruct.TotalUpload / (float)debugNetworkPacketStatisticsStruct.TotalPackets,
				";"
			}));
			outStr.AppendLine<string>("Total Upload: " + debugNetworkPacketStatisticsStruct.TotalUpload + " in bits");
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x000E14A8 File Offset: 0x000DF6A8
		private static void ShowUploadData()
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "ShowUploadData");
			DebugNetworkEventStatistics.GetFormattedDebugUploadDataOutput(ref mbstringBuilder);
			string[] array = mbstringBuilder.ToStringAndRelease().Split(new char[] { '\n' });
			for (int i = 0; i < array.Length; i++)
			{
				Imgui.Text(array[i]);
			}
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x000E1500 File Offset: 0x000DF700
		private static DebugNetworkEventStatistics.TotalEventData GetCurrentEventData()
		{
			GameNetwork.DebugNetworkPacketStatisticsStruct debugNetworkPacketStatisticsStruct = default(GameNetwork.DebugNetworkPacketStatisticsStruct);
			GameNetwork.DebugNetworkPositionCompressionStatisticsStruct debugNetworkPositionCompressionStatisticsStruct = default(GameNetwork.DebugNetworkPositionCompressionStatisticsStruct);
			GameNetwork.GetDebugUploadsInBits(ref debugNetworkPacketStatisticsStruct, ref debugNetworkPositionCompressionStatisticsStruct);
			return new DebugNetworkEventStatistics.TotalEventData(debugNetworkPacketStatisticsStruct.TotalPackets, debugNetworkPacketStatisticsStruct.TotalUpload, debugNetworkPacketStatisticsStruct.TotalConstantsUpload, debugNetworkPacketStatisticsStruct.TotalReliableEventUpload, debugNetworkPacketStatisticsStruct.TotalReplicationUpload, debugNetworkPacketStatisticsStruct.TotalUnreliableEventUpload);
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x000E154F File Offset: 0x000DF74F
		private static void DumpReplicationData()
		{
			GameNetwork.PrintReplicationTableStatistics();
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x000E1556 File Offset: 0x000DF756
		private static void ClearReplicationData()
		{
			GameNetwork.ClearReplicationTableStatistics();
		}

		// Token: 0x0400175F RID: 5983
		private static DebugNetworkEventStatistics.TotalData _totalData = new DebugNetworkEventStatistics.TotalData();

		// Token: 0x04001760 RID: 5984
		private static int _curEventType = -1;

		// Token: 0x04001761 RID: 5985
		private static Dictionary<int, DebugNetworkEventStatistics.PerEventData> _statistics = new Dictionary<int, DebugNetworkEventStatistics.PerEventData>();

		// Token: 0x04001762 RID: 5986
		private static int _samplesPerSecond;

		// Token: 0x04001763 RID: 5987
		public static int MaxGraphPointCount;

		// Token: 0x04001764 RID: 5988
		private static bool _showUploadDataText = false;

		// Token: 0x04001765 RID: 5989
		private static bool _useAbsoluteMaximum = false;

		// Token: 0x04001766 RID: 5990
		private static float _collectSampleCheck = 0f;

		// Token: 0x04001767 RID: 5991
		private static float _collectFpsSampleCheck = 0f;

		// Token: 0x04001768 RID: 5992
		private static float _curMaxGraphHeight = 0f;

		// Token: 0x04001769 RID: 5993
		private static float _targetMaxGraphHeight = 0f;

		// Token: 0x0400176A RID: 5994
		private static float _currMaxLossGraphHeight = 0f;

		// Token: 0x0400176B RID: 5995
		private static float _targetMaxLossGraphHeight = 0f;

		// Token: 0x0400176C RID: 5996
		private static DebugNetworkEventStatistics.PerSecondEventData UploadPerSecondEventData;

		// Token: 0x0400176D RID: 5997
		private static readonly Queue<DebugNetworkEventStatistics.TotalEventData> _eventSamples = new Queue<DebugNetworkEventStatistics.TotalEventData>();

		// Token: 0x0400176E RID: 5998
		private static readonly Queue<float> _lossSamples = new Queue<float>();

		// Token: 0x0400176F RID: 5999
		private static DebugNetworkEventStatistics.TotalEventData _prevEventData = new DebugNetworkEventStatistics.TotalEventData();

		// Token: 0x04001770 RID: 6000
		private static DebugNetworkEventStatistics.TotalEventData _currEventData = new DebugNetworkEventStatistics.TotalEventData();

		// Token: 0x04001771 RID: 6001
		private static readonly List<float> _fpsSamplesUntilNextSampling = new List<float>();

		// Token: 0x04001772 RID: 6002
		private static readonly Queue<float> _fpsSamples = new Queue<float>();

		// Token: 0x04001773 RID: 6003
		private static bool _useImgui = !GameNetwork.IsDedicatedServer;

		// Token: 0x04001774 RID: 6004
		public static bool TrackFps = false;

		// Token: 0x0200068F RID: 1679
		public class TotalEventData
		{
			// Token: 0x0600422E RID: 16942 RVA: 0x000FFCF0 File Offset: 0x000FDEF0
			protected bool Equals(DebugNetworkEventStatistics.TotalEventData other)
			{
				return this.TotalPackets == other.TotalPackets && this.TotalUpload == other.TotalUpload && this.TotalConstantsUpload == other.TotalConstantsUpload && this.TotalReliableUpload == other.TotalReliableUpload && this.TotalReplicationUpload == other.TotalReplicationUpload && this.TotalUnreliableUpload == other.TotalUnreliableUpload && this.TotalOtherUpload == other.TotalOtherUpload;
			}

			// Token: 0x0600422F RID: 16943 RVA: 0x000FFD61 File Offset: 0x000FDF61
			public override bool Equals(object obj)
			{
				return obj != null && (this == obj || (obj.GetType() == base.GetType() && this.Equals((DebugNetworkEventStatistics.TotalEventData)obj)));
			}

			// Token: 0x06004230 RID: 16944 RVA: 0x000FFD90 File Offset: 0x000FDF90
			public override int GetHashCode()
			{
				return (((((((((((this.TotalPackets * 397) ^ this.TotalUpload) * 397) ^ this.TotalConstantsUpload) * 397) ^ this.TotalReliableUpload) * 397) ^ this.TotalReplicationUpload) * 397) ^ this.TotalUnreliableUpload) * 397) ^ this.TotalOtherUpload;
			}

			// Token: 0x06004231 RID: 16945 RVA: 0x000FFDF1 File Offset: 0x000FDFF1
			public TotalEventData()
			{
			}

			// Token: 0x06004232 RID: 16946 RVA: 0x000FFDFC File Offset: 0x000FDFFC
			public TotalEventData(int totalPackets, int totalUpload, int totalConstants, int totalReliable, int totalReplication, int totalUnreliable)
			{
				this.TotalPackets = totalPackets;
				this.TotalUpload = totalUpload;
				this.TotalConstantsUpload = totalConstants;
				this.TotalReliableUpload = totalReliable;
				this.TotalReplicationUpload = totalReplication;
				this.TotalUnreliableUpload = totalUnreliable;
				this.TotalOtherUpload = totalUpload - (totalConstants + totalReliable + totalReplication + totalUnreliable);
			}

			// Token: 0x17000B1C RID: 2844
			// (get) Token: 0x06004233 RID: 16947 RVA: 0x000FFE4E File Offset: 0x000FE04E
			public bool HasData
			{
				get
				{
					return this.TotalUpload > 0;
				}
			}

			// Token: 0x06004234 RID: 16948 RVA: 0x000FFE5C File Offset: 0x000FE05C
			public static DebugNetworkEventStatistics.TotalEventData operator -(DebugNetworkEventStatistics.TotalEventData d1, DebugNetworkEventStatistics.TotalEventData d2)
			{
				return new DebugNetworkEventStatistics.TotalEventData(d1.TotalPackets - d2.TotalPackets, d1.TotalUpload - d2.TotalUpload, d1.TotalConstantsUpload - d2.TotalConstantsUpload, d1.TotalReliableUpload - d2.TotalReliableUpload, d1.TotalReplicationUpload - d2.TotalReplicationUpload, d1.TotalUnreliableUpload - d2.TotalUnreliableUpload);
			}

			// Token: 0x06004235 RID: 16949 RVA: 0x000FFEBC File Offset: 0x000FE0BC
			public static bool operator ==(DebugNetworkEventStatistics.TotalEventData d1, DebugNetworkEventStatistics.TotalEventData d2)
			{
				return d1.TotalPackets == d2.TotalPackets && d1.TotalUpload == d2.TotalUpload && d1.TotalConstantsUpload == d2.TotalConstantsUpload && d1.TotalReliableUpload == d2.TotalReliableUpload && d1.TotalReplicationUpload == d2.TotalReplicationUpload && d1.TotalUnreliableUpload == d2.TotalUnreliableUpload;
			}

			// Token: 0x06004236 RID: 16950 RVA: 0x000FFF1F File Offset: 0x000FE11F
			public static bool operator !=(DebugNetworkEventStatistics.TotalEventData d1, DebugNetworkEventStatistics.TotalEventData d2)
			{
				return !(d1 == d2);
			}

			// Token: 0x0400230F RID: 8975
			public readonly int TotalPackets;

			// Token: 0x04002310 RID: 8976
			public readonly int TotalUpload;

			// Token: 0x04002311 RID: 8977
			public readonly int TotalConstantsUpload;

			// Token: 0x04002312 RID: 8978
			public readonly int TotalReliableUpload;

			// Token: 0x04002313 RID: 8979
			public readonly int TotalReplicationUpload;

			// Token: 0x04002314 RID: 8980
			public readonly int TotalUnreliableUpload;

			// Token: 0x04002315 RID: 8981
			public readonly int TotalOtherUpload;
		}

		// Token: 0x02000690 RID: 1680
		private class PerEventData : IComparable<DebugNetworkEventStatistics.PerEventData>
		{
			// Token: 0x06004237 RID: 16951 RVA: 0x000FFF2B File Offset: 0x000FE12B
			public int CompareTo(DebugNetworkEventStatistics.PerEventData other)
			{
				return other.TotalDataSize - this.TotalDataSize;
			}

			// Token: 0x04002316 RID: 8982
			public string Name;

			// Token: 0x04002317 RID: 8983
			public int DataSize;

			// Token: 0x04002318 RID: 8984
			public int TotalDataSize;

			// Token: 0x04002319 RID: 8985
			public int Count;
		}

		// Token: 0x02000691 RID: 1681
		public class PerSecondEventData
		{
			// Token: 0x06004239 RID: 16953 RVA: 0x000FFF42 File Offset: 0x000FE142
			public PerSecondEventData(int totalUploadPerSecond, int constantsUploadPerSecond, int reliableUploadPerSecond, int replicationUploadPerSecond, int unreliableUploadPerSecond, int otherUploadPerSecond)
			{
				this.TotalUploadPerSecond = totalUploadPerSecond;
				this.ConstantsUploadPerSecond = constantsUploadPerSecond;
				this.ReliableUploadPerSecond = reliableUploadPerSecond;
				this.ReplicationUploadPerSecond = replicationUploadPerSecond;
				this.UnreliableUploadPerSecond = unreliableUploadPerSecond;
				this.OtherUploadPerSecond = otherUploadPerSecond;
			}

			// Token: 0x0400231A RID: 8986
			public readonly int TotalUploadPerSecond;

			// Token: 0x0400231B RID: 8987
			public readonly int ConstantsUploadPerSecond;

			// Token: 0x0400231C RID: 8988
			public readonly int ReliableUploadPerSecond;

			// Token: 0x0400231D RID: 8989
			public readonly int ReplicationUploadPerSecond;

			// Token: 0x0400231E RID: 8990
			public readonly int UnreliableUploadPerSecond;

			// Token: 0x0400231F RID: 8991
			public readonly int OtherUploadPerSecond;
		}

		// Token: 0x02000692 RID: 1682
		private class TotalData
		{
			// Token: 0x04002320 RID: 8992
			public float TotalTime;

			// Token: 0x04002321 RID: 8993
			public int TotalFrameCount;

			// Token: 0x04002322 RID: 8994
			public int TotalCount;

			// Token: 0x04002323 RID: 8995
			public int TotalDataSize;
		}
	}
}
