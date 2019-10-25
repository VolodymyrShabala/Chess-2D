using UnityEngine;

public class AIOpponent {
    private readonly BoardData boardData;
    private readonly CellValue cellValue;
    private readonly int difficulty;
    private FigureType figureType = FigureType.Empty;
    private Vector2Int tempBestMove;
    private Vector2Int bestMove;

    private Vector2Int figureToMove;

    public AIOpponent(BoardData board, int computerDifficulty) {
        boardData = board;
        difficulty = computerDifficulty;
        cellValue = new CellValue(board);
    }

    public void CalculateNextMove(bool white = false) {
        Vector2Int[] gamePieces = boardData.GetAllChessPiecesByColor(white);
        int index = white ? -int.MaxValue : int.MaxValue;
        foreach(Vector2Int cell in gamePieces) {
            figureType = boardData.GetFigureType(cell);
            int i = AlphaBetaPruning(cell, difficulty, -int.MaxValue, int.MaxValue, white);
            if(white) {
                if(i > index) {
                    index = i;
                    figureToMove = cell;
                    bestMove = tempBestMove;
                }
            } else {
                if(i < index) {
                    index = i;
                    figureToMove = cell;
                    bestMove = tempBestMove;
                }
            }
        }
    }

    // TODO: Sometimes returns infinity
    private int AlphaBetaPruning(Vector2Int cell, int depth, int alpha = -int.MaxValue, int beta = int.MaxValue, bool maximizingPlayer = false) {
        if(depth == 0 || (boardData.IsCellOccupied(FigureType.King, cell) &&
                               boardData.IsCellOccupied(maximizingPlayer ? FigureType.Black : FigureType.White, cell))) {
            return Evaluate(cell);
        }

        if(maximizingPlayer) {
            int maxEvaluation = -int.MaxValue;
            Vector2Int[] possiblePositions = boardData.GetPossibleMoves(figureType, cell, true);
            foreach(Vector2Int position in possiblePositions) {
                int evaluation = AlphaBetaPruning(position, depth - 1, alpha, beta, false);
                if(evaluation > maxEvaluation) {
                    alpha = evaluation;
                    maxEvaluation = evaluation;
                    tempBestMove = position;
                }
                if(beta <= alpha) {
                    break;
                }
            }
            
            // Debug.Log("Returning max: " + maxEvaluation + ". Alpha is: " + alpha);
            return maxEvaluation;
        } else {
            int minEvaluation = int.MaxValue;
            Vector2Int[] possiblePositions = boardData.GetPossibleMoves(figureType, cell, false);
            foreach(Vector2Int position in possiblePositions) {
                int evaluation = AlphaBetaPruning(position, depth - 1, alpha, beta, true);
                if(evaluation < minEvaluation) {
                    beta = evaluation;
                    minEvaluation = evaluation;
                    tempBestMove = position;
                }
                if(beta <= alpha) {
                    break;
                }
            }
            
            // Debug.Log("Returning min: " + minEvaluation + ". Beta is: " + beta);
            return minEvaluation;
        }
    }

    private int Evaluate(Vector2Int cell) {
        return cellValue.GetCellValue(figureType, cell);
    }
    
    public Vector2Int GetChessPieceToMove() {
        return figureToMove;
    }

    public Vector2Int GetBestMove() {
        return bestMove;
    }
}