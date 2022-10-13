using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Chess;
using System.Collections.ObjectModel;

namespace ChessGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 00 01 02 03 04 05 06 07
        // 08 09 10 11 12 13 14 15
        // -- -- -- -- -- -- -- --
        // -- -- -- -- -- -- -- --
        // -- -- -- -- -- -- -- --
        // -- -- -- -- -- -- -- --
        // 16 17 18 19 20 21 22 23
        // 24 25 26 27 28 29 30 31
        public Label[] pieces;
        public List<Label> lighting;
        public ObservableCollection<Label> eatenStackW;
        public ObservableCollection<Label> eatenStackB;
        public Board board;
        public Label selected;
        public bool isSelected;

        public MainWindow()
        {
            InitializeComponent();

            //local variables
            board = new Board();
            pieces = new Label[32];
            lighting = new List<Label>();
            eatenStackW = new ObservableCollection<Label>();
            eatenStackB = new ObservableCollection<Label>();
            isSelected = false;

            eatStackWhite.ItemsSource = eatenStackW;
            eatStackBlack.ItemsSource = eatenStackB;

            //board's image
            ImageBrush brush0 = new ImageBrush();
            ImageBrush brush1 = new ImageBrush();
            brush0.ImageSource = new BitmapImage(new Uri("../../src/others/brown_boardBackground.png", UriKind.Relative));
            brush1.ImageSource = new BitmapImage(new Uri("../../src/others/brown_board.png", UriKind.Relative));
            behindBoard.Background = brush0;
            boardGrid.Background = brush1;

            //init pieces
            init_ChessPieces();
        }

        private void cp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isSelected)
            {
                foreach (Label l in lighting)
                {
                    if (Grid.GetColumn(l) == Grid.GetColumn((Label)sender) &&
                        Grid.GetRow(l) == Grid.GetRow((Label)sender))
                    {
                        selected_destination(l, e);
                        turnOffLights();
                        return;
                    }
                }

                //if the click wasn't on lighting square or again on the same chessPiece
                isSelected = false;
                turnOffLights();
            }
            else
            {
                Label s = (sender as Label);
                selected = s;
                foreach (Chess.Point p in board.possibleDestinations(new Square(board.board[Grid.GetColumn(s), Grid.GetRow(s)].chessPiece)))
                {
                    Label label = new Label();
                    label.Name = "light" + p.x + p.y;
                    label.Background = new SolidColorBrush(Colors.Yellow);
                    label.Opacity = 0.6;
                    label.Visibility = Visibility.Visible;
                    label.MouseUp += selected_destination;
                    boardGrid.Children.Add(label);
                    Grid.SetColumn(label, p.x);
                    Grid.SetRow(label, p.y);
                    Grid.SetZIndex(label, -1);

                    lighting.Add(label);
                }

                isSelected = true;
            }
        }
        private void init_ChessPieces()
        {
            #region images
            ImageBrush brush0 = new ImageBrush();
            ImageBrush brush1 = new ImageBrush();
            ImageBrush brush2 = new ImageBrush();
            ImageBrush brush3 = new ImageBrush();
            ImageBrush brush4 = new ImageBrush();
            ImageBrush brush5 = new ImageBrush();
            ImageBrush brush6 = new ImageBrush();
            ImageBrush brush7 = new ImageBrush();
            ImageBrush brush8 = new ImageBrush();
            ImageBrush brush9 = new ImageBrush();
            ImageBrush brush10 = new ImageBrush();
            ImageBrush brush11 = new ImageBrush();
            brush0.ImageSource = new BitmapImage(new Uri("../../src/BlackWhite/bishopB.png", UriKind.Relative));
            brush1.ImageSource = new BitmapImage(new Uri("../../src/BlackWhite/castleB.png", UriKind.Relative));
            brush2.ImageSource = new BitmapImage(new Uri("../../src/BlackWhite/kingB.png", UriKind.Relative));
            brush3.ImageSource = new BitmapImage(new Uri("../../src/BlackWhite/knightB.png", UriKind.Relative));
            brush4.ImageSource = new BitmapImage(new Uri("../../src/BlackWhite/pawnB.png", UriKind.Relative));
            brush5.ImageSource = new BitmapImage(new Uri("../../src/BlackWhite/queenB.png", UriKind.Relative));
            brush6.ImageSource = new BitmapImage(new Uri("../../src/BlackWhite/bishopW.png", UriKind.Relative));
            brush7.ImageSource = new BitmapImage(new Uri("../../src/BlackWhite/castleW.png", UriKind.Relative));
            brush8.ImageSource = new BitmapImage(new Uri("../../src/BlackWhite/kingW.png", UriKind.Relative));
            brush9.ImageSource = new BitmapImage(new Uri("../../src/BlackWhite/knightW.png", UriKind.Relative));
            brush10.ImageSource = new BitmapImage(new Uri("../../src/BlackWhite/pawnW.png", UriKind.Relative));
            brush11.ImageSource = new BitmapImage(new Uri("../../src/BlackWhite/queenW.png", UriKind.Relative));
            #endregion

            {
                #region  King & queen
                Label kib = new Label();//king black
                kib.Background = brush2;
                kib.Name = "kingB";
                kib.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(kib);
                Grid.SetColumn(kib, Tools.E);
                Grid.SetRow(kib, Tools.eight);
                pieces[4] = kib;
                Label qb = new Label();//queen black
                qb.Background = brush5;
                qb.Name = "queenB";
                qb.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(qb);
                Grid.SetColumn(qb, Tools.D);
                Grid.SetRow(qb, Tools.eight);
                pieces[3] = qb;
                Label kiw = new Label();//king white
                kiw.Background = brush8;
                kiw.Name = "kingW";
                kiw.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(kiw);
                Grid.SetColumn(kiw, Tools.E);
                Grid.SetRow(kiw, Tools.one);
                pieces[28] = kiw;
                Label qw = new Label();//queen white
                qw.Background = brush11;
                qw.Name = "queenW";
                qw.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(qw);
                Grid.SetColumn(qw, Tools.D);
                Grid.SetRow(qw, Tools.one);
                pieces[27] = qw;
                #endregion
            }
            {
                #region  Bishop
                Label bb1 = new Label();//bishop black on white square
                bb1.Background = brush0;
                bb1.Name = "bishopBw";
                bb1.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(bb1);
                Grid.SetColumn(bb1, Tools.C);
                Grid.SetRow(bb1, Tools.eight);
                pieces[2] = bb1;
                Label bb2 = new Label();//bishop black on black square
                bb2.Background = brush0;
                bb2.Name = "bishopBb";
                bb2.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(bb2);
                Grid.SetColumn(bb2, Tools.F);
                Grid.SetRow(bb2, Tools.eight);
                pieces[5] = bb2;
                Label bw1 = new Label();//bishop white on black square
                bw1.Background = brush6;
                bw1.Name = "bishopWb";
                bw1.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(bw1);
                Grid.SetColumn(bw1, Tools.C);
                Grid.SetRow(bw1, Tools.one);
                pieces[26] = bw1;
                Label bw2 = new Label();//bishop white on white square
                bw2.Background = brush6;
                bw2.Name = "bishopWw";
                bw2.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(bw2);
                Grid.SetColumn(bw2, Tools.F);
                Grid.SetRow(bw2, Tools.one);
                pieces[29] = bw2;
                #endregion
            }
            {
                #region  Knight
                Label knb1 = new Label();//knight black on black square
                knb1.Background = brush3;
                knb1.Name = "knightBb";
                knb1.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(knb1);
                Grid.SetColumn(knb1, Tools.B);
                Grid.SetRow(knb1, Tools.eight);
                pieces[1] = knb1;
                Label knb2 = new Label();//knight black on white square
                knb2.Background = brush3;
                knb2.Name = "knightBw";
                knb2.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(knb2);
                Grid.SetColumn(knb2, Tools.G);
                Grid.SetRow(knb2, Tools.eight);
                pieces[6] = knb2;
                Label knw1 = new Label();//knight white on white square
                knw1.Background = brush9;
                knw1.Name = "knightWw";
                knw1.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(knw1);
                Grid.SetColumn(knw1, Tools.B);
                Grid.SetRow(knw1, Tools.one);
                pieces[25] = knw1;
                Label knw2 = new Label();//knight white on black square
                knw2.Background = brush9;
                knw2.Name = "knightWb";
                knw2.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(knw2);
                Grid.SetColumn(knw2, Tools.G);
                Grid.SetRow(knw2, Tools.one);
                pieces[30] = knw2;
                #endregion
            }
            {
                #region Castle
                Label cb1 = new Label();//castle black on white square
                cb1.Background = brush1;
                cb1.Name = "castleBw";
                cb1.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(cb1);
                Grid.SetColumn(cb1, Tools.A);
                Grid.SetRow(cb1, Tools.eight);
                pieces[0] = cb1;
                Label cb2 = new Label();//castle black on black square
                cb2.Background = brush1;
                cb2.Name = "castleBb";
                cb2.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(cb2);
                Grid.SetColumn(cb2, Tools.H);
                Grid.SetRow(cb2, Tools.eight);
                pieces[7] = cb2;
                Label cw1 = new Label();//castle white on black square
                cw1.Background = brush7;
                cw1.Name = "castleWb";
                cw1.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(cw1);
                Grid.SetColumn(cw1, Tools.A);
                Grid.SetRow(cw1, Tools.one);
                pieces[24] = cw1;
                Label cw2 = new Label();//castle white on white square
                cw2.Background = brush7;
                cw2.Name = "castleWw";
                cw2.MouseUp += cp_MouseUp;
                boardGrid.Children.Add(cw2);
                Grid.SetColumn(cw2, Tools.H);
                Grid.SetRow(cw2, Tools.one);
                pieces[31] = cw2;
                #endregion
            }
            {
                #region Pawn
                for (int i = 0; i < 8; i++)
                {
                    Label pb = new Label();//pawn black
                    pb.Background = brush4;
                    pb.Name = "pawnB_Nr" + i;
                    pb.MouseUp += cp_MouseUp;
                    boardGrid.Children.Add(pb);
                    Grid.SetColumn(pb, i);
                    Grid.SetRow(pb, Tools.seven);
                    pieces[8 + i] = pb;

                    Label pw = new Label();//pawn white
                    pw.Background = brush10;
                    pw.Name = "pawnW_Nr" + i;
                    pw.MouseUp += cp_MouseUp;
                    boardGrid.Children.Add(pw);
                    Grid.SetColumn(pw, i);
                    Grid.SetRow(pw, Tools.two);
                    pieces[16 + i] = pw;
                }
                #endregion
            }
        }
        private void Message(string v)
        {
            MessageBox.Show(v, "Message!", MessageBoxButton.OK);
        }
        private void selected_destination(object sender, MouseButtonEventArgs e)
        {
            ////////////////TODO
            Label l = sender as Label;
            ChessPiece cp = board.board[Grid.GetColumn(l), Grid.GetRow(l)].chessPiece;
            /*if (board.parameters[(int)Params.derech_hilucho] && cp.kind == Kind.pawn)
                ;
            if (board.parameters[(int)Params.hatzracha] && //הדרך לא מאויימת)
                ;
            if (board.parameters[(int)Params.pawn_at_end])
                ;*/

            if (!board.doTurn(board.board[Grid.GetColumn(selected), Grid.GetRow(selected)], new Chess.Point(Grid.GetColumn(l), Grid.GetRow(l))))
                Message(string.Format("Error at moving a chess-piece from {0} to {1}.",
                    new Chess.Point(Grid.GetColumn(selected), Grid.GetRow(selected)),
                    new Chess.Point(Grid.GetColumn(l), Grid.GetRow(l))));

            foreach (ChessPiece name in board.moovedPieces)
            {
                foreach (Label label in pieces)
                {
                    if (label.Name == name.Name())
                    {
                        if (name.currPosition == new Chess.Point(Tools.None, Tools.None))
                        {
                            if (name.color == PlayerColor.White)
                                eatenStackW.Add(label);
                            else if (name.color == PlayerColor.Black)
                                eatenStackB.Add(label);

                            boardGrid.Children.Remove(label);
                        }
                        else
                        {
                            Grid.SetColumn(label, name.currPosition.x);
                            Grid.SetRow(label, name.currPosition.y);
                        }
                    }
                }
            }

            turnOffLights();
            isSelected = false;
        }
        private void turnOffLights()
        {
            foreach (Label l in lighting)
            {
                boardGrid.Children.Remove(l);
            }
            lighting.Clear();
        }
        private void turnOffLights(List<Chess.Point> points)
        {
            foreach (Chess.Point p in points)
            {
                foreach (Label label in lighting)
                {
                    if (label.Name == "light" + p.x + p.y)
                    {
                        lighting.Remove(label);
                        boardGrid.Children.Remove(label);
                    }
                }
            }
        }
    }
}