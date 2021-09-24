using BenchmarkDotNet.Attributes;
using Common;
using ECS.Components;
using ECS.CustomDataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace IterationPatternBenchmarking.Benchmarks
{
    interface IGenericList
    {
    }

    struct CompTypeIndexPair
    {
        public int ComponentType;
        public int ComponentID;

        public CompTypeIndexPair(int componentType, int componentID)
        {
            ComponentType = componentType;
            ComponentID = componentID;
        }
    }

    class ListObject<T> : IGenericList where T : struct
    {
        private MutableList<Component<T>> list = new MutableList<Component<T>>();
        public ref T this[int index] {
            get {
                return ref list[index].Data;
            }
        }

        public ListObject(MutableList<Component<T>> list)
        {
            this.list = list;
        }
    }


    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class IterationPatternBenchmarks
    {
        public const int NUMELEMENTS = 10000;
        public const float DELTATIME = 1f / 60f;

        MutableList<Acceleration> accels;
        MutableList<Velocity> velocities;
        MutableList<Position> positions;

        MutableList<Component<Acceleration>> accelComponents;
        MutableList<Component<Velocity>> velocityComponents;
        MutableList<Component<Position>> positionComponents;

        List<IGenericList> componentDoublePointer = new List<IGenericList>();

        List<MutableList<CompTypeIndexPair>> entityMap = new List<MutableList<CompTypeIndexPair>>();

        int[] selectedComponentIDs = new int[3];
        int[] compIDs = new int[3];

        public IterationPatternBenchmarks()
        {
            InitPureStructArrays();

            InitComponentStructArrays();

            InitNestedComponentStructArrays();

            selectedComponentIDs[0] = RegisteredComponents.LookupTypeID(typeof(Acceleration));
            selectedComponentIDs[1] = RegisteredComponents.LookupTypeID(typeof(Velocity));
            selectedComponentIDs[2] = RegisteredComponents.LookupTypeID(typeof(Position));

            for (int i = 0; i < NUMELEMENTS; i++)
            {
                MutableList<CompTypeIndexPair> components = new MutableList<CompTypeIndexPair>(3);
                components.Add(new CompTypeIndexPair(selectedComponentIDs[0], (int)i));
                components.Add(new CompTypeIndexPair(selectedComponentIDs[1], (int)i));
                components.Add(new CompTypeIndexPair(selectedComponentIDs[2], (int)i));

                entityMap.Add(components);
            }
        }

        private void InitPureStructArrays()
        {
            accels = createDefaultList(NUMELEMENTS, new Acceleration());
            velocities = createDefaultList(NUMELEMENTS, new Velocity(0.5f, 0));
            positions = createDefaultList(NUMELEMENTS, new Position());
        }

        private static MutableList<T> createDefaultList<T>(int numItems, T baseElement) where T : struct
        {
            MutableList<T> list = new MutableList<T>(numItems);
            for (int i = 0; i < numItems; i++)
            {
                list.Add(baseElement);
            }
            return list;
        }

        private void InitComponentStructArrays()
        {
            accelComponents = createDefaultList(NUMELEMENTS, new Component<Acceleration>());
            velocityComponents = createDefaultList(NUMELEMENTS, new Component<Velocity>());
            positionComponents = createDefaultList(NUMELEMENTS, new Component<Position>());
            for(int i = 0; i < positionComponents.Count; i++)
            {
                accelComponents[i].EntityID = i;
                velocityComponents[i].EntityID = i;
                positionComponents[i].EntityID = i;

                positionComponents[i].Data.X = 0.5f;
            }
        }


        private void InitNestedComponentStructArrays()
        {
            var accelComponents2 = createDefaultList(NUMELEMENTS, new Component<Acceleration>());
            var velocityComponents2 = createDefaultList(NUMELEMENTS, new Component<Velocity>());
            var positionComponents2 = createDefaultList(NUMELEMENTS, new Component<Position>());
            for (int i = 0; i < positionComponents2.Count; i++)
            {
                positionComponents2[i].Data.X = 0.5f;
            }

            componentDoublePointer.Add(new ListObject<Acceleration>(accelComponents2));
            componentDoublePointer.Add(new ListObject<Velocity>(velocityComponents2));
            componentDoublePointer.Add(new ListObject<Position>(positionComponents2));
        }

        void verlet(ref Acceleration acc, ref Velocity vel, ref Position pos)
        {
            float halfDelta = DELTATIME * 0.5f;
            pos.X += vel.X * halfDelta;
            pos.Y += vel.Y * halfDelta;

            vel.X += acc.X * DELTATIME;
            vel.Y += acc.Y * DELTATIME;

            pos.X += vel.X * halfDelta;
            pos.Y += vel.Y * halfDelta;
        }


        //[Benchmark]
        public void IteratePureStructs()
        {
            for(int i = 0; i < NUMELEMENTS; i++)
            {
                verlet(ref accels[i], ref velocities[i], ref positions[i]);
            }
        }


        //[Benchmark]
        public void IterateComponentWrappedStructs()
        {
            for (int i = 0; i < NUMELEMENTS; i++)
            {
                verlet(ref accelComponents[i].Data, ref velocityComponents[i].Data, ref positionComponents[i].Data);
            }
        }


        //[Benchmark]
        public void IterateNestedComponentWrappedStructsVer1()
        {
            ListObject<Acceleration> accelList = (ListObject<Acceleration>)componentDoublePointer[0];
            ListObject<Velocity> velocityList = (ListObject<Velocity>)componentDoublePointer[1];
            ListObject<Position> posList = (ListObject<Position>)componentDoublePointer[2];

            for (int i = 0; i < NUMELEMENTS; i++)
            {
                verlet(ref accelList[i], ref velocityList[i], ref posList[i]);
            }
        }


        //[Benchmark]
        public void IterateNestedComponentWrappedStructsVer2()
        {
            for (int i = 0; i < NUMELEMENTS; i++)
            {
                ListObject<Acceleration> accelList = (ListObject<Acceleration>)componentDoublePointer[0];
                ListObject<Velocity> velocityList = (ListObject<Velocity>)componentDoublePointer[1];
                ListObject<Position> posList = (ListObject<Position>)componentDoublePointer[2];
                verlet(ref accelList[i], ref velocityList[i], ref posList[i]);
            }
        }

        ref T GetComponent<T>(int ComponentID, int initOrder) where T :unmanaged
        {
            return ref ((ListObject<T>)componentDoublePointer[initOrder])[ComponentID];
        }

        //[Benchmark]
        public void IterateNestedComponentWrappedStructsVer3()
        {
            for (int i = 0; i < NUMELEMENTS; i++)
            {
                verlet(
                    ref GetComponent<Acceleration>(i, 0),
                    ref GetComponent<Velocity>(i, 1),
                    ref GetComponent<Position>(i, 2)
                );
            }
        }

        //[Benchmark]
        public void IterateNestedComponentWrappedStructsThroughComponentIDArray()
        {
            for (int i = 0; i < NUMELEMENTS; i++)
            {
                compIDs[0] = i;
                compIDs[1] = i;
                compIDs[2] = i;

                Iterate();
            }
        }

        private void Iterate()
        {
            verlet(
                ref GetComponent<Acceleration>(compIDs[0], 0),
                ref GetComponent<Velocity>(compIDs[1], 1),
                ref GetComponent<Position>(compIDs[2], 2)
            );
        }



        [Benchmark]
        public void ECSLikeIteration()
        {
            int shortestComponent = 1;
            for (int i = 0; i < NUMELEMENTS; i++)
            {
                compIDs[shortestComponent] = i;
                MutableList<CompTypeIndexPair> entityComponents = entityMap[(int)i];

                int found = 1;

                for(int j = 0; j < entityComponents.Count; j++)
                {
                    if (entityComponents[j].ComponentType == shortestComponent)
                        continue;

                    for(int k = 0; k < selectedComponentIDs.Length; k++)
                    {
                        if(entityComponents[j].ComponentType == selectedComponentIDs[k])
                        {
                            compIDs[k] = entityComponents[j].ComponentID;
                            found++;

                            if (found == selectedComponentIDs.Length)
                                break;
                        }
                    }

                    if (found == selectedComponentIDs.Length)
                        break;
                }

                if(found == selectedComponentIDs.Length)
                {
                    Iterate();
                }
            }
        }


        [Benchmark]
        public void ECSLikeIterationWithPtrs()
        {
            int shortestComponent = 1;
            for (int i = 0; i < NUMELEMENTS; i++)
            {
                compIDs[shortestComponent] = i;
                MutableList<CompTypeIndexPair> entityComponents = entityMap[i];

                int found = 1;

                for (int j = 0; j < entityComponents.Count; j++)
                {
                    if (entityComponents[j].ComponentType == shortestComponent)
                        continue;

                    for (int k = 0; k < selectedComponentIDs.Length; k++)
                    {
                        if (entityComponents[j].ComponentType == selectedComponentIDs[k])
                        {
                            compIDs[k] = entityComponents[j].ComponentID;
                            found++;

                            if (found == selectedComponentIDs.Length)
                                break;
                        }
                    }

                    if (found == selectedComponentIDs.Length)
                        break;
                }

                if (found == selectedComponentIDs.Length)
                {
                    Iterate();
                }
            }
        }
    }
}
