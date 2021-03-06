﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class ItemManager
    {
        public List<Platform> platforms = new List<Platform>();
        public List<MovingPlatform> movingplatforms = new List<MovingPlatform>();
        public List<Torch> torches = new List<Torch>();
        public List<Key> keys = new List<Key>();
        public List<Wallet> wallets = new List<Wallet>();
        public List<Cellphone> cellphones = new List<Cellphone>();
        public List<Heart> hearts = new List<Heart>();
        public List<Painkiller> painkillers = new List<Painkiller>();
        public List<Teleport> teleports = new List<Teleport>();
        public List<Money> moneys = new List<Money>();
        public List<Pant> pants = new List<Pant>();
        public List<Burger> burgers = new List<Burger>();
        public List<Pizza> pizzas = new List<Pizza>();
        public List<Kebab> kebabs = new List<Kebab>();
        public List<Bottle> bottles = new List<Bottle>();
        public List<Jagerbomb> jagerbombs = new List<Jagerbomb>();
        public List<Toilet> toilets = new List<Toilet>();
        public List<Vodka> vodkas = new List<Vodka>();
        public List<RedbullVodka> redbullVodkas = new List<RedbullVodka>();
        public List<FireOnGround> fires = new List<FireOnGround>();
        public List<Wall> walls = new List<Wall>();
        public List<Door> doors = new List<Door>();
        public List<Button> buttons = new List<Button>();
        public List<Bar> bars = new List<Bar>();
        static public List<BulletNote> bulletNotes = new List<BulletNote>();

        HamburgareVapen hamburgareVapen;
        BottleWeapon bottleWeapon;
        KebabWeapon kebabWeapon;
        PizzaWeapon pizzaWeapon;
        Bar bar;
        private Random rnd = new Random();
        public static ParticleEngine particleEngine = new ParticleEngine();

        public ItemManager()
        {

        }
        #region Add-metoder
        public void AddToilet(Toilet toilet)
        {
            toilets.Add(toilet);
        }
        static public void AddBulletNotes(BulletNote bulletNote)
        {
            bulletNotes.Add(bulletNote);
        }
        public void AddButton(Button button)
        {
            buttons.Add(button);
        }
        public void AddDoor(Door door)
        {
            doors.Add(door);
        }
        public void AddKebab(Kebab kebab)
        {
            kebabs.Add(kebab);
        }
        public void AddJagerbomb(Jagerbomb jagerbomb)
        {
            jagerbombs.Add(jagerbomb);
        }
        public void AddBottle(Bottle bottle)
        {
            bottles.Add(bottle);
        }
        public void AddPizza(Pizza pizza)
        {
            pizzas.Add(pizza);
        }
        public void AddPant(Pant pant)
        {
            pants.Add(pant);
        }
        public void AddMoney(Money money)
        {
            moneys.Add(money);
        }
        public void AddPlatform(Platform platform)
        {
            platforms.Add(platform);
        }
        public void AddTorch(Torch torch)
        {
            torches.Add(torch);
        }
        public void AddKey(Key key)
        {
            keys.Add(key);
        }
        public void AddWallet(Wallet wallet)
        {
            wallets.Add(wallet);
        }
        public void AddCellphone(Cellphone cellphone)
        {
            cellphones.Add(cellphone);
        }
        public void Addheart(Heart heart)
        {
            hearts.Add(heart);
        }
        public void AddPainkiller(Painkiller painkiller)
        {
            painkillers.Add(painkiller);
        }
        public void AddTeleport(Teleport teleport)
        {
            teleports.Add(teleport);
        }
        public void AddBurger(Burger burger)
        {
            burgers.Add(burger);
        }
        public void AddVodka(Vodka vodka)
        {
            vodkas.Add(vodka);
        }
        public void AddRedbullVodka(RedbullVodka redbullVodka)
        {
            redbullVodkas.Add(redbullVodka);
        }
        public void AddWall(Wall wall)
        {
            walls.Add(wall);
        }

        public void AddBar(Bar bar)
        {
            bars.Add(bar);
        }

        #endregion
        public void Update(GameTime gameTime, Player player, List<AngryNeighbour> angryNeighbours)
        {
            UpdatePlatforms(gameTime, player, angryNeighbours);
            UpdateBars(gameTime, player);
            UpdateTorches(gameTime, player);
            UpdateKeys(gameTime, player);
            UpdateWallets(gameTime, player);
            UpdateCellphones(gameTime, player);
            UpdateHearts(gameTime, player);
            UpdatePainkillers(gameTime, player);
            UpdateTeleport(gameTime, player);
            UpdateMoney(gameTime, player);
            UpdatePant(gameTime, player);
            UpdateBurgers(gameTime, player);
            UpdatePizza(gameTime, player);
            UpdateBottles(gameTime, player);
            UpdateJagerbombs(gameTime, player);
            UpdateKebabs(gameTime, player);
            UpdateToilets(gameTime, player);
            UpdateVodkas(gameTime, player);
            UpdateRedbullVodkas(gameTime, player);
            UpdateWalls(gameTime, player, angryNeighbours);
            UpdateFires(gameTime, angryNeighbours);
            UpdateDoors(gameTime, player);
            UpdateButton(gameTime, player);
            UpdateBulletNotes(gameTime, player);
            GUI.itemsLeftToCollect = ItemsLeftToCollect();
            particleEngine.Update();
        }

        private void UpdateFires(GameTime gameTime, List<AngryNeighbour> angryNeighbours)
        {
            foreach (FireOnGround fire in fires)
            {
                fire.Update(gameTime);
                if (fire.isActive == false)
                {
                    fires.Remove(fire);
                    break;
                }
            }
        }

        int burgerCost = 100,
            kebabCost = 100,
            bottleCost = 100;
        private bool firstSpawnActivated;
        private void UpdateBars(GameTime gameTime, Player player)
        {
            foreach (Bar bar in bars)
            {
                if (bar.DetectPixelCollision(player))
                {
                    if (!bar.hasBought)
                    {
                        if (KeyMouseReader.KeyPressed(Keys.Q))
                        {
                            if (Player.score >= burgerCost)
                            {
                                for (int i = 0; i < 10; i++)
                                {
                                    player.PickUpAmmo(Player.weaponType.burger);
                                }
                                Player.score -= burgerCost;
                                Player.targetScore -= burgerCost;
                                bar.choseBurgers = true;
                                bar.hasBought = true;
                            }
                        }
                        if (KeyMouseReader.KeyPressed(Keys.W))
                        {
                            if (Player.score >= kebabCost)
                            {
                                for (int i = 0; i < 10; i++)
                                {
                                    player.PickUpAmmo(Player.weaponType.kebab);
                                }
                                Player.score -= kebabCost;
                                Player.targetScore -= kebabCost;
                                bar.choseKebab = true;
                                bar.hasBought = true;
                            }
                        }
                        if (KeyMouseReader.KeyPressed(Keys.E))
                        {
                            if (Player.score >= bottleCost)
                            {
                                for (int i = 0; i < 10; i++)
                                {
                                    player.PickUpAmmo(Player.weaponType.bottle);
                                }
                                Player.score -= bottleCost;
                                Player.targetScore -= bottleCost;
                                bar.choseBottles = true;
                                bar.hasBought = true;
                            }
                        }
                        break;
                    }
                }
            }
        }
        private void UpdateToilets(GameTime gameTime, Player player)
        {
            foreach (Toilet toilet in toilets)
            {
                toilet.Update(player);
                if (!toilet.isActivated && toilet.DetectPixelCollision(player))
                {
                    player.SetSpawnPosition(toilet.pos);
                    toilet.tex = Textures.toilet_open;
                    toilet.isActivated = true;
                    if (firstSpawnActivated)
                    {
                        Sound.activatingToilet.Play();
                    }
                    firstSpawnActivated = true;
                    foreach (Toilet t in toilets)
                    {
                        t.isCurrentSpawn = false; //Tar bort currentSpawn från alla andra toaletter så att bara en är det åt gången
                    }
                    toilet.isCurrentSpawn = true;
                    break;
                }
            }
        }
        private void UpdateWalls(GameTime gameTime, Player player, List<AngryNeighbour> angryNeighbours)
        {
            foreach (Wall wall in walls)
            {
                if (wall.DetectPixelCollision(player))
                {
                    player.MovePlayerBack(wall.pos, wall.srcRect.Width);
                    
                    break;
                }
                foreach (Bullet bullet in BulletManager.bullets)
                {
                    if (wall.DetectPixelCollision(bullet))
                    {
                        GenerateParticleEngine(bullet);
                        bullet.isActive = false;
                    }
                }
                foreach (AngryNeighbour an in angryNeighbours)
                {
                    if (wall.BoundingBox.Intersects(an.BoundingBox))
                    {
                        an.ChangeDirection();
                    }
                }
            }
        }
        private void UpdateDoors(GameTime gameTime, Player player)
        {
            foreach (Door door in doors)
            {
                if (door.isActivated == false && door.DetectPixelCollision(player))
                {
                    player.MovePlayerBack(door.pos, door.srcRect.Width);
                    break;
                }
                foreach (Bullet bullet in BulletManager.bullets)
                {
                    if (door.DetectPixelCollision(bullet) && door.isActivated == false)
                    {
                        GenerateParticleEngine(bullet);
                        bullet.isActive = false;
                    }
                }

            }
        }

        private void UpdateButton(GameTime gameTime, Player player)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons.ElementAt(i).DetectPixelCollision(player) && KeyMouseReader.KeyPressed(Keys.Down))
                {
                    buttons[i].activate();
                    if (doors.ElementAt(i) != null)
                    {
                        doors.ElementAt(i).activate();
                        doors.ElementAt(i).isActivated = true;                        
                    }
                    break;
                }
            }
        }

        private void UpdateBulletNotes(GameTime gameTime, Player player)
        {
            foreach (BulletNote bulletNote in bulletNotes)
            {
                bulletNote.Update(gameTime);
                if (bulletNote.DetectPixelCollision(player))
                {
                    bulletNotes.Remove(bulletNote);
                    player.LoseHealth(Constants.damage_flashlight, bulletNote.pos, bulletNote.srcRect.Width);
                    break;
                }

                if (!bulletNote.isActive)
                {
                    bulletNotes.Remove(bulletNote);
                    break;
                }
            }
        }

        private void UpdateJagerbombs(GameTime gameTime, Player player)
        {
            foreach (Jagerbomb jagerbomb in jagerbombs)
            {
                jagerbomb.Update(gameTime);
                if (jagerbomb.DetectPixelCollision(player))
                {
                    Sound.pickUp.Play();
                    jagerbombs.Remove(jagerbomb);
                    player.AddHealth(Constants.health_jagerbomb);
                    break;
                }
            }
        }
        private void UpdateBottles(GameTime gameTime, Player player)
        {
            foreach (Bottle bottle in bottles)
            {
                if (bottle.DetectPixelCollision(player))
                {
                    Sound.pickUp.Play();
                    bottles.Remove(bottle);
                    player.PickUpAmmo(Player.weaponType.bottle);
                    break;
                }
            }
        }
        private void UpdateKebabs(GameTime gameTime, Player player)
        {
            foreach (Kebab kebab in kebabs)
            {
                if (kebab.DetectPixelCollision(player))
                {
                    Sound.pickUp.Play();
                    kebabs.Remove(kebab);
                    player.PickUpAmmo(Player.weaponType.kebab);
                    break;
                }
            }
        }
        private void UpdatePizza(GameTime gameTime, Player player)
        {
            foreach (Pizza pizza in pizzas)
            {
                if (pizza.DetectPixelCollision(player))
                {
                    Sound.pickUp.Play();
                    pizzas.Remove(pizza);
                    player.PickUpAmmo(Player.weaponType.pizza);
                    break;
                }

            }
        }
        private void UpdateBurgers(GameTime gameTime, Player player)
        {
            foreach (Burger burger in burgers)
            {
                if (burger.DetectPixelCollision(player))
                {
                    Sound.pickUp.Play();
                    burgers.Remove(burger);
                    player.PickUpAmmo(Player.weaponType.burger);
                    break;
                }
            }
        }
        private void UpdatePant(GameTime gameTime, Player player)
        {
            foreach (Pant pant in pants)
            {
                if (pant.DetectPixelCollision(player))
                {
                    Sound.pickUp.Play();
                    pant.PickUp();
                }
                pant.Update(gameTime);
                if (pant.isActive == false)
                {
                    player.AddScore(Constants.score_pant);
                    pants.Remove(pant);
                    break;
                }
            }
        }
        private void UpdateTeleport(GameTime gameTime, Player player)
        {
            foreach (Teleport teleport in teleports)
            {
                if (ItemsLeftToCollect() == 0)
                {
                    teleport.activate();
                }
                if (teleport.isActivated && teleport.DetectPixelCollision(player))
                {
                    Player.score += (int)Level.timeLeft;
                    Player.targetScore += (int)Level.timeLeft;
                    Game1.currentGameState = Game1.gameState.levelComplete;
                }
                teleport.Update(gameTime);
            }
        }
        private void UpdatePainkillers(GameTime gameTime, Player player)
        {
            foreach (Painkiller painkiller in painkillers)
            {
                if (painkiller.DetectPixelCollision(player))
                {
                    Sound.pickUp.Play();
                    painkiller.PickUp();
                }
                painkiller.Update(gameTime);
                if (painkiller.isActive == false)
                {
                    player.AddHealth(Constants.health_painkiller);
                    painkillers.Remove(painkiller);
                    break;
                }
            }
        }
        private void UpdateMoney(GameTime gameTime, Player player)
        {
            foreach (Money money in moneys)
            {
                if (money.DetectPixelCollision(player))
                {
                    Sound.pickUp.Play();
                    money.PickUp();
                }
                money.Update(gameTime);
                if (money.isActive == false)
                {
                    player.AddScore(Constants.score_money);
                    moneys.Remove(money);
                    break;
                }

            }
        }
        private void UpdateKeys(GameTime gameTime, Player player)
        {
            foreach (Key key in keys)
            {
                if (key.DetectPixelCollision(player))
                {
                    keys.Remove(key);
                    break;
                }
                key.Update(gameTime);
            }
        }
        private void UpdateWallets(GameTime gameTime, Player player)
        {
            foreach (Wallet wallet in wallets)
            {
                if (wallet.DetectPixelCollision(player))
                {
                    Sound.pickUp.Play();
                    wallets.Remove(wallet);
                    break;
                }
                wallet.Update(gameTime);
            }
        }
        private void UpdateCellphones(GameTime gameTime, Player player)
        {
            foreach (Cellphone cellphone in cellphones)
            {
                if (cellphone.DetectPixelCollision(player))
                {
                    Sound.pickUp.Play();
                    cellphones.Remove(cellphone);
                    break;
                }
                cellphone.Update(gameTime);
            }
        }
        private void UpdateTorches(GameTime gameTime, Player player)
        {
            foreach (Torch torch in torches)
            {
                if (Player.currentWeapon == Player.weaponType.bottle && player.DetectPixelCollision(torch))
                {
                    player.SwitchWeaponsAndTexture(Player.weaponType.molotovCocktail);
                }
                torch.Update(gameTime);
            }
        }
        private void UpdateHearts(GameTime gameTime, Player player)
        {
            foreach (Heart heart in hearts)
            {
                if (heart.DetectPixelCollision(player))
                {
                    Sound.pickUp.Play();
                    heart.PickUp();
                }
                heart.Update(gameTime);
                if (heart.isActive == false)
                {
                    player.AddALife();
                    hearts.Remove(heart);
                    break;
                }
            }
        }
        private void UpdateVodkas(GameTime gameTime, Player player)
        {
            foreach (Vodka vodka in vodkas)
            {
                if (vodka.DetectPixelCollision(player))
                {
                    Sound.pickUp.Play();
                    vodkas.Remove(vodka);
                    player.ActivatePowerUp(1, Constants.powerUpTimeVodka);
                    break;
                }
            }
        }
        private void UpdateRedbullVodkas(GameTime gameTime, Player player)
        {
            foreach (RedbullVodka redbullVodka in redbullVodkas)
            {
                if (redbullVodka.DetectPixelCollision(player))
                {
                    Sound.pickUp.Play();
                    redbullVodkas.Remove(redbullVodka);
                    player.ActivatePowerUp(2, Constants.powerUpTimeRedbullVodka);
                    break;
                }
            }
        }
        public int ItemsLeftToCollect()
        {
            return keys.Count() + wallets.Count() + cellphones.Count();
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bar bar in bars)
            {
                bar.Draw(spriteBatch);
            }
            foreach (Platform platform in platforms)
            {
                platform.Draw(spriteBatch);
            }
            foreach (Torch torch in torches)
            {
                torch.Draw(spriteBatch);
            }
            foreach (Key key in keys)
            {
                key.Draw(spriteBatch);
            }
            foreach (Wallet wallet in wallets)
            {
                wallet.Draw(spriteBatch);
            }
            foreach (BulletNote bulletNote in bulletNotes)
            {
                bulletNote.Draw(spriteBatch);
            }
            foreach (Cellphone cellphone in cellphones)
            {
                cellphone.Draw(spriteBatch);
            }
            foreach (Heart heart in hearts)
            {
                heart.Draw(spriteBatch);
            }
            foreach (Painkiller painkiller in painkillers)
            {
                painkiller.Draw(spriteBatch);
            }
            foreach (Teleport teleport in teleports)
            {
                teleport.Draw(spriteBatch);
            }
            foreach (Money money in moneys)
            {
                money.Draw(spriteBatch);
            }
            foreach (Pant pant in pants)
            {
                pant.Draw(spriteBatch);
            }
            foreach (Burger burger in burgers)
            {
                burger.Draw(spriteBatch);
            }
            foreach (Pizza pizza in pizzas)
            {
                pizza.Draw(spriteBatch);
            }
            foreach (Bottle bottle in bottles)
            {
                bottle.Draw(spriteBatch);
            }
            foreach (Jagerbomb jagerbomb in jagerbombs)
            {
                jagerbomb.Draw(spriteBatch);
            }
            foreach (Kebab kebab in kebabs)
            {
                kebab.Draw(spriteBatch);
            }
            foreach (Toilet toilet in toilets)
            {
                toilet.Draw(spriteBatch);
            }
            foreach (Vodka vodka in vodkas)
            {
                vodka.Draw(spriteBatch);
            }
            foreach (RedbullVodka redbullVodka in redbullVodkas)
            {
                redbullVodka.Draw(spriteBatch);
            }
            foreach (FireOnGround fire in fires)
            {
                fire.Draw(spriteBatch);
            }
            foreach (Wall wall in walls)
            {
                wall.Draw(spriteBatch);
            }
            foreach (Door door in doors)
            {
                door.Draw(spriteBatch);
            }
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }
            particleEngine.Draw(spriteBatch);
        }
        private void UpdatePlatforms(GameTime gameTime, Player player, List<AngryNeighbour> angryNeighbours)
        {
            foreach (Platform platform in platforms)
            {
                platform.Update(gameTime, player);

                if (player.movement.Y > 0) //Going down
                {
                    if (player.BottomBoundingBox.Intersects(platform.TopBoundingBox))
                    {
                        player.activePlatform = platform; //Sets the activate platform
                        player.pos.Y = platform.BoundingBox.Top - player.BoundingBox.Height + 1; //+1 to maintain the Intersection
                        player.movement.Y = 0;
                    }
                }
                //Försöker få det att se ut som att partiklarna från jet-motorn när player flyger tar emot plattformen och sprider sig längs med. 
                //Blev inte riktigt bra, antar att det är för att de är mindre än 1 pixel stora.
                foreach (Particle p in player.particleEngine.particles)
                {
                    if (p.BoundingBox.Intersects(platform.BoundingBox))
                    {
                        p.Velocity.Y = 0;
                        p.Velocity.X += rnd.Next(-3, 3);
                    }
                }
                foreach (Bullet b in BulletManager.bullets)
                {
                    if (b.BoundingBox.Intersects(platform.TopBoundingBox))
                    {
                        if (b is MolotovWeapon) 
                        {
                            platform.StartFire();
                            b.isActive = false;
                        }
                        else
                        {
                            b.isActive = false;
                        }
                    } 
                }
                foreach (AngryNeighbour an in angryNeighbours)
                {
                    if (an.movement.Y > 0)
                    {
                        if (an.BottomBoundingBox.Intersects(platform.TopBoundingBox))
                        {
                            an.activePlatform = platform; //Sets the activate platform
                            an.pos.Y = platform.BoundingBox.Top - an.BoundingBox.Height + 1; //+1 to maintain the Intersection
                            an.movement.Y = 0;
                            if (platform.fire != null && platform.fire.isActive == true)
                            {
                                an.LoseHealth(0.01f);
                            }
                        }
                    }
                }
            }
        }
        private void GenerateParticleEngine(Bullet bullet)
        {
            if (bullet is PizzaWeapon)
            {
                particleEngine.Textures = Textures.pizzaParticles;
                particleEngine.CreateParticlesInCircleRange(bullet.pos);
            }
            if (bullet is HamburgareVapen)
            {
                particleEngine.Textures = Textures.burgerParticles;
                particleEngine.CreateParticlesInCircleRange(bullet.pos);
            }
            if (bullet is BottleWeapon)
            {
                Sound.bottleCrash.Play();
                particleEngine.Textures = Textures.bottleparticles;
                particleEngine.CreateParticlesInCircleRange(bullet.pos);
            }
            if (bullet is MolotovWeapon)
            {
                Sound.bottleCrash.Play();
                particleEngine.Textures = Textures.bottleparticles;
                particleEngine.CreateParticlesInCircleRange(bullet.pos);
            }
            if (bullet is KebabWeapon)
            {
                particleEngine.Textures = Textures.kebabParticles;
                particleEngine.CreateParticlesInCircleRange(bullet.pos);
            }
        }
    }
}
