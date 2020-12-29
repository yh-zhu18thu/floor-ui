using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class setText : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    public void setText_(string name)
    {
        text.text = name;
    }
}
