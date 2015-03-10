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
        private GameObject selectedObject;

        KeyboardState keyBoardState;
        MouseState mouseState, oldMouseState;
        private int camSpeed = 4;
        private bool saved;
        private bool drawLevelSaved;
        private double drawTextTimer, drawTextTimerDefault = 1000;
        enum items { Platform, Player, RemovingObject }; //osv...
        items selectedItem;
        private bool showMenu;

        private String strItemMenu = "P: Platform\nY: Player";
        private String strItemMenu2 = "Right-click for remove tool\n\",\" Zooms out and \".\" Zooms in\nCtrl-S to save";

        public LevelEditor(GraphicsDevice gd, String levelTextFilePath, ContentManager content) :
            base(gd, levelTextFilePath, content)
        {
            drawTextTimer = drawTextTimerDefault;
            LoadContent(levelTextFilePath);
            selectedItem = items.Platform;
            selectedObject = new Platform(new Vector2(mouseState.X, mouseState.Y), TextureManager.platform, true);
            camera.Position = new Vector2(0, levelHeight - Game1.windowHeight);
        }
        public override void Update(GameTime gameTime)
        {
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
            //Zoom();

            if (KeyMouseReader.KeyPressed(Keys.D1))
            {
                editingLevel = 0;
                LoadContent(Constants.LEVELS[editingLevel]);
            }
            if (KeyMouseReader.KeyPressed(Keys.D2))
            {
                editingLevel = 1;
                LoadContent(Constants.LEVELS[editingLevel]);
            }
            if (KeyMouseReader.KeyPressed(Keys.D3))
            {
                editingLevel = 2;
                LoadContent(Constants.LEVELS[editingLevel]);
            }
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
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(camera.GetViewMatrix(Vector2.One)));
        }
        /// <summary>
        /// Adds the selected item to the objects list at the mouse position or snapped to a platform
        /// </summary>
        private void AddSelectedItemOnLeftClick()
        {
            Platform intersectingPlatform;
            if (KeyMouseReader.LeftClick())
            {
                Vector2 mouseIsAt = ScreenToWorld(new Vector2(mouseState.X, mouseState.Y));  
                switch (selectedItem)
                    {
                        case items.Platform: 
                            intersectingPlatform = IntersectsPlatform(mouseIsAt, selectedObject.BoundingBox);
                            if (intersectingPlatform != null)
                            {
                                if (mouseIsAt.X < intersectingPlatform.pos.X - selectedObject.BoundingBox.Width / 2) //Snap to left or right of exisisting platform
                                {
                                    objects.Add(new Platform(new Vector2(intersectingPlatform.BoundingBox.Left - selectedObject.BoundingBox.Width, intersectingPlatform.pos.Y), 
                                                TextureManager.platform, true));
                                }
                                else
                                {
                                    objects.Add(new Platform(new Vector2(intersectingPlatform.BoundingBox.Right, intersectingPlatform.pos.Y), 
                                                TextureManager.platform, true));
                                }
                            }
                            else
                            {
                                objects.Add(new Platform(new Vector2(mouseIsAt.X, mouseIsAt.Y), TextureManager.platform, true));
                            }
                            break;

                        case items.Player:
                            intersectingPlatform = IntersectsPlatform(mouseIsAt, selectedObject.BoundingBox);
                            if (intersectingPlatform != null)
                            {
                                selectedObject.pos = new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height);
                                objects.Add(selectedObject);
                                //objects.Add(new SpawnPosition(selectedObject.type, new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height),
                                //    selectedObject.BoundingBox));
                            }
                            else
                            {
                                selectedObject.pos = new Vector2(mouseIsAt.X, mouseIsAt.Y);
                                objects.Add(selectedObject);
                                //objects.Add(new SpawnPosition(Constants.spawnCharSymbol, new Vector2(mouseIsAt.X, mouseIsAt.Y), Constants.SPAWN_SRC_RECT));
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
        private Platform IntersectsPlatform (Vector2 mouseIsAt, Rectangle srcRect)
        {
            foreach (GameObject anObject in objects)
            {
                if (anObject is Platform && anObject.BoundingBox.Intersects(new Rectangle((int)mouseIsAt.X, (int)mouseIsAt.Y, srcRect.Width, srcRect.Height)))
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
                selectedItem = items.RemovingObject;
            }
            if (KeyMouseReader.LeftClick() && selectedItem == items.RemovingObject)
            {
                Vector2 mouseIsAt = ScreenToWorld(new Vector2(mouseState.X, mouseState.Y)); 
                foreach (GameObject o in objects)
                {
                    if (o.BoundingBox.Contains(new Point((int)mouseIsAt.X, (int)mouseIsAt.Y)))
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
                selectedObject = new Platform(new Vector2(mouseState.X, mouseState.Y), TextureManager.platform, true);
            }
            if (KeyMouseReader.KeyPressed(Keys.Y))
            {
                selectedItem = items.Player;
<<<<<<< HEAD

                selectedObject = new Player(new Vector2(mouseState.X, mouseState.Y), TextureManager.player, 
                                            new Rectangle(0,0,100,200), true, 1, 80);

=======
                selectedObject = new Player(new Vector2(mouseState.X, mouseState.Y), Textures.player, new Rectangle(0,0,100,200), true, 1);
>>>>>>> parent of 95fa569... LevelEditor har nu Zoom med musScroll
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
                sw.Write(g.type+":"+g.pos.X+":"+g.pos.Y+"\r\n");
            }

            sw.Close();
            drawLevelSaved = true;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (BackgroundLayer layer in layers) //Ritar ut varje lager med alla bakgrunder som finns i respektive
                layer.Draw(spriteBatch);

            Vector2 parallax = Vector2.One; //parallax vector2.one innebär ingen parallax, kameran rör sig med full hastighet. Mindre värde gör den långsammare
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(parallax));
            foreach (GameObject g in objects)
            {
                g.Draw(spriteBatch);
            }
            
            spriteBatch.End();

            spriteBatch.Begin(); //Allt som inte ska följa med kameran ritas ut här
            spriteBatch.Draw(selectedObject.tex, new Vector2(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y), selectedObject.srcRect, Color.White);
            
            if (drawLevelSaved)
            {
                spriteBatch.DrawString(Constants.FONT_BIG, "Level has been saved", 
                    new Vector2(Game1.windowWidth / 2 - Constants.FONT_BIG.MeasureString("Level has been saved").X / 2, 
                                Game1.windowHeight / 2 - Constants.FONT.MeasureString("Level has been saved").Y / 2), Color.White);
            }
            if(showMenu)
            {
                spriteBatch.DrawString(Constants.FONT, strItemMenu, new Vector2(10, 10), Color.White);
                spriteBatch.DrawString(Constants.FONT, strItemMenu2, new Vector2(250, 10), Color.White);
                
            }
            else
            {
                spriteBatch.DrawString(Constants.FONT, "Currently Editing: " + Constants.LEVELS[editingLevel] + "\nF1 for menu", new Vector2(10, 10), Color.White);
                spriteBatch.DrawString(Constants.FONT, "Cam pos: " + camera.Position, new Vector2(10, 60), Color.White);  
            }
            spriteBatch.End();
            
        }
    }
}
