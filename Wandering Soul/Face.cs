﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lost_Soul
{
    public class Face
    {
        public int ID { get; set; }
        public int FaceSprite { get; set; }
        public byte Gender { get; set; }

        public Face(int id, int face, byte gender)
        {
            ID = id;
            FaceSprite = face;
            Gender = gender;
        }

        public void DrawFace()
        {
        }

        public void DrawSprite()
        {
        }
    }
}
