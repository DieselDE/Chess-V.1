using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance;
    public GameObject ButtonCanvasPrefab;
    public ButtonWhite ButtonWhitePrefab;
    public ButtonBlack ButtonBlackPrefab;

    void Awake(){

        Instance = this;
    }

    public void SpawnButtons(){
        
        GameObject buttonCanvas = Instantiate(ButtonCanvasPrefab);

        ButtonWhite buttonWhite = Instantiate(ButtonWhitePrefab, transform);
        buttonWhite.onClick.AddListener(() => buttonWhite.OnButtonClick());

        ButtonBlack buttonBlack = Instantiate(ButtonBlackPrefab, transform);
        buttonBlack.onClick.AddListener(() => buttonBlack.OnButtonClick());
    }
}