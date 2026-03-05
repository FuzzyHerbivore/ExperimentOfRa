# ExperimentOfRa

## Context

This experiment is based on the idea of [Barque of Ra](https://github.com/FuzzyHerbivore/BarqueOfRa), the 2nd semester project I was involved in as part of S4G's engineering track.

Due to my lack of experience with Unity and unfortunate circumstances while the project was running, I was unable to get a feel for what an at least somewhat modular architecture would look like in the game until it was too late to course-correct. That's why I re-implemented a small part of the game as this separate experiment, to play around with what in my view might have been a good foundation.


## Architecture

I had already used **composition** to split up responsibilities in [Yokai Parade](https://github.com/FuzzyHerbivore/yokai-parade) by using Nodes with attached scripts, that selectively referenced other nodes/scripts they cooperated with. In this experiment I applied that thinking to Unity's components.


### [Combat](./Assets/Combat)

The components and prefabs in this folder are related to health and detection of combatants, take the [DamageTaker](./Assets/Combat/DamageTaker.cs) and [DamageDealer](./Assets/Combat/DamageDealer.cs) scripts and prefabs and the associated [Health](./Assets/Combat/Health.cs) script for example:

The health script, that is attached to the DamageTaker prefab, keeps track of health points and provides two events for when the amount of health points have been changed and when there is no points left... that's all. The `DamageTaker` prefab requires both a Health and a `Rigidbody`[^1] component, the latter of which provides a collision shape that registers/unregisters this `DamageTaker` component with any `DamageDealer` component that enters/leaves its collision shape. It also provides a `TakeDamage` method that calculates the damage to be taken, based on its own defense strength, and forwards the deduction of health points to the `Health` component.

Similarly, the [CombatantDetectorByReach](./Assets/Combat/CombatantDetectorByReach.cs) component provides book-keeping for potential combatants within the reach of its associated `SphereCollider`, which is used by one of the AI states explained in the next section.

The player units and enemies use a generic [Unit](./Assets/Units/Unit.cs) script component, that is totally oblivious to the existence of the combat-related components and only takes care of turning the units into `NavMeshAgents`.


### [AI States](./Assets/Units/UnitAIState.cs)

Similarly to the way unit states work in [Yokai Parade](https://github.com/FuzzyHerbivore/yokai-parade), there are both states and state brains to separate state-machine-related code from the code that uses the help of other components to determine which state to transition to. States are basically just `Empty` nodes that are children of a `StateMachine` node, which hold a reference to individual state brains.

An idea that I had this time though, is that [UnitAIStateBrain](./Assets/Units/UnitAIStateBrain.cs)s have to implement a `ThinkEnterPreconditions` method, that determines, whether the associated state can actually be transitioned to or not. In Yokai Parade that responsibility lay with the state a transition starts from, which lead to those states having to know an awful lot about details, i.e. have connections to helper components, only the target state should actually care about.

This can be seen in action in the `ThinkUpdate` method of [EnemyIdleAIStateBrain](./Assets/Units/Enemies/EnemyIdleAIStateBrain.cs), where the check whether to transition to the `AttackState` is delegated to the state's associated brain.

[^1]: Apparently at least one Rigidbody needs to be involved in a collision for Unity to actually notice, two objects of any `Collider` subtype will not register each other.
