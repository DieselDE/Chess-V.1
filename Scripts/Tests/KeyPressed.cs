using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPressed : MonoBehaviour
{
    void Update(){
        
        if(Input.GetKeyDown(KeyCode.I)){
            
            TestPiecesInDictionary();
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            QuitApplication();
        }
    }

    void TestPiecesInDictionary(){

        PieceManager.Instance.CheckForAllPieces();
    }

    void QuitApplication(){

        #if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;
        #else

            Application.Quit();
        #endif
    }
}

