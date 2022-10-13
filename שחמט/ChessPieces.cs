using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public enum Kind
    {
        pawn,
        knight,
        bishop,
        castle,
        king,
        queen,
        none
    }

    public class Pawn : ChessPiece
    {
        public Pawn()
        {
            kind = Kind.pawn;
            color = PlayerColor.None;
            currPosition = new Point();
            initPosition = new Point();
            isBound = false;
            moved = false;
        }

        public override List<Point> allDestinations()
        {
            List<Point> des = new List<Point>();

            //not on board, or at the edge
            if (Tools.inBoard(currPosition.x, currPosition.y) ||
                (currPosition.y == Tools.one && color == PlayerColor.Black) ||
                (currPosition.y == Tools.eight && color == PlayerColor.White))
                return des;

            //double move at beggining
            if (currPosition == initPosition)
                des.Add(new Point(currPosition.x, currPosition.y + (2 * (int)color)));

            //regular move
            des.Add(new Point(currPosition.x, currPosition.y + (int)color));

            //eat right
            if (currPosition.x < Tools.H)
                des.Add(new Point(currPosition.x + 1, currPosition.y + (int)color));

            //eat left
            if (currPosition.x > Tools.A)
                des.Add(new Point(currPosition.x - 1, currPosition.y + (int)color));

            //TODO: derech hilucho

            return des;
        }
    }
    public class Knight : ChessPiece
    {
        public Knight()
        {
            kind = Kind.knight;
            color = PlayerColor.None;
            currPosition = new Point();
            initPosition = new Point();
            isBound = false;
            moved = false;
        }

        public override List<Point> allDestinations()
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
            List<Point> des = new List<Point>();

            if (Tools.inBoard(currPosition.x + 1, currPosition.y + 2))
                des.Add(new Point(currPosition.x + 1, currPosition.y + 2));
            if (Tools.inBoard(currPosition.x + 1, currPosition.y - 2))
                des.Add(new Point(currPosition.x + 1, currPosition.y - 2));
            if (Tools.inBoard(currPosition.x - 1, currPosition.y + 2))
                des.Add(new Point(currPosition.x - 1, currPosition.y + 2));
            if (Tools.inBoard(currPosition.x - 1, currPosition.y - 2))
                des.Add(new Point(currPosition.x - 1, currPosition.y - 2));
            if (Tools.inBoard(currPosition.x + 2, currPosition.y + 1))
                des.Add(new Point(currPosition.x + 2, currPosition.y + 1));
            if (Tools.inBoard(currPosition.x + 2, currPosition.y - 1))
                des.Add(new Point(currPosition.x + 2, currPosition.y - 1));
            if (Tools.inBoard(currPosition.x - 2, currPosition.y + 1))
                des.Add(new Point(currPosition.x - 2, currPosition.y + 1));
            if (Tools.inBoard(currPosition.x - 2, currPosition.y - 1))
                des.Add(new Point(currPosition.x - 2, currPosition.y - 1));

            return des;
        }
    }
    public class Bishop : ChessPiece
    {
        public Bishop()
        {
            kind = Kind.bishop;
            color = PlayerColor.None;
            currPosition = new Point();
            initPosition = new Point();
            isBound = false;
            moved = false;
        }

        public override List<Point> allDestinations()
        {
            List<Point> destination = new List<Point>();

            for (int i = 1; Tools.inBoard(currPosition.x + i, currPosition.y + i); i++)
                destination.Add(new Point(currPosition.x + i, currPosition.y + i));
            for (int i = 1; Tools.inBoard(currPosition.x + i, currPosition.y - i); i++)
                destination.Add(new Point(currPosition.x + i, currPosition.y - i));
            for (int i = 1; Tools.inBoard(currPosition.x - i, currPosition.y + i); i++)
                destination.Add(new Point(currPosition.x - i, currPosition.y + i));
            for (int i = 1; Tools.inBoard(currPosition.x - i, currPosition.y - i); i++)
                destination.Add(new Point(currPosition.x - i, currPosition.y - i));

            return destination;
        }
    }
    public class Castle : ChessPiece
    {
        public Castle()
        {
            kind = Kind.castle;
            color = PlayerColor.None;
            currPosition = new Point();
            initPosition = new Point();
            isBound = false;
            moved = false;
        }

        public override List<Point> allDestinations()
        {
            List<Point> destination = new List<Point>();//moreSpace to look good...

            for (int i = 1; Tools.inBoard(currPosition.x + i, currPosition.y); i++)
                destination.Add(new Point(currPosition.x + i, currPosition.y));
            for (int i = 1; Tools.inBoard(currPosition.x - i, currPosition.y); i++)
                destination.Add(new Point(currPosition.x - i, currPosition.y));
            for (int i = 1; Tools.inBoard(currPosition.x, currPosition.y + i); i++)
                destination.Add(new Point(currPosition.x, currPosition.y + i));
            for (int i = 1; Tools.inBoard(currPosition.x, currPosition.y - i); i++)
                destination.Add(new Point(currPosition.x, currPosition.y - i));

            return destination;
        }
    }
    public class King : ChessPiece
    {
        public King()
        {
            kind = Kind.king;
            color = PlayerColor.None;
            currPosition = new Point();
            initPosition = new Point();
            isBound = false;
            moved = false;
        }

        public override List<Point> allDestinations()
        {
            List<Point> des = new List<Point>();

            if (Tools.inBoard(currPosition.x + 1, currPosition.y - 0))//right
                des.Add(new Point(currPosition.x + 1, currPosition.y - 0));
            if (Tools.inBoard(currPosition.x + 1, currPosition.y + 1))//up-right
                des.Add(new Point(currPosition.x + 1, currPosition.y + 1));
            if (Tools.inBoard(currPosition.x + 0, currPosition.y + 1))//up
                des.Add(new Point(currPosition.x + 0, currPosition.y + 1));
            if (Tools.inBoard(currPosition.x - 1, currPosition.y + 1))//up-left
                des.Add(new Point(currPosition.x - 1, currPosition.y + 1));
            if (Tools.inBoard(currPosition.x - 1, currPosition.y - 0))//left
                des.Add(new Point(currPosition.x - 1, currPosition.y - 0));
            if (Tools.inBoard(currPosition.x - 1, currPosition.y - 1))//down-left
                des.Add(new Point(currPosition.x - 1, currPosition.y - 1));
            if (Tools.inBoard(currPosition.x - 0, currPosition.y - 1))//down
                des.Add(new Point(currPosition.x - 0, currPosition.y - 1));
            if (Tools.inBoard(currPosition.x + 1, currPosition.y - 1))//down-right
                des.Add(new Point(currPosition.x + 1, currPosition.y - 1));

            //Hatzracha
            if (currPosition == initPosition)
            {
                des.Add(new Point(currPosition.x + 2, currPosition.y));
                des.Add(new Point(currPosition.x - 2, currPosition.y));
            }

            return des;
        }
    }
    public class Queen : ChessPiece
    {
        public Queen()
        {
            kind = Kind.queen;
            color = PlayerColor.None;
            currPosition = new Point();
            initPosition = new Point();
            isBound = false;
            moved = false;
        }

        public override List<Point> allDestinations()
        {
            List<Point> destination = new List<Point>();//moreSpace to look good...

            for (int i = 1; Tools.inBoard(currPosition.x + i, currPosition.y - 0); i++)//right
                destination.Add(new Point(currPosition.x + i, currPosition.y - 0));
            for (int i = 1; Tools.inBoard(currPosition.x + i, currPosition.y + i); i++)//up-right
                destination.Add(new Point(currPosition.x + i, currPosition.y + i));
            for (int i = 1; Tools.inBoard(currPosition.x + 0, currPosition.y + i); i++)//up
                destination.Add(new Point(currPosition.x + 0, currPosition.y + i));
            for (int i = 1; Tools.inBoard(currPosition.x - i, currPosition.y + i); i++)//up-left
                destination.Add(new Point(currPosition.x - i, currPosition.y + i));
            for (int i = 1; Tools.inBoard(currPosition.x - i, currPosition.y - 0); i++)//left
                destination.Add(new Point(currPosition.x - i, currPosition.y - 0));
            for (int i = 1; Tools.inBoard(currPosition.x - i, currPosition.y - i); i++)//down-left
                destination.Add(new Point(currPosition.x - i, currPosition.y - i));
            for (int i = 1; Tools.inBoard(currPosition.x - 0, currPosition.y - i); i++)//down
                destination.Add(new Point(currPosition.x - 0, currPosition.y - i));
            for (int i = 1; Tools.inBoard(currPosition.x + i, currPosition.y - i); i++)//down-right
                destination.Add(new Point(currPosition.x + i, currPosition.y - i));

            return destination;
        }
    }
    public class None : ChessPiece
    {
        public None()
        {
            kind = Kind.none;
            color = PlayerColor.None;
            currPosition = new Point();
            initPosition = new Point();
            isBound = false;
            moved = false;
        }

        public override List<Point> allDestinations()
        {
            return new List<Point>();
        }
    }

    public abstract class ChessPiece
    {
        public Kind kind { get; set; }
        public PlayerColor color { get; set; }

        public Point currPosition { get; set; }
        public Point initPosition { get; set; }

        public bool isBound { get; set; } //can't move because of the king - 'merutak'
        public bool moved { get; set; }

        public abstract List<Point> allDestinations();

        public static bool operator ==(ChessPiece cp1, ChessPiece cp2)
        {
            return (cp1.initPosition == cp2.initPosition);
        }
        public static bool operator !=(ChessPiece cp1, ChessPiece cp2)
        {
            return !(cp1 == cp2);
        }
        public override bool Equals(object obj)
        {
            if (obj is ChessPiece)
                return this == (obj as ChessPiece);
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return (initPosition.x << 16 + initPosition.y);
        }
        public override string ToString()
        {
            if (kind == Kind.none)
                return string.Format("-none-");
            return string.Format("{0} {1} ({2}) at {3}", color, kind, initPosition, currPosition);
        }

        public string Name()
        {
            string name = "";
            if (kind == Kind.none)
                return name;

            name += kind.ToString();
            name += (color == PlayerColor.Black ? "B" : "W");
            if (kind == Kind.pawn)
                name += "_Nr" + initPosition.x.ToString();
            else if (kind == Kind.bishop)
            {
                if (initPosition == new Point(Tools.F, Tools.eight) || initPosition == new Point(Tools.C, Tools.one))
                    name += "b";
                else
                    name += "w";
            }
            else if (kind == Kind.knight)
            {
                if (initPosition == new Point(Tools.B, Tools.eight) || initPosition == new Point(Tools.G, Tools.one))
                    name += "b";
                else
                    name += "w";
            }
            else if (kind == Kind.castle)
            {
                if (initPosition == new Point(Tools.H, Tools.eight) || initPosition == new Point(Tools.A, Tools.one))
                    name += "b";
                else
                    name += "w";
            }

            return name;
        }
    }
}
