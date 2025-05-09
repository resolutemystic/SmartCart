using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;

namespace SmartCart
{
    public static class Database
    {
        static string dbPath = FileSystem.AppDataDirectory + "\\grocery.db";
        static string connectionString = $"Data Source={dbPath}";

        public static Dictionary<string, int> categoryDict = new();
        public static Dictionary<string, int> groceryItemDict = new();
        public static Dictionary<string, int> priorityDict = new();
        public static Dictionary<string, int> categorizedItemDict = new();

        static Database()
        {
            if (!File.Exists(dbPath))
            {
                SqliteConnection connection = new(connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    BEGIN TRANSACTION;
                    CREATE TABLE IF NOT EXISTS Categories (
                        categoryID INTEGER NOT NULL UNIQUE,
                        name TEXT NOT NULL UNIQUE,
                        PRIMARY KEY(categoryID)
                    );
                    CREATE TABLE IF NOT EXISTS GroceryItems (
                        itemID INTEGER NOT NULL UNIQUE,
                        name TEXT NOT NULL,
                        category INTEGER NOT NULL,
                        PRIMARY KEY(itemID)
                    );
                    CREATE TABLE IF NOT EXISTS GroceryList (
                        entryID INTEGER NOT NULL UNIQUE,
                        itemID INTEGER NOT NULL,
                        quantity INTEGER NOT NULL DEFAULT 1,
                        priority INTEGER NOT NULL,
                        isChecked INTEGER NOT NULL DEFAULT 0 CHECK(isChecked IN (0, 1)),
                        PRIMARY KEY(entryID),
                        FOREIGN KEY(itemID) REFERENCES GroceryItems(itemID)
                    );
                    CREATE TABLE IF NOT EXISTS Priorities (
                        priorityID INTEGER NOT NULL,
                        name TEXT NOT NULL UNIQUE,
                        PRIMARY KEY(priorityID)
                    );
                    INSERT INTO Categories (categoryID, name) VALUES
                        (1,'Dairy'), (2,'Beverages'), (3,'Deli'), (4,'Canned/Dried Foods'),
                        (5,'Produce'), (6,'Baked Goods'), (7,'Frozen Foods'), (8,'Snacks'), (9,'Other');
                    INSERT INTO GroceryItems (itemID, name, category) VALUES
                        (1,'Milk',1), (2,'Block Cheese',1), (3,'Cream Cheese',1), (4,'Cottage Cheese',1),
                        (5,'Cola',2), (6,'Diet Cola',2), (7,'Bottled Water',2), (8,'Orange Juice',2),
                        (9,'Lemon-Lime Soda',2), (10,'Diet Lemon-Lime Soda',2), (11,'Iced Tea',2),
                        (12,'Tea',2), (13,'Coffee',2), (14,'Frozen Vegetable',7), (15,'Frozen Fruit',7),
                        (16,'Frozen Pizza',7), (17,'Ice Cream',7), (18,'Frozen French Fries',7),
                        (19,'Frozen Chicken',7), (20,'Sliced Ham',3), (21,'Sliced Turkey',3),
                        (22,'Sliced Roast Beef',3), (23,'Sliced Cheese',3), (24,'Canned Meat',4),
                        (25,'Canned Fruit',4), (26,'Canned Vegetables',4), (27,'Canned Fish',4),
                        (28,'Cereal',4), (29,'Dried Noodles',4), (30,'Apple',5), (31,'Banana',5),
                        (32,'Orange',5), (33,'Lettuce',5), (34,'Broccoli',5), (35,'Carrot',5),
                        (36,'White Bread Loaf',6), (37,'Sourdough Bread Loaf',6), (38,'Whole Wheat Bread Loaf',6),
                        (39,'Cherry Pie',6), (40,'Apple Pie',6), (41,'Berry Pie',6), (42,'Muffins',6),
                        (43,'Cake',6), (44,'Potato Chips',8), (45,'Tortilla Chips',8), (46,'Tortillas',6),
                        (47,'Popcorn',8), (48,'Fruit Snacks',8), (49,'Trail Mix',8), (50,'Mixed Nuts',8),
                        (51,'Hard Candy',8), (52,'Sour Candy',8), (53,'Gummy Candy',8),
                        (54,'Bouillon Powder',9), (55,'Beef Broth',9), (56,'Bone Broth',9),
                        (57,'Chicken Broth',9), (58,'Mushrooms',5);
                    INSERT INTO Priorities (priorityID, name) VALUES
                        (1,'Low'), (2,'Medium'), (3,'High');
                    COMMIT;";
                cmd.ExecuteNonQuery();
                connection.Close();
            }

            BuildCategories();
            BuildGroceryItems();
            BuildPriorities();
            UpdateCategorizedItems(0);
        }

        public static void PullList()
        {
            using var connection = new SqliteConnection(connectionString);
            var cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandText = @"
                SELECT entryID, name, quantity, priority, isChecked
                FROM GroceryList
                INNER JOIN GroceryItems on GroceryList.itemID = GroceryItems.itemID";
            GroceryList.currentList = GroceryList.BuildList(cmd.ExecuteReader());
        }

        public static void AddToList(int itemID, int quantity, int priority, bool isChecked)
        {
            using var connection = new SqliteConnection(connectionString);
            var cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandText = $@"
                BEGIN TRANSACTION;
                INSERT INTO GroceryList (itemID, quantity, priority, isChecked)
                VALUES ({itemID}, {quantity}, {priority}, {Convert.ToInt32(isChecked)});
                COMMIT;";
            cmd.ExecuteNonQuery();
        }

        public static void UpdateCheck(int entryID)
        {
            using var connection = new SqliteConnection(connectionString);
            var cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandText = $@"
                BEGIN TRANSACTION;
                UPDATE GroceryList
                SET isChecked = CASE isChecked WHEN 0 THEN 1 ELSE 0 END
                WHERE entryID = {entryID};
                COMMIT;";
            cmd.ExecuteNonQuery();
        }

        public static bool GetCheckState(int entryID)
        {
            using var connection = new SqliteConnection(connectionString);
            var cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandText = $@"SELECT isChecked FROM GroceryList WHERE entryID ={entryID}";
            return Convert.ToBoolean(Convert.ToInt32(cmd.ExecuteScalar()));
        }

        public static void DeleteEntry(int entryID)
        {
            using var connection = new SqliteConnection(connectionString);
            var cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandText = $@"
                BEGIN TRANSACTION;
                DELETE FROM GroceryList WHERE entryID = {entryID};
                COMMIT;";
            cmd.ExecuteNonQuery();
        }

        public static void DeleteCheckedItems()
        {
            using var connection = new SqliteConnection(connectionString);
            var cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandText = @"
                BEGIN TRANSACTION;
                DELETE FROM GroceryList WHERE isChecked = 1;
                COMMIT;";
            cmd.ExecuteNonQuery();
        }

        public static void DeleteAllItems()
        {
            using var connection = new SqliteConnection(connectionString);
            var cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandText = @"
                BEGIN TRANSACTION;
                DELETE FROM GroceryList;
                COMMIT;";
            cmd.ExecuteNonQuery();
        }

        public static void UpdateQuantity(int entryID, int quantity)
        {
            using var connection = new SqliteConnection(connectionString);
            var cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandText = $@"
                BEGIN TRANSACTION;
                UPDATE GroceryList SET quantity = {quantity} WHERE entryID = {entryID};
                COMMIT;";
            cmd.ExecuteNonQuery();
        }

        public static void IncreaseQuantity(int entryID, int quantity)
        {
            using var connection = new SqliteConnection(connectionString);
            var cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandText = $@"
                BEGIN TRANSACTION;
                UPDATE GroceryList SET quantity = quantity + {quantity} WHERE entryID = {entryID};
                COMMIT;";
            cmd.ExecuteNonQuery();
        }

        public static void UpdatePriority(int entryID, int priority)
        {
            using var connection = new SqliteConnection(connectionString);
            var cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandText = $@"
                BEGIN TRANSACTION;
                UPDATE GroceryList SET priority = {priority} WHERE entryID = {entryID};
                COMMIT;";
            cmd.ExecuteNonQuery();
        }

        public static int ExistingListItem(int itemID)
        {
            using var connection = new SqliteConnection(connectionString);
            var cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandText = $@"SELECT entryID FROM GroceryList WHERE itemID = {itemID}";
            var result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        private static void BuildCategories()
        {
            categoryDict.Clear();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT categoryID, name FROM Categories";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
                categoryDict.Add(reader.GetString(1), Convert.ToInt32(reader.GetValue(0)));
        }

        private static void BuildPriorities()
        {
            priorityDict.Clear();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT priorityID, name FROM Priorities";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
                priorityDict.Add(reader.GetString(1), Convert.ToInt32(reader.GetValue(0)));
        }

        private static void BuildGroceryItems()
        {
            groceryItemDict.Clear();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT itemID, name FROM GroceryItems";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
                groceryItemDict.Add(reader.GetString(1), Convert.ToInt32(reader.GetValue(0)));
        }

        public static void UpdateCategorizedItems(int categoryID)
        {
            categorizedItemDict.Clear();
            if (categoryID == 0)
            {
                foreach (var item in groceryItemDict)
                    categorizedItemDict.Add(item.Key, item.Value);
            }

            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = $@"SELECT itemID, name FROM GroceryItems WHERE category IS {categoryID}";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
                categorizedItemDict.Add(reader.GetString(1), Convert.ToInt32(reader.GetValue(0)));
        }
    }
}

