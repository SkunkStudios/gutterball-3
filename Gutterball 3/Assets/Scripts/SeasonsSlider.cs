using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonsSlider : MonoBehaviour
{
    [Header("Halloween")]
    public ColorBlock halloweenSlider;
    [Header("Christmas")]
    public ColorBlock xmasSlider;

    void Awake()
    {
        if (DateTime.Now.Month == 10)
        {
            GetComponent<Slider>().colors = halloweenSlider;
        }
        else if (DateTime.Now.Month == 12)
        {
            GetComponent<Slider>().colors = xmasSlider;
        }
    }
}
