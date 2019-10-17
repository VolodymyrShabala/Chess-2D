using UnityEngine;

public class AIOpponent {
    private BoardData boardData;
    private int difficulty;
    private FigureType figureType = FigureType.Empty;
    private Vector2Int tempBestMove;
    private Vector2Int bestMove;

    private Vector2Int figureToMove;

    public AIOpponent(BoardData board, int computerDifficulty) {
        boardData = board;
        difficulty = computerDifficulty;
    }

    //TODO: Rewrite for better reading/understanding
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

    public Vector2Int GetChessPieceToMove() {
        return figureToMove;
    }

    public Vector2Int GetBestMove() {
        return bestMove;
    }

    // MiniMax
    private int AlphaBetaPruning(Vector2Int cell, int depth, int alpha = -int.MaxValue, int beta = int.MaxValue, bool maximazingPlayer = false) {
        if(depth == 0 || (boardData.IsCellOccupied(FigureType.King, cell) &&
                               boardData.IsCellOccupied(maximazingPlayer ? FigureType.Black : FigureType.White, cell))) {
            return Evaluate(cell);
        }

        if(maximazingPlayer) {
            int maxEvaluation = -int.MaxValue;
            Vector2Int[] possiblePositions = boardData.GetPossibleMoves(figureType, cell, maximazingPlayer);
            foreach(Vector2Int position in possiblePositions) {
                int evaluation = AlphaBetaPruning(position, depth - 1, alpha, beta, false);
                maxEvaluation = Mathf.Max(maxEvaluation, evaluation);
                alpha = Mathf.Max(alpha, evaluation);
                if(evaluation > alpha) {
                    alpha = evaluation;
                    maxEvaluation = evaluation;
                    tempBestMove = position;
                }
                if(beta <= alpha) {
                    break;
                }
            }
            return maxEvaluation;
        } else {
            int minEvaluation = int.MaxValue;
            Vector2Int[] possiblePositions = boardData.GetPossibleMoves(figureType, cell, maximazingPlayer);
            foreach(Vector2Int position in possiblePositions) {
                int evaluation = AlphaBetaPruning(position, depth - 1, alpha, beta, true);
                if(evaluation < beta) {
                    beta = evaluation;
                    minEvaluation = evaluation;
                    tempBestMove = position;
                }
                if(beta <= alpha) {
                    break;
                }
            }
            return minEvaluation;
        }
    }

    private int Evaluate(Vector2Int cell) {
        return boardData.figureData.GetCellValue(figureType, cell);

        float pieceDifference = 0;
        float whiteWeight = 0;
        float blackWeight = 0;

        Vector2Int[] whites = boardData.GetAllChessPiecesByColor(true);
        foreach(Vector2Int chessPiece in whites) {
            whiteWeight += boardData.figureData.GetCellValue(boardData.GetFigureType(chessPiece), chessPiece, true);
        }

        Vector2Int[] blacks = boardData.GetAllChessPiecesByColor(true);
        foreach(Vector2Int chessPiece in whites) {
            blackWeight += boardData.figureData.GetCellValue(boardData.GetFigureType(chessPiece), chessPiece, false);
        }
        //Debug.Log("White score: " + whiteWeight);
        //Debug.Log("Black score: " + blackWeight);
        //Debug.Log("Main white score: " + boardData.WhiteScore);
        //Debug.Log("Main black score: " + boardData.BlackScore);
        pieceDifference = (boardData.BlackScore + (blackWeight / 100000)) - (boardData.WhiteScore + (whiteWeight / 100000));
        Debug.Log("Peice difference " + pieceDifference);
        //Debug.Log("Evaluation: " + Mathf.RoundToInt(pieceDifference * 100));
        return Mathf.RoundToInt(pieceDifference);
    }
}