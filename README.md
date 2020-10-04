# FancyToys
  FancyToys is a list of useful tools just like PowerToys, I wish so.


## Nursery
Nursery is a weak daemon process manager that can start/stop process in background. 

![FancyToys](Assets/repository/FancyToys.jpg)

### How does it work?

I had accomplished a WPF version of Nursery several days ago. I designed the xaml page and subscribed to events on Control, nothing bothered me a lot. But I didn't realize that many capabilities are restricted in UWP. It is not possible to start a `Process`, abort a thread and so on. To start a process, I put a launcher in App local storage folder. It's a socket server named [`NurseryLauncher`](Assets\Tools\NurseryLauncher.exe). By listening on port 626(Stitch), it starts a process for UWP-Nursery after receiving the instruction. UWP-Nursery will receive processes' information which is sent by the server per second later on.

### Controls

+ [`NavigationView`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.navigationview?view=winrt-19041) (with a `Frame`)
+ [`CommandBarFlyout`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.commandbarflyout?view=winrt-19041)
+ [`MenuBar`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.menubar?view=winrt-19041)
+ `ListBox`
+ `ToggleSwitch`
+ `Rectangle`
+ [`DataGrid`](https://docs.microsoft.com/en-us/windows/communitytoolkit/controls/datagrid)
+ `ContentDialog`

### Deficiencies
+ Please note that you should stop all processes before closing the app. I've noted the bug [here](https://moeext.github.io/2020/09/30/fancy-toys-notes/#%E7%88%B6%E8%BF%9B%E7%A8%8B%E9%80%80%E5%87%BA%E5%90%8E%EF%BC%88%E5%AD%90%E8%BF%9B%E7%A8%8B%E4%B8%8D%E9%80%80%E5%87%BA%EF%BC%89%E7%AB%AF%E5%8F%A3%E4%B8%8D%E9%87%8A%E6%94%BE).
+ It's incapable of updating process information in that information doesn't in UI Thread. I will fix it with the help of `Task`.
+ I really wanna expand the content into the title bar area. I tried but lost. As you can see, I've used arcylic in the title bar area.

