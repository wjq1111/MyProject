# MyProject
My First Unity Project

## English ? Chinglish !

*ConfigManager*:

1. To manage more and more configures, we use ConfigManager to load all these configures.
2. You should first define your configuration path in the ConfigManager.cs, then create a serialized structure of your configuration in the Config/YourConfigName.cs, and finally create a json file in the Assets/StreamingAssets.
3. To use your data, you can simply get the ConfigManager.Instance.YourConfigName.

*FormManager*:

1. To manage more and more forms, we use FormManager to implement logic.
2. Each form should be a prefab, and prefabs can be created or destroyed in different places.

*EventManager*:

1. To manage UI Event (maybe some other events, whatever), we use EventManager to listen events and to dispatch events.
2. Each listener should be bound to a callback function that will throw an event when an object is clicked , thus executing your callback function.
