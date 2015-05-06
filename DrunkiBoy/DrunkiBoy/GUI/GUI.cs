using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace DrunkiBoy
{
    public class GUI
    {
        private string strLives, strLevel, strCurrentLevel, strItemsLeft, strTime, strTitle, strTitleVersion;
        private Vector2 strLivesPos, healthBarPos, strLevelPos, strItemsLeftPos, strTimePos, strTitlePos, powerUpPos;
        private int scorePosY;
        private ActivePowerUpDisplay activePowerUp;
        private Texture2D healthBarTex;
        private double healthBarBlinkTimer, healthBarBlinkTimerDefault = 250;
        public static bool healthBarBlinking;
        public static int itemsLeftToCollect;

        private float burgerOpacity = 0.5f, kebabOpacity = 0.5f, bottleOpacity = 0.5f, pizzaOpacity = 0.5f;
        private Color burgerColor = Color.White, kebabColor = Color.White, bottleColor = Color.White, pizzaColor = Color.White;

        public GUI()
        {
            strLives = "LIVES ";
            strLevel = "LEVEL ";
            strItemsLeft = "ITEMS LEFT ";
            strTime = "TIME ";
            strTitle = "DRUNKIBOY ADVENTURES ";
            strTitleVersion = "V 2.0";
            int crntLevel = Game1.currentLevel + 1; //Första leveln är 0 så måste plussa på en för att visa rätt
            strCurrentLevel = crntLevel.ToString();

            strLivesPos = new Vector2(16, 16);
            healthBarPos = new Vector2(13, 57);
            strLevelPos = new Vector2(577, 16);
            scorePosY = 57;
            strItemsLeftPos = new Vector2(920, 16);
            strTimePos = new Vector2(1032, 57);
            strTitlePos = new Vector2(833, 698);
            
            powerUpPos = new Vector2(Textures.healthBarRed.Width+25, strLivesPos.Y + 3);
            activePowerUp = new ActivePowerUpDisplay(powerUpPos, Textures.powerUpTimer, new Rectangle(0,0,63,63), false, 13, 1000, 0);

            healthBarTex = Textures.healthBarRed;

        }

        public virtual void Update(GameTime gameTime)
        {
            activePowerUp.Update(gameTime);
            HealthBarBlinking(gameTime);
            BlinkHealthBar();
            SelectAmmo();
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //Liv kvar
            spriteBatch.DrawString(Constants.FONT_BIG, strLives, strLivesPos, Constants.fontColor);
            for (int i = 0; i < Player.livesLeft; i++)
            {
                spriteBatch.Draw(Textures.heart, new Vector2(strLivesPos.X + Constants.FONT_BIG.MeasureString(strLives).X + i*40, strLivesPos.Y+5), new Rectangle(0, 0, 31, 26), Color.White);
            }
            //Health bar
            spriteBatch.Draw(healthBarTex, healthBarPos, Color.White);
            float greenBarWidth = ((float)Player.healthLeft / (float)Constants.player_defaultHealth) * Textures.healthBarGreen.Width;
            spriteBatch.Draw(Textures.healthBarGreen, new Vector2(healthBarPos.X + 3, healthBarPos.Y + 3), new Rectangle(0, 0, (int)greenBarWidth, Textures.healthBarGreen.Height), Color.White);
            //Aktiv powerup
            activePowerUp.Draw(spriteBatch);
            //Level
            float centerPosLevel = Game1.windowWidth / 2 - Constants.FONT.MeasureString(strLevel + strCurrentLevel).X / 2;
            spriteBatch.DrawString(Constants.FONT, strLevel, new Vector2(centerPosLevel, strLevelPos.Y), Constants.fontColor);
            spriteBatch.DrawString(Constants.FONT, strCurrentLevel, new Vector2(centerPosLevel + Constants.FONT.MeasureString(strLevel).X, strLevelPos.Y), Constants.fontColor2);
            //Poäng
            float centerPosScore = Game1.windowWidth/2-Constants.FONT.MeasureString(Player.score.ToString()).X/2;
            spriteBatch.DrawString(Constants.FONT, Player.score.ToString(), new Vector2(centerPosScore, scorePosY), Constants.fontColor2);
            //Föremål kvar att plocka
            spriteBatch.DrawString(Constants.FONT_BIG, strItemsLeft, strItemsLeftPos, Constants.fontColor);
            spriteBatch.DrawString(Constants.FONT_BIG, itemsLeftToCollect.ToString(), new Vector2(strItemsLeftPos.X + Constants.FONT_BIG.MeasureString(strItemsLeft).X, strItemsLeftPos.Y), Constants.fontColor2);
            //Tid kvar
            spriteBatch.DrawString(Constants.FONT_BIG, strTime, strTimePos, Constants.fontColor);
            int timeLeft = (int)Level.timeLeft; //För att avrunda till heltal
            spriteBatch.DrawString(Constants.FONT_BIG, timeLeft.ToString(), new Vector2(strTimePos.X + Constants.FONT_BIG.MeasureString(strTime).X, strTimePos.Y), Constants.fontColor2);
            //Spelets titel
            spriteBatch.DrawString(Constants.FONT, strTitle, strTitlePos, Constants.fontColor);
            spriteBatch.DrawString(Constants.FONT, strTitleVersion, new Vector2(strTitlePos.X + Constants.FONT.MeasureString(strTitle).X, strTitlePos.Y), Constants.fontColor2);
            //Ammo
            spriteBatch.DrawString(Constants.FONT, "1", new Vector2(15, 80), burgerColor * burgerOpacity);
            spriteBatch.Draw(Textures.hamburgare, new Vector2(25, 80), Color.White * burgerOpacity);
            spriteBatch.DrawString(Constants.FONT_MEDIUM, Player.burgerWeapons.ToString(), new Vector2(55, 76), burgerColor * burgerOpacity);

            spriteBatch.DrawString(Constants.FONT, "2", new Vector2(150, 80), kebabColor * kebabOpacity);
            spriteBatch.Draw(Textures.kebab, new Vector2(170, 75), Color.White * kebabOpacity);
            spriteBatch.DrawString(Constants.FONT_MEDIUM, Player.kebabWeapons.ToString(), new Vector2(197, 76), kebabColor * kebabOpacity);

            spriteBatch.DrawString(Constants.FONT, "3", new Vector2(15, 120), bottleColor * bottleOpacity);
            spriteBatch.Draw(Textures.bottle, new Vector2(25, 100), Color.White * bottleOpacity);
            spriteBatch.DrawString(Constants.FONT_MEDIUM, Player.bottleWeapons.ToString(), new Vector2(55, 116), bottleColor * bottleOpacity);

            spriteBatch.DrawString(Constants.FONT, "4", new Vector2(150, 120), pizzaColor * pizzaOpacity);
            spriteBatch.Draw(Textures.GUIPizza, new Vector2(170, 115), Color.White * pizzaOpacity);
            spriteBatch.DrawString(Constants.FONT_MEDIUM, Player.pizzaWeapons.ToString(), new Vector2(197, 116), pizzaColor * pizzaOpacity);

            spriteBatch.End();
        }
        public void SelectAmmo()
        {
            switch (Player.currentWeapon)
            {
                case Player.weaponType.burger:
                    burgerColor = Color.Gold;
                    burgerOpacity = 1f;
                    kebabColor = Color.White;
                    kebabOpacity = 0.5f;
                    bottleColor = Color.White;
                    bottleOpacity = 0.5f;
                    pizzaColor = Color.White;
                    pizzaOpacity = 0.5f;
                    break;
                case Player.weaponType.kebab:
                    burgerColor = Color.White;
                    burgerOpacity = 0.5f;
                    kebabColor = Color.Gold;
                    kebabOpacity = 1f;
                    bottleColor = Color.White;
                    bottleOpacity = 0.5f;
                    pizzaColor = Color.White;
                    pizzaOpacity = 0.5f;
                    break;
                case Player.weaponType.bottle:
                    burgerColor = Color.White;
                    burgerOpacity = 0.5f;
                    kebabColor = Color.White;
                    kebabOpacity = 0.5f;
                    bottleColor = Color.Gold;
                    bottleOpacity = 1f;
                    pizzaColor = Color.White;
                    pizzaOpacity = 0.5f;
                    break;
                case Player.weaponType.pizza:
                    burgerColor = Color.White;
                    burgerOpacity = 0.5f;
                    kebabColor = Color.White;
                    kebabOpacity = 0.5f;
                    bottleColor = Color.White;
                    bottleOpacity = 0.5f;
                    pizzaColor = Color.Gold;
                    pizzaOpacity = 1f;
                    break;
            }
            if (Player.burgerWeapons == 0)
            {
                burgerColor = Color.DarkRed;
            }
            if (Player.kebabWeapons == 0)
            {
                kebabColor = Color.DarkRed;
            }
            if (Player.bottleWeapons == 0)
            {
                bottleColor = Color.DarkRed;
            }
            if (Player.pizzaWeapons == 0)
            {
                pizzaColor = Color.DarkRed;
            }
        }
        /// <summary>
        /// Den där bilden som täcker poweruppen
        /// </summary>
        /// <param name="powerUp"></param>
        /// <param name="time"></param>
        public void ShowPowerUpCounter(int powerUp, double time)
        {
            int frameInterval = (int)(time / 12); //tiden i ms delat med antal frames
            activePowerUp = new ActivePowerUpDisplay(powerUpPos, Textures.powerUpTimer, new Rectangle(0, 0, 63, 63), true, 12, frameInterval, powerUp);
        }
        /// <summary>
        /// Ändrar healthBar-texturen om healthBarBlinkTimer är >= 0 så att det ser ut som att den blinkar till
        /// </summary>
        /// <param name="gameTime"></param>
        private void HealthBarBlinking(GameTime gameTime)
        {
            if (healthBarBlinkTimer >= 0)
            {
                healthBarTex = Textures.healthBarRedBlink;
            }
            else
            {
                healthBarTex = Textures.healthBarRed;
            }
            healthBarBlinkTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        /// <summary>
        /// I Player klassen så sätts healthBarBlinking till true när player ökar/minskar health. Det aktiverar timern här så att healthBar blinkar till
        /// </summary>
        public void BlinkHealthBar()
        {
            if (healthBarBlinking) 
            { 
                healthBarBlinkTimer = healthBarBlinkTimerDefault;
                healthBarBlinking = false;
            }
        }
        public void ResetPowerUp()
        {
            activePowerUp.isActive = false;
        }
    }
}
