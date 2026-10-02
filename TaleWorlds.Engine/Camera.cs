using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200000F RID: 15
	[EngineClass("rglCamera_object")]
	public sealed class Camera : NativeObject
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00003090 File Offset: 0x00001290
		internal Camera(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000309F File Offset: 0x0000129F
		public static Camera CreateCamera()
		{
			return EngineApplicationInterface.ICamera.CreateCamera();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000030AB File Offset: 0x000012AB
		public void ReleaseCamera()
		{
			EngineApplicationInterface.ICamera.Release(base.Pointer);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000030BD File Offset: 0x000012BD
		public void ReleaseCameraEntity()
		{
			EngineApplicationInterface.ICamera.ReleaseCameraEntity(base.Pointer);
			this.ReleaseCamera();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000030D5 File Offset: 0x000012D5
		public void LookAt(Vec3 position, Vec3 target, Vec3 upVector)
		{
			EngineApplicationInterface.ICamera.LookAt(base.Pointer, position, target, upVector);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000030EC File Offset: 0x000012EC
		public void ScreenSpaceRayProjection(Vec2 screenPosition, ref Vec3 rayBegin, ref Vec3 rayEnd)
		{
			EngineApplicationInterface.ICamera.ScreenSpaceRayProjection(base.Pointer, screenPosition, ref rayBegin, ref rayEnd);
			if (this.Entity != null)
			{
				rayBegin = this.Entity.GetGlobalFrame().TransformToParent(in rayBegin);
				rayEnd = this.Entity.GetGlobalFrame().TransformToParent(in rayEnd);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000314E File Offset: 0x0000134E
		public bool CheckEntityVisibility(GameEntity entity)
		{
			return EngineApplicationInterface.ICamera.CheckEntityVisibility(base.Pointer, entity.Pointer);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003168 File Offset: 0x00001368
		public void SetViewVolume(bool perspective, float dLeft, float dRight, float dBottom, float dTop, float dNear, float dFar)
		{
			EngineApplicationInterface.ICamera.SetViewVolume(base.Pointer, perspective, dLeft, dRight, dBottom, dTop, dNear, dFar);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003190 File Offset: 0x00001390
		public static void GetNearPlanePointsStatic(ref MatrixFrame cameraFrame, float verticalFov, float aspectRatioXY, float newDNear, float newDFar, Vec3[] nearPlanePoints)
		{
			EngineApplicationInterface.ICamera.GetNearPlanePointsStatic(ref cameraFrame, verticalFov, aspectRatioXY, newDNear, newDFar, nearPlanePoints);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000031A4 File Offset: 0x000013A4
		public void GetNearPlanePoints(Vec3[] nearPlanePoints)
		{
			EngineApplicationInterface.ICamera.GetNearPlanePoints(base.Pointer, nearPlanePoints);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000031B7 File Offset: 0x000013B7
		public void SetFovVertical(float verticalFov, float aspectRatioXY, float newDNear, float newDFar)
		{
			EngineApplicationInterface.ICamera.SetFovVertical(base.Pointer, verticalFov, aspectRatioXY, newDNear, newDFar);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000031CE File Offset: 0x000013CE
		public void SetFovHorizontal(float horizontalFov, float aspectRatioXY, float newDNear, float newDFar)
		{
			EngineApplicationInterface.ICamera.SetFovHorizontal(base.Pointer, horizontalFov, aspectRatioXY, newDNear, newDFar);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000031E5 File Offset: 0x000013E5
		public void GetViewProjMatrix(ref MatrixFrame viewProj)
		{
			EngineApplicationInterface.ICamera.GetViewProjMatrix(base.Pointer, ref viewProj);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000031F8 File Offset: 0x000013F8
		public float GetFovVertical()
		{
			return EngineApplicationInterface.ICamera.GetFovVertical(base.Pointer);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000320A File Offset: 0x0000140A
		public float GetFovHorizontal()
		{
			return EngineApplicationInterface.ICamera.GetFovHorizontal(base.Pointer);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000321C File Offset: 0x0000141C
		public float GetAspectRatio()
		{
			return EngineApplicationInterface.ICamera.GetAspectRatio(base.Pointer);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000322E File Offset: 0x0000142E
		public void FillParametersFrom(Camera otherCamera)
		{
			EngineApplicationInterface.ICamera.FillParametersFrom(base.Pointer, otherCamera.Pointer);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003246 File Offset: 0x00001446
		public void RenderFrustrum()
		{
			EngineApplicationInterface.ICamera.RenderFrustrum(base.Pointer);
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00003258 File Offset: 0x00001458
		// (set) Token: 0x06000062 RID: 98 RVA: 0x0000326A File Offset: 0x0000146A
		public GameEntity Entity
		{
			get
			{
				return EngineApplicationInterface.ICamera.GetEntity(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.ICamera.SetEntity(base.Pointer, value.Pointer);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00003282 File Offset: 0x00001482
		// (set) Token: 0x06000064 RID: 100 RVA: 0x0000328F File Offset: 0x0000148F
		public Vec3 Position
		{
			get
			{
				return this.Frame.origin;
			}
			set
			{
				EngineApplicationInterface.ICamera.SetPosition(base.Pointer, value);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000032A2 File Offset: 0x000014A2
		public Vec3 Direction
		{
			get
			{
				return -this.Frame.rotation.u;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000066 RID: 102 RVA: 0x000032BC File Offset: 0x000014BC
		// (set) Token: 0x06000067 RID: 103 RVA: 0x000032E4 File Offset: 0x000014E4
		public MatrixFrame Frame
		{
			get
			{
				MatrixFrame matrixFrame = default(MatrixFrame);
				EngineApplicationInterface.ICamera.GetFrame(base.Pointer, ref matrixFrame);
				return matrixFrame;
			}
			set
			{
				EngineApplicationInterface.ICamera.SetFrame(base.Pointer, ref value);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000068 RID: 104 RVA: 0x000032F8 File Offset: 0x000014F8
		public float Near
		{
			get
			{
				return EngineApplicationInterface.ICamera.GetNear(base.Pointer);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000069 RID: 105 RVA: 0x0000330A File Offset: 0x0000150A
		public float Far
		{
			get
			{
				return EngineApplicationInterface.ICamera.GetFar(base.Pointer);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600006A RID: 106 RVA: 0x0000331C File Offset: 0x0000151C
		public float HorizontalFov
		{
			get
			{
				return EngineApplicationInterface.ICamera.GetHorizontalFov(base.Pointer);
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000332E File Offset: 0x0000152E
		public void ViewportPointToWorldRay(ref Vec3 rayBegin, ref Vec3 rayEnd, Vec2 viewportPoint)
		{
			EngineApplicationInterface.ICamera.ViewportPointToWorldRay(base.Pointer, ref rayBegin, ref rayEnd, viewportPoint.ToVec3(0f));
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000334E File Offset: 0x0000154E
		public Vec3 WorldPointToViewPortPoint(ref Vec3 worldPoint)
		{
			return EngineApplicationInterface.ICamera.WorldPointToViewportPoint(base.Pointer, ref worldPoint);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003361 File Offset: 0x00001561
		public bool EnclosesPoint(Vec3 pointInWorldSpace)
		{
			return EngineApplicationInterface.ICamera.EnclosesPoint(base.Pointer, pointInWorldSpace);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003374 File Offset: 0x00001574
		public static MatrixFrame ConstructCameraFromPositionElevationBearing(Vec3 position, float elevation, float bearing)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.ICamera.ConstructCameraFromPositionElevationBearing(position, elevation, bearing, ref matrixFrame);
			return matrixFrame;
		}
	}
}
