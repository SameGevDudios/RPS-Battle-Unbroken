using UnityEngine;

public class ChangeVisibility : MonoBehaviour
{
    private void OnBecameVisible()
    {
        gameObject.SetActive(true);
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
