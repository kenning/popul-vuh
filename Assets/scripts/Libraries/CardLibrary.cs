using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CardLibrary : MonoBehaviour {

	public static Dictionary<string, LibraryCard> Lib;
	public List<LibraryCard> StartingItems;

    //This method is called in GameControl.BeginGame() unless the tutorial is on.
	public void SetStartingItems () {
		StartingItems = new List<LibraryCard> ();
	
		for(int i = 0; i < SaveData.StartingDeckCards.Count; i++) {
			StartingItems.Add(SaveData.StartingDeckCards[i]);
		}

//		LibraryCard[] allCards = Lib.Values.ToArray();
//		for(int i = 0; i < Lib.Count; i++) {
//			StartingItems.Add(allCards[i]);
//		}
	}

    //This method is called in GameControl.Awake() along with a lot of other stuff.
    //It's got all the cards in it.
	public void Startup() {
		Lib = new Dictionary<string, LibraryCard> ();
		//Ikka
			//Start unlocked
		Lib.Add ("Cloth Shoes", new LibraryCard ("Cloth Shoes",
		                                         "Cloth \nShoes", 
		                      		             "leather-boot",
		                                            ShopControl.Gods.Ikka,
		                                            Card.Rarity.Paper,
		                                            "To play, tap the card again",
		                                            "Gain 2 moves this turn",
		                                            "Move +2",
		                                            Card.CardActionTypes.NoTarget));
		Lib.Add ("Spin Kick", new LibraryCard ("Spin Kick", 
		                                       "quake-stomp",
		                                       ShopControl.Gods.Ikka, 
		                                       Card.Rarity.Bronze, 
		                                       "To play, tap the card again", 
		                                       "Deals 1 damage to all adjacent squares",
		                                       "AOE around \nplayer", 
		                                       Card.CardActionTypes.NoTargetGridSquare, 
		                                       GridControl.TargetTypes.none, 0,0, GridControl.TargetTypes.square, 1, 1));
		Lib.Add("Speed Burst", new LibraryCard("Speed Burst",
		                                       "Speed \nBurst", 
		                      		             "dodging",
		                                         ShopControl.Gods.Ikka,
		                                         Card.Rarity.Silver,
		                                         "To play, tap the card again",
		                                         "Gain 3 moves and 1 play.",
		                                         "+3 moves\n+1 play",
		                                         Card.CardActionTypes.NoTarget));
		Lib.Add("Fencing Stance", new LibraryCard("Fencing Stance",
		                                          "Fencing \nStance",
		                                          "fist",
		                                          ShopControl.Gods.Ikka,
		                                          Card.Rarity.Gold,
		                                          "To play, tap the card again",
		                                          "Gain 2 moves and 1 play, and another 2 moves every time you punch an enemy.",
		                                          "Move +2 Play +1\nTrigger:\nPunch enemy:\nMove +2",
		                                          Card.CardActionTypes.NoTarget));
		Lib.Add("Ninja Flipout", new LibraryCard("Ninja Flipout",
		                                         "Ninja \nFlipout",
		                                         "x",
		                                         ShopControl.Gods.Ikka,
		                                         Card.Rarity.Silver,
		                                         "To play, tap the card again",
		                                         "Deal 1 damage in a range of X around you, where X is the number of cards " + 
		                                         "in your hand to the right of this, max 3.",
		                                         "Deal 1 damage \nin AOE = # cards \nto the right",
		                                         Card.CardActionTypes.NoTargetGridSquare, 
		                                         GridControl.TargetTypes.none, 0,0, GridControl.TargetTypes.diamond, 1, 1));
			//Unlockable
		Lib.Add("Boomerang", new LibraryCard("Boomerang", 
		                                     "boomerang",
		                                     ShopControl.Gods.Ikka,
		                                     Card.Rarity.Gold,
		                                     "To play, select a square",
		                                     "Deals 2 damage at a range. Reusable.",
		                                     "Ranged\n1 damage\nReusable",
		                                     Card.CardActionTypes.TargetGridSquare,
		                                     GridControl.TargetTypes.diamond, 2, GridControl.TargetTypes.none, 0));
        Lib.Add("Nunchucks", new LibraryCard("Nunchucks",
		                                     "bolas",
		                                     ShopControl.Gods.Ikka,
		                                     Card.Rarity.Bronze,
		                                     "To play, select a square",
		                                     "Deals 1 damage. If you have another copy of this card in your hand, gain 1 play.",
		                                     "1 damage\nThis in hand =>\n+1 play",
		                                     Card.CardActionTypes.TargetGridSquare,
		                                     GridControl.TargetTypes.square, 1,1, GridControl.TargetTypes.none, 0, 0));
		Lib.Add ("Spymaster Style", new LibraryCard ("Spymaster Style",
		                                            "Spymaster \nStyle",
		                                            "spyglass",
		                                            ShopControl.Gods.Ikka,
		                                            Card.Rarity.Silver,
		                                            "To play, tap the card again",
		                                            "Until the end of the level, draw a card each time you damage an enemy.",
		                                           "Trigger:\nDamage enemy:\nDraw 1",
		                                           Card.CardActionTypes.NoTarget));
		Lib.Add ("Tiger Stance", new LibraryCard ("Tiger Stance",
		                                        "Tiger \nStance",
		                                        "triple-claws",
		                                        ShopControl.Gods.Ikka,
		                                        Card.Rarity.Silver,
		                                        "To play, tap the card again",
		                                        "Gain 1 move and 1 play, and another 1 move and 1 play each time you kill an enemy.",
		                                        "Move +1 Play +1 \nTrigger:\nKill enemy:\nMove +1 Play +1",
		                                          Card.CardActionTypes.NoTarget));
		//Ekcha 
			//Start unlocked
		Lib.Add("Quick Prayer", new LibraryCard("Quick Prayer",
		                                        "Quick \nPrayer", 
		                      		             "glowing-hands",
		                                          ShopControl.Gods.Ekcha,
		                                          Card.Rarity.Paper,
		                                          "To play, tap the card again",
		                                          "Gain $1 favor with the Gods",
		                                          "+$1",
		                                          Card.CardActionTypes.NoTarget));
		Lib.Add ("Prayer Bread", new LibraryCard ("Prayer Bread",
		                                          "Prayer \nBread",
		                                		   "food/sliced-bread",
		                                          ShopControl.Gods.Ekcha,
		                                          Card.Rarity.Bronze,
		                                          "To play, tap the card again",
		                                          "Gain $1 favor with the Gods and draw 1 card",
		                                          "+$1\nDraw 1",
		                                          Card.CardActionTypes.NoTarget));
		Lib.Add("Deep Prayer", new LibraryCard("Deep Prayer",
		                                       "Deep \nPrayer", 
		                      		             "glowing-hands",
		                                         ShopControl.Gods.Ekcha,
		                                         Card.Rarity.Silver,
		                                         "To play, tap the card again",
		                                         "Gain $2 favor with the Gods",
		                                         "+$2", 
		                                         Card.CardActionTypes.NoTarget));
		Lib.Add("Ritual Pyre", new LibraryCard("Ritual Pyre",
		                                       "Ritual \nPyre", 
		                                       "magic-swirl",
		                                       ShopControl.Gods.Ekcha,
		                                       Card.Rarity.Gold,
		                                       "To play, tap the card again",
		                                       "Burn 1 card in your hand to gain $3 favor with the Gods",
		                                       "Burn 1 in hand: \n+$3",
		                                       Card.CardActionTypes.TargetCard));
		//Unlockable
		Lib.Add ("Book Of History", new LibraryCard ("Book Of History",
		                                             "Book Of \nHistory",
		                                             "black-book",
		                                             ShopControl.Gods.Ekcha,
		                                             Card.Rarity.Gold,
		                                             "To play, tap the card again",
		                                             "Pick another card from your hand to play twice.",
		                                             "Play another \ncard twice",
		                                             Card.CardActionTypes.TargetCard));
//		Lib.Add ("Computer", new LibraryCard ("Computer",
//		                                    "microchip",
//		                                    ShopControl.Gods.Ekcha,
//		                                    Card.Rarity.Silver,
//		                                    "To play, tap the card again",
//		                                    "Gain £1 Unlock bucks",
//		                                    "+£2 Unlock",
//		                                    Card.CardActionTypes.NoTarget));
		Lib.Add ("Desperate Prayer", new LibraryCard ("Desperate Prayer",
		                                            "Desperate \nPrayer",
		                                            "usable",
		                                            ShopControl.Gods.Ekcha,
		                                            Card.Rarity.Silver,
		                                            "To play, tap the card again",
		                                            "Gain $1, or gain $5 if you have $0",
		                                            "If $0 => \n+$5\n else +$1",
		                                            Card.CardActionTypes.NoTarget));
		Lib.Add ("Prayer Beads", new LibraryCard ("Prayer Beads", 
		                                          "Prayer \nBeads", 
		                                          "wavy-chains",
		                                          ShopControl.Gods.Ekcha,
		                                          Card.Rarity.Bronze,
		                                          "To play, tap the card again",
		                                          "Gain 1 play. Every time you burn an item this turn, gain $3",
		                                          "+1 Play\nTrigger: \nBurn: +$3",
		                                          Card.CardActionTypes.NoTarget));
		//Ixchel
			//Start unlocked
		Lib.Add("Cloth Shirt", (new LibraryCard("Cloth Shirt",
		                                        "Cloth \nShirt",
		                      		             "leather-vest",
		                                          ShopControl.Gods.Ixchel,
		                                          Card.Rarity.Paper,
		                                          "This card will discard itself to prevent damage done to you",
		                                          "Protect 1 Range 1: This card will protect you from 1 damage done at a maximum of 1 range",
		                                          "Protect 1\nRange 1",
		                                          Card.CardActionTypes.Armor,
		                                          GridControl.TargetTypes.diamond, 1, GridControl.TargetTypes.none, 0)));
		Lib.Add ("Cloth Armor", (new LibraryCard ("Cloth Armor",
		                                        "Cloth \nArmor",
		                                        "robe",
		                                        ShopControl.Gods.Ixchel,
		                                        Card.Rarity.Silver,
		                                        "This card will discard itself to prevent damage done to you",
		                                        "Protect 1 Range Any: This card will protect you from 1 damage at any range",
		                                        "Protect 1\nRange Any",
		                                        Card.CardActionTypes.Armor)));
		Lib.Add ("Iron Shirt", new LibraryCard ("Iron Shirt",
		                                        "Iron \nShirt", 
		                      		             "chain-mail",
		                                          ShopControl.Gods.Ixchel,
		                                          Card.Rarity.Bronze,
		                                          "This card will discard itself to prevent damage done to you",
		                                     	  "Protect 2 Range 1: This card will protect you from 2 damage done at a maximum of 1 range",
		                                          "Protect 2\nRange 1",
		                                          Card.CardActionTypes.Armor,
		                                          GridControl.TargetTypes.diamond, 1, GridControl.TargetTypes.none, 0));
		Lib.Add("Wood Shield", (new LibraryCard("Wood Shield",
		                                        "Wood \nShield", 
		                      		             "round-shield",
		                                          ShopControl.Gods.Ixchel,
		                                          Card.Rarity.Bronze,
		                                          "This card will discard itself to prevent damage done to you",
		                                          "Protect 1 Range 2-3: This card will protect you from 1 damage from 2-3 squares away",
		                                          "Protect 1 \nRange 2-3",
		                                          Card.CardActionTypes.Armor,
		                                          GridControl.TargetTypes.diamond, 2, 3, GridControl.TargetTypes.none, 0, 0)));
		Lib.Add("Spiked Shield", (new LibraryCard("Spiked Shield",
		                                          "Spiked \nShield",
		                                          "skull-shield",
		                                          ShopControl.Gods.Ixchel,
		                                          Card.Rarity.Silver,
		                                          "To play, select a square",
		                                          "2 damage range 1 or \nProtect 1 Range 2-3: \nYou can use this card to deal damage, but it will also " +
		                                          "prevent 1 damage from 2-3 squares away.",
		                                          "1 Damage Range 1\nOr Protect 1 Range 2-3",
		                                          Card.CardActionTypes.TargetGridSquare,
		                                          GridControl.TargetTypes.diamond, 1, GridControl.TargetTypes.none, 0)));
		Lib.Add("Iron Shield", (new LibraryCard("Iron Shield",
		                                        "Iron \nShield",
		                      		             "edged-shield",
		                                          ShopControl.Gods.Ixchel,
		                                          Card.Rarity.Gold,
		                                          "This card will discard itself to prevent damage done to you",
		                                          "Protect 2 Range 2-3: This card will protect you from 2 damage from 2-3 squares away",
		                                          "Protect 2 \nRange 2-3",
		                                          Card.CardActionTypes.Armor,
		                                          GridControl.TargetTypes.diamond, 2, 3, GridControl.TargetTypes.none, 0, 0)));
		//Buluc	
			//Start unlocked
		Lib.Add("Wooden Pike", (new LibraryCard("Wooden Pike",
		                                        "Wooden \nPike", 
		                                        "stone-spear",
		                                        ShopControl.Gods.Buluc, 
		                                        Card.Rarity.Paper, 
		                                        "To play, select a square", 
		                                        "Deals 1 damage to a target", 
		                                        "Cross, \n1 damage", 
		                                        Card.CardActionTypes.TargetGridSquare, 
		                                        GridControl.TargetTypes.cross, 2, GridControl.TargetTypes.none, 0)));
		Lib.Add("Iron Macana", (new LibraryCard("Iron Macana",
		                                       "Iron \nMacana", 
		                      		             "macana",
		                                         ShopControl.Gods.Buluc, 
		                                         Card.Rarity.Paper, 
		                                         "To play, select a square", 
		                                         "Deals 2 damage", 
		                                         "Melee, \n2 damage", 
		                                         Card.CardActionTypes.TargetGridSquare, 
		                                       GridControl.TargetTypes.diamond, 1, GridControl.TargetTypes.none, 0)));
		Lib.Add("Iron Pike", (new LibraryCard("Iron Pike", 
		                                      "barbed-spear",
		                                      ShopControl.Gods.Buluc, 
		                                      Card.Rarity.Bronze, 
		                                      "To play, select a square", 
		                                      "Deals 2 damage to a target", 
		                                      "Cross, \n2 damage", 
		                                      Card.CardActionTypes.TargetGridSquare, 
		                                      GridControl.TargetTypes.cross, 2, GridControl.TargetTypes.none, 0)));
		Lib.Add ("Magic Missile", new LibraryCard ("Magic Missile",
		                                           "Magic \nMissile", 
		                      		             "plasma-bolt",
		                                             ShopControl.Gods.Buluc,
		                                             Card.Rarity.Silver,
		                                             "To play, select a square",
		                                             "Deals 1 damage. Range 3",
		                                             "Range 3, \n1 damage",
		                                             Card.CardActionTypes.TargetGridSquare,
		                                             GridControl.TargetTypes.diamond, 3, GridControl.TargetTypes.diamond, 0));
		Lib.Add("Fireball", new LibraryCard("Fireball", 
		                                    "burning-meteor",
		                                    ShopControl.Gods.Buluc, 
		                                    Card.Rarity.Gold, 
		                                    "To play, select a square (A.O.E. 1)", 
		                                    "Deals 1 damage at a range in a small area", 
		                                    "Ranged, \nA.O.E. 1, \n1 damage", 
		                                    Card.CardActionTypes.TargetGridSquare, 
		                                    GridControl.TargetTypes.diamond, 2, GridControl.TargetTypes.diamond, 1));
			//Unlockable
		Lib.Add("Shuriken", new LibraryCard ("Shuriken",
		                      		         "shuriken",
		                                     ShopControl.Gods.Buluc,
		                                     Card.Rarity.Bronze,
		                                     "To play, select a square",
		                                     "Discard a card.\nDeals X damage at X range.\nIf the discarded card is paper, x = 1.\n" +
		                                     "If Bronze, x = 2. If Silver, x=3.\nIf gold, x=4.",
		                                     "Discard 1:\nRarity damage at\nRarity range",
		                                     Card.CardActionTypes.TargetCard));
		//Chac
			//Start out unlocked
		Lib.Add("Apple",  (new LibraryCard ("Apple",
		                                   "food/shiny-apple",
		                                    ShopControl.Gods.Chac,
		                                    Card.Rarity.Paper,
		                                    "To play, tap the card again", 
		                                    "Draw 1 card",
		                                    "Draw 1",
		                                    Card.CardActionTypes.NoTarget)));
		Lib.Add("Coffee",  (new LibraryCard ("Coffee",
		                                     "food/coffee-mug",
		                                     ShopControl.Gods.Chac,
		                                     Card.Rarity.Paper,
		                                     "To play, tap the card again", 
		                                     "Gain 2 plays, allowing you to play 2 more cards.",
		                                     "+2 plays",
		                                     Card.CardActionTypes.NoTarget)));
//		Lib.Add ("Bread Bowl", new LibraryCard ("Bread Bowl",
//		                                        "Bread \nBowl",
//		                                        "food/sliced-bread",
//		                                        ShopControl.Gods.Chac,
//		                                        Card.Rarity.Bronze,
//		                                        "To play, tap the card again",
//		                                        "Peek at 3 cards from your deck and pick one to put in your hand.",
//		                                        "Peek 3, \nDraw 1 of them",
//		                                        Card.CardActionTypes.TargetCard));
		Lib.Add("Bag Of Bread", new LibraryCard("Bag Of Bread",
		                                        "Bag Of \nBread",
		                                 		  "food/burn",
		                                          ShopControl.Gods.Chac,
		                                          Card.Rarity.Silver,
		                                          "To play, tap the card again",
		                                          "Draw 1. This card is reusable -- it is not discarded when you play it.",
		                                          "Draw 1. \nReusable",
		                                          Card.CardActionTypes.NoTarget));
		Lib.Add("Energy Drink", new LibraryCard("Energy Drink",
		                                        "Energy \nDrink",
		                                        "pouring-chalice",
		                                        ShopControl.Gods.Chac,
		                                        Card.Rarity.Gold,
		                                        "To play, tap the card again",
		                                        "+3 play, Draw 1.",
		                                        "+3 plays, \nDraw 1.",
		                                        Card.CardActionTypes.NoTarget));
			//Unlockable
        //Lib.Add("Banana", new LibraryCard("Banana",
        //                                   "food/carrot",
        //                                  ShopControl.Gods.Chac,
        //                                  Card.Rarity.Bronze,
        //                                  "To play, select a square",
        //                                  "Draw 1, then throw the peel at an enemy, stunning it",
        //                                  "Draw 1,\nstun 1 enemy",
        //                                  Card.CardActionTypes.TargetGridSquare,
        //                                  GridControl.TargetTypes.diamond, 3, GridControl.TargetTypes.none, 0));
		Lib.Add("Corn Dog", new LibraryCard("Corn Dog",
		                                   "food/chicken-leg",
		                                    ShopControl.Gods.Chac,
		                                    Card.Rarity.Bronze,
		                                    "To play, tap the card again",
		                                    "Draw 2, then discard 2.\n+1 play",
		                                    "Draw 2,\ndiscard 2.\n+1 play",
		                                    Card.CardActionTypes.TargetCard));
		Lib.Add("Donut", new LibraryCard("Donut",
		                                   "food/wheat",
		                                 ShopControl.Gods.Chac,
		                                 Card.Rarity.Silver,
		                                 "To play, tap the card again",
		                                 "Draw 1, then discard 1. \n+1 play, +2 Move.",
		                                 "Draw 1,\ndiscard 1.\n+1 play, \n+2 Move",
		                                 Card.CardActionTypes.TargetCard));
//		Lib.Add("Donald's X-Cappuccino", new LibraryCard("Donald's X-Cappuccino",
//		                                                 "Donald's X-\nCappuccino",
//		                                                 "food/coffee-mug",
//		                                                 ShopControl.Gods.Chac,
//		                                                 Card.Rarity.Gold,
//		                                                 "To play, tap the card again",
//		                                                 "+2 plays, or you may burn this card to tuck everything back into your deck and draw 5.",
//		                                                 "+2 plays \nOr tuck all \nand draw 5",
//		                                                 Card.CardActionTypes.Options));
		Lib.Add ("Pizza", new LibraryCard ("Pizza",
		                                   "food/pizza-cutter",
		                                 ShopControl.Gods.Chac,
		                                 Card.Rarity.Bronze,
		                                 "To play, tap the card again",
		                                 "+2 plays. If you have no Chac cards in hand, +2 cards.",
		                                 "+2 plays\nNo Chac in hand =>\n  +2 cards",
		                                 Card.CardActionTypes.NoTarget));
		Lib.Add("Fruit Smoothie", new LibraryCard("Fruit Smoothie",
		                                          "Fruit \nSmoothie",
		                               			    "glass-shot",
		                                            ShopControl.Gods.Chac,
		                                            Card.Rarity.Silver,
		                                            "To play, tap the card again",
		                                            "Draw 3 cards, then shuffle two of them back into your deck.",
		                                            "Draw 3, \nTuck 2",
		                                            Card.CardActionTypes.TargetCard));
		Lib.Add ("Goulash", new LibraryCard ("Goulash",
		                                   "cauldron",
		                                   ShopControl.Gods.Chac,
		                                   Card.Rarity.Silver,
		                                   "To play, tap the card again",
		                                   "Discard a card. If it is paper, draw 3 cards and 1 play. Otherwise draw 1 card and 3 plays.",
		                                   "Discard 1:\n  Paper => \n    Draw 3, +1 play\n  Other => \n    Draw 1, +3 plays",
		                                     Card.CardActionTypes.TargetCard));
		Lib.Add("Shepherd's Pie", new LibraryCard("Shepherd's Pie",
		                                          "Shepherd's \nPie",
		                                			"food/pie-slice",
		                                            ShopControl.Gods.Chac,
		                                            Card.Rarity.Bronze,
		                                            "To play, tap the card again",
		                                            "+2 cards",
		                                            "+2 cards",
		                                            Card.CardActionTypes.NoTarget));
		Lib.Add("Soda", new LibraryCard("Soda",
		                                "Soda",
		                                "boiling-bubbles",
		                                ShopControl.Gods.Chac,
		                                Card.Rarity.Bronze,
		                                "To play, tap the card again",
		                                "Discard a card, then draw a card, and gain 1 play and 1 move.",
		                                "Discard 1:\nDraw 1\n+1 play \n+1 Move",
		                                Card.CardActionTypes.TargetCard));
		Lib.Add ("Soda Machine", new LibraryCard ("Soda Machine",
		                                          "Soda \nMachine",
		                                          "chemical-tank",
		                                          ShopControl.Gods.Chac,
		                                          Card.Rarity.Silver,
		                                          "To play, tap the card again",
		                                          "Gain a card 'Soda Cup,' which reads: 'Draw 2 and gain 2 plays, then burn this,' into your deck.",
		                                          "Gain Soda Cup",
		                                          Card.CardActionTypes.NoTarget));
		Lib.Add ("Soup Or Salad", new LibraryCard ("Soup Or Salad",
		                                           "Soup Or\nSalad",
		                                           "cauldron",
		                                           ShopControl.Gods.Chac,
		                                         Card.Rarity.Silver,
		                                         "To play, tap the card again",
		                                         "Either draw 2 cards or gain 2 plays.",
		                                         "+2 cards or\n+2 plays",
		                                         Card.CardActionTypes.Options));
		// Peeking is currently bugged
//		Lib.Add ("Pinata", new LibraryCard ("Pinata", 
//		                		             "spiked-mace",
//		                                   ShopControl.Gods.Chac,
//		                                   Card.Rarity.Silver,
//		                                   "To play, tap the card again",
//		                                   "Peek at the top two cards of your library. For each card that is paper, Bronze, silver or gold, " +
//		                                    	"gain \n   +1$, \n   +2 cards, \n   +2 plays or \n   +$2, \nrespectively.",
//		                                   "Peek 2\n  Paper => +$1\n  Bronze => +2 cards\n  Silver => +2 plays\n  Gold => +$2",
//		                                   Card.CardActionTypes.NoTargetNoInput));
		//Kinich
			//Start unlocked
		Lib.Add("Book Of Matches", new LibraryCard("Book Of Matches",
		                                           "Book Of \nMatches",
		                      		             "match-head",
		                                             ShopControl.Gods.Kinich,
		                                             Card.Rarity.Bronze,
		                                             "To play, tap the card again",
		                                             "Burn 1 card in your hand, permanently removing it from your deck.",
		                                             "Burn 1 in hand",
		                                             Card.CardActionTypes.TargetCard));
		Lib.Add ("Burnt Coffee", new LibraryCard ("Burnt Coffee",
		                                          "Burnt \nCoffee", 
		                               		    "food/coffee-mug",
		                                          ShopControl.Gods.Kinich,
		                                          Card.Rarity.Silver,
		                                          "to play, tap the card again",
		                                          "Burn 1 card in your hand to gain 2 plays",
		                                          "Burn 1 in hand: \n+2 plays",
		                                          Card.CardActionTypes.TargetCard));
			//Unlockable
		Lib.Add ("Birthday Cake", new LibraryCard ("Birthday Cake", 
		                                         "Birthday \nCake",
		                                         "food/cake-slice",
		                                         ShopControl.Gods.Kinich,
		                                         Card.Rarity.Bronze,
		                                         "To play, tap the card again",
		                                         "Draw 3. If you burn this card, gain $10",
		                                         "Draw 3\nIf burn => \n+$10",
		                                         Card.CardActionTypes.NoTarget));
		Lib.Add ("Charcoal Shoes", new LibraryCard ("Charcoal Shoes",
		                                            "Charcoal \nShoes", 
		                                            "leather-boot",
		                                            ShopControl.Gods.Kinich,
		                                            Card.Rarity.Bronze,
		                                            "To play, tap the card again", 
		                                            "Burn 1 card in your hand to gain 1 move",
		                                            "Burn 1 in hand: \n+1 Move",
		                                            Card.CardActionTypes.TargetCard));
		Lib.Add ("Cheese", new LibraryCard ("Cheese",
		                                  "food/cheese-wedge",
		                                  ShopControl.Gods.Kinich,
		                                  Card.Rarity.Bronze,
		                                  "To play, tap the card again",
		                                  "Burn a card in your hand. Get another copy of Cheese in your deck. \nIf you burn this card, draw 1 and gain 1 play.",
		                                  "Burn 1: \nGet another Cheese \nIf burn => \nDraw 1, +1 Play",
		                                  Card.CardActionTypes.TargetCard));
		Lib.Add ("Chocolate Surprise Egg", new LibraryCard ("Chocolate Surprise Egg",
		                                                   "Chocolate \nSurprise Egg",
		                                                   "food/big-egg",
		                                                   ShopControl.Gods.Kinich,
		                                                   Card.Rarity.Bronze,
		                                                   "To play, tap the card again",
		                                                   "Draw 1, +1 Play, +$1. If you burn this card, you get a new card called Phoenix Fireball.",
		                                                   "Draw 1, +1 Play, +$1\nIf burn => \n+Phoenix Fireball",
		                                                   Card.CardActionTypes.NoTarget));
		Lib.Add("Flamethrower", new LibraryCard("Flamethrower",
		                                        "Flame\nthrower",
		                                        "goo-spurt",
		                                        ShopControl.Gods.Kinich,
		                                        Card.Rarity.Gold,
		                                        "To play, tap the card again",
		                                        "Burn a card in your hand to deal 1 damage at a range in a small area", 
		                                        "Burn 1 in hand: \nRanged, \nA.O.E. 1,\n1 damage, ",
		                                        Card.CardActionTypes.TargetCard,
		                                        GridControl.TargetTypes.diamond, 3, GridControl.TargetTypes.diamond, 1));
        Lib.Add("Well Done Steak", new LibraryCard("Well Done Steak", 
		                                           "Well Done \nSteak",
		                                                 "food/meat",
		                                             ShopControl.Gods.Kinich,
		                                             Card.Rarity.Silver,
		                                             "To play, tap the card again",
		                                             "Burn a card in your hand to draw 2 and gain 1 play",
		                                             "Burn 1 in hand: \nDraw 2, \n+1 Play",
		                                             Card.CardActionTypes.TargetCard));
		Lib.Add ("Firebomb", new LibraryCard ("Firebomb",
		                                    "fire-bomb",
		                                    ShopControl.Gods.Kinich,
		                                    Card.Rarity.Silver,
		                                    "To play, select a square",
		                                    "This card does 2 damage in A.O.E. 2, distance 3, and burns itself when played.",
		                                    "2 damage\nAOE 2 Dist 3\nBurns self",
		                                    Card.CardActionTypes.TargetGridSquare,
		                                    GridControl.TargetTypes.diamond, 3, GridControl.TargetTypes.diamond, 2));
        //Akan
			//Start unlocked	
		Lib.Add ("Glass Of Chardonnay", new LibraryCard ("Glass Of Chardonnay",
		                                                 "Glass Of \nChardonnay",
		                                                 "food/wine-glass",
		                                                 ShopControl.Gods.Akan,  
		                                                 Card.Rarity.Bronze,
		                                                 "To play, tap the card again",
		                                                 "Tuck 1 from the discard pile back into your deck, then deal 1 damage at a range",
		                                                 "Tuck 1 discard; \n1 damage,\nRange 2",
		                                                 Card.CardActionTypes.TargetCard,
		                                                 GridControl.TargetTypes.diamond, 2, GridControl.TargetTypes.none, 0));
		Lib.Add ("Fairy Tequila", new LibraryCard ("Fairy Tequila",
		                                         "Fairy \nTequila",
		                                         "food/martini",
		                                           ShopControl.Gods.Akan,
		                                           Card.Rarity.Silver,
		                                           "To play, tap the card again",
		                                           "Return 1 card from the discard to your hand",
		                                           "Return 1",
		                                           Card.CardActionTypes.TargetCard));
		Lib.Add ("Beer Batter Fries", new LibraryCard ("Beer Batter Fries",
		                                               "Beer Batter \nFries",
		                      		            	 "food/blade-fall",
		                                              ShopControl.Gods.Akan,
		                                              Card.Rarity.Gold,
		                                              "To play, tap the card again. Make sure to have a card in your discard pile.",
		                                              "Tuck 1 from the discard pile back into your deck, draw 1 and gain 1 play",
		                                              "Tuck 1 discard;\nDraw 1\n+1 Play",
		                                              Card.CardActionTypes.TargetCard));
			//Unlockable
		Lib.Add ("Broken Beer Bottle", new LibraryCard ("Broken Beer Bottle",
		                                              "Broken \nBeer Bottle",
		                                              "broken-bottle",
		                                              ShopControl.Gods.Akan,
		                                              Card.Rarity.Silver,
		                                              "To play, select a square",
		                                              "Deals 1 damage at range 2. \nIf tucked back into the deck, this card will instead return to your hand.",
		                                              "1 damage, \nRange 2\nIf tuck=> \nreturn to hand",
		                                              Card.CardActionTypes.TargetGridSquare,
		                                              GridControl.TargetTypes.diamond, 2, GridControl.TargetTypes.none, 0));

		//gotta actually think to make this VVVVV
		//Lib.Add ("Dragon \nWhiskey", new LibraryCard ("Dragon Whiskey",
		//												"Dragon \nWhiskey"
		//                      		             "glass-shot",
		//                                            ShopControl.Gods.Akan,
		//                                            Card.Rarity.Silver,
		//                                            "To play, tap the card again",
		//                                            "Burn a card in your hand and return two cards",
		//                                            "Burn 1: \nReturn 2",
		//                                            Card.CardActionTypes.NoTarget));

		//No god (created items)
		Lib.Add("Phoenix Fireball", new LibraryCard("Phoenix Fireball",
		                                            "Phoenix \nFireball",
		                                            "burning-meteor",
		                                            ShopControl.Gods.none,
		                                            Card.Rarity.Gold,
		                                            "To play, select a square (A.O.E. 1)",
		                                            "Deals 1 damage at a range in a small area", 
		                                            "Ranged, \nA.O.E. 1, \n1 damage", 
		                                            Card.CardActionTypes.TargetGridSquare, 
		                                            GridControl.TargetTypes.diamond, 2, GridControl.TargetTypes.diamond, 1));
		Lib.Add ("Soda Cup", new LibraryCard ("Soda Cup",
		                                         "Soda \nCup",
		                                         "glass-shot",
		                                         ShopControl.Gods.none,
		                                         Card.Rarity.Bronze,
		                                         "To play, tap the card again",
		                                         "Draw 2 cards and gain 2 plays. This card burns itself when played.",
		                                         "+2 cards \n+2 plays \n Burns self",
		                                         Card.CardActionTypes.NoTarget));


	}

	public LibraryCard PullCardFromPack (ShopControl.Gods god, Card.Rarity rarity) {
		List<LibraryCard> tempList = new List<LibraryCard> ();

		List<LibraryCard> allCards = CardLibrary.Lib.Values.ToList<LibraryCard>();

		if(SaveData.UnlockedGods.Contains(god)) {
			// => this goal isn't from the pantheon.
			foreach(LibraryCard LC in allCards) {
				if((LC.God == god && LC.ThisRarity == rarity)) {
					tempList.Add(LC);
				}
			}
		}
		else {
			// => this goal is from the pantheon and the card reward is random.
			foreach(LibraryCard LC in allCards) {
				if(LC.ThisRarity == Card.Rarity.Silver | LC.ThisRarity == Card.Rarity.Bronze) {
					tempList.Add(LC);
				}
			}
		}
		
		if(tempList.Count==0) {
			Debug.Log("templist is 0! picked god is " + god.ToString() + "and the rarity is " + rarity.ToString());
			return null;
		}

		int randomNumber = Random.Range (0, tempList.Count-1);
		
		return tempList[randomNumber];
	}
}

[System.Serializable]
public class LibraryCard {

	public string CardName;
	public string DisplayName;
	public string PrefabPath;
	public string IconPath;
	public string Tooltip;
	public string DisplayText;
	public string MiniDisplayText;
	public GridControl.TargetTypes RangeTargetType;
	public int rangeMin;
	public int rangeMax;
	public GridControl.TargetTypes AoeTargetType;
	public int aoeMinRange;
	public int aoeMaxRange;
	public Card.Rarity ThisRarity;
	public Card.CardActionTypes CardAction;
	public ShopControl.Gods God;
	public int Cost;
	public int UnlockCost;

	public LibraryCard() { }

	//normal
	public LibraryCard(string cardname, string displayname, string iconpath, ShopControl.Gods god, Card.Rarity rarity, 
	                   string tooltip, string displaytext, string minidisplaytext, Card.CardActionTypes cardAction,
	                   GridControl.TargetTypes rangeTargetType, int rangeMinArg, int rangeMaxArg, GridControl.TargetTypes aoeTargetType, int aoeMinArg, int aoeMaxArg) {
		CardName = cardname;
		DisplayName = displayname;
		IconPath = iconpath;
		God = god;
		
		ThisRarity = rarity;
		
		Tooltip = tooltip;
		
		DisplayText = displaytext;
		MiniDisplayText = minidisplaytext;
		
		CardAction = cardAction;
		RangeTargetType = rangeTargetType;
		rangeMin = rangeMinArg;
		rangeMax = rangeMaxArg;
		AoeTargetType = aoeTargetType;
		aoeMinRange = aoeMinArg;
		aoeMaxRange = aoeMaxArg;

		switch(rarity) {
		case Card.Rarity.Paper:
			Cost = 0;
			break;
		case Card.Rarity.Bronze:
			Cost = 2;
			break;
		case Card.Rarity.Silver:
			Cost = 4;
			break;
		case Card.Rarity.Gold:
			Cost = 6;
			break;
		case Card.Rarity.Platinum:
			Cost = 8;
			break;
		}
		UnlockCost = Cost;
	}
    #region Overloads
    //overload: no displayname
	public LibraryCard(string cardname, string iconpath, ShopControl.Gods god, Card.Rarity rarity, 
	                   string tooltip, string displaytext, string minidisplaytext, Card.CardActionTypes cardAction,
	                   GridControl.TargetTypes rangeTargetType, int rangeMinRangeArg, int rangeMaxRangeArg, GridControl.TargetTypes aoeTargetType, int aoeMinRangeArg, int aoeMaxRangeArg) 
	: this (cardname, cardname, iconpath, god, rarity, tooltip, displaytext, minidisplaytext, cardAction, rangeTargetType, rangeMinRangeArg, rangeMaxRangeArg, aoeTargetType, aoeMinRangeArg, aoeMaxRangeArg) { }
	//overload: no displayname, no rangeMin
	public LibraryCard(string cardname, string iconpath, ShopControl.Gods god, Card.Rarity rarity, 
	                   string tooltip, string displaytext, string minidisplaytext, Card.CardActionTypes cardAction,
	                   GridControl.TargetTypes rangeTargetType, int rangeArg, GridControl.TargetTypes aoeTargetType, int aoeRangeArg) 
	: this (cardname, cardname, iconpath, god, rarity, tooltip, displaytext, minidisplaytext, cardAction, rangeTargetType, 1, rangeArg, aoeTargetType, 0, aoeRangeArg) { }
	//overload: no rangeMin (different displayname)
	public LibraryCard(string cardname, string displayname, string iconpath, ShopControl.Gods god, Card.Rarity rarity, 
	                   string tooltip, string displaytext, string minidisplaytext, Card.CardActionTypes cardAction,
	                   GridControl.TargetTypes rangeTargetType, int rangeArg, GridControl.TargetTypes aoeTargetType, int aoeRangeArg) 
	: this (cardname, displayname, iconpath, god, rarity, tooltip, displaytext, minidisplaytext, cardAction, rangeTargetType, 1, rangeArg, aoeTargetType, 0, aoeRangeArg) { }
	//overload: no targeting squares
	public LibraryCard(string cardname, string displayname, string iconpath, ShopControl.Gods god, Card.Rarity rarity, 
	                   string tooltip, string displaytext, string minidisplaytext, Card.CardActionTypes cardAction) 
	: this (cardname, displayname, iconpath, god, rarity, tooltip, displaytext, minidisplaytext, cardAction, GridControl.TargetTypes.none, 0, 0, GridControl.TargetTypes.none, 0, 0) { }
	//overload: no targeting squares or displayname
	public LibraryCard(string cardname, string iconpath, ShopControl.Gods god, Card.Rarity rarity, 
	                   string tooltip, string displaytext, string minidisplaytext, Card.CardActionTypes cardAction) 
	: this (cardname, cardname, iconpath, god, rarity, tooltip, displaytext, minidisplaytext, cardAction, GridControl.TargetTypes.none, 0, 0, GridControl.TargetTypes.none, 0, 0) { }
    #endregion
}