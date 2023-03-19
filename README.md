# GameSharp SDK for Unity
One SDK for cross-platform publishing HTML5 games developed in Unity Engine.

## Supported platforms:

- Yandex Games

## Usage

- Fluent Command/Query
- Initialization
- Advert
- Player Profile (Authentication)


##### Fluent Command/Query
We are pleased to announce a better, more intuitive way to construct and configure service clients in the SDK by Fluent Builder

Command Methods:
- AddCallback(ActionResult, Action) - callback on command execution;
- IsSupported() - return true if command supported;
- RemoveCallback(string) - where string is Callback ID;
- SetForceResultData(ActionResult, Data) - set Force Data for result without Proxy to Js Layer;
- ThrowIfNotSupported() - throw Exceptions if Command is not Supported;
- Execute() - execute comand;

Query Methods:
- IsSupported() - return true if query supported;
- SetForceResultData(ActionResult, Data) - set Force Data for result without Proxy to Js Layer;
- ThrowIfNotSupported() - throw Exceptions if Command is not Supported;
- Ask() - Execute Query

###### Example
#
```sh
GameSharp.Initialization.Initialize.AddCallback(ActionResult.Success, isInited =>
{
    Debug.Log("Initialization is Succesfull");
}).ThrowIfNotSupported().Execute();
```
