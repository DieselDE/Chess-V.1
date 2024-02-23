using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWhite : Button
{
    public void OnButtonClick(){

        Debug.Log($"ButtonPressed!");
        
        GameManager.Instance.UpdateGameState(GameState.GenerateGridWhite);
        gameObject.SetActive(false);
    }
}