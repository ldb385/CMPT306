using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpookBar : MonoBehaviour
{
    public Image ImgHealth;
    public GameObject TrackPlayerSpook;
    public int Min;
    public int Max;
    private float curSpook;
    private float curSpookPercent;

    public void SetSpook(float Spook){
        if(Spook != curSpook){
            if(Max - Min == 0){
                curSpook = 0;
                curSpookPercent = 0;
            }
            else{
                curSpook = Spook;

                curSpookPercent = (float)curSpook / (float)(Max-Min);
            }
            ImgHealth.fillAmount = curSpookPercent;
        }
    }

    void Update()
    {
        SetSpook(TrackPlayerSpook.GetComponent<Player>().spookLevel);
    }
}