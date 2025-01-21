/*
Copyright (c) 2024 Mimy Quality
Released under the MIT license
https://opensource.org/licenses/mit-license.php
*/

namespace MimyLab.CombatAssemblyToolit
{
    using UdonSharp;
    using UnityEngine;
    using VRC.SDKBase;
    using VRC.Udon;

    [RequireComponent(typeof(ParticleSystem))]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DragonBreathInvolver : CombatInvolver
    {
        [SerializeField]
        private float _radius;

        [SerializeField, Tooltip("meter")]
        private float _searchRange = 20.0f;
        [SerializeField, Tooltip("degree")]
        private float _searchFOV = 45.0f;
        [SerializeField, Tooltip("deg/sec")]
        private float _hormingAccuracy = 36.0f;

        private ParticleSystem _breathEffect;
        private ParticleSystem.Particle[] _particles;

        private bool _initialized = false;
        private void Initialize()
        {
            if (_initialized) { return; }

            _breathEffect = GetComponent<ParticleSystem>();
            _particles = new ParticleSystem.Particle[_breathEffect.main.maxParticles];

            _initialized = true;
        }
        private void Start()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            var alive = _breathEffect.GetParticles(_particles);
            for (int i = 0; i < alive; i++)
            {
                var position = _particles[i].position;
                var velocity = _particles[i].velocity;
                var ray = new Ray(position, velocity);

                CombatUnit target;
                if (target = CheckUnitHit(ray))
                {
                    skill.OnUnitHit(target);
                    _breathEffect.TriggerSubEmitter(1, ref _particles[i]);
                    _particles[i].remainingLifetime = 0.0f;
                    continue;
                }

                if (target = SearchTargetUnit(ray))
                {
                    var lookAt = target.transform.position - position;
                    var rotateAngle = Time.deltaTime * _hormingAccuracy;
                    _particles[i].velocity = Vector3.RotateTowards(velocity, lookAt, rotateAngle, 0.0f);
                }
            }
            _breathEffect.SetParticles(_particles, alive);
        }

        public override void Involve()
        {
            Initialize();

            _breathEffect.Emit(1);
        }

        private CombatUnit CheckUnitHit(Ray ray)
        {
            var candidate = gamemaster.units;
            for (int i = 0; i < candidate.Length; i++)
            {
                var hitbox = candidate[i].hitBox;
                for (int j = 0; j < hitbox.Length; j++)
                {
                    if ((hitbox[j].ClosestPoint(ray.origin) == ray.origin)
                     || hitbox[j].Raycast(ray, out RaycastHit hitinfo, _radius))
                    {
                        return candidate[i];
                    }
                }
            }

            return null;
        }

        private CombatUnit SearchTargetUnit(Ray ray)
        {
            return null;
        }
    }
}
