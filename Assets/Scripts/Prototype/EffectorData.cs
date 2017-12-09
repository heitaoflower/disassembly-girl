namespace Prototype
{
    public class EffectorData : PrototypeObject
    {
        public string iconID = null;

        public string resourceID = null;

        public float parameter1 = default(float);

        public float parameter2 = default(float);

        public float parameter3 = default(float);

        public float parameter4 = default(float);

        public float parameter5 = default(float);

        private EffectorData()
        {

        }

        public static EffectorData FromConfig(int id)
        {
            return FromConfig(ConfigMgr.GetInstance().DisassemblygirlEffector.GetConfigById(id));
        }

        public static EffectorData FromConfig(DisassemblygirlEffectorConfig config)
        {
            if (config == null)
            {
                return null;
            }

            EffectorData effector = new EffectorData();
            effector.id = config.id;
            effector.name = config.name;
            effector.iconID = config.iconID;
            effector.resourceID = config.resourceID;

            effector.parameter1 = config.parameter1;
            effector.parameter2 = config.parameter2;
            effector.parameter3 = config.parameter3;
            effector.parameter4 = config.parameter4;
            effector.parameter5 = config.parameter5;

            return effector;
        }
    }
}