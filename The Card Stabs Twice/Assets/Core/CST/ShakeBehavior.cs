using UnityEngine;

namespace CST
{
    public class ShakeBehavior : MonoSingleton<ShakeBehavior>
    {
        // Transform of the GameObject you want to shake
        private Transform transform;

        // Desired duration of the shake effect
        [SerializeField] private float shakeDuration = 0f;

        // A measure of magnitude for the shake. Tweak based on your preference
        [SerializeField] private float shakeMagnitude = 0.7f;

        // A measure of how quickly the shake effect should evaporate
        [SerializeField] private float dampingSpeed = 1.0f;

        // The initial position of the GameObject
        Vector3 initialPosition;

        public override void Init()
        {
            if (transform == null)
            {
                transform = GetComponent(typeof(Transform)) as Transform;
            }
        }

        void OnEnable()
        {
            initialPosition = transform.localPosition;
        }

        void Update()
        {
            if (shakeDuration > 0)
            {
                transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

                shakeDuration -= Time.deltaTime * dampingSpeed;
            }
            else
            {
                shakeDuration = 0f;
                transform.localPosition = initialPosition;
            }
        }

        public void TriggerShake(float duration)
        {
            shakeDuration = duration;
        }

        [ContextMenu("TestShake")]
        public void TestShake()
        {
            TriggerShake(0.2f);
        }
    }
}