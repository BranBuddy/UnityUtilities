README: SOUNDMANAGER AND MUSIC BOXES

How to set up:
-Ensure a SoundManager game object is set up in the scene.

-Have a game object in the scene for creating the music box; preferably on the SoundManager game object as well

-Intialize audio source list in inspector and assign where needed. Adjust settings as you please.

-For each audio source, assign all the clips that are needed.

-Down below in the create music box script, pick what type of music box you need.

-Adjust size of as needed, along with position.

-Input the index of the audio source and song you want this music box to use.

-If you do not want to use the index method, just assign them directly. If it is assigned, it will overwrite any and all indexes inputted.

-Press create.

-In the music box adjust settings as you please.

Common Problems:
-Objects are not tagged as the proper tag when entering, therefore no sound plays.

-If everything seems like it should be playing, audio mixer and all, go into the project settings > audio > system sample rate. 
Change the system sample rate to 1.
