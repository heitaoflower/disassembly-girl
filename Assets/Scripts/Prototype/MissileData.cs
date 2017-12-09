using System.Collections.Generic;

namespace Prototype
{
    public class MissileData : PrototypeObject
    {
        public float WOE = default(float);

        public float CD = default(float);

        public string resourceID = default(string);

        public string hitEffectID = default(string);

        public string audioID = default(string);

        public PhysicsType physicsType = PhysicsType.Default;

        public IList<EffectorData> effectors = null;
    }
}
