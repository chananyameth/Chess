using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Point
    {
        private int X;
        private int Y;
        public int x
        {
            get { return X; }
            set
            {
                if (value < Tools.None || Tools.one < value)
                    X = Tools.None;
                else
                    X = value;
            }
        }
        public int y
        {
            get { return Y; }
            set
            {
                if (value < Tools.None || Tools.one < value)
                    Y = Tools.None;
                else
                    Y = value;
            }
        }

        public Point()
        {
            x = Tools.None;
            y = Tools.None;
        }
        public Point(int letter, int number)
        {
            x = letter;
            y = number;
        }
        public static bool operator ==(Point p1, Point p2)
        {
            if (p1.x == p2.x && p1.y == p2.y)
                return true;
            return false;
        }
        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }
        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.x + p2.x, p1.y - p2.y);
        }
        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.x - p2.x, p1.y + p2.y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
                return this == (obj as Point);
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return x << 16 + y;
        }
        public override string ToString()
        {
            if (!Tools.inBoard(this))
                return string.Format("--");
            return string.Format("{0}{1}", (char)(x + 65), (8 - y));
        }
    }
    public class Square
    {
        public Point point
        {
            get
            {
                return chessPiece.currPosition;
            }
            set
            {
                chessPiece.currPosition = value;
            }
        }
        public ChessPiece chessPiece;

        public Square()
        {
            chessPiece = new None();
        }
        public Square(Kind kind, PlayerColor color, Point p)
        {
            switch (kind)
            {
                case Kind.bishop:
                    chessPiece = new Bishop();
                    break;
                case Kind.castle:
                    chessPiece = new Castle();
                    break;
                case Kind.king:
                    chessPiece = new King();
                    break;
                case Kind.knight:
                    chessPiece = new Knight();
                    break;
                case Kind.pawn:
                    chessPiece = new Pawn();
                    break;
                case Kind.queen:
                    chessPiece = new Queen();
                    break;
            }
            chessPiece.color = color;
            chessPiece.currPosition = p;
            chessPiece.initPosition = p;
        }
        public Square(ChessPiece ChessPiece)
        {
            chessPiece = ChessPiece;
            point = ChessPiece.currPosition;
        }
    }
    public class Board
    {
        public Square[,] board;
        public bool[] parameters;
        public List<ChessPiece> eaten;
        public List<ChessPiece> alive;
        public List<ChessPiece> moovedPieces;

        public Board()
        {
            board = new Square[8, 8];
            moovedPieces = new List<ChessPiece>();

            for (int i = 0; i < 8; i++)
                for (int j = Tools.three; j > Tools.seven; j--)//empty squares
                {
                    board[i, j] = new Square();
                    board[i, j].point.x = i;
                    board[i, j].point.y = j;
                }

            board[Tools.A, Tools.one] = new Square(Kind.castle, PlayerColor.White, new Point(Tools.A, Tools.one));
            board[Tools.B, Tools.one] = new Square(Kind.knight, PlayerColor.White, new Point(Tools.B, Tools.one));
            board[Tools.C, Tools.one] = new Square(Kind.bishop, PlayerColor.White, new Point(Tools.C, Tools.one));
            board[Tools.D, Tools.one] = new Square(Kind.queen, PlayerColor.White, new Point(Tools.D, Tools.one));
            board[Tools.E, Tools.one] = new Square(Kind.king, PlayerColor.White, new Point(Tools.E, Tools.one));
            board[Tools.F, Tools.one] = new Square(Kind.bishop, PlayerColor.White, new Point(Tools.F, Tools.one));
            board[Tools.G, Tools.one] = new Square(Kind.knight, PlayerColor.White, new Point(Tools.G, Tools.one));
            board[Tools.H, Tools.one] = new Square(Kind.castle, PlayerColor.White, new Point(Tools.H, Tools.one));
            board[Tools.A, Tools.eight] = new Square(Kind.castle, PlayerColor.Black, new Point(Tools.A, Tools.eight));
            board[Tools.B, Tools.eight] = new Square(Kind.knight, PlayerColor.Black, new Point(Tools.B, Tools.eight));
            board[Tools.C, Tools.eight] = new Square(Kind.bishop, PlayerColor.Black, new Point(Tools.C, Tools.eight));
            board[Tools.D, Tools.eight] = new Square(Kind.queen, PlayerColor.Black, new Point(Tools.D, Tools.eight));
            board[Tools.E, Tools.eight] = new Square(Kind.king, PlayerColor.Black, new Point(Tools.E, Tools.eight));
            board[Tools.F, Tools.eight] = new Square(Kind.bishop, PlayerColor.Black, new Point(Tools.F, Tools.eight));
            board[Tools.G, Tools.eight] = new Square(Kind.knight, PlayerColor.Black, new Point(Tools.G, Tools.eight));
            board[Tools.H, Tools.eight] = new Square(Kind.castle, PlayerColor.Black, new Point(Tools.H, Tools.eight));
            for (int i = 0; i < 8; i++)//from A to H
            {
                board[i, Tools.two] = new Square(Kind.pawn, PlayerColor.White, new Point(i, Tools.two));
                board[i, Tools.seven] = new Square(Kind.pawn, PlayerColor.Black, new Point(i, Tools.seven));
            }


            eaten = new List<ChessPiece>();
            eaten.Clear();
            alive = new List<ChessPiece>();
            for (int i = Tools.A; i <= Tools.H; i++)
            {
                alive.Add(board[i, Tools.eight].chessPiece);
                alive.Add(board[i, Tools.seven].chessPiece);
                alive.Add(board[i, Tools.two].chessPiece);
                alive.Add(board[i, Tools.one].chessPiece);
            }


            parameters = new bool[(int)Params.count];
            parameters[(int)Params.hatzracha] = false;
            parameters[(int)Params.pawn_at_end] = false;
            parameters[(int)Params.derech_hilucho] = false;
            parameters[(int)Params.d_h_0] = false;
            parameters[(int)Params.d_h_1] = false;
            parameters[(int)Params.d_h_2] = false;
        }
        public bool doTurn(Square from, Point to)
        {
            moovedPieces.Clear();

            if (from.chessPiece.color == board[to.x, to.y].chessPiece.color)//same
                return false;
            if (from.chessPiece.isBound)
                return false;
            if (!possibleDestinations(new Square(from.chessPiece)).Exists(t => t == to))
                return false;

            if (board[to.x, to.y].chessPiece.color == PlayerColor.None)//none - just move
                move(from, to);
            else if ((int)from.chessPiece.color == -(int)board[to.x, to.y].chessPiece.color)//pay attention to the minus
                eat(from, board[to.x, to.y]);                                               //different colors - eat

            return true;
        }
        public List<Point> impossibleDestinations(Square s)
        {
            ChessPiece c = s.chessPiece;

            switch (c.kind)
            {
                case Kind.bishop:
                    return c.allDestinations().Except(possibleDestinations_bishop(c)).ToList();
                case Kind.castle:
                    return c.allDestinations().Except(possibleDestinations_castle(c)).ToList();
                case Kind.king:
                    return c.allDestinations().Except(possibleDestinations_king(c)).ToList();
                case Kind.knight:
                    return c.allDestinations().Except(possibleDestinations_knight(c)).ToList();
                case Kind.none:
                    return c.allDestinations().Except(possibleDestinations_none(c)).ToList();
                case Kind.pawn:
                    return c.allDestinations().Except(possibleDestinations_pawn(c)).ToList();
                case Kind.queen:
                    return c.allDestinations().Except(possibleDestinations_queen(c)).ToList();
            }

            return new List<Point>();
        }
        public List<Point> possibleDestinations(Square s)
        {
            ChessPiece c = s.chessPiece;

            switch (c.kind)
            {
                case Kind.bishop:
                    return possibleDestinations_bishop(c);
                case Kind.castle:
                    return possibleDestinations_castle(c);
                case Kind.king:
                    return possibleDestinations_king(c);
                case Kind.knight:
                    return possibleDestinations_knight(c);
                case Kind.none:
                    return possibleDestinations_none(c);
                case Kind.pawn:
                    return possibleDestinations_pawn(c);
                case Kind.queen:
                    return possibleDestinations_queen(c);
            }

            return new List<Point>();
        }

        private List<Point> possibleDestinations_pawn(ChessPiece c)
        {
            List<Point> des = new List<Point>();

            if (c.currPosition.y == Tools.H)
                return des;

            if (board[c.currPosition.x, c.currPosition.y + (int)c.color].chessPiece.kind == Kind.none)//if free
            {
                des.Add(new Point(c.currPosition.x, c.currPosition.y + (int)c.color));//regular move
                if (c.currPosition == c.initPosition && board[c.currPosition.x, c.currPosition.y + (2 * (int)c.color)].chessPiece.kind == Kind.none)//if free
                    des.Add(new Point(c.currPosition.x, c.currPosition.y + (2 * (int)c.color)));//double move at beggining
            }


            if (c.currPosition.x < Tools.H &&
                board[c.currPosition.x + 1, c.currPosition.y + (int)c.color].chessPiece.kind != Kind.none &&
                board[c.currPosition.x + 1, c.currPosition.y + (int)c.color].chessPiece.color != c.color)
                des.Add(new Point(c.currPosition.x + 1, c.currPosition.y + (int)c.color));//eat right

            if (c.currPosition.x > Tools.A &&
                board[c.currPosition.x - 1, c.currPosition.y + (int)c.color].chessPiece.kind != Kind.none &&
                board[c.currPosition.x - 1, c.currPosition.y + (int)c.color].chessPiece.color != c.color)
                des.Add(new Point(c.currPosition.x - 1, c.currPosition.y + (int)c.color));//eat left

            //TODO: derech hilucho

            return des;
        }
        private List<Point> possibleDestinations_knight(ChessPiece c)
        {
            //all moves:
            //(+1,+2)
            //(+1,-2)         
            //(-1,+2)
            //(-1,-2)
            //(+2,+1)
            //(+2,-1)
            //(-2,+1)
            //(-2,-1)
            List<Point> destinati = new List<Point>();

            if (Tools.inBoard(new Point(c.currPosition.x + 1, c.currPosition.y + 2)) && board[c.currPosition.x + 1, c.currPosition.y + 2].chessPiece.color != c.color)
                destinati.Add(new Point(c.currPosition.x + 1, c.currPosition.y + 2));
            if (Tools.inBoard(new Point(c.currPosition.x + 1, c.currPosition.y - 2)) && board[c.currPosition.x + 1, c.currPosition.y - 2].chessPiece.color != c.color)
                destinati.Add(new Point(c.currPosition.x + 1, c.currPosition.y - 2));
            if (Tools.inBoard(new Point(c.currPosition.x - 1, c.currPosition.y + 2)) && board[c.currPosition.x - 1, c.currPosition.y + 2].chessPiece.color != c.color)
                destinati.Add(new Point(c.currPosition.x - 1, c.currPosition.y + 2));
            if (Tools.inBoard(new Point(c.currPosition.x - 1, c.currPosition.y - 2)) && board[c.currPosition.x - 1, c.currPosition.y - 2].chessPiece.color != c.color)
                destinati.Add(new Point(c.currPosition.x - 1, c.currPosition.y - 2));
            if (Tools.inBoard(new Point(c.currPosition.x + 2, c.currPosition.y + 1)) && board[c.currPosition.x + 2, c.currPosition.y + 1].chessPiece.color != c.color)
                destinati.Add(new Point(c.currPosition.x + 2, c.currPosition.y + 1));
            if (Tools.inBoard(new Point(c.currPosition.x + 2, c.currPosition.y - 1)) && board[c.currPosition.x + 2, c.currPosition.y - 1].chessPiece.color != c.color)
                destinati.Add(new Point(c.currPosition.x + 2, c.currPosition.y - 1));
            if (Tools.inBoard(new Point(c.currPosition.x - 2, c.currPosition.y + 1)) && board[c.currPosition.x - 2, c.currPosition.y + 1].chessPiece.color != c.color)
                destinati.Add(new Point(c.currPosition.x - 2, c.currPosition.y + 1));
            if (Tools.inBoard(new Point(c.currPosition.x - 2, c.currPosition.y - 1)) && board[c.currPosition.x - 2, c.currPosition.y - 1].chessPiece.color != c.color)
                destinati.Add(new Point(c.currPosition.x - 2, c.currPosition.y - 1));

            return destinati;
        }
        private List<Point> possibleDestinations_bishop(ChessPiece c)
        {
            List<Point> destination = new List<Point>();

            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x + i, c.currPosition.y + i)); i++)
            {
                if (board[c.currPosition.x + i, c.currPosition.y + i].chessPiece.color == PlayerColor.None)
                    destination.Add(new Point(c.currPosition.x + i, c.currPosition.y + i));
                else if (board[c.currPosition.x + i, c.currPosition.y + i].chessPiece.color != c.color)
                {
                    destination.Add(new Point(c.currPosition.x + i, c.currPosition.y + i));
                    break;
                }
                else if (board[c.currPosition.x + i, c.currPosition.y + i].chessPiece.color == c.color)
                    break;
            }
            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x + i, c.currPosition.y - i)); i++)
            {
                if (board[c.currPosition.x + i, c.currPosition.y - i].chessPiece.color == PlayerColor.None)
                    destination.Add(new Point(c.currPosition.x + i, c.currPosition.y - i));
                else if (board[c.currPosition.x + i, c.currPosition.y - i].chessPiece.color != c.color)
                {
                    destination.Add(new Point(c.currPosition.x + i, c.currPosition.y - i));
                    break;
                }
                else if (board[c.currPosition.x + i, c.currPosition.y - i].chessPiece.color == c.color)
                    break;
            }
            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x - i, c.currPosition.y + i)); i++)
            {
                if (board[c.currPosition.x - i, c.currPosition.y + i].chessPiece.color == PlayerColor.None)
                    destination.Add(new Point(c.currPosition.x - i, c.currPosition.y + i));
                else if (board[c.currPosition.x - i, c.currPosition.y + i].chessPiece.color != c.color)
                {
                    destination.Add(new Point(c.currPosition.x - i, c.currPosition.y + i));
                    break;
                }
                else if (board[c.currPosition.x - i, c.currPosition.y + i].chessPiece.color == c.color)
                    break;
            }
            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x - i, c.currPosition.y - i)); i++)
            {
                if (board[c.currPosition.x - i, c.currPosition.y - i].chessPiece.color == PlayerColor.None)
                    destination.Add(new Point(c.currPosition.x - i, c.currPosition.y - i));
                else if (board[c.currPosition.x - i, c.currPosition.y - i].chessPiece.color != c.color)
                {
                    destination.Add(new Point(c.currPosition.x - i, c.currPosition.y - i));
                    break;
                }
                else if (board[c.currPosition.x - i, c.currPosition.y - i].chessPiece.color == c.color)
                    break;
            }

            return destination;
        }
        private List<Point> possibleDestinations_castle(ChessPiece c)
        {
            List<Point> destination = new List<Point>();

            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x + i, c.currPosition.y)); i++)
            {
                if (board[c.currPosition.x + i, c.currPosition.y].chessPiece.color == PlayerColor.None)
                    destination.Add(new Point(c.currPosition.x + i, c.currPosition.y));
                else if (board[c.currPosition.x + i, c.currPosition.y].chessPiece.color != c.color)
                {
                    destination.Add(new Point(c.currPosition.x + i, c.currPosition.y));
                    break;
                }
                else if (board[c.currPosition.x + i, c.currPosition.y].chessPiece.color == c.color)
                    break;
            }
            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x - i, c.currPosition.y)); i++)
            {
                if (board[c.currPosition.x - i, c.currPosition.y].chessPiece.color == PlayerColor.None)
                    destination.Add(new Point(c.currPosition.x - i, c.currPosition.y));
                else if (board[c.currPosition.x - i, c.currPosition.y].chessPiece.color != c.color)
                {
                    destination.Add(new Point(c.currPosition.x - i, c.currPosition.y));
                    break;
                }
                else if (board[c.currPosition.x - i, c.currPosition.y].chessPiece.color == c.color)
                    break;
            }
            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x, c.currPosition.y + i)); i++)
            {
                if (board[c.currPosition.x, c.currPosition.y + i].chessPiece.color == PlayerColor.None)
                    destination.Add(new Point(c.currPosition.x, c.currPosition.y + i));
                else if (board[c.currPosition.x, c.currPosition.y + i].chessPiece.color != c.color)
                {
                    destination.Add(new Point(c.currPosition.x, c.currPosition.y + i));
                    break;
                }
                else if (board[c.currPosition.x, c.currPosition.y + i].chessPiece.color == c.color)
                    break;
            }
            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x, c.currPosition.y - i)); i++)
            {
                if (board[c.currPosition.x, c.currPosition.y - i].chessPiece.color == PlayerColor.None)
                    destination.Add(new Point(c.currPosition.x, c.currPosition.y - i));
                else if (board[c.currPosition.x, c.currPosition.y - i].chessPiece.color != c.color)
                {
                    destination.Add(new Point(c.currPosition.x, c.currPosition.y - i));
                    break;
                }
                else if (board[c.currPosition.x, c.currPosition.y - i].chessPiece.color == c.color)
                    break;
            }

            return destination;
        }
        private List<Point> possibleDestinations_king(ChessPiece c)
        {
            List<Point> destinati = new List<Point>();

            if (Tools.inBoard(new Point(c.currPosition.x + 1, c.currPosition.y - 0)) && board[c.currPosition.x + 1, c.currPosition.y - 0].chessPiece.color != c.color)//right
                destinati.Add(new Point(c.currPosition.x + 1, c.currPosition.y - 0));
            if (Tools.inBoard(new Point(c.currPosition.x + 1, c.currPosition.y + 1)) && board[c.currPosition.x + 1, c.currPosition.y + 1].chessPiece.color != c.color)//up-right
                destinati.Add(new Point(c.currPosition.x + 1, c.currPosition.y + 1));
            if (Tools.inBoard(new Point(c.currPosition.x + 0, c.currPosition.y + 1)) && board[c.currPosition.x + 0, c.currPosition.y + 1].chessPiece.color != c.color)//up
                destinati.Add(new Point(c.currPosition.x + 0, c.currPosition.y + 1));
            if (Tools.inBoard(new Point(c.currPosition.x - 1, c.currPosition.y + 1)) && board[c.currPosition.x - 1, c.currPosition.y + 1].chessPiece.color != c.color)//up-left
                destinati.Add(new Point(c.currPosition.x - 1, c.currPosition.y + 1));
            if (Tools.inBoard(new Point(c.currPosition.x - 1, c.currPosition.y - 0)) && board[c.currPosition.x - 1, c.currPosition.y - 0].chessPiece.color != c.color)//left
                destinati.Add(new Point(c.currPosition.x - 1, c.currPosition.y - 0));
            if (Tools.inBoard(new Point(c.currPosition.x - 1, c.currPosition.y - 1)) && board[c.currPosition.x - 1, c.currPosition.y - 1].chessPiece.color != c.color)//down-left
                destinati.Add(new Point(c.currPosition.x - 1, c.currPosition.y - 1));
            if (Tools.inBoard(new Point(c.currPosition.x - 0, c.currPosition.y - 1)) && board[c.currPosition.x - 0, c.currPosition.y - 1].chessPiece.color != c.color)//down
                destinati.Add(new Point(c.currPosition.x - 0, c.currPosition.y - 1));
            if (Tools.inBoard(new Point(c.currPosition.x + 1, c.currPosition.y - 1)) && board[c.currPosition.x + 1, c.currPosition.y - 1].chessPiece.color != c.color)//down-right
                destinati.Add(new Point(c.currPosition.x + 1, c.currPosition.y - 1));

            //Hatzracha
            if (!c.moved)
            {
                if (board[c.currPosition.x + 1, c.currPosition.y].chessPiece.color == PlayerColor.None &&
                    board[c.currPosition.x + 2, c.currPosition.y].chessPiece.color == PlayerColor.None &&
                    board[c.currPosition.x + 3, c.currPosition.y].chessPiece.moved == false)
                    destinati.Add(new Point(c.currPosition.x + 2, c.currPosition.y));
                if (board[c.currPosition.x - 1, c.currPosition.y].chessPiece.color == PlayerColor.None &&
                    board[c.currPosition.x - 2, c.currPosition.y].chessPiece.color == PlayerColor.None &&
                    board[c.currPosition.x - 3, c.currPosition.y].chessPiece.color == PlayerColor.None &&
                    board[c.currPosition.x - 4, c.currPosition.y].chessPiece.moved == false)
                    destinati.Add(new Point(c.currPosition.x - 2, c.currPosition.y));
            }

            return destinati;
        }
        private List<Point> possibleDestinations_queen(ChessPiece c)
        {
            List<Point> destination_moreSpace = new List<Point>();//moreSpace to look good...

            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x + i, c.currPosition.y - 0)); i++)//right
            {
                if (board[c.currPosition.x + i, c.currPosition.y - 0].chessPiece.color == PlayerColor.None)
                    destination_moreSpace.Add(new Point(c.currPosition.x + i, c.currPosition.y - 0));
                else if (board[c.currPosition.x + i, c.currPosition.y - 0].chessPiece.color != c.color)
                {
                    destination_moreSpace.Add(new Point(c.currPosition.x + i, c.currPosition.y - 0));
                    break;
                }
                else if (board[c.currPosition.x + i, c.currPosition.y - 0].chessPiece.color == c.color)
                    break;
            }
            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x + i, c.currPosition.y + i)); i++)//up-right
            {
                if (board[c.currPosition.x + i, c.currPosition.y + i].chessPiece.color == PlayerColor.None)
                    destination_moreSpace.Add(new Point(c.currPosition.x + i, c.currPosition.y + i));
                else if (board[c.currPosition.x + i, c.currPosition.y + i].chessPiece.color != c.color)
                {
                    destination_moreSpace.Add(new Point(c.currPosition.x + i, c.currPosition.y + i));
                    break;
                }
                else if (board[c.currPosition.x + i, c.currPosition.y + i].chessPiece.color == c.color)
                    break;
            }
            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x + 0, c.currPosition.y + i)); i++)//up
            {
                if (board[c.currPosition.x + 0, c.currPosition.y + i].chessPiece.color == PlayerColor.None)
                    destination_moreSpace.Add(new Point(c.currPosition.x + 0, c.currPosition.y + i));
                else if (board[c.currPosition.x + 0, c.currPosition.y + i].chessPiece.color != c.color)
                {
                    destination_moreSpace.Add(new Point(c.currPosition.x + 0, c.currPosition.y + i));
                    break;
                }
                else if (board[c.currPosition.x + 0, c.currPosition.y + i].chessPiece.color == c.color)
                    break;
            }
            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x - i, c.currPosition.y + i)); i++)//up-left
            {
                if (board[c.currPosition.x - i, c.currPosition.y + i].chessPiece.color == PlayerColor.None)
                    destination_moreSpace.Add(new Point(c.currPosition.x - i, c.currPosition.y + i));
                else if (board[c.currPosition.x - i, c.currPosition.y + i].chessPiece.color != c.color)
                {
                    destination_moreSpace.Add(new Point(c.currPosition.x - i, c.currPosition.y + i));
                    break;
                }
                else if (board[c.currPosition.x - i, c.currPosition.y + i].chessPiece.color == c.color)
                    break;
            }
            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x - i, c.currPosition.y - 0)); i++)//left
            {
                if (board[c.currPosition.x - i, c.currPosition.y - 0].chessPiece.color == PlayerColor.None)
                    destination_moreSpace.Add(new Point(c.currPosition.x - i, c.currPosition.y - 0));
                else if (board[c.currPosition.x - i, c.currPosition.y - 0].chessPiece.color != c.color)
                {
                    destination_moreSpace.Add(new Point(c.currPosition.x - i, c.currPosition.y - 0));
                    break;
                }
                else if (board[c.currPosition.x - i, c.currPosition.y - 0].chessPiece.color == c.color)
                    break;
            }
            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x - i, c.currPosition.y - i)); i++)//down-left
            {
                if (board[c.currPosition.x - i, c.currPosition.y - i].chessPiece.color == PlayerColor.None)
                    destination_moreSpace.Add(new Point(c.currPosition.x - i, c.currPosition.y - i));
                else if (board[c.currPosition.x - i, c.currPosition.y - i].chessPiece.color != c.color)
                {
                    destination_moreSpace.Add(new Point(c.currPosition.x - i, c.currPosition.y - i));
                    break;
                }
                else if (board[c.currPosition.x - i, c.currPosition.y - i].chessPiece.color == c.color)
                    break;
            }
            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x - 0, c.currPosition.y - i)); i++)//down
            {
                if (board[c.currPosition.x - 0, c.currPosition.y - i].chessPiece.color == PlayerColor.None)
                    destination_moreSpace.Add(new Point(c.currPosition.x - 0, c.currPosition.y - i));
                else if (board[c.currPosition.x - 0, c.currPosition.y - i].chessPiece.color != c.color)
                {
                    destination_moreSpace.Add(new Point(c.currPosition.x - 0, c.currPosition.y - i));
                    break;
                }
                else if (board[c.currPosition.x - 0, c.currPosition.y - i].chessPiece.color == c.color)
                    break;
            }
            for (int i = 1; Tools.inBoard(new Point(c.currPosition.x + i, c.currPosition.y - i)); i++)//down-right
            {
                if (board[c.currPosition.x + i, c.currPosition.y - i].chessPiece.color == PlayerColor.None)
                    destination_moreSpace.Add(new Point(c.currPosition.x + i, c.currPosition.y - i));
                else if (board[c.currPosition.x + i, c.currPosition.y - i].chessPiece.color != c.color)
                {
                    destination_moreSpace.Add(new Point(c.currPosition.x + i, c.currPosition.y - i));
                    break;
                }
                else if (board[c.currPosition.x + i, c.currPosition.y - i].chessPiece.color == c.color)
                    break;
            }

            return destination_moreSpace;
        }
        private List<Point> possibleDestinations_none(ChessPiece c)
        {
            return new List<Point>();
        }
        private void swap(Square s1, Square s2)
        {
            ChessPiece temp = s1.chessPiece;
            s1.chessPiece = s2.chessPiece;
            s2.chessPiece = temp;

            Point position = s1.chessPiece.currPosition;//by swaping the piece, currPosition also changed, so this is fixing:
            s1.chessPiece.currPosition = s2.chessPiece.currPosition;
            s2.chessPiece.currPosition = position;
        }
        private void move(Square from, Point to)
        {
            if (from.chessPiece.kind == Kind.none)
                return;

            moovedPieces.Add(from.chessPiece);

            from.chessPiece.moved = true;
            swap(board[from.point.x, from.point.y], board[to.x, to.y]);
        }
        private void eat(Square from, Square to)
        {
            if (from.chessPiece.kind == Kind.none || to.chessPiece.kind == Kind.none)
                return;

            moovedPieces.Add(from.chessPiece);
            moovedPieces.Add(to.chessPiece);

            from.chessPiece.moved = true;

            ChessPiece temp = to.chessPiece;
            to.chessPiece = new None();
            to.chessPiece.currPosition = new Point(temp.currPosition.x, temp.currPosition.y);
            swap(board[from.point.x, from.point.y], board[to.point.x, to.point.y]);

            alive.Remove(temp);
            temp.currPosition = new Point(Tools.None, Tools.None);
            eaten.Add(temp);
        }
    }
}