﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace Lost_Soul
{
    public class ViewPartyEquipmentRight : GUIButton
    {
        RenderWindow _screen;
        public ViewPartyEquipmentRight(RenderWindow rw, int id, int x, int y, int slotid)
        {
            _screen = rw;
            ID = id;
            X = x;
            Y = y;
            SlotID = slotid;
        }
        public void Clicked()
        {
        }
        public void Picked()
        {
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

            PickPartyGUI g = (PickPartyGUI)Program.SM.States[0].GameGUI[1];
            if (g.SelectedParty > -1 && g.SelectedParty < Program.Data.MyPlayerDatas.Count && Program.Data.MyPlayerDatas[g.SelectedParty] != null)
            {
                if (SlotID < Program.Data.MyPlayerDatas[g.SelectedParty].MainParty.MyParty.Count)
                {
                    if (Program.Data.MyPlayerDatas[g.SelectedParty].MainParty.MyParty[SlotID].Equipment[7] != null)
                    {
                        s.Texture = Program.Data.SpriteBasedOnType(SpriteType.Items)[Program.Data.MyPlayerDatas[SlotID].MainParty.MyParty[SlotID].Equipment[7].ID];
                        s.Position = new Vector2f(X, Y);
                        _screen.Draw(s);
                    }
                }
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