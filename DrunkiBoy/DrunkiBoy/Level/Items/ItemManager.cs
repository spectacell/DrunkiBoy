using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class ItemManager
    {
        public List<Platform> platforms = new List<Platform>();
        public List<Enemy> enemies = new List<Enemy>();
        public List<Torch> torches = new List<Torch>();
        public List<Key> keys = new List<Key>();
        public List<Heart> hearts = new List<Heart>();
        public List<Painkiller> painkillers = new List<Painkiller>();
        public List<Teleport> teleports = new List<Teleport>();
        public List<Money> moneys = new List<Money>();
        public List<Pant> pants = new List<Pant>();
        public List<Burger> burgers = new List<Burger>();
        public List<Pizza> pizzas = new List<Pizza>();
        public List<Bottle> bottles = new List<Bottle>();
        

        public ItemManager()
        {

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
        public void Update(GameTime gameTime, Player player)
        {
            UpdatePlatforms(player);
            UpdateTorches(gameTime, player);
            UpdateKeys(gameTime, player);
            UpdateHeart(gameTime, player);
            UpdatePainkiller(gameTime, player);
            UpdateTeleport(gameTime);
            UpdateMoney(gameTime, player);
            UpdatePant(gameTime, player);
            UpdateBurgers(gameTime, player);
            UpdatePizza(gameTime, player);
            UpdateBottles(gameTime, player);

            GUI.itemsLeftToCollect = ItemsLeftToCollect();
        }
        private void UpdateBottles(GameTime gameTime, Player player)
        {
            foreach (Bottle bottle in bottles)
            {
                if (bottle.DetectPixelCollision(player))
                {
                    bottles.Remove(bottle);
                    player.PickUpWeapon(Player.weaponType.bottle);
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
                    player.PickUpWeapon(Player.weaponType.pizza);
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
                    pants.Remove(pant);
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
                    moneys.Remove(money);
                    break;
                }
            }
        }

       
        private void UpdateTeleport(GameTime gameTime)
        {
            foreach (Teleport teleport in teleports)
            {
                teleport.Update(gameTime);
            }
        }

        private void UpdatePainkiller(GameTime gameTime, Player player)
        {
            foreach (Painkiller painkiller in painkillers)
            {
                if (painkiller.DetectPixelCollision(player))
                {
                    painkillers.Remove(painkiller);
                    break;
                }
                painkiller.Update(gameTime);
            }
        }
       
       
        private void UpdateHeart(GameTime gameTime, Player player)
        {
            foreach (Heart heart in hearts)
            {
                if (heart.DetectPixelCollision(player))
                {
                    hearts.Remove(heart);
                    player.AddLife();
                    break;
                }
                heart.Update(gameTime);
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

        private void UpdateTorches(GameTime gameTime, Player player)
        {
            foreach (Torch torch in torches)
            {
                if (player.currentWeapon == Player.weaponType.bottle && player.DetectPixelCollision(torch)) 
                {
                    player.PickUpWeapon(Player.weaponType.molotovCocktail);
                }
                torch.Update(gameTime);
            }
        }
        private void UpdateBurgers(GameTime gameTime, Player player)
        {
            foreach (Burger burger in burgers)
            {
                if (burger.DetectPixelCollision(player))
                {
                    burgers.Remove(burger);
                    player.PickUpWeapon(Player.weaponType.burger);
                    break;
                }
            }
        }
        public int ItemsLeftToCollect()
        {
            return keys.Count(); //Räkna alla listor med saker som måste plockas här sen...
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Platform platform in platforms)
            {
                spriteBatch.Draw(platform.tex, platform.pos, platform.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Torch torchs in torches)
            {
                spriteBatch.Draw(torchs.tex,torchs.pos, torchs.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Key key in keys)
            {
                spriteBatch.Draw(key.tex, key.pos, key.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Heart heart in hearts)
            {
                spriteBatch.Draw(heart.tex, heart.pos, heart.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Painkiller painkiller in painkillers)
            {
                spriteBatch.Draw(painkiller.tex, painkiller.pos, painkiller.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Teleport teleport in teleports)
            {
                spriteBatch.Draw(teleport.tex, teleport.pos, teleport.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Money money in moneys)
            {
                spriteBatch.Draw(money.tex, money.pos, money.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Pant pant in pants)
            {
                spriteBatch.Draw(pant.tex, pant.pos, pant.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Burger burger in burgers)
            {
                spriteBatch.Draw(burger.tex, burger.pos, burger.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Pizza pizza in pizzas)
            {
                spriteBatch.Draw(pizza.tex, pizza.pos, pizza.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Bottle bottle in bottles)
            {
                spriteBatch.Draw(bottle.tex, bottle.pos, bottle.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
        }
        private void UpdatePlatforms(Player player)
        {
            foreach (Platform platform in platforms)
            {
                if (player.movement.Y > 0) //Going down
                {
                    if (player.BottomBoundingBox.Intersects(platform.TopBoundingBox))
                    {
                        player.activePlatform = platform; //Sets the activate platform
                        player.pos.Y = platform.BoundingBox.Top - player.BoundingBox.Height + 1; //+1 to maintain the Intersection
                        player.movement.Y = 0;
                    }
                } 
                
                //foreach (Enemy enemy in enemies)
                //{
                //    if (enemy.activePlatform == null)
                //    {
                //        if (enemy.BoundingBox.Intersects(platform.BoundingBox))// && !enemy.isKilled)
                //        {
                //            enemy.activePlatform = platform; //Sets the activate platform
                //            enemy.pos.Y = platform.BoundingBox.Top - enemy.BoundingBox.Height + 1; //+1 to maintain the Intersection
                //            enemy.isOnGround = true;
                //            //enemy.hasJumped = false;
                //            enemy.movement.Y = 0;
                //        }
                //    }
                //}               
            }
        }
    }
}
