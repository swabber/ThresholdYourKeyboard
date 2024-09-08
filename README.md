<h1 align="center">
	Threshold Your Keyboard
</h1>

 Is a unicorn startup :rocket: from Bayonne, NJ. A great way to spend Friday evening with :beers: and :wrestling:.
Two players must click `Ctrl` button on :keyboard:. `Left Ctrl` and `Right Ctrl` respectively.
Wins the one who makes more clicks per/min.
To win, the player must sustain its supremacy for 5 sec. Complexity can be adjusted by changing threshold size and method of threshold calculation.
> Warning:
This project serve as proof-of-concept to validate the feasibility of scaling out and potentially monetizing the game. Before going to IPO, we need to check with the attorney if we can be liable for any broken keyboards.

## Local setup

- [Clone the repo](https://github.com/swabber/ThresholdYourKeyboard) to your local drive with `git clone https://github.com/swabber/ThresholdYourKeyboard`
- Open `ThresholdYourKeyboard.sln` in [Visual Studio](https://visualstudio.microsoft.com/)
- Ensure projects located in [Plugins](./Plugins/) folder have option `..\..\ThresholdYourKeyboard\bin\` set in `Properties` -> `Build` -> `Output` -> `Base output path`
- Run the games with the `Debug`->`Start Debugging` (default hotkey `F5`) option in the menu strip.
  ![Start screen](./Doc/StartScreen.jpg)
- 1st and 2nd Threshold methods are default, pre-build functionality which you are welcome to [extend](#get-involved). They are loaded dynamically on game start.
- choose the Threshold method and set the value. This would lead you into the main screen.
  ![](./Doc/GamePlay1.jpg)
- Game will give 5 sec for players to rump up to speed after both players do their first click on `Ctrl` button.
  ![](./Doc/GamePlay2.jpg)
- After threshold breached, the winning player has to sustain its supremacy for 5 sec. to win.
  ![](./Doc/GamePlay3.jpg)

## Get Involved.
You are welcome to write your own custom threshold calculation rules by inheriting from [ThresholdChecker.cs](./Plugins/ExtensionAbstraction/ThresholdChecker.cs) located in `ExtensionAbstraction.dll` and dropping your .dll into the root folder of the game.
Implementation of abstract `ThresholdChecker` class would force you to implement a constructor with no arguments. It is needed to smoothly instantiate class dynamical.

```mermaid
classDiagram
    ThresholdChecker <|-- ThresholdRelativeChecker
    ThresholdChecker <|-- ThresholdAbsoluteChecker
    ThresholdChecker <|-- ThresholdMyCustomChecker
    ThresholdChecker : +T Threshold
    ThresholdChecker : +String FriendlyName
    ThresholdChecker : +String UserInstructions
    ThresholdChecker: +bool SetThreshold(T value)
    ThresholdChecker: +bool CheckThreshold(T value1, T value2)
    class ThresholdRelativeChecker{
      bool SetThreshold(double threshold)
      bool CheckThreshold(double value1, double value2)
    }
    class ThresholdAbsoluteChecker{
      bool SetThreshold(double threshold)
      bool CheckThreshold(double value1, double value2)
    }
    class ThresholdMyCustomChecker{
      bool SetThreshold(double threshold)
      bool CheckThreshold(double value1, double value2)
    }
```

![](./Doc/Extend.jpg)
