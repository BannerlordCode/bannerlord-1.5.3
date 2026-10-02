using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000144 RID: 324
	public static class MBExtensions
	{
		// Token: 0x0600100B RID: 4107 RVA: 0x0002C634 File Offset: 0x0002A834
		private static Vec2 GetGlobalOrganicDirectionAux(ColumnFormation columnFormation, int depthCount = -1)
		{
			IEnumerable<Agent> unitsAtVanguardFile = columnFormation.GetUnitsAtVanguardFile<Agent>();
			Vec2 vec = Vec2.Zero;
			int num = 0;
			Agent agent = null;
			foreach (Agent agent2 in unitsAtVanguardFile)
			{
				if (agent != null)
				{
					Vec2 vec2 = (agent.Position - agent2.Position).AsVec2.Normalized();
					vec += vec2;
					num++;
				}
				agent = agent2;
				if (depthCount > 0 && num >= depthCount)
				{
					break;
				}
			}
			if (num == 0)
			{
				return Vec2.Invalid;
			}
			return vec * (1f / (float)num);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0002C6E0 File Offset: 0x0002A8E0
		public static Vec2 GetGlobalOrganicDirection(this ColumnFormation columnFormation)
		{
			return MBExtensions.GetGlobalOrganicDirectionAux(columnFormation, -1);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0002C6E9 File Offset: 0x0002A8E9
		public static Vec2 GetGlobalHeadDirection(this ColumnFormation columnFormation)
		{
			return MBExtensions.GetGlobalOrganicDirectionAux(columnFormation, 3);
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0002C6F2 File Offset: 0x0002A8F2
		public static IEnumerable<T> FindAllWithType<T>(this IEnumerable<GameEntity> entities) where T : ScriptComponentBehavior
		{
			return entities.SelectMany<GameEntity, T>((GameEntity e) => e.GetScriptComponents<T>());
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0002C71C File Offset: 0x0002A91C
		public static IEnumerable<T> FindAllWithType<T>(this IEnumerable<MissionObject> missionObjects) where T : MissionObject
		{
			return from e in missionObjects
				where e != null && e is T
				select e as T;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0002C774 File Offset: 0x0002A974
		public static List<GameEntity> FindAllWithCompatibleType(this IEnumerable<GameEntity> sceneProps, params Type[] types)
		{
			List<GameEntity> list = new List<GameEntity>();
			foreach (GameEntity gameEntity in sceneProps)
			{
				foreach (ScriptComponentBehavior scriptComponentBehavior in gameEntity.GetScriptComponents())
				{
					Type type = scriptComponentBehavior.GetType();
					for (int i = 0; i < types.Length; i++)
					{
						if (types[i].IsAssignableFrom(type))
						{
							list.Add(gameEntity);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0002C824 File Offset: 0x0002AA24
		public static List<MissionObject> FindAllWithCompatibleType(this IEnumerable<MissionObject> missionObjects, params Type[] types)
		{
			List<MissionObject> list = new List<MissionObject>();
			foreach (MissionObject missionObject in missionObjects)
			{
				if (missionObject != null)
				{
					Type type = missionObject.GetType();
					for (int i = 0; i < types.Length; i++)
					{
						if (types[i].IsAssignableFrom(type))
						{
							list.Add(missionObject);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0002C8A0 File Offset: 0x0002AAA0
		private static void CollectScriptComponentsIncludingChildrenAux<T>(GameEntity entity, MBList<T> list) where T : ScriptComponentBehavior
		{
			IEnumerable<T> scriptComponents = entity.GetScriptComponents<T>();
			list.AddRange(scriptComponents);
			foreach (GameEntity gameEntity in entity.GetChildren())
			{
				MBExtensions.CollectScriptComponentsIncludingChildrenAux<T>(gameEntity, list);
			}
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0002C8FC File Offset: 0x0002AAFC
		private static void CollectScriptComponentsIncludingChildrenAux<T>(WeakGameEntity entity, MBList<T> list) where T : ScriptComponentBehavior
		{
			list.AddRange(entity.GetScriptComponents<T>());
			foreach (WeakGameEntity weakGameEntity in entity.GetChildren())
			{
				MBExtensions.CollectScriptComponentsIncludingChildrenAux<T>(weakGameEntity, list);
			}
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0002C958 File Offset: 0x0002AB58
		public static MBList<T> CollectScriptComponentsIncludingChildrenRecursive<T>(this GameEntity entity) where T : ScriptComponentBehavior
		{
			MBList<T> mblist = new MBList<T>();
			MBExtensions.CollectScriptComponentsIncludingChildrenAux<T>(entity, mblist);
			return mblist;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0002C974 File Offset: 0x0002AB74
		public static MBList<T> CollectScriptComponentsIncludingChildrenRecursive<T>(this WeakGameEntity entity) where T : ScriptComponentBehavior
		{
			MBList<T> mblist = new MBList<T>();
			MBExtensions.CollectScriptComponentsIncludingChildrenAux<T>(entity, mblist);
			return mblist;
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0002C990 File Offset: 0x0002AB90
		public static List<T> CollectScriptComponentsWithTagIncludingChildrenRecursive<T>(this GameEntity entity, string tag) where T : ScriptComponentBehavior
		{
			List<T> list = new List<T>();
			foreach (GameEntity gameEntity in entity.GetChildren())
			{
				if (gameEntity.HasTag(tag))
				{
					IEnumerable<T> scriptComponents = gameEntity.GetScriptComponents<T>();
					list.AddRange(scriptComponents);
				}
				if (gameEntity.ChildCount > 0)
				{
					list.AddRange(gameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<T>(tag));
				}
			}
			return list;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0002CA0C File Offset: 0x0002AC0C
		public static List<T> CollectScriptComponentsWithTagIncludingChildrenRecursive<T>(this WeakGameEntity entity, string tag) where T : ScriptComponentBehavior
		{
			List<T> list = new List<T>();
			foreach (WeakGameEntity weakGameEntity in entity.GetChildren())
			{
				if (weakGameEntity.HasTag(tag))
				{
					list.AddRange(weakGameEntity.GetScriptComponents<T>());
				}
				if (weakGameEntity.ChildCount > 0)
				{
					list.AddRange(weakGameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<T>(tag));
				}
			}
			return list;
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0002CA88 File Offset: 0x0002AC88
		public static List<GameEntity> CollectChildrenEntitiesWithTag(this GameEntity entity, string tag)
		{
			List<GameEntity> list = new List<GameEntity>();
			foreach (GameEntity gameEntity in entity.GetChildren())
			{
				if (gameEntity.HasTag(tag))
				{
					list.Add(gameEntity);
				}
				if (gameEntity.ChildCount > 0)
				{
					list.AddRange(gameEntity.CollectChildrenEntitiesWithTag(tag));
				}
			}
			return list;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0002CAFC File Offset: 0x0002ACFC
		public static List<WeakGameEntity> CollectChildrenEntitiesWithTag(this WeakGameEntity entity, string tag)
		{
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			foreach (WeakGameEntity weakGameEntity in entity.GetChildren())
			{
				if (weakGameEntity.HasTag(tag))
				{
					list.Add(weakGameEntity);
				}
				if (weakGameEntity.ChildCount > 0)
				{
					list.AddRange(weakGameEntity.CollectChildrenEntitiesWithTag(tag));
				}
			}
			return list;
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0002CB74 File Offset: 0x0002AD74
		public static WeakGameEntity GetFirstChildEntityWithName(this WeakGameEntity entity, string name)
		{
			foreach (WeakGameEntity weakGameEntity in entity.GetChildren())
			{
				if (weakGameEntity.Name == name)
				{
					return weakGameEntity;
				}
			}
			return WeakGameEntity.Invalid;
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0002CBD8 File Offset: 0x0002ADD8
		public static T GetFirstScriptInFamilyDescending<T>(this GameEntity entity) where T : ScriptComponentBehavior
		{
			T t = entity.GetFirstScriptOfType<T>();
			if (t != null)
			{
				return t;
			}
			foreach (GameEntity gameEntity in entity.GetChildren())
			{
				t = gameEntity.GetFirstScriptInFamilyDescending<T>();
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0002CC4C File Offset: 0x0002AE4C
		public static T GetFirstScriptInFamilyDescending<T>(this WeakGameEntity entity) where T : ScriptComponentBehavior
		{
			T t = entity.GetFirstScriptOfType<T>();
			if (t != null)
			{
				return t;
			}
			foreach (WeakGameEntity weakGameEntity in entity.GetChildren())
			{
				t = weakGameEntity.GetFirstScriptInFamilyDescending<T>();
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0002CCC4 File Offset: 0x0002AEC4
		public static TSource ElementAtOrValue<TSource>(this IEnumerable<TSource> source, int index, TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (index >= 0)
			{
				IList<TSource> list = source as IList<TSource>;
				if (list == null)
				{
					using (IEnumerator<TSource> enumerator = source.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (index == 0)
							{
								return enumerator.Current;
							}
							index--;
						}
					}
					return value;
				}
				if (index < list.Count)
				{
					return list[index];
				}
			}
			return value;
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0002CD40 File Offset: 0x0002AF40
		public static bool IsOpponentOf(this BattleSideEnum s, BattleSideEnum side)
		{
			return (s == BattleSideEnum.Attacker && side == BattleSideEnum.Defender) || (s == BattleSideEnum.Defender && side == BattleSideEnum.Attacker);
		}
	}
}
