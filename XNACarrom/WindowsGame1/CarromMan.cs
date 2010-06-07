using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    class CarromMan : IEntity
    {
        public Vector3 position, direction;
        public float speed;

        public void Update(float delta)
        {
            //position += direction * speed * GameConstants.CarromMenSpeedAdjustment * delta;

            //if the pieces hit the edges, make them bounce back
            //TODO account for the piece sizes when bouncing back
            if (position.X > GameConstants.PlayFieldSizeX)
            {
                direction.X *= -1;
            }
            if (position.X < -GameConstants.PlayFieldSizeX)
            {
                direction.X *= -1;
            }
            if (position.Y > GameConstants.PlayFieldSizeY)
            {
                direction.Y *= -1;
            }
            if (position.Y < -GameConstants.PlayFieldSizeY)
            {
                direction.Y *= -1;
            }
        }
    }
}
