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
        private Vector2 targetPos;
        public bool movingBack, weaponThrown;

        //LB = Lower Body. För att kunna animera benen för sig så att player inte springer på stället när man kör skjutanimationen
        private Texture2D texUpperBody, texLowerBody, prevTexUpperBody, texUpperBodyWithoutPowerUp;
        private double timeTilNextFrameLB = 0;
        private int frameLB;
        private Rectangle srcRectLB;
        private float rotation = 0;
        private bool isMorphing;
        public bool animateShooting;
        private double shotDelay, shotDelayDefault = 700;
        private int prevFacing;
        private bool invincible;

        private const int playerSpeed = 80;
        public static int livesLeft;
        public static int healthLeft;
        private int targetHealth;
        public static int score = 0;
        private int targetScore, realScore; //realScore för att score-räknare inte hann med att räkna upp om man tog många poäng på en gång

        public static int activePowerUp; //1: odödlig, 2: flygförmåga, osv och "0" för ingenting
        private double activePowerUpTimer;
        private Random rnd = new Random();
        public ParticleEngine2 particleEngine;
        private Vector2 particleEngingePos;
        public enum weaponType { none, burger, pizza, kebab, bottle, molotovCocktail };
        public static weaponType currentWeapon;

        public int jumpHeight = 12;
        public bool isDead { get; private set; }
        public Vector2 currentSpawnPos;
        private double spawnTimer, spawnTimerDefault = 1750;
        public bool spawning;
        private bool hasJumpedAfterSpawn;
        private Rectangle srcRectSpawnHead;

        int activeWeapon;
        HamburgareVapen burgerWeapon;
        BottleWeapon bottleWeapon;
        PizzaWeapon pizzaWeapon;
        KebabWeapon kebabWeapon;

        public static int burgerWeapons, bottleWeapons, kebabWeapons, pizzaWeapons;
        bool gotBurgers, gotBottles, gotKebab, gotPizza;

        public Player(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            score = 0;
            currentSpawnPos = pos;
            srcRectLB = srcRect;
            livesLeft = Constants.player_defaultLives;
            Reset();
            this.type = "player";
            texUpperBody = Textures.player_upper_body;
            texLowerBody = Textures.player_lower_body;
            prevTexUpperBody = texUpperBody;
            particleEngine = new ParticleEngine2(Textures.smokeParticles, Vector2.Zero, 2, 2, Textures.explosionTexture, false);

        }
        public override void Update(GameTime gameTime)
        {
            switch (activePowerUp)
            {
                case 0: //Vanlig
                    PlayerMovement(gameTime);
                    AddFriction();
                    PlayerJumping();
                    Shooting();
                    CheckIfPlayerIsOnPlatform();
                    AnimateWhenInAir(gameTime);

                    SetDeadFallingOffPlatform();
                    break;
                case 1: //Odödlig
                    PlayerMovement(gameTime);
                    AddFriction();
                    PlayerJumping();
                    Shooting();
                    CheckIfPlayerIsOnPlatform();
                    AnimateWhenInAir(gameTime);
                    SetDeadFallingOffPlatform();

                    break;

                case 2: //Flygförmåga
                    PlayerFlying(gameTime);
                    AddFriction();
                    Shooting();
                    SetDeadFallingOffPlatform();
                    CheckIfPlayerIsOnPlatform();

                    if (!isMorphing)
                    {
                        particleEngine.Update(particleEngingePos, true);
                    }

                    break;


            }

            if (KeyMouseReader.KeyPressed(Keys.D1) && burgerWeapons > 0)
            {
                PickUpWeapon(weaponType.burger);
                //gotBurgers = true;
                //gotKebab = false;
                //gotBottles = false;
                //gotPizza = false;
            }
            if (KeyMouseReader.KeyPressed(Keys.D2) && kebabWeapons > 0)
            {
                PickUpWeapon(weaponType.kebab);
                //gotBurgers = false;
                //gotKebab = true;
                //gotBottles = false;
                //gotPizza = false;
            }
            if (KeyMouseReader.KeyPressed(Keys.D3) && bottleWeapons > 0)
            {
                PickUpWeapon(weaponType.bottle);
                //gotBurgers = false;
                //gotKebab = false;
                //gotBottles = true;
                //gotPizza = false;
            }
            if (KeyMouseReader.KeyPressed(Keys.D4) && pizzaWeapons > 0)
            {
                PickUpWeapon(weaponType.pizza);
                //gotBurgers = false;
                //gotKebab = false;
                //gotBottles = false;
                //gotPizza = true;
            }


            if (burgerWeapons > 0)
                gotBurgers = true;
            else if (burgerWeapons <= 0)
                gotBurgers = false;

            if (kebabWeapons > 0)
                gotKebab = true;
            else
                gotKebab = false;

            if (bottleWeapons > 0)
                gotBottles = true;
            else
                gotBottles = false;

            if (pizzaWeapons > 0)
                gotPizza = true;
            else
                gotPizza = false;

            Console.WriteLine("burgare" + burgerWeapons);
            Console.WriteLine("flaskor" + bottleWeapons);
            //switch (currentWeapon)
            //{
            //    case weaponType.burger:
            //        if (burgerWeapons <= 0)
            //        {
            //            gotBurgers = false;
            //            PickUpWeapon(weaponType.none);
            //            currentWeapon = weaponType.none;
            //        }
            //        break;
            //}


            base.Update(gameTime);
            AnimateLowerBody();
            AnimateShooting(gameTime);
            AnimateHealthBar();
            MoveBackWhenEnemyContact(gameTime);
            CountDownShotDelay(gameTime);
            AnimatingScore();
            SpawnAnimation(gameTime);
            PowerUpTimerAandDeactivation(gameTime);
            prevFacing = facing;
        }

        /// <summary>
        /// Två Draw() här, en för underkroppen och en för överkroppen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!animateShooting && !isMorphing)//Borde kunna få in den här någon annanstans men kommer inte på nåt bra nu. srcRect är samma om man inte skjuter
            {
                srcRectLB = srcRect;
            }
            if (!spawning)
            {
                spriteBatch.Draw(texLowerBody, pos, srcRectLB, Color.White, rotation, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
                spriteBatch.Draw(texUpperBody, pos, srcRect, Color.White, rotation, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
            }
            else //När player spawnas så ritas bara huvududet ut
            {
                spriteBatch.Draw(Textures.player_head, new Vector2(pos.X + 2, pos.Y + 50), srcRectSpawnHead, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
            }
            particleEngine.Draw(spriteBatch);
        }
        /// <summary>
        /// Flygförmåga efter intag av Redbull vodka
        /// </summary>
        private void PlayerFlying(GameTime gameTime)
        {
            facing = 1;

            particleEngingePos = new Vector2(pos.X + 14, pos.Y + 115);
            rotation = 0f;
            if (!isMorphing)
            {
                if (KeyMouseReader.keyState.IsKeyDown(Keys.Left))
                {
                    particleEngingePos = new Vector2(pos.X + 20, pos.Y + 110);
                    rotation = -0.1f;
                    movement.X -= 1;
                }
                if (KeyMouseReader.keyState.IsKeyDown(Keys.Right))
                {
                    particleEngingePos = new Vector2(pos.X + 11, pos.Y + 110);
                    rotation = +0.1f;
                    movement.X += 1;
                }
                if (KeyMouseReader.keyState.IsKeyDown(Keys.Up) && activePowerUpTimer >= 0)
                {
                    movement.Y -= 0.5f;
                }
                if (activePlatform != null && activePowerUpTimer >= 0) //Simulera jetpack motor
                {
                    movement.Y -= (float)rnd.NextDouble() * 3;
                }
            }
            AddGravity(0.2f);
            movement.Y -= movement.Y * 0.05f; //Lite "friktion" i Y-led så att man inte flyger för snabbt
            pos += movement * (float)gameTime.ElapsedGameTime.TotalSeconds * playerSpeed;
            pos.X = MathHelper.Clamp(pos.X, 0, Level.levelWidth - srcRect.Width); //Hindrar player från att flyga utanför skärmen
        }
        /// <summary>
        /// Räknar ner timeTilNextFrameLB och timeTilNextFrame när man styr player
        /// </summary>
        private void PlayerMovement(GameTime gameTime)
        {
            if (!movingBack && !spawning)
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
        /// Tvingar fram uppdatering av frame även om timeTilNextFrame inte hunnit bli noll.
        /// Undviker bl a "skridskoeffekten" som fanns innnan.
        /// </summary>
        private void ForceFrameChange()
        {
            srcRectLB.X = facingSrcRects[facing].X + (frameLB % nrFrames) * frameWidth;
            srcRect.X = facingSrcRects[facing].X + (frame % nrFrames) * frameWidth;
        }
        private void AddFriction()
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
            if (livesLeft < Constants.player_defaultLives)
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
                DeactivatePowerUp();
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
                hasJumpedAfterSpawn = false;
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
                ForceFrameChange();
                if (!hasJumpedAfterSpawn)
                {
                    movement.Y += -jumpHeight;
                    activePlatform = null;
                    hasJumpedAfterSpawn = true;
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
            currentSpawnPos = new Vector2(pos.X - 15, pos.Y - srcRect.Height + Textures.toilet_open.Height);
        }
        /// <summary>
        /// Körs när man tar en PowerUp. Switch/case satsen i Update() avgör vad som händer med player när poweruppen är aktiv
        /// </summary>
        /// <param name="powerUp">1: Odödlighet, 2: Flygförmåga</param>
        /// <param name="time">Tid i ms för hur länge powerup är aktiv</param>
        public void ActivatePowerUp(int powerUp, double time)
        {
            tex = texUpperBody;
            isMorphing = true;
            DeactivatePowerUp();
            activePowerUp = powerUp;
            activePowerUpTimer = time;
            Game1.gui.ShowPowerUpCounter(powerUp, time);

            //Ser till att frame sätts till första framen i animationen
            if (facing == 0) //Vänd åt vänster
            {
                frame = 8;
            }
            else //Vänd åt höger
            {
                frame = 0;
            }
            ForceFrameChange();
            if (powerUp == 1) //Odödlighet...
            {
                invincible = true;
                texUpperBody = Textures.player_invincible_morph;
            }
            else if (powerUp == 2) //Flygning...
            {
                texUpperBody = Textures.player_jetpack_morph;
                particleEngine = new ParticleEngine2(Textures.smokeParticles, pos, 6, 100, Textures.explosionTexture, true);
            }
        }
        /// <summary>
        /// Räknar ner activePowerUpTimer och avaktiverar powerup när tiden gått ut
        /// </summary>
        private void PowerUpTimerAandDeactivation(GameTime gameTime)
        {
            if (activePowerUpTimer >= 0) //Om powerup är aktiv
            {
                if (isMorphing)
                {
                    timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    //Byter textur när man når sista framen i animationen eller player vänder sig om
                    if ((facing == 1 && frame == 7) || (facing == 0 && frame == 15) || (facing != prevFacing))
                    {
                        SwitchTextureAfterMorphing();
                    }
                }
                activePowerUpTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else if (activePowerUp != 0 && activePowerUpTimer <= 0) //Avaktiverar poweruppen när tiden gått ut
            {
                rotation = 0f;
                if (activePowerUp == 2 && activePlatform == null)
                {
                    AddGravity(0.2f);//Om man är uppe i luften när tiden går ut så fortsätt med 0.2 gravitation tills man är tillbaka på en plattform. Annars riskerar man att falla för snabbt och rakt igenom en plattform
                }
                else //Annars avaktivera direkt
                {
                    DeactivatePowerUp();
                }
            }
        }

        /// <summary>
        /// Byter till aktuell powerup textur efter att morph-animationen körts färdigt
        /// </summary>
        private void SwitchTextureAfterMorphing()
        {
            isMorphing = false;
            if (activePowerUp == 1)
            {
                texUpperBody = Textures.player_invincible;
            }
            else if (activePowerUp == 2)
            {
                texUpperBody = Textures.player_jetpack;
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
        public void MovePlayerBack(Vector2 enemyPos, int enemyWidth)
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
                    if (activePowerUp == 0)
                    {
                        texUpperBody = Textures.player_upper_body;
                    }
                    else
                    {
                        texUpperBodyWithoutPowerUp = Textures.player_upper_body;
                    }
                    currentWeapon = weaponType.none;
                    break;
                case weaponType.burger:
                    if (activePowerUp == 0)
                    {
                        texUpperBody = Textures.player_burger;
                    }
                    else
                    {
                        texUpperBodyWithoutPowerUp = Textures.player_burger;
                    }
                    currentWeapon = weaponType.burger;
                    break;
                case weaponType.pizza:
                    if (activePowerUp == 0)
                    {
                        texUpperBody = Textures.player_pizza;
                    }
                    else
                    {
                        texUpperBodyWithoutPowerUp = Textures.player_pizza;
                    }
                    currentWeapon = weaponType.pizza;
                    break;
                case weaponType.kebab:
                    if (activePowerUp == 0)
                    {
                        texUpperBody = Textures.player_kebab;
                    }
                    else
                    {
                        texUpperBodyWithoutPowerUp = Textures.player_bottle;
                    }
                    currentWeapon = weaponType.kebab;
                    break;
                case weaponType.bottle:
                    if (activePowerUp == 0)
                    {
                        texUpperBody = Textures.player_bottle;
                    }
                    else
                    {
                        texUpperBodyWithoutPowerUp = Textures.player_bottle;
                    }
                    currentWeapon = weaponType.bottle;
                    break;
                case weaponType.molotovCocktail:
                    if (activePowerUp == 0)
                    {
                        texUpperBody = Textures.player_bottle_molotov;
                    }
                    else
                    {
                        texUpperBodyWithoutPowerUp = Textures.player_bottle_molotov;
                    }
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
            if (currentWeapon != weaponType.none && !movingBack && shotDelay <= 0 && KeyMouseReader.KeyPressed(Keys.Space) && (gotBurgers || gotKebab || gotBottles || gotPizza))
            {
                shotDelay = shotDelayDefault;
                Vector2 bulletPos, bulletVelocity;
                prevTexUpperBody = texUpperBody;
                switch (activePowerUp)
                {
                    case 0:
                        texUpperBody = Textures.player_shooting;
                        break;

                    case 1:
                        texUpperBody = Textures.player_invincible_shooting;
                        break;
                }
                animateShooting = true;
                if (facing == 0)  // Skjuter vänster
                {
                    bulletPos = new Vector2(pos.X, pos.Y + 60);
                    bulletVelocity = new Vector2(-10, 0);
                    frame = 8;
                }
                else //Skjuter höger
                {
                    bulletPos = new Vector2(pos.X + 60, pos.Y + 60);
                    bulletVelocity = new Vector2(10, 0);
                    frame = 0;
                }

                switch (currentWeapon)
                {
                    case weaponType.burger:
                        BulletManager.AddBullet(new HamburgareVapen(bulletPos, bulletVelocity, true));
                        burgerWeapons--;
                        if (burgerWeapons <= 0)
                        {
                            PickUpWeapon(weaponType.none);
                        }
                        break;

                    case weaponType.pizza:
                        BulletManager.AddBullet(new PizzaWeapon(bulletPos, bulletVelocity, false, true));
                        pizzaWeapons--;
                        if (pizzaWeapons <= 0)
                        {
                            PickUpWeapon(weaponType.none);
                        }
                        if (activePowerUp == 0)
                            prevTexUpperBody = Textures.player_upper_body;
                        //currentWeapon = weaponType.none;
                        break;

                    case weaponType.kebab:
                        BulletManager.AddBullet(new KebabWeapon(bulletPos, bulletVelocity, true));
                        kebabWeapons--;
                        if (kebabWeapons <= 0)
                        {
                            PickUpWeapon(weaponType.none);
                        }
                        break;

                    case weaponType.bottle:
                        BulletManager.AddBullet(new BottleWeapon(bulletPos, bulletVelocity, true));
                        bottleWeapons--;
                        if (bottleWeapons <= 0)
                        {
                            PickUpWeapon(weaponType.none);
                        }
                        break;

                    case weaponType.molotovCocktail:
                        BulletManager.AddBullet(new MolotovWeapon(bulletPos, bulletVelocity, true));
                        bottleWeapons--;
                        if (bottleWeapons <= 0)
                        {
                            PickUpWeapon(weaponType.none);
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Räknar ner shotDelay variablen som styr hur snabbt player kan skjuta.
        /// </summary>
        private void CountDownShotDelay(GameTime gameTime)
        {
            if (shotDelay >= 0)
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
                if ((facing == 1 && frame == 7) || (facing == 0 && frame == 15) || (facing != prevFacing))
                {
                    animateShooting = false;
                    texUpperBody = prevTexUpperBody; //Byter tillbaka till föregående textur så att skottanimationen bara körs en gång per skott
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
            activePowerUp = 0;
            burgerWeapons = 0;
            pizzaWeapons = 0;
            kebabWeapons = 0;
            bottleWeapons = 0;
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
        private void DeactivatePowerUp()
        {
            if (activePowerUp != 0)
            {
                rotation = 0;
                particleEngine.isActive = false;
                invincible = false;
                activePowerUp = 0;
                Game1.gui.ResetPowerUp();
                texUpperBody = texUpperBodyWithoutPowerUp;
            }
        }
        #endregion

    }
}