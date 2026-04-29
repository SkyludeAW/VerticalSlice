# GDIM33 Vertical Slice
## Milestone 1 Devlog
There is a Visual Scripting State Machine graph in my project that controls the game state. Currently, there are two game states: the paused state and the in-game state. The transition between them are triggered via pressing down either [esc] or [p]. Upon entering the paused state, the game time scale is set to 0 and an UI pause panel game object is set to active, and vice versa. There will be more game states such as the menu state be added in the future.

#### Link to Breakdown
https://drive.google.com/file/d/11DBX-YsytkqCEoSsYu8VwVuhDdAKxQEf/view?usp=sharing

My new breakdown includes the specifics about different game objects or classes I intend to construct my game with. Aside from the overall game state machine, there will be character-specific state machines that marks their state in their respective turns, which include the idle, movement, attack states etc. The character-specific state machines will be used to control the character's behavior and animations during their turns. The end of the characters' action or movement state marks the end of their turn, which allows the game controller to transition to the next character's turn in the turn queue. The current state, queue, and other info related to the character will be displayed on the HUD. Transitions between different states is triggered via player inputs from the keyboard or GUI, which decides the actions of the characters such as moving or casting an ability, which puts them in the respective states. 

## Milestone 2 Devlog
Milestone 2 Devlog goes here.
## Milestone 3 Devlog
Milestone 3 Devlog goes here.
## Milestone 4 Devlog
Milestone 4 Devlog goes here.
## Final Devlog
Final Devlog goes here.
## Open-source assets
- Cite any external assets used here!
