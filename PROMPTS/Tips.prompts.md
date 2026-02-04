# 5 New Anthropic Engineers Workflow Prompting Techniques (Leaked ?)

*By Joe Njenga — AI Software Engineer*  
*Medium • 6 min read*  
*Published recently*  [oai_citation:1‡Medium](https://medium.com/ai-software-engineer/5-new-lessons-from-anthropic-engineer-workflow-leaked-4e2a648185dc?utm_source=chatgpt.com)

> _Note:_ I couldn’t verify whether this actually came from inside Anthropic, or if someone repackaged well-known best practices. Some claims — like “3× better performance” or “40% fewer hallucinations” — lack published backing. Still, I tested each technique in Claude and found them very useful.  [oai_citation:2‡Medium](https://medium.com/ai-software-engineer/5-new-lessons-from-anthropic-engineer-workflow-leaked-4e2a648185dc?utm_source=chatgpt.com)

---

## 1. Memory Injection — Teaching the Model Your Defaults

**Concept:**  

Front-load a block of context that tells the model **how you work** — your style, stack, and preferences — so it carries that forward in future prompts.

**Example Prompt:**  
> _“You’re my coding assistant. Remember these preferences: I use Python 3.11, prefer type hints, favor functional programming, and always include error handling. Acknowledge these preferences and use them in all future responses.”_  [oai_citation:3‡LinkedIn](https://www.linkedin.com/posts/daviscon_anthropic-engineers-just-leaked-their-internal-activity-7421481544227172352-lRK-?utm_source=chatgpt.com)

**Why it helps:**  
LLMs start “fresh” in each new chat. This injects your defaults so the model behaves more like a consistent teammate.

---

## 2. Reverse Prompting — Make the Model Ask First

**Concept:**  
Instead of telling the model what to do, require it to **ask clarifying questions**. This reduces ambiguous responses and hallucinations.

**Technique:**  
Have the model demand important context before executing any action.

**Example Prompt:**  
> _“Before you build this dashboard, ask me 5 clarifying questions about metrics, sources, and outputs. Don’t start until I answer.”_  [oai_citation:4‡LinkedIn](https://www.linkedin.com/posts/daviscon_anthropic-engineers-just-leaked-their-internal-activity-7421481544227172352-lRK-?utm_source=chatgpt.com)

**Why it helps:**  
Hallucinations often result from missing information. Forcing questions narrows gaps.

---

## 3. Constraint Cascade — Layer Constraints Sequentially

**Concept:**  
Instead of giving all instructions at once, **give them in stages**.

**Example Workflow:**  

1. Ask for a logic outline.  
2. Ask for security concerns in that outline.  
3. Finally, ask for implementation code that meets those constraints.  

[oai_citation:5‡LinkedIn](https://www.linkedin.com/posts/daviscon_anthropic-engineers-just-leaked-their-internal-activity-7421481544227172352-lRK-?utm_source=chatgpt.com)

**Why it helps:**  
Complex prompts crowd a context window. Sequential constraints keep the model focused and reduce errors.

---

## 4. Role Stacking — Multi-Perspective Analysis

**Concept:**  
Ask the model to **simultaneously adopt multiple expert roles** to analyze or solve a problem.

**Example Prompt:**  

“Analyze this infrastructure plan from three perspectives: scalability lead, security architect, CFO. Highlight disagreements.”
[oai_citation:6‡LinkedIn](https://www.linkedin.com/posts/daviscon_anthropic-engineers-just-leaked-their-internal-activity-7421481544227172352-lRK-?utm_source=chatgpt.com)

**Why it helps:**  
Mimics an internal “boardroom” debate. Reduces bias and blind spots.

---

## 5. Verification Loop — Self-Critique and Fix

**Concept:**  
Force the model to critique its own output before finalizing it.

**Example Prompt:**  

“Write this SQL migration. Then assume the role of a DBA and find three failure modes. Rewrite the script to address them.”
[oai_citation:7‡LinkedIn](https://www.linkedin.com/posts/daviscon_anthropic-engineers-just-leaked-their-internal-activity-7421481544227172352-lRK-?utm_source=chatgpt.com)

**Why it helps:**  

LLMs are generally better at spotting mistakes than avoiding them initially — so self-review catches errors early.

---

## Summary

These five techniques — **Memory Injection, Reverse Prompting, Constraint Cascade, Role Stacking,** and **Verification Loop** — are designed to make interactions with large language models (especially Anthropic’s Claude) **more structured, reliable, and closer to real engineering workflows**.  [oai_citation:8‡LinkedIn](https://www.linkedin.com/posts/daviscon_anthropic-engineers-just-leaked-their-internal-activity-7421481544227172352-lRK-?utm_source=chatgpt.com)

---

## References

- Original article: *5 New Anthropic Engineers Workflow Prompting Techniques (Leaked ?)* by Joe Njenga — *Medium* (member-only)
[oai_citation:9‡Medium](https://medium.com/ai-software-engineer/5-new-lessons-from-anthropic-engineer-workflow-leaked-4e2a648185dc?utm_source=chatgpt.com)

- Public breakdown of techniques from social media summaries of the leak  
[oai_citation:10‡LinkedIn](https://www.linkedin.com/posts/daviscon_anthropic-engineers-just-leaked-their-internal-activity-7421481544227172352-lRK-?utm_source=chatgpt.com)