using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBlack : Button
{
    public void OnButtonClick(){
        
        GameManager.Instance.UpdateGameState(GameState.GenerateGridBlack);
        gameObject.SetActive(false);
    }
}