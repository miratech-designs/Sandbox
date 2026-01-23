# The Essential Git Command Breakdown

## Daily Drivers

```bash
git status          # Where am I? What's changed?
git add .           # Stage everything (yeah, I'm lazy)
git commit -m "msg" # Save this snapshot
git pull            # Get everyone else's changes
git push            # Send it to the cloud
```

## Commands When you mess up

```bash
git reset --hard    # Nuclear option: undo everything
git revert <commit> # Undo politely (keeps history)
git stash           # Hide my mess temporarily
git stash pop       # Bring back my mess
```

### Branch Mastery

```bash
git branch          # List branches
git checkout -b     # Create new branch
git merge           # Combine branches
git rebase          # Make history pretty

## The Power User Stuff

```bash
git log --oneline   # Clean commit history
git diff            # What actually changed?
git blame           # Who broke this? (usually me)
git cherry-pick     # Steal specific commits
```
