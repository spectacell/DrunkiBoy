﻿using Microsoft.Xna.Framework;
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
        private String strItemMenu = "P: Platform\nY: Player"; //osv...           
        enum items { Platform, Player, RemovingObject, Torch, Key, Heart, Painkiller }; //osv...

        private int editingLevel = 0;
        private GameObject selectedObject;

        KeyboardState keyBoardState;
        MouseState mouseState, oldMouseState;
        protected Int32 scroll;

        private int camSpeed = 4;
        private bool saved;
        private bool drawLevelSaved;
        private double drawTextTimer, drawTextTimerDefault = 1000;

        items selectedItem;
        private bool showMenu;
        private String strItemMenu2 = "Right-click for remove tool\n\",\" Zooms out and \".\" Zooms in\nCtrl-S to save";
        private String strItemMenu3 = "T: Torch ";
        private String strItemManu4 = "K: Key ";
        private String strItemMenu5 = "H: Heart ";
        private String strItemMenu6 = "S: Painkiller ";

        public LevelEditor(GraphicsDevice gd, String levelTextFilePath, ContentManager content) :
            base(gd, levelTextFilePath, content)
        {
            drawTextTimer = drawTextTimerDefault;
            LoadContent(levelTextFilePath);
            selectedItem = items.Platform;
            selectedObject = new Platform(new Vector2(mouseState.X, mouseState.Y), Textures.platform, true);
            camera.Position = new Vector2(0, levelHeight - Game1.windowHeight);
        }
        public override void Update(GameTime gameTime)
        {
            keyBoardState = Keyboard.GetState();

            oldMouseState = mouseState;
            mouseState = Mouse.GetState();

            F1ToShowMenu();
            MoveCamera();
            Zoom();
            camera.Update();

            SelectItemBasedOnKeyInput();

            RemoveItem();

            AddSelectedItemOnLeftClick();

            SaveOnCtrlS();

            CountDownDrawLevelSavedTimer(gameTime);


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
                intersectingPlatform = IntersectsPlatform(mouseIsAt, selectedObject.BoundingBox); //Rör objektet vid någon plattform så returneras vilken det är

                switch (selectedItem)
                {
                    case items.Platform:
                        if (intersectingPlatform != null)
                        {
                            if (mouseIsAt.X < intersectingPlatform.pos.X - selectedObject.BoundingBox.Width / 2) //Snap to left or right of exisisting platform
                            {
                                objects.Add(new Platform(new Vector2(intersectingPlatform.BoundingBox.Left - selectedObject.BoundingBox.Width, intersectingPlatform.pos.Y), Textures.platform, true));
                            }
                            else
                            {
                                objects.Add(new Platform(new Vector2(intersectingPlatform.BoundingBox.Right, intersectingPlatform.pos.Y), Textures.platform, true));
                            }
                        }
                        else
                        {
                            objects.Add(new Platform(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.platform, true));
                        }
                        break;

                    case items.Player:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Player(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.player, new Rectangle(0, 0, 100, 200), true, 1, 80));
                        }
                        else
                        {
                            objects.Add(new Player(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.player, new Rectangle(0, 0, 100, 200), true, 1, 80));
                        }
                        break;

                    case items.Torch:
                        if (intersectingPlatform!= null)
                        {
                            objects.Add(new Torch(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.torchTex, new Rectangle(0, 0, 60, 53), true, 1, 50));

                        }
                        else
                        {
                            objects.Add(new Torch(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.torchTex, new Rectangle(0, 0, 60, 53), true, 1, 50));
                        }
                        break;
                    case items.Key:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Key(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.Key, new Rectangle(0, 0, 35, 64), true, 1, 50));

                        }
                        else
                        {
                            objects.Add(new Key(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.Key, new Rectangle(0, 0, 35, 64), true, 1, 50));
                        }
                        break;
                    case items.Heart:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Heart(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.heart, new Rectangle(0, 0, 64, 32), true, 1, 50));

                        }
                        else
                        {
                            objects.Add(new Heart(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.heart, new Rectangle(0, 0, 31, 32), true, 1, 50));
                        }
                        break;
                    case items.Painkiller:
                        if (intersectingPlatform!= null)
                        {
                            objects.Add(new Painkiller(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.painkiller, new Rectangle(0, 0, 53, 37), true, 1, 50));
                        }
                        else
                        {
                            objects.Add(new Painkiller(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.painkiller, new Rectangle(0, 0, 53, 37), true, 1, 50));
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
        private Platform IntersectsPlatform(Vector2 mouseIsAt, Rectangle srcRect)
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
                selectedObject = new Cursor_RemoveItem(new Vector2(mouseState.X, mouseState.Y), Textures.deleteCursor, true);
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
                selectedObject = new Platform(new Vector2(mouseState.X, mouseState.Y), Textures.platform, true);
            }
            if (KeyMouseReader.KeyPressed(Keys.Y))
            {
                selectedItem = items.Player;
                selectedObject = new Player(new Vector2(mouseState.X, mouseState.Y), Textures.player, new Rectangle(0, 0, 138, 190), true, 1, 80);
            }
            if (KeyMouseReader.KeyPressed(Keys.T))
            {
                selectedItem = items.Torch;
                selectedObject = new Torch(new Vector2(mouseState.X, mouseState.Y), Textures.torchTex, new Rectangle(0, 0, 60, 53), true, 1, 50);
            }
            if (KeyMouseReader.KeyPressed(Keys.K))
            {
                selectedItem = items.Key;
                selectedObject = new Key(new Vector2(mouseState.X, mouseState.Y), Textures.Key, new Rectangle(0, 0, 20, 30), true, 1, 50);
            }
            if (KeyMouseReader.KeyPressed(Keys.H))
            {
                selectedItem = items.Heart;
                selectedObject = new Heart(new Vector2(mouseState.X, mouseState.Y), Textures.heart, new Rectangle(0, 0, 31, 26), true, 1, 50);
            }
            if (KeyMouseReader.KeyPressed(Keys.S))
            {
                selectedItem = items.Painkiller;
                selectedObject = new Painkiller(new Vector2(mouseState.X, mouseState.Y), Textures.painkiller, new Rectangle(0, 0, 53, 37), true, 1, 50);
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

        private void Zoom()
        {
            if (mouseState.ScrollWheelValue > scroll)
            {
                camera.Zoom += 0.1f;
                scroll = mouseState.ScrollWheelValue;
            }
            else if (mouseState.ScrollWheelValue < scroll)
            {
                camera.Zoom -= 0.1f;
                scroll = mouseState.ScrollWheelValue;
            }
        }
        /// <summary>
        /// Writes from the objects list to the current Level text-file.
        /// </summary>
        private void SaveLevelToFile()
        {
            StreamWriter sw;

            sw = new StreamWriter(Constants.LEVELS[editingLevel]);

            foreach (GameObject g in objects)
            {
                sw.Write(g.type + ":" + g.pos.X + ":" + g.pos.Y + "\r\n");
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
            if (showMenu)
            {
                spriteBatch.DrawString(Constants.FONT, strItemMenu, new Vector2(10, 10), Color.White);
                spriteBatch.DrawString(Constants.FONT, strItemMenu2, new Vector2(250, 10), Color.White);
                spriteBatch.DrawString(Constants.FONT, strItemMenu3, new Vector2(10, 60), Color.White);
                spriteBatch.DrawString(Constants.FONT, strItemManu4, new Vector2(10,85), Color.White);
                spriteBatch.DrawString(Constants.FONT, strItemMenu5, new Vector2(10, 110), Color.White);
                spriteBatch.DrawString(Constants.FONT, strItemMenu6, new Vector2(10, 135), Color.White);
                    

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
