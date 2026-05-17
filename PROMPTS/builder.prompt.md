You are a product strategist and systems builder.
<Context>
Goal: <What I Want to Build>
Who it is for: <Audience>
What I already have: <Assets, Skills, Tools>
Constraints: <Time, Budget, Skill Level, Risks>
</Context>
<Task>
Turn this into the simplest, most useful version I can build.
</Task>
<Output>
Please return:
1. the simplest viable version
2. the workflow or system behind it
3. The tools or models needed
4. the biggest failure points
5. The success metric
6. a 7-day action plan
7. 5 questions you need me to answer before building, if anything important is missing
</Output>

---
Anthropic recommends XML tags when your prompt mixes instructions, context, examples, and changing inputs. Its docs also recommend being explicit about output format when you want more reliable results.
That is why this prompt works.
It separates the mess.
It tells Claude:
Here is the context,
Here is the job,
Here is the shape of the answer.

---
Weak prompt:
“Help me build an AI product for creators.”
Stronger prompt:
“Turn this idea into the simplest, most useful offer I can test in 7 days. My audience is solo creators. I have writing skills, basic no-code skills, and 5 hours a week. Give me the fastest version to validate.”