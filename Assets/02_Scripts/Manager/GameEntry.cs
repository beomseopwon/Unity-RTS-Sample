using UnityEngine;

namespace RTS
{
    public class GameEntry : MonoBehaviour
    {
        private void Update()
        {
            UnitManager.GetInstance().Update();
        }
    }
}
