using UnityEngine;

namespace DataBase
{
    public class DbTrigger : MonoBehaviour
    {
        private void Start()
        {
            DbContext.INSTANCE.Initialize();
        }
    }
}
