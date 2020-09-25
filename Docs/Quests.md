# Quests

Quests are a mechanism which allows actions to persist between multiple steps.

Quests are stored on a Thing (A subject) and can point to multiple other things (Objects). Quests have their own set of attributes which can be used to modfiy the strength of the quest (For example, a quest to muder somebody could have attribtues to describe how much the Thing wants to murder somebody, and Aspects to describe why they want to murder them). Each quest can be marked as Excusive, which indicates that only one quest of this type (controlled by the Name of the quest) can occur on the person. 

Things can have any number of quests, and Events can have quest specifiers. It's intended for quests to be progressed via events.