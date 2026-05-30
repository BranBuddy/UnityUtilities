Week 3: Interactions + PlayerHSM

This week I wanted to create a simple and easy to use interaction system that allowed for robust additions; so I did just that. However, before doing so I needed 
to set up some kind of player movement system. After a lot of thinking (and searching), I decided to go with a Hierachical State Machine. 

I followed a tutorial by IHeartGameDev then refactored the code to my liking. The link to the playlist is linked below, much thanks!
The player prefab provided can walk, run, jump, and interact using states! Other states included are, grounded, falling, and idle (lot less flashy huh).

Now that the movement has been set up, the interaction system is ready to be tackled. Here I created a inheritance system that starts from an interface.
This is then inherited by a master interaction class which holds variables and functions all subclasses will need. While more will be created in the future,
I have only set up character and item interactions.

Character interactions, for now, start the dialouge tree that was made last week with full functionality. Items will be collected, and optionally destroyed, then get
added to a player inventory.

This is all for now! I will be back sooner rather than later hehe.

IHeartGameDev HSM playlist: https://www.youtube.com/playlist?list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy

What is Included:
-Scripts:
 -PlayerStateMachine.cs: Central hub that all states will go for variables; also checks for state updates
 -PlayerStateFactory.cs: Creates and stores player states
 -IRootState: Interface that all root states must inherit from; currently only is used for HandleGravity() function
 -PlayerBaseState.cs: abstract class that is inherited by all state classes
 -PlayerGroundState.cs: Handles Ground State
 -PlayerIdleState.cs: Handles Idle State
 -PlayerFallState.cs: Handles Fall State
 -PlayerJumpState.cs: Handles Jump State
 -PlayerRunState.cs: Handles Run State
 -PlayerWalkState.cs: Handles Walk State
 -PlayerInteractState.cs: Handles Interact State
 -IInteractable.cs: Interface with critical interactable variables
 -InteractionMasterClass.cs: Master class for all interaction types
 -CharacterInteractions.cs: Handles Character Interaction
 -ItemInteractions.cs: Handles Item Interaction
 -InteractionController.cs: Sets up information for interact state and handles interact text
 -PlayerMovement.cs: Handles actual player movement
 -PlayerJump.cs: Sets up information for jump state

-GameObjects/Prefabs
 -InteractDisplayText: Text that displays when player enters a interaction trigger
 -Player: Player GO will the scripts all set up and ready, adjust values to your liking.
 -Bob: Simple character that uses CharacterInteraction.cs
 -TheOrb: Simple item that uses ItemInteraction.cs\
 -Ground: Environment GO that has some stairs and is assigned to the ground layer

-Art:
 -A plethora of programmer materials used to add colors to scene

How to Setup:
 1.) Determine which interaction to use (character, item, or perhaps a new one)
 2.) Assign respective script to a object in the scene. Ensure this object has two colliders.
     One with isTrigger that'll encompass the area around to activate trigger, and another regular one that
     is the actual bounds of the object
 3.) Assign the respective references

Future Additions:
 1.) Make only two root states, grounded and airborne. Then turn fall and jump into substates
 2.) Add more unique interactions