using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000096 RID: 150
	[Serializable]
	public struct Transformation
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x000133B4 File Offset: 0x000115B4
		public static Transformation Identity
		{
			get
			{
				return new Transformation(new Vec3(0f, 0f, 0f, 1f), Mat3.Identity, Vec3.One);
			}
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000133DE File Offset: 0x000115DE
		public Transformation(Vec3 origin, Mat3 rotation, Vec3 scale)
		{
			this.Origin = origin;
			this.Rotation = rotation;
			this.Scale = scale;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x000133F8 File Offset: 0x000115F8
		public MatrixFrame AsMatrixFrame
		{
			get
			{
				MatrixFrame matrixFrame = default(MatrixFrame);
				matrixFrame.origin = this.Origin;
				matrixFrame.rotation = this.Rotation;
				matrixFrame.rotation.ApplyScaleLocal(in this.Scale);
				matrixFrame.Fill();
				return matrixFrame;
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00013444 File Offset: 0x00011644
		public static Transformation CreateFromMatrixFrame(MatrixFrame matrixFrame)
		{
			Mat3 rotation = matrixFrame.rotation;
			Vec3 scaleVector = matrixFrame.rotation.GetScaleVector();
			Vec3 vec = new Vec3(1f / scaleVector.x, 1f / scaleVector.y, 1f / scaleVector.z, -1f);
			rotation.ApplyScaleLocal(in vec);
			return new Transformation(matrixFrame.origin, rotation, scaleVector);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x000134AA File Offset: 0x000116AA
		public static Transformation CreateFromRotation(Mat3 rotation)
		{
			return new Transformation(Vec3.Zero, rotation, Vec3.One);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x000134BC File Offset: 0x000116BC
		public Vec3 TransformToParent(Vec3 v)
		{
			return this.AsMatrixFrame.TransformToParent(in v);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000134DC File Offset: 0x000116DC
		public Transformation TransformToParent(Transformation t)
		{
			MatrixFrame asMatrixFrame = this.AsMatrixFrame;
			MatrixFrame asMatrixFrame2 = t.AsMatrixFrame;
			return Transformation.CreateFromMatrixFrame(asMatrixFrame.TransformToParent(in asMatrixFrame2));
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00013508 File Offset: 0x00011708
		public Vec3 TransformToLocal(Vec3 v)
		{
			return this.AsMatrixFrame.TransformToLocal(in v);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00013528 File Offset: 0x00011728
		public Transformation TransformToLocal(Transformation t)
		{
			MatrixFrame asMatrixFrame = this.AsMatrixFrame;
			MatrixFrame asMatrixFrame2 = t.AsMatrixFrame;
			return Transformation.CreateFromMatrixFrame(asMatrixFrame.TransformToLocal(in asMatrixFrame2));
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00013554 File Offset: 0x00011754
		public void Rotate(float radian, Vec3 axis)
		{
			Transformation transformation = this;
			transformation.Scale = Vec3.One;
			MatrixFrame asMatrixFrame = transformation.AsMatrixFrame;
			asMatrixFrame.Rotate(radian, in axis);
			this.Rotation = asMatrixFrame.rotation;
			this.Origin = asMatrixFrame.origin;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001359E File Offset: 0x0001179E
		public static bool operator ==(Transformation t1, Transformation t2)
		{
			return t1.Origin == t2.Origin && (in t1.Rotation) == (in t2.Rotation) && t1.Scale == t2.Scale;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x000135DC File Offset: 0x000117DC
		public void ApplyScale(Vec3 vec3)
		{
			this.Scale.x = this.Scale.x * vec3.x;
			this.Scale.y = this.Scale.y * vec3.y;
			this.Scale.z = this.Scale.z * vec3.z;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00013628 File Offset: 0x00011828
		public static bool operator !=(Transformation t1, Transformation t2)
		{
			return t1.Origin != t2.Origin || (in t1.Rotation) != (in t2.Rotation) || t1.Scale != t2.Scale;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00013665 File Offset: 0x00011865
		public override bool Equals(object obj)
		{
			return this == (Transformation)obj;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00013678 File Offset: 0x00011878
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001368C File Offset: 0x0001188C
		public override string ToString()
		{
			string text = "Transformation:\n";
			text = string.Concat(new object[]
			{
				text,
				"Origin: ",
				this.Origin.x,
				", ",
				this.Origin.y,
				", ",
				this.Origin.z,
				"\n"
			});
			text += "Rotation:\n";
			text += this.Rotation.ToString();
			return string.Concat(new object[]
			{
				text,
				"Scale: ",
				this.Scale.x,
				", ",
				this.Scale.y,
				", ",
				this.Scale.z,
				"\n"
			});
		}

		// Token: 0x040001AF RID: 431
		public Vec3 Origin;

		// Token: 0x040001B0 RID: 432
		public Mat3 Rotation;

		// Token: 0x040001B1 RID: 433
		public Vec3 Scale;
	}
}
