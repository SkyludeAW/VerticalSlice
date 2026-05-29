# GDIM33 Vertical Slice
## Milestone 1 Devlog
There is a Visual Scripting State Machine graph in my project that controls the game state. Currently, there are two game states: the paused state and the in-game state. The transition between them are triggered via pressing down either [esc] or [p]. Upon entering the paused state, the game time scale is set to 0 and an UI pause panel game object is set to active, and vice versa. There will be more game states such as the menu state be added in the future.

#### Link to Breakdown
https://drive.google.com/file/d/11DBX-YsytkqCEoSsYu8VwVuhDdAKxQEf/view?usp=sharing

My new breakdown includes the specifics about different game objects or classes I intend to construct my game with. Aside from the overall game state machine, there will be character-specific state machines that marks their state in their respective turns, which include the idle, movement, attack states etc. The character-specific state machines will be used to control the character's behavior and animations during their turns. The end of the characters' action or movement state marks the end of their turn, which allows the game controller to transition to the next character's turn in the turn queue. The current state, queue, and other info related to the character will be displayed on the HUD. Transitions between different states is triggered via player inputs from the keyboard or GUI, which decides the actions of the characters such as moving or casting an ability, which puts them in the respective states. 


## Milestone 2 Devlog
### 1.
I had restructured my game into a top-down action RPG during this milestone (named Alicia's Slime Battle in the repository). My main complicating gameplay feature now is the continuous interaction between player and enemies that simplifies up to attacking and damaging each other. The player can move around the map and attack the enemies via a melee or ranged attack, and the enemies will pathfind their way near the player until in-range and attack them. The player and enemies have hitpoints, which ends the game/kills them when dropped to 0.

I broke down the dev process into the following steps:
#### a) Create a health system for player and enemies.
	I) Create an abstract class called "Entity" that contains core attributes like base health or attack that player and enemy classes will inherit from.
	II) Create hitboxes for players and enemies.
	III) Implement TakeDamage() and Die() functions for players and enemies, where the public TakeDamage() will reduce the health and record other data such as damage source, while the private Die() will be called when the health drops to 0, which will trigger the death animation and destroy the game object or trigger other effects.

#### b) Create an attack system for player and enemies.
	I) Create melee attack hitboxes for player and enemies, which will be used to detect if the attack hits the target.
	II) Create an melee attack class that controls the isTrigger hitboxes that will be activated during the attack period, which will trigger TakeDamage() if it hits the target.
	III) Create a ranged attack system for the player, which will instantiate a projectile that moves towards the target and triggers TakeDamage() if it hits the target.
	IV) Create a simple internal cooldown system for the player and enemies, which will prevent them from attacking again until the cooldown is over.

#### c) Create movement system for player and pathfinding for enemies.
	I) Create a basic 2D top-down character controller for the player that involes basic movement around the map.
	II) Add a dash system for the player. The dash would temporarily disable the player hitbox and adds a burst of force onto the player depending on their movement direction.
	III) Use a pathfinding system to the enemies' movement modules, with the player as the target. 
	IV) Stop the pathfinding when the the player is within the enemies' set attack range, and trigger the attack animation. 

### 2.  
Yes, as they established a clear path of development for the core gameplay features. Listing out my plan pre-dev allowed me to work in a module-after-module routine, and frees the brain fog that kind of happens to me during the dev, where I would be confused by what to add next to the whole system. Since I changed my game design during this milestone, I had to change my breakdown a lot, but the new breakdown still serves as a good reference for me to follow during the dev process. 

### 3.
I have a visual scripting state machine named GameStateManager that controls the game state, which includes the paused state and the in-game state. In the C# class GameManager, there is a Pause() and Unpause() function that invokes a custom event that the GameStateManager listens to, which triggers the transition between the paused state and the in-game state. For example, the button on the paused panel UI triggers the Unpause() function, which bridges to the GameStateManager and allows the player to resume the game.

#### Link to the screenshots of the graph
https://drive.google.com/drive/folders/16Vh7Cv5u-HZdnwuFQ9oNmJI-Gg5eMSU7?usp=sharing

### 4.
I implemented the animation system using Unity's Animator component, where I use script-based state machines to control the player and enemies' animation states and added keyframes of animation events to trigger their attacks. The animator system's components can be found on the Player game object, and the Basic Slime prefab.


## Milestone 3 Devlog
### 1.
I added two shader graphs in this milestone. 
The first is used for a melee slash vfx that plays when the player performs a melee attack. This shader alters the UV before sampling a slash texture, using the Rotate node on the shader graph. A float variable named Rotation is passed into the Rotate node, controlling the angle of the UV alteration, before sampling the texture. I also added a fade transition that multiplies to the sampled texture, which masks a portion of the texture, so it would seem like there is a beginning and an end to the slash. The Rotation variable is updated via combination of Unity's Visual Effect system, which allows me to spawn the shader as a single particle and adjust its Rotation variable during its lifetime, which creates the effect of a rotating slash.

The second is a small blur effect that is applied to the player when they perform a dash. This shader also alters the UV by adding a small tiling and offset to the UV on all four directions that overlaps on top of the original texture to create the blur effect. The color as well as the alpha is divided by 5 before being passed into the output so it would not just be some arbitrary bright texture on top of the original texture. A float variable named BlurStrength is used control the amount of tiling and offset. This variable is updated in the DashController C# script, which changes along with the speed of the dash in the dash animation curve that I newly added and will talk about in question 3.

#### Link to the screenshots of the shader graphs
https://drive.google.com/drive/folders/1RpmZdtIeUMypGfw0Q_hdZpJwP_9OK3dq?usp=sharing

### 2.
Two major bugs that occurred during the previous playtest are fixed. One major bug is the failure of the pathfinding system in some random spots on the map. I fixed this by reducing the node size of the A* pathfinding grid while adjusting some colliders on my tilemap. Another bug is the disordering of some parts of the shadows of the map, which now properly displays via separation into multiple layers of the tilemap. I also made the melee attack more flexible, where the player can not swing their mouse around to perform a wider melee attack, which kind of adds immersion to the game. 

### 3.
Most of the changes I made here are script-based improvements. I added a dash animation curve that allows customization of the player's speed, distance over the time of the dash, which I also combined with the blur shader (I am planning to make a screen-wide bullet-time/motion blur shader during some well-timed dashes). Object pools are added for bullets and enemies to ensure the game does not lag late-game. I created an AnimationListener class that listens to specific keyframes of the attack animations of the player and enemies, which fires an animation event that I encapsules as custom events that trigger the attack hitboxes to be active during the attack period. This ensures the animation and attacks are synced properly, and allows me to easily adjust the attack timing by just moving the keyframes of the animation events. A certain amount of force is also added in the direction of the attacks to make it feels more satisfying. In the context of the gameplay loop, they are crucial for the core combat mechanics, as they ensure the optimization and the player and enemies' attacks are responsive. The blur and slash vfx are on the other hand, part of the aesthetic component of the game. Sound effects are also added to slime on-hit (which still requires adjustments).

## Milestone 4 Devlog
Milestone 4 Devlog goes here.
## Final Devlog
Final Devlog goes here.
## Open-source assets
- Cite any external assets used here!
