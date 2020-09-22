/*
 * Description:     A basic PONG simulator
 * Author:          Max Jarvis 
 * Date:            9/21/2020
 */

#region libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values

        //graphics objects for drawing
        SolidBrush drawBrush = new SolidBrush(Color.White);
        SolidBrush sideBrush = new SolidBrush(Color.Red);
        Font drawFont = new Font("Courier New", 10);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);

        //determines whether a key is being pressed or not
        Boolean wKeyDown, sKeyDown, aKeyDown, dKeydown;
        Boolean upKeyDown, downKeyDown, leftKeyDown, rightKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        //ball directions, speed, and rectangle
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = true;
        const int BALL_SPEED = 6;
        Rectangle ball;

        //paddle speeds and rectangles
        const int PADDLE_SPEED = 8;
        Rectangle p1, p2;

        //sides
        Rectangle topRec;
        Rectangle bottomRec;
        Rectangle leftTop;
        Rectangle leftBottom;
        Rectangle rightTop;
        Rectangle rightBottom;
        const int GOALSIZE = 75;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 2;  // number of points needed to win game

        #endregion

        public Form1()
        {
            InitializeComponent();
            topRec = new Rectangle(0, 0, this.Width, 5);
            bottomRec = new Rectangle(0, this.Height - 5, this.Width, 5);
            leftTop = new Rectangle(0, 0, 5, this.Height / 2 - GOALSIZE);
            leftBottom = new Rectangle(0, this.Height / 2 + GOALSIZE, 5, this.Height / 2 - GOALSIZE);
            rightTop = new Rectangle(this.Width - 5, 0, 5, this.Height / 2 - GOALSIZE);
            rightBottom = new Rectangle(this.Width - 5, this.Height / 2 + GOALSIZE, 5, this.Height / 2 - GOALSIZE);
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = true;
                    break;
                case Keys.S:
                    sKeyDown = true;
                    break;
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.D:
                    dKeydown = true;
                    break;

                case Keys.Up:
                    upKeyDown = true;
                    break;
                case Keys.Down:
                    downKeyDown = true;
                    break;
                case Keys.Left:
                    leftKeyDown = true;
                    break;
                case Keys.Right:
                    rightKeyDown = true;
                    break;

                case Keys.Y:
                case Keys.Space:
                    if (newGameOk)
                    {
                        SetParameters();
                    }
                    break;
                case Keys.N:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
            }
        }
        
        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = false;
                    break;
                case Keys.S:
                    sKeyDown = false;
                    break;
                case Keys.A:
                    aKeyDown = false;
                    break;
                case Keys.D:
                    dKeydown = false;
                    break;

                case Keys.Up:
                    upKeyDown = false;
                    break;
                case Keys.Down:
                    downKeyDown = false;
                    break;
                case Keys.Left:
                    leftKeyDown = false;
                    break;
                case Keys.Right:
                    rightKeyDown = false;
                    break;
            }
        }

        /// <summary>
        /// sets the ball and paddle positions for game start
        /// </summary>
        private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                topLabel.Visible = false;
                bottomLabel.Visible = false;
                gameUpdateLoop.Start();
            }

            //set starting position for paddles on new game and point scored 
            const int PADDLE_EDGE = 20;  // buffer distance between screen edge and paddle            

            p1.Width = p2.Width = 10;    //height for both paddles set the same
            p1.Height = p2.Height = 40;  //width for both paddles set the same

            //p1 starting position
            p1.X = PADDLE_EDGE;
            p1.Y = this.Height / 2 - p1.Height / 2;

            //p2 starting position
            p2.X = this.Width - PADDLE_EDGE - p2.Width;
            p2.Y = this.Height / 2 - p2.Height / 2;

            // TODO set Width and Height of ball
            ball.Width = 20;
            ball.Height = 20;

            // TODO set starting X position for ball to middle of screen, (use this.Width and ball.Width)
            ball.X = this.Width / 2 - ball.Width/2;

            // TODO set starting Y position for ball to middle of screen, (use this.Height and ball.Height)
            ball.Y = this.Height / 2 - ball.Height/2;

        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {

            #region update ball position

            // TODO create code to move ball either left or right based on ballMoveRight and using BALL_SPEED
            if (ballMoveRight==true)
            {
                ball.X = ball.X+BALL_SPEED;
            }
            else
            {
                ball.X = ball.X - BALL_SPEED;
            }

            // TODO create code move ball either down or up based on ballMoveDown and using BALL_SPEED
            if(ballMoveDown==true)
            {
                ball.Y = ball.Y + BALL_SPEED;
            }
            else
            {
                ball.Y = ball.Y - BALL_SPEED;
            }

            #endregion

            #region update paddle positions

            if (wKeyDown == true && p1.Y > 5)
            { 
                p1.Y = p1.Y - PADDLE_SPEED;
            }
            if(sKeyDown==true&&p1.Y<this.Height-5-p1.Height)
            {
                p1.Y = p1.Y + PADDLE_SPEED;
            }
            if (aKeyDown==true&&p1.X>5)
            {
                p1.X = p1.X - PADDLE_SPEED;
            }
            if (dKeydown == true&& this.Width / 2 - p1.Width > p1.X)
            {
                p1.X = p1.X + PADDLE_SPEED;
            }
            

            if(upKeyDown==true&&p2.Y>5)
            {
                p2.Y = p2.Y - PADDLE_SPEED;
            }
            if (downKeyDown==true&&p2.Y<this.Height-5-p2.Height)
            {
                p2.Y = p2.Y + PADDLE_SPEED;
            }
            if (leftKeyDown==true&&this.Width/2-p2.Width <p2.X)
            {
                p2.X = p2.X - PADDLE_SPEED;
            }
            if (rightKeyDown == true && p2.X < this.Width - 5 - p2.Width)
            {
                p2.X = p2.X + PADDLE_SPEED;
            }

            #endregion 
            //
            #region ball collision with top and bottom lines

            if (ball.IntersectsWith(topRec)) // if ball hits top line
            {
                ballMoveDown = true;

                collisionSound.Play();
                // TODO use ballMoveDown boolean to change direction
                // TODO play a collision sound
            }
            else if (ball.IntersectsWith(bottomRec))
            {
                ballMoveDown = false;
                collisionSound.Play();
            }

            #endregion
            //
            #region ball collision with paddles

            if (ball.IntersectsWith(p1))
            {
                ballMoveRight = true;
                collisionSound.Play();
            }
            else if (ball.IntersectsWith(p2))
            {
                ballMoveRight = false;
                collisionSound.Play();
            }

            /*  ENRICHMENT
             *  Instead of using two if statments as noted above see if you can create one
             *  if statement with multiple conditions to play a sound and change direction
             */

            #endregion
            //
            #region ball collision with side walls (point scored)

            if (ball.X < 0)
            {
                scoreSound.Play();
                player2Score++;

                if (player2Score == gameWinScore)
                {
                    GameOver("PLAYER 2!");
                }
                else
                {
                    //change direction of ball and call SetParameters method.
                    ballMoveRight = false;
                    SetParameters();
                }
            }

            if (ball.X > this.Width)
            {
                scoreSound.Play();
                player1Score++;

                if (player1Score == gameWinScore)
                {
                    GameOver("PLAYER 1!");
                }
                else
                {
                    //change direction of ball and call SetParameters method.
                    ballMoveRight = true;
                    SetParameters();

                }
            }

            if (ball.IntersectsWith(leftTop)||ball.IntersectsWith(leftBottom))
            {
                ballMoveRight = true;
                collisionSound.Play();
            }

            if (ball.IntersectsWith(rightTop)||ball.IntersectsWith(rightBottom))
            {
                ballMoveRight = false;
                collisionSound.Play();
            }
            #endregion

            //refresh the screen, which causes the Form1_Paint method to run
            this.Refresh();
        }
        
        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {
            topLabel.Visible = true;
           
            newGameOk = true;
            gameUpdateLoop.Stop();
            topLabel.Text = ("GAME OVER! CONGRADULATIONS " + winner);
            this.Refresh();
            Thread.Sleep(2000);
            scoreSound.Play();
            bottomLabel.Visible = true;
            topLabel.Text = ("Want to play again?");
            bottomLabel.Text = ("Yes = Space bar  /  No = N");
           
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // TODO draw paddles using FillRectangle
            e.Graphics.FillRectangle(drawBrush, p1);
            e.Graphics.FillRectangle(drawBrush, p2);

            // TODO draw ball using FillRectangle
            e.Graphics.FillEllipse(drawBrush, ball);

            // TODO draw scores to the screen using DrawString
            e.Graphics.DrawString(player1Score.ToString(), drawFont, drawBrush, 10, 10);
            e.Graphics.DrawString(player2Score.ToString(), drawFont, drawBrush, this.Width - 20, 10);

            //draw sides
            e.Graphics.FillRectangle(sideBrush, topRec);
            e.Graphics.FillRectangle(sideBrush, bottomRec);
            e.Graphics.FillRectangle(sideBrush, leftTop);
            e.Graphics.FillRectangle(sideBrush, leftBottom);
            e.Graphics.FillRectangle(sideBrush, rightTop);
            e.Graphics.FillRectangle(sideBrush, rightBottom);
        }

    }
}
