using UnityEngine;
using Orange.Utils;
namespace Utils
{
    public class CameraUtils
    {
        private static float xVibe = 0.025f;
        private static float yVibe = 0.025f;
        private static float zVibe = 0.0f;
        private static float xRot = 0f;
        private static float yRot = 0f;
        private static float zRot = 0f;
        private static float speed = 60.0f;
        private static float diminish = 0.9f;
        private static int numberOfShakes = 1;

        public static void Shake()
        {
            Vibration vibration = Camera.main.GetComponent<Vibration>();

            vibration.StartShaking(new Vector3(xVibe, yVibe, zVibe), new Quaternion(xRot, yRot, zRot, 1), speed, diminish, numberOfShakes);
        }
    }
}