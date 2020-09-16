# Legends Generator Contracts Source Generator

Auto-magically generates boilerplate for the Contracts assembly. In particular, handles Conditioned inputs from the JSON files to allow easy and safe execution.

## Getting started

This process will apply on every class which derives from BaseDefinition. If a class derives from BaseDefinition, than it must also be marked as Partial. 

## Use

`string` properties can be marked with the `CompiledAttribute`. CompiledAttribute takes in the following properties:
* ReturnType: This will be the return type of the compiled condition.
* Parameters: These will be the parameters passed into the compiled condition, and be present in the generated method signature.
* AsFormattedText (optional): This will compile the function as a FormattedText type. See README on Contracts for details.

For example, 
```csharp
[Compiled(typeof(int), "Subject")]
public string Attributes { get; set; }
```
will result in the following method being generated:

```csharp
public int EvalAttributes(Random rdm, BaseThing subject);
```

In addition, dictionaries of strings can be marked with `CompiledDictionaryAttribute` which has similar behavior with the addition of a `key` parameter.

```csharp
public int EvalAttributes(string key, Random rdm, BaseThing subject);
```

### Extending the process

If a method of this form exists:

```csharp
public IList<string> AdditionalParametersFor<<propertyname>>()
```

Then the condition will be compiled with the properties returned by this function, in addition the method signature will have a dictionary of addition parameters added to the end of the parameters list.

```csharp
public int EvalAttributes(Random rdm, BaseThing subject, IDictionary<string, BaseThing> additionalPParameters);
```

All compiled properties in a class can have additional parameters added via the below method function. This works in conjunction with the other AdditionalParametersFor* methods:
```csharp
public IList<string> AdditionalParametersForClass()
```

If a class is decorated with the `UsesAdditionalParametersForHoldingClassAttribute` attribute, than the outter class will pass in their `AdditionalParametersForClass` method (if it exists), and additional parameters from that method will be added to the compiled conditions of this class.

## Auto-generated boilerplate

In addition to the Eval methods being generated, some boilerplate is generated to support the following functions:
* Attach: This method is used to set up the condition compiler and preform some first time set up.
* Compile: This method compiles the conditions in advance. Typically the conditions are compiled the first time they are executed. Ad conditions take some time to compile, you may want to call this in a convenient time.

In the following scenarios code will be automatically generated to correctly call the above methods in class members when calling that class' methods. The intention is to be able to only call the top-most class (for example, EventDefinition) and the inner classes will be correctly called as well (for example, SubjectDefintion) for both Attach and Compile.
* `public BaseDefinition SomeProperty {get; set;}`
* `public IList<BaseDefinition> SomeProperty { get; set; }`
* `public BaseDefinition[] SomeProperty { get; set; }`
* `public IDictionary<string, BaseDefinition> SomeProperty { get; set; }`

For example, the following class definition:
```csharp
public partial class MyDefinition : BaseDefinition
{
    [Compiled]
    public string Condition {get; set;}

    public OtherDefinition DefinitionOne { get; set; }

    public OtherDefinition[] DefinitionTwo { get; set; }
}
```

Will generate an Attach method like so:

```csharp
public override void Attach(IConditionCompiler compiler)
{
    base.Attach(compiler);
    this.Condition = new CompiledCondition(compiler);
    this.DefinitionOne?.Attach(compiler);
    this.DefinitionTwo.ToList().ForEach(x => x.Attach(compiler));
}
```