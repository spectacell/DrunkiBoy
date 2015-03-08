using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DrunkiBoy
{
    class LevelEditor : Level
    {
        private int editingLevel = 0;
        private Rectangle itemSrcRect;
        KeyboardState keyBoardState;
        MouseState mouseState, oldMouseState;
        private int camSpeed = 4;
        private bool saved;
        private bool drawLevelSaved;
        private double drawTextTimer, drawTextTimerDefault = 1000;
        enum items { Platform, Player }; //osv...
        items selectedItem;
        private bool showMenu;

        private int nrOfSpawns;
        private String strItemMenu = "P: Platform\nY: Player";
        private String strItemMenu2 = "Right-click for remove tool\n\",\" Zooms out and \".\" Zooms in\nCtrl-S to save";

        public LevelEditor(GraphicsDevice gd, String levelTextFilePath, ContentManager content) :
            base(gd, levelTextFilePath, content)
        {
            drawTextTimer = drawTextTimerDefault;
            LoadContent(levelTextFilePath);
            selectedItem = items.Platform;
            itemSrcRect = Constants.PLATFORM_SRC_RECT;
        }
        public override void Update(GameTime gameTime)
        {
            nrOfSpawns = 0;
            //foreach (GameObject ob in objects)
            //{
            //    if (ob is SpawnPosition)
            //        nrOfSpawns++;
            //}
            keyBoardState = Keyboard.GetState();

            oldMouseState = mouseState;
            mouseState = Mouse.GetState();

            F1ToShowMenu();
            MoveCamera();

            SelectItemBasedOnKeyInput();

            RemoveItem();

            AddSelectedItemOnLeftClick();

            SaveOnCtrlS();

            CountDownDrawLevelSavedTimer(gameTime);
            Zoom();

            //if (KeyMouseReader.KeyPressed(Keys.D1))
            //{
            //    editingLevel = 0;
            //    LoadContent(Constants.LEVELS[editingLevel], true);
            //}
            //if (KeyMouseReader.KeyPressed(Keys.D2))
            //{
            //    editingLevel = 1;
            //    LoadContent(Constants.LEVELS[editingLevel], true);
            //}
            //if (KeyMouseReader.KeyPressed(Keys.D3))
            //{
            //    editingLevel = 2;
            //    LoadContent(Constants.LEVELS[editingLevel], true);
            //}
        }
        
        private void CountDownDrawLevelSavedTimer(GameTime gameTime)
        {
            if (drawLevelSaved)
            {
                drawTextTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (drawTextTimer <= 0)
                {
                    drawTextTimer = drawTextTimerDefault;
                    drawLevelSaved = false;
                }
            }
        }

        private void F1ToShowMenu()
        {
            if (KeyMouseReader.KeyPressed(Keys.F1))
            {
                showMenu = !showMenu;
            }
        }

        private void SaveOnCtrlS()
        {
            if ((keyBoardState.IsKeyDown(Keys.LeftControl) || keyBoardState.IsKeyDown(Keys.RightControl)) && keyBoardState.IsKeyDown(Keys.S) && !saved)
            {
                SaveLevelToFile();
                saved = true;
            }
        }
        /// <summary>
        /// Adds the selected item to the objects list at the mouse position or snapped to a platform
        /// </summary>
        private void AddSelectedItemOnLeftClick()
        {
            Platform intersectingPlatform;
            if (KeyMouseReader.LeftClick())
            {
                
                Point mouseIsAt = new Point(mouseState.X + (int)camera.Position.X - (Game1.windowWidth / 2), mouseState.Y + (int)camera.Position.Y - (Game1.windowHeight / 2));
                    switch (selectedItem)
                    {
                        case items.Platform: 
                            intersectingPlatform = IntersectsPlatform(mouseIsAt, Constants.PLATFORM_SRC_RECT);
                            if (intersectingPlatform != null)
                            {
                                if (mouseIsAt.X < intersectingPlatform.pos.X - Constants.PLAYER_SRC_RECT.Width/2) //Snap to left or right of exisisting platform
                                {
                                    objects.Add(new Platform(Constants.platformCharSymbol, 
                                        new Vector2(intersectingPlatform.BoundingBox.Left - Constants.PLATFORM_SRC_RECT.Width, intersectingPlatform.pos.Y), Constants.PLATFORM_SRC_RECT));
                                }
                                else
                                {
                                    objects.Add(new Platform(Constants.platformCharSymbol, new Vector2(intersectingPlatform.BoundingBox.Right, intersectingPlatform.pos.Y), Constants.PLATFORM_SRC_RECT));
                                }
                            }
                            else
                            {
                                objects.Add(new Platform(Constants.platformCharSymbol, new Vector2(mouseIsAt.X, mouseIsAt.Y), Constants.PLATFORM_SRC_RECT));
                            }
                            break;

                        case items.Player:
                            intersectingPlatform = IntersectsPlatform(mouseIsAt, Constants.PLAYER_SRC_RECT);
                            if (intersectingPlatform != null)
                            {
                                objects.Add(new Player(Constants.playerCharSymbol, new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - Constants.PLAYER_SRC_RECT.Height),
                                    Constants.PLAYER_SRC_RECT));
                                objects.Add(new SpawnPosition(Constants.spawnCharSymbol, new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - Constants.SPAWN_SRC_RECT.Height),
                                    Constants.SPAWN_SRC_RECT));
                            }
                            else
                            {
                                objects.Add(new Player(Constants.playerCharSymbol, new Vector2(mouseIsAt.X, mouseIsAt.Y), Constants.PLAYER_SRC_RECT));
                                objects.Add(new SpawnPosition(Constants.spawnCharSymbol, new Vector2(mouseIsAt.X, mouseIsAt.Y), Constants.SPAWN_SRC_RECT));
                            }
                            break;
                    }
                    
                    saved = false;
                
            }
        }
        /// <summary>
        /// Checks if the item that is placed intersects a platform
        /// </summary>
        /// <param name="mouseIsAt">Coordinates of the mouse</param>
        /// <param name="srcRect">Source Rectangle of the selected Item</param>
        /// <returns></returns>
        private Platform IntersectsPlatform (Point mouseIsAt, Rectangle srcRect)
        {
            foreach (GodObject anObject in objects)
            {
                if (anObject is Platform && anObject.BoundingBox.Intersects(new Rectangle(mouseIsAt.X, mouseIsAt.Y, srcRect.Width, srcRect.Height)))
                {
                    return (Platform)anObject;
                }
            }
            return null;
        }

        /// <summary>
        /// Changes to the remove tool if right-clicka and then removes if left-click
        /// </summary>
        private void RemoveItem()
        {
            if (KeyMouseReader.RightClick())
            {
                selectedItem = items.Arrow;
                itemSrcRect = Constants.ARROW_SRC_RECT;
            }
            if (KeyMouseReader.LeftClick() && selectedItem == items.Arrow)
            {
                Point mouseIsAt = new Point(mouseState.X + (int)camera.Position.X - (Game1.windowWidth / 2), mouseState.Y + (int)camera.Position.Y - (Game1.windowHeight / 2));
                foreach (GodObject o in objects)
                {
                    if (o.BoundingBox.Contains(mouseIsAt))
                    {
                        objects.Remove(o);
                        break;
                    }
                }
                saved = false;
            }
        }
        /// <summary>
        /// Change the selectedItem and itemSrcRect based on keyboard input
        /// </summary>
        private void SelectItemBasedOnKeyInput()
        {
            if (KeyMouseReader.KeyPressed(Keys.P))
            {
                selectedItem = items.Platform;
                itemSrcRect = Constants.PLATFORM_SRC_RECT;
            }
            if (KeyMouseReader.KeyPressed(Keys.Y))
            {
                selectedItem = items.Player;
                itemSrcRect = Constants.PLAYER_SRC_RECT;
            }
        }

        private void MoveCamera()
        {
            if (keyBoardState.IsKeyDown(Keys.Left))
            {
                camera.Move(new Vector2(-camSpeed, 0));
            }
            if (keyBoardState.IsKeyDown(Keys.Right))
            {
                camera.Move(new Vector2(+camSpeed, 0));
            }
            if (keyBoardState.IsKeyDown(Keys.Up))
            {
                camera.Move(new Vector2(0, -camSpeed));
            }
            if (keyBoardState.IsKeyDown(Keys.Down))
            {
                camera.Move(new Vector2(0, +camSpeed));
            }
        }

        //private void Zoom()
        //{
        //    if (KeyMouseReader.KeyPressed(Keys.OemComma))
        //    {
        //        cam.Zoom -= 0.1f;
        //    }
        //    if (KeyMouseReader.KeyPressed(Keys.OemPeriod))
        //    {
        //        cam.Zoom += 0.1f;
        //    }
        //}
        /// <summary>
        /// Writes from the objects list to the current Level text-file.
        /// </summary>
        private void SaveLevelToFile()
        {
            StreamWriter sw;

            sw = new StreamWriter(Constants.LEVELS[editingLevel]);

            foreach (GameObject g in objects)
            {
                sw.Write(g.charSymbol+":"+g.pos.X+":"+g.pos.Y+"\r\n");
            }

            sw.Close();
            drawLevelSaved = true;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 parallax = new Vector2(1f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(parallax));
            foreach (GameObject g in objects)
            {
                g.Draw(spriteBatch);
            }
            
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(Constants.SPRITESHEET, new Vector2(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y), itemSrcRect, Color.White);
            
            if (drawLevelSaved)
            {
                spriteBatch.DrawString(Constants.FONT_BIG, "Level has been saved", 
                    new Vector2(Game1.windowWidth / 2 - Constants.FONT_BIG.MeasureString("Level has been saved").X / 2, 
                                Game1.windowHeight / 2 - Constants.FONT.MeasureString("Level has been saved").Y / 2), Color.Black);
            }
            if(showMenu)
            {
                spriteBatch.DrawString(Constants.FONT, strItemMenu, new Vector2(10, 10), Color.White);
                spriteBatch.DrawString(Constants.FONT, strItemMenu2, new Vector2(250, 10), Color.White);
                
            }
            else
            {
                spriteBatch.DrawString(Constants.FONT, "Currently Editing: " + Constants.LEVELS[editingLevel] + "\nF1 for menu", new Vector2(10, 10), Color.Black);
            }
            spriteBatch.DrawString(Constants.FONT, "Spawn positions: " + nrOfSpawns, new Vector2(Game1.windowWidth-220, 10), Color.Gold);
            spriteBatch.End();
            foreach (BackgroundLayer layer in layers) //Ritar ut varje lager med alla bakgrunder som finns i respektive
                layer.Draw(spriteBatch);
        }
    }
}
