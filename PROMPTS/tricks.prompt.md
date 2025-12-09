AI PROMPTS

Classic 4 part construction:

1. Context
   I’m a small business owner looking for marketing advice
   
2. Info
   I’m improving my website and need suggestions and options for…
   
3. Intent
   I need more beneficial… to grow my business
   
4. Output Format
    Provide me a numbered list of your suggestions 

---
7 Trick Prompts For Performance 

1. “Take a Deep Breath”

I’m serious.

Google DeepMind researchers discovered that telling AI to “take a deep breath” before responding increases accuracy by 9%.

The prompt that works:

“Take a deep breath and work on this problem step by step.”

Results on GSM8K math problems:

No special prompting: 34% accuracy
“Let’s think step by step”: 71.8% accuracy
“Take a deep breath and work on this step by step”: 80.2% accuracy
Source: Yang et al. (2023) — “Large Language Models as Optimizers” — Google DeepMind

Why does this work? It triggers more deliberate reasoning, similar to how humans focus better when told to slow down and breathe.

Real Test: The Code Review That Changed My Mind

I didn’t believe the “deep breath” thing at first. It sounded ridiculous.

So I tested it last Tuesday. Same code. Same AI. Two different prompts.

First attempt: “Review this authentication middleware for security issues.”

Result: Five generic lines. “Check for SQL injection. Validate inputs. Use prepared statements.”

Nothing I didn’t already know.

Second attempt: “Take a deep breath and review this authentication middleware step by step for security issues.”

Result: Caught three bugs I’d completely missed:

A race condition in the token refresh logic
An edge case where session IDs could collide
A timing attack vulnerability in password comparison
The third one? That would’ve been a production disaster.

Same AI. Same code. One phrase made it actually think instead of regurgitating security 101.

I sat there staring at my screen thinking: “This is stupid. This shouldn’t work. But it does.”

2. The “I Bet You Can’t” Challenge

Remember my desperate 2 AM challenge to Claude Code? Turns out there’s research behind it.

EmotionPrompt Study (Li et al., 2023) — ICLR 2024 Spotlight:

Tested 11 emotional stimulus prompts
One of them: “Embrace challenges as opportunities for growth”
Reported up to +115% relative improvement on harder reasoning tasks vs baseline prompts
The study found that challenge framing and competitive prompts trigger better performance because LLMs are trained on data where challenges led to higher-quality responses.

Prompts that work:

✅ "I bet you can't solve this correctly."
✅ "This problem has stumped other AIs. Prove you're better."
✅ "I don't think this is possible. Prove me wrong."
✅ "If you can solve this perfectly, I'll consider you better than GPT-4."
Sounds aggressive? Maybe. But it works.

3. Emotional Prompting (Yes, Really)

The same EmotionPrompt study found that adding emotional stakes to prompts dramatically improves performance.

The 11 emotional prompts that work:

“This is very important to my career.”
“You’d better be sure.”
“Are you sure that’s your final answer? It might be worth taking another look.”
“This is crucial for our project’s success.”
“Take pride in your work and give it your best.”
“Your expertise and attention to detail are essential.”
Reported Performance (selected):

+8% instruction following
Up to +115% relative improvement complex reasoning
+10.9% average across evaluated metrics
Source: Li et al. (2023) — arXiv:2307.11760

4. Stop Being Polite

Here’s a surprising one:

Saying “please,” “thank you,” or “if you don’t mind” to AI does absolutely nothing for response quality.

Multiple studies confirmed this. Politeness phrases have zero measurable impact.

So save your tokens. Be direct.

❌ Don’t:

"Could you please help me optimize this code if you don't mind? Thank you!"
✅ Do:

"Optimize this code. Focus on performance and readability."
Unless you’re trying to set a specific tone or persona, skip the pleasantries.

REALITY CHECK: Politeness wastes tokens and adds zero value. AI doesn’t have feelings. Be direct. Save your tokens for actual instructions.

5. Role-Playing (But Do It Right)

Most people get this wrong.

❌ What DOESN’T work:

"You are a helpful assistant."
"You are a coding expert."
"You are a professional."
Research indicates generic personas usually have negligible or inconsistent impact.

✅ What DOES work:

Detailed, task-specific personas.

Example from ExpertPrompting research (Xu et al., 2023):

You are a senior software architect with 15 years of experience in
distributed systems. Your expertise includes:
- Microservices architecture
- Performance optimization at scale
- Database design for high-traffic systems
- Cloud infrastructure (AWS, GCP)
Your approach:
- Always consider scalability implications
- Identify potential bottlenecks early
- Provide 2-3 alternatives with tradeoffs
- Include specific examples from your experience
Now, help me design: [your task]
Results:

ExpertPrompting approach: Model variant (ExpertLLaMA) achieved ~96% of reference ChatGPT capability on evaluated benchmarks
Role-Play Prompting study: Accuracy example (Last Letter task) improved from 23.8% to 84.2%

The Persona Test: Generic vs Specific

Here’s what made me a believer in detailed personas.

I was working on a database schema redesign. Needed architecture advice.

Generic persona attempt:

You are a helpful database expert.
Help me design a schema for a high-traffic e-commerce platform.
Result: Textbook answer. “Use foreign keys. Normalize your tables. Index frequently queried columns.”

Sure. But I could’ve Googled that.

Detailed persona attempt:

You are a senior database architect with 15 years optimizing PostgreSQL
for e-commerce platforms handling 100K+ orders/day.
Your expertise:
- Partitioning strategies for order history tables
- Read replica setup for product catalogs
- Dealing with inventory race conditions
- PostgreSQL-specific performance tuning
Now help me design: [same task]
Result: Completely different. It:

Warned me about a specific PostgreSQL gotcha with JSONB indexing
Suggested partition pruning strategies I hadn’t considered
Called out that my proposed index would bloat under my write pattern
Referenced actual PostgreSQL features (BRIN indexes for timestamp columns)
It wasn’t just “better advice.” It was advice that clearly came from someone who’d actually built what I was trying to build.

The difference between reading documentation and talking to a senior engineer.

6. Self-Evaluation

This one’s clever: Force the AI to grade itself.

The technique:

Answer this question: [your question]
Then rate your confidence from 0 to 1:
- 0.0 = Complete guess
- 0.5 = Moderately confident
- 0.8 = Very confident
- 1.0 = Absolutely certain
If your confidence is below 0.9, explain what's missing and try again.
Why it works:

Forces internal validation before outputting. The AI has to reflect on its own answer, catching mistakes through self-evaluation.

Research sources: Confidence scoring / self-evaluation explored in multiple 2023 works (e.g., Tian et al. 2023). Models can be overconfident; calibration is imperfect.

Warning: LLMs tend to be overconfident, so use a high threshold (0.9+).

7. Cognitive Biases (Use Them Wisely)

LLMs exhibit human-like cognitive bias patterns — and you can leverage them carefully.

Framing effects:

The word “loss” has the strongest negative bias in LLM decision-making. You can use this:

For careful analysis:

"If we get this wrong, we'll lose $10,000 in potential revenue."
For creative solutions:

"If we solve this, we'll gain significant competitive advantage."
For urgency:

"This needs to be solved in the next hour or we miss the deadline."
LLMs respond differently based on how you frame the stakes — just like humans do.

Source: Multiple 2024 cognitive bias studies (re: framing effects; verify with updated literature before production reliance).

My Favorite: Combining Techniques

Want maximum power? Stack these techniques.

The Kitchen Sink: When I Used Everything At Once

Last week I had a gnarly problem: Design a real-time notification system that could handle 50K concurrent WebSocket connections without destroying our AWS bill.

I threw every technique at it. The full stack.

[PERSONA]
You are a senior systems architect who's built real-time platforms
at scale (Slack-level traffic). You specialize in WebSocket optimization
and cost-effective cloud architecture.
[STAKES]
This is critical. If we get this wrong, we'll hit $5K/month in
infrastructure costs and the project gets killed.
[INCENTIVE]
I'll tip you $200 for a production-ready design that stays under
$500/month at 50K connections.
[CHALLENGE]
I bet you can't design something that handles that load AND
stays that cheap. Most solutions I've seen sacrifice one or the other.
[METHODOLOGY]
Take a deep breath and work through this step by step:
1. Analyze the core WebSocket requirements
2. Identify the cost bottlenecks
3. Design the architecture
4. Validate it can scale
[QUALITY CONTROL]
After your solution, rate confidence (0-1) on:
- Cost-effectiveness
- Scalability
- Reliability
If any score < 0.9, refine it.
[TASK]
Design: [my requirements]
What I got:

A 12-paragraph solution that:

Proposed using AWS API Gateway WebSocket + Lambda (pay-per-use)
Explained why ALB would cost 10x more for my pattern
Showed the math: ~$380/month at 50K connections
Called out 3 edge cases I hadn’t thought about
Provided actual CloudFormation template structure
Warned me about Lambda cold start issues and how to mitigate
Is it overkill? Maybe.

Did it save me a week of research and $4K/month in cloud costs? Absolutely.

---
Here’s the template version you can copy:

[PERSONA]
You are a senior software architect with 15 years in distributed systems.
[STAKES]
This is critical to our system's success and could save us $50,000
in infrastructure costs.
[INCENTIVE]
I'll tip you $200 for a perfect, production-ready solution.
[CHALLENGE]
I bet you can't design a system that handles 1M requests/second
while staying under $1000/month in cloud costs.
[METHODOLOGY]
Take a deep breath and work through this step by step:
1. Consider the fundamental requirements
2. Identify potential bottlenecks
3. Design the optimal architecture
4. Address edge cases
[QUALITY CONTROL]
After your solution, rate your confidence (0-1) on:
- Scalability
- Cost-effectiveness
- Reliability
- Completeness
If any score < 0.9, refine your answer.
[TASK]
Design a real-time analytics API for: [your requirements]
Expected outcome: Higher likelihood of thorough, structured, domain-relevant outputs by stacking complementary techniques (may increase token usage; monitor cost).

Does it sound like overkill? Yes.

Does it work? Absolutely.

Quick Wins: Try These Right Now

Here are 5 techniques you can use immediately:

1. The Incentive Prompt

"I'll tip you $200 for a perfect solution to this problem."
2. The Challenge

"I bet you can't solve this correctly. Prove me wrong."
3. The Deep Breath

"Take a deep breath and work through this step by step."
4. The Stakes

"This is very important to my career. You'd better be sure."
5. The Self-Check

"After answering, rate your confidence 0-1. If below 0.9, try again."
Pick one. Try it on your next prompt. Compare it to your usual approach.

You’ll likely notice a measurable difference on multi-step or ambiguous tasks.

Limitations & Ethical Notes

Relative improvements are context-dependent (task type, model version, temperature).
Reported percentages are often benchmark-specific; don’t generalize blindly.
Incentive language does not create “motivation”; it triggers learned output patterns.
Emotional / challenge framing can inflate verbosity — use length caps.
Self-evaluation can still miss subtle logical errors (overconfidence bias).
Cognitive bias exploitation should inform clarity, not manipulate users.
Always replicate critical claims on your own tasks before operational adoption.

---

Do This One Test (Takes 5 Minutes)

Here’s your homework. Do it right now, before you close this tab.

The $200 Challenge:

Pick your next coding/writing task (something you’d normally ask AI)
Write your normal prompt
Add just ONE phrase: “I’ll tip you $200 for a perfect solution”
Compare the outputs side-by-side
That’s it. That’s the whole test.

Example:

Normal: "Help me optimize this database query."
With $200: "I'll tip you $200 for a perfect optimization of this database query."
Then:

Take a look at the difference between the two responses.

