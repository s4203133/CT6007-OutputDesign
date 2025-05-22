This Project serves as the practical implementation for the research conducted in the paper "An Exploration into advancing wave function collapse through machine learning strategies".

In the sample scene, a castle generator runs a series of algorithms to form a structure made out of a variety of different castle tiles. It begins with binary space partitioning, where randomly sized rooms are generated on a plane, and the centre is carved out to represent the floor plan of the castle. Each cell is then evaluated to test whether it is an empty cell (outside of the castle so not part of it), a perimeter cell, or an interior cell.
The wave function collapse algorithm then begins filling out the cells with the provided tiles (made as 'Scriptable Object' assets). Different layers are defined to make the algorithm focus on different aspects of the castle in a certain order. The first layer is the perimeter layer, where the wave function collapse algoritm only fills in the edge cells of the castle, and can only use different types of wall tiles. Once that has completed, the interior layer begins, where only cells contained within the walls are filled out with tiles such as grass, trees and pillars. The final layer is a vertical layer, which assesses the tiles placed on the grid and determines which ones can be placed upon, and then builds up towers up to maximum height, where the castle is considered 'complete'.

Once the castle has been generated, an army of 100 soldiers will attack from the south (negative Z axis) side of the castle. Once they can detect a castle tile, they will begin throwing projectile to break it down and eventually destroy it. When a soldier has detroyed 3 tiles, it will begin to advance into the castle to reach its target. The castle attempts to defend itself through its attack tiles, which fire projectiles back at the soldiers to damage them once they are within range. If one of the soldiers reach the centre target in the castle, the simulation ends in a 'defeat', but if all soldiers are eliminated first, the simulation ends in a 'victory'.

The Unity ML Agents package has been imported into the project to set up the machine learning aspect of the research. An agent has been created in the scene, which adjusts the priority of different tiles at run time to test which ones have a greater ability at defending the castle. The agent recieves rewards every time a soldier is eliminated or the simulation ends in a victory, but recieves penalties every time a soldier breaks down a tile, enters the castle through a doorway, or the simulation ends in a defeat. The goal is for the machine learning to understand which tiles are more useful for keeping the attackers out, and build castles with an improved amount of defenses which are placed more strategically to increase the chances of success.

*** *****DISCLAIMER***** ***

In order for the machine learning agent to work, a version of python between 3.10.0 and 3.10.11 will need to be installed, and a virtual environment will need to be set up through the command prompt. The project by default however comes with the base statistics for the tiles and algorithm for the training to begin taking place.

References (3D Models) -

Kenney (2024a) Castle Kit · Kenney, · Kenney. Available at: https://kenney.nl/assets/castle-kit (Accessed: 22 May 2025). 

Kenney (2024b) Mini Arena · Kenney, · Kenney. Available at: https://kenney.nl/assets/mini-arena (Accessed: 22 May 2025). 

Kenney (2024c) Tower defense kit · Kenney, · Kenney. Available at: https://kenney.nl/assets/tower-defense-kit (Accessed: 22 May 2025). 
