﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace Lost_Soul
{
    public class CraftGUISquarePickButton : GUIButton
    {
        RenderWindow _screen;
        public CraftGUISquarePickButton(RenderWindow rw, int id, int x, int y, int slotid)
        {
            _screen = rw;
            ID = id;
            X = x;
            Y = y;
            SlotID = slotid;
            Visibility = true;
        }
        public void Clicked()
        {
        }
        public void Picked()
        {
            //if (Mouse.IsButtonPressed(Mouse.Button.Left))
                //Program.SM.States[1].GameGUI[2].Visibility = !Program.SM.States[1].GameGUI[2].Visibility;
        }
        public bool isMouseHover()
        {
            return false;
        }
        public void Draw()
        {
            SFML.Graphics.Sprite s = new SFML.Graphics.Sprite(Program.Data.SpriteBasedOnType(SpriteType.Button)[ID]);
            s.Position = new Vector2f(X, Y);
            _screen.Draw(s);

            CraftGUI g = (CraftGUI)Program.SM.States[1].GameGUI[10];
            if (SlotID + 4 * g.PickPage < Logic.KnownRecipeForThisCharacter(Program.Data.CurrentParty.MainParty.MyParty[0], g.CurClass).Count)
            {
                s = new SFML.Graphics.Sprite(Program.Data.SpriteBasedOnType(SpriteType.Items)[Program.Data.MyItems[Logic.KnownRecipeForThisCharacter(Program.Data.CurrentParty.MainParty.MyParty[0], g.CurClass)[SlotID + 4 * g.PickPage]].Sprite]);
                s.Position = new Vector2f(X, Y);
                _screen.Draw(s);
            }
        }
        public void Update()
        {
        }
        public bool isFocused()
        {
            return false;
        }
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Visibility { get; set; }
        public int SlotID { get; set; }
    }
}
