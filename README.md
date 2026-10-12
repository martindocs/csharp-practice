# Software Project Template: Generic Build Checklist

A reusable sequence of steps for building an app, so you always know what comes next. Not specific to one project.

## Phase 0: Understand

- [ ] Write one sentence on what the app does and who uses it.
- [ ] List what's **in scope** and **out of scope**.
- [ ] Define "done" in testable terms ("I can do X, Y, Z").

## Phase 1: Design before code

- [ ] List the entities and how they relate (one-to-many, etc.).
- [ ] Sketch the screens on paper. Just boxes, no art.
- [ ] List the rules the data must obey (validation, uniqueness, business rules).
- [ ] Note the risky or unknown parts. Tackle those early, not last.

## Phase 2: Set up

- [ ] Create the repo, README and `.gitignore`.
- [ ] Create the solution and project, and pick a folder structure.
- [ ] Make a first commit that builds and runs, even if it only shows a blank page.
- [ ] Decide your conventions (naming, where ViewModels live, etc.).

## Phase 3: Foundation

- [ ] Entities, DbContext, first migration.
- [ ] Seed data.
- [ ] Check the database actually looks the way you intended.

## Phase 4: The feature loop

Repeat this for every feature, one at a time:

1. **Define** the feature in one sentence.
2. **Build the thinnest version** that works end to end (database to screen).
3. **Try to break it** with empty input, huge input, odd characters, and the Back button.
4. **Clean up** by removing duplication and renaming unclear things.
5. **Commit** with a message saying *why*, not just what.

This loop is the real habit. It's what stops you from asking "what next?", because the answer is always the next feature on your list.

## Phase 5: Harden

- [ ] Validation and error handling.
- [ ] Edge cases (empty lists, last page, deleting something that has children).
- [ ] Check behavior with JavaScript turned off.

## Phase 6: Polish and wrap up

- [ ] Consistent look and messages.
- [ ] Update the README with how to run it, what it does, and "next version" ideas.
- [ ] Review your own code as if someone else wrote it.
- [ ] Write down what you learned and what you'd do differently.

## Habits that apply to every phase

- Small commits, often.
- Get it working, then make it clean, then make it fast. In that order.
- When stuck for more than about 20 minutes, write down the problem precisely. That often solves it, and if not, you can ask a better question.
- Keep a short `NOTES.md` of decisions and why you made them.
