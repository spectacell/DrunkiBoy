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
        private double activePowerUpTimer = 10;
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
                //activePowerUpTimer = 15;
            }
            base.Update(gameTime);
            AnimateLowerBody();
            AnimateShooting(gameTime);
            AnimateHealthBar();
            MoveBackWhenEnemyContact(gameTime);
            CountDownShotDelay(gameTime);
            AnimatingScore();
            SpawnAnimation(gameTime);
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
            
            if (!spawning) { 
                spriteBatch.Draw(texLowerBody, pos, srcRectLB, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                spriteBatch.Draw(texUpperBody, pos, srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
            }
            else
            {
                spriteBatch.Draw(Textures.player_head, new Vector2(pos.X+2, pos.Y+50), srcRectSpawnHead, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
            }
        }
               
        /// <summary>
        /// Räknar ner timeTilNextFrameLB och timeTilNextFrame när man styr player
        /// </summary>
        /// <param name="gameTime"></param>
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
            pos += movement * (float)gameTime.ElapsedGameTime.TotalSeconds * playerSpeed;
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
                        BulletManager.AddBullet(new HamburgareVapen(bulletPos, bulletVelocity));
                        
                    break;
                    case weaponType.pizza:
                        texUpperBody = Textures.player_shooting;
                        animateShooting = true;
                        BulletManager.AddBullet(new PizzaWeapon(bulletPos, bulletVelocity, false));
                        prevTexUpperBody = Textures.player_upper_body;                        
                        currentWeapon = weaponType.none;
                    break;
                    case weaponType.kebab:
                    texUpperBody = Textures.player_shooting;
                    animateShooting = true;
                    BulletManager.AddBullet(new KebabWeapon(bulletPos, bulletVelocity));
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
                        BulletManager.AddBullet(new HamburgareVapen(bulletPos, bulletVelocity));
                        weaponThrown = true;
                        break;
                    case weaponType.pizza:
                        BulletManager.AddBullet(new PizzaWeapon(bulletPos, bulletVelocity, true));
                        weaponThrown = true;
                        break;
                    case weaponType.kebab:
                        BulletManager.AddBullet(new KebabWeapon(bulletPos, bulletVelocity));
                        weaponThrown = true;
                        break;
                    case weaponType.bottle:
                        BulletManager.AddBullet(new BottleWeapon(bulletPos, bulletVelocity));
                        weaponThrown = true;
                        break;
                    case weaponType.molotovCocktail:
                        BulletManager.AddBullet(new MolotovWeapon(bulletPos, bulletVelocity));
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
        }
        /// <summary>
        /// Återställer hälsan till defaultHealth
        /// </summary>
        private void ResetHealth()
        {
            healthLeft = targetHealth = Constants.player_defaultHealth;
        }
        #endregion
           
    }
}