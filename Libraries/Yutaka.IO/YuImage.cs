using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Yutaka.IO
{
	public class YuImage
	{
		#region Fields
		const int PROPERTY_TAG_EXIF_DATE_TAKEN = 36867; // PropertyTagExifDTOrig //
		public DateTime CreationTime;
		public DateTime DateTaken;
		public DateTime LastAccessTime;
		public DateTime LastWriteTime;
		public DateTime MinDateTime;
		public DateTime MinDateTimeThreshold = new DateTime(1970, 1, 1); // based on Unix time //
		public string[] Top1000GirlNames = new string[] { "Alessandra", "Alexandria", "Clementine", "Evangeline", "Jacqueline", "Alejandra", "Alexandra", "Anastasia", "Annabella", "Annabelle", "Antonella", "Aubriella", "Aubrielle", "Brooklynn", "Brynleigh", "Cassandra", "Catherine", "Charleigh", "Charlotte", "Christina", "Christine", "Elisabeth", "Elizabeth", "Esmeralda", "Esperanza", "Everleigh", "Francesca", "Gabriella", "Gabrielle", "Genevieve", "Gracelynn", "Guadalupe", "Gwendolyn", "Josephine", "Katherine", "Lillianna", "Mackenzie", "Madeleine", "Paisleigh", "Priscilla", "Remington", "Scarlette", "Stephanie", "Valentina", "Addilynn", "Adelaide", "Adrianna", "Adrienne", "Angelica", "Angelina", "Annalise", "Arabella", "Beatrice", "Braelynn", "Brittany", "Brooklyn", "Calliope", "Carolina", "Caroline", "Cataleya", "Catalina", "Cheyenne", "Clarissa", "Coraline", "Daniella", "Danielle", "Ellianna", "Emmaline", "Emmalynn", "Emmeline", "Estrella", "Felicity", "Fernanda", "Florence", "Gabriela", "Giavanna", "Giovanna", "Giuliana", "Gracelyn", "Hadassah", "Hadleigh", "Harleigh", "Heavenly", "Isabella", "Isabelle", "Itzayana", "Izabella", "Jennifer", "Julianna", "Juliette", "Kataleya", "Katalina", "Kathleen", "Kaydence", "Kayleigh", "Khaleesi", "Kimberly", "Leighton", "Lilianna", "Lilliana", "Madalynn", "Maddison", "Madeline", "Madelynn", "Madilynn", "Magnolia", "Makenzie", "Malaysia", "Margaret", "Marianna", "Marleigh", "Mckenzie", "Mckinley", "Meredith", "Michaela", "Michelle", "Nathalie", "Patricia", "Penelope", "Princess", "Rosemary", "Samantha", "Savannah", "Scarlett", "Serenity", "Treasure", "Veronica", "Victoria", "Virginia", "Vivienne", "Yamileth", "Aaliyah", "Abigail", "Adaline", "Adalynn", "Addilyn", "Addison", "Addisyn", "Addyson", "Adelina", "Adeline", "Adelynn", "Adriana", "Ainsley", "Aislinn", "Alannah", "Alessia", "Alianna", "Alisson", "Allison", "Allyson", "Alondra", "Annabel", "Annalee", "Ariadne", "Arianna", "Ariella", "Arielle", "Arlette", "Ashlynn", "Aurelia", "Avalynn", "Avianna", "Azariah", "Barbara", "Bellamy", "Berkley", "Bethany", "Blakely", "Braelyn", "Braylee", "Brianna", "Bridget", "Briella", "Brielle", "Brinley", "Bristol", "Brynlee", "Cadence", "Cameron", "Camilla", "Camille", "Carolyn", "Cassidy", "Cecelia", "Cecilia", "Celeste", "Charlee", "Charley", "Charlie", "Chelsea", "Claudia", "Colette", "Collins", "Corinne", "Crystal", "Cynthia", "Daleyza", "Daniela", "Deborah", "Delaney", "Delilah", "Destiny", "Dorothy", "Eleanor", "Elianna", "Elliana", "Elliott", "Ellison", "Emberly", "Emerson", "Emersyn", "Emmalyn", "Emmarie", "Estella", "Estelle", "Evelynn", "Everlee", "Frances", "Frankie", "Galilea", "Genesis", "Georgia", "Giselle", "Haisley", "Harmoni", "Harmony", "Holland", "Isabela", "Jaelynn", "Jaliyah", "Janelle", "Janessa", "Janiyah", "Jasmine", "Jayleen", "Jaylene", "Jazlynn", "Jazmine", "Jenesis", "Jessica", "Jillian", "Jocelyn", "Johanna", "Joselyn", "Journee", "Journey", "Juliana", "Julieta", "Julissa", "Juniper", "Justice", "Kadence", "Kailani", "Kaitlyn", "Kaliyah", "Kamilah", "Kamiyah", "Kassidy", "Katelyn", "Kathryn", "Kaylani", "Kehlani", "Keilani", "Kendall", "Kennedi", "Kennedy", "Kensley", "Kinslee", "Kinsley", "Kyleigh", "Leilani", "Liberty", "Liliana", "Lillian", "Lilyana", "Lindsey", "Lorelai", "Lorelei", "Luciana", "Lucille", "Madalyn", "Madelyn", "Madilyn", "Madison", "Madisyn", "Makayla", "Makenna", "Maliyah", "Mallory", "Mariana", "Marilyn", "Marisol", "Marissa", "Matilda", "Mckenna", "Meilani", "Melanie", "Melissa", "Mikaela", "Mikayla", "Miracle", "Miranda", "Natalia", "Natalie", "Natasha", "Novalee", "Oaklynn", "Octavia", "Ophelia", "Paislee", "Paisley", "Paulina", "Phoenix", "Presley", "Promise", "Raelynn", "Rebecca", "Rebekah", "Rosalee", "Rosalie", "Rosalyn", "Roselyn", "Royalty", "Ryleigh", "Sabrina", "Saniyah", "Saoirse", "Sariyah", "Savanna", "Scarlet", "Taliyah", "Tatiana", "Tiffany", "Tinsley", "Trinity", "Valeria", "Valerie", "Vanessa", "Violeta", "Viviana", "Waverly", "Whitley", "Whitney", "Xiomara", "Yaretzi", "Zaniyah", "Zariyah", "Adalee", "Adalyn", "Adelyn", "Ailani", "Aileen", "Aitana", "Aiyana", "Alaina", "Alanna", "Alayah", "Alayna", "Aleena", "Alexia", "Alexis", "Aliana", "Alicia", "Alisha", "Alison", "Alivia", "Aliyah", "Alyson", "Alyssa", "Amalia", "Amanda", "Amaris", "Amayah", "Amelia", "Amelie", "Amirah", "Amiyah", "Analia", "Andrea", "Angela", "Aniyah", "Annika", "Ansley", "Ariana", "Ariyah", "Armani", "Ashley", "Ashlyn", "Astrid", "Athena", "Aubree", "Aubrey", "Aubrie", "Audrey", "August", "Aurora", "Austyn", "Autumn", "Avalyn", "Averie", "Aviana", "Ayleen", "Azalea", "Azaria", "Bailee", "Bailey", "Baylee", "Bexley", "Bianca", "Blaire", "Bonnie", "Briana", "Brooke", "Brylee", "Callie", "Camila", "Camryn", "Carmen", "Carter", "Celine", "Chanel", "Charli", "Claire", "Dahlia", "Dakota", "Dalary", "Dallas", "Daphne", "Davina", "Dayana", "Eileen", "Elaina", "Elaine", "Eliana", "Elliot", "Eloise", "Emelia", "Emerie", "Emilee", "Emilia", "Ensley", "Esther", "Evelyn", "Everly", "Fatima", "Finley", "Gianna", "Gloria", "Gracie", "Hadlee", "Hadley", "Hailee", "Hailey", "Hallie", "Hannah", "Harlee", "Harley", "Harlow", "Harper", "Hattie", "Hayden", "Haylee", "Hayley", "Heaven", "Helena", "Henley", "Hunter", "Iliana", "Ingrid", "Isabel", "Ivanna", "Jaelyn", "Jaycee", "Jayden", "Jaylah", "Jaylee", "Jazlyn", "Jazmin", "Jessie", "Jimena", "Joanna", "Joelle", "Jolene", "Jordan", "Jordyn", "Journi", "Judith", "Juliet", "Jurnee", "Kaelyn", "Kailey", "Kailyn", "Kalani", "Kallie", "Kamila", "Kamryn", "Karina", "Karlee", "Karsyn", "Karter", "Kaylee", "Kaylie", "Kaylin", "Kelsey", "Kendra", "Kenley", "Kenzie", "Kimber", "Kimora", "Kinley", "Kynlee", "Lailah", "Lainey", "Landry", "Laurel", "Lauren", "Lauryn", "Laylah", "Leanna", "Legacy", "Lennon", "Lennox", "Leslie", "Lilian", "Lilith", "Lillie", "London", "Londyn", "Louisa", "Louise", "Luella", "Lyanna", "Maggie", "Maisie", "Malani", "Malaya", "Maleah", "Maliah", "Margot", "Mariah", "Mariam", "Marina", "Marlee", "Marley", "Martha", "Maryam", "Maxine", "Meadow", "Meghan", "Melany", "Melina", "Melody", "Milana", "Milani", "Milena", "Millie", "Miriam", "Monica", "Monroe", "Morgan", "Nalani", "Nataly", "Nayeli", "Nevaeh", "Nicole", "Noelle", "Oaklee", "Oakley", "Oaklyn", "Olivia", "Palmer", "Paloma", "Parker", "Payton", "Peyton", "Phoebe", "Rachel", "Raegan", "Raelyn", "Ramona", "Raquel", "Reagan", "Regina", "Renata", "Romina", "Samara", "Samira", "Sandra", "Sariah", "Sawyer", "Saylor", "Selena", "Selene", "Serena", "Shelby", "Shiloh", "Sienna", "Sierra", "Simone", "Skylar", "Skyler", "Sloane", "Sophia", "Sophie", "Stella", "Stevie", "Summer", "Sutton", "Sydney", "Sylvia", "Sylvie", "Taylor", "Teagan", "Tenley", "Teresa", "Thalia", "Tinley", "Valery", "Vienna", "Violet", "Vivian", "Willow", "Winter", "Wynter", "Ximena", "Yareli", "Zainab", "Zariah", "Zaylee", "Zhavia", "Adele", "Adley", "Aisha", "Alaia", "Alana", "Alani", "Alaya", "Aleah", "Alena", "Alexa", "Alice", "Alina", "Aliya", "Aliza", "Allie", "Alora", "Amaia", "Amani", "Amara", "Amari", "Amaya", "Amber", "Amina", "Amira", "Amiya", "Amora", "Anahi", "Anais", "Anaya", "Angie", "Anika", "Aniya", "Annie", "April", "Arely", "Ariah", "Ariel", "Ariya", "Aspen", "Averi", "Avery", "Aylin", "Belen", "Bella", "Belle", "Blair", "Blake", "Briar", "Brynn", "Carly", "Casey", "Celia", "Chana", "Chaya", "Chloe", "Clare", "Daisy", "Danna", "Della", "Diana", "Dream", "Dulce", "Dylan", "Edith", "Egypt", "Elena", "Elina", "Elisa", "Elise", "Eliza", "Ellen", "Ellie", "Ellis", "Elora", "Elsie", "Elyse", "Emely", "Emery", "Emily", "Emmie", "Emory", "Erica", "Erika", "Faith", "Fiona", "Freya", "Frida", "Gemma", "Giana", "Grace", "Greta", "Haley", "Halle", "Hanna", "Haven", "Hazel", "Heidi", "Helen", "Holly", "Imani", "India", "Irene", "Itzel", "Ivory", "Jamie", "Jayda", "Jayde", "Jayla", "Jemma", "Jenna", "Jewel", "Jolie", "Josie", "Joyce", "Julia", "Julie", "Kairi", "Kaiya", "Karen", "Karla", "Katie", "Kayla", "Keily", "Keira", "Kelly", "Kenia", "Kenna", "Keyla", "Khloe", "Kiana", "Kiara", "Kiera", "Kylee", "Kylie", "Lacey", "Laila", "Laney", "Laura", "Layla", "Leila", "Leona", "Lexie", "Leyla", "Liana", "Lilah", "Lilly", "Linda", "Livia", "Lucia", "Lydia", "Lylah", "Lyric", "Mabel", "Macie", "Maeve", "Malia", "Maren", "Margo", "Maria", "Marie", "Mavis", "Megan", "Mercy", "Micah", "Milan", "Miley", "Molly", "Mylah", "Nadia", "Nancy", "Naomi", "Noemi", "Norah", "Novah", "Nylah", "Olive", "Paige", "Paola", "Paris", "Paula", "Pearl", "Penny", "Perla", "Piper", "Poppy", "Queen", "Quinn", "Raina", "Raven", "Rayna", "Rayne", "Reese", "Reign", "Reina", "Reyna", "Riley", "Rivka", "Robin", "Rosie", "Rowan", "Royal", "Ryann", "Rylan", "Rylee", "Rylie", "Sadie", "Saige", "Salma", "Sarah", "Sarai", "Sasha", "Scout", "Selah", "Siena", "Skyla", "Sloan", "Sofia", "Sunny", "Talia", "Tatum", "Tessa", "Tiana", "Wendy", "Willa", "Zahra", "Zaria", "Zelda", };
		public string[] Top1000GirlNamesFalsePositives = new string[] { "Angel", "Arden", "Clara", "Ember", "Logan", "River", "Abby", "Alia", "Alma", "Amia", "Andi", "Anna", "Anne", "Anya", "Aria", "Arya", "Avah", "Ayla", "Bria", "Cali", "Cara", "Cora", "Dana", "Dani", "Demi", "Dior", "Eden", "Ella", "Elle", "Elsa", "Emma", "Emmy", "Erin", "Esme", "Etta", "Evie", "Ezra", "Faye", "Gwen", "Hana", "Hope", "Iris", "Isla", "Jada", "Jade", "Jana", "Jane", "June", "Kaia", "Kali", "Kara", "Kate", "Kira", "Kora", "Kori", "Kyla", "Kyra", "Lana", "Lara", "Leah", "Leia", "Lena", "Lexi", "Lila", "Lily", "Lina", "Lisa", "Lola", "Lucy", "Luna", "Lyla", "Lyra", "Maci", "Macy", "Maia", "Mara", "Mary", "Maya", "Miah", "Mila", "Mina", "Mira", "Myah", "Myla", "Myra", "Nala", "Naya", "Nina", "Nola", "Noor", "Nora", "Nova", "Nyla", "Olga", "Opal", "Remi", "Remy", "Rhea", "Rory", "Rosa", "Rose", "Ruby", "Ruth", "Ryan", "Sage", "Sara", "Skye", "Thea", "Tori", "Vada", "Veda", "Vera", "Wren", "Yara", "Zara", "Zoey", "Zoie", "Zola", "Zora", "Zuri", "Ada", "Amy", "Ana", "Ann", "Ari", "Ava", "Aya", "Eva", "Eve", "Gia", "Ivy", "Joy", "Kai", "Lea", "Lia", "Liv", "Mae", "Mia", "Mya", "Nia", "Noa", "Sky", "Zoe", };
		public long Size;
		public string DirectoryName;
		public string Extension;
		public string FullName;
		public string Name;
		public string NameWithoutExtension;
		public string NewFilename;
		public string NewFolder;
		public string ParentFolder;
		#endregion Fields

		public YuImage(string filename)
		{
			var fi = new FileInfo(filename);
			fi.IsReadOnly = false;
			try {
				CreationTime = fi.CreationTime;
			}
			catch (Exception) {
				CreationTime = new DateTime();
			}
			LastAccessTime = fi.LastAccessTime;
			try {
				LastWriteTime = fi.LastWriteTime;
			}
			catch (Exception) {
				LastWriteTime = new DateTime();
			}
			Size = fi.Length;
			DirectoryName = fi.DirectoryName;
			Extension = fi.Extension;
			FullName = fi.FullName.Replace(Extension, Extension.ToLower());
			Name = fi.Name.Replace(Extension, Extension.ToLower());
			NameWithoutExtension = fi.Name.Replace(Extension, "");
			ParentFolder = fi.Directory.Name;
			fi = null;

			SetDateTaken();
			SetMinDateTime();
			SetNewFolderAndFilename();
		}

		// Retrieves the datetime WITHOUT loading the whole image //
		private void SetDateTaken()
		{
			var r = new Regex(":");

			try {
				using (var fs = new FileStream(FullName, FileMode.Open, FileAccess.Read)) {
					using (var img = Image.FromStream(fs, false, false)) {
						var propItem = img.GetPropertyItem(PROPERTY_TAG_EXIF_DATE_TAKEN);
						var dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
						DateTaken = DateTime.Parse(dateTaken);
					}
				}
			}

			catch (Exception) {
				DateTaken = new DateTime();
			}
		}

		private void SetMinDateTime()
		{
			MinDateTime = DateTime.Now;

			if (DateTaken != null && MinDateTimeThreshold < DateTaken && DateTaken < MinDateTime)
				MinDateTime = DateTaken; // prioritize DateTaken //

			else {
				if (CreationTime != null && MinDateTimeThreshold < CreationTime && CreationTime < MinDateTime)
					MinDateTime = CreationTime;

				if (LastWriteTime != null && MinDateTimeThreshold < LastWriteTime && LastWriteTime < MinDateTime)
					MinDateTime = LastWriteTime;
			}
		}

		private void SetNewFolderAndFilename()
		{
			var fullnameUpper = FullName.ToUpper();

			#region Case: 4 or more characters
			var specialFolders1 = new string[,] {
				// search term,			new folder name //
				{ @"MICHAEL CONTURSI", @"Michael Contursi\", },
				{ @"CONSUMER REPORT", @"Magazines\Consumer Reports\", },
				{ @"UNITED AIRLINES", @"yyyy\United Airlines\", },
				{ @"CLASH ROYALE", @"Games\Clash Royale\", },
				{ @"CONFIRMATION", @"Receipts\", },
				{ @"VIDEO PLAYER", @"z\Video Player\", },
				{ @"DRAGONFRUIT", @"z\DragonFruit\", },
				{ @"GROOMING", @"Grooming\", },
				{ @"WOMENS HEALTH", @"Magazines\Womens Health\", },
				{ @"MENS HEALTH", @"Magazines\Mens Health\", },
				{ @"PHILIPS HUE", @"Philips Hue\", },
				{ @"MAXIMUM PC", @"Magazines\Maximum PC\", },
				{ @"APARTMENT", @"Apartment\", },
				{ @"INSTAGRAM", @"z\Instagram\", },
				{ @"MAXIMUMPC", @"Magazines\Maximum PC\", },
				{ @"STEAMGIRL", @"z\Steamgirl\PARENTFOLDER\", },
				{ @"THANK YOU", @"Receipts\", },
				{ @"CHECKOUT", @"Receipts\", },
				{ @"ITINERAR", @"Itineraries\", },
				{ @"ME\USING", @"zMe\Using\", },
				{ @"MESSAGES", @"yyyy\Messsages\", },
				{ @"PC GAMER", @"Magazines\PC Gamer\", },
				{ @"SNAPCHAT", @"z\Snapchat\", },
				{ @"UNSPLASH", @"Unsplash\", },
				{ @"INVOICE", @"Invoices\", },
				{ @"ME\TEST", @"zMe\Test\", },
				{ @"SAMSUNG", @"yyyy\Samsung\", },
				{ @"POSE", @"Poses\", },
				{ @"OKCUPID", @"z\OkCupid\", },
				{ @"RECEIPT", @"Receipts\", },
				{ @"WELCOME", @"Receipts\", },
				// Less than 7 characters //
				{ @"BUMBLE", @"z\Bumble\", },
				{ @"CHROME", @"yyyy\Chrome\", },
				{ @"NANAMI", @"Nanami\", },
				{ @"TATTOO", @"Tattoos\", },
				{ @"THANKS", @"Receipts\", },
				{ @"TIKTOK", @"z\TikTok\", },
				{ @"TINDER", @"z\Tinder\", },
				{ @"BIXBY", @"yyyy\Bixby\", },
				{ @"CHASE", @"yyyy\Chase\", },
				{ @"DELTA", @"yyyy\Delta\", },
				{ @"GMAIL", @"yyyy\Gmail\", },
				{ @"HAPPN", @"z\Happn\", },
				{ @"MAXIM", @"Magazines\Maxim\", },
				{ @"SHIRT", @"Shirts\", },
				{ @"SLEEP", @"yyyy\Sleep\", },
				{ @"ETNT", @"Magazines\ETNT\", },
				{ @"GAME", @"Games\", },
				{ @"IKEA", @"Ikea\", },
				{ @"LINE", @"yyyy\Line\", },
				{ @"MAPS", @"yyyy\Maps\", },
				{ @"TURO", @"yyyy\Turo\", },
				{ @"ZME", @"zMe\", },
				// leave screenshots last //
				{ @"SCREENSHOT", @"yyyy\Screenshots\", },
			};

			for (int i = 0; i < specialFolders1.Length/2; i++) {
				if (fullnameUpper.Contains(specialFolders1[i,0])) {
					if (specialFolders1[i, 1].StartsWith(@"yyyy\"))
						NewFolder = specialFolders1[i, 1].Replace("yyyy", MinDateTime.ToString("yyyy"));
					else if (specialFolders1[i, 1].Contains("PARENTFOLDER"))
						NewFolder = specialFolders1[i, 1].Replace("PARENTFOLDER", ParentFolder);
					else
						NewFolder = specialFolders1[i, 1];

					NewFilename = Name;
					return; // only match one, then return //
				}
			}
			#endregion Case: 4 or more characters

			#region Case: Top 1000 Girl Names
			for (int i = 0; i < Top1000GirlNames.Length; i++) {
				if (fullnameUpper.Contains(Top1000GirlNames[i].ToUpper())) {
					NewFolder = String.Format(@"z\{0}\", Top1000GirlNames[i]);
					NewFilename = Name;
					return; // only match one, then return //
				}
			}

			string girlName;
			var directoryNameUpper = DirectoryName.ToUpper();
			var nameWithoutExtensionUpper = NameWithoutExtension.ToUpper();
			for (int i = 0; i < Top1000GirlNamesFalsePositives.Length; i++) {
				girlName = Top1000GirlNamesFalsePositives[i];
				if (directoryNameUpper.Contains(String.Format(@"\{0}\", girlName.ToUpper())) || Regex.IsMatch(NameWithoutExtension, String.Format(@"\b{0}\b", girlName), RegexOptions.IgnoreCase)) {
					NewFolder = String.Format(@"z\{0}\", girlName);
					NewFilename = Name;
					return; // only match one, then return //
				}
			}
			#endregion Case: Top 1000 Girl Names

			#region Case: Less than 4 characters
			// Order these by string length, descending //
			var specialFolders2 = new string[,] {
				// search term, new folder name, new filename // null or empty filename will keep the original name (won't rename it) //
				{ "GQ", @"Magazines\GQ\", },
				{ "Me", @"zMe\", },
			};

			for (int i = 0; i < specialFolders2.Length / 2; i++) {
				if (FullName.Contains(String.Format(@"\{0}\", specialFolders2[i, 0])) || Name.StartsWith(String.Format("{0} ", specialFolders2[i,0]))) {
					if (specialFolders2[i, 1].StartsWith(@"yyyy\"))
						NewFolder = specialFolders2[i, 1].Replace("yyyy", MinDateTime.ToString("yyyy"));
					else
						NewFolder = specialFolders2[i, 1];

					NewFilename = Name;
					return; // only match one, then return //
				}
			}
			#endregion Case: Less than 4 characters

			#region Default: Everything else
			int year;
			var parentFolderUppoer = ParentFolder.ToUpper();
			var ignoreList = new List<string> { "CAMERA", "XPERIA TL", "CAMERA ROLL", "IMAGES", "PICTURES", "_UNPROCESSED", "_PROCESS THESE", "TEST", "_TEST", };
			if (ignoreList.Exists(x => parentFolderUppoer.Equals(x)) || int.TryParse(ParentFolder, out year))
				NewFolder = String.Format(@"{0:yyyy}\", MinDateTime);
			else
				NewFolder = String.Format(@"{0:yyyy}\{1}\", MinDateTime, ParentFolder);

			NewFilename = Name;
			#endregion Default: Everything else
		}
	}
}