using Microsoft.Xna.Framework;
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

        Door Door;
        HamburgareVapen hamburgareVapen;
        BottleWeapon bottleWeapon;
        KebabWeapon kebabWeapon;
        PizzaWeapon pizzaWeapon;
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
        public void AddCellphone (Cellphone cellphone)
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

        #endregion
        public void Update(GameTime gameTime, Player player, List<AngryNeighbour> angryNeighbours)
        {
            UpdatePlatforms(player, angryNeighbours);
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
                foreach (AngryNeighbour an in angryNeighbours)
                {
                    if (fire.DetectPixelCollision(an))
                    {
                        an.LoseHealth(0.01f);
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
                if (door.DetectPixelCollision(player))
                {
                    player.MovePlayerBack(door.pos, door.srcRect.Width);
                    break;
                }
                foreach (Bullet bullet in BulletManager.bullets)
                {
                    if (door.DetectPixelCollision(bullet))
                    {
                        GenerateParticleEngine(bullet);
                        bullet.isActive = false;
                    }
                    if (door.isActivated)
                    {
                        Door.activate();
                    }
                }
            }
        }

        private void UpdateButton(GameTime gameTime, Player player)
        {
            foreach (Button button in buttons)
            {
                if (button.DetectPixelCollision(player) && KeyMouseReader.KeyPressed(Keys.U))
                {
                    Door.isActivated = true;
                    Door.activate();
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
                        player.AddScore((int)Level.timeLeft);
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
                    wallets.Remove(wallet);
                    break;
                }
                wallet.Update(gameTime);
            }
        }
        private void UpdateCellphones(GameTime gameTime, Player player)
        {
            foreach(Cellphone cellphone in cellphones)
            {
                if (cellphone.DetectPixelCollision(player))
                {
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
        private void UpdatePlatforms(Player player, List<AngryNeighbour> angryNeighbours)
        {
            foreach (Platform platform in platforms)
            {
                if (platform is MovingPlatform)
                {

                    ((MovingPlatform)platform).Update(player);
                }

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
                        p.Velocity.X += rnd.Next(-3,3);
                    }
                }
                foreach (Bullet b in BulletManager.bullets)
                {
                    if (b is MolotovWeapon && b.DetectPixelCollision(platform))
                    {
                        fires.Add(new FireOnGround(new Vector2(platform.pos.X+20, platform.pos.Y-Textures.fire.Height), Textures.fire, new Rectangle(0, 0, 240, 29), true, 4, 180));
                        b.isActive = false;
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
                particleEngine.Textures = Textures.bottleparticles;
                particleEngine.CreateParticlesInCircleRange(bullet.pos);
            }
            if (bullet is MolotovWeapon)
            {
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
