using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace DrunkiBoy
{
    static class Highscore
    {
        private static List<KeyValuePair<int, string>> listHighscore;

        private static StreamReader sr; //Reader and writer for the highscore
        private static StreamWriter sw;

        private static string name;

        private static string strGoBack = "Click here to go back to the menu";
        private static Text textGoBack = new Text(Constants.FONT, strGoBack, new Vector2(20, 650));

        public enum state { show, enteringNewHighScore }
        public static state highScoreState = state.enteringNewHighScore;

        public static void Update()
        {
            if (highScoreState == state.enteringNewHighScore)
            {
                KeyboardInputToName();
            }
            if (textGoBack.IsClicked())
            {
                Game1.currentGameState = Game1.gameState.menu;
            }
        }

        /// <summary>
        /// Draws the highscore and allows for players to enter their name if top 10 is reached
        /// </summary>
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (highScoreState == state.enteringNewHighScore)
            {
                DrawHighScoreEntering(spriteBatch);
                DrawNameAsTyped(spriteBatch);
            }
            else
                DrawHighScore(spriteBatch);
            textGoBack.DrawClickableText(spriteBatch, Constants.fontColor);
            spriteBatch.End();
        }
        /// <summary>
        /// Records keyboard input in to the String name, used for the highscore
        /// </summary>
        private static void KeyboardInputToName()
        {
            if (ReturnPlaceInHighScore() <= 10 && Player.score > 0)
            {
                foreach (Keys key in KeyMouseReader.keyState.GetPressedKeys())
                {
                    if (KeyMouseReader.oldKeyState.IsKeyUp(key))
                    {
                        if (key == Keys.Back)
                        {
                            if (name.Length > 0)
                            {
                                name = name.Remove(name.Length - 1, 1);
                            }
                        }
                        else if (key == Keys.Space)
                        {
                            name += ' ';
                        }
                        else if (key == Keys.OemTilde)
                        {
                            name += 'o';
                        }
                        else if (key == Keys.OemCloseBrackets)
                        {
                            name += 'a';
                        }
                        else if (key == Keys.OemQuotes)
                        {
                            name += 'a';
                        }
                        else if ((key == Keys.LeftShift) || (key == Keys.RightShift) || (key == Keys.Left) || (key == Keys.Right) || (key == Keys.Up) || (key == Keys.Down) || (key == Keys.LeftAlt) ||
                            (key == Keys.RightAlt))
                        { }
                        else if (key == Keys.Enter)
                        {
                            SaveHighscore();
                            Player.score = 0;
                            highScoreState = state.show;
                        }
                        else
                        {
                            name += key.ToString();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Reads from the highscore.txt file and adds the entries to List<KeyValuePair<int, string>> listHighScore
        /// </summary>
        private static void AddHighScoreFromTxtToList()
        {
            listHighscore = new List<KeyValuePair<int, string>>();
            if (File.Exists("highscore.txt"))
            {
                sr = new StreamReader("highscore.txt");
            }
            else
            {
                File.Create("highscore.txt").Close();
                sr = new StreamReader("highscore.txt");
            }
            while (!sr.EndOfStream)
            {
                string textString = sr.ReadLine();
                int score = Convert.ToInt16(textString.Split(':')[0]);
                string name = textString.Split(':')[1];

                listHighscore.Add(new KeyValuePair<int, string>(score, name));
            }
            sr.Close();
            listHighscore = listHighscore.OrderByDescending(x => x.Key).ToList();
        }

        /// <summary>
        /// If the player score is good enough for top 10 it returns the place
        /// </summary>
        private static int ReturnPlaceInHighScore()
        {
            AddHighScoreFromTxtToList();
            int placeInHighscore = 0, i = 1;

            foreach (KeyValuePair<int, string> s in listHighscore)
            {
                if (Player.score > s.Key)
                {
                    placeInHighscore = i;
                    break;
                }
                i++;
            }
            if (placeInHighscore == 0 && listHighscore.Count <= 10) //Last of the ones there is
            {
                placeInHighscore = listHighscore.Count + 1;
            }

            return placeInHighscore;
        }

        /// <summary>
        /// Adds the listHighscore List to highscore.txt. If there are more than 10 entries one is removed.
        /// </summary>
        private static void SaveHighscore()
        {
            AddHighScoreFromTxtToList();
            if (listHighscore.Count > 9)
            {
                listHighscore.RemoveAt(listHighscore.Count - 1);
            }
            listHighscore.Add(new KeyValuePair<int, string>(Player.score, name));

            File.Create("highscore.txt").Close(); //Creates a new empty file
            sw = new StreamWriter("highscore.txt", true);

            foreach (KeyValuePair<int, string> s in listHighscore)
            {
                sw.WriteLine(s.Key + ":" + s.Value);
            }
            sw.Close();
            name = "";
        }
        /// <summary>
        /// Sort and then draw the top 10 highscore from highscore.txt
        /// </summary>
        private static void DrawHighScore(SpriteBatch spriteBatch)
        {
            AddHighScoreFromTxtToList();

            spriteBatch.DrawString(Constants.FONT_BIG, "HIGHSCORE", new Vector2(20, 20), Constants.fontColor);


            for (int i = 0; i < listHighscore.Count; i++)
            {
                spriteBatch.DrawString(Constants.FONT, (i + 1).ToString(), new Vector2(20, 100 + (i * 30)), new Color(124, 65, 3)); //# in list
                spriteBatch.DrawString(Constants.FONT, listHighscore[i].Key.ToString(), new Vector2(60, 100 + (i * 30)), Constants.fontColor); //Score
                spriteBatch.DrawString(Constants.FONT, listHighscore[i].Value, new Vector2(190, 100 + (i * 30)), Constants.fontColor); //Name
            }
        }

        private static void DrawHighScoreEntering(SpriteBatch spriteBatch)
        {
            AddHighScoreFromTxtToList();

            spriteBatch.DrawString(Constants.FONT_BIG, "HIGHSCORE", new Vector2(20, 20), Constants.fontColor);
            int offset = 0;
            int numberOfEntriesToShow;

            if (listHighscore.Count < 10)
            {
                numberOfEntriesToShow = listHighscore.Count;
                if (Player.score > 0)
                {
                    numberOfEntriesToShow++;
                }
            }
            else
            {
                numberOfEntriesToShow = 10;
            }

            for (int i = 0; i < numberOfEntriesToShow; i++)
            {
                if (i == ReturnPlaceInHighScore() - 1) //Color green and draw current Score if it is a new highscore
                {
                    spriteBatch.DrawString(Constants.FONT, (i + 1).ToString(), new Vector2(20, 100 + (i * 30)), Color.Green); //# in list
                    spriteBatch.DrawString(Constants.FONT, Player.score.ToString(), new Vector2(60, 100 + (i * 30)), Color.Green); //Score
                    if (listHighscore.Count >= 10)
                    {
                        i--; //So that the highscore after the new one gets drawn
                        offset = 30; //To move the other entries down and make room for the new one
                    }
                }
                else
                {
                    spriteBatch.DrawString(Constants.FONT, (i + 1).ToString(), new Vector2(20, 100 + (i * 30)), new Color(124, 65, 3)); //# in list
                    spriteBatch.DrawString(Constants.FONT, listHighscore[i].Key.ToString(), new Vector2(60, 100 + (i * 30) + offset), Constants.fontColor); //Score
                    spriteBatch.DrawString(Constants.FONT, listHighscore[i].Value, new Vector2(190, 100 + (i * 30) + offset), Constants.fontColor); //Name
                }
            }
        }

        /// <summary>
        /// Draws name, as typed, on screen if top 10 is reached. Otherwise it just show a text saying better luck next time...
        /// </summary>
        private static void DrawNameAsTyped(SpriteBatch spriteBatch)
        {
            if (ReturnPlaceInHighScore() <= 10 && Player.score > 0)
            {
                if (name != null)
                {
                    spriteBatch.DrawString(Constants.FONT, name, new Vector2(190, 100 + ((ReturnPlaceInHighScore() - 1) * 30)), Color.Green);
                }
                else
                {
                    spriteBatch.DrawString(Constants.FONT, ".........", new Vector2(190, 100 + ((ReturnPlaceInHighScore() - 1) * 30)), Color.Green);
                }
                spriteBatch.DrawString(Constants.FONT, "Type to enter your name\npress enter when you are done.", new Vector2(20, 440), Color.DarkOrange);
            }
            else
            {
                spriteBatch.DrawString(Constants.FONT, "You did not reach the highscore...",new Vector2(20, 440), Color.DarkOrange);
                spriteBatch.DrawString(Constants.FONT, "You only got " + Player.score + " points.", new Vector2(20, 470), Constants.fontColor);
                spriteBatch.DrawString(Constants.FONT, "Better luck next time, punk.", new Vector2(20, 500), Color.DarkOrange);
            }
        }
    }
}
