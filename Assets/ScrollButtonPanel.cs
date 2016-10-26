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
        float perc = (float)n / nButtons;
        FindObjectOfType<MenuControl>().nButtonReport = n;
        scroll.value = 1.0f - perc;
    }
}
