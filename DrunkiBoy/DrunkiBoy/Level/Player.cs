using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrunkiBoy
{
    class Player : AnimatedObject
    {
        private const int playerSpeed = 80;
        public static int livesLeft;
        private int defaultLives = 3;
        public static int healthLeft, defaultHealth = 200;
        public static int score = 132432;

        public static int activePowerUp; //Tänker mig numrerade powerups, typ 1: odödlig, 2: flygförmåga, 3: nånting och så "0" för ingenting
        private double activePowerUpTimer;
        public enum weaponType { none, burger, pizza, bottle, molotovCocktail };
        private weaponType currentWeapon;

        public int jumpHeight = 12;
        //public Vector2 currentSpawnPos;
        private bool playerDead;
        //private double spawnTimer, spawnTimerDefault = 750;
        //private bool movingLeft;

        public Player(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            livesLeft = 2;
            healthLeft = 60;
            this.type = "player";
            //ResetSpawnTimer();
        }
        public override void Update(GameTime gameTime)
        {
            switch (activePowerUp)
            {
                case 0: //Vanlig
                    PlayerMovement(gameTime);
                    AddFriction(facing);

                    PlayerJumping();
                    Shoot();
                    CheckIfPlayerIsOnPlatform();
                    AnimateWhenInAir(gameTime);
                break;
                case 1: //Odödlig
                    PlayerMovement(gameTime);
                    AddFriction(facing);

                    PlayerJumping();
                    Shoot();
                    CheckIfPlayerIsOnPlatform();
                    AnimateWhenInAir(gameTime);
                    activePowerUpTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                break;

                case 2: //Flygförmåga
                    activePowerUpTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                break;
            }
            if (activePowerUpTimer <= 0) //Avaktiverar poweruppen när tiden gått ut
            {
                activePowerUp = 0;
            }
            base.Update(gameTime);
        }

        private void PlayerMovement(GameTime gameTime)
        {
            if (KeyMouseReader.keyState.IsKeyDown(Keys.Left) && pos.X > -(Game1.windowWidth / 2) && !playerDead)
            {
                timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;
                movement.X -= 1;
                facing = 0;
            }
            if (KeyMouseReader.keyState.IsKeyDown(Keys.Right) && !playerDead)
            {
                timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;
                movement.X += 1;
                facing = 1;
            }
            pos += movement * (float)gameTime.ElapsedGameTime.TotalSeconds * playerSpeed;
            //pos.X = MathHelper.Clamp(pos.X, -(Game1.windowWidth / 2), Level.currentLevel.levelLength - srcRect.Width);
        }
        private void AddFriction(int facing)
        {
            movement.X -= movement.X * 0.2f;
        }

        private void PlayerJumping()
        {
            if (KeyMouseReader.KeyPressed(Keys.Up) && activePlatform != null)
            {
                movement.Y += -jumpHeight;
                activePlatform = null;
            }
        }
        /// <summary>
        /// När livet tar slut eller om man ramlar av plattform
        /// </summary>
        public void SetPlayerDead()
        {
            playerDead = true;
            movement = Vector2.Zero;
            if (livesLeft > 1)
            {
                livesLeft--;
                //Metod här sen som spawnar vid senaste checkpoint
            }
            else
            {
                //Game1.currentGameState = Game1.gameState.GameOver;
            }
        }
        /// <summary>
        /// Körs hela tiden för att kolla om spelaren fortfarande är på en plattform, annars sätts activePlatform till null. 
        /// </summary>
        private void CheckIfPlayerIsOnPlatform()
        {
            if (activePlatform != null)
            {
                if (!(BottomBoundingBox.Intersects(activePlatform.TopBoundingBox)))
                {
                    activePlatform = null;
                }
            }
        }

        /// <summary>
        /// Så att player fortsätter animera när man hoppar
        /// </summary>
        /// <param name="gameTime"></param>
        private void AnimateWhenInAir(GameTime gameTime)
        {
            if (activePlatform == null)
            {
                timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }
        /// <summary>
        /// Körs när man tar ett extraliv
        /// </summary>
        public void AddLife()
        {
            if (livesLeft < defaultLives)
            {
                livesLeft++;
            }
        }
        /// <summary>
        /// Körs när man tar en PowerUp. Switch/case satsen i Update() avgör vad som händer med player när poweruppen är aktiv
        /// </summary>
        /// <param name="powerUp"></param>
        public void ActivatePowerUp(int powerUp) //Skickar nog in ett powerUp-objekt här sen istället. Tänker att tiden poweruppen ska vara aktiv finns i varje powerup-objekt
        {
            activePowerUp = powerUp;
            //activePowerUpTimer = powerUp.timer; //Nåt i den här stilen sen
            Game1.gui.ShowPowerUpCounter(powerUp);
        }
        /// <summary>
        /// Körs när man tar ett föremål som ger hälsa
        /// </summary>
        /// <param name="amountToAdd"></param>
        public void AddHealth(int amountToAdd)
        {
            if (healthLeft + amountToAdd < defaultHealth) //Kollar så att man inte får mer health än max, vilket är defaultHealth
            {
                for (int i = 0; i < amountToAdd; i++)
                {
                    healthLeft += 1; //Tänker mig nån delay här så att healthbar ökar lite snyggt
                }
            }
            else
            {
                healthLeft = defaultHealth;
            }
        }
        /// <summary>
        /// Körs när man springer in i något som ger en skada
        /// </summary>
        /// <param name="amountToLose"></param>
        public void LoseHealth(int amountToLose)
        {
            if (healthLeft - amountToLose > 0) 
            {
                for (int i = 0; i < amountToLose; i++)
                {
                    healthLeft -= 1; //Tänker mig nån delay här så att healthbar minskar lite snyggt
                }
            }
            else //Då är man död...
            {
                SetPlayerDead();
            }
        }
        /// <summary>
        /// Körs när man springer på något som ger poäng
        /// </summary>
        /// <param name="scoreToAdd">Hur mycket poäng att lägga till</param>
        public void AddScore(int scoreToAdd)
        {
            score += scoreToAdd;
        }

        public void PickUpWeapon(weaponType type)
        {
            switch (type)
            {
                case weaponType.none:
                    tex = Textures.player;
                    currentWeapon = weaponType.none;
                    break;
                case weaponType.burger:
                    tex = Textures.player_burger;
                    currentWeapon = weaponType.burger;
                    break;
                case weaponType.pizza:
                    //tex = Textures.player_pizza;
                    currentWeapon = weaponType.pizza;
                    break;
                case weaponType.bottle:
                    //tex = Textures.player_bottle;
                    currentWeapon = weaponType.bottle;
                    break;
                case weaponType.molotovCocktail:
                    //tex = Textures.player_molotovCocktail;
                    currentWeapon = weaponType.molotovCocktail;
                    break;
            }
        }
        public void Shoot()
        {
            if (KeyMouseReader.KeyPressed(Keys.Space))
            {
                Vector2 bulletPos, bulletVelocity;
                if (facing == 0)  // vänster hållet att skjuta
                {
                    bulletPos = new Vector2(pos.X, pos.Y + 60);
                    bulletVelocity = new Vector2(-2, 0);
                }
                else //Högeråt
                {
                    bulletPos = new Vector2(pos.X + 60, pos.Y + 60);
                    bulletVelocity = new Vector2(2, 0);
                }
                switch (currentWeapon)
                {
                    case weaponType.burger:
                        BulletManager.AddBullet(new HamburgareVapen(bulletPos, bulletVelocity));
                    break;
                    case weaponType.pizza:
                    BulletManager.AddBullet(new PizzaWeapon(bulletPos, bulletVelocity));

                    break;
                    case weaponType.bottle:
                    BulletManager.AddBullet(new BottleWeapon(bulletPos, bulletVelocity));

                    break;
                    case weaponType.molotovCocktail:

                    break;
                }  
            }
        }                
    }
}