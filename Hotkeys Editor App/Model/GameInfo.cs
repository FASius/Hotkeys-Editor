using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Hotkeys_Editor_App.Data
{
    public class GameInfo
    {
        private Side[] Sides;
        private class Side
        {
            public string Name { get; set; }
            public string[] Armies { get; set; }
            public Category[] Categories { get; set; }
        }

        private class Category
        {
            public string Name { get; set; }
            public Unit[] Units { get; set; }
        }

        private class Unit
        {
            public string Name { get; set; }
            public string[][] Hotkeys { get; set; }
        }


        public GameInfo(string path)
        {
            string file = File.ReadAllText(path);
            Sides = JsonSerializer.Deserialize<Side[]>(file);
        }

        private Side GetSide(string sideName)
        {
            for (int i = 0; i < Sides.Length; ++i)
                if (Sides[i].Name == sideName)
                    return Sides[i];
            return null;
        }

        private Category GetCategory(string sideName, string categoryName)
        {
            Side side = GetSide(sideName);
            for (int i = 0; i < side.Categories.Length; ++i)
                if (side.Categories[i].Name == categoryName)
                    return side.Categories[i];
            return null;
        }

        private Unit GetUnit(string sideName, string categoryName, string unitName)
        {
            var category = GetCategory(sideName, categoryName);
            for (int i = 0; i < category.Units.Length; ++i)
                if (category.Units[i].Name == unitName)
                    return category.Units[i];
            return null;
        }

        public IEnumerable<string> GetSides()
        {
            for (int i = 0; i < Sides.Length; ++i)
                yield return Sides[i].Name;
        }

        public IEnumerable<string> GetArmies(string sideName)
        {
            for (int i = 0; i < Sides.Length; ++i)
                if (Sides[i].Name == sideName)
                    for (int j = 0; j < Sides[i].Armies.Length; ++j)
                        yield return Sides[i].Armies[j];
        }

        public IEnumerable<string> GetCategories(string sideName)
        {
            for (int i = 0; i < Sides.Length; ++i)
                if (Sides[i].Name == sideName)
                    for (int j = 0; j < Sides[i].Categories.Length; ++j)
                        yield return Sides[i].Categories[j].Name;
        }

        public IEnumerable<string> GetUnits(string sideName, string categoryName)
        {
            var category = GetCategory(sideName, categoryName);
            for (int i = 0; i < category.Units.Length; ++i)
                yield return category.Units[i].Name;
        }

        public string[][] GetHotkeys(string sideName, string categoryName, string unitName)
        {
            var unit = GetUnit(sideName, categoryName, unitName);
            return unit.Hotkeys;
        }
    }
}
