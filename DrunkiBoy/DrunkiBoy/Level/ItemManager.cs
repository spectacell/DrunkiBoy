using Microsoft.Xna.Framework;
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

        public ItemManager()
        {

        }
        public void AddPlatform(Platform platform)
        {
            platforms.Add(platform);
        }
        public void Update(GameTime gameTime, Player player)
        {
            UpdatePlatforms(player);
        }

        private void UpdatePlatforms(Player player)
        {
            foreach (Platform platform in platforms)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.activePlatform == null)
                    {
                        if (enemy.BoundingBox.Intersects(platform.BoundingBox) && !enemy.isKilled)
                        {
                            enemy.activePlatform = platform; //Sets the activate platform
                            enemy.pos.Y = platform.BoundingBox.Top - enemy.BoundingBox.Height + 1; //+1 to maintain the Intersection
                            enemy.isOnGround = true;
                            //enemy.hasJumped = false;
                            enemy.movement.Y = 0;
                        }
                    }
                }

                if (player.movement.Y > 0) //Going down
                {
                    if (player.BottomBoundingBox.Intersects(platform.TopBoundingBox))
                    {
                        player.activePlatform = platform; //Sets the activate platform
                        player.pos.Y = platform.BoundingBox.Top - player.BoundingBox.Height + 1; //+1 to maintain the Intersection
                        player.movement.Y = 0;
                    }
                }
            }
        }
    }
}
