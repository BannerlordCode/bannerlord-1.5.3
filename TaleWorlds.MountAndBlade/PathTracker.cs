using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000341 RID: 833
	public class PathTracker
	{
		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06002EEC RID: 12012 RVA: 0x000B5E80 File Offset: 0x000B4080
		// (set) Token: 0x06002EED RID: 12013 RVA: 0x000B5E88 File Offset: 0x000B4088
		public float TotalDistanceTraveled { get; set; }

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06002EEE RID: 12014 RVA: 0x000B5E91 File Offset: 0x000B4091
		public bool HasChanged
		{
			get
			{
				return this._path != null && this._version < this._path.GetVersion();
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06002EEF RID: 12015 RVA: 0x000B5EB6 File Offset: 0x000B40B6
		public bool IsValid
		{
			get
			{
				return this._path != null;
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06002EF0 RID: 12016 RVA: 0x000B5EC4 File Offset: 0x000B40C4
		public bool HasReachedEnd
		{
			get
			{
				return this.TotalDistanceTraveled >= this._path.TotalDistance;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06002EF1 RID: 12017 RVA: 0x000B5EDC File Offset: 0x000B40DC
		public float PathTraveledPercentage
		{
			get
			{
				return this.TotalDistanceTraveled / this._path.TotalDistance;
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002EF2 RID: 12018 RVA: 0x000B5EF0 File Offset: 0x000B40F0
		public MatrixFrame CurrentFrame
		{
			get
			{
				MatrixFrame frameForDistance = this._path.GetFrameForDistance(this.TotalDistanceTraveled);
				frameForDistance.rotation.RotateAboutUp(3.1415927f);
				frameForDistance.rotation.ApplyScaleLocal(in this._initialScale);
				return frameForDistance;
			}
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x000B5F33 File Offset: 0x000B4133
		public PathTracker(Path path, Vec3 initialScaleOfEntity)
		{
			this._path = path;
			this._initialScale = initialScaleOfEntity;
			if (path != null)
			{
				this.UpdateVersion();
			}
			this.Reset();
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x000B5F65 File Offset: 0x000B4165
		public void UpdateVersion()
		{
			this._version = this._path.GetVersion();
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x000B5F78 File Offset: 0x000B4178
		public bool PathExists()
		{
			return this._path != null;
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x000B5F86 File Offset: 0x000B4186
		public void Advance(float deltaDistance)
		{
			this.TotalDistanceTraveled += deltaDistance;
			this.TotalDistanceTraveled = MathF.Min(this.TotalDistanceTraveled, this._path.TotalDistance);
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x000B5FB2 File Offset: 0x000B41B2
		public float GetPathLength()
		{
			return this._path.TotalDistance;
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x000B5FBF File Offset: 0x000B41BF
		public void CurrentFrameAndColor(out MatrixFrame frame, out Vec3 color)
		{
			this._path.GetFrameAndColorForDistance(this.TotalDistanceTraveled, out frame, out color);
			frame.rotation.RotateAboutUp(3.1415927f);
			frame.rotation.ApplyScaleLocal(in this._initialScale);
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x000B5FF5 File Offset: 0x000B41F5
		public void Reset()
		{
			this.TotalDistanceTraveled = 0f;
		}

		// Token: 0x040012AA RID: 4778
		private readonly Path _path;

		// Token: 0x040012AB RID: 4779
		private readonly Vec3 _initialScale;

		// Token: 0x040012AC RID: 4780
		private int _version = -1;
	}
}
