﻿using System;
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
        private ParticleEngine2 particleEngine = new ParticleEngine2(Textures.smokeParticles, Vector2.Zero, Textures.explosionTexture, false);

        private Texture2D texUpperBody, texLowerBody, prevTexUpperBody;
        private Vector2 targetPos;
        private bool movingBack;

        //LB = Lower Body. För att kunna animera benen för sig så att player inte springer på stället när man kör skjutanimationen
        double timeTilNextFrameLB = 0; 
        private int frameLB;
        private Rectangle srcRectLB;

        private bool animateShooting;
        private bool shootingLeft;

        private const int playerSpeed = 80;
        public static int livesLeft;        
        private int defaultLives = 3;
        public static int healthLeft, defaultHealth = 200;
        private int targetHealth;
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

        public Player(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            targetPos = pos;
            currentSpawnPos = pos;
            srcRectLB = srcRect;
            livesLeft = 2;
            ResetHealth();
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
            AnimateHealthBar();
            MoveBackWhenEnemyContact(gameTime);
        }
        /// <summary>
        /// Räknar ner timeTilNextFrameLB och timeTilNextFrame när man styr player
        /// </summary>
        /// <param name="gameTime"></param>
        private void PlayerMovement(GameTime gameTime)
        {
            if (KeyMouseReader.keyState.IsKeyDown(Keys.Left) && !isDead && !movingBack)
            {
                timeTilNextFrameLB -= gameTime.ElapsedGameTime.TotalMilliseconds;
                timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;

                movement.X -= 1;
                facing = 0;
                ForceFrameChange();
            }
            else if (KeyMouseReader.keyState.IsKeyDown(Keys.Right) && !isDead && !movingBack)
            {
                timeTilNextFrameLB -= gameTime.ElapsedGameTime.TotalMilliseconds;
                timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;

                movement.X += 1;
                facing = 1;
                ForceFrameChange();
            }
            pos += movement * (float)gameTime.ElapsedGameTime.TotalSeconds * playerSpeed;

            //pos.X = MathHelper.Clamp(pos.X, -(Game1.windowWidth / 2), Level.currentLevel.levelLength - srcRect.Width);
        }
        /// <summary>
        /// Ser till att player vänder sig så fort man trycker vänster eller höger piltangent även om timeTilNextFrame inte hunnit bli noll.
        /// Undviker "skridskoeffekten" som fanns innnan.
        /// </summary>
        private void ForceFrameChange()
        {
            srcRectLB.X = facingSrcRects[facing].X + (frameLB % nrFrames) * frameWidth;
            srcRect.X = facingSrcRects[facing].X + (frame % nrFrames) * frameWidth;
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
        /// Skiljer på under- och överkropp. Underkroppen ska bara animeras när man styr player. Överkroppen animeras även då man skjuter.
        /// Den andra animeringsmetoden ligger i AnimatedObject-klassen
        /// </summary>
        private void AnimateLowerBody()
        {
            if (timeTilNextFrameLB <= 0)
            {
                timeTilNextFrameLB = frameInterval;
                frameLB++;
                srcRectLB.X = facingSrcRects[facing].X + (frameLB % nrFrames) * frameWidth;
            }
        }
        /// <summary>
        /// Räknar ner timeTilNextFrame när animateShooting == true så att överkroppen animeras då
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
                if ((shootingLeft && facing == 1) || (!shootingLeft && facing == 0)) //Slutar animera skottanimationen om player vänder sig om
                {
                    animateShooting = false;
                }
            }
        }
        private void SetDeadFallingOffPlatform()
        {
            if (pos.Y > 2000) //Bättre sätt?
            {
                LooseALife();
            }
        }
        /// <summary>
        /// Körs när man tar ett extraliv
        /// </summary>
        public void AddALife()
        {
            if (livesLeft < defaultLives)
            {
                livesLeft++;
            }
        } 
        /// <summary>
        /// När livet tar slut eller om man ramlar av plattform
        /// </summary>
        public void LooseALife()
        {
            movement = Vector2.Zero;
            if (livesLeft > 0)
            {
                isDead = true; //Level ändrar automatiskt levelState i Level till lostLife när player.isDead == true
                livesLeft--;
                ResetHealth();
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
        /// <param name="amountToAdd">Hur mycket hälsa man vill öka med</param>
        public void AddHealth(int amountToAdd)
        {
            if (healthLeft + amountToAdd < defaultHealth) //Kollar så att man inte får mer health än max, vilket är defaultHealth
            {
                targetHealth = healthLeft + amountToAdd;
                GUI.healthBarBlinking = true;
            }
            else
            {
                targetHealth = defaultHealth;
                GUI.healthBarBlinking = true;
            }
        }
        /// <summary>
        /// Körs när man springer in i något som ger en skada
        /// </summary>
        /// <param name="amountToLose">Hur mycket skada man vill att spelaren ska ta</param>
        public void LoseHealth(int amountToLose, Vector2 enemyPos)
        {
            if (healthLeft - amountToLose > 0) 
            {
                targetHealth = healthLeft - amountToLose;
                GUI.healthBarBlinking = true;
                MovePlayerBack(enemyPos);
            }
            else //Då är man död...
            {
                healthLeft = 0;
                LooseALife();
            }
        }
        /// <summary>
        /// Flyttar players pos gradvis bakåt vid kontakt med Enemy
        /// </summary>
        /// <param name="gameTime"></param>
        private void MoveBackWhenEnemyContact(GameTime gameTime)
        {
            if (movingBack) 
            {
                if (pos.X > targetPos.X)
                {
                    pos.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 350;
                    particleEngine.Update(new Vector2(pos.X+srcRect.Width, pos.Y + srcRect.Height / 2));
                }
                if (pos.X < targetPos.X)
                {
                    pos.X += (float)gameTime.ElapsedGameTime.TotalSeconds * 350;
                    particleEngine.Update(new Vector2(pos.X, pos.Y + srcRect.Height / 2));
                }
                if (Math.Abs(targetPos.X - pos.X) < 10)
                {
                    movingBack = false;
                    particleEngine.isActive = false;
                }
            }
        }
        private void MovePlayerBack(Vector2 enemyPos)
        {
            movingBack = true;
            //particleEngine = new ParticleEngine2(Textures.smokeParticles, pos, Textures.flashlight, true);
            if (pos.X < enemyPos.X)
            {
                targetPos.X = enemyPos.X - 150;
            }
            else
            {
                targetPos.X = enemyPos.X + 150;
            }
        }
        /// <summary>
        /// Ökar eller minskar hälsan i GUI gradvis när targetHealth sätts till mer eller mindre än aktuell health (healthLeft)
        /// </summary>
        private void AnimateHealthBar()
        {
            if (healthLeft != targetHealth)
            {
                if (healthLeft < targetHealth)
                {
                    healthLeft++;
                }
                else
                {
                    healthLeft--;
                }
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
        public void Shooting()
        {
            if (KeyMouseReader.KeyPressed(Keys.Space))
            {
                Vector2 bulletPos, bulletVelocity;
                if (facing == 0)  // Skjuter vänster
                {
                    shootingLeft = true;
                    bulletPos = new Vector2(pos.X, pos.Y + 60);
                    bulletVelocity = new Vector2(-10, 0);
                    frame = 8;
                }
                else //Skjuter höger
                {
                    shootingLeft = false;
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
        /// Återställer hälsan till defaultHealth
        /// </summary>
        public void ResetHealth()
        {
            healthLeft = targetHealth = defaultHealth;
        }
        /// <summary>
        /// Två Draw() här, en för underkroppen och en för överkroppen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!animateShooting)//Borde kunna få in den här någon annanstans men kommer inte på nåt bra nu. srcRect är samma om man inte skjuter
                srcRectLB = srcRect;
            spriteBatch.Draw(texLowerBody, pos, srcRectLB, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(texUpperBody, pos, srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
            if (particleEngine.isActive)
                particleEngine.Draw(spriteBatch);
        }   
    }
}