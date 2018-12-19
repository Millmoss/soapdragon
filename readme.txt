Game AI Project - AI Driven Story
Chris Chen & Owen Elliff

Run Info:

Run the scene GradeThisScene.unity
Hit spacebar to progress time.
Toggle "Maximize on Play" if the dialogue and character action narration isn't visible.

Brian is Red
Boy Pool is Blue
Jane is Green
Jane does not appear for the first 20 steps

Code Info:

Everything is run from Main.

For the character AI, we used blackboard architecture for deciding each character's actions in Blackboard.
We used A* for pathing in Pathfinding.
We used json files to store character and dialogue data.
FileManager loads the json files and stores the data in PersonData, LineLibrary, and ExpressionLibrary.
PersonData is loaded into Person at startup.
Person is the primary class for each character, their data, and their dialogue logic.
Preferences stores a character's preferences for various people, places, things, features, etc.
Memory stores the character's memory of his surroundings and the world.

Conversation is the primary class for the internal logic of dialogue lines.
Conversation pulls from LineLibrary and ExpressionLibrary to determine lines used and the expression data.
Line stores a set of Expression objects that form a sentence.
Expression stores a string and the information of the expression used.
Expression can take the form of Expression, Noun, Verb, Adjective, and Adverb.

Place stores a Rectangle object representing the place the character's are in.
Thing stores data for various items and objects in the world.
EnumStructs stores all enums and structs that are used throughout the program.
Visual_Controller displays the visual data.
Print_Text prints out the actions, intentions, and dialogue of the characters.



Note : These files do not do anything : WorldMap.cs, NotMain.cs, Item.cs