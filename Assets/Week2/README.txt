Unity Utilites Week 2: Dialouge Tree

This week I have created my own little dialouge tree system. It is simple for now, but the core is important.
Here the designer can create their own little text-based interactions with a character. All information pertaining to set up is listed below.

What is Included:
-Scripts:
 -CharacterSO.cs: Scriptable Object class for character creation
 -DialougeTreeSO.cs: Scriptable Object class for designing dialouge tree
 -MasterDialougeTree.cs: Uses the two SO's above to create functionality for the tree
 -DialougeUIController.cs: Singleton class that act's as central hub for UI

-GameObjects/Prefabs
 -DialougeTreeUI: Basic UI for dialouge tree, including: background image, npc name text, npc image, main dialouge text, and container for
player response buttons
 -ResponseButton: Basic UI button for when the master class needs to instantiate new buttons

-Art:
 -Three basic sprites for character's reaction to player response.

-Other:
 -Premade Character and DialougeTree SO's

How to Set Up:
-Setting Up Dialouge Tree
 1.) Create DialougeTreeSo by right clicking then going from Create<ScriptableObject<DialougeTreeSO
 2.) Press "+" in the bottom right of the dialouge tree list to create nodes for this tree. Each node is block of the dialouge tree.
 Essentially each node will contain an NPC message and the player's response(s).
 3.) Customize the text to your liking. For player responses you can add as many as needed, but 3 is recommened if using the prefabs.
 4.) Optional Response text is the NPC's reaction to the player response. This will show after pressing the respective button,
 but if none is entered it'll continue as regular.
 5.) Do not touch Response Index, it is only shown as a reference for designers.
 6.) If you would like to end the conversation at a certain response click the boolean.
 7.) Optionally if you'd like this conversation to only happen once, click the boolean at the bottom.

-Setting Up Character
 1.) Create CharacterSO by right clicking then going from Create<ScriptableObject<CharacterSO
 2.) Enter a name for the character and how they feel for the player. Feeling scores don't need to be touched, they're there as a reference.
 3.) Drag in the sprites from Assets into their respective fields

-Setting Up GameObjects in Scene
 1.) Make an empty gameobject and assign that the MasterDialouge class
 2.) Drag in the SO's and enter the buffer time for text (in the future the next will be handled by interaction but it is time based for now).
 3.) Create a canvas gameobject and drag the dialougetree prefab onto it. Then assign the DialougeUIController script preferably to the canvas.
 4.) Then drag in the respective fields

-For correct functionality
 1.) Create a new button in canvas
 2.) For its on click event drag in the gameobject with the MasterDialouge class
 3.) For its event, select "InitializeUI".
 This is done like this for now before we can add actual interaction with characters

Future Additions:
 1.) Quest and Shop support so not all dialouge is the same
 2.) Intergration with future interaction support
 3.) Information gathering based upon player response
