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
        private Vector2 targetPos;
        public bool movingBack, weaponThrown;

        //LB = Lower Body. För att kunna animera benen för sig så att player inte springer på stället när man kör skjutanimationen
        double timeTilNextFrameLB = 0; 
        private int frameLB;
        private Rectangle srcRectLB;
        private float rotation = 0;
        public bool animateShooting;
        private bool shootingLeft;
        private double shotDelay, shotDelayDefault = 300;

        public bool invincible;

        private const int playerSpeed = 80;
        public static int livesLeft;        
        private int defaultLives = 3;
        public static int healthLeft;
        private int targetHealth;
        public static int score = 0;
        private int targetScore, realScore; //realScore för att score-räknare inte hann med att räkna upp om man tog många poäng på en gång

        public static int activePowerUp; //Tänker mig numrerade powerups, typ 1: odödlig, 2: flygförmåga, 3: nånting och så "0" för ingenting
        private double activePowerUpTimer;
        private Random rnd = new Random();
        private ParticleEngine2 particleEngine;
        private Vector2 particleEngingePos;
        public enum weaponType { none, burger, pizza, kebab, bottle, molotovCocktail };
        public weaponType currentWeapon;

        public int jumpHeight = 12;
        public Vector2 currentSpawnPos;
        public bool isDead {get; private set;}
        private double spawnTimer, spawnTimerDefault = 1750;
        public bool spawning;
        private bool hasJumped;
        private Rectangle srcRectSpawnHead;
        public Player(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            score = 0;
            currentSpawnPos = pos;
            srcRectLB = srcRect;
            livesLeft = 2;
            Reset();
            this.type = "player";
            texUpperBody = Textures.player_upper_body;
            texLowerBody = Textures.player_lower_body;
            particleEngine = new ParticleEngine2(Textures.smokeParticles, Vector2.Zero, 2, 2, Textures.explosionTexture, false);
        }
        public override void Update(GameTime gameTime)
        {
            switch (activePowerUp)
            {
                case 0: //Vanlig
                    PlayerMovement(gameTime);
                    AddFriction(facing);
                    invincible = false;
                    PlayerJumping();
                    Shooting();
                    CheckIfPlayerIsOnPlatform();
                    AnimateWhenInAir(gameTime);

                    SetDeadFallingOffPlatform();
                break;
                case 1: //Odödlig
                    PlayerMovement(gameTime);
                    AddFriction(facing);
                    invincible = true;
                    PlayerJumping();
                    Shooting();
                    CheckIfPlayerIsOnPlatform();
                    AnimateWhenInAir(gameTime);
                    SetDeadFallingOffPlatform();
                break;

                case 2: //Flygförmåga
                    PlayerFlying(gameTime);
                    AddFriction(facing);
                    Shooting();
                    SetDeadFallingOffPlatform();
                    CheckIfPlayerIsOnPlatform();
                    particleEngine.Update(particleEngingePos, true);
                break;
            }
            
            base.Update(gameTime);
            AnimateLowerBody();
            AnimateShooting(gameTime);
            AnimateHealthBar();
            MoveBackWhenEnemyContact(gameTime);
            CountDownShotDelay(gameTime);
            AnimatingScore();
            SpawnAnimation(gameTime);
            PowerUpTimerAandDeactivation(gameTime);
        }
        
        /// <summary>
        /// Två Draw() här, en för underkroppen och en för överkroppen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!animateShooting)//Borde kunna få in den här någon annanstans men kommer inte på nåt bra nu. srcRect är samma om man inte skjuter
            { 
                srcRectLB = srcRect;
            }
            if (!spawning) 
            {
                if (activePowerUp == 2) //Om player har flygförmåga så ritas textur med jetpack
                {
                    spriteBatch.Draw(Textures.player_jetpack, pos, new Rectangle(0, 0, 95, 146), Color.White, rotation, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
                    particleEngine.Draw(spriteBatch);
                }
                else //Annars de vanliga texturerna för under- och överkropp
                {
                    spriteBatch.Draw(texLowerBody, pos, srcRectLB, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(texUpperBody, pos, srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
                }
            }
            else //När player spawnas så ritas bara huvududet ut
            {
                spriteBatch.Draw(Textures.player_head, new Vector2(pos.X+2, pos.Y+50), srcRectSpawnHead, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
            }
        }
        /// <summary>
        /// Flygförmåga efter intag av Redbull vodka
        /// </summary>
        private void PlayerFlying(GameTime gameTime)
        {
            facing = 1;
            if (activePlatform != null && activePowerUpTimer >= 0) //Simulera jetpack motor
            {
                movement.Y -= (float)rnd.NextDouble()*3;
            }
            if (KeyMouseReader.keyState.IsKeyDown(Keys.Left))
            {
                particleEngingePos = new Vector2(pos.X + 19, pos.Y + 115);
                if (activePlatform != null)
                {
                    rotation = 0f;
                    timeTilNextFrameLB -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                movement.X -= 1;
                ForceFrameChange();
                if (activePlatform == null)
                {
                    rotation = -0.1f;
                }
            }
            if (KeyMouseReader.keyState.IsKeyDown(Keys.Right))
            {
                particleEngingePos = new Vector2(pos.X + 10, pos.Y + 115);
                if (activePlatform != null)
                {
                    rotation = 0f;
                    timeTilNextFrameLB -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                movement.X += 1;
                ForceFrameChange();
                if (activePlatform == null)
                {
                    rotation = +0.1f;
                }
            }
            if (activePowerUpTimer >= 0 && KeyMouseReader.keyState.IsKeyDown(Keys.Up))
            {
                movement.Y -= 0.5f;
                particleEngine.height = 10;
            }
            if (!(KeyMouseReader.keyState.IsKeyDown(Keys.Left) || KeyMouseReader.keyState.IsKeyDown(Keys.Right))) //Rätar ut player när man inte rör sig framåt eller bakåt
            {
                particleEngine.height = 2;
                particleEngingePos = new Vector2(pos.X + 13, pos.Y + 120);
                rotation = 0f;
            }
            
            AddGravity(0.2f);
            movement.Y -= movement.Y * 0.05f;
            pos += movement * (float)gameTime.ElapsedGameTime.TotalSeconds * playerSpeed;
            pos.X = MathHelper.Clamp(pos.X, 0, Level.levelWidth-srcRect.Width); //Hindrar player från att flyga utanför skärmen
        }
        /// <summary>
        /// Räknar ner timeTilNextFrameLB och timeTilNextFrame när man styr player
        /// </summary>
        private void PlayerMovement(GameTime gameTime)
        {
            if (!isDead && !movingBack && !spawning)
            { 
                if (KeyMouseReader.keyState.IsKeyDown(Keys.Left))
                {
                    timeTilNextFrameLB -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;

                    movement.X -= 1;
                    facing = 0;
                    ForceFrameChange();
                }
                else if (KeyMouseReader.keyState.IsKeyDown(Keys.Right))
                {
                    timeTilNextFrameLB -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;

                    movement.X += 1;
                    facing = 1;
                    ForceFrameChange();
                }
            }
            AddGravity(0.6f);
            pos += movement * (float)gameTime.ElapsedGameTime.TotalSeconds * playerSpeed;
            pos.X = MathHelper.Clamp(pos.X, 0, Level.levelWidth - srcRect.Width); //Hindrar player från att gå utanför skärmen
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
            if (!spawning && KeyMouseReader.KeyPressed(Keys.Up) && activePlatform != null)
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
        /// Räknar ner shotDelay variablen som styr hur snabbt player kan skjuta.
        /// </summary>
        private void CountDownShotDelay(GameTime gameTime)
        {
            if(shotDelay >= 0)
                shotDelay -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        /// <summary>
        /// Räknar ner timeTilNextFrame när animateShooting == true så att överkroppen animeras då
        /// </summary>
        /// <param name="gameTime"></param>
        private void AnimateShooting(GameTime gameTime)
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
            if (pos.Y > Level.levelHeight)
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
                ResetPowerUp();
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
        /// Animerar players huvud så att det kollar åt ena och sen andra hållet
        /// </summary>
        /// <param name="gameTime"></param>
        private void SpawnAnimation(GameTime gameTime)
        {
            if (spawnTimer > 0)
            {
                spawning = true;
                hasJumped = false;
                spawnTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;

                if (spawnTimer <= spawnTimerDefault / 2)
                {
                    srcRectSpawnHead = new Rectangle(0, 0, 75, 60);
                }
                else if (spawnTimer >= 0)
                {
                    srcRectSpawnHead = new Rectangle(75, 0, 75, 60);
                }
            }
            else
            {
                spawning = false;
                if (!hasJumped)
                {
                    movement.Y += -jumpHeight;
                    activePlatform = null;
                    hasJumped = true;
                }
            } 
        }
        /// <summary>
        /// Sätter Spawntimern till defaultvärdet så att timern börjar räkna ner
        /// </summary>
        public void ResetSpawnTimer()
        {
            spawning = true;
            spawnTimer = spawnTimerDefault;
        }
        /// <summary>
        /// Sätter players spawnposition. Körs från Toilet när player rör vid den.
        /// </summary>
        /// <param name="pos">Players position</param>
        public void SetSpawnPosition(Vector2 pos)
        {
            currentSpawnPos = new Vector2(pos.X-15, pos.Y - srcRect.Height + Textures.toilet_open.Height);
        }
        /// <summary>
        /// Körs när man tar en PowerUp. Switch/case satsen i Update() avgör vad som händer med player när poweruppen är aktiv
        /// </summary>
        /// <param name="powerUp">1: Odödlighet, 2: Flygförmåga</param>
        /// <param name="time">Tid i ms för hur länge powerup är aktiv</param>
        public void ActivatePowerUp(int powerUp, double time) 
        {
            activePowerUp = powerUp;
            activePowerUpTimer = time;
            Game1.gui.ShowPowerUpCounter(powerUp, time);
            if (powerUp == 2)
            {
                particleEngine = new ParticleEngine2(Textures.smokeParticles, pos, 2, 2, Textures.explosionTexture, true);
            }
        }
        /// <summary>
        /// Räknar ner activePowerUpTimer och avaktiverar powerup när tiden gått ut
        /// </summary>
        private void PowerUpTimerAandDeactivation(GameTime gameTime)
        {
            if (activePowerUpTimer >= 0)
            {
                activePowerUpTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else if (activePowerUpTimer <= 0) //Avaktiverar poweruppen när tiden gått ut
            {
                rotation = 0f;
                if (activePowerUp == 2 && activePlatform == null)
                {
                    AddGravity(0.2f);//Om man är uppe i luften när tiden går ut så fortsätt med 0.2 gravitation tills man är tillbaka på en plattform. Annars riskerar man att falla för snabbt och rakt igenom en plattform
                }
                if (activePowerUp == 2 && activePlatform != null)
                {
                    activePowerUp = 0; //Avaktiverar flygförmågan när man är på fast mark igen
                }
                if (activePowerUp != 2) //Om det inte är flygförmåga man har så avaktivera powerup direkt tiden gått ut
                {
                    activePowerUp = 0;
                }
            }
        }
        /// <summary>
        /// Körs när man tar ett föremål som ger hälsa
        /// </summary>
        /// <param name="amountToAdd">Hur mycket hälsa man vill öka med</param>
        public void AddHealth(int amountToAdd)
        {
            if (healthLeft + amountToAdd < Constants.player_defaultHealth) //Kollar så att man inte får mer health än max, vilket är defaultHealth
            {
                targetHealth = healthLeft + amountToAdd;
                GUI.healthBarBlinking = true;
            }
            else
            {
                targetHealth = Constants.player_defaultHealth;
                GUI.healthBarBlinking = true;
            }
        }
        /// <summary>
        /// Körs när man springer in i något som ger en skada
        /// </summary>
        /// <param name="amountToLose">Hur mycket skada man vill att spelaren ska ta</param>
        /// /// <param name="enemyPos">Position att ugå från när players nya targetPos sätts</param>
        public void LoseHealth(int amountToLose, Vector2 enemyPos, int enemyWidth)
        {
            if (healthLeft - amountToLose > 0) 
            {
                if (!invincible)
                {
                    targetHealth = healthLeft - amountToLose;
                    GUI.healthBarBlinking = true;
                    MovePlayerBack(enemyPos, enemyWidth);
                    ThrowWeaponInAir();
                }
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
                texUpperBody = Textures.player_upper_body_hurt;
                if (pos.X > targetPos.X)
                {
                    pos.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 350;
                }
                if (pos.X < targetPos.X)
                {
                    pos.X += (float)gameTime.ElapsedGameTime.TotalSeconds * 350;
                }
                if (Math.Abs(targetPos.X - pos.X) < 10)
                {
                    weaponThrown = false;
                    movingBack = false;
                    texUpperBody = prevTexUpperBody;
                }
            }
        }
        private void MovePlayerBack(Vector2 enemyPos, int enemyWidth)
        {
            movingBack = true;
            if (pos.X < enemyPos.X)
            {
                targetPos.X = enemyPos.X - 150;
            }
            else
            {
                targetPos.X = enemyPos.X + 50 + enemyWidth;
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
            targetScore = realScore + scoreToAdd;
            realScore = targetScore;
        }
        private void AnimatingScore()
        {
            if (score != targetScore)
            {
                score += 1;
            }
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
                    texUpperBody = prevTexUpperBody = Textures.player_upper_body;
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
                case weaponType.kebab:
                    texUpperBody = prevTexUpperBody = Textures.player_kebab;
                    currentWeapon = weaponType.kebab;
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
        public void Shooting()
        {
            if (!movingBack && shotDelay <= 0 && KeyMouseReader.KeyPressed(Keys.Space))
            {
                shotDelay = shotDelayDefault;
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
                        BulletManager.AddBullet(new HamburgareVapen(bulletPos, bulletVelocity, true));
                        
                    break;
                    case weaponType.pizza:
                        texUpperBody = Textures.player_shooting;
                        animateShooting = true;
                        BulletManager.AddBullet(new PizzaWeapon(bulletPos, bulletVelocity, false, true));
                        prevTexUpperBody = Textures.player_upper_body;                        
                        currentWeapon = weaponType.none;
                    break;
                    case weaponType.kebab:
                    texUpperBody = Textures.player_shooting;
                    animateShooting = true;
                    BulletManager.AddBullet(new KebabWeapon(bulletPos, bulletVelocity, true));
                    break;
                    case weaponType.bottle:     
                        texUpperBody = Textures.player_shooting;
                        animateShooting = true;
                        BulletManager.AddBullet(new BottleWeapon(bulletPos, bulletVelocity, true));
                    break;
                    case weaponType.molotovCocktail:
                        texUpperBody = Textures.player_shooting;
                        animateShooting = true;
                        BulletManager.AddBullet(new MolotovWeapon(bulletPos, bulletVelocity, true));
                    break;
                }  
            }
        }
        /// <summary>
        /// Kastar vapnet rakt upp i luften. Körs när player stöter emot en fiende
        /// </summary>
        private void ThrowWeaponInAir()
        {
            if (!weaponThrown)
            {
                Vector2 bulletPos, bulletVelocity = new Vector2(0, -5);
                if (facing == 0)  //vänster
                {
                    bulletPos = new Vector2(pos.X + 20, pos.Y);
                }
                else //höger
                {
                    bulletPos = new Vector2(pos.X + 30, pos.Y);
                }
                switch (currentWeapon)
                {
                    case weaponType.burger:
                        BulletManager.AddBullet(new HamburgareVapen(bulletPos, bulletVelocity, false));
                        weaponThrown = true;
                        break;
                    case weaponType.pizza:
                        BulletManager.AddBullet(new PizzaWeapon(bulletPos, bulletVelocity, true, false));
                        weaponThrown = true;
                        break;
                    case weaponType.kebab:
                        BulletManager.AddBullet(new KebabWeapon(bulletPos, bulletVelocity, false));
                        weaponThrown = true;
                        break;
                    case weaponType.bottle:
                        BulletManager.AddBullet(new BottleWeapon(bulletPos, bulletVelocity, false));
                        weaponThrown = true;
                        break;
                    case weaponType.molotovCocktail:
                        BulletManager.AddBullet(new MolotovWeapon(bulletPos, bulletVelocity, false));
                        weaponThrown = true;
                        break;
                }
            }
        }

        #region Reset metoder
        /// <summary>
        /// Återstället position, hälsa och sätter vapentyp till none
        /// </summary>
        public void Reset()
        {
            facing = 1; //Så att player tittar åt höger när den återställs
            targetScore = score;
            shotDelay = shotDelayDefault;
            ResetPos();
            BringToLife();
            ResetWeapon();
            ResetHealth();
        }
        /// <summary>
        /// Sätter isDead = false
        /// </summary>
        private void BringToLife()
        {
            isDead = false;
        }
        /// <summary>
        /// Sätter pos och targetPos till currentSpawnPos
        /// </summary>
        private void ResetPos()
        {
            targetPos = pos = currentSpawnPos;
        }
        /// <summary>
        /// Återställer currentWeapon till none
        /// </summary>
        private void ResetWeapon()
        {
            currentWeapon = weaponType.none;
            PickUpWeapon(weaponType.none);
            BulletManager.bullets.Clear();
        }
        /// <summary>
        /// Återställer hälsan till defaultHealth
        /// </summary>
        private void ResetHealth()
        {
            healthLeft = targetHealth = Constants.player_defaultHealth;
        }
        /// <summary>
        /// Avaktiverar eventuell powerup
        /// </summary>
        private void ResetPowerUp()
        {
            activePowerUp = 0;
            Game1.gui.ResetPowerUp();
        }
        #endregion
           
    }
}