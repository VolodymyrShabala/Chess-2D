using UnityEngine;

public class CellValue{
    private readonly BoardData boardData;
    private readonly int boardSize;
    
    private readonly int[] pawnBoardValues = new[] {
                 0,  0,  0,  0,  0,  0,  0,  0,
                50, 50, 50, 50, 50, 50, 50, 50,
                10, 10, 20, 30, 30, 20, 10, 10,
                 5,  5, 10, 25, 25, 10,  5,  5,
                 0,  0,  0, 20, 20,  0,  0,  0,
                 5, -5,-10,  0,  0,-10, -5,  5,
                 5, 10, 10,-20,-20, 10, 10,  5,
                 0,  0,  0,  0,  0,  0,  0,  0
    };

    private readonly int[] pawnBoardMirroredValues = new[] {
                0,  0,  0,  0,  0,  0,  0,  0,
                5, 10, 10,-20,-20, 10, 10,  5,
                5, -5,-10,  0,  0,-10, -5,  5,
                0,  0,  0, 20, 20,  0,  0,  0,
                5,  5, 10, 25, 25, 10,  5,  5,
                10, 10, 20, 30, 30, 20, 10, 10,
                50, 50, 50, 50, 50, 50, 50, 50,
                0,  0,  0,  0,  0,  0,  0,  0
    };

    private readonly int[] knightBoardValues = {
                -50,-40,-30,-30,-30,-30,-40,-50,
                -40,-20,  0,  0,  0,  0,-20,-40,
                -30,  0, 10, 15, 15, 10,  0,-30,
                -30,  5, 15, 20, 20, 15,  5,-30,
                -30,  0, 15, 20, 20, 15,  0,-30,
                -30,  5, 10, 15, 15, 10,  5,-30,
                -40,-20,  0,  5,  5,  0,-20,-40,
                -50,-40,-30,-30,-30,-30,-40,-50,
    };

    private readonly int[] knightBoardMirroredValues = {
                -50,-40,-30,-30,-30,-30,-40,-50,
                -40,-20,  0,  5,  5,  0,-20,-40,
                -30,  5, 10, 15, 15, 10,  5,-30,
                -30,  0, 15, 20, 20, 15,  0,-30,
                -30,  5, 15, 20, 20, 15,  5,-30,
                -30,  0, 10, 15, 15, 10,  0,-30,
                -40,-20,  0,  0,  0,  0,-20,-40,
                -50,-40,-30,-30,-30,-30,-40,-50
    };

    private readonly int[] bishopBoardValues = {
                -20,-10,-10,-10,-10,-10,-10,-20,
                -10,  0,  0,  0,  0,  0,  0,-10,
                -10,  0,  5, 10, 10,  5,  0,-10,
                -10,  5,  5, 10, 10,  5,  5,-10,
                -10,  0, 10, 10, 10, 10,  0,-10,
                -10, 10, 10, 10, 10, 10, 10,-10,
                -10,  5,  0,  0,  0,  0,  5,-10,
                -20,-10,-10,-10,-10,-10,-10,-20,
    };

    private readonly int[] bishopBoardMirroredValues = {
                -20,-10,-10,-10,-10,-10,-10,-20,
                -10,  5,  0,  0,  0,  0,  5,-10,
                -10, 10, 10, 10, 10, 10, 10,-10,
                -10,  0, 10, 10, 10, 10,  0,-10,
                -10,  5,  5, 10, 10,  5,  5,-10,
                -10,  0,  5, 10, 10,  5,  0,-10,
                -10,  0,  0,  0,  0,  0,  0,-10,
                -20,-10,-10,-10,-10,-10,-10,-20
    };

    private readonly int[] rookBoardValues = {
                  0,  0,  0,  0,  0,  0,  0,  0,
                  5, 10, 10, 10, 10, 10, 10,  5,
                 -5,  0,  0,  0,  0,  0,  0, -5,
                 -5,  0,  0,  0,  0,  0,  0, -5,
                 -5,  0,  0,  0,  0,  0,  0, -5,
                 -5,  0,  0,  0,  0,  0,  0, -5,
                 -5,  0,  0,  0,  0,  0,  0, -5,
                  0,  0,  0,  5,  5,  0,  0,  0
    };

    private readonly int[] rookBoardMirroredValues = {
                0,  0,  0,  5,  5,  0,  0,  0,
                -5,  0,  0,  0,  0,  0,  0, -5,
                -5,  0,  0,  0,  0,  0,  0, -5,
                -5,  0,  0,  0,  0,  0,  0, -5,
                -5,  0,  0,  0,  0,  0,  0, -5,
                -5,  0,  0,  0,  0,  0,  0, -5,
                5, 10, 10, 10, 10, 10, 10,  5,
                0,  0,  0,  0,  0,  0,  0,  0
    };

    private readonly int[] queenBoardValues = {
                -20,-10,-10, -5, -5,-10,-10,-20,
                -10,  0,  0,  0,  0,  0,  0,-10,
                -10,  0,  5,  5,  5,  5,  0,-10,
                 -5,  0,  5,  5,  5,  5,  0, -5,
                  0,  0,  5,  5,  5,  5,  0, -5,
                -10,  5,  5,  5,  5,  5,  0,-10,
                -10,  0,  5,  0,  0,  0,  0,-10,
                -20,-10,-10, -5, -5,-10,-10,-20
    };

    private readonly int[] queenBoardMirroredValues = {
                -20,-10,-10, -5, -5,-10,-10,-20,
                -10,  0,  5,  0,  0,  0,  0,-10,
                -10,  5,  5,  5,  5,  5,  0,-10,
                 0,  0,  5,  5,  5,  5,  0, -5,
                -5,  0,  5,  5,  5,  5,  0, -5,
                -10,  0,  5,  5,  5,  5,  0,-10,
                -10,  0,  0,  0,  0,  0,  0,-10,
                -20,-10,-10, -5, -5,-10,-10,-20
    };

    private readonly int[] kingBoardMiddleGameValues = {
                -30,-40,-40,-50,-50,-40,-40,-30,
                -30,-40,-40,-50,-50,-40,-40,-30,
                -30,-40,-40,-50,-50,-40,-40,-30,
                -30,-40,-40,-50,-50,-40,-40,-30,
                -20,-30,-30,-40,-40,-30,-30,-20,
                -10,-20,-20,-20,-20,-20,-20,-10,
                 20, 20,  0,  0,  0,  0, 20, 20,
                 20, 30, 10,  0,  0, 10, 30, 20
    };

    private readonly int[] kingBoardMirroredMiddleGameValues = {
                 20, 30, 10,  0,  0, 10, 30, 20,
                 20, 20,  0,  0,  0,  0, 20, 20,
                -10,-20,-20,-20,-20,-20,-20,-10,
                -20,-30,-30,-40,-40,-30,-30,-20,
                -30,-40,-40,-50,-50,-40,-40,-30,
                -30,-40,-40,-50,-50,-40,-40,-30,
                -30,-40,-40,-50,-50,-40,-40,-30,
                -30,-40,-40,-50,-50,-40,-40,-30
    };

    //private int[] kingBoardLateGameValues = new int[] {
    //            -50,-40,-30,-20,-20,-30,-40,-50,
    //            -30,-20,-10,  0,  0,-10,-20,-30,
    //            -30,-10, 20, 30, 30, 20,-10,-30,
    //            -30,-10, 30, 40, 40, 30,-10,-30,
    //            -30,-10, 30, 40, 40, 30,-10,-30,
    //            -30,-10, 20, 30, 30, 20,-10,-30,
    //            -30,-30,  0,  0,  0,  0,-30,-30,
    //            -50,-30,-30,-30,-30,-30,-30,-50
    //};

    //private int[] kingBoardMirroredLateGameValues = new int[] {
    //            -50,-30,-30,-30,-30,-30,-30,-50,
    //            -30,-30,  0,  0,  0,  0,-30,-30,
    //            -30,-10, 20, 30, 30, 20,-10,-30,
    //            -30,-10, 30, 40, 40, 30,-10,-30,
    //            -30,-10, 30, 40, 40, 30,-10,-30,
    //            -30,-10, 20, 30, 30, 20,-10,-30,
    //            -30,-20,-10,  0,  0,-10,-20,-30,
    //            -50,-40,-30,-20,-20,-30,-40,-50
    //};

    public CellValue(BoardData boardData){
        this.boardData = boardData;
        boardSize = boardData.BoardSize;
    }

    public int GetCellValue(FigureType figureType, Vector2Int cell, bool white = true){
        int value = 0;
        switch(figureType) {
            case FigureType.Pawn:
                value = white ? pawnBoardValues[cell.y * boardSize + cell.x] : pawnBoardMirroredValues[cell.y * boardSize + cell.x];
                if(boardData.IsCellOccupied(boardData.GetFigureType(cell), cell) && (boardData.IsCellOccupied(white ? FigureType.Black : FigureType.White, cell))) {
                    value += GetFigureValue(figureType, white);
                }

                break;
            case FigureType.Rook:
                value =  white ? rookBoardValues[cell.y * boardSize + cell.x] : rookBoardMirroredValues[cell.y * boardSize + cell.x];
                if(boardData.IsCellOccupied(boardData.GetFigureType(cell), cell) && (boardData.IsCellOccupied(white ? FigureType.Black : FigureType.White, cell))) {
                    value += GetFigureValue(figureType, white);
                }

                break;
            case FigureType.Knight:
                value =  white ? knightBoardValues[cell.y * boardSize + cell.x] : knightBoardMirroredValues[cell.y * boardSize + cell.x];
                if(boardData.IsCellOccupied(boardData.GetFigureType(cell), cell) && (boardData.IsCellOccupied(white ? FigureType.Black : FigureType.White, cell))) {
                    value += GetFigureValue(figureType, white);
                }

                break;
            case FigureType.Bishop:
                value = white ? bishopBoardValues[cell.y * boardSize + cell.x] : bishopBoardMirroredValues[cell.y * boardSize + cell.x];
                if(boardData.IsCellOccupied(boardData.GetFigureType(cell), cell) && (boardData.IsCellOccupied(white ? FigureType.Black : FigureType.White, cell))) {
                    value += GetFigureValue(figureType, white);
                }

                break;
            case FigureType.Queen:
                value =  white ? queenBoardValues[cell.y * boardSize + cell.x] : queenBoardMirroredValues[cell.y * boardSize + cell.x];
                if(boardData.IsCellOccupied(boardData.GetFigureType(cell), cell) && (boardData.IsCellOccupied(white ? FigureType.Black : FigureType.White, cell))) {
                    value += GetFigureValue(figureType, white);
                }

                break;
            case FigureType.King:
                value = white ? kingBoardMiddleGameValues[cell.y * boardSize + cell.x] : kingBoardMirroredMiddleGameValues[cell.y * boardSize + cell.x];
                if(boardData.IsCellOccupied(boardData.GetFigureType(cell), cell) && (boardData.IsCellOccupied(white ? FigureType.Black : FigureType.White, cell))) {
                    value += GetFigureValue(figureType, white);
                }

                break;
            default:
                Debug.Log("Illegal call to GetCellValue. You can call this only on figures.");
                break;
        }

        return value;
    }
    
    private int GetFigureValue(FigureType type, bool white) {
        switch(type) {
            case FigureType.Pawn:
                return white ? -10 :  10;
            case FigureType.Rook:
                return white ? -30 :  30;
            case FigureType.Knight:
                return white ? -30 :  30;
            case FigureType.Bishop:
                return white ? -50 :  50;
            case FigureType.Queen:
                return white ? -90 :  90;
            case FigureType.King:
                return white ? -900 : 900;
            default:
                Debug.Log("Illegal call to GetFigureValue. You can call this only on figures.");
                break;
        }

        return 0;
    }
}