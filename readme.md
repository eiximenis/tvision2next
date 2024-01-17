# TVision2 - A (highly opinionated) TUI Library

Let's be clear: **this is my 3rd attempt** to build this library. Previous attempts ended in catastrophic failures, but hopefully I learnt from the mistakes, so maybe this time I am succesful!

## What is this?

TVision2 is a console library for dotnet do develop not only TUI applications but also console games. Also it tries to provide a easy-to-use replacement for `System.Console` to leverage the power of modern terminals (unicode support, full color, etc).

## How it works?

Currently does not work, because it's pure WIP. API is changing every day, or several times a day, as new features emerge, other are simplified. But overall goal is:

- Offer a base API similar to game engines: based on components and update/draw cycle
- Create a set of controls (like text boxes, dropdowns or lists) over this base API to allow creation of TUI applications
- Offer a basic buf expandable layout system
- Offer a styling mechanism

Controls API is totally optional, no needed to use it if you don't want to (maybe games). However it's a goal that controls and base API are fully interoprable between them, so you can build 95% of your game using the base API and leverage some basic things to the controls.

## When it will be ready?

I don't know. I don't even know if it will be ever ready! As I said this is my 3rd attempt:

- First attempt: https://github.com/eiximenis/tvision2. This failed because it was too complex to add a reliable layout system (which is needed for almost anything that is not a game). But more of the ideas of this first idea are here.
- Second attempt (not public, sorry). Failed because adding controls was too hard. Layout system was (more or less) working, but his usage was very grumpy.

In this 3rd attempt, most of the ideas of the 1st attempt still are in place (idea of components and update/draw cycle) but:
- Layout system is designed from scratch
- Control support still to be defined

Repository is public since day 1, so you can see in history how everything evolved :)

Greetings!