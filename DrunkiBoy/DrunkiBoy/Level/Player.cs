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
        private Texture2D texUpperBody, texLowerBody, prevTexUpperBody;

        //LB = Lower Body. För att kunna animera benen för sig så att player inte springer på stället när man kör skjutanimationen
        double timeTilNextFrameLB = 0; 
        private int frameLB;
        private Rectangle srcRectLB;
        bool animateShooting;

        private const int playerSpeed = 80;
        public static int livesLeft;        
        private int defaultLives = 3;
        public static int healthLeft, defaultHealth = 200;
        public static int score = 0;
        public static int itemleft = 3;

        public static int activePowerUp; //Tänker mig numrerade powerups, typ 1: odödlig, 2: flygförmåga, 3: nånting och så "0" för ingenting
        private double activePowerUpTimer;
        public enum weaponType { none, burger, pizza, bottle, molotovCocktail };
        public weaponType currentWeapon;

        public int jumpHeight = 12;
        public Vector2 currentSpawnPos;
        public bool isDead;
        //private double spawnTimer, spawnTimerDefault = 750;
        //private bool movingLeft;

        public Player(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            currentSpawnPos = pos;
            srcRectLB = srcRect;
            livesLeft = 2;
            healthLeft = 60;
            this.type = "player";
            texUpperBody = Textures.player_upper_body;
            texLowerBody = Textures.player_lower_body;
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
                    Shooting();
                    CheckIfPlayerIsOnPlatform();
                    AnimateWhenInAir(gameTime);

                    SetDeadFallingOffPlatform();
                break;
                case 1: //Odödlig
                    PlayerMovement(gameTime);
                    AddFriction(facing);

                    PlayerJumping();
                    Shooting();
                    CheckIfPlayerIsOnPlatform();
                    AnimateWhenInAir(gameTime);
                    SetDeadFallingOffPlatform();
                    activePowerUpTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                break;

                case 2: //Flygförmåga
                    Shooting();
                    SetDeadFallingOffPlatform();
                    activePowerUpTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                break;
            }
            if (activePowerUpTimer <= 0) //Avaktiverar poweruppen när tiden gått ut
            {
                activePowerUp = 0;
            }
            base.Update(gameTime);
            AnimateLowerBody();
            AnimateShooting(gameTime);
        }

        private void SetDeadFallingOffPlatform()
        {
            if (pos.Y > 2000)
            {
                SetPlayerDead();
            }
        }

        private void AnimateLowerBody()
        {
            if (timeTilNextFrameLB <= 0)
            {
                timeTilNextFrameLB = frameInterval;
                frameLB++;
                srcRectLB.X = facingSrcRects[facing].X + (frameLB % nrFrames) * frameWidth;
            }
        }
        

        private void PlayerMovement(GameTime gameTime)
        {
            if (KeyMouseReader.keyState.IsKeyDown(Keys.Left) && !isDead)
            {
                timeTilNextFrameLB -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (!animateShooting)
                {
                    timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                movement.X -= 1;
                facing = 0;
            }
            else if (KeyMouseReader.keyState.IsKeyDown(Keys.Right) && !isDead)
            {
                timeTilNextFrameLB -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (!animateShooting)
                {
                    timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }
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
            movement = Vector2.Zero;
            if (livesLeft > 0)
            {
                livesLeft--;
            }
            isDead = true; //Level ändrar automatiskt levelState till lostLife när player.isDead = true
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
                timeTilNextFrameLB -= gameTime.ElapsedGameTime.TotalMilliseconds;
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
        /// <param name="amountToAdd">Hur mycket hälsa man öka med </param>
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
        /// <param name="amountToLose">Hur mycket skada man vill att spelaren ska ta</param>
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
        /// <summary>
        /// Körs från ItemManager när player tar upp ett vapen. Ändrar players textur och currentWeapon-variabel så att rätt skott skjuts
        /// </summary>
        /// <param name="type">Vapentyp</param>
        public void PickUpWeapon(weaponType type)
        {
            switch (type)
            {
                case weaponType.none:
                    texUpperBody = Textures.player_upper_body;
                    currentWeapon = weaponType.none;
                    break;
                case weaponType.burger:
                    texUpperBody = prevTexUpperBody = Textures.player_burger;
                    currentWeapon = weaponType.burger;
                    break;
                case weaponType.pizza:
                    texUpperBody = prevTexUpperBody = Textures.player_pizza;
                    currentWeapon = weaponType.pizza;
                    break;
                case weaponType.bottle:
                    texUpperBody = prevTexUpperBody = Textures.player_bottle;
                    currentWeapon = weaponType.bottle;
                    break;
                case weaponType.molotovCocktail:
                    texUpperBody = prevTexUpperBody = Textures.player_bottle_molotov;
                    currentWeapon = weaponType.molotovCocktail;
                    break;
            }
        }
        /// <summary>
        /// Så att en skottanimation körs när man skjuter. Körs när animateShooting-variabeln sätts till true och avslutas när alla 8 frames körts. Då
        /// byter player till föregående textur igen.
        /// </summary>
        /// <param name="gameTime"></param>
        protected void AnimateShooting(GameTime gameTime)
        {
            if (animateShooting)
            {
                timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (facing == 1 && frame == 7)
                {
                    animateShooting = false;
                    texUpperBody = prevTexUpperBody; //Byter tillbaka till föregående textur så att skottanimationen bara körs en gång per skott
                }
                if (facing == 0 && frame == 15)
                {
                    animateShooting = false;
                    texUpperBody = prevTexUpperBody; //Byter tillbaka till föregående textur så att skottanimationen bara körs en gång per skott
                }
            }
        }
        public void Shooting()
        {
            if (KeyMouseReader.KeyPressed(Keys.Space))
            {
                Vector2 bulletPos, bulletVelocity;
                if (facing == 0)  // vänster hållet att skjuta
                {
                    bulletPos = new Vector2(pos.X, pos.Y + 60);
                    bulletVelocity = new Vector2(-10, 0);
                    frame = 7;
                }
                else //Högeråt
                {
                    bulletPos = new Vector2(pos.X + 60, pos.Y + 60);
                    bulletVelocity = new Vector2(10, 0);
                    frame = 0;
                }
                switch (currentWeapon)
                {
                    case weaponType.burger:
                        texUpperBody = Textures.player_shooting;
                        animateShooting = true;
                        BulletManager.AddBullet(new HamburgareVapen(bulletPos, bulletVelocity));
                    break;

                    case weaponType.pizza:
                        texUpperBody = Textures.player_shooting;
                        animateShooting = true;
                        BulletManager.AddBullet(new PizzaWeapon(bulletPos, bulletVelocity));
                    break;
                    case weaponType.bottle:     
                        texUpperBody = Textures.player_shooting;
                        animateShooting = true;
                        BulletManager.AddBullet(new BottleWeapon(bulletPos, bulletVelocity));

                    break;
                    case weaponType.molotovCocktail:
                        texUpperBody = Textures.player_shooting;
                        animateShooting = true;
                        BulletManager.AddBullet(new MolotovWeapon(bulletPos, bulletVelocity));
                    break;
                }  
            }
        }
        /// <summary>
        /// Två Draw() här, en för underkroppen och en för överkroppen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texLowerBody, pos, srcRectLB, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(texUpperBody, pos, srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
        }   
    }
}