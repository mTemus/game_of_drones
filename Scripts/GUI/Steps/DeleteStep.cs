using UnityEngine;

public class DeleteStep : MonoBehaviour
{
    public void DeleteThisStep() {
        Destroy(transform.gameObject);
    }
    
}
