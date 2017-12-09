using UnityEngine;
using System.Collections;

namespace Orange.Utils
{
    public class Preset
    {
        public bool vibrateOnAwake = true;
        public Vector3 startingShakeDistance = default(Vector3);
        public Quaternion startingRotationAmount = default(Quaternion);
        public float shakeSpeed = 60.0f;
        public float decreaseMultiplier = 0.5f;
        public int numberOfShakes = 8;
        public bool shakeContinuous = false;
    }

    public class Vibration : MonoBehaviour
    {
        public Preset[] presets = null;
        public int presetToUse = 0;

        public bool vibrateOnAwake = true;
        public Vector3 startingShakeDistance = default(Vector3);
        public Quaternion startingRotationAmount = default(Quaternion);
        public float shakeSpeed = 60.0f;
        public float decreaseMultiplier = 0.5f;
        public int numberOfShakes = 8;
        public bool shakeContinuous = false;

        private Vector3 actualStartingShakeDistance = default(Vector3);
        private Quaternion actualStartingRotationAmount = default(Quaternion);
        private float actualShakeSpeed = default(float);
        private float actualDecreaseMultiplier = default(float);
        private float actualNumberOfShakes = default(float);

        private Vector3 originalPosition = default(Vector3);
        private Quaternion originalRotation = default(Quaternion);

        void Awake()
        {
            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;

            if (vibrateOnAwake)
            {
                StartShaking();
            }
        }

        public void StartShaking()
        {
            actualStartingShakeDistance = startingShakeDistance;
            actualStartingRotationAmount = startingRotationAmount;
            actualShakeSpeed = shakeSpeed;
            actualDecreaseMultiplier = decreaseMultiplier;
            actualNumberOfShakes = numberOfShakes;
            StopShaking();
            StartCoroutine("Shake");
        }

        public void StartShaking(Vector3 shakeDistance, Quaternion rotationAmount, float speed, float diminish, int numOfShakes)
        {
            actualStartingShakeDistance = shakeDistance;
            actualStartingRotationAmount = rotationAmount;
            actualShakeSpeed = speed;
            actualDecreaseMultiplier = diminish;
            actualNumberOfShakes = numOfShakes;
            StopShaking();
            StartCoroutine("Shake");
        }

        public void StartShakingRandom(float minDistance, float maxDistance, float minRotationAmount, float maxRotationAmount)
        {
            actualStartingShakeDistance = new Vector3(Random.Range(minDistance, maxDistance), Random.Range(minDistance, maxDistance), Random.Range(minDistance, maxDistance));
            actualStartingRotationAmount = new Quaternion(Random.Range(minRotationAmount, maxRotationAmount), Random.Range(minRotationAmount, maxRotationAmount), Random.Range(minRotationAmount, maxRotationAmount), 1);
            actualShakeSpeed = shakeSpeed * Random.Range(0.8f, 1.2f);
            actualDecreaseMultiplier = decreaseMultiplier * Random.Range(0.8f, 1.2f);
            actualNumberOfShakes = numberOfShakes + Random.Range(-2, 2);
            StopShaking();
            StartCoroutine("Shake");
        }

        public void StopShaking()
        {
            // Stop the shake coroutine if its running
            StopCoroutine("Shake");

            // Reset the position of the GameObject to its original position
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
        }

        private IEnumerator Shake()
        {
            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;

            float hitTime = Time.time;
            float shake = actualNumberOfShakes;

            float shakeDistanceX = actualStartingShakeDistance.x;
            float shakeDistanceY = actualStartingShakeDistance.y;
            float shakeDistanceZ = actualStartingShakeDistance.z;

            float shakeRotationX = actualStartingRotationAmount.x;
            float shakeRotationY = actualStartingRotationAmount.y;
            float shakeRotationZ = actualStartingRotationAmount.z;

            // Shake the number of times specified in actualNumberOfShakes
            while (shake > 0 || shakeContinuous)
            {
                float timer = (Time.time - hitTime) * actualShakeSpeed;
                float x = originalPosition.x + Mathf.Sin(timer) * shakeDistanceX;
                float y = originalPosition.y + Mathf.Sin(timer) * shakeDistanceY;
                float z = originalPosition.z + Mathf.Sin(timer) * shakeDistanceZ;

                float xr = originalRotation.x + Mathf.Sin(timer) * shakeRotationX;
                float yr = originalRotation.y + Mathf.Sin(timer) * shakeRotationY;
                float zr = originalRotation.z + Mathf.Sin(timer) * shakeRotationZ;

                transform.localPosition = new Vector3(x, y, z);
                transform.localRotation = new Quaternion(xr, yr, zr, transform.localRotation.w);

                if (timer > Mathf.PI * 2)
                {
                    hitTime = Time.time;
                    shakeDistanceX *= actualDecreaseMultiplier;
                    shakeDistanceY *= actualDecreaseMultiplier;
                    shakeDistanceZ *= actualDecreaseMultiplier;

                    shakeRotationX *= actualDecreaseMultiplier;
                    shakeRotationY *= actualDecreaseMultiplier;
                    shakeRotationZ *= actualDecreaseMultiplier;

                    shake--;
                }
                yield return true;
            }

            // Reset the position of the GameObject to its original position
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
        }
    }
}