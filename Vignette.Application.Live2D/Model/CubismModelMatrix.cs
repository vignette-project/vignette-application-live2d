﻿// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Collections.Generic;
using osuTK;

namespace Vignette.Application.Live2D.Model
{
    public class CubismModelMatrix
    {
        public float Width
        {
            get => size.X * matrix[0, 0];
            set
            {
                matrix[0, 0] = value / size.X;
                matrix[1, 1] = value / size.X;
            }
        }

        public float Height
        {
            get => size.Y * matrix[1, 1];
            set
            {
                matrix[0, 0] = value / size.Y;
                matrix[1, 1] = value / size.Y;
            }
        }

        public float X
        {
            get => matrix[3, 0];
            set => matrix[3, 0] = value;
        }

        public float Y
        {
            get => matrix[3, 1];
            set => matrix[3, 1] = value;
        }

        private Vector2 size;

        private Matrix4 matrix = Matrix4.Identity;

        public CubismModelMatrix(float width, float height)
            : this(new Vector2(width, height))
        {
        }

        public CubismModelMatrix(Vector2 size)
        {
            this.size = size;
        }

        public void SetupFromLayout(Dictionary<string, float> layout)
        {
            foreach ((string key, float value) in layout)
            {
                switch (key)
                {
                    case "width":
                        Width = value;
                        break;

                    case "height":
                        Height = value;
                        break;
                }
            }

            foreach ((string key, float value) in layout)
            {
                switch (key)
                {
                    case "x":
                        X = value;
                        break;

                    case "y":
                        Y = value;
                        break;

                    case "center_x":
                        matrix[3, 0] = value - (Width / 2.0f);
                        break;

                    case "center_y":
                        matrix[3, 1] = value - (Height / 2.0f);
                        break;

                    case "top":
                        Y = value;
                        break;

                    case "bottom":
                        matrix[3, 1] = value - Height;
                        break;

                    case "left":
                        X = value;
                        break;

                    case "right":
                        matrix[3, 0] = value - Width;
                        break;
                }
            }
        }
    }
}