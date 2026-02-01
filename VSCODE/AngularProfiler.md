# Profiling Angular Applications (Angular DevTools & Chrome DevTools)

You profile Angular apps using Angular DevTools and Chrome DevTools, not the React Profiler, but the concepts (recording renders, spotting slow components) are very similar.

## Core tools for Angular profiling

- Angular DevTools (Chrome extension) gives you a **Profiler** tab specific to Angular change detection and components.
- Chrome DevTools Performance panel shows browser‑level timing and, with Angular integration enabled, an extra Angular track for framework events.

## Setup: what you need

- Use a recent Angular (v16+; full Chrome integration is officially in Angular 20+).
- Install Angular DevTools from the Chrome Web Store, then open DevTools and you’ll see an Angular tab when an Angular app is loaded.

To enable deeper profiling in Chrome’s Performance tab, enable Angular profiling:

```ts
// main.ts (Angular standalone bootstrap example)
import { enableProfiling } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';

enableProfiling();
bootstrapApplication(AppComponent);
You can also run ng.enableProfiling() from the browser console in dev mode.

Using Angular DevTools Profiler (Angular tab)
Typical workflow (very analogous to React Profiler):

Open your app in Chrome, open DevTools → Angular tab → Profiler.

Click the record circle, then use the app in the way that feels slow (scroll, type, navigate, etc.).

Stop recording; you’ll see:

A timeline of Angular change detection cycles.

A flame‑graph‑like view of components and how long each took during change detection.

Key things to look for:

Components that repeatedly appear with high render time in the flame graph.

Large change‑detection runs triggered by minor interactions (e.g., typing in one field causes huge parts of the tree to recheck).

From there, you apply optimizations like ChangeDetectionStrategy.OnPush, trackBy on *ngFor, and moving heavy logic out of templates.

Using Chrome DevTools Performance with Angular integration
This is closer to “browser plus framework” profiling:

Make sure profiling is enabled (enableProfiling() or ng.enableProfiling()).

Open DevTools → Performance → click Record, reproduce the issue, then stop.

In the Performance trace you’ll see:

Usual browser events (scripting, rendering, painting).

An extra Angular track for component rendering, lifecycle hooks, and change detection cycles.

Use this view to:

Correlate a long Angular change detection cycle with expensive JS on the main thread.

See whether slowness is Angular work (component render, pipes, template expressions) versus other JS, layout, or painting.