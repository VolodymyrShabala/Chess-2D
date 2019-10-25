using UnityEngine;
using System.Collections.Generic;

public class BoardData {
    private readonly FigureData figureData;
    public int BoardSize => 8;

    private long whiteFiguresBoard = 0L;
    private long blackFiguresBoard = 0L;
    private long kingsBoard =        0L;
    private long queensBoard =       0L;
    private long rooksBoard =        0L;
    private long bishopsBoard =      0L;
    private long knightsBoard =      0L;
    private long pawnsBoard =        0L;
    private long dummyBoard =        0L;
    
    #region Initialise Board Data
    public BoardData() {
        FillBoardData();
        figureData = new FigureData(this);
    }
    
    private void FillBoardData() {
        // Rooks
        SetCellOccupied(FigureType.Rook, new Vector2Int(0, 0));
        SetCellOccupied(FigureType.Rook, new Vector2Int(7, 0));
        SetCellOccupied(FigureType.Rook, new Vector2Int(0, 7));
        SetCellOccupied(FigureType.Rook, new Vector2Int(7, 7));
        
        // Knights
        SetCellOccupied(FigureType.Knight, new Vector2Int(1, 0));
        SetCellOccupied(FigureType.Knight, new Vector2Int(6, 0));
        SetCellOccupied(FigureType.Knight, new Vector2Int(1, 7));
        SetCellOccupied(FigureType.Knight, new Vector2Int(6, 7));
        
        // Bishops
        SetCellOccupied(FigureType.Bishop, new Vector2Int(2, 0));
        SetCellOccupied(FigureType.Bishop, new Vector2Int(5, 0));
        SetCellOccupied(FigureType.Bishop, new Vector2Int(2, 7));
        SetCellOccupied(FigureType.Bishop, new Vector2Int(5, 7));
        
        //Queens
        SetCellOccupied(FigureType.Queen, new Vector2Int(3, 0));
        SetCellOccupied(FigureType.Queen, new Vector2Int(3, 7));
        
        // Kings
        SetCellOccupied(FigureType.King, new Vector2Int(4, 0));
        SetCellOccupied(FigureType.King, new Vector2Int(4, 7));
        
        for(int c = 0; c < BoardSize; c++) {
            SetCellOccupied(FigureType.Pawn, new Vector2Int(c, 1));
            SetCellOccupied(FigureType.Pawn, new Vector2Int(c, 6));
        }
        
        for(int r = 0; r <= 1; r++) {
            for(int c = 0; c < BoardSize; c++) {
                SetCellOccupied(FigureType.White, new Vector2Int(c, r));
            }
        }
        for(int r = 6; r <= 7; r++) {
            for(int c = 0; c < BoardSize; c++) {
                SetCellOccupied(FigureType.Black, new Vector2Int(c, r));
            }
        }
    }
    #endregion
    
    #region Main Functions
    public Vector2Int[] GetPossibleMoves(FigureType type, Vector2Int pos, bool whitesTurn = true) {
        return figureData.GetPossibleMoves(type, pos.x, pos.y, whitesTurn);
    }

    // TODO: Look into this function to make it prettier
    public void MoveFigure(Vector2Int from, Vector2Int to) {
        if(!IsCellOccupiedGlobal(from)) {
            Debug.Log("There is no chess piece by this coordinates.");
            return;
        }

        // Get figure type and free the corresponding board
        FigureType type = GetFigureType(from);
        SetCellFree(type, from);
           
        // Cell with that figure type is free. Now only color left. Get Color and remove if from corresponding color board
        FigureType color = GetFigureType(from);
        SetCellFree(color, from);

        // Do the same but for the new coordinates so we remove any chess piece there is
        FigureType oldType = GetFigureType(to);
        SetCellFree(oldType, to);

        FigureType oldColor = GetFigureType(to);
        SetCellFree(oldColor, to);

        // Set figure and it's color to a new position
        SetCellOccupied(type, to);
        SetCellOccupied(color, to);

        CheckWinConditions(oldType, oldColor);
    }

    private void CheckWinConditions(FigureType type, FigureType color){
        if(type != FigureType.King) {
            return;
        }

        Debug.Log(color == FigureType.White ? "Black won!" : "White won!");
    }
#endregion
    
    #region Helper Functions
    public Vector2Int[] GetAllChessPiecesByColor(bool white) {
        List<Vector2Int> moves = new List<Vector2Int>();

        for(int row = 0; row < BoardSize; row++) {
            for(int col = 0; col < BoardSize; col++) {
                Vector2Int pos = new Vector2Int(col, row);
                if(IsCellOccupied(white ? FigureType.White : FigureType.Black, pos)) {
                    moves.Add(pos);
                }
            }
        }

        return moves.ToArray();
    }

    public FigureType GetFigureType(Vector2Int pos) {
        if(!IsWithinGameBoard(pos)) {
            return FigureType.Empty;
        }
        for(int i = 0; i <= (int)FigureType.Empty; i++) {
            if(IsCellOccupied((FigureType)i, pos)) {
                return (FigureType)i;
            }
        }

        return FigureType.Empty;
    }
    
    public bool IsCellOccupied(FigureType boardType, Vector2Int pos) {
        int index = GetCellIndex(pos.x, pos.y);
        if(index == -1) {
            return true;
        }
        long mask = 1L << index;
        return (GetBoardByReference(boardType) & mask) != 0;
    }

    public bool IsCellOccupiedGlobal(Vector2Int pos) {
        int index = GetCellIndex(pos.x, pos.y);
        if(index == -1) {
            return true;
        }
        long mask = 1L << index;
        return ((whiteFiguresBoard | blackFiguresBoard) & mask) != 0;    
    }
    
    private void SetCellOccupied(FigureType boardType, Vector2Int pos) {
        int index = GetCellIndex(pos.x, pos.y);
        if(index == -1) {
            return;
        }

        long mask = 1L << index;
        GetBoardByReference(boardType) |= mask;
    }

    private void SetCellFree(FigureType boardType, Vector2Int pos) {
        int index = GetCellIndex(pos.x, pos.y);
        if(index == -1) {
            return;
        }
        
        long mask = 1L << index;
        GetBoardByReference(boardType) &= ~mask;
    }

    // Added this function to improve readability of the code
    private ref long GetBoardByReference(FigureType boardType){
        switch(boardType) {
            case FigureType.Pawn:
                return ref pawnsBoard;
            case FigureType.Rook:
                return ref rooksBoard;
            case FigureType.Knight:
                return ref knightsBoard;
            case FigureType.Bishop:
                return ref bishopsBoard;
            case FigureType.Queen:
                return ref queensBoard;
            case FigureType.King:
                return ref kingsBoard;
            case FigureType.White:
                return ref whiteFiguresBoard;
            case FigureType.Black:
                return ref blackFiguresBoard;
            case FigureType.Empty:
                break;
        }
        dummyBoard = 0L;
        return ref dummyBoard;
    }

    public bool IsWithinGameBoard(Vector2Int pos) {
        return pos.x >= 0 && pos.x < BoardSize && pos.y >= 0 && pos.y < BoardSize;
    }
    
    private int GetCellIndex(int col, int row) {
        if(!IsWithinGameBoard(new Vector2Int(col, row))) {
            return -1;
        }
        return (1 + row) * BoardSize - col - 1;
    }
    #endregion
}