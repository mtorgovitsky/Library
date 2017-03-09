using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookLib;
using BL;
using BL.Modules;
using static BL.Categories;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private ItemsCollection collection = new ItemsCollection();

        [TestMethod]
        public void TestAddItem()
        {

            Book book = new Book(
                "book", new DateTime(2016, 12, 30), eBaseCategory.Study,
                eInnerCategory.Medicine, "author");

            Journal journal = new Journal(
                "journal", new DateTime(2016, 12, 20), eBaseCategory.Cooking,
                eInnerCategory.Desserts, 9);

            collection.Items.Add(book);

            collection.Items.Add(journal);

            Assert.AreEqual(2, collection.Items.Count);
        }

        [TestMethod]
        public void TestAddUser()
        {

            User user = new User("admin", "1", User.eUserType.Administrator);

            collection.LibraryUsers.Users.Add(user);
            
            //There have to be twoo: The BigBoss and admin
            Assert.AreEqual(2, collection.LibraryUsers.Users.Count);

        }

        [TestMethod]
        public void TestSearch()
        {

            Book book = new Book("book", new DateTime(2016, 12, 30), 
                eBaseCategory.Study, eInnerCategory.Medicine, "author");

            Journal journal = new Journal("journal", new DateTime(2016, 12, 20),
                eBaseCategory.Cooking, eInnerCategory.Desserts, 9);

            collection.Items.Add(book);

            collection.Items.Add(journal);

            var item = collection.FindItemByName("a");

            Assert.AreEqual(1, item.Count);

        }
    }
}
