using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;

    void Awake(){
        
        Instance = this;
    }

    void Start(){

        UpdateGameState(State);
    }

    public void UpdateGameState(GameState newState){

        State = newState;

        switch(newState){
            
            case GameState.SelectColor:
                ButtonManager.Instance.SpawnButtons();
                break;
            case GameState.GenerateGridWhite:
                GridManager.Instance.GenerateGridWhite();
                break;
            case GameState.GenerateGridBlack:
                GridManager.Instance.GenerateGridBlack();
                break;
            case GameState.SpawnPiecesWhite:
                PieceManager.Instance.SpawnPiecesOnBoardWhite();
                break;
            case GameState.SpawnPiecesBlack:
                PieceManager.Instance.SpawnPiecesOnBoardBlack();
                break;
            case GameState.WhiteTurn:
                break;
            case GameState.BlackTurn:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState{
    SelectColor,
     GenerateGridWhite,
     GenerateGridBlack,
     SpawnPiecesWhite,
     SpawnPiecesBlack,
    WhiteTurn,
    BlackTurn,
    Victory,
    Lose
}