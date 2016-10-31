using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollButtonPanel : MonoBehaviour
{
    public int nButtons = 0;

    public Scrollbar scroll;
    void Start()
    {
        nButtons = this.transform.childCount -1;
    }

    public void RefreshScroll(int n)
    {
        float perc = 1.0f - (float)n / nButtons;
        FindObjectOfType<MenuControl>().nButtonReport = n;
        StartCoroutine(LerpScroll(perc));
    }

    public IEnumerator LerpScroll(float n1)
    {
        float elapsedTime = 0.0f;
        float startScroll = scroll.value;
        while (elapsedTime < 0.3f)
        {
            scroll.value = Mathf.Lerp(startScroll, n1, (elapsedTime / 0.2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
