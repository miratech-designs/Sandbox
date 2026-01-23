# PLAN FILE

Here’s how to use it:
Ask AI what to work on next — or give it a goal.
AI will think holistically and generate a step-by-step plan.
Use “Opus Plan Mode” — opusplan. Opus for planning, Sonnet for execution.

You then choose:

1) Accept the plan and let it auto-execute

2) Accept the plan but approve each step

3) Reject and adjust

---

Wrap important text with “**”. Use ALL CAPS and exclamation marks for additional emphasis. 
Logically separate sections with title and subtitle marks. 
For example, this can be put into Claude.md or any other file that you want the AI to reference at specific times:

```markdown
## During Testing
**IMPORTANT!** When you find a failure during testing, **NEVER** ignore or remove the test to bypass the failure. 
**ALWAYS** identify the root cause first.
If the root cause is in the *application's implementation*, then fix the application.
If the root cause is because the *test* was incorrectly made to validate the proper expected behavior, then fix the test. 
```

Use “#” to make AI memorize instructions that it should know for all sessions. 
For example, if you’re building an app that runs in a docker container, tell AI to always rebuild the container image before testing:

```markdown
# **ALWAYS** rebuild the docker image after **EVERY** code change and **BEFORE** every test run.
```


🧹 Clean House
Cleanup frequently. Files, documents, remove finished sprints from planning-docs. 
Ask Claude to create a plan for removing outdated materials. Do this in groups of user documentation, planning-docs, technical docs, transient scripts (often created for ad-hoc testing), etc. Review each plan to ensure that you’re not deleting files that you may need.

Remove technical debt frequently. This reduces the size of the source code and outdated components that can confuse the AI.

Compact frequently at stop points that are logical. Try to do this before it auto-compacts. 
But, it’s not the end of the world if it auto-compacts anyway, because you’ve followed the other guidance for Spec Coding!

```command
/compact
```

If you’re in the middle of a long task or sprint that you want Claude to remember some details of what it’s done so far, 
tell it in the compact command. Tell it what to keep and what to ignore. 
Remember details are kept in your planning-docs by your Project Manager agent. 

For example,

```command
/compact Remember the initial goals and plan that I approved. Keep track of your task progress and next steps.
```

Clear (/clear or /exit and claude back in) when you are done with a sprint or major work.
