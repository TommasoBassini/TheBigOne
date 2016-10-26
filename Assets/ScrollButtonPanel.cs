using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ScrollButtonPanel : MonoBehaviour
{
    public int nButtons = 0;

    public Scrollbar scroll;
    void Start()
    {
        nButtons = this.transform.childCount;
    }

    public void RefreshScroll(int n)
    {
        n++;
        float perc = n / nButtons;
        scroll.value = 1.0f - (n / nButtons);
        Debug.Log(perc);

    }
}
