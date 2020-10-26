# Attributes and Aspects

Attributes and Aspects are freeform name-value bags which allow you to attach arbitrary integers or strings to Things. Combined with predefined fields on the object, they form the identity of the Thing they are placed on.

Attributes and Aspects are very similar in many ways, but differ in some key areas.

Their similarities:

* Are either Static (have a value, calculated at creation time, and never changed) or Dynamic (have a value which is recalculated with every step).
* Are changed by Effects, which cause a temporary modification to the value.
* Are accessed by a string (series of letters, like a word) Name.
* Have special operators to access their data.

Their differences:

* Attributes are integers (whole numbers, positive or negative, such as -5, 0 or 10). Aspects are strings (series of letters, like a word).
* Attributes have both Static (called Base) and Dynamic fields, and their values are added to eachother. Aspects have one field which is either Static or Dynamic.
* The sum of all Effects are *added* for Attributes. The *most recent* Effect *replaces* for Aspects.

|      | Attribute | Aspect |
|------|-----------|--------|
| Type | Integer | String |
| Value | Base + Dynamic + Sum(Effects) | Newest Effect, or Static/Dynamic value if no Effect.
| Operator | Variable->Name | Variable-->Name
| Default | 0 | `null`

## Technical Description

* Are both dictionaries on the Thing object.
* The Base value is calculated at creation time and is never reevalulated. It can be changed by a "Tranform" Effect at a later time, but recalculation never occurs (even when the thing definition is changed).
* Dynamic values are calculated after all effects are applied to a thing, and are stored on the object for that step. This will ensure the dynamic value is kept static, even if it contains non-deterministic calculations (such as a random call).
  * Unlike Base values, dynamic values always follow the definition. This means that if a Tranform chanages a Thing to a new definition which lacks the dynamic attribute, only the Base value wil lbe used for calculating.