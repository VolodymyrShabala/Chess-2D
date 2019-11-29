using UnityEngine;

public class AIOpponent{
    private readonly BoardData boardData;
    private readonly CellValue cellValue;
    private readonly int difficulty;
    private Move trueMove;
    
    public AIOpponent(BoardData board, int computerDifficulty, BoardController newController){
        boardData = board;
        difficulty = computerDifficulty;
        cellValue = new CellValue(board);
    }

    public void CalculateNextMove(bool white){
        trueMove = new Move();
        Negamax(new Vector2Int(0, 0), difficulty, white);
    }

    private int Negamax(Vector2Int cell, int depth, bool white){
        if(depth == 0 || (boardData.IsCellOccupied(FigureType.King, cell) &&
                          boardData.IsCellOccupied(white ? FigureType.Black : FigureType.White, cell))) {
            return Evaluate(boardData.GetFigureType(cell), cell);
        }

        int evaluation = int.MinValue;
        Vector2Int[] whitePieces = boardData.GetAllChessPiecesByColor(white);
        foreach(Vector2Int piece in whitePieces) {
            FigureType type = boardData.GetFigureType(piece);
            Vector2Int[] possibleMoves = boardData.GetPossibleMoves(type, piece, white);
            foreach(Vector2Int position in possibleMoves) {
                int i = -Negamax(position, depth - 1, !white);
                
                if(i > evaluation) {
                    evaluation = i;
                    trueMove.from = piece;
                    trueMove.to = position;
                }
            }
        }

        return evaluation;
    }
    
    private int Evaluate(FigureType type, Vector2Int cell) {
        return cellValue.GetCellValue(type, cell);
    }
    
    public Vector2Int GetChessPieceToMove() {
        return trueMove.from;
    }

    public Vector2Int GetBestMove() {
        return trueMove.to;
    }
}

public struct Move{
    public Vector2Int from;
    public Vector2Int to;
}