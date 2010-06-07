using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    public class Striker : IEntity
    {
        public Model Model;
        public Matrix[] Transforms;

        //position of the model in world space
        //public Vector3 Position = Vector3.Zero;
        public Vector3 Position = new Vector3(GameConstants.PlayFieldSizeX/12, -1*GameConstants.PlayFieldSizeY/2, 0.0f);

        //velocity of the model, applied each frame to the model's position
        public Vector3 Velocity = Vector3.Zero;

        //amplify the speed
        private const float VelocityScale = 5.0f;

        public Matrix RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2);
        private float rotation;
        public float Rotation
        {
            get { return rotation; }
            set
            {
                float val = value;
                while (val >= MathHelper.TwoPi)
                {
                    val -= MathHelper.TwoPi;
                }
                while (val < 0)
                {
                    val += MathHelper.TwoPi;
                }

                if (rotation != value)
                {
                    rotation = value;
                    RotationMatrix =
                        Matrix.CreateRotationX(MathHelper.PiOver2) *
                        Matrix.CreateRotationZ(rotation);
                }
            }
        }

        /// <summary>
        /// Rotate the trajectory of the striker and change its initial velocity
        /// </summary>
        /// <param name="controllerState"></param>
        public void Update(KeyboardState controllerState)
        {
            //rotate the model using the arrow keys
            //check PREVIOUS state to make sure the key isnt being held down
            if (controllerState.IsKeyDown(Keys.Left))
            {
                Rotation += (MathHelper.PiOver4 / 8);
            }
            else if (controllerState.IsKeyDown(Keys.Right))
            {
                Rotation -= (MathHelper.PiOver4 / 8);
            }

            //modify velocity vector if required, also using arrow keys
            //check PREVIOUS state to make sure the key isnt being held down
            if (controllerState.IsKeyDown(Keys.Up))
            {
                Velocity += RotationMatrix.Forward * VelocityScale;
            }
            else if (controllerState.IsKeyDown(Keys.Down))
            {
                Velocity -= RotationMatrix.Forward * VelocityScale;
            }
        }
    }
}
