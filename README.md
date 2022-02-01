# Game of Drones
![GOD_Logo][logo]

Game of Drones is an application that allows to design choreography of unmanned aerial vehicles. The application implements drone properties such as: speed, acceleration and separation, which are used by algorithms that simulate physical phenomena. The application implements drone properties such as: speed, acceleration and separation, which are used by algorithms that simulate physical phenomena. 

Main purpose of this project was to create an application with friendly interface which will allow user to create a drone choreography from rectangles filled with data, like in Scratch programming language.

Choreography projected in this way is processed and simulated and when the simulation ends with a success, it is exported into .CSV file. File contains drones coordinates in word space depending on specific reference point.

### Features
[Most features on youtube](https://www.youtube.com/watch?v=i2JU-79L54c)

#### Serialization
User can name a choreography data, then save it and close the application. After next opening previously saved data can be successfully loaded and modified/used again.

#### Placing objects
In the application user can place two types of objects: **ground points** and **drones**. Ground point is a reference point for drone coordinates that will be saved into CSV file. Drone can't be placed when there's no ground point on the map.
Objects can be placed through UI menu which displays all available objects to set on surface.

#### Selecting objects
Every previously placed object can be selected by user. On selection camera will focus the object and there will be displayed an UI with available object options.

#### Application states
Application has two states, **Projecting** and **Playing**. Depend on current state, user has other features to use. Like name says, in projecting state happens all the choreography creating process, and on playing - simulating and processing the choreography.
User can't change application states is there are any alerts with drones or collisions.

#### Projecting Choreography
Choreography is projected individually for each drone. It can be copied from one to another to speed up whole projecting process.
It is made from a defined *step* which user selects and fill its properties. User can set maximum speed for each step to control drone acceleration.
Step has a type, so steps are sorted to help user find the one that he need. They can be also searched using keywords of their name or description.

#### Choreography Path
After creating choreography for drone, a path is created to display its flying route. Path can be hidden and its color can be changed for easier recognizing drone flying route when placing a swarm.

#### Collision detecting
Drones have a property - separation, it defines distance between two drones that is safe. When one drone enters separation field of another, it causes a collision. Collisions are checked on placing drones and while flying, it always returns error, and user can't change state from projecting to playing or if it occurs while simulating, he can't end choreography and export it.

### Third party assets
* [Lean Tween](https://assetstore.unity.com/packages/tools/animation/leantween-3595)
* [Ui Color Picker](https://assetstore.unity.com/packages/tools/gui/flexible-color-picker-150497)

*& Models/Objects from Unity Asset Store and other sources*


[logo]: https://imgur.com/xzS4Ezf.png "Game of Drones Logo"
