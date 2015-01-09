UIDragDropGestureRecognizer-Xamarin.iOS
=======================================
An iOS drag-and-drop UIGestureRecognizer for Xamarin.iOS.

What's Needed
-------------
* Xamarin.iOS
* Xamarin Studio or Visual Studio

What's Included
---------------
* Class library project
* Demo app project
* Help document (CHM)

How to Use
----------
To add a `DragDropGestureRecognizer` to your app, simple add these files to your project:
* `DragDropGestureRecognizer.cs`
* `DragDropEventArgs.cs`
* Add a preprocessor definition to the project's build settings or `#define` in code:
	* XAMARIN_CLASSIC_API
	* XAMARIN_UNIFIED_API

Alternatively, you can:
* Add the `RedCell.UI.iOS.DragDrop` project to your solution; or
* Build and reference `RedCell.UI.iOS.DragDrop.dll`

See the demo app for more details.

Built and Tested on
-------------------
* Visual Studio 2013
* Xamarin.iOS 8.4.0.0
* XCode 6.1
* iPad 3 (MD328LL/A)
* iOS 8.1.2