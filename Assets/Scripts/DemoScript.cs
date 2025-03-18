using UnityEngine;

public class DemoScript : MonoBehaviour
{
    [SerializeField] private GameObject objecToDeactive;
    [SerializeField] private GameObject objectToActive;

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            objectToActive.SetActive(true);
            objecToDeactive.SetActive(false);
        }
        else
        {
            objectToActive.SetActive(false);
            objecToDeactive.SetActive(true);
        }
    }
}
