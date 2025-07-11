
using UnityEngine;

public class TapDetector : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Plushie plushie = hit.collider.GetComponent<Plushie>();
                if (plushie != null)
                {
                    plushie.Tap();
                }
            }
        }
    }
}
