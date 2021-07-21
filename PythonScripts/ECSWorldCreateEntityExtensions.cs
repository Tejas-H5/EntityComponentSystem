
//This code was auto-generated with a python script. I hope you didn't think I would type all this out by hand
namespace ECS
{
	public partial class ECSWorld
	{
		public int CreateEntity<T0>(T0 c0)
			where T0 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1>(T0 c0, T1 c1)
			where T0 : struct where T1 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2>(T0 c0, T1 c1, T2 c2)
			where T0 : struct where T1 : struct where T2 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3>(T0 c0, T1 c1, T2 c2, T3 c3)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6, T7>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6, T7 c7)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			addComponentNoEvent(entity, c7);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6, T7 c7, T8 c8)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			addComponentNoEvent(entity, c7);
			addComponentNoEvent(entity, c8);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6, T7 c7, T8 c8, T9 c9)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			addComponentNoEvent(entity, c7);
			addComponentNoEvent(entity, c8);
			addComponentNoEvent(entity, c9);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6, T7 c7, T8 c8, T9 c9, T10 c10)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			addComponentNoEvent(entity, c7);
			addComponentNoEvent(entity, c8);
			addComponentNoEvent(entity, c9);
			addComponentNoEvent(entity, c10);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6, T7 c7, T8 c8, T9 c9, T10 c10, T11 c11)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			addComponentNoEvent(entity, c7);
			addComponentNoEvent(entity, c8);
			addComponentNoEvent(entity, c9);
			addComponentNoEvent(entity, c10);
			addComponentNoEvent(entity, c11);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6, T7 c7, T8 c8, T9 c9, T10 c10, T11 c11, T12 c12)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			addComponentNoEvent(entity, c7);
			addComponentNoEvent(entity, c8);
			addComponentNoEvent(entity, c9);
			addComponentNoEvent(entity, c10);
			addComponentNoEvent(entity, c11);
			addComponentNoEvent(entity, c12);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6, T7 c7, T8 c8, T9 c9, T10 c10, T11 c11, T12 c12, T13 c13)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			addComponentNoEvent(entity, c7);
			addComponentNoEvent(entity, c8);
			addComponentNoEvent(entity, c9);
			addComponentNoEvent(entity, c10);
			addComponentNoEvent(entity, c11);
			addComponentNoEvent(entity, c12);
			addComponentNoEvent(entity, c13);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6, T7 c7, T8 c8, T9 c9, T10 c10, T11 c11, T12 c12, T13 c13, T14 c14)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			addComponentNoEvent(entity, c7);
			addComponentNoEvent(entity, c8);
			addComponentNoEvent(entity, c9);
			addComponentNoEvent(entity, c10);
			addComponentNoEvent(entity, c11);
			addComponentNoEvent(entity, c12);
			addComponentNoEvent(entity, c13);
			addComponentNoEvent(entity, c14);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6, T7 c7, T8 c8, T9 c9, T10 c10, T11 c11, T12 c12, T13 c13, T14 c14, T15 c15)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct where T15 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			addComponentNoEvent(entity, c7);
			addComponentNoEvent(entity, c8);
			addComponentNoEvent(entity, c9);
			addComponentNoEvent(entity, c10);
			addComponentNoEvent(entity, c11);
			addComponentNoEvent(entity, c12);
			addComponentNoEvent(entity, c13);
			addComponentNoEvent(entity, c14);
			addComponentNoEvent(entity, c15);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6, T7 c7, T8 c8, T9 c9, T10 c10, T11 c11, T12 c12, T13 c13, T14 c14, T15 c15, T16 c16)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct where T15 : struct where T16 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			addComponentNoEvent(entity, c7);
			addComponentNoEvent(entity, c8);
			addComponentNoEvent(entity, c9);
			addComponentNoEvent(entity, c10);
			addComponentNoEvent(entity, c11);
			addComponentNoEvent(entity, c12);
			addComponentNoEvent(entity, c13);
			addComponentNoEvent(entity, c14);
			addComponentNoEvent(entity, c15);
			addComponentNoEvent(entity, c16);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6, T7 c7, T8 c8, T9 c9, T10 c10, T11 c11, T12 c12, T13 c13, T14 c14, T15 c15, T16 c16, T17 c17)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct where T15 : struct where T16 : struct where T17 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			addComponentNoEvent(entity, c7);
			addComponentNoEvent(entity, c8);
			addComponentNoEvent(entity, c9);
			addComponentNoEvent(entity, c10);
			addComponentNoEvent(entity, c11);
			addComponentNoEvent(entity, c12);
			addComponentNoEvent(entity, c13);
			addComponentNoEvent(entity, c14);
			addComponentNoEvent(entity, c15);
			addComponentNoEvent(entity, c16);
			addComponentNoEvent(entity, c17);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6, T7 c7, T8 c8, T9 c9, T10 c10, T11 c11, T12 c12, T13 c13, T14 c14, T15 c15, T16 c16, T17 c17, T18 c18)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct where T15 : struct where T16 : struct where T17 : struct where T18 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			addComponentNoEvent(entity, c7);
			addComponentNoEvent(entity, c8);
			addComponentNoEvent(entity, c9);
			addComponentNoEvent(entity, c10);
			addComponentNoEvent(entity, c11);
			addComponentNoEvent(entity, c12);
			addComponentNoEvent(entity, c13);
			addComponentNoEvent(entity, c14);
			addComponentNoEvent(entity, c15);
			addComponentNoEvent(entity, c16);
			addComponentNoEvent(entity, c17);
			addComponentNoEvent(entity, c18);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
		
		public int CreateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T0 c0, T1 c1, T2 c2, T3 c3, T4 c4, T5 c5, T6 c6, T7 c7, T8 c8, T9 c9, T10 c10, T11 c11, T12 c12, T13 c13, T14 c14, T15 c15, T16 c16, T17 c17, T18 c18, T19 c19)
			where T0 : struct where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct where T15 : struct where T16 : struct where T17 : struct where T18 : struct where T19 : struct 
		{
			int entity = CreateEntity();
			
			addComponentNoEvent(entity, c0);
			addComponentNoEvent(entity, c1);
			addComponentNoEvent(entity, c2);
			addComponentNoEvent(entity, c3);
			addComponentNoEvent(entity, c4);
			addComponentNoEvent(entity, c5);
			addComponentNoEvent(entity, c6);
			addComponentNoEvent(entity, c7);
			addComponentNoEvent(entity, c8);
			addComponentNoEvent(entity, c9);
			addComponentNoEvent(entity, c10);
			addComponentNoEvent(entity, c11);
			addComponentNoEvent(entity, c12);
			addComponentNoEvent(entity, c13);
			addComponentNoEvent(entity, c14);
			addComponentNoEvent(entity, c15);
			addComponentNoEvent(entity, c16);
			addComponentNoEvent(entity, c17);
			addComponentNoEvent(entity, c18);
			addComponentNoEvent(entity, c19);
			
			invokeEntityCreatedEvent(entity);
			
			return entity;
		}
	}
}
