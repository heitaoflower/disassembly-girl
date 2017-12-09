using UnityEngine;

namespace Map
{
    public enum SpwanPointIds : int
    {
        Default = 0,
        A,
        B,
        C,
        D,
        E,
        Boss,
    }

    public class SpwanPoint2D : MonoBehaviour
    {
        public SpwanPointIds type = SpwanPointIds.Default;

        public Vector2 direction = Vector2.zero;
    }
}
