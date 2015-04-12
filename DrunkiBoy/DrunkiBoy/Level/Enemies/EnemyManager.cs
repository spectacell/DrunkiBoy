using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class EnemyManager
    {
        public List<Enemy> enemies = new List<Enemy>();

        public void AddEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
        }
        public void Update(GameTime gameTime, Player player)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);
                
            }            
        }
    }
}
