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
        enum items { Platform, Player, RemovingObject, Torch, Key, Heart, Painkiller, Teleport, Money, Pant, Burger, Pizza, Bottle, Jagerbomb, Flashlight, Radio, Kebab, Toilet}; //osv...
        items selectedItem;

        private int editingLevel = 0;
        private GameObject selectedObject;

        KeyboardState keyBoardState;
        MouseState mouseState, oldMouseState;
        protected Int32 scroll;

        private int camSpeed = 4;
        private bool saved;
        private bool drawLevelSaved;
        private double drawTextTimer, drawTextTimerDefault = 1000;

        private bool showMenu;
        private List<string> menuEntries = new List<string>();
        private string menuInstructions = "Right-click for remove tool\nScroll mousewheel to zoom\nCtrl-S to save";

        public LevelEditor(GraphicsDevice gd, String levelTextFilePath, ContentManager content) :
            base(gd, levelTextFilePath, content)
        {
            menuEntries.Add("P: Platform");
            menuEntries.Add("Y: Player");
            menuEntries.Add("T: Torch");
            menuEntries.Add("K: Key");
            menuEntries.Add("H: Heart");
            menuEntries.Add("S: Painkiller");
            menuEntries.Add("F: Teleport");
            menuEntries.Add("M: Money");
            menuEntries.Add("E: Pant");
            menuEntries.Add("B: Burger");
            menuEntries.Add("V: Pizza");
            menuEntries.Add("C: Bottle");
            menuEntries.Add("J: Jagerbomb");
            menuEntries.Add("L: Flashlight");
            menuEntries.Add("R: Radio");
            menuEntries.Add("A: Kebab");
            menuEntries.Add("I: Toilet");

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
            screenPosition = Vector2.Transform(screenPosition, Matrix.Invert(camera.GetViewMatrix(Vector2.One)));
            screenPosition = new Vector2((int)screenPosition.X, (int)screenPosition.Y);
            return screenPosition;
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
                            objects.Add(new Player(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.player, new Rectangle(0, 0, 95, 146), true, 1, 80));
                        }
                        else
                        {
                            objects.Add(new Player(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.player, new Rectangle(0, 0, 95, 146), true, 1, 80));
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
                    case items.Teleport:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Teleport(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.teleport, new Rectangle(0, 0, 200, 267), true, 1, 50));
                        }
                        else
                        {
                            objects.Add(new Teleport(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.teleport, new Rectangle(0, 0, 200, 267), true, 1, 50));
                        }
                        break;
                    case items.Money:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Money(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.money, new Rectangle(0, 0, 70, 32), true));
                        }
                        else
                        {
                            objects.Add(new Money(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.money, new Rectangle(0, 0, 70, 32), true));
                        }

                        break;
                    case items.Pant:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Pant(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.pant, new Rectangle(0, 0, 33, 77), true));
                        }
                        else
                        {
                            objects.Add(new Pant(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.pant, new Rectangle(0, 0, 33, 77), true));
                        }

                        break;

                    case items.Burger:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Burger(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.hamburgare, true));
                        }
                        else
                        {
                            objects.Add(new Burger(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.hamburgare, true));
                        }
                        break;
                    case items.Pizza:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Pizza(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.pizza, true));
                        }
                        else
                        {
                            objects.Add(new Pizza(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.pizza, true));
                        }
                        break;
                    case items.Kebab:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Kebab(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.kebab, true));
                        }
                        else
                        {
                            objects.Add(new Kebab(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.kebab, true));
                        }
                        break;
                    case items.Bottle:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Bottle(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.bottle, true));
                        }
                        else
                        {
                            objects.Add(new Bottle(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.bottle, true));
                        }
                        break;
                    case items.Jagerbomb:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Jagerbomb(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.jagerbomb, true));
                        }
                        else
                        {
                            objects.Add(new Jagerbomb(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.jagerbomb, true));
                        }
                        break;
                    case items.Flashlight:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Flashlight(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.flashlight, new Rectangle(0, 0, 109, 146), true, 1, 50));
                        }
                        else
                        {
                            objects.Add(new Flashlight(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.flashlight, new Rectangle(0, 0, 109, 146), true, 1, 50));
                        }
                        break;
                    case items.Radio:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Radio(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.radio, new Rectangle(0, 0, 174, 114), true, 1, 50));
                        }
                        else
                        {
                            objects.Add(new Radio(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.radio, new Rectangle(0, 0, 174, 114), true, 1, 50));
                        }
                        break;
                    case items.Toilet:
                        if (intersectingPlatform != null)
                        {
                            objects.Add(new Toilet(new Vector2(mouseIsAt.X, intersectingPlatform.BoundingBox.Top - selectedObject.BoundingBox.Height), Textures.toilet_closed, true));
                        }
                        else
                        {
                            objects.Add(new Toilet(new Vector2(mouseIsAt.X, mouseIsAt.Y), Textures.toilet_closed, true));
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
                selectedObject = new Player(new Vector2(mouseState.X, mouseState.Y), Textures.player, new Rectangle(0, 0, 95, 146), true, 1, 80);
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
            if (KeyMouseReader.KeyPressed(Keys.F))
            {
                selectedItem = items.Teleport;
                selectedObject = new Teleport(new Vector2(mouseState.X, mouseState.Y), Textures.teleport, new Rectangle(0, 0, 200, 267), true, 1, 50);
            }
            if (KeyMouseReader.KeyPressed(Keys.M))
            {
                selectedItem = items.Money;
                selectedObject = new Money(new Vector2(mouseState.X, mouseState.Y), Textures.money, new Rectangle(0, 0, 70, 32), true);
            }
            if (KeyMouseReader.KeyPressed(Keys.E))
            {
                selectedItem = items.Pant;
                selectedObject = new Pant(new Vector2(mouseState.X, mouseState.Y), Textures.pant, new Rectangle(0, 0, 33, 77), true);
            }
            if (KeyMouseReader.KeyPressed(Keys.B))
            {
                selectedItem = items.Burger;
                selectedObject = new Burger(new Vector2(mouseState.X, mouseState.Y), Textures.hamburgare, true);
            }
            if (KeyMouseReader.KeyPressed(Keys.V))
            {
                selectedItem = items.Pizza;
                selectedObject = new Pizza(new Vector2(mouseState.X, mouseState.Y), Textures.pizza, true);
            }
            if (KeyMouseReader.KeyPressed(Keys.C))
            {
                selectedItem = items.Bottle;
                selectedObject = new Bottle(new Vector2(mouseState.X, mouseState.Y), Textures.bottle, true);
            }
            if (KeyMouseReader.KeyPressed(Keys.J))
            {
                selectedItem = items.Jagerbomb;
                selectedObject = new Jagerbomb(new Vector2(mouseState.X, mouseState.Y), Textures.jagerbomb, true);
            }
            if (KeyMouseReader.KeyPressed(Keys.L))
            {
                selectedItem = items.Flashlight;
                selectedObject = new Flashlight(new Vector2(mouseState.X, mouseState.Y), Textures.flashlight, new Rectangle(0, 0, 109, 146), true, 1, 50);
            }
            if (KeyMouseReader.KeyPressed(Keys.R))
            {
                selectedItem = items.Radio;
                selectedObject = new Radio(new Vector2(mouseState.X, mouseState.Y), Textures.radio, new Rectangle(0, 0, 174, 114), true, 1, 50);
            }
            if (KeyMouseReader.KeyPressed(Keys.A))
            {
                selectedItem = items.Kebab;
                selectedObject = new Kebab(new Vector2(mouseState.X, mouseState.Y), Textures.kebab, true);
            }
            if (KeyMouseReader.KeyPressed(Keys.I))
            {
                selectedItem = items.Toilet;
                selectedObject = new Toilet(new Vector2(mouseState.X, mouseState.Y), Textures.toilet_closed, true);
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
            spriteBatch.Draw(selectedObject.tex, new Vector2(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y), selectedObject.srcRect, Color.White, 0, Vector2.Zero, camera.Zoom, SpriteEffects.None, 0);
            if (drawLevelSaved)
            {
                spriteBatch.DrawString(Constants.FONT_BIG, "Level has been saved",
                    new Vector2(Game1.windowWidth / 2 - Constants.FONT_BIG.MeasureString("Level has been saved").X / 2,
                                Game1.windowHeight / 2 - Constants.FONT.MeasureString("Level has been saved").Y / 2), Constants.fontColor);
            }
            if (showMenu)
            {
                spriteBatch.DrawString(Constants.FONT, menuInstructions, new Vector2(10, 10), Constants.fontColor);
                
                Vector2 entryPos = new Vector2(10, 100);
                int offset = 0;
                foreach (string entry in menuEntries)
                {
                    spriteBatch.DrawString(Constants.FONT, entry, new Vector2(entryPos.X, entryPos.Y + offset), Constants.fontColor);
                    offset += 25;
                }
            }
            else
            {
                spriteBatch.DrawString(Constants.FONT, "Currently Editing: " + Constants.LEVELS[editingLevel] + "\nF1 for menu", new Vector2(10, 10), Constants.fontColor);
                spriteBatch.DrawString(Constants.FONT, "Cam pos: " + camera.Position, new Vector2(10, 60), Constants.fontColor);
            }
            spriteBatch.End();

        }
    }
}
