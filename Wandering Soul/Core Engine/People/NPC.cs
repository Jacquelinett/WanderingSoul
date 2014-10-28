﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Window;
using SFML.Graphics;

namespace Lost_Soul
{
    public class NPC : LivingObject
    {
        /* ORIGINALL INTERFACE'S STUFF START HERE*/

        public bool Moved { get; set; }
        public int LastX { get; set; }
        public int LastY { get; set; }
        public int SideMapID { get; set; }
        public Map CurMap { get; set; }
        public LivingObject Targeting { get; set; }
        public int LeftAttackCooldown { get; set; }
        public int RightAttackCooldown { get; set; }
        public int AttackSpeed { get; set; }
        public AttackAction CurrentDefenseAction { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Sprite { get; set; }
        public int OnMapType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }
        public int Dir { get; set; }
        public int TargetDir { get; set; }
        public int Speed { get; set; } //Please make it factor of 16 like 1 2 4 8
        public int Range { get; set; }
        public int Level { get; set; }
        public int Index { get; set; }
        public int Behavior { get; set; }
        public int CurHP { get; set; }
        public int CurMana { get; set; }
        public bool Lefted { get; set; }
        public bool IsWalking { get; set; }
        public List<Action> ActionQueue { get; set; }

        // End here obviously



        public List<int> KnownBlueprint { get; set; }
        public List<int> KnownRecipe { get; set; }
        public List<int> KnowledgeKnown { get; set; }
        public int LearningPoint { get; set; }

        public int Face { get; set; }
        public int Body { get; set; }
        public int Hair { get; set; }
        public int HairColor { get; set; }
        public bool UseSprite { get; set; }

        public bool Playable { get; set; }
        public byte Gender { get; set; }

        public int PlayerPartySlot { get; set; }
        public int NPCBehavior { get; set; }
        public int PartyBehavior { get; set; }
        public int EXP { get; set; }
        public int CurStamina { get; set; }
        public int Hunger { get; set; }

        public int Job { get; set; }

        public int ExtraInventorySpace { get; set; }

        public Inventory Inventory;
        public Equipment Equipment;

        public int Strength{ get; set; }
        public int Endurance{ get; set; }
        public int Agility{ get; set; }
        public int Dexterity{ get; set; }
        public int Willpower{ get; set; }
        public int Intelligence{ get; set; }
        public int Luck{ get; set; }
        public int Defense{ get; set; }
        public int Resistance{ get; set; }

        public int StatsPoint { get; set; }

        public NPC(string name, int type, byte gender, int sprite, bool usesprite, int body, int face, int hair, int haircolor,int onmaptype, bool playable, int speed, int range, int job, int party)
        {
            Inventory = new Inventory(this, 16, 8);
            Equipment = new Equipment(this);
            KnownBlueprint = new List<int>();
            KnownRecipe = new List<int>();
            KnowledgeKnown = new List<int>();
            KnowledgeKnown.Add(0);
            PlayerPartySlot = party;

            Strength = 0;
            Endurance = 0;
            Agility = 0;
            Dexterity = 0;
            Willpower = 0;
            Intelligence = 0;
            Luck = 0;
            Defense = 0;
            Resistance = 0;
            StatsPoint = 0;

            PartyBehavior = (int)PartyBehaviorType.Roaming;
            NPCBehavior = 0;

            Name = name;
            Type = type;
            Sprite = sprite;
            UseSprite = usesprite;
            Body = body;
            Face = face;
            Hair = hair;
            HairColor = haircolor;
            OnMapType = onmaptype;
            Speed = speed;
            Gender = gender;
            Playable = playable;
            Dir = 3;
            Range = range;
            CurHP = Logic.GetMaxHealthBasedOnStat(this);
            CurMana = Logic.GetMaxManaBasedOnStat(this);
            CurStamina = 30;
            EXP = 0;
            Hunger = 200;

            ExtraInventorySpace = 0;

            LearningPoint = 30;
        }

        public void DropItemFromInventory(int id)
        {
            Inventory.DropItem(id, X, Y, CurMap);
        }

        public void DropItemFromEquipment(ItemType type, int secondary)
        {
            Equipment.DropItem(type, secondary, X, Y, CurMap);
        }

        public void EquipItem(int slot, ItemType type, int secondary)
        {
            Equipment.EquipItem(Inventory.GetItem(slot), type, secondary);
        }

        public void UnequipItems(ItemType type, int secondary, int slot)
        {
            Inventory.Slot[slot].Item = Equipment.UnequipItem(type, secondary);
        }

        public override void Draw(RenderWindow rw)
        {

            SFML.Graphics.Sprite s = new SFML.Graphics.Sprite(Program.Data.SpriteBasedOnType(SpriteType.Body)[Program.Data.GetBodyBasedOnGender(Gender)[Body].ID]);
            if (IsWalking)
            {
                switch (Dir)
                {
                    case 0:
                        if (Speed < 48)
                        {
                            if (WalkCount < 48)
                                s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            else if (WalkCount >= 48 && WalkCount < 88)
                                s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            else if (WalkCount >= 88 && WalkCount < 128)
                                s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                        }
                        else if (Speed >= 48 && Speed < 128)
                        {
                            if (WalkCount < 128)
                                s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            else
                                s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                        }
                        else if (Speed >= 128)
                        {
                            if (WalkCount >= 128)
                            {
                                if (Lefted)
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else
                                    s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            }
                        }
                        break;
                    case 1:
                        if (Speed < 48)
                        {
                            if (WalkCount < 48)
                                s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            else if (WalkCount >= 48 && WalkCount < 88)
                                s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            else if (WalkCount >= 88 && WalkCount < 128)
                                s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                        }
                        else if (Speed >= 48 && Speed < 128)
                        {
                            if (WalkCount < 128)
                                s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            else
                                s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                        }
                        else if (Speed >= 128)
                        {
                            if (WalkCount >= 128)
                            {
                                if (Lefted)
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else
                                    s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            }
                        }
                        break;
                    case 2:
                        if (Speed < 48)
                        {
                            if (WalkCount < 48)
                                s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            else if (WalkCount >= 48 && WalkCount < 88)
                                s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            else if (WalkCount >= 88 && WalkCount < 128)
                                s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                        }
                        else if (Speed >= 48 && Speed < 128)
                        {
                            if (WalkCount < 128)
                                s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            else
                                s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                        }
                        else if (Speed >= 128)
                        {
                            if (WalkCount >= 128)
                            {
                                if (Lefted)
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else
                                    s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            }
                        }
                        break;
                    case 3:
                        if (Speed < 48)
                        {
                            if (WalkCount < 48)
                                s.TextureRect = new IntRect(0, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            else if (WalkCount >= 48 && WalkCount < 88)
                                s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            else if (WalkCount >= 88 && WalkCount < 128)
                                s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                        }
                        else if (Speed >= 48 && Speed < 128)
                        {
                            if (WalkCount < 128)
                                s.TextureRect = new IntRect(0, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            else
                                s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                        }
                        else if (Speed >= 128)
                        {
                            if (WalkCount >= 128)
                            {
                                if (Lefted)
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else
                                    s.TextureRect = new IntRect(0, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            }
                        }
                        break;
                }
            }
            else
            {
                switch (Dir)
                {
                    case 0:
                        s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                        break;
                    case 1:
                        s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                        break;
                    case 2:
                        s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                        break;
                    case 3:
                        s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                        break;
                }
            }
            int tempoffsetX = 0;
            int tempoffsetY = 0;
            switch (TargetDir)
            {
                case 0:
                    tempoffsetX = -WalkCount;
                    break;
                case 1:
                    tempoffsetY = -WalkCount;
                    break;
                case 2:
                    tempoffsetX = WalkCount;
                    break;
                case 3:
                    tempoffsetY = WalkCount;
                    break;
            }

            s.Position = new Vector2f((LastX + CurMap.MinX) * 16 + tempoffsetX / 8 - 8, (LastY + CurMap.MinY) * 16 - 16 + tempoffsetY / 8);
            rw.Draw(s);
            s.Texture = Program.Data.SpriteBasedOnType(SpriteType.Face)[Program.Data.GetFaceBasedOnGender(Gender)[Face].ID];
            rw.Draw(s);
            s.Texture = Program.Data.SpriteBasedOnType(SpriteType.Hair)[Program.Data.GetHairBasedOnGender(Gender)[Hair].ID[HairColor]];
            rw.Draw(s);

            //Draw minihud

            s = new SFML.Graphics.Sprite(Program.Data.SpriteBasedOnType(SpriteType.SmallHUD)[0]);
            s.TextureRect = new IntRect(0, 0, 1, 1);
            for (int i = 1; i < 3; i++)
            {
                s.Position = new Vector2f((LastX + CurMap.MinX) * 16 + tempoffsetX / 8, (LastY + CurMap.MinY) * 16 + tempoffsetY / 8 - 8 / 2 + i - 11);
                rw.Draw(s);
                s.Position = new Vector2f((LastX + CurMap.MinX) * 16 + tempoffsetX / 8 + 16, (LastY + CurMap.MinY) * 16 + tempoffsetY / 8 - 8 / 2 + i - 11);
                rw.Draw(s);
            }
            for (int i = 0; i < 16 + 1; i++)
            {
                s.Position = new Vector2f((LastX + CurMap.MinX) * 16 + tempoffsetX / 8 + i, (LastY + CurMap.MinY) * 16 + tempoffsetY / 8 - 8 / 2 - 11);
                rw.Draw(s);
                s.Position = new Vector2f((LastX + CurMap.MinX) * 16 + tempoffsetX / 8 + i, (LastY + CurMap.MinY) * 16 + tempoffsetY / 8 - 8 / 2 - 9);
                rw.Draw(s);
            }
            //find health percentage
            int percentage = CurHP * 100 / MaxHealth;
            for (int i = 1; i < 16; i++)
            {
                if (i * 100 / (16 - 1) > percentage)
                    s.TextureRect = new IntRect(2, 0, 1, 1);
                else
                    s.TextureRect = new IntRect(1, 0, 1, 1);
                s.Position = new Vector2f((LastX + CurMap.MinX) * 16 + tempoffsetX / 8 + i, (LastY + CurMap.MinY) * 16 + tempoffsetY / 8 - 8 / 2 - 10);
                rw.Draw(s);
            }


        }

        public override void Draw(RenderWindow rw, int x, int y)
        {
            if (UseSprite)
                base.Draw(rw, x, y);
            else
            {
                SFML.Graphics.Sprite s = new SFML.Graphics.Sprite(Program.Data.SpriteBasedOnType(SpriteType.Body)[Program.Data.GetBodyBasedOnGender(Gender)[Body].ID]);
                int width = (int)s.Texture.Size.X / 3;
                int height = (int)s.Texture.Size.Y / 4;
                if (IsWalking)
                {
                    switch (Dir)
                    {
                        case 0:
                            if (Speed < 48)
                            {
                                if (WalkCount < 48)
                                    s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else if (WalkCount >= 48 && WalkCount < 88)
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else if (WalkCount >= 88 && WalkCount < 128)
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            }
                            else if (Speed >= 48 && Speed < 128)
                            {
                                if (WalkCount < 128)
                                    s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            }
                            else if (Speed >= 128)
                            {
                                if (WalkCount >= 128)
                                {
                                    if (Lefted)
                                        s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                    else
                                        s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                }
                            }
                            break;
                        case 1:
                            if (Speed < 48)
                            {
                                if (WalkCount < 48)
                                    s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else if (WalkCount >= 48 && WalkCount < 88)
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else if (WalkCount >= 88 && WalkCount < 128)
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            }
                            else if (Speed >= 48 && Speed < 128)
                            {
                                if (WalkCount < 128)
                                    s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            }
                            else if (Speed >= 128)
                            {
                                if (WalkCount >= 128)
                                {
                                    if (Lefted)
                                        s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                    else
                                        s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                }
                            }
                            break;
                        case 2:
                            if (Speed < 48)
                            {
                                if (WalkCount < 48)
                                    s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else if (WalkCount >= 48 && WalkCount < 88)
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else if (WalkCount >= 88 && WalkCount < 128)
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            }
                            else if (Speed >= 48 && Speed < 128)
                            {
                                if (WalkCount < 128)
                                    s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            }
                            else if (Speed >= 128)
                            {
                                if (WalkCount >= 128)
                                {
                                    if (Lefted)
                                        s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                    else
                                        s.TextureRect = new IntRect(0, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                }
                            }
                            break;
                        case 3:
                            if (Speed < 48)
                            {
                                if (WalkCount < 48)
                                    s.TextureRect = new IntRect(0, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else if (WalkCount >= 48 && WalkCount < 88)
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else if (WalkCount >= 88 && WalkCount < 128)
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            }
                            else if (Speed >= 48 && Speed < 128)
                            {
                                if (WalkCount < 128)
                                    s.TextureRect = new IntRect(0, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                else
                                    s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            }
                            else if (Speed >= 128)
                            {
                                if (WalkCount >= 128)
                                {
                                    if (Lefted)
                                        s.TextureRect = new IntRect((int)s.Texture.Size.X / 3 * 2, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                    else
                                        s.TextureRect = new IntRect(0, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                                }
                            }
                            break;
                    }
                }
                else
                {
                    switch (Dir)
                    {
                        case 0:
                            s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4 * 2, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            break;
                        case 1:
                            s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4 * 3, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            break;
                        case 2:
                            s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            break;
                        case 3:
                            s.TextureRect = new IntRect((int)s.Texture.Size.X / 3, 0, (int)s.Texture.Size.X / 3, (int)s.Texture.Size.Y / 4);
                            break;
                    }
                }
                int tempoffsetX = 0;
                int tempoffsetY = 0;
                switch (TargetDir)
                {
                    case 0:
                        tempoffsetX = -WalkCount;
                        break;
                    case 1:
                        tempoffsetY = -WalkCount;
                        break;
                    case 2:
                        tempoffsetX = WalkCount;
                        break;
                    case 3:
                        tempoffsetY = WalkCount;
                        break;
                }

                s.Position = new Vector2f(x, y);
                rw.Draw(s);
                s.Texture = Program.Data.SpriteBasedOnType(SpriteType.Face)[Program.Data.GetFaceBasedOnGender(Gender)[Face].ID];
                rw.Draw(s);
                s.Texture = Program.Data.SpriteBasedOnType(SpriteType.Hair)[Program.Data.GetHairBasedOnGender(Gender)[Hair].ID[HairColor]];
                rw.Draw(s);

            }
        }

        public bool PickItems(SpawnItems item)
        {
            return Inventory.PutItem(item);
        }

        public override void Update()
        {
            if (ActionCooldown > 0)
                ActionCooldown--;

            if (LeftAttackCooldown > 0)
                LeftAttackCooldown--;

            if (RightAttackCooldown > 0)
                RightAttackCooldown--;

            switch (CurrentAction)
            {
                case 2:
                    if (ActionCooldown == 0)
                    {
                        ConstructionGUI g = (ConstructionGUI)Program.SM.States[1].GameGUI[9];
                        if (g.Visibility)
                        {
                            if (CurMap.SpawnedSpawnableLocation[g.LocY][g.LocX] > -1)
                            {
                                SpawnBuildable b = (SpawnBuildable)CurMap.SpawnedSpawnable[CurMap.SpawnedSpawnableLocation[g.LocY][g.LocX]];
                                for (int ibr = 0; ibr < b.Required.Count; ibr++)
                                {
                                    if (b.Required.ElementAt(ibr).Value.Count > 0)
                                    {
                                        SpawnItems consitem = b.Required.ElementAt(ibr).Value[0];
                                        b.Required.ElementAt(ibr).Value.RemoveAt(0);
                                        b.Built.ElementAt(ibr).Value.Add(consitem);
                                        ActionCooldown = 60;
                                    }
                                }
                                bool constructhadallitem = true;
                                for (int cihbe = 0; cihbe < b.Required.Count; cihbe++)
                                {
                                    if (b.Built.ElementAt(cihbe).Value.Count < Program.Data.GetBuildableList()[b.ID].RequiredItems.ElementAt(cihbe).Value)
                                        constructhadallitem = false;
                                }
                                if (constructhadallitem)
                                {

                                    CurrentAction = 0;
                                    b.FinishBuilding();
                                    g.Visibility = false;
                                }
                            }
                        }
                    }
                    break;
                case 3:
                    if (ActionCooldown == 0)
                    {
                        //Logic.RemoveItemsFromInventory(Program.Data.CurrentParty.MainParty.MyParty[0], i.ItemRequired.ElementAt(r).Key, i.ItemRequired.ElementAt(r).Value);
                        CraftGUI g = (CraftGUI)Program.SM.States[1].GameGUI[10];
                        for (int r = 0; r < Program.Data.MyItems[Logic.KnownRecipeForThisCharacter(Program.Data.CurrentParty.MainParty.MyParty[0], g.CurClass)[g.CurPick + 4 * g.PickPage]].ItemRequired.Count; r++)
                        {
                            Logic.RemoveItemsFromInventory(Program.Data.CurrentParty.MainParty.MyParty[0], Program.Data.MyItems[Logic.KnownRecipeForThisCharacter(Program.Data.CurrentParty.MainParty.MyParty[0], g.CurClass)[g.CurPick + 4 * g.PickPage]].ItemRequired.ElementAt(r).Key, Program.Data.MyItems[Logic.KnownRecipeForThisCharacter(Program.Data.CurrentParty.MainParty.MyParty[0], g.CurClass)[g.CurPick + 4 * g.PickPage]].ItemRequired.ElementAt(r).Value);
                        }
                        CurrentAction = 0;
                        //Program.Data.CurrentParty.MainParty.MyParty[0].Inventory[Program.Data.CurrentParty.MainParty.MyParty[0].FindNextEmptySpace()] = new SpawnItems(Logic.KnownRecipeForThisCharacter(Program.Data.CurrentParty.MainParty.MyParty[0], g.CurClass)[g.CurPick + 4 * g.PickPage]);
                        Inventory[FindNextEmptySpace()] = new SpawnItems(CurrentActionIndex);
                    }
                    break;
            }
            if (IsWalking)
            {
                WalkCount += Speed;
                DropGUI d = (DropGUI)Program.SM.States[1].GameGUI[5];
                
                if (WalkCount >= 64 && Moved)
                {
                    switch (TargetDir)
                    {
                        case 0:
                            CurMap.SpawnedLivingThing[Y + CurMap.MinY][X + CurMap.MinX].Remove(Index);
                            X--;
                            CurMap.SpawnedLivingThing[Y + CurMap.MinY][X + CurMap.MinX].Add(Index);
                            if (this == Program.Data.CurrentParty.MainParty.MyParty[0])
                            {
                                d.DropX = X + CurMap.MinX;
                                d.DropY = Y + CurMap.MinY;
                            }
                            Moved = false;
                            break;
                        case 1:
                            CurMap.SpawnedLivingThing[Y + CurMap.MinY][X + CurMap.MinX].Remove(Index);
                            Y--;
                            CurMap.SpawnedLivingThing[Y + CurMap.MinY][X + CurMap.MinX].Add(Index);
                            if (this == Program.Data.CurrentParty.MainParty.MyParty[0])
                            {
                                d.DropX = X + CurMap.MinX;
                                d.DropY = Y + CurMap.MinY;
                            }
                            Moved = false;
                            break;
                        case 2:
                            CurMap.SpawnedLivingThing[Y + CurMap.MinY][X + CurMap.MinX].Remove(Index);
                            X++;
                            CurMap.SpawnedLivingThing[Y + CurMap.MinY][X + CurMap.MinX].Add(Index);
                            if (this == Program.Data.CurrentParty.MainParty.MyParty[0])
                            {
                                d.DropX = X + CurMap.MinX;
                                d.DropY = Y + CurMap.MinY;
                            }
                            Moved = false;
                            break;
                        case 3:
                            CurMap.SpawnedLivingThing[Y + CurMap.MinY][X + CurMap.MinX].Remove(Index);
                            Y++;
                            CurMap.SpawnedLivingThing[Y + CurMap.MinY][X + CurMap.MinX].Add(Index);
                            if (this == Program.Data.CurrentParty.MainParty.MyParty[0])
                            {
                                d.DropX = X + CurMap.MinX;
                                d.DropY = Y + CurMap.MinY;
                            }
                            Moved = false;
                            break;
                    }
                }

                if (WalkCount >= 128)
                {
                    switch (TargetDir)
                    {
                        case 0:
                            LastX--;
                            break;
                        case 1:
                            LastY--;
                            break;
                        case 2:
                            LastX++;
                            break;
                        case 3:
                            LastY++;
                            break;
                    }
                    
                    WalkCount = 0;
                    IsWalking = false;

                    if (GeneralBehavior == (int)GeneralBehaviorType.FollowingPath)
                    {
                        if (PathfindingPath.Count > 0)
                        {
                            PathfindingPath.RemoveAt(PathfindingPath.Count - 1);
                            WalkCooldown = 0;
                        }
                    }

                    
                    if (_playerParty > 0)
                        WalkCooldown = 30;
                }
                return;
            }

            else if (PathfindingPath.Count == 0 && GeneralBehavior == (int)GeneralBehaviorType.FollowingPath)
            {
                int tempx = X;
                int tempY = Y;
                switch (ActionDir)
                {
                    case 0:
                        tempx--;
                        break;
                    case 1:
                        tempY--;
                        break;
                    case 2:
                        tempx++;
                        break;
                    case 3:
                        tempY++;
                        break;
                }
                GeneralBehavior = (int)GeneralBehaviorType.Normal;
                /*
                 * 0 = normal
                 * 1 = build
                 * 2 = construction
                 * 3 = craft
                 * 4 = lit fire
                 */
                switch (CurrentAction)
                {                        
                    case 0:
                        break;
                    case 1:
                        Dir = ActionDir;

                        for (int r = tempY + CurMap.MinY; r < tempY + CurMap.MinY + Program.Data.GetBuildableList()[CurrentActionIndex].SizeY; r++)
                        {
                            for (int c = tempx + CurMap.MinX; c < tempx + CurMap.MinX + Program.Data.GetBuildableList()[CurrentActionIndex].SizeX; c++)
                            {
                                if (Logic.BlockedAt(c, r, CurMap, 1))
                                {
                                    CurrentActionIndex = -1;
                                    CurrentAction = 0;
                                    ActionDir = -1;
                                    return;
                                }
                            }
                        }
                        if (CurMap.NullList.Count > 0)
                        {
                            CurMap.SpawnedSpawnable[CurMap.NullList[0]] = Logic.GetBuildableTypeBasedOnID(CurrentActionIndex, TargetX, TargetY, CurMap);
                            for (int r = TargetY + CurMap.MinY; r < TargetY + CurMap.MinY + Program.Data.GetBuildableList()[CurrentActionIndex].SizeY; r++)
                            {
                                for (int c = TargetX + CurMap.MinX; c < TargetX + CurMap.MinX + Program.Data.GetBuildableList()[CurrentActionIndex].SizeX; c++)
                                {
                                    CurMap.SpawnedSpawnableLocation[r][c] = CurMap.NullList[0];
                                    CurMap.NullList.RemoveAt(0);
                                }
                            }
                        }
                        else
                        {
                            CurMap.SpawnedSpawnable.Add(Logic.GetBuildableTypeBasedOnID(CurrentActionIndex, TargetX, TargetY, CurMap));
                            for (int r = TargetY + CurMap.MinY; r < TargetY + CurMap.MinY + Program.Data.GetBuildableList()[CurrentActionIndex].SizeY; r++)
                            {
                                for (int c = TargetX + CurMap.MinX; c < TargetX + CurMap.MinX + Program.Data.GetBuildableList()[CurrentActionIndex].SizeX; c++)
                                {
                                    CurMap.SpawnedSpawnableLocation[r][c] = CurMap.SpawnedSpawnable.Count - 1;
                                }
                            }
                        }
                        CurrentActionIndex = -1;
                        CurrentAction = 0;
                        ActionDir = -1;
                        ConstructionGUI cg1 = (ConstructionGUI)Program.SM.States[1].GameGUI[9];
                        cg1.LocX = tempx + CurMap.MinX;
                        cg1.LocY = tempY + CurMap.MinY;
                        cg1.Visibility = true;
                        break;
                    case 2:
                        Dir = ActionDir;
                        CurrentAction = 0;
                        ActionDir = -1;
                        ConstructionGUI cg2 = (ConstructionGUI)Program.SM.States[1].GameGUI[9];
                        cg2.LocX = tempx + CurMap.MinX;
                        cg2.LocY = tempY + CurMap.MinY;
                        cg2.Visibility = true;
                        break;
                    case 3:
                        break;
                    case 4:
                        Dir = ActionDir;
                        CurrentAction = 0;
                        ActionDir = -1;
                        SpawnBuildableFire bfire = (SpawnBuildableFire)CurMap.SpawnedSpawnable[CurMap.SpawnedSpawnableLocation[tempY + CurMap.MinY][tempx + CurMap.MinX]];
                        bfire.OnFire = true;
                        break;
                }
            }

            else if (_playerParty > 0)
            {
                if (WalkCooldown == 0)
                {
                    switch ((PartyBehaviorType)_partyBehavior)
                    {
                        case PartyBehaviorType.Roaming:
                            Walk(Logic.RandomNumber(0, 4), true);
                            break;
                        case PartyBehaviorType.FollowTheLeader:
                            if (Program.Data.CurrentParty.MainParty.MyParty[0].X < X && X - Program.Data.CurrentParty.MainParty.MyParty[0].X > 1)
                            {
                                Walk(0, true);
                            }
                            else if (Program.Data.CurrentParty.MainParty.MyParty[0].X > X && Program.Data.CurrentParty.MainParty.MyParty[0].X - X > 1)
                            {
                                Walk(2, true);
                            }
                            else if (Program.Data.CurrentParty.MainParty.MyParty[0].Y < Y && Y - Program.Data.CurrentParty.MainParty.MyParty[0].Y > 1)
                            {
                                Walk(1, true);
                            }
                            else if (Program.Data.CurrentParty.MainParty.MyParty[0].Y > Y && Program.Data.CurrentParty.MainParty.MyParty[0].Y - Y > 1)
                            {
                                Walk(3, true);
                            }
                            break;
                    }
                }
                else
                {
                    WalkCooldown--;
                }
                return;
            }

            else if (PlayerParty == 0)
            {
                if (WalkCooldown == 0)
                {
                    if (GeneralBehavior == (int)GeneralBehaviorType.FollowingPath && PathfindingPath.Count > 0)
                    {
                        Walk(PathfindingPath[PathfindingPath.Count - 1], true);
                    }
                }
                else
                    WalkCooldown--;
                //else if (GeneralBehavior == (int)GeneralBehaviorType.Normal)
                //    Walk(Logic.RandomNumber(0, 3));
            }
        }

        public override void Action(byte hand)
        {
            int tempx = X;
            int tempy = Y;
            switch (Dir)
            {
                case 0:
                    tempx--;
                    break;
                case 1:
                    tempy--;
                    break;
                case 2:
                    tempx++;
                    break;
                case 3:
                    tempy++;
                    break;
            }

            if (CurMap.SpawnedSpawnableLocation[tempy + CurMap.MinY][tempx + CurMap.MinX] > -1)
            {
                Logic.AttackSpawnable(this, CurMap.SpawnedSpawnable[CurMap.SpawnedSpawnableLocation[tempy + CurMap.MinY][tempx + CurMap.MinX]]);
            }

            if (LeftAttackCooldown <= 0)
            {
                if (hand == 0)
                {
                    if (Equipment[7] == null)
                    {
                        CurMap.AtkM.ExistingAttack.Add(new BasicAttackAction(this, Program.Data.MyAttack[1], Dir, CurMap.AtkM));
                        LeftAttackCooldown = 60;
                    }
                    else
                    {
                        WeaponItem w = (WeaponItem)Program.Data.MyItems[Equipment[7].ID];
                        CurMap.AtkM.ExistingAttack.Add(Logic.CreateAttackActionFromAttack(w.Attack, this, CurMap.AtkM));
                        LeftAttackCooldown = 60;
                    }
                }
            }

            if (RightAttackCooldown <= 0)
            {
                if (hand == 1)
                {
                    if (Equipment[11] == null)
                    {
                        CurMap.AtkM.ExistingAttack.Add(new BasicAttackAction(this, Program.Data.MyAttack[1], Dir, CurMap.AtkM));
                        //CurMap.AtkM.ExistingAttack.Add(new ShieldAction(this, Program.Data.MyAttack[4], Dir, CurMap.AtkM));
                        RightAttackCooldown = 60;
                    }
                    else
                    {
                        WeaponItem w = (WeaponItem)Program.Data.MyItems[Equipment[11].ID];
                        CurMap.AtkM.ExistingAttack.Add(Logic.CreateAttackActionFromAttack(w.Attack, this, CurMap.AtkM));
                        RightAttackCooldown = 60;
                    }
                }
            }
            //if (CurMap.SpawnedLivingThing[tempy + CurMap.MinY][tempx + CurMap.MinX].Count > 0)
            //{
            //    Logic.AttackLivingObject(this, Program.Data.MyLivingObject[CurMap.SpawnedLivingThing[tempy + CurMap.MinY][tempx + CurMap.MinX][CurMap.SpawnedLivingThing[tempy + CurMap.MinY][tempx + CurMap.MinX].Count - 1]]);
            //}
        }

        public void SetViewToThisNPC(RenderWindow rw)
        {
            int tempoffsetX = 0;
            int tempoffsetY = 0;
            switch (TargetDir)
            {
                case 0:
                    tempoffsetX = -WalkCount;
                    tempoffsetX /= 8;
                    break;
                case 1:
                    tempoffsetY = -WalkCount;
                    tempoffsetY /= 8;
                    break;
                case 2:
                    tempoffsetX = WalkCount;
                    tempoffsetX /= 8;
                    break;
                case 3:
                    tempoffsetY = WalkCount;
                    tempoffsetY /= 8;
                    break;
            }
            rw.SetView(new View(new FloatRect((LastX + CurMap.MinX) * 16 - 8 - rw.Size.X / 2 + tempoffsetX, (LastY + CurMap.MinY) * 16 - 8 - rw.Size.Y / 2 + tempoffsetY, rw.Size.X, rw.Size.Y)));
        }

        public void SpawnMoreMapDueToThisNPC()
        {
            if (OnMapType != (int)MapType.MainMap)
                return;
            int tempx = X;
            int tempy = Y;
            if (IsWalking)
            {
                switch (Dir)
                {
                    case 0:
                        tempx--;
                        break;
                    case 1:
                        tempy--;
                        break;
                    case 2:
                        tempx++;
                        break;
                    case 3:
                        tempy++;
                        break;
                }
            }

            if (tempx + Program.Data.CurrentWorld.OverWorld.MinX < 50)
            {
                Program.Generator.SpawnLeft(CurMap);
            }

            if (Program.Data.CurrentWorld.OverWorld.Y[0].Tile.Count - (tempx + Program.Data.CurrentWorld.OverWorld.MinX) < 50)
            {
                Program.Generator.SpawnRight(CurMap);
            }

            if (tempy + Program.Data.CurrentWorld.OverWorld.MinY < 50)
            {
                Program.Generator.SpawnTop(CurMap);
            }

            if (Program.Data.CurrentWorld.OverWorld.Y.Count - (tempy + Program.Data.CurrentWorld.OverWorld.MinY) < 50)
            {
                Program.Generator.SpawnBottom(CurMap);
            }
        }
        
    }
}
