# r.a.t.s
 
### Version Control

#### Picking up an Issue
- Assign yourself to an issue
- Checkout a branch based from `master`
- Name your branch `branch-name#issueNumber`
  - If the branch for example was named "Rat Hats" with an issue number of "13" then the branch would be `rat-hats#13`
- Make sure that you make regular fetches to update your branch in line with its origin. As well as regular descriptive commits
- Most issues should have some form of description to go off, if they do not and you are not sure reach out to Samuel [samuel@morgan.ooo]

#### Merging
- Before you merge anything in make sure you've done a fetch so it is as close to `master`, or other origin, as possible
- Add Samuel as the reviewer on the pull request
- Most merges will be done Sunday evening, so make sure that you update on Sunday

#### Scene Control
Scenes are a bit of a fucker to manage. For this reason we should sperate out the scenes that we work on. From the Scenes that we want to maintain as playable demos. These will be refered to as `Test` and `Production` repectively. 
- The `Test` scenes are not tracked in GitHub and can be found under `Scenes/Test`. These are for your reference only.
- Under `Scenes/Production` you'll find the Production Scenes. These are tracked on GitHub and you should only be working in them if you intend to.
- The functionality you add when working on tickets should come from adding said functionality to prefabs that are present (i.e. if adding new functionality to the Rats, you should make that change to prefabs and make sure it's saved. Provided those prefabs are used in production then it should be updated)

### Code Base

#### Conventions
##### Control
All scripts that are used to Control GameObjects (i.e. tell them to move, resize them). Most of these have an underlying data model, to explain this let's look at the Unit and its Controller: 
- Each "Unit" refers to either a player controlled unit or an enemy one.
- The "Unit" GameObject is controlled by a `MonoBehaviour` component named `UnitController`
- The methods within are used to order directives to the "Unit", methods such as moving to a location, attacking, looting, etc...
- Values on the "Unit" should not be contained as variables on the `UnitController` (i.e. we don't want to have values like `speed` or `health`). This type of information should instead be tracked in a `Unit` model, which contains `health`, `speed`, `attackRate` etc.

#### Handlers
Handlers are used to handle a specific event or situation. The most notable is that of the `IOHandler` which handles the input and output given to the game.


### Set Up

#### Load Order
In order to get the LoadSystem to load Units in correctly first you'll need to make sure that your load order is set-up corrrectly. Go under `Edit > Project Settings > Script Execution Order` and set the order to match the following:

   ![execution-order](https://i.imgur.com/Uig70la.png)
