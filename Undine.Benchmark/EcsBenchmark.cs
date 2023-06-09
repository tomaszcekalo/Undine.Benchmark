﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Undine.Audrey;
using Undine.Benchmark.Components;
using Undine.Benchmark.Systems;
using Undine.Core;
using Undine.DefaultEcs;
using Undine.Entitas;
using Undine.LazyECS;
using Undine.LeopotamEcs;
using Undine.MinEcs;
using Undine.MonoGame;
using Undine.Simplecs;

namespace Undine.Benchmark
{
    [SimpleJob(RuntimeMoniker.Net70)]
    [MemoryDiagnoser]
    public class EcsBenchmark
    {
        private LeopotamEcsContainer _leopotamEcsContainer;
        private DefaultEcsContainer _defaultEcsContainer;
        private MinEcsContainer _minEcsContainer;
        private EntitasContainer _entitasContainer;
        private AudreyContainer _audreyContainer;
        private LazyEcsContainer _lazyEcsContainer;
        private LeopotamEcsLite.LeopotamEcsContainer _leopotamEcsLiteContainer;
        private MonoGame.Extended.Entities.MGEContainer _MGEEContainer;
        private SimplecsContainer _simpleEcsContainer;

        private int _amountOfEntities = 1024;
        private int _amountOfIterations = 1024;

        [GlobalSetup]
        public void Setup()
        {
            _leopotamEcsContainer = new LeopotamEcsContainer();
            AddSystems(_leopotamEcsContainer);
            _leopotamEcsContainer.Init();
            _defaultEcsContainer = new DefaultEcsContainer();
            AddSystems(_defaultEcsContainer);
            _minEcsContainer = new MinEcsContainer();
            AddSystems(_minEcsContainer);
            _entitasContainer = new EntitasContainer();
            AddSystems(_entitasContainer);
            _audreyContainer = new AudreyContainer();
            AddSystems(_audreyContainer);
            _lazyEcsContainer = new LazyEcsContainer();
            AddSystems(_lazyEcsContainer);
            _lazyEcsContainer.Init();
            _leopotamEcsLiteContainer = new LeopotamEcsLite.LeopotamEcsContainer();
            AddSystems(_leopotamEcsLiteContainer);
            _leopotamEcsLiteContainer.Init();
            _MGEEContainer = new MonoGame.Extended.Entities.MGEContainer()
            {
                GameTimeProvider = new GameTimeProvider()
            };
            AddSystems(_MGEEContainer);
            _MGEEContainer.Init();
            _simpleEcsContainer = new SimplecsContainer();
            AddSystems(_simpleEcsContainer);

            for (int i = 0; i < _amountOfEntities; i++)
            {
                var leopotamEntity = _leopotamEcsContainer.CreateNewEntity();
                AddComponents(leopotamEntity);
                var defaultEntity = _defaultEcsContainer.CreateNewEntity();
                AddComponents(defaultEntity);
                var minEntity = _minEcsContainer.CreateNewEntity();
                AddComponents(minEntity);
                var entitasEntity = _entitasContainer.CreateNewEntity();
                AddComponents(entitasEntity);
                var audreyEntity = _audreyContainer.CreateNewEntity();
                AddComponents(audreyEntity);
                var lazyEntity = _lazyEcsContainer.CreateNewEntity();
                AddComponents(lazyEntity);
                var leopotamEcsLiteEntity = _leopotamEcsLiteContainer.CreateNewEntity();
                AddComponents(leopotamEcsLiteEntity);
                var mgeeEntity = _MGEEContainer.CreateNewEntity();
                AddComponents(mgeeEntity);
                var simplecsEntity = _simpleEcsContainer.CreateNewEntity();
                AddComponents(simplecsEntity);
            }
        }

        public void AddSystems(EcsContainer container)
        {
            container.AddSystem(new AccelerationSystem());
            container.AddSystem(new SpeedSystem());
        }

        public void AddComponents(IUnifiedEntity entity)
        {
            entity.AddComponent(new PositionComponent()
            {
            });
            entity.AddComponent(new VelocityComponent()
            {
            });
            entity.AddComponent(new AccelerationComponent()
            {
            });
        }

        [Benchmark]
        public void LeopotamEcs()
        {
            Scenario1(_leopotamEcsContainer);
        }

        [Benchmark]
        public void MgeEcs()
        {
            Scenario1(_MGEEContainer);
        }

        [Benchmark]
        public void MinEcs()
        {
            Scenario1(_minEcsContainer);
        }

        [Benchmark]
        public void EntitasEcs()
        {
            Scenario1(_entitasContainer);
        }

        [Benchmark]
        public void LazyEcs()
        {
            Scenario1(_lazyEcsContainer);
        }

        [Benchmark]
        public void DefaultEcs()
        {
            Scenario1(_defaultEcsContainer);
        }

        [Benchmark]
        public void Simplecs()
        {
            Scenario1(_simpleEcsContainer);
        }

        public void Scenario1(EcsContainer container)
        {
            for (int i = 0; i < _amountOfIterations; i++)
            {
                container.Run();
            }
        }
    }
}