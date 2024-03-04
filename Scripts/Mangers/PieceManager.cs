using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    [SerializeField] string _EPD;
    public static PieceManager Instance;
    private List<ScriptablePiece> _pieces;
    private Dictionary<Vector2, BasePiece> _piecePosition = new Dictionary<Vector2, BasePiece>();
    public static bool Player; // if true then Player picked White else Black
    public Vector2 LastMoved;
    public int PawnMoved;

    void Awake(){

        Instance = this;
        _pieces = Resources.LoadAll<ScriptablePiece>("Pieces").ToList();
    }

    public Vector2 GetLastMovedPieceCoord(){

        return LastMoved;
    }

    public void SetLastMovedPiece(Vector2 newPiece){

        LastMoved = newPiece;
    }

    public void SetPawnMoved(int distance){

        PawnMoved = distance;
    }

    public Vector2 FindPieceVec(Name name, Team team){

        foreach(var kvp in _piecePosition){

            BasePiece piece = kvp.Value;

            if(piece.ScriptablePiece.Name == name && piece.ScriptablePiece.Team == team){

                return kvp.Key;
            }
        }

        Debug.Log($"{team} {name} could not be found");
        return new Vector2(-1, -1);
    }

    public BasePiece FindPieceBase(Vector2 newTile){

        if(_piecePosition.TryGetValue(newTile, out BasePiece piece)){

            return piece;
        }
        else{

            Debug.LogError($"Piece not found at tile {newTile}");
            return null;
        }
    }

    public bool IsTileOccupied(Vector2 tilePosition){

        if(_piecePosition.ContainsKey(tilePosition)){
            
            return true;
        }

        return false;
    }

    public void MovePiece(Vector2 newTile, BasePiece piece){

        Vector2 oldTile = _piecePosition.FirstOrDefault(x => x.Value == piece).Key;

        _piecePosition.Remove(newTile);
        _piecePosition.Remove(oldTile);
        _piecePosition.Add(newTile, piece);
    }

    public void RemovePiece(Vector2 position){

        if(_piecePosition.ContainsKey(position)){
            _piecePosition.Remove(position);
        }

        Debug.Log($"Piece could not be found {position}");
    }

    public void AddPiece(Vector2 position, BasePiece piece){

        if(!_piecePosition.ContainsKey(position)){
            _piecePosition.Add(position, piece);
        }
        else{
            Debug.Log($"Piece already in Dictionary at {position}");
        }
        
        
    }

    public void CheckForAllPieces(){

        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){

                if(IsTileOccupied(new Vector2(j,i))){

                    BasePiece piece = _piecePosition[new Vector2(j,i)];

                    Debug.Log($"{piece}, x: {j}, y: {i}");
                }
            }
        }
    }

    public void PutInAllPiecesInDictionary(){

        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){

                Tile tile = GridManager.Instance.GetPieceSpawnTile(j, i);

                if(tile != null){

                    BasePiece piece = tile.GetPiece();

                    if(piece != null){

                        Vector2 tilePos = new Vector2(j, i);

                        if(!_piecePosition.ContainsKey(tilePos)){
                            
                            _piecePosition.Add(tilePos, piece);
                        }
                    }
                }
            }
        }
    }

    private T GetSpecificPiece<T>(Team team, Name name) where T : BasePiece {

        return _pieces.Where(p => p.Team == team && p.PiecePrefab is T && p.Name == name).Select(p => (T)p.PiecePrefab).FirstOrDefault();
    }

    private void SpawnPiece<T>(Team team, Name name, int x, int y) where T : BasePiece {

        var specificPrefab = GetSpecificPiece<T>(team, name);

        if(specificPrefab != null){

            BasePiece spawnedPiece = Instantiate(specificPrefab);
            Tile specificSpawnTile = GridManager.Instance.GetPieceSpawnTile(x, y);

            specificSpawnTile.SetPiece(spawnedPiece);
            spawnedPiece.transform.position = specificSpawnTile.transform.position;

            spawnedPiece.SetScriptablePiece(_pieces.Find(p => p.Team == team && p.PiecePrefab == specificPrefab));
            spawnedPiece.OccupiedTile = specificSpawnTile;
        } 
        else{

            Debug.LogError($"Failed to find valid prefab for team: {team} and name: {name}");
        }
    }

    public void SpawnPiecesOnBoardWhite(){

        int k = 0;
        GetPlayerSide(true);

        if(_EPD != null && _EPD.Length > 0){
            Debug.Log("_EPD able to use");
        }
        else{
            Debug.LogError("_EPD is null or Length = 0");
        }

        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){

                var tempEPD = _EPD[k];

                if(k >= _EPD.Length){
                    Debug.Log("Reached end of EPD");
                    break;
                }

                if(tempEPD == 'p'){
                    SpawnPiece<BaseBlack>(Team.Black, Name.Pawn, j, i);
                }
                else if(tempEPD == 'P'){
                    SpawnPiece<BaseWhite>(Team.White, Name.Pawn, j, i);
                }
                else if(tempEPD == 'n'){
                    SpawnPiece<BaseBlack>(Team.Black, Name.Knight, j, i);
                }
                else if(tempEPD == 'N'){
                    SpawnPiece<BaseWhite>(Team.White, Name.Knight, j, i);
                }
                else if(tempEPD == 'b'){
                    SpawnPiece<BaseBlack>(Team.Black, Name.Bishop, j, i);
                }
                else if(tempEPD == 'B'){
                    SpawnPiece<BaseWhite>(Team.White, Name.Bishop, j, i);
                }
                else if(tempEPD == 'r'){
                    SpawnPiece<BaseBlack>(Team.Black, Name.Rook, j, i);
                }
                else if(tempEPD == 'R'){
                    SpawnPiece<BaseWhite>(Team.White, Name.Rook, j, i);
                }
                else if(tempEPD == 'q'){
                    SpawnPiece<BaseBlack>(Team.Black, Name.Queen, j, i);
                }
                else if(tempEPD == 'Q'){
                    SpawnPiece<BaseWhite>(Team.White, Name.Queen, j, i);
                }
                else if(tempEPD == 'k'){
                    SpawnPiece<BaseBlack>(Team.Black, Name.King, j, i);
                }
                else if(tempEPD == 'K'){
                    SpawnPiece<BaseWhite>(Team.White, Name.King, j, i);
                }
                else if (tempEPD > '0' && tempEPD < '9'){
                    j += (int)(tempEPD - '0');
                }
                else if(tempEPD == '/'){
                    //Debug.Log("next line");
                    j--;
                }
                else{
                    Debug.LogError($"EPD not valid: {tempEPD}");
                }
                k++;

                //Debug.Log($"Coords {i} {j} k: {k}");
            }
        }

        GameManager.Instance.UpdateGameState(GameState.WhiteTurn);
    }

    public void SpawnPiecesOnBoardBlack(){

        int k = 0;
        GetPlayerSide(false);

        char[] tempArray = _EPD.ToCharArray();
        Array.Reverse(tempArray);
        string _EPDrev = new string(tempArray);

        if(_EPD != null && _EPD.Length > 0){
            Debug.Log("_EPD able to use");
        }
        else{
            Debug.LogError("_EPD is null or Length = 0");
        }

        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){

                var tempEPD = _EPDrev[k];

                if(k >= _EPDrev.Length){
                    Debug.Log("Reached end of EPD");
                    break;
                }

                if(tempEPD == 'p'){
                    SpawnPiece<BaseBlack>(Team.Black, Name.Pawn, j, i);
                }
                else if(tempEPD == 'P'){
                    SpawnPiece<BaseWhite>(Team.White, Name.Pawn, j, i);
                }
                else if(tempEPD == 'n'){
                    SpawnPiece<BaseBlack>(Team.Black, Name.Knight, j, i);
                }
                else if(tempEPD == 'N'){
                    SpawnPiece<BaseWhite>(Team.White, Name.Knight, j, i);
                }
                else if(tempEPD == 'b'){
                    SpawnPiece<BaseBlack>(Team.Black, Name.Bishop, j, i);
                }
                else if(tempEPD == 'B'){
                    SpawnPiece<BaseWhite>(Team.White, Name.Bishop, j, i);
                }
                else if(tempEPD == 'r'){
                    SpawnPiece<BaseBlack>(Team.Black, Name.Rook, j, i);
                }
                else if(tempEPD == 'R'){
                    SpawnPiece<BaseWhite>(Team.White, Name.Rook, j, i);
                }
                else if(tempEPD == 'q'){
                    SpawnPiece<BaseBlack>(Team.Black, Name.Queen, j, i);
                }
                else if(tempEPD == 'Q'){
                    SpawnPiece<BaseWhite>(Team.White, Name.Queen, j, i);
                }
                else if(tempEPD == 'k'){
                    SpawnPiece<BaseBlack>(Team.Black, Name.King, j, i);
                }
                else if(tempEPD == 'K'){
                    SpawnPiece<BaseWhite>(Team.White, Name.King, j, i);
                }
                else if (tempEPD > '0' && tempEPD < '9'){
                    j += (int)(tempEPD - '0');
                }
                else if(tempEPD == '/'){
                    //Debug.Log("next line");
                    j--;
                }
                else{
                    Debug.LogError($"EPD not valid: {tempEPD}");
                }
                k++;

                //Debug.Log($"Coords {i} {j} k: {k}");
            }
        }

        GameManager.Instance.UpdateGameState(GameState.WhiteTurn);
    }

    private static void GetPlayerSide(bool PlayerValue){

        Player = PlayerValue;
    }

    public bool IsCastleConditionMet(Team team, Vector2 pos){

        Vector2 kingPosition = FindPieceVec(Name.King, team);
        Vector2 queensideRook = Player ? (team == Team.White ? new Vector2(0, kingPosition.y) : new Vector2(7, kingPosition.y)) : (team == Team.White ? new Vector2(7, kingPosition.y) : new Vector2(0, kingPosition.y));
        Vector2 kingsideRook = Player ? (team == Team.White ? new Vector2(7, kingPosition.y) : new Vector2(0, kingPosition.y)) : (team == Team.White ? new Vector2(0, kingPosition.y) : new Vector2(7, kingPosition.y));

        if((pos.x < kingPosition.x && pos.y == queensideRook.y) || (pos.x > kingPosition.x && pos.y == kingsideRook.y)){

            BasePiece tempPiece = _piecePosition[Player ? queensideRook : kingsideRook];
            
            if (tempPiece.ScriptablePiece.Name == Name.Rook && tempPiece.ScriptablePiece.Team == team && kingPosition == new Vector2(Player ? 4 : 3, queensideRook.y)){

                return true;
            }
    }

    return false;
}

    public bool IsTileUnderAttack(Tile tile, Team team){

        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){

                Vector2 tempPos = new Vector2(i, j);
                    
                if(_piecePosition.ContainsKey(tempPos)){

                    BasePiece tempPiece = _piecePosition[tempPos];

                    if(tempPiece != null && tempPiece.ScriptablePiece != null){

                        if(tempPiece.ScriptablePiece.Team != team && tempPiece.IsViableMove(tile.transform.position)){

                            Debug.Log($"{tempPiece}");
                            return true;
                        }
                    }
                }
            }
        }
        
        return false;
    }

    private Vector2 IsTileUnderAttackVec(Tile tile, Team team){

        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){

                Vector2 tempPos = new Vector2(i, j);
                    
                if(_piecePosition.ContainsKey(tempPos)){

                    BasePiece tempPiece = _piecePosition[tempPos];

                    if(tempPiece.ScriptablePiece.Team != team && tempPiece.IsViableMove(tile.transform.position)){

                        // Debug.Log($"{tempPiece}");
                        return new Vector2(i, j);
                    }
                }
            }
        }

        Debug.LogWarning($"Piece not found that is attacking tile: {tile}");
        return new Vector2(-1, -1);
    }

    // specific just for IsMoveViable()
    public bool IsKingUnderAttack(Team team){

        Vector2 kingVector = FindPieceVec(Name.King, team);
        Tile kingTile = GridManager.Instance.GetTile(kingVector);

        // move the piece and then check if king under attack
        // after put piece back
        
        Debug.Log($"Blub {kingVector} {kingTile} {team}");

        return IsTileUnderAttack(kingTile, team);
    }

    public BasePiece PawnToQueen<T>(Tile newTile, BasePiece pawn) where T : BasePiece{

        SpawnPiece<T>(pawn.ScriptablePiece.Team, Name.Queen, (int)newTile.transform.position.x, (int)newTile.transform.position.y);
        BasePiece tempPiece = newTile.GetPiece();
        MovePiece(tempPiece.OccupiedTile.transform.position, tempPiece);
        SetLastMovedPiece(tempPiece.transform.position);

        return tempPiece;
    }

    public bool IsWinConditionMet(Team team){

        Vector2 kingVec = FindPieceVec(Name.King, team);
        BasePiece kingBase = FindPieceBase(kingVec);
        Tile kingTile = GridManager.Instance.GetTile(kingVec);
        Vector2 attackVec = IsTileUnderAttackVec(kingTile, team);

        if(_piecePosition.TryGetValue(attackVec, out BasePiece attackPiece)){

            Tile attackTile = GridManager.Instance.GetTile(attackVec);
            Team oppositeTeam = (team == Team.White) ? Team.Black : Team.White;

            if(IsTileUnderAttack(attackTile, oppositeTeam)){

                Vector2 tempAttackVec = IsTileUnderAttackVec(attackTile, oppositeTeam); // check for edge cases

                if(tempAttackVec == new Vector2(-1, -1)){
                    Debug.Log("No tempAttack found");
                }

                Tile tempAttackTile = GridManager.Instance.GetTile(tempAttackVec);
                BasePiece tempAttackPiece = FindPieceBase(tempAttackVec);

                MovePiece(attackVec, tempAttackPiece);

                if(!IsTileUnderAttack(kingTile, team)){

                    MovePiece(tempAttackVec, tempAttackPiece);
                    AddPiece(attackVec, attackPiece);
                    return false;
                }

                MovePiece(tempAttackVec, tempAttackPiece);
                AddPiece(attackVec, attackPiece);
            }

            // work in progress -> after checking the scenario above, it breaks but the below works somehow
            // probably because it will not continue after not seeing a viable move for the enemy, XD
            // nvm its because im checking a tile for being under attack even tho the king isnt there
            // wtf am i shitting about
            // the function works. its something else
            // add functional comments to every part, where it could be an issue

            for(int x = -1; x < 2; x++){
                for(int y = -1; y < 2; y++){

                    Vector2 tempKingVec = new Vector2(kingVec.x + x, kingVec.y + y);

                    if(tempKingVec.x < 0 || tempKingVec.x > 7 || tempKingVec.y < 0 || tempKingVec.y > 7){

                        continue;
                    }

                    Tile tempKingTile = GridManager.Instance.GetTile(tempKingVec);
                    
                    if(kingBase.IsViableMove(tempKingVec)){
                        Debug.Log("Blub1");
                        if(!IsTileUnderAttack(tempKingTile, team)){
                            
                            Debug.Log("Blub2");
                            return false;
                        }
                    }
                }
            }
            
            // Check other conditions like move in between king and piece if others fail
        }
        else{

            Debug.LogError($"attack piece not found at {attackVec}");
        }
        
        Debug.Log("Blub3");
        return true;
    }

    //public bool IsWinConditionMetSubKing(Vector2 kingVec, Team team)
}
