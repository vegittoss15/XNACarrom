﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    class GameConstants
    {
        //camera constants
        public const float CameraHeight = 25000.0f;
        public const float PlayFieldSizeX = 16000.0f;
        public const float PlayFieldSizeY = 12500.0f;
        //carrommen constants
        public const int NumCarromMen = 19;
        public const float CarromMenMinSpeed = 100.0f;
        public const float CarromMenMaxSpeed = 300.0f;
        public const float CarromMenSpeedAdjustment = 5.0f;
        public const int pieceWidth = 1500;
        public const int pieceHeight = 1500;

        public const float carromManBoundingSphereScale = 0.95f;
        public const float strikerBoundingSphereScale = 0.5f;

        public static Vector3 center = new Vector3(0.0f, 0.0f, 0.0f);
        public static Vector3[] pieceLocations = {
                                                    new Vector3(center.X, center.Y, 0.0f),
                                                    
                                                    new Vector3(center.X, center.Y + pieceWidth, 0.0f),
                                                    new Vector3(center.X, center.Y - pieceWidth, 0.0f),

                                                    // draw second layer: surrounding pieces
                                                    new Vector3(center.X + pieceHeight, center.Y + pieceWidth / 2, 0.0f),
                                                    new Vector3(center.X + pieceHeight, center.Y - pieceWidth / 2, 0.0f),
                                                    new Vector3(center.X - pieceHeight, center.Y + pieceWidth / 2, 0.0f),
                                                    new Vector3(center.X - pieceHeight, center.Y - pieceWidth / 2, 0.0f),
                                                    
                                                    //draw third layer: four corners
                                                    new Vector3(center.X, center.Y + pieceWidth*2, 0.0f),
                                                    new Vector3(center.X, center.Y - pieceWidth*2, 0.0f),
                                                    new Vector3(center.X + pieceHeight * 2, center.Y, 0.0f),
                                                    new Vector3(center.X - pieceHeight * 2, center.Y, 0.0f),

                                                    //draw third layer: immediately higher/lower pieces
                                                    new Vector3(center.X + pieceHeight, center.Y + pieceWidth / (2.0f/3), 0.0f),
                                                    new Vector3(center.X + pieceHeight, center.Y - pieceWidth / (2.0f/3), 0.0f),
                                                    new Vector3(center.X - pieceHeight, center.Y + pieceWidth / (2.0f/3), 0.0f),
                                                    new Vector3(center.X - pieceHeight, center.Y - pieceWidth / (2.0f/3), 0.0f),

                                                    // diagonal pieces
                                                    new Vector3(center.X + pieceHeight / (2.0f / 3), center.Y + pieceWidth, 0.0f),
                                                    new Vector3(center.X + pieceHeight / (2.0f / 3), center.Y - pieceWidth, 0.0f),
                                                    new Vector3(center.X - pieceHeight / (2.0f / 3), center.Y + pieceWidth, 0.0f),
                                                    new Vector3(center.X - pieceHeight / (2.0f / 3), center.Y - pieceWidth, 0.0f)
                                                };
    }
}
