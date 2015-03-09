using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class EnemyManager
    {
        public List<Enemy> enemies = new List<Enemy>();

        public void AddPlatform(Enemy enemy)
        {
            enemies.Add(enemy);
        }
        public void Update(GameTime gameTime, Player player)
        {
            UpdateEnemies(gameTime);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemies)
            {
                spriteBatch.Draw(enemy.tex, enemy.pos, enemy.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
        }
        private void UpdateEnemies(GameTime gameTime)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);
            }
        }
    }
}
