using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TorchPanel : MonoBehaviour
{
    float load = 0.0f;
    public Slider barra;
    public GameObject indietro;
    public Text percText;
    public Text InfoText;
    public void startBarra()
    {
        StartCoroutine(BarraTorch());
    }

    IEnumerator BarraTorch()
    {
        while (barra.value < 1.0f)
        {
            barra.value += 0.01f;
            percText.text = (barra.value * 100).ToString("0") + "%";
            yield return null;
        }
        for (int i = 0; i < 3; i++)
        {
            InfoText.text = "POD aggiornato";
            yield return new WaitForSeconds(0.3f);
            InfoText.text = "";
            yield return new WaitForSeconds(0.3f);
        }

        indietro.SetActive(true);
        indietro.GetComponent<Button>().Select();
    }
}
