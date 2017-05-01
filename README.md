Tyler Beaupre
Brian Keeley-DeBonis
Hope Wallace

World Wizards MQP Design Doc
Abstract / Idea High Level
We are creating a World Builder for the World Wizards project. The World Builder will support users creating levels from tilesets and also allow the placement of props and interactable objects in the scene. The World Wizards Level Builder (WWLB) will be developed for Virtual Reality (VR) platforms, specifically the HTC Vive. WWLB will be developed largely using the Unity Game Engine.
Goals
Intuitive tile and prop placement, leveraging the power of VR.
Multi-level support with Portals to traverse them.
Fully fledged and sophisticated, yet simple, level editor.
Import and setup custom tiles and props using Unity Editor extension and then export as asset bundles to be used in the World Wizards Application.
Place interactable objects in the world such as doors, levers, traps.
Gesture-based object transformations and controls.
Group objects together for multi-object editing capabilities.
Portable file system to save levels and user setting. Use a common format like JSON.
Controls
Menu Traversal:
Large Cinema Menus:
Move your head to look at what you want to select, look long enough to select it. Swipe to scroll menu to see other options. 
Radial Menu:
Workbench Dome: Rather than an actual menu, a portable workbench that follows the player around, allows the player easy access and placement of all possible tools.

Tile Editing:
	Gestures for rotation, scaling, and translating.
	Smart placement of tiles use contextual filters, and volumetric cogniscience (only placing props and tiles that fit in the available space). 

Camera Modes
First Person Player Perspective:
Allows the user to drop into the world and see it up close and personal. Useful for testing things from the player’s perspective and getting a true sense of scale and proportion or fine tuning prop and interactable placement. Users can toggle between modes to either fly or walk through the level.
	
Workbench Perspective:
The world sits upon a table in the center of the user’s VR space. The player can walk around the table and edit the level as they see fit from this higher level perspective. Useful for high-level level design and sweeping changes.
	
Architecture:
The Core World Wizards Project contains three packages:
The entity package contains data types and structures for World Wizards.
The controller package contains the manager/ controller classes which exist in the Unity scenes. The controllers will be accessed through the user controls and menus to manipulate the entity objects.
The user package will be responsible for handling the UI menus and user controls and interactions and interface to the controller objects.

Control Flow:
User interacts with UI and other controls. These interactions interface with the controller objects which mutate the World Wizards data and data structures.

SceneGraph: 
The scene graph will be a flat data structure implemented with a hashmap where the key is a Guid and the value is the WorldWizardsObject. Each implementation of a WorldWizardsObject will have a bidirectional connection to a parent and a list of children. When a child is removed, it must first be removed from the parent’s list of children, and then deleted to ensure no dangling references.

Coding Standards:
Capitalize Method Names (PascalCase)
Capitalize Class Names
100 character line width limit
camelCasing
Curly Brackets on the next line
All expressions following an if or else statement must be wrapped in { }
