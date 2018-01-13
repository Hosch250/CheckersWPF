﻿using Checkers;
using Microsoft.FSharp.Core;

namespace CheckersWPF.Facade
{
    public enum Player
    {
        White, Black
    }

    public enum PieceType
    {
        Checker,
        King
    }

    public enum Variant
    {
        AmericanCheckers,
        PoolCheckers
    }

    public static class Extensions
    {
        public static Player Convert(this Generic.Player value) =>
            value.IsBlack ? Player.Black : Player.White;

        public static Generic.Player ConvertBack(this Player value) =>
            value == Player.Black ? Generic.Player.Black : Generic.Player.White;

        public static Variant Convert(this Generic.Variant value) =>
            value.IsAmericanCheckers ? Variant.AmericanCheckers : Variant.PoolCheckers;

        public static Generic.Variant ConvertBack(this Variant value) =>
            value == Variant.AmericanCheckers ? Generic.Variant.AmericanCheckers : Generic.Variant.PoolCheckers;

        public static PieceType Convert(this Generic.PieceType value) =>
            Equals(value, Generic.PieceType.Checker) ? PieceType.Checker : PieceType.King;

        public static Generic.PieceType ConvertBack(this PieceType value) =>
            value == PieceType.Checker ? Generic.PieceType.Checker : Generic.PieceType.King;

        public static Variant ToVariant(this GameVariant.GameVariant gameVariant)
        {
            if (gameVariant.variant.IsAmericanCheckers)
            {
                return Variant.AmericanCheckers;
            }

            if (gameVariant.variant.IsPoolCheckers)
            {
                return Variant.PoolCheckers;
            }

            throw new System.ArgumentException("Unknown variant", nameof(gameVariant));
        }

        public static GameVariant.GameVariant ToGameVariant(this Variant variant)
        {
            switch (variant)
            {
                case Variant.AmericanCheckers:
                    return GameVariant.GameVariant.AmericanCheckers;
                case Variant.PoolCheckers:
                    return GameVariant.GameVariant.PoolCheckers;
                default:
                    throw new System.ArgumentException(nameof(variant));
            }
        }

        public static Piece Convert(this FSharpOption<Checkers.Piece.Piece> piece)
        {
            if (Equals(piece, Checkers.Piece.whiteChecker))
            {
                return Piece.WhiteChecker;
            }
            if (Equals(piece, Checkers.Piece.whiteKing))
            {
                return Piece.WhiteKing;
            }
            if (Equals(piece, Checkers.Piece.blackChecker))
            {
                return Piece.BlackChecker;
            }
            if (Equals(piece, Checkers.Piece.blackKing))
            {
                return Piece.BlackKing;
            }

            return null;
        }

        public static FSharpOption<Checkers.Piece.Piece> ConvertBack(this Piece piece)
        {
            if (Equals(piece, Piece.WhiteChecker))
            {
                return Checkers.Piece.whiteChecker;
            }
            if (Equals(piece, Piece.WhiteKing))
            {
                return Checkers.Piece.whiteKing;
            }
            if (Equals(piece, Piece.BlackChecker))
            {
                return Checkers.Piece.blackChecker;
            }
            if (Equals(piece, Piece.BlackKing))
            {
                return Checkers.Piece.blackKing;
            }

            return null;
        }
    }
}
