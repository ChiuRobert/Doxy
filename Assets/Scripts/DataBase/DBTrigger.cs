using Doxy.DataBase;
using UnityEngine;

public class DBTrigger : MonoBehaviour
{
    private void Start()
    {
        DBContext.INSTANCE.Initialize();
    }
}
