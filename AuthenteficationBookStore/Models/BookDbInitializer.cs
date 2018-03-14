using System.Data.Entity;

namespace AuthenteficationBookStore.Models
{
    public class BookDbInitializer : DropCreateDatabaseIfModelChanges<BookStoreContext>
    {
        protected override void Seed(BookStoreContext db)
        {
            db.Books.Add(new Book { Name = "Hobbit", Author = "John Ronald Reuel Tolkien", Price = 140, ImageURL = @"http://d1vzko4h6qahek.cloudfront.net/images/2/books/large/HOI2.jpg", Details= "A great modern classic and the prelude to THE LORD OF THE RINGS   Bilbo Baggins is a hobbit who enjoys a comfortable, unambitious life, rarely traveling any farther than his pantry or cellar. But his contentment is disturbed when the wizard Gandalf and a company of dwarves arrive on his doorstep one day to whisk him away on an adventure. They have launched a plot to raid the treasure hoard guarded by Smaug the Magnificent, a large and very dangerous dragon. Bilbo reluctantly joins their quest, unaware that on his journey to the Lonely Mountain he will encounter both a magic ring and a frightening creature known as Gollum." });
            db.Books.Add(new Book { Name = "Blood of elves", Author = "Andrzej Sapkowski", Price = 150, ImageURL = @"https://upload.wikimedia.org/wikipedia/en/6/61/Blood_of_Elves_UK.jpg", Details = "For more than a hundred years humans, dwarves, gnomes and elves lived together in relative peace. But times have changed, the uneasy peace is over and now the races once again fight each other - and themselves: Dwarves are killing their kinsmen, and elves are murdering humans and elves, at least those elves who are friendly to humans... Into this tumultuous time is born a child for whom the witchers of the world have been waiting.Ciri, the granddaughter of Queen Calanthe, the Lioness of Cintra, has strange powers and a stranger destiny, for prophecy names her the Flame, one with the power to change the world - for good, or for evil...Geralt, the witcher of Rivia, has taken Ciri to the relative safety of the Witchers' Settlement, but it soon becomes clear that Ciri isn't like the other witchers.As the political situation grows ever dimmer and the threat of war hangs almost palpably over the land, Geralt searches for someone to train Ciri's unique powers. But someone else has an eye on the young girl, someone who understand exactly what the prophecy means - and exactly what Ciri's power can do. This time Geralt may have met his match."});
            db.Books.Add(new Book { Name = "Mercenary of His Majesty", Author = "Vitaliy Zikov", Price = 190, ImageURL = @"https://fantasy-worlds.org/img/full/1/199.jpg", Details = "The winds of change continue to gain strength over the long-suffering Thorn. Legendary artifacts emerge from non-existence, rulers become toys in the hands of secret societies, and the powerful of this world are once again on the verge of a new Great War ... The last war in this world! And again the blades are ringing, the lands of the Thorn are shaking the battle of the sorcerers, and in the night the shadows of the hired killers slide silently. The struggle for life, freedom and happiness continues!"});


            base.Seed(db);
        }
    }
}