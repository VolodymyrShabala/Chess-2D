using UnityEngine;
using System;
using System.Collections.Generic;

public class BoardController : MonoBehaviour{
    private BoardData boardData;
    private AIOpponent ai;
    private bool playerPlaysWhite = true;

    [SerializeField] private GameObject whiteCellPrefab;
    [SerializeField] private GameObject blackCellPrefab;
    [SerializeField] private Figures whiteFigurePrefabs;
    [SerializeField] private Figures blackFigurePrefabs;
    [SerializeField] private GameObject highlightCellPrefab;
    private GameObject[] highlightCell;
    private int maxHighlightCell = 27; // Came up with this number through testing

    private GameObject[] gameFigures;
    private Vector2Int[] savedPossibleMoves;
    
    [Header("Debug")]
    [SerializeField] private FigureType figureBoardToShow;

    private void Start(){
        boardData = new BoardData();
        ai = new AIOpponent(boardData, 3, this);
        CreateBoard();
        PopulateBoard();
    }

    private void CreateBoard(){
        Transform boardParent = new GameObject("Board Holder").transform;
        boardParent.parent = transform;
        int index = 0;
        for(int r = 0; r < boardData.BoardSize; r++) {
            index++;
            for(int c = 0; c < boardData.BoardSize; c++) {
                Instantiate(index % 2 == 0 ? whiteCellPrefab : blackCellPrefab, 
                            new Vector3(c, r, 1), Quaternion.identity, boardParent);
                index++;
            }
        }
    }

    private void PopulateBoard(){
        SpawnFigures();
        SpawnHighlightCells();
    }

    private void SpawnFigures(){
        List<GameObject> temp = new List<GameObject>();
        Transform figureParent = new GameObject("Figure Holder").transform;
        figureParent.parent = transform;
        for(int r = 0; r < boardData.BoardSize; r++) {
            for(int c = 0; c < boardData.BoardSize; c++) {
                GameObject figureToSpawn = GetFigureToSpawn(c, r);
                if(figureToSpawn == null) {
                    continue;
                }

                temp.Add(Instantiate(figureToSpawn, new Vector3(c, r), Quaternion.identity, figureParent));
            }
        }

        gameFigures = temp.ToArray();
    }

    private GameObject GetFigureToSpawn(int c, int r){
        Vector2Int pos = new Vector2Int(c, r);
        FigureType figureType = boardData.GetFigureType(pos);
        switch(figureType) {
            case FigureType.Pawn:
                return boardData.IsCellOccupied(FigureType.White, pos) ? whiteFigurePrefabs.pawn : blackFigurePrefabs.pawn;
            case FigureType.Rook:
                return boardData.IsCellOccupied(FigureType.White, pos) ? whiteFigurePrefabs.rook : blackFigurePrefabs.rook;
            case FigureType.Knight:
                return boardData.IsCellOccupied(FigureType.White, pos) ? whiteFigurePrefabs.knight : blackFigurePrefabs.knight;
            case FigureType.Bishop:
                return boardData.IsCellOccupied(FigureType.White, pos) ? whiteFigurePrefabs.bishop : blackFigurePrefabs.bishop;
            case FigureType.Queen:
                return boardData.IsCellOccupied(FigureType.White, pos) ? whiteFigurePrefabs.queen : blackFigurePrefabs.queen;
            case FigureType.King:
                return boardData.IsCellOccupied(FigureType.White, pos) ? whiteFigurePrefabs.king : blackFigurePrefabs.king;
        }

        return null;
    }

    private void SpawnHighlightCells(){
        Transform highlightParent = new GameObject("Highlight Holder").transform;
        highlightParent.parent = transform;
        highlightCell = new GameObject[maxHighlightCell];
        for(int i = 0; i < maxHighlightCell; i++) {
            highlightCell[i] = Instantiate(highlightCellPrefab, new Vector3(0, 0, 0), Quaternion.identity, highlightParent);
            highlightCell[i].SetActive(false);
        }
    }

    public void AIMove(bool white) {
        ai.CalculateNextMove(white);
        Vector2Int chessToMove = ai.GetChessPieceToMove();
        MoveFigure(new Vector3(chessToMove.x, chessToMove.y), ai.GetBestMove());
    }

    // TODO: Remove finding chess piece by it's position
    public void MoveFigure(Vector3 oldPos, Vector2Int newPos) {
        if(!boardData.MoveFigure(new Vector2Int((int) oldPos.x, (int) oldPos.y), newPos)) {
            return;
        }

        
        Vector3 newPos3D = new Vector3(newPos.x, newPos.y, oldPos.z);
        int size = gameFigures.Length;
        for(int i = 0; i < size; i++) {
            if(gameFigures[i].transform.position == newPos3D) {
                gameFigures[i].SetActive(false);
                gameFigures[i].transform.position = new Vector3(-10 * i, -10 * i, -1);
                break;
            }
        }

        for(int i = 0; i < size; i++) {
            if(gameFigures[i].transform.position == oldPos) {
                gameFigures[i].transform.position = newPos3D;
                break;
            }
        }
        
        UnhighlightPreviousMoves();
    }
    
    public void HighlightPossibleMoves(Vector2Int pos) {
        UnhighlightPreviousMoves();
        FigureType figureType = boardData.GetFigureType(pos);
        savedPossibleMoves = boardData.GetPossibleMoves(figureType, pos);
        int size = savedPossibleMoves.Length;
        for(int i = 0; i < size; i++) {
            highlightCell[i].transform.position = new Vector3(savedPossibleMoves[i].x, savedPossibleMoves[i].y);
            highlightCell[i].SetActive(true);
        }
    }

    private void UnhighlightPreviousMoves(){
        for(int i = 0; i < maxHighlightCell; i++) {
            if(highlightCell[i].activeSelf) {
                highlightCell[i].SetActive(false);
            } else {
                break;
            }
        }
    }

    public bool IsLegalMove(Vector2Int pos) {
        if(savedPossibleMoves == null) {
            return false;
        }
        
        if(boardData.IsWithinGameBoard(pos)) {
            for(int i = 0; i < savedPossibleMoves.Length; i++) {
                if(pos == savedPossibleMoves[i]) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsCellOccupiedWithFiguresOfPlayerColor(Vector2Int pos) {
        return boardData.IsCellOccupied(playerPlaysWhite ? FigureType.White : FigureType.Black, pos);
    }
    
    private void OnGUI() {
        if(figureBoardToShow == FigureType.Empty) {
            return;
        }

        long boardToShow = boardData.GetBoardByReference(figureBoardToShow);

        string toShow = Convert.ToString(boardToShow, 2).PadLeft(64, '0');
        int index = 0;
        for(int i = 1; i <= boardData.BoardSize; i++) {
            toShow = toShow.Insert(boardData.BoardSize * i + index, "\n");
            index++;
        }
        GUI.BeginGroup(new Rect(10, 10, 100, 200));
        GUI.Box(new Rect(0, 0, 60, 125), "");
        GUI.Label(new Rect(0, 0, 60, 125), toShow);
        GUI.EndGroup();
    }
}

[Serializable]
struct Figures {
    public GameObject king;
    public GameObject queen;
    public GameObject rook;
    public GameObject bishop;
    public GameObject knight;
    public GameObject pawn;
}