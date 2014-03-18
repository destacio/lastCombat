using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace qwerty
{
    public partial class Form1 : Form
    {
        public Bitmap combatBitmap;

        combatMap cMap = new combatMap(10, 7);  // создаем поле боя с указанной размерностью
        ObjectManager objectManager = new ObjectManager();
        int select = -1; // служебная переменная, пока сам не знаю на кой хер она мне, но пусть будет. да.
        int activePlayer = 1; // ход 1-ого или 2-ого игрока
        Ship activeShip = null; // выделенное судно
        List<Ship> allShips = new List<Ship>();
        List<Meteor> meteors = new List<Meteor>();
        int blueShipsCount;
        int redShipsCount;
        
        public Form1()
        {
            
            Ship penumbra = new Ship(Constants.SCOUT, 1);
            allShips.Add(penumbra);
            Ship holycow = new Ship(Constants.SCOUT, 1);
            allShips.Add(holycow);
            Ship leroy = new Ship(Constants.ASSAULTER, 1);
            allShips.Add(leroy);


            Ship pandorum = new Ship(Constants.SCOUT, 2);
            allShips.Add(pandorum);
            Ship exodar = new Ship(Constants.SCOUT, 2);
            allShips.Add(exodar);
            Ship neveria = new Ship(Constants.ASSAULTER, 2);
            allShips.Add(neveria);  

            //ShipScout penumbra = new ShipScout(1);
            //allShips.Add((Ship)penumbra);

            //Meteor meteor = new Meteor(11, 100, 25, 1, 1);
            //cMap.boxes[11].spaceObject = meteor;

            objectManager.meteorCreate(cMap);

            for (int count = 0; count < allShips.Count; count++ )
            {
                allShips[count].placeShip(ref cMap);
            }

            InitializeComponent();
            Draw();

            shipsCount();

            //boxDescription.Text = "id = " + cMap.getBoxIdByCoords(0, 0);

    
        }

        public void shipsCount()
        {
            blueShipsCount = 0;
            redShipsCount = 0;
            for (int count = 0; count < allShips.Count; count++)
            {

                if (allShips[count].player == 1)
                    blueShipsCount++;
                else if (allShips[count].player == 2)
                    redShipsCount++;
            }
            txtBlueShips.Text = "" + blueShipsCount;
            txtRedShips.Text = "" + redShipsCount;
        }

        
        public void Draw()
        {
            combatBitmap = new Bitmap(pictureMap.Width, pictureMap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(combatBitmap);
            g.FillRectangle(Brushes.Black, 0, 0, combatBitmap.Width, combatBitmap.Height);//рисуем фон окна

            Pen generalPen;
            Pen redPen = new Pen(Color.Red, 3);
            Pen grayPen = new Pen(Color.Gray, 3);
            Pen PurplePen = new Pen(Color.Purple);
            Pen activeShipAriaPen = new Pen(Color.Purple, 5);

            SolidBrush redBrush = new SolidBrush(Color.Red);
            SolidBrush blueBrush = new SolidBrush(Color.Blue);
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            SolidBrush grayBrush = new SolidBrush(Color.Gray);
            SolidBrush activeShipBrush = new SolidBrush(Color.DarkGreen);
            SolidBrush mediumPurpleBrush = new SolidBrush(Color.MediumPurple);
            SolidBrush brush;



            for (int i = 0; i < cMap.boxes.Count; i++)
            {
                generalPen = PurplePen;
                Point[] myPointArrayHex = {  //точки для отрисовки шестиугольника
                        new Point(cMap.boxes[i].xpoint1, cMap.boxes[i].ypoint1),
                        new Point(cMap.boxes[i].xpoint2, cMap.boxes[i].ypoint2),
                        new Point(cMap.boxes[i].xpoint3, cMap.boxes[i].ypoint3),
                        new Point(cMap.boxes[i].xpoint4, cMap.boxes[i].ypoint4),
                        new Point(cMap.boxes[i].xpoint5, cMap.boxes[i].ypoint5),
                        new Point(cMap.boxes[i].xpoint6, cMap.boxes[i].ypoint6)
                };
                // Если выделили судно с очками передвижения, подсвечиваем его и соседние клетки

                if (activeShip != null)
                {
                    for(int count = 0; count < allShips.Count; count++)
                    {
                        if (allShips[count].player != activePlayer && allShips[count].boxId >= 0)
                        {
                            if(allShips[count].player == 0)
                            {
                                int x1;
                            }
                            else
                            {
                                double x1 = cMap.boxes[activeShip.boxId].x;
                                double y1 = cMap.boxes[activeShip.boxId].y;
                                double x2 = cMap.boxes[allShips[count].boxId].x;
                                double y2 = cMap.boxes[allShips[count].boxId].y;
                                double range;
                                range = Math.Sqrt((x2 - x1) * (x2 - x1) + ((y2 - y1) * (y2 - y1)) * 0.45);
                                if((int)range <= activeShip.attackRange)
                                {
                                    Point[] myPointArrayHex99 = {  //точки для отрисовки шестиугольника
                                        new Point(cMap.boxes[allShips[count].boxId].xpoint1, cMap.boxes[allShips[count].boxId].ypoint1),
                                        new Point(cMap.boxes[allShips[count].boxId].xpoint2, cMap.boxes[allShips[count].boxId].ypoint2),
                                        new Point(cMap.boxes[allShips[count].boxId].xpoint3, cMap.boxes[allShips[count].boxId].ypoint3),
                                        new Point(cMap.boxes[allShips[count].boxId].xpoint4, cMap.boxes[allShips[count].boxId].ypoint4),
                                        new Point(cMap.boxes[allShips[count].boxId].xpoint5, cMap.boxes[allShips[count].boxId].ypoint5),
                                        new Point(cMap.boxes[allShips[count].boxId].xpoint6, cMap.boxes[allShips[count].boxId].ypoint6)
                                     };
                                    g.DrawPolygon(redPen, myPointArrayHex99);
                                }
                            }
                        }
                    }
                }

                g.DrawPolygon(PurplePen, myPointArrayHex);

                if (activeShip != null && activeShip.boxId == i)
                {
                    g.DrawPolygon(activeShipAriaPen, myPointArrayHex);
                    
                }
                

                if (cMap.boxes[i].spaceObject != null && cMap.boxes[i].spaceObject.objectType == Constants.SHIP)
                {
                    if (cMap.boxes[i].spaceObject.player == 1)
                        brush = blueBrush;
                    else if (cMap.boxes[i].spaceObject.player == 2)
                        brush = redBrush;
                    else brush = grayBrush;
                    
                    Point[] myPointArray = {
                     
                        new Point(cMap.boxes[i].xpoint2 + cMap.boxes[i].spaceObject.xpoints[0], cMap.boxes[i].ypoint2 + cMap.boxes[i].spaceObject.ypoints[0]),
                        new Point(cMap.boxes[i].xpoint2 + cMap.boxes[i].spaceObject.xpoints[1], cMap.boxes[i].ypoint2 + cMap.boxes[i].spaceObject.ypoints[1]),
                        new Point(cMap.boxes[i].xpoint2 + cMap.boxes[i].spaceObject.xpoints[2], cMap.boxes[i].ypoint2 + cMap.boxes[i].spaceObject.ypoints[2]),
                        new Point(cMap.boxes[i].xpoint2 + cMap.boxes[i].spaceObject.xpoints[3], cMap.boxes[i].ypoint2 + cMap.boxes[i].spaceObject.ypoints[3]),
                        new Point(cMap.boxes[i].xpoint2 + cMap.boxes[i].spaceObject.xpoints[4], cMap.boxes[i].ypoint2 + cMap.boxes[i].spaceObject.ypoints[4])
                
                    }; 
                    g.FillPolygon(brush, myPointArray);
                    
                    g.DrawString(cMap.boxes[i].spaceObject.actionsLeft.ToString(), new Font("Arial", 8.0F), Brushes.Blue, new PointF(cMap.boxes[i].xpoint1 + 25, cMap.boxes[i].ypoint1 + 15));
                    
                }
                else if (cMap.boxes[i].spaceObject != null && cMap.boxes[i].spaceObject.objectType == Constants.METEOR)
                {
                    if (cMap.boxes[i].spaceObject.boxId != -1)
                    {
                        g.FillEllipse(grayBrush, cMap.boxes[i].xpoint1 + 17, cMap.boxes[i].ypoint1 - 12, 25, 25);
                    }
                }
                //g.DrawString(cMap.boxes[i].id.ToString(), new Font("Arial", 8.0F), Brushes.Green, new PointF(cMap.boxes[i].xpoint1 + 20, cMap.boxes[i].ypoint1 + 10));
                g.DrawString(cMap.boxes[i].x.ToString(), new Font("Arial", 8.0F), Brushes.Green, new PointF(cMap.boxes[i].xpoint1 + 10, cMap.boxes[i].ypoint1 + 10));
                g.DrawString(cMap.boxes[i].y.ToString(), new Font("Arial", 8.0F), Brushes.Green, new PointF(cMap.boxes[i].xpoint1 + 40, cMap.boxes[i].ypoint1 + 10));
                if(cMap.boxes[i].spaceObject != null && cMap.boxes[i].spaceObject.boxId != -1)
                    g.DrawString(cMap.boxes[i].spaceObject.currentHealth.ToString(), new Font("Arial", 8.0F), Brushes.Red, new PointF(cMap.boxes[i].xpoint1 + 20, cMap.boxes[i].ypoint1 - 25));
            }
            pictureMap.Image = combatBitmap;
            pictureMap.Refresh();

        }

        private void pictureMap_MouseClick(object sender, MouseEventArgs e)
        {

            for (int i = 0; i < cMap.boxes.Count; i++)
            {

                if ((e.X > cMap.boxes[i].xpoint2) &&
                    (e.X < cMap.boxes[i].xpoint3) &&
                    (e.Y > cMap.boxes[i].ypoint2) &&
                    (e.Y < cMap.boxes[i].ypoint6))
                {
                    select = i;

                    if (activeShip == null && cMap.boxes[select].spaceObject != null)
                    {
                        if (cMap.boxes[select].spaceObject.objectType == Constants.SHIP
                            || cMap.boxes[select].spaceObject.objectType == Constants.METEOR)
                        {
                            if (activePlayer == cMap.boxes[select].spaceObject.player)
                            {
                                boxDescription.Text = cMap.boxes[select].spaceObject.description();
                                activeShip = (Ship)cMap.boxes[select].spaceObject;

                                Draw();
                                break;
                            }
                            else
                            {
                                Draw();
                                boxDescription.Text = cMap.boxes[i].spaceObject.description();
                            }
                        }
                    }


                // Если до этого ткнули по дружественному судну
                    else if (activeShip != null)
                    {

                        // если выбранная клетка пуста - определяем возможность перемещения 
                        if (cMap.boxes[select].spaceObject == null)
                        {
                            int flag = 0;
                            // перемещение на одну клетку вверх
                            // старый алгоритм, основанный на вычислении айдишника клетки!!! 
                            int a = activeShip.boxId;
                            if (a + 1 == select && a % cMap.height != cMap.height - 1)
                            {
                                flag = 1;
                            }
                            // перемещение на одну клетку вниз
                            // старый алгоритм, основанный на вычислении айдишника клетки!!! 
                            else if (a - 1 == select && a % cMap.height != 0)
                            {
                                flag = 1;
                            }
                            // перемещение на клетку справа вверху
                            else if (cMap.boxes[a].y != 0 && cMap.boxes[a].x != cMap.width
                                && cMap.boxes[a].x + 1 == cMap.boxes[select].x
                                && cMap.boxes[a].y - 1 == cMap.boxes[select].y)
                            {
                                flag = 1;
                            }
                            // перемещение на клетку справа внизу
                            else if (cMap.boxes[a].y + 1 != cMap.height * 2 && cMap.boxes[a].x != cMap.width
                                && cMap.boxes[a].x + 1 == cMap.boxes[select].x
                                && cMap.boxes[a].y + 1 == cMap.boxes[select].y)
                            {
                                flag = 1;
                            }
                            // перемещение на клетку слева вверху
                            else if (cMap.boxes[a].y != 0 && cMap.boxes[a].x != 0
                                && cMap.boxes[a].x - 1 == cMap.boxes[select].x
                                && cMap.boxes[a].y - 1 == cMap.boxes[select].y)
                            {
                                flag = 1;
                            }
                            // перемещение на клетку слева внизу
                            else if (cMap.boxes[a].y + 1 != cMap.height * 2 && cMap.boxes[a].x != 0
                                && cMap.boxes[a].x - 1 == cMap.boxes[select].x
                                && cMap.boxes[a].y + 1 == cMap.boxes[select].y)
                            {
                                flag = 1;
                            }
                            if (flag == 1)
                            {
                                activeShip.moveShip(ref cMap, a, select);
                                boxDescription.Text = activeShip.description();

                                if (activeShip.actionsLeft == 0) activeShip = null;
                                Draw();

                                break;
                            }
                        }
                        else if (cMap.boxes[select].spaceObject != null)
                        {
                            if (cMap.boxes[select].spaceObject.player == activePlayer)
                            {
                                boxDescription.Text = cMap.boxes[select].spaceObject.description();
                                activeShip = (Ship)cMap.boxes[select].spaceObject;

                                Draw();
                                break;
                            }

                            // просчет возможности атаки 

                            else if (cMap.boxes[select].spaceObject.player != activePlayer)
                            {
                                int flag = 0;
                                int a = activeShip.boxId;

                                double x1 = cMap.boxes[a].x;
                                double y1 = cMap.boxes[a].y;
                                double x2 = cMap.boxes[select].x;
                                double y2 = cMap.boxes[select].y;
                                double range;

                                //range = ((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1))/4;
                                range = Math.Sqrt((x2-x1)*(x2-x1)+((y2-y1)*(y2-y1))*0.33);
                                if(activeShip.attackRange >= (int)range)
                                {
                                    flag = 1;
                                }
                                if (flag == 1)
                                {
                                    if (activeShip.attack(ref cMap, select) == 1)
                                        shipsCount();
                                    if (activeShip.actionsLeft == 0) activeShip = null;
                                    flag = 0;
                                    Draw();
                                    break;
                                }
                            } 

                            }
                        }
                    
                }
            }
        }

        private void btnEndTurn_Click(object sender, EventArgs e)
        {
            if (activePlayer == 1) activePlayer = 2;
            else activePlayer = 1;

            lblTurn.Text = "Ходит " + activePlayer + "-й игрок";

            activeShip = null;

            for (int count = 0; count < allShips.Count; count++)
            {
                allShips[count].refill();
            }

            objectManager.moveMeteors(cMap);

            Draw();
        }

    }
}
