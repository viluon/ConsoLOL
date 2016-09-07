using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program {
	static void Main( string[] args ) {
		Monster Alois = new Monster( "Alois", 5 );

		Alois.Interactive();

		Console.ReadLine();
	}
}

class Monster {
	static Dictionary<string, int> menu = new Dictionary<string, int>();

	private string name;
	private int food;

	public static void Print( string text ) {
		Console.WriteLine( text );
	}

	public static void Write( string text ) {
		Console.Write( text );
	}

	public static void Usage( string text ) {
		Print( "Usage: " + text );
	}

	public static void TeachFood( string name, int nutritionValue ) {
		menu[ name ] = nutritionValue;
	}

	public Monster( string name, int food ) {
		this.name = name;
		this.food = food;
	}

	public Monster( string name ) {
		this.name = name;
		this.food = 0;
	}

	public Monster() {
		this.name = "Pedro";
		this.food = 0;
	}

	public void PrettyPrint() {
		Print( "I'm " + this.name + ", I've got " + this.food + " food points." );
	}

	public void Say( string text ) {
		Print( this.name + ": " + text );
	}

	public void Emote( string text ) {
		Print( this.name + " " + text );
	}

	public void Exercise( int time ) {
		if ( time <= 0 ) {
			this.Emote( "doesn't have a time machine" );
			return;
		}

		this.Emote( "goes for a run" );
		this.food -= time;

		this.CheckFood();
	}

	public void Feed( string foodName ) {
		this.Feed( foodName, 1 );
	}

	public void Feed( string foodName, int amount ) {
		if ( !menu.ContainsKey( foodName ) ) {
			this.Say( "I won't eat that." );
			return;
		}

		if ( amount == 0 ) {
			this.Say( "You gave me nothing to eat!" );
			return;
		}

		if ( amount == 1 ) {
			this.Emote( "eats a piece of " + foodName );
		} else {
			this.Emote( "eats " + amount + " pieces of " + foodName );
		}

		this.food += menu[ foodName ] * amount;
		this.CheckFood();
	}

	public void Feed( int amount ) {
		if ( amount <= 0 ) {
			this.Say( "That's a nope." );
			return;
		}

		this.Emote( "has a snack" );
		this.food += amount;
		this.CheckFood();
	}

	public void CheckFood() {
		if        ( this.food > 10 ) {
			this.Emote( "throws up" );
			this.food = 0;
		} else if ( this.food < 0  ) {
			this.Emote( "collapses" );
			this.food = 0;
		} else if ( this.food < 2  ) {
			this.Emote( "is hungry" );
		}
	}

	public string[] DigestInteractiveCommand() {
		Console.Write( this.name + " > " );

		return Console.ReadLine().Split( ' ' );
	}

	public void Interactive() {
		Print( "Welcome to " + this.name + " Command Line Awesomeness (tm)." );

		bool running = true;

		while ( running ) {
			string[] cmd = DigestInteractiveCommand();

			switch ( cmd[ 0 ].ToLower() )
			{
				case "rename":
					if ( cmd.Length < 2 ) {
						Usage( "rename <new name>" );
					} else {
						Print( this.name + " is now " + cmd[ 1 ] );
						this.name = cmd[ 1 ];
					}
					break;
				case "feed":
					if ( cmd.Length < 3 ) {
						Usage( "feed [food type] <amount>" );
						break;
					}

					int amount;

					if ( int.TryParse( cmd[ 1 ], out amount ) ) {
						this.Feed( amount );
					} else {
						if ( int.TryParse( cmd[ 2 ], out amount ) ) {
							this.Feed( cmd[ 1 ], amount );
						} else {
							Usage( "feed [food type] <amount>" );
						}
					}

					break;
				case "teach":
					if ( cmd.Length != 3 ) {
						Usage( "teach <food type> <nutrition value>" );
						break;
					}

					int val;

					if ( int.TryParse( cmd[ 2 ], out val ) ) {
						TeachFood( cmd[ 1 ], val );
						Print( "Monsters can now eat " + cmd[ 1 ] );
					} else {
						Usage( "teach <food type> <nutrition value>" );
					}

					break;
				case "list":
					if ( menu.ToArray().Length == 0 ) {
						Print( "No food types to list." );
						break;
					}

					Print( "Food Name (Nutrition Value)" );

					foreach ( KeyValuePair< string, int > kv in menu ) {
						Print( kv.Key + " (" + kv.Value + ")" );
					}

					break;
				case "help":
				case "?":
					if ( cmd.Length == 1 ) {
						Print( "You can ask for 'help', or perhaps 'help <command>', use 'rename', 'feed', 'teach', 'list' or 'quit'." );
						break;
					}

					switch ( cmd[ 1 ] ) 
					{
						case "rename":
							Print( "Give a new name to your animal." );
							Usage( "rename <new name>" );
							break;
						case "feed":
							Print( "Feed your animal nutrition points or a known type of food." );
							Usage( "feed [food type] <amount>" );
							break;
						case "teach":
							Print( "Teach your animal a new kind of food." );
							Usage( "teach <food type> <nutrition value>" );
							break;
						case "list":
							Print( "List the kinds of food known to monsters." );
							break;
						case "help":
							Print( "Get help on a specific command, or a list of commands available." );
							Usage( "help [command]" );
							break;
						case "quit":
							Print( "Quit the CLI." );
							break;

						default:
							Print( "No help available for '" + cmd[ 1 ] + "'" );
							break;
					}

					break;
				case "exit":
				case "quit":
				case "q":
					Print( "Well bye duh" );
					running = false;
					break;
				case "":
					break;

				default:
					Print( "Unknown command '" + cmd[ 0 ] + "', would you like some 'help'?" );
					break;
			}
		}
	}
}
