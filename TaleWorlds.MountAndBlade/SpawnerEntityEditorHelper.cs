using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200035F RID: 863
	public class SpawnerEntityEditorHelper
	{
		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060031BA RID: 12730 RVA: 0x000CA466 File Offset: 0x000C8666
		// (set) Token: 0x060031BB RID: 12731 RVA: 0x000CA46E File Offset: 0x000C866E
		public bool IsValid { get; private set; }

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060031BC RID: 12732 RVA: 0x000CA477 File Offset: 0x000C8677
		// (set) Token: 0x060031BD RID: 12733 RVA: 0x000CA47F File Offset: 0x000C867F
		public GameEntity SpawnedGhostEntity { get; private set; }

		// Token: 0x060031BE RID: 12734 RVA: 0x000CA488 File Offset: 0x000C8688
		public SpawnerEntityEditorHelper(ScriptComponentBehavior spawner)
		{
			this.spawner_ = spawner;
			if (this.AddGhostEntity(this.spawner_.GameEntity, this.GetGhostName()) != null)
			{
				this.SyncMatrixFrames(true);
				this.IsValid = true;
				return;
			}
			Debug.FailedAssert("No prefab found. Spawner script will remove itself.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\SpawnerEntityEditorHelper.cs", ".ctor", 75);
			spawner.GameEntity.RemoveScriptComponent(this.spawner_.ScriptComponent.Pointer, 11);
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x000CA530 File Offset: 0x000C8730
		public GameEntity GetGhostEntityOrChild(string name)
		{
			if (this.SpawnedGhostEntity.Name == name)
			{
				return this.SpawnedGhostEntity;
			}
			List<GameEntity> list = new List<GameEntity>();
			this.SpawnedGhostEntity.GetChildrenRecursive(ref list);
			GameEntity gameEntity = list.FirstOrDefault<GameEntity>((GameEntity x) => x.Name == name);
			if (gameEntity != null)
			{
				return gameEntity;
			}
			return null;
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x000CA59C File Offset: 0x000C879C
		public void Tick(float dt)
		{
			if (this.SpawnedGhostEntity.Parent != this.spawner_.GameEntity)
			{
				this.IsValid = false;
				this.spawner_.GameEntity.RemoveScriptComponent(this.spawner_.ScriptComponent.Pointer, 12);
			}
			if (this.IsValid)
			{
				if (this.LockGhostParent)
				{
					MatrixFrame frame = this.SpawnedGhostEntity.GetFrame();
					MatrixFrame identity = MatrixFrame.Identity;
					bool flag = (in frame) != (in identity);
					MatrixFrame identity2 = MatrixFrame.Identity;
					this.SpawnedGhostEntity.SetFrame(ref identity2, true);
					if (flag)
					{
						this.SpawnedGhostEntity.UpdateTriadFrameForEditor();
					}
				}
				this.SyncMatrixFrames(false);
				if (this._ghostMovementMode)
				{
					this.UpdateGhostMovement(dt);
				}
			}
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x000CA654 File Offset: 0x000C8854
		public void GivePermission(string childName, SpawnerEntityEditorHelper.Permission permission, Action<float> onChangeFunction)
		{
			this._stableChildrenPermissions.Add(Tuple.Create<string, SpawnerEntityEditorHelper.Permission, Action<float>>(childName, permission, onChangeFunction));
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x000CA66C File Offset: 0x000C886C
		private void ApplyPermissions()
		{
			using (List<Tuple<string, SpawnerEntityEditorHelper.Permission, Action<float>>>.Enumerator enumerator = this._stableChildrenPermissions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Tuple<string, SpawnerEntityEditorHelper.Permission, Action<float>> item = enumerator.Current;
					KeyValuePair<string, MatrixFrame> keyValuePair = this.stableChildrenFrames.Find((KeyValuePair<string, MatrixFrame> x) => x.Key == item.Item1);
					MatrixFrame frame = this.GetGhostEntityOrChild(item.Item1).GetFrame();
					if (!frame.NearlyEquals(keyValuePair.Value, 1E-05f))
					{
						SpawnerEntityEditorHelper.PermissionType typeOfPermission = item.Item2.TypeOfPermission;
						if (typeOfPermission != SpawnerEntityEditorHelper.PermissionType.scale)
						{
							if (typeOfPermission == SpawnerEntityEditorHelper.PermissionType.rotation)
							{
								switch (item.Item2.PermittedAxis)
								{
								case SpawnerEntityEditorHelper.Axis.x:
								{
									MatrixFrame matrixFrame = keyValuePair.Value;
									if (!frame.rotation.f.NearlyEquals(in matrixFrame.rotation.f, 1E-05f))
									{
										MatrixFrame matrixFrame2 = keyValuePair.Value;
										if (!frame.rotation.u.NearlyEquals(in matrixFrame2.rotation.u, 1E-05f))
										{
											MatrixFrame matrixFrame3 = keyValuePair.Value;
											if (frame.rotation.s.NearlyEquals(in matrixFrame3.rotation.s, 1E-05f))
											{
												this.ChangeStableChildMatrixFrame(item.Item1, frame);
												item.Item3(frame.rotation.GetEulerAngles().x);
											}
										}
									}
									break;
								}
								case SpawnerEntityEditorHelper.Axis.y:
								{
									MatrixFrame matrixFrame = keyValuePair.Value;
									if (!frame.rotation.s.NearlyEquals(in matrixFrame.rotation.s, 1E-05f))
									{
										MatrixFrame matrixFrame2 = keyValuePair.Value;
										if (!frame.rotation.u.NearlyEquals(in matrixFrame2.rotation.u, 1E-05f))
										{
											MatrixFrame matrixFrame3 = keyValuePair.Value;
											if (frame.rotation.f.NearlyEquals(in matrixFrame3.rotation.f, 1E-05f))
											{
												this.ChangeStableChildMatrixFrame(item.Item1, frame);
												item.Item3(frame.rotation.GetEulerAngles().y);
											}
										}
									}
									break;
								}
								case SpawnerEntityEditorHelper.Axis.z:
								{
									MatrixFrame matrixFrame = keyValuePair.Value;
									if (!frame.rotation.f.NearlyEquals(in matrixFrame.rotation.f, 1E-05f))
									{
										MatrixFrame matrixFrame2 = keyValuePair.Value;
										if (!frame.rotation.s.NearlyEquals(in matrixFrame2.rotation.s, 1E-05f))
										{
											MatrixFrame matrixFrame3 = keyValuePair.Value;
											if (frame.rotation.u.NearlyEquals(in matrixFrame3.rotation.u, 1E-05f))
											{
												this.ChangeStableChildMatrixFrame(item.Item1, frame);
												item.Item3(frame.rotation.GetEulerAngles().z);
											}
										}
									}
									break;
								}
								}
							}
						}
						else
						{
							MatrixFrame matrixFrame = keyValuePair.Value;
							if (frame.origin.NearlyEquals(in matrixFrame.origin, 0.0001f))
							{
								Vec3 vec = frame.rotation.f.NormalizedCopy();
								MatrixFrame matrixFrame2 = keyValuePair.Value;
								Vec3 vec2 = matrixFrame2.rotation.f.NormalizedCopy();
								if (vec.NearlyEquals(in vec2, 0.0001f))
								{
									vec = frame.rotation.u.NormalizedCopy();
									matrixFrame2 = keyValuePair.Value;
									Vec3 vec3 = matrixFrame2.rotation.u.NormalizedCopy();
									if (vec.NearlyEquals(in vec3, 0.0001f))
									{
										vec = frame.rotation.s.NormalizedCopy();
										matrixFrame2 = keyValuePair.Value;
										Vec3 vec4 = matrixFrame2.rotation.s.NormalizedCopy();
										if (vec.NearlyEquals(in vec4, 0.0001f))
										{
											switch (item.Item2.PermittedAxis)
											{
											case SpawnerEntityEditorHelper.Axis.x:
												matrixFrame = keyValuePair.Value;
												if (!frame.rotation.f.NearlyEquals(in matrixFrame.rotation.f, 1E-05f))
												{
													this.ChangeStableChildMatrixFrame(item.Item1, frame);
													item.Item3(frame.rotation.f.Length);
												}
												break;
											case SpawnerEntityEditorHelper.Axis.y:
												matrixFrame = keyValuePair.Value;
												if (!frame.rotation.s.NearlyEquals(in matrixFrame.rotation.s, 1E-05f))
												{
													this.ChangeStableChildMatrixFrame(item.Item1, frame);
													item.Item3(frame.rotation.s.Length);
												}
												break;
											case SpawnerEntityEditorHelper.Axis.z:
												matrixFrame = keyValuePair.Value;
												if (!frame.rotation.u.NearlyEquals(in matrixFrame.rotation.u, 1E-05f))
												{
													this.ChangeStableChildMatrixFrame(item.Item1, frame);
													item.Item3(frame.rotation.u.Length);
												}
												break;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x000CAC04 File Offset: 0x000C8E04
		private void ChangeStableChildMatrixFrame(string childName, MatrixFrame matrixFrame)
		{
			this.stableChildrenFrames.RemoveAll((KeyValuePair<string, MatrixFrame> x) => x.Key == childName);
			KeyValuePair<string, MatrixFrame> keyValuePair = new KeyValuePair<string, MatrixFrame>(childName, matrixFrame);
			this.stableChildrenFrames.Add(keyValuePair);
			if (SpawnerEntityEditorHelper.HasField(this.spawner_, childName, true))
			{
				SpawnerEntityEditorHelper.SetSpawnerMatrixFrame(this.spawner_, childName, matrixFrame);
			}
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x000CAC77 File Offset: 0x000C8E77
		public void ChangeStableChildMatrixFrameAndApply(string childName, MatrixFrame matrixFrame, bool updateTriad = true)
		{
			this.ChangeStableChildMatrixFrame(childName, matrixFrame);
			this.GetGhostEntityOrChild(childName).SetFrame(ref matrixFrame, true);
			if (updateTriad)
			{
				this.SpawnedGhostEntity.UpdateTriadFrameForEditorForAllChildren();
			}
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x000CACA0 File Offset: 0x000C8EA0
		private GameEntity AddGhostEntity(WeakGameEntity parent, List<string> possibleEntityNames)
		{
			this.spawner_.GameEntity.RemoveAllChildren();
			foreach (string text in possibleEntityNames)
			{
				if (GameEntity.PrefabExists(text))
				{
					this.SpawnedGhostEntity = GameEntity.Instantiate(parent.Scene, text, true, true, "");
					break;
				}
			}
			if (this.SpawnedGhostEntity == null)
			{
				return null;
			}
			this.SpawnedGhostEntity.SetMobility(GameEntity.Mobility.Dynamic);
			this.SpawnedGhostEntity.EntityFlags |= EntityFlags.DontSaveToScene;
			parent.AddChild(this.SpawnedGhostEntity.WeakEntity, false);
			MatrixFrame identity = MatrixFrame.Identity;
			this.SpawnedGhostEntity.SetFrame(ref identity, true);
			this.GetChildrenInitialFrames();
			this.SpawnedGhostEntity.UpdateTriadFrameForEditorForAllChildren();
			return this.SpawnedGhostEntity;
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x000CAD90 File Offset: 0x000C8F90
		private void SyncMatrixFrames(bool first)
		{
			this.ApplyPermissions();
			List<GameEntity> list = new List<GameEntity>();
			this.SpawnedGhostEntity.GetChildrenRecursive(ref list);
			using (List<GameEntity>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameEntity item = enumerator.Current;
					if (SpawnerEntityEditorHelper.HasField(this.spawner_, item.Name, false))
					{
						if (first)
						{
							MatrixFrame matrixFrame = (MatrixFrame)SpawnerEntityEditorHelper.GetFieldValue(this.spawner_, item.Name);
							if (!matrixFrame.IsZero)
							{
								item.SetFrame(ref matrixFrame, true);
							}
						}
						else
						{
							SpawnerEntityEditorHelper.SetSpawnerMatrixFrame(this.spawner_, item.Name, item.GetFrame());
						}
					}
					else
					{
						MatrixFrame value = this.stableChildrenFrames.Find((KeyValuePair<string, MatrixFrame> x) => x.Key == item.Name).Value;
						if (!value.NearlyEquals(item.GetFrame(), 1E-05f))
						{
							item.SetFrame(ref value, true);
							this.SpawnedGhostEntity.UpdateTriadFrameForEditorForAllChildren();
						}
					}
				}
			}
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x000CAED0 File Offset: 0x000C90D0
		private void GetChildrenInitialFrames()
		{
			List<GameEntity> list = new List<GameEntity>();
			this.SpawnedGhostEntity.GetChildrenRecursive(ref list);
			foreach (GameEntity gameEntity in list)
			{
				if (!SpawnerEntityEditorHelper.HasField(this.spawner_, gameEntity.Name, false))
				{
					this.stableChildrenFrames.Add(new KeyValuePair<string, MatrixFrame>(gameEntity.Name, gameEntity.GetFrame()));
				}
			}
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x000CAF5C File Offset: 0x000C915C
		private List<string> GetGhostName()
		{
			string text = this.GetPrefabName();
			List<string> list = new List<string>();
			list.Add(text + "_ghost");
			text = text.Remove(text.Length - text.Split(new char[] { '_' }).Last<string>().Length - 1);
			list.Add(text + "_ghost");
			return list;
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x000CAFC4 File Offset: 0x000C91C4
		public string GetPrefabName()
		{
			return this.spawner_.GameEntity.Name.Remove(this.spawner_.GameEntity.Name.Length - this.spawner_.GameEntity.Name.Split(new char[] { '_' }).Last<string>().Length - 1);
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x000CB034 File Offset: 0x000C9234
		public void SetupGhostMovement(string pathName)
		{
			this._ghostMovementMode = true;
			this._pathName = pathName;
			Path pathWithName = this.SpawnedGhostEntity.Scene.GetPathWithName(pathName);
			Vec3 scaleVector = this.SpawnedGhostEntity.GetFrame().rotation.GetScaleVector();
			this._tracker = new PathTracker(pathWithName, scaleVector);
			this._ghostObjectPosition = ((pathWithName != null) ? pathWithName.GetTotalLength() : 0f);
			this.SpawnedGhostEntity.UpdateTriadFrameForEditor();
			List<GameEntity> list = new List<GameEntity>();
			this.SpawnedGhostEntity.GetChildrenRecursive(ref list);
			this._wheels.Clear();
			this._wheels.AddRange(list.Where<GameEntity>((GameEntity x) => x.HasTag("wheel")));
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x000CB0FC File Offset: 0x000C92FC
		public void SetEnableAutoGhostMovement(bool enableAutoGhostMovement)
		{
			this._enableAutoGhostMovement = enableAutoGhostMovement;
			if (!this._enableAutoGhostMovement && this._tracker.IsValid)
			{
				this._ghostObjectPosition = this._tracker.GetPathLength();
			}
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x000CB12C File Offset: 0x000C932C
		private void UpdateGhostMovement(float dt)
		{
			if (this._tracker.HasChanged)
			{
				this.SetupGhostMovement(this._pathName);
				this._tracker.Advance(this._tracker.GetPathLength());
			}
			if (this.spawner_.GameEntity.IsSelectedOnEditor() || this.SpawnedGhostEntity.IsSelectedOnEditor())
			{
				if (this._tracker.IsValid)
				{
					float num = 10f;
					if (Input.DebugInput.IsShiftDown())
					{
						num = 1f;
					}
					if (Input.DebugInput.IsKeyDown(InputKey.MouseScrollUp))
					{
						this._ghostObjectPosition += dt * num;
					}
					else if (Input.DebugInput.IsKeyDown(InputKey.MouseScrollDown))
					{
						this._ghostObjectPosition -= dt * num;
					}
					if (this._enableAutoGhostMovement)
					{
						this._ghostObjectPosition += dt * num;
						if (this._ghostObjectPosition >= this._tracker.GetPathLength())
						{
							this._ghostObjectPosition = 0f;
						}
					}
					this._ghostObjectPosition = MBMath.ClampFloat(this._ghostObjectPosition, 0f, this._tracker.GetPathLength());
				}
				else
				{
					this._ghostObjectPosition = 0f;
				}
			}
			if (this._tracker.IsValid)
			{
				MatrixFrame globalFrame = this.spawner_.GameEntity.GetGlobalFrame();
				this._tracker.Advance(0f);
				MatrixFrame matrixFrame;
				Vec3 vec;
				this._tracker.CurrentFrameAndColor(out matrixFrame, out vec);
				if ((in globalFrame) != (in matrixFrame))
				{
					this.spawner_.GameEntity.SetGlobalFrame(in matrixFrame, true);
					this.spawner_.GameEntity.UpdateTriadFrameForEditor();
				}
				this._tracker.Advance(this._ghostObjectPosition);
				this._tracker.CurrentFrameAndColor(out matrixFrame, out vec);
				if (this._wheels.Count == 2)
				{
					matrixFrame = this.LinearInterpolatedIK(ref this._tracker);
				}
				if ((in globalFrame) != (in matrixFrame))
				{
					this.SpawnedGhostEntity.SetGlobalFrame(in matrixFrame, true);
					this.SpawnedGhostEntity.UpdateTriadFrameForEditor();
				}
				this._tracker.Reset();
				return;
			}
			MatrixFrame matrixFrame2 = this.SpawnedGhostEntity.GetGlobalFrame();
			MatrixFrame globalFrame2 = this.spawner_.GameEntity.GetGlobalFrame();
			if ((in matrixFrame2) != (in globalFrame2))
			{
				GameEntity spawnedGhostEntity = this.SpawnedGhostEntity;
				matrixFrame2 = this.spawner_.GameEntity.GetGlobalFrame();
				spawnedGhostEntity.SetGlobalFrame(in matrixFrame2, true);
				this.SpawnedGhostEntity.UpdateTriadFrameForEditor();
			}
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x000CB39C File Offset: 0x000C959C
		private MatrixFrame LinearInterpolatedIK(ref PathTracker pathTracker)
		{
			MatrixFrame matrixFrame;
			Vec3 vec;
			pathTracker.CurrentFrameAndColor(out matrixFrame, out vec);
			MatrixFrame matrixFrame2 = SiegeWeaponMovementComponent.FindGroundFrameForWheelsStatic(ref matrixFrame, 2.45f, 1.3f, this.SpawnedGhostEntity.WeakEntity, this._wheels, this.SpawnedGhostEntity.Scene);
			return MatrixFrame.Lerp(in matrixFrame, in matrixFrame2, vec.x);
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x000CB3F1 File Offset: 0x000C95F1
		private static object GetFieldValue(object src, string propName)
		{
			return src.GetType().GetField(propName).GetValue(src);
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x000CB405 File Offset: 0x000C9605
		private static bool HasField(object obj, string propertyName, bool findRestricted)
		{
			return obj.GetType().GetField(propertyName) != null && (findRestricted || obj.GetType().GetField(propertyName).GetCustomAttribute<RestrictedAccess>() == null);
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x000CB438 File Offset: 0x000C9638
		private static bool SetSpawnerMatrixFrame(object target, string propertyName, MatrixFrame value)
		{
			value.Fill();
			FieldInfo field = target.GetType().GetField(propertyName);
			if (field != null)
			{
				field.SetValue(target, value);
				return true;
			}
			return false;
		}

		// Token: 0x040014F4 RID: 5364
		private List<Tuple<string, SpawnerEntityEditorHelper.Permission, Action<float>>> _stableChildrenPermissions = new List<Tuple<string, SpawnerEntityEditorHelper.Permission, Action<float>>>();

		// Token: 0x040014F5 RID: 5365
		private ScriptComponentBehavior spawner_;

		// Token: 0x040014F6 RID: 5366
		private List<KeyValuePair<string, MatrixFrame>> stableChildrenFrames = new List<KeyValuePair<string, MatrixFrame>>();

		// Token: 0x040014F9 RID: 5369
		public bool LockGhostParent = true;

		// Token: 0x040014FA RID: 5370
		private bool _ghostMovementMode;

		// Token: 0x040014FB RID: 5371
		private PathTracker _tracker;

		// Token: 0x040014FC RID: 5372
		private float _ghostObjectPosition;

		// Token: 0x040014FD RID: 5373
		private string _pathName;

		// Token: 0x040014FE RID: 5374
		private bool _enableAutoGhostMovement;

		// Token: 0x040014FF RID: 5375
		private readonly List<GameEntity> _wheels = new List<GameEntity>();

		// Token: 0x0200063F RID: 1599
		public enum Axis
		{
			// Token: 0x0400216D RID: 8557
			x,
			// Token: 0x0400216E RID: 8558
			y,
			// Token: 0x0400216F RID: 8559
			z
		}

		// Token: 0x02000640 RID: 1600
		public enum PermissionType
		{
			// Token: 0x04002171 RID: 8561
			scale,
			// Token: 0x04002172 RID: 8562
			rotation
		}

		// Token: 0x02000641 RID: 1601
		public struct Permission
		{
			// Token: 0x060040F7 RID: 16631 RVA: 0x000FC96F File Offset: 0x000FAB6F
			public Permission(SpawnerEntityEditorHelper.PermissionType permission, SpawnerEntityEditorHelper.Axis axis)
			{
				this.TypeOfPermission = permission;
				this.PermittedAxis = axis;
			}

			// Token: 0x04002173 RID: 8563
			public SpawnerEntityEditorHelper.PermissionType TypeOfPermission;

			// Token: 0x04002174 RID: 8564
			public SpawnerEntityEditorHelper.Axis PermittedAxis;
		}
	}
}
