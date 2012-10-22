using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS8803AGA.engine;
using CS8803AGA.controllers;
using Microsoft.Xna.Framework;
using CS8803AGA.collision;
using CS8803AGAGameLibrary;

namespace CS8803AGA.story.characters
{
    class Food : Character
    {
        public enum FoodType { CHICKEN, LOBSTER, PIZZA, STEAK, CAKE };

        public FoodType Type
        {
            get
            {
                return type;
            }
        }
        private FoodType type;

        public Food(FoodType tp)
            : base()
        {
            type = tp;
        }

        public override string getDialogue(bool shouting)
        {
            return "Food can't talk!";
        }

        public override void act(Collider mover, bool shouting)
        {
        }

        public override CharacterInfo getCharacterInfo()
        {
            switch (type)
            {
                case FoodType.CAKE:
                    return GlobalHelper.loadContent<CharacterInfo>(@"Characters/Cake");
                case FoodType.CHICKEN:
                    return GlobalHelper.loadContent<CharacterInfo>(@"Characters/Chicken");
                case FoodType.LOBSTER:
                    return GlobalHelper.loadContent<CharacterInfo>(@"Characters/Lobster");
                case FoodType.PIZZA:
                    return GlobalHelper.loadContent<CharacterInfo>(@"Characters/Pizza");
                default:
                    return GlobalHelper.loadContent<CharacterInfo>(@"Characters/Steak");
            }
        }
    }
}