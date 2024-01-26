using Keys;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace Controllers
{
    public class ParticleEmitController : View
    {
        [Inject]
        public IParticleEmitService ParticleEmitService { get; set; }

        [SerializeField] private Vector3 emitPositionAdjust;
        [SerializeField] private int particleStartSize;
        [SerializeField] private int burstCount;

        private ParticleSystem _particleSystem;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _particleSystem.Stop();
            transform.Rotate(0, 0, 0);
        }

        public void EmitParticle(Vector3 burstPos)
        {
            gameObject.SetActive(true);
            var emitParams = new ParticleSystem.EmitParams
            {
                position = burstPos + emitPositionAdjust,
                startSize = particleStartSize,
            };
            _particleSystem.Emit(emitParams, burstCount);
            _particleSystem.Play();
        }

        public void EmitStop()
        {
            _particleSystem.Stop();
            gameObject.SetActive(false);
        }

        public void LookRotation(Quaternion toRotation)
        {
            var mainModule = new ParticleSystem().shape;
            var rotationEuler = mainModule.rotation;
            Quaternion rotation = Quaternion.Euler(rotationEuler);
            rotation = Quaternion.RotateTowards(rotation, toRotation, 30);
        }
    }

    public interface IParticleEmitService
    {
    }
}
