using RTS.Unit;
using System.Collections.Generic;

namespace RTS
{
    public sealed class UnitManager
    {
        private const int UNIT_MAX_COUNT = 200;
        private List<AUnitBase> _units = new List<AUnitBase>(UNIT_MAX_COUNT);
        private static UnitManager _instance;

        public static UnitManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UnitManager();
            }
            return _instance;
        }

        public void Update()
        {
            //TODO : 유닛 업데이트
        }
    }
}
